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
            .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && (d.Estado != "Entregado" && d.Estado != "Cancelado"))
            .ToList();

            var listaCocina = _context.DetallePedidos
            .Include(d => d.Encabezado)
            .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Local" && (d.Estado != "Entregado" && d.Estado != "Cancelado"))
            .ToList();

            var listaOnline = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Online" && (d.Estado != "Entregado" && d.Estado != "Cancelado"))
                .ToList();

            ViewBag.CocinaCount = listaCocina.Count;
            ViewBag.OnlineCount = listaOnline.Count;

            var pedidos = lista.OrderByDescending(d=> d.IdDetallePedido).Select(d => new DetallePedidoViewModel
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

            return View(pedidos);
        }

        public IActionResult ObtenerPedidosEditados(string searchQuery, string sortOrder)
        {
            // Iniciar la consulta con los filtros base
            var lista = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado.Estado != "Cerrado" && d.Estado != "Entregado" && d.Estado != "Cancelado");

            // Filtrar por número de pedido si se especifica el query de búsqueda
            if (!string.IsNullOrEmpty(searchQuery))
            {
                lista = lista.Where(d => d.Encabezado.IdPedido.ToString().Contains(searchQuery));
            }

            // Ordenar según la opción seleccionada
            switch (sortOrder)
            {
                case "antiguo":
                    lista = lista.OrderBy(d => d.Encabezado.FechaApertura);  // Más antiguo
                    break;
                case "mesa":
                    lista = lista.OrderBy(d => d.Encabezado.IdMesa);  // Por mesa
                    break;
                default:
                    lista = lista.OrderByDescending(d => d.Encabezado.FechaApertura);  // Más reciente (por defecto)
                    break;
            }

            // Cargar los datos en memoria y luego proyectarlos
            var pedidos = lista.ToList().Select(d => new DetallePedidoViewModel
            {
                IdDetallePedido = d.IdDetallePedido,
                IdPedido = d.Encabezado.IdPedido,
                IdMesa = d.Encabezado.IdMesa,
                Hora = d.Encabezado.FechaApertura != null ? d.Encabezado.FechaApertura.Value.ToString("HH:mm") : "--:--",
                NombreItem = ObtenerNombreItem(d.TipoItem, d.ItemId), // Proyección después de cargar en memoria
                TipoItem = d.TipoItem,
                Comentarios = d.Comentarios ?? "",
                Estado = d.Estado
            }).ToList();

            return PartialView("_ListadoPedidos", pedidos);
        }


        public IActionResult ObtenerPedidos()
        {
            var detalles = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado.Estado != "Cerrado")
                .ToList();

            var pedidos = detalles.Select(d => new DetallePedidoViewModel
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

            return PartialView("_ListadoPedidos", pedidos);
        }

        // Cambiar a un método estático
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

            switch (detalle.Estado)
            {
                case "Pendiente":
                    detalle.Estado = "En Proceso";
                    break;
                case "En Proceso":
                    detalle.Estado = "Finalizado";
                    break;
                default:
                    break;
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
