using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        #region createBook
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
            if (ModelState.IsValid)
            {
                //Adding authors by splitting string
                string[] auths = author.Split(',');
                List<Author> authors = new List<Author>();
                foreach (var item in auths)
                {
                    Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    authors.Add(auth);
                }
                book.Authors = authors;

                //File upload, add some sort of waiting to complete uploading
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                        file.SaveAs(path);
                        book.ImagePath = fileName;
                    }
                }

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", book.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name", book.SeriesID);

            return View(book);
        }

        #endregion

        #region EditBook

        //GET: /BookStoreManager/EditBook/5
        public ActionResult EditBook(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = db.Books.Find(id);

            if (book == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", book.CategoryID);
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name", book.SeriesID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            ViewBag.Authors = book.AuthorsToString();

            return View(book);
        }

        //POST: /BookStoreManager/EditBook/5
        [HttpPost]
        public ActionResult EditBook([Bind(Include = "ID, Title, NumOfPages, Price, Authors, CategoryID, SeriesID, PublisherID")]Book book, string author)
        {
            //TODO fix updating authors, it doesnt work
            if (ModelState.IsValid)
            {
                string[] auths = author.Split(',');
                List<Author> authors = new List<Author>();
                foreach (var item in auths)
                {
                    Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    authors.Add(auth);
                }
                book.Authors = authors;
                
                //File upload, add some sort of waiting to complete uploading
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                        file.SaveAs(path);
                        book.ImagePath = fileName;
                    }
                }
                db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", book.CategoryID);
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name", book.SeriesID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            ViewBag.Authors = book.AuthorsToString();

            return View(book);
        }

        #endregion
    }
}