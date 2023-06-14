using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Estatuscarrito
{
    public int IdEstatus { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
}
