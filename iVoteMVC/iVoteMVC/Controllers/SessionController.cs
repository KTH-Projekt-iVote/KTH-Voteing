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
using Microsoft.AspNet.Identity;
namespace iVoteMVC.Controllers
{
    public class SessionController : Controller
    {
        private iVoteContext db = new iVoteContext();

        // GET: /Session/
        public ActionResult Index()
        {
            string username = User.Identity.GetUserName();
            List<Teacher> teachers = db.Teachers.Where(t => t.username.Equals(username)).ToList();
            List<Session> sessions = new List<Session>();

            if (teachers.Count != 0)
            {
                sessions = db.Sessions.Where(s => s.TeacherID == teachers.ElementAt(0).ID).ToList();
            }
            
            return View(sessions);
        }

        // GET: /Session/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            Teacher teacher = db.Teachers.Find(session.TeacherID);
            if (session == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserName().Equals(teacher.name))
            {
                return View(session);
            }
            else
                return RedirectToAction("index", "home");
        }

        // GET: /Session/Create
        public ActionResult Create(int id)
        {
            @ViewBag.teacherID = id;
            return View();
        }

        // POST: /Session/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include="ID,name,description,published")] Session session)
        {
            session.TeacherID = id;
            session.dateCreated = System.DateTime.Now;
            session.dateModifed = System.DateTime.Now;

            if (session.published)
            {
                string pin = "";
                while(!unique(pin)){
                    Random random = new Random();
                    pin = "";
                    for (int i = 0; i < 4; i++)
                        pin += random.Next(0, 10);

                    session.PIN = pin;
                }
            }

            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Details/" + session.TeacherID, "Teacher");
            }

            return View(session);
        }

        private bool unique(string pin)
        {

            if (String.IsNullOrEmpty(pin))
                return false;

            var pinQuery = from s in db.Sessions
                           select s.PIN;

            List<string> pins = pinQuery.ToList();
            
            foreach(string s in pins){
                if (s.Equals(pin))
                    return false;
            }

            return true;
        }

        // GET: /Session/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            Teacher teacher = db.Teachers.Find(session.TeacherID);
            if (session == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserName().Equals(teacher.name))
            {
                return View(session);
            }
            else
                return RedirectToAction("index", "home");
        }

        // POST: /Session/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int teacherID, [Bind(Include="ID,TeacherID,name,description,published,PIN")] Session session)
        {

            session.TeacherID = teacherID;
            session.dateModifed = System.DateTime.Now;
            session.dateCreated = System.DateTime.Now;

            if (session.published && !String.IsNullOrEmpty(session.PIN))
                throw new InvalidOperationException("PIN : " + session.PIN);

            if (session.published && String.IsNullOrEmpty(session.PIN))
            {
                Random random = new Random();
                string pin = "";
                for (int i = 0; i < 4; i++)
                    pin += "" + random.Next(0, 10);
                session.PIN = pin;
            }

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + session.TeacherID, "Teacher");
            }
            return View(session);
        }

        // GET: /Session/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            Teacher teacher = db.Teachers.Find(session.TeacherID);
            if (session == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserName().Equals(teacher.name))
            {
                return View(session);
            }
            else
                return RedirectToAction("index", "home");
        }

        // POST: /Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Details/" + session.TeacherID, "Teacher");
        }


        public ActionResult VoteControll(int id)
        {
            Session session = db.Sessions.Find(id);
            
            session.published = true;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            //this.HttpContext.Response.AddHeader("refresh", "7");

            return View(session);
        }
        
        public PartialViewResult StatsPartial(int id)
        {
            Session session = db.Sessions.Find(id);
            return PartialView("_Stats", session);
        }

        public ActionResult FinishSession(int id)
        {
            List<Student> students = db.Students.ToList();

            if (students.Count > 0)
            {
                foreach (Student s in students)
                {
                    if (s.session.ID == id)
                    {
                        db.Students.Remove(s);
                        //db.SaveChanges();
                    }
                }
                db.SaveChanges();
            }

            Session session = db.Sessions.Find(id);
            session.published = false;
            session.PIN = null;
            session.CurrentQuestionIndex = 0;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("VoteControll", new { id = id });
        }

        public ActionResult NextQuestion(int id)
        {
            Session session = db.Sessions.Find(id);
            
            if(session.CurrentQuestionIndex < session.NoOfQuestions-1)
                session.CurrentQuestionIndex++;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("VoteControll", new { id = id });

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
