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
    public class OFFICERSController : Controller
    {
        private WF_EN db = new WF_EN();

        //
        // GET: /OFFICERS/

        public ActionResult Index()
        {
            var person_data = db.PERSON_DATA.Include(p => p.FIRM_DEPT_BRANCH_TYPES).Include(p => p.FIRM_CODE).Include(p => p.PERSON_SPECIALIZATIONS).Include(p => p.RANKS).Include(p => p.RELIGIONS).Include(p => p.VACATION_MODELS);
            return View("index");
            //   return View(person_data.ToList());
        }

        //
        // GET: /OFFICERS/Details/5

        // get data of officer grid
        public ActionResult bind_data()
        {
            WF_EN E = new WF_EN();
            string q = "";

            q = @"SELECT RELIGIONS.NAME RELIGION_NAME,BLOOD_TYPES.NAME BLOOD_TYPE_NAME,PERSON_SPECIALIZATIONS.NAME SPECIALIZATIONS_NAME,PERSON_SPECIALIZATIONS.SPECIALIZATION_ID ,JOBS_TYPES.JOB_NAME,RANKS.RANK,person_data.OUT_UN_FORCE,
       ARMY_DEPARTMENTS.NAME DEPARTMENT_NAME,FIRMS.NAME FIRM_NAME,
          to_char(  person_data.HIRE_DATE,'dd/mm/yyyy') HIRE_DATE,
             to_char(  person_data.CURRENT_RANK_DATE,'dd/mm/yyyy')CURRENT_RANK_DATE,   
        to_char(  person_data.NEXT_RANK_DATE,'dd/mm/yyyy')NEXT_RANK_DATE,   
          to_char(   person_data.LEAVE_DATE,'dd/mm/yyyy')LEAVE_DATE,   
         to_char(   person_data.JOIN_DATE,'dd/mm/yyyy')JOIN_DATE,   
         person_data.SEX,   
         nvl(person_data.MARRIGE_CONT,0) marrige_cont,   
         person_data.COMMUNITY_NO,   
         person_data.ARKAN_HARB,   
         nvl(person_data.DAUGHTERS_COUNT,0) daughters_count,   
         nvl(person_data.SONS_COUNT,0) sons_count,   
         person_data.BIRTH_PLACE,   
         person_data.SORT_NO,   
         person_data.ID_NO,   
         person_data.PHONE_2,   
         person_data.PHONE1,   
          to_char( person_data.BIRTHDATE,'dd/mm/yyyy')BIRTHDATE,   
         person_data.ADDERESS,   
         person_data.PERSON_NAME, 
         person_data.PERSONAL_ID_NO,
         person_data.BORROW_STATUS,
         person_data.BORROW_FIRM_CODE,   
         person_data.PERSON_CAT_ID,   
         person_data.RANK_CAT_ID,   
         person_data.SOCIAL_STATE_ID,   
         person_data.FIRM_CODE,   
         person_data.PARENT_STATUS_ID,   
         person_data.ID_TYPE_ID,   
         person_data.AGE_CATEGORY_ID,   
         person_data.BLOOD_TYPE_ID,   
         person_data.RELIGION_ID,   
         person_data.SECTOR_ID,   
         person_data.GOVERNERATE_ID,   
         person_data.JOB_TYPE_ID,   
         person_data.DEPARTMENT_ID,   
         person_data.RANK_ID,   
         person_data.PERSON_CODE, 
            person_data.GRADUATION_NAME, 
            person_data.CATEGORY_ID ,
            person_data.SHOE_SIZE,
            person_data.SUIT_SIZE,
            person_data.OVERALL_SIZE,
            person_data.MASK_SIZE,
          to_char(  person_data.RANK_RENEW_DATE,'dd/mm/yyyy')RANK_RENEW_DATE,
            person_data.TRANSFER_NO,
            person_data.FIELD_SERVICE_ABILITY,
            person_data.FEED_REPLACE,
           to_char(person_data.NOZOM_DATE,'dd/mm/yyyy')NOZOM_DATE,
         (select count(person_accendents.person_code) from person_accendents where  person_accendents.person_code (+)= person_data.person_code) P_ACCEDENT,
            (select count(persons_ranking.person_code) from persons_ranking where  persons_ranking.person_code (+)= person_data.person_code) P_RANKING,      
            (select count(courses.person_code) from courses where  courses.person_code (+)= person_data.person_code) P_COURSES,
         (select count(persons_travels.person_code) from persons_travels where persons_travels.person_code (+)= person_data.person_code) P_TRAVEL,   
         (select count(person_languages.person_code) from person_languages where person_languages.person_code (+)= person_data.person_code) P_LANGUAGE,   
         (select count(person_punishments.person_code) from person_punishments where person_punishments.person_code (+)= person_data.person_code) P_PUNISHMENTS,   
         (select count(person_batteles.person_code) from person_batteles where person_batteles.person_code (+)= person_data.person_code) P_BATTELES,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 1) P_JOBS,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 2) T_JOBS,   
         (select count(person_hospitals.person_code) from person_hospitals where person_hospitals.person_code (+)= person_data.person_code) P_HOSPITALS,   
         (select count(person_investgations.person_code) from person_investgations where person_investgations.person_code (+)= person_data.person_code) P_INVESTGATIONS,   
         (select count(person_medals.person_code) from person_medals where person_medals.person_code (+)= person_data.person_code) PERSON_MEDALS,   
         (select count(person_studies.person_code) from person_studies where person_studies.person_code (+)= person_data.person_code) PERSON_STUDIES,   
         (select count(persons_ranking.person_code) from persons_ranking where persons_ranking.person_code (+)= person_data.person_code) PERSONS_RANKING  ,
         (SELECT count(person_complains.person_code) FROM person_complains WHERE person_complains.person_code (+) = person_data.person_code ) PERSON_COMPLAINS,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 1 ) AND ( person_secret_reports.person_code (+) = person_data.person_code  ) ) PERSON_EXCELANT,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 2 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_V_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 3 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 4 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_ACC,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 5 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_WEAK,
         nvl(off_l_w.off_length,0) lgh,
         nvl(off_l_w.off_weight ,0)  wh
          
            FROM person_data  ,RANKS,ARMY_DEPARTMENTS,FIRMS,JOBS_TYPES,PERSON_SPECIALIZATIONS,BLOOD_TYPES,RELIGIONS,


                 (SELECT person_code , nvl(x.length,0) off_length,nvl(x.weight,0) off_weight
                   from person_measures  x
                   where x.measurement_date=(    select max(y.measurement_date)
                                                     from person_measures  y
                                                     where y.person_code=x.person_code)) off_l_w

                where (person_data.person_code=off_l_w.person_code(+) ) and
                        (person_data.rank_cat_id = 1) and
                        (nvl(person_data.out_un_force,0) = 0)
                            AND PERSON_DATA.BORROW_STATUS = 0
                            AND PERSON_DATA.RANK_ID=RANKS.RANK_ID
                            AND ARMY_DEPARTMENTS.DEPARTMENT_ID=PERSON_DATA.DEPARTMENT_ID 
                            AND FIRMS.FIRM_CODE=PERSON_DATA.FIRM_CODE
                            AND JOBS_TYPES.JOB_TYPE_ID(+)=PERSON_DATA.JOB_TYPE_ID
                            AND PERSON_SPECIALIZATIONS.SPECIALIZATION_ID(+)=PERSON_DATA.SPECIALIZATION_ID
                            AND BLOOD_TYPES.BLOOD_TYPE_ID(+)=PERSON_DATA.BLOOD_TYPE_ID
                            AND RELIGIONS.RELIGION_ID(+)=PERSON_DATA.RELIGION_ID
                order by person_data.rank_id , person_data.sort_no";



            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // get data of officer grid BorrowIn
        public ActionResult bind_data_BorrowIn()
        {
            string query = @"SELECT RELIGIONS.NAME RELIGION_NAME,BLOOD_TYPES.NAME BLOOD_TYPE_NAME,PERSON_SPECIALIZATIONS.NAME SPECIALIZATIONS_NAME,PERSON_SPECIALIZATIONS.SPECIALIZATION_ID ,JOBS_TYPES.JOB_NAME,RANKS.RANK,person_data.OUT_UN_FORCE,
       ARMY_DEPARTMENTS.NAME DEPARTMENT_NAME,FIRMS.NAME FIRM_NAME,
          to_char(  person_data.HIRE_DATE,'dd/mm/yyyy') HIRE_DATE,
             to_char(  person_data.CURRENT_RANK_DATE,'dd/mm/yyyy')CURRENT_RANK_DATE,   
        to_char(  person_data.NEXT_RANK_DATE,'dd/mm/yyyy')NEXT_RANK_DATE,   
          to_char(   person_data.LEAVE_DATE,'dd/mm/yyyy')LEAVE_DATE,   
         to_char(   person_data.JOIN_DATE,'dd/mm/yyyy')JOIN_DATE,   
         person_data.SEX,   
         nvl(person_data.MARRIGE_CONT,0) marrige_cont,   
         person_data.COMMUNITY_NO,   
         person_data.ARKAN_HARB,   
         nvl(person_data.DAUGHTERS_COUNT,0) daughters_count,   
         nvl(person_data.SONS_COUNT,0) sons_count,   
         person_data.BIRTH_PLACE,   
         person_data.SORT_NO,   
         person_data.ID_NO,   
         person_data.PHONE_2,   
         person_data.PHONE1,   
          to_char( person_data.BIRTHDATE,'dd/mm/yyyy')BIRTHDATE,   
         person_data.ADDERESS,   
         person_data.PERSON_NAME,
         person_data.PERSONAL_ID_NO,
         person_data.BORROW_STATUS,
         person_data.BORROW_FIRM_CODE,    
         person_data.PERSON_CAT_ID,   
         person_data.RANK_CAT_ID,   
         person_data.SOCIAL_STATE_ID,   
         person_data.FIRM_CODE,   
         person_data.PARENT_STATUS_ID,   
         person_data.ID_TYPE_ID,   
         person_data.AGE_CATEGORY_ID,   
         person_data.BLOOD_TYPE_ID,   
         person_data.RELIGION_ID,   
         person_data.SECTOR_ID,   
         person_data.GOVERNERATE_ID,   
         person_data.JOB_TYPE_ID,   
         person_data.DEPARTMENT_ID,   
         person_data.RANK_ID,   
         person_data.PERSON_CODE, 
            person_data.GRADUATION_NAME, 
            person_data.CATEGORY_ID ,
            person_data.SHOE_SIZE,
            person_data.SUIT_SIZE,
            person_data.OVERALL_SIZE,
            person_data.MASK_SIZE,
          to_char(  person_data.RANK_RENEW_DATE,'dd/mm/yyyy')RANK_RENEW_DATE,
            person_data.TRANSFER_NO,
            person_data.FIELD_SERVICE_ABILITY,
            person_data.FEED_REPLACE,
           to_char(person_data.NOZOM_DATE,'dd/mm/yyyy')NOZOM_DATE,
         (select count(person_accendents.person_code) from person_accendents where  person_accendents.person_code (+)= person_data.person_code) P_ACCEDENT,
            (select count(persons_ranking.person_code) from persons_ranking where  persons_ranking.person_code (+)= person_data.person_code) P_RANKING,      
            (select count(courses.person_code) from courses where  courses.person_code (+)= person_data.person_code) P_COURSES,
         (select count(persons_travels.person_code) from persons_travels where persons_travels.person_code (+)= person_data.person_code) P_TRAVEL,   
         (select count(person_languages.person_code) from person_languages where person_languages.person_code (+)= person_data.person_code) P_LANGUAGE,   
         (select count(person_punishments.person_code) from person_punishments where person_punishments.person_code (+)= person_data.person_code) P_PUNISHMENTS,   
         (select count(person_batteles.person_code) from person_batteles where person_batteles.person_code (+)= person_data.person_code) P_BATTELES,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 1) P_JOBS,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 2) T_JOBS,   
         (select count(person_hospitals.person_code) from person_hospitals where person_hospitals.person_code (+)= person_data.person_code) P_HOSPITALS,   
         (select count(person_investgations.person_code) from person_investgations where person_investgations.person_code (+)= person_data.person_code) P_INVESTGATIONS,   
         (select count(person_medals.person_code) from person_medals where person_medals.person_code (+)= person_data.person_code) PERSON_MEDALS,   
         (select count(person_studies.person_code) from person_studies where person_studies.person_code (+)= person_data.person_code) PERSON_STUDIES,   
         (select count(persons_ranking.person_code) from persons_ranking where persons_ranking.person_code (+)= person_data.person_code) PERSONS_RANKING  ,
         (SELECT count(person_complains.person_code) FROM person_complains WHERE person_complains.person_code (+) = person_data.person_code ) PERSON_COMPLAINS,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 1 ) AND ( person_secret_reports.person_code (+) = person_data.person_code  ) ) PERSON_EXCELANT,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 2 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_V_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 3 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 4 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_ACC,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 5 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_WEAK,
         nvl(off_l_w.off_length,0) lgh,
         nvl(off_l_w.off_weight ,0)  wh
          
            FROM person_data  ,RANKS,ARMY_DEPARTMENTS,FIRMS,JOBS_TYPES,PERSON_SPECIALIZATIONS,BLOOD_TYPES,RELIGIONS,


                 (SELECT person_code , nvl(x.length,0) off_length,nvl(x.weight,0) off_weight
                   from person_measures  x
                   where x.measurement_date=(    select max(y.measurement_date)
                                                     from person_measures  y
                                                     where y.person_code=x.person_code)) off_l_w

                where (person_data.person_code=off_l_w.person_code(+) ) and
                        (person_data.rank_cat_id = 1) AND
                         (nvl(person_data.out_un_force,0) = 0)
                            AND person_data.BORROW_STATUS = 1
                            AND PERSON_DATA.RANK_ID=RANKS.RANK_ID
                            AND ARMY_DEPARTMENTS.DEPARTMENT_ID=PERSON_DATA.DEPARTMENT_ID 
                            AND FIRMS.FIRM_CODE=PERSON_DATA.FIRM_CODE
                            AND JOBS_TYPES.JOB_TYPE_ID(+)=PERSON_DATA.JOB_TYPE_ID
                            AND PERSON_SPECIALIZATIONS.SPECIALIZATION_ID(+)=PERSON_DATA.SPECIALIZATION_ID
                            AND BLOOD_TYPES.BLOOD_TYPE_ID(+)=PERSON_DATA.BLOOD_TYPE_ID
                            AND RELIGIONS.RELIGION_ID(+)=PERSON_DATA.RELIGION_ID                    
                order by person_data.rank_id , person_data.sort_no";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        // get data of officer grid BorrowOut
        public ActionResult bind_data_BorrowOut()
        {
            string query = @"SELECT RELIGIONS.NAME RELIGION_NAME,BLOOD_TYPES.NAME BLOOD_TYPE_NAME,PERSON_SPECIALIZATIONS.NAME SPECIALIZATIONS_NAME,PERSON_SPECIALIZATIONS.SPECIALIZATION_ID ,JOBS_TYPES.JOB_NAME,RANKS.RANK,person_data.OUT_UN_FORCE,
       ARMY_DEPARTMENTS.NAME DEPARTMENT_NAME,FIRMS.NAME FIRM_NAME,
          to_char(  person_data.HIRE_DATE,'dd/mm/yyyy') HIRE_DATE,
             to_char(  person_data.CURRENT_RANK_DATE,'dd/mm/yyyy')CURRENT_RANK_DATE,   
        to_char(  person_data.NEXT_RANK_DATE,'dd/mm/yyyy')NEXT_RANK_DATE,   
          to_char(   person_data.LEAVE_DATE,'dd/mm/yyyy')LEAVE_DATE,   
         to_char(   person_data.JOIN_DATE,'dd/mm/yyyy')JOIN_DATE,   
         person_data.SEX,   
         nvl(person_data.MARRIGE_CONT,0) marrige_cont,   
         person_data.COMMUNITY_NO,   
         person_data.ARKAN_HARB,   
         nvl(person_data.DAUGHTERS_COUNT,0) daughters_count,   
         nvl(person_data.SONS_COUNT,0) sons_count,   
         person_data.BIRTH_PLACE,   
         person_data.SORT_NO,   
         person_data.ID_NO,   
         person_data.PHONE_2,   
         person_data.PHONE1,   
          to_char( person_data.BIRTHDATE,'dd/mm/yyyy')BIRTHDATE,   
         person_data.ADDERESS,   
         person_data.PERSON_NAME,
         person_data.PERSONAL_ID_NO,
         person_data.BORROW_STATUS,
         person_data.BORROW_FIRM_CODE,    
         person_data.PERSON_CAT_ID,   
         person_data.RANK_CAT_ID,   
         person_data.SOCIAL_STATE_ID,   
         person_data.FIRM_CODE,   
         person_data.PARENT_STATUS_ID,   
         person_data.ID_TYPE_ID,   
         person_data.AGE_CATEGORY_ID,   
         person_data.BLOOD_TYPE_ID,   
         person_data.RELIGION_ID,   
         person_data.SECTOR_ID,   
         person_data.GOVERNERATE_ID,   
         person_data.JOB_TYPE_ID,   
         person_data.DEPARTMENT_ID,   
         person_data.RANK_ID,   
         person_data.PERSON_CODE, 
            person_data.GRADUATION_NAME, 
            person_data.CATEGORY_ID ,
            person_data.SHOE_SIZE,
            person_data.SUIT_SIZE,
            person_data.OVERALL_SIZE,
            person_data.MASK_SIZE,
          to_char(  person_data.RANK_RENEW_DATE,'dd/mm/yyyy')RANK_RENEW_DATE,
            person_data.TRANSFER_NO,
            person_data.FIELD_SERVICE_ABILITY,
            person_data.FEED_REPLACE,
           to_char(person_data.NOZOM_DATE,'dd/mm/yyyy')NOZOM_DATE,
         (select count(person_accendents.person_code) from person_accendents where  person_accendents.person_code (+)= person_data.person_code) P_ACCEDENT,
            (select count(persons_ranking.person_code) from persons_ranking where  persons_ranking.person_code (+)= person_data.person_code) P_RANKING,      
            (select count(courses.person_code) from courses where  courses.person_code (+)= person_data.person_code) P_COURSES,
         (select count(persons_travels.person_code) from persons_travels where persons_travels.person_code (+)= person_data.person_code) P_TRAVEL,   
         (select count(person_languages.person_code) from person_languages where person_languages.person_code (+)= person_data.person_code) P_LANGUAGE,   
         (select count(person_punishments.person_code) from person_punishments where person_punishments.person_code (+)= person_data.person_code) P_PUNISHMENTS,   
         (select count(person_batteles.person_code) from person_batteles where person_batteles.person_code (+)= person_data.person_code) P_BATTELES,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 1) P_JOBS,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 2) T_JOBS,   
         (select count(person_hospitals.person_code) from person_hospitals where person_hospitals.person_code (+)= person_data.person_code) P_HOSPITALS,   
         (select count(person_investgations.person_code) from person_investgations where person_investgations.person_code (+)= person_data.person_code) P_INVESTGATIONS,   
         (select count(person_medals.person_code) from person_medals where person_medals.person_code (+)= person_data.person_code) PERSON_MEDALS,   
         (select count(person_studies.person_code) from person_studies where person_studies.person_code (+)= person_data.person_code) PERSON_STUDIES,   
         (select count(persons_ranking.person_code) from persons_ranking where persons_ranking.person_code (+)= person_data.person_code) PERSONS_RANKING  ,
         (SELECT count(person_complains.person_code) FROM person_complains WHERE person_complains.person_code (+) = person_data.person_code ) PERSON_COMPLAINS,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 1 ) AND ( person_secret_reports.person_code (+) = person_data.person_code  ) ) PERSON_EXCELANT,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 2 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_V_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 3 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 4 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_ACC,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 5 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_WEAK,
         nvl(off_l_w.off_length,0) lgh,
         nvl(off_l_w.off_weight ,0)  wh
          
            FROM person_data  ,RANKS,ARMY_DEPARTMENTS,FIRMS,JOBS_TYPES,PERSON_SPECIALIZATIONS,BLOOD_TYPES,RELIGIONS,


                 (SELECT person_code , nvl(x.length,0) off_length,nvl(x.weight,0) off_weight
                   from person_measures  x
                   where x.measurement_date=(    select max(y.measurement_date)
                                                     from person_measures  y
                                                     where y.person_code=x.person_code)) off_l_w

                where (person_data.person_code=off_l_w.person_code(+) ) and
                        (person_data.rank_cat_id = 1) AND
                         (nvl(person_data.out_un_force,0) = 0)
                            AND person_data.BORROW_STATUS = 2
                            AND PERSON_DATA.RANK_ID=RANKS.RANK_ID
                            AND ARMY_DEPARTMENTS.DEPARTMENT_ID=PERSON_DATA.DEPARTMENT_ID 
                            AND FIRMS.FIRM_CODE=PERSON_DATA.FIRM_CODE
                            AND JOBS_TYPES.JOB_TYPE_ID(+)=PERSON_DATA.JOB_TYPE_ID
                            AND PERSON_SPECIALIZATIONS.SPECIALIZATION_ID(+)=PERSON_DATA.SPECIALIZATION_ID
                            AND BLOOD_TYPES.BLOOD_TYPE_ID(+)=PERSON_DATA.BLOOD_TYPE_ID
                            AND RELIGIONS.RELIGION_ID(+)=PERSON_DATA.RELIGION_ID                     
                order by person_data.rank_id , person_data.sort_no";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // get data of officer grid out of unit
        public ActionResult bind_data_outforce()
        {
            string query = @"SELECT RELIGIONS.NAME RELIGION_NAME,BLOOD_TYPES.NAME BLOOD_TYPE_NAME,PERSON_SPECIALIZATIONS.NAME SPECIALIZATIONS_NAME,PERSON_SPECIALIZATIONS.SPECIALIZATION_ID ,JOBS_TYPES.JOB_NAME,RANKS.RANK,person_data.OUT_UN_FORCE,
       ARMY_DEPARTMENTS.NAME DEPARTMENT_NAME,FIRMS.NAME FIRM_NAME,
          to_char(  person_data.HIRE_DATE,'dd/mm/yyyy') HIRE_DATE,
             to_char(  person_data.CURRENT_RANK_DATE,'dd/mm/yyyy')CURRENT_RANK_DATE,   
        to_char(  person_data.NEXT_RANK_DATE,'dd/mm/yyyy')NEXT_RANK_DATE,   
          to_char(   person_data.LEAVE_DATE,'dd/mm/yyyy')LEAVE_DATE,   
         to_char(   person_data.JOIN_DATE,'dd/mm/yyyy')JOIN_DATE,   
         person_data.SEX,   
         nvl(person_data.MARRIGE_CONT,0) marrige_cont,   
         person_data.COMMUNITY_NO,   
         person_data.ARKAN_HARB,   
         nvl(person_data.DAUGHTERS_COUNT,0) daughters_count,   
         nvl(person_data.SONS_COUNT,0) sons_count,   
         person_data.BIRTH_PLACE,   
         person_data.SORT_NO,   
         person_data.ID_NO,   
         person_data.PHONE_2,   
         person_data.PHONE1,   
          to_char( person_data.BIRTHDATE,'dd/mm/yyyy')BIRTHDATE,   
         person_data.ADDERESS,   
         person_data.PERSON_NAME,
         person_data.PERSONAL_ID_NO,
         person_data.BORROW_STATUS,
         person_data.BORROW_FIRM_CODE,    
         person_data.PERSON_CAT_ID,   
         person_data.RANK_CAT_ID,   
         person_data.SOCIAL_STATE_ID,   
         person_data.FIRM_CODE,   
         person_data.PARENT_STATUS_ID,   
         person_data.ID_TYPE_ID,   
         person_data.AGE_CATEGORY_ID,   
         person_data.BLOOD_TYPE_ID,   
         person_data.RELIGION_ID,   
         person_data.SECTOR_ID,   
         person_data.GOVERNERATE_ID,   
         person_data.JOB_TYPE_ID,   
         person_data.DEPARTMENT_ID,   
         person_data.RANK_ID,   
         person_data.PERSON_CODE, 
            person_data.GRADUATION_NAME, 
            person_data.CATEGORY_ID ,
            person_data.SHOE_SIZE,
            person_data.SUIT_SIZE,
            person_data.OVERALL_SIZE,
            person_data.MASK_SIZE,
          to_char(  person_data.RANK_RENEW_DATE,'dd/mm/yyyy')RANK_RENEW_DATE,
            person_data.TRANSFER_NO,
            person_data.FIELD_SERVICE_ABILITY,
            person_data.FEED_REPLACE,
           to_char(person_data.NOZOM_DATE,'dd/mm/yyyy')NOZOM_DATE,
         (select count(person_accendents.person_code) from person_accendents where  person_accendents.person_code (+)= person_data.person_code) P_ACCEDENT,
            (select count(persons_ranking.person_code) from persons_ranking where  persons_ranking.person_code (+)= person_data.person_code) P_RANKING,      
            (select count(courses.person_code) from courses where  courses.person_code (+)= person_data.person_code) P_COURSES,
         (select count(persons_travels.person_code) from persons_travels where persons_travels.person_code (+)= person_data.person_code) P_TRAVEL,   
         (select count(person_languages.person_code) from person_languages where person_languages.person_code (+)= person_data.person_code) P_LANGUAGE,   
         (select count(person_punishments.person_code) from person_punishments where person_punishments.person_code (+)= person_data.person_code) P_PUNISHMENTS,   
         (select count(person_batteles.person_code) from person_batteles where person_batteles.person_code (+)= person_data.person_code) P_BATTELES,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 1) P_JOBS,   
         (select count(person_jobs.person_code) from person_jobs where person_jobs.person_code (+)= person_data.person_code and person_jobs.job_flag = 2) T_JOBS,   
         (select count(person_hospitals.person_code) from person_hospitals where person_hospitals.person_code (+)= person_data.person_code) P_HOSPITALS,   
         (select count(person_investgations.person_code) from person_investgations where person_investgations.person_code (+)= person_data.person_code) P_INVESTGATIONS,   
         (select count(person_medals.person_code) from person_medals where person_medals.person_code (+)= person_data.person_code) PERSON_MEDALS,   
         (select count(person_studies.person_code) from person_studies where person_studies.person_code (+)= person_data.person_code) PERSON_STUDIES,   
         (select count(persons_ranking.person_code) from persons_ranking where persons_ranking.person_code (+)= person_data.person_code) PERSONS_RANKING  ,
         (SELECT count(person_complains.person_code) FROM person_complains WHERE person_complains.person_code (+) = person_data.person_code ) PERSON_COMPLAINS,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 1 ) AND ( person_secret_reports.person_code (+) = person_data.person_code  ) ) PERSON_EXCELANT,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 2 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_V_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 3 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_G,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 4 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_ACC,
         (SELECT count(person_secret_reports.person_code) FROM person_secret_reports WHERE ( person_secret_reports.approving_officer_grade_id = 5 ) AND ( person_secret_reports.person_code (+)= person_data.person_code  ) ) PERSON_WEAK,
         nvl(off_l_w.off_length,0) lgh,
         nvl(off_l_w.off_weight ,0)  wh
          
            FROM person_data  ,RANKS,ARMY_DEPARTMENTS,FIRMS,JOBS_TYPES,PERSON_SPECIALIZATIONS,BLOOD_TYPES,RELIGIONS,


                 (SELECT person_code , nvl(x.length,0) off_length,nvl(x.weight,0) off_weight
                   from person_measures  x
                   where x.measurement_date=(    select max(y.measurement_date)
                                                     from person_measures  y
                                                     where y.person_code=x.person_code)) off_l_w

                where (person_data.person_code=off_l_w.person_code(+) ) and
                        (person_data.rank_cat_id = 1) AND
                         (nvl(person_data.out_un_force,0) = 1)
                            AND person_data.BORROW_STATUS = 2 
                            AND PERSON_DATA.RANK_ID=RANKS.RANK_ID
                            AND ARMY_DEPARTMENTS.DEPARTMENT_ID=PERSON_DATA.DEPARTMENT_ID 
                            AND FIRMS.FIRM_CODE=PERSON_DATA.FIRM_CODE
                            AND JOBS_TYPES.JOB_TYPE_ID(+)=PERSON_DATA.JOB_TYPE_ID
                            AND PERSON_SPECIALIZATIONS.SPECIALIZATION_ID(+)=PERSON_DATA.SPECIALIZATION_ID
                            AND BLOOD_TYPES.BLOOD_TYPE_ID(+)=PERSON_DATA.BLOOD_TYPE_ID
                            AND RELIGIONS.RELIGION_ID(+)=PERSON_DATA.RELIGION_ID 
                                            
                order by person_data.rank_id , person_data.sort_no";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        

        //insert data 
        public int insertData(string officer_number, string senior_number, string national_id , int rank_code, string officer_name, string patch_name,
              int army_departments, string united_code, int category_code, string government_name, int job_code, int religion_code,
              int blood_code, string phone_number, string borrow_code, int borrow_status, int out_unForce, string dateOfBirth, string graduation_date, string combine_date, string current_promotion_date, string renewal_date,
               string coming_promotion_date, string address)
        {
            int messagae;
            int RankCatId = 1;
            int maxint2 = autoINcrement_Dep("PERSON_CODE", "PERSON_DATA");
            string query2 = @"INSERT INTO FIRM_WORK.PERSON_DATA (RANK_CAT_ID , PERSON_CODE , ID_NO , SORT_NO ,PERSONAL_ID_NO , RANK_ID , PERSON_NAME , GRADUATION_NAME , DEPARTMENT_ID , FIRM_CODE , SPECIALIZATION_ID
           , BIRTH_PLACE , JOB_TYPE_ID , RELIGION_ID , BLOOD_TYPE_ID , PHONE1 , BORROW_FIRM_CODE , BORROW_STATUS , OUT_UN_FORCE , BIRTHDATE , HIRE_DATE , JOIN_DATE , CURRENT_RANK_DATE , LEAVE_DATE , NEXT_RANK_DATE , ADDERESS) VALUES
           ( '" + RankCatId + "' , '" + maxint2 + "' , '" + officer_number + "', '" + senior_number + "' ,'" + national_id + "' ,'" + rank_code + "' , '" + officer_name + "' , '" + patch_name + @"'
               , '" + army_departments + "' , '" + united_code + "' , '" + category_code + "' , '" + government_name + "' , '" + job_code + "' , '" + religion_code + @"'
               , '" + blood_code + "' , '" + phone_number + "' , '" + borrow_code + "' , '" + borrow_status + "' , '" + out_unForce + "' , to_date('" + dateOfBirth + "' , 'dd/mm/yyyy') , to_date('" + graduation_date + "' , 'dd/mm/yyyy') , to_date('" + combine_date + @"' , 'dd/mm/yyyy') , 
               to_date('" + current_promotion_date + "' , 'dd/mm/yyyy'), to_date('" + renewal_date + "' , 'dd/mm/yyyy') ,to_date('" + coming_promotion_date + "' , 'dd/mm/yyyy') , '" + address + "')";

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

        //*******************************************************************************************************
        //Method used to get Max ID
        public int autoINcrement_Dep(string colName, string tableName)
        {

            string maxQUERY = " select nvl(max(" + colName + "),0)+1 from " + tableName + " ";
            OracleConnection conn = new OracleConnection("Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True");
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand(maxQUERY, conn);
            ds = GetData(cmd);
            string maxstr = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            int num = Convert.ToInt32(maxstr);
            return num;

        }

        //*******************************************************************************************************
        //update data 
        public int updateData(int PersonCode, string officer_number, string senior_number, string national_id, int rank_code, string officer_name, string patch_name,
              int army_departments, int category_code, string government_name, int job_code, int religion_code,
              int blood_code, string phone_number, string borrow_code, int borrow_status, int out_unForce , string dateOfBirth, string graduation_date, string combine_date, string current_promotion_date, string renewal_date,
               string coming_promotion_date, string address)
        {
            int messagae;
            string query2 = @"UPDATE FIRM_WORK.PERSON_DATA SET  ID_NO = '" + officer_number + @"' , SORT_NO = '" + senior_number + @"' , PERSONAL_ID_NO = '" + national_id + @"' ,  RANK_ID = '" + rank_code + @"' ,
            PERSON_NAME = '" + officer_name + @"' , GRADUATION_NAME = '" + patch_name + @"' , DEPARTMENT_ID = '" + army_departments + @"' ,  SPECIALIZATION_ID = '" + category_code + @"' ,
        BIRTH_PLACE = '" + government_name + @"' , JOB_TYPE_ID = '" + job_code + @"' , RELIGION_ID = '" + religion_code + @"' , BLOOD_TYPE_ID = '" + blood_code + @"' ,
        PHONE1 = '" + phone_number + @"' , BORROW_FIRM_CODE = '" + borrow_code + @"' , BORROW_STATUS = '" + borrow_status + @"' , OUT_UN_FORCE = '" + out_unForce + @"' , BIRTHDATE = to_date('" + dateOfBirth + @"' , 'dd/mm/yyyy') , HIRE_DATE = to_date('" + graduation_date + @"' , 'dd/mm/yyyy') ,
        JOIN_DATE =  to_date('" + combine_date + @"' , 'dd/mm/yyyy') , CURRENT_RANK_DATE = to_date('" + current_promotion_date + @"' , 'dd/mm/yyyy') , 
        LEAVE_DATE =  to_date('" + renewal_date + @"' , 'dd/mm/yyyy') , NEXT_RANK_DATE = to_date('" + coming_promotion_date + @"' , 'dd/mm/yyyy') ,
        ADDERESS = '" + address + @"'   WHERE  PERSON_CODE = '" + PersonCode + "' ";

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
        //************************************************************************************
        // DELETE OFFICERS
        public int delete_Off(int PersonCode)
        {
            int message;
            string query = "delete from FIRM_WORK.PERSON_DATA where PERSON_CODE = '" + PersonCode + "' ";
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

        //************************************************************************************

        // fill ranks
        public JsonResult fillDropDown_ranks()
        {
            string query = "SELECT RANK , RANK_ID FROM FIRM_WORK.RANKS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // fill army_departments
        public JsonResult fillDropDown_army_departments()
        {
            string query = "SELECT NAME, BRANCH_NAME, DEPARTMENT_ID FROM FIRM_WORK.ARMY_DEPARTMENTS";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // fill fropdown category
        public JsonResult fillDropDown_category()
        {
            string query = @"SELECT   s.SPECIALIZATION_ID , s.NAME 
                           FROM FIRM_WORK.PERSON_SPECIALIZATIONS s ,  FIRM_WORK.PERSON_SPEC_CATEGORIES c 
                           where  C.CATEGORY_ID = S.CATEGORY_ID and   C.RANK_CAT_ID = 1";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // fill fropdown jobs
        public JsonResult fillDropDown_jobs()
        {
            string query = " SELECT JOB_NAME, JOB_TYPE_ID FROM FIRM_WORK.JOBS_TYPES where JOBS_TYPES.RANK_CAT_ID = 1";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //fill fropdown blood
        public JsonResult fillDropDown_blood()
        {
            string query = "SELECT BLOOD_TYPE_ID , NAME FROM FIRM_WORK.BLOOD_TYPES";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //fill fropdown borrow_units
        public JsonResult fillDropDown_OutForce()
        {
            string query = "SELECT FIRM_CODE , NAME  FROM FIRM_WORK.FIRMS";

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



        public ActionResult GET_fin_year()
        {
            //  var off_abs_steps = db.OFF_ABS_STEPS.Include(o => o.OFF_ABS_GROUP);
            //return View(off_abs_steps.ToList());
            var q = @"SELECT TRAINING_PERIODS.TRAINING_PERIOD,TRAINING_PERIODS.FIN_YEAR,TRAINING_PERIODS.TRAINING_PERIOD_ID FROM TRAINING_PERIODS WHERE   TO_DATE(SYSDATE) BETWEEN TRAINING_PERIODS.PERIOD_FROM AND TRAINING_PERIODS.PERIOD_TO";
            OracleCommand cmd = new OracleCommand(q);
            var data = General.GetData_New(cmd);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id = null)
        {
            PERSON_DATA person_data = db.PERSON_DATA.Find(id);
            if (person_data == null)
            {
                return HttpNotFound();
            }
            return View(person_data);
        }


        //************************************************************************************************
        //
        // GET: /OFFICERS/Create

        public ActionResult Create()
        {
            ViewBag.BRANCH_CODE = new SelectList(db.FIRM_DEPT_BRANCH_TYPES, "BRANCH_CODE", "BRANCH_NAME");
            ViewBag.BORROW_FIRM_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE");
            ViewBag.SPECIALIZATION_ID = new SelectList(db.PERSON_SPECIALIZATIONS, "SPECIALIZATION_ID", "NAME");
            ViewBag.RANK_ID = new SelectList(db.RANKS, "RANK_ID", "RANK");
            //ViewBag.RELIGION_ID = new SelectList(db.RELIGIONS, "RELIGION_ID", "NAME");
            ViewBag.VACATION_MODEL_ID = new SelectList(db.VACATION_MODELS, "VACATION_MODEL_ID", "VACATION_MODEL");
            return View();
        }

        //
        // POST: /OFFICERS/Create

        [HttpPost]
        public ActionResult Create(PERSON_DATA person_data)
        {
            if (ModelState.IsValid)
            {
                db.PERSON_DATA.Add(person_data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BRANCH_CODE = new SelectList(db.FIRM_DEPT_BRANCH_TYPES, "BRANCH_CODE", "BRANCH_NAME", person_data.BRANCH_CODE);
            ViewBag.BORROW_FIRM_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", person_data.BORROW_FIRM_CODE);
            ViewBag.SPECIALIZATION_ID = new SelectList(db.PERSON_SPECIALIZATIONS, "SPECIALIZATION_ID", "NAME", person_data.SPECIALIZATION_ID);
            ViewBag.RANK_ID = new SelectList(db.RANKS, "RANK_ID", "RANK", person_data.RANK_ID);
            ViewBag.RELIGION_ID = new SelectList(db.RELIGIONS, "RELIGION_ID", "NAME", person_data.RELIGION_ID);
            ViewBag.VACATION_MODEL_ID = new SelectList(db.VACATION_MODELS, "VACATION_MODEL_ID", "VACATION_MODEL", person_data.VACATION_MODEL_ID);
            return View(person_data);
        }

        //
        // GET: /OFFICERS/Edit/5

        public ActionResult Edit(string id = null)
        {
            PERSON_DATA person_data = db.PERSON_DATA.Find(id);
            if (person_data == null)
            {
                return HttpNotFound();
            }
            ViewBag.BRANCH_CODE = new SelectList(db.FIRM_DEPT_BRANCH_TYPES, "BRANCH_CODE", "BRANCH_NAME", person_data.BRANCH_CODE);
            ViewBag.BORROW_FIRM_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", person_data.BORROW_FIRM_CODE);
            ViewBag.SPECIALIZATION_ID = new SelectList(db.PERSON_SPECIALIZATIONS, "SPECIALIZATION_ID", "NAME", person_data.SPECIALIZATION_ID);
            ViewBag.RANK_ID = new SelectList(db.RANKS, "RANK_ID", "RANK", person_data.RANK_ID);
            ViewBag.RELIGION_ID = new SelectList(db.RELIGIONS, "RELIGION_ID", "NAME", person_data.RELIGION_ID);
            ViewBag.VACATION_MODEL_ID = new SelectList(db.VACATION_MODELS, "VACATION_MODEL_ID", "VACATION_MODEL", person_data.VACATION_MODEL_ID);
            return View(person_data);
        }

        //
        // POST: /OFFICERS/Edit/5

        [HttpPost]
        public ActionResult Edit(PERSON_DATA person_data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person_data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BRANCH_CODE = new SelectList(db.FIRM_DEPT_BRANCH_TYPES, "BRANCH_CODE", "BRANCH_NAME", person_data.BRANCH_CODE);
            ViewBag.BORROW_FIRM_CODE = new SelectList(db.FIRMS, "FIRM_CODE", "PARENT_FIRM_CODE", person_data.BORROW_FIRM_CODE);
            ViewBag.SPECIALIZATION_ID = new SelectList(db.PERSON_SPECIALIZATIONS, "SPECIALIZATION_ID", "NAME", person_data.SPECIALIZATION_ID);
            ViewBag.RANK_ID = new SelectList(db.RANKS, "RANK_ID", "RANK", person_data.RANK_ID);
            ViewBag.RELIGION_ID = new SelectList(db.RELIGIONS, "RELIGION_ID", "NAME", person_data.RELIGION_ID);
            ViewBag.VACATION_MODEL_ID = new SelectList(db.VACATION_MODELS, "VACATION_MODEL_ID", "VACATION_MODEL", person_data.VACATION_MODEL_ID);
            return View(person_data);
        }

        //
        // GET: /OFFICERS/Delete/5

        public ActionResult Delete(string id = null)
        {
            PERSON_DATA person_data = db.PERSON_DATA.Find(id);
            if (person_data == null)
            {
                return HttpNotFound();
            }
            return View(person_data);
        }

        //
        // POST: /OFFICERS/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            PERSON_DATA person_data = db.PERSON_DATA.Find(id);
            db.PERSON_DATA.Remove(person_data);
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