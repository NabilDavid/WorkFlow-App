using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Microsoft.Reporting.WebForms;
using System.Data.OracleClient;
using WORKFLOW_APP.REPORTS.REP_TAMMAM;
using System.Data;
using WORKFLOW_APP.Models;

namespace WORKFLOW_APP.REPORTS.PER_M2M
{
    public partial class REP_M2M : System.Web.UI.Page
    {

        static string mission_id_param;
       public string geha_name = "";

       protected void Page_Load(object sender, EventArgs e)
       {
           if (!IsPostBack)
           {
               mission_id_param = Request.QueryString["MISSION_ID"].ToString();
               WF_EN E = new WF_EN();
               int xxx = Convert.ToInt16(mission_id_param);
               var pers_name = from o in E.FIRM_MISSIONS
                               where o.MISSION_ID == xxx
                               select o;
               foreach (var p in pers_name)
               {


                   geha_name = p.FIRM_NAME;

               }
               DataSet1 ds = new DataSet1();
               OracleConnection con = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

               con.Open();

               string xx = @" 
SELECT ROWNUM, FIRM_MISSIONS.IS_DONE,   
         FIRM_MISSIONS.IS_PLANNED,   
         FIRM_MISSIONS.MISSION_TYPE, 
        PERSON_DATA.PERSON_CODE,
         PERSON_DATA.PERSONAL_ID_NO,
         PERSON_DATA.PERSON_NAME, 
       MISSION_TYPES.NAME MISSION_TYPE_NAME,
         to_char(FIRM_MISSIONS.TO_DATE,'dd/mm/yyyy')TO_DATE,   
         to_char(FIRM_MISSIONS.FROM_DATE,'dd/mm/yyyy')FROM_DATE,   
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
    FROM FIRM_MISSIONS      ,MISSION_TYPES  ,FIRM_MISSIONS_MEMBERS  ,PERSON_DATA,ABSENCE_TYPES
   WHERE
     
        MISSION_TYPES.MISSION_TYPE_ID(+) = FIRM_MISSIONS.MISSION_FIRM_CODE
                and FIRM_MISSIONS_MEMBERS.FIN_YEAR=FIRM_MISSIONS.FIN_YEAR
        and FIRM_MISSIONS_MEMBERS.FIRM_CODE=FIRM_MISSIONS.FIRM_CODE
        and FIRM_MISSIONS_MEMBERS.TRAINING_PERIOD_ID=FIRM_MISSIONS.TRAINING_PERIOD_ID
        and FIRM_MISSIONS_MEMBERS.MISSION_ID=FIRM_MISSIONS.MISSION_ID
        and ABSENCE_TYPES.ABSENCE_TYPE_ID=FIRM_MISSIONS.MISSION_TYPE
and FIRM_MISSIONS.MISSION_ID=" + mission_id_param + @"
         AND PERSON_DATA.PERSON_CODE = FIRM_MISSIONS_MEMBERS.PERSON_CODE

          order by ROWNUM desc";



               DataTable dt = GetData(new OracleCommand(xx)).Tables[0];
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

                           //string X = ArabicNumeralHelper.ConvertNumerals(dt.Rows[i].ItemArray[j].ToString());

                           // dt.Rows[i][j] = dt.Rows[i][j].ToString();
                       }
                   }
               }
               //ReportDataSource rds = new ReportDataSource("DataSet1", dt);


               //OracleDataAdapter adapter = new OracleDataAdapter(st, con);
               //adapter.Fill(ds.DataTable3);
               ReportViewer1.LocalReport.DataSources.Clear();
               ReportDataSource ds1 = new ReportDataSource("M2M_DET", dt);
               ReportViewer1.LocalReport.DataSources.Add(ds1);
               con.Close();

               ReportViewer1.ProcessingMode = ProcessingMode.Local;
               ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/REPORTS/PER_M2M/Report_M2m.rdlc");


               // ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("day_date", dd));//aaaTextBox2.Text
               //ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("date_1", ArabicNumeralHelper.ConvertNumerals(date)));




               //ReportViewer1.LocalReport.DataSources.Clear();
               //ReportViewer1.LocalReport.DataSources.Add(ds1);



               this.ReportViewer1.AsyncRendering = true;
               byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");



               System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

               string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"99%\" height=\"600px\">";
               embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
               embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
               embed += "</object>";
               ltEmbed.Text = string.Format(embed, ResolveUrl("xx1.pdf"));

           }

       }
        protected void view_report_Click(object sender, EventArgs e)
        {


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
    }
}