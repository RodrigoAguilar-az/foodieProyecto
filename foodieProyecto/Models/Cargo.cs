using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Cargo
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Estado { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
