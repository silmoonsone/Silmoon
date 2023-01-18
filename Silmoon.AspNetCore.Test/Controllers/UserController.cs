using Microsoft.AspNetCore.Mvc;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.AspNetCore.Test.Models;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class UserController : Controller
    {
        ISilmoonUserService<User> SilmoonUserService { get; set; }
        public UserController(ISilmoonUserService<User> silmoonUserService)
        {
            SilmoonUserService = silmoonUserService;
        }
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
