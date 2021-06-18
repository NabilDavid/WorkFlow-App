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
namespace WORKFLOW_APP.REPORTS.REP_TAMMAM_FATRA
{
    public partial class REPORT_PAGE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // TextBox2.Text = DateTime.Now.ToString("mm:HH yyyy/MM/dd");
                TextBox2.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                pub_Date_From = DateTime.Now.ToString("dd/MM/yyyy");
                pub_Date_TO = DateTime.Now.ToString("dd/MM/yyyy");
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

        
            

            

            

            

            


            #region tamam_TAMAM_pers_fees
            OracleConnection con5 = new OracleConnection("Data Source=WF;Persist Security Info=True;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True");

            con5.Open();
            string xx5 = @"
                                SELECT to_char(person_daily_entry.entry_date,'HH24:mi') AS TIM, PERSON_DAILY_ENTRY.PERSON_CODE,PERSON_DATA.RANK_ID,
                             PERSON_DATA.CATEGORY_ID,
                             PERSON_DATA.CURRENT_RANK_DATE,
                             PERSON_DATA.SORT_NO,
                                PERSON_DAILY_ENTRY_FEE.FEE_NAME,PERSON_DAILY_ENTRY.FEE_VAL,
                                PERSON_DATA.PERSON_NAME,RANKS.RANK,TO_CHAR(PERSON_DAILY_ENTRY_FEE.FEE_TIME,'HH24:mi')FEE_TIME,
               to_number(GET_FEE(TO_CHAR(PERSON_DAILY_ENTRY_FEE.FEE_TIME,'HH24:mi'), to_char(person_daily_entry.entry_date,'HH24:mi'))) as daily_period,
TO_CHAR(person_daily_entry.from_date,'dd/mm/yyyy')from_date
                                FROM person_daily_entry,person_daily_entry_fee,person_data,ranks
                                WHERE (person_daily_entry.from_date >=
                                        to_date('" + pub_Date_From+@"','dd/mm/yyyy') and person_daily_entry.from_date <=
                                        to_date('" + pub_Date_TO + @"','dd/mm/yyyy')
                                        )
                                        
                                        and PERSON_DAILY_ENTRY.FIRM_CODE=PERSON_DAILY_ENTRY_FEE.FIRM_CODE
                                        and PERSON_DAILY_ENTRY.FIRM_CODE=PERSON_DATA.FIRM_CODE
                                        and PERSON_DAILY_ENTRY.PERSON_CODE=PERSON_DATA.PERSON_CODE
                                        and RANKS.RANK_ID=PERSON_DATA.RANK_ID
                                        and PERSON_DAILY_ENTRY.FEE_VAL>0
                                                 ORDER BY PERSON_DATA.RANK_ID,
                             PERSON_DATA.CATEGORY_ID,
                             PERSON_DATA.CURRENT_RANK_DATE,
                             PERSON_DATA.SORT_NO
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
           
            con5.Close();
            #endregion

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/REPORTS/REP_TAMMAM_FATRA/PERORT_TAMMAM1.rdlc");

            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("day_date", dd));//aaaTextBox2.Text
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("day_date1", ArabicNumeralHelper.ConvertNumerals(TextBox2.Text)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("from", ArabicNumeralHelper.ConvertNumerals(pub_Date_From)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("to", ArabicNumeralHelper.ConvertNumerals(pub_Date_TO)));


            ReportViewer1.LocalReport.DataSources.Clear();
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



        public static string pub_Date_From = " ";
        public static string pub_Date_TO = " ";
      //  public static string pub_Drop_value = " ";

        [WebMethod]
        public static void SET_variables(string Date_From, string Date_TO)
        {
            pub_Date_From = Date_From;
            pub_Date_TO = Date_TO;
         //   pub_Drop_value = drop_vl;





        }
    }
}