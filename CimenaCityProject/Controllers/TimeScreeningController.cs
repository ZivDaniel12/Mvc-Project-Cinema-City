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
using CimenaCityProject.Logic;

namespace CimenaCityProject.Controllers
{
    public class TimeScreeningController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /TimeScreening/
        public ActionResult Index(string Error)
        {
            TempData.Clear();
            bool flag = false;
            TempData.Add("flagDisplay", flag);
            TempData.Add("flagMovieName", flag);
            TempData.Add("flagTime", flag);
            TempData.Add("flagHomeCinema", flag);
            TempData.Add("flagTheaters", flag);
            TempData.Add("flagDate", flag);
            TempData.Add("flagPrice", flag);

            TimeScreeningData timeScreening = new TimeScreeningData();

            if (!string.IsNullOrEmpty(Error))
            {
                TempData["msg"] = "<script>alert('" + Error + "');</script>";
            }

            return View(timeScreening);
        }

        // POST: /TimeScreening/
        [HttpPost]
        public ActionResult Index(string searchByMovie, string searchByHomeCinema,string sortingOrder)
        {
            TimeScreeningData timeScreening;

            //search by movie or homeCinema
            if (!string.IsNullOrEmpty(searchByMovie) && !string.IsNullOrEmpty(searchByHomeCinema))
            {
                timeScreening = new TimeScreeningData(null, searchByMovie, searchByHomeCinema);
            }
            else if (!string.IsNullOrEmpty(searchByMovie))
            {
                timeScreening = new TimeScreeningData(null, searchByMovie, null);
            }
            else if (!string.IsNullOrEmpty(searchByHomeCinema))
            {
                timeScreening = new TimeScreeningData(null, null, searchByHomeCinema);
            }
            else
            {
                timeScreening = new TimeScreeningData(null, null, null);
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

        public PartialViewResult CreateOneTimeScreen(string Error)
        {
            if (!string.IsNullOrEmpty(Error))
            {
                ViewBag.Error = Error;
            }
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName");
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "MovieName");
            return PartialView("CreateOneTimeScreen");
        }

        // POST: /TimeScreening/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimeScreeningID,MovieShowTimeID,TheatresID,Date,Price,IsDisplayed")] TimeScreening Timescreening, int MovieTheatersID)
        {
            if ( Timescreening.MovieTheatersID == 0)
            {
                Timescreening.MovieTheatersID = MovieTheatersID;
            }
            MovieShowTime movieshowtime = db.MovieShowTimes.Find(Timescreening.MovieShowTimeID);
            HomeCinema homecinema = db.HomeCinemas.Where(x => x.HomeCinemaID == db.Theaters.Where(y => y.MovieTheatersID == Timescreening.MovieTheatersID).FirstOrDefault().HomeCinemaID).FirstOrDefault();
            MovieTheaters theatres = db.Theaters.Find(MovieTheatersID);
            if (Timescreening.IsDisplayed == true)
            {
                theatres.IsActive = true;
                movieshowtime.IsDisplay = true;
                homecinema.Showing = true;
            }
            
                if (ModelState.IsValid)
                {
                    db.TimeScreening.Add(Timescreening);
                    db.Entry(movieshowtime).State = EntityState.Modified;
                    db.Entry(theatres).State = EntityState.Modified;
                    db.Entry(homecinema).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { Error = "You Cant add a Contract now. try again later. " });
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
            ViewBag.HomeCinemaID = new SelectList(db.HomeCinemas, "HomeCinemaID", "CinemaName",timescreening.MovieTheaters.HomeCinema.CinemaName);
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "MovieName",timescreening.MovieShowTime.Movie.MovieName);
            return View(timescreening);
        }

