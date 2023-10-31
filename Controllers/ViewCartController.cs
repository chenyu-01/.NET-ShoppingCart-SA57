using Microsoft.AspNetCore.Mvc;
using ASP.NET_CA.Models;
using System.Diagnostics;
using System.Text.Json;
using ASP.NET_CA.Data;
using System.Collections.Generic;
using System;

namespace ASP.NET_CA.Controllers
{
    public class ViewCartController : Controller
    {
        private readonly MyDbContext _db;
        public ViewCartController(MyDbContext db)
        {
            _db = db;
        }

        // GET: Cart

        public IActionResult Index()
        {
            return RedirectToAction("ViewCart", "ViewCart");
        }

        public IActionResult ViewCart()
        {
            
            string? usernameInsession = HttpContext.Session.GetString("username");
            if (usernameInsession == null)
            {
                // not login, unkown user
                usernameInsession = "unknown";
                HttpContext.Session.SetString("username", "unknown");
            }
            // get all CartItems from database for current user
            List<CartItem> cartItems = _db.CartItems.Where(cartItem => cartItem.User.UserName == usernameInsession).ToList();

            
            

            
            ViewData["cartItems"] = cartItems;
            return View();
        }
        public IActionResult Checkout()
        {
            // get all CartItems from database for current user
            string? usernameInsession = HttpContext.Session.GetString("username");
            
            // not login or unknown user
            if(usernameInsession == null || usernameInsession == "unknown")
            {
                TempData["CheckoutFlag"]="true";
                return RedirectToAction("Login", "Login");
            }
            
            List<CartItem> cartItems = _db.CartItems.Where(cartItem => cartItem.User.UserName == usernameInsession).ToList();
            
            List<Order> orderData = generateOrderData(cartItems);
            // add orders and delete cart items
            foreach (var orderRow in orderData)
            {
                _db.Orders.Add(orderRow);
                _db.SaveChanges();
            }
            // remove cart items for current user
            _db.CartItems.RemoveRange(cartItems);
            _db.SaveChanges();
            return RedirectToAction("Purchase", "MyPurchase");
        }
        public IActionResult ContinueShopping()
        {
            return RedirectToAction("Gallery", "Gallery");
        }
        [HttpPost]
        public IActionResult DeleteItem(int ItemID)
        {
            CartItem cartItem = _db.CartItems.FirstOrDefault(cartItem => cartItem.Item.ItemID == ItemID);
            _db.Remove(cartItem);
            _db.SaveChanges();
            return RedirectToAction("ViewCart", "ViewCart");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int ItemID, int Quantity)
        {
            // update quantity in database
            CartItem cartItem = _db.CartItems.FirstOrDefault(cartItem => cartItem.Item.ItemID == ItemID);
            // if quantity is 0, delete the cart item
            if (Quantity == 0)
            {
                _db.Remove(cartItem);
                _db.SaveChanges();
                return RedirectToAction("ViewCart", "ViewCart");
            }
            // else update quantity
            cartItem.Quantity = Quantity;
            _db.SaveChanges();
            return RedirectToAction("ViewCart", "ViewCart");
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
                        // id_count.key is the ItemID, get item from db
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