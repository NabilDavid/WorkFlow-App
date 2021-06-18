using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WORKFLOW_APP.Models;
using System.Data.OracleClient;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WORKFLOW_APP.Views.Shared;
using Newtonsoft.Json;
namespace WORKFLOW_APP.Controllers
{
    public class groupUsersController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /groupUsers/

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

        public JsonResult getGroupToSearchFoundation()
        {

            
            string query =
               @"SELECT 
                GROUP_NAME 
                FROM ADMIN_GROUPS";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
          
        }
        


        public JsonResult getgroupGrid()
        {

            
            string query =
               @"SELECT 
               GROUP_ID, GROUP_NAME, DECODE (DEVELOPERS, 0, ' غير فعال',1,' فعال') ACTIVATION , DEVELOPERS 
                FROM ADMIN_GROUPS
                order by GROUP_ID desc";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
          
        }


        //----------------------------------------------------
        public int Max_groupId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(GROUP_ID,   INSTR(GROUP_ID,'-') +1    ))),0)+1) MAX_CODE  from ADMIN_GROUPS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return int.Parse(data[0]["MAX_CODE"].ToString());

        }



        //method to insert data in table admingroups
        public int insertGroup(string groupName, short activation)
        {

            int maxGroupId = Max_groupId();

            string query = @"INSERT INTO FIRM_WORK.ADMIN_GROUPS (DEVELOPERS, GROUP_ID, GROUP_NAME)
     VALUES ('" + activation + @"','" + maxGroupId + @"','" + groupName + @"')";

            int result = Controllers.DB.excutecommand(query);

            return result;
           
            
        }


        //method to update data in table admingroups
        public int updateGroup(string groupName, short activation, int groupId)
        {



            string query = @"UPDATE FIRM_WORK.ADMIN_GROUPS
    SET    
       DEVELOPERS    = '" + activation + @"',
       
       GROUP_NAME    = '" + groupName + @"'
      
      WHERE  GROUP_ID      = '" + groupId + @"'";

            int result = Controllers.DB.excutecommand(query);

            return result;


        }

        //method to delete data from table admingroups
        public int changeActivation(int groupID)
        {


            string query = @"UPDATE FIRM_WORK.ADMIN_GROUPS
         SET    
       DEVELOPERS  = 0  WHERE  GROUP_ID      = '" + groupID + @"'";

            int result = Controllers.DB.excutecommand(query);

            return result;


        }

        


        //----------------------------------------------------------------





        // GET: /groupUsers/Details/5

        public ActionResult Details(short id = 0)
        {
            ADMIN_GROUPS admin_groups = db.ADMIN_GROUPS.Find(id);
            if (admin_groups == null)
            {
                return HttpNotFound();
            }
            return View(admin_groups);
        }

        //
        // GET: /groupUsers/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /groupUsers/Create

        [HttpPost]
        public ActionResult Create(ADMIN_GROUPS admin_groups)
        {
            if (ModelState.IsValid)
            {
                db.ADMIN_GROUPS.Add(admin_groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin_groups);
        }

        //
        // GET: /groupUsers/Edit/5

        public ActionResult Edit(short id = 0)
        {
            ADMIN_GROUPS admin_groups = db.ADMIN_GROUPS.Find(id);
            if (admin_groups == null)
            {
                return HttpNotFound();
            }
            return View(admin_groups);
        }

        //
        // POST: /groupUsers/Edit/5

        [HttpPost]
        public ActionResult Edit(ADMIN_GROUPS admin_groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin_groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin_groups);
        }

        //
        // GET: /groupUsers/Delete/5

        public ActionResult Delete(short id = 0)
        {
            ADMIN_GROUPS admin_groups = db.ADMIN_GROUPS.Find(id);
            if (admin_groups == null)
            {
                return HttpNotFound();
            }
            return View(admin_groups);
        }

        //
        // POST: /groupUsers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            ADMIN_GROUPS admin_groups = db.ADMIN_GROUPS.Find(id);
            db.ADMIN_GROUPS.Remove(admin_groups);
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