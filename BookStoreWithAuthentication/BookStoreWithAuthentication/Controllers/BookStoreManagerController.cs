using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace BookStoreWithAuthentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookStoreManagerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}