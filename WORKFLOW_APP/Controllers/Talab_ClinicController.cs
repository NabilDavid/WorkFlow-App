using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
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
    public class Talab_ClinicController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        //
        // GET: /Talab_Clinic/


        // get data of ClinicGrid
        public JsonResult bind_data_clinic(string fromdate, string FirmCode, string PersonId)
        {

            string query = @"SELECT  PERSON_DATA.RANK_ID,
                                     RANKS.RANK,
                                     PERSON_DATA.PERSON_NAME,
                                     FIRMS_ABSENCES_PERSONS.FIRM_CODE,
                                 to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE , 'hh24:mi yyyy/mm/dd') as FROM_DATE,
                                 to_char(FIRMS_ABSENCES_PERSONS.TO_DATE , 'hh24:mi yyyy/mm/dd') as TO_DATE,
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
                                     FIRMS_ABSENCES_PERSONS.DAY_STATUS
                              FROM   FIRMS_ABSENCES_PERSONS, PERSON_DATA, RANKS
                            WHERE   (PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE)
                                    AND   RANKS.RANK_ID=PERSON_DATA.RANK_ID
                                    AND ( (FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FirmCode + @")
                                        AND (person_data.rank_cat_id IN (1, 2, 3))
                                         and FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG not in(2,4)
                                         and person_data.PERSONAL_ID_NO='" + PersonId + @"'
                                        
                                        AND (TO_DATE ('" + fromdate + @"', 'dd/mm/yyyy') BETWEEN TO_DATE (FIRMS_ABSENCES_PERSONS.FROM_DATE)
                                        AND  NVL (TO_DATE(FIRMS_ABSENCES_PERSONS.TO_DATE), TO_DATE('" + fromdate + @"' , 'dd/mm/yyyy') ))
                                        AND (firms_absences_persons.absence_type_id = 11))";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
//*************************************************************************************************
        // filter drop down
        public ActionResult GET_off_role(string firm, string rol)
        {
             //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
//            var q = @"SELECT PD.PERSON_CODE,
//                           PD.PERSON_CAT_ID,
//                           R.RANK || ' / ' || PD.PERSON_NAME AS NM,
//                           PD.PERSONAL_ID_NO,
//                           PD.RANK_CAT_ID,
//                           PD.RANK_ID,
//                           PD.CATEGORY_ID,
//                           R.RANK,
//                           R.DISPLAY_ORDER,
//                           F.NAME,
//                           O.CIV_ID_CARD_NO,
//                           O.JOB_NAME,
//                           O.BRANCH_NAME
//                      FROM OFF_ROLE_JOB_ARCHIVE ROL,
//                           OFF_SKELETON_OFFICERS O,
//                           PERSON_DATA PD,
//                           RANKS R,
//                           FIRMS F
//                     WHERE     (PD.FIRM_CODE = F.FIRM_CODE)
//                           AND (R.RANK_ID = PD.RANK_ID)
//                           AND (R.RANK_CAT_ID = PD.RANK_CAT_ID)
//                           AND ROL.JOB_TYPE_NO = O.JOB_TYPE_NO
//                           AND ROL.ARH_ROLE_CODE = " + rol + @"
//                           AND O.HIA_UNIT_CODE = '" + firm + @"'
//                           AND PD.ID_NO = TO_CHAR (O.MIL_NO)
//                           AND PD.OUT_UN_FORCE = 0
//                    UNION
//                    SELECT DISTINCT(PD.PERSON_CODE),
//                           PD.PERSON_CAT_ID,
//                           R.RANK || ' / ' || PD.PERSON_NAME AS NM,
//                           PD.PERSONAL_ID_NO,
//                           PD.RANK_CAT_ID,
//                           PD.RANK_ID,
//                           PD.CATEGORY_ID,
//                           R.RANK,
//                           R.DISPLAY_ORDER,
//                           F.NAME,
//                           O.CIV_ID_CARD_NO,
//                           O.JOB_NAME,
//                           O.BRANCH_NAME
//                      FROM OFF_ROLE_OFF_ARCHIVE OJ,
//                           OFF_ROLE_ARCHIVE ROL,
//                           OFF_SKELETON_OFFICERS O,
//                           PERSON_DATA PD,
//                           RANKS R,
//                           FIRMS F
//                     WHERE     (PD.FIRM_CODE = F.FIRM_CODE)
//                           AND (R.RANK_ID = PD.RANK_ID)
//                           AND (R.RANK_CAT_ID = PD.RANK_CAT_ID)
//                           AND ROL.ARH_ROLE_CODE = OJ.ARH_ROLE_CODE
//                           AND OJ.CIV_ID_CARD_NO = O.CIV_ID_CARD_NO
//                           AND OJ.ARH_ROLE_CODE = " + rol + @"
//                           AND O.HIA_UNIT_CODE = '" + firm + @"'
//                           AND O.HIA_UNIT_CODE = PD.FIRM_CODE
//                           AND PD.ID_NO = TO_CHAR (O.MIL_NO)
//                           AND PD.OUT_UN_FORCE = 0";
                       var q = @" SELECT DISTINCT(PD.PERSON_CODE),
                                        PD.PERSON_CAT_ID,
                                        R.RANK || ' / ' || PD.PERSON_NAME AS NM,
                                        PD.PERSONAL_ID_NO,
                                        PD.RANK_CAT_ID,
                                        PD.RANK_ID,
                                        PD.CATEGORY_ID,
                                        R.RANK,
                                        R.DISPLAY_ORDER,
                                        F.NAME,
                                       JB.JOB_NAME
                                FROM 
                                        PERSON_DATA PD,
                                        RANKS R,
                                        FIRMS F,
                                        JOBS_TYPES jb,
                                        PERSON_JOBS p_jb
                                WHERE     (PD.FIRM_CODE = F.FIRM_CODE)
                                        AND (R.RANK_ID = PD.RANK_ID)
                                    --  and JB.JOB_TYPE_ID=PD.JOB_TYPE_ID
                                        and P_JB.JOB_TYPE_ID=JB.JOB_TYPE_ID
                                        and P_JB.PERSON_CODE=PD.PERSON_CODE
                                        AND (R.RANK_CAT_ID = PD.RANK_CAT_ID)
                                        AND PD.OUT_UN_FORCE = 0 
                                        and F.FIRM_CODE='" + firm + @"'
                                        and P_JB.TO_DATE is null
                                        and JB.JOB_TYPE_ID=" + rol + @"
                                        and P_JB.APP_FLAG=1 ";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

//*************************************************************************************************
        public ActionResult GET_fin_year()
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT TRAINING_PERIODS.TRAINING_PERIOD,TRAINING_PERIODS.FIN_YEAR,TRAINING_PERIODS.TRAINING_PERIOD_ID FROM TRAINING_PERIODS WHERE   SYSDATE BETWEEN TRAINING_PERIODS.PERIOD_FROM AND TRAINING_PERIODS.PERIOD_TO";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
//*************************************************************************************************

        public string Max_FIRMS_ABSENCES_PERSONS_DET_ID()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(FIRMS_ABSENCES_PERSONS_DET.FIRMS_ABSENCES_PERSONS_DET_ID,   INSTR(FIRMS_ABSENCES_PERSONS_DET.FIRMS_ABSENCES_PERSONS_DET_ID,'-') +1    ))),0)+1) MAX_CODE  from FIRMS_ABSENCES_PERSONS_DET";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }

