using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using BookStoreWithAuthentication.ViewModels;

namespace BookStoreWithAuthentication.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        //GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addedBook = db.Books.Single(book => book.ID == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedBook);
            return RedirectToAction("Index");
        }

        //AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string bookTitle = db.Carts.Single(item => item.BookId == id).Book.Title;

            int itemCount = cart.RemoveFromCart(id);

            var result = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(bookTitle) + " został usunięty z koszyka.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(result);
        }

        //Get: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}