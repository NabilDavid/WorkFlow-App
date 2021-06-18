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
    public class HolidaysController : Controller
    {
        private WF_EN db = new WF_EN();
        int flagCreate = 0;
        int flagedit = 0;
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Holidays/Create
        public ActionResult Create()
        {
            flagCreate = 1;
            ViewBag.flagCreate = flagCreate;
            return PartialView("_PartialHoliday");
        }

        public ActionResult Edit(string FIRM_CODE,short H_YEAR, DateTime H_FROM_DATE, DateTime H_TO_DATE, string H_DESC)
        {
            flagedit = 1;
            ViewBag.flagedit = flagedit;

            var p = db.FIRM_HOLIDAYS.First(o => o.FIRM_CODE == FIRM_CODE && o.H_YEAR == H_YEAR && o.H_FROM_DATE == H_FROM_DATE && o.H_TO_DATE == H_TO_DATE);

            ViewBag.FIRM_CODE = p.FIRM_CODE;
            ViewBag.H_YEAR = p.H_YEAR;
            ViewBag.H_FROM_DATE = p.H_FROM_DATE;
            ViewBag.H_TO_DATE = p.H_TO_DATE;
            ViewBag.H_DESC = p.H_DESC;


             FIRM_HOLIDAYS  F_H = db.FIRM_HOLIDAYS.First(o => o.FIRM_CODE == FIRM_CODE && o.H_YEAR == H_YEAR && o.H_FROM_DATE == H_FROM_DATE && o.H_TO_DATE == H_TO_DATE);
           
            if (F_H == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsUpdate = true;
            return PartialView("_PartialHoliday", F_H);
        }

        
          
        
        public JsonResult getHoliday(string FIRM)
        {
           // var p = db.PERSON_DATA.First(o => o.FIRM_CODE == FIRM);
            string query =
                   @" select to_char(H_FROM_DATE,'yyyy/MM/dd') as H_FROM_DATE , to_char(H_TO_DATE,'yyyy/MM/dd') as H_TO_DATE ,H_DESC ,FIRM_CODE , H_YEAR
                     from FIRM_HOLIDAYS
                     where FIRM_CODE =" + FIRM + "order by h_year desc";
            OracleCommand cmd = new OracleCommand(query);
           var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getHolidaySearch(string FIRM, string year)
        {
            string  query =
                      @" select to_char(H_FROM_DATE,'yyyy/MM/dd') as H_FROM_DATE , to_char(H_TO_DATE,'yyyy/MM/dd') as H_TO_DATE ,H_DESC ,FIRM_CODE , H_YEAR
                     from FIRM_HOLIDAYS
                     where FIRM_CODE =" + FIRM + "  AND  H_YEAR like '%" + year + "%' order by h_year desc";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
       
        public int checkIfHolidayFound( DateTime fromDate, string FIRM_CODE, short year )
        {
            int flag = 0;
//           

            var holiday = db.FIRM_HOLIDAYS.Where(o => o.H_FROM_DATE == fromDate  && o.FIRM_CODE == FIRM_CODE && o.H_YEAR == year).FirstOrDefault();

            if (holiday != null)
            {
                flag = 1;
            }
            else {
                flag = 0;
            }
            return flag;

        }
        public int checkIfNameFound(string FIRM_CODE, short year, string vacationName)
        {
            int flag = 0;
            //           

            var holiday = db.FIRM_HOLIDAYS.Where(o => o.H_DESC == vacationName && o.FIRM_CODE == FIRM_CODE && o.H_YEAR == year).FirstOrDefault();

            if (holiday != null)
            {
                flag = 1;
            }
            else
            {
                flag = 0;
            }
            return flag;

        }
        public JsonResult getfirm(string FIRM)
        {

            string query =
                   @" select FIRM_CODE,NAME 
                      from firms
                      where firm_code= '" + FIRM + "'";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getYear(string FIRM,String year)
        {

            string query =
                   @" select DISTINCT H_YEAR
                      from FIRM_HOLIDAYS
                      where firm_code= '" + FIRM + "' ORDER BY H_YEAR DESC";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
         
        [HttpPost]
        public int createVacationHoliday(string vacationName, DateTime fromDate, DateTime toDate, string FIRM_CODE, short year)
        {
            int flag = 0;
            int status = 0;
            db.FIRM_HOLIDAYS.Add(new FIRM_HOLIDAYS()
            {
            
            H_DESC =vacationName,
            H_FROM_DATE=fromDate,
            H_TO_DATE=toDate,
            FIRM_CODE = FIRM_CODE,
            H_YEAR = year
            
            });
             flag = db.SaveChanges();


            if(flag == 0)
            {
                status = 0;
               
            }
            else
            {
               status = 1;
               
            }

            return status ;
            
        }


        public int editVacationHoliday(string vacationName, DateTime fromDate, DateTime toDate, short year, string FIRM_CODE, string newVacationName, DateTime newFromDate, DateTime newToDate, short newYear)
        {
            var db = new WF_EN();
            int flag = 0;
            int status = 0;

            var holiday = db.FIRM_HOLIDAYS.Where(o => o.H_FROM_DATE == fromDate && o.FIRM_CODE == FIRM_CODE && o.H_YEAR==year).FirstOrDefault();
            if (holiday != null)
            {
                holiday.H_DESC = newVacationName;
                //holiday.H_FROM_DATE = newFromDate;
                holiday.H_TO_DATE = newToDate;
                //holiday.H_YEAR = newYear;
            }
            db.Entry(holiday).State = System.Data.EntityState.Modified;
            flag = db.SaveChanges();
            if (flag == 0)
            {
                status = 0;
            }
            else {
            status = 1;
            }
            return status;
        }


        public ActionResult Delete(DateTime H_FROM_DATE, DateTime H_TO_DATE, string FIRM_CODE, short H_YEAR)
        {
            string message = "";
            bool status = false;
            string title = "";
            string type = "";
            var det = db.FIRM_HOLIDAYS.First(o => o.FIRM_CODE == FIRM_CODE && o.H_FROM_DATE == H_FROM_DATE && o.H_TO_DATE == H_TO_DATE && o.H_YEAR == H_YEAR);
            db.FIRM_HOLIDAYS.Remove(det);
            if (db.SaveChanges() > 0)
            {
                message = "تم الحذف بنجاح";
                status = true;
                title = "تم الحذف";
                type = "success";
            }
            else
            {
                message = "لم يتم الحذف بنجاح ";
                status = false;
                title = " لم تم الحذف";
                type = "error";
            }
            return new JsonResult { Data = new { status = status, message = message, title = title, type = type } };

        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}