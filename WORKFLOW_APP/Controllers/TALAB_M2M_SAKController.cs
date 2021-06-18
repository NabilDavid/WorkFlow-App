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
    public class TALAB_M2M_SAKController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;

        //
        // GET: /TALAB_M2M/

        public string Max_missionId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(FIRM_MISSIONS.MISSION_ID,   INSTR(FIRM_MISSIONS.MISSION_ID,'-') +1    ))),0)+1) MAX_CODE  from FIRM_MISSIONS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }

        public string Max_mission_det_Id()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(FIRM_MISSIONS_DET.FIRM_MISSIONS_DET_ID,   INSTR(FIRM_MISSIONS_DET.FIRM_MISSIONS_DET_ID,'-') +1    ))),0)+1) MAX_CODE  from FIRM_MISSIONS_DET";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }
        public ActionResult GET_grid_m2m(string firm_code, string fin_year, int P, string date, string person_id)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
//            string q = @" 
//SELECT ROWNUM, FIRM_MISSIONS.IS_DONE,   
//         FIRM_MISSIONS.IS_PLANNED,   
//         FIRM_MISSIONS.MISSION_TYPE, 
//       
//       MISSION_TYPES.NAME MISSION_TYPE_NAME,
//         to_char(FIRM_MISSIONS.TO_DATE,'dd/mm/yyyy  HH24:MI:SS')TO_DATE,   
//         to_char(FIRM_MISSIONS.FROM_DATE,'dd/mm/yyyy  HH24:MI:SS')FROM_DATE,
//         FIRM_MISSIONS.DISTRIBUTION,   
//         FIRM_MISSIONS.PROJECT_ID,   
//         FIRM_MISSIONS.FIRM_CODE,   
//         FIRM_MISSIONS.FIN_YEAR,   
//         FIRM_MISSIONS.TRAINING_PERIOD_ID,   
//         FIRM_MISSIONS.FIRM_NAME,   
//         FIRM_MISSIONS.MISSION_ID,   
//         FIRM_MISSIONS.MISSION_FIRM_CODE,   
//         FIRM_MISSIONS.INTRODUCTION,    FIRM_MISSIONS.SUBJECT,
//         FIRM_MISSIONS.FINAL,   
//         ABSENCE_TYPES.NAME TYP_NAME
//    FROM FIRM_MISSIONS      ,MISSION_TYPES  ,ABSENCE_TYPES
//   WHERE ( FIRM_MISSIONS.FIRM_CODE = '" + firm_code + @"' ) AND  
//         ( FIRM_MISSIONS.FIN_YEAR = '" + fin_year + @"' ) AND  
//         ( FIRM_MISSIONS.TRAINING_PERIOD_ID = " + P + @" ) AND  
//      --   ( TO_DATE('" + date + @"','dd/mm/yyyy') BETWEEN FIRM_MISSIONS.FROM_DATE AND NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy')) ) AND
//        TO_DATE(FIRM_MISSIONS.FROM_DATE)  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  TO_DATE(FIRM_MISSIONS.TO_DATE) <= NVL(TO_DATE(FIRM_MISSIONS.TO_DATE) ,TO_DATE('" + date + @"','dd/mm/yyyy')) AND
//        MISSION_TYPES.MISSION_TYPE_ID(+) = FIRM_MISSIONS.MISSION_FIRM_CODE
//        and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRM_MISSIONS.MISSION_TYPE
//
//
//          order by ROWNUM desc";
            string q = @"SELECT  distinct FIRM_MISSIONS.IS_DONE,   
         FIRM_MISSIONS.IS_PLANNED,   
         FIRM_MISSIONS.MISSION_TYPE, 
     --   PERSON_DATA.PERSON_CODE,
      --   PERSON_DATA.PERSONAL_ID_NO,
   --      PERSON_DATA.PERSON_NAME, 
       MISSION_TYPES.NAME MISSION_TYPE_NAME,
         to_char(FIRM_MISSIONS.TO_DATE,'dd/mm/yyyy  HH24:MI')TO_DATE,   
         to_char(FIRM_MISSIONS.FROM_DATE,'dd/mm/yyyy  HH24:MI')FROM_DATE,   
         FIRM_MISSIONS.DISTRIBUTION,   
         FIRM_MISSIONS.PROJECT_ID,   
         FIRM_MISSIONS.FIRM_CODE,   
         FIRM_MISSIONS.FIN_YEAR,   
         FIRM_MISSIONS.TRAINING_PERIOD_ID,   
         FIRM_MISSIONS.FIRM_NAME,   
         FIRM_MISSIONS.MISSION_ID,   
         FIRM_MISSIONS.MISSION_FIRM_CODE,   
         FIRM_MISSIONS.INTRODUCTION,    FIRM_MISSIONS.SUBJECT,
         FIRM_MISSIONS.FINAL,   
         ABSENCE_TYPES.NAME TYP_NAME
    FROM FIRM_MISSIONS      ,MISSION_TYPES  ,FIRM_MISSIONS_MEMBERS  ,PERSON_DATA,ABSENCE_TYPES
   WHERE ( FIRM_MISSIONS.FIRM_CODE = '" + firm_code + @"' ) AND  
        ( FIRM_MISSIONS.FIN_YEAR = '" + fin_year + @"' ) AND  
       ( FIRM_MISSIONS.TRAINING_PERIOD_ID = " + P + @" ) AND  
     --   ( TO_DATE('" + date + @"','dd/mm/yyyy') BETWEEN FIRM_MISSIONS.FROM_DATE AND NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy')) ) AND
        TO_DATE(FIRM_MISSIONS.FROM_DATE)  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  TO_DATE(FIRM_MISSIONS.TO_DATE) <= NVL(TO_DATE(FIRM_MISSIONS.TO_DATE) ,TO_DATE('" + date + @"','dd/mm/yyyy')) AND
        MISSION_TYPES.MISSION_TYPE_ID(+) = FIRM_MISSIONS.MISSION_FIRM_CODE
                and FIRM_MISSIONS_MEMBERS.FIN_YEAR(+)=FIRM_MISSIONS.FIN_YEAR
        and FIRM_MISSIONS_MEMBERS.FIRM_CODE(+)=FIRM_MISSIONS.FIRM_CODE
        and FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID(+)=FIRM_MISSIONS.TRAINING_PERIOD_ID
        and FIRM_MISSIONS_MEMBERS.MISSION_ID(+)=FIRM_MISSIONS.MISSION_ID
        and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRM_MISSIONS.MISSION_TYPE
      and (FIRM_MISSIONS_MEMBERS.RANK_CAT_ID in (2,3) or FIRM_MISSIONS_MEMBERS.RANK_CAT_ID is null)";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /TALAB_M2M_SH/
        public ActionResult GET_off_role(string firm_code, int P)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            string q = @"  SELECT PERSON_DATA.PERSON_CODE,
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
                                             PERSON_DATA.PERSON_CAT_ID,
                                             RANKS.RANK || ' / ' || PERSON_DATA.PERSON_NAME AS NM
                                        FROM PERSON_DATA, FIRMS, RANKS
                                       WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
                                             AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                                             AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
                                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1)
                                                  AND (PERSON_DATA.FIRM_CODE IN
                                                          (SELECT FIRMS_B.FIRM_CODE
                                                             FROM FIRMS FIRMS_A, FIRMS FIRMS_B
                                                            WHERE (FIRMS_A.FIRM_CODE = '" + firm_code + @"')
                                                                  AND (FIRMS_B.FIRM_LIKE_CODE LIKE
                                                                          CONCAT (FIRMS_A.FIRM_LIKE_CODE, '%')))
                                                       OR PERSON_DATA.BORROW_FIRM_CODE = '" + firm_code + @"'))
                                             AND PERSON_DATA.RANK_CAT_ID = " + P + @"
                                    ORDER BY person_data.firm_code ASC,
                                             person_data.person_cat_id ASC,
                                             person_data.rank_cat_id ASC,
                                             person_data.rank_id ASC,
                                             TO_NUMBER(person_data.id_no) ASC,
                                             person_data.current_rank_date ASC
