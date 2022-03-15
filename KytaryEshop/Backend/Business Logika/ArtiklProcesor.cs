using Dapper;
using Kytary.Backend.BModels;
using Kytary.Backend.Data_Access;
using System.Data.SqlClient;

namespace Kytary.Backend.Business_Logika
{
    /// <summary>
    /// Třída datového přístupu k tabulce dbo.Artikl obsahující data vztahující se k prodejním artiklů našeho eshopu.
    /// </summary>
    public static class ArtiklProcesor
    {

        const string NazevDatabaze = "dbo.Artikl";
        //***********************************Administrace počtu kusů na skladě***************************************************************
        /// <summary>
        /// Změní počet kusů na skladě o zadanou hodnotu.
        /// </summary>
        /// <param name="IdArtikl"> Identifikátor zboží, u kterého chceme změnit počet kusů na skladě. </param>
        /// <param name="OKolikKusu"> Počet, o který chceme změnit množství artiklu na skladě. </param>
        /// <returns>Počet položek tabulky, u kterých došlo ke změně.</returns>
        private static int ZmenPocetKusuO(int IdArtikl, int OKolikKusu)
        {
            int ovlivnenychRadku;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                string sql2 = "spArtikl_ZmenPocetKusuO";


                DynamicParameters param = new DynamicParameters();
                param.Add("@IdArtikl", IdArtikl);
                param.Add("@OKolikKusu", OKolikKusu);

                ovlivnenychRadku = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }

            return ovlivnenychRadku;
        }
        /// <summary>
        /// Sníží počet kusů na skladě o zadanou hodnotu.
        /// </summary>
        /// <param name="IdArtikl">Identifikátor zboží, u kterého chceme změnit počet kusů na skladě.</param>
        /// <param name="pocet">Počet, o který chceme snížit množství artiklu na skladě.</param>
        /// <returns>Počet řádků, u kterých došlo ke změně.</returns>
        public static int ProdejArtikl(int IdArtikl, int pocet)
        {
            if (pocet < 0)
                throw new Exception("Počet kusů, o jaký chcete snížit množství na skladě, musí být alespoň 0.");
            return ZmenPocetKusuO(IdArtikl, -pocet);
        }
        /// <summary>
        /// Sníží počet kusů na skladě o zadanou hodnotu v rámci probíhající transakce.
        /// </summary>
        /// <param name="IdArtikl">Identifikátor zboží, u kterého chceme změnit počet uskladněných kusů.</param>
        /// <param name="pocet"> Počet, o který chceme změnit množství artiklu na skladě.</param>
        /// <param name="spojeni">Spojení, na kterém probíhá aktuální transakce.</param>
        /// <param name="transakce">Transakce, v rámci které má být metoda vykonána.</param>
        /// <returns>Počet záznamů tabulky, u kterých došlo ke změně.</returns>
        public static int ProdejArtikl(int IdArtikl, int pocet, SqlConnection spojeni, SqlTransaction transakce)
        {
            return ZmenPocetKusuO(IdArtikl, -pocet, spojeni, transakce);
        }
        /// <summary>
        /// Sníží počet kusů na skladě o zadanou hodnotu v rámci probíhající transakce.
        /// </summary>
        /// <param name="IdArtikl"> Identifikátor zboží, u kterého chceme změnit počet uskladněných kusů. </param>
        /// <param name="OKolikKusu"> Počet, o který chceme snížit množství artiklu na skladě.</param>
        /// <param name="spojeni">Spojení, na kterém probíhá aktuální transakce.</param>
        /// <param name="transakce">Transakce, v rámci které má být metoda vykonána.</param>
        /// <returns>Počet záznamů tabulky, u kterých došlo ke změně.</returns>
        private static int ZmenPocetKusuO(int IdArtikl, int OKolikKusu, SqlConnection spojeni, SqlTransaction transakce)
        {
            int ovlivnenychRadku;
            string sql2 = "spArtikl_ZmenPocetKusuO";
            DynamicParameters param = new DynamicParameters();
            param.Add("@IdArtikl", IdArtikl);
            param.Add("@OKolikKusu", OKolikKusu);
            ovlivnenychRadku = spojeni.ExecuteScalar<int>(sql2, param, transakce,commandType: System.Data.CommandType.StoredProcedure);

            return ovlivnenychRadku;
        }

