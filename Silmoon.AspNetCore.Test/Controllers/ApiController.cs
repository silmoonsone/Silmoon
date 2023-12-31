﻿using Microsoft.AspNetCore.Mvc;
using Silmoon.AspNetCore.Filters;
using Silmoon.AspNetCore.Test.Services;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //[ServiceFilter(typeof(FilterSampleAttribute))]
        [RequestSignatureValidation(Require = true)]
        //[ParameterSignture("Key", Configure.SigntrueKey, "Signature", true)]
        public IActionResult KeyTest()
        {
            return Ok("Success");
        }


        [HttpGet]
        //[ServiceFilter(typeof(FilterSampleAttribute))]
        //[ParameterSignture("Key", Configure.SigntrueKey, "Signature", true)]
        public IActionResult KeyTest2()
        {
            return Ok("Success");
        }
    }
}
