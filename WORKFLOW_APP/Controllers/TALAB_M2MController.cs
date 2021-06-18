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
    public class TALAB_M2MController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;

        //
        // GET: /TALAB_M2M/

        public string Max_missionId( )
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


        public JsonResult TYP_ddl(string firm_code)
        {
            string query = @"SELECT ABSENCE_TYPE_ID,NAME
                             FROM ABSENCE_TYPES

where ABSCENCE_CATEGORY_ID in (3,4)";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Index()
        {
           // var firm_missions_members = db.FIRM_MISSIONS_MEMBERS.Include(f => f.FIRM_MISSIONS).Include(f => f.PERSON_DATA);
            return View("index");
        }

        //
        // GET: /TALAB_M2M/Details/5


        public ActionResult GET_fin_year()
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT TRAINING_PERIODS.TRAINING_PERIOD,TRAINING_PERIODS.FIN_YEAR,TRAINING_PERIODS.TRAINING_PERIOD_ID FROM TRAINING_PERIODS WHERE   TO_DATE(SYSDATE) BETWEEN TRAINING_PERIODS.PERIOD_FROM AND TRAINING_PERIODS.PERIOD_TO";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_grid_m2m(string firm_code, string fin_year, int P, string date, string person_id) 
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            string q = @" 
SELECT ROWNUM, FIRM_MISSIONS.IS_DONE,   
         FIRM_MISSIONS.IS_PLANNED,   
         FIRM_MISSIONS.MISSION_TYPE, 
        PERSON_DATA.PERSON_CODE,
         PERSON_DATA.PERSONAL_ID_NO,
         PERSON_DATA.PERSON_NAME, 
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
 -- FIRM_MISSIONS.FROM_DATE  BETWEEN     TO_DATE('" + date + @"','dd/mm/yyyy HH24:MI:SS') AND NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy HH24:MI:SS')) AND
         FIRM_MISSIONS.FROM_DATE  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  FIRM_MISSIONS.TO_DATE <= NVL(FIRM_MISSIONS.TO_DATE ,TO_DATE('" + date + @"','dd/mm/yyyy')) AND
        MISSION_TYPES.MISSION_TYPE_ID(+) = FIRM_MISSIONS.MISSION_FIRM_CODE
                and FIRM_MISSIONS_MEMBERS.FIN_YEAR=FIRM_MISSIONS.FIN_YEAR
        and FIRM_MISSIONS_MEMBERS.FIRM_CODE=FIRM_MISSIONS.FIRM_CODE
        and FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID=FIRM_MISSIONS.TRAINING_PERIOD_ID
        and FIRM_MISSIONS_MEMBERS.MISSION_ID=FIRM_MISSIONS.MISSION_ID
        and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRM_MISSIONS.MISSION_TYPE
        and PERSON_DATA.PERSONAL_ID_NO='" + person_id + @"'
         AND PERSON_DATA.PERSON_CODE = FIRM_MISSIONS_MEMBERS.PERSON_CODE

          order by ROWNUM desc";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GET_off_role(string firm_code,  int P)
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
                                                       OR PERSON_DATA.BORROW_FIRM_CODE = '" +firm_code+@"'))
                                             AND PERSON_DATA.RANK_CAT_ID = "+P+@"
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

        public ActionResult GET_grid_gehat()
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            string q = @"SELECT FIRM_CODE,PARENT_FIRM_CODE, NAME FROM FIRM_WORK.FIRMS order by PARENT_FIRM_CODE asc
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
                                where o.PERSONAL_ID_NO == person_id
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

        public ActionResult Details(string id = null)
        {
            FIRM_MISSIONS_MEMBERS firm_missions_members = db.FIRM_MISSIONS_MEMBERS.Find(id);
            if (firm_missions_members == null)
            {
                return HttpNotFound();
            }
            return View(firm_missions_members);
        }

        //
        // GET: /TALAB_M2M/CreateCreate_Geha

        public ActionResult Create()
        {
            ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create");
        }
        public ActionResult Create_Geha()
        {
           // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
           // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_Geha");
        }


        public ActionResult Create_Off()
        {
           // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
           // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_Off");
        }

        [HttpPost]
        public ActionResult Create_OFF(FIRM_MISSIONS_MEMBERS FIRM_MISSIONS, string per)
        {
            message = "";
            status = false;
            // var max = Max_stepId();
          //  var maxorder = Max_offId(Create_OFF_group.OFF_ABS_GROUP_ID);
            //    if (ModelState.IsValid)|| o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID
            //{
            WF_EN en = new WF_EN();
            if (!en.FIRM_MISSIONS_MEMBERS.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            //&& General.check_ABS(FIRM_MISSIONS.PERSON_CODE.ToString(), FIRM_MISSIONS.FIN_YEAR.ToString(), FIRM_MISSIONS.TRAINING_PERIOD_ID.ToString(), FIRM_MISSIONS.FIRM_CODE.ToString(), "1", "1", FIRM_MISSIONS.FIRM_MISSIONS.FROM_DATE.ToString(), FIRM_MISSIONS.FIRM_MISSIONS.TO_DATE.ToString()))
            {
               // var pess = en.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per).PERSON_CODE;
                var datee = en.FIRM_MISSIONS.First(o=>o.MISSION_ID==FIRM_MISSIONS.MISSION_ID);
                if (datee.FROM_DATE >= DateTime.Now && datee.TO_DATE >= DateTime.Now && datee.MISSION_TYPE != null)
            {
                if (General.check_ABS(FIRM_MISSIONS.PERSON_CODE, FIRM_MISSIONS.FIN_YEAR.ToString(),
                    FIRM_MISSIONS.TRAINING_PERIOD_ID.ToString(), FIRM_MISSIONS.FIRM_CODE.ToString(), "1", "1", datee.FROM_DATE.Value.ToString("dd/MM/yyyy HH:mm"), datee.TO_DATE.Value.ToString("dd/MM/yyyy HH:mm")))
                {
                   // string cccc = FIRM_MISSIONS.FIRM_MISSIONS.FROM_DATE.Value.ToLongDateString();

                    if (datee.FROM_DATE <= datee.TO_DATE)
                {
           
                
                    db.FIRM_MISSIONS_MEMBERS.Add(new FIRM_MISSIONS_MEMBERS()
                    {
                       // OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
                        TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID,
                        PERSON_CODE = FIRM_MISSIONS.PERSON_CODE,
                        MISSION_ID = FIRM_MISSIONS.MISSION_ID,
                        FIN_YEAR = FIRM_MISSIONS.FIN_YEAR,
                        FIRM_CODE = FIRM_MISSIONS.FIRM_CODE
                    });

                    db.SaveChanges();
                    WF_EN E = new WF_EN();
                    string name = "";
                    var pers_name = from o in E.PERSON_DATA
                                    where o.PERSONAL_ID_NO == per
                               select o;
                    foreach (var p in pers_name)
                    {


                        name = p.RANKS.RANK + " / " +  p.PERSON_NAME;

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
                        decimal   grp_id=0;
                        var group_id = from o in db.OFF_ABS_GROUP

                                       where o.ABSCENCE_CATEGORY_ID == 3 && o.UNIT_DEF_GROUP==1
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
                            Fee.OFF_ABS_GROUP_ID = grp_id ;
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
        }

        [HttpPost]
        public ActionResult add_officer_fun(FIRM_MISSIONS_DET FIRM_MISSIONS, string per, string step_name)
        {
            message = "";
            status = false;
            string name = "";
            string xx = "";

            WF_EN en = new WF_EN();


            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.DECTION != null && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
            {

                if (FIRM_MISSIONS.PERSON_DATE_OWEN != FIRM_MISSIONS.PERSON_CODE && FIRM_MISSIONS.PERSON_DATE_OWEN != null)
                {

                    var xx1 = en.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.DECTION == null && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR);
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
        //[HttpPost]
        //public ActionResult add_officer_fun(FIRM_MISSIONS_DET FIRM_MISSIONS, string per, string step_name)

        //{
        //    message = "";
        //    status = false;
        //    string name = "";
        //    string xx = "";
        //    // var max = Max_stepId();
        //    //  var maxorder = Max_offId(Create_OFF_group.OFF_ABS_GROUP_ID);
        //    //    if (ModelState.IsValid)|| o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID
        //    //{
        //    WF_EN en = new WF_EN();
        //    if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE &&  o.DECTION == null  && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
        //   {
        //        var max = Max_mission_det_Id();

        //        db.FIRM_MISSIONS_DET.Add(new FIRM_MISSIONS_DET()
        //        {
        //            // OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
        //            FIRM_MISSIONS_DET_ID=Convert.ToInt16(max),
        //            TRAINING_PERIOD_ID = FIRM_MISSIONS.TRAINING_PERIOD_ID,
        //           PERSON_CODE = FIRM_MISSIONS.PERSON_CODE,
        //            MISSION_ID = FIRM_MISSIONS.MISSION_ID,
        //            FIN_YEAR = FIRM_MISSIONS.FIN_YEAR,
        //            FIRM_CODE = FIRM_MISSIONS.FIRM_CODE,
        //            OFF_ABS_STEPS_ID = FIRM_MISSIONS.OFF_ABS_STEPS_ID,
        //            OFF_ABS_GROUP_ID = FIRM_MISSIONS.OFF_ABS_GROUP_ID,
        //            OFF_SKELETON_OFFICERS_ID =Convert.ToInt64( FIRM_MISSIONS.PERSON_CODE),
        //            PERSON_DATE_OWEN = FIRM_MISSIONS.PERSON_DATE_OWEN,

        //        });

        //        db.SaveChanges();
        //        WF_EN E = new WF_EN();
               
        //        var pers_name = from o in E.PERSON_DATA
        //                        where o.PERSONAL_ID_NO == per
        //                        select o;
        //        foreach (var p in pers_name)
        //        {


        //            name = p.PERSON_NAME;

        //        }
               
        //        var pers = from o in E.PERSON_DATA
        //                   where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
        //                   select o;
        //        foreach (var p in pers)
        //        {


        //            xx = p.PERSONAL_ID_NO;

        //        }
        //        var per_id_name = db.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_DATE_OWEN).PERSONAL_ID_NO;
        //        var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " , بوضعك " + step_name + "    في المأمورية', 0, " + per_id_name + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per_id_name + " ')";
        //        General.exec_q(Q, "");
        //        General.exec_q("COMMIT", "");

        //        //var Q1 = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'تم التكليف للمأمورية', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn=" + xx + "' )";
        //        //General.exec_q(Q1, "");
        //        //General.exec_q("COMMIT", "");

        //        status = true;
        //        message = "Successfully Saved.";

             

        //   }
        //   else
        //    {
        //        WF_EN E = new WF_EN();

        //        var pers_name = from o in E.PERSON_DATA
        //                        where o.PERSONAL_ID_NO == per
        //                        select o;
        //        foreach (var p in pers_name)
        //        {


        //            name = p.PERSON_NAME;

        //        }

        //        var pers = from o in E.PERSON_DATA
        //                   where o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE
        //                   select o;
        //        foreach (var p in pers)
        //        {


        //            xx = p.PERSONAL_ID_NO;

        //        }

        //         // WF_EN E = new WF_EN();
        //         //FIRM_MISSIONS f = new FIRM_MISSIONS();
        //         //f = E.FIRM_MISSIONS.First(o => o.MISSION_ID == firm_missions.MISSION_ID && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == firm_missions.TRAINING_PERIOD_ID);
        //        if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.DECTION != null && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR))
        //        {

        //            if (FIRM_MISSIONS.PERSON_DATE_OWEN != FIRM_MISSIONS.PERSON_CODE)
        //            {

        //                var xx1 = E.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == FIRM_MISSIONS.TRAINING_PERIOD_ID && o.DECTION == null && o.PERSON_CODE == FIRM_MISSIONS.PERSON_CODE && o.FIRM_MISSIONS_DET_ID == FIRM_MISSIONS.FIRM_MISSIONS_DET_ID && o.MISSION_ID == FIRM_MISSIONS.MISSION_ID && o.FIN_YEAR == FIRM_MISSIONS.FIN_YEAR);
        //                xx1.PERSON_DATE_OWEN = FIRM_MISSIONS.PERSON_DATE_OWEN;
        //                E.SaveChanges();

        //                message = "  تم التعديل  ";
        //                status = true;

        //                var per_id_name = db.PERSON_DATA.First(o => o.PERSON_CODE == FIRM_MISSIONS.PERSON_DATE_OWEN).PERSONAL_ID_NO;
        //                if (per_id_name != null)
        //                {
        //                    var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " , بوضعك " + step_name + "    في المأمورية', 0, " + per_id_name + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + per_id_name + " ')";
        //                    General.exec_q(Q, "");
        //                    General.exec_q("COMMIT", "");
        //                }

        //            }
        //            else
        //            {
        //                message = "لا يجوز هذه الصلاحية";
        //                status = false;
        //            }
        //        }
        //        else
        //        {
        //            message = "حطأ!!!! لقد تم التصديق ع المامورية من قبل    ";
        //            status = false;
        //        }

        //   }

        //    return new JsonResult { Data = new { status = status, message = message } };
        //}

        //
        // POST: /TALAB_M2M/Create

        [HttpPost]
        public ActionResult Create(FIRM_MISSIONS firm_missions, string PERSON_CODE)
        {
            message = "";
            status = false;
            var max = Max_missionId();
            WF_EN E = new WF_EN();
            //WF_EN enw = new WF_EN();
            //WF_EN Ee = new WF_EN();
           
            //    if (ModelState.IsValid)
            //{
            var pess = E.PERSON_DATA.First(o=>o.PERSONAL_ID_NO==PERSON_CODE).PERSON_CODE;
            if (firm_missions.FROM_DATE >= DateTime.Now && firm_missions.TO_DATE >= DateTime.Now && firm_missions.MISSION_TYPE != null  )
            {
                if (General.check_ABS(pess.ToString(), firm_missions.FIN_YEAR.ToString(), 
                    firm_missions.TRAINING_PERIOD_ID.ToString(), firm_missions.FIRM_CODE.ToString(), "1", "1", firm_missions.FROM_DATE.Value.ToString("dd/MM/yyyy HH:mm"), firm_missions.TO_DATE.Value.ToString("dd/MM/yyyy HH:mm")))
                {
                    string cccc = firm_missions.FROM_DATE.Value.ToLongDateString();

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
                        MISSION_TYPE = firm_missions.MISSION_TYPE,
                        // IS_DONE = firm_missions.IS_DONE,
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
                    string name = "";
                    var pers_name = from o in E.PERSON_DATA
                                    where o.PERSONAL_ID_NO == PERSON_CODE
                                    select o;
                    foreach (var p in pers_name)
                    {


                        name = p.PERSON_NAME;

                    }
                    // string xx = "select PERSON_CODE from PERSON_DATA where PERSONAL_ID_NO =="+ person_id;
                    // string xx = pers.ToString();
                    // FIRM_MISSIONS_MEMBERS mmber=new FIRM_MISSIONS_MEMBERS();
                    // var FIN_YEAR = E.FIRM_MISSIONS_MEMBERS.First(o => o.FIRM_CODE == FIRM_CODE_PARAM && o.FIN_YEAR == FIN_YEAR_PARAM && o.PERSON_CODE == pers.ToString() && o.MISSION_ID == mission_id_para[0].ToString());

                   
                    var m = firm_missions.TRAINING_PERIOD_ID;
                    var tr = firm_missions.TRAINING_PERIOD_ID;
                    //var xx = pers.ToString();
            //         if (General.check_ABS(firm_missions., firm_missions.FIN_YEAR, firm_missions.FIRM_CODE, firm_missions.TRAINING_PERIOD_ID))
            //{}
                    if (!E.FIRM_MISSIONS_MEMBERS.Any(o => o.MISSION_ID == m && o.PERSON_CODE == xx && o.FIN_YEAR == firm_missions.FIN_YEAR && o.FIRM_CODE == firm_missions.FIRM_CODE && o.TRAINING_PERIOD_ID == tr))
                    {
                       // && General.check_ABS(xx.ToString(), firm_missions.FIN_YEAR.ToString(), tr.ToString(), firm_missions.FIRM_CODE.ToString(), "1", "1", firm_missions.FROM_DATE.Value.ToString("dd/MM/yyyy hh:mm"), firm_missions.TO_DATE.Value.ToString("dd/MM/yyyy hh:mm")) == false)
                        FIRM_MISSIONS_MEMBERS Fe = new FIRM_MISSIONS_MEMBERS();
                        Fe.FIN_YEAR = firm_missions.FIN_YEAR;
                        Fe.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                        Fe.FIRM_CODE = firm_missions.FIRM_CODE;
                        Fe.MISSION_ID = Convert.ToInt16(max);

                        Fe.PERSON_CODE = xx;
                        //  Fe.RANK_ID = short.Parse(RANK_ID_PARAM);
                        //Fe.RANK_CAT_ID = (RANK_CAT_ID_PARAM);
                        // Fe.PERSON_CAT_ID = (PERSON_CAT_ID_PARAM);
                        // Fe.TRAINING_PERIOD_ID = short.Parse(TRAINING_PERIOD_ID_PARAM);
                        // Fe.PERSON_MISSION_DATE = DateTime.ParseExact(PERSON_MISSION_DATE_PARAM, "dd/MM/yyyy", CultureInfo.InvariantCulture); 
                        //  DateTime.Parse(PERSON_MISSION_DATE_PARAM);

                        E.FIRM_MISSIONS_MEMBERS.Add(Fe);
                        E.SaveChanges();


                    }

                    if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == xx && o.ABS_CAT_ID == 3))
                    {


                        var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == xx && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                        var steps = from o in db.OFF_ABS_STEPS
                                    where o.OFF_ABS_GROUP_ID == pg
                                    select o;
                        foreach (var s in steps)
                        {
                            var Fee = new FIRM_MISSIONS_DET();
                            // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                            var max1 = Max_mission_det_Id();
                           // WF_EN enw = new WF_EN();
                            // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
                            Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
                            Fee.MISSION_ID = Convert.ToInt16(max);
                            Fee.FIRM_CODE = firm_missions.FIRM_CODE;
                            Fee.FIN_YEAR = firm_missions.FIN_YEAR;
                            Fee.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                            Fee.PERSON_CODE = xx;
                            Fee.OFF_ABS_GROUP_ID = pg;
                            Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                            E.FIRM_MISSIONS_DET.Add(Fee);
                            E.SaveChanges();
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
                          
                            // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
                            Fee.FIRM_MISSIONS_DET_ID = Convert.ToInt16(max1);
                            Fee.MISSION_ID = Convert.ToInt16(max);
                            Fee.FIRM_CODE = firm_missions.FIRM_CODE;
                            Fee.FIN_YEAR = firm_missions.FIN_YEAR;
                            Fee.TRAINING_PERIOD_ID = firm_missions.TRAINING_PERIOD_ID;
                            Fee.PERSON_CODE = xx;
                            Fee.OFF_ABS_GROUP_ID = grp_id;
                            Fee.OFF_ABS_STEPS_ID = s.OFF_ABS_STEPS_ID;
                            E.FIRM_MISSIONS_DET.Add(Fee);
                            E.SaveChanges();
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

                    //var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == xx && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                    //var steps = from o in db.OFF_ABS_STEPS
                    //            where o.OFF_ABS_GROUP_ID == pg
                    //            select o;
                    //foreach (var s in steps)
                    //{
                    //    var Fee = new FIRM_MISSIONS_DET();
                    //   // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
                    //    var max1 = Max_mission_det_Id();
                    //    WF_EN en = new WF_EN();
                    //   // FIRM_MISSIONS_DET Fee = new FIRM_MISSIONS_DET();
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
                    status = false;
                    message = "تاريخ المأمورية اكبر  من تاريخ العودة";
                }
                }
                else
                {
                    status = false;
                    message = "الضابط له تمام أخر ف نفس التوقيت";
                }
            }
            //  }

            else
            {
                message = "برجاء استكمال بيانات المأمورية";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.FIRM_MISSIONS.Add(firm_missions);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE", firm_missions_members.FIRM_CODE);
        //    ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_members.PERSON_CODE);
        //    return View(firm_missions);
        //}

        //
        // GET: /TALAB_M2M/Edit/5

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
                 var pess = E.FIRM_MISSIONS_MEMBERS.First(o => o.MISSION_ID == firm_missions.MISSION_ID).PERSON_CODE;
            if (firm_missions.FROM_DATE >= DateTime.Now && firm_missions.TO_DATE >= DateTime.Now && firm_missions.MISSION_TYPE != null  )
            {
                if (General.check_ABS(pess.ToString(), firm_missions.FIN_YEAR.ToString(), 
                    firm_missions.TRAINING_PERIOD_ID.ToString(), firm_missions.FIRM_CODE.ToString(), "1", "1", firm_missions.FROM_DATE.Value.ToString("dd/MM/yyyy HH:mm"), firm_missions.TO_DATE.Value.ToString("dd/MM/yyyy HH:mm")))
                {
                    string cccc = firm_missions.FROM_DATE.Value.ToLongDateString();

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
                         // db.Entry(firm_missions).State = EntityState.Modified;
                         // db.SaveChanges();
                }
                else
                {
                    status = false;
                    message = "تاريخ المأمورية اكبر  من تاريخ العودة";
                }
                }
                else
                {
                    status = false;
                    message = "الضابط له تمام أخر ف نفس التوقيت";
                }
            }
            //  }

            else
            {
                message = "برجاء استكمال بيانات المأمورية";
            }
              //  return RedirectToAction("Index");
            }
          //  ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE", firm_missions_members.FIRM_CODE);
          //  ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firm_missions_members.PERSON_CODE);
            return new JsonResult { Data = new { status = status, message = message } };
        }

        //
        // GET: /TALAB_M2M/Delete/5

        public ActionResult Delete(string id = null)
        {
            FIRM_MISSIONS_MEMBERS firm_missions_members = db.FIRM_MISSIONS_MEMBERS.Find(id);
            if (firm_missions_members == null)
            {
                return HttpNotFound();
            }
            return View(firm_missions_members);
        }

        //
        // POST: /TALAB_M2M/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per)
        {
            WF_EN en = new WF_EN();
            // FIRM_MISSIONS v = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
            //   if (v.FROM_DATE >= DateTime.Now.Date)
            // {
            var pers = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per).PERSON_CODE;
                       FIRM_MISSIONS_MEMBERS mm = en.FIRM_MISSIONS_MEMBERS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == pers);

            if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == pers && o.DECTION != null))
            {
                //if (db.OFF_ABS_GROP_OFF.Any(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3))
                //{

                 //   var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
                    var steps = from o in db.FIRM_MISSIONS_DET
                                where o.MISSION_ID == MISSION_ID && o.PERSON_CODE == pers
                                select o;
                    foreach (var s in steps)
                    {
                        var det = db.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == s.FIRM_MISSIONS_DET_ID && o.PERSON_CODE == pers);
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


        [HttpPost, ActionName("Delete_OFF")]
        public ActionResult DeleteConfirmed1(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per)
        {
            WF_EN en = new WF_EN();
            var off_mission = en.FIRM_MISSIONS_MEMBERS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == PERSON_CODE);
            en.FIRM_MISSIONS_MEMBERS.Remove(off_mission);
            en.SaveChanges();
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
            var pers = from o in E.PERSON_DATA
                       where o.PERSON_CODE == PERSON_CODE
                       select o;
            foreach (var p in pers)
            {


                xx = p.PERSONAL_ID_NO;

            }
            var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء المأمورية بالنسبه لك  ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
            General.exec_q(Q, "");
            General.exec_q("COMMIT", "");

            status = true;
            return new JsonResult { Data = new { status = status, message = message } };
        }

        //[HttpPost, ActionName("Delete_OFF_det")]
        //public ActionResult DeleteConfirmed1(short TRAINING_PERIOD_ID, string FIRM_CODE, string FIN_YEAR, short MISSION_ID, string PERSON_CODE, string per, decimal mission_dett)
        //{

        //    WF_EN en = new WF_EN();
        //   // FIRM_MISSIONS v = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );
        // //   if (v.FROM_DATE >= DateTime.Now.Date)
        //   // {
        //    var pers = db.PERSON_DATA.First(o => o.PERSONAL_ID_NO == per ).PERSON_CODE;
        //    FIRM_MISSIONS de = en.FIRM_MISSIONS.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID );

        //    if (!en.FIRM_MISSIONS_DET.Any(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.PERSON_CODE == pers && o.DECTION != null))
        //    {
        //        var pg = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == pers && o.ABS_CAT_ID == 3).OFF_ABS_GROUP_ID;
        //        var steps = from o in db.FIRM_MISSIONS_DET
        //                    where o.MISSION_ID == MISSION_ID
        //                    select o;
        //        foreach (var s in steps)
        //        {
        //            var det = db.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == s.FIRM_MISSIONS_DET_ID && o.PERSON_CODE == pers);
        //            db.FIRM_MISSIONS_DET.Remove(det);
        //            db.SaveChanges();
        //        }
        //         en.FIRM_MISSIONS.Remove(de);
        //        en.SaveChanges();
        //        status = true;
        //        message = "Successfully Saved.";
        //    }
        //    else
        //    {
        //        status = false;
        //        message = "لا يمكن حذف أجازة بعد التصديق عليها";
        //    }

        //  //  }

        //    return new JsonResult { Data = new { status = status, message = message } };







           
        //    //var off_mission = en.FIRM_MISSIONS_DET.First(o => o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.FIN_YEAR == FIN_YEAR && o.MISSION_ID == MISSION_ID && o.FIRM_MISSIONS_DET_ID == mission_dett && o.PERSON_CODE == PERSON_CODE);
        //    //en.FIRM_MISSIONS_DET.Remove(off_mission);
        //    //en.SaveChanges();
        //    //WF_EN E = new WF_EN();
        //    //string name = "";
        //    //var pers_name = from o in E.PERSON_DATA
        //    //                where o.PERSONAL_ID_NO == per
        //    //                select o;
        //    //foreach (var p in pers_name)
        //    //{


        //    //    name = p.PERSON_NAME;

        //    //}
        //    //string xx = "";
        //    //var pers = from o in E.PERSON_DATA
        //    //           where o.PERSON_CODE == PERSON_CODE
        //    //           select o;
        //    //foreach (var p in pers)
        //    //{


        //    //    xx = p.PERSONAL_ID_NO;

        //    //}
        //    //var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + " بالغاء المأمورية بالنسبه لك  ', 0, " + xx + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + xx + " ')";
        //    //General.exec_q(Q, "");
        //    //General.exec_q("COMMIT", "");


        //    //return new JsonResult { Data = new { status = status, message = message } };
        //}

        public PartialViewResult GET_ROLE()
        {
           //// var g = db.OFF_ABS_GROP_OFF.First(o => o.PERSON_DATA_ID == ID && o.ABS_CAT_ID == 2 && o.OFF_ABS_GROUP.FIRMS_CODE == FIRM).OFF_ABS_GROUP_ID;
           // var steps = from o in db.OFF_ABS_STEPS
           //             where o.OFF_ABS_GROUP_ID == g
           //             orderby o.ORDER_ID
           //             select o;
            return PartialView("../Shared/GET_ROLE");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}