        //***********************************Administrace počtu kusů na skladě***************************************************************
        //-----------------------------------Nacitani modelu artiklů---------------------------------------------------------------------------
        /// <summary>
        /// Načte artikl se specifikovaným identifikátorem. Pokud daný artikl není na skladě, tak vrací null.
        /// </summary>
        /// <param name="idArtikl">Identifikátor artiklu, který chceme načíst.</param>
        /// <returns> Načtený artikl (může být null).</returns>
        public static ArtiklBModel NactiArtikl(int idArtikl)
        {

            string sql2 = "spArtikl_NactiArtikl";
            ArtiklBModel vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdArtikl", idArtikl);

                vysledek = spojeni.QueryFirstOrDefault<ArtiklBModel>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vysledek;
        }
        /// <summary>
        /// Načte specifikovaný počet artiklů určitého typu z databáze s tím, že přeskočí zadaný počet položek.
        /// </summary>
        /// <param name="offset">Počet artiklů, které má metoda při načítání přeskočit.</param>
        /// <param name="pocet">Počet artiklů, které má metoda načíst.</param>
        /// <param name="typ">Typ artiklu, který má metoda načíst.</param>
        /// <returns>Seznam artiklů na skladě s příslušnými indexy. </returns>
        public static List<ArtiklBModel> NactiArtikly(int offset, int pocet, TypyArtiklu typ)
        {
            string sql2 = "spArtikl_NactiArtiklyUrcitehoTypu";
            List<ArtiklBModel> vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@Offset", offset);
                param.Add("@Pocet", pocet);
                param.Add("@Typ", ((int)typ));

                vysledek = spojeni.Query<ArtiklBModel>(sql2, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            return vysledek;
        }
        /// <summary>
        /// Načte specifikovaný počet artiklů z databáze s tím, že přeskočí zadaný počet položek.
        /// </summary>
        /// <param name="offset">Počet artiklů, které má metoda při načítání přeskočit.</param>
        /// <param name="pocet">Počet artiklů, které má metoda načíst.</param>
        /// <returns>Seznam artiklů na skladě s příslušnými indexy. </returns>
        public static List<ArtiklBModel> NactiArtikly(int offset, int pocet)
        {
            string sql2 = "spArtikl_NactiArtiklyLibovolnehoTypu";
            List<ArtiklBModel> vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@Offset", offset);
                param.Add("@Pocet", pocet);

                vysledek = spojeni.Query<ArtiklBModel>(sql2, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            return vysledek;
        }
        //-----------------------------------Nacitani modelu artiklů---------------------------------------------------------------------------
        //***********************************Načítání počtu artiklů a jejich četností***************************************************************

        /// <summary>
        /// Zjistí, počet různých artiklů (ve smyslu druhu položky - př. Gibson Les Paul) na skladě od určitého typy artiklu (např. baskytara).  
        /// </summary>
        /// <param name="typ"> Typ artiklu jehož počet chceme načíst.</param>
        /// <returns> Počet artiklů zadaného typu. </returns>
        public static int PocetArtiklu(TypyArtiklu typ)
        {
            string sql2 = "spArtikl_PocetArtikluUrcitehoTypu";
            int vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@Typ", typ);

                vysledek = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vysledek;

        }
        /// <summary>
        /// Zjistí, počet různých artiklů (ve smyslu druhu položky - př. Gibson Les Paul) na skladě přes všechny typy artiklu (např. baskytara).  
        /// </summary>
        /// <returns>Počet různých artiklů na skladě.</returns>
        public static int PocetArtiklu()
        {
            string sql2 = "spArtikl_PocetArtikluPresVsechnyTypy";

            int vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {
                vysledek = spojeni.ExecuteScalar<int>(sql2, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vysledek;
        }
        /// <summary>
        /// Zjistí počet produktů na skladě od artiklu se zadaným identifikátorem.
        /// </summary>
        /// <param name="id">Id artiklu, jehož četnost chceme zjistit.</param>
        /// <returns>Četnost artiklu se zadaným identifikátorem.</returns>
        public static int CetnostArtiklu(int id)
        {
            string sql2 = "spArtikl_CetnostArtiklu";
            int vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdArtikl",id);

                vysledek = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vysledek;
        }
        /// <summary>
        /// Zjistí prodejní cenu artiklu se zadaným identifikátorem.
        /// </summary>
        /// <param name="id">Id artiklu, jehož četnost chceme zjistit</param>
        /// <returns>Cena artiklu se zadaným identifikátorem. </returns>
        public static int CenaArtiklu(int id)
        {
            string sql2 = "spArtikl_CenaArtiklu";
            int vysledek;
            using (SqlConnection spojeni = SqlPristup.NoveSpojeni())
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@IdArtikl", id);

                vysledek = spojeni.ExecuteScalar<int>(sql2, param, commandType: System.Data.CommandType.StoredProcedure);
            }
            return vysledek;
        }
        //***********************************Načítání počtu artiklů a jejich četností***************************************************************
    }
}
