using System;
using System.Collections.Generic;

namespace AppPeliculas.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public string Nombre { get; set; } = null!;

    public string APaterno { get; set; } = null!;

    public string AMaterno { get; set; } = null!;

    public virtual ICollection<CategoriaAutor> CategoriaAutors { get; set; } = new List<CategoriaAutor>();
}
