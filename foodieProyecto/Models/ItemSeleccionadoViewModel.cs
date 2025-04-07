namespace foodieProyecto.Models
{
    public class ItemSeleccionadoViewModel
    {
        public int DetallePedidoId { get; set; }
        public string NombreItem { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadSeleccionada { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool TieneDescuento { get; set; }
        public decimal Subtotal => PrecioUnitario * CantidadSeleccionada;
    }
}
