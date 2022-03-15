using Kytary.Backend.Data_Access;

namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.PolozkaKosiku.
    /// </summary>
    public class PolozkaKosikuBModel
    {
        public int Id { get; set; }
        public string IdUzivatel { get; set; }
        public int IdArtikl { get; set; }
        public int PocetKusu { get; set; }

        public PolozkaKosikuBModel(int Id, string IdUzivatel, int IdArtikl, int PocetKusu)
        {
            this.Id = Id;
            this.IdUzivatel = IdUzivatel;
            this.IdArtikl = IdArtikl;
            this.PocetKusu = PocetKusu;
        }
        public PolozkaKosikuBModel(int IdArtikl, int PocetKusu)
        {
            this.Id = -1;
            this.IdUzivatel = "";
            this.IdArtikl = IdArtikl;
            this.PocetKusu = PocetKusu;
        }
    }
}
