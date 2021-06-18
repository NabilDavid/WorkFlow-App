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
    public class JobsController : Controller
    {
        private WF_EN db = new WF_EN();
        int flag = 0;
        
     
        //
        // GET: /Jobs/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getfirm()
        {
            string userID = HttpContext.Session["userID"].ToString();

             string query =
                   @" select FIRM_CODE,NAME 
                      from firms
                      where firm_code = (select firm_code from ADMIN_USERS  where PERSON_ID = " + userID + ")";
                 
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getJobs()
        {
            string userID = HttpContext.Session["userID"].ToString();

             string query =
                   @" select JOB_NAME , JOB_TYPE_ID , SHORT_NAME , DECODE (APP_FLAG , 0, ' غير فعال',1,' فعال') APP_FLAG , APP_FLAG AS ACTIVATION
                      from JOBS_TYPES
                      where APP_FLAG is NOT NULL AND
                    firm_code = (select firm_code from ADMIN_USERS  where PERSON_ID = " + userID + ")";
                 
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        public JsonResult getperson(int JOB_TYPE_ID)
        {
            string query = "";

            query = @"select PERSON_DATA.PERSON_NAME , to_char(PERSON_JOBS.FROM_DATE,'yyyy/MM/dd') AS FROM_DATE 
                         ,to_char(PERSON_JOBS.TO_DATE,'yyyy/MM/dd') AS TO_DATE ,RANKS.RANK ,PERSON_JOBS.PERSON_CODE,PERSON_JOBS.SEQ
                  from PERSON_DATA , RANKS,PERSON_JOBS
                  where 
                 PERSON_DATA.PERSON_CODE = PERSON_JOBS.PERSON_CODE
                 AND RANKS.RANK_ID = PERSON_DATA.RANK_ID
                 And PERSON_JOBS.APP_FLAG IS NOT NULL 
                AND PERSON_JOBS.JOB_TYPE_ID =" + JOB_TYPE_ID;
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getperson2(int JOB_TYPE_ID , int state)
        {
            string query = "";
            if (state == 0)
            {
                query = @" select PERSON_DATA.PERSON_NAME , to_char(PERSON_JOBS.FROM_DATE,'yyyy/MM/dd') AS FROM_DATE 
                         ,to_char(PERSON_JOBS.TO_DATE,'yyyy/MM/dd') AS TO_DATE ,RANKS.RANK,PERSON_JOBS.PERSON_CODE,PERSON_JOBS.SEQ
                  from PERSON_DATA , RANKS,PERSON_JOBS
                  where 
                 PERSON_DATA.PERSON_CODE = PERSON_JOBS.PERSON_CODE
                 AND RANKS.RANK_ID = PERSON_DATA.RANK_ID
                 And PERSON_JOBS.APP_FLAG IS NOT NULL 
                 AND PERSON_JOBS.TO_DATE IS NOT NULL
                AND PERSON_JOBS.JOB_TYPE_ID =" + JOB_TYPE_ID;
            
                }
            else if (state == 1)
            {
                query = @" select PERSON_DATA.PERSON_NAME , to_char(PERSON_JOBS.FROM_DATE,'yyyy/MM/dd') AS FROM_DATE 
                         ,to_char(PERSON_JOBS.TO_DATE,'yyyy/MM/dd') AS TO_DATE ,RANKS.RANK,PERSON_JOBS.PERSON_CODE,PERSON_JOBS.SEQ
                  from PERSON_DATA , RANKS,PERSON_JOBS
                  where 
                 PERSON_DATA.PERSON_CODE = PERSON_JOBS.PERSON_CODE
                 AND RANKS.RANK_ID = PERSON_DATA.RANK_ID
                 And PERSON_JOBS.APP_FLAG IS NOT NULL 
                 AND PERSON_JOBS.TO_DATE IS NULL
                AND PERSON_JOBS.JOB_TYPE_ID =" + JOB_TYPE_ID;


            }

            else
            {

                query = @" select PERSON_DATA.PERSON_NAME , to_char(PERSON_JOBS.FROM_DATE,'yyyy/MM/dd') AS FROM_DATE 
                         ,to_char(PERSON_JOBS.TO_DATE,'yyyy/MM/dd') AS TO_DATE ,RANKS.RANK,PERSON_JOBS.PERSON_CODE,PERSON_JOBS.SEQ
                  from PERSON_DATA , RANKS,PERSON_JOBS
                  where 
                 PERSON_DATA.PERSON_CODE = PERSON_JOBS.PERSON_CODE
                 AND RANKS.RANK_ID = PERSON_DATA.RANK_ID
                 And PERSON_JOBS.APP_FLAG IS NOT NULL 
                AND PERSON_JOBS.JOB_TYPE_ID =" + JOB_TYPE_ID;
            
            }
                 
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Create()
        {
            flag = 1;
            ViewBag.flag = flag;
            return PartialView("_CreateJops");

        
        }

        public ActionResult Create2()
        {
            flag = 1;
            ViewBag.flag = flag;
            return PartialView("_CreatePerson");

        
        }


        public ActionResult Edit(int JOB_TYPE_ID, string JOB_NAME, string SHORT_NAME, int ACTIVATION)
        {
            flag = 0;

            string query = @" select JOB_NAME , JOB_TYPE_ID , SHORT_NAME , APP_FLAG
                      from JOBS_TYPES
                      where JOB_TYPE_ID =" + JOB_TYPE_ID;


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
           ViewBag.JOB_TYPE_ID = JOB_TYPE_ID;
           ViewBag.JOB_NAME = data[0]["JOB_NAME"];
           ViewBag.SHORT_NAME = data[0]["SHORT_NAME"];
           ViewBag.APP_FLAG = data[0]["APP_FLAG"];
           ViewBag.IsUpdate = true;
            return PartialView("_CreateJops");
        }
        public ActionResult Edit2(string PERSON_CODE,int SEQ)
        {
            flag = 0;
            int date_state = 0;
            string query = @" select PERSON_CODE , RANK_ID , FROM_DATE , TO_DATE
                      from PERSON_JOBS
                      where SEQ =" + SEQ+ @" AND PERSON_CODE = '"+ PERSON_CODE+"'";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            ViewBag.PERSON_CODE = PERSON_CODE;
            ViewBag.RANK_ID = data[0]["RANK_ID"];
            ViewBag.FROM_DATE = data[0]["FROM_DATE"];
            ViewBag.TO_DATE = data[0]["TO_DATE"];
            ViewBag.SEQ = SEQ;
            PERSON_JOBS F_H = db.PERSON_JOBS.First(o => o.PERSON_CODE == PERSON_CODE && o.SEQ == SEQ);

            if (F_H.TO_DATE == null)
            {
                date_state = 1;
            }
            else
            {
                date_state = 0;
            }
            ViewBag.flag = flag;
            ViewBag.date_state = date_state;
            if (F_H == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsUpdate = true;
            return PartialView("_CreatePerson",F_H);
        }

        public int Max_JopId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(JOB_TYPE_ID,   INSTR(JOB_TYPE_ID,'-') +1    ))),0)+1) MAX_CODE  from JOBS_TYPES";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return int.Parse(data[0]["MAX_CODE"].ToString());

        }
        public int Max_SeqId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(SEQ,   INSTR(SEQ,'-') +1    ))),0)+1) MAX_CODE  from PERSON_JOBS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return int.Parse(data[0]["MAX_CODE"].ToString());

        }

        
        public int insertJops(string jopType, string shortCut, int CreateState, string firm)
        {
            int maxJopId = Max_JopId();
            int result = 0; ;
            string query = @"INSERT INTO JOBS_TYPES (JOB_TYPE_ID, RANK_CAT_ID,PERSON_CAT_ID,JOB_NAME,SHORT_NAME,APP_FLAG,FIRM_CODE)
               VALUES ('" + maxJopId + @"','" + '1' + @"','" + '1' + @"','" + jopType + @"','" + shortCut + @"','" + CreateState + @"','" + firm + @"')";
                result = Controllers.DB.excutecommand(query);
           
            return result;

        }

        public int updateJob(string jopType, string shortCut, int CreateState, int Job_Type_ID)
        {
            string query = @"UPDATE FIRM_WORK.JOBS_TYPES
          SET    
            JOB_NAME    = '" + jopType + @"',
       
            SHORT_NAME    = '" + shortCut + @"',

            APP_FLAG    = '" + CreateState + @"'
      
            WHERE  JOB_TYPE_ID  = '" + Job_Type_ID + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }






        public JsonResult getrank()
        {


            string query =
                  @" SELECT    RANK, RANK_ID
                            
                        FROM  RANKS
                   WHERE RANK_ID BETWEEN 1 AND 8
                            ORDER BY  rank_id  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getOfficer(string firm_code, int rank_id)
        {


            string query =
                   @"       SELECT 
                             PERSON_DATA.PERSON_CODE,
                             RANKS.RANK , PERSON_DATA.PERSON_NAME  ,
                             PERSON_DATA.RANK_ID
                        FROM PERSON_DATA, RANKS
                       WHERE   
                              (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) = 0))
                             AND PERSON_DATA.RANK_CAT_ID = 1
                             AND PERSON_DATA.RANK_ID = '" + rank_id + @"'
                             AND PERSON_DATA.FIRM_CODE ='" + firm_code + @"'
                            ORDER BY      person_data.rank_id  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public int Delete(int SEQ , string PERSON_CODE)
        {


            string query = @"delete from  PERSON_JOBS where SEQ = '" + SEQ + @"'AND PERSON_CODE ='"+PERSON_CODE+"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }
        public int insertPerson(string name, DateTime from, DateTime? to, string firm_code, int rank_id, int app_flag, int JOB_TYPE_ID)
        {
            int result = 0; 
            int flag = 0;
            int status = 0;
            short  maxJopId = (short)Max_SeqId();
            

            db.PERSON_JOBS.Add(new PERSON_JOBS 
            {
                
                SEQ = maxJopId,
                PERSON_CODE = name,
                JOB_TYPE_ID = (short)JOB_TYPE_ID,
                RANK_ID = (short)rank_id,
                RANK_CAT_ID =1,
                PERSON_CAT_ID=1,
                FIRM_CODE = firm_code,
                FROM_DATE = from,
                TO_DATE = to,
                
            });
            flag = db.SaveChanges();




            // another query becuase app flag is not in model 
            string query = @"Update  PERSON_JOBS
               SET APP_FLAG = '" + app_flag + @"' 
               WHERE seq = '"+ maxJopId +@"'";
              result = Controllers.DB.excutecommand(query);



            if (flag == 1 && result==1)
            {
                status = 1;

            }
            else
            {
                status = 0;

            }
            return status;

        }
       
        public int UpdatePerson(string newName, string name1, DateTime newFrom, DateTime? newTo, string firm_code, int newRank_id, int app_flag, int JOB_TYPE_ID , int SEQ)
        {
            var db = new WF_EN();
            int flag = 0;
            int result = 0; 
            int status = 0;

            var job = db.PERSON_JOBS.Where(o => o.SEQ == SEQ && o.PERSON_CODE == name1).FirstOrDefault();
            if (job != null)
            {
                job.PERSON_CODE = newName;
                job.RANK_ID = (short)newRank_id;
                job.FROM_DATE = newFrom;
                job.TO_DATE=newTo;
               
            }
            db.Entry(job).State = System.Data.EntityState.Modified;
            flag = db.SaveChanges();

            string query = @"Update  PERSON_JOBS
               SET APP_FLAG = '" + app_flag + @"' 
               WHERE seq = '"+ SEQ +@"' AND PERSON_CODE ='"+name1+"'";
              result = Controllers.DB.excutecommand(query);



            if (flag == 1 && result == 1)
            {
                status = 1;
            }
            else {
            status = 0;
            }







            return status;
        }
       
        
    }
}
