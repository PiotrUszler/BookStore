using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWithAuthentication.DAL
{
    public interface IPublisherRepository : IDisposable
    {
        IEnumerable<Publisher> GetPublishers();
        Publisher GetPublisherByID(int? id);
        void InsertPublisher(Publisher publisher);
        void UpdatePublisher(Publisher publisher);
        void DeletePublisher(int id);
        void Save();
    }
}
