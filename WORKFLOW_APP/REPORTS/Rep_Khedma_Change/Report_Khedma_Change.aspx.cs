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

namespace WORKFLOW_APP.REPORTS.Rep_Khedma_Change
{
    public partial class Report_Khedma_Change : System.Web.UI.Page
    {
       //public string fromdate = "";
     //  public string todate = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            // parameters of report
            string FIRM = Request.QueryString["FIRM"];
            string TYP = Request.QueryString["TYP"];
            string MonthYear = Request.QueryString["MonthYear"];
            string From_PersonCode = Request.QueryString["From_PersonCode"];
            string To_PersonCode = Request.QueryString["To_PersonCode"];
            string FromDate = Request.QueryString["FromDate"];

            // method to get data of report
            bind_khedma_off(FIRM, TYP, MonthYear, From_PersonCode, To_PersonCode, FromDate);
            bind_step_Report(FIRM, TYP,To_PersonCode, FromDate);
          //  ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("datefrom",fromdate));
           // ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("dateto", todate));


            // show report in pdf
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");

            System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"99%\" height=\"2700px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("xx1.pdf"));

        }


        // query to get data of report
        public void bind_khedma_off(string FIRM, string TYP, string MonthYear, string From_PersonCode, string To_PersonCode, string FromDate)
        {

            string query = @"SELECT CH.FIRM_CODE,
                             F.FIRM_CODE as UNITID,
            (SELECT FIRMS.name   from firms where FIRMS.FIRM_CODE =(select FIRMS.parent_firm_code from firms where FIRMS.firm_code= FRM_OFF.FIRM_CODE)) AS PARENT_NAME ,
                            F.NAME AS NAMEOFUNIT,
                            T.ABSENCE_TYPE_ID as KHEDMAID,
                           T.NAME AS NAMEOFKHEDMA,
                            FROM_PERSON_CODE,
                            TO_PERSON_CODE,
                            CH.ABSENCE_TYPE_ID,
                            FROM_DATE as dddd,
                            to_char(FROM_DATE,'RRRR/MM/DD') FROM_DATE,
                            TO_CHAR(FROM_DATE,'MM/YYYY')  AS DD,
                            FROM_RANK_ID,
                            FROM_RANK_CAT_ID,
                            FROM_PERSON_CAT_ID,
                            TO_RANK_ID,
                            TO_RANK_CAT_ID,
                            TO_PERSON_CAT_ID,
                            TO_CHAR(EXCHANGE_DATE,'RRRR/MM/DD') AS EXCHANGE_DATE,  
                            to_char(TO_DATE,'RRRR/MM/DD') TO_DATE,
                            TO_DATE as dddd2,
                            OPENION1,
                            SEC_COMMAND_OPENION,
                            COMMAND_DECISION,
                            IS_APPROVED,
                            APPROVAL_NO,
                            APPROVAL_DATE,
                            FRM_R.RANK as RANK1,
                            FRM_OFF.PERSON_NAME as NAME1,
                            TO_R.RANK as RANK2,
                            TO_OFF.PERSON_NAME as NAME2, 
                            CH.OTHER_PER_DECS,
                            CH.PLANNING_DECESION,
                            CH.PLANNING_NOTES,
                            CH.VICE_COMMAND_DECESION,
                            CH.VICE_COMMAND_NOTES,
                            FRM_R.RANK || ' / ' || FRM_OFF.PERSON_NAME AS FOFF,
                            TO_R.RANK || ' / ' || TO_OFF.PERSON_NAME AS TOFF
                    FROM FIRMS_ABSENCE_EXCHANGE CH,
                            PERSON_DATA FRM_OFF,
                            PERSON_DATA TO_OFF,
                            RANKS FRM_R,
                            RANKS TO_R,
                       FIRMS F,
                      ABSENCE_TYPES T
                    WHERE     CH.FROM_PERSON_CODE = FRM_OFF.PERSON_CODE
                            AND CH.FROM_PERSON_CODE = '" + From_PersonCode + @"'
                            AND CH.TO_PERSON_CODE = '" + To_PersonCode + @"'
                            AND TO_CHAR(CH.FROM_DATE,'YYYY/MM/DD') = '" + FromDate + @"'
                            AND CH.FROM_RANK_CAT_ID = FRM_R.RANK_CAT_ID
                            AND CH.FROM_RANK_ID = FRM_R.RANK_ID
                            AND CH.FIRM_CODE = FRM_OFF.FIRM_CODE
                            and CH.FIRM_CODE = F.FIRM_CODE
                            and CH.ABSENCE_TYPE_ID =T.ABSENCE_TYPE_ID
                            AND CH.TO_PERSON_CODE = TO_OFF.PERSON_CODE
                            AND CH.TO_RANK_CAT_ID = TO_R.RANK_CAT_ID
                            AND CH.TO_RANK_ID = TO_R.RANK_ID
                            AND CH.FIRM_CODE = TO_OFF.FIRM_CODE
                            AND CH.FIRM_CODE = '" + FIRM + @"'
                            AND CH.ABSENCE_TYPE_ID = '" + TYP + @"' 
                            AND (FRM_OFF.OUT_UN_FORCE = 0 OR TO_OFF.OUT_UN_FORCE = 0)
                            AND TO_CHAR(FROM_DATE,'YYYY/MM') =   '" + MonthYear + @"'
                        ORDER BY EXCHANGE_DATE DESC, FROM_DATE";


            OracleCommand cmd = new OracleCommand(query);

            DataTable data = GetData(cmd);

            //// get fromdate & todate in english
            //fromdate = data.Rows[0].ItemArray[8].ToString();
            //todate = data.Rows[0].ItemArray[17].ToString();

            // convert data to arabic
            data = TO_ARABIC(data);

            // ADD data to DataSet
            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }
        //get steps of report
        public void bind_step_Report(string FIRM, string TYP, string To_PersonCode, string FromDate)
        {


            string xx = @"  
                    SELECT          
                           OFF_ABS_STEPS.OFF_ABS_STEPS_NAME,
                           OFF_ABS_STEPS.ORDER_ID,
                           PERSON_DATA.PERSON_NAME,
                           RANKS.RANK,
                           FIRMS_ABSENCES_PERSONS_DET.DECTION
                       
                    FROM 
                          FIRMS_ABSENCES_PERSONS_DET,
                          FIRMS_ABSENCES_PERSONS,OFF_ABS_STEPS,
                          PERSON_DATA,RANKS

                    WHERE 

                        FIRMS_ABSENCES_PERSONS.PERSON_CODE = '" + To_PersonCode + @"'
                        AND to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE ,'yyyy/MM/dd') = '" + FromDate + @"'
                        AND  FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID ='" + TYP + @"' 
                        AND FIRMS_ABSENCES_PERSONS.PERSON_CODE = FIRMS_ABSENCES_PERSONS_DET.PERSON_CODE
                        AND FIRMS_ABSENCES_PERSONS.FIRM_CODE = FIRMS_ABSENCES_PERSONS_DET.FIRM_CODE
                        AND FIRMS_ABSENCES_PERSONS.FIRM_CODE ='" + FIRM + @"'
                        AND FIRMS_ABSENCES_PERSONS_DET.ABSENCE_TYPE_ID=FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID
                        AND FIRMS_ABSENCES_PERSONS_DET.FROM_DATE  = FIRMS_ABSENCES_PERSONS.FROM_DATE
                        AND OFF_ABS_STEPS.OFF_ABS_STEPS_ID = FIRMS_ABSENCES_PERSONS_DET.OFF_ABS_STEPS_ID
                        AND PERSON_DATA.PERSON_CODE = FIRMS_ABSENCES_PERSONS_DET.PERSON_DATE_OWEN
                        AND RANKS.RANK_ID = PERSON_DATA.RANK_ID ";

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