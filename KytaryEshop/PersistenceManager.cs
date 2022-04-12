using Kytary.Backend.BModels;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate;

namespace KytaryEshop
{
    public static class PersistenceManager
    {
        public static ISessionFactory SessionFabrika;
        public static void ConfigureNHibernate()
        {
            try
            {
                // Inicializace konfiguracniho souboru
                Configuration cfg = new Configuration();
                cfg.Configure();

                // Predani konfiguracnich souboru z aktualni assembly
                Assembly thisAssembly = typeof(ArtiklBModel).Assembly;
                cfg.AddAssembly(thisAssembly);

                //Zkompilovani ConfigurationFactory
                SessionFabrika = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }
}
