using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreWithAuthentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookStoreManagerController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookStoreManager
        public ActionResult Index()
        {
            return View();
        }

        //GET: /BookStoreManager/CreateBook
        public ActionResult CreateBook()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name");
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name");

            return View();
        }

        //POST: /BookStoreManager/CreateBook
        [HttpPost]
        public ActionResult CreateBook([Bind(Include = "Title, NumOfPages, Price, CategoryID, PublisherID, SeriesID")] Book book, string author)
        {
            //Need to figure out how to add multiple authors
            Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == author);
            List<Author> authors = new List<Author>();
            authors.Add(auth);
            book.Authors = authors;

            if(Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if(file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    file.SaveAs(path);
                    book.ImagePath = fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", book.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name", book.SeriesID);

            return View(book);
        }
    }
}