using System;
using System.Linq;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();


        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree);
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        private decimal CalculateQuote(Insuree insuree)
        {
            decimal baseQuote = 50m;


            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;


            if (age <= 18)
            {
                baseQuote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                baseQuote += 50;
            }
            else if (age >= 26)
            {
                baseQuote += 25;
            }


            if (insuree.CarYear < 2000)
            {
                baseQuote += 25;
            }
            else if (insuree.CarYear > 2015)
            {
                baseQuote += 25;
            }


            if (insuree.CarMake.ToLower() == "porsche")
            {
                baseQuote += 25;
                if (insuree.CarModel.ToLower() == "911 carrera")
                {
                    baseQuote += 25;
                }
            }


            baseQuote += insuree.SpeedingTickets * 10;


            if (insuree.DUI)
            {
                baseQuote *= 1.25m;
            }


            if (insuree.CoverageType)
            {
                baseQuote *= 1.50m;
            }

            return baseQuote;
        }


        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }
    }
}
