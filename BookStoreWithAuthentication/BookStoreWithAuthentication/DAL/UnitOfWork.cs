using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreWithAuthentication.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Author> authorRepository;
        private GenericRepository<Book> bookRepository;
        private GenericRepository<Series> seriesRepository;
        private GenericRepository<Publisher> publisherRepository;

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if(this.categoryRepository == null)
                {
                    this.categoryRepository = new GenericRepository<Category>(db);
                }
                return categoryRepository;
            }
        }

        public GenericRepository<Author> AuthorRepository
        {
            get
            {
                if(this.authorRepository == null)
                {
                    this.authorRepository = new GenericRepository<Author>(db);
                }
                return authorRepository;
            }
        }

        public GenericRepository<Book> BookRepository
        {
            get
            {
                if(bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(db);
                }
                return bookRepository;
            }
        }

        public GenericRepository<Series> SeriesRepository
        {
            get
            {
                if(seriesRepository == null)
                {
                    this.seriesRepository = new GenericRepository<Series>(db);
                }
                return seriesRepository;
            }
        }

        public GenericRepository<Publisher> PublisherRepository
        {
            get
            {
                if(publisherRepository == null)
                {
                    this.publisherRepository = new GenericRepository<Publisher>(db);
                }
                return publisherRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
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