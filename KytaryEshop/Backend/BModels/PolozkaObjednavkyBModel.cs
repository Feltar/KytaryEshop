namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.ObjednavkaArtikl.
    /// </summary>
    public class PolozkaObjednavkyBModel
    {
        public int IdPolozkaObjednavky { get; set; }
        public PolozkaObjednavkyBModel() { }
        public virtual ArtiklBModel Artikl { get; set; }
        
        public int IdObjednavka { get; set; }
        public int PocetKusu { get; set; }
        public int CenaKus { get; set; }

    }
}