        // POST: /TimeScreening/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimeScreeningID,MovieShowTimeID,TheatresID,Date,Price,IsDisplayed")] TimeScreening timescreening, int? MovieTheatersID)
        {
            timescreening.MovieTheatersID = MovieTheatersID.Value;


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

       // get the theatres for CreatePage via cinema id 
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

        // get the showtime for CreatePage via movie id
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

        // get the showtime for CreatePage via movie id
        public JsonResult CheckIfContractExist(int TheatresID , int ShowTimeID)
        {
            var data = EcomLogic.CheckIfTheContractExist(TheatresID, ShowTimeID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // getting the table view after sorting
        public PartialViewResult SortingIndexResult(TimeScreeningData timeScreening, string sortingOrder)
        {
            bool flagDisplay = false, flagMovieName = false, flagTime = false, flagHomeCinema = false, flagTheaters = false, flagDate = false, flagPrice = false;

            if (TempData["flagDisplay"] != null ||TempData["flagMovieName"] != null ||TempData["flagTime"] != null ||TempData["flagHomeCinema"] != null ||
                TempData["flagTheaters"] != null ||TempData["flagDate"] != null ||TempData["flagPrice"] != null  )
            {
                 flagDisplay = (bool)TempData["flagDisplay"];
                 flagMovieName = (bool)TempData["flagMovieName"];
                 flagTime = (bool)TempData["flagTime"];
                 flagHomeCinema = (bool)TempData["flagHomeCinema"];
                 flagTheaters = (bool)TempData["flagTheaters"];
                 flagDate = (bool)TempData["flagDate"];
                 flagPrice = (bool)TempData["flagPrice"];
            }

            List<TimeScreening> ts = new List<TimeScreening>();
            switch (sortingOrder)
            {
                case "MovieName":
                    if (flagMovieName == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.MovieShowTime.Movie.MovieName).ToList();
                        flagMovieName = !flagMovieName;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.MovieShowTime.Movie.MovieName).ToList();
                        flagMovieName = !flagMovieName;
                    }
                    break;
                case "Time":
                    if (flagTime == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.MovieShowTime.ShowTime.TimeOfDay).ToList();
                        flagTime = !flagTime;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.MovieShowTime.ShowTime.TimeOfDay).ToList();
                        flagTime = !flagTime;
                    }
                    break;
                case "HomeCinema":
                    if (flagHomeCinema == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.MovieTheaters.HomeCinema.CinemaName).ToList();
                        flagHomeCinema = !flagHomeCinema;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.MovieTheaters.HomeCinema.CinemaName).ToList();
                        flagHomeCinema = !flagHomeCinema;
                    }
                    break;
                case "Theaters":
                    if (flagTheaters == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.MovieTheaters.TheatersName).ToList();
                        flagTheaters = !flagTheaters;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.MovieTheaters.TheatersName).ToList();
                        flagTheaters = !flagTheaters;
                    }
                    break;
                case "Date":
                    if (flagDate == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.Date).ToList();
                        flagDate = !flagDate;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.Date).ToList();
                        flagDate = !flagDate;
                    }
                    break;
                case "Price":
                    if (flagPrice == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.Price).ToList();
                        flagPrice = !flagPrice;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.Price).ToList();
                        flagPrice = !flagPrice;
                    }
                    break;
                case "IsDisplayed":
                    if (flagDisplay == true)
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderByDescending(x => x.IsDisplayed).ToList();
                        flagDisplay = !flagDisplay;
                    }
                    else
                    {
                        timeScreening.TimeScreening = timeScreening.TimeScreening.OrderBy(x => x.IsDisplayed).ToList();
                        flagDisplay = !flagDisplay;

                    }
                    break;
                default:
                    break;
            }

            TempData["flagDisplay"] = flagDisplay;
            TempData["flagMovieName"] = flagMovieName;
            TempData["flagTime"]=flagTime;
            TempData["flagHomeCinema"]= flagHomeCinema;
            TempData["flagTheaters"]=flagTheaters;
            TempData["flagDate"]=flagDate;
            TempData["flagPrice"]= flagPrice;


            return PartialView("SortingIndexResult", timeScreening);
        }

        // GET: /TimeScreening/AskQuestion
        public PartialViewResult AskQuestion()
        {
            return PartialView("AskQuestion");
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
