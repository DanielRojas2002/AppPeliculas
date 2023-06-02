using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppPeliculas.Models.ViewModels
{
    public class PeliculaAutorVM
    {
        public Pelicula pelicula { get; set; }

        public PeliculaAutor peliculaautor { get; set; }
        public Autor autor { get; set; }

      

        public IEnumerable<SelectListItem> ListaAutores { get; set; }
    }
}
