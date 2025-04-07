using System.ComponentModel.DataAnnotations;

namespace foodieProyecto.Models
{
    public class Pedido_Online
    {
        [Key]
        public int id_pedido { get; set; }

        public int id_cliente { get; set; }

        public DateTime fecha_pedido { get; set; }

        public string estado { get; set; } = "Pendiente";

        public virtual Cliente Cliente { get; set; } = null!;

        public virtual ICollection<carrito> Carritos { get; set; } = new List<carrito>();

        public virtual ICollection<historial_pedido> Historiales { get; set; } = new List<historial_pedido>();
    }
}
