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
        public async Task<IActionResult> ProcesarPago([FromBody] dynamic datos)
        {
            int idPedido = datos.idPedido;
            decimal monto = datos.monto;
            int metodoPagoId = datos.metodoPagoId;
            string tipoTarjeta = datos.tipoTarjeta;
            string digitos = datos.digitos;
            string titular = datos.titular;
            string referencia = datos.referencia;

            var factura = await _context.Facturas
                .Include(f => f.Pagos)
                .FirstOrDefaultAsync(f => f.IdPedido == idPedido);

            if (factura == null)
            {
                // Crear factura si no existe aún
                var pedido = await _context.PedidoLocals
                    .Include(p => p.DetallePedidos)
                    .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

                if (pedido == null) return NotFound();

                var subtotal = (decimal)pedido.DetallePedidos.Sum(d => d.Subtotal);
                var iva = subtotal * 0.13m;
                var total = subtotal + iva;

                factura = new Factura
                {
                    IdPedido = idPedido,
                    Fecha = DateTime.Now,
                    Total = total
                };

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();
            }

            var totalPagado = factura.Pagos.Sum(p => p.Monto ?? 0);
            if (totalPagado >= factura.Total)
                return BadRequest(new { success = false, message = "Esta factura ya está completamente pagada." });

            var pago = new Pago
            {
                FacturaId = factura.FacturaId,
                MetodoPagoId = metodoPagoId,
                Monto = monto,
                Fecha = DateTime.Now
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            if ((metodoPagoId == 1 || metodoPagoId == 2) && !string.IsNullOrEmpty(digitos))
            {
                var pagoTarjeta = new PagoTarjetum
                {
                    PagoId = pago.PagoId,
                    Tipo = tipoTarjeta,
                    Digitos = digitos,
                    Titular = titular,
                    Referencia = referencia
                };

                _context.PagoTarjeta.Add(pagoTarjeta);
                await _context.SaveChangesAsync();
            }

            totalPagado += monto;

            return Ok(new { success = true, totalPagado });
        }
    }
}
