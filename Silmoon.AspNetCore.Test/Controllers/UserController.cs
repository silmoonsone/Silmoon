using Microsoft.AspNetCore.Mvc;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Signin(string Url)
        {
            if (await HttpContext.IsSignin()) return Redirect(Url ?? "~/");
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
    }
}
