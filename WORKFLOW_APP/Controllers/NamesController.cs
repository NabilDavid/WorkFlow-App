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
    public class NamesController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /Names/

        public ActionResult index()
        {
            return View();
        }


        public JsonResult getOfficers(string FIRM)
        {
           // var p = db.PERSON_DATA.First(o => o.FIRM_CODE == FIRM);
            string query =
                   @"SELECT PERSON_DATA.PERSON_CODE,
                             PERSON_DATA.PERSON_CAT_ID,
                             PERSON_DATA.PERSONAL_ID_NO ,
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
                             PERSON_DATA.ADDERESS,
                             PERSON_DATA.FIRM_CODE
                        FROM PERSON_DATA, FIRMS, RANKS
                       WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
                             AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1))
                                  AND (PERSON_DATA.FIRM_CODE IN
                                          (SELECT FIRMS_B.FIRM_CODE
                                             FROM FIRMS FIRMS_A, FIRMS FIRMS_B
                                            WHERE (FIRMS_A.FIRM_CODE ='" + FIRM + @"' )))
                                                
                             AND PERSON_DATA.RANK_CAT_ID = 1
                    ORDER BY person_data.firm_code ,
                             person_data.person_cat_id ,
                             person_data.rank_cat_id ,
                             person_data.rank_id ,
                             person_data.current_rank_date ,
                             person_data.id_no ";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
       
       

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}