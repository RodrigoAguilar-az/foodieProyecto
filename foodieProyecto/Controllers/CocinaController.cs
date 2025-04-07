using foodieProyecto.Models.Vistas;
using foodieProyecto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
            .Where(d => d.Encabezado != null && d.TipoVenta == "Local" && d.Encabezado.Estado != "Cerrado" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
            .ToList();

            var listaCocina = _context.DetallePedidos
            .Include(d => d.Encabezado)
            .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Local" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
            .ToList();

            var listaOnline = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Online" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
                .ToList();

            ViewBag.CocinaCount = listaCocina.Count;
            ViewBag.OnlineCount = listaOnline.Count;

            var pedidos = lista.OrderBy(d=> d.IdDetallePedido).Select(d => new DetallePedidoViewModel
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
        //[HttpGet]
        //public IActionResult ObtenerListadoPedidos()
        //{
        //    var listaPedidos = _context.DetallePedidos
        //        .Include(d => d.Encabezado)
        //        .Where(d => d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Local"&& d.Estado != "Finalizado" && d.Estado != "Cancelado")
        //        .ToList();

        //    var pedidosViewModel = listaPedidos.Select(d => new DetallePedidoViewModel
        //    {
        //        IdDetallePedido = d.IdDetallePedido,
        //        IdPedido = d.Encabezado.IdPedido,
        //        IdMesa = d.Encabezado.IdMesa,
        //        Hora = d.Encabezado.FechaApertura?.ToString("HH:mm") ?? "--:--",
        //        NombreItem = ObtenerNombreItem(d.TipoItem, d.ItemId),
        //        TipoItem = d.TipoItem,
        //        Comentarios = d.Comentarios ?? "",
        //        Estado = d.Estado
        //    }).ToList();

        //    return PartialView("_ListadoPedidos", pedidosViewModel); 
        //}
        [HttpGet]
        public IActionResult ObtenerPedidosEditados(string searchQuery, string activeTab, string sortOrder = "antiguo")
        {
            GlobalData.ActiveTab = activeTab;
            GlobalData.Search = searchQuery;
            GlobalData.Filtro = sortOrder;

            var lista = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado.Estado != "Cerrado" && d.Estado != "Finalizado" && d.Estado != "Cancelado");

            if (activeTab == "tab-local")
            {
                lista = lista.Where(d => d.TipoVenta == "Local");
            }else if (activeTab == "tab-online")
            {
                lista = lista.Where(d => d.TipoVenta == "En linea");
            }


            if (!string.IsNullOrEmpty(searchQuery))
            {
                lista = lista.Where(d => d.Encabezado.IdPedido.ToString().Contains(searchQuery));
            }

            switch (sortOrder)
            {
                case "antiguo":
                    lista = lista.OrderBy(d => d.IdDetallePedido);  
                    break;
                case "mesa":
                    lista = lista.OrderBy(d => d.Encabezado.IdMesa); 
                    break;
                default:
                    lista = lista.OrderByDescending(d => d.IdDetallePedido);  
                    break;
            }

            var pedidos = lista.ToList().Select(d => new DetallePedidoViewModel
            {
                IdDetallePedido = d.IdDetallePedido,
                IdPedido = d.Encabezado.IdPedido,
                IdMesa = d.Encabezado.IdMesa,
                Hora = d.Encabezado.FechaApertura != null ? d.Encabezado.FechaApertura.Value.ToString("HH:mm") : "--:--",
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
            int idPedido = _context.DetallePedidos
                .Where(d => d.IdDetallePedido == idDetalle)
                .Select(d => d.EncabezadoId)
                .FirstOrDefault();

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

            var validacion = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.EncabezadoId == idPedido && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Local" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
                .ToList();
            if (validacion.Count == 0)
            {
                return Content($"<script>document.getElementById('pedido-{idPedido}').remove();</script>");
            }
            if (detalle.Estado == "Finalizado")
            {
                return Content(""); 
            }

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

        [HttpGet]
        public IActionResult ObtenerContadores()
        {
            var listaCocina = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "Local" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
                .ToList();

            var listaOnline = _context.DetallePedidos
                .Include(d => d.Encabezado)
                .Where(d => d.Encabezado != null && d.Encabezado.Estado != "Cerrado" && d.TipoVenta == "En linea" && (d.Estado != "Finalizado" && d.Estado != "Cancelado"))
                .ToList();

            ViewBag.CocinaCount = listaCocina.Count;
            ViewBag.OnlineCount = listaOnline.Count;

            return PartialView("_Tabs");
        }




    }
}

public static class GlobalData
{
    public static string ActiveTab { get; set; } = "tab-local";
    public static string Search { get; set; } = "";
    public static string Filtro { get; set; } = "antiguo";
}
