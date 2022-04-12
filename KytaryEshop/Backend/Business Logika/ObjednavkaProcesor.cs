using Kytary.Backend.BModels;
using SqlKata;
using System.Data.SqlClient;
using Dapper;
using KytaryEshop;

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
        public static bool PridejObjednavku(DateTime datum, string IdUzivatel, IList<PolozkaKosikuBModel> polozkyKosiku)
        {
            //Zjisti jestli uzivatel existuje a jestli polozky kosiku nejsou null
            try
            {
                var fabrika = PersistenceManager.SessionFabrika;
                using (var session = fabrika.OpenSession())
                {
                    using (var transakce = session.BeginTransaction())
                    {
                        //Vytvoreni zaznamu o probehle objednavce
                        ObjednavkaBModel objednavka = new ObjednavkaBModel() { IdUzivatel = IdUzivatel, Datum = datum };
                        session.Save(objednavka);

                        //Vyrizeni jednotlivych pohledavek (= paru artikl,objednavka)
                        foreach (var polozka in polozkyKosiku)
                        {
                            //[TO DO]
                            int sklademArtiklu;
                            sklademArtiklu = session.Query<ArtiklBModel>()
                                                    .Where(y => y.IdArtikl == polozka.Artikl.IdArtikl)
                                                    .Select(x => x.KusuNaSklade)
                                                    .Single();
                            if (sklademArtiklu < polozka.PocetKusu)
                            {
                                transakce.Rollback();
                                return false;
                            }

                            PolozkaObjednavkyBModel polozkaObj = new PolozkaObjednavkyBModel()
                            {
                                Artikl = polozka.Artikl,
                                PocetKusu = polozka.PocetKusu,
                                CenaKus = polozka.Artikl.CenaKus,
                                IdObjednavka = objednavka.IdObjednavka,
                            };

                            //vytvoreni zaznamu o polozce objednavky
                            session.Save(polozkaObj);
                            //odebrani zakoupeneho poctu kusu ze skladu
                            polozka.Artikl.KusuNaSklade -= polozka.PocetKusu;
                            session.Update(polozka.Artikl);
                            //vyprazdneni polozky z kosiku uzivatele
                            var polozka2 = session.Get<PolozkaKosikuBModel>(polozka.Id);
                            session.Delete(polozka2);
                        }

                        transakce.Commit();
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        //***************************************************************************Vytváření záznamů o objednávkách*****************************************************
    }
}
