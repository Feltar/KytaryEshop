namespace Kytary.Backend.Data_Access
{
    using System.Configuration;
    using System.Data.SqlClient;

    /// <summary>
    /// Třída skrze kterou přistupujeme k řetězci spojení databáze s názvem DatabazeKytary.
    /// </summary>
    public static class SqlPristup
    {
        private static string JmenoRetezceVKonfiguracnimManageru = "RetezecSpojeniDatabazeKytary";
        private static string RetezecSpojeni() {
            
            return ConfigurationManager.ConnectionStrings[JmenoRetezceVKonfiguracnimManageru].ConnectionString;
        }
        /// <summary>
        /// Vytvoří novou instanci spojení k databázi s názvem DatabazeKytary.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection NoveSpojeni() {
            return new SqlConnection(RetezecSpojeni());        
        }
    }
}
