using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ASP.NET_CA.Controllers
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}