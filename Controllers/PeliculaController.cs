using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System;

namespace AppPeliculas.Controllers
{
    public class PeliculaController : Controller
    {

        private readonly DbpeliculasContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public PeliculaController(DbpeliculasContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
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
        public async  Task<IActionResult> Upsert(PeliculaVM modelo)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnviroment.WebRootPath; // acceso a la ruta

            if (modelo.pelicula.IdPelicula == 0)
            {

               

                try
                {




                    string upload = webRootPath + WcRuta.ImgRuta;
                    string fileName = Guid.NewGuid().ToString();// guardo el nomre de la imagen en filename
                    string extension = Path.GetExtension(files[0].FileName); // accedo a la extension de la imagen 

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }



                    Pelicula pelicula = new Pelicula()
                    {
                        IdCategoria = modelo.pelicula.IdCategoria,
                        IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                        Titulo = modelo.pelicula.Titulo.ToUpper(),
                        Descripcion = modelo.pelicula.Descripcion.ToLower(),
                        Duracion = modelo.pelicula.Duracion,
                        FechaRegistro = DateTime.Now,
                        Stock = 0,
                        Imagen = fileName + extension
                    };

                    _context.Peliculas.Add(pelicula);


                }
                catch
                {
                    Pelicula pelicula = new Pelicula()
                    {
                        IdCategoria = modelo.pelicula.IdCategoria,
                        IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                        Titulo = modelo.pelicula.Titulo.ToUpper(),
                        Descripcion = modelo.pelicula.Descripcion.ToLower(),
                        Duracion = modelo.pelicula.Duracion,
                        FechaRegistro = DateTime.Now,
                        Stock = 0

                    };
                    _context.Peliculas.Add(pelicula);


                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));





            }
            else
            {
                //editar

                // verifica si esta intenado agregar una nueva imagen
                if (files.Count > 0)
                {
                    string upload = webRootPath + WcRuta.ImgRuta;
                    string fileName = Guid.NewGuid().ToString();// aguardo el nombre de la imagen
                    string extension = Path.GetExtension(files[0].FileName); // guardo su extension

                    // borrar img anterior
                    var objproducto = _context.Peliculas.AsNoTracking().FirstOrDefault(p => p.IdPelicula == modelo.pelicula.IdPelicula);

                    var anteriorfile = Path.Combine(upload, objproducto.Imagen);
                    if (System.IO.File.Exists(anteriorfile))
                    {
                        System.IO.File.Delete(anteriorfile);
                    }
                    // imagen anterior borrada

                    // guardo la nueva imagen
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    Pelicula pelicula = new Pelicula()
                    {
                        IdPelicula = modelo.pelicula.IdPelicula,
                        IdCategoria = modelo.pelicula.IdCategoria,
                        IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                        Titulo = modelo.pelicula.Titulo.ToUpper(),
                        Descripcion = modelo.pelicula.Descripcion.ToLower(),
                        Duracion = modelo.pelicula.Duracion,
                        FechaRegistro = modelo.pelicula.FechaRegistro,
                        Imagen = fileName + extension


                    };

                    _context.Peliculas.Update(pelicula);
                }
                else
                {
                    Pelicula pelicula = new Pelicula()
                    {
                        IdPelicula = modelo.pelicula.IdPelicula,
                        IdCategoria = modelo.pelicula.IdCategoria,
                        IdEstatusPelicula = modelo.pelicula.IdEstatusPelicula,
                        Titulo = modelo.pelicula.Titulo.ToUpper(),
                        Descripcion = modelo.pelicula.Descripcion.ToLower(),
                        Duracion = modelo.pelicula.Duracion,
                        FechaRegistro = modelo.pelicula.FechaRegistro


                    };
                    _context.Peliculas.Update(pelicula);
                }

               

               
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

                string upload = _webHostEnviroment.WebRootPath + WcRuta.ImgRuta;

                var objproducto = _context.Peliculas.AsNoTracking().FirstOrDefault(p => p.IdPelicula == modelo.pelicula.IdPelicula);
                var anteriorfile = Path.Combine(upload, objproducto.Imagen);
                if (System.IO.File.Exists(anteriorfile))
                {
                    System.IO.File.Delete(anteriorfile);
                }

                var almacenesAEliminar = _context.Almacens.Where(a => a.IdPelicula == modelo.pelicula.IdPelicula);
                _context.Almacens.RemoveRange(almacenesAEliminar);

                _context.Peliculas.Remove(pelicula);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ocurrió un error al eliminar la película
                var errorMessage = "No se puede eliminar esta pelicula antes de eliminar todas las asignaciones ";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Pelicula"


                };

                return View("Error", model);
            }


        }

        public IActionResult AsignarAutores(int idpelicula)
        {
            return RedirectToAction("Index", "AsignarAutores", new { idpelicula = idpelicula });

        }

        public IActionResult AgregarStock(int idpelicula)
        {
            return RedirectToAction("Index", "Almacen", new { idpelicula = idpelicula });
        }


    }
}
