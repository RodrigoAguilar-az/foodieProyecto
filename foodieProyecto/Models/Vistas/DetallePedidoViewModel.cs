namespace foodieProyecto.Models.Vistas
{
    public class DetallePedidoViewModel
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public int IdMesa { get; set; }
        public string Hora { get; set; } = string.Empty;

        public string NombreItem { get; set; } = string.Empty;
        public string TipoItem { get; set; } = string.Empty; // "S", "P", etc
        public string Comentarios { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
