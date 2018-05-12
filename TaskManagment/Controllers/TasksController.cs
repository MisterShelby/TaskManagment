using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagment.Models;

namespace TaskManagment.Controllers
{
    public class TasksController : Controller
    {
        private TaskDBEntities db = new TaskDBEntities();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.User);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.PriorityID = new SelectList(db.Priorities, "PriorityID", "PriorityText");
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusText");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Tasks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,ProjectID,UserID,PriorityID,StatusID,TaskDescription,DateCreated,DateEnd")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.TaskID = Guid.NewGuid();
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PriorityID = new SelectList(db.Priorities, "PriorityID", "PriorityText", task.PriorityID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusText", task.StatusID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", task.UserID);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.PriorityID = new SelectList(db.Priorities, "PriorityID", "PriorityText", task.PriorityID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusText", task.StatusID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", task.UserID);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,ProjectID,UserID,PriorityID,StatusID,TaskDescription,DateCreated,DateEnd")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriorityID = new SelectList(db.Priorities, "PriorityID", "PriorityText", task.PriorityID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "StatusText", task.StatusID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", task.UserID);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
