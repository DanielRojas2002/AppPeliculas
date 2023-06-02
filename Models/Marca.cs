using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
