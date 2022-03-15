using Kytary.Backend.BModels;
using Kytary.Backend.Business_Logika;
using Kytary.Models;
using KytaryEshop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kytary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("StrankaVsechnyArtikly", "Artikly", new { KolikataPolozka = 1 });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}