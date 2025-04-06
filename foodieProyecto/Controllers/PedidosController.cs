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

        [HttpPost]
        public async Task<IActionResult> ProcesarPago(
    int IdPedido,
    decimal Monto,
    int MetodoPagoId,
    string? TipoTarjeta,
    string? Digitos,
    string? Titular,
    string? Referencia)
        {
            var factura = await _context.Facturas
                .Include(f => f.Pagos)
                .FirstOrDefaultAsync(f => f.IdPedido == IdPedido);

            if (factura == null)
            {
                var pedido = await _context.PedidoLocals
                    .Include(p => p.DetallePedidos)
                    .FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

                if (pedido == null) return NotFound();

                var subtotal = pedido.DetallePedidos.Sum(d => d.Subtotal);
                var iva = subtotal * 0.13m;
                var total = subtotal + iva;

                factura = new Factura
                {
                    IdPedido = IdPedido,
                    Fecha = DateTime.Now,
                    Total = total
                };

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();
            }

            var totalPagado = factura.Pagos.Sum(p => p.Monto ?? 0);
            if (totalPagado >= factura.Total)
            {
                TempData["Error"] = "La factura ya está completamente pagada.";
                return RedirectToAction("DetalleCuenta", new { id = IdPedido });
            }

            var pago = new Pago
            {
                FacturaId = factura.FacturaId,
                MetodoPagoId = MetodoPagoId,
                Monto = Monto,
                Fecha = DateTime.Now
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            if ((MetodoPagoId == 1 || MetodoPagoId == 2) && !string.IsNullOrEmpty(Digitos))
            {
                var pagoTarjeta = new PagoTarjetum
                {
                    PagoId = pago.PagoId,
                    Tipo = TipoTarjeta,
                    Digitos = Digitos,
                    Titular = Titular,
                    Referencia = Referencia
                };

                _context.PagoTarjeta.Add(pagoTarjeta);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Pago registrado correctamente.";
            return RedirectToAction("DetalleCuenta", new { id = IdPedido });
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
