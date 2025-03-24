using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Combo
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? CategoriaId { get; set; }

    public int? Estado { get; set; }

    public virtual Categorium? Categoria { get; set; }

    public virtual ICollection<ComboPromocion> ComboPromocions { get; set; } = new List<ComboPromocion>();

    public virtual ICollection<MenuCombo> MenuCombos { get; set; } = new List<MenuCombo>();

    public virtual ICollection<PlatosCombo> PlatosCombos { get; set; } = new List<PlatosCombo>();
}