//*************************************************************************************************

        public int add_rec(string PERSON_CODE, string fin_year, string period_id, string firms, string RANK_CAT_ID, string person_rank_cat, int ABSENCE_TYPE_ID, string FullDateFrom, string FullDateTo)
        {
            Boolean TF = General.check_ABS(PERSON_CODE, fin_year, period_id, firms, RANK_CAT_ID, person_rank_cat, FullDateFrom, FullDateTo);

            if (TF == true)
            {
                string query = @" INSERT INTO FIRM_WORK.FIRMS_ABSENCES_PERSONS (
   PERSON_CODE, FIN_YEAR, TRAINING_PERIOD_ID, 
   FIRM_CODE, FROM_DATE, ABSENCE_TYPE_ID, 
   RANK_CAT_ID, PERSON_CAT_ID,  TO_DATE , ACT_DATE , COMMANDER_FLAG , ABSENCE_NOTES) 
   VALUES ('" + PERSON_CODE + "' , '" + fin_year + "' , '" + period_id + "' , '" + firms + "' ,TO_TIMESTAMP ('" + FullDateFrom + @"', 'dd/mm/yyyy hh24:mi') ,
   '" + ABSENCE_TYPE_ID + "' ,'" + RANK_CAT_ID + "' , '" + person_rank_cat + "' , TO_TIMESTAMP ('" + FullDateTo + @"', 'dd/mm/yyyy hh24:mi') , 
   TO_TIMESTAMP ('" + FullDateTo + "', 'dd/mm/yyyy hh24:mi') , 1 , '')";

                string conn = "1";
                OracleCommand cmd = new OracleCommand(query);
                General.exec_q(query, conn);


                string q2 = @"SELECT OFF_ABS_GROUP_ID FROM FIRM_WORK.OFF_ABS_GROP_OFF where ( ABS_CAT_ID = 10 and PERSON_DATA_ID = '" + PERSON_CODE + "' ) or (ABS_CAT_ID = 10 and PERSON_DATA_ID is null) ";
                var data = General.returnqueryresult(q2);
                if (data.Tables[0].Rows.Count == 0)
                {
                    string qq = @"SELECT OFF_ABS_GROUP_ID
                           FROM FIRM_WORK.OFF_ABS_GROUP where ABSCENCE_CATEGORY_ID = 10 and UNIT_DEF_GROUP = 1 and FIRMS_CODE = '" + firms + "' ";
                    var dd = General.returnqueryresult(qq);

                    string q3 = @"SELECT OFF_ABS_STEPS_ID, OFF_ABS_GROUP_ID, OFF_ABS_STEPS_NAME,  JOB_TYPE_ID, ORDER_ID
                         FROM FIRM_WORK.OFF_ABS_STEPS where OFF_ABS_GROUP_ID = " + dd.Tables[0].Rows[0].ItemArray[0] + " ";
                    var data2 = General.returnqueryresult(q3);


                    for (int i = 0; i < data2.Tables[0].Rows.Count; i++)
                    {
                        int j = 0;
                        var maxid = Max_FIRMS_ABSENCES_PERSONS_DET_ID();

                        string q4 = @"INSERT INTO FIRM_WORK.FIRMS_ABSENCES_PERSONS_DET (
   FIRMS_ABSENCES_PERSONS_DET_ID, PERSON_CODE, FIN_YEAR, 
   TRAINING_PERIOD_ID, FIRM_CODE, FROM_DATE,TO_DATE , ACT_TO_DATE ,  
   ABSENCE_TYPE_ID, RANK_CAT_ID, PERSON_CAT_ID, 
    OFF_ABS_STEPS_ID, OFF_ABS_GROUP_ID ) 
VALUES ('" + maxid + "' , '" + PERSON_CODE + "' , '" + fin_year + "' , '" + period_id + "' , '" + firms + @"' , 
              TO_TIMESTAMP ('" + FullDateFrom + @"', 'dd/mm/yyyy hh24:mi') ,TO_TIMESTAMP ('" + FullDateTo + @"', 'dd/mm/yyyy hh24:mi') ,
TO_TIMESTAMP ('" + FullDateTo + @"', 'dd/mm/yyyy hh24:mi'), '" + ABSENCE_TYPE_ID + "' , '" + RANK_CAT_ID + "' , '" + person_rank_cat + @"' ,
              '" + data2.Tables[0].Rows[i].ItemArray[j] + "' , '" + data2.Tables[0].Rows[i].ItemArray[j + 1] + "' )";

                        General.exec_q(q4, conn);

                    }

                    return 1;
                }
                else
                {
                    string q3 = @"SELECT OFF_ABS_STEPS_ID, OFF_ABS_GROUP_ID, OFF_ABS_STEPS_NAME,  JOB_TYPE_ID, ORDER_ID
                         FROM FIRM_WORK.OFF_ABS_STEPS where OFF_ABS_GROUP_ID = " + data.Tables[0].Rows[0].ItemArray[0] + " ";
                    var data2 = General.returnqueryresult(q3);


                    for (int i = 0; i < data2.Tables[0].Rows.Count; i++)
                    {
                        int j = 0;
                        var maxid = Max_FIRMS_ABSENCES_PERSONS_DET_ID();

                        string q4 = @"INSERT INTO FIRM_WORK.FIRMS_ABSENCES_PERSONS_DET (
   FIRMS_ABSENCES_PERSONS_DET_ID, PERSON_CODE, FIN_YEAR, 
   TRAINING_PERIOD_ID, FIRM_CODE, FROM_DATE,TO_DATE , ACT_TO_DATE ,  
   ABSENCE_TYPE_ID, RANK_CAT_ID, PERSON_CAT_ID, 
    OFF_ABS_STEPS_ID, OFF_ABS_GROUP_ID ) 
VALUES ('" + maxid + "' , '" + PERSON_CODE + "' , '" + fin_year + "' , '" + period_id + "' , '" + firms + @"' , 
              TO_TIMESTAMP ('" + FullDateFrom + "', 'dd/mm/yyyy hh24:mi') ,TO_TIMESTAMP ('" + FullDateTo + "', 'dd/mm/yyyy hh24:mi') ,TO_TIMESTAMP ('" + FullDateTo + "', 'dd/mm/yyyy hh24:mi') , '" + ABSENCE_TYPE_ID + "' , '" + RANK_CAT_ID + "' , '" + person_rank_cat + @"' ,
              '" + data2.Tables[0].Rows[i].ItemArray[j] + "' , '" + data2.Tables[0].Rows[i].ItemArray[j + 1] + "'   )";

                        General.exec_q(q4, conn);


                    }


                    return 1;

                }
            }
            else {

                return 2;
            }


        }
