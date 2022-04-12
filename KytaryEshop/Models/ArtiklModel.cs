using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models.Enum;
using KytaryEshop;
using NHibernate;


namespace Kytary.Models
{
    /// <summary>
    /// Model artiklu skladu pro zobrazeni v katalogu produktů.
    /// </summary>
    public class ArtiklModel
    {
        public TypArtiklu TypArtiklu { get; set; }
        public int IdArtikl { get; }
        public int CenaKus { get; set; }
        public string Znacka { get; set; }
        public string NazevProdukt { get; set; }
        public int KusuNaSklade { get; set; }        
        public ArtiklModel(ArtiklBModel nactenaPolozka)
        {
            this.CenaKus = nactenaPolozka.CenaKus;
            this.Znacka = nactenaPolozka.Znacka;
            this.NazevProdukt = nactenaPolozka.NazevProdukt;
            this.KusuNaSklade = nactenaPolozka.KusuNaSklade;
            this.IdArtikl = nactenaPolozka.IdArtikl;
            this.TypArtiklu = (Kytary.Models.Enum.TypArtiklu)((int)nactenaPolozka.TypArtiklu);
        }
        
    }
}
