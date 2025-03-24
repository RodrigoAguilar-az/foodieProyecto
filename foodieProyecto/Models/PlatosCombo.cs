using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class PlatosCombo
{
    public int Id { get; set; }

    public int? Estado { get; set; }

    public int? ComboId { get; set; }

    public int? PlatoId { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Plato? Plato { get; set; }
}
