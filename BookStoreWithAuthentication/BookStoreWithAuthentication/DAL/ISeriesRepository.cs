using BookStoreWithAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWithAuthentication.DAL
{
    public interface ISeriesRepository : IDisposable
    {
        IEnumerable<Series> GetSeries();
        Series GetSeriesByID(int? id);
        void InsertSeries(Series series);
        void UpdateSeries(Series series);
        void DeleteSeries(int id);
        void Save();
    }
}
