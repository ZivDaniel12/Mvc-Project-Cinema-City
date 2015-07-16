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
    public class MovieShowTimeController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /MovieShowTime/
        public ActionResult Index(int? id, int? MovieShowTimeID)
        {
            var viewMovieQry = new MovieData();

            viewMovieQry.Movie = (from me in db.Movies
                                  orderby me.MovieID 
                                  select me).ToArray();

            viewMovieQry.MovieShowTime = (from mst in db.MovieShowTimes
                                          orderby mst.ShowTime.Hour
                                          select mst).ToArray();

            //select * movieShowTime only movie by the ID 
            if (id != null)
            {
                ViewBag.MovieDataID = id.Value;
                viewMovieQry.Movie = (from me in db.Movies
                                      where me.MovieID == id.Value
                                      select me);

                viewMovieQry.MovieShowTime = (from mst in db.MovieShowTimes
                                              where mst.MovieID == id.Value
                                              orderby mst.ShowTime
                                              select mst).ToArray();
            }

            // get spesific movieShowTime by his ID 
            if (MovieShowTimeID != null)
            {
                ViewBag.movieShowTimeID = MovieShowTimeID.Value;
                viewMovieQry.MovieShowTime = (from me in db.MovieShowTimes
                                      where me.MovieShowTimeID == id.Value
                                      select me);
            }

            return View(viewMovieQry);
        }

        // GET: /MovieShowTime/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var viewMovieQry = new MovieData();

            viewMovieQry.Movie = (from me in db.Movies
                                  where me.MovieID == id
                                  select me).ToArray();
            viewMovieQry.MovieShowTime = (from mst in db.MovieShowTimes
                                          where mst.MovieID == id
                                          select mst).ToArray();


            ViewBag.MovieName = viewMovieQry.Movie.Where(mov => mov.MovieID == id).Single().MovieName;
            // MovieShowTime movieshowtime = db.MovieShowTimes.Find(id);

            if (viewMovieQry.MovieShowTime == null)
            {
                return HttpNotFound();
            }
            return View(viewMovieQry);
        }

        // GET: /MovieShowTime/Create
        public ActionResult Create(int? id)
        {

            //declare the SelectListItem for the dropdown 
            List<SelectListItem> movieListItems = new List<SelectListItem>();

            if (id == null)
            {
                var movieList = (from movie
                            in db.Movies
                                 select movie);

                foreach (var movieListTemp in movieList)
                {
                    //add value'a for the SelectListItem
                    movieListItems.Add(new SelectListItem
                    {
                        Text = movieListTemp.MovieName,
                        Value = movieListTemp.MovieID.ToString()
                    });
                }
            }
            else
            {
                // get all the Movie via ID
                var movieList = (from movie
                           in db.Movies
                                 where movie.MovieID == id
                                 select movie);

                foreach (var movieListTemp in movieList)
                {
                    //add value'a for the SelectListItem
                    movieListItems.Add(new SelectListItem
                    {
                        Text = movieListTemp.MovieName,
                        Value = movieListTemp.MovieID.ToString()
                    });
                }
            }

            //send the data via ViewData key: movieList 
            ViewData.Add("movieList", movieListItems);


            return View();
        }

        // POST: /MovieShowTime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string movieList, List<DateTime> ShowTime, [Bind(Include = "MovieShowTimeID,MovieID,ShowTime")] MovieShowTime movieshowtime)
        {

            if (ShowTime.Count != 0)
            {
                // create list of Movie Show Time     
                List<MovieShowTime> MovieShowTimeList = new List<MovieShowTime>();

                foreach (var item in ShowTime)
                {
                    if (item.Date != Convert.ToDateTime("01/01/0001 00:00:00"))
                    {

                        MovieShowTime ShowTimeItem = new MovieShowTime();
                        // add Detials 
                        ShowTimeItem.MovieID = Convert.ToInt32(movieList);
                        ShowTimeItem.ShowTime = item;

                        // add Show Time Item's for the List 
                        MovieShowTimeList.Add(ShowTimeItem);
                    }
                }

                //Add all the Item's to DB . 
                if (ModelState.IsValid)
                {
                    foreach (var item in MovieShowTimeList)
                    {
                        db.MovieShowTimes.Add(item);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Movie");
                }
                else
                {
                    ViewBag.ErrorMessage = "Unable to add all the Time's";
                    return RedirectToAction("Create", "Movie");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to add Movie Show Time";
            }

            return View(movieshowtime);
        }


        // GET: /MovieShowTime/Edit/5
        public ActionResult Edit(int? id)
        {
            int? movieID;
            string movieName;

            if (TempData.ContainsKey("MovieID"))
            {
                 movieID = (int)TempData["MovieID"];
                 movieName = (from me in db.Movies
                              where me.MovieID == movieID
                              orderby me.MovieID
                              select me.MovieName).SingleOrDefault().ToString();
            }
            else
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                movieName = (from me in db.Movies
                             where me.MovieID == (from mst in db.MovieShowTimes
                                                  where mst.MovieShowTimeID == id.Value 
                                                  select mst.MovieID).FirstOrDefault()
                             orderby me.MovieID
                             select me.MovieName).SingleOrDefault().ToString();
            }

           
           
            ViewBag.MovieName = movieName;

            MovieShowTime movieshowtime = db.MovieShowTimes.Find(id);

            if (movieshowtime == null)
            {
                return HttpNotFound();
            }
            return View(movieshowtime);

        }

        // POST: /MovieShowTime/Edit/5
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Edit(string newShowTimme, [Bind(Include = "MovieShowTimeID,MovieID,ShowTime")] MovieShowTime movieshowtime)
        {

            if (newShowTimme != null || newShowTimme != string.Empty)
            {
                 try
                {
                    movieshowtime.ShowTime = Convert.ToDateTime(newShowTimme);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
              
                } 
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

            if (ModelState.IsValid)
            {
                db.Entry(movieshowtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieshowtime);
        }

        // GET: /MovieShowTime/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieShowTime movieshowtime = db.MovieShowTimes.Find(id);
            if (movieshowtime == null)
            {
                return HttpNotFound();
            }
            return View(movieshowtime);
        }

        // POST: /MovieShowTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieShowTime movieshowtime = db.MovieShowTimes.Find(id);
            db.MovieShowTimes.Remove(movieshowtime);
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
