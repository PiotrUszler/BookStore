using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class SeriesRepository : ISeriesRepository, IDisposable
    {

        private ApplicationDbContext db;

        public SeriesRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void DeleteSeries(int id)
        {
            Series series = db.Series.Find(id);
            db.Series.Remove(series);
        }

        public IEnumerable<Series> GetSeries()
        {
            return db.Series.ToList();
        }

        public Series GetSeriesByID(int? id)
        {
            return db.Series.Find(id);
        }

        public void InsertSeries(Series series)
        {
            db.Series.Add(series);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateSeries(Series series)
        {
            db.Entry(series).State = System.Data.Entity.EntityState.Modified;
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