using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Controllers
{
    public class AlmacenController : Controller
    {


        private readonly DbpeliculasContext _context;

        public  AlmacenController(DbpeliculasContext context)
        {
            _context=context;
        }

        public IActionResult Index(int idpelicula)
        {
            var datos=_context.Almacens.Include(m=>m.IdPeliculaNavigation).Include(m=>m.IdTipoEntradaNavigation).Where(p=>p.IdPelicula==idpelicula);
            TempData["idpelicula"] = idpelicula;
            return View(datos.ToList());
        }

        public IActionResult AgregarStock(int idpelicula)
        {
            // solo se va a poder agregar y eliminar no modificar
            return View();
        }
    }
}
