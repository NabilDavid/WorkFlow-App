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
    public class TALAB_M2M_OKController : Controller
    {

        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;
        string title = "";
        string type = "success";
        static string sql = "";
        //
        // GET: /TALAB_M2M_OK/
         
        public ActionResult Index()
        {
          //  var x = db.FIRM_MISSIONS_DET.First(o=>o.ACT_TO_DATE != null).ACT_TO_DATE;
            //var firm_missions_det = db.FIRM_MISSIONS_DET.Include(f => f.OFF_ABS_STEPS).Include(f => f.PERSON_DATA).Include(f => f.FIRM_MISSIONS_MEMBERS);
            return View();
        }

        //
        // GET: /TALAB_M2M_OK/Details/5

        public ActionResult Details(decimal id = 0)
        {
            FIRM_MISSIONS_DET firm_missions_det = db.FIRM_MISSIONS_DET.Find(id);
            if (firm_missions_det == null)
            {
                return HttpNotFound();
            }
            return View(firm_missions_det);
        }

        //
        // GET: /TALAB_M2M_OK/Create

        public ActionResult Create()
        {
            ViewBag.OFF_ABS_STEPS_ID = new SelectList(db.OFF_ABS_STEPS, "OFF_ABS_STEPS_ID", "OFF_ABS_STEPS_NAME");
            ViewBag.PERSON_DATE_OWEN = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            ViewBag.FIN_YEAR = new SelectList(db.FIRM_MISSIONS_MEMBERS, "FIN_YEAR", "PERSON_MISSION");
            return View();
        }


        public ActionResult GET_grid_m2m(string firm_code, string fin_year, int P, string date, string person_id)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            WF_EN en =new WF_EN();
            var pers = en.PERSON_DATA.First(o => o.PERSONAL_ID_NO == person_id).PERSON_CODE;

            var q = @"SELECT DISTINCT
                            FIRM_MISSIONS.FIRM_CODE,
                            FIRM_MISSIONS.FIN_YEAR,
                            FIRM_MISSIONS.TRAINING_PERIOD_ID,
                            FIRM_MISSIONS.MISSION_ID AS ID,
                            OFF_ABS_STEPS.OFF_ABS_STEPS_NAME AS STP, 
                            SUBJECT AS OFF,
                             OFF_ABS_STEPS.OFF_ABS_STEPS_NAME
                            || ' لمأمورية الى  ' || FIRM_NAME || ' / ' || SUBJECT  AS SUBJECT,
                            TO_CHAR (FIRM_MISSIONS.FROM_DATE, 'dd/mm/yyyy') FROM_DATE,
                            TO_CHAR (FIRM_MISSIONS.TO_DATE, 'dd/mm/yyyy') TO_DATE_DT,
                            'مأمورية' AS TYP,
                            OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                            OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                            FIRM_MISSIONS_DET.PERSON_CODE,
                            FIRM_MISSIONS_DET_ID AS SEQ,
                            FIRM_MISSIONS.INTRODUCTION AS TITLE,
                            FIRM_MISSIONS_DET.PERSON_DATE_OWEN  AS COM,
                            STEP.ORD
                        FROM FIRM_MISSIONS_DET,
                            PERSON_DATA malktab,
                            PERSON_DATA tastab,
                            OFF_ABS_STEPS,
                            OFF_ABS_GROUP,
                            FIRM_MISSIONS,
                           (SELECT MIN (ST.ORDER_ID) AS ORD,
                                     DET.MISSION_ID AS SEQ,
                                     DET.PERSON_CODE
                                FROM OFF_ABS_STEPS st, FIRM_MISSIONS_DET det
                               WHERE st.OFF_ABS_STEPS_ID = det.OFF_ABS_STEPS_ID
                                     AND (DET.DECTION IS NULL OR DET.DECTION = 2)
                            GROUP BY DET.MISSION_ID, DET.PERSON_CODE) STEP
                        WHERE      FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID = OFF_ABS_GROUP.OFF_ABS_GROUP_ID(+)
                            AND FIRM_MISSIONS_DET.OFF_ABS_STEPS_ID = OFF_ABS_STEPS.OFF_ABS_STEPS_ID(+)
                            AND (FIRM_MISSIONS.FIRM_CODE = '" + firm_code + @"')
                            AND (FIRM_MISSIONS.FIN_YEAR = '" + fin_year + @"')
                            AND (FIRM_MISSIONS.TRAINING_PERIOD_ID = " + P + @") 
                         -- AND (TO_DATE ('" + date + @"', 'dd/mm/yyyy') BETWEEN FIRM_MISSIONS.FROM_DATE AND NVL (FIRM_MISSIONS.TO_DATE,TO_DATE ('" + date + @"','dd/mm/yyyy')))
                            AND FIRM_MISSIONS.FROM_DATE  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  FIRM_MISSIONS.TO_DATE <= NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy')) 
                            AND malktab.PERSON_CODE = FIRM_MISSIONS_DET.PERSON_CODE
                            AND tastab.PERSON_CODE = FIRM_MISSIONS_DET.PERSON_DATE_OWEN(+)
                            AND FIRM_MISSIONS_DET.MISSION_ID = STEP.SEQ
                            AND FIRM_MISSIONS_DET.PERSON_CODE = STEP.PERSON_CODE
                            AND OFF_ABS_STEPS.ORDER_ID = STEP.ORD
                            AND FIRM_MISSIONS_DET.DECTION IS NULL
                            AND FIRM_MISSIONS.FIRM_CODE = FIRM_MISSIONS_DET.FIRM_CODE
                            AND FIRM_MISSIONS.FIN_YEAR = FIRM_MISSIONS_DET.FIN_YEAR
                            AND FIRM_MISSIONS.TRAINING_PERIOD_ID =
                                    FIRM_MISSIONS_DET.TRAINING_PERIOD_ID
                            AND FIRM_MISSIONS.MISSION_ID = FIRM_MISSIONS_DET.MISSION_ID
                            AND FIRM_MISSIONS_DET.PERSON_DATE_OWEN = '" + pers + @"'
                    UNION
                    SELECT VAC.FIRM_CODE,
                            FIN.FIN_YEAR,
                            PER.TRAINING_PERIOD_ID,
                            VAC_DET.PERSON_VACATIONS_SEQ,
                            OFF_ABS_STEPS.OFF_ABS_STEPS_NAME AS STP,
                            persn_name.RANK || ' / ' || persn_name.PERSON_NAME as OFF,
                            OFF_ABS_STEPS.OFF_ABS_STEPS_NAME || ' عن ' || persn_name.RANK || ' / ' || persn_name.PERSON_NAME AS SUBJECT,
                            TO_CHAR (VAC.FROM_DATE, 'dd/mm/yyyy') FROM_DATE,
                            TO_CHAR (VAC.TO_DATE, 'dd/mm/yyyy') TO_DATE_DT,
                            'أجازة' AS TYP,
                            OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                            OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                            VAC_DET.PERSON_DATE_ID,
                            VAC_DET.PERSON_VACATIONS_DET_ID,
                            VTYP.NAME AS VAC_TYP,
                            VAC_DET.PERSON_DATE_OWEN,
                            STEP.ORD
                        FROM OFF_ABS_STEPS,
                            OFF_ABS_GROUP,
                            PERSON_VACATIONS_DET VAC_DET,
                            PERSON_VACATIONS VAC,
                            VACATION_TYPES VTYP,
                            (SELECT PERSON_DATA.PERSON_NAME, RANKS.RANK, PERSON_DATA.PERSON_CODE
                                FROM person_data, ranks
                                WHERE PERSON_DATA.RANK_ID = RANKS.RANK_ID) persn_name,
                            FINANCIAL_YEAR fin,
                            TRAINING_PERIODS PER,
                           (SELECT MIN (ST.ORDER_ID) AS ORD,
                                     DET.PERSON_VACATIONS_SEQ AS SEQ,
                                     DET.PERSON_DATE_ID
                                FROM OFF_ABS_STEPS st, PERSON_VACATIONS_DET det
                               WHERE st.OFF_ABS_STEPS_ID = det.OFF_ABS_STEPS_ID
                                     AND (DET.DECTION IS NULL OR DET.DECTION = 2)
                            GROUP BY DET.PERSON_VACATIONS_SEQ, DET.PERSON_DATE_ID) STEP
                        WHERE     VAC_DET.OFF_ABS_GROUP_ID = OFF_ABS_GROUP.OFF_ABS_GROUP_ID(+)
                            AND VAC_DET.OFF_ABS_STEPS_ID = OFF_ABS_STEPS.OFF_ABS_STEPS_ID(+)
                            AND VAC_DET.PERSON_DATE_ID = persn_name.PERSON_CODE(+)
                            --AND TO_CHAR (VAC.REQUEST_DATE, 'dd/mm/yyyy')  = '" + date + @"'
                            AND VAC_DET.PERSON_DATE_OWEN = '" + pers + @"'
                            AND VAC.SEQ = VAC_DET.PERSON_VACATIONS_SEQ
                            AND VAC.PERSON_CODE = VAC_DET.PERSON_DATE_ID
                            AND VAC.VACATION_TYPE_ID = VTYP.VACATION_TYPE_ID
                            AND VAC_DET.PERSON_VACATIONS_SEQ = STEP.SEQ
                            AND VAC_DET.PERSON_DATE_ID = STEP.PERSON_DATE_ID
                            AND OFF_ABS_STEPS.ORDER_ID = STEP.ORD
                            AND VAC_DET.DECTION IS NULL
                            AND VAC.FIRM_CODE = '" + firm_code + @"'
                            AND FIN.IS_CURRENT = 1      
                            AND (TO_DATE ('" + date + @"', 'dd/mm/yyyy')) BETWEEN PER.PERIOD_FROM
                                                                            AND PER.PERIOD_TO
                        UNION
                        SELECT DET.FIRM_CODE,
                                DET.FIN_YEAR,
                                DET.TRAINING_PERIOD_ID,
                                DET.ABSENCE_TYPE_ID,
                                OFF_ABS_STEPS.OFF_ABS_STEPS_NAME AS STP,
                                persn_name.RANK || ' / ' || persn_name.PERSON_NAME as OFF,
                                OFF_ABS_STEPS.OFF_ABS_STEPS_NAME  || ' عن '  || persn_name.RANK  || ' / ' || persn_name.PERSON_NAME  AS SUBJECT,
                                TO_CHAR (AB.FROM_DATE, 'dd/mm/yyyy HH24:mi:ss') FROM_DATE,
                                TO_CHAR (AB.TO_DATE, 'dd/mm/yyyy'),
                                TYP.NAME,
                                OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                                OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                                DET.PERSON_CODE,
                                DET.FIRMS_ABSENCES_PERSONS_DET_ID,
                                TYP.NAME,
                                DET.PERSON_DATE_OWEN,
                                OFF_ABS_STEPS.ORDER_ID
                        FROM OFF_ABS_STEPS,
                                FIRMS_ABSENCES_PERSONS_DET DET,
                                FIRMS_ABSENCES_PERSONS AB,
                                ABSENCE_TYPES TYP,
                                (SELECT DISTINCT
                                        PERSON_DATA.PERSON_NAME, RANKS.RANK, PERSON_DATA.PERSON_CODE
                                FROM person_data, FIRMS_ABSENCES_PERSONS_DET, ranks
                                WHERE FIRMS_ABSENCES_PERSONS_DET.PERSON_CODE =
                                        person_data.PERSON_CODE
                                        AND PERSON_DATA.RANK_ID = RANKS.RANK_ID) PERSN_NAME,
                                (  SELECT MIN (ST.ORDER_ID) AS ORD, DET.PERSON_CODE, ABSENCE_TYPE_ID, DET.FIRM_CODE, DET.FIN_YEAR, DET.TRAINING_PERIOD_ID, DET.FROM_DATE
                                    FROM OFF_ABS_STEPS st, FIRMS_ABSENCES_PERSONS_DET det
                                    WHERE st.OFF_ABS_STEPS_ID = det.OFF_ABS_STEPS_ID
                                        AND DET.DECTION IS NULL
                                        --AND TO_CHAR (DET.FROM_DATE, 'dd/mm/yyyy') ='" + date + @"' 
                                    GROUP BY DET.PERSON_CODE, ABSENCE_TYPE_ID, DET.FIRM_CODE, DET.FIN_YEAR, DET.TRAINING_PERIOD_ID, DET.FROM_DATE) STEP
                        WHERE     DET.OFF_ABS_STEPS_ID = OFF_ABS_STEPS.OFF_ABS_STEPS_ID(+)
                                AND DET.PERSON_CODE = PERSN_NAME.PERSON_CODE
                                AND DET.ABSENCE_TYPE_ID = TYP.ABSENCE_TYPE_ID
                                AND DET.ABSENCE_TYPE_ID = AB.ABSENCE_TYPE_ID
                                AND DET.FIRM_CODE = AB.FIRM_CODE
                                AND DET.FIN_YEAR = AB.FIN_YEAR
                                AND DET.TRAINING_PERIOD_ID = AB.TRAINING_PERIOD_ID
                                AND DET.FROM_DATE = AB.FROM_DATE
                                AND DET.PERSON_CODE = AB.PERSON_CODE
                                AND DET.PERSON_CODE = AB.PERSON_CODE
                                AND DET.RANK_CAT_ID = AB.RANK_CAT_ID
                                AND DET.PERSON_CAT_ID = AB.PERSON_CAT_ID
                                AND DET.ABSENCE_TYPE_ID = STEP.ABSENCE_TYPE_ID
                                AND DET.FIRM_CODE = STEP.FIRM_CODE
                                AND DET.PERSON_CODE = STEP.PERSON_CODE
                                AND DET.FIN_YEAR = STEP.FIN_YEAR
                                AND DET.TRAINING_PERIOD_ID = STEP.TRAINING_PERIOD_ID
                                AND DET.FROM_DATE = STEP.FROM_DATE
                                AND DET.DECTION IS NULL
                                AND (OFF_ABS_STEPS.ORDER_ID = STEP.ORD  OR   DET.OFF_ABS_STEPS_ID IS NULL)        
                                AND DET.PERSON_DATE_OWEN = '" + pers + @"'
                                AND DET.FIRM_CODE = '" + firm_code + @"'
                                --AND TO_CHAR (DET.FROM_DATE, 'dd/mm/yyyy') ='" + date + @"'";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            sql = q;
            //all_ok();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_grid_m2m_mem(string firm_code, string fin_year, int P, string date, string person_id, string MISSION_ID)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            WF_EN en = new WF_EN();
            var pers = en.PERSON_DATA.First(o => o.PERSONAL_ID_NO == person_id).PERSON_CODE;
            string q = @" 
                            SELECT DISTINCT(FIRM_MISSIONS_DET.PERSON_CODE) AS MALK,
                                    FIRM_MISSIONS.FIRM_CODE,
                                    FIRM_MISSIONS.FIN_YEAR,
                                    FIRM_MISSIONS.TRAINING_PERIOD_ID,
                                    FIRM_MISSIONS_DET.PERSON_DATE_OWEN AS TASDEK,
                                    RANKS.RANK,
                                    MALKTAB.PERSON_NAME AS MALK_NAME,
                                    tastab.PERSON_NAME AS TASDEK_NAME,
                                    FIRM_MISSIONS.INTRODUCTION,
                                    FIRM_MISSIONS.MISSION_ID,
                                    FIRM_MISSIONS.SUBJECT,
                                    FIRM_MISSIONS.FROM_DATE,
                                    FIRM_MISSIONS.TO_DATE
                            FROM FIRM_MISSIONS_DET,
                                    PERSON_DATA malktab,
                                    PERSON_DATA tastab,
                                    FIRM_MISSIONS,
                                    ranks
                            WHERE     (FIRM_MISSIONS.FIRM_CODE = '" + firm_code + @"')
                                    AND (FIRM_MISSIONS.FIN_YEAR = '" + fin_year + @"')
                                    AND (FIRM_MISSIONS.TRAINING_PERIOD_ID = " + P + @")
                                    AND RANKS.RANK_ID = malktab.RANK_ID
                                    --AND (TO_DATE ('" + date + @"', 'dd/mm/yyyy') BETWEEN FIRM_MISSIONS.FROM_DATE AND NVL (FIRM_MISSIONS.TO_DATE,TO_DATE ('" + date + @"','dd/mm/yyyy')))
                                    AND FIRM_MISSIONS.FROM_DATE  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  FIRM_MISSIONS.TO_DATE <= NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy')) 
                                    AND malktab.PERSON_CODE = FIRM_MISSIONS_DET.PERSON_CODE
                                    AND tastab.PERSON_CODE = FIRM_MISSIONS_DET.PERSON_DATE_OWEN
                                    AND FIRM_MISSIONS_DET.DECTION IS NULL
                                    AND FIRM_MISSIONS.FIRM_CODE = FIRM_MISSIONS_DET.FIRM_CODE
                                    AND FIRM_MISSIONS.FIN_YEAR = FIRM_MISSIONS_DET.FIN_YEAR
                                    AND FIRM_MISSIONS.TRAINING_PERIOD_ID =
                                        FIRM_MISSIONS_DET.TRAINING_PERIOD_ID
                                    AND FIRM_MISSIONS.MISSION_ID = FIRM_MISSIONS_DET.MISSION_ID
                                    AND FIRM_MISSIONS_DET.MISSION_ID = " + MISSION_ID + @"
                                    AND FIRM_MISSIONS_DET.PERSON_DATE_OWEN = " + pers + @"
                        ORDER BY FIRM_MISSIONS.MISSION_ID";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /TALAB_M2M_OK/Create

        [HttpPost]
        public ActionResult Create(FIRM_MISSIONS_DET firm_missions_det)
        {
            if (ModelState.IsValid)
            {
                db.FIRM_MISSIONS_DET.Add(firm_missions_det);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OFF_ABS_STEPS_ID = new SelectList(db.OFF_ABS_STEPS, "OFF_ABS_STEPS_ID", "OFF_ABS_STEPS_NAME", firm_missions_det.OFF_ABS_STEPS_ID);
            ViewBag.PERSON_DATE_OWEN = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_det.PERSON_DATE_OWEN);
            ViewBag.FIN_YEAR = new SelectList(db.FIRM_MISSIONS_MEMBERS, "FIN_YEAR", "PERSON_MISSION", firm_missions_det.FIN_YEAR);
            return View(firm_missions_det);
        }

        //
        // GET: /TALAB_M2M_OK/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            FIRM_MISSIONS_DET firm_missions_det = db.FIRM_MISSIONS_DET.Find(id);
            if (firm_missions_det == null)
            {
                return HttpNotFound();
            }
            ViewBag.OFF_ABS_STEPS_ID = new SelectList(db.OFF_ABS_STEPS, "OFF_ABS_STEPS_ID", "OFF_ABS_STEPS_NAME", firm_missions_det.OFF_ABS_STEPS_ID);
            ViewBag.PERSON_DATE_OWEN = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_det.PERSON_DATE_OWEN);
            ViewBag.FIN_YEAR = new SelectList(db.FIRM_MISSIONS_MEMBERS, "FIN_YEAR", "PERSON_MISSION", firm_missions_det.FIN_YEAR);
            return View(firm_missions_det);
        }

        //
        // POST: /TALAB_M2M_OK/Edit/5

        [HttpPost]
        public ActionResult Edit(FIRM_MISSIONS_DET firm_missions )
        {
            if (ModelState.IsValid)
            {
                WF_EN E = new WF_EN();
                //FIRM_MISSIONS_DET f = new FIRM_MISSIONS_DET();
                var ff = from o in E.FIRM_MISSIONS_DET
                    where o.MISSION_ID == firm_missions.MISSION_ID && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE
                        && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID && o.PERSON_DATE_OWEN == firm_missions.PERSON_DATE_OWEN && o.FIRM_MISSIONS_DET_ID == firm_missions.FIRM_MISSIONS_DET_ID
                    select o;
                var date = E.FIRM_MISSIONS.First(o => o.MISSION_ID == firm_missions.MISSION_ID && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID);
                var from = date.FROM_DATE;
                var to = date.TO_DATE;
                if (firm_missions.PERSON_CODE != null)
                {
                    ff = ff.Where(o=>o.PERSON_CODE == firm_missions.PERSON_CODE);
                }
                if (from >= DateTime.Today && to >= DateTime.Today)
                {
                    foreach (var m in ff)
	                {
                        FIRM_MISSIONS_DET f = new FIRM_MISSIONS_DET();
                        f = m;
                        string name = "";
                        var pers_name = from o in E.PERSON_DATA
                                        where o.PERSON_CODE == f.PERSON_DATE_OWEN
                                        select o;
                        foreach (var p in pers_name)
                        {
                            name = p.RANKS.RANK + " / " + p.PERSON_NAME;
                        }
                        string xx = "";
                        var pers = E.PERSON_DATA.First(o => o.PERSON_CODE == m.PERSON_CODE);
                        xx = pers.PERSONAL_ID_NO;
                        f.DECTION = firm_missions.DECTION;
                        E.SaveChanges();

                        var leader = (from o in E.FIRM_MISSIONS_DET
                                      from s in E.OFF_ABS_STEPS
                                      where o.MISSION_ID == m.MISSION_ID && o.PERSON_CODE == m.PERSON_CODE && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID
                                      select s.ORDER_ID).Max();
                        var x = f.OFF_ABS_STEPS.ORDER_ID == leader;
                        if (firm_missions.DECTION == 1)
                        {
                                var MS = db.FIRM_MISSIONS.First(O=>O.MISSION_ID == m.MISSION_ID && O.FIRM_CODE == m.FIRM_CODE &&
                                    O.FIN_YEAR == m.FIN_YEAR && O.TRAINING_PERIOD_ID == m.TRAINING_PERIOD_ID);
                            if (x && MS.TO_DATE == MS.TO_DATE)
                            {
                                var y = E.TRAINING_YEARS.First(o => o.IS_CURRENT == 1);
                                var p = E.TRAINING_PERIODS.First(o => o.FIN_YEAR == y.FIN_YEAR && o.PERIOD_FROM <= MS.FROM_DATE && o.PERIOD_TO >= MS.FROM_DATE);
                                FIRMS_ABSENCES_PERSONS OBJ = new FIRMS_ABSENCES_PERSONS();
                                OBJ.PERSON_CODE = m.PERSON_CODE;
                                OBJ.FIN_YEAR = y.FIN_YEAR;
                                OBJ.TRAINING_PERIOD_ID = p.TRAINING_PERIOD_ID;
                                OBJ.FIRM_CODE = m.FIRM_CODE;
                                OBJ.RANK_CAT_ID = (short)pers.RANK_CAT_ID;
                                OBJ.PERSON_CAT_ID = (short)pers.PERSON_CAT_ID;
                                OBJ.ABSENCE_TYPE_ID = (short)MS.MISSION_TYPE;
                                OBJ.FROM_DATE = (DateTime)MS.FROM_DATE;
                                OBJ.TO_DATE = (DateTime)MS.TO_DATE;
                                OBJ.ACT_DATE = (DateTime)MS.TO_DATE;
                                OBJ.ACT_DATE = (DateTime)MS.TO_DATE;
                                OBJ.ABS_REF = MS.MISSION_ID;
                                OBJ.ABSENCE_NOTES = MS.FIRM_NAME;
                                OBJ.COMMANDER_FLAG = 1;
                                E.FIRMS_ABSENCES_PERSONS.Add(OBJ);
                                E.SaveChanges();

                                var det = from o in db.FIRM_MISSIONS_DET
                                              where o.PERSON_CODE == m.PERSON_CODE && o.MISSION_ID == m.MISSION_ID
                                              select o;
                                foreach (var i in det)
                                {
                                    var ab_dt = new FIRMS_ABSENCES_PERSONS_DET();
                                    ab_dt.FIRMS_ABSENCES_PERSONS_DET_ID = db.FIRMS_ABSENCES_PERSONS_DET.Any() ? (db.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1) : 1;
                                    ab_dt.ABSENCE_TYPE_ID = (short)MS.MISSION_TYPE;
                                    ab_dt.FIN_YEAR = y.FIN_YEAR;
                                    ab_dt.FIRM_CODE = MS.FIRM_CODE;
                                    ab_dt.PERSON_CODE = m.PERSON_CODE;
                                    ab_dt.PERSON_CAT_ID = (short)pers.PERSON_CAT_ID;
                                    ab_dt.RANK_CAT_ID = (short)pers.RANK_CAT_ID;
                                    ab_dt.TRAINING_PERIOD_ID = p.TRAINING_PERIOD_ID;
                                    ab_dt.FROM_DATE = (DateTime)MS.FROM_DATE;
                                    ab_dt.TO_DATE = (DateTime)MS.TO_DATE;
                                    ab_dt.ACT_TO_DATE = (DateTime)MS.TO_DATE;
                                    ab_dt.OFF_ABS_STEPS_ID = m.OFF_ABS_STEPS_ID;
                                    ab_dt.OFF_ABS_GROUP_ID = m.OFF_ABS_GROUP_ID;
                                    ab_dt.PERSON_DATE_OWEN = m.PERSON_DATE_OWEN;
                                    ab_dt.DECTION = m.DECTION;
                                    db.FIRMS_ABSENCES_PERSONS_DET.Add(ab_dt);
                                    db.SaveChanges();

                                }
                            }
                            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالتصديق ع المأمورية', 1, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M?nn=" + xx + " ')";
                            General.exec_q(Q, "");
                            General.exec_q("COMMIT", "");

                            message = "تم التصديق على المأمورية";
                            status = true;
                            title = "تم التصديق";
                            type = "success";

                        }
                        else
                        {
                            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + "  رفض المأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M?nn=" + xx + " ')";
                            General.exec_q(Q, "");
                            General.exec_q("COMMIT", "");

                            message = "تم رفض المأمورية";
                            status = true;
                            title = "تم الرفض";
                            type = "error";
                        }
                        status = true;
		 
                    }
                    
                }
                else
                {
                    status = false;
                    message = " تاريخ المأمورية أقل من تاريخ اليوم .";
                }
            }
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }


        [HttpPost]
        public ActionResult vac_ok(PERSON_VACATIONS_DET vac)
        {
            if (ModelState.IsValid)
            {
                WF_EN E = new WF_EN();
                PERSON_VACATIONS_DET f = new PERSON_VACATIONS_DET();
                var par_v = E.PERSON_VACATIONS.First(o => o.SEQ == vac.PERSON_VACATIONS_SEQ && o.PERSON_CODE == vac.PERSON_DATE_ID);
                f = E.PERSON_VACATIONS_DET.First(o => o.PERSON_VACATIONS_DET_ID == vac.PERSON_VACATIONS_DET_ID && o.PERSON_VACATIONS_SEQ == vac.PERSON_VACATIONS_SEQ && o.PERSON_DATE_ID == vac.PERSON_DATE_ID);
                var date = E.PERSON_VACATIONS.First(o => o.PERSON_CODE == vac.PERSON_DATE_ID && o.SEQ == vac.PERSON_VACATIONS_SEQ);
                var from = date.FROM_DATE;
                var to = date.TO_DATE;
                var leader = (from o in E.PERSON_VACATIONS_DET
                             from s in E.OFF_ABS_STEPS
                             where o.PERSON_VACATIONS_SEQ == vac.PERSON_VACATIONS_SEQ && o.PERSON_DATE_ID == vac.PERSON_DATE_ID && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID
                                 select s.ORDER_ID).Max() ;
                var x = f.OFF_ABS_STEPS.ORDER_ID == leader;
                if (from >= DateTime.Today && to >= DateTime.Today)
                {
                    string name = "";
                    var pers_name = E.PERSON_DATA.First(o => o.PERSON_CODE == vac.PERSON_DATE_OWEN);
                    name = pers_name.RANKS.RANK + " / " + pers_name.PERSON_NAME;
                    string xx = "";
                    var pers = E.PERSON_DATA.First(o => o.PERSON_CODE == vac.PERSON_DATE_ID);
                    xx = pers.PERSONAL_ID_NO;

                    f.DECTION = vac.DECTION;
                    //E.SaveChanges();
                    if (f.DECTION == 1)
                    {

                        if (x)
                        {
                            
                            var y = E.TRAINING_YEARS.First(o => o.IS_CURRENT == 1);
                            var p = E.TRAINING_PERIODS.First(o => o.FIN_YEAR == y.FIN_YEAR && o.PERIOD_FROM <= from && o.PERIOD_TO >= from);
                            FIRMS_ABSENCES_PERSONS OBJ = new FIRMS_ABSENCES_PERSONS();
                            OBJ.PERSON_CODE = vac.PERSON_DATE_ID;
                            OBJ.FIN_YEAR = y.FIN_YEAR;
                            OBJ.TRAINING_PERIOD_ID = p.TRAINING_PERIOD_ID;
                            OBJ.FIRM_CODE = par_v.FIRM_CODE;
                            OBJ.RANK_CAT_ID = (short)pers.RANK_CAT_ID;
                            OBJ.PERSON_CAT_ID = (short)pers.PERSON_CAT_ID;
                            OBJ.ABSENCE_TYPE_ID = (short)par_v.VACATION_TYPE_ID;
                            OBJ.FROM_DATE = (DateTime)par_v.FROM_DATE;
                            OBJ.TO_DATE = (DateTime)par_v.TO_DATE;
                            OBJ.ABS_REF = vac.PERSON_VACATIONS_SEQ;
                            OBJ.ACT_DATE = (DateTime)par_v.TO_DATE;
                            OBJ.COMMANDER_FLAG = 1;
                            E.FIRMS_ABSENCES_PERSONS.Add(OBJ);
                            //E.SaveChanges();
                            var seq = db.FIRMS_ABSENCES_PERSONS_DET.Any() ? (db.FIRMS_ABSENCES_PERSONS_DET.Max(o => o.FIRMS_ABSENCES_PERSONS_DET_ID) + 1) : 1;
                            var vac_det = from o in db.PERSON_VACATIONS_DET
                                          where o.PERSON_DATE_ID == vac.PERSON_DATE_ID && o.PERSON_VACATIONS_SEQ == vac.PERSON_VACATIONS_SEQ
                                          select o;
                            foreach (var i in vac_det)
                            {
                                var ab_dt = new FIRMS_ABSENCES_PERSONS_DET();
                                ab_dt.FIRMS_ABSENCES_PERSONS_DET_ID = seq;
                                ab_dt.ABSENCE_TYPE_ID = (short)par_v.VACATION_TYPE_ID;
                                ab_dt.FIN_YEAR = y.FIN_YEAR;
                                ab_dt.FIRM_CODE = par_v.FIRM_CODE;
                                ab_dt.PERSON_CODE = vac.PERSON_DATE_ID;
                                ab_dt.PERSON_CAT_ID = (short)pers.PERSON_CAT_ID;
                                ab_dt.RANK_CAT_ID = (short)pers.RANK_CAT_ID;
                                ab_dt.TRAINING_PERIOD_ID = p.TRAINING_PERIOD_ID;
                                ab_dt.FROM_DATE = (DateTime)par_v.FROM_DATE;
                                ab_dt.TO_DATE = (DateTime)par_v.TO_DATE;
                                ab_dt.OFF_ABS_STEPS_ID = vac.OFF_ABS_STEPS_ID;
                                ab_dt.OFF_ABS_GROUP_ID = vac.OFF_ABS_GROUP_ID;
                                ab_dt.PERSON_DATE_OWEN = vac.PERSON_DATE_OWEN;
                                ab_dt.DECTION = vac.DECTION;
                                ab_dt.ACT_TO_DATE = (DateTime)par_v.TO_DATE;
                                E.FIRMS_ABSENCES_PERSONS_DET.Add(ab_dt);
                                //db.SaveChanges();
                                seq++;
                            }
                            par_v.COMANDER_DECESION = 1;
                            //db.SaveChanges();
                        }

                        E.SaveChanges();
                        //db.SaveChanges();

                        var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + name + " بالتصديق على طلب الأجازة', 1, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/SD_VAC?nn=" + xx + " ')";
                        General.exec_q(Q, "");
                        General.exec_q("COMMIT", "");

                        message = "تم التصديق على طلب الأجازة";
                        status = true;
                        title = "تم التصديق";
                        type = "success";
                    }
                    else
                    {
                        var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + name + "  رفض طلب الأجازة', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/SD_VAC?nn=" + xx + " ')";
                        General.exec_q(Q, "");
                        General.exec_q("COMMIT", "");

                        message = "تم رفض طلب الأجازة";
                        status = true;
                        title = "تم الرفض";
                        type = "error";
                    }
                }
                else
                {
                    status = false;
                    message = " لا يمكن أتمام الأمر .";
                }
            }

            return new JsonResult { Data = new { status = status, message = message, type= type, title = title  } };
        }


        [HttpPost]
        public ActionResult abs_ok(FIRMS_ABSENCES_PERSONS_DET det)
        {
            if (ModelState.IsValid)
            {
                WF_EN E = new WF_EN();
                FIRMS_ABSENCES_PERSONS_DET f = new FIRMS_ABSENCES_PERSONS_DET();
                f = E.FIRMS_ABSENCES_PERSONS_DET.First(o => o.FIRMS_ABSENCES_PERSONS_DET_ID == det.FIRMS_ABSENCES_PERSONS_DET_ID && o.FIN_YEAR == det.FIN_YEAR && o.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID
                    && o.PERSON_CODE == det.PERSON_CODE && o.FROM_DATE == det.FROM_DATE && o.FIRM_CODE == det.FIRM_CODE && o.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID);
                var leader = (from o in E.FIRMS_ABSENCES_PERSONS_DET
                              from s in E.OFF_ABS_STEPS
                              where o.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && o.PERSON_CODE == det.PERSON_CODE && o.OFF_ABS_STEPS_ID == s.OFF_ABS_STEPS_ID && o.FROM_DATE == det.FROM_DATE && o.TO_DATE == f.TO_DATE
                              select s.ORDER_ID).Max();
                var x = f.OFF_ABS_STEPS_ID != null? f.OFF_ABS_STEPS.ORDER_ID == leader : false;
                //if (f.FROM_DATE >= DateTime.Today && f.TO_DATE >= DateTime.Today)
                //{
                    string name = "";
                    var pers_name = E.PERSON_DATA.First(o => o.PERSON_CODE == det.PERSON_DATE_OWEN);
                    name = pers_name.RANKS.RANK + " / " + pers_name.PERSON_NAME;
                    string xx = "";
                    var pers = E.PERSON_DATA.First(o => o.PERSON_CODE == det.PERSON_CODE);
                    xx = pers.PERSONAL_ID_NO;

                    f.DECTION = det.DECTION;
                    E.SaveChanges();
                    if (f.DECTION == 1)
                    {
                        try
                        {
                            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + name + " بالتصديق على طلب الأجازة', 1, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/SD_VAC?nn=" + xx + " ')";
                            General.exec_q(Q, "");
                            General.exec_q("COMMIT", "");
                        }
                        catch 
                        {
                            
                        }

                        message = "تم التصديق على الطلب";
                        status = true;
                        title = "تم التصديق";
                        type = "success";
                        if (x)
                        {
                            var tdt = f.TO_DATE.Value.Date;
                            var pa = db.FIRMS_ABSENCES_PERSONS.First(p=>p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                            p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.FROM_DATE == det.FROM_DATE );
                            var seq = pa.ABS_REF != null ? pa.ABS_REF : null;
                            pa.COMMANDER_FLAG = 1;
                            pa.ABSENCE_STATUS = null;
                            List<int> x3 = new List<int> { 11, 48 };
                            pa.ACT_DATE = x3.Contains(pa.ABSENCE_TYPE_ID) ? f.TO_DATE : f.FROM_DATE.AddDays(1);
                            pa.TO_DATE = f.TO_DATE.Value.Date;
                            db.SaveChanges();
                            var exc = seq != null? db.FIRMS_ABSENCE_EXCHANGE.First(o => o.FIRM_CODE == det.FIRM_CODE && o.SEQ == seq) : null;
                            if (exc != null)
                            {
                                if (exc.TO_DATE == null)
                                {
                                    var pao2 = db.FIRMS_ABSENCES_PERSONS.First(p => p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == exc.FROM_PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                           p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.ABSENCE_STATUS == 1 && p.ABS_REF == seq);
                                    var det1 = from p in db.FIRMS_ABSENCES_PERSONS_DET
                                               where p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == exc.FROM_PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                           p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.FROM_DATE == pao2.FROM_DATE
                                               select p;
                                    foreach (var o in det1)
                                    {
                                       db.FIRMS_ABSENCES_PERSONS_DET.Remove(o); 
                                    }
                                    //pao2.ABSENCE_STATUS = 2;
                                    //pao2.COMMANDER_FLAG = 0;
                                    db.FIRMS_ABSENCES_PERSONS.Remove(pao2);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    if (db.FIRMS_ABSENCES_PERSONS.Any(p => p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                        p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.ABSENCE_STATUS == 1 && p.ABS_REF == seq))
                                    {
                                        var pao = db.FIRMS_ABSENCES_PERSONS.First(p => p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                        p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.ABSENCE_STATUS == 1 && p.ABS_REF == seq);
                                        var det2 = from p in db.FIRMS_ABSENCES_PERSONS_DET
                                                   where p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                               p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.FROM_DATE == pao.FROM_DATE
                                                   select p;
                                        foreach (var o in det2)
                                        {
                                            db.FIRMS_ABSENCES_PERSONS_DET.Remove(o);
                                        }
                                        //pao.ABSENCE_STATUS = 2;
                                        //pao.COMMANDER_FLAG = 0;
                                        db.FIRMS_ABSENCES_PERSONS.Remove(pao);
                                        db.SaveChanges();
                                    }
                                }
                                
                            }
                            else
                            {
                                //if (db.FIRMS_ABSENCES_PERSONS.Any(p => p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                //    p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.ABSENCE_STATUS == 1 && p.ABS_REF == seq))
                                //{
                                //    var pao = db.FIRMS_ABSENCES_PERSONS.First(p => p.ABSENCE_TYPE_ID == det.ABSENCE_TYPE_ID && p.PERSON_CODE == det.PERSON_CODE && p.FIN_YEAR == det.FIN_YEAR &&
                                //    p.TRAINING_PERIOD_ID == det.TRAINING_PERIOD_ID && p.FIRM_CODE == det.FIRM_CODE && p.ABSENCE_STATUS == 1 && p.ABS_REF == seq);
                                //    pao.ABSENCE_STATUS = 2;
                                //    pao.COMMANDER_FLAG = 0;
                                //    db.SaveChanges();
                                //}
                            }
                            if (!db.FIRMS_ABSENCES_PERSONS.Any(o => o.ABS_REF == seq && o.FIRM_CODE == det.FIRM_CODE && o.ABSENCE_STATUS == 1) && exc != null)
                            {
                                exc.IS_APPROVED = 1;
                                db.SaveChanges();
                            }
                            
                        }

                    }
                    else
                    {
                        try
                        {
                            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 5, 'قام " + name + "  رفض الطلب , 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/KHDMA_CHANG?nn= " + xx + " ')";
                            General.exec_q(Q, "");
                            General.exec_q("COMMIT", "");
                        }
                        catch 
                        {

                        }

                        message = "تم رفض الطلب";
                        status = true;
                        title = "تم الرفض";
                        type = "error";
                    }
                //}
                //else
                //{
                //    status = false;
                //    message = " لا يمكن أتمام الأمر .";
                //}
            }

            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }

        //
        // GET: /TALAB_M2M_OK/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            FIRM_MISSIONS_DET firm_missions_det = db.FIRM_MISSIONS_DET.Find(id);
            if (firm_missions_det == null)
            {
                return HttpNotFound();
            }
            return View(firm_missions_det);
        }

        //
        // POST: /TALAB_M2M_OK/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            FIRM_MISSIONS_DET firm_missions_det = db.FIRM_MISSIONS_DET.Find(id);
            db.FIRM_MISSIONS_DET.Remove(firm_missions_det);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult all_ok()
        {
            OracleCommand cmd = new OracleCommand(sql);
            var data = General.GetData_New(cmd);
            foreach (var r in data)
            {
                var item = r.Values.ToList();
                var typ = item[7].ToString() == "مأمورية" ? 1 : r.Values.ToList()[7].ToString() == "أجازة" ? 2 : 3;
                if (typ == 1)
                {
                    var m = new FIRM_MISSIONS_DET()
                    {
                        FIRM_CODE = item[0].ToString(),
                        FIRM_MISSIONS_DET_ID = Convert.ToDecimal(item[11]),
                        FIN_YEAR = item[1].ToString(),
                        TRAINING_PERIOD_ID = Convert.ToInt16(item[2]),
                        MISSION_ID = Convert.ToInt16(item[3]),
                        PERSON_CODE = item[10].ToString(),
                        OFF_ABS_STEPS_ID = Convert.ToDecimal(item[8]),
                        OFF_ABS_GROUP_ID = Convert.ToDecimal(item[9]),
                        DECTION = 1,
                        PERSON_DATE_OWEN = item[13].ToString()
                    };
                    Edit(m);
                }
                else if (typ == 2)
                {
                    var v = new PERSON_VACATIONS_DET()
                    {
                        PERSON_VACATIONS_DET_ID = Convert.ToDecimal(item[11]),
                        PERSON_VACATIONS_SEQ = Convert.ToInt16(item[3]),
                        PERSON_DATE_ID = item[10].ToString(),
                        OFF_ABS_STEPS_ID = Convert.ToDecimal(item[8]),
                        OFF_ABS_GROUP_ID = Convert.ToDecimal(item[9]),
                        DECTION = 1,
                        PERSON_DATE_OWEN = item[13].ToString()
                    };
                    vac_ok(v);
                }
                else
                {
                    var abs = new FIRMS_ABSENCES_PERSONS_DET()
                    {
                        FIRM_CODE = item[0].ToString(),
                        FIRMS_ABSENCES_PERSONS_DET_ID = Convert.ToDecimal(item[11]),
                        FIN_YEAR = item[1].ToString(),
                        TRAINING_PERIOD_ID = Convert.ToInt16(item[2]),
                        ABSENCE_TYPE_ID = Convert.ToInt16(item[3]),
                        PERSON_CODE = item[10].ToString(),
                        OFF_ABS_STEPS_ID = Convert.ToDecimal(item[8]),
                        OFF_ABS_GROUP_ID = Convert.ToDecimal(item[9]),
                        DECTION = 1,
                        PERSON_DATE_OWEN = item[13].ToString(),
                        FROM_DATE = Convert.ToDateTime(item[5])
                    };
                    abs_ok(abs);
                }
            }
            return new JsonResult { Data = new { status = status, message = message, type = type, title = title } };
        }
    }
}