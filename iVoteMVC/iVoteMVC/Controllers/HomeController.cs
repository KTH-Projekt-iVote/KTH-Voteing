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

        public ActionResult JoinVote()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinVote(FormCollection form)
        {
            string pin = form["pin"];
            List<Session> sessions = db.Sessions.Where(s => s.PIN.Equals(pin) && s.published == true).ToList();

            if (sessions.Count == 0)
            {
                @ViewBag.message = "Could not find session with pin: " + pin + ".";
                return RedirectToAction("JoinVote", "Home");
            }

            return RedirectToAction("iVote", "Home", new { pin = pin });
        }

        public ActionResult iVote(string pin, string message)
        {

            if (String.IsNullOrEmpty(pin))
                return RedirectToAction("JoinVote");

            @ViewBag.pin = pin;
            @ViewBag.message = message;

            Student student = null;

            List<Student> students = db.Students.Where(s => s.ip.Equals(Request.UserHostAddress)).ToList();
            if (students.Count() > 0)
            {
                student = students.ElementAt(0);
                if (student.pin == null)
                    student.pin = pin;
            }

            if (student == null)
            {
                student = new Student(pin);
                student.ip = Request.UserHostAddress;
                
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                }
            }

            Session session = student.session;
            if (session == null)
                return RedirectToAction("JoinVote");
            else if (!session.published)
                return RedirectToAction("JoinVote");

            //try
            //{
            //    Session session = student.session;
            //    if (!session.published || session == null)
            //    {
            //        return RedirectToAction("JoinVote");
            //    }
            //}

            if (student.Voted)
            {
                this.HttpContext.Response.AddHeader("refresh", "5");
            }
            
 
            return View(student);
        }

        public PartialViewResult StatsPartial(int id)
        {
            Session session = db.Sessions.Find(id);
            return PartialView("_Stats", session);
        }

        public ActionResult Vote(int vote)
        {

            Student student = db.Students.Where(s => s.ip.Equals(Request.UserHostAddress)).ToList().ElementAt(0);

            String message = "";
            if (student.Voted)
                message = "Already voted.";

            if (vote >= 0 && vote < student.currentQuestion.NoOfAnswers && !student.Voted)
            {
                Answer answer = student.currentQuestion.Answers.ToList().ElementAt(vote);
                answer.voteCount++;

                 if (ModelState.IsValid)
                 {
                     db.Entry(answer).State = EntityState.Modified;
                     db.SaveChanges();

                     //student.Voted = true;
                     db.Entry(student).State = EntityState.Modified;
                     db.SaveChanges();
                 }
                 else
                 {
                     message = "Unable to vote.";
                 }
            }

            return RedirectToAction("iVote", new { pin = student.pin, message = message });
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