namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.Objednavka.
    /// </summary>
    public class ObjednavkaBModel
    {
        public List<PolozkaObjednavkyBModel> PolozkyObjednavky { get; set; }
        public DateTime Datum { get; set; }
        public int IdObjednavka { get; set; }
        public string IdUzivatel { get; set; }
        public TypyArtiklu typArtiklu { get; set; }
        public ObjednavkaBModel(DateTime Datum, int IdObjednavka, string idUzivatel) { 
            this.IdUzivatel = idUzivatel;
            this.IdObjednavka = IdObjednavka;
            this.Datum = Datum;
        }
    }
}
