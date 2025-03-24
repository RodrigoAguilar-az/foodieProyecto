using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Promocione
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Descuento { get; set; }

    public int? Estado { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categorium? Categoria { get; set; }

    public virtual ICollection<ComboPromocion> ComboPromocions { get; set; } = new List<ComboPromocion>();
}
