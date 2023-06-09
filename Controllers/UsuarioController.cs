using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly DbpeliculasContext _context;

        public UsuarioController(DbpeliculasContext context)
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
                               where usu.Correo == modelo.Correo  && usu.Contrasena == modelo.Contrasena && usu.IdTipoUsuario == 1
                             select usu.IdUsuario).SingleOrDefault();

            if (idusuario>0)
            {
                return RedirectToAction(nameof(Menu),new {idusuario=idusuario});
            }
            else
            {
                var errorMessage = "Credenciales Incorrectas";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Usuario"

                };

                return View("Error", model);
            }
            
        }

        public IActionResult Menu(int idusuario)
        {
            TempData["idusuario"] = idusuario;
            var peliculas = _context.Peliculas
            .Include(m => m.IdCategoriaNavigation)
            .Where(a => a.Stock > 0 && a.IdEstatusPelicula == 1)
            .ToList()
            .Select((pelicula, indice) => new { Pelicula = pelicula, Indice = indice })
            .GroupBy(x => x.Indice / 3)
            .Select(g => g.Select(x => x.Pelicula).ToList()) // Convertir IEnumerable en List
            .ToList();

            return View(peliculas);
        }
    }
}
