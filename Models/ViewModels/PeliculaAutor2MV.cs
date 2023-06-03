namespace AppPeliculas.Models.ViewModels
{
    public class PeliculaAutor2MV
    {
        public PeliculaAutor PeliculaAutores { get; set; }
        public Pelicula IdPeliculaNavigation { get; set; }
        public Autor IdAutorNavigation { get; set; }

        public List<PeliculaAutor> ListaPeliculaAutor { get; set; }
    }
}
