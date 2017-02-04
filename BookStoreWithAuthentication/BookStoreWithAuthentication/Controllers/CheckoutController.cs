using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreWithAuthentication.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //Get: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            order.Username = User.Identity.Name;
            order.OrderDate = DateTime.Now;

            db.Orders.Add(order);
            db.SaveChanges();

            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.CreateOrder(order);

            return RedirectToAction("Complete", new { id = order.OrderId });
        }

        //GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            bool isValid = db.Orders.Any(order => order.OrderId == id && order.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
        
    }
}