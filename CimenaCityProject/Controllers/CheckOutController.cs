using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;
using CimenaCityProject.ViewModels;


namespace CimenaCityProject.Controllers
{
    public class CheckOutController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /CheckOut/
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: /CheckOut/Details/5
        public ActionResult Details(string _cartID)
        {
            string cartID = _cartID;

            if (cartID == null || cartID == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event eventOrder = db.Events.Single(c => c.cartID == cartID);
            var quryEventInfo = new TimeScreeningDetails(eventOrder);

            if (ModelState.IsValid)
            {
                db.Orders.Add(quryEventInfo.Order);
                db.SaveChanges();
            }
            else
            {
                quryEventInfo.ifEror = "error by adding order.";
            }

            if (quryEventInfo.Event == null)
            {
                return HttpNotFound();
            }
            return View(quryEventInfo);
        }

        //GET: /CheckOut/CheckOutComplete/cartID
        public ActionResult CheckoutComplete(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event evnt = db.Events.Find(id);
            var orderComplete = new TimeScreeningDetails(evnt);

            return View(orderComplete);
        }


        // GET: /CheckOut/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CheckOut/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrderID,PersonID,CartId,ChairsOrderdID,TimeScreeningID,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: /CheckOut/Edit/5
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

        // POST: /CheckOut/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrderID,PersonID,CartId,ChairsOrderdID,TimeScreeningID,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: /CheckOut/Delete/5
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

        // POST: /CheckOut/Delete/5
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
