using System.ComponentModel.DataAnnotations;

namespace foodieProyecto.Models
{
    public class ventas_en_linea
    {
        [Key]
        public int ventas_linea_id { get; set; }

        public int menu_id { get; set; }

        public int cliente_id { get; set; }

        public int carrito_id { get; set; }

        public int pedido__id { get; set; }

        public int historial_id { get; set; }
    }
}
