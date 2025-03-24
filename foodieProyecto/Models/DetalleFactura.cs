using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class DetalleFactura
{
    public int DetalleFacturaId { get; set; }

    public int? FacturaId { get; set; }

    public int? DetallePedidoId { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual DetallePedido? DetallePedido { get; set; }

    public virtual Factura? Factura { get; set; }
}
