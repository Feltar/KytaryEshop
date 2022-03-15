using Kytary.Backend.BModels;
using Kytary.Models.Enum;

namespace KytaryEshop.Models
{
    /// <summary>
    /// Model položky objednávky uživatele s kompletními informacemi potřebnými pro zobrazeni produktu v rámci některé z objednávek.
    /// </summary>
    public class PolozkaObjednavkyModel
    {
        public TypArtiklu TypArtiklu { get; set; }
        public string Znacka { get; set; }
        public string NazevProdukt { get; set; }
        public int PocetKusu { get; set; }
        public int CenaKus { get; set; }
        public PolozkaObjednavkyModel(PolozkaObjednavkyBModel polozka)
        {
            this.TypArtiklu = (TypArtiklu)((int)polozka.TypArtiklu);
            this.Znacka = polozka.Znacka;
            this.PocetKusu = polozka.PocetKusu;
            this.NazevProdukt = polozka.NazevProdukt;
            this.CenaKus = polozka.CenaKus;
        }

    }
}
