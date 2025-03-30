using foodieProyecto.Models.Vistas;
using foodieProyecto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace foodieProyecto.Controllers
{
    public class CocinaController : Controller
    {
        private readonly RestauranteContext _context;
        public CocinaController(RestauranteContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var lista = _context.DetallePedidos
            .Include(d => d.Encabezado)
            .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado")
            .ToList();


            var pedidos = lista.Select(d => new DetallePedidoViewModel
            {
                IdDetallePedido = d.IdDetallePedido,
                IdPedido = d.Encabezado.IdPedido,
                IdMesa = d.Encabezado.IdMesa,
                Hora = d.Encabezado.FechaApertura?.ToString("HH:mm") ?? "--:--",
                NombreItem = ObtenerNombreItem(d.TipoItem, d.ItemId),
                TipoItem = d.TipoItem,
                Comentarios = d.Comentarios ?? "",
                Estado = d.Estado
            }).ToList();

            return View(pedidos); // 👈 Esto va al Index.cshtml
        }


        public IActionResult ObtenerPedidos()
        {
            var detalles = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado.Estado != "Cerrado")
                .ToList(); // 👈 Esto trae los datos a memoria

            var pedidos = detalles.Select(d => new DetallePedidoViewModel
            {
                IdDetallePedido = d.IdDetallePedido,
                IdPedido = d.Encabezado.IdPedido,
                IdMesa = d.Encabezado.IdMesa,
                Hora = d.Encabezado.FechaApertura?.ToString("HH:mm") ?? "--:--", // ✅ Ahora sí puedes usar ?.
                NombreItem = ObtenerNombreItem(d.TipoItem, d.ItemId),
                TipoItem = d.TipoItem,
                Comentarios = d.Comentarios ?? "",
                Estado = d.Estado
            }).ToList();

            return PartialView("_ListadoPedidos", pedidos);
        }

        private string ObtenerNombreItem(string tipo, int id)
        {
            if (tipo == "Plato")
                return _context.Platos.FirstOrDefault(p => p.Id == id)?.Nombre ?? "(Plato eliminado)";
            if (tipo == "Combo")
                return _context.Combos.FirstOrDefault(c => c.Id == id)?.Nombre ?? "(Combo eliminado)";
            return "(Desconocido)";
        }
        [HttpPost]
        public IActionResult ActualizarEstado(int idDetalle)
        {
            var detalle = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .FirstOrDefault(d => d.IdDetallePedido == idDetalle);

            if (detalle == null || detalle.Encabezado == null)
                return NotFound();

            // Lógica basada en estado real actual
            switch (detalle.Estado)
            {
                case "Pendiente":
                    detalle.Estado = "En Proceso";
                    break;
                case "En Proceso":
                    detalle.Estado = "Finalizado";
                    break;
                default:
                    break; // No hace nada si ya está Finalizado
            }

            _context.SaveChanges();

            var viewModel = new DetallePedidoViewModel
            {
                IdPedido = detalle.Encabezado.IdPedido,
                IdMesa = detalle.Encabezado.IdMesa,
                Hora = detalle.Encabezado.FechaApertura?.ToString("HH:mm") ?? "--:--",
                NombreItem = ObtenerNombreItem(detalle.TipoItem, detalle.ItemId),
                TipoItem = detalle.TipoItem,
                Comentarios = detalle.Comentarios ?? "",
                Estado = detalle.Estado,
                IdDetallePedido = detalle.IdDetallePedido
            };

            return PartialView("_ItemPedido", viewModel);
        }



    }
}
