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

        public ActionResult iVote(int? id, string pin, string message)
        {

            @ViewBag.pin = pin;

            //Student student = db.Students.Find(id);
            Student student = null;

            List<Student> students = db.Students.Where(s => s.ip.Equals(Request.UserHostAddress)).ToList();
            if (students.Count() > 0)
                student = students.ElementAt(0);
            

            if (student == null)
            {
                student = new Student(pin);
                student.Voted = false;
                student.ip = Request.UserHostAddress;
                
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                }
            }
 
            return View(student);
        }

        public ActionResult Vote(int id, int vote, string ip)
        {

            Student student = db.Students.Find(id);

            if(student.Voted)
                return RedirectToAction("iVote", new { id = student.ID, pin = student.pin });

            if (!String.IsNullOrEmpty(student.ip))
                if (!student.ip.Equals(ip))
                    throw new InvalidOperationException("Only one vote allowed.");
  
            if (vote >= 0 && vote < student.currentQuestion.NoOfAnswers && !student.Voted){
                Answer answer = student.currentQuestion.Answers.ToList().ElementAt(vote);
                answer.voteCount++;
                if (ModelState.IsValid)
                {
                    db.Entry(answer).State = EntityState.Modified;
                    db.SaveChanges();

                    student.Voted = true;
                    student.ip = ip;
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("iVote", new { id = student.ID, pin = student.pin });
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