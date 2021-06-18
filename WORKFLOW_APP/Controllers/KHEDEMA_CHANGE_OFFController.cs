using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WORKFLOW_APP.Models;
using WORKFLOW_APP.Views.Shared;

namespace WORKFLOW_APP.Controllers
{
    public class KHEDEMA_CHANGE_OFFController : Controller
    {
        Boolean status = true;
        string title = "";
        string type = "success";
        string message = "";
        private WF_EN db = new WF_EN();

        //
        // GET: /KHDMA_CHANG/

        public ActionResult Index()
        {
            //var firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Include(f => f.PERSON_DATA);
            return View();
        }

        //
        // GET: /KHDMA_CHANG/Details/5

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
        // GET: /KHDMA_CHANG/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return View();
        }

        public ActionResult add_off(string off)
        {
            ViewBag.SD = true;
            ViewBag.off = off;
            return PartialView("../KHEDEMA_CHANGE_OFF/CREate_Off");
        }

        //
        // POST: /KHDMA_CHANG/Create

        [HttpPost]
        public ActionResult Create(FIRMS_ABSENCES_PERSONS exc)
        {
            if (ModelState.IsValid)
            {
                db.FIRMS_ABSENCES_PERSONS.Add(exc);
                db.SaveChanges();
            }

            message = "تمت الإضافة بنجاح";
            title = "تم";
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        //
        // GET: /KHDMA_CHANG/Edit/5

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
        // POST: /KHDMA_CHANG/Edit/5

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
        // GET: /KHDMA_CHANG/Delete/5

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
        // POST: /KHDMA_CHANG/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(FIRMS_ABSENCE_EXCHANGE ab)
        {
            var ch = db.FIRMS_ABSENCE_EXCHANGE.First(o => o.FIRM_CODE == ab.FIRM_CODE && o.FROM_PERSON_CODE == ab.FROM_PERSON_CODE &&
                o.TO_PERSON_CODE == ab.TO_PERSON_CODE && o.FROM_DATE == ab.FROM_DATE && o.ABSENCE_TYPE_ID == ab.ABSENCE_TYPE_ID);
            db.FIRMS_ABSENCE_EXCHANGE.Remove(ch);
            db.SaveChanges();

            var other_per_id = db.PERSON_DATA.First(o => o.PERSON_CODE == ab.TO_PERSON_CODE);
            var per = db.PERSON_DATA.First(o => o.PERSON_CODE == ab.FROM_PERSON_CODE);
            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 8, 'تم حذف طلب المبادلة مع  " + other_per_id.PERSON_NAME + " وذلك بتاريخ " + ab.FROM_DATE.ToString("yyyy/MM/dd") + "', 0, " + other_per_id.PERSONAL_ID_NO + " , 'http://192.223.30.3:90/mster_msdc/PAGES/w_present_vacation.aspx?nn= " + other_per_id.PERSONAL_ID_NO + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");
            message = "تمت الحذف بنجاح";
            title = "تم";
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult GET_O_K1(string FIRM, string TYP, string MON, string PERS, string TY, string PY)
        {
            var q = @"  SELECT TO_CHAR (AB.FROM_DATE, 'dd/mm/yyyy HH:mi:ss') DT
                        FROM FIRMS_ABSENCES_PERSONS AB,
                             PERSON_DATA PD,
                             ABSCENCE_CATEGORIES ABC,
                             ABSENCE_TYPES ABT,
                             RANKS R
                       WHERE     (PD.PERSON_CODE = AB.PERSON_CODE)
                             AND (R.RANK_ID = PD.RANK_ID)
                             AND (ABT.ABSCENCE_CATEGORY_ID = ABC.ABSCENCE_CATEGORY_ID)
                             AND (ABT.ABSENCE_TYPE_ID = AB.ABSENCE_TYPE_ID)
                             AND (AB.FIRM_CODE = '" + FIRM + @"')
                             AND (PD.rank_cat_id = 1)
                             AND AB.PERSON_CODE = " + PERS + @"
                             AND TO_CHAR (AB.from_date, 'mm') = '" + MON + @"'
                             AND (AB.absence_type_id = " + TYP + @")
                             AND (AB.fin_year = '" + TY + @"')
                             AND (AB.TRAINING_PERIOD_ID = " + PY + @")
                             AND COMMANDER_FLAG = 1
                             AND (ABSENCE_STATUS IS NULL)
                    ORDER BY AB.FROM_DATE";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_MON()
        {
            var q = @"SELECT DISTINCT(TO_CHAR (FROM_DATE, 'yyyy/MM')) AS NAME,TO_CHAR (FROM_DATE, 'yyyy') AS YR,TO_CHAR (FROM_DATE, 'MM') AS ID FROM FIRMS_ABSENCES_PERSONS EX ORDER BY YR DESC,ID DESC";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_khd_grd(string FIRM, string TYP, string MON, string PERS)
        {
            var q = @"SELECT CH.FIRM_CODE,
                            FROM_PERSON_CODE,
                            TO_PERSON_CODE,
                            ABSENCE_TYPE_ID,
                            to_char(FROM_DATE,'HH:mi:ss RRRR/MM/DD') FROM_DATE,
                            TO_CHAR(FROM_DATE,'MM/YYYY')  AS DD,
                            FROM_RANK_ID,
                            FROM_RANK_CAT_ID,
                            FROM_PERSON_CAT_ID,
                            TO_RANK_ID,
                            TO_RANK_CAT_ID,
                            TO_PERSON_CAT_ID,
                            TO_CHAR(EXCHANGE_DATE,'HH:mi:ss RRRR/MM/DD') AS EXCHANGE_DATE,
                            to_char(TO_DATE,'HH:mi:ss RRRR/MM/DD') TO_DATE,
                            OPENION1,
                            SEC_COMMAND_OPENION,
                            COMMAND_DECISION,
                            IS_APPROVED,
                            APPROVAL_NO,
                            APPROVAL_DATE,
                            FRM_R.RANK as RANK1,
                            FRM_OFF.PERSON_NAME as NAME1,
                            TO_R.RANK as RANK2,
                            TO_OFF.PERSON_NAME as NAME2, 
                            CH.OTHER_PER_DECS,
                            CH.PLANNING_DECESION,
                            CH.PLANNING_NOTES,
                            CH.VICE_COMMAND_DECESION,
                            CH.VICE_COMMAND_NOTES,
                            FRM_R.RANK || ' / ' || FRM_OFF.PERSON_NAME AS FOFF,
                            TO_R.RANK || ' / ' || TO_OFF.PERSON_NAME AS TOFF
                    FROM FIRMS_ABSENCE_EXCHANGE CH,
                            PERSON_DATA FRM_OFF,
                            PERSON_DATA TO_OFF,
                            RANKS FRM_R,
                            RANKS TO_R
                    WHERE     CH.FROM_PERSON_CODE = FRM_OFF.PERSON_CODE
                            AND CH.FROM_RANK_CAT_ID = FRM_R.RANK_CAT_ID
                            AND CH.FROM_RANK_ID = FRM_R.RANK_ID
                            AND CH.FIRM_CODE = FRM_OFF.FIRM_CODE
                            AND CH.TO_PERSON_CODE = TO_OFF.PERSON_CODE
                            AND CH.TO_RANK_CAT_ID = TO_R.RANK_CAT_ID
                            AND CH.TO_RANK_ID = TO_R.RANK_ID
                            AND CH.FIRM_CODE = TO_OFF.FIRM_CODE
                            AND CH.FIRM_CODE = " + FIRM + @"
                            AND CH.ABSENCE_TYPE_ID = " + TYP + @"
                            AND (FRM_OFF.OUT_UN_FORCE = 0 OR TO_OFF.OUT_UN_FORCE = 0)
                            AND TO_CHAR(FROM_DATE,'YYYY/MM') = '" + MON + @"'
                        ORDER BY EXCHANGE_DATE DESC, FROM_DATE";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult add_exc(string FIRM, Int16 TYP, string PC1, string PC2, string F_DT, string T_DT)
        {
            Nullable<DateTime> DT2 = null;
            DateTime fdt = DateTime.ParseExact(F_DT, "dd/MM/yyyy hhhh:mm:ss", CultureInfo.InvariantCulture); 
            if (T_DT != "")
            {
                DT2 = DateTime.ParseExact(T_DT, "dd/MM/yyyy hhhh:mm:ss", CultureInfo.InvariantCulture);
            }

            var P1 = db.PERSON_DATA.First(o => o.PERSON_CODE == PC1);
            var P2 = db.PERSON_DATA.First(o => o.PERSON_CODE == PC2);
            var p1 = db.PERSON_DATA.First(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM);
            var p2 = db.PERSON_DATA.First(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM);
            var dnm = (fdt).DayOfWeek.ToString();
            var y = db.TRAINING_YEARS.First(o => o.IS_CURRENT == 1).FIN_YEAR;
            var priod = db.TRAINING_PERIODS.First(o => o.FIN_YEAR == y && o.PERIOD_FROM <= fdt && o.PERIOD_TO >= fdt).TRAINING_PERIOD_ID;
            decimal ser = 0;
            if (!db.FIRMS_ABSENCE_EXCHANGE.Any(o => o.FIRM_CODE == FIRM && o.FROM_DATE == fdt && o.FROM_PERSON_CODE == PC1 && o.TO_PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP))
            {
                ser = (decimal)(db.FIRMS_ABSENCE_EXCHANGE.Any(o => o.SEQ != null && o.FIRM_CODE == FIRM) ? (db.FIRMS_ABSENCE_EXCHANGE.Where(o => o.FIRM_CODE == FIRM).Max(o => o.SEQ) + 1) : 1);
                FIRMS_ABSENCE_EXCHANGE ex = new FIRMS_ABSENCE_EXCHANGE();
                ex.ABSENCE_TYPE_ID = TYP;
                ex.FROM_DATE = fdt;
                if (T_DT != "")
                {
                    ex.TO_DATE = DT2;
                }
                ex.FIRM_CODE = FIRM;
                ex.FROM_PERSON_CODE = PC1;
                ex.TO_PERSON_CODE = PC2;
                ex.FROM_PERSON_CAT_ID = P1.PERSON_CAT_ID;
                ex.TO_PERSON_CAT_ID = P2.PERSON_CAT_ID;
                ex.FROM_RANK_CAT_ID = P1.RANK_CAT_ID;
                ex.TO_RANK_CAT_ID = P2.RANK_CAT_ID;
                ex.FROM_RANK_ID = P1.RANK_ID;
                ex.TO_RANK_ID = P2.RANK_ID;
                ex.EXCHANGE_DATE = DateTime.Now;
                ex.SEQ = ser;
                ex.IS_APPROVED = 0;
                db.FIRMS_ABSENCE_EXCHANGE.Add(ex);
                //db.SaveChanges();
            }
            else
            {
                var ex = db.FIRMS_ABSENCE_EXCHANGE.First(o => o.FIRM_CODE == FIRM && o.FROM_DATE == fdt && o.FROM_PERSON_CODE == PC1 && o.TO_PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP);
                ser = (decimal)ex.SEQ;
                ex.IS_APPROVED = 0;
            }

            #region P2
            // adding new absence to p2
            if (!db.FIRMS_ABSENCES_PERSONS.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt))
            {
                var abs = new FIRMS_ABSENCES_PERSONS();
                abs.COMMANDER_FLAG = 0;
                abs.FIN_YEAR = y;
                abs.TRAINING_PERIOD_ID = priod;
                abs.ABSENCE_TYPE_ID = TYP;
                abs.FIRM_CODE = FIRM;
                abs.PERSON_CODE = PC2;
                abs.PERSON_CAT_ID = (short)p2.PERSON_CAT_ID;
                abs.RANK_CAT_ID = (short)p2.RANK_CAT_ID;
                abs.FROM_DATE = fdt;
                abs.TO_DATE = abs.FROM_DATE.Date.AddDays(1);
                abs.DAY_STATUS = (short)(dnm == "Friday" ? 2 : dnm == "Thursday" ? 1 : 0);
                abs.ABSENCE_STATUS = 2;
                abs.ABS_REF = ser;
                db.FIRMS_ABSENCES_PERSONS.Add(abs);
            }
            else
            {
                var p = db.FIRMS_ABSENCES_PERSONS.First(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt);
                p.COMMANDER_FLAG = 0;
            }

            /////////////////////////////

            //update absdcence to pc1
            WF_EN en = new WF_EN();
            FIRMS_ABSENCES_PERSONS v = en.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt);
            //v.COMMANDER_FLAG = 0;
            v.ABSENCE_STATUS = 1;
            v.ABS_REF = ser;
            en.SaveChanges();

            // adding abs_det for p2
            var max = db.FIRMS_ABSENCES_PERSONS_DET.Any() ? db.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1 : 1;
            if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt && o.PERSON_DATE_OWEN == PC2))
            {
                var d1 = new FIRMS_ABSENCES_PERSONS_DET();
                d1.FIRMS_ABSENCES_PERSONS_DET_ID = max;
                d1.ABSENCE_TYPE_ID = TYP;
                d1.FIN_YEAR = y;
                d1.TRAINING_PERIOD_ID = priod;
                d1.PERSON_CODE = PC2;
                d1.PERSON_CAT_ID = (short)p2.PERSON_CAT_ID;
                d1.RANK_CAT_ID = (short)p2.RANK_CAT_ID;
                d1.FIRM_CODE = FIRM;
                d1.FROM_DATE = fdt;
                d1.TO_DATE = d1.FROM_DATE.Date.AddDays(1);
                d1.ACT_TO_DATE = d1.FROM_DATE.AddDays(1);
                d1.PERSON_DATE_OWEN = PC2;
                db.FIRMS_ABSENCES_PERSONS_DET.Add(d1);
                max += 1;
            }
            else
            {
                var p = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt && o.PERSON_DATE_OWEN == PC2);
                p.DECTION = null;
            }
            // adding group steps for p2

            var pg = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == PC2 && o.ABS_CAT_ID == 1) ?
                db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == PC2 && o.ABS_CAT_ID == 1).OFF_ABS_GROUP_ID :
                db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 1).OFF_ABS_GROUP_ID;
            var steps = from o in db.OFF_ABS_STEPS
                        where o.OFF_ABS_GROUP_ID == pg
                        select o;
            foreach (var s in steps)
            {
                var det = new FIRMS_ABSENCES_PERSONS_DET();
                var x = @"    SELECT PD.PERSON_CODE
                                FROM PERSON_DATA PD, PERSON_JOBS R, OFF_ABS_STEPS STP
                                WHERE     PD.PERSON_CODE = R.PERSON_CODE
                                    AND STP.JOB_TYPE_ID = R.JOB_TYPE_ID
                                    AND R.TO_DATE is NULL
                                    AND STP.OFF_ABS_GROUP_ID = " + pg + @"
                                    AND STP.OFF_ABS_STEPS_ID = " + s.OFF_ABS_STEPS_ID;
                var res = General.exec_f(x, "");
                var old_abs = db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod &&
                    o.ABSENCE_TYPE_ID == TYP && o.OFF_ABS_GROUP_ID == pg && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID && o.PERSON_DATE_OWEN != null) ?
                    db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod &&
                    o.ABSENCE_TYPE_ID == TYP && o.OFF_ABS_GROUP_ID == pg && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID && o.PERSON_DATE_OWEN != null) : null;
                var own = old_abs != null ? old_abs.PERSON_DATE_OWEN : res;
                if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt && o.PERSON_DATE_OWEN == own))
                {
                    det.FIRMS_ABSENCES_PERSONS_DET_ID = max;
                    det.PERSON_CODE = PC2;
                    det.FIN_YEAR = y;
                    det.TRAINING_PERIOD_ID = priod;
                    det.FIRM_CODE = FIRM;
                    det.FROM_DATE = fdt;
                    det.ACT_TO_DATE = fdt.AddDays(1);
                    det.TO_DATE = fdt.Date.AddDays(1);
                    det.RANK_CAT_ID = (short)p2.RANK_CAT_ID;
                    det.PERSON_CAT_ID = (short)p2.PERSON_CAT_ID;
                    det.ABSENCE_TYPE_ID = TYP;
                    det.OFF_ABS_GROUP_ID = pg;
                    det.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                    det.PERSON_DATE_OWEN = own;
                    db.FIRMS_ABSENCES_PERSONS_DET.Add(det);
                    max += 1;
                }
                else
                {
                    var dt = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC2 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == fdt && o.PERSON_DATE_OWEN == own);
                    dt.DECTION = null;
                }
                //db.SaveChanges();
            }
