using Kytary.Models;

namespace KytaryEshop.Models
{
    /// <summary>
    /// Model položky košíku uživatele s kompletními informacemi potřebnými pro zobrazeni produktu v seznamu produktů nacházejících se v uživatelově košíku.
    /// </summary>
    public class PolozkaKosikuKompletModel
    {
        public ArtiklModel Artikl;
        public int PocetKusuVKosiku { get; set; }
        public PolozkaKosikuKompletModel(ArtiklModel artikl, int pocetKusu)
        {
            this.Artikl = artikl;
            this.PocetKusuVKosiku = pocetKusu;
        }
    }
}
