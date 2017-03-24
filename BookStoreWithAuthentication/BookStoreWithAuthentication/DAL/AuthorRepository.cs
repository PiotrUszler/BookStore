using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {

        private ApplicationDbContext db;

        public AuthorRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
        }

        public Author GetAuthorByID(int? id)
        {
            return db.Authors.Find(id);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return db.Authors.ToList();
        }

        public void InsertAuthor(Author author)
        {
            db.Authors.Add(author);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateAuthor(Author author)
        {
            db.Entry(author).State = System.Data.Entity.EntityState.Modified;
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