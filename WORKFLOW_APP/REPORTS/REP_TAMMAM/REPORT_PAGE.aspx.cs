using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using WORKFLOW_APP.REPORTS;
using System.Globalization;
namespace WORKFLOW_APP.REPORTS.REP_TAMMAM
{
    public partial class REPORT_PAGE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // TextBox2.Text = DateTime.Now.ToString("mm:HH yyyy/MM/dd");
                TextBox2.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                BUILD_REP();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BUILD_REP();
        }
        public static DataSet GetData(OracleCommand cmd)
        {

            // OracleConnection con1 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");
            string strConnString = "Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True";
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
                        con.Close();

                        return ds;
                    }
                }

            }
        }

        public void BUILD_REP()
        {
            
            string FIRM_CODE = Request.QueryString["FIRM_CODE"].ToString();
            // var dd = Convert.ToDateTime(TextBox2.Text.Substring(6) + " " + String.Join(":", TextBox2.Text.Substring(0, 6).Split(':').Reverse())).ToString("yyyy/MM/dd HH:mm");
            var dd = Convert.ToDateTime(TextBox2.Text).ToString("yyyy/MM/dd HH:mm");
            var dd1 = Convert.ToDateTime(dd).ToString("yyyy/MM/dd");
            //TextBox2.Text = dd;
            string date = "to_date('" + TextBox2.Text + "','yyyy/MM/dd HH24:mi')";
            string date1 = "to_date('" + dd1 + "','yyyy/mm/dd')";

            DataSet1 ds = new DataSet1();
            OracleConnection con = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con.Open();
            #region OLD_SQL
            string xx = @"SELECT   person_data.person_name,
                                      person_data.person_code,
                                      person_data.rank_id,
                                      RANKS.RANK,
                                      person_data.id_no,
                                      person_data.current_rank_date,
                                      person_data.sort_no,
                                      person_data.category_id,
                                      (SELECT   firms_absences_persons.absence_type_id
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                          
                                                                                                             and           (
                                        ( " + date + @" >= firms_absences_persons.FROM_DATE )
                                         AND 
                                        ( " + date + @" <= firms_absences_persons.ACT_DATE )
                                       )
                                                     AND (firms_absences_persons.absence_type_id NOT IN
                                                                (16, 17, 31, 32, 54, 55, 50))
                           
                                                     AND (firms_absences_persons.firm_code =" + FIRM_CODE + @")))
                                         AS absence_type_id,
                                                       (SELECT   ABSENCE_TYPES.NAME
                                         FROM   firms_absences_persons,ABSENCE_TYPES
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                    and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                          
                                                                                                             and           (
                                        ( " + date + @" >= firms_absences_persons.FROM_DATE )
                                         AND 
                                        ( " + date + @" <= firms_absences_persons.ACT_DATE )
                                       )
                                                     AND (firms_absences_persons.absence_type_id NOT IN
                                                                (16, 17, 31, 32, 54, 55, 50))
                           
                                                     AND (firms_absences_persons.firm_code =" + FIRM_CODE + @")))
                                         AS absence_type_name,
                                      (SELECT   COUNT (firms_absences_persons.person_code)
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.rank_cat_id = 1)
                                                AND (TO_DATE (firms_absences_persons.from_date) <=
                                                              " + date + @")
                                               AND (firms_absences_persons.absence_type_id NOT IN
                                                           (16, 17, 31, 32, 54, 55, 50))
                                                AND (TO_DATE (firms_absences_persons.ACT_DATE) >=
                                                               " + date + @")
                                                AND (firms_absences_persons.firm_code = " + FIRM_CODE + @"))
                                         AS absence_count,
                                      (SELECT   firms_absences_persons.absence_notes
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                                                                             and           (
                                        (" + date + @" >= firms_absences_persons.FROM_DATE )
                                         AND 
                                        (" + date + @" <= firms_absences_persons.ACT_DATE )
                                       )
                                                    AND (firms_absences_persons.absence_type_id NOT IN
                                                               (16, 17, 31, 32, 54, 55, 50))
                                                     AND (firms_absences_persons.firm_code=" + FIRM_CODE + @")))
                                         AS notes,
                                      1 AS one,
                                      1 AS two,
                                      1 AS three,
                                      1 AS four,
                                      1 AS five,
                                      1 AS six,
                                      1 AS seven,
                                      1 AS eight,
                                      1 AS nine,
                                      1 AS ten,
                                      1 AS eleven,
                                      1 AS tewelve,
                                      1 AS thirten,
                                      1 AS fourteen,
                                      (SELECT   COUNT (person_data.person_code)
                                         FROM   person_data
                                        WHERE       (person_data.firm_code = " + FIRM_CODE + @")
                                                AND (person_data.rank_cat_id = 1)
                                                AND (person_data.borrow_status IN (0, 2))
                                                AND (person_data.OUT_UN_FORCE = 0))
                                         AS total_count,
                                      to_char((SELECT   TO_DATE (firms_absences_persons.from_date)
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                                                     AND (firms_absences_persons.absence_type_id NOT IN
                                                               (16, 17, 31, 32, 54, 55, 50, 50))
                                                                                  and           (
                                        (" + date + @" >= firms_absences_persons.FROM_DATE )
                                         AND 
                                        (" + date + @" <= firms_absences_persons.ACT_DATE )
                                       )
                                                     AND (firms_absences_persons.firm_code = " + FIRM_CODE + @"))),'dd/mm')
                                         AS FF,
                                      to_char((SELECT   TO_DATE (firms_absences_persons.ACT_DATE)
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                                                    AND (firms_absences_persons.absence_type_id NOT IN
                                                                (16, 17, 31, 32, 54, 55, 50))
                                                              and           (
                                        (" + date + @" >= firms_absences_persons.FROM_DATE )
                                         AND 
                                        (" + date + @" <= firms_absences_persons.ACT_DATE )
                                       )
                                                     AND (firms_absences_persons.firm_code = " + FIRM_CODE + @"))),'dd/mm')
                                         AS DD,
                                      to_char((SELECT   person_daily_entry.entry_date
                                         FROM   person_daily_entry
                                        WHERE   person_daily_entry.person_code =
                                                   person_data.person_code
                                                AND person_daily_entry.from_date =          " + date1 + @"),'hh24:mi')
                                         entry_hour,
                                      (SELECT   firms_absences_persons.absence_type_id
                                         FROM   firms_absences_persons
                                        WHERE   (firms_absences_persons.person_code =
                                                    person_data.person_code)
                                                AND ( (firms_absences_persons.rank_cat_id = 1)
                                                     AND (firms_absences_persons.absence_type_id IN
                                                               (16, 17, 31, 32, 54, 55))
                                                     AND (TO_DATE (firms_absences_persons.from_date) =
                                                                   " + date + @")
                                                     AND (firms_absences_persons.firm_code = " + FIRM_CODE + @")))
                                         AS absence_type_id_y,
                                    nobtgia.N_NAME
                               FROM   person_data, RANKS,
                                    (SELECT   FIRMS_ABSENCES_PERSONS.PERSON_CODE,
                                                FIRMS_ABSENCES_PERSONS.FIRM_CODE,
                                                PERSON_DATA.PERSON_NAME,
                                                ranks.RANK,
                                                ABSENCE_TYPES.NAME as N_NAME
                                         FROM   FIRMS_ABSENCES_PERSONS,
                                           PERSON_DATA,
                                           ABSENCE_TYPES,
                                           ranks
                                     WHERE (PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE)
                                           AND (FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID =
                                                   ABSENCE_TYPES.ABSENCE_TYPE_ID)
                                           AND ( (FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FIRM_CODE + @")
                                                AND (ABSENCE_TYPES.ABSCENCE_CATEGORY_ID = 1)
                                                AND ( (" + date + @" >= firms_absences_persons.FROM_DATE)
                                                     AND (" + date + @" <= firms_absences_persons.ACT_DATE))
                                                AND (firms_absences_persons.rank_cat_id = 1))
                                           AND ranks.rank_id = PERSON_DATA.rank_id) nobtgia
                              WHERE       (person_data.firm_code = " + FIRM_CODE + @")
                                      AND  person_data.firm_code = nobtgia.firm_code(+)
                                      AND  person_data.person_code = nobtgia.person_code(+)
                                      AND PERSON_DATA.RANK_ID = RANKS.RANK_ID
                                      AND (person_data.rank_cat_id = 1)
                                      AND (NVL (person_data.OUT_UN_FORCE, 0) = 0)
                                      AND (person_data.borrow_status IN (0, 2))
                                      AND (person_data.person_code NOT IN
                                                 (SELECT   aa.person_code
                                                    FROM   person_data aa
                                                   WHERE       (aa.firm_code =" + FIRM_CODE + @")
                                                           AND (aa.job_type_id IN (1, 2))
                                                           AND (aa.out_un_force = 0)))
                                      AND (NVL (person_data.join_date," + date + @") <= " + date + @")
                           ORDER BY   PERSON_DATA.RANK_ID,PERSON_DATA.CATEGORY_ID,PERSON_DATA.CURRENT_RANK_DATE,PERSON_DATA.SORT_NO

                        ";
            #endregion

            #region tamam_SH


            var AA = @"SELECT person_data.person_name,
                             person_data.person_code,
                             person_data.rank_id,
                             RANKS.RANK,
                             person_data.id_no,
                             person_data.current_rank_date,
                             person_data.sort_no,
                             person_data.category_id,
                             A1.AB AS absence_type_id,
                             A1.TYP_NM AS absence_type_name,
                             A1.NOTE AS notes,
                             TO_CHAR (A1.FD, 'mm/dd') AS FF,
                             TO_CHAR (A1.TD, 'mm/dd') AS DD,
                            -- TO_CHAR (TIM.TIM, 'hh24:mi') entry_hour, 
                             (CASE
                                         WHEN  (" + date + @") >
                                                 TIM.TIM
                                         THEN
                                             TO_CHAR (TIM.TIM, 'hh24:mi')
                                         ELSE
                                            TO_CHAR ('')
                                      END)
                                       entry_hour,
                             A1.AB AS absence_type_id_y,
                             AB1.TYP_NM AS N_NAME,
                             NVL(A1.CNT,1) AS CNT
                        FROM person_data,
                             RANKS,
                             (SELECT AB.ABSENCE_TYPE_ID AS AB,
                                        AB.PERSON_CODE,
                                        TYP.NAME AS TYP_NM,
                                        AB.ABSENCE_NOTES AS NOTE,
                                        AB.FROM_DATE AS FD,
                                        AB.ACT_DATE AS TD,
                                        AB.FIRM_CODE,
                                        DECODE(TYP.ABSCENCE_CATEGORY_ID,1,0,1) AS CNT
                                  FROM firms_absences_persons AB, ABSENCE_TYPES TYP
                                 WHERE (rank_cat_id = 1) AND AB.ABSENCE_TYPE_ID = TYP.ABSENCE_TYPE_ID
                                       AND ( (" + date + @" >= FROM_DATE)
                                            AND (" + date + @" <= ACT_DATE))
                                       AND (AB.ABSENCE_TYPE_ID NOT IN (16, 17, 31, 32, 54, 55, 50))
                                       AND (firm_code = " + FIRM_CODE + @")) A1,

                                (SELECT   FIRMS_ABSENCES_PERSONS.PERSON_CODE,           
                                        ABSENCE_TYPES.NAME TYP_NM
                                    FROM   FIRMS_ABSENCES_PERSONS,           
                                        ABSENCE_TYPES
                                WHERE    (FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID = ABSENCE_TYPES.ABSENCE_TYPE_ID)
                                        AND ( (FIRMS_ABSENCES_PERSONS.FIRM_CODE =" + FIRM_CODE + @")
                                                AND (ABSENCE_TYPES.ABSCENCE_CATEGORY_ID = 1)
                                                   AND ( (" + date + @" >= FROM_DATE)
                                            AND (" + date + @" <= ACT_DATE))  



                                    and   FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 



                                          AND (firms_absences_persons.rank_cat_id = 1))) AB1,

                             (SELECT COUNT (person_data.person_code) AS CNT
                                FROM person_data
                               WHERE     (person_data.firm_code = " + FIRM_CODE + @")
                                     AND (person_data.rank_cat_id = 1)
                                     AND (person_data.borrow_status IN (0, 2))
                                     AND (person_data.OUT_UN_FORCE = 0)) A2,
                             (SELECT person_daily_entry.entry_date AS TIM, PERSON_CODE
                                FROM person_daily_entry
                               WHERE person_daily_entry.from_date =
                                        " + date1 + @") TIM
                       WHERE     (person_data.firm_code = " + FIRM_CODE + @")
                             AND person_data.firm_code = A1.FIRM_CODE(+)
                             AND person_data.person_code = A1.PERSON_CODE(+)
                            AND person_data.person_code = AB1.PERSON_CODE(+)
                             AND person_data.person_code = TIM.PERSON_CODE(+)
                             AND PERSON_DATA.RANK_ID = RANKS.RANK_ID
                             AND (person_data.rank_cat_id = 1)
                             AND (NVL (person_data.OUT_UN_FORCE, 0) = 0)
                             AND (person_data.borrow_status IN (0, 2))
                             AND (person_data.person_code NOT IN
                                     (SELECT aa.person_code
                                        FROM person_data aa
                                       WHERE     (aa.firm_code = " + FIRM_CODE + @")
                                             AND (aa.job_type_id IN (1, 2))
                                             AND (aa.out_un_force = 0)))

                             AND (NVL (person_data.join_date,
                                       " + date + @") <= " + date + @")
                    ORDER BY PERSON_DATA.RANK_ID,
                             PERSON_DATA.CATEGORY_ID,
                             PERSON_DATA.CURRENT_RANK_DATE,
                             PERSON_DATA.SORT_NO";
            DataTable dt = GetData(new OracleCommand(AA)).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    try
                    {
                        dt.Rows[i][j] = ArabicNumeralHelper.ConvertNumerals(dt.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {
                    }
                }
            }

            ReportDataSource ds1 = new ReportDataSource("TAMAM_SH", dt);
            ReportViewer1.LocalReport.DataSources.Add(ds1);
            con.Close();
#endregion

            #region tamam_kada
            OracleConnection con1 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con1.Open();
            string xx1 = @"
   SELECT   PERSON_DATA.PERSON_NAME,
            PERSON_DATA.RANK_ID,
            RANKS.RANK,
            PERSON_DATA.PERSON_CODE,
            PERSON_DATA.CURRENT_RANK_DATE,
            (SELECT  ABSENCE_TYPES.NAME
            -- firms_absences_persons.absence_type_id
               FROM   firms_absences_persons
               , ABSENCE_TYPES
              WHERE   (firms_absences_persons.person_code = person_data.person_code)


and   FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 



                          and firms_absences_persons.absence_type_id=ABSENCE_TYPES.ABSENCE_TYPE_ID(+)
                      AND " + date + @" >= firms_absences_persons.from_date AND (" + date + @" <=  firms_absences_persons.act_DATE)  )
               AS abs_type
     FROM   PERSON_DATA, ranks
    WHERE       (person_data.firm_code = " + FIRM_CODE + @")
            AND (person_data.job_type_id IN (1, 2))
            AND (person_data.out_un_force = 0)
            AND RANKS.RANK_ID = PERSON_DATA.RANK_ID

order by PERSON_CODE";

            DataTable dt1 = GetData(new OracleCommand(xx1)).Tables[0];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    try
                    {
                        dt1.Rows[i][j] = ArabicNumeralHelper.ConvertNumerals(dt1.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {
                    }
                }
            }

            ReportDataSource ds2 = new ReportDataSource("TAMAM_AL_KADA_DS", dt1);
            ReportViewer1.LocalReport.DataSources.Add(ds2);


            con1.Close();
            #endregion

            #region tamam_nobtgya
            OracleConnection con2 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con2.Open();
            string xx2 = @"
   SELECT   FIRMS_ABSENCES_PERSONS.PERSON_CODE,
            FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES,
            PERSON_DATA.RANK_ID,
            PERSON_DATA.PERSON_NAME,
            ranks.RANK,
            ABSENCE_TYPES.NAME,
            FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID,
            PERSON_DATA.SORT_NO,
            PERSON_DATA.CATEGORY_ID,
            PERSON_DATA.CURRENT_RANK_DATE
     FROM   FIRMS_ABSENCES_PERSONS,
            PERSON_DATA,
            ABSENCE_TYPES,
            ranks
    WHERE   (PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS.PERSON_CODE)
            AND (FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID =
                    ABSENCE_TYPES.ABSENCE_TYPE_ID)


and   FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 AND ABSENCE_STATUS IS NULL


            AND ( (FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FIRM_CODE + @")
                 AND (ABSENCE_TYPES.ABSCENCE_CATEGORY_ID = 1)
                        AND ( (" + date + @" >= firms_absences_persons.FROM_DATE)
                             AND (" + date + @" <= firms_absences_persons.ACT_DATE))
            --     AND (" + date + @" BETWEEN TO_DATE(firms_absences_persons.from_date)
                           --            AND  TO_DATE (
                             --                  firms_absences_persons.TO_DATE
                          --                  ))

                 AND (firms_absences_persons.rank_cat_id = 1))
            AND ranks.rank_id = PERSON_DATA.rank_id";

            DataTable dt2 = GetData(new OracleCommand(xx2)).Tables[0];
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    try
                    {
                        dt2.Rows[i][j] = ArabicNumeralHelper.ConvertNumerals(dt2.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {

                    }
                }
            }

            ReportDataSource ds3 = new ReportDataSource("TAMAM_NOBTGYA_DS", dt2);
            ReportViewer1.LocalReport.DataSources.Add(ds1);
            con2.Close();
            #endregion

            #region tamam_TAMAM_OTH
            OracleConnection con4 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con4.Open();
            string xx4 = @"
     SELECT person_data.person_name,
                             person_data.person_code,
                             person_data.rank_id,
                             RANKS.RANK,
                             person_data.id_no,
                             person_data.current_rank_date,
                             person_data.sort_no,
                             person_data.category_id,
                             A1.AB AS absence_type_id,
                             A1.TYP_NM AS absence_type_name,
                             A1.NOTE AS notes,
                             TO_CHAR (A1.FD, 'mm/dd') AS FF,
                             TO_CHAR (A1.TD, 'mm/dd') AS DD,
                            -- TO_CHAR (TIM.TIM, 'hh24:mi') entry_hour, 
                             (CASE
                                         WHEN  (" + date + @") >
                                                 TIM.TIM
                                         THEN
                                             TO_CHAR (TIM.TIM, 'hh24:mi')
                                         ELSE
                                            TO_CHAR ('')
                                      END)
                                       entry_hour,
                             A1.AB AS absence_type_id_y,
                             AB1.TYP_NM AS N_NAME,
                             NVL(A1.CNT,1) AS CNT
                        FROM person_data,
                             RANKS,
                             (SELECT AB.ABSENCE_TYPE_ID AS AB,
                                        AB.PERSON_CODE,
                                        TYP.NAME AS TYP_NM,
                                        AB.ABSENCE_NOTES AS NOTE,
                                        AB.FROM_DATE AS FD,
                                        AB.ACT_DATE AS TD,
                                        AB.FIRM_CODE,
                                        DECODE(TYP.ABSCENCE_CATEGORY_ID,1,0,1) AS CNT
                                  FROM firms_absences_persons AB, ABSENCE_TYPES TYP
                                 WHERE (rank_cat_id = 1) AND AB.ABSENCE_TYPE_ID = TYP.ABSENCE_TYPE_ID
                                       AND ( (" + date + @" >= FROM_DATE)
                                            AND (" + date + @" <= ACT_DATE))
                                       AND (AB.ABSENCE_TYPE_ID NOT IN (16, 17, 31, 32, 54, 55, 50))
                                       AND (firm_code = " + FIRM_CODE + @")) A1,

                                (SELECT   FIRMS_ABSENCES_PERSONS.PERSON_CODE,           
                                        ABSENCE_TYPES.NAME TYP_NM
                                    FROM   FIRMS_ABSENCES_PERSONS,           
                                        ABSENCE_TYPES
                                WHERE    (FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID = ABSENCE_TYPES.ABSENCE_TYPE_ID)
                                        AND ( (FIRMS_ABSENCES_PERSONS.FIRM_CODE =" + FIRM_CODE + @")
                                                AND (ABSENCE_TYPES.ABSCENCE_CATEGORY_ID = 1)
                                                   AND ( (" + date + @" >= FROM_DATE)
                                            AND (" + date + @" <= ACT_DATE)) 



                                    and   FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 



 
                                          AND (firms_absences_persons.rank_cat_id = 1))) AB1,

                             (SELECT COUNT (person_data.person_code) AS CNT
                                FROM person_data
                               WHERE     (person_data.firm_code = " + FIRM_CODE + @")
                                     AND (person_data.rank_cat_id = 1)
                                     AND (person_data.borrow_status IN (0, 2))
                                     AND (person_data.OUT_UN_FORCE = 0)) A2,
                             (SELECT person_daily_entry.entry_date AS TIM, PERSON_CODE
                                FROM person_daily_entry
                               WHERE person_daily_entry.from_date =
                                        " + date1 + @") TIM
                       WHERE     (person_data.firm_code = " + FIRM_CODE + @")
                             AND person_data.firm_code = A1.FIRM_CODE(+)
                             AND person_data.person_code = A1.PERSON_CODE(+)
                            AND person_data.person_code = AB1.PERSON_CODE(+)
                             AND person_data.person_code = TIM.PERSON_CODE(+)
                             AND PERSON_DATA.RANK_ID = RANKS.RANK_ID
                             AND (person_data.rank_cat_id = 1)
                             AND (NVL (person_data.OUT_UN_FORCE, 0) = 0)
                             AND (person_data.borrow_status =1)
                             AND (person_data.person_code NOT IN
                                     (SELECT aa.person_code
                                        FROM person_data aa
                                       WHERE     (aa.firm_code = " + FIRM_CODE + @")
                                             AND (aa.job_type_id IN (1, 2))
                                             AND (aa.out_un_force = 0)))
                             AND (NVL (person_data.join_date,
                                       " + date + @") <= " + date + @")

                    ORDER BY PERSON_DATA.RANK_ID,
                             PERSON_DATA.CATEGORY_ID,
                             PERSON_DATA.CURRENT_RANK_DATE,
                             PERSON_DATA.SORT_NO

";

            DataTable dt4 = GetData(new OracleCommand(xx4)).Tables[0];
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                for (int j = 0; j < dt4.Columns.Count; j++)
                {
                    try
                    {
                        dt4.Rows[i][j] = ArabicNumeralHelper.ConvertNumerals(dt4.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {

                    }
                }
            }

            ReportDataSource ds4 = new ReportDataSource("TAMAM_OTH2", dt4);
            ReportViewer1.LocalReport.DataSources.Add(ds4);
            con4.Close();
            #endregion


            #region tamam_TAMAM_pers_fees
            OracleConnection con5 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con5.Open();
            string xx5 = @"
SELECT to_char(person_daily_entry.entry_date,'HH24:mi') AS TIM, PERSON_DAILY_ENTRY.PERSON_CODE,
PERSON_DAILY_ENTRY_FEE.FEE_NAME,PERSON_DAILY_ENTRY.FEE_VAL,
PERSON_DATA.PERSON_NAME,RANKS.RANK,TO_CHAR(PERSON_DAILY_ENTRY_FEE.FEE_TIME,'HH24:mi')FEE_TIME
                                FROM person_daily_entry,person_daily_entry_fee,person_data,ranks
                               WHERE person_daily_entry.from_date =
                                        to_date('" + dd1 + @"','yyyy/mm/dd')
                                        
                                        and PERSON_DAILY_ENTRY.FIRM_CODE=PERSON_DAILY_ENTRY_FEE.FIRM_CODE
                                        and PERSON_DAILY_ENTRY.FIRM_CODE=PERSON_DATA.FIRM_CODE
                                        and PERSON_DAILY_ENTRY.PERSON_CODE=PERSON_DATA.PERSON_CODE
                                        and RANKS.RANK_ID=PERSON_DATA.RANK_ID
                                        and PERSON_DAILY_ENTRY.FEE_VAL<>0
";

            DataTable dt5 = GetData(new OracleCommand(xx5)).Tables[0];
            for (int i = 0; i < dt5.Rows.Count; i++)
            {
                for (int j = 0; j < dt5.Columns.Count; j++)
                {
                    try
                    {
                        dt5.Rows[i][j] = ArabicNumeralHelper.ConvertNumerals(dt5.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {

                    }
                }
            }

            ReportDataSource ds5 = new ReportDataSource("TAMMAM_FEES", dt5);
            ReportViewer1.LocalReport.DataSources.Add(ds4);
            con4.Close();
            #endregion

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/REPORTS/REP_TAMMAM/PERORT_TAMMAM.rdlc");

            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("day_date", dd));//aaaTextBox2.Text
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("day_date1", ArabicNumeralHelper.ConvertNumerals(TextBox2.Text)));

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(ds1);
            ReportViewer1.LocalReport.DataSources.Add(ds2);
            ReportViewer1.LocalReport.DataSources.Add(ds3);
            ReportViewer1.LocalReport.DataSources.Add(ds4);
            ReportViewer1.LocalReport.DataSources.Add(ds5);


            this.ReportViewer1.AsyncRendering = true;
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");



            System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"99%\" height=\"900px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            ltEmbed.Text = string.Format(embed, ResolveUrl("xx1.pdf"));
        }
    }
}