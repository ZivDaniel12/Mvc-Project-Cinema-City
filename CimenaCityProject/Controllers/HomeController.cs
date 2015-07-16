using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;
using CimenaCityProject.CustomHtmlHelper;
using CimenaCityProject.ViewModels;

namespace CimenaCityProject.Controllers
{
    public class HomeController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();
        public ActionResult Index()
        {
            var viewMovieQry = new MovieData();

            viewMovieQry.Movie = (from me in db.Movies select me).ToArray();
            viewMovieQry.MovieShowTime = (from mst in db.MovieShowTimes select mst).ToArray();

            return View(viewMovieQry);
//            return View(db.Movies.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Null";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = db.HomeCinemas.ToList();

            return View();
        }
    }
}