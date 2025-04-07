using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foodieProyecto.Models;

public partial class Login_Cliente
{
    [Key]
    public int loginid { get; set; }

    public string correo { get; set; } = null!;

    public string contraseña { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }
}
