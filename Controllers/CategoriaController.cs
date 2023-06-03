using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;
using AppPeliculas.Models;
using System.Linq.Expressions;

namespace AppPeliculas.Controllers
{
    public class CategoriaController : Controller
    {


        private readonly DbpeliculasContext _context;

        public CategoriaController(DbpeliculasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categoria.ToList());
        }


        public IActionResult Upsert(int? idcategoria)
        {
           
            Categorium categoria = new Categorium();
            if (idcategoria == null)
            {
                categoria.IdCategoria = 0;
               
                return View(categoria);
            }
            else
            {
                categoria = _context.Categoria.Find(idcategoria);
               
                return View(categoria);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Categorium modelo)
        {
            if (modelo.IdCategoria == 0)
            {
                if (ModelState.IsValid)
                {
                    Categorium categoria = new Categorium()
                    {
                        Descripcion = modelo.Descripcion.ToUpper()
                    };
                    _context.Categoria.Add(categoria);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(modelo);

                }
            }
            else
            {
               

                Categorium categoria = new Categorium()
                {
                    IdCategoria=modelo.IdCategoria,
                    Descripcion = modelo.Descripcion.ToUpper()
                };

                _context.Categoria.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


        }

        public IActionResult Editar(int idcategoria)
        {
            
            return View(_context.Categoria.Find(idcategoria));
        }

        public IActionResult Eliminar(int? idcategoria)
        {

            return View(_context.Categoria.Find(idcategoria));
        }

        [HttpPost]
        public IActionResult Eliminar(Categorium modelo)
        {
            try
            {
                _context.Categoria.Remove(modelo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Ocurrió un error al eliminar la película
                var errorMessage = "No se puede eliminar la categoria ya que tiene asignado una pelicula o tiene asignado una union Categoria Autor";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Categoria"

                };

                return View("Error", model);
            }

        
        }
    }
}
