using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class CarritoDetalle
{
    public int IdCarritoDetalle { get; set; }

    public int IdCarrito { get; set; }

    public int IdPelicula { get; set; }

    public int Stock { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
