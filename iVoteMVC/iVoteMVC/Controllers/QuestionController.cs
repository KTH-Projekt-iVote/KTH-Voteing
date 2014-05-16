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
    public class QuestionController : Controller
    {
        private iVoteContext db = new iVoteContext();

        // GET: /Question/
        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        // GET: /Question/Details/5
        public ActionResult Details(int? id)
        {
            Question question = db.Questions.Find(id);
            Session session = db.Sessions.Find(question.SessionID);
            Teacher teacher = db.Teachers.Find(session.TeacherID);
          //  Teacher teacher = db.Teachers.Find(db.Sessions.Find(id).TeacherID);
            if (!User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: /Question/Create
        public ActionResult Create(int SessionID)
        {
            return View();
        }

        // POST: /Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int SessionID, [Bind(Include="text, voteCount")] Question question)
        {
            question.SessionID = SessionID;
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Details/" + question.SessionID, "Session");
            }

            return View(question);
        }

        // GET: /Question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(db.Sessions.Find(id).TeacherID);
            if (!User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");

            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int SessionID, [Bind(Include="ID,text")] Question question)
        {
            question.SessionID = SessionID;

            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + question.SessionID, "Session");
            }
            return View(question);
        }

        // GET: /Question/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(db.Sessions.Find(id).TeacherID);
            if (!User.Identity.GetUserName().Equals(teacher.username))
                return RedirectToAction("Login", "Account");

            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Details/" + question.SessionID, "Session");
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
