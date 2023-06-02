using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdMarca { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;
}
