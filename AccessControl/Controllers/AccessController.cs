using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessControl.Models;

namespace AccessControl.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccessController : Controller
    {
        private readonly DbZeusEntities _db = new DbZeusEntities();

        // GET: Access
        public ActionResult Index()
        {
            return View(_db.Access.ToList());
        }
        

        // GET: Access/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Access/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Password,Active,Profile,Name,LastName")] Access access)
        {

            var exist = _db.Access.Any(x => x.Email == access.Email);

            if (exist)
            {
                ModelState.AddModelError("Email", "This email has already been registered.");
                return View(access);
            }
            else if (ModelState.IsValid)
            {
                access.Password = Access.Encrypt(access.Password);
                _db.Access.Add(access);

                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(access);
        }

        // GET: Access/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Access access = _db.Access.Find(id);
            if (access == null)
                return HttpNotFound();

            return View(access);
        }

        // POST: Access/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password,Active,Profile,Name,LastName")] Access access)
        {
            var exist = _db.Access.Any(x => x.Id != access.Id && x.Email == access.Email);

            if (exist)
            {
                ModelState.AddModelError("Email", "This email has already been registered.");
                return View(access);
            }
            else if (ModelState.IsValid)
            {
                var acc = _db.Access.Find(access.Id);

                if (!acc.ValidatePassword(access.Password))
                {
                    acc.Password = Access.Encrypt(access.Password);

                }

                acc.Name = access.Name;
                acc.Name = access.LastName;
                acc.Name = access.Email;
                acc.Name = access.Profile;
                
                _db.Access.AddOrUpdate(acc);
                //_db.Entry(access).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(access);
        }

        // GET: Access/Delete/5
        public ActionResult Delete([Bind(Include = "Email,Id,Password,Active,Profile,Name,LastName")] Access access)
        {
            if (access == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Access accessDb = _db.Access.Find(access.Id);
            if (accessDb == null)
            {
                return HttpNotFound();
            }
            return View(access);
        }

        // POST: Access/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Access access = _db.Access.Find(id);
            _db.Access.Remove(access);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
