using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Almacen
{
    public int IdAlmacen { get; set; }

    public int IdPelicula { get; set; }

    public int IdTipoEntrada { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int Cantidad { get; set; }

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Tipoentradum IdTipoEntradaNavigation { get; set; } = null!;
}
