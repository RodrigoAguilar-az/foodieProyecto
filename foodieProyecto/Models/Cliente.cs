using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public int? Loginid { get; set; }

    public virtual LoginCliente? Login { get; set; }
}
