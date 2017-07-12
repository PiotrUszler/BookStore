using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreWithAuthentication.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IGenericRepository<Category> categoryRepository;
        private IGenericRepository<Author> authorRepository;
        private IGenericRepository<Book> bookRepository;
        private IGenericRepository<Series> seriesRepository;
        private IGenericRepository<Publisher> publisherRepository;

        public IGenericRepository<Category> CategoryRepository
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

        public IGenericRepository<Author> AuthorRepository
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

        public IGenericRepository<Book> BookRepository
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

        public IGenericRepository<Series> SeriesRepository
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

        public IGenericRepository<Publisher> PublisherRepository
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