//*************************************************************************************************
       // bind steps grid
        public ActionResult retrieveSteps(string PERSON_CODE, string fin_year, int period_id, string FullDateFrom, string FullDateTo, string firms, int ABSENCE_TYPE_ID, int RANK_CAT_ID, int person_rank_cat)
        {

            string query = @"  SELECT DISTINCT OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                  OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                  OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                  OFF_ABS_STEPS.JOB_TYPE_ID AS ROL,
                  OFF_ABS_STEPS.ORDER_ID,
                  FIRMS_ABSENCES_PERSONS_DET.FIRMS_ABSENCES_PERSONS_DET_ID ,
  persn_name.RANK || ' / ' || persn_name.PERSON_NAME as PERSON_NAME,
                   persn_name.RANK,
                   persn_name.PERSON_CODE
    FROM OFF_ABS_STEPS, OFF_ABS_GROUP, FIRMS_ABSENCES_PERSONS_DET ,
 (SELECT distinct PERSON_DATA.PERSON_NAME, RANKS.RANK, PERSON_DATA.PERSON_CODE
            FROM person_data, FIRMS_ABSENCES_PERSONS_DET, ranks
           WHERE FIRMS_ABSENCES_PERSONS_DET.PERSON_DATE_OWEN = person_data.PERSON_CODE
              AND PERSON_DATA.RANK_ID = RANKS.RANK_ID ) persn_name
   WHERE FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_GROUP_ID =
            OFF_ABS_GROUP.OFF_ABS_GROUP_ID
         AND FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_STEPS_ID(+) =
                OFF_ABS_STEPS.OFF_ABS_STEPS_ID
         AND FIRMS_ABSENCES_PERSONS_DET.PERSON_DATE_OWEN = persn_name.PERSON_CODE(+)
         AND FIRMS_ABSENCES_PERSONS_DET.PERSON_CODE = '" + PERSON_CODE + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.FIN_YEAR = '" + fin_year + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.TRAINING_PERIOD_ID = '" + period_id + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.FIRM_CODE = '" + firms + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.FROM_DATE = TO_DATE ('" + FullDateFrom + @"', 'hh24:mi yyyy/mm/dd')
         AND FIRMS_ABSENCES_PERSONS_DET.TO_DATE = TO_DATE ('" + FullDateTo + @"', 'hh24:mi yyyy/mm/dd')
         AND FIRMS_ABSENCES_PERSONS_DET.ABSENCE_TYPE_ID = '" + ABSENCE_TYPE_ID + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.RANK_CAT_ID = '" + RANK_CAT_ID + @"'
         AND FIRMS_ABSENCES_PERSONS_DET.PERSON_CAT_ID = '" + person_rank_cat + @"'
ORDER BY OFF_ABS_STEPS.ORDER_ID
";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
  
        }

        // delete clinic order
        public int Update_details(int FIRMS_ABSENCES_PERSONS_DET_ID, string PERSON_CODE, string FIN_YEAR, int TRAINING_PERIOD_ID, string PERSON_DATE_OWEN, string FullDateFrom, string FullDateTo, string FIRM_CODE, int ABSENCE_TYPE_ID, int RANK_CAT_ID, int PERSON_CAT_ID)
        {

            string conn = "1";

            string query = @"UPDATE FIRM_WORK.FIRMS_ABSENCES_PERSONS_DET
                           SET   
                            PERSON_DATE_OWEN              = '" + PERSON_DATE_OWEN + @"'

                            WHERE 
                                   FIRMS_ABSENCES_PERSONS_DET_ID = '" + FIRMS_ABSENCES_PERSONS_DET_ID + @"'
                            AND    PERSON_CODE                   = '" + PERSON_CODE + @"'
                            AND    FIN_YEAR                      = '" + FIN_YEAR + @"'
                            AND    TRAINING_PERIOD_ID            = '" + TRAINING_PERIOD_ID + @"'
                            AND    FIRM_CODE                     = '" + FIRM_CODE + @"'
                            AND FROM_DATE = TO_DATE('" + FullDateFrom + @"', 'hh24:mi yyyy/mm/dd')
                            AND  TO_DATE = TO_DATE('" + FullDateTo + @"', 'hh24:mi yyyy/mm/dd')
                            AND    ABSENCE_TYPE_ID               = '" + ABSENCE_TYPE_ID + @"'
                            AND    RANK_CAT_ID                   = '" + RANK_CAT_ID + @"'
                            AND    PERSON_CAT_ID                 = '" + PERSON_CAT_ID + @"' ";

            General.exec_q(query, conn);

            return 1;
        }

        // delete clinic order
        public int Dele_Clinic_Order(string PERSON_CODE, string FIN_YEAR, int TRAINING_PERIOD_ID, string FullDateFrom, string FullDateTo, string FIRM_CODE, int ABSENCE_TYPE_ID, int RANK_CAT_ID, int PERSON_CAT_ID)
        {

            string conn = "1";

            string query = @"delete from FIRM_WORK.FIRMS_ABSENCES_PERSONS where PERSON_CODE = '" + PERSON_CODE + "' and FIN_YEAR = '" + FIN_YEAR + @"' and 
         TRAINING_PERIOD_ID = " + TRAINING_PERIOD_ID + " and  FROM_DATE = TO_DATE ('" + FullDateFrom + @"', 'hh24:mi yyyy/mm/dd') and TO_DATE = TO_DATE ('" + FullDateTo + @"', 'hh24:mi yyyy/mm/dd') and FIRM_CODE = '" + FIRM_CODE + @"' and
            ABSENCE_TYPE_ID = " + ABSENCE_TYPE_ID + " and RANK_CAT_ID = " + RANK_CAT_ID + " and PERSON_CAT_ID = " + PERSON_CAT_ID + " ";

            General.exec_q(query, conn);

            return 1;
        }

        // delete clinic order details
        public int Dele_Clinic_Order_Det(string PERSON_CODE, string FIN_YEAR, int TRAINING_PERIOD_ID, string FullDateFrom, string FullDateTo, string FIRM_CODE, int ABSENCE_TYPE_ID, int RANK_CAT_ID, int PERSON_CAT_ID)
        {

            string conn = "1";

            string query = @"delete from FIRM_WORK.FIRMS_ABSENCES_PERSONS_DET where  PERSON_CODE = '" + PERSON_CODE + "' and FIN_YEAR = '" + FIN_YEAR + @"' and 
         TRAINING_PERIOD_ID = " + TRAINING_PERIOD_ID + " and  FROM_DATE = TO_DATE ('" + FullDateFrom + @"', 'hh24:mi yyyy/mm/dd') and TO_DATE = TO_DATE ('" + FullDateTo + @"', 'hh24:mi yyyy/mm/dd') and FIRM_CODE = '" + FIRM_CODE + @"' and
            ABSENCE_TYPE_ID = " + ABSENCE_TYPE_ID + " and RANK_CAT_ID = " + RANK_CAT_ID + " and PERSON_CAT_ID = " + PERSON_CAT_ID + " ";

            General.exec_q(query, conn);

            return 1;
        }
