using Microsoft.AspNetCore.Mvc;

namespace foodieProyecto.Controllers
{
    public class CocinaController : Controller
    {
        public IActionResult Cocina()
        {
            return View();
        }
    }
}
