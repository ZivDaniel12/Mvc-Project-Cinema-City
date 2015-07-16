﻿using System;
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
    public class TimeScreeningController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /TimeScreening/
        public ActionResult Index()
        {
            TimeScreeningData timeScreening = new TimeScreeningData();

            return View(timeScreening);
        }

        // POST: /TimeScreening/
        [HttpPost]
        public ActionResult Index(string searchByMovie, string searchByHomeCinema)
        {
            TimeScreeningData timeScreening;
            if (!string.IsNullOrEmpty(searchByMovie) && !string.IsNullOrEmpty(searchByHomeCinema))
            {
                timeScreening = new TimeScreeningData(searchByMovie, searchByHomeCinema);
            }
            else if (!string.IsNullOrEmpty(searchByMovie))
            {
                 timeScreening = new TimeScreeningData(searchByMovie, null);
            }
            else if (!string.IsNullOrEmpty(searchByHomeCinema))
            {
                timeScreening = new TimeScreeningData(null, searchByHomeCinema);
            }
            else
            {
                timeScreening = new TimeScreeningData(null, null);
            }
            return View(timeScreening);
        }
        
        // GET: /TimeScreening/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeScreening timescreening = db.TimeScreening.Find(id);
            if (timescreening == null)
            {
                return HttpNotFound();
            }
            return View(timescreening);
        }

        // GET: /TimeScreening/AskQuestion
        public PartialViewResult AskQuestion()
        {
            return PartialView("AskQuestion");
        }

        // GET: /TimeScreening/Create
        public ActionResult Create(int? number)
        {
            // i need to edit the create for i can add a multiplay timeScteening 
            ViewBag.number = number;
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName");
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "MovieName");
            return View();
        }

        // GET: /TimeScreening/Create

        public PartialViewResult CreateOneTimeScreen()
        {
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName");
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "MovieName");
            return PartialView("CreateOneTimeScreen");
        }

        // POST: /TimeScreening/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimeScreeningID,MovieShowTimeID,TheatresID,Date,Price,IsDisplayed")] TimeScreening Timescreening)
        {

            MovieShowTime movieshowtime = db.MovieShowTimes.Find(Timescreening.MovieShowTimeID);
            HomeCinema homecinema = db.HomeCinemas.Where(x => x.HomeCinemaID == db.Theaters.Where(y=>y.MovieTheatersID == Timescreening.TheatresID).FirstOrDefault().HomeCinemaID).FirstOrDefault();
            homecinema.Showing = true;
            movieshowtime.IsDisplay = true;
            
                if (ModelState.IsValid)
                {
                    db.TimeScreening.Add(Timescreening);
                    db.Entry(movieshowtime).State = EntityState.Modified;
                    db.Entry(homecinema).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               return View("Create", Timescreening);
        }

        // GET: /TimeScreening/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeScreening timescreening = db.TimeScreening.Find(id);
            if (timescreening == null)
            {
                return HttpNotFound();
            }
            return View(timescreening);
        }

        // POST: /TimeScreening/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TimeScreeningID,MovieShowTimeID,TheatresID,Date,Price,IsDisplayed")] TimeScreening timescreening)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timescreening).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timescreening);
        }

        // GET: /TimeScreening/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeScreening timescreening = db.TimeScreening.Find(id);
            if (timescreening == null)
            {
                return HttpNotFound();
            }
            return View(timescreening);
        }

        // POST: /TimeScreening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeScreening timescreening = db.TimeScreening.Find(id);
            db.TimeScreening.Remove(timescreening);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //auto complete for movie name 
        public JsonResult GetMovieName(string term)
        {
            List<string> movieName;
            movieName = db.Movies.Where(x => x.MovieName.Contains(term))
                .Select(y => y.MovieName).ToList();

            return Json(movieName, JsonRequestBehavior.AllowGet);
        }
        //auto complete for Cinema
        public JsonResult GetHomeCinemaName(string term)
        {
            List<string> HomeCinemaName;
            HomeCinemaName = db.HomeCinemas.Where(x => x.CinemaName.Contains(term))
                .Select(y => y.CinemaName).ToList();

            return Json(HomeCinemaName, JsonRequestBehavior.AllowGet);
        }

    
        public ActionResult GetTheatres(int? CinemaID)
        {
            var data = (from d in db.Theaters
                        where d.HomeCinemaID == CinemaID
                        select new
                        {
                            id = d.MovieTheatersID,
                            name = d.TheatersName
                        }).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShowTime(int? MovieID)
        {

            var data = (from m in db.MovieShowTimes
                        where m.MovieID == MovieID
                        select new
                        {
                            id = m.MovieShowTimeID,
                            name = m.ShowTime
                        }).ToList();
                
            return Json(data, JsonRequestBehavior.AllowGet);
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