//*************************************************************************************************
        // get data of person
        public JsonResult GET_JOP(string ID, string FIRM)
        {
            WF_EN en = new WF_EN();
            var typ = "";
            var per_id = "";
            var per_nm = "";
            var rnk_nm = "";
            var rnk_id = "";
            var rnk_cat_id = "";
            var per_cat = "";
            var off = "";
            var mang = "0";
            var add = "";
          
            if (ID != "")
            {
                if (en.PERSON_DATA.Any(o => o.PERSONAL_ID_NO == ID))
                {
                    var pcd = en.PERSON_DATA.First(o => o.PERSONAL_ID_NO == ID);
                    per_id = pcd.PERSON_CODE.ToString();
                    add = pcd.ADDERESS;
                    per_nm = pcd.PERSON_NAME;
                    rnk_nm = pcd.RANKS.RANK;
                    per_cat = pcd.PERSON_CAT_ID.ToString();
                    rnk_id = pcd.RANK_ID.ToString();
                    rnk_cat_id = pcd.RANK_CAT_ID.ToString();
                 

                }


            }

            var data = per_id + "/" + rnk_nm + "/" + per_nm + "/" + per_cat + "/" + rnk_id + "/" + rnk_cat_id + "/" + add + "/" + typ + "/" + off + "/" + mang;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
//**************************************************************************************************

        // get firm code of unit
        public JsonResult firms_ddl(string firm_code)
        {
            string query = @"SELECT FIRM_CODE,NAME
                             FROM firms
where FIRM_CODE='" + firm_code + "'";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
//*************************************************************************************

        // get rank
        public JsonResult RANK_CAT_ID_fun()
        {
            string query = @"SELECT RANK_CAT_ID,NAME FROM RANK_CATEGORIES
order by RANK_CAT_ID";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
//******************************************************************************************
        // get tamam
        public JsonResult absc_cat()
        {
            string query = @"SELECT ABSCENCE_CATEGORY_ID,ABSCENCE_CATEGORY
                             FROM ABSCENCE_CATEGORIES";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
//******************************************************************************************
        public ActionResult Index()
        {
           // var firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Include(f => f.PERSON_DATA);
            return View("Index");
        }

        //
        // GET: /Talab_Clinic/Details/5

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
        // GET: /Talab_Clinic/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return View();
        }

        //
        // POST: /Talab_Clinic/Create

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
        // GET: /Talab_Clinic/Edit/5

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
        // POST: /Talab_Clinic/Edit/5

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
        // GET: /Talab_Clinic/Delete/5

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
        // POST: /Talab_Clinic/Delete/5

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