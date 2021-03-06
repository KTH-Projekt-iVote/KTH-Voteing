﻿using System;
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
    public class AnswerController : Controller
    {
        private iVoteContext db = new iVoteContext();

        // GET: /Answer/
        public ActionResult Index()
        {
            return View(db.Answers.ToList());
        }

        // GET: /Answer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(db.Sessions.Find(db.Answers.Find(id).QuestionID).TeacherID);
            if (User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");

            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: /Answer/Create
        public ActionResult Create(int id)
        {
            @ViewBag.questionID = id;
            return View();
        }

        // POST: /Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include="ID,voteCount,text")] Answer answer)
        //public ActionResult Create([Bind(Include = "ID,QuestionID,voteCount,text")] Answer answer)
        {
            answer.QuestionID = id;
            answer.voteCount = 0;
            if (ModelState.IsValid)
            {
                db.Answers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Details/"+id, "Question");
               // return RedirectToAction("Details/" + answer.QuestionID, "Question");
            }

            return View(answer);
        }

        // GET: /Answer/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(db.Sessions.Find(db.Answers.Find(id).QuestionID).TeacherID);
            if (!User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");

            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
             
            return View(answer);
        }

        // POST: /Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int QuestionID, [Bind(Include = "ID,voteCount,text")] Answer answer)
        {
            answer.QuestionID = QuestionID;

            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Details/"+answer.QuestionID, "Question");
                return RedirectToAction("Details/" + QuestionID, "Question");
            }
            return View();
        }

        // GET: /Answer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(db.Sessions.Find(db.Answers.Find(id).QuestionID).TeacherID);
            if (!User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");

            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Details/"+answer.QuestionID, "Question");
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
