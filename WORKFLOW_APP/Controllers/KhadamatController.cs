using System.Data.OracleClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WORKFLOW_APP.Models;
using WORKFLOW_APP.Views.Shared;

namespace WORKFLOW_APP.Controllers
{
    public class KhadamatController : Controller
    {
        Boolean status = true;
        string title = "";
        string type = "success";
        string message = "";
        private WF_EN db = new WF_EN();

        //
        // GET: /Khadamat/

        public ActionResult Index()
        {
            var firms_absences_persons = from o in db.FIRMS_ABSENCES_PERSONS
                                         from t in db.ABSENCE_TYPES
                                         where o.ABSENCE_TYPE_ID == t.ABSENCE_TYPE_ID && t.ABSCENCE_CATEGORY_ID == 1 && o.PERSON_CODE == "0"//"35257"
                                         orderby o.FROM_DATE descending
                                         select o;
            return View(firms_absences_persons.ToList());
        }

        //
        // GET: /Khadamat/Details/5

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
        // GET: /Khadamat/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            //return View();
            return PartialView("Create");
        }

        //
        // POST: /Khadamat/Create

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
            //return View(firms_absences_persons);
            return View();
        }

        //
        // GET: /Khadamat/Edit/5

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
        // POST: /Khadamat/Edit/5

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
        // GET: /Khadamat/Delete/5

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
        // POST: /Khadamat/Delete/5

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

        public ActionResult GET_KHDMA()
        {
            var q = @"SELECT ABSENCE_TYPE_ID AS ID, NAME FROM ABSENCE_TYPES where ABSCENCE_CATEGORY_ID = 1 AND  FOR_OFFICERS = 1 ORDER BY 1";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_V_ROLE(string firm_code, string person_id, string ABS_TYP, string FDT, string TDT)
        {
            WF_EN E = new WF_EN();
            var g = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == person_id && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code) ?
                db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == person_id && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code).OFF_ABS_GROUP_ID : 0;
            var steps = from o in db.OFF_ABS_STEPS
                        where o.OFF_ABS_GROUP_ID == g
                        orderby o.ORDER_ID
                        select o;
            string q =
                        @"SELECT OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                                 OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                                 OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                                 ORDER_ID,
                                 DET.FIRMS_ABSENCES_PERSONS_DET_ID AS ID,
                                 DET.PERSON_CODE,
                                 DET.ABSENCE_TYPE_ID AS TYPE_ID,
                                 DET.FIRM_CODE,
                                 TO_CHAR(DET.FROM_DATE) AS FDT,
                                 TO_CHAR(DET.TO_DATE) AS TDT,
                                 persn_name.RANK || ' / ' || persn_name.PERSON_NAME AS PERSON_NAME,
                                 persn_name.RANK,
                                 persn_name.PERSON_CODE,
                                 JB.JOB_NAME AS JOB,
                                 P_JB.JOB_TYPE_ID AS ROL
                            FROM OFF_ABS_STEPS,
                                 OFF_ABS_GROUP,
                                 FIRMS_ABSENCES_PERSONS_DET DET,
                                 JOBS_TYPES jb,
                                 PERSON_JOBS p_jb,
                                 (SELECT PERSON_DATA.PERSON_NAME, RANKS.RANK, PERSON_DATA.PERSON_CODE
                                    FROM person_data,  ranks
                                   WHERE PERSON_DATA.RANK_ID = RANKS.RANK_ID ) persn_name
                           WHERE  DET.OFF_ABS_GROUP_ID =
                                        OFF_ABS_GROUP.OFF_ABS_GROUP_ID(+)
                                 AND DET.OFF_ABS_STEPS_ID =
                                        OFF_ABS_STEPS.OFF_ABS_STEPS_ID(+)
                                 AND DET.PERSON_DATE_OWEN = persn_name.PERSON_CODE(+)
                                 AND OFF_ABS_STEPS.JOB_TYPE_ID = P_JB.JOB_TYPE_ID
                                 AND P_JB.JOB_TYPE_ID = JB.JOB_TYPE_ID
                                 AND P_JB.TO_DATE IS NULL
                                 AND DET.ABSENCE_TYPE_ID = " + ABS_TYP + @"
                                 AND DET.FIRM_CODE = '" + firm_code + @"'
                                 AND DET.PERSON_CODE = " + person_id + @"
                                 AND TO_CHAR(DET.FROM_DATE, 'DD/MM/RRRR HH24:MI:SS') = '" + FDT + @"'
                                 AND TO_CHAR(DET.TO_DATE, 'DD/MM/RRRR HH24:MI:SS') = '" + TDT + @"'
                        ORDER BY ORDER_ID";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_khd_grd(string FIRM, string TYP, string MON, string TY, string PRIOD)
        {
            var q = @"SELECT  PERSON_DATA.RANK_ID,
                                     RANKS.RANK,
                                     PERSON_DATA.PERSON_NAME,
                                     FIRMS_ABSENCES_PERSONS.FIRM_CODE,
                                     to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy hh24:mi:ss') FROM_DATE,
                                      to_char(FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy hh24:mi:ss') TO_DATE,
                                     --TO_CHAR(FIRMS_ABSENCES_PERSONS . TO_DATE, 'D'),  
                                     DECODE(TO_CHAR(FIRMS_ABSENCES_PERSONS . FROM_DATE, 'D'),1,'الأحد',2,'الأثنين',3,'الثلاثاء',4,'الأربعاء',5,'الخميس',6,'الجمعة','السبت') AS DY,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES,
                                     PERSON_DATA.ID_NO,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CODE,
                                     FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG,
                                     FIRMS_ABSENCES_PERSONS.FIN_YEAR,
                                     FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID,
                                     FIRMS_ABSENCES_PERSONS.RANK_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_STATUS,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID,
                                     PERSON_DATA.CATEGORY_ID,
                                     PERSON_DATA.SORT_NO,
                                     PERSON_DATA.CURRENT_RANK_DATE,
                                     FIRMS_ABSENCES_PERSONS.FORCE_DELETE_DATE,
                                     FIRMS_ABSENCES_PERSONS.ESCAPE_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.RETURN_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.DAY_STATUS,
                                     DECODE(FIRMS_ABSENCES_PERSONS.DAY_STATUS,2,'عطلة',1,'قبل العطلة','عادية') AS DAY_STAT
                              FROM  FIRMS_ABSENCES_PERSONS ,   
                                      PERSON_DATA ,   
                                      ABSCENCE_CATEGORIES ,   
                                      ABSENCE_TYPES,
                                      RANKS   
                               WHERE (  PERSON_DATA . PERSON_CODE  =  FIRMS_ABSENCES_PERSONS . PERSON_CODE  ) and  
                                     (RANKS.RANK_ID=PERSON_DATA.RANK_ID) AND
                                     (  ABSENCE_TYPES . ABSCENCE_CATEGORY_ID  =  ABSCENCE_CATEGORIES . ABSCENCE_CATEGORY_ID  ) and  
                                     (  ABSENCE_TYPES . ABSENCE_TYPE_ID  =  FIRMS_ABSENCES_PERSONS . ABSENCE_TYPE_ID  ) and  
                                     (ABSENCE_STATUS IS NULL OR ABSENCE_STATUS = 1) AND
                                     ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FIRM + @" ) AND  
                                     ( person_data.rank_cat_id = 1 ) AND  
                                     ( to_number(to_char(firms_absences_persons.from_date , 'mm')) = " + MON + @" ) AND  
                                     ( abscence_categories.abscence_category_id = 1 ) AND  
                                     ( firms_absences_persons.absence_type_id = " + TYP + @" ) AND  
                                     ( firms_absences_persons.fin_year = '" + TY + @"' ) AND  
                                     ( FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID = " + PRIOD + @" ) )
                                ORDER BY FIRMS_ABSENCES_PERSONS . TO_DATE";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_khd_off(string FIRM, string CAT)
        {
            var q = @"SELECT PERSON_DATA.PERSON_CODE,
                             PERSON_DATA.PERSON_CAT_ID,
                             RANKS.RANK,
                             PERSON_DATA.PERSON_NAME,
                             PERSON_DATA.ID_NO,
                             PERSON_DATA.RANK_ID,
                             FIRMS.NAME,
                             PERSON_DATA.FIRM_CODE,
                             PERSON_DATA.CATEGORY_ID,
                             RANKS.DISPLAY_ORDER,
                             PERSON_DATA.RANK_CAT_ID,
                             PERSON_DATA.PERSON_CAT_ID
                        FROM PERSON_DATA, FIRMS, RANKS
                       WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
                             AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1)
                                  AND (PERSON_DATA.FIRM_CODE IN
                                          (SELECT FIRMS_B.FIRM_CODE
                                             FROM FIRMS FIRMS_A, FIRMS FIRMS_B
                                            WHERE (FIRMS_A.FIRM_CODE = '" + FIRM + @"')
                                                  AND (FIRMS_B.FIRM_LIKE_CODE LIKE
                                                          CONCAT (FIRMS_A.FIRM_LIKE_CODE, '%')))
                                       OR PERSON_DATA.BORROW_FIRM_CODE = '" + FIRM + @"'))
                             AND PERSON_DATA.RANK_CAT_ID = " + CAT + @"
                    ORDER BY person_data.firm_code ASC,
                             person_data.person_cat_id ASC,
                             person_data.rank_cat_id ASC,
                             person_data.rank_id ASC,
                             person_data.current_rank_date ASC,
                             person_data.id_no ASC";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_cat()
        {
            var q = @"SELECT RANK_CAT_ID AS ID,NAME FROM RANK_CATEGORIES";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult addrec(FIRMS_ABSENCES_PERSONS ab)
        {
            var PERSON_CODE = ab.PERSON_CODE;
            var TRAINING_PERIOD_ID = ab.TRAINING_PERIOD_ID;
            var FIN_YEAR = ab.FIN_YEAR;
            var FROM_DATE = ab.FROM_DATE;//DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(x[4]);
            var TO_DATE = ab.TO_DATE.Value.Date; // != "" ? Convert.ToDateTime(x[8]) : DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); // Convert.ToDateTime(x[4]);                                          //DateTime.ParseExact(x[3], "dd/MM/yyyy", CultureInfo.InvariantCulture); 
            var FIRM_CODE = ab.FIRM_CODE;
            var RANK_CAT_ID = ab.RANK_CAT_ID;
            var PERSON_CAT_ID = ab.PERSON_CAT_ID;
            var stat = 0;
            var dnm = FROM_DATE.DayOfWeek.ToString();
            var sp = new FIRMS_ABSENCES_PERSONS();
            sp.PERSON_CODE = PERSON_CODE;
            sp.FIN_YEAR = FIN_YEAR;
            sp.FIRM_CODE = FIRM_CODE;
            sp.FROM_DATE = FROM_DATE;
            sp.TO_DATE = TO_DATE;
            sp.RANK_CAT_ID = RANK_CAT_ID;
            sp.DAY_STATUS = (short)(dnm == "Friday" ? 2 : dnm == "Thursday" ? 1 : 0);
            sp.TRAINING_PERIOD_ID = (short)TRAINING_PERIOD_ID;
            sp.PERSON_CAT_ID = PERSON_CAT_ID;
            sp.ABSENCE_TYPE_ID = ab.ABSENCE_TYPE_ID;
            sp.COMMANDER_FLAG = 0;
            sp.ACT_DATE = ab.TO_DATE;
            db.FIRMS_ABSENCES_PERSONS.Add(sp);
            db.SaveChanges();
            message = "تمت الإضافة بنجاح";
            title = "تم";


            var d1 = new FIRMS_ABSENCES_PERSONS_DET();
            var max = db.FIRMS_ABSENCES_PERSONS_DET.Any() ? db.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1 : 1;
            d1.FIRMS_ABSENCES_PERSONS_DET_ID = max;
            d1.ABSENCE_TYPE_ID = ab.ABSENCE_TYPE_ID;
            d1.FIN_YEAR = FIN_YEAR;
            d1.TRAINING_PERIOD_ID = (short)TRAINING_PERIOD_ID;
            d1.PERSON_CODE = PERSON_CODE;
            d1.PERSON_CAT_ID = PERSON_CAT_ID;
            d1.RANK_CAT_ID = RANK_CAT_ID;
            d1.FIRM_CODE = FIRM_CODE;
            d1.FROM_DATE = FROM_DATE;
            d1.TO_DATE = TO_DATE;
            d1.PERSON_DATE_OWEN = PERSON_CODE;
            db.FIRMS_ABSENCES_PERSONS_DET.Add(d1);
            db.SaveChanges();
            max += 1;

            var pg = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == PERSON_CODE && o.ABS_CAT_ID == 1) ?
                db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == PERSON_CODE && o.ABS_CAT_ID == 1).OFF_ABS_GROUP_ID :
                db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 1).OFF_ABS_GROUP_ID;
            var steps = from o in db.OFF_ABS_STEPS
                        where o.OFF_ABS_GROUP_ID == pg
                        select o;
            foreach (var s in steps)
            {
                var det = new FIRMS_ABSENCES_PERSONS_DET();
                var max1 = db.FIRMS_ABSENCES_PERSONS_DET.Any() ? db.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1 : 1;
                det.FIRMS_ABSENCES_PERSONS_DET_ID = max1;
                det.PERSON_CODE = PERSON_CODE;
                det.FIN_YEAR = FIN_YEAR;
                det.TRAINING_PERIOD_ID = (short)TRAINING_PERIOD_ID;
                det.FIRM_CODE = FIRM_CODE;
                det.FROM_DATE = FROM_DATE;
                det.TO_DATE = TO_DATE;
                det.RANK_CAT_ID = RANK_CAT_ID;
                det.PERSON_CAT_ID = PERSON_CAT_ID;
                det.ABSENCE_TYPE_ID = ab.ABSENCE_TYPE_ID;
                det.OFF_ABS_GROUP_ID = pg;
                det.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                db.FIRMS_ABSENCES_PERSONS_DET.Add(det);
                db.SaveChanges();
            }
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        [HttpPost]
        public ActionResult delrec(FIRMS_ABSENCES_PERSONS ab)
        {
            FIRMS_ABSENCES_PERSONS v = db.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == ab.PERSON_CODE && o.FIRM_CODE == ab.FIRM_CODE && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID && o.FROM_DATE == ab.FROM_DATE);
            if (v.FROM_DATE >= DateTime.Now.Date)
            {
                if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.PERSON_CODE == ab.PERSON_CODE && o.FIRM_CODE == ab.FIRM_CODE && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID && o.FROM_DATE == ab.FROM_DATE && o.DECTION != null))
                {
                    var steps = from o in db.FIRMS_ABSENCES_PERSONS_DET
                                where o.PERSON_CODE == ab.PERSON_CODE && o.FIRM_CODE == ab.FIRM_CODE && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID && o.FROM_DATE == ab.FROM_DATE
                                select o;
                    foreach (var s in steps)
                    {
                        var det = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == s.FIRMS_ABSENCES_PERSONS_DET_ID && o.PERSON_CODE == ab.PERSON_CODE &&
                            o.FIRM_CODE == ab.FIRM_CODE && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID && o.FROM_DATE == ab.FROM_DATE);
                        db.FIRMS_ABSENCES_PERSONS_DET.Remove(det);
                        db.SaveChanges();
                    }
                    var PERSON_CODE = ab.PERSON_CODE;
                    var TRAINING_PERIOD_ID = ab.TRAINING_PERIOD_ID;
                    var FIN_YEAR = ab.FIN_YEAR;
                    var FROM_DATE = ab.FROM_DATE;//DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(x[4]);
                    var TO_DATE = ab.TO_DATE; // != "" ? Convert.ToDateTime(x[8]) : DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); // Convert.ToDateTime(x[4]);                                          //DateTime.ParseExact(x[3], "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                    var FIRM_CODE = ab.FIRM_CODE;
                    var RANK_CAT_ID = ab.RANK_CAT_ID;
                    var PERSON_CAT_ID = ab.PERSON_CAT_ID;
                    var stat = 0;
                    var dnm = FROM_DATE.DayOfWeek.ToString();
                    var sp = db.FIRMS_ABSENCES_PERSONS.First(o => o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.FROM_DATE == FROM_DATE && o.PERSON_CODE == PERSON_CODE && o.PERSON_CAT_ID == PERSON_CAT_ID &&
                        o.RANK_CAT_ID == RANK_CAT_ID && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID);
                    db.FIRMS_ABSENCES_PERSONS.Remove(sp);
                    db.SaveChanges();
                    message = "تمت الحذف بنجاح";
                    title = "تم";
                }
                else
                {
                    message = "تم التصديق على الطلب \n راجع المختص";
                    title = "خطأ";
                    type = "error";
                }
            }
            else
            {
                message = "لا يمكن حذف خدمة منتهية";
                title = "خطأ";
                type = "error";
            }
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        [HttpPost]
        public ActionResult add_officer_fun(FIRMS_ABSENCES_PERSONS_DET P_VAC, string per, string step_name)
        {
            message = "";
            status = false;
            WF_EN en = new WF_EN();
            if (!en.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == P_VAC.FIRMS_ABSENCES_PERSONS_DET_ID && o.PERSON_CODE == P_VAC.PERSON_CODE && o.OFF_ABS_STEPS_ID == P_VAC.OFF_ABS_STEPS_ID
                && o.ABSENCE_TYPE_ID == P_VAC.ABSENCE_TYPE_ID && o.FROM_DATE == P_VAC.FROM_DATE && o.TO_DATE == P_VAC.TO_DATE))
            {
                var max = en.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == P_VAC.FIRMS_ABSENCES_PERSONS_DET_ID) ? en.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1 : 1;

                db.FIRMS_ABSENCES_PERSONS_DET.Add(new FIRMS_ABSENCES_PERSONS_DET()
                {
                    FIRMS_ABSENCES_PERSONS_DET_ID = Convert.ToInt16(max),
                    ABSENCE_TYPE_ID = P_VAC.ABSENCE_TYPE_ID,
                    PERSON_CODE = P_VAC.PERSON_CODE,
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
                var xx = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == P_VAC.FIRMS_ABSENCES_PERSONS_DET_ID && o.PERSON_CODE == P_VAC.PERSON_CODE && o.OFF_ABS_STEPS_ID == P_VAC.OFF_ABS_STEPS_ID
                && o.ABSENCE_TYPE_ID == P_VAC.ABSENCE_TYPE_ID && o.FROM_DATE == P_VAC.FROM_DATE && o.TO_DATE == P_VAC.TO_DATE);
                xx.PERSON_DATE_OWEN = P_VAC.PERSON_DATE_OWEN;
                xx.OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(per);
                db.SaveChanges();

                message = "    تم التعديل      ";
            }

            string name = "";
            var pers_data = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per);

            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + pers_data.PERSON_NAME + " , بوضعك " + step_name + "  وذلك لطلب الأجازة الخاص بة ', 0, " + pers_data.PERSON_CODE + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");

            status = true;
            return new JsonResult { Data = new { status = status, message = message } };
        }
    }
}