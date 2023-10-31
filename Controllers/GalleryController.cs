using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA.Models;
using System.Diagnostics;
using ASP.NET_CA.Data;
using System.Web;
using System.Text.Json;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;


namespace ASP.NET_CA.Controllers
{

    public class GalleryController : Controller
    {
        private readonly MyDbContext _db;

        public GalleryController(MyDbContext db)
        {
            _db = db;
        }


        public IActionResult Gallery(string searchstr)
        {


            string? usernameInsession = HttpContext.Session.GetString("username");
            if (usernameInsession == null)
            {
                // not login, a temp unkown user
                HttpContext.Session.SetString("username", "unknown");
                ViewData["username"] = "unknown";
            }
            else
            {
                ViewData["username"] = usernameInsession;
            }




            
            // get all items from database
            List<Item> items = _db.Items.ToList();

            
            if (searchstr != null)
            {
                ViewData["items"] = FilterBySearchStr(searchstr, items);
            }
            else
            {
                ViewData["items"] = items;
            }

            ViewData["CartItemCount"] = GetTotalItemCount();
            ViewData["searchStr"] = searchstr; // to keep input search string in search box
            return View();
        }

        public List<Item> FilterBySearchStr(string searchstr, List<Item> items)
        {
            List<Item> Filter = new List<Item>();
            foreach (Item It in items)
            {
                if (It.ItemName != null)
                {
                    if (It.ItemName.ToLower().IndexOf(searchstr.ToLower()) != -1)
                    {
                        Filter.Add(It);
                        continue; // skip the search for description to avoid duplicate items
                    }
                    if (It.ItemDescription.ToLower().IndexOf(searchstr.ToLower()) != -1)
                    {
						Filter.Add(It);
                        continue;
					}
                }
            }
            return Filter;
        }

        public IActionResult AddToCart(int Id)
        {
            // find item by id
            Item itemClicked = _db.Items.Find(Id);
            // find current user by username
            User user = _db.Users.FirstOrDefault(user => user.UserName == HttpContext.Session.GetString("username"));
            
            CartItem userCartItem = _db.CartItems.FirstOrDefault(cartItem => cartItem.User == user && cartItem.Item.ItemID == Id);
            // if item is not in CartItem table in database, add it to database
            if (userCartItem == null)
            {
                CartItem cartItem = new CartItem(); // create a new CartItem object with attributes from itemClicked
                cartItem.Item = itemClicked;
                cartItem.User = user;
                cartItem.Quantity = 1;
                _db.CartItems.Add(cartItem);
                _db.SaveChanges();
            } else
            {
                // if item is already in CartItem table in database, increase its quantity by 1
                userCartItem.Quantity += 1;
                _db.SaveChanges();
            }
            
            // get total cart item count from database
            ViewData["cartItemCounts"] = GetTotalItemCount();
            
            
            //TempData["CartItemCount"] = GetTotalItemCount();
            return RedirectToAction("Gallery", "Gallery");
        }




        // get total cart item count from database for current user
        private int GetTotalItemCount()
        {
            var totalItemCount = 0;
            // get all cart items for current user
            List<CartItem> cartItems = _db.CartItems.Where(cartItem => cartItem.User.UserName == HttpContext.Session.GetString("username")).ToList();

            foreach (CartItem cartItem in cartItems)
            {
                totalItemCount += cartItem.Quantity;
            }

            return totalItemCount;
        }
        


    }
}