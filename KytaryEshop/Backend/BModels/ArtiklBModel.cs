namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.Artikl.
    /// </summary>
    public class ArtiklBModel
    {
        public TypyArtiklu TypArtiklu { get; set; }
        public int IdArtikl { get; }
        public int CenaKus { get; set; }
        public string Znacka { get; set; }
        public string NazevProdukt { get; set; }
        public int KusuNaSklade { get; set; }
        public ArtiklBModel(int IdArtikl, int CenaKus, string Znacka, string NazevProdukt, int KusuNaSklade) { 
            this.CenaKus = CenaKus;
            this.Znacka = Znacka;
            this.NazevProdukt = NazevProdukt;
            this.KusuNaSklade = KusuNaSklade;        
            this.IdArtikl = IdArtikl;
        }
        public ArtiklBModel(int IdArtikl, int CenaKus, string Znacka, string NazevProdukt, int KusuNaSklade, int TypArtiklu)
        {
            this.CenaKus = CenaKus;
            this.Znacka = Znacka;
            this.NazevProdukt = NazevProdukt;
            this.KusuNaSklade = KusuNaSklade;
            this.IdArtikl = IdArtikl;
            this.TypArtiklu = (TypyArtiklu)TypArtiklu;
        }

    }
}
