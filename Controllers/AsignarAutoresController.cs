using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Controllers
{
    public class AsignarAutoresController : Controller
    {

        private readonly DbpeliculasContext _context;

        
        public AsignarAutoresController(DbpeliculasContext context)
        {
            _context = context;
        }

        public IActionResult Index(int idpelicula)
        {
           
            
            var datos = _context.PeliculaAutors.Include(m => m.IdPeliculaNavigation).Include(m => m.IdAutorNavigation).Where
                (m=>m.IdPelicula== idpelicula);
            return View(datos.ToList());

        }

        public IActionResult Upsert ( int? idpeliculaautor, int? idpelicula)
        {
            PeliculaAutorVM peliculaautor = new PeliculaAutorVM()
            {
                pelicula = new Pelicula(),
                peliculaautor= new PeliculaAutor(),
                ListaAutores = _context.Autors.Select(m => new SelectListItem
                {
                    Text = m.Nombre + " " + m.APaterno + " " + m.AMaterno,
                    Value = m.IdAutor.ToString()
                })
            }; 

            if (idpeliculaautor == null)
            {
                // crear asignar autor
              
                peliculaautor.peliculaautor.IdPeliculaAutor = 0;
                //peliculaautor.peliculaautor.IdPelicula = idpelicula;


                return View(peliculaautor);

            }
            else
            {
                // editar autor
                //peliculaautor.peliculaautor.IdPelicula = idpeli;
                peliculaautor.peliculaautor = _context.PeliculaAutors.Find(idpeliculaautor);
                return View(peliculaautor);
            }
          

          
        }


        [HttpPost]
        public IActionResult Upsert( PeliculaAutorVM modelo)
        {
            if (modelo.peliculaautor.IdPeliculaAutor==0)
            {
                // crear

                PeliculaAutor peliculaautor = new PeliculaAutor()
                {
                    IdAutor = modelo.autor.IdAutor,
                    IdPelicula = modelo.peliculaautor.IdPelicula
                   
                };

                _context.PeliculaAutors.Add(peliculaautor);
                _context.SaveChanges();
                return RedirectToAction("Index", "AsignarAutores", new { idpelicula = modelo.peliculaautor.IdPelicula });
            }
            else
            {
                // editar

                PeliculaAutor peliculaautor = new PeliculaAutor()
                {
                    IdPeliculaAutor=modelo.peliculaautor.IdPeliculaAutor,
                    IdPelicula = modelo.peliculaautor.IdPelicula,
                    IdAutor = modelo.autor.IdAutor
                };

                _context.PeliculaAutors.Update(peliculaautor);
                _context.SaveChanges();
                return RedirectToAction("Index", "AsignarAutores", new { idpelicula = modelo.peliculaautor.IdPelicula });
            }
            
        }

        public IActionResult Eliminar (int idpeliculaautor)
        {
            PeliculaAutorVM peliculaautor = new PeliculaAutorVM()
            {
                peliculaautor = new PeliculaAutor(),
                ListaAutores = _context.Autors.Select(m => new SelectListItem
                {
                    Text = m.Nombre + " " + m.APaterno + " " + m.AMaterno,
                    Value = m.IdAutor.ToString()
                })
            };


            peliculaautor.peliculaautor = _context.PeliculaAutors.Find(idpeliculaautor);

            return View(peliculaautor);
        }

        [HttpPost]
        public IActionResult Eliminar (PeliculaAutorVM modelo)
        {
            PeliculaAutor peliculaautor = new PeliculaAutor()
            {
                IdPeliculaAutor = modelo.peliculaautor.IdPeliculaAutor
            };


            _context.PeliculaAutors.Remove(peliculaautor);
            _context.SaveChanges();
             return RedirectToAction("Index", "AsignarAutores", new { idpelicula = modelo.peliculaautor.IdPelicula });

        }
    }
}
