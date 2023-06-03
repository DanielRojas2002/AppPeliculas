using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Controllers
{
    public class PeliculaController : Controller
    {

        private readonly DbpeliculasContext _context;

        public PeliculaController(DbpeliculasContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            var datos = _context.Peliculas.Include(m => m.IdCategoriaNavigation).Include(d => d.IdEstatusPeliculaNavigation);

            return View(datos.ToList());
        }

        public IActionResult Upsert(int? idpelicula)
        {
            PeliculaVM peliculas = new PeliculaVM()
            {
                pelicula = new Pelicula(),

                ListaCateogoria = _context.Categoria.Select(m => new SelectListItem
                {
                    Text = m.Descripcion,
                    Value = m.IdCategoria.ToString()
                }),

                ListaEstatus = _context.EstatusPeliculas.Select(a=> new SelectListItem
                {
                    Text=a.Descripcion.ToString(),
                    Value=a.IdEstatusPelicula.ToString()
                })

            };

            if (idpelicula == null)
            {
                //crear

                peliculas.pelicula.IdPelicula = 0;

                return View(peliculas);

            }
            else
            {
                //editar

                peliculas.pelicula = _context.Peliculas.Find(idpelicula);
                return View(peliculas);
            }

            
          
        }

        [HttpPost]
        public IActionResult Upsert(PeliculaVM modelo)
        {
            if (modelo.pelicula.IdPelicula==0)
            {
               


                Pelicula pelicula = new Pelicula()
                {
                    IdCategoria = modelo.pelicula.IdCategoria,
                    IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                    Titulo = modelo.pelicula.Titulo.ToUpper(),
                    Descripcion = modelo.pelicula.Descripcion.ToLower(),
                    Duracion = modelo.pelicula.Duracion,
                    FechaRegistro= DateTime.Now

                };

                _context.Peliculas.Add(pelicula);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                //editar

                Pelicula pelicula = new Pelicula()
                {
                    IdPelicula = modelo.pelicula.IdPelicula,
                    IdCategoria = modelo.pelicula.IdCategoria,
                    IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                    Titulo = modelo.pelicula.Titulo.ToUpper(),
                    Descripcion = modelo.pelicula.Descripcion.ToLower(),
                    Duracion = modelo.pelicula.Duracion,
                    FechaRegistro=modelo.pelicula.FechaRegistro
                    

                };

                _context.Peliculas.Update(pelicula);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
           
        }

        public IActionResult Eliminar (int? idpelicula)
        {
            PeliculaVM peliculas = new PeliculaVM()
            {
                pelicula = new Pelicula(),

                ListaCateogoria = _context.Categoria.Select(m => new SelectListItem
                {
                    Text = m.Descripcion,
                    Value = m.IdCategoria.ToString()
                }),

                ListaEstatus=_context.EstatusPeliculas.Select(s=> new SelectListItem
                {
                    Text=s.Descripcion,
                    Value=s.IdEstatusPelicula.ToString()
                })
            };

            peliculas.pelicula = _context.Peliculas.Find(idpelicula);

            return View(peliculas);
        }

        [HttpPost]
        public IActionResult Eliminar (PeliculaVM modelo)
        {
            try
            {
                Pelicula pelicula = new Pelicula()
                {
                    IdPelicula = modelo.pelicula.IdPelicula
                };

                _context.Peliculas.Remove(pelicula);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ocurrió un error al eliminar la película
                var errorMessage = "No se puede eliminar esta pelicula antes de eliminar todas las asignaciones de autores";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage
                };

                return View("Error", model);
            }


        }

        public IActionResult AsignarAutores(int idpelicula)
        {
            return RedirectToAction("Index", "AsignarAutores", new { idpelicula = idpelicula });

        }


    }
}
