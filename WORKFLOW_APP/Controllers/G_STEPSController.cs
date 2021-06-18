using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WORKFLOW_APP.Models;
using WORKFLOW_APP.Views.Shared;

namespace WORKFLOW_APP.Controllers
{
    public class G_STEPSController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /G_STEPS/

        public ActionResult Index()
        {
            var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            ViewBag.OFF_ABS_GROUP_ID = new SelectList(db.OFF_ABS_GROUP, "OFF_ABS_GROUP_ID", "OFF_ABS_GROUP_NAME");
            foreach (var item in ViewBag.OFF_ABS_GROUP_ID)
            {
                var x = item;
            }
            return View(off_abs_steps.ToList());
        }

        public ActionResult GET_GRP()
        {
            var off_abs_steps = db.OFF_ABS_GROUP.Where(o => o.ACTIV_F == 1);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT OFF_ABS_GROUP_ID, OFF_ABS_GROUP_NAME FROM OFF_ABS_GROUP WHERE ACTIV_F = 1";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_STEPS(string GR)
        {
            var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT OFF_ABS_STEPS_ID,
                           STP.OFF_ABS_GROUP_ID,
                           OFF_ABS_STEPS_NAME,
                           STP.OFF_ROLE_ARCHIVE,
                           ORDER_ID,
                           GR.OFF_ABS_GROUP_NAME,
                           AR.ARH_ROLE_NAME
                      FROM OFF_ABS_STEPS STP, OFF_ABS_GROUP GR, OFF_ROLE_ARCHIVE AR
                     WHERE STP.OFF_ABS_GROUP_ID = GR.OFF_ABS_GROUP_ID
                           AND STP.OFF_ROLE_ARCHIVE = AR.ARH_ROLE_CODE
                           AND STP.OFF_ABS_GROUP_ID = " + GR;
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /G_STEPS/Details/5

        public ActionResult Details(decimal id = 0)
        {
            OFF_ABS_STEPS off_abs_steps = db.OFF_ABS_STEPS.Find(id);
            if (off_abs_steps == null)
            {
                return HttpNotFound();
            }
            return View(off_abs_steps);
        }

        //
        // GET: /G_STEPS/Create

        public ActionResult Create()
        {
          //  ViewBag.OFF_ABS_GROUP_ID = new SelectList(db.OFF_ABS_GROUP, "OFF_ABS_GROUP_ID", "OFF_ABS_GROUP_NAME");
            //return View();

            return PartialView("Create");
        }

        //
        // POST: /G_STEPS/Create

        [HttpPost]
        public ActionResult Create(OFF_ABS_STEPS off_abs_steps)
        {
            if (ModelState.IsValid)
            {
                db.OFF_ABS_STEPS.Add(off_abs_steps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OFF_ABS_GROUP_ID = new SelectList(db.OFF_ABS_GROUP, "OFF_ABS_GROUP_ID", "OFF_ABS_GROUP_NAME", off_abs_steps.OFF_ABS_GROUP_ID);
            return View(off_abs_steps);
        }

        //
        // GET: /G_STEPS/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            OFF_ABS_STEPS off_abs_steps = db.OFF_ABS_STEPS.Find(id);
            if (off_abs_steps == null)
            {
                return HttpNotFound();
            }
            ViewBag.OFF_ABS_GROUP_ID = new SelectList(db.OFF_ABS_GROUP, "OFF_ABS_GROUP_ID", "OFF_ABS_GROUP_NAME", off_abs_steps.OFF_ABS_GROUP_ID);
            return View(off_abs_steps);
        }

        //
        // POST: /G_STEPS/Edit/5

        [HttpPost]
        public ActionResult Edit(OFF_ABS_STEPS off_abs_steps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(off_abs_steps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OFF_ABS_GROUP_ID = new SelectList(db.OFF_ABS_GROUP, "OFF_ABS_GROUP_ID", "OFF_ABS_GROUP_NAME", off_abs_steps.OFF_ABS_GROUP_ID);
            return View(off_abs_steps);
        }

        //
        // GET: /G_STEPS/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            OFF_ABS_STEPS off_abs_steps = db.OFF_ABS_STEPS.Find(id);
            if (off_abs_steps == null)
            {
                return HttpNotFound();
            }
            return View(off_abs_steps);
        }

        //
        // POST: /G_STEPS/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            OFF_ABS_STEPS off_abs_steps = db.OFF_ABS_STEPS.Find(id);
            db.OFF_ABS_STEPS.Remove(off_abs_steps);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}