using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Mesa
{
    public int Id { get; set; }

    public int? Numero { get; set; }

    public int? Capacidad { get; set; }

    public string? Disponibilidad { get; set; }

    public int? Estado { get; set; }
}
