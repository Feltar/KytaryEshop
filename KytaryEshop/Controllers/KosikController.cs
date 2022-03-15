using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models;
using KytaryEshop.Areas.Identity.Data;
using KytaryEshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Kytary.Controllers
{
    /// <summary>
    /// Kontroler zpracovávající žádosti směřující ke zobrazení a případné modifikaci košíku přihlášeného uživatele.  Obsahuje dependency injection.
    /// </summary>
    public class KosikController : Controller
    {
        private readonly UserManager<UzivatelIdentita> _userManager;
        private readonly SignInManager<UzivatelIdentita> _signInManager;
        public KosikController(UserManager<UzivatelIdentita> userManager, SignInManager<UzivatelIdentita> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary> 
        /// Načte z databáze prvky košíku přihlášeného uživatele, převede je na typ frontendového modelu kosikKomplet a tato data společně se zobrazením ZobrazKošík dá na výstup.
        /// </summary>
        [Authorize]
        public IActionResult ZobrazKosik()
        {
            var idUzivatel = _userManager.GetUserId(HttpContext.User);
            List<PolozkaKosikuLightModel> kosikLight = KosikUtility.nactiKosikZDatabaze(idUzivatel);

            List<PolozkaKosikuKompletModel> kosikKomplet = new List<PolozkaKosikuKompletModel>();

            foreach (var item in kosikLight)
            {
                ArtiklModel artikl = new ArtiklModel(item.idArtiklu);
                PolozkaKosikuKompletModel polozka = new PolozkaKosikuKompletModel(artikl, item.pocetKusu);
                kosikKomplet.Add(polozka);
            }

            return View("ZobrazKosik",kosikKomplet ??= new List<PolozkaKosikuKompletModel>());
        }

        //********************************************************************Kontrolery tlacitek pro nastavení četnoasti artiklů v košíku.
        /// <summary>
        ///Metoda zpracovává požadavek na dekrementaci počtu artiklů v tabulce košíku přihlášeného uživatele. 
        /// Vrací výsledný počet artiklů.
        /// </summary>
        /// <param name="idArtikl"> Identifikátor artiklu, jehož četnost v košíku chceme snížit. </param>
        /// <param name="pocet">Aktuální četnost odkazovaného artiklu v košíku.</param>
        /// <returns>Počet artiklů v tabulce košíku po skončení této metody.</returns>
        public int MinusTlacitkoKontroler(int idArtikl, int pocet)
        {

            var idUzivatel = _userManager.GetUserId(HttpContext.User);


            if (idUzivatel is null ||pocet<=0) return 0;

            if (pocet == 1)
            {
                if (PolozkaKosikuProcesor.SmazPolozkuZKosiku(idArtikl,idUzivatel))
                    return 0;
                else
                    return 1;

            }
            else {
                if (PolozkaKosikuProcesor.ZmenKvantitu(idArtikl, idUzivatel, pocet - 1))
                    return pocet - 1;
                else
                    return pocet;

            }

        }
        /// <summary>
        ///Metoda zpracovává požadavek na inkrementaci počtu artiklů s identifikátorem idArtikl v tabulce košíku přihlášeného uživatele.  Vrací výsledný počet artiklů.
        /// </summary>
        /// <param name="idArtikl">Identifikátor artiklu, jehož četnost v košíku chceme zvýšit. </param>
        /// <param name="pocet">Aktuální četnost odkazovaného artiklu v košíku.</param>
        /// <returns>Počet artiklů v tabulce košíku po skončení této metody.</returns>
        public int PlusTlacitkoKontroler(int idArtikl, int pocet)
        {
            var idUzivatel = _userManager.GetUserId(HttpContext.User);

            if (idUzivatel is null || pocet + 1 > ArtiklProcesor.CetnostArtiklu(idArtikl)) return pocet;


            if (pocet == 0)
                PolozkaKosikuProcesor.UlozPolozkuUzivatele(idArtikl, 1, idUzivatel);
            else
                PolozkaKosikuProcesor.ZmenKvantitu(idArtikl, idUzivatel, pocet + 1);

            return pocet + 1;
        }
        //********************************************************************Kontrolery tlacitek pro nastavení četnoasti artiklů v košíku.

        /// <summary>
        ///Předá uživatelův košík k odbavení do datové vrstvy (metodě ObjednavkaProcesor.PridejObjednavku). 
        ///Posléze vrátí zobrazení informující uživatele, o tom, jestli vyřízení objednávky proběhlo úspěšně.
        /// </summary>
        /// <returns>Zobrazení informující klienta o průběhu vyřízení objednávky.</returns>
        [Authorize]
        public IActionResult VyridObjednavku() {     

            var idUzivatel = _userManager.GetUserId(HttpContext.User);
            
            List<PolozkaKosikuBModel> kosik = KosikUtility.nactiKosikZDatabaze(idUzivatel)
                                                               .Select(x => new  PolozkaKosikuBModel(x.idArtiklu,x.pocetKusu)).ToList();
            bool vyrizeniObjednavkyDopadloZdarne = ObjednavkaProcesor.PridejObjednavku(DateTime.Now,idUzivatel, kosik);
            return View("ObjednavkaVyrizena", vyrizeniObjednavkyDopadloZdarne);
        }


    }
}
