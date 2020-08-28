using DentistApp.Application.Interfaces;
using DentistApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDentistAppService _service;
        public HomeController(ILogger<HomeController> logger, IDentistAppService service)
        {
            _service = service;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View(_service.GetVisitsForDate(DateTime.Now));
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
    }
}
