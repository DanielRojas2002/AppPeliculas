using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Pelicula
{
    public int IdPelicula { get; set; }

    public int IdCategoria { get; set; }

    public int IdEstatusPelicula { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Imagen { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public TimeSpan Duracion { get; set; }

    public int? Stock { get; set; }

    public virtual ICollection<Almacen> Almacens { get; set; } = new List<Almacen>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual EstatusPelicula IdEstatusPeliculaNavigation { get; set; } = null!;

    public virtual ICollection<PeliculaAutor> PeliculaAutors { get; set; } = new List<PeliculaAutor>();
}
