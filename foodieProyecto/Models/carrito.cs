using System.ComponentModel.DataAnnotations;
using static Azure.Core.HttpHeader;

namespace foodieProyecto.Models
{
    public class carrito
    {
        [Key]
        public int venta_lineaId { get; set; }

        public int pedido_id { get; set; }

        public int? plato_id { get; set; }

        public int? combo_id { get; set; }

        public int cantidad { get; set; }

        public decimal total { get; set; }

        public int metodo_pago_id { get; set; }

        public string estado { get; set; } = "Pendiente";

        public DateTime fecha_venta { get; set; }

        public virtual Pedido_Online Pedido { get; set; } = null!;

        public virtual Plato? Platos { get; set; }

        public virtual Combo? Combos { get; set; }

        public virtual MetodoPago MetodoPago { get; set; } = null!;
    }
}
