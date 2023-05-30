using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppPeliculas.Models.ViewModels
{
    public class CategoriaAutorVM
    {

        public CategoriaAutor categoriaautor { get; set; }

        public IEnumerable<SelectListItem> CategoriaLista { get; set; }

        public IEnumerable<SelectListItem> AutorLista { get; set; }
    }
}