#endregion

            #region P1
            if (DT2 != null)
            {

                // adding new absence to p1
                if (!db.FIRMS_ABSENCES_PERSONS.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC1 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2))
                {
                    var abs2 = new FIRMS_ABSENCES_PERSONS();
                    abs2.COMMANDER_FLAG = 0;
                    abs2.FIN_YEAR = y;
                    abs2.TRAINING_PERIOD_ID = priod;
                    abs2.ABSENCE_TYPE_ID = TYP;
                    abs2.FIRM_CODE = FIRM;
                    abs2.PERSON_CODE = PC1;
                    abs2.PERSON_CAT_ID = (short)p1.PERSON_CAT_ID;
                    abs2.RANK_CAT_ID = (short)p1.RANK_CAT_ID;
                    abs2.FROM_DATE = (DateTime)DT2;
                    abs2.TO_DATE = abs2.FROM_DATE.Date.AddDays(1);
                    abs2.DAY_STATUS = (short)(dnm == "Friday" ? 2 : dnm == "Thursday" ? 1 : 0);
                    abs2.ABSENCE_STATUS = 2;
                    abs2.ABS_REF = ser;
                    db.FIRMS_ABSENCES_PERSONS.Add(abs2);
                }
                else
                {
                    var p = db.FIRMS_ABSENCES_PERSONS.First(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC1 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2);
                    p.COMMANDER_FLAG = 0;
                }

                //update absdcence to pc1

                WF_EN en1 = new WF_EN();
                FIRMS_ABSENCES_PERSONS v1 = db.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2);
                //v1.COMMANDER_FLAG = 0;
                v1.ABSENCE_STATUS = 1;
                v1.ABS_REF = ser;
                en1.SaveChanges();
                /////////////////////////////
                // adding abs_det for p1
                if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC1 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2 && o.PERSON_DATE_OWEN == PC1))
                {
                    var d2 = new FIRMS_ABSENCES_PERSONS_DET();
                    d2.FIRMS_ABSENCES_PERSONS_DET_ID = max;
                    d2.ABSENCE_TYPE_ID = TYP;
                    d2.FIN_YEAR = y;
                    d2.TRAINING_PERIOD_ID = priod;
                    d2.PERSON_CODE = PC1;
                    d2.PERSON_CAT_ID = (short)p1.PERSON_CAT_ID;
                    d2.RANK_CAT_ID = (short)p1.RANK_CAT_ID;
                    d2.FIRM_CODE = FIRM;
                    d2.FROM_DATE = (DateTime)DT2;
                    d2.TO_DATE = d2.FROM_DATE.Date.AddDays(1);
                    d2.ACT_TO_DATE = d2.FROM_DATE.AddDays(1);
                    d2.PERSON_DATE_OWEN = PC1;
                    db.FIRMS_ABSENCES_PERSONS_DET.Add(d2);
                    max += 1;
                }
                // adding group steps for p1

                var pg1 = db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == PC1 && o.ABS_CAT_ID == 1) ?
                    db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == PC1 && o.ABS_CAT_ID == 1).OFF_ABS_GROUP_ID :
                    db.OFF_ABS_GROUP.First(o => o.UNIT_DEF_GROUP == 1 && o.ABSCENCE_CATEGORY_ID == 1).OFF_ABS_GROUP_ID;
                var steps1 = from o in db.OFF_ABS_STEPS
                             where o.OFF_ABS_GROUP_ID == pg1
                             select o;
                foreach (var s in steps1)
                {
                    var det = new FIRMS_ABSENCES_PERSONS_DET();
                    var x = @"  SELECT PD.PERSON_CODE
                                FROM PERSON_DATA PD, PERSON_JOBS R, OFF_ABS_STEPS STP
                                WHERE     PD.PERSON_CODE = R.PERSON_CODE
                                    AND STP.JOB_TYPE_ID = R.JOB_TYPE_ID
                                    AND R.TO_DATE is NULL
                                    AND STP.OFF_ABS_GROUP_ID = " + pg1 + @"
                                    AND STP.OFF_ABS_STEPS_ID = " + s.OFF_ABS_STEPS_ID;
                    var res = General.exec_f(x, "");
                    var old_abs = db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod &&
                        o.ABSENCE_TYPE_ID == TYP && o.OFF_ABS_GROUP_ID == pg && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID) ?
                        db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod &&
                        o.ABSENCE_TYPE_ID == TYP && o.OFF_ABS_GROUP_ID == pg && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID) : null;
                    var own = old_abs != null ? old_abs.PERSON_DATE_OWEN : res;
                    if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.FIRM_CODE == FIRM && o.FIN_YEAR == y && o.TRAINING_PERIOD_ID == priod && o.PERSON_CODE == PC1 && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2 && o.PERSON_DATE_OWEN == own))
                    {
                        det.FIRMS_ABSENCES_PERSONS_DET_ID = max;
                        det.PERSON_CODE = PC1;
                        det.FIN_YEAR = y;
                        det.TRAINING_PERIOD_ID = priod;
                        det.FIRM_CODE = FIRM;
                        det.FROM_DATE = (DateTime)DT2;
                        det.ACT_TO_DATE = ((DateTime)DT2).AddDays(1);
                        det.TO_DATE = ((DateTime)DT2).Date.AddDays(1);
                        det.RANK_CAT_ID = (short)p1.RANK_CAT_ID;
                        det.PERSON_CAT_ID = (short)p1.PERSON_CAT_ID;
                        det.ABSENCE_TYPE_ID = TYP;
                        det.OFF_ABS_GROUP_ID = pg;
                        det.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                        det.PERSON_DATE_OWEN = own;
                        db.FIRMS_ABSENCES_PERSONS_DET.Add(det);
                        max += 1;
                    }
                }
            }
            #endregion
            db.SaveChanges();


            var other_per_id = db.PERSON_DATA.First(o => o.PERSON_CODE == PC2);
            var per = db.PERSON_DATA.First(o => o.PERSON_CODE == PC1);
            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 8, 'قام " + per.PERSON_NAME + " بتخصيصك قائم بالعمل عنه', 0, " + other_per_id.PERSONAL_ID_NO + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + other_per_id.PERSONAL_ID_NO + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");
            message = "تمت الإضافة بنجاح";
            title = "تم";
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        public ActionResult del_exc(string FIRM, Int16 TYP, string PC1, string PC2, string F_DT, string T_DT)
        {
            var DT1 = DateTime.ParseExact(F_DT, "hhhh:mm:ss yyyy/MM/dd", CultureInfo.InvariantCulture);
            Nullable<DateTime> DT2 = null;
            var ex = new FIRMS_ABSENCE_EXCHANGE();
            if (T_DT != "")
            {
                DT2 = DateTime.ParseExact(T_DT, "hhhh:mm:ss yyyy/MM/dd", CultureInfo.InvariantCulture);
                ex = db.FIRMS_ABSENCE_EXCHANGE.First(o => o.ABSENCE_TYPE_ID == TYP && o.FIRM_CODE == FIRM && o.FROM_DATE == DT1 && o.FROM_PERSON_CODE == PC1 && o.TO_PERSON_CODE == PC2 && o.TO_DATE == DT2);
            }
            else
            {
                ex = db.FIRMS_ABSENCE_EXCHANGE.First(o => o.ABSENCE_TYPE_ID == TYP && o.FIRM_CODE == FIRM && o.FROM_DATE == DT1 && o.FROM_PERSON_CODE == PC1 && o.TO_PERSON_CODE == PC2 && o.TO_DATE == null);
            }

            db.FIRMS_ABSENCE_EXCHANGE.Remove(ex);
            db.SaveChanges();


            FIRMS_ABSENCES_PERSONS v = db.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT1);
            if (v.FROM_DATE >= DateTime.Now.Date)
            {
                if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT1 && o.DECTION != null))
                {
                    var steps = from o in db.FIRMS_ABSENCES_PERSONS_DET
                                where o.PERSON_CODE == PC2 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT1
                                select o;
                    foreach (var s in steps)
                    {
                        var det = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == s.FIRMS_ABSENCES_PERSONS_DET_ID && o.PERSON_CODE == PC2 &&
                            o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT1);
                        db.FIRMS_ABSENCES_PERSONS_DET.Remove(det);
                        db.SaveChanges();
                    }
                    var PERSON_CODE = PC2;
                    var TRAINING_PERIOD_ID = v.TRAINING_PERIOD_ID;
                    var FIN_YEAR = v.FIN_YEAR;
                    var FROM_DATE = DT1;//DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(x[4]);
                    var TO_DATE = v.TO_DATE; // != "" ? Convert.ToDateTime(x[8]) : DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); // Convert.ToDateTime(x[4]);                                          //DateTime.ParseExact(x[3], "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                    var FIRM_CODE = FIRM;
                    var RANK_CAT_ID = v.RANK_CAT_ID;
                    var PERSON_CAT_ID = v.PERSON_CAT_ID;
                    var sp = db.FIRMS_ABSENCES_PERSONS.First(o => o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.FROM_DATE == FROM_DATE && o.PERSON_CODE == PERSON_CODE && o.PERSON_CAT_ID == PERSON_CAT_ID &&
                        o.RANK_CAT_ID == RANK_CAT_ID && o.ABSENCE_TYPE_ID == TYP);
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

            if (DT1 != null)
            {
                FIRMS_ABSENCES_PERSONS v2 = db.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2);
                if (v2.FROM_DATE >= DateTime.Now.Date)
                {
                    if (!db.FIRMS_ABSENCES_PERSONS_DET.Any(o => o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2 && o.DECTION != null))
                    {
                        var steps = from o in db.FIRMS_ABSENCES_PERSONS_DET
                                    where o.PERSON_CODE == PC1 && o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2
                                    select o;
                        foreach (var s in steps)
                        {
                            var det = db.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == s.FIRMS_ABSENCES_PERSONS_DET_ID && o.PERSON_CODE == PC1 &&
                                o.FIRM_CODE == FIRM && o.ABSENCE_TYPE_ID == TYP && o.FROM_DATE == DT2);
                            db.FIRMS_ABSENCES_PERSONS_DET.Remove(det);
                            db.SaveChanges();
                        }
                        var PERSON_CODE = PC1;
                        var TRAINING_PERIOD_ID = v.TRAINING_PERIOD_ID;
                        var FIN_YEAR = v2.FIN_YEAR;
                        var FROM_DATE = DT2;//DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(x[4]);
                        var TO_DATE = v2.TO_DATE; // != "" ? Convert.ToDateTime(x[8]) : DateTime.ParseExact(x[4], "dd/MM/yyyy", CultureInfo.InvariantCulture); // Convert.ToDateTime(x[4]);                                          //DateTime.ParseExact(x[3], "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                        var FIRM_CODE = FIRM;
                        var RANK_CAT_ID = v2.RANK_CAT_ID;
                        var PERSON_CAT_ID = v2.PERSON_CAT_ID;
                        var sp = db.FIRMS_ABSENCES_PERSONS.First(o => o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.FROM_DATE == FROM_DATE && o.PERSON_CODE == PERSON_CODE && o.PERSON_CAT_ID == PERSON_CAT_ID &&
                            o.RANK_CAT_ID == RANK_CAT_ID && o.ABSENCE_TYPE_ID == TYP);
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
            }

            var other_per_id = db.PERSON_DATA.First(o => o.PERSON_CODE == PC2);
            var per = db.PERSON_DATA.First(o => o.PERSON_CODE == PC1);
            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 8, 'قام " + per.PERSON_NAME + " بتخصيصك قائم بالعمل عنه', 0, " + other_per_id.PERSONAL_ID_NO + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + other_per_id.PERSONAL_ID_NO + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");
            message = "تمت الحذف بنجاح";
            title = "تم";
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        //public static void exec_q(string qry, string CON)
        //{
        //    //string strConnString = @"";
        //    OracleConnection con = new OracleConnection(CON);
        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = qry;
        //    con.Open();
        //    cmd.ExecuteScalar();
        //    con.Close();
        //    //return x.ToString();
        //}
    }
}