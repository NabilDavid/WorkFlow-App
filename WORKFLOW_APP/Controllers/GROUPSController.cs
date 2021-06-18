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
    public class GROUPSController : Controller
    {
        private WF_EN db = new WF_EN();
        string message = "";
        bool status = false;

        public JsonResult getgroupGrid( string GR)
        {

          //  var off_abs_group = db.OFF_ABS_GROUP.Include(o => o.ABSCENCE_CATEGORIES).Include(o => o.FIRMS).Include(o => o.FIRMS1);
            string query =
               @"SELECT 
  OFF_ABS_GROUP_ID, FIRMS_CODE,abc. ABSCENCE_CATEGORY_ID,f.FIRM_CODE,ABC.ABSCENCE_CATEGORY_ID, ABC.ABSCENCE_CATEGORY,
   OFF_ABS_GROUP_NAME, ACTIV_F, PARANT_FIRMS_CODE, 

     DECODE (UNIT_DEF_GROUP, 0, ' غير اساسية',1,'اساسية') DEF_NAME,
   UNIT_CODE_N,
   F.NAME,
   P.NAME PN 
FROM FIRM_WORK.OFF_ABS_GROUP, firms f , firms p ,ABSCENCE_CATEGORIES abc
where OFF_ABS_GROUP.FIRMS_CODE=F.FIRM_CODE(+)
and OFF_ABS_GROUP.PARANT_FIRMS_CODE=P.FIRM_CODE(+)
and OFF_ABS_GROUP.ABSCENCE_CATEGORY_ID=abc.ABSCENCE_CATEGORY_ID(+)
and OFF_ABS_GROUP.ABSCENCE_CATEGORY_ID="+GR+@"
order by OFF_ABS_GROUP_ID";

            //OracleCommand cmd = new OracleCommand(query);
            //// Populate the DataSet.
            //DataSet data = General.GetData(cmd);
            //// return the Customers table as XML.
            //System.IO.StringWriter writer = new System.IO.StringWriter();
            //data.Tables[0].WriteXml(writer, XmlWriteMode.WriteSchema, false);
            //return writer.ToString();

           // DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(ds);

        }



        public JsonResult GET_JOP(string ID,  string FIRM)
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
                    //if (en.PERSON_VACATIONS.Any(o => o.SEQ == SEQ && o.PERSON_CODE == PER))
                    //{
                    //    var vac = en.PERSON_VACATIONS.First(o => o.SEQ == SEQ && o.PERSON_CODE == PER);
                    //    if (vac.OTHER_PERSON_CODE == pcd.PERSON_CODE)
                    //    {
                    //        typ = "exc";
                    //    }
                    //    if (vac.SUPERVISOR_CODE == pcd.PERSON_CODE)
                    //    {
                    //        typ += typ.Length != 0 ? ",spr" : "spr";
                    //    }
                    //}

                    //if (pcd.PERSON_CODE == en.PERSON_DATA.First(o => o.JOB_TYPE_ID == 1 && o.OUT_UN_FORCE == 0).PERSON_CODE)
                    //{
                    //    typ += typ.Length != 0 ? ",cmd" : "cmd";
                    //    mang = "1";
                    //}
                    //if (pcd.PERSON_CODE == en.PERSON_DATA.First(o => o.JOB_TYPE_ID == 2 && o.OUT_UN_FORCE == 0).PERSON_CODE)
                    //{
                    //    typ += typ.Length != 0 ? ",vcm" : "vcm";
                    //    mang = "2";
                    //}
                    //var of = en.FIRM_DEPTS_RESPONSIBLES.First(o => o.DEPARTMENT_ID == 39 && o.FIRM_CODE == FIRM);
                    //if (pcd.PERSON_CODE == of.OFF1_PERSON_CODE || pcd.PERSON_CODE == of.OFF2_PERSON_CODE || pcd.PERSON_CODE == of.SOL1_PERSON_CODE || pcd.PERSON_CODE == of.SOL2_PERSON_CODE)
                    //{
                    //    typ += typ.Length != 0 ? ",pln" : "pln";
                    //    off = "0";
                    //}
                    per_nm = pcd.PERSON_NAME;
                    rnk_nm = pcd.RANKS.RANK;
                    per_cat = pcd.PERSON_CAT_ID.ToString();
                    rnk_id = pcd.RANK_ID.ToString();
                    rnk_cat_id = pcd.RANK_CAT_ID.ToString();
                }


            }
            var data= per_id + "/" + rnk_nm + "/" + per_nm + "/" + per_cat + "/" + rnk_id + "/" + rnk_cat_id + "/" + add + "/" + typ + "/" + off + "/" + mang;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public string Max_groupId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(OFF_ABS_GROUP_ID,   INSTR(OFF_ABS_GROUP_ID,'-') +1    ))),0)+1) MAX_CODE  from OFF_ABS_GROUP";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }
        public string Max_stepId()
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(OFF_ABS_STEPS_ID,   INSTR(OFF_ABS_STEPS_ID,'-') +1    ))),0)+1) MAX_CODE  from OFF_ABS_STEPS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }
        public string Max_ORDERId(decimal GROUP_ID)
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(ORDER_ID,   INSTR(OFF_ABS_GROUP_ID,'-') +1    ))),0)+1) MAX_CODE  from OFF_ABS_STEPS
WHERE OFF_ABS_GROUP_ID=" + GROUP_ID;

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }
        public string Max_offId(decimal GROUP_ID)
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(OFF_ABS_GROUP_OFF_ID,   INSTR(OFF_ABS_GROUP_OFF_ID,'-') +1    ))),0)+1) MAX_CODE  from OFF_ABS_GROP_OFF
WHERE OFF_ABS_GROUP_ID=" + GROUP_ID;

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }

        public ActionResult GET_STEPS(string GR)
        {
            var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT OFF_ABS_STEPS_ID,
                           STP.OFF_ABS_GROUP_ID,
                           OFF_ABS_STEPS_NAME,
                           STP.OFF_ROLE_ARCHIVE,
                           ORDER_ID,
                           GR.OFF_ABS_GROUP_NAME,
                           JBTYP.SHORT_NAME ARH_ROLE_NAME
                      FROM OFF_ABS_STEPS STP, OFF_ABS_GROUP GR, JOBS_TYPES jbtyp
                     WHERE STP.OFF_ABS_GROUP_ID = GR.OFF_ABS_GROUP_ID
                           AND STP.JOB_TYPE_ID = jbtyp.JOB_TYPE_ID
                           AND STP.OFF_ABS_GROUP_ID = " + GR + " order by ORDER_ID";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GET_off(string GR)
        {
          //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT 
OFF_ABS_GROUP_OFF_ID, OFF_ABS_GROUP_ID, OFF_SKELETON_OFFICERS_ID, PERSON_DATA.PERSON_NAME,RANKS.RANK,
   ABS_CAT_ID, PERSON_DATA_ID
FROM FIRM_WORK.OFF_ABS_GROP_OFF,PERSON_DATA,ranks
where PERSON_DATA.PERSON_CODE=OFF_ABS_GROP_OFF.PERSON_DATA_ID
and PERSON_DATA.RANK_ID=RANKS.RANK_ID 
and OFF_ABS_GROUP_ID=" + GR;
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GET_off_all(string firms, string rank_cat)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT PERSON_DATA.PERSON_CODE,
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
                             PERSON_DATA.ADDERESS,
                             PERSON_DATA.FIRM_CODE
                        FROM PERSON_DATA, FIRMS, RANKS
                       WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
                             AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1)
                                  AND (PERSON_DATA.FIRM_CODE IN
                                          (SELECT FIRMS_B.FIRM_CODE
                                             FROM FIRMS FIRMS_A, FIRMS FIRMS_B
                                            WHERE (FIRMS_A.FIRM_CODE = '" + firms+@"')
                                                  AND (FIRMS_B.FIRM_LIKE_CODE LIKE
                                                          CONCAT (FIRMS_A.FIRM_LIKE_CODE, '%')))
                                       OR PERSON_DATA.BORROW_FIRM_CODE = '"+firms+@"'))
                             AND PERSON_DATA.RANK_CAT_ID = "+rank_cat+@"
                    ORDER BY person_data.firm_code ,
                             person_data.person_cat_id ,
                             person_data.rank_cat_id ,
                             person_data.rank_id ,
                             person_data.current_rank_date ,
                             person_data.id_no ";

            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GET_off_all_SAF(string firms, string rank_cat)
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT PERSON_DATA.PERSON_CODE,
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
                             PERSON_DATA.ADDERESS,
                             PERSON_DATA.FIRM_CODE
                        FROM PERSON_DATA, FIRMS, RANKS
                       WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
                             AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
                             AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
                             AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
                             AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1)
                                  AND (PERSON_DATA.FIRM_CODE IN
                                          (SELECT FIRMS_B.FIRM_CODE
                                             FROM FIRMS FIRMS_A, FIRMS FIRMS_B
                                            WHERE (FIRMS_A.FIRM_CODE = '" + firms + @"')
                                                  AND (FIRMS_B.FIRM_LIKE_CODE LIKE
                                                          CONCAT (FIRMS_A.FIRM_LIKE_CODE, '%')))
                                       OR PERSON_DATA.BORROW_FIRM_CODE = '" + firms + @"'))
                             AND PERSON_DATA.RANK_CAT_ID = " + rank_cat + @"
                    ORDER BY person_data.firm_code ,
                             person_data.person_cat_id ,
                             person_data.rank_cat_id ,
                             person_data.rank_id ,
                             person_data.current_rank_date ,
                             person_data.id_no ";

            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult firms_ddl(string firm_code)
        {
            string query = @"SELECT FIRM_CODE,NAME
                             FROM firms
where FIRM_CODE='" + firm_code+"'";
                            

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ARH_ROLE(string firm_code)
        {
            string query = @"select JOBS_TYPES.JOB_TYPE_ID,JOBS_TYPES.JOB_NAME
from JOBS_TYPES
where JOBS_TYPES.RANK_CAT_ID=1
and app_flag=1
";


//            string query = @"select ARH_ROLE_CODE, ARH_ROLE_NAME 
//from OFF_ROLE_ARCHIVE 
//where CALC_TNZ_UNIT_CODE=" + firm_code + @"
//order by ARH_ROLE_CODE";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        public JsonResult RANK_CAT_ID_fun()
        {
            string query = @"SELECT RANK_CAT_ID,NAME FROM RANK_CATEGORIES 
order by RANK_CAT_ID";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult RANK_CAT_ID_fun_SAF()
        {
            string query = @"SELECT RANK_CAT_ID,NAME FROM RANK_CATEGORIES where RANK_CAT_ID not in(1,10)
order by RANK_CAT_ID";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult absc_cat()
        {
            string query = @"SELECT ABSCENCE_CATEGORY_ID,ABSCENCE_CATEGORY
                             FROM ABSCENCE_CATEGORIES";


            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

      
        // GET: /GROUPS/

        public ActionResult Index()
        {
            var off_abs_group = db.OFF_ABS_GROUP.Include(o => o.ABSCENCE_CATEGORIES).Include(o => o.FIRMS).Include(o => o.FIRMS1);
            return View(off_abs_group.ToList());
        }

        //
        // GET: /GROUPS/Details/5

        public ActionResult Details(decimal id = 0)
        {
            OFF_ABS_GROUP off_abs_group = db.OFF_ABS_GROUP.Find(id);
            if (off_abs_group == null)
            {
                return HttpNotFound();
            }
            return View(off_abs_group);
        }

        //
        // GET: /GROUPS/Create

        public ActionResult Create()
        {
            ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY",1);
            ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "NAME");
            ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "PARENT_FIRM_CODE", "NAME");
            return PartialView("Create");
        }


        public ActionResult Create_Steps()
        {
           // ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", 1);
            //ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "NAME");
            //ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "PARENT_FIRM_CODE", "NAME");
            return PartialView("Create_Steps");
        }
        public ActionResult Create_Off()
        {
            // ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", 1);
            //ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "NAME");
            //ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "PARENT_FIRM_CODE", "NAME");
            return PartialView("Create_Off");
        }
        //
        // POST: /GROUPS/Create
        [HttpPost]
        public ActionResult Create(OFF_ABS_GROUP off_abs_group)
        {
            message = "";
            status = false;
            decimal grp_id = -1;
            var max = Max_groupId();

            //var pg = db.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == xx && o.UNIT_DEF_GROUP == 1).OFF_ABS_GROUP_ID;
            //var steps = from o in db.OFF_ABS_GROUP
            //            where o.OFF_ABS_GROUP_ID == off_abs_group.ABSCENCE_CATEGORY_ID && o.UNIT_DEF_GROUP == 1
            //            select o;
            //foreach (var s in steps)
            //{
            //    grp_id = s.OFF_ABS_GROUP_ID;
            //}
       //    if (ModelState.IsValid)
            //{
            if (!db.OFF_ABS_GROUP.Any(o => o.ABSCENCE_CATEGORY_ID == off_abs_group.ABSCENCE_CATEGORY_ID && o.UNIT_DEF_GROUP == 1))
            {
                var Fee = new OFF_ABS_GROUP();
                WF_EN enw = new WF_EN();
                Fee.OFF_ABS_GROUP_ID = Convert.ToDecimal(max);

                Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                Fee.ACTIV_F = off_abs_group.ACTIV_F;
                Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                Fee.UNIT_DEF_GROUP = 1;
                enw.OFF_ABS_GROUP.Add(Fee);
                enw.SaveChanges();
                //db.OFF_ABS_GROUP.Add(new OFF_ABS_GROUP()
                //{
                //    OFF_ABS_GROUP_ID =Convert.ToDecimal(max),
                //    FIRMS_CODE = off_abs_group.FIRMS_CODE,
                //    ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID,
                //    OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME,
                //    ACTIV_F = off_abs_group.ACTIV_F,
                //    PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE,
                //    UNIT_DEF_GROUP = off_abs_group.UNIT_DEF_GROUP
                //});

                //db.SaveChanges();
                status = true;
                message = "تم اضافة مجموعه جديدة";
            }
            else
            {
                var Fee = new OFF_ABS_GROUP();
                WF_EN enw = new WF_EN();
                Fee.OFF_ABS_GROUP_ID = Convert.ToDecimal(max);

                Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                Fee.ACTIV_F = off_abs_group.ACTIV_F;
                Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                Fee.UNIT_DEF_GROUP = 0;
                enw.OFF_ABS_GROUP.Add(Fee);
                enw.SaveChanges();
                //db.OFF_ABS_GROUP.Add(new OFF_ABS_GROUP()
                //{
                //    OFF_ABS_GROUP_ID =Convert.ToDecimal(max),
                //    FIRMS_CODE = off_abs_group.FIRMS_CODE,
                //    ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID,
                //    OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME,
                //    ACTIV_F = off_abs_group.ACTIV_F,
                //    PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE,
                //    UNIT_DEF_GROUP = off_abs_group.UNIT_DEF_GROUP
                //});

                //db.SaveChanges();
                status = true;
              
             
                message = "تم الاضافة وتعديل المجموعه لغير اساسية لوجود مجموعه اخري اساسية";
            }
                //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));

            
            // var max = db.FIRM_MISSIONS_DET.Any(o => o.PERSON_VACATIONS_SEQ == seq) ? db.PERSON_VACATIONS_DET.Where(o => o.PERSON_VACATIONS_SEQ == seq).Max(o => o.PERSON_VACATIONS_DET_ID) + 1 : 1;
        //    var max1 = Max_mission_det_Id();


         //  }

         //  else
          // {
         //      message = "Error! Please try again.";
         //  }

            return new JsonResult { Data = new { status = status, message = message } };
        }

                [HttpPost]
        public ActionResult Create_Steps(OFF_ABS_STEPS off_steps)
        {
            message = "";
            status = false;
            var max = Max_stepId();
            var maxorder = Max_ORDERId(off_steps.OFF_ABS_GROUP_ID);
       //    if (ModelState.IsValid)
            //{
            WF_EN en = new WF_EN();
            if (off_steps.OFF_ABS_STEPS_NAME != null && off_steps.OFF_ROLE_ARCHIVE != null)
            {

                if (!en.OFF_ABS_STEPS.Any(o => o.OFF_ABS_GROUP_ID == off_steps.OFF_ABS_GROUP_ID && o.OFF_ROLE_ARCHIVE == off_steps.OFF_ROLE_ARCHIVE))
                {
                    if (!en.OFF_ABS_STEPS.Any(o => o.OFF_ABS_GROUP_ID == off_steps.OFF_ABS_GROUP_ID && o.ORDER_ID == off_steps.ORDER_ID))
                    {
                        db.OFF_ABS_STEPS.Add(new OFF_ABS_STEPS()
                        {
                            OFF_ABS_STEPS_ID = Convert.ToDecimal(max),
                            JOB_TYPE_ID = Convert.ToInt16(off_steps.OFF_ROLE_ARCHIVE),
                            OFF_ABS_STEPS_NAME = off_steps.OFF_ABS_STEPS_NAME,
                            OFF_ABS_GROUP_ID = off_steps.OFF_ABS_GROUP_ID,
                            ORDER_ID = Convert.ToInt16(maxorder) //off_steps.ORDER_ID
                        });

                        db.SaveChanges();

                        status = true;
                        message = "Successfully Saved.";
                    }
                    else
                    {
                        message = "خطا ف الترتيب ";
                    }
                    //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));

                }
                //  }

                else
                {
                    message = "خطأ الخطوه موجودة من قبل";
                    //status = false;
                }
            }
            else
            {
                status = false;
                message = "من فضل استكمل بيانات الخطوه";

            }



            return new JsonResult { Data = new { status = status, message = message } };
        }



                [HttpPost]
        public ActionResult Create_OFF(OFF_ABS_GROP_OFF Create_OFF_group)

                {
                    message = "";
                    status = false;
                   // var max = Max_stepId();
                    var maxorder = Max_offId(Create_OFF_group.OFF_ABS_GROUP_ID);
                    //    if (ModelState.IsValid)|| o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID
                    //{
                    WF_EN en = new WF_EN();
                    if (!en.OFF_ABS_GROP_OFF.Any(o => o.ABS_CAT_ID == Create_OFF_group.ABS_CAT_ID && o.PERSON_DATA_ID == Create_OFF_group.PERSON_DATA_ID))
                    {
                   
                    if (!en.OFF_ABS_GROP_OFF.Any(o => o.OFF_ABS_GROUP_ID == Create_OFF_group.OFF_ABS_GROUP_ID && o.PERSON_DATA_ID == Create_OFF_group.PERSON_DATA_ID ))
                        {
                            db.OFF_ABS_GROP_OFF.Add(new OFF_ABS_GROP_OFF()
                            {
                                OFF_ABS_GROUP_OFF_ID = Convert.ToDecimal(maxorder),
                                OFF_ABS_GROUP_ID = Create_OFF_group.OFF_ABS_GROUP_ID,
                                OFF_SKELETON_OFFICERS_ID = Convert.ToInt64(Create_OFF_group.PERSON_DATA_ID),
                                ABS_CAT_ID = Create_OFF_group.ABS_CAT_ID,
                                PERSON_DATA_ID = Create_OFF_group.PERSON_DATA_ID
                            });

                            db.SaveChanges();

                            status = true;
                            message = "Successfully Saved.";
                        }
                        else
                        {
                            message = " الضابط موجود من قبل  ";
                        }
                        //decimal max = Convert.ToDecimal(db.AG_SECTORS.Max(o => o.SECTORE_CODE));

                    }
                    else
                    {
                        message = "  الضابط موجود من قبل ف هذه المجموعه   ";
                    }

                    return new JsonResult { Data = new { status = status, message = message } };
                }
        
        //public ActionResult Create(OFF_ABS_GROUP off_abs_group)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.OFF_ABS_GROUP.Add(off_abs_group);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", off_abs_group.ABSCENCE_CATEGORY_ID);
        //    ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", off_abs_group.FIRMS_CODE);
        //    ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", off_abs_group.PARANT_FIRMS_CODE);
        //    return View(off_abs_group);
        //}

        //
        // GET: /GROUPS/Edit/5
                public ActionResult Edit_Steps(decimal id ,decimal id1)
        {
           // OFF_ABS_STEPS off_abs_steps = db.OFF_ABS_STEPS.Find(id,id1);
            WF_EN en = new WF_EN();
            var off_abs_steps = en.OFF_ABS_STEPS.First(o => o.OFF_ABS_GROUP_ID == id1 && o.OFF_ABS_STEPS_ID == id);
            if (off_abs_steps == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = true;
            return PartialView("Create_Steps", off_abs_steps);
        }

        public ActionResult Edit(decimal id = 0)
        {
            OFF_ABS_GROUP off_abs_group = db.OFF_ABS_GROUP.Find(id);
            if (off_abs_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsUpdate = true;
            return PartialView("Create", off_abs_group);
        }

        //public ActionResult Edit(string id = null)
        //{
        //    OFF_ABS_GROUP off_abs_group = db.OFF_ABS_GROUP.Find(id);
        //    if (off_abs_group == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", off_abs_group.ABSCENCE_CATEGORY_ID);
        //    ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", off_abs_group.FIRMS_CODE);
        //    ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", off_abs_group.PARANT_FIRMS_CODE);
        //    return View(off_abs_group);
        //}

        //
        // POST: /GROUPS/Edit/5Edit_StepsOFF_ABS_STEPS


        [HttpPost]
        public ActionResult Edit(OFF_ABS_GROUP off_abs_group)
        {


            OFF_ABS_GROUP Fee = new OFF_ABS_GROUP();
            WF_EN enw = new WF_EN();
           // decimal grp_id = -1;
            //Fee.OFF_ABS_GROUP_ID = off_abs_group
           // FIRM_MISSIONS f = new FIRM_MISSIONS();

            if (!db.OFF_ABS_GROUP.Any(o => o.ABSCENCE_CATEGORY_ID == off_abs_group.ABSCENCE_CATEGORY_ID && o.UNIT_DEF_GROUP == 1))
            {
                if (off_abs_group.OFF_ABS_GROUP_ID != 0)
                {
                    Fee = enw.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == off_abs_group.OFF_ABS_GROUP_ID);

                    Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                    Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                    Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                    Fee.ACTIV_F = off_abs_group.ACTIV_F;
                    Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                    Fee.UNIT_DEF_GROUP = 1;
                    // enw.OFF_ABS_GROUP.Add(Fee);
                    enw.SaveChanges();
                }
                else
                {
               
                //Fee = enw.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == off_abs_group.OFF_ABS_GROUP_ID);

                //Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                //Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                //Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                //Fee.ACTIV_F = off_abs_group.ACTIV_F;
                //Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                //Fee.UNIT_DEF_GROUP =0;
                //// enw.OFF_ABS_GROUP.Add(Fee);
                //enw.SaveChanges();

                }
                status = true;
            }
            else
            {
                var steps = from o in db.OFF_ABS_GROUP
                            where o.ABSCENCE_CATEGORY_ID == off_abs_group.ABSCENCE_CATEGORY_ID && o.UNIT_DEF_GROUP == 1
                            select o;
                foreach (var s in steps)
                {
                    //grp_id = s.OFF_ABS_GROUP_ID;
                    Fee = enw.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == s.OFF_ABS_GROUP_ID);

                   // Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                   // Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                   // Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                   // Fee.ACTIV_F = off_abs_group.ACTIV_F;
                   // Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                    Fee.UNIT_DEF_GROUP = 0;
                    // enw.OFF_ABS_GROUP.Add(Fee);
                    enw.SaveChanges();

                }
                Fee = enw.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == off_abs_group.OFF_ABS_GROUP_ID);

                Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
                Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
                Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
                Fee.ACTIV_F = off_abs_group.ACTIV_F;
                Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
                Fee.UNIT_DEF_GROUP = 1;
                // enw.OFF_ABS_GROUP.Add(Fee);
                enw.SaveChanges();

            }

           // Fee = enw.OFF_ABS_GROUP.First(o => o.OFF_ABS_GROUP_ID == off_abs_group.OFF_ABS_GROUP_ID );

           // Fee.FIRMS_CODE = off_abs_group.FIRMS_CODE;
           // Fee.ABSCENCE_CATEGORY_ID = off_abs_group.ABSCENCE_CATEGORY_ID;
           // Fee.OFF_ABS_GROUP_NAME = off_abs_group.OFF_ABS_GROUP_NAME;
           // Fee.ACTIV_F = off_abs_group.ACTIV_F;
           // Fee.PARANT_FIRMS_CODE = off_abs_group.PARANT_FIRMS_CODE;
           // Fee.UNIT_DEF_GROUP = 0;
           //// enw.OFF_ABS_GROUP.Add(Fee);
           // enw.SaveChanges();


            status = true;

           // if (ModelState.IsValid)
           // {
           //     db.Entry(off_abs_group).State = EntityState.Modified;
           //     db.SaveChanges();
           ////    return RedirectToAction("Index");
           // }
         //   ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", off_abs_group.ABSCENCE_CATEGORY_ID);
           // ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "NAME", off_abs_group.FIRMS_CODE);
          //  ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "PARENT_FIRM_CODE", "NAME", off_abs_group.PARANT_FIRMS_CODE);
          //  return View(off_abs_group);
            return new JsonResult { Data = new { status = status, message = message } };
        }
        [HttpPost]
        public ActionResult Edit_Steps(OFF_ABS_STEPS off_abs_step)
        {
            status = true;
            if (ModelState.IsValid)
            {
                db.Entry(off_abs_step).State = EntityState.Modified;
                db.SaveChanges();
                //    return RedirectToAction("Index");
            }
            //   ViewBag.ABSCENCE_CATEGORY_ID = new SelectList(db.ABSCENCE_CATEGORIES, "ABSCENCE_CATEGORY_ID", "ABSCENCE_CATEGORY", off_abs_group.ABSCENCE_CATEGORY_ID);
            // ViewBag.FIRMS_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "NAME", off_abs_group.FIRMS_CODE);
            //  ViewBag.PARANT_FIRMS_CODE = new SelectList(db.FIRMS, "PARENT_FIRM_CODE", "NAME", off_abs_group.PARANT_FIRMS_CODE);
            //  return View(off_abs_group);
            return new JsonResult { Data = new { status = status, message = message } };
        }
        //
        // GET: /GROUPS/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            OFF_ABS_GROUP off_abs_group = db.OFF_ABS_GROUP.Find(id);
            if (off_abs_group == null)
            {
                return HttpNotFound();
            }
            return View(off_abs_group);
        }

        //
        // POST: /GROUPS/Delete/5


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            OFF_ABS_GROUP off_abs_group = db.OFF_ABS_GROUP.Find(id);
            db.OFF_ABS_GROUP.Remove(off_abs_group);
            db.SaveChanges();
            return new JsonResult { Data = new { status = status, message = message } };
        }


        [HttpPost, ActionName("Delete_Steps")]
        public ActionResult DeleteConfirmed(decimal id, decimal id1)
        {
            WF_EN en = new WF_EN();
            var off_abs_steps = en.OFF_ABS_STEPS.First(o => o.OFF_ABS_GROUP_ID == id1 && o.OFF_ABS_STEPS_ID == id);
            en.OFF_ABS_STEPS.Remove(off_abs_steps);
            en.SaveChanges();
            return new JsonResult { Data = new { status = status, message = message } };
        }

        [HttpPost, ActionName("Delete_OFF")]
        public ActionResult DeleteConfirmed1(decimal id, decimal id1)
        {
            WF_EN en = new WF_EN();
            var off_abs_off = en.OFF_ABS_GROP_OFF.First(o => o.OFF_ABS_GROUP_ID == id1 && o.OFF_ABS_GROUP_OFF_ID == id);
            en.OFF_ABS_GROP_OFF.Remove(off_abs_off);
            en.SaveChanges();
            return new JsonResult { Data = new { status = status, message = message } };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
          // [WebMethod]
          
    }
}