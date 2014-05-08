using iVoteMVC.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iVoteMVC.Models;
using iVoteMVC.DAL;

namespace iVoteMVC.Controllers
{
    public class HomeController : Controller
    
    {
        private iVoteContext db = new iVoteContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(db.Teachers.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult iVote(string pin)
        {

            @ViewBag.pin = pin;
                
            Student student = new Student(pin);
            student.Voted = false;

            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
            }

            return View(student);
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