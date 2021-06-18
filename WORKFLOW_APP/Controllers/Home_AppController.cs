using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WORKFLOW_APP.Models;
using WORKFLOW_APP.Views.Shared;
using Newtonsoft.Json;



namespace WORKFLOW_APP.Controllers
{
    public class Home_AppController : Controller
    {
        //
        // GET: /Home_App/
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult get_notif_fun( string per, string step_name)
        {
            message = "";
            status = false;
            WF_EN en = new WF_EN();

            string query = @"SELECT PRT_NOTIF.NOTIF_ID,
       PRT_NOTIF.APP_ID,
       PRT_NOTIF.NOTIF_DATE,
       PRT_NOTIF.NOTIF_DESCR,
       PRT_NOTIF.NOTIF_APROVAL_FLAG,
       PRT_NOTIF.NOTIF_READ,
       PRT_NOTIF.NOTIF_READ_DATE,
       PRT_NOTIF.NN,
       PRT_NOTIF.NOTIF_URL
  FROM PRT_NOTIF
  where   PRT_NOTIF.NN='29205251203156'
  and   PRT_NOTIF.APP_ID in (5,7,8,4)
  order by NOTIF_ID desc ";


            //var xx = Newtonsoft.Json.JsonConvert.SerializeObject(GetData(new OracleCommand(query)));
            var xx = GetData(new OracleCommand(query));
            var l = xx.Tables[0].Rows;
            //string x1 = "" ;
            //string x2 = "";
            //string x3 = "";
            //string x4 = "";
            //for (int i = 0; i < l.Count; i++)
            //{
            //    x1 += l[i]["GROUP_NAME"].ToString() + (i != l.Count - 1 ? "," : "");
            //    x2 += l[i]["FUNCTION_ANAME"].ToString() + (i != l.Count - 1 ? "," : "");
            //    x3 += l[i]["WINDOW_NAME"].ToString() + (i != l.Count - 1 ? "," : "");
            //    x4 += l[i]["USER_NAME"].ToString() + (i != l.Count - 1 ? "," : "");
            //}
            //var z = x1 + "/" + x2 + "/" + x3 + "/" + x4;
            var s = "";
            for (int i = 0; i < l.Count; i++)
            {
//                s += @"<li class=''>
//						<a href='" + l[i][8] + @"' target='_blank'>
//							<i class='menu-icon fa fa-picture-o'></i>
//							<span class='menu-text' style='font-size: large;font-size: 17px;font-weight: bold;'> " + l[i][3] + @" </span>
//						</a>
//
//						<b class='arrow'></b>
//					</li>";
                var w = l[i][5];
                int read = Convert.ToInt16(w);
                if (read == 0)
                {


                    s += @"     <li class='dropdown-content'>
                                <ul class='dropdown-menu dropdown-navbar navbar-pink'>
                                    <li style='background-color: #E1BCBC;'>
                                        <a href='" + l[i][8] + @"' target='_blank'>
                                            <div class='clearfix'>
                                                <span class='pull-left'>
                                                    <i class='btn btn-xs no-hover btn-pink fa fa-comment'></i>
                                                  " + l[i][3] + @"
                                                </span>
                                                <span class='pull-right badge badge-info'>  " + l[i][2] + @"</span>
                                            </div>
                                        </a>
                                    </li>

                  
                                </ul>
                            </li>";
                }

                else
                {
                    s += @"     <li class='dropdown-content'>
                                <ul class='dropdown-menu dropdown-navbar navbar-pink'>
                                    <li >
                                        <a href='" + l[i][8] + @"' target='_blank'>
                                            <div class='clearfix'>
                                                <span class='pull-left'>
                                                    <i class='btn btn-xs no-hover btn-pink fa fa-comment'></i>
                                                  " + l[i][3] + @"
                                                </span>
                                                <span class='pull-right badge badge-info'>  " + l[i][2] + @"</span>
                                            </div>
                                        </a>
                                    </li>

                  
                                </ul>
                            </li>";
                }
            }
         


            return new JsonResult { Data = new { status = status, message = message,s=s } };
        }



