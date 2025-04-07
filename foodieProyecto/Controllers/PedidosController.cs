using foodieProyecto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace foodieProyecto.Controllers
{
    public class PedidosController : Controller
    {
        private readonly RestauranteContext _context;

        public PedidosController(RestauranteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string estado, string orden, string busqueda)
        {
            var pedidos = _context.PedidoLocals
                .Include(p => p.DetallePedidos)
                .AsQueryable();

            // Filtrar por estado
            if (!string.IsNullOrEmpty(estado))
                pedidos = pedidos.Where(p => p.Estado == estado);

            // Buscar por mesa
            if (!string.IsNullOrWhiteSpace(busqueda))
                pedidos = pedidos.Where(p => p.IdMesa.ToString().Contains(busqueda));

            // Ordenar por mesa o estado
            switch (orden)
            {
                case "Por mesa":
                    pedidos = pedidos.OrderBy(p => p.IdMesa);
                    break;
                case "Por estado":
                    pedidos = pedidos.OrderBy(p => p.Estado);
                    break;
                default:
                    pedidos = pedidos.OrderBy(p => p.IdMesa);
                    break;
            }

            // Solo mostrar pedidos del día actual
            var hoy = DateTime.Today;
            pedidos = pedidos.Where(p => p.FechaApertura.HasValue &&
                p.FechaApertura.Value.Date == hoy);

            return View(await pedidos.ToListAsync());
        }

        public async Task<IActionResult> DetalleCuenta(int id)
        {
            var pedido = await _context.PedidoLocals
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound();

            // Cargar datos adicionales para cada detalle
            foreach (var detalle in pedido.DetallePedidos)
            {
                if (detalle.TipoItem == "Plato")
                {
                    detalle.ItemInfo = await _context.Platos
                        .Where(p => p.Id == detalle.ItemId)
                        .Select(p => new { Nombre = p.Nombre, Precio = p.Precio })
                        .FirstOrDefaultAsync();
                }
                else if (detalle.TipoItem == "Combo")
                {
                    // Obtener combo con promoción si existe
                    var combo = await _context.Combos
                        .Where(c => c.Id == detalle.ItemId)
                        .Select(c => new { Nombre = c.Nombre, Precio = c.Precio })
                        .FirstOrDefaultAsync();

                    var promocion = await _context.ComboPromocions
                        .Where(cp => cp.ComboId == detalle.ItemId && cp.Estado == 1 &&
                            DateTime.Now >= cp.FechaInicio && DateTime.Now <= cp.FechaFin)
                        .Join(_context.Promociones,
                            cp => cp.PromocionId,
                            pr => pr.Id,
                            (cp, pr) => new { Nombre = pr.Nombre, Descuento = pr.Descuento })
                        .FirstOrDefaultAsync();

                    detalle.ItemInfo = combo;
                    detalle.PromocionInfo = promocion;

                    // Recalcular el subtotal con descuento si hay promoción
                    if (promocion != null)
                    {
                        decimal descuentoTotal = (decimal)(promocion.Descuento * detalle.Cantidad);
                        detalle.SubtotalConDescuento = detalle.Subtotal - descuentoTotal > 0
                            ? detalle.Subtotal - descuentoTotal
                            : 0;
                    }
                }
            }

            return PartialView("DetalleCuenta", pedido);
        }


        [HttpGet]
        public async Task<IActionResult> DividirCuenta(int id)
        {
            var pedido = await _context.PedidoLocals
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound();

            return View("DividirCuenta", pedido); // asegúrate que esta vista exista
        }


    }
}
