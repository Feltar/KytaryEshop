using Dapper;
using Kytary.Backend.BModels;
using Kytary.Backend.Data_Access;
using System.Data.SqlClient;

namespace Kytary.Backend.Business_Logika
{
    /// <summary>
    /// Třída datového přístupu pro tabulku dbo.PolozkaKosiku obsahující data o položkách košíku jednotlivých uživatelů.
    /// </summary>
    public static class PolozkaKosikuProcesor
    {
        /// <summary>
        /// Uloží položku košíku uživatele se zadaným identifikátorem do databáze. Položka se sestává z identifikátoru artiklu a jeho kvantity v košíku (a identifikátoru uživatele).
        /// </summary>
        /// <param name="idArtikl"> Identifikátor artiklu, ke kterému se daná polžka vztahuje. </param>
        /// <param name="pocet"> Kvantita artiklu v košíku. </param>
        /// <param name="idUzivatel"> Identifikátor uživatele, o jehož košík se jedná. </param>
        public static void UlozPolozkuUzivatele(int idArtikl, int pocet, string idUzivatel) {
            
            string sql2 = "spPolozkaKosiku_UlozPolozkuUzivatele";

            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);
                param.Add("@IdArtikl", idArtikl);
                param.Add("@PocetKusu", pocet);

                spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Načte  z databáze položky košíku uživatele, jehož identifikátor se nachází na vstupu. V případě zjištění, že některá z položek se již nenachází na skladě 
        /// v dostatečné kvantitě, sníží podle potřeby příslušnou kvantitu v košíku na maximální možnou hodnotu.
        /// </summary>
        /// <param name="idUzivatel"> Identifikátor uživatele, jehož položky košíku chceme načíst.</param>
        /// <returns>Seznam položek košíku zadaného uživatele.</returns>
        public static List<PolozkaKosikuBModel> NactiPolozkyKosikuUzivatele(string idUzivatel)
        {
            
            string sql2 = "spPolozkaKosiku_NactiPolozkyKosikuUzivatele";
            List<PolozkaKosikuBModel> modely;

            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);

                spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
                modely = spojeni.Query<PolozkaKosikuBModel>(sql2, param, commandType: System.Data.CommandType.StoredProcedure).ToList();

            }
      
            
            //Zjistujeme. jestli se v prubehu toho, co polozky lezi v kosiku nestalo, ze uz jich neni dost na sklade
            for (int i = modely.Count - 1; i >= 0; i--)
            {
                int cetnostNaSklade = ArtiklProcesor.CetnostArtiklu(modely[i].IdArtikl);
                if (cetnostNaSklade < modely[i].PocetKusu)
                {
                    if (cetnostNaSklade == 0)
                    {
                        SmazPolozkuZKosiku(modely[i].IdArtikl, idUzivatel);
                        modely.RemoveAt(i);
                    }
                    else
                    {
                        ZmenKvantitu(modely[i].IdArtikl, idUzivatel, cetnostNaSklade);
                        modely[i].PocetKusu = cetnostNaSklade;
                    }
                }
            }
            return modely;
        }
        /// <summary>
        /// Smaže položku košíku vztahující se k artiklu a uživateli se zadanými identifikátory.
        /// </summary>
        /// <param name="idArtikl">Identifikátor artiklu, ke kterému se váže mazaná položka košíku.</param>
        /// <param name="idUzivatel">Identifikátor uživatele, ke kterému se váže mazaná položka košíku.</param>
        /// <returns>Informace o tom, jestli položka košíku byla smazána úspěšně.</returns>
        public static bool SmazPolozkuZKosiku(int idArtikl, string idUzivatel)
        {
            string sql = "spPolozkaKosiku_SmazPolozkuZKosiku";
            int vystup;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni()) { 
                
                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);
                param.Add("@IdArtikl", idArtikl);
                
                vystup = spojeni.ExecuteScalar<int> (sql, param, commandType: System.Data.CommandType.StoredProcedure);
            }


            if (vystup>0)  return true; 
            else return false;
        }
        /// <summary>
        /// Položce košíku vztahující se uživateli a artiklu se zadanými identifikátory nastaví tato metoda v databázi kvantitu na zadaný počet prvků.
        /// </summary>
        /// <param name="idArtikl">Identifikátor artiklu, kterému se váže odkazovaná košíku.</param>
        /// <param name="idUzivatel">Identifikátor uživatele, ke kterému se váže odkazovaná položka košíku.</param>
        /// <param name="pocetKusu">Počet kusů, na který chceme změnit kvantitu košíku odkazované položky.</param>
        /// <returns>Informace o tom, jestli kvantita položky košíku byla změněna úspěšně.</returns>
        public static bool ZmenKvantitu(int idArtikl, string idUzivatel, int pocetKusu)
        {
            int vystup;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                string sql2 = "spPolozkaKosiku_ZmenKvantitu";

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);
                param.Add("@IdArtikl", idArtikl);
                param.Add("@PocetKusu", pocetKusu);


                vystup = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            if (vystup > 0) return true;
            else return false;
        }
        /// <summary>
        ///  Zjistí kvantitu položky košíku vztahující se k artiklu a uživateli se zadanými identifikátory.
        /// </summary>
        /// <param name="idArtikl">Identifikátor artiklu, jehož kvantitu v košíku chceme zjistit.</param>
        /// <param name="idUzivatel">Identifikátor uživatele jemuž přísluší odkazovaná položka košíku.</param>
        /// <returns>Zjištěná kvantita dotazované položky košíku.</returns>
        public static int ZjistiKvantitu(int idArtikl, string idUzivatel) {
            int vystup;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                string sql2 = "spPolozkaKosiku_ZjistiKvantitu";

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);
                param.Add("@IdArtikl", idArtikl);

                vystup = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vystup;
        }
        /// <summary>
        /// Smaže z tabulky položky košíku uživatele se zadaným identifikátorem.
        /// </summary>
        /// <param name="idUzivatel">Identifikátor uživatele, jehož položky chceme smazat.</param>
        /// <returns>Informace, jestli smazání proběhlo úspěšně.</returns>
        public static bool SmazPolozkyZKosikuUzivatele(string idUzivatel)
        {
            int vystup;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                string sql2 = "spPolozkaKosiku_SmazPolozkyZKosikuUzivatele";

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdUzivatel", idUzivatel);

                vystup = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            if (vystup > 0) return true;
            else return false;
        }
        /// <summary>
        /// Smaže položky z košíku uživatele se zadaným identifikátorem v rámci probíhající transakce.
        /// </summary>
        /// <param name="idUzivatel">Identifikátor uživatele, jehož položky chceme smazat.</param>
        /// <param name="spojeni">Spojení, na kterém probíhá aktuální transakce.</param>
        /// <param name="transakce">Transakce, v rámci které má být metoda vykonána.</param>
        /// <returns></returns>
        public static bool SmazPolozkyZKosikuUzivatele(string idUzivatel, SqlConnection spojeni, SqlTransaction transakce)
        {
            int vystup;
            string sql2 = "spPolozkaKosiku_SmazPolozkyZKosikuUzivatele";
            DynamicParameters param = new DynamicParameters();
            param.Add("@IdUzivatel", idUzivatel);
            vystup = spojeni.ExecuteScalar<int>(sql2, param, transakce,commandType: System.Data.CommandType.StoredProcedure);
            if (vystup > 0) return true;
            else return false;
        }
    }
}
