using Microsoft.AspNetCore.Mvc;
using SV21T1020324.Web.Models;
using System.Diagnostics;

namespace SV21T1020324.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult TestInput(string hoTen= "", string diaChi = "")
        {
            return Content($"Họ tên: {hoTen}, Địa chỉ: {diaChi}");
        }

        public IActionResult Customers()
        {
            var model = BusinessLayers.CommonDataService.ListOfCustomers();
            return View(model);
        }
    }
}
