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


namespace WORKFLOW_APP.REPORTS.Rep_Clinic_Sh
{
    public partial class Report_Clinic_Sh : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // parameters of report
            string rank_off = Request.QueryString["rank_off"];
            string person_name = Request.QueryString["person_name"];
            string from_date = Request.QueryString["from_date"];
            string from_Time = Request.QueryString["from_Time"];
            string To_Time = Request.QueryString["To_Time"];
            string PERSON_CODE = Request.QueryString["PERSON_CODE"];

            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("datefrom", ArabicNumeralHelper.ConvertNumerals(from_date)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("from_Time", ArabicNumeralHelper.ConvertNumerals(from_Time)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("To_Time", ArabicNumeralHelper.ConvertNumerals(To_Time)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("RANK", ArabicNumeralHelper.ConvertNumerals(rank_off)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("PersonName", ArabicNumeralHelper.ConvertNumerals(person_name)));

            bind_firms(PERSON_CODE);
            bind_step_Report(from_date, from_Time, To_Time, PERSON_CODE);
            // show report in pdf
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");

            System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"97%\" height=\"750px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("xx1.pdf"));
        }

        public void bind_step_Report(string from_date, string from_Time, string To_Time, string PERSON_CODE)
        {
            string xx = @" SELECT 
                           
                        
                             OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                            -- OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                             OFF_ABS_STEPS.ORDER_ID ,
                          --  OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                       --    FIRMS_ABSENCES_PERSONS_DET.PERSON_DATE_OWEN ,
                           PERSON_DATA.PERSON_NAME,
                           RANKS.RANK
                             
FROM   OFF_ABS_STEPS ,FIRMS_ABSENCES_PERSONS_DET,PERSON_DATA,RANKS,
                         (SELECT FIRMS_ABSENCES_PERSONS.PERSON_CODE ,
                          OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                          MAX (OFF_ABS_STEPS.ORDER_ID) AS ORDER_ID
                          FROM FIRMS_ABSENCES_PERSONS_DET, OFF_ABS_STEPS,
                          FIRMS_ABSENCES_PERSONS
    
             WHERE   FIRMS_ABSENCES_PERSONS.PERSON_CODE ='" + PERSON_CODE + @"'
                     AND FIRMS_ABSENCES_PERSONS.PERSON_CODE = FIRMS_ABSENCES_PERSONS_DET.PERSON_CODE
                     AND FIRMS_ABSENCES_PERSONS.FIRM_CODE = FIRMS_ABSENCES_PERSONS_DET.FIRM_CODE
                     AND FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_GROUP_ID = FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_GROUP_ID
                    AND FIRMS_ABSENCES_PERSONS_DET.FROM_DATE  = FIRMS_ABSENCES_PERSONS.FROM_DATE
                    AND FIRMS_ABSENCES_PERSONS_DET.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID
                     AND OFF_ABS_STEPS.OFF_ABS_STEPS_ID = FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_STEPS_ID
                     AND  FIRMS_ABSENCES_PERSONS_DET.ABSENCE_TYPE_ID = 11
                     AND to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'yyyy/MM/dd') ='" + from_date + @"'
                     AND to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'hh24:mi') ='" + from_Time + @"'
                   
                     GROUP BY OFF_ABS_STEPS.OFF_ABS_GROUP_ID,  FIRMS_ABSENCES_PERSONS.PERSON_CODE) DT

                   WHERE OFF_ABS_STEPS.ORDER_ID = DT.ORDER_ID
                     AND OFF_ABS_STEPS.OFF_ABS_GROUP_ID = DT.OFF_ABS_GROUP_ID
                   AND FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_STEPS_ID= OFF_ABS_STEPS.OFF_ABS_STEPS_ID
                   AND to_char(FIRMS_ABSENCES_PERSONS_DET.FROM_DATE,'yyyy/MM/dd') ='" + from_date + @"'
                   AND to_char(FIRMS_ABSENCES_PERSONS_DET.FROM_DATE,'hh24:mi') ='" + from_Time + @"'
                  AND PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS_DET.PERSON_DATE_OWEN
                  AND RANKS.RANK_ID = PERSON_DATA.RANK_ID";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;




        }
        public void bind_firms(string PERSON_CODE)
        {


            string xx = @"   
        SELECT  
             FS.NAME AS FIRM_NAME ,
             FS.POSTAL_GROUP,
            (SELECT FIRMS.name   from firms where FIRMS.FIRM_CODE =(select FIRMS.parent_firm_code from firms where FIRMS.firm_code= PD.FIRM_CODE)) AS PARENT_NAME 
             
 FROM
            PERSON_DATA PD , FIRMS FS 
        WHERE 
             PD.PERSON_CODE = '" + PERSON_CODE + @"' AND 
             FS.FIRM_CODE = PD.FIRM_CODE  ";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet2", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;




        }
        
        
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