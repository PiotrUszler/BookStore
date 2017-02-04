using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreWithAuthentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookStoreManagerController : Controller
    {
        // GET: BookStoreManager
        public ActionResult Index()
        {
            return View();
        }
    }
}