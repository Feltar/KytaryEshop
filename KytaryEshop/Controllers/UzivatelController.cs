using Kytary.Backend.Business_Logika;
using Kytary.Models;
using KytaryEshop.Areas.Identity.Data;
using KytaryEshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Kytary.Controllers
{
    /// <summary>
    /// Kontroler zpracovávající žádosti směřující k uživatelským profilům - v našem případě pouze k přihlášení a registraci.  
    /// Obsahuje dependency injection.
    /// </summary>
    public class UzivatelController : Controller
    {
        private readonly UserManager<UzivatelIdentita> _userManager;
        private readonly SignInManager<UzivatelIdentita> _signInManager;
        public UzivatelController(UserManager<UzivatelIdentita> userManager, SignInManager<UzivatelIdentita> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Akce zpracovávající žádost ke zobrazení stránky s formulářem registrace uživatele
        /// </summary>
        /// <returns>
        /// Vrací stránku s formulářem registrace uživatele.
        /// </returns>
        public IActionResult Registrace()
        {
            return View();
        }


        /// <summary>
        /// Verifikace dat zadaných do vstupního formuláře zobrazení Registrace.cshtml. V případě kladného výstupu verifikace následuje vytvoření účtu a přesměrování na domovskou stránku, 
        /// nebo vrácení na zobrazení Registrace společně s výčtem chyb.
        /// </summary>
        /// <param name="vystup">Frontendový model registrovaného uživatele sloužící k verifikaci a ukládání dat uživatelského vstupu.</param>
        /// <returns>V závislosti na průběhu verifikace buď úvodní stránka, nebo stránka registrace.</returns>
        [HttpPost]
        public async Task<IActionResult> Registrace(UzivatelModel vystup)
        {
            vystup.ChybovaHlaska = "";

            if (!ModelState.IsValid) return View(vystup);

            var uzivatel = new UzivatelIdentita
            { 
                KrestniJmeno = vystup.KrestniJmeno,
                Prijmeni = vystup.Prijmeni,
                Adresa = vystup.Adresa,
                Email = vystup.EmailovaAdresa,
                UserName = vystup.UzivatelskeJmeno
            };

            var vysledekPrihlaseni = await _userManager.CreateAsync(uzivatel, vystup.Heslo); //predame CreateAync naseho custom usera, kterej dedi od IdentityUsera	
            if (vysledekPrihlaseni.Succeeded)//if we successfullu created user with that password
            {   
                await _signInManager.SignInAsync(uzivatel, false);
                return RedirectToAction("Index", "Home"); //kam chce presmerovat po prihlaseni
            }
            else
            {
                foreach (var chyba in vysledekPrihlaseni.Errors)
                {
                    ModelState.AddModelError("", chyba.Description);
                    vystup.ChybovaHlaska += chyba.Description;
                }
                return View(vystup);
            }
        }
        //*******************************************************************************************************
        /// <summary>
        /// Akce zpracovávající žádost ke zobrazení stránky s formulářem přihlášení uživatele
        /// </summary>
        /// <returns>
        /// Vrací stránku s formulářem přihlášení uživatele.
        /// </returns>
        public IActionResult Prihlaseni()
        {
            return View("Prihlaseni");
        }
        /// <summary>
        /// Verifikace dat zadaných do vstupního formuláře zobrazení Prihlaseni.cshtml. V případě kladného výstupu verifikace následuje přihlášení do systému a přesměrování na domovskou stránku, 
        /// nebo vrácení na zobrazení Prihlaseni.cshtml společně s výčtem chyb.
        /// </summary>
        /// <param name="vystup">Frontendový model přihlašovaného uživatele sloužící k verifikaci a ukládání dat uživatelského vstupu.</param>
        /// <returns>V závislosti na průběhu verifikace buď úvodní stránka, nebo stránka přihlášení.</returns>
        [HttpPost]
        public async Task<IActionResult> Prihlaseni(UzivatelModel vystupZFormulare)
        {

            if (vystupZFormulare.EmailovaAdresa == null|| vystupZFormulare.Heslo==null)
            {
                return View();
            }

            var uzivatelSeZadanymEmailem = await _userManager.FindByEmailAsync(vystupZFormulare.EmailovaAdresa);

            if (uzivatelSeZadanymEmailem == null)
            {
                ModelState.AddModelError("EmailovaAdresa", "Email nebyl nalezen.");
                return View(uzivatelSeZadanymEmailem);
            }
            if (await _userManager.CheckPasswordAsync(uzivatelSeZadanymEmailem, vystupZFormulare.Heslo) == false)
            {
                ModelState.AddModelError("Heslo", "Chybné heslo");
                return View(vystupZFormulare);

            }
            var vysledekPrihlaseni = await _signInManager.PasswordSignInAsync(uzivatelSeZadanymEmailem, vystupZFormulare.Heslo, true, true);
            
            if (vysledekPrihlaseni.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else if (vysledekPrihlaseni.IsLockedOut)
            {
                return View("Ucet je uzamceny.");
            }
            else
            {
                ModelState.AddModelError("", "Přihlášení se nepovedlo.");
                return View(vystupZFormulare);
            }
        }
        public async Task<ActionResult> Odhlaseni()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Zpracovává žádost ke zobrazení historie objednávek přihlášeného uživatele.
        /// </summary>
        /// <returns>
        /// Stránka s přehledem objednávek provedených přihlášeným uživatelem.
        /// </returns>
        [Authorize]
        public async Task<ActionResult> ZobrazHistoriiObjednavek()
        {
            var idUzivatel = _userManager.GetUserId(HttpContext.User);
            var objednavkyBackend = ObjednavkaProcesor.NacteniObjednavekVykonanychUzivatelem(idUzivatel);
            
            var objednavky = objednavkyBackend.Select(x => new ObjednavkaModel(x)).ToList();
            return View("ZobrazHistoriiObjednavek",objednavky);
        }

    }
}
