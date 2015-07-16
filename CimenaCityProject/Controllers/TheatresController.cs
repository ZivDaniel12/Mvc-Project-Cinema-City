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
    public class TheatresController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /Theatres/
        public ActionResult Index()
        {
            var theaters = db.Theaters.Include(m => m.HomeCinema);
            return View(theaters.ToList());
        }

        public PartialViewResult Theatres(int? id)
        {
            ViewBag.CinemaName = db.HomeCinemas.Find(id).CinemaName.ToString();
            return PartialView("Theatres");
        }

        // GET: /Theatres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieTheaters movietheaters = db.Theaters.Find(id);
            if (movietheaters == null)
            {
                return HttpNotFound();
            }
            return View(movietheaters);
        }

        // GET: /Theatres/Create
        public ActionResult Create(int? id, int? number)
        {
            if (id == null)
            {
                ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName");
                number = 1;
                ViewBag.number = number;
            }
            else
            {
                ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas.Where(x => x.HomeCinemaID == id).ToArray(), "HomeCinemaID", "CinemaName");
                ViewBag.number = number;
            }
            ViewBag.ErrorMassage = "";
            return View();
        }

        // POST: /Theatres/Create
        [HttpPost]
        public ActionResult Create( List<MovieTheaters> NewTheaters,List<int> ChairCapacity)
        {        
            if (NewTheaters == null || ChairCapacity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // flags for check succsss db
            bool checkTheatres = false;
            bool checkRows = false;
            bool checkChairs = false;
            int HomeCinemaID = NewTheaters[0].HomeCinemaID;

            for (int i = 0; i < NewTheaters.Count; i++)
            {
                MovieTheaters movietheaters = NewTheaters[i];
                if (db.Theaters.Where(x => x.TheatersName == movietheaters.TheatersName && x.HomeCinemaID == movietheaters.HomeCinemaID).FirstOrDefault() == movietheaters)
                {
                    ViewBag.ErrorMassage = "You cant add this Theatres again. Choose another name.";
                    ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas.Where(x => x.HomeCinemaID == movietheaters.HomeCinemaID).ToArray(), "HomeCinemaID", "CinemaName", movietheaters.HomeCinemaID);
                    return View(movietheaters);
                }
                else
                {
                    // adding the Theatres for Getting the ID. 
                    if (ModelState.IsValid)
                    {
                        db.Theaters.Add(movietheaters);
                        db.SaveChanges();
                        checkTheatres = true;
                    }
                    else
                        checkTheatres = false;

                    for (int f = 0; f < movietheaters.RowAmount; f++)
                    {
                        // adding new row ditels
                        var row = new Rows();
                        row.ChairCapacity = ChairCapacity[(i)];
                        row.RowNumber = (f + 1);
                        row.TheatersID = movietheaters.MovieTheatersID;

                        // add row to db to get the RowID 
                        if (ModelState.IsValid)
                        {
                            db.Rows.Add(row);
                            db.SaveChanges();
                            checkRows = true;
                        }
                        else
                            checkRows = false;

                        var Chairs = new List<HallChairs>();
                        for (int j = 0; j < row.ChairCapacity; j++)
                        {
                            var chair = new HallChairs();
                            chair.ChairNumber = (j + 1);
                            chair.IsSelected = false;
                            chair.RowID = row.RowsID;
                            Chairs.Add(chair);
                        }

                        // add chair to db. 
                        if (ModelState.IsValid)
                        {
                            db.HallChairs.AddRange(Chairs);
                            db.SaveChanges();
                            checkChairs = true;
                        }
                        else
                            checkChairs = false;
                    }
                }
            }

            if (checkChairs &&  checkTheatres && checkRows)
            {
                return RedirectToAction("Index", "TimeScreening", new { id = HomeCinemaID });
            }
            else
            {
                ViewBag.ErrorMassage = "Try again later";
                ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName");
                return View();
            }
        }

        // GET: /Theatres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieTheaters movietheaters = db.Theaters.Find(id);
            if (movietheaters == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName", movietheaters.HomeCinemaID);
            return View(movietheaters);
        }

        // POST: /Theatres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieTheatersID,HomeCinemaID,TheatersName,NumberHall,RowAmount")] MovieTheaters movietheaters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movietheaters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName", movietheaters.HomeCinemaID);
            return View(movietheaters);
        }

        // GET: /Theatres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieTheaters movietheaters = db.Theaters.Find(id);
            if (movietheaters == null)
            {
                return HttpNotFound();
            }
            return View(movietheaters);
        }

        // POST: /Theatres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieTheaters movietheaters = db.Theaters.Find(id);
            db.Theaters.Remove(movietheaters);
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
