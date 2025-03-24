using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class PagoTarjetum
{
    public int IdPagoTarjeta { get; set; }

    public int? PagoId { get; set; }

    public string? Tipo { get; set; }

    public string? Digitos { get; set; }

    public string? Titular { get; set; }

    public string? Referencia { get; set; }

    public virtual Pago? Pago { get; set; }
}
