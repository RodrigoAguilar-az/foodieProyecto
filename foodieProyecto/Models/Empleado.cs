using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Empleado
{
    public int Id { get; set; }

    public int? Codigo { get; set; }

    public string? Contrasena { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public int? CargoId { get; set; }

    public int? Estado { get; set; }

    public virtual Cargo? Cargo { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
