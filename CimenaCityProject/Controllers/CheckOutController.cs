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
            return View(db.Orders.OrderByDescending(x=>x.OrderDate).ToList());
        }

        // GET: /CheckOut/Details/1
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            return View(order);
        }

        // GET: /CheckOut/Create
        public ActionResult Create()
        {

            return RedirectToAction("Index", "Home");
        }

        // GET: /CheckOut/Edit/5
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);

            var data = db.TimeScreening
                .ToList().Select(s=>new 
                                          {
                                              TimeScreeningID = s.TimeScreeningID,
                                              Description = string.Format("{0} - {1}, {2}",s.MovieShowTime.Movie.MovieName,s.MovieShowTime.ShowTime.TimeOfDay,s.MovieTheaters.TheatersName)

                                           });


            ViewBag.TimeScreeningList = new SelectList(data, "TimeScreeningID", "Description", order.TimeScreeningID);

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
        public ActionResult Edit(int? TimeScreeningList, [Bind(Include="OrderID,PersonID,CartId,ChairsOrderdID,TimeScreeningID,OrderDate")] Order order)
        {
            // i have bug here. 
            if (!TimeScreeningList.HasValue)
            {
                return View(order);
            }
            ICollection<TimeScreening> ts = db.TimeScreening.Where(x => x.TimeScreeningID == TimeScreeningList.Value).ToList();

            order = db.Orders.Find(order.OrderID);
            order.TimeScreeningID = TimeScreeningList.Value;
            order.TimeScreening = ts;

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
