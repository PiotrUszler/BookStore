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
        //private ApplicationDbContext db = new ApplicationDbContext();
        private IBookRepository bookRepo;
        private ICategoryRepository categoryRepo;
        private IPublisherRepository publisherRepo;
        private ISeriesRepository seriesRepo;
        private IAuthorRepository authorRepo;

        private UnitOfWork unitOfWork = new UnitOfWork();

        private ApplicationDbContext _db = new ApplicationDbContext();

        private ApplicationDbContext db = new ApplicationDbContext();

        public BookController()
        {
            this.bookRepo = new BookRepository(_db);
            this.categoryRepo = new CategoryRepository(_db);
            this.publisherRepo = new PublisherRepository(_db);
            this.seriesRepo = new SeriesRepository(_db);
            this.authorRepo = new AuthorRepository(_db);
        }

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryrepository, IPublisherRepository publisherRepository,
            ISeriesRepository seriesRepository, IAuthorRepository authorRepository)
        {
            this.bookRepo = bookRepository;
            this.categoryRepo = categoryrepository;
            this.publisherRepo = publisherRepository;
            this.seriesRepo = seriesRepository;
            this.authorRepo = authorRepository;
        }

        // GET: Book
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string currentFilter, string search, string category, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "price" ? "price_desc" : "price";

            ViewBag.Category = category;

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;

            //var books = db.Books.Include(b => b.Category).Include(b => b.Publisher).Include(b => b.Series);
            //var books = from b in bookRepo.GetBooks() select b;
            var books = unitOfWork.BookRepository.Get();

            if (!String.IsNullOrEmpty(search))
            {
                books = books.Where(
                    s => s.Title.Contains(search) ||
                    s.Category.Name.Contains(search) ||
                    s.Authors.Any(a => a.FirstName.Contains(search) || a.LastName.Contains(search))
                    );
            } else if (!String.IsNullOrEmpty(category))
            {
                books = books.Where(
                    b => b.Category.Name.ToUpper() == category.ToUpper()
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
        public ActionResult Category(string category, string sortOrder, int? page)
        {
            if(category == null)
            {
                return View("Index");
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "price" ? "price_desc" : "price";

            //var books = db.Books.Where(
            //    b => b.Category.Name.ToLower() == category.ToLower()
            //    );
            //var books = bookRepo.GetBooks().Where(b => b.Category.Name.ToLower() == category.ToLower());
            var books = unitOfWork.BookRepository.Get(b => b.Category.Name.ToLower() == category.ToLower());

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

            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);

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
            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);
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
            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);
            Rating userRating = book.Ratings.FirstOrDefault(r => r.User == this.HttpContext.User.Identity.Name);
            return Json(new { rating = userRating.Rate });
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult RateBook(int id, int vote)
        {
            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);
            book.Ratings.Add(new Rating(){ book = book, Rate = vote, User = this.HttpContext.User.Identity.Name });
            //db.SaveChanges();
            //bookRepo.Save();
            unitOfWork.Save();
            return Json("");
        }

        #region createBook
        //GET: /Book/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(unitOfWork.CategoryRepository.Get(), "ID", "Name");
            ViewBag.PublisherID = new SelectList(unitOfWork.PublisherRepository.Get(), "ID", "Name");
            ViewBag.SeriesID = new SelectList(unitOfWork.SeriesRepository.Get(), "ID", "Name");

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
                    //Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    //Author auth = authorRepo.GetAuthors().SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    Author auth = unitOfWork.AuthorRepository.Get().SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
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

                //db.Books.Add(book);
                //bookRepo.InsertBook(book);
                unitOfWork.BookRepository.Insert(book);
                //bookRepo.Save();
                //db.SaveChanges();
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(unitOfWork.CategoryRepository.Get(), "ID", "Name");
            ViewBag.PublisherID = new SelectList(unitOfWork.PublisherRepository.Get(), "ID", "Name");
            ViewBag.SeriesID = new SelectList(unitOfWork.SeriesRepository.Get(), "ID", "Name");

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

            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);

            if (book == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(unitOfWork.CategoryRepository.Get(), "ID", "Name");
            ViewBag.PublisherID = new SelectList(unitOfWork.PublisherRepository.Get(), "ID", "Name");
            ViewBag.SeriesID = new SelectList(unitOfWork.SeriesRepository.Get(), "ID", "Name");
            ViewBag.Authors = book.AuthorsToString();

            return View(book);
        }

        //TODO Fix updating books authors
        //POST: /Book/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id, string author)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //var bookToUpdate = db.Books.Include(b => b.Authors).Where(i => i.ID == id).Single();
            //var bookToUpdate = bookRepo.GetBookByID(id);
            var bookToUpdate = unitOfWork.BookRepository.GetByID(id);
            if (TryUpdateModel(bookToUpdate, "", new string[] { "Title", "NumOfPages", "Price", "CategoryID", "SeriesID", "PublisherID" }))
            {
                try
                {

                    //UpdateBookAuthors(author, bookToUpdate);
                    UpdateBookCover(bookToUpdate, this.Request);


                    //db.Entry(bookToUpdate).State = EntityState.Modified;
                    //db.SaveChanges();


                    //bookRepo.UpdateBook(bookToUpdate);
                    //bookRepo.Save();
                    unitOfWork.BookRepository.Update(bookToUpdate);
                    unitOfWork.Save();


                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException e)
                {
                    ModelState.AddModelError("", e);
                }
            }
            ViewBag.CategoryID = new SelectList(unitOfWork.CategoryRepository.Get(), "ID", "Name");
            ViewBag.PublisherID = new SelectList(unitOfWork.PublisherRepository.Get(), "ID", "Name");
            ViewBag.SeriesID = new SelectList(unitOfWork.SeriesRepository.Get(), "ID", "Name");
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

            //Book book = db.Books.Find(id);
            //Book book = bookRepo.GetBookByID(id);
            Book book = unitOfWork.BookRepository.GetByID(id);

            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        //POST: /BookStoreManager/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //Book book = db.Books.Find(id);
            //db.Books.Remove(book);
            //db.SaveChanges();
            //bookRepo.DeleteBook(id);
            //bookRepo.Save();
            unitOfWork.BookRepository.Delete(id);
            unitOfWork.Save();
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
                    //Author auth = db.Authors.SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    //Author auth = authorRepo.GetAuthors().SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    Author auth = unitOfWork.AuthorRepository.Get().SingleOrDefault(a => (a.FirstName + " " + a.LastName) == item);
                    authors.Add(auth);
                }
            }
            //bookToUpdate.Authors = authors;
            bookRepo.UpdateBookAuthors(authors, bookToUpdate);
            bookRepo.Save();

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
            bookRepo.Dispose();
            publisherRepo.Dispose();
            categoryRepo.Dispose();
            seriesRepo.Dispose();
            authorRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
