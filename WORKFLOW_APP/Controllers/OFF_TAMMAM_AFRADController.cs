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
    public class OFF_TAMMAM_AFRADController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;

        //
        // GET: /TALAB_M2M/

        public string Max_ABSCENCEId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(FIRMS_ABSENCES_PERSONS.MISSION_ID,   INSTR(FIRMS_ABSENCES_PERSONS.MISSION_ID,'-') +1    ))),0)+1) MAX_CODE  from FIRMS_ABSENCES_PERSONS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }
        //
        // GET: /OFF_TAMMAM/

        public ActionResult Index()
        {
            var firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Include(f => f.PERSON_DATA);
            return View("index");
        }


        public ActionResult GET_grid_m2m_mem(string firm, string date)
        {
            WF_EN E = new WF_EN();
            string q = "";

            q = @" 
SELECT PERSON_DATA.RANK_ID,
         RANKS.RANK,   
         PERSON_DATA.PERSON_NAME,   
         FIRMS_ABSENCES_PERSONS.FIRM_CODE,   
     --   TO_CHAR( FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy')FROM_DATE,   
      --  TO_CHAR( FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy')TO_DATE,   
         to_char(FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy  HH24:MI:SS')TO_DATE,   
         to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy  HH24:MI:SS')FROM_DATE,  
     to_char(FIRMS_ABSENCES_PERSONS.ACT_DATE,'dd/mm/yyyy  HH24:MI:SS')ACT_DATE,  
         FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES, 
         ABSENCE_TYPES.NAME ABSENCE_NAME,  
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
         PERSON_DATA.CURRENT_RANK_DATE  
    FROM FIRMS_ABSENCES_PERSONS,   
         PERSON_DATA  ,RANKS,ABSENCE_TYPES
   WHERE ( PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE ) and  
           RANKS.RANK_ID=PERSON_DATA.RANK_ID AND
           ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID and
      --  FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 and
         ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + firm + @" ) AND  
         ( person_data.rank_cat_id in( 2,3) ) AND  
-- FIRMS_ABSENCES_PERSONS.FROM_DATE  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  FIRMS_ABSENCES_PERSONS.to_date <= NVL(FIRMS_ABSENCES_PERSONS.to_date ,TO_DATE('" + date + @"','dd/mm/yyyy')) 
       (to_date('" + date + @"','dd/mm/yyyy') between  to_date(FIRMS_ABSENCES_PERSONS.FROM_DATE) and  to_date(FIRMS_ABSENCES_PERSONS.to_date))) 
--act_date -----to_date
order by PERSON_DATA.RANK_ID
";



            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GET_grid_m2m_mem1(string firm, string date)
        {
            WF_EN E = new WF_EN();
            string q = "";

            q = @" 
SELECT PERSON_DATA.RANK_ID,
         RANKS.RANK,   
         PERSON_DATA.PERSON_NAME,   
         FIRMS_ABSENCES_PERSONS.FIRM_CODE,   
     --   TO_CHAR( FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy')FROM_DATE,   
      --  TO_CHAR( FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy')TO_DATE,   
         to_char(FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy  HH24:MI:SS')TO_DATE,   
         to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy  HH24:MI:SS')FROM_DATE,  
     to_char(FIRMS_ABSENCES_PERSONS.ACT_DATE,'dd/mm/yyyy  HH24:MI:SS')ACT_DATE,  
         FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES, 
         ABSENCE_TYPES.NAME ABSENCE_NAME,  
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
         PERSON_DATA.CURRENT_RANK_DATE  
    FROM FIRMS_ABSENCES_PERSONS,   
         PERSON_DATA  ,RANKS,ABSENCE_TYPES
   WHERE ( PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE ) and  
           RANKS.RANK_ID=PERSON_DATA.RANK_ID AND
           ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID and
      --  FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 and
         ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + firm + @" ) AND  
         ( person_data.rank_cat_id in( 2,3) ) AND  
-- FIRMS_ABSENCES_PERSONS.FROM_DATE  >=     TO_DATE('" + date + @"','dd/mm/yyyy') AND  FIRMS_ABSENCES_PERSONS.to_date <= NVL(FIRMS_ABSENCES_PERSONS.to_date ,TO_DATE('" + date + @"','dd/mm/yyyy')) 
       (to_date('" + date + @"','dd/mm/yyyy') between  to_date(FIRMS_ABSENCES_PERSONS.FROM_DATE) and  to_date(FIRMS_ABSENCES_PERSONS.to_date))) 
--act_date -----to_date
order by PERSON_DATA.RANK_ID
";



            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GET_grid_OFF_TIM(string firm_code, string fin_year, int P, string date)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());


            var dd = DateTime.Now.ToString(" HH:mm:ss");
            var datee = date + dd;
            string date1 = "to_date('" + datee + "','dd/mm/yyyy HH24:MI:SS')";

            string q = @" 
SELECT  RANKS.RANK,PERSON_DATA.PERSON_NAME, PERSON_DATA.PERSON_CODE,   
        TO_CHAR(person_daily_entry.from_date,'yyyy/mm/dd')FROM_DATE,
        TO_CHAR(PERSON_DAILY_ENTRY.ENTRY_DATE,'hh24:mi')ENTRY_DATE,  
        decode(TO_CHAR(PERSON_DAILY_ENTRY.ENTRY_DATE,'hh24:mi'),'','00:00',TO_CHAR(PERSON_DAILY_ENTRY.ENTRY_DATE,'hh24:mi'),TO_CHAR(PERSON_DAILY_ENTRY.ENTRY_DATE,'hh24:mi')  )TIME,
 decode(TO_CHAR(PERSON_DAILY_ENTRY.EXIT_DATE,'hh24:mi'),'','00:00',TO_CHAR(PERSON_DAILY_ENTRY.EXIT_DATE,'hh24:mi'),TO_CHAR(PERSON_DAILY_ENTRY.EXIT_DATE,'hh24:mi')  )EXIT_DATE,
   TO_CHAR(PERSON_DAILY_ENTRY.ENTRY_DATE,'dd/mm/yyyy')ENTRY_DATE1,  
        TO_CHAR( person_daily_entry.exit_date,'hh24:mi')EXIT_DATE1,   
         PERSON_DATA.firm_code,   
         person_data.id_no,   
         person_data.current_rank_date,   
         person_data.rank_id,   
         person_daily_entry.is_line,   
         person_data.sort_no,   
         person_data.category_id  
    FROM person_daily_entry,   
         person_data  ,RANKS
   WHERE ( person_daily_entry.person_code(+) = person_data.person_code ) and  
    person_daily_entry.firm_code(+)= person_data.firm_code  and
         ( ( person_daily_entry.from_date(+) = to_date('" + date + @"','dd/mm/yyyy') ) AND  
         ( person_data.firm_code = '" + firm_code + @"' ) AND  
         ( person_data.rank_id >= 2 ) AND  
   PERSON_DATA.RANK_CAT_ID in (2,3) AND
         ( person_data.out_un_force = 0 ) )    AND RANKS.RANK_ID = PERSON_DATA.RANK_ID 
 and  
       PERSON_DATA.PERSON_CODE not in(SELECT 
         FIRMS_ABSENCES_PERSONS.PERSON_CODE 
         
    FROM FIRMS_ABSENCES_PERSONS,   
         PERSON_DATA  ,RANKS,ABSENCE_TYPES
   WHERE ( PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE ) and  
           RANKS.RANK_ID=PERSON_DATA.RANK_ID AND
                  ABSENCE_TYPES.ABSENCE_TYPE_ID not in(16,17,50) and
           ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID and
         ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = '" + firm_code + @"' ) AND  
         ( person_data.rank_cat_id in (2,3) )   
 and           (
                (" + date1 + @" >= firms_absences_persons.FROM_DATE )
                 AND 
                (" + date1 + @" <= firms_absences_persons.to_date )
               )
        --(to_date('" + date1 + @"','dd/mm/yyyy') between  to_date(FIRMS_ABSENCES_PERSONS.FROM_DATE) and nvl(to_date(FIRMS_ABSENCES_PERSONS.TO_DATE),to_date('" + date1 + @"','dd/mm/yyyy'))) )

)
)
ORDER BY person_data.rank_id ASC,   
         person_data.current_rank_date ASC,   
         person_data.sort_no ASC 
";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TYP_ddl(string firm_code)
        {
            string query = @"SELECT ABSENCE_TYPE_ID,NAME
                             FROM ABSENCE_TYPES

--where ABSCENCE_CATEGORY_ID not in (1,2,3,4)";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //
        // GET: /OFF_TAMMAM/Details/5

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
        // GET: /OFF_TAMMAM/CreateCreate_OFF

        public ActionResult Create()
        {
            ViewBag.creat = true;
            //    ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create");
        }
        public ActionResult Create_Geha()
        {
            // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
            // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_Geha");
        }
        public ActionResult Create_OFF()
        {
            // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Create_OFF");
        }
        public ActionResult Edit_Time()
        {
            // ViewBag.FIRM_CODE = new SelectList(db.FIRM_MISSIONS, "FIRM_CODE", "MISSION_FIRM_CODE");
            // ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return PartialView("Edit_Time");
        }

        public ActionResult Edit_CUT(string FIN_YEAR, short TRAINING_PERIOD_ID, int ABSENCE_TYPE_ID, string FIRM_CODE, string PERSON_CODE, string FROM_DATE, int RANK_CAT_ID, int PERSON_CAT_ID)
        {
            //ViewBag.IsUpdate = false;

            var FROMs_DATE = Convert.ToDateTime(FROM_DATE);
            WF_EN en = new WF_EN();
            var firm_abs_name = en.FIRMS_ABSENCES_PERSONS.First(o => o.FIN_YEAR == FIN_YEAR && o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.ABSENCE_TYPE_ID == ABSENCE_TYPE_ID && o.PERSON_CODE == PERSON_CODE && o.FROM_DATE == FROMs_DATE && o.RANK_CAT_ID == RANK_CAT_ID && o.PERSON_CAT_ID == PERSON_CAT_ID);
            if (firm_abs_name == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = false;
            return PartialView("Create", firm_abs_name);
        }

        //
        // POST: /OFF_TAMMAM/Create

        [HttpPost]
        public ActionResult Create(FIRMS_ABSENCES_PERSONS firms_absences_persons, string PERSON_CODE1)
        {
            message = "";
            status = false;
            WF_EN E = new WF_EN();
            var name = E.PERSON_DATA.First(o => o.PERSONAL_ID_NO == PERSON_CODE1).PERSON_NAME;
            if (firms_absences_persons.FROM_DATE.Date >= DateTime.Now.Date && firms_absences_persons.TO_DATE.Value.Date >= DateTime.Now.Date && firms_absences_persons.ABSENCE_TYPE_ID != null)
            {
                if (General.check_ABS(firms_absences_persons.PERSON_CODE, firms_absences_persons.FIN_YEAR.ToString(),
                    firms_absences_persons.TRAINING_PERIOD_ID.ToString(), firms_absences_persons.FIRM_CODE.ToString(), "1", "1", firms_absences_persons.FROM_DATE.ToString("dd/MM/yyyy HH:mm"), firms_absences_persons.TO_DATE.Value.ToString("dd/MM/yyyy HH:mm")))
                {
                    string cccc = firms_absences_persons.FROM_DATE.ToLongDateString();

                    if (firms_absences_persons.FROM_DATE <= firms_absences_persons.TO_DATE)
                    {
                        string xx = "";
                        string yy = "";
                        var pers = from o in E.PERSON_DATA
                                   where o.PERSON_CODE == firms_absences_persons.PERSON_CODE
                                   select o;
                        foreach (var p in pers)
                        {


                            xx = Convert.ToString(p.RANK_CAT_ID);
                            yy = Convert.ToString(p.PERSON_CAT_ID);

                        }

                        db.FIRMS_ABSENCES_PERSONS.Add(new FIRMS_ABSENCES_PERSONS()
                        {


                            PERSON_CODE = firms_absences_persons.PERSON_CODE,// Convert.ToInt16(max),
                            TRAINING_PERIOD_ID = firms_absences_persons.TRAINING_PERIOD_ID,
                            ABSENCE_TYPE_ID = firms_absences_persons.ABSENCE_TYPE_ID,
                            FIRM_CODE = firms_absences_persons.FIRM_CODE,
                            FIN_YEAR = firms_absences_persons.FIN_YEAR,
                            ABSENCE_NOTES = firms_absences_persons.ABSENCE_NOTES,
                            RANK_CAT_ID = Convert.ToInt16(xx),
                            PERSON_CAT_ID = Convert.ToInt16(yy),
                            COMMANDER_FLAG = 1,
                            FROM_DATE = firms_absences_persons.FROM_DATE,
                            TO_DATE = firms_absences_persons.TO_DATE,
                            ACT_DATE = Convert.ToDateTime(firms_absences_persons.TO_DATE)
                            //MISSION_TYPE = firms_absences_persons.MISSION_TYPE,
                            // IS_DONE = firm_missions.IS_DONE,
                            //  INTRODUCTION = firms_absences_persons.INTRODUCTION
                        });

                        db.SaveChanges();
                        status = true;
                        message = "Successfully Saved.";

                        var Q = "CALL COMMAND.PRT_INS_NOTIF ( 7, 'قام " + name + "     بوضع التمام  ', 0, " + PERSON_CODE1 + " , 'http://192.223.30.3:90/WORKFLOW_APP/TALAB_M2M_OK?nn= " + PERSON_CODE1 + " ')";
                        General.exec_q(Q, "");
                        General.exec_q("COMMIT", "");



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



            //if (ModelState.IsValid)
            //{
            //    db.FIRMS_ABSENCES_PERSONS.Add(firms_absences_persons);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", firms_absences_persons.PERSON_CODE);
            //return View(firms_absences_persons);
        }
        [HttpPost]
        public ActionResult add_officer_fun_TIM(PERSON_DAILY_ENTRY person_daly_entry, string per, string step_name, string TIM)
        {
            message = "";
            status = false;
            string name = "";
            string xx = "";

            WF_EN en = new WF_EN();



            var FEE = en.PERSON_DAILY_ENTRY_FEE.First(O => O.FIRM_CODE == person_daly_entry.FIRM_CODE && O.SEQ == 1);
            var FEE_TIME = FEE.FEE_TIME.Value.Hour + ":" + FEE.FEE_TIME.Value.Minute;
            var VAL = FEE.FEE_VAL;
            var ENTRY_FEE = person_daly_entry.ENTRY_DATE.Value.Hour + ":" + person_daly_entry.ENTRY_DATE.Value.Minute;
            var X = Convert.ToDecimal(General.exec_f("SELECT GET_FEE('" + FEE_TIME + @"', '" + ENTRY_FEE + "') FROM DUAL", ""));
            var F_VAL = X > 0 ? (X * VAL) / FEE.FEE_PERIOD : 0;
            if (!en.PERSON_DAILY_ENTRY.Any(o => o.PERSON_CODE == person_daly_entry.PERSON_CODE && o.FIRM_CODE == person_daly_entry.FIRM_CODE && o.FROM_DATE == person_daly_entry.FROM_DATE))
            {

                if (person_daly_entry.FROM_DATE == DateTime.Now.Date)
                {
                    if (TIM != "00:00" && TIM != "" && TIM != "0:00" && TIM != "0:0" && TIM != ":00" && TIM != ":" && TIM != "")
                    {


                        PERSON_DAILY_ENTRY ent = new PERSON_DAILY_ENTRY();
                        ent.ENTRY_DATE = person_daly_entry.ENTRY_DATE;
                        ent.PERSON_CODE = person_daly_entry.PERSON_CODE;
                        ent.FROM_DATE = person_daly_entry.FROM_DATE;
                        ent.FIRM_CODE = person_daly_entry.FIRM_CODE;
                        ent.EXIT_DATE = person_daly_entry.EXIT_DATE;
                        ent.FEE_VAL = F_VAL;
                        en.PERSON_DAILY_ENTRY.Add(ent);
                        en.SaveChanges();
                        status = true;
                        status = true;


                    }
                    else
                    {
                        message = "أدخل التوقيت";
                        status = false;
                    }

                }
                else
                {
                    message = "لا يجوز تعديل التوقيت  ف غير اليوم الحالي";
                    status = false;
                }
            }



            else
            {

                if (person_daly_entry.FROM_DATE == DateTime.Now.Date)
                {
                    if (TIM != "00:00" && TIM != "" && TIM != "0:00" && TIM != "0:0" && TIM != ":00" && TIM != ":" && TIM != "")
                    {
                        var saad = en.PERSON_DAILY_ENTRY.First(o => o.PERSON_CODE == person_daly_entry.PERSON_CODE && o.FIRM_CODE == person_daly_entry.FIRM_CODE && o.FROM_DATE == person_daly_entry.FROM_DATE);
                        saad.EXIT_DATE = person_daly_entry.EXIT_DATE;
                        saad.ENTRY_DATE = person_daly_entry.ENTRY_DATE;
                        saad.FEE_VAL = F_VAL;
                        en.SaveChanges();
                        status = true;
                        //message = "حطأ!!!! لقد تم التصديق ع المامورية من قبل    ";
                        //status = false;
                    }
                    else if (TIM == "")
                    {
                        var entr = db.PERSON_DAILY_ENTRY.First(o => o.FIRM_CODE == person_daly_entry.FIRM_CODE && o.PERSON_CODE == person_daly_entry.PERSON_CODE && o.FROM_DATE == person_daly_entry.FROM_DATE);
                        db.PERSON_DAILY_ENTRY.Remove(entr);
                        db.SaveChanges();
                        message = "تم حذف التوقيت";
                        status = true;
                    }
                    else
                    {
                        message = "أدخل التوقيت";
                        status = false;
                    }
                }

                else
                {
                    message = "لا يجوز تعديل التوقيت  ف غير اليوم الحالي";
                    status = false;
                }

            }

            return new JsonResult { Data = new { status = status, message = message } };
        }
        //
        // GET: /OFF_TAMMAM/Edit/5

        public ActionResult Edit(string FIN_YEAR, short TRAINING_PERIOD_ID, int ABSENCE_TYPE_ID, string FIRM_CODE, string PERSON_CODE, string FROM_DATE, int RANK_CAT_ID, int PERSON_CAT_ID)
        {


            var FROMs_DATE = Convert.ToDateTime(FROM_DATE);
            WF_EN en = new WF_EN();
            var firm_abs_name = en.FIRMS_ABSENCES_PERSONS.First(o => o.FIN_YEAR == FIN_YEAR && o.TRAINING_PERIOD_ID == TRAINING_PERIOD_ID && o.FIRM_CODE == FIRM_CODE && o.ABSENCE_TYPE_ID == ABSENCE_TYPE_ID && o.PERSON_CODE == PERSON_CODE && o.RANK_CAT_ID == RANK_CAT_ID && o.PERSON_CAT_ID == PERSON_CAT_ID && o.FROM_DATE == FROMs_DATE);
            if (firm_abs_name == null)
            {
                return HttpNotFound();
            }


            ViewBag.IsUpdate = true;
            return PartialView("Create", firm_abs_name);
        }

        //
        // POST: /OFF_TAMMAM/Edit/5

        [HttpPost]
        public ActionResult Edit(FIRMS_ABSENCES_PERSONS firms_absences_persons)
        {

            WF_EN E = new WF_EN();
            short xx = 0;
            short yy = 0;
            var abs_cat = 0;
            var pers = from o in E.PERSON_DATA
                       where o.PERSON_CODE == firms_absences_persons.PERSON_CODE
                       select o;
            foreach (var p in pers)
            {


                xx = Convert.ToInt16(p.RANK_CAT_ID);
                yy = Convert.ToInt16(p.PERSON_CAT_ID);

            }

            FIRMS_ABSENCES_PERSONS f = new FIRMS_ABSENCES_PERSONS();
            f = E.FIRMS_ABSENCES_PERSONS.First(o => o.FIN_YEAR == firms_absences_persons.FIN_YEAR && o.TRAINING_PERIOD_ID == firms_absences_persons.TRAINING_PERIOD_ID && o.FIRM_CODE == firms_absences_persons.FIRM_CODE && o.ABSENCE_TYPE_ID == firms_absences_persons.ABSENCE_TYPE_ID && o.PERSON_CODE == firms_absences_persons.PERSON_CODE && o.FROM_DATE == firms_absences_persons.FROM_DATE && o.RANK_CAT_ID == xx && o.PERSON_CAT_ID == yy);
            if (firms_absences_persons.TO_DATE >= DateTime.Now)
            {
                if (firms_absences_persons.FROM_DATE <= firms_absences_persons.TO_DATE)
                {

                    f.ABSENCE_NOTES = firms_absences_persons.ABSENCE_NOTES;
                    f.TO_DATE = firms_absences_persons.TO_DATE;
                    f.ABSENCE_TYPE_ID = firms_absences_persons.ABSENCE_TYPE_ID;
                    f.ACT_DATE = firms_absences_persons.TO_DATE;
                    //f.FIN_YEAR = firms_absences_persons.FIN_YEAR;
                    //f.FIRM_NAME = firms_absences_persons.FIRM_NAME;
                    //f.SUBJECT = firms_absences_persons.SUBJECT;
                    //f.FROM_DATE = firms_absences_persons.FROM_DATE;
                    //f.TO_DATE = firms_absences_persons.TO_DATE;
                    //f.MISSION_TYPE = firms_absences_persons.MISSION_TYPE;
                    //// f.IS_DONE = firm_missions.IS_DONE;
                    //f.INTRODUCTION = firms_absences_persons.INTRODUCTION;
                    E.SaveChanges();

                    status = true;
                    message = "Successfully Saved.";

                }
                else
                {
                    status = false;
                    message = " تاريخ التمام الذي ادخلته اكبر من تاريخ العودة";
                }
            }
            else
            {
                status = false;
                message = " التاريخ الذي ادخلته اقل من تاريخ اليوم";
            }
            //  return RedirectToAction("Index");


            return new JsonResult { Data = new { status = status, message = message } };
        }

        [HttpPost]
        public ActionResult Edit_CUT(FIRMS_ABSENCES_PERSONS firms_absences_persons)
        {

            WF_EN E = new WF_EN();
            short xx = 0;
            short yy = 0;
            var abs_cat = 0;
            var pers = from o in E.PERSON_DATA
                       where o.PERSON_CODE == firms_absences_persons.PERSON_CODE
                       select o;
            foreach (var p in pers)
            {


                xx = Convert.ToInt16(p.RANK_CAT_ID);
                yy = Convert.ToInt16(p.PERSON_CAT_ID);

            }

            FIRMS_ABSENCES_PERSONS f = new FIRMS_ABSENCES_PERSONS();
            f = E.FIRMS_ABSENCES_PERSONS.First(o => o.FIN_YEAR == firms_absences_persons.FIN_YEAR && o.TRAINING_PERIOD_ID == firms_absences_persons.TRAINING_PERIOD_ID && o.FROM_DATE == firms_absences_persons.FROM_DATE && o.FIRM_CODE == firms_absences_persons.FIRM_CODE && o.ABSENCE_TYPE_ID == firms_absences_persons.ABSENCE_TYPE_ID && o.PERSON_CODE == firms_absences_persons.PERSON_CODE && o.RANK_CAT_ID == xx && o.PERSON_CAT_ID == yy);
            var ss = f.TO_DATE;
            if (firms_absences_persons.ACT_DATE >= DateTime.Now)
            {
                if (firms_absences_persons.FROM_DATE <= firms_absences_persons.ACT_DATE && ss >= firms_absences_persons.ACT_DATE)
                {

                    if (f.ABS_REF != null)
                    {

                        var abs = from s in E.ABSENCE_TYPES
                                  where s.ABSENCE_TYPE_ID == f.ABSENCE_TYPE_ID
                                  select s;
                        foreach (var pp in abs)
                        {


                            abs_cat = Convert.ToInt16(pp.ABSCENCE_CATEGORY_ID);
                            if (abs_cat == 2)
                            {
                                var vac = E.PERSON_VACATIONS.First(o => o.SEQ == f.ABS_REF);
                                vac.ACTUAL_END = firms_absences_persons.ACT_DATE;

                            }
                            else if (abs_cat == 3)
                            {
                                var mis = E.FIRM_MISSIONS_MEMBERS.First(o => o.MISSION_ID == f.ABS_REF);
                                mis.ACT_DATE = firms_absences_persons.ACT_DATE;
                            }


                        }
                    }

                    f.ABSENCE_NOTES = firms_absences_persons.ABSENCE_NOTES;
                    // f.TO_DATE = firms_absences_persons.TO_DATE;
                    // f.ABSENCE_TYPE_ID = firms_absences_persons.ABSENCE_TYPE_ID;
                    f.ACT_DATE = firms_absences_persons.ACT_DATE;
                    //f.FIN_YEAR = firms_absences_persons.FIN_YEAR;
                    //f.FIRM_NAME = firms_absences_persons.FIRM_NAME;
                    //f.SUBJECT = firms_absences_persons.SUBJECT;
                    //f.FROM_DATE = firms_absences_persons.FROM_DATE;
                    //f.TO_DATE = firms_absences_persons.TO_DATE;
                    //f.MISSION_TYPE = firms_absences_persons.MISSION_TYPE;
                    //// f.IS_DONE = firm_missions.IS_DONE;
                    //f.INTRODUCTION = firms_absences_persons.INTRODUCTION;
                    E.SaveChanges();

                    status = true;
                    message = "Successfully Saved.";

                }
                else
                {
                    status = false;
                    message = " التاريخ القطع الذي ادخلته أكبر  من تاريخ العودة";
                }
            }
            else
            {
                status = false;
                message = " التاريخ الذي ادخلته اقل من تاريخ اليوم";
            }
            //  return RedirectToAction("Index");


            return new JsonResult { Data = new { status = status, message = message } };
        }


        //
        // GET: /OFF_TAMMAM/Delete/5

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
        // POST: /OFF_TAMMAM/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(FIRMS_ABSENCES_PERSONS firms_absences_persons)
        {


            WF_EN E = new WF_EN();
            short xx = 0;
            short yy = 0;
            var pers = from o in E.PERSON_DATA
                       where o.PERSON_CODE == firms_absences_persons.PERSON_CODE
                       select o;
            foreach (var p in pers)
            {


                xx = Convert.ToInt16(p.RANK_CAT_ID);
                yy = Convert.ToInt16(p.PERSON_CAT_ID);

            }

            WF_EN en = new WF_EN();
            //            if (firms_absences_persons.FROM_DATE >= DateTime.Today && firms_absences_persons.TO_DATE >= DateTime.Today)
            //{
            FIRMS_ABSENCES_PERSONS de = en.FIRMS_ABSENCES_PERSONS.First(o => o.FIN_YEAR == firms_absences_persons.FIN_YEAR && o.TRAINING_PERIOD_ID == firms_absences_persons.TRAINING_PERIOD_ID && o.FIRM_CODE == firms_absences_persons.FIRM_CODE && o.ABSENCE_TYPE_ID == firms_absences_persons.ABSENCE_TYPE_ID && o.PERSON_CODE == firms_absences_persons.PERSON_CODE && o.FROM_DATE == firms_absences_persons.FROM_DATE && o.RANK_CAT_ID == xx && o.PERSON_CAT_ID == yy);

            en.FIRMS_ABSENCES_PERSONS.Remove(de);
            en.SaveChanges();




            // en.FIRM_MISSIONS.Remove(de);
            //en.SaveChanges();
            status = true;
            message = "Successfully Saved.";

            //}
            //else
            //{
            //    status = false;
            //    message = "تاريخ التمام اقل من تاريخ اليوم لا يمكن حذف التمام ";
            //}
            return new JsonResult { Data = new { status = status, message = message } };

            //FIRMS_ABSENCES_PERSONS firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Find(id);
            //db.FIRMS_ABSENCES_PERSONS.Remove(firms_absences_persons);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}