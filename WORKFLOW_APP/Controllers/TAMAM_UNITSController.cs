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
    public class TAMAM_UNITSController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /TAMAM_UNITS/

        public ActionResult Index()
        {
           // var firms_absences_persons = db.FIRMS_ABSENCES_PERSONS.Include(f => f.PERSON_DATA);
            return View("Index");
        }

        public JsonResult getall_tammamDataFN(string datee, string firms, string from, string to)
        {

            //  var off_abs_group = db.OFF_ABS_GROUP.Include(o => o.ABSCENCE_CATEGORIES).Include(o => o.FIRMS).Include(o => o.FIRMS1);
            string query =


                @"select * from(
select o.firm_code,o.name,o.firm_like_code,ROWNUM as RW,

                                        nvl((select count(i.person_code) 
                                  from person_data i
                                 where i.rank_cat_id =1 
                                                      and i.out_un_force <> 1
                                           and i.firm_code in( select a.firm_code
                                                         from firms a
                                                         where a.firm_like_code like concat(o.firm_like_code,'%')) ),0) TOTAL ,
                                                                                                               
                                                         
                                                         nvl((select count(*)
                               from firms_absences_persons i
                               where 
                                    -- i.absence_type_id  in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,18,19,20,21,22,23,24,25,45,46,47,48,30,31,32,33,34) and 
                                               i.rank_cat_id =1 and
                                                                           (  (TO_DATE ('" + datee + @"', 'dd/mm/yyyy hh24:mi')  >= from_date and to_date is null) or
                                                                                (
                (TO_DATE ('" + datee + @"' , 'dd/mm/yyyy hh24:mi') >= FROM_DATE )
                 AND 
                (TO_DATE ('" + datee + @"' , 'dd/mm/yyyy hh24:mi') <= TO_DATE )
               )
               )
                                     and i.firm_code in( select a.firm_code
                                                         from firms a
                                                         where a.firm_like_code like concat(o.firm_like_code , '%')) ),0) OUT,    
                                                         
                                                                                                             
                                                         
                                                          (          ( nvl((select count(i.person_code) 
                                  from person_data i
                                 where i.rank_cat_id =1 
                                                      and i.out_un_force <> 1
                                           and i.firm_code in( select a.firm_code
                                                         from firms a
                                                         where a.firm_like_code like concat(o.firm_like_code,'%')) ),0))-( nvl((select count(*)
                               from firms_absences_persons i
                               where 
                                 --    i.absence_type_id  in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,18,19,20,21,22,23,24,25,45,46,47,48,30,31,32,33,34) and 
                                               i.rank_cat_id =1 and
                                                                  (  (TO_DATE ('" + datee + @"', 'dd/mm/yyyy hh24:mi')  >= from_date and to_date is null) or
                                                                                (
                (TO_DATE ('" + datee + @"' , 'dd/mm/yyyy hh24:mi') >= FROM_DATE )
                 AND 
                (TO_DATE ('" + datee + @"' , 'dd/mm/yyyy hh24:mi') <= TO_DATE )
               )
               )
                                     and i.firm_code in( select a.firm_code
                                                         from firms a
                                                         where a.firm_like_code like concat(o.firm_like_code , '%')) ),0))) IIN
                                                         
                                                         

                                     

from     firms o 

/*where o.firm_like_code like (select ss.firm_like_code from firms ss where firm_code = :par_code )||'%'*/

 

/*where in_tree_flag =1*/
connect by prior o.firm_code= o.parent_firm_code
start with o.firm_code ='1400000000' /*and in_tree_flag =1   1010000000*/
        order by RW        )
        where RW between " + from + @" and " + to + @"       ";
