using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAplikacjaBudowlanka.DBModels;
using ProjektAplikacjaBudowlanka.Models;
using System.Diagnostics;

namespace ProjektAplikacjaBudowlanka.Controllers
{
    public class HomeController : Controller
    {
        private readonly BudowlankaDBContext _budowlankaContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(BudowlankaDBContext budowlankaContext, ILogger<HomeController> logger)
        {
            _budowlankaContext = budowlankaContext;
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View();
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