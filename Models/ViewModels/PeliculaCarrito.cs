using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppPeliculas.Models.ViewModels
{
    public class PeliculaCarrito
    {
        public Pelicula pelicula { get; set; }

        public Usuario usuario { get; set; }

        public Carrito carrito { get; set; }
        public CarritoDetalle carritodetalle { get; set; }

        public List<Pelicula> ListaPelicula { get; set; }
    }
}
