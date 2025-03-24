using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class LoginCliente
{
    public int Loginid { get; set; }

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }
}