";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GET_grid_ROLE(string firm_code, string fin_year, int P, string person_id, string mission)
        {
            WF_EN E = new WF_EN();
            string q = "";
            string pers_cod = "";
            var pers_name = from o in E.PERSON_DATA
                            where o.PERSON_CODE == person_id
                            select o;
            foreach (var p in pers_name)
            {


                pers_cod = p.PERSON_CODE;

            }
            if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == pers_cod && o.ABS_CAT_ID == 3))
            {
                var g = db.OFF_ABS_GROP_OFF.First(o => o.ABS_CAT_ID == 3 && o.PERSON_DATA_ID == pers_cod && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code).OFF_ABS_GROUP_ID;
                q = @" 
 SELECT OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
         OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
         OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
OFF_ABS_STEPS.JOB_TYPE_ID ROL,
         ORDER_ID,
         FIRM_MISSIONS_DET.FIRM_MISSIONS_DET_ID,
       --  OFF_ABS_GROUP.PERSON_DATA_ID,
 RANKS.RANK || ' / ' || PERSON_DATA.PERSON_NAME as PERSON_NAME,
        -- persn_name.PERSON_NAME,
         RANKS.RANK,
         PERSON_DATA.PERSON_CODE,
FIRM_MISSIONS_DET.PERSON_CODE PERSON_DATA_ID
  FROM FIRM_MISSIONS_DET,
       OFF_ABS_STEPS,
       OFF_ABS_GROUP,
       PERSON_DATA,
       RANKS
 WHERE (OFF_ABS_STEPS.OFF_ABS_GROUP_ID = OFF_ABS_GROUP.OFF_ABS_GROUP_ID)
       AND (FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID =
               OFF_ABS_GROUP.OFF_ABS_GROUP_ID)
       AND (OFF_ABS_GROUP.FIRMS_CODE = FIRM_MISSIONS_DET.FIRM_CODE)
       AND (OFF_ABS_STEPS.OFF_ABS_STEPS_ID =
               FIRM_MISSIONS_DET.OFF_ABS_STEPS_ID)
       AND (PERSON_DATA.RANK_ID= RANKS.RANK_ID(+) )
       AND (FIRM_MISSIONS_DET.PERSON_DATE_OWEN = PERSON_DATA.PERSON_CODE(+))
       AND (FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID = " + g + @")
       AND (FIRM_MISSIONS_DET.PERSON_CODE = '" + pers_cod + @"')
       and FIRM_MISSIONS_DET.MISSION_ID=" + mission + @"
      --  and PERSON_DATA.FIRM_CODE=" + firm_code + @"
       order by ORDER_ID
";
            }
            else
            {
                decimal grp_id = 0;
                var group_id = from o in db.OFF_ABS_GROUP

                               where o.ABSCENCE_CATEGORY_ID == 3 && o.UNIT_DEF_GROUP == 1
                               select o;
                foreach (var gg in group_id)
                {
                    grp_id = gg.OFF_ABS_GROUP_ID;
                }


                // var g = db.OFF_ABS_GROP_OFF.First(o => o.ABS_CAT_ID == 3 && o.OFF_ABS_GROUP.FIRMS_CODE == firm_code).OFF_ABS_GROUP_ID;
                var steps = from o in db.OFF_ABS_STEPS
                            where o.OFF_ABS_GROUP_ID == grp_id

                            orderby o.ORDER_ID
                            select o;
                q = @" 
 SELECT OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
         OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
         OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
OFF_ABS_STEPS.JOB_TYPE_ID ROL,
         ORDER_ID,
         FIRM_MISSIONS_DET.FIRM_MISSIONS_DET_ID,
       --  OFF_ABS_GROUP.PERSON_DATA_ID,
 RANKS.RANK || ' / ' || PERSON_DATA.PERSON_NAME as PERSON_NAME,
        -- persn_name.PERSON_NAME,
         RANKS.RANK,
         PERSON_DATA.PERSON_CODE,
FIRM_MISSIONS_DET.PERSON_CODE PERSON_DATA_ID
  FROM FIRM_MISSIONS_DET,
       OFF_ABS_STEPS,
       OFF_ABS_GROUP,
       PERSON_DATA,
       RANKS
 WHERE (OFF_ABS_STEPS.OFF_ABS_GROUP_ID = OFF_ABS_GROUP.OFF_ABS_GROUP_ID)
       AND (FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID =
               OFF_ABS_GROUP.OFF_ABS_GROUP_ID)
       AND (OFF_ABS_GROUP.FIRMS_CODE = FIRM_MISSIONS_DET.FIRM_CODE)
       AND (OFF_ABS_STEPS.OFF_ABS_STEPS_ID =
               FIRM_MISSIONS_DET.OFF_ABS_STEPS_ID)
       AND (PERSON_DATA.RANK_ID= RANKS.RANK_ID(+) )
       AND (FIRM_MISSIONS_DET.PERSON_DATE_OWEN = PERSON_DATA.PERSON_CODE(+))
       AND (FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID = " + grp_id + @")
       AND (FIRM_MISSIONS_DET.PERSON_CODE = '" + pers_cod + @"')
       and FIRM_MISSIONS_DET.MISSION_ID=" + mission + @"
      --  and PERSON_DATA.FIRM_CODE=" + firm_code + @"
       order by ORDER_ID
";
            }

            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_grid_m2m_mem(string firm_code, string fin_year, int P, string date, string person_id, string MISSION_ID)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            string q = @" 
