using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class PedidoLocal
{
    public int IdPedido { get; set; }

    public int IdMesa { get; set; }

    public string NombreCliente { get; set; } = null!;

    public DateTime? FechaApertura { get; set; }

    public string Estado { get; set; } = null!;

    public int IdMesero { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
