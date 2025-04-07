using System.ComponentModel.DataAnnotations;

namespace foodieProyecto.Models
{
    public class historial_pedido
    {
        [Key]
        public int historial_id { get; set; }

        public string tipoItem { get; set; } = null!;

        public int itemId { get; set; }

        public int cantidad { get; set; }

        public string estado { get; set; } = "Pendiente";

        public DateTime fecha_venta { get; set; }

        public virtual Pedido_Online Pedido { get; set; } = null!;
    }
}