//               @"select o.firm_code,o.name,o.firm_like_code,
//
//                                        nvl((select count(i.person_code) 
//                                  from person_data i
//                                 where i.rank_cat_id =1 
//                                                      and i.out_un_force <> 1
//                                           and i.firm_code in( select a.firm_code
//                                                         from firms a
//                                                         where a.firm_like_code like concat(o.firm_like_code,'%')) ),0) TOTAL ,
//                                                                                                               
//                                                         
//                                                         nvl((select count(*)
//                               from firms_absences_persons i
//                               where 
//                                    -- i.absence_type_id  in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,18,19,20,21,22,23,24,25,45,46,47,48,30,31,32,33,34) and 
//                                               i.rank_cat_id =1 and
//                                                 ( (TO_DATE('" + fromDateDay + "/" + fromDateMonth + "/" + fromDateYear + @"','dd/mm/yyyy') >= from_date and to_date is null) or
//                                                    (TO_DATE('" + fromDateDay + "/" + fromDateMonth + "/" + fromDateYear + @"','dd/mm/yyyy') between from_date and to_date))
//                                     and i.firm_code in( select a.firm_code
//                                                         from firms a
//                                                         where a.firm_like_code like concat(o.firm_like_code , '%')) ),0) OUT,    
//                                                         
//                                                                                                             
//                                                         
//                                                          (          ( nvl((select count(i.person_code) 
//                                  from person_data i
//                                 where i.rank_cat_id =1 
//                                                      and i.out_un_force <> 1
//                                           and i.firm_code in( select a.firm_code
//                                                         from firms a
//                                                         where a.firm_like_code like concat(o.firm_like_code,'%')) ),0))-( nvl((select count(*)
//                               from firms_absences_persons i
//                               where 
//                                 --    i.absence_type_id  in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,18,19,20,21,22,23,24,25,45,46,47,48,30,31,32,33,34) and 
//                                               i.rank_cat_id =1 and
//                                                 ( (TO_DATE('" + fromDateDay + "/" + fromDateMonth + "/" + fromDateYear + @"','dd/mm/yyyy') >= from_date and to_date is null) or
//                                                    (TO_DATE('" + fromDateDay + "/" + fromDateMonth + "/" + fromDateYear + @"','dd/mm/yyyy') between from_date and to_date))
//                                     and i.firm_code in( select a.firm_code
//                                                         from firms a
//                                                         where a.firm_like_code like concat(o.firm_like_code , '%')) ),0))) IIN
//                                                         
//                                                         
//
//                                     
//
//from     firms o 
//
//where o.firm_like_code like (select ss.firm_like_code from firms ss where firm_code = '"+firms+@"' )||'%'
//
// 
//
//
//connect by prior o.firm_code= o.parent_firm_code
//start with o.firm_code ='"+firms+@"' 
//        order by unit_order        ";

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



        public JsonResult getall__detail_tammamDataFN(string datee, string firms, int abs_cat)
        {

            //  var off_abs_group = db.OFF_ABS_GROUP.Include(o => o.ABSCENCE_CATEGORIES).Include(o => o.FIRMS).Include(o => o.FIRMS1);
            string query =
               @" SELECT COUNT (dt.PERSON_CODE) tammam,
         dt.name,
           dt.ABSENCE_TYPE_ID
    FROM firms,
         (SELECT                                           /*pd.person_name,*/
                PD.PERSON_CODE,
                 PD.FIRM_CODE,
                 DECODE (ab.NAME, NULL, 'موجود',ab.NAME,ab.NAME) NAME,
                 from_date,
                 TO_DATE,ABSENCE_TYPE_ID
            FROM (SELECT pd.person_name, PD.PERSON_CODE, PD.FIRM_CODE
                    FROM person_data pd
                   WHERE (pd.out_un_force <> 1) AND (pd.rank_cat_id = 1)) pd,
                       (SELECT firm_code,
                         person_code,
                         TP.NAME,
                         from_date,
                         TO_DATE  , tp.ABSENCE_TYPE_ID
                    FROM firms_absences_persons ab, ABSENCE_TYPES tp
                   WHERE tp.ABSENCE_TYPE_ID = ab.ABSENCE_TYPE_ID
 and tp.ABSCENCE_CATEGORY_ID=" + abs_cat + @"
 and ab.COMMANDER_FLAG=1
                                     AND                                                                (
                (TO_DATE ('" + datee+@"' , 'dd/mm/yyyy hh24:mi') >= ab.FROM_DATE )
                 AND 
                (TO_DATE ('"+datee+@"' , 'dd/mm/yyyy hh24:mi') <= ab.TO_DATE )
               )) ab
           WHERE pd.PERSON_CODE = ab.person_code(+)
                 AND pd.firm_code = AB.FIRM_CODE(+)) dt
   WHERE FIRMS.FIRM_CODE = dt.firm_code(+)
   and  dt.ABSENCE_TYPE_ID is not null
         AND firms.FIRM_CODE IN
                (SELECT a.firm_code
                   FROM firms a
              /*
WHERE a.parent_firm_code = '8886666666'

                        OR firm_code = '8886666666'*/
                        connect by prior a.firm_code= a.parent_firm_code
start with a.firm_code ='" + firms + @"')
GROUP BY   dt.ABSENCE_TYPE_ID,
         dt.name
";

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

        public JsonResult geta_detail_tammamDataFN_f(string datee, string firms)
        {

            //  var off_abs_group = db.OFF_ABS_GROUP.Include(o => o.ABSCENCE_CATEGORIES).Include(o => o.FIRMS).Include(o => o.FIRMS1);
            string query =
               @"   SELECT COUNT (dt.ABSCENCE_CATEGORY_ID) tammam,
        
         --dt.ABSENCE_TYPE_ID,
         dt.ABSCENCE_CATEGORY,
         dt.ABSCENCE_CATEGORY_ID
    FROM firms,
         (SELECT                                           /*pd.person_name,*/
                PD.PERSON_CODE,
                 PD.FIRM_CODE,
                 DECODE (ab.NAME,  NULL, 'موجود',  ab.NAME, ab.NAME) NAME,
                 from_date,
                 TO_DATE,
                 ABSENCE_TYPE_ID,
                 ABSCENCE_CATEGORY,
                 ABSCENCE_CATEGORY_ID
            FROM (SELECT pd.person_name, PD.PERSON_CODE, PD.FIRM_CODE
                    FROM person_data pd
                   WHERE (pd.out_un_force <> 1) AND (pd.rank_cat_id = 1)) pd,
                    (SELECT firm_code,
                         person_code,
                         TP.NAME,
                         from_date,
                         TO_DATE,
                         tp.ABSENCE_TYPE_ID,
                         ABS_CAT.ABSCENCE_CATEGORY,
                         ABS_CAT.ABSCENCE_CATEGORY_ID
                    FROM firms_absences_persons ab,
                         ABSENCE_TYPES tp,
                         ABSCENCE_CATEGORIES abs_cat
                   WHERE tp.ABSENCE_TYPE_ID = ab.ABSENCE_TYPE_ID
                         AND ABS_CAT.ABSCENCE_CATEGORY_ID =
                                TP.ABSCENCE_CATEGORY_ID       
          and ab.COMMANDER_FLAG=1
                         AND                                                                (
                (TO_DATE ('" + datee+@"' , 'dd/mm/yyyy hh24:mi') >= ab.FROM_DATE )
                 AND 
                (TO_DATE ('"+datee+@"' , 'dd/mm/yyyy hh24:mi') <= ab.TO_DATE )
               )) ab
           WHERE pd.PERSON_CODE = ab.person_code(+)
                 AND pd.firm_code = AB.FIRM_CODE(+)) dt
   WHERE FIRMS.FIRM_CODE = dt.firm_code(+)
   and  dt.ABSENCE_TYPE_ID is not null
         AND firms.FIRM_CODE IN
                (SELECT a.firm_code
                   FROM firms a
              /*
WHERE a.parent_firm_code = '8886666666'

                        OR firm_code = '8886666666'*/
                        connect by prior a.firm_code= a.parent_firm_code
start with a.firm_code ='" + firms + @"')
GROUP BY                           --dt.ABSENCE_TYPE_ID,
         dt.ABSCENCE_CATEGORY,
         dt.ABSCENCE_CATEGORY_ID

";

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

        public JsonResult bindUnit_detail_tammamFN(string ABSCECE_CAT_ID, string FIRM_CODE, string datee)
        {

            if (ABSCECE_CAT_ID == null)
            {
                ABSCECE_CAT_ID = "-1";
            }
            string query = @"select PERSON_DATA.PERSON_CODE,PERSON_DATA.PERSON_NAME,ABSENCE_TYPES.NAME as ABS_NAME, ABSENCE_TYPES.ABSENCE_TYPE_ID,FIRMS.FIRM_LIKE_CODE,FIRMS.NAME as FIRM_NAME,
RANKS.SHORT_NAME,PERSON_DATA.ID_NO,to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'dd/mm/yyyy')FROM_DATE,
to_char(FIRMS_ABSENCES_PERSONS.TO_DATE,'dd/mm/yyyy')TO_DATE
from person_data,firms_absences_persons,firms,absence_types,ranks
where 
PERSON_DATA.PERSON_CODE=firms_absences_persons.PERSON_CODE
and person_data.firm_code=firms.firm_code 
and ABSENCE_TYPES.ABSENCE_TYPE_ID=firms_absences_persons.ABSENCE_TYPE_ID
and RANKS.RANK_ID=PERSON_DATA.RANK_ID
and FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1
                                                        and          (  (TO_DATE ('" + datee+@"', 'dd/mm/yyyy hh24:mi')  >= from_date and to_date is null) or
                                                                                (
                (TO_DATE ('"+datee+@"' , 'dd/mm/yyyy hh24:mi') >= FROM_DATE )
                 AND 
                (TO_DATE ('"+datee+@"' , 'dd/mm/yyyy hh24:mi') <= TO_DATE )
               )
               )
        and  person_data.rank_cat_id in (1) 
                                                    and person_data.out_un_force <> 1
                                                        and (ABSENCE_TYPES.ABSENCE_TYPE_ID=" + ABSCECE_CAT_ID + @")
    AND firms.FIRM_CODE IN
                (SELECT a.firm_code
                   FROM firms a

                        connect by prior a.firm_code= a.parent_firm_code
start with a.firm_code ='" + FIRM_CODE + @"')

        order by FIRM_LIKE_CODE ,ID_NO";

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
        //
        // GET: /TAMAM_UNITS/Details/5

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
        // GET: /TAMAM_UNITS/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return View();
        }

        //
        // POST: /TAMAM_UNITS/Create

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
        // GET: /TAMAM_UNITS/Edit/5

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
        // POST: /TAMAM_UNITS/Edit/5

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
        // GET: /TAMAM_UNITS/Delete/5

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
        // POST: /TAMAM_UNITS/Delete/5

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