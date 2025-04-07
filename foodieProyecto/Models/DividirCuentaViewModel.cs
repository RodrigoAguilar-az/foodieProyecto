using static foodieProyecto.Controllers.PedidosController;
using System.ComponentModel.DataAnnotations;

namespace foodieProyecto.Models
{
    public class DividirCuentaViewModel
    {
        public PedidoLocal Pedido { get; set; }
        public List<ItemSeleccionadoViewModel> ItemsSeleccionados { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [Display(Name = "Nombre del Cliente")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "Seleccione un método de pago")]
        [Display(Name = "Método de Pago")]
        public int MetodoPagoId { get; set; }

        // Campos para pago con tarjeta
        [Display(Name = "Últimos 4 dígitos")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Debe ingresar exactamente 4 dígitos")]
        public string Digitos { get; set; }

        [Display(Name = "Titular")]
        public string Titular { get; set; }

        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        public decimal TotalParcial => ItemsSeleccionados?.Sum(i => i.Subtotal) ?? 0;
        public decimal IVA => TotalParcial * 0.13m;
        public decimal Total => TotalParcial + IVA;
    }
}
