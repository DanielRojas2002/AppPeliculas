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

            int cantidadentrada = _context.Almacens
            .Where(almacen => almacen.IdPelicula == idpelicula && almacen.IdTipoEntrada == 1)
            .Sum(almacen => almacen.Cantidad);

            int cantidadsalida = _context.Almacens
           .Where(almacen => almacen.IdPelicula == idpelicula && almacen.IdTipoEntrada==2)
           .Sum(almacen => almacen.Cantidad);

            int total = cantidadentrada - cantidadsalida;

            TempData["idpelicula"] = idpelicula;
            TempData["cantidadentrada"] = cantidadentrada;
            TempData["cantidadsalida"] = cantidadsalida;
            TempData["cantidadstock"] = total;
            return View(datos.ToList());
        }

        public IActionResult AgregarStock(int idpelicula)
        {
            // solo se va a poder agregar y eliminar no modificar
            Almacen almacen = new Almacen()
            {
                IdPelicula= idpelicula
            };
            return View(almacen);
        }

        [HttpPost]
        public IActionResult AgregarStock(Almacen modelo)
        {
            Almacen al = new Almacen()
            {
                IdPelicula = modelo.IdPelicula,
                IdTipoEntrada = 1,
                Cantidad = modelo.Cantidad,
                FechaRegistro = DateTime.Now
            };

            var stockactual = (from peli in _context.Peliculas
                               where peli.IdPelicula == modelo.IdPelicula
                               select peli.Stock).SingleOrDefault();






            if (modelo.Cantidad > 0)
            {
                _context.Almacens.Add(al);
                

                Pelicula pelicula = new Pelicula()
                {
                    IdPelicula = modelo.IdPelicula,
                    Stock = modelo.Cantidad + stockactual
                };

                _context.Entry(pelicula).Property(x => x.Stock).IsModified = true;

              

                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { idpelicula = modelo.IdPelicula });
            }
            else
            {
                var errorMessage = "Debe de agregar al menos 1 de stock";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Almacen",
                    parametro = modelo.IdPelicula

                };

                return View("Error", model);
            }
           
              
           
            
          
        }

        public IActionResult VerDetalle(int idalmacen,int idpelicula)
        {
           

            var objeto = _context.Almacens.Include(s => s.IdTipoEntradaNavigation).Where(m => m.IdAlmacen == idalmacen);
            TempData["idpelicula"] = idpelicula;
            return View(objeto.ToList());
        }

        public IActionResult Regresar(int idpelicula)
        {
            return RedirectToAction("Index", "Almacen", new { idpelicula = idpelicula });

        }
    }
}
