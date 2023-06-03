using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AppPeliculas.Controllers
{
    public class CategoriaAutorController : Controller
    {

        private readonly DbpeliculasContext _context;

        public CategoriaAutorController (DbpeliculasContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            // include debes de tener antes un modelo que junta las tablas 
            var datos = _context.CategoriaAutors.Include(m => m.IdCategoriaNavigation).Include(m => m.IdAutorNavigation);


         


            //var query_join4 = from tablaunion in  _context.CategoriaAutors
            //                  join Categoria in _context.Categoria
            //                  on tablaunion.IdCategoria equals Categoria.IdCategoria
            //                  join Autor in _context.Autors
            //                  on tablaunion.IdAutor equals Autor.IdAutor
            //                  select new CategoriaAutorVM
            //                  {
            //                      idcategoria = tablaunion.IdCategoria,
            //                      descripcioncategoria = Categoria.Descripcion,
            //                      idautor = tablaunion.IdAutor,
            //                      nombrecompleto = Autor.Nombre + " " + Autor.APaterno + " " + Autor.AMaterno,


            //                  };

            return View(datos.ToList());
        }

        public IActionResult Upsert(int? idautorcategoria)
        {
            CategoriaAutorVM categoriaautorvm = new CategoriaAutorVM()
            {
                categoriaautor = new CategoriaAutor(),

                CategoriaLista = _context.Categoria.Select(i => new SelectListItem
                {
                    Text = i.Descripcion,
                    Value = i.IdCategoria.ToString()
                }),

                AutorLista= _context.Autors.Select(a => new SelectListItem
                {
                    Text=a.Nombre+" " + a.APaterno +" "+ a.AMaterno,
                    Value=a.IdAutor.ToString()
                })

            };



            if (idautorcategoria==null)
            {
                // crear

                categoriaautorvm.categoriaautor.IdCategoriaAutor = 0;

            
                return View(categoriaautorvm);

            }
            else
            {
                categoriaautorvm.categoriaautor = _context.CategoriaAutors.Find(idautorcategoria);
                return View(categoriaautorvm);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(CategoriaAutorVM modelo)
        {
            if (modelo.categoriaautor.IdCategoriaAutor == 0)
            {

                // crear 

                try
                {
                    CategoriaAutor ca = new CategoriaAutor()
                    {
                        IdCategoriaAutor = modelo.categoriaautor.IdCategoriaAutor,
                        IdAutor = modelo.categoriaautor.IdAutor,
                        IdCategoria = modelo.categoriaautor.IdCategoria
                    };

                    _context.CategoriaAutors.Add(ca);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    //string nombrecompleto = modelo.categoriaautor.IdAutorNavigation.Nombre + " " + modelo.categoriaautor.IdAutorNavigation.APaterno + " " + modelo.categoriaautor.IdAutorNavigation.AMaterno;
                    //string descripcioncategoria= modelo.categoriaautor.IdCategoriaNavigation.Descripcion; 
                    TempData["error102"] = "Ya cuenta con esta Asingacion:" ;
                    return View(modelo);
                }
               
              
              
              
            }
            else
            {
                // editar
                try
                {
                    CategoriaAutor ca = new CategoriaAutor()
                    {
                        IdCategoriaAutor = modelo.categoriaautor.IdCategoriaAutor,
                        IdAutor = modelo.categoriaautor.IdAutor,
                        IdCategoria = modelo.categoriaautor.IdCategoria
                    };

                    _context.CategoriaAutors.Update(ca);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["error102"] = "Ya cuenta con esta Asingacion:";
                    return View(modelo);
                }
              


            }
        }

        public IActionResult Eliminar(int idautorcategoria)
        {


            // var datos = _context.CategoriaAutors.Where(f=>f.IdCategoriaAutor==idautorcategoria).Include(m => m.IdCategoriaNavigation).Include(m => m.IdAutorNavigation);
            CategoriaAutorVM categoriaautorvm = new CategoriaAutorVM()
            {
                categoriaautor = new CategoriaAutor(),

                CategoriaLista = _context.Categoria.Select(i => new SelectListItem
                {
                    Text = i.Descripcion,
                    Value = i.IdCategoria.ToString()
                }),

                AutorLista = _context.Autors.Select(a => new SelectListItem
                {
                    Text = a.Nombre + " " + a.APaterno + " " + a.AMaterno,
                    Value = a.IdAutor.ToString()
                })

            };

            categoriaautorvm.categoriaautor = _context.CategoriaAutors.Find(idautorcategoria);
            return View(categoriaautorvm);



            //return View(_context.CategoriaAutors.Find(idautorcategoria));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(CategoriaAutorVM modelo)
        {
            try
            {
                CategoriaAutor categoriautor = new CategoriaAutor()
                {
                    IdCategoriaAutor = modelo.categoriaautor.IdCategoriaAutor

                };

                _context.CategoriaAutors.Remove(categoriautor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ocurrió un error al eliminar la película
                var errorMessage = "No se puede eliminar esta asignacion ya que esta siendo utilizada";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "CategoriaAutor"
                  


                };

                return View("Error", model);
            }
           

          


        }
    }
}
