using Microsoft.AspNetCore.Mvc;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class PortalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Editor()
        {
            return View();
        }
    }
}
