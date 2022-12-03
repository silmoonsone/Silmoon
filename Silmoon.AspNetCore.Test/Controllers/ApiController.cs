using Microsoft.AspNetCore.Mvc;
using Silmoon.AspNetCore.Filters;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, ParameterSignture("Key", Configure.SigntrueKey, "Sign", true)]
        public IActionResult KeyTest()
        {
            return Ok("Success");
        }
    }
}
