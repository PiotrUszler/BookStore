using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class PublisherRepository : IPublisherRepository, IDisposable
    {

        private ApplicationDbContext db;

        public PublisherRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void DeletePublisher(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
        }

        public Publisher GetPublisherByID(int? id)
        {
            return db.Publishers.Find(id);
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return db.Publishers.ToList();
        }

        public void InsertPublisher(Publisher publisher)
        {
            db.Publishers.Add(publisher);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdatePublisher(Publisher publisher)
        {
            db.Entry(publisher).State = System.Data.Entity.EntityState.Modified;
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