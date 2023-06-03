using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class PeliculaAutor
{
    public int IdPeliculaAutor { get; set; }

    public int IdPelicula { get; set; }

    public int IdCategoriaAutor { get; set; }

    public virtual CategoriaAutor IdCategoriaAutorNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
