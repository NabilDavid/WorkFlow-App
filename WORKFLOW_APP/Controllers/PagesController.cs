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
    public class PagesController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /Pages/

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

        public JsonResult getPages()
        {
            string query = @"SELECT FUNCTION_ID,SYSTEM_DEPT_ID,FUNCTION_ANAME,DECODE (IS_AVAILABLE, 0, ' غير فعال',1,' فعال') MYACTIVATION , IS_AVAILABLE , WINDOW_NAME  FROM ADMIN_SYSTEM_FUNCTIONS
                             WHERE APP_FLAG IS NOT NULL ORDER BY FUNCTION_ID DESC ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getGroups()
        {
            string query = @"select GROUP_ID,GROUP_NAME from ADMIN_GROUPS  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getParts()
        {
            string query = @"select SYSTEM_DEPT_ID,SYSTEM_DEPT from ADMIN_SYSTEM_PARTS  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        



        public JsonResult gePagesGroups(int FUNCTION_ID)
        {
            string query = @"select GROUP_ID from ADMIN_GROUPS_SYSTEM_FUNCTIONS WHERE FUNCTION_ID = '" + FUNCTION_ID + "'";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getPageToSearchFoundation()
        {
            string query = @"select function_aname from ADMIN_SYSTEM_FUNCTIONS  ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //------------------------------------------
        // function for insert page and his groups
        public int Max_FunctionId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(FUNCTION_ID,   INSTR(FUNCTION_ID,'-') +1    ))),0)+1) MAX_CODE  from ADMIN_SYSTEM_FUNCTIONS";

            OracleCommand cmd = new OracleCommand(query);

            var data = General.GetData_New(cmd);
            return int.Parse(data[0]["MAX_CODE"].ToString());

        }

        public int insertGroup(int maxFunctionId, string[] groups)
        {
            int result = 0;
            string query = "";
            for (int i = 0; i < groups.Length; i++)
            {
                query = @"INSERT INTO FIRM_WORK.ADMIN_GROUPS_SYSTEM_FUNCTIONS (FUNCTION_ID, GROUP_ID)
               VALUES ('" + maxFunctionId + @"','" + groups[i] + @"')";
                result = Controllers.DB.excutecommand(query);
            }

            return result;

        }

        public int deletePage(int maxPageId)
        {


            string query = @"delete from  FIRM_WORK.ADMIN_SYSTEM_FUNCTIONS where FUNCTION_ID = '" + maxPageId + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }

        public int insertPage(string functionName, short myActivation, string windowName, string groups , short part)
        {
            int result = 0;

            if (groups != "")
            {
                var groupsArray = groups.Split(',');
                int maxFunctionId = Max_FunctionId();


                string query = @"INSERT INTO FIRM_WORK.ADMIN_SYSTEM_FUNCTIONS (FUNCTION_ID, FUNCTION_ANAME , SYSTEM_DEPT_ID, IS_AVAILABLE , WINDOW_NAME , APP_FLAG)
               VALUES ('" + maxFunctionId + @"','" + functionName + @"','" + part + @"','" + myActivation + @"','" + windowName + @"', 1 )";

                result = Controllers.DB.excutecommand(query);


                if (result == 1)
                {
                    int x = insertGroup(maxFunctionId, groupsArray);
                    if (x == 0)
                    {
                        deletePage(maxFunctionId);
                        result = 0;
                    }

                }
            }
            else
            {



                int maxFunctionId = Max_FunctionId();
                string query = @"INSERT INTO FIRM_WORK.ADMIN_SYSTEM_FUNCTIONS (FUNCTION_ID, FUNCTION_ANAME , IS_AVAILABLE , WINDOW_NAME , APP_FLAG)
               VALUES ('" + maxFunctionId + @"','" + functionName + @"','" + myActivation + @"','" + windowName + @"', 1 )";
                result = Controllers.DB.excutecommand(query);


            }

            return result;
        }





        //--------------------------------------------

        // update page and groups

        public int updatePageAndGroup(int function_ID, string functionName, string windowName, short myActivation, 
                                    string newFunctionName, string newWindowName, int newMyActivation, int myNewPart ,string newGroups)
        {

            var groupsArray = newGroups.Split(',');
            int result = 0;
            int z = 0;

            // update page with new values
            int x = updatePage(function_ID, newFunctionName, newWindowName, newMyActivation, myNewPart);
            var data = gePagesGroups(function_ID);
            var aa = (((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>)(data.Data))).Count;

            if (x == 1)
            {
                if (aa != 0 && newGroups != "")
                {
                    int y = deleteGroups(function_ID);

                    if (y == 1)
                    {


                        z = insertGroup(function_ID, groupsArray);

                        if (z == 1)

                            result = 1;
                        else
                            result = 0;

                    }
                    else
                    {

                        // update page with old values
                        updatePage(function_ID, functionName, windowName, myActivation, myNewPart);
                        result = 0;
                    }
                }
                else
                {
                    if (newGroups != "")
                    {
                        z = insertGroup(function_ID, groupsArray);

                        if (z == 1)

                            result = 1;
                        else
                            result = 0;
                    }
                    else
                    {
                        deleteGroups(function_ID);
                        updatePage(function_ID, newFunctionName, newWindowName, newMyActivation, myNewPart);
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










        public int updatePage(int function_ID, string functionName, string windowName, int myActivation, int myNewPart)
        {
            string query = @"UPDATE FIRM_WORK.ADMIN_SYSTEM_FUNCTIONS
          SET    
            FUNCTION_ANAME    = '" + functionName + @"',
       
            WINDOW_NAME    = '" + windowName + @"',

             SYSTEM_DEPT_ID    = '" + myNewPart + @"',

            IS_AVAILABLE    = '" + myActivation + @"'
      
            WHERE  FUNCTION_ID  = '" + function_ID + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }


        public int deleteGroups(int function_ID)
        {


            string query = @"delete from  FIRM_WORK.ADMIN_GROUPS_SYSTEM_FUNCTIONS where FUNCTION_ID = '" + function_ID + @"'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }


        //--------------------------------------------
        // change activation for this page

        public int changeActivation(int function_ID)
        {


            string query = @"UPDATE FIRM_WORK.ADMIN_SYSTEM_FUNCTIONS SET IS_AVAILABLE = 0  WHERE  FUNCTION_ID = '" + function_ID + "'";

            int result = Controllers.DB.excutecommand(query);
            return result;


        }
        //--------------------------------------------

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}