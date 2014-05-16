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
    [Authorize]
    public class SessionController : Controller
    {
        private iVoteContext db = new iVoteContext();

        private Teacher findTeacher()
        {
            string username = User.Identity.GetUserName();
            List<Teacher> teachers = db.Teachers.Where(t => t.username.Equals(username)).ToList();

            if (teachers.Count > 0)
            {
                return teachers.ElementAt(0);
            }

            return null;
        }

        // GET: /Session/
        public ActionResult Index(string searchString, string sortOrder)
        {

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.DateModSortParm = sortOrder == "DateMod" ? "dateMod_desc" : "DateMod";


            Teacher teacher = findTeacher();
            List<Session> sessions = new List<Session>();

            if (teacher != null)
            {
                sessions = db.Sessions.Where(s => s.TeacherID == teacher.ID).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                sessions = sessions.Where(s => s.name.Contains(searchString) || s.description.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sessions = sessions.OrderByDescending(s => s.name).ToList();
                    break;
                case "Date":
                    sessions = sessions.OrderBy(s => s.dateCreated).ToList();
                    break;
                case "date_desc":
                    sessions = sessions.OrderByDescending(s => s.dateCreated).ToList();
                    break;
                case "dateMod":
                    sessions = sessions.OrderBy(s => s.dateModifed).ToList();
                    break;
                case "dateMod_desc":
                    sessions = sessions.OrderByDescending(s => s.dateModifed).ToList();
                    break;
                default:
                    sessions = sessions.OrderBy(s => s.name).ToList();
                    break;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Session/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,name,description")] Session session)
        {
               
            session.TeacherID = findTeacher().ID;
            session.dateCreated = System.DateTime.Now;
            session.dateModifed = System.DateTime.Now;

            session.published = false;

            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = session.ID });
            }

            return View(session);
        }

        private string GeneratePIN()
        {

            string pin = "";
            Random random = new Random();

            while (!unique(pin))
            {
                pin = "";
                for (int i = 0; i < 4; i++)
                    pin += random.Next(0, 10);
            }

            return pin;
        }

        private bool unique(string pin)
        {

            if (pin.Equals("") || pin == null)
                return false;

            if (String.IsNullOrEmpty(pin))
                return false;

            var pinQuery = from s in db.Sessions
                           select s.PIN;

            List<Session> sessions = db.Sessions.Where(s => s.PIN.Equals(pin)).ToList();

            if (sessions.Count > 0)
            {
                return false;
            }

            return true;

            //throw new NotImplementedException("count : " + sessions.Count + "\nPIN : " + pin);

            //if (sessions.Count > 0 && sessions != null)
            //    foreach (string s in pins)
            //    {
            //        if (s.Equals(pin))
            //            return false;
            //    }
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
                return RedirectToAction("Login", "Account");
        }

        // POST: /Session/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int teacherID, [Bind(Include="ID,TeacherID,name,description,published,PIN")] Session session)
        {


            session.TeacherID = findTeacher().ID;
            session.dateModifed = System.DateTime.Now;
            session.dateCreated = System.DateTime.Now;

            if (session.published && !String.IsNullOrEmpty(session.PIN))
                throw new InvalidOperationException("PIN : " + session.PIN);

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {id = session.ID });
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
                return RedirectToAction("Login", "Account");
        }

        // POST: /Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult VoteControll(int id)
        {
            Session session = db.Sessions.Find(id);

            if (findTeacher().ID != session.TeacherID)
                return RedirectToAction("Login", "Account");

            session.published = true;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            //this.HttpContext.Response.AddHeader("refresh", "7");

            return View(session);
        }
        
        public PartialViewResult StatsPartial(int QuestionID)
        {
            //Session session = db.Sessions.Find(id);
            Question question = db.Questions.Find(QuestionID);
            return PartialView("_Stats", question);
        }

        public ActionResult FinishSession(int id)
        {
            Session session = db.Sessions.Find(id);

            if (findTeacher().ID != session.TeacherID)
                return RedirectToAction("Login", "Account");

            List<Student> students = db.Students.ToList();

            if (students.Count > 0 && students != null)
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

            int index = session.CurrentQuestionIndex;

            session.published = false;
            session.PIN = null;
            session.CurrentQuestionIndex = 0;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            //clearNcopy(id);

            return RedirectToAction("Result", new { id = id, index = index });
        }

        public ActionResult Clear(int id)
        {
            Session session = db.Sessions.Find(id);
            
            if(session.Questions != null)
            foreach (Question q in session.Questions)
            {
                if(q.Answers != null)
                foreach (Answer a in q.Answers)
                {
                    a.voteCount = 0;
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult NextQuestion(int id)
        {

            Session session = db.Sessions.Find(id);
       
            if (findTeacher().ID != session.TeacherID)
                return RedirectToAction("Login", "Account");
   
            if(session.CurrentQuestionIndex < session.NoOfQuestions-1)
                session.CurrentQuestionIndex++;

            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }

            List<Student> students = db.Students.ToList();

            if (students.Count > 0 && students != null)
            {
                foreach (Student s in students)
                {
                    if (s.session.ID == id)
                    {
                        s.Voted = false;
                        //db.SaveChanges();
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("VoteControll", new { id = id });

        }

        public ActionResult PublishSession(int id)
        {

            Session session = db.Sessions.Find(id);

            if (findTeacher().ID != session.TeacherID)
                return RedirectToAction("Login", "Account");

            if (session != null)
            {
                session.published = true;
                session.PIN = GeneratePIN();
                session.CurrentQuestionIndex = 0;

                if (ModelState.IsValid)
                {
                    db.Entry(session).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("VoteControll", new { id = session.ID });
            }

            return RedirectToAction("Details", new { id = session.ID });
        }

        public ActionResult Result(int id, int index)
        {
            @ViewBag.index = index;
            Session session = db.Sessions.Find(id);

            if (findTeacher().ID != session.TeacherID)
                return RedirectToAction("Login", "Account");

            return View(session);
        }

        public void clearNcopy(int id)
        {
            Session session = db.Sessions.Find(id);
            Session sessionCopy = new Session();

            sessionCopy.TeacherID = session.TeacherID;
            sessionCopy.name = session.name + " - " + DateTime.Now.ToString();
            sessionCopy.CurrentQuestionIndex = session.CurrentQuestionIndex;
            sessionCopy.dateCreated = session.dateCreated;
            sessionCopy.dateModifed = session.dateModifed;
            sessionCopy.description = session.description;
            sessionCopy.PIN = null;
            sessionCopy.published = false;

            sessionCopy.Questions = new List<Question>(session.Questions);

            foreach (Question q in session.Questions)
            {
                foreach (Answer a in q.Answers)
                {
                    a.voteCount = 0;
                  db.Entry(a).State = EntityState.Modified;
                  db.SaveChanges();
                }
            }

      


            if (ModelState.IsValid)
            {
                db.Sessions.Add(sessionCopy);
                db.SaveChanges();

                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
            }
            
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
