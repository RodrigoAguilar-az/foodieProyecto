using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public string? ClienteNombre { get; set; }

    public string? CodigoFactura { get; set; }

    public int? IdPedido { get; set; }

    public string? TipoVenta { get; set; }

    public int? EmpleadoId { get; set; }

    public decimal? Total { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Empleado? Empleado { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
