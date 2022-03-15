/*  BYLO NAHRAZENO IDENTITY COREM
 * 
 * 
 * 
 * 
 * 
 * using Kytary.Backend.BModels;
using Kytary.Backend.Data_Access;
using SqlKata;

namespace Kytary.Backend.Business_Logika
{
    public static class UzivatelProcesor
    {
        const string NazevDatabaze = "dbo.Uzivatel";
        private static string GenerujSul() {
            Random rn = new Random();
            String sul_prac = "";
            String znaky = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < Math.Max(rn.Next(256), 10); i++)
            {
                sul_prac += znaky[rn.Next(znaky.Length)];
            }
            return sul_prac;
        }
        private static string SpoctiHash(string sul, string heslo) {
            if (String.IsNullOrEmpty(heslo))
            {
                return String.Empty;
            }
            //Sha256 je typu IDisposable, using je elegantnější způsob, kdy nemusim vypisovat try/catch atp...
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // SHA256
                byte[] prekodovano_na_pole_bajtu = System.Text.Encoding.UTF8.GetBytes(heslo + sul);
                byte[] hash_byty = sha.ComputeHash(prekodovano_na_pole_bajtu);
                //metoda BitConverter.ToString(...) oddeluje jednotlive hexadecimalni kody vstupních znaků znakem "-". Ty musime smazat - napr funkcí string.Replace(...). 
                string hash = BitConverter.ToString(hash_byty).Replace("-", String.Empty);
                return hash;
            }
        }
        public static bool PrihlaseniEmailHeslo(string email, string heslo) { //pak se vypise chybny email nebo heslo na frontendu!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            UzivatelBModel uzivatel   = NactiUzivateleEmailem(email); 
            if (uzivatel == null) return false;

            string sul = uzivatel.Sul;
            string hesloHash = uzivatel.HesloHash;

            //Porovnani nacteneho hesloHashe s posolenym vstupem
            string verHesloHash = SpoctiHash(sul, heslo);
            if (hesloHash == SpoctiHash(sul, heslo))
                return true;
            else
                return false;
        }
        //--------------------------------------------------------------------Bezpecnost a Verifikace
        //********************************************************************Databaze
        public static int Registrace(string uzivatelskeJmeno, string emailovaAdresa,string heslo) {
            
            //Generovani soli a Hashe heslo||sul
            string sul = GenerujSul();
            string hesloHash = SpoctiHash(sul, heslo); ;

            //Tvoreni Sql dotazu pomoci SqlKata knihovny
            SqlKata.Query query = new SqlKata.Query(NazevDatabaze).AsInsert(new { UzivatelskeJmeno = uzivatelskeJmeno, HesloHash = hesloHash, EmailovaAdresa = emailovaAdresa, Sul = sul });
            SqlKata.Compilers.SqlServerCompiler compiler = new SqlKata.Compilers.SqlServerCompiler();
            string sql = compiler.Compile(query).ToString();
            return SqlPristup.VykonejDML(sql);
        }
        public static bool JeEmailObsazenVDatabazi( string email ) {
            //Tvoreni Sql dotazu
            SqlKata.Query query = new SqlKata.Query();
            query.Select("*").From(NazevDatabaze).Where("EmailovaAdresa", email).AsCount();
            SqlKata.Compilers.SqlServerCompiler compiler = new SqlKata.Compilers.SqlServerCompiler();
            string sql = compiler.Compile(query).ToString();
            
            //Vykonani Sql dotazu
            if (SqlPristup.NactiPrvniDapper<int>(sql) > 0) return true;
            else return false;
        }
        public static UzivatelBModel NactiUzivateleEmailem(string email)
        {
            //Tvoreni Sql dotazu
            SqlKata.Query query = new SqlKata.Query();
            query.Select("*").From(NazevDatabaze).Where("EmailovaAdresa", email);
            SqlKata.Compilers.SqlServerCompiler compiler = new SqlKata.Compilers.SqlServerCompiler();
            string sql = compiler.Compile(query).ToString();
            
            
            //Vykonani Sql dotazu
            UzivatelBModel uzivatel = SqlPristup.NactiPrvniDapper<UzivatelBModel>(sql);
            return SqlPristup.NactiPrvniDapper<UzivatelBModel>(sql);
        }

        //********************************************************************Databaze






        





    }
}
*/