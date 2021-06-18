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

namespace WORKFLOW_APP.REPORTS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string FIRM_CODE = Request.QueryString["FIRM_CODE"].ToString();
            string MISSION = Request.QueryString["MISSION"].ToString();
           // string MISSION_DETAIL_ID = Request.QueryString["MISSION_DETAIL_ID"].ToString();
           
            //string MISSION = "204";
            //string FIRM_CODE = "1402102001";
            //string MISSION_DETAIL_ID = "125";



            // paramater in the report
            string dateNow = (ConvertNumerals(DateTime.Now.Date.ToString("yyyy/MM/dd")));
            string hour = ConvertNumerals(DateTime.Now.Hour.ToString());
            string min = ConvertNumerals(DateTime.Now.Minute.ToString());
            ReportViewer1.LocalReport.SetParameters(new ReportParameter("min", min));
            ReportViewer1.LocalReport.SetParameters(new ReportParameter("dateNow", dateNow));
            ReportViewer1.LocalReport.SetParameters(new ReportParameter("hour", hour));
            

            // functions to bind the report 
            bind_M2M_Report(MISSION, FIRM_CODE);
            bind_step_Report(MISSION);
        
            
            // to show in Pdf
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");
           System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"97%\" height=\"750px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("xx1.pdf"));

        }


        // query to get data of report
        public void bind_M2M_Report(string MISSION, string FIRM_CODE)
        {
            string xx = @" 
SELECT ROWNUM, FIRM_MISSIONS.IS_DONE,   
         FIRM_MISSIONS.IS_PLANNED,  
         FIRM_MISSIONS.MISSION_TYPE, 
      (SELECT FIRMS.name   from firms where FIRMS.FIRM_CODE =(select FIRMS.parent_firm_code from firms where FIRMS.firm_code= FIRM_MISSIONS.FIRM_CODE)) AS PARENT_NAME ,
        PERSON_DATA.PERSON_CODE,
         PERSON_DATA.PERSONAL_ID_NO,
         PERSON_DATA.PERSON_NAME,
         PERSON_DATA.RANK_ID  ,
        RANKS.RANK_ID,
         RANKS.RANK ,
     PERSON_DATA.FIRM_CODE ,
           FIRMS.FIRM_CODE,
           FIRMS.NAME,
       MISSION_TYPES.NAME MISSION_TYPE_NAME,
         to_char(FIRM_MISSIONS.TO_DATE,'yyyy/MM/dd')TO_DATE,   
         to_char(FIRM_MISSIONS.FROM_DATE,'yyyy/MM/dd')FROM_DATE,   
         FIRM_MISSIONS.DISTRIBUTION,   
         FIRM_MISSIONS.PROJECT_ID,   
         FIRM_MISSIONS.FIRM_CODE,   
         FIRM_MISSIONS.FIN_YEAR,   
         FIRM_MISSIONS.TRAINING_PERIOD_ID,   
         FIRM_MISSIONS.FIRM_NAME,   
         FIRM_MISSIONS.MISSION_ID,   
         FIRM_MISSIONS.MISSION_FIRM_CODE,   
         FIRM_MISSIONS.INTRODUCTION,    FIRM_MISSIONS.SUBJECT,
         FIRM_MISSIONS.FINAL,   
         ABSENCE_TYPES.NAME TYP_NAME 
    FROM FIRM_MISSIONS      ,MISSION_TYPES  ,FIRM_MISSIONS_MEMBERS  ,PERSON_DATA,ABSENCE_TYPES , RANKS ,FIRMS
   WHERE
PERSON_DATA.FIRM_CODE = FIRMS.FIRM_CODE and
     PERSON_DATA.RANK_ID = RANKS.RANK_ID and
        MISSION_TYPES.MISSION_TYPE_ID(+) = FIRM_MISSIONS.MISSION_FIRM_CODE
                and FIRM_MISSIONS_MEMBERS.FIN_YEAR=FIRM_MISSIONS.FIN_YEAR
        and FIRM_MISSIONS_MEMBERS.FIRM_CODE=FIRM_MISSIONS.FIRM_CODE
        and FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID=FIRM_MISSIONS.TRAINING_PERIOD_ID
        and FIRM_MISSIONS_MEMBERS.MISSION_ID=FIRM_MISSIONS.MISSION_ID
        and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRM_MISSIONS.MISSION_TYPE
and FIRM_MISSIONS.MISSION_ID=" + MISSION + @"
and FIRM_MISSIONS.FIRM_CODE=" + FIRM_CODE + @"
         AND PERSON_DATA.PERSON_CODE = FIRM_MISSIONS_MEMBERS.PERSON_CODE

          order by ROWNUM desc  ";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;




        }










       public void bind_step_Report(string MISSION)
       { 


           string xx = @"  SELECT DISTINCT
                           OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                        -- OFF_ABS_STEPS.OFF_ABS_STEPS_ID,
                        -- OFF_ABS_STEPS.ORDER_ID ,
                        -- FIRM_MISSIONS_DET.PERSON_DATE_OWEN,
                           PERSON_DATA.PERSON_NAME,
                           RANKS.RANK
                FROM 
                OFF_ABS_STEPS ,FIRM_MISSIONS_DET,PERSON_DATA,RANKS,

                  (SELECT FIRM_MISSIONS.MISSION_ID,
                         OFF_ABS_STEPS.OFF_ABS_GROUP_ID,
                         MAX (OFF_ABS_STEPS.ORDER_ID) AS ORDER_ID
                    FROM FIRM_MISSIONS_DET, OFF_ABS_STEPS, FIRM_MISSIONS
                   WHERE     FIRM_MISSIONS.MISSION_ID =" + MISSION + @"
                         AND FIRM_MISSIONS.MISSION_ID = FIRM_MISSIONS_DET.MISSION_ID
                         AND FIRM_MISSIONS.MISSION_FIRM_CODE = FIRM_MISSIONS_DET.FIRM_CODE
                         AND OFF_ABS_STEPS.OFF_ABS_GROUP_ID =
                                FIRM_MISSIONS_DET.OFF_ABS_GROUP_ID
                         AND OFF_ABS_STEPS.OFF_ABS_STEPS_ID =
                                FIRM_MISSIONS_DET.OFF_ABS_STEPS_ID
                GROUP BY OFF_ABS_STEPS.OFF_ABS_GROUP_ID, FIRM_MISSIONS.MISSION_ID) DT

            WHERE DT.OFF_ABS_GROUP_ID = OFF_ABS_STEPS.OFF_ABS_GROUP_ID
                        AND OFF_ABS_STEPS.ORDER_ID = DT.ORDER_ID
                        AND OFF_ABS_STEPS.OFF_ABS_STEPS_ID = FIRM_MISSIONS_DET.OFF_ABS_STEPS_ID
                        AND  FIRM_MISSIONS_DET.MISSION_ID =" + MISSION + @"
                        AND PERSON_DATA.PERSON_CODE = FIRM_MISSIONS_DET.PERSON_DATE_OWEN
                        AND RANKS.RANK_ID = PERSON_DATA.RANK_ID";



           OracleCommand cmd = new OracleCommand(xx);

           DataTable data = GetData(cmd);

           data = TO_ARABIC(data);

           ReportDataSource RDS = new ReportDataSource("DataSet2", data);
           ReportViewer1.LocalReport.DataSources.Add(RDS);
           this.ReportViewer1.LocalReport.Refresh();
           this.ReportViewer1.AsyncRendering = true;




       }

        //get data of query
        public static DataTable GetData(OracleCommand cmd)
        {

            //-------------تعديل------------------------------
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
                    using (DataTable ds = new DataTable())
                    {
                        sda.Fill(ds);
                        return ds;
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
            if (true)
            {
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
            }
            else return input;
        }

        
    }
}