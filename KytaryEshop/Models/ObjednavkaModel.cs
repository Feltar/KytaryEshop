using Kytary.Backend.BModels;
using Kytary.Models;

namespace KytaryEshop.Models
{
    /// <summary>
    /// Model objednávky uživatele s kompletními informacemi potřebnými pro zobrazeni partikulární objednávky při zobrazení přehledu objednávek určitého uživatele.
    /// </summary>
    public class ObjednavkaModel
    {
        public List<PolozkaObjednavkyModel> Artikly { get; set; }
        public DateTime Datum { get; set; }
        public int IdObjednavka { get; set; }
        public ObjednavkaModel(ObjednavkaBModel objednavkaBackend)
        {
            this.IdObjednavka = objednavkaBackend.IdObjednavka;
            this.Datum = objednavkaBackend.Datum;

            this.Artikly = objednavkaBackend.PolozkyObjednavky.Select(x => new PolozkaObjednavkyModel(x)).ToList();
        }
    }
}
