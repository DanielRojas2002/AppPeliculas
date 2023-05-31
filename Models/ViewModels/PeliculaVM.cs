using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppPeliculas.Models.ViewModels
{
    public class PeliculaVM
    {
        public Pelicula pelicula { get; set; }

        public IEnumerable<SelectListItem> ListaCateogoria  { get; set; }

        public IEnumerable<SelectListItem> ListaEstatus { get; set; }
    }
}
