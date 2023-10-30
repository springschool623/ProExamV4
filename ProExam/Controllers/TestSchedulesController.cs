using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProExam.Models;

namespace ProExam.Controllers
{
    public class TestSchedulesController : Controller
    {
        private ProExamDBEntities7 db = new ProExamDBEntities7();

        // GET: TestSchedules
        public ActionResult Index()
        {
            var testSchedules = db.TestSchedules.Include(t => t.Subject);
            return View(testSchedules.ToList());
        }

        // GET: TestSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = db.TestSchedules.Find(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            return View(testSchedule);
        }

        // GET: TestSchedules/Create
        public ActionResult Create()
        {
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Subject_Name");
            return View();
        }

        // POST: TestSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TestSchedule_ID,Subject_ID,Testing_Date,Begining_Time,Testroom,StuTest_Quantity")] TestSchedule testSchedule)
        {
            if (ModelState.IsValid)
            {
                // Lấy giá trị lớn nhất của TestSchedule_ID trong database
                int maxTestId = db.TestSchedules.Max(ts => (int?)ts.TestSchedule_ID) ?? 0;

                // Tăng giá trị lên 1
                testSchedule.TestSchedule_ID = maxTestId + 1;

                // Thêm mới vào database
                db.TestSchedules.Add(testSchedule);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Subject_Name", testSchedule.Subject_ID);
            return View(testSchedule);
        }

        // GET: TestSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = db.TestSchedules.Find(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Subject_Name", testSchedule.Subject_ID);
            return View(testSchedule);
        }

        // POST: TestSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TestSchedule_ID,Subject_ID,Testing_Date,Begining_Time,Testroom,StuTest_Quantity")] TestSchedule testSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "Subject_Name", testSchedule.Subject_ID);
            return View(testSchedule);
        }

        // GET: TestSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = db.TestSchedules.Find(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            return View(testSchedule);
        }

        // POST: TestSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestSchedule testSchedule = db.TestSchedules.Find(id);
            db.TestSchedules.Remove(testSchedule);
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
