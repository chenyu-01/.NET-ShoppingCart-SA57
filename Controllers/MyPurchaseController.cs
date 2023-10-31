using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA.Models;
using System.Diagnostics;
using ASP.NET_CA.Data;
using Microsoft.AspNetCore.Http.Features;

namespace ASP.NET_CA.Controllers
{
    public class MyPurchaseController : Controller
    {
        private readonly MyDbContext _db;


        public MyPurchaseController(MyDbContext db)
        {
            _db = db;
            ViewData["SearchResult"] = false;
        }


        

        

        [HttpGet]
        public IActionResult Purchase()
        {
            string? usernameInsession = HttpContext.Session.GetString("username");
            if (usernameInsession == null || usernameInsession == "unknown")
            {
                return RedirectToAction("Login", "Login");
            }
            // get the username from session
            string username = HttpContext.Session.GetString("username");
            // fetch the Orders from database and convert it to a list

            // LinQ query to get the Orders from current user
            List<Order> OrdersUser = _db.Orders.Where(o => o.User.UserName == username).ToList();
            ViewData["Orders"] = OrdersUser;
            return View();
        }


        

    }



}
