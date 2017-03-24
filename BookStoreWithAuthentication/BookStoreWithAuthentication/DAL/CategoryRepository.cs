using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {

        private ApplicationDbContext db;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void DeleteCategory(int? id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
        }


        public IEnumerable<Category> GetCategories()
        {
            return db.Categories.ToList();
        }

        public Category GetCategoryByID(int? id)
        {
            return db.Categories.Find(id);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return db.Categories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
        }

        public void InsertCategory(Category category)
        {
            db.Categories.Add(category);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
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