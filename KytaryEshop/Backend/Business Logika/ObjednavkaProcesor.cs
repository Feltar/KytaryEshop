using Kytary.Backend.BModels;
using Kytary.Backend.Data_Access;
using SqlKata;
using System.Data.SqlClient;
using Dapper;
namespace Kytary.Backend.Business_Logika
{
    /// <summary>
    /// Třída datového přístupu pro tabulky dbo.Objednavka a dbo.ObjednavkaArtikl obsahující data o proběhlých objednávkách.
    /// </summary>
    public static class ObjednavkaProcesor
    {
        //***************************************************************************Vytváření záznamů o objednávkách*****************************************************
        /// <summary>
        /// Zařadí objednávku a její položky do databáze. V případě úspěšného vyřízení objednávky vrátí true, v opačném případě false.
        /// </summary>
        /// <param name="datum"> Datum, ve kterém došlo k vytvoření objednávky. </param>
        /// <param name="IdUzivatel"> Identifikátor uživatele, jež přidal objednávku.  </param>
        /// <param name="polozkyKosiku"> Seznam položek, které si uživatel objednal. </param>
        /// <returns>Informace o tom, jestli vytvoření záznamu o objednávce proběhlo v pořádku.</returns>
        public static bool PridejObjednavku(DateTime datum, string IdUzivatel, List<PolozkaKosikuBModel> polozkyKosiku)
        {
            //Zjisti jestli uzivatel existuje a jestli polozky kosiku nejsou null

            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                try
                {
                    //Zapoceti Transakce
                    spojeni.Open();
                    SqlTransaction transakce = spojeni.BeginTransaction();

                    //Vytvoreni zaznamu o probehle objednavce
                    int idObjednavka = ObjednavkaProcesor.VytvoreniZaznamuVDatabaziObjednavka(IdUzivatel, datum, spojeni, transakce);

                    //Vyrizeni jednotlivych pohledavek (= paru artikl,objednavka)
                    foreach (var polozka in polozkyKosiku)
                    {
                        int sklademArtiklu = Kytary.Backend.Business_Logika.ArtiklProcesor.CetnostArtiklu(polozka.IdArtikl);
                        if (sklademArtiklu < polozka.PocetKusu)
                        {
                            transakce.Rollback();
                            return false;
                        }
                        if (!VytvoreniZaznamuVObjednavkaArtikl(idObjednavka, polozka.IdArtikl, polozka.PocetKusu, spojeni, transakce))
                        {
                            transakce.Rollback();
                            return false;
                        }
                        //Snizeni poctu artiklu na sklade
                        if (ArtiklProcesor.ProdejArtikl(polozka.IdArtikl, polozka.PocetKusu, spojeni, transakce) < 1)
                        {
                            transakce.Rollback();
                            return false;
                        }

                    }
                    //Vyprazdneni kosiku Uzivatele
                    if (!PolozkaKosikuProcesor.SmazPolozkyZKosikuUzivatele(IdUzivatel, spojeni, transakce))
                    {
                        transakce.Rollback();
                        return false;
                    }
                    transakce.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
        }
        /// <summary>
        /// Vytvoří záznam o objednávce v tabulce dbo.Objednavka v rámci probíhající transakce.
        /// </summary>
        /// <param name="IdUzivatel"> Identifikátor uživatele, jež přidal objednávku.  </param>
        /// <param name="datum">Datum, při kterém byla objednávka vytvořena. </param>
        /// <param name="spojeni"> Spojení na kterém probíhá transakce. </param>
        /// <param name="transakce"> Transakce v rámci které je třeba vykonat dotaz. </param>
        /// <returns> Vrací počet záznamů, které byly vatvořeny. </returns>
        private static int VytvoreniZaznamuVDatabaziObjednavka(string IdUzivatel, DateTime datum, SqlConnection spojeni, SqlTransaction transakce)
        {
            string sqlVytvoreniZaznamuVDatabaziObjednavka = "spObjednavka_VytvoreniZaznamuVDatabaziObjednavka";
            DynamicParameters param = new DynamicParameters();
            param.Add("@IdUzivatel", IdUzivatel);
            param.Add("@Datum", datum);
            int idObjednavka = spojeni.ExecuteScalar<int>(sqlVytvoreniZaznamuVDatabaziObjednavka,
                                        param, transakce, commandType: System.Data.CommandType.StoredProcedure);

            return idObjednavka;
        
        }
        /// <summary>
        ///  Ukládá položku objednávky do tabulky dbo.ObjednavkaArtikl v rámci probíhající transakce.
        /// </summary>
        /// <param name="idObjednavka"> Identifikátor objednávky, k níž patří daná položka. </param>
        /// <param name="IdArtikl"> Identifikátor artiklu, příslušícího výsledné položce objednávky. </param>
        /// <param name="PocetKusu"> Počet kusů artiklu, jež byly objednány. </param>
        /// <param name="spojeni"> Spojení v rámci kterého probíhá transakce. </param>
        /// <param name="transakce">Transakce v rámci které je třeba vykonat dotaz.</param>
        /// <returns> Informace o tom, jestli byla položka v tabulce dbo.Objednávka vytvořena úspěšně. </returns>
        private static bool VytvoreniZaznamuVObjednavkaArtikl(int idObjednavka, int IdArtikl, int PocetKusu, SqlConnection spojeni, SqlTransaction transakce) {
            DynamicParameters param = new DynamicParameters();
            string VytvoreniZaznamuVObjednavkaArtikl = "spObjednavkaArtikl_VytvoreniZaznamuVObjednavkaArtikl";
            param = new DynamicParameters();
            param.Add("@IdObjednavka", idObjednavka);
            param.Add("@IdArtikl", IdArtikl);
            param.Add("@PocetKusu", PocetKusu);

            int CenaKus = ArtiklProcesor.CenaArtiklu(IdArtikl);
            param.Add("@CenaKus", CenaKus);
            int vysledek = spojeni.Execute(VytvoreniZaznamuVObjednavkaArtikl, param, transakce, commandType: System.Data.CommandType.StoredProcedure);
            if (vysledek >=  1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //***************************************************************************Vytváření záznamů o objednávkách*****************************************************
        //---------------------------------------------------------------------------Načítání informace o objednávkách-----------------------------------------------------
        /// <summary>
        /// Načte z databáze položky objednávky se specifikovaným identifikátorem.
        /// </summary>
        /// <param name="idObjednavka">Identifikátor načítané objednávky. </param>
        /// <returns> Seznam položek objednávky se specifikovnaným identifikátorem. </returns>
        public static List<PolozkaObjednavkyBModel> NactiPolozkyPodleIdObjednavky(int idObjednavka)
        {
            string sqlPrikaz = "spObjednavka_NactiPolozkyPodleIdObjednavky";
            DynamicParameters param = new DynamicParameters();
            param.Add("@IdObjednavka", idObjednavka);
            List<PolozkaObjednavkyBModel> list;

            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                list = spojeni.Query<PolozkaObjednavkyBModel>(sqlPrikaz, param,commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            return list;
        }
        /// <summary>
        /// Načte z databáze seznam objednávek vykonaných uživatelem se specifikovaným identifikátorem.
        /// </summary>
        /// <param name="IdUzivatel">Identifikátor uživatele, jehož objednávky chceme načíst. </param>
        /// <returns>Seznam objednávek, vytvořených uživatelem s identifikátorem zadaným ve vstupu. </returns>
        public static List<ObjednavkaBModel> NacteniObjednavekVykonanychUzivatelem(string IdUzivatel)
        {
            string sql2 = "spObjednavka_NacteniZaznamuOObjednavkachVykonanychUzivatelem";
            List<ObjednavkaBModel> modely;

            
            DynamicParameters param = new DynamicParameters();
            param.Add("@IdUzivatel", IdUzivatel);
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                modely = spojeni.Query<ObjednavkaBModel>(sql2,param,commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            foreach (var item in modely)
            {
                item.PolozkyObjednavky = NactiPolozkyPodleIdObjednavky(item.IdObjednavka);
            }
            return modely;
        }
        //---------------------------------------------------------------------------Načítání informace o objednávkách-----------------------------------------------------

    }
}
