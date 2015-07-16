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
    public class ChairsController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /Chairs/
        public ActionResult Index()
        {
            return View(db.ChairsOrderd.ToList());
        }

        // GET: /Chairs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChairsOrderd chairsorderd = db.ChairsOrderd.Find(id);
            if (chairsorderd == null)
            {
                return HttpNotFound();
            }


            return View(chairsorderd);
        }

        // GET: /Chairs/SelectChair/showTimeID
        public ActionResult SelectChair(int? id, int? theatresID,int? timescreenID)
        {
            if (!id.HasValue || !theatresID.HasValue || !timescreenID.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var theatresChair = new TheatersChairs(id, theatresID,timescreenID);

            return View(theatresChair);
        }

        // POST: /Chairs/SelectChair
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectChair(string[] SelectedChair, TheatersChairs theatersChairs)
        {
            // here  start to close the Event with the cartID
            string cartID = Guid.NewGuid().ToString();
            bool flag = false;

            var theatresChair = new TheatersChairs(
                                                    theatersChairs.TimeScreening.MovieShowTimeID
                                                    , theatersChairs.TimeScreening.TheatresID
                                                     , theatersChairs.TimeScreening.TimeScreeningID);

            int MovieID = db.MovieShowTimes.Where(mID => mID.MovieShowTimeID == theatersChairs.TimeScreening.MovieShowTimeID).SingleOrDefault().MovieID;

            Event newEvent = new Event();
            newEvent.cartID = cartID;
            newEvent.MovieShowTimeID = theatersChairs.TimeScreening.MovieShowTimeID;
            if (ModelState.IsValid)
            {
                db.Events.Add(newEvent);
                db.SaveChanges();
                flag = true;
            }
            else
            {
                flag = false;
                cartID = "Eror";
            }

            for (int i = 0; i < SelectedChair.Length; i++)
            {
                HallChairs hallChairSelected = db.HallChairs.Find(Convert.ToInt16(SelectedChair[i]));
                hallChairSelected.IsSelected = true;

                ChairsOrderd newChairOrder = new ChairsOrderd();
                newChairOrder.EventID = newEvent.EventID;
                newChairOrder.HallChairID = hallChairSelected.HallChairsID;
 
                try
                {
                    if (ModelState.IsValid)
                    {
                        
                        db.Entry<HallChairs>(hallChairSelected).State = EntityState.Modified;
                        db.ChairsOrderd.Add(newChairOrder);
                        db.SaveChanges();
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        cartID = "Eror";
                        break;
                    }
                }
                catch (Exception)
                {
                    flag = false;
                    cartID = "Eror";
                    break;
                }
            }

            if (flag == true)
            {
                //continue to check out. 
                return RedirectToAction("Details", "CheckOut", new { _cartID = cartID });
            }
            else
            {
                //get back to movie via MovieID and error massage 
                return RedirectToAction("Details", "Movie", new {id=MovieID,  error = "Cant add you chair to ticket, try again. " });
            }

        }

        // GET: /Chairs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Chairs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ChairsOrderdiD,HallChairID,EventID,CartId")] ChairsOrderd chairsorderd)
        {
            if (ModelState.IsValid)
            {
                db.ChairsOrderd.Add(chairsorderd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chairsorderd);
        }

        // GET: /Chairs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChairsOrderd chairsorderd = db.ChairsOrderd.Find(id);
            if (chairsorderd == null)
            {
                return HttpNotFound();
            }
            return View(chairsorderd);
        }

        // POST: /Chairs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ChairsOrderdiD,HallChairID,EventID,CartId")] ChairsOrderd chairsorderd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chairsorderd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chairsorderd);
        }

        // GET: /Chairs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChairsOrderd chairsorderd = db.ChairsOrderd.Find(id);
            if (chairsorderd == null)
            {
                return HttpNotFound();
            }
            return View(chairsorderd);
        }

        // POST: /Chairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChairsOrderd chairsorderd = db.ChairsOrderd.Find(id);
            db.ChairsOrderd.Remove(chairsorderd);
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
