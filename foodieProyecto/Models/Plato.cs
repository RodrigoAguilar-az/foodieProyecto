using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Plato
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public string? Imagen { get; set; }

    public int? CategoriaId { get; set; }

    public int? Estado { get; set; }

    public virtual Categorium? Categoria { get; set; }

    public virtual ICollection<MenuPlato> MenuPlatos { get; set; } = new List<MenuPlato>();

    public virtual ICollection<PlatosCombo> PlatosCombos { get; set; } = new List<PlatosCombo>();
}
