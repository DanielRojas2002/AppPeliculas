using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;

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
        public  IActionResult Upsert(CategoriaAutorVM modelo)
        {
            if (modelo.categoriaautor.IdCategoriaAutor == 0)
            {

                // crear 

              

                var coincidencia = from CategoriaAutor in _context.CategoriaAutors
                                   where CategoriaAutor.IdAutor == modelo.categoriaautor.IdAutor
                                   & CategoriaAutor.IdCategoria == modelo.categoriaautor.IdCategoria

                                   select CategoriaAutor.IdAutor;


                int validacion = 0;
                foreach (var persona in coincidencia)
                {
                    validacion = 1;
                }



                if (validacion==0)
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
                else
                {
                    TempData["error102"] = "Ya cuenta con esta Asignacion";
                    return View(modelo);
                }
              
              
            }
            else
            {
                // editar

                var coincidencia = _context.CategoriaAutors.Find(from CategoriaAutor in _context.CategoriaAutors
                                                                 where CategoriaAutor.IdAutor == modelo.categoriaautor.IdAutor 
                                                                 & CategoriaAutor.IdCategoria== modelo.categoriaautor.IdCategoria
                                                                 select CategoriaAutor.IdAutor);

            // no existe
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
        }
    }
}
