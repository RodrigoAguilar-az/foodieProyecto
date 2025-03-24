using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Estado { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = new List<Combo>();

    public virtual ICollection<Plato> Platos { get; set; } = new List<Plato>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
