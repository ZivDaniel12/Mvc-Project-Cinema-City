﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CimenaCityProject.Models;

namespace CimenaCityProject.Admin
{
    public class AdminController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        // GET: /Admin/
        public ActionResult Index()
        {
            return View(db.Persons.ToList());
        }

        // GET: /ControlPanel/
        public ActionResult ControlPanel()
        {
            return View();
        }

        public PartialViewResult GeneralStatistic()
        {
            if (TempData.Keys != null)
            {
                TempData.Remove("list");
            }
            Dictionary<string, decimal> model = new Dictionary<string, decimal>();

            if (db.CheckOut.Count() != 0)
            {

                model.Add("TotalIncome", db.CheckOut.Where(x => x.ISOrderComplete)
                    .Select(x => x.TotalPrice)
                    .Sum());

                model.Add("TotalMovies", db.Movies.Count());
                model.Add("TotalHomeCinemas", db.HomeCinemas.Count());
                model.Add("TotalTheatres", db.Theaters.Count());
                model.Add("TotalOrders", db.Orders.Where(x => x.IsComplete == true).Count());

            }

            TempData.Add("Dictionary", model);
            return PartialView("GeneralStatistic");
        }


        // GET: /Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PersonID,PersonalID,FirstName,LastName,Address,PhoneNumber,BirthDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: /Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: /Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PersonID,PersonalID,FirstName,LastName,Address,PhoneNumber,BirthDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: /Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
