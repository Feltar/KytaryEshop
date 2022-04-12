using Kytary.Backend.BModels;

namespace Kytary.Models
{
    /// <summary>
    /// Model položky košíku uživatele s informacemi nutnými ke zobrazení aktuální četnosti jednotlivých produktů v košíku uživatele v rámci prohlížení  katalogu dostupných artiklů na skladě.
    /// </summary>
    public class PolozkaKosikuLightModel
    {
        
        public int idArtiklu { get; set; }
        public int pocetKusu { get; set; }
        public PolozkaKosikuLightModel(int idArtiklu, int pocetKusu) {
            this.idArtiklu = idArtiklu;
            this.pocetKusu = pocetKusu;
        }
        public static implicit operator PolozkaKosikuLightModel(PolozkaKosikuBModel vstup) {
            return new PolozkaKosikuLightModel(vstup.Artikl.IdArtikl, vstup.PocetKusu);
        }
    }
}