SELECT RANKS.RANK,
       TO_CHAR (FIRM_MISSIONS_MEMBERS.PERSON_MISSION_DATE, 'dd/mm/yyyy')
          PERSON_MISSION_DATE,
       FIRM_MISSIONS_MEMBERS.PERSON_MISSION,
       FIRM_MISSIONS_MEMBERS.PERSON_CAT_ID,
       FIRM_MISSIONS_MEMBERS.RANK_CAT_ID,
       FIRM_MISSIONS_MEMBERS.FIRM_CODE,
       FIRM_MISSIONS_MEMBERS.PERSON_CODE,
       PERSON_DATA.PERSON_NAME,
       FIRM_MISSIONS_MEMBERS.RANK_ID,
       FIRM_MISSIONS_MEMBERS.MISSION_ID,
       FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID,
       FIRM_MISSIONS_MEMBERS.FIN_YEAR
  FROM FIRM_MISSIONS_MEMBERS, PERSON_DATA, RANKS
 WHERE (FIRM_MISSIONS_MEMBERS.FIN_YEAR = '" + fin_year + @"' ) AND   ( FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID ='" + P + @"') AND 
 ( FIRM_MISSIONS_MEMBERS.MISSION_ID = '" + MISSION_ID + @"') 
   AND   PERSON_DATA.PERSON_CODE=FIRM_MISSIONS_MEMBERS.PERSON_CODE  AND    RANKS.RANK_ID=PERSON_DATA.RANK_ID";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            var firm_missions_det = db.FIRM_MISSIONS_DET.Include(f => f.OFF_ABS_STEPS).Include(f => f.PERSON_DATA).Include(f => f.FIRM_MISSIONS_MEMBERS);
            return View(firm_missions_det.ToList());
        }

        //
        // GET: /TALAB_M2M_SH/Details/5

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
        // GET: /TALAB_M2M_SH/Create

        public ActionResult Create()
        {
            ViewBag.OFF_ABS_STEPS_ID = new SelectList(db.OFF_ABS_STEPS, "OFF_ABS_STEPS_ID", "OFF_ABS_STEPS_NAME");
            ViewBag.PERSON_DATE_OWEN = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            ViewBag.FIN_YEAR = new SelectList(db.FIRM_MISSIONS_MEMBERS, "FIN_YEAR", "PERSON_MISSION");
            return PartialView("Create");
        }
        public ActionResult Create_Geha()
        {
            // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
            // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_Geha");
        }
        //
        // POST: /TALAB_M2M_SH/Create

        [HttpPost]
        public ActionResult Create(FIRM_MISSIONS firm_missions, string PERSON_CODE)
        {
            message = "";
            status = false;
            var max = Max_missionId();
            WF_EN E = new WF_EN();
            //    if (ModelState.IsValid)
            //{
            //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));
            if (firm_missions.FROM_DATE.Value.Date >= DateTime.Now.Date && firm_missions.TO_DATE.Value.Date >= DateTime.Now.Date && firm_missions.MISSION_TYPE != null)
            {

                if (firm_missions.FROM_DATE <= firm_missions.TO_DATE)
                {

                    db.FIRM_MISSIONS.Add(new FIRM_MISSIONS()
                    {
                        MISSION_ID = Convert.ToInt16(max),
                        TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID,
                        MISSION_FIRM_CODE = firm_missions.MISSION_FIRM_CODE,
                        FIRM_CODE = firm_missions.FIRM_CODE,
                        FIN_YEAR = firm_missions.FIN_YEAR,
                        FIRM_NAME = firm_missions.FIRM_NAME,
                        SUBJECT = firm_missions.SUBJECT,
                        FROM_DATE = firm_missions.FROM_DATE,
                        TO_DATE = firm_missions.TO_DATE,
                        IS_PLANNED = firm_missions.IS_PLANNED,
                        //  IS_DONE = firm_missions.IS_DONE,
                        MISSION_TYPE = firm_missions.MISSION_TYPE,
                        INTRODUCTION = firm_missions.INTRODUCTION
                    });

                    db.SaveChanges();
                    status = true;
                    message = "Successfully Saved.";

                    string xx = "";
                    var pers = from o in E.PERSON_DATA
                               where o.PERSONAL_ID_NO == PERSON_CODE
                               select o;
                    foreach (var p in pers)
                    {


                        xx = p.PERSON_CODE;

                    }
                    // string xx = "select PERSON_CODE from PERSON_DATA where PERSONAL_ID_NO =="+ person_id;
                    // string xx = pers.ToString();
                    // FIRM_MISSIONS_MEMBERS mmber=new FIRM_MISSIONS_MEMBERS();
                    // var FIN_YEAR = E.FIRM_MISSIONS_MEMBERS.First(o => o.FIRM_CODE == FIRM_CODE_PARAM && o.FIN_YEAR == FIN_YEAR_PARAM && o.PERSON_CODE == pers.ToString() && o.MISSION_ID == mission_id_para[0].ToString());

                    WF_EN Ee = new WF_EN();
                    var m = firm_missions.TRAINING_PERIOD_ID;
                    var tr = firm_missions.TRAINING_PERIOD_ID;
                    //var xx = pers.ToString();
                    //if (!Ee.FIRM_MISSIONS_MEMBERS.Any(o => o.MISSION_ID == m && o.PERSON_CODE == xx && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == tr))
                    //{
                    //    FIRM_MISSIONS_MEMBERS Fe = new FIRM_MISSIONS_MEMBERS();
                    //    Fe.FIN_YEAR = firm_missions.FIN_YEAR;
                    //    Fe.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                    //    Fe.FIRM_CODE = firm_missions.FIRM_CODE;
                    //    Fe.MISSION_ID = Convert.ToInt16(max);

                    //    Fe.PERSON_CODE = xx;
                    //    //  Fe.RANK_ID = short.Parse(RANK_ID_PARAM);
                    //    //Fe.RANK_CAT_ID = (RANK_CAT_ID_PARAM);
                    //    // Fe.PERSON_CAT_ID = (PERSON_CAT_ID_PARAM);
                    //    // Fe.TRAINING_PERIOD_ID = short.Parse(TRAINING_PERIOD_ID_PARAM);
                    //    // Fe.PERSON_MISSION_DATE = DateTime.ParseExact(PERSON_MISSION_DATE_PARAM, "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                    //    //  DateTime.Parse(PERSON_MISSION_DATE_PARAM);

                    //    Ee.FIRM_MISSIONS_MEMBERS.Add(Fe);
                    //    Ee.SaveChanges();


                    //}


                    //  var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == xx && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                    // var steps = from o in db.OFF_ABS_STEPS
                    //    where o.OFF_ABS_GROUP_ID == pg
                    //    select o;
                    //foreach (var s in steps)
                    //{
                    //    var Fee = new FIRM_MISSIONS_DET();
                    //    // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                    //    var max1 = Max_mission_det_Id();
                    //    WF_EN en = new WF_EN();
                    //    // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
                    //    Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
                    //    Fee.MISSION_ID = Convert.ToInt16(max);
                    //    Fee.FIRM_CODE = firm_missions.FIRM_CODE;
                    //    Fee.FIN_YEAR = firm_missions.FIN_YEAR;
                    //    Fee.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                    //    Fee.PERSON_CODE = xx;
                    //    Fee.OFF_ABS_GROUP_ID = pg;
                    //    Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                    //    en.FIRM_MISSIONS_DET.Add(Fee);
                    //    en.SaveChanges();
                    //}
                    status = true;




                }
                else
                {
                    message = "   تاريخ المأمورية اكبر من تاريخ العودة";
                }
            }

            //  }

            else
            {
                message = "التاريخ الذي ادخلتة اقل من تاريخ اليوم";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }


        public ActionResult Create_Off()
        {
            // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
            // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_OFF");
        }
        [HttpPost]
        public ActionResult add_officer_fun(FIRM_MISSIONS_DET FIRM_MISSIONS, string per, string step_name)
        {
            message = "";
            status = false;
            string name = "";
            string xx = "";
            // var max = Max_stepId();
            //  var maxorder = Max_offId(Create_OFF_group.OFF_ABS_GROUP_ID);
            //    if (ModelState.IsValid)|| o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID
            //{
            WF_EN en = new WF_EN();

            //if (en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.DECTION == null && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            //{
            //    var max = Max_mission_det_Id();

            //    db.FIRM_MISSIONS_DET.Add(new FIRM_MISSIONS_DET()
            //    {
            //        // OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
            //        FIRM_MISSIONS_DET_ID = Convert.ToInt16(max),
            //        TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID,
            //        PERSON_CODE = FIRM_MISSIONS.PERSON_CODE,
            //        MISSION_ID = FIRM_MISSIONS.MISSION_ID,
            //        FIN_YEAR = FIRM_MISSIONS.FIN_YEAR,
            //        FIRM_CODE = FIRM_MISSIONS.FIRM_CODE,
            //        OFF_ABS_STEPS_ID = FIRM_MISSIONS.OFF_ABS_STEPS_ID,
            //        OFF_ABS_GROUP_ID = FIRM_MISSIONS.OFF_ABS_GROUP_ID,
            //        OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(FIRM_MISSIONS.PERSON_CODE),
            //        PERSON_DATE_OWEN = FIRM_MISSIONS.PERSON_DATE_OWEN,

            //    });

            //    db.SaveChanges();
            //    WF_EN E = new WF_EN();

            //    var pers_name = from o in E.PERSON_DATA
            //                    where o.PERSONAL_ID_NO == per
            //                    select o;
            //    foreach (var p in pers_name)
            //    {


            //        name = p.PERSON_NAME;

            //    }

            //    var pers = from o in E.PERSON_DATA
            //               where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
            //               select o;
            //    foreach (var p in pers)
            //    {


            //        xx = p.PERSONAL_ID_NO;

            //    }
            //    var per_id_name = db.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_DATE_OWEN).PERSONAL_ID_NO;
            //    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " , بوضعك " + step_name + "    في المأمورية', 0, " + per_id_name + " , 'http://192.223.30.3:90/mster_msdc/PAGES/w_present_vacation.aspx?nn= " + per_id_name + " ')";
            //    General.exec_q(Q, "");
            //    General.exec_q("COMMIT", "");

            //    //var Q1 = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'تم التكليف للمأمورية', 0, " + xx + " , 'http://192.223.30.3:90/mster_msdc/PAGES/OFFICERS_MAMORIAT.aspx?nn=" + xx + "' )";
            //    //General.exec_q(Q1, "");
            //    //General.exec_q("COMMIT", "");

            //    status = true;
            //    message = "Successfully Saved.";



            //}
            //else
            //{
            //    WF_EN E = new WF_EN();

            //    var pers_name = from o in E.PERSON_DATA
            //                    where o.PERSONAL_ID_NO == per
            //                    select o;
            //    foreach (var p in pers_name)
            //    {


            //        name = p.PERSON_NAME;

            //    }

            //    var pers = from o in E.PERSON_DATA
            //               where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
            //               select o;
            //    foreach (var p in pers)
            //    {


            //        xx = p.PERSONAL_ID_NO;

            //    }

            // WF_EN E = new WF_EN();
            //FIRM_MISSIONS f = new FIRM_MISSIONS();
            //f = E.FIRM_MISSIONS.First(o => o.MISSION_ID == firm_missions.MISSION_ID && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID);
            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.DECTION == 1 && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            {

                //if (FIRM_MISSIONS.PERSON_DATE_OWEN != FIRM_MISSIONS.PERSON_CODE && FIRM_MISSIONS.PERSON_DATE_OWEN!=null)
                if (FIRM_MISSIONS.PERSON_DATE_OWEN != null)
                {

                    var xx1 = en.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR);
                    xx1.PERSON_DATE_OWEN = FIRM_MISSIONS.PERSON_DATE_OWEN;
                    xx1.OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(FIRM_MISSIONS.PERSON_CODE);
                    en.SaveChanges();

                    message = "  تم التعديل  ";
                    status = true;

                    var per_id_name = db.PERSON_DATA.FirstOrDefault(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_DATE_OWEN).PERSONAL_ID_NO;
                    if (per_id_name != null)
                    {
                        var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " , بوضعك " + step_name + "    في المأمورية', 0, " + per_id_name + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per_id_name + " ')";
                        General.exec_q(Q, "");
                        General.exec_q("COMMIT", "");
                    }

                }
                else
                {
                    message = "لا يجوز هذه الصلاحية";
                    status = false;
                }
            }
            else
            {
                message = "حطأ!!!! لقد تم التصديق ع المامورية من قبل    ";
                status = false;
            }



            return new JsonResult { Data = new { status = status, message = message } };
        }
        [HttpPost]
        public ActionResult Create_OFF(FIRM_MISSIONS_MEMBERS FIRM_MISSIONS, string per)
        {

            message = "";
            status = false;
            WF_EN en = new WF_EN();
            if (!en.FIRM_MISSIONS_MEMBERS.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            //&& General.check_ABS(FIRM_MISSIONS.PERSON_CODE.ToString(), FIRM_MISSIONS.FIN_YEAR.ToString(), FIRM_MISSIONS.TRAINING_PERIOD_ID.ToString(), FIRM_MISSIONS.FIRM_CODE.ToString(), "1", "1", FIRM_MISSIONS.FIRM_MISSIONS.FROM_DATE.ToString(), FIRM_MISSIONS.FIRM_MISSIONS.TO_DATE.ToString()))
            {
                // var pess = en.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per).PERSON_CODE;
                var datee = en.FIRM_MISSIONS.First(o => o.MISSION_ID == FIRM_MISSIONS.MISSION_ID);
                if (datee.FROM_DATE.Value.Date >= DateTime.Now.Date && datee.TO_DATE.Value.Date >= DateTime.Now.Date && datee.MISSION_TYPE != null)
                {
                    if (General.check_ABS(FIRM_MISSIONS.PERSON_CODE, FIRM_MISSIONS.FIN_YEAR.ToString(),
                        FIRM_MISSIONS.TRAINING_PERIOD_ID.ToString(), FIRM_MISSIONS.FIRM_CODE.ToString(), "1", "1", datee.FROM_DATE.Value.ToString("dd/MM/yyyy HH:mm"), datee.TO_DATE.Value.ToString("dd/MM/yyyy HH:mm")))
                    {
                        // string cccc = FIRM_MISSIONS.FIRM_MISSIONS.FROM_DATE.Value.ToLongDateString();

                        if (datee.FROM_DATE <= datee.TO_DATE)
                        {
                            var rank_id = en.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE).RANK_ID;
                            var rank_cat = en.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE).RANK_CAT_ID;

                            db.FIRM_MISSIONS_MEMBERS.Add(new FIRM_MISSIONS_MEMBERS()
                            {
                                // OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
                                
                                TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID,
                                PERSON_CODE = FIRM_MISSIONS.PERSON_CODE,
                                MISSION_ID = FIRM_MISSIONS.MISSION_ID,
                                FIN_YEAR = FIRM_MISSIONS.FIN_YEAR,
                                FIRM_CODE = FIRM_MISSIONS.FIRM_CODE,
                                RANK_ID = rank_id,
                                RANK_CAT_ID = rank_cat
                            });

                            db.SaveChanges();
                            WF_EN E = new WF_EN();
                            string name = "";
                            var pers_name = from o in E.PERSON_DATA
                                            where o.PERSONAL_ID_NO == per
                                            select o;
                            foreach (var p in pers_name)
                            {


                                name = p.RANKS.RANK + " / " + p.PERSON_NAME;

                            }
                            string xx = "";
                            var pers = from o in E.PERSON_DATA
                                       where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
                                       select o;
                            foreach (var p in pers)
                            {


                                xx = p.PERSONAL_ID_NO;

                            }

                            if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == FIRM_MISSIONS.PERSON_CODE && o.ABS_CAT_ID == 3))
                            {


                                var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == FIRM_MISSIONS.PERSON_CODE && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                                var steps = from o in db.OFF_ABS_STEPS
                                            where o.OFF_ABS_GROUP_ID == pg
                                            select o;
                                foreach (var s in steps)
                                {
                                    var Fee = new FIRM_MISSIONS_DET();
                                    // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                                    var max1 = Max_mission_det_Id();
                                    WF_EN enw = new WF_EN();
                                    // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
                                    Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
                                    Fee.MISSION_ID = FIRM_MISSIONS.MISSION_ID;
                                    Fee.FIRM_CODE = FIRM_MISSIONS.FIRM_CODE;
                                    Fee.FIN_YEAR = FIRM_MISSIONS.FIN_YEAR;
                                    Fee.TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID;
                                    Fee.PERSON_CODE = FIRM_MISSIONS.PERSON_CODE;
                                    Fee.OFF_ABS_GROUP_ID = pg;
                                    Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                                    enw.FIRM_MISSIONS_DET.Add(Fee);
                                    enw.SaveChanges();
                                }
                                var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بتكليفك قائم  معه في المأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
                                General.exec_q(Q, "");
                                General.exec_q("COMMIT", "");

                                //var Q1 = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'تم التكليف للمأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn=" + xx + "' )";
                                //General.exec_q(Q1, "");
                                //General.exec_q("COMMIT", "");

                                status = true;
                                message = "Successfully Saved.";

                                //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));

                            }
                            else
                            {
                                decimal grp_id = 0;
                                var group_id = from o in db.OFF_ABS_GROUP

                                               where o.ABSCENCE_CATEGORY_ID == 3 && o.UNIT_DEF_GROUP == 1
                                               select o;
                                foreach (var g in group_id)
                                {
                                    grp_id = g.OFF_ABS_GROUP_ID;
                                }





                                var steps = from o in db.OFF_ABS_STEPS

                                            where o.OFF_ABS_GROUP_ID == grp_id
                                            select o;
                                foreach (var s in steps)
                                {

                                    var Fee = new FIRM_MISSIONS_DET();
                                    // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                                    var max1 = Max_mission_det_Id();
                                    WF_EN enw = new WF_EN();
                                    // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
                                    Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
                                    Fee.MISSION_ID = FIRM_MISSIONS.MISSION_ID;
                                    Fee.FIRM_CODE = FIRM_MISSIONS.FIRM_CODE;
                                    Fee.FIN_YEAR = FIRM_MISSIONS.FIN_YEAR;
                                    Fee.TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID;
                                    Fee.PERSON_CODE = FIRM_MISSIONS.PERSON_CODE;
                                    Fee.OFF_ABS_GROUP_ID = grp_id;
                                    Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                                    enw.FIRM_MISSIONS_DET.Add(Fee);
                                    enw.SaveChanges();
                                }

                                var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بتكليفك قائم  معه في المأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
                                General.exec_q(Q, "");
                                General.exec_q("COMMIT", "");

                                //var Q1 = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'تم التكليف للمأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn=" + xx + "' )";
                                //General.exec_q(Q1, "");
                                //General.exec_q("COMMIT", "");

                                status = true;
                                message = "Successfully Saved.";
                            }

                        }
                        else
                        {
                            message = "تاريخ المأمورية اكبر  من تاريخ العودة";
                        }
                    }
                    else
                    {
                        status = false;

                        message = "الضابط له تمام أخر ف نفس التوقيت";
                    }
                }
                else
                {
                    status = false;
                    // 
                    message = "برجاء استكمال بيانات المأمورية";
                }
            }


            else
            {

                message = "  الضابط موجود من قبل      ";
            }


            return new JsonResult { Data = new { status = status, message = message } };

            //message = "";
            //status = false;
            //// var max = Max_stepId();
            ////  var maxorder = Max_offId(Create_OFF_group.OFF_ABS_GROUP_ID);
            ////    if (ModelState.IsValid)|| o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID
            ////{
            //WF_EN en = new WF_EN();
            //if (!en.FIRM_MISSIONS_MEMBERS.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            //{
            //    if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == FIRM_MISSIONS.PERSON_CODE && o.ABS_CAT_ID == 3))
            //    {

            //    db.FIRM_MISSIONS_MEMBERS.Add(new FIRM_MISSIONS_MEMBERS()
            //    {
            //        // OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
            //        TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID,
            //        PERSON_CODE = FIRM_MISSIONS.PERSON_CODE,
            //        MISSION_ID = FIRM_MISSIONS.MISSION_ID,
            //        FIN_YEAR = FIRM_MISSIONS.FIN_YEAR,
            //        FIRM_CODE = FIRM_MISSIONS.FIRM_CODE
            //    });

            //    db.SaveChanges();
            //    WF_EN E = new WF_EN();
            //    string name = "";
            //    var pers_name = from o in E.PERSON_DATA
            //                    where o.PERSONAL_ID_NO == per
            //                    select o;
            //    foreach (var p in pers_name)
            //    {


            //        name = p.PERSON_NAME;

            //    }
            //    string xx = "";
            //    var pers = from o in E.PERSON_DATA
            //               where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
            //               select o;
            //    foreach (var p in pers)
            //    {


            //        xx = p.PERSONAL_ID_NO;

            //    }


            //    var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == FIRM_MISSIONS.PERSON_CODE && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
            //    var steps = from o in db.OFF_ABS_STEPS
            //                where o.OFF_ABS_GROUP_ID == pg
            //                select o;
            //    foreach (var s in steps)
            //    {
            //        var Fee = new FIRM_MISSIONS_DET();
            //        // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
            //        var max1 = Max_mission_det_Id();
            //        WF_EN enw = new WF_EN();
            //        // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
            //        Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
            //        Fee.MISSION_ID = FIRM_MISSIONS.MISSION_ID;
            //        Fee.FIRM_CODE = FIRM_MISSIONS.FIRM_CODE;
            //        Fee.FIN_YEAR = FIRM_MISSIONS.FIN_YEAR;
            //        Fee.TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID;
            //        Fee.PERSON_CODE = FIRM_MISSIONS.PERSON_CODE;
            //        Fee.OFF_ABS_GROUP_ID = pg;
            //        Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
            //        enw.FIRM_MISSIONS_DET.Add(Fee);
            //        enw.SaveChanges();
            //    }
            //    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بتكليفك قائم  معه في المأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
            //    General.exec_q(Q, "");
            //    General.exec_q("COMMIT", "");

            //    //var Q1 = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'تم التكليف للمأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn=" + xx + "' )";
            //    //General.exec_q(Q1, "");
            //    //General.exec_q("COMMIT", "");

            //    status = true;
            //    message = "Successfully Saved.";

            //    //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));

            //    }
            //    else
            //    {
            //        message = "  الضابط الذي اخترته ليس له مجموعه  ";
            //    }
            //         }
            //else
            //{
            //    message = "  الضابط موجود من قبل      ";
            //}

            //return new JsonResult { Data = new { status = status, message = message } };
        }
        //
        // GET: /TALAB_M2M_SH/Edit/5

        public ActionResult Edit(string FIN_YEAR, short TRAINING_PERIOD_ID, short MISSION_ID, string FIRM_CODE)
        {


            WF_EN en = new WF_EN();
            var firm_missions = en.FIRM_MISSIONS.First(o => o.FIN_YEAR == FIN_YEAR && o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.MISSION_ID == MISSION_ID);
            if (firm_missions == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = true;
            return PartialView("Create", firm_missions);
        }
        public ActionResult Edit_STATUS(string FIN_YEAR, short TRAINING_PERIOD_ID, short MISSION_ID, string FIRM_CODE)
        {


            WF_EN en = new WF_EN();
            var firm_missions = en.FIRM_MISSIONS.First(o => o.FIN_YEAR == FIN_YEAR && o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.MISSION_ID == MISSION_ID);
            if (firm_missions == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = false;
            return PartialView("Create", firm_missions);
        }
        [HttpPost]
        public ActionResult Edit_STATUS(FIRM_MISSIONS firm_missions, short MISSION_TYPE, string PERSON_CODE)
        {


            short RANK_CAT_ID = 0;
            short PERSON_CAT_ID = 0;

            if (ModelState.IsValid)
            {

                WF_EN E = new WF_EN();
                var pers = from o in E.PERSON_DATA
                           where o.PERSON_CODE == PERSON_CODE
                           select o;
                foreach (var p in pers)
                {
                    RANK_CAT_ID = Convert.ToInt16(p.RANK_CAT_ID);
                    PERSON_CAT_ID = Convert.ToInt16(p.PERSON_CAT_ID);

                    // xx = p.PERSONAL_ID_NO;

                }
                FIRMS_ABSENCES_PERSONS f = new FIRMS_ABSENCES_PERSONS();
                if (!E.FIRMS_ABSENCES_PERSONS.Any(o => o.PERSON_CODE == PERSON_CODE && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.FROM_DATE == firm_missions.FROM_DATE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID && o.RANK_CAT_ID == RANK_CAT_ID && o.PERSON_CAT_ID == PERSON_CAT_ID && o.ABSENCE_TYPE_ID == MISSION_TYPE && o.COMMANDER_FLAG != 1))
                {


                    f = E.FIRMS_ABSENCES_PERSONS.First(o => o.PERSON_CODE == PERSON_CODE && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.FROM_DATE == firm_missions.FROM_DATE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID && o.RANK_CAT_ID == RANK_CAT_ID && o.PERSON_CAT_ID == PERSON_CAT_ID && o.ABSENCE_TYPE_ID == MISSION_TYPE);

                    if (firm_missions.FROM_DATE >= DateTime.Today && firm_missions.TO_DATE >= DateTime.Today)
                    {


                        //       f.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                        //        f.MISSION_FIRM_CODE = firm_missions.MISSION_FIRM_CODE;
                        //        f.FIRM_CODE = firm_missions.FIRM_CODE;
                        //        f.FIN_YEAR = firm_missions.FIN_YEAR;
                        //        f.FIRM_NAME = firm_missions.FIRM_NAME;
                        //        f.SUBJECT = firm_missions.SUBJECT;
                        f.FROM_DATE = Convert.ToDateTime(firm_missions.FROM_DATE);
                        f.TO_DATE = firm_missions.TO_DATE;
                        //        f.MISSION_TYPE = firm_missions.MISSION_TYPE;
                        //        // f.IS_DONE = firm_missions.IS_DONE;
                        //        f.INTRODUCTION = firm_missions.INTRODUCTION;
                        E.SaveChanges();

                        status = true;
                        message = "Successfully Saved.";
                    }
                    else
                    {
                        status = false;
                        message = "التاريخ الذي ادخلته اقل من تاريخ اليوم";
                    }
                }
                else
                {
                    status = false;
                    message = "لا يمكن قطع المأمورية لم يتصدق عليها بعد";
                }
                //    //  return RedirectToAction("Index");
            }
            //  ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE", firm_missions_members.FIRM_CODE);
            //  ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_members.PERSON_CODE);
            return new JsonResult { Data = new { status = status, message = message } };
        }
        //
        // POST: /TALAB_M2M/Edit/5

        [HttpPost]
        public ActionResult Edit(FIRM_MISSIONS firm_missions)
        {
            if (ModelState.IsValid)
            {
                WF_EN E = new WF_EN();
                FIRM_MISSIONS f = new FIRM_MISSIONS();
                f = E.FIRM_MISSIONS.First(o => o.MISSION_ID == firm_missions.MISSION_ID && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID);
                if (firm_missions.FROM_DATE >= DateTime.Today && firm_missions.TO_DATE >= DateTime.Today)
                {
                    if (firm_missions.FROM_DATE <= firm_missions.TO_DATE)
                    {

                        f.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                        f.MISSION_FIRM_CODE = firm_missions.MISSION_FIRM_CODE;
                        f.FIRM_CODE = firm_missions.FIRM_CODE;
                        f.FIN_YEAR = firm_missions.FIN_YEAR;
                        f.FIRM_NAME = firm_missions.FIRM_NAME;
                        f.SUBJECT = firm_missions.SUBJECT;
                        f.FROM_DATE = firm_missions.FROM_DATE;
                        f.TO_DATE = firm_missions.TO_DATE;
                        f.MISSION_TYPE = firm_missions.MISSION_TYPE;
                        // f.IS_DONE = firm_missions.IS_DONE;
                        f.INTRODUCTION = firm_missions.INTRODUCTION;
                        E.SaveChanges();

                        status = true;
                        message = "Successfully Saved.";
                    }
                    else
                    {
                        status = false;
                        message = " تاريخ المأمورية اكبر من تاريخ العودة";
                    }
                }
                else
                {
                    status = false;
                    message = " التاريخ الذي ادخلته اقل من تاريخ اليوم";
                }
                //  return RedirectToAction("Index");
            }
            //  ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE", firm_missions_members.FIRM_CODE);
            //  ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_members.PERSON_CODE);
            return new JsonResult { Data = new { status = status, message = message } };
        }

        //
        // GET: /TALAB_M2M_SH/Delete/5

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
        // POST: /TALAB_M2M_SH/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per)
        {
            string name = "";
            string xx = "";
            string pers_cod = "";
            status = true;
            WF_EN en = new WF_EN();
            // FIRM_MISSIONS v = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //   if (v.FROM_DATE >= DateTime.Now.Date)
            // {

            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.DECTION != 1))
            {


                var members = from o in db.FIRM_MISSIONS_MEMBERS
                              where o.MISSION_ID == MISSION_ID && o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR
                              select o;

                foreach (var m in members)
                {
                    pers_cod = m.PERSON_CODE;
                    var steps = from o in db.FIRM_MISSIONS_DET
                                where o.MISSION_ID == MISSION_ID && o.PERSON_CODE == m.PERSON_CODE
                                select o;
                    foreach (var s in steps)
                    {
                        var det = db.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == s.FIRM_MISSIONS_DET_ID && o.PERSON_CODE == m.PERSON_CODE);
                        db.FIRM_MISSIONS_DET.Remove(det);
                        db.SaveChanges();
                    }
                    FIRM_MISSIONS_MEMBERS mem = en.FIRM_MISSIONS_MEMBERS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == m.PERSON_CODE);
                    en.FIRM_MISSIONS_MEMBERS.Remove(mem);
                    en.SaveChanges();

                    ///notif////////////////////////////////////////////////////////////

                    WF_EN E = new WF_EN();

                    var pers_name = from o in E.PERSON_DATA
                                    where o.PERSONAL_ID_NO == per
                                    select o;
                    foreach (var p in pers_name)
                    {


                        name = p.PERSON_NAME;

                    }

                    var pers = from o in E.PERSON_DATA
                               where o.PERSON_CODE == m.PERSON_CODE
                               select o;
                    foreach (var p in pers)
                    {


                        xx = p.PERSONAL_ID_NO;

                    }
                    // var per_id_name = db.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_DATE_OWEN).PERSONAL_ID_NO;
                    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء المأمورية    ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
                    General.exec_q(Q, "");
                    General.exec_q("COMMIT", "");

                }


                FIRM_MISSIONS de = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID);

                en.FIRM_MISSIONS.Remove(de);
                en.SaveChanges();



            }
            else
            {
                status = false;
                message = "لا يمكن حذف أجازة بعد التصديق عليها";
            }


            return new JsonResult { Data = new { status = status, message = message } };




            //status = true;
            //WF_EN en = new WF_EN();
            //var off_mission = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //en.FIRM_MISSIONS.Remove(off_mission);
            //en.SaveChanges();
            //return new JsonResult { Data = new { status = status, message = message } };
        }




        [HttpPost, ActionName("Delete_OFF")]
        public ActionResult DeleteConfirmed1(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per)
        {
            WF_EN en = new WF_EN();
            // FIRM_MISSIONS v = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //   if (v.FROM_DATE >= DateTime.Now.Date)
            // {
            var pers = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per).PERSON_CODE;
            FIRM_MISSIONS_MEMBERS mm = en.FIRM_MISSIONS_MEMBERS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE);

            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE && o.DECTION != null))
            {
                //if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3))
                //{

                //   var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                var steps = from o in db.FIRM_MISSIONS_DET
                            where o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE
                            select o;
                foreach (var s in steps)
                {
                    var det = db.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == s.FIRM_MISSIONS_DET_ID && o.PERSON_CODE == PERSON_CODE);
                    db.FIRM_MISSIONS_DET.Remove(det);
                    db.SaveChanges();
                }
                en.FIRM_MISSIONS_MEMBERS.Remove(mm);
                en.SaveChanges();

                if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID))
                {
                    FIRM_MISSIONS de = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID);

                    en.FIRM_MISSIONS.Remove(de);
                    en.SaveChanges();
                }



                // en.FIRM_MISSIONS.Remove(de);
                //en.SaveChanges();
                status = true;
                message = "Successfully Saved.";
                string xx = "";
                string name = "";
                var perss = from o in en.PERSON_DATA
                            where o.PERSON_CODE == PERSON_CODE
                            select o;
                foreach (var p in perss)
                {


                    xx = p.PERSONAL_ID_NO;

                }
                var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء المأمورية بالنسبه لك  ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
                General.exec_q(Q, "");
                General.exec_q("COMMIT", "");
                //}
                //else
                //{

                //}
            }
            else
            {
                status = false;
                message = "لا يمكن حذف أجازة بعد التصديق عليها";
            }

            //  }

            return new JsonResult { Data = new { status = status, message = message } };




            //status = true;
            //WF_EN en = new WF_EN();
            //var off_mission = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //en.FIRM_MISSIONS.Remove(off_mission);
            //en.SaveChanges();
            //return new JsonResult { Data = new { status = status, message = message } };
        }
        //{
        //    WF_EN en = new WF_EN();
        //    var off_mission = en.FIRM_MISSIONS_MEMBERS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE);
        //    en.FIRM_MISSIONS_MEMBERS.Remove(off_mission);
        //    en.SaveChanges();
        //    WF_EN E = new WF_EN();
        //    string name = "";
        //    var pers_name = from o in E.PERSON_DATA
        //                    where o.PERSONAL_ID_NO == per
        //                    select o;
        //    foreach (var p in pers_name)
        //    {


        //        name = p.PERSON_NAME;

        //    }
        //    string xx = "";
        //    var pers = from o in E.PERSON_DATA
        //               where o.PERSON_CODE == PERSON_CODE
        //               select o;
        //    foreach (var p in pers)
        //    {


        //        xx = p.PERSONAL_ID_NO;

        //    }
        //    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء المأمورية بالنسبه لك  ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
        //    General.exec_q(Q, "");
        //    General.exec_q("COMMIT", "");

        //    status = true;
        //    return new JsonResult { Data = new { status = status, message = message } };
        //}

        [HttpPost, ActionName("Delete_OFF_det")]
        public ActionResult DeleteConfirmed2(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per, decimal mission_dett)
        {

            WF_EN en = new WF_EN();
            // FIRM_MISSIONS v = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //   if (v.FROM_DATE >= DateTime.Now.Date)
            // {
            var pers = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per).PERSON_CODE;
            FIRM_MISSIONS de = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID);

            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.FIRM_MISSIONS_DET_ID == mission_dett && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE && o.DECTION == 1))
            {
                // var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == PERSON_CODE && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                var steps = from o in db.FIRM_MISSIONS_DET
                            where o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == mission_dett
                            select o;
                foreach (var s in steps)
                {
                    var det = db.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == mission_dett && o.PERSON_CODE == PERSON_CODE);
                    db.FIRM_MISSIONS_DET.Remove(det);
                    db.SaveChanges();
                }
                // en.FIRM_MISSIONS.Remove(de);
                //  en.SaveChanges();
                status = true;
                message = "Successfully Saved.";



                WF_EN E = new WF_EN();
                string name = "";
                var pers_name = from o in E.PERSON_DATA
                                where o.PERSONAL_ID_NO == per
                                select o;
                foreach (var p in pers_name)
                {


                    name = p.PERSON_NAME;

                }
                string xx = "";
                var perss = from o in E.PERSON_DATA
                            where o.PERSON_CODE == PERSON_CODE
                            select o;
                foreach (var p in perss)
                {


                    xx = p.PERSONAL_ID_NO;

                }

                // var stp = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء الخطوه بالنسبه لك  ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
                General.exec_q(Q, "");
                General.exec_q("COMMIT", "");
            }
            else
            {
                status = false;
                message = "لا يمكن حذف أجازة بعد التصديق عليها";
            }

            //  }

            //  return new JsonResult { Data = new { status = status, message = message } };








            // var off_mission = en.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == mission_dett && o.PERSON_CODE == PERSON_CODE);
            //  en.FIRM_MISSIONS_DET.Remove(off_mission);
            //   en.SaveChanges();



            return new JsonResult { Data = new { status = status, message = message } };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}