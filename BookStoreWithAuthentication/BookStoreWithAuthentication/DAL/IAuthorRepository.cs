using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWithAuthentication.DAL
{
    public interface IAuthorRepository : IDisposable
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthorByID(int? id);
        void InsertAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(int id);
        void Save();
    }
}
