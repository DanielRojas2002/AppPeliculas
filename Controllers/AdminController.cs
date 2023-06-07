using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppPeliculas.Controllers
{
    public class AdminController : Controller
    {

        private readonly DbpeliculasContext _context;

        public AdminController(DbpeliculasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario modelo)
        {


            int idusuario = (from usu in _context.Usuarios
                             where usu.Correo == modelo.Correo && usu.Contrasena == modelo.Contrasena && usu.IdTipoUsuario==2
                             select usu.IdUsuario).SingleOrDefault();

            if (idusuario > 0)
            {
                return RedirectToAction("Index", "Home", new { idusuario = idusuario }); 
            }
            else
            {
                var errorMessage = "Credenciales Incorrectas";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Admin"

                };

                return View("Error", model);
            }

        }
    }
}
