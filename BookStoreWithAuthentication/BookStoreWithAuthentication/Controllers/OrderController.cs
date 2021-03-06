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
using System.Data.Entity.Infrastructure;

namespace BookStoreWithAuthentication.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Order/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            List<OrderDetail> details = db.OrderDetails.Where(o => o.OrderId == id).ToList();
            ViewBag.details = details;
            ViewBag.orderStatus = order.Status.GetName();
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Order/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Details")]
        public ActionResult UpdateOrderStatus(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderToUpdate = db.Orders.Find(id);
            if(TryUpdateModel(orderToUpdate, "", new string[] { "Status"}))
            if (ModelState.IsValid)
            {
                    try
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch(RetryLimitExceededException ex)
                    {
                        Console.WriteLine(ex.Message);
                        ModelState.AddModelError("", "Nie udało się zapisać zmian.");
                    }

            }
            return View(orderToUpdate);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
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
