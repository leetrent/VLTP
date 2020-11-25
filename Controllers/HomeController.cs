using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VLTP.Models;
using VLTP.Services;

namespace VLTP.Controllers
{
    public class HomeController : Controller
    {
        public readonly ILoginService _loginService;
        private readonly ILogger<HomeController> _logger;
 
        public HomeController(ILoginService loginSvc, ILogger<HomeController> logger)
        {
            _loginService = loginSvc;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string logSnippet = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][HomeController][Index] => ";
            LoginInfo loginInfo = _loginService.RetreiveLoginInfo();
            System.Console.WriteLine(logSnippet + "BEGIN LoginInfo:");
            System.Console.WriteLine(loginInfo);
            System.Console.WriteLine(logSnippet + ":END LoginInfo");
            return View(loginInfo);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }
    }
}
