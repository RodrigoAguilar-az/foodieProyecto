using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foodieProyecto.Models;

public partial class DetallePedido
{
    [Key]
    public int IdDetallePedido { get; set; }

    public int EncabezadoId { get; set; }

    public string? TipoVenta { get; set; }

    public string TipoItem { get; set; } = null!;

    public int ItemId { get; set; }

    public int Cantidad { get; set; }

    public string? Comentarios { get; set; }

    public string Estado { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual PedidoLocal? Encabezado { get; set; } = null!;

    // Propiedades no mapeadas para almacenar información adicional
    [NotMapped]
    public dynamic ItemInfo { get; set; }

    [NotMapped]
    public dynamic PromocionInfo { get; set; }

    [NotMapped]
    public decimal SubtotalConDescuento { get; set; }
}
