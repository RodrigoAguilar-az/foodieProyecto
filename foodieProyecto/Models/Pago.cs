using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int? FacturaId { get; set; }

    public int? MetodoPagoId { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Factura? Factura { get; set; }

    public virtual MetodoPago? MetodoPago { get; set; }

    public virtual ICollection<PagoTarjetum> PagoTarjeta { get; set; } = new List<PagoTarjetum>();
}
