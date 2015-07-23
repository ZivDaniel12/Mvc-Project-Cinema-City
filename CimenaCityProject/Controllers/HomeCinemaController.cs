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
    public class HomeCinemaController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /HomeCinema/
        public ActionResult Index(string sortingOrder)
        {
            var result = new List<HomeCinema>();
            if (!string.IsNullOrEmpty(sortingOrder))
            {
                switch (sortingOrder)
                {
                    case "CinemaName":
                        result = db.HomeCinemas.OrderBy(x => x.CinemaName).ToList();
                        break;
                    case "City":
                        result = db.HomeCinemas.OrderBy(x => x.CityID).ToList();
                        break;
                    case "Address":
                        result = db.HomeCinemas.OrderBy(x => x.Address).ToList();
                        break;
                    case "PhoneNumber":
                        result = db.HomeCinemas.OrderBy(x => x.PhoneNumber).ToList();
                        break;
                    case "Showing":
                        result = db.HomeCinemas.OrderByDescending(x => x.Showing).ToList();
                        break;
                    default:
                        result = db.HomeCinemas.ToList();
                        break;
                }
            }
            else
            {
                result = db.HomeCinemas.ToList();
            }
            return View(result);
        }

        // GET: /HomeCinema/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeCinema homecinema = db.HomeCinemas.Find(id);
            if (homecinema == null)
            {
                return HttpNotFound();
            }
            return View(homecinema);
        }

        // GET: /HomeCinema/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");
            ViewBag.ErrorMassage = "";
            return View();
        }


        // POST: /HomeCinema/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HomeCinemaID,CinemaName,City,Address,PhoneNumber")]HomeCinema homeCinema,int? number, int? CityID)
        {
           
            if (!CityID.HasValue)
            {
                ViewBag.ErrorMassage = "You must Select a City. ";
                ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");
                return View(homeCinema);
            }
            homeCinema.CityID = CityID.Value;

            // Check if the homeCinema is alredy enterd. 
            foreach (var Cinema in db.HomeCinemas)
            {
                if (Cinema.CityID == homeCinema.CityID && Cinema.CinemaName == homeCinema.CinemaName)
                {
                    ViewBag.ErrorMassage = "You cant add this Theatres again. Choose another name.";
                    ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");

                    return View();
                }
            }

            if (ModelState.IsValid)
            {
                db.HomeCinemas.Add(homeCinema);
                db.SaveChanges();

                homeCinema = db.HomeCinemas.Where(x => x.CinemaName == homeCinema.CinemaName).FirstOrDefault();
                return RedirectToAction("Create", "Theatres", new { id = homeCinema.HomeCinemaID , number = number});
            }

            ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");
            return View(homeCinema);
        }

        // GET: /HomeCinema/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            HomeCinema homecinema = db.HomeCinemas.Find(id);
            if (homecinema == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.CityList, "CityID", "EnglishName",homecinema.CityID);
            return View(homecinema);
        }

        // POST: /HomeCinema/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="HomeCinemaID,CinemaName,City,Address,PhoneNumber")] HomeCinema homecinema, int? CityID)
        {
            if (!CityID.HasValue)
            {
                ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");
                return View(homecinema);
            }
            homecinema.CityID = CityID.Value;
            if (ModelState.IsValid)
            {
                db.Entry(homecinema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.CityList.ToArray(), "CityID", "EnglishName");
            return View(homecinema);
        }

        // GET: /HomeCinema/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HomeCinema homecinema = db.HomeCinemas.Find(id);
            if (homecinema == null)
            {
                return HttpNotFound();
            }
            return View(homecinema);
        }

        // POST: /HomeCinema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HomeCinema homecinema = db.HomeCinemas.Find(id);
            db.HomeCinemas.Remove(homecinema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Error(string ErrorMessge)
        {
            TempData.Add("error", ErrorMessge);
            return PartialView();
        }

        private HomeCinemaDetails homeCinemaDetails(string _switch, int? id)
        {
            return new HomeCinemaDetails(_switch, id);
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
