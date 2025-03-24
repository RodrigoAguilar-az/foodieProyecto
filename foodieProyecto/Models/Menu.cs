using System;
using System.Collections.Generic;

namespace foodieProyecto.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string? TipoMenu { get; set; }

    public string? TipoVenta { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public int? Estado { get; set; }

    public virtual ICollection<MenuCombo> MenuCombos { get; set; } = new List<MenuCombo>();

    public virtual ICollection<MenuPlato> MenuPlatos { get; set; } = new List<MenuPlato>();
}
