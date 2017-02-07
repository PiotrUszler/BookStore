using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        [HttpPost]
        public ActionResult EditBook(int? id, string author)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bookToUpdate = db.Books.Include(b => b.Authors).Where(i => i.ID == id).Single();
            if(TryUpdateModel(bookToUpdate, "", new string[] { "Title", "NumOfPages", "Price", "CategoryID", "SeriesID", "PublisherID" }))
            {
                try
                {
                    
                    UpdateBookAuthors(author, bookToUpdate);
                    UpdateBookCover(bookToUpdate, this.Request);
                    

                    db.Entry(bookToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                } catch(RetryLimitExceededException e)
                {
                    ModelState.AddModelError("", e);
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", bookToUpdate.CategoryID);
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name", bookToUpdate.SeriesID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", bookToUpdate.PublisherID);
            ViewBag.Authors = bookToUpdate.AuthorsToString();

            return View(bookToUpdate);

        }
        #endregion

        private void UpdateBookAuthors(string author, Book bookToUpdate)
        {
            List<Author> authors = new List<Author>();
            if (author != null)
            {
                string[] auths = author.Split(',');
                foreach (var item in auths)
                {
                    Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    authors.Add(auth);
                }
            }
            bookToUpdate.Authors = authors;
        }

        private void UpdateBookCover(Book bookToUpdate, HttpRequestBase request)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    file.SaveAs(path);
                    bookToUpdate.ImagePath = fileName;
                }
            }
        }
    }
    }