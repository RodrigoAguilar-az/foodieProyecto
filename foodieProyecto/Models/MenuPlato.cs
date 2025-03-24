using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class MenuPlato
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public int? PlatoId { get; set; }

    public int? Estado { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Plato? Plato { get; set; }
}
