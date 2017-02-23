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

    public class BookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Book
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "price" ? "price_desc" : "price";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;

            var books = db.Books.Include(b => b.Category).Include(b => b.Publisher).Include(b => b.Series);

            if (!String.IsNullOrEmpty(search))
            {
                books = books.Where(
                    s => s.Title.Contains(search) ||
                    s.Category.Name.Contains(search) ||
                    s.Authors.Any(a => a.FirstName.Contains(search) || a.LastName.Contains(search))
                    );
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                case "price":
                    books = books.OrderBy(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);

            if (book == null)
            {
                return HttpNotFound();
            }
            double rating;
            try
            {
                rating = book.Ratings.Average(r => r.Rate);
            }
            catch(InvalidOperationException ex)
            {
                rating = 0;
            }
            ViewBag.Rating = rating;
            book.Ratings.FirstOrDefault(r => r.User == this.HttpContext.User.Identity.Name ? ViewBag.UserVoted = true : ViewBag.UserVoted = false);

            return View(book);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetRating(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            double rating;
            try
            {
                rating = book.Ratings.Average(r => r.Rate);
            }
            catch (InvalidOperationException ex)
            {
                rating = 0;
            }

            return Json(new { rating = rating });
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult GetUserRating(int id)
        {
            Book book = db.Books.Find(id);
            Rating userRating = book.Ratings.FirstOrDefault(r => r.User == this.HttpContext.User.Identity.Name);
            return Json(new { rating = userRating.Rate });
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult RateBook(int id, int vote)
        {
            Book book = db.Books.Find(id);
            book.Ratings.Add(new Rating(){ book = book, Rate = vote, User = this.HttpContext.User.Identity.Name });
            db.SaveChanges();
            return Json("");
        }

        #region createBook
        //GET: /Book/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name");
            ViewBag.SeriesID = new SelectList(db.Series, "ID", "Name");

            return View();
        }

        //POST: /Book/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title, NumOfPages, Price, CategoryID, PublisherID, SeriesID")] Book book, string author)
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

        //GET: /Book/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
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

        //POST: /Book/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id, string author)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bookToUpdate = db.Books.Include(b => b.Authors).Where(i => i.ID == id).Single();
            if (TryUpdateModel(bookToUpdate, "", new string[] { "Title", "NumOfPages", "Price", "CategoryID", "SeriesID", "PublisherID" }))
            {
                try
                {

                    UpdateBookAuthors(author, bookToUpdate);
                    UpdateBookCover(bookToUpdate, this.Request);


                    db.Entry(bookToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException e)
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

        //GET: /BookStoreManager/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = db.Books.Find(id);

            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        //POST: /BookStoreManager/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
