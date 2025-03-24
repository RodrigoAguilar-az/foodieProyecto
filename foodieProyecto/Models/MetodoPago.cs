using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class MetodoPago
{
    public int MetodoPagoId { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
