using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWithAuthentication.DAL
{
    public interface ICategoryRepository : IDisposable
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByName(string categoryName);
        Category GetCategoryByID(int? id);
        void InsertCategory(Category category);
        void DeleteCategory(int? id);
        void UpdateCategory(Category category);
        void Save();
    }
}
