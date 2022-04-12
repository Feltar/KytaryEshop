
namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.PolozkaKosiku.
    /// </summary>
    public class PolozkaKosikuBModel
    {
        public virtual ArtiklBModel Artikl { get; set; }


        public PolozkaKosikuBModel() {  }
        public virtual int Id { get; set; }
        public virtual string IdUzivatel { get; set; }
        public virtual int PocetKusu { get; set; }
    }
}
