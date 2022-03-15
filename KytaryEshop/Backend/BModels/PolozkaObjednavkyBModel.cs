namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.ObjednavkaArtikl.
    /// </summary>
    public class PolozkaObjednavkyBModel
    {

        public int IdArtikl { get; set; }
        public int PocetKusu { get; set; }
        public int CenaKus { get; set; }
        public string Znacka { get; set; }
        public string NazevProdukt { get; set; }
        public TypyArtiklu TypArtiklu { get; set; }
        public PolozkaObjednavkyBModel(int IdArtikl, string Znacka, string NazevProdukt, int TypArtiklu, int CenaKus, int PocetKusu)
        {
            this.IdArtikl = IdArtikl;
            this.PocetKusu = PocetKusu;
            this.CenaKus = CenaKus;
            this.Znacka = Znacka;
            this.NazevProdukt = NazevProdukt;
            this.TypArtiklu = (TypyArtiklu)TypArtiklu;
        }

    }
}
