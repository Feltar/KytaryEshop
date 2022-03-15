using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models;


namespace KytaryEshop.Models
{
    /// <summary>
    /// Třída obsahující funkce (v našem případě lépe řečeno funkci) určenou k načítání košíku uživatele a komunikaci s backendem.
    /// </summary>
    public class KosikUtility
    {
        public static List<PolozkaKosikuLightModel>  nactiKosikZDatabaze(string idUzivatel) {
            List<PolozkaKosikuBModel> backanedKosik = PolozkaKosikuProcesor.NactiPolozkyKosikuUzivatele(idUzivatel) ?? new List<PolozkaKosikuBModel>();
            List<PolozkaKosikuLightModel> frontendKosik = backanedKosik.Select(x => (PolozkaKosikuLightModel)x).ToList();
            return frontendKosik;
        }

    }
}
