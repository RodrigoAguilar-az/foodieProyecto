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
            var pedidos = _context.PedidoLocals.Include(p => p.DetallePedidos).AsQueryable();

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
            }

            return View(await pedidos.ToListAsync());
        }


        public async Task<IActionResult> DetalleCuenta(int id)
        {
            var pedido = await _context.PedidoLocals
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound();

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
