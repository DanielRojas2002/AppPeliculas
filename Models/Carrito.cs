using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaPedido { get; set; }

    public virtual ICollection<CarritoDetalle> CarritoDetalles { get; set; } = new List<CarritoDetalle>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
