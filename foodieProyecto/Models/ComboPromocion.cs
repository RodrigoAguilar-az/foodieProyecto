using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class ComboPromocion
{
    public int Id { get; set; }

    public int? ComboId { get; set; }

    public int? PromocionId { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? Estado { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Promocione? Promocion { get; set; }
}
