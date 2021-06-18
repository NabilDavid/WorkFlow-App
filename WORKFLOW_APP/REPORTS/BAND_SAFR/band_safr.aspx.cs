using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WORKFLOW_APP.REPORTS.BAND_SAFR
{
    public partial class band_safr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string TRAVEL_DATE = "2006/05/06";
            string PERSON_CODE = "8864";
            string COUNTRY_ID = "200";

            //string TRAVEL_DATE = Request.QueryString["TRAVEL_DATE"];
            //string PERSON_CODE = Request.QueryString["PERSON_CODE"];
            //string COUNTRY_ID = Request.QueryString["COUNTRY_ID"];

            bind_Report(TRAVEL_DATE, PERSON_CODE, COUNTRY_ID);

            string dateNow = (ConvertNumerals(DateTime.Now.Date.ToString("yyyy/MM/dd")));
            ReportViewer1.LocalReport.SetParameters(new ReportParameter("dateNow", dateNow));
            

            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");
            System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"97%\" height=\"750px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("xx1.pdf"));
        }
        public void bind_Report(string TRAVEL_DATE, string PERSON_CODE, string COUNTRY_ID)
        {


            string xx = @"
SELECT  
           
             PT.NOTES ,
             PT.TRAVEL_BAND_NO,
             PT.SUBJECT,
             to_char(PT.TRAVEL_BND_DATE,'yyyy/MM/dd')TRAVEL_BND_DATE ,
             to_char(PT.START_DATE,'yyyy/MM/dd')START_DATE , 
             to_char(PT.END_DATE , 'yyyy/MM/dd') END_DATE ,
             to_char(PT.TRAVEL_DATE,'yyyy/MM/dd') TRAVEL_DATE, 
             to_char(PT.RETURN_DATE,'yyyy/MM/dd') RETURN_DATE ,
             PT.APPROVAL_NO,
             to_char(PT.APPROVAL_DATE,'yyyy/MM/dd') APPROVAL_DATE ,
             PD.PERSON_NAME ,
             PD.SORT_NO ,
             FS.NAME AS FIRM_NAME ,
             RS.RANK,
             CS.COUNTRY_NAME,
             AD.NAME AS WEAPON_NAME,
             (SELECT FIRMS.name   from firms where FIRMS.FIRM_CODE =(select FIRMS.parent_firm_code from firms where FIRMS.firm_code= PD.FIRM_CODE)) AS PARENT_NAME 
             
 FROM
             PERSONS_TRAVELS PT , PERSON_DATA PD , FIRMS FS , RANKS RS , COUNTRIES CS ,ARMY_DEPARTMENTS AD
 WHERE 
             PT.PERSON_CODE = " + PERSON_CODE + @" AND 
             PT.COUNTRY_ID = " + COUNTRY_ID + @"  AND
             TO_CHAR (PT.TRAVEL_DATE,'yyyy/MM/dd') = '" +TRAVEL_DATE+@"' AND
             PT.PERSON_CODE = PD.PERSON_CODE AND
             FS.FIRM_CODE = PD.FIRM_CODE AND 
             PD.RANK_ID = RS.RANK_ID AND
             PT.COUNTRY_ID = CS.COUNTRY_ID AND 
             AD.DEPARTMENT_ID = PD.DEPARTMENT_ID";

            OracleCommand cmd = new OracleCommand(xx);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
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