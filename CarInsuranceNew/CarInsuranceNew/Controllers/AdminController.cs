using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsuranceNew.Models;

namespace CarInsuranceNew.Controllers
{
    public class AdminController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }
        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress," +
            "DateOfBirh,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            // extract YYYY from Datetime
            DateTime today = DateTime.Now;


            //Check if minor
            var MinorDate = today.AddYears(-18);

            //determine YoungerDriver lower and upper dates (ages of 19 to 25)
            var YoungDriverLower = today.AddYears(-25);
            var YoungDriverUpper = today.AddYears(-19);





            if (ModelState.IsValid)
            {
                //set Base Quote = 50 + 10 * number of speeding tickets
                insuree.Quote = 50m + insuree.SpeedingTickets * 10;

                //add 100 to base quote if user is 18 yo or younger
                if (insuree.DateOfBirh >= MinorDate)
                {
                    insuree.Quote += 100;
                }
                //add 50 if user is between 19 and 25
                if (insuree.DateOfBirh >= YoungDriverLower && insuree.DateOfBirh <= YoungDriverUpper)
                {
                    insuree.Quote += 50;
                }
                //add 25 if user is over 25
                if (insuree.DateOfBirh < YoungDriverLower)
                {
                    insuree.Quote += 25;
                }

                //if car year older than 2000 or newer than 2015, add 25 to quote
                if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
                {
                    insuree.Quote += 25;
                }
                //if the car make is a Porsche add 25 to quote
                if (insuree.CarMake == "Porsche")
                {
                    insuree.Quote += 25;
                }
                if (insuree.CarMake == "Porsche" && insuree.CarModel == "911 Carrera")
                {
                    insuree.Quote += 25;
                }


                //add 25% to the base quote for DUI
                if (insuree.DUI == true)
                {
                    insuree.Quote += insuree.Quote / 4;
                }

                //add 50% to the base quote for full coverage
                if (insuree.CoverageType == true)
                {
                    insuree.Quote += insuree.Quote / 2;
                }


                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress," +
            "DateOfBirh,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                //recompute quote
                DateTime today = DateTime.Now;


                //Check if minor
                var MinorDate = today.AddYears(-18);

                //determine YoungerDriver lower and upper dates (ages of 19 to 25)
                var YoungDriverLower = today.AddYears(-25);
                var YoungDriverUpper = today.AddYears(-19);

                //set Base Quote = 50 + 10 * number of speeding tickets
                insuree.Quote = 50m + insuree.SpeedingTickets * 10;

                //add 100 to base quote if user is 18 yo or younger
                if (insuree.DateOfBirh >= MinorDate)
                {
                    insuree.Quote += 100;
                }
                //add 50 if user is between 19 and 25
                if (insuree.DateOfBirh >= YoungDriverLower && insuree.DateOfBirh <= YoungDriverUpper)
                {
                    insuree.Quote += 50;
                }
                //add 25 if user is over 25
                if (insuree.DateOfBirh < YoungDriverLower)
                {
                    insuree.Quote += 25;
                }

                //if car year older than 2000 or newer than 2015, add 25 to quote
                if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
                {
                    insuree.Quote += 25;
                }
                //if the car make is a Porsche add 25 to quote
                if (insuree.CarMake == "Porsche")
                {
                    insuree.Quote += 25;
                }
                if (insuree.CarMake == "Porsche" && insuree.CarModel == "911 Carrera")
                {
                    insuree.Quote += 25;
                }


                //add 25% to the base quote for DUI
                if (insuree.DUI == true)
                {
                    insuree.Quote += insuree.Quote / 4;
                }

                //add 50% to the base quote for full coverage
                if (insuree.CoverageType == true)
                {
                    insuree.Quote += insuree.Quote / 2;
                }
                db.Entry(insuree).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
