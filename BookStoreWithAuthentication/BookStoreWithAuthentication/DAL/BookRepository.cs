using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private ApplicationDbContext db;

        public BookRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void DeleteBook(int bookID)
        {
            Book book = db.Books.Find(bookID);
            db.Books.Remove(book);
        }

        public Book GetBookByID(int? bookID)
        {
            return db.Books.Find(bookID);
        }

        public IEnumerable<Book> GetBooks()
        {
            return db.Books.ToList();
        }

        public void InsertBook(Book book)
        {
            db.Books.Add(book);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
        }

        public void UpdateBookAuthors(ICollection<Author> authors, Book book)
        {
            book.Authors = authors;
            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}