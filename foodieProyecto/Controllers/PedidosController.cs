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

            // Filtro por estado
            if (!string.IsNullOrEmpty(estado))
            {
                pedidos = pedidos.Where(p => p.Estado == estado);
            }

            // Búsqueda por número de mesa
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                pedidos = pedidos.Where(p => p.IdMesa.ToString().Contains(busqueda));
            }

            // Ordenar según la selección
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
    }
}
