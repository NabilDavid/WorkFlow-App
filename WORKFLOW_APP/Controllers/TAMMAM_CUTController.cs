using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OracleClient;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WORKFLOW_APP.Models;
using WORKFLOW_APP.Views.Shared;
using Newtonsoft.Json;

namespace WORKFLOW_APP.Controllers
{
    public class TAMMAM_CUTController : Controller
    {

        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;
        string title = "";
        string type = "success";
       // private WF_EN db = new WF_EN();

        //
        // GET: /TAMMAM_CUT/

        public ActionResult Index()
        {
            var firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Include(f => f.PERSON_DATA);
            return View("index");
        }

        //
        // GET: /TAMMAM_CUT/Details/5

        public ActionResult Details(string id = null)
        {
            FIRMS_ABSENCES_PERSONS firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Find(id);
            if (firms_absences_persons == null)
            {
                return HttpNotFound();
            }
            return View(firms_absences_persons);
        }

        //
        // GET: /TAMMAM_CUT/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return View();
        }

        //
        // POST: /TAMMAM_CUT/Create

        [HttpPost]
        public ActionResult Create(FIRMS_ABSENCES_PERSONS firms_absences_persons)
        {
            if (ModelState.IsValid)
            {
                db.FIRMS_ABSENCES_PERSONS.Add(firms_absences_persons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firms_absences_persons.PERSON_CODE);
            return View(firms_absences_persons);
        }

        //
        // GET: /TAMMAM_CUT/Edit/5

        public ActionResult Edit(string id = null)
        {
            FIRMS_ABSENCES_PERSONS firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Find(id);
            if (firms_absences_persons == null)
            {
                return HttpNotFound();
            }
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firms_absences_persons.PERSON_CODE);
            return View(firms_absences_persons);
        }

        //
        // POST: /TAMMAM_CUT/Edit/5

        [HttpPost]
        public ActionResult Edit(FIRMS_ABSENCES_PERSONS firms_absences_persons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(firms_absences_persons).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firms_absences_persons.PERSON_CODE);
            return View(firms_absences_persons);
        }

        //
        // GET: /TAMMAM_CUT/Delete/5

        public ActionResult Delete(string id = null)
        {
            FIRMS_ABSENCES_PERSONS firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Find(id);
            if (firms_absences_persons == null)
            {
                return HttpNotFound();
            }
            return View(firms_absences_persons);
        }

        //
        // POST: /TAMMAM_CUT/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            FIRMS_ABSENCES_PERSONS firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Find(id);
            db.FIRMS_ABSENCES_PERSONS.Remove(firms_absences_persons);
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