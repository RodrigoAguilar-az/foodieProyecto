using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class MenuCombo
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public int? ComboId { get; set; }

    public int? Estado { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Menu? Menu { get; set; }
}
