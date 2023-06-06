using Microsoft.AspNetCore.Mvc;

namespace AppPeliculas.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
