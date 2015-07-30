using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CimenaCityProject.Models;

namespace CimenaCityProject.Controllers
{
    public class RowsController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /Rows/
        public ActionResult Index()
        {
            var rows = db.Rows.Include(r => r.MovieTheaters);
            return View(rows.ToList());
        }

        // GET: /Rows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rows rows = db.Rows.Find(id);
            if (rows == null)
            {
                return HttpNotFound();
            }
            return View(rows);
        }

        // GET: /Rows/Create
        public PartialViewResult Create(List<int?> id, List<int?> rwcpcty,int? last)
        {
            // check if id null 
            if (id== null)
                ViewBag.TheatersID = new SelectList(db.Theaters, "MovieTheatersID", "TheatersName");
            
            else
                ViewBag.TheatersID = new SelectList(db.Theaters.Where
                                                     (x=>x.MovieTheatersID == id[0]).ToArray(), "MovieTheatersID", "TheatersName");
            // check if row capicity is null. 
            if (rwcpcty == null)
            {
                rwcpcty = new List<int?>();
                rwcpcty.Add(1);
                TempData.Add("RowAmount", rwcpcty);
                ViewBag.last = 1;
            }
            else
            {
                TempData.Add("RowAmount", rwcpcty);
                ViewBag.last = last;
            }
            ViewBag.Message = "";
            return PartialView();
        }

        // POST: /Rows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RowsID,TheatersID,RowNumber,ChairCapacity")] Rows rows,int? last,int? rowCapacity)
        {
                int check = 0;
                if (rows.ChairCapacity == 0 )
                {
                     ViewBag.TheatersID = new SelectList(db.Theaters, "MovieTheatersID", "TheatersName", rows.TheatersID);
                     ViewBag.Message = "you must enter a capacity.";
                     return View(rows);
                }

                if (ModelState.IsValid)
                {
                    db.Rows.Add(rows);
                    check = db.SaveChanges();
                }

                if(check == 1)
                {
                    rows = db.Rows.Where(r => r.TheatersID == rows.TheatersID && r.RowNumber == rows.RowNumber).SingleOrDefault();
                    for (int i = 0; i < rows.ChairCapacity; i++)
                    {
                        HallChairs chair = new HallChairs();
                        chair.ChairNumber = i++;
                        chair.IsSelected = false;
                        chair.RowID = rows.RowsID;

                        if (ModelState.IsValid)
                        {
                            db.HallChairs.Add(chair);
                            db.SaveChanges();
                        }
                    }
                    if (last == rowCapacity)
                    {
                        return RedirectToAction("Index", "TimeScreening", new { id = rows.TheatersID });
                    }
                    else
                    {
                        last++;
                        return RedirectToAction("Create", new { id = rows.TheatersID, rwcpcty = rowCapacity, last = last });
                        //View();
                    }
                }

            ViewBag.Message = "Error by adding the Row's. try again. ";
            ViewBag.TheatersID = new SelectList(db.Theaters, "MovieTheatersID", "TheatersName", rows.TheatersID);
            return View(rows);
        }

        // GET: /Rows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rows rows = db.Rows.Find(id);
            if (rows == null)
            {
                return HttpNotFound();
            }
            ViewBag.TheatersID = new SelectList(db.Theaters, "MovieTheatersID", "TheatersName", rows.TheatersID);
            return View(rows);
        }

        // POST: /Rows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RowsID,TheatersID,RowNumber,ChairCapacity")] Rows rows)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rows).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TheatersID = new SelectList(db.Theaters, "MovieTheatersID", "TheatersName", rows.TheatersID);
            return View(rows);
        }

        // GET: /Rows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rows rows = db.Rows.Find(id);
            if (rows == null)
            {
                return HttpNotFound();
            }
            return View(rows);
        }

        // POST: /Rows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rows rows = db.Rows.Find(id);
            db.Rows.Remove(rows);
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
