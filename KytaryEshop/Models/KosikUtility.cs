using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models;
using NHibernate;
using System.Linq;

namespace KytaryEshop.Models
{
    /// <summary>
    /// Třída obsahující funkce (v našem případě lépe řečeno funkci) určenou k načítání košíku uživatele ve formátu vhodném pro zobrazení.
    /// </summary>
    public class KosikUtility
    {
        public static List<PolozkaKosikuLightModel>  nactiKosikZDatabaze(string idUzivatel) {

            IList<PolozkaKosikuBModel> backendKosik;
            ISessionFactory fabrika = PersistenceManager.SessionFabrika;
            using (var session = fabrika.OpenSession())
            {
                backendKosik = session.QueryOver<PolozkaKosikuBModel>()
                                      .Where(x=> x.IdUzivatel == idUzivatel)
                                      .List() ?? new List<PolozkaKosikuBModel>();  
            }

            //[TO DO]
            //List<PolozkaKosikuBModel> backanedKosik = PolozkaKosikuProcesor.NactiPolozkyKosikuUzivatele(idUzivatel) ?? new List<PolozkaKosikuBModel>();
            List<PolozkaKosikuLightModel> frontendKosik = backendKosik.Select(x => (PolozkaKosikuLightModel)x).ToList();
            return frontendKosik;
        }

    }
}
