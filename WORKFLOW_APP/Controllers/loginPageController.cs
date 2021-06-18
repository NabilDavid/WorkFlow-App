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

//using System.Web.HttpContext.Current.Session;

namespace WORKFLOW_APP.Controllers
{
    public class loginPageController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /loginPage/

        public ActionResult mainlogin()
        {
            
            return View();
        }




        [HttpPost]
        public ActionResult mainlogin(string userName, string passWord)
        {
            string query = @"select PERSON_ID , USER_NAME from admin_users where USER_NAME ='" + userName + "' AND USER_PASSWORD ='" + passWord + "'";
            OracleCommand cmd = new OracleCommand(query);
            DataSet data = DB.GetData(cmd);

       // string person_id= Json(data, JsonRequestBehavior.AllowGet);

        

            if (data.Tables[0].Rows.Count == 0)
            {
               
                return RedirectToAction("mainlogin", "loginPage");

            }

            else
            {
                string person_id = data.Tables[0].Rows[0].ItemArray[0].ToString();
                string name = data.Tables[0].Rows[0].ItemArray[1].ToString();
                Session["userID"] = person_id;
                Session["name"] = name;

                return RedirectToAction("index", "Home_App");

            }


        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}