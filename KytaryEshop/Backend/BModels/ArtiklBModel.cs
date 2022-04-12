namespace Kytary.Backend.BModels
{
    using KytaryEshop;
    using NHibernate;
    /// <summary>
    /// Model položky v tabulce dbo.Artikl.
    /// </summary>
    public class ArtiklBModel
    {
        public virtual TypyArtiklu TypArtiklu { get; set; }
        public virtual int IdArtikl { get; set; }
        public virtual int CenaKus { get; set; }
        public virtual string Znacka { get; set; }
        public virtual string NazevProdukt { get; set; }
        public virtual int KusuNaSklade { get; set; }

        public ArtiklBModel() { }

    }
}
