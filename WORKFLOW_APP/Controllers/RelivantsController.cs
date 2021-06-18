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
    public class RelivantsController : Controller
    {
        private WF_EN db = new WF_EN();
   
        // get data of Relivants
        public JsonResult bind_data_Relivants(string personcode)
        {

            string query = @"SELECT PR.SEQ,
       PR.PERSON_CODE,
       ROWNUM,
       PR.RELATIONSHIP_ID,
       R.NAME,
       PR.RELIVANT_NAME,
       PR.RELIVANT_JOB,
       TO_CHAR (PR.BIRTH_DATE, 'dd/mm/yyyy') AS BIRTH_DATE,
       PR.RELIVANT_ADDRESS,
       PR.COUPLE_NAME,
       PR.COUPLE_JOB,
       TO_CHAR (PR.COUPLE_BIRTHDATE, 'dd/mm/yyyy') AS COUPLE_BIRTHDATE,
       PR.EDUCATION_LEVEL_ID,
       PR.EDUCATION_SPEC_ID,
       PR.EDUCATION_CERTIFICATE_ID,
       PR.COUPLE_EDUCATION_LEVEL_ID,
       PR.COUPLE_EDUCATION_SPEC_ID,
       (SELECT EDUCATION_LEVEL_NAME
          FROM FIRM_WORK.EDUCATION_LEVELS
         WHERE EDUCATION_LEVELS.EDUCATION_LEVEL_ID = PR.EDUCATION_LEVEL_ID)
          EDUCATION_LEVEL_NAME,
       (SELECT EDUCATION_CERTIFICATE_NAME
          FROM FIRM_WORK.EDUCATION_CERTIFICATES
         WHERE EDUCATION_CERTIFICATES.EDUCATION_LEVEL_ID =
                  PR.EDUCATION_LEVEL_ID
               AND EDUCATION_CERTIFICATES.EDUCATION_CERTIFICATE_ID =
                      PR.EDUCATION_CERTIFICATE_ID)
          EDUCATION_CERTIFICATE_NAME,
       (SELECT EDUCATION_SPEC_NAME
          FROM FIRM_WORK.EDUCATION_SPECIFICATIONS
         WHERE EDUCATION_SPECIFICATIONS.EDUCATION_LEVEL_ID =
                  PR.EDUCATION_LEVEL_ID
               AND EDUCATION_SPECIFICATIONS.EDUCATION_CERTIFICATE_ID =
                      PR.EDUCATION_CERTIFICATE_ID
               AND EDUCATION_SPECIFICATIONS.EDUCATION_SPEC_ID =
                      PR.EDUCATION_SPEC_ID)
          EDUCATION_SPEC_NAME,
           (SELECT EDUCATION_LEVEL_NAME
          FROM FIRM_WORK.EDUCATION_LEVELS
         WHERE EDUCATION_LEVELS.EDUCATION_LEVEL_ID = PR.COUPLE_EDUCATION_LEVEL_ID)
         COUPLE_EDUCATION_LEVEL_NAME,
          (SELECT EDUCATION_CERTIFICATE_NAME
          FROM FIRM_WORK.EDUCATION_CERTIFICATES
         WHERE EDUCATION_CERTIFICATES.EDUCATION_LEVEL_ID =
                PR.COUPLE_EDUCATION_LEVEL_ID
               AND EDUCATION_CERTIFICATES.EDUCATION_CERTIFICATE_ID =
                      PR.COUPLE_EDUCATION_CERT_ID)
          COUPLE_EDUCATION_CERT_NAME,
            (SELECT EDUCATION_SPEC_NAME
          FROM FIRM_WORK.EDUCATION_SPECIFICATIONS
         WHERE EDUCATION_SPECIFICATIONS.EDUCATION_LEVEL_ID =
                  PR.COUPLE_EDUCATION_LEVEL_ID
               AND EDUCATION_SPECIFICATIONS.EDUCATION_CERTIFICATE_ID =
                      PR.COUPLE_EDUCATION_CERT_ID
               AND EDUCATION_SPECIFICATIONS.EDUCATION_SPEC_ID =
                      PR.COUPLE_EDUCATION_SPEC_ID)
          COUPLE_EDUCATION_SPEC_NAME,
       PR.COUPLE_EDUCATION_CERT_ID,
       PR.PERSON_INFERIOR,
      decode(PR.NATIONALITY , 1  , 'مصرى' , 2 , 'مصرية' ) NATIONALITY,
      decode(PR.COUPLE_NATIONALITY , 1  , 'مصرى' , 2 , 'مصرية' ) COUPLE_NATIONALITY,
       PR.CHILD_COUNT
  FROM FIRM_WORK.PERSON_RELIVANTS PR, FIRM_WORK.RELATIONSHIPS R
 WHERE PERSON_CODE = '" + personcode + "' AND PR.RELATIONSHIP_ID = R.RELATIONSHIP_ID ";
            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //get officer name
        public JsonResult getOffName(string personcode)
        {
            string query = @"select ( R.RANK || ' / ' || P.PERSON_NAME ) PERSON_NAME 
            from PERSON_DATA P  , RANKS R where PERSON_CODE = '" + personcode + "' and P.RANK_ID = R.RANK_ID";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            string offName =  data[0]["PERSON_NAME"].ToString();
            return Json(offName, JsonRequestBehavior.AllowGet);
        }


        //insert data 
        public int insertData(string personcode ,string relationShipId, string relivantName, string job_Name, string dateOfBirth, string addressId, string coupleRelivantName,
              string coupleJobName, string coupleDateOfBirth, string educationLevelId, string specificatioId, string certificatesId, string coupleEducationLevelId,
              string coupleSpecificationsId, string coupleCertificatesId, string nationaltyId, string coupleNationaltyId, string numberOfSons)
        {
            int messagae;
            int maxint2 = autoINcrement_Seq(personcode);
            string query2 = @"INSERT INTO FIRM_WORK.PERSON_RELIVANTS (
   SEQ, PERSON_CODE, RELATIONSHIP_ID, 
   RELIVANT_NAME, RELIVANT_JOB, BIRTH_DATE, 
   RELIVANT_ADDRESS, COUPLE_NAME, COUPLE_JOB, 
   COUPLE_BIRTHDATE, EDUCATION_LEVEL_ID, EDUCATION_SPEC_ID, 
   EDUCATION_CERTIFICATE_ID, COUPLE_EDUCATION_LEVEL_ID, COUPLE_EDUCATION_SPEC_ID, 
   COUPLE_EDUCATION_CERT_ID, NATIONALITY, 
   COUPLE_NATIONALITY, CHILD_COUNT) 
VALUES ('" + maxint2 + "' , '" + personcode + "'  , '" + relationShipId + "' , '" + relivantName + "' , '" + job_Name + @"' ,
            to_date('" + dateOfBirth + "' , 'dd/mm/yyyy') , '" + addressId + "' , '" + coupleRelivantName + "' , '" + coupleJobName + "' , to_date('" + coupleDateOfBirth + @"' , 'dd/mm/yyyy') ,
       '" + educationLevelId + "' , '" + specificatioId + "' , '" + certificatesId + "' , '" + coupleEducationLevelId + "' , '" + coupleSpecificationsId + @"' ,
     '" + coupleCertificatesId + "' , '" + nationaltyId + "' , '" + coupleNationaltyId + "' , '" + numberOfSons + "')";

            if (excutecommand(query2) == 1)
            {
                messagae = 1;
            }
            else
            {
                messagae = 0;
            }
            return messagae;
        }

        //update data 
        public int updateData(string PersonCode, string SeqNumber, string relationShipId, string relivantName, string job_Name, string nationaltyId, string dateOfBirth,
              string addressId, string educationLevelId, string certificatesId, string specificatioId, string coupleRelivantName,
              string coupleJobName, string coupleNationaltyId, string coupleDateOfBirth , string numberOfSons ,
            string coupleEducationLevelId , string coupleCertificatesId , string coupleSpecificationsId )
        {
            int messagae;
            string query2 = @"UPDATE FIRM_WORK.PERSON_RELIVANTS
                            SET
                                   RELATIONSHIP_ID           = '" + relationShipId + @"',
                                   RELIVANT_NAME             = '" + relivantName + @"',
                                   RELIVANT_JOB              = '" + job_Name + @"',
                                   BIRTH_DATE                = to_date( '" + dateOfBirth + @"' , 'dd/mm/yyyy'),
                                   RELIVANT_ADDRESS          = '" + addressId + @"',
                                   COUPLE_NAME               = '" + coupleRelivantName + @"',
                                   COUPLE_JOB                = '" + coupleJobName + @"',
                                   COUPLE_BIRTHDATE          = to_date('" + coupleDateOfBirth + @"' , 'dd/mm/yyyy'),
                                   EDUCATION_LEVEL_ID        = '" + educationLevelId + @"',
                                   EDUCATION_SPEC_ID         = '" + specificatioId + @"',
                                   EDUCATION_CERTIFICATE_ID  = '" + certificatesId + @"',
                                   COUPLE_EDUCATION_LEVEL_ID = '" + coupleEducationLevelId + @"',
                                   COUPLE_EDUCATION_SPEC_ID  = '" + coupleSpecificationsId + @"',
                                   COUPLE_EDUCATION_CERT_ID  = '" + coupleCertificatesId + @"',
                                   NATIONALITY               = '" + nationaltyId + @"',
                                   COUPLE_NATIONALITY        = '" + coupleNationaltyId + @"',
                                   CHILD_COUNT               = '" + numberOfSons + @"'
                            WHERE  SEQ                       = '" + SeqNumber + @"'
                            AND    PERSON_CODE               = '" + PersonCode + @"' ";

            if (excutecommand(query2) == 1)
            {
                messagae = 1;
            }
            else
            {
                messagae = 0;
            }
            return messagae;
        }

        // DELETE Relivants
        public int delete_Relivants(string seqNum , string PersonCode)
        {
            int message;
            string query = "delete from FIRM_WORK.PERSON_RELIVANTS where PERSON_CODE = '" + PersonCode + "' and SEQ = '" + seqNum + "' ";
            if (excutecommand(query) == 1)
            {
                message = 1;
            }
            else
            {
                message = 0;
            }
            return message;
        }

        //Method used to get Max ID
        public int autoINcrement_Seq(string personCode)
        {

            string maxQUERY = " select nvl(max(seq) , 0)+1 from person_relivants where person_code = '" + personCode + "'";
            OracleConnection conn = new OracleConnection("Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True");
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand(maxQUERY, conn);
            ds = GetData(cmd);
            string maxstr = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            int num = Convert.ToInt32(maxstr);
            return num;

        }

        // fill Relationships
        public JsonResult fillDropDown_Relationships()
        {
            string query = "SELECT RELATIONSHIP_ID, NAME FROM FIRM_WORK.RELATIONSHIPS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // fill EducationLevel
        public JsonResult fillDropDown_EducationLevel()
        {
            string query = "SELECT EDUCATION_LEVEL_ID, EDUCATION_LEVEL_NAME FROM FIRM_WORK.EDUCATION_LEVELS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // fill Certificates
        public JsonResult fillDropDown_Certificates(string parm)
        {
            string query = @"SELECT EDUCATION_CERTIFICATE_ID,  EDUCATION_CERTIFICATE_NAME
            FROM FIRM_WORK.EDUCATION_CERTIFICATES where EDUCATION_CERTIFICATES.EDUCATION_LEVEL_ID = '" + parm + "' ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
         // fill Specifications
        public JsonResult fillDropDown_Specifications(string parm , string parm1)
        {
            string query = @"SELECT  EDUCATION_SPEC_ID, EDUCATION_SPEC_NAME
                FROM FIRM_WORK.EDUCATION_SPECIFICATIONS where
                EDUCATION_SPECIFICATIONS.EDUCATION_LEVEL_ID = '"+parm+"' and EDUCATION_SPECIFICATIONS.EDUCATION_CERTIFICATE_ID = '"+parm1+"' ";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        //************************************************************************************
        private DataSet GetData(OracleCommand cmd)
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
        //connect db and insert rows
        public int excutecommand(String query)
        {
            try
            {
                OracleConnection conn = new OracleConnection("Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True");
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
                conn.Close();
                if (r == 0)
                    return 0;
                else
                    return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        //*************************************************************************************


        //
        // GET: /Relivants/

        public ActionResult Index()
        {
            // var person_relivants = db.PERSON_RELIVANTS.Include(p => p.PERSON_DATA);
            return View("index");

           // return View(person_relivants.ToList());
        }

        //
        // GET: /Relivants/Details/5

        public ActionResult Details(short id = 0)
        {
            PERSON_RELIVANTS person_relivants = db.PERSON_RELIVANTS.Find(id);
            if (person_relivants == null)
            {
                return HttpNotFound();
            }
            return View(person_relivants);
        }

        //
        // GET: /Relivants/Create

        public ActionResult Create()
        {
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE");
            return View();
        }

        //
        // POST: /Relivants/Create

        [HttpPost]
        public ActionResult Create(PERSON_RELIVANTS person_relivants)
        {
            if (ModelState.IsValid)
            {
                db.PERSON_RELIVANTS.Add(person_relivants);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", person_relivants.PERSON_CODE);
            return View(person_relivants);
        }

        //
        // GET: /Relivants/Edit/5

        public ActionResult Edit(short id = 0)
        {
            PERSON_RELIVANTS person_relivants = db.PERSON_RELIVANTS.Find(id);
            if (person_relivants == null)
            {
                return HttpNotFound();
            }
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", person_relivants.PERSON_CODE);
            return View(person_relivants);
        }

        //
        // POST: /Relivants/Edit/5

        [HttpPost]
        public ActionResult Edit(PERSON_RELIVANTS person_relivants)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person_relivants).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PERSON_CODE = new SelectList(db.PERSON_DATA, "PERSON_CODE", "FIRM_CODE", person_relivants.PERSON_CODE);
            return View(person_relivants);
        }

        //
        // GET: /Relivants/Delete/5

        public ActionResult Delete(short id = 0)
        {
            PERSON_RELIVANTS person_relivants = db.PERSON_RELIVANTS.Find(id);
            if (person_relivants == null)
            {
                return HttpNotFound();
            }
            return View(person_relivants);
        }

        //
        // POST: /Relivants/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            PERSON_RELIVANTS person_relivants = db.PERSON_RELIVANTS.Find(id);
            db.PERSON_RELIVANTS.Remove(person_relivants);
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