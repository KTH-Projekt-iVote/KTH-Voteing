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
    public class TeacherController : Controller
    {
       //private iVoteContext db = new iVoteContext();

       private iVoteContext db = new iVoteContext();
       

        public ActionResult Index()
        {

            if (!User.Identity.IsAuthenticated)
                RedirectToAction("Login", "Account");

            string name = User.Identity.GetUserName();
            return View(db.Teachers.Where(t => t.username.Equals(name)));

        }

        // GET: /Teacher/Details/5
        public ActionResult Details(int? id, string currentFilter, string searchTerm, string sortOrder)
        {

            
            @ViewBag.currentSort = sortOrder;
            @ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            @ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            @ViewBag.sortOrder = sortOrder;
            @ViewBag.currentFilter = searchTerm;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!String.IsNullOrEmpty(searchTerm))
                @ViewBag.searchTerm = searchTerm;
            else
                @ViewBag.searchTerm = "";

            Teacher teacher = db.Teachers.Find(id);

            if (teacher == null)
            {
                return HttpNotFound();
            }

                if (User.Identity.GetUserName().Equals(teacher.name))
                {
            return View(teacher);
        }
                else
                    return RedirectToAction("index","home");

        }

        // GET: /Teacher/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: /Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // public ActionResult Create([Bind(Include="ID,name,username,password,email")] Teacher teacher)
        public ActionResult Create(Teacher teacher)
        {

            Console.WriteLine();
            int teachCount;
            teachCount = db.Teachers.Count();
            teacher.ID = teachCount + 1;
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }
 
        // GET: /Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserName().Equals(teacher.name))
            {
            return View(teacher);
        }
            else
                return RedirectToAction("index", "home");
        }

        // POST: /Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,name,username,password,email")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: /Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserName().Equals(teacher.name))
            {
            return View(teacher);
        }
            else
                return RedirectToAction("index", "home");
        }

        // POST: /Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
