using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppPeliculas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int idusuario)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
    }
}