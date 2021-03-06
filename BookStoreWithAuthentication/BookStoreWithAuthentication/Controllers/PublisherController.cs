﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.Controllers
{
    public class PublisherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Publisher
        public ActionResult Index()
        {
            return View(unitOfWork.PublisherRepository.Get().ToList());
        }

        // GET: Publisher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Publisher publisher = db.Publishers.Find(id);
            Publisher publisher = unitOfWork.PublisherRepository.GetByID(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // GET: Publisher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                //db.Publishers.Add(publisher);
                //db.SaveChanges();
                unitOfWork.PublisherRepository.Insert(publisher);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Publisher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Publisher publisher = db.Publishers.Find(id);
            Publisher publisher = unitOfWork.PublisherRepository.GetByID(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publisher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(publisher).State = EntityState.Modified;
                //db.SaveChanges();
                unitOfWork.PublisherRepository.Update(publisher);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        // GET: Publisher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Publisher publisher = db.Publishers.Find(id);
            Publisher publisher = unitOfWork.PublisherRepository.GetByID(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publisher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Publisher publisher = db.Publishers.Find(id);
            //db.Publishers.Remove(publisher);
            //db.SaveChanges();
            unitOfWork.PublisherRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
