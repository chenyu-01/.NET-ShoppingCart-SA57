using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA.Models;
using System.Diagnostics;
using ASP.NET_CA.Data;
namespace ASP.NET_CA.Controllers
{
    public class LoginController : Controller
    {
		
		public IActionResult Index()
		{
			return RedirectToAction("Login");
		}
        private readonly MyDbContext _db;
        //inject database
        public LoginController(MyDbContext db)
        {
            _db = db;
        }
        


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            
            var userInDb = _db.Users.FirstOrDefault(u => u.UserName == username && u.UserPassword == password);

            if (userInDb != null)
            {
                // Authentication successful, perform necessary operations set http context session
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("password", password);
				
				ViewBag.Error = "";
                // if TempData["CheckoutFlag"] is null, then not redirected from checkout, go to gallery
                if (TempData["CheckoutFlag"] == null)
                {
					return RedirectToAction("Gallery", "Gallery");
				}
				// if redirected from checkout, checkout cart items from unknown user add order to current user
				if (TempData["CheckoutFlag"].Equals("true"))
                {
                    checkoutCart();
                    TempData["CheckoutFlag"] = "false";
                    return RedirectToAction("Purchase", "MyPurchase");
                }
				// delete unknown user's cart items
				_db.RemoveRange(_db.CartItems.Where(cartItem => cartItem.User.UserName == "unknown"));
				_db.SaveChanges();

				return RedirectToAction("Gallery", "Gallery"); // Redirect to the home page after successful login
            }
            else
            {
                if (username == null || password == null)
                {
                    ViewBag.Error = "Please enter username or password";
                    return View();
                }
                ViewBag.Error = "Invalid username or password";
                return View();
            }
        }

        // GET
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("username") != null && HttpContext.Session.GetString("password") != null)
            {
                return RedirectToAction("Gallery", "Gallery");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        
        // after successful login, checkout cart items from unknown user add order to current user
        public void checkoutCart()
        {
            // get cart items from database for unknown user
            List<CartItem> cartItems = _db.CartItems.Where(cartItem => cartItem.User.UserName == "unknown").ToList();
            
            
            List<Order> orderData = new List<Order>();
            // using method from ViewCartController to generate order data
            
            orderData = generateOrderData(cartItems);
            // add orders to database
            foreach (var orderRow in orderData)
            {
                _db.Orders.Add(orderRow);
                _db.SaveChanges();
            }
			// delete unknown user's cart items
			_db.RemoveRange(_db.CartItems.Where(cartItem => cartItem.User.UserName == "unknown"));
			_db.SaveChanges();

        }

        public List<Order> generateOrderData(List<CartItem> cartItems)
        {
            string username = HttpContext.Session.GetString("username");
            User user = _db.Users.FirstOrDefault(u => u.UserName == username);
            List<Order> orders = new List<Order>();
            foreach (CartItem cartItem in cartItems)
            {
                List<Activationcode> ids = new List<Activationcode>();
                for (int j = 0; j < cartItem.Quantity; j++)
                {
                    Activationcode actCode = new Activationcode();//initialize a random activation code
                    ids.Add(actCode);

                }

                orders.Add(
                    new Order
                    {
                        User = user,
                        OrderID = Guid.NewGuid(),
                        // id_count.key is the ItemID, get itemCart from db
                        Item = _db.Items.Find(cartItem.Item.ItemID),
                        OrderStatus = "Placed",
                        // get format date like 10 Apr 2029
                        OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString("dd MMM yyyy"),
                        Quantity = cartItem.Quantity,
                        Activationcodes = ids

                    });

            }
            return orders;

        }
    }
}