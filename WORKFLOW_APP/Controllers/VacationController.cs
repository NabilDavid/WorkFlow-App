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
    public class VacationController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;
        string title = "";
        string type = "success";
        //
        // GET: /Vacation/

        public ActionResult Index()
        {
            //var person_vacations = db.PERSON_VACATIONS.Include(p => p.FIRMS).Include(p => p.PERSON_DATA).Include(p => p.PERSON_DATA1).Include(p => p.PERSON_DATA2).Include(p => p.PERSON_DATA3).Include(p => p.PERSON_DATA4).Include(p => p.PERSON_DATA5).Include(p => p.RANKS).Include(p => p.RANKS1).Include(p => p.VACATION_TYPES);
            //var person_vacations = from o in db.PERSON_VACATIONS
            //                           where o.PERSON_CODE == id
            //                           select o;
            //var rnk =
            //ViewBag.pers_nm = db.PERSON_DATA.First(o => o.PERSON_CODE == id).RANKS.RANK + " / " + db.PERSON_DATA.First(o => o.PERSON_CODE == id).PERSON_NAME;
            return View();
        }

        //
        // GET: /Vacation/Details/5

        public ActionResult Details(short id = 0)
        {
            PERSON_VACATIONS person_vacations = db.PERSON_VACATIONS.Find(id);
            if (person_vacations == null)
            {
                return HttpNotFound();
            }
            return View(person_vacations);
        }

        //
        // GET: /Vacation/Create

        public ActionResult Create()
        {
            ViewBag.SD = false;
            var pers = Session["pers"].ToString();
            var firm = Session["firm"].ToString();
            var p = db.PERSON_DATA.First(o => o.FIRM_CODE == firm && o.PERSON_CODE == pers);
            ViewBag.pnm = p.PERSON_NAME;
            ViewBag.rnm = p.RANKS.RANK;
            ViewBag.addr = p.ADDERESS;
            ViewBag.pid = p.PERSONAL_ID_NO;
            ViewBag.pers_c = p.PERSON_CODE + "," + p.RANK_ID + "," + p.RANK_CAT_ID + "," + p.PERSON_CAT_ID + "," + p.RANKS.RANK + "," + p.PERSON_NAME + "," + p.ADDERESS + "," + p.PERSONAL_ID_NO;
            return PartialView("_vac_det");
        }

        //
        // POST: /Vacation/Create

        [HttpPost]
        public ActionResult Create(PERSON_VACATIONS person_vacations)
        {
            message = "الإضافة";
            status = false;
            var d1 = person_vacations.FROM_DATE;
            var d2 = person_vacations.TO_DATE.Value.AddHours(23);
            if (d1 > DateTime.Now.Date)
            {
                if (!db.PERSON_VACATIONS.Any(o => o.FROM_DATE >= d1 && o.TO_DATE <= d2 && o.PERSON_CODE == person_vacations.PERSON_CODE))
                {
                    var seq = (short)(db.PERSON_VACATIONS.Where(o => o.FIRM_CODE == person_vacations.FIRM_CODE).Max(o => o.SEQ) + 1);
                    db.PERSON_VACATIONS.Add(new PERSON_VACATIONS()
                    {
                        SEQ = seq,
                        FIRM_CODE = person_vacations.FIRM_CODE,
                        PERSON_CODE = person_vacations.PERSON_CODE,
                        RANK_ID = person_vacations.RANK_ID,
                        RANK_CAT_ID = person_vacations.RANK_CAT_ID,
                        PERSON_CAT_ID = person_vacations.PERSON_CAT_ID,
                        VACATION_TYPE_ID = person_vacations.VACATION_TYPE_ID,
                        REQUEST_DATE = DateTime.Now,
                        FROM_DATE = person_vacations.FROM_DATE,
                        TO_DATE = d2,
                        ACTUAL_START = person_vacations.ACTUAL_START,
                        ACTUAL_END = d2,
                        ADDRESS = person_vacations.ADDRESS
                    });

                    db.SaveChanges();
                    var pg = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == person_vacations.PERSON_CODE && o.ABS_CAT_ID == 2) ?
                        db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == person_vacations.PERSON_CODE && o.ABS_CAT_ID == 2).OFF_ABS_GROUP_ID :
                        db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 2).OFF_ABS_GROUP_ID;
                    var steps = from o in db.OFF_ABS_STEPS
                                where o.OFF_ABS_GROUP_ID == pg
                                select o;
                    foreach (var s in steps)
                    {
                        var det = new PERSON_VACATIONS_DET();
                        var max = db.PERSON_VACATIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                        det.PERSON_VACATIONS_DET_ID = max;
                        det.PERSON_DATE_ID = person_vacations.PERSON_CODE;
                        det.PERSON_VACATIONS_SEQ = seq;
                        det.OFF_ABS_GROUP_ID = pg;
                        det.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                        db.PERSON_VACATIONS_DET.Add(det);
                        db.SaveChanges();

                        
                    }
                    string xx = "";
                    var pers = db.PERSON_DATA.First(o => o.PERSON_CODE ==  person_vacations.PERSON_CODE);
                    xx = pers.PERSONAL_ID_NO;
                    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + pers.RANKS.RANK + " / " + pers.PERSON_NAME + "  بطلب الأجازة', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/SD_VAC?nn=" + xx + " ')";
                    General.exec_q(Q, "");
                    General.exec_q("COMMIT", "");
                    status = true;

                    
                }
                else
                {
                    status = false;
                    message = "يوجد أجازة مسجلة أثناء هذه الفترة";
                }
            }
            else
            {
                status = false;
                message = "لا يمكن تسجيل أجازة بتاريخ سابق";
            }
            //message = "Successfully Saved.";
            return new JsonResult { Data = new { status = status, message = message } };
        }

        //
        // GET: /Vacation/Edit/5

        public ActionResult Edit(short id , string firm, string pers)
        {
            ViewBag.SD = false;
            var p = db.PERSON_DATA.First(o => o.FIRM_CODE == firm && o.PERSON_CODE == pers);
            ViewBag.pnm = p.PERSON_NAME;
            ViewBag.rnm = p.RANKS.RANK;
            ViewBag.addr = p.ADDERESS;
            ViewBag.pid = p.PERSONAL_ID_NO;
            ViewBag.pers_c = p.PERSON_CODE + "," + p.RANK_ID + "," + p.RANK_CAT_ID + "," + p.PERSON_CAT_ID + "," + p.RANKS.RANK + "," + p.PERSON_NAME + "," + p.ADDERESS + "," + p.PERSONAL_ID_NO;
            PERSON_VACATIONS vac = db.PERSON_VACATIONS.First(o=>o.FIRM_CODE == firm && o.SEQ == id && o.PERSON_CODE == pers);
            ViewBag.SEQ = vac.SEQ;
            if (vac == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = true;
            return PartialView("_vac_det", vac);
        }

        //
        // POST: /Vacation/Edit/5

        [HttpPost]
        public ActionResult Edit(PERSON_VACATIONS person_vacations)
        {
            status = true;
            message = "التعديل";
            var d1 = person_vacations.FROM_DATE;
            var d2 = person_vacations.TO_DATE;
            if (d1 > DateTime.Now.Date)
            {
                if (!db.PERSON_VACATIONS.Any(o => o.FROM_DATE >= d1 && o.TO_DATE <= d2 && o.SEQ != person_vacations.SEQ && o.PERSON_CODE == person_vacations.PERSON_CODE))
                {
                    if (ModelState.IsValid)
                    {
                       
                        db.Entry(person_vacations).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    status = false;
                    message = "يوجد أجازة مسجلة أثناء هذه الفترة";
                }
            }
            else
            {
                status = false;
                message = "لا يمكن تعديل أجازة بتاريخ سابق";
            }
            return new JsonResult { Data = new { status = status, message = message } };
        }

        //
        // GET: /Vacation/Delete/5

        public ActionResult Delete(short id = 0)
        {
            PERSON_VACATIONS person_vacations = db.PERSON_VACATIONS.Find(id);
            if (person_vacations == null)
            {
                return HttpNotFound();
            }
            return View(person_vacations);
        }

        //
        // POST: /Vacation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id , string firm, string pers)
        {
            PERSON_VACATIONS v = db.PERSON_VACATIONS.First(o=>o.SEQ == id && o.FIRM_CODE == firm && o.PERSON_CODE == pers);
            if (v.FROM_DATE >= DateTime.Now.Date)
            {
                if (!db.PERSON_VACATIONS_DET.Any(o=>o.PERSON_VACATIONS_SEQ == v.SEQ && o.PERSON_DATE_ID == v.PERSON_CODE && o.DECTION != null))
                {
                    var pg = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == v.PERSON_CODE && o.ABS_CAT_ID == 2) ?
                        db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == v.PERSON_CODE && o.ABS_CAT_ID == 2).OFF_ABS_GROUP_ID :
                        db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 2).OFF_ABS_GROUP_ID;
                    var steps = from o in db.PERSON_VACATIONS_DET
                                where o.PERSON_VACATIONS_SEQ == v.SEQ
                                select o;
                    foreach (var s in steps)
                    {
                        var det = db.PERSON_VACATIONS_DET.First(o => o.PERSON_VACATIONS_SEQ == s.PERSON_VACATIONS_SEQ && o.PERSON_DATE_ID == s.PERSON_DATE_ID && o.PERSON_VACATIONS_DET_ID == s.PERSON_VACATIONS_DET_ID);
                        db.PERSON_VACATIONS_DET.Remove(det);
                        db.SaveChanges();
                    }
                    db.PERSON_VACATIONS.Remove(v);
                    db.SaveChanges(); 
                    message = "تم الحذف بنجاح";
                    status = true;
                    title = "تم الحذف";
                    type = "success";
                }
                else
                {
                    message = "لا يمكن حذف أجازة بعد التصديق عليها";
                    status = false;
                    title = "خطأ";
                    type = "error";
                }
                
            }
            else
            {
                message = "لا يمكن حذف أجازة منتهية";
                status = false;
                title = "خطأ";
                type = "error";
            }
            
            return new JsonResult { Data = new { status = status, message = message , title = title, type = type} };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult get_vac(string DFRM, string DTO, string ID, string FIRM)
        {
            var p = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == ID && o.FIRM_CODE == FIRM);
            Session.Add("firm", p.FIRM_CODE);
            Session.Add("pers", p.PERSON_CODE);
            var q = @"SELECT PERSON_VACATIONS.SEQ,
                                    PERSON_VACATIONS.FIRM_CODE,
                                    PERSON_VACATIONS.PERSON_CODE,
                                    PERSON_VACATIONS.RANK_ID,
                                    PERSON_VACATIONS.RANK_CAT_ID,
                                    PERSON_VACATIONS.PERSON_CAT_ID,
                                    PERSON_VACATIONS.VACATION_TYPE_ID,
                                    to_char(  PERSON_VACATIONS.REQUEST_DATE,'yyyy/mm/dd')REQUEST_DATE,
                                    to_char(  PERSON_VACATIONS.FROM_DATE,'yyyy/mm/dd')FROM_DATE,
                                    to_char(  PERSON_VACATIONS.TO_DATE,'yyyy/mm/dd')TO_DATE,
                                    PERSON_VACATIONS.OTHER_PERSON_CODE,
                                    (SELECT PERSON_DATA.PERSON_NAME FROM PERSON_DATA WHERE PERSON_DATA.PERSON_CODE= PERSON_VACATIONS.OTHER_PERSON_CODE)OTHER_PERSON_NAME,
                                    PERSON_VACATIONS.OTHER_PER_DECS,
                                    PERSON_VACATIONS.RAN_RANK_ID,
                                    RANKS.RANK RAN_RANK_NAME,
                                    PERSON_VACATIONS.RAN_RANK_CAT_ID,
                                    PERSON_VACATIONS.RAN_PERSON_CAT_ID,
                                    PERSON_VACATIONS.SUPERVISOR_CODE,
                                    PERSON_DATA.PERSON_NAME  SUPERVISOR_NAME,
                                    PERSON_DATA.PERSONAL_ID_NO  ,
                                    PERSON_VACATIONS.SUPERVISOR_NOTES,
                                    PERSON_VACATIONS.SUPERVISOR_DECESION,
                                    PERSON_VACATIONS.PLANNING_CODE,
                                    PERSON_VACATIONS.PLANNING_NOTES,
                                    PERSON_VACATIONS.PLANNING_DECESION,
                                    PERSON_VACATIONS.VICE_COMMAND_CODE,
                                    PERSON_VACATIONS.VICE_COMMAND_NOTES,
                                    PERSON_VACATIONS.VICE_COMMAND_DECESION,
                                    PERSON_VACATIONS.COMANDER_CODE,
                                    PERSON_VACATIONS.COMANDER_NOTES,
                                    PERSON_VACATIONS.COMANDER_DECESION,
                                    to_char(  PERSON_VACATIONS.ACTUAL_START,'yyyy/mm/dd')ACTUAL_START,
                                    to_char(  PERSON_VACATIONS.ACTUAL_END,'yyyy/mm/dd')ACTUAL_END,
                                    PERSON_VACATIONS.APPROVED_BY,
                                    PERSON_VACATIONS.APPROVAL_NO,
                                    to_char(PERSON_VACATIONS.APPROVAL_DATE,'yyyy/mm/dd') APPROVAL_DATE,
                                    PERSON_VACATIONS.ADDRESS,
                                    to_char(PERSON_VACATIONS.EXCHANGE_FOR_DATE,'yyyy/mm/dd') EXCHANGE_FOR_DATE,
                                    PERSON_VACATIONS.FLAG_PLAN,
                                    VACATION_TYPES.NAME as VACATION_TYPE_ID_NAME
                            FROM   PERSON_VACATIONS,VACATION_TYPES,PERSON_DATA,RANKS
                            WHERE ( person_vacations.firm_code = '" + FIRM + @"') AND  
                                    ( PERSON_DATA.PERSONAL_ID_NO = '" + ID + @"') AND
                                    (VACATION_TYPES.VACATION_TYPE_ID=PERSON_VACATIONS.VACATION_TYPE_ID) AND
                                    PERSON_DATA.PERSON_CODE=PERSON_VACATIONS.PERSON_CODE AND
                                    PERSON_VACATIONS.firm_code = PERSON_DATA.FIRM_CODE AND  
                                    RANKS.RANK_ID=PERSON_VACATIONS.RANK_ID AND
                                    person_vacations.from_date >= to_date('" + DFRM + "','dd/mm/yyyy') and  PERSON_VACATIONS.TO_DATE<=to_date('" + DTO + @"','dd/mm/yyyy')
                            ORDER BY PERSON_VACATIONS.FROM_DATE DESC";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_vac_his(string PERS, string FIRM)
        {
            var q = @"SELECT NVL (H1.CNT, 0) AS CNT1,
                                NVL (H2.CNT, 0) AS CNT2,
                                TOT.NAME,
                                NVL ( (7 - H1.CNT), 7) AS CNT3,
                                NVL ( (7 - (H2.CNT + H1.CNT)), (7 - H1.CNT)) AS CNT4
                            FROM (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '30/06/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H1,
                                (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/07/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H2,
                                (SELECT DISTINCT (TYP.NAME) AS NAME
                                    FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                    WHERE     PERSON_CODE = '" + PERS + @"'
                                        AND FIRM_CODE = " + FIRM + @"
                                        AND COMANDER_DECESION = 1
                                        AND TYP.VACATION_TYPE_ID = 21
                                        AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                        AND ACTUAL_START >=
                                                TO_DATE ('01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')
                                        AND ACTUAL_END <=
                                                TO_DATE ('31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')) TOT
                            WHERE TOT.NAME = H2.NAME(+) AND TOT.NAME = H1.NAME(+)
                        UNION
                        SELECT NVL (H1.CNT, 0) AS CNT1,
                                NVL (H2.CNT, 0) AS CNT2,
                                TOT.NAME,
                                NVL ( (15 - H1.CNT), 15) AS CNT3,
                                NVL ( (15 - H2.CNT), 15) AS CNT4
                            FROM (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '30/06/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H1,
                                (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/07/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H2,
                                (SELECT DISTINCT (TYP.NAME) AS NAME
                                    FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                    WHERE     PERSON_CODE = '" + PERS + @"'
                                        AND FIRM_CODE = " + FIRM + @"
                                        AND COMANDER_DECESION = 1
                                        AND TYP.VACATION_TYPE_ID = 23
                                        AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                        AND ACTUAL_START >=
                                                TO_DATE ('01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')
                                        AND ACTUAL_END <=
                                                TO_DATE ('31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')) TOT
                            WHERE TOT.NAME = H2.NAME(+) AND TOT.NAME = H1.NAME(+)
                        UNION
                        SELECT NVL (H1.CNT, 0) AS CNT1,
                                NVL (H2.CNT, 0) AS CNT2,
                                TOT.NAME,
                                NVL (0, 0) AS CNT3,
                                NVL (0, 0) AS CNT4
                            FROM (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '30/06/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H1,
                                (  SELECT SUM (V_CNT) AS CNT, NAME
                                    FROM (SELECT (VAC.ACTUAL_END - VAC.ACTUAL_START) + 1 AS V_CNT,
                                                    TYP.NAME,
                                                    SEQ
                                            FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                            WHERE     PERSON_CODE = '" + PERS + @"'
                                                    AND FIRM_CODE = " + FIRM + @"
                                                    AND COMANDER_DECESION = 1
                                                    AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                                    AND ACTUAL_START >=
                                                        TO_DATE (
                                                            '01/07/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR')
                                                    AND ACTUAL_END <=
                                                        TO_DATE (
                                                            '31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                            'DD/MM/RRRR'))
                                GROUP BY NAME) H2,
                                (SELECT DISTINCT (TYP.NAME) AS NAME
                                    FROM PERSON_VACATIONS VAC, VACATION_TYPES TYP
                                    WHERE     PERSON_CODE = '" + PERS + @"'
                                        AND FIRM_CODE = " + FIRM + @"
                                        AND COMANDER_DECESION = 1
                                        AND TYP.VACATION_TYPE_ID NOT IN (23, 21)
                                        AND TYP.VACATION_TYPE_ID = VAC.VACATION_TYPE_ID
                                        AND ACTUAL_START >=
                                                TO_DATE ('01/01/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')
                                        AND ACTUAL_END <=
                                                TO_DATE ('31/12/' || EXTRACT (YEAR FROM SYSDATE),
                                                        'DD/MM/RRRR')) TOT
                            WHERE TOT.NAME = H2.NAME(+) AND TOT.NAME = H1.NAME(+)

";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_UNT()
        {
            var q = @"SELECT FIRM_CODE, NAME FROM FIRMS WHERE FIRM_CODE = '1402102001'";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_V_TYP()
        {
            var q = @"SELECT VACATION_TYPES.VACATION_TYPE_ID,VACATION_TYPES.NAME FROM VACATION_TYPES";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GET_ROLE(string ID, string FIRM)
        {
            var g = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == ID && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == FIRM).OFF_ABS_GROUP_ID;
            var steps = from o in db.OFF_ABS_STEPS
                        where o.OFF_ABS_GROUP_ID == g
                        orderby o.ORDER_ID
                        select o;
            return PartialView("_Responser", steps);
            
        }

        public ActionResult GET_V_ROLE(string firm_code, string person_id, string VAC_ID)
        {
            WF_EN E = new WF_EN();
            string pers_cod = E.PERSON_DATA.First(o => o.PERSONAL_ID_NO == person_id).PERSON_CODE;
            var g = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == pers_cod && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code) ?
                db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == pers_cod && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code).OFF_ABS_GROUP_ID :
                db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 2 && o.FIRMS_CODE == firm_code).OFF_ABS_GROUP_ID;
            var steps = from o in db.OFF_ABS_STEPS
                        where o.OFF_ABS_GROUP_ID == g
                        orderby o.ORDER_ID
                        select o;
            string q =
                        @"SELECT OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                                 OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                                 OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                                 ORDER_ID,
                                 VAC_DET.PERSON_VACATIONS_DET_ID,
                                 VAC_DET.PERSON_DATE_ID,
                                 VAC_DET.PERSON_VACATIONS_SEQ,
                                 persn_name.RANK || ' / ' || persn_name.PERSON_NAME AS PERSON_NAME,
                                 persn_name.RANK,
                                 persn_name.PERSON_CODE,
                                 JB.JOB_NAME AS JOB,
                                 P_JB.JOB_TYPE_ID AS ROL
                            FROM OFF_ABS_STEPS,
                                 OFF_ABS_GROUP,
                                 PERSON_VACATIONS_DET VAC_DET,
                                 --OFF_ROLE_ARCHIVE ROL,
                                 JOBS_TYPES jb,
                                 PERSON_JOBS p_jb,
                                 (SELECT PERSON_DATA.PERSON_NAME, RANKS.RANK, PERSON_DATA.PERSON_CODE
                                    FROM person_data,  ranks
                                   WHERE PERSON_DATA.RANK_ID = RANKS.RANK_ID ) persn_name
                           WHERE  VAC_DET.OFF_ABS_GROUP_ID =
                                        OFF_ABS_GROUP.OFF_ABS_GROUP_ID
                                 AND VAC_DET.OFF_ABS_STEPS_ID =
                                        OFF_ABS_STEPS.OFF_ABS_STEPS_ID
                                 AND VAC_DET.PERSON_DATE_OWEN = persn_name.PERSON_CODE(+)
                                 AND OFF_ABS_STEPS.JOB_TYPE_ID = P_JB.JOB_TYPE_ID
                                 AND P_JB.JOB_TYPE_ID = JB.JOB_TYPE_ID
                                 AND P_JB.TO_DATE IS NULL
                                 AND VAC_DET.PERSON_DATE_ID = '" + pers_cod + @"'
                                 AND VAC_DET.PERSON_VACATIONS_SEQ = " + VAC_ID + @"
                        ORDER BY ORDER_ID";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult add_officer_fun(PERSON_VACATIONS_DET P_VAC, string per, string step_name)
        {
            message = "";
            status = false;
            WF_EN en = new WF_EN();
            if (!en.PERSON_VACATIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == P_VAC.PERSON_VACATIONS_SEQ && o.PERSON_DATE_ID == P_VAC.PERSON_DATE_ID && o.OFF_ABS_STEPS_ID == P_VAC.OFF_ABS_STEPS_ID))
            {
                var max = en.PERSON_VACATIONS_DET.Any(o=>o.PERSON_VACATIONS_SEQ == P_VAC.PERSON_VACATIONS_SEQ) ? en.PERSON_VACATIONS_DET.Where(o=>o.PERSON_VACATIONS_SEQ == P_VAC.PERSON_VACATIONS_SEQ).Max(o=>o.PERSON_VACATIONS_DET_ID) + 1 : 1;

                db.PERSON_VACATIONS_DET.Add(new PERSON_VACATIONS_DET()
                {
                    PERSON_VACATIONS_DET_ID = Convert.ToInt16(max),
                    PERSON_VACATIONS_SEQ = P_VAC.PERSON_VACATIONS_SEQ,
                    PERSON_DATE_ID = P_VAC.PERSON_DATE_ID,
                    PERSON_DATE_OWEN = P_VAC.PERSON_DATE_OWEN,
                    OFF_ABS_STEPS_ID = P_VAC.OFF_ABS_STEPS_ID,
                    OFF_ABS_GROUP_ID = P_VAC.OFF_ABS_GROUP_ID,
                    OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(per)

                });

                db.SaveChanges();
                //WF_EN E = new WF_EN();
                //string name = "";
                //var pers_data = E.PERSON_DATA.First(o=>o.PERSONAL_ID_NO == per);

                //var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + pers_data.PERSON_NAME + " , بوضعك " + step_name + " قائم  معه في المأمورية', 0, " + pers_data.PERSON_CODE + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per + " ')";
                //General.exec_q(Q, "");
                //General.exec_q("COMMIT", "");


                message = "Successfully Saved.";



            }
            else
            {
                var xx = db.PERSON_VACATIONS_DET.First(o => o.PERSON_VACATIONS_SEQ == P_VAC.PERSON_VACATIONS_SEQ && o.PERSON_DATE_ID == P_VAC.PERSON_DATE_ID && o.PERSON_VACATIONS_DET_ID == P_VAC.PERSON_VACATIONS_DET_ID && o.OFF_ABS_STEPS_ID == P_VAC.OFF_ABS_STEPS_ID);
                xx.PERSON_DATE_OWEN = P_VAC.PERSON_DATE_OWEN;
                xx.OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(per);
                db.SaveChanges();

                message = "    تم التعديل      ";
            }

            string name = "";
            var pers_data = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per);

            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + pers_data.RANKS.RANK + " / " + pers_data.PERSON_NAME + " , بوضعك " + step_name + "  وذلك لطلب الأجازة الخاص بة ', 0, " + pers_data.PERSON_CODE + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");

            status = true;
            return new JsonResult { Data = new { status = status, message = message } };
        }
    }
}