using iVoteMVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iVoteMVC.DAL;
using iVoteMVC.Models;

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

            //if (ModelState.IsValid)
            //{
            //    db.Students.Add(student);
            //    db.SaveChanges();
            //}

            return View(student);
        }
    }
}