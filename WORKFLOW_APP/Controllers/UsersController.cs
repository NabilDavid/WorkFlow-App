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
    public class UsersController : Controller
    {

        int flag = 0;
        private WF_EN db = new WF_EN();

        //
        // GET: /Users/

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Create()
        {
            flag = 1;
            ViewBag.flag = flag;
            return PartialView("_Parialusers");
        }
        public ActionResult Edit(short PERSON_ID, string USER_NAME, string USER_PASSWORD, string USER_DESC, int STATUS, int RANK_ID, string PERSON_CODE)
        {
            flag = 0;
            ViewBag.flag = flag;
            var groups = getUserGroups(PERSON_ID);
            var p = db.ADMIN_USERS.First(o => o.PERSON_ID == PERSON_ID);

            ViewBag.PERSON_ID = p.PERSON_ID;
            ViewBag.USER_NAME = p.USER_NAME;
            ViewBag.USER_PASSWORD = p.USER_PASSWORD;
            ViewBag.USER_DESC = p.USER_DESC;
            ViewBag.STATUS = p.STATUS;
            ViewBag.PERSON_CODE = PERSON_CODE;
            ViewBag.RANK_ID = RANK_ID;




            ViewBag.IsUpdate = true;
            return PartialView("_Parialusers");
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

        public JsonResult getPerson2(string firm_code, int rank_id)
        {


            string query =
                  @"       SELECT PERSON_DATA.PERSON_CODE,
                            RANKS.RANK ,
                              RANKS.RANK  ||' '|| PERSON_DATA.PERSON_NAME as PERSON_NAME  ,
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
        public JsonResult getPerson(string firm_code)
        {


            string query =
                  @"         SELECT PERSON_DATA.PERSON_CODE,
                            RANKS.RANK ,
                              RANKS.RANK  ||' '|| PERSON_DATA.PERSON_NAME as PERSON_NAME  ,
                             PERSON_DATA.RANK_ID
                        FROM PERSON_DATA, RANKS
                       WHERE   
                              (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) = 0))
                             AND PERSON_DATA.RANK_CAT_ID = 1
                             AND PERSON_DATA.FIRM_CODE ='" + firm_code + @"'
                            ORDER BY      person_data.rank_id  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

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


        // get the users for full gridView
        public JsonResult getUsers()
        {
            string query = @"SELECT ADMIN_USERS.PERSON_ID,
ADMIN_USERS.USER_NAME,
PERSON_DATA.PERSON_CODE,
PERSON_DATA.RANK_ID,
ADMIN_USERS.USER_PASSWORD,
ADMIN_USERS.USER_DESC,DECODE (ADMIN_USERS.STATUS, 0, ' غير فعال',1,' فعال') Activation , 
ADMIN_USERS.STATUS ,
PERSON_DATA.PERSON_NAME,
RANKS.RANK
FROM ADMIN_USERS ,PERSON_DATA,RANKS
where ADMIN_USERS.PERSON_CODE=PERSON_DATA.PERSON_CODE 
AND RANKS.RANK_ID = PERSON_DATA.RANK_ID
ORDER BY ADMIN_USERS.PERSON_ID DESC";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getUserToSearchFoundation()
        {
            string query = @"SELECT USER_NAME FROM ADMIN_USERS ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }


        // get the groups to full the combobox
        public JsonResult getGroups()
        {
            string query = @"select GROUP_ID,GROUP_NAME from ADMIN_GROUPS  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }




        // get the groups which users selected
        public JsonResult getUserGroups(int PERSON_ID)
        {
            string query = @"select GROUP_ID from ADMIN_GROUPS_USERS WHERE PERSON_ID = '" + PERSON_ID + "'";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }


        // max person id to insert new record 
        public int Max_userId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(PERSON_ID,   INSTR(PERSON_ID,'-') +1    ))),0)+1) MAX_CODE  from ADMIN_USERS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return int.Parse(data[0]["MAX_CODE"].ToString());

        }

        // insert new users 
        public int insertUser(string userName, short activation, string pssWord, string desc, string items, string firm, string person_code)
        {
            var groupsArray = items.Split(',');
            int maxUserId = Max_userId();
            int result = 0;
            if (items != "")
            {
                string query = @"INSERT INTO FIRM_WORK.ADMIN_USERS (PERSON_ID, USER_NAME , USER_PASSWORD , USER_DESC , STATUS , FIRM_CODE ,PERSON_CODE)
               VALUES ('" + maxUserId + @"','" + userName + @"','" + pssWord + @"','" + desc + @"','" + activation + @"','" + firm + @"','" + person_code + @"')";

                 result = Controllers.DB.excutecommand(query);


                if (result == 1)
                {
                    int x = insertGroup(maxUserId, groupsArray);
                    if (x == 0)
                    {
                        deleteUser(maxUserId);
                        result = 0;
                    }

                }
            }
            else {

                string query = @"INSERT INTO FIRM_WORK.ADMIN_USERS (PERSON_ID, USER_NAME , USER_PASSWORD , USER_DESC , STATUS , FIRM_CODE ,PERSON_CODE)
               VALUES ('" + maxUserId + @"','" + userName + @"','" + pssWord + @"','" + desc + @"','" + activation + @"','" + firm + @"','" + person_code + @"')";

                 result = Controllers.DB.excutecommand(query);
            
            }

            return result;
        }

        //insert new groups for this user
        public int insertGroup(int maxUserId, string[] groups)
        {
            int result = 0;
            string query = "";
            for (int i = 0; i < groups.Length; i++)
            {
                query = @"INSERT INTO FIRM_WORK.ADMIN_GROUPS_USERS (PERSON_ID, GROUP_ID)
               VALUES ('" + maxUserId + @"','" + groups[i] + @"')";
                result = Controllers.DB.excutecommand(query);
            }

            return result;

        }

        // delete users if group not inserted
        public int deleteUser(int maxUserId)
        {


            string query = @"delete from  FIRM_WORK.ADMIN_USERS where PERSON_ID = '" + maxUserId + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }

        //Change Activation for this user 
        public int changeActivation(int PERSON_ID)
        {


            string query = @"UPDATE FIRM_WORK.ADMIN_USERS SET STATUS = 0 WHERE  PERSON_ID = '" + PERSON_ID + "'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }




        public int updateUserAndGroup(int PERSON_ID, string userName, int Status, string passWord, string userDesc,
                                     string newUserName, string newPssWord, int newActivation, string newDesc, string newGroups)
        {

            var groupsArray = newGroups.Split(',');
            int result = 0;
            int z = 0;
            int x = updateUser(PERSON_ID, newUserName, newPssWord, newActivation, newDesc);
            var data = getUserGroups(PERSON_ID);
            var aa = (((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>)(data.Data))).Count;



            if (x == 1)
            {
                if (aa != 0 && newGroups != "")
                {
                    int y = deleteGroups(PERSON_ID);

                    if (y == 1)
                    {


                         z = insertGroup(PERSON_ID, groupsArray);

                        if (z == 1)

                            result = 1;
                        else
                            result = 0;

                    }
                    else
                    {

                        // update user with old values
                        updateUser(PERSON_ID, userName, passWord, Status, userDesc);
                        result = 0;
                    }
                }
                else
                {
                    if (newGroups != "")
                    {
                        z = insertGroup(PERSON_ID, groupsArray);

                        if (z == 1)

                            result = 1;
                        else
                            result = 0;
                    }
                    else
                    {
                        deleteGroups(PERSON_ID);
                        updateUser(PERSON_ID, newUserName, newPssWord, newActivation, newDesc);

                        result = 1;
                    }
                
                
                
                }
            }



            else
            {
                result = 0;
            }

            return result; ;
        }






        public int deleteGroups(int personId)
        {


            string query = @"delete from  FIRM_WORK.ADMIN_GROUPS_USERS where PERSON_ID = '" + personId + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }

        public int updateUser(int personID, string UserName, string PssWord, int Activation, string Desc)
        {
            string query = @"UPDATE FIRM_WORK.ADMIN_USERS
          SET    
            USER_NAME    = '" + UserName + @"',
       
            USER_PASSWORD    = '" + PssWord + @"',

            USER_DESC    = '" + Desc + @"',

            STATUS    = '" + Activation + @"'
      
            WHERE  PERSON_ID  = '" + personID + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}