        [HttpPost]
        public ActionResult get_firm_fun(string per)
        {
            message = "";
            status = false;
            WF_EN en = new WF_EN();

            string query = @"select  FIRMS.FIRM_CODE, FIRMS.NAME
from firms,person_data
where 
firms.firm_code=person_data.firm_code
and person_data.PERSONAL_ID_NO='" + per+"'";



            //var xx = Newtonsoft.Json.JsonConvert.SerializeObject(GetData(new OracleCommand(query)));
            var xx = GetData_1(new OracleCommand(query));
            var firm = xx.Tables[0].Rows[0].ItemArray[0];




            return new JsonResult { Data = new { status = status, message = message, firm = firm } };
        }

        public JsonResult getPages()
        {

            string userID = HttpContext.Session["userID"].ToString();
            string query = @" SELECT FUNCTION_ANAME, WINDOW_NAME, ADMIN_SYSTEM_FUNCTIONS.FUNCTION_ID,ADMIN_SYSTEM_FUNCTIONS.IS_FOLDER
  FROM ADMIN_SYSTEM_FUNCTIONS, ADMIN_GROUPS_SYSTEM_FUNCTIONS, ADMIN_GROUPS
 WHERE ADMIN_SYSTEM_FUNCTIONS.FUNCTION_ID =
          ADMIN_GROUPS_SYSTEM_FUNCTIONS.FUNCTION_ID
       and ADMIN_SYSTEM_FUNCTIONS.app_flag= 1
       AND ADMIN_GROUPS.GROUP_ID = ADMIN_GROUPS_SYSTEM_FUNCTIONS.GROUP_ID 
                       and ADMIN_GROUPS.GROUP_ID in (select group_id from ADMIN_GROUPS_USERS where person_id = '" + userID + "') order by ADMIN_SYSTEM_FUNCTIONS.DISPLAY_ORDER";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        private static DataSet GetData_1(OracleCommand cmd)
        {

            string strConnString = @"Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True";
            //"DATA SOURCE=nspo;PERSIST SECURITY INFO=False;USER ID=nspo;PASSWORD=nspo";
            using (OracleConnection con = new OracleConnection(strConnString))
            {
                using (OracleDataAdapter sda = new OracleDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        ds.Tables.Clear();
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        private static DataSet GetData(OracleCommand cmd)
        {

            string strConnString = @"Data Source=wf;User ID=COMMAND;Password=COMMAND;Unicode=True";
            //"DATA SOURCE=nspo;PERSIST SECURITY INFO=False;USER ID=nspo;PASSWORD=nspo";
            using (OracleConnection con = new OracleConnection(strConnString))
            {
                using (OracleDataAdapter sda = new OracleDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        ds.Tables.Clear();
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        [HttpPost]
        public JsonResult person_id()
        {
           
           
            if (HttpContext.Session["userID"].ToString() != "")
            {
                string userID = HttpContext.Session["userID"].ToString();
                string query =
                      @" select PERSON_DATA.PERSONAL_ID_NO
                      from PERSON_DATA,ADMIN_USERS
                      where ADMIN_USERS.PERSON_CODE=PERSON_DATA.PERSON_CODE
                      and    ADMIN_USERS.PERSON_ID= " + userID;
                //OracleCommand cmd = new OracleCommand(query);
                //var data = General.GetData_New(cmd);

                var xx = GetData_1(new OracleCommand(query));
               var  data = xx.Tables[0].Rows[0].ItemArray[0];
               return new JsonResult { Data = new { status = status, message = message, pid = data } };
            }
            else
            {
                return new JsonResult { Data = new { status = status, message = message, pid = "" } };
            }
            

        }

    }
}
