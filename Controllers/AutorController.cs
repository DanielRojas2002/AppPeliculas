using AppPeliculas.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppPeliculas.Controllers
{
    public class AutorController : Controller
    {
        private readonly DbpeliculasContext _context;

        public AutorController(DbpeliculasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Autors.ToList());
        }

        public IActionResult Upsert(int? idautor)
        {

            Autor autor = new Autor();
            if (idautor == null)
            {
                // crear
                autor.IdAutor = 0;
                return View(autor);
            }
            else
            {
                // editar
                autor = _context.Autors.Find(idautor);
                return View(autor);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(Autor modelo)
        {
            if (modelo.IdAutor == 0)
            {
                // crear

                if (ModelState.IsValid)
                {
                    Autor autor = new Autor()
                    {
                        Nombre = modelo.Nombre.ToUpper(),
                        APaterno = modelo.APaterno.ToUpper(),
                        AMaterno = modelo.AMaterno.ToUpper()

                    };

                    _context.Autors.Add(autor);
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
                // editar
                Autor autor = new Autor()
                {
                    IdAutor=modelo.IdAutor,
                    Nombre = modelo.Nombre.ToUpper(),
                    APaterno = modelo.APaterno.ToUpper(),
                    AMaterno = modelo.AMaterno.ToUpper()

                };
                _context.Autors.Update(autor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
          
        }

        public IActionResult Eliminar(int? idautor)
        {
            return View(_context.Autors.Find(idautor));
        }



        [HttpPost]
        public IActionResult Eliminar (Autor modelo)
        {
            try
            {
                _context.Autors.Remove(modelo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                // Ocurrió un error al eliminar la película
                var errorMessage = "No se puede eliminar el autor ya que tiene asignado una pelicula o tiene asignado una union Categoria Autor";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Autor"

                };

                return View("Error", model);
            }



        }
    }
}
