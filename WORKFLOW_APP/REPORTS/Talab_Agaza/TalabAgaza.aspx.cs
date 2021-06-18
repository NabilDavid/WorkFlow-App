using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Web.Services;


namespace WORKFLOW_APP.REPORTS2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
           
            // parameters of report
              string firm_code = Request.QueryString["firm"];
              string person_code = Request.QueryString["PERSON_CODE"]; 
              string SEQ = Request.QueryString["SEQ"];
              string from_date = Request.QueryString["from_date"];
              string dateNow = (ConvertNumerals(DateTime.Now.Date.ToString("yyyy/MM/dd")));     
              ReportViewer1.LocalReport.SetParameters(new ReportParameter("dateNow", dateNow));
              string dateYear = DateTime.Now.Date.ToString("yyyy");

              string hour = ConvertNumerals( DateTime.Now.Hour.ToString());
              ReportViewer1.LocalReport.SetParameters(new ReportParameter("hour", hour));

              string min =  ConvertNumerals(DateTime.Now.Minute.ToString());
              ReportViewer1.LocalReport.SetParameters(new ReportParameter("min", min));

            
            // fire to get data of report
            bind_Agaza_Report(SEQ,person_code);
            bind_Steps_Report(SEQ);
            bind_count_Report(person_code, firm_code,dateYear);
            bind_count2_Report(person_code, firm_code,dateYear);
            bind_precent(firm_code, from_date);
            // to show in Pdf
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");

            System.IO.File.WriteAllBytes(Server.MapPath("TalabAgaza.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"97%\" height=\"750px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("TalabAgaza.pdf"));

        }



        public void bind_Agaza_Report(string SEQ ,string person_code)
        {
            string xx = @"  select
  
  RANKS.RANK 
 ,PERSON_DATA.PERSON_NAME
 ,PERSON_DATA.ADDERESS
 ,PERSON_DATA.PHONE_2
 ,PERSON_DATA.FIRM_CODE
 ,(SELECT FIRMS.name   from firms where FIRMS.FIRM_CODE =(select FIRMS.parent_firm_code from firms where FIRMS.firm_code= PERSON_DATA.FIRM_CODE)) AS PARENT_NAME 
 ,FIRMS.NAME
 ,VACATION_TYPES.NAME as VACATION_NAME
 ,PERSON_VACATIONS.SEQ
 ,to_char(PERSON_VACATIONS.FROM_DATE,'yyyy/MM/dd') AS FROM_DATE
,PERSON_VACATIONS.FROM_DATE AS FROM_DAY
 ,to_char(PERSON_VACATIONS.TO_DATE,'yyyy/MM/dd') AS TO_DATE
, PERSON_VACATIONS.TO_DATE AS TO_DAY
 , to_char ( ( to_date( PERSON_VACATIONS.TO_DATE,'dd/MM/yyyy')-( to_date(PERSON_VACATIONS.FROM_DATE,'dd/MM/yyyy')))+1)ass
 ,PERSON_VACATIONS.VACATION_TYPE_ID
,to_char(PERSON_VACATIONS.EXCHANGE_FOR_DATE,'yyyy/MM/dd') AS EXCHANGE_FOR_DATE
,PERSON_VACATIONS.EXCHANGE_FOR_DATE AS EXCHANGE_FOR_DAY


from 
    RANKS,PERSON_DATA,PERSON_VACATIONS,FIRMS,VACATION_TYPES
where
    PERSON_DATA.PERSON_CODE=PERSON_VACATIONS.PERSON_CODE AND
    PERSON_DATA.RANK_ID=RANKS.RANK_ID AND
    PERSON_DATA.FIRM_CODE=FIRMS.FIRM_CODE AND
    VACATION_TYPES.VACATION_TYPE_ID = PERSON_VACATIONS.VACATION_TYPE_ID AND
    PERSON_VACATIONS.SEQ=" + SEQ+@" AND   PERSON_DATA.PERSON_CODE ="+person_code;

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;




        }

        public void bind_Steps_Report(string SEQ)
        {
            string xx = @" select PERSON_VACATIONS_DET.PERSON_VACATIONS_DET_ID
,PERSON_VACATIONS_DET.OFF_SKELETON_OFFICERS_ID
,PERSON_VACATIONS_DET.DECTION
 ,r1.RANK own_vac_rank
, dd1.PERSON_NAME as own_vac
 ,r2.RANK step_person_rank
 , dd2.PERSON_NAME as step_person
 ,OFF_ABS_STEPS.OFF_ABS_STEPS_NAME
 ,OFF_ABS_STEPS.ORDER_ID
from PERSON_VACATIONS_DET,PERSON_DATA dd1
,PERSON_DATA dd2
,OFF_ABS_STEPS
,RANKS r1
,RANKS r2
where PERSON_VACATIONS_DET.PERSON_VACATIONS_SEQ= " + SEQ + @"
and dd1.PERSON_CODE=PERSON_VACATIONS_DET.PERSON_DATE_ID
and dd2.PERSON_CODE=PERSON_VACATIONS_DET.PERSON_DATE_OWEN
and OFF_ABS_STEPS.OFF_ABS_GROUP_ID=PERSON_VACATIONS_DET.OFF_ABS_GROUP_ID
and OFF_ABS_STEPS.OFF_ABS_STEPS_ID=  PERSON_VACATIONS_DET.OFF_ABS_STEPS_ID
and r1.RANK_ID=dd1.RANK_ID
and r2.RANK_ID=dd2.RANK_ID
order by ORDER_ID
";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet2", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }
        public void bind_count_Report(string person_code,string firm_code, string dateYear)
        {
            string xx = @" SELECT H1.VACATION_TYPE_ID,
         H1.NAME,
         H1.CNT AS CNT1,
         H2.CNT AS CNT2
         ,

case H1.VACATION_TYPE_ID when 23 then 15  when  21 then 4 else 0  end rased1,
case H2.VACATION_TYPE_ID when 23 then 15  when  21 then 3 else 0  end rased2,
(case H1.VACATION_TYPE_ID when 23 then 15  when  21 then 4 else (1 *H1.CNT)  end -  H1.CNT) ba2y1,
(case H2.VACATION_TYPE_ID when 23 then 15  when  21 then 3 else (1 *H2.CNT)  end -  H2.CNT) ba2y2
         
    FROM (  SELECT ROUND (SUM (VAC.cnt)) CNT, VAC.VACATION_TYPE_ID, VAC.name
              FROM (  SELECT VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             TO_CHAR (SUM ( (ACTUAL_END - ACTUAL_START) + 1))
                                AS CNT,
                             PERSON_VACATIONS.VACATION_TYPE_ID
                        FROM VACATION_TYPES, PERSON_VACATIONS
                          WHERE 
                            VACATION_TYPES.VACATION_TYPE_ID = PERSON_VACATIONS.VACATION_TYPE_ID
                             AND ACTUAL_START >=TO_DATE ('01/01/" + dateYear + @"', 'dd/MM/yyyy')
                             AND ACTUAL_END <= TO_DATE ('30/06/" + dateYear + @"', 'dd/MM/yyyy')
                             AND PERSON_VACATIONS.PERSON_CODE = " + person_code + @"
                             AND PERSON_VACATIONS.FIRM_CODE = " + firm_code + @"
                            AND PERSON_VACATIONS.COMANDER_DECESION = 1

                    GROUP BY VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             PERSON_VACATIONS.VACATION_TYPE_ID) VAC
          GROUP BY (VAC.VACATION_TYPE_ID, VAC.name)) H1,
          
          
          
          
          
          
              (  SELECT ROUND (SUM (VAC.cnt)) CNT, VAC.VACATION_TYPE_ID, VAC.name
              FROM (  SELECT VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             TO_CHAR (SUM ( (ACTUAL_END - ACTUAL_START) + 1))
                                AS CNT,
                             PERSON_VACATIONS.VACATION_TYPE_ID
                        FROM VACATION_TYPES, PERSON_VACATIONS
                       WHERE 
                             VACATION_TYPES.VACATION_TYPE_ID = PERSON_VACATIONS.VACATION_TYPE_ID
                             AND ACTUAL_START >=TO_DATE ('01/07/" + dateYear + @"', 'dd/MM/yyyy')
                             AND ACTUAL_END <= TO_DATE ('31/12/" + dateYear + @"', 'dd/MM/yyyy')
                             AND PERSON_VACATIONS.PERSON_CODE = " + person_code + @"
                             AND PERSON_VACATIONS.FIRM_CODE = " + firm_code + @"
                            AND PERSON_VACATIONS.COMANDER_DECESION = 1

                    GROUP BY VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             PERSON_VACATIONS.VACATION_TYPE_ID) VAC
          GROUP BY (VAC.VACATION_TYPE_ID, VAC.name)) H2
          WHERE H1.VACATION_TYPE_ID = H2.VACATION_TYPE_ID(+)
          and H1.VACATION_TYPE_ID  in(23)";



            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);
            
           data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet3", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }

        public void bind_count2_Report(string person_code, string firm_code, string dateYear)
        {
            string xx = @" 
   


  SELECT H1.VACATION_TYPE_ID,
         H1.NAME,
         H1.CNT AS CNT1,
         H2.CNT AS CNT2,
          case H1.VACATION_TYPE_ID when  21 then 7 else 0  end rased1,
       (case H2.VACATION_TYPE_ID when  21 then 7 else (1 *(H2.CNT +H1.CNT ))  end -  H2.CNT - H1.CNT) ba2y1
    
    
    FROM (  SELECT ROUND (SUM (VAC.cnt)) CNT, VAC.VACATION_TYPE_ID, VAC.name
              FROM (  SELECT VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             TO_CHAR (SUM ( (ACTUAL_END - ACTUAL_START) + 1))
                                AS CNT,
                             PERSON_VACATIONS.VACATION_TYPE_ID
                        FROM VACATION_TYPES, PERSON_VACATIONS
                       WHERE  
                             VACATION_TYPES.VACATION_TYPE_ID = PERSON_VACATIONS.VACATION_TYPE_ID
                             AND ACTUAL_START >=TO_DATE ('01/01/" + dateYear + @"', 'dd/MM/yyyy')
                             AND ACTUAL_END <= TO_DATE ('30/06/" + dateYear + @"', 'dd/MM/yyyy')
                             AND PERSON_VACATIONS.PERSON_CODE = " + person_code + @"
                             AND PERSON_VACATIONS.FIRM_CODE = " + firm_code + @"
                        --     AND PERSON_VACATIONS.COMANDER_DECESION = 1

                    GROUP BY VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             PERSON_VACATIONS.VACATION_TYPE_ID) VAC
          GROUP BY (VAC.VACATION_TYPE_ID, VAC.name)) H1,
          
              (  SELECT ROUND (SUM (VAC.cnt)) CNT, VAC.VACATION_TYPE_ID, VAC.name
              FROM (  SELECT VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             TO_CHAR (SUM ( (ACTUAL_END - ACTUAL_START) + 1))
                                AS CNT,
                             PERSON_VACATIONS.VACATION_TYPE_ID
                        FROM VACATION_TYPES, PERSON_VACATIONS
                       WHERE VACATION_TYPES.VACATION_TYPE_ID = PERSON_VACATIONS.VACATION_TYPE_ID
                             AND ACTUAL_START >=TO_DATE ('01/07/" + dateYear + @"', 'dd/MM/yyyy')
                             AND ACTUAL_END <= TO_DATE ('31/12/" + dateYear + @"', 'dd/MM/yyyy')
                             AND PERSON_VACATIONS.PERSON_CODE = " + person_code + @"
                             AND PERSON_VACATIONS.FIRM_CODE = " + firm_code + @"
                           --  AND PERSON_VACATIONS.COMANDER_DECESION = 1

                    
               GROUP BY VACATION_TYPES.NAME,
                             ACTUAL_START,
                             ACTUAL_END,
                             PERSON_VACATIONS.VACATION_TYPE_ID) VAC
          GROUP BY (VAC.VACATION_TYPE_ID, VAC.name)) H2
          
WHERE H1.VACATION_TYPE_ID = H2.VACATION_TYPE_ID(+)
and H1.VACATION_TYPE_ID not in(23)

















";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            //data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet4", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }

        public void bind_precent( string firm_code , string from_date )
        {
            string xx = @"  SELECT aa.cn1,bb.cn2, round((bb.cn2/aa.cn1)*100) div
  FROM (SELECT COUNT (*) cn1
          FROM PERSON_DATA, FIRMS, RANKS
         WHERE     (PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE)
               AND (RANKS.RANK_ID = PERSON_DATA.RANK_ID)
               AND (RANKS.RANK_CAT_ID = PERSON_DATA.RANK_CAT_ID)
               AND (RANKS.PERSON_CAT_ID = PERSON_DATA.PERSON_CAT_ID)
               AND ( (NVL (PERSON_DATA.OUT_UN_FORCE, 0) <> 1))
               AND (PERSON_DATA.FIRM_CODE IN (SELECT FIRMS_B.FIRM_CODE
                                                FROM FIRMS FIRMS_A,
                                                     FIRMS FIRMS_B
                                               WHERE (FIRMS_A.FIRM_CODE =
                                                         '" + firm_code + @"')))
               AND PERSON_DATA.RANK_CAT_ID = 1 ) aa,
       (SELECT COUNT(*) CN2
  FROM FIRMS_ABSENCES_PERSONS
 WHERE     (FIRMS_ABSENCES_PERSONS.FIRM_CODE =  '" + firm_code + @"')
       AND FIRMS_ABSENCES_PERSONS.RANK_CAT_ID = 1
       AND FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID IN (SELECT VACATION_TYPE_ID FROM VACATION_TYPES)
       AND (TO_DATE('" + from_date + @"', 'DD/MM/YYYY') <= FIRMS_ABSENCES_PERSONS.TO_DATE  
       AND TO_DATE('" + from_date + @"', 'DD/MM/YYYY') >= FIRMS_ABSENCES_PERSONS.FROM_DATE ) ) bb
";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            //data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet5", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }




        //get data of query
        public static DataTable GetData(OracleCommand cmd)
        {
            string conn = "DATA SOURCE=wf;PASSWORD=firm_work;PERSIST SECURITY INFO=True;USER ID=firm_work";
            OracleConnection con = new OracleConnection(conn);

            //OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            using (con)
            {
                con.Open();
                using (OracleDataAdapter sda = new OracleDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        //convert to arabic
        public DataTable TO_ARABIC(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    try
                    {
                        dt.Rows[i][j] = ConvertNumerals(dt.Rows[i].ItemArray[j].ToString());
                    }
                    catch
                    {
                    }
                }
            }
            return dt;

        }

        //convert number to arabic
        public string ConvertNumerals(string input)
        {
            //if (true)
            //{
             return input.Replace('0', '\u0660')
                            .Replace('1', '\u0661')
                            .Replace('2', '\u0662')
                            .Replace('3', '\u0663')
                            .Replace('4', '\u0664')
                            .Replace('5', '\u0665')
                            .Replace('6', '\u0666')
                            .Replace('7', '\u0667')
                            .Replace('8', '\u0668')
                            .Replace('9', '\u0669');
            //}
            //else return input;
        }


    }
}