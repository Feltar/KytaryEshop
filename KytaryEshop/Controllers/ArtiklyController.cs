using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models.Enum;
using KytaryEshop.Areas.Identity.Data;
using KytaryEshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kytary.Models
{
    /// <summary>
    /// Kontroler zpracovávající žádosti směřující ke zobrazení jednotlivých stránek katalogu našeho eshopu. Katalog je implementován tak, že jednotlivé artikly jsou sdružovány do skupin po 12. 
    /// Podle toho, jaký seznam si uživatel zvolí, se mu může zobrazit seznam baskytar, akustických kytar či elektrických kytar.
    /// </summary>
    public class ArtiklyController : Controller
    {
        private readonly UserManager<UzivatelIdentita> _userManager;
        private readonly SignInManager<UzivatelIdentita> _signInManager;
        public ArtiklyController(UserManager<UzivatelIdentita> userManager, SignInManager<UzivatelIdentita> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Na vstupu akce obdrží typ artiklů na skladě, které si chce uživatel zobrazit a o kolikátou stránku seznamu se jedná. Artikly jsou sdružovány do skupin po 12. 
        /// Na výstup dá zobrazení s příslušnými artikly a výstupními daty. 
        /// </summary>
        /// <param name="typ">Typ artiklu, které si chce uživatel zobrazit.</param>
        /// <param name="KolikataStranka">Kolikátou stránku katalogu si chce uživatel zobrazit.</param>
        /// <returns>Zobrazení požadované stránky katalogu.</returns>
        IActionResult VratVhodnouStranku(Kytary.Backend.BModels.TypyArtiklu typ, int KolikataStranka)
        {
            //Nacteni artiklu
            //Spočteme indexy artiklů, které se na dané strance vyskytuji a příslušné artikly načteme z databáze
            int PocetPolozek = ArtiklProcesor.PocetArtiklu(typ);
            int k = 0;
            if (PocetPolozek % 12 > 0)
            {
                k = 1;
            }
            ViewBag.PocetStranek = (PocetPolozek / 12) + k;
            ViewBag.TypArtiklu = (int)typ;

            ViewBag.KolikataPolozka = KolikataStranka;
            List<ArtiklBModel> nacteneArtikly = ArtiklProcesor.NactiArtikly(12 * (KolikataStranka - 1), 12, typ);


            //U prvky v uživatelově košíku, načteme jejich četnost. S touto informací převedem načtené artikly z databáze na frontendové položky zobrazení
            var idUzivatel = _userManager.GetUserId(HttpContext.User);
            List<PolozkaKosikuLightModel> kosikLight = KosikUtility.nactiKosikZDatabaze(idUzivatel);
            List<PolozkaKosikuKompletModel> polozkyKomplet = new List<PolozkaKosikuKompletModel>();
            foreach (var item in nacteneArtikly)
            {
                ArtiklModel artikl = new ArtiklModel(item); 
                var prvekSPrislusnymId = kosikLight.Where(x => x.idArtiklu == item.IdArtikl).FirstOrDefault(); 
                PolozkaKosikuKompletModel polozka;
                if (prvekSPrislusnymId is not null) 
                {
                    polozka = new PolozkaKosikuKompletModel(artikl, prvekSPrislusnymId.pocetKusu);
                }
                else
                {
                    polozka = new PolozkaKosikuKompletModel(artikl, 0);
                }
                polozkyKomplet.Add(polozka);
            }
            return View("ZobrazeniArtiklu", polozkyKomplet);
        }
        /***********************************************************************************/
        /// <summary>
        /// Akce vrací některou ze stránek seznamu baskytar, které má eshop na skladě. Artikly jsou sdružovány do stránek po 12. 
        /// Na vstupu akce obdrží kolikátou stránku seznamu si chce uživatel zobrazit, na výstup dá její zobrazení společně s příslušnými daty.
        /// </summary>
        /// <param name="KolikataPolozka">Kolikátou stránku katalogu si chce uživatel zobrazit.</param>
        /// <returns>Zobrazení požadované stránky katalogu.</returns>
        public IActionResult StrankaBaskytary(int KolikataPolozka)
        {
            return VratVhodnouStranku(Kytary.Backend.BModels.TypyArtiklu.Baskytary, KolikataPolozka);

        }
        /***********************************************************************************/
        /// <summary>
        /// Akce vrací některou ze stránek seznamu elektrických kytar, které má eshop na skladě. Artikly jsou sdružovány do stránek po 12. 
        /// Na vstupu akce obdrží kolikátou stránku seznamu si chce uživatel zobrazit, na výstup dá její zobrazení společně s příslušnými daty.
        /// </summary>
        /// <param name="KolikataPolozka">Kolikátou stránku katalogu si chce uživatel zobrazit.</param>
        /// <returns>Zobrazení požadované stránky katalogu.</returns>
        public IActionResult StrankaElektrickeKytary(int KolikataPolozka)
        {
            return VratVhodnouStranku(Kytary.Backend.BModels.TypyArtiklu.ElektrickeKytary, KolikataPolozka);
        }

        /***********************************************************************************/

        /// <summary>
        /// Akce vrací některou ze stránek seznamu akustických kytar, které má eshop na skladě. Artikly jsou sdružovány do stránek po 12. 
        /// Na vstupu akce obdrží kolikátou stránku seznamu si chce uživatel zobrazit, na výstup dá její zobrazení společně s příslušnými daty.
        /// </summary>
        /// <param name="KolikataPolozka">Kolikátou stránku katalogu si chce uživatel zobrazit.</param>
        /// <returns>Zobrazení požadované stránky katalogu.</returns>
        public IActionResult StrankaAkustickeKytary(int KolikataPolozka)
        {
            return VratVhodnouStranku(Kytary.Backend.BModels.TypyArtiklu.AkustickeKytary, KolikataPolozka);
        }
        /****************************************************************************************/
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Akce vrací některou ze stránek seznamu směsi artiklů, které má eshop na skladě. Artikly jsou sdružovány do stránek po 12. 
        /// Na vstupu akce obdrží kolikátou stránku seznamu si chce uživatel zobrazit, na výstup dá její zobrazení společně s příslušnými daty.
        /// </summary>
        /// <param name="KolikataPolozka">Kolikátou stránku katalogu si chce uživatel zobrazit.</param>
        /// <returns>Zobrazení požadované stránky katalogu.</returns>
        public IActionResult StrankaVsechnyArtikly(int KolikataPolozka)
        {
            /// <summary> 
            /// Akce vrací některou ze stránek seznamu všech artiklů, které má eshop na skladě. Artikly jsou sdružovány do stránek po 12. 
            /// Na vstupu akce obdrží kolikátou stránku seznamu si chce uživatel zobrazit a na výstup dá její zobrazení společně s příslušnými daty.
            /// </summary>
            int PocetPolozek = ArtiklProcesor.PocetArtiklu();
            int k = 0;
            if (PocetPolozek % 12 > 0)
            {
                k = 1;
            }
            //Zavolej nacti kosik z cookies, pokud prazdnej zavolej nacteni z databaze...

            ViewBag.PocetStranek = (PocetPolozek / 12) + k;
            ViewBag.TypArtiklu = (int)TypArtiklu.Smes;
            ViewBag.KolikataPolozka = KolikataPolozka;
            //predelej nacti artikly na nacti polozky - pak pridej do view, pocet kusu
            List<ArtiklBModel> nacteneArtikly = ArtiklProcesor.NactiArtikly(12 * (KolikataPolozka - 1), 12);



            //!!!!!!!Je treba osetrit jeslize tam ten kosik neni - Inicializace kosiku.
            var idUzivatel = _userManager.GetUserId(HttpContext.User);
            List<PolozkaKosikuLightModel> kosikLight = KosikUtility.nactiKosikZDatabaze(idUzivatel);
            List<PolozkaKosikuKompletModel> polozkyKomplet = new List<PolozkaKosikuKompletModel>();
            foreach (var item in nacteneArtikly) //pro kazdy nacteny prvek, jez patri na tuto stranku
            {
                ArtiklModel artikl = new ArtiklModel(item); //prevedeme na front end artikl (z Backend artiklu)
                var prvekSPrislusnymId = kosikLight.Where(x => x.idArtiklu == item.IdArtikl).FirstOrDefault(); //podivame se, jestli je dany artikl v kosiku.
                PolozkaKosikuKompletModel polozka = (prvekSPrislusnymId is null) ?
                                                new PolozkaKosikuKompletModel(artikl, 0)
                                              : new PolozkaKosikuKompletModel(artikl, prvekSPrislusnymId.pocetKusu);
                polozkyKomplet.Add(polozka);
            }
            return View("ZobrazeniArtiklu", polozkyKomplet);
        }
        /****************************************************************************************/

        public IActionResult ZvysPocetPrvku(int pocetPrvku)
        {
            return PartialView("_urceniPoctuPrvkuKarty", pocetPrvku);
        }

        /****************************************************************************************/

    }
}
