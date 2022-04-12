namespace Kytary.Backend.BModels
{
    /// <summary>
    /// Model položky v tabulce dbo.Objednavka.
    /// </summary>
    public class ObjednavkaBModel
    {
        public ObjednavkaBModel() { }
        public IList<PolozkaObjednavkyBModel> PolozkyObjednavky { get; set; }

        public virtual DateTime Datum { get; set; }
        public virtual int IdObjednavka { get; set; }
        public virtual string IdUzivatel { get; set; }
    }
}
