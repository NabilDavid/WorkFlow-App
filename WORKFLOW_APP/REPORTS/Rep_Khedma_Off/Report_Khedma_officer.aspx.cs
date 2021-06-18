using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WORKFLOW_APP.REPORTS.Rep_Khedma_Off
{
    public partial class Report_Khedma_officer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Parameters from url 
            string FIRM = Request.QueryString["FIRM"];
            string TYP = Request.QueryString["TYP"];
            string MON = Request.QueryString["MON"];
            string monthplus = Request.QueryString["monthplus"];

            string TY = Request.QueryString["TY"];
            string PROID = Request.QueryString["PROID"];


            // method to get data of report
            bind_khedma_off(FIRM, TYP, MON, TY, PROID);
            bind_khedma_off_wait(FIRM, TYP, monthplus, TY, PROID);

            // Code To show report in pdf
            byte[] reportContent = ReportViewer1.LocalReport.Render("PDF");

            System.IO.File.WriteAllBytes(Server.MapPath("xx1.pdf"), reportContent);

            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"99%\" height=\"750px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            Literal1.Text = string.Format(embed, ResolveUrl("xx1.pdf"));

        }

        // query to get data of report
        public void bind_khedma_off(string FIRM, string TYP, string MON, string TY, string PROID)
        {
            string query = @"SELECT  FIRMS.NAME as nameUnit , ABSENCE_TYPES.NAME as nameKhedma , PERSON_DATA.RANK_ID,
                                     RANKS.RANK,
                                     PERSON_DATA.PERSON_NAME,
                                     FIRMS_ABSENCES_PERSONS.FIRM_CODE,
                                     to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'yyyy/mm/dd')FROM_DATE,
                                    to_char(FIRMS_ABSENCES_PERSONS.TO_DATE, 'yyyy/mm') || '/' || TO_CHAR (last_day(to_date(FIRMS_ABSENCES_PERSONS.TO_DATE, 'yyyy/mm/dd')),'dd')  TO_DATE,
                                     --TO_CHAR(FIRMS_ABSENCES_PERSONS . TO_DATE, 'D'),  
                                     DECODE(TO_CHAR(FIRMS_ABSENCES_PERSONS . FROM_DATE, 'D'),1,'الأحد',2,'الأثنين',3,'الثلاثاء',4,'الأربعاء',5,'الخميس',6,'الجمعة','السبت') AS DY,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES,
                                     PERSON_DATA.ID_NO,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CODE,
                                     FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG,
                                     FIRMS_ABSENCES_PERSONS.FIN_YEAR,
                                     FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID,
                                     FIRMS_ABSENCES_PERSONS.RANK_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_STATUS,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID,
                                     PERSON_DATA.CATEGORY_ID,
                                     PERSON_DATA.SORT_NO,
                                     PERSON_DATA.CURRENT_RANK_DATE,
                                     FIRMS_ABSENCES_PERSONS.FORCE_DELETE_DATE,
                                     FIRMS_ABSENCES_PERSONS.ESCAPE_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.RETURN_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.DAY_STATUS,
                                     DECODE(FIRMS_ABSENCES_PERSONS.DAY_STATUS,2,'عطلة',1,'قبل العطلة','عادية') AS DAY_STAT
                              FROM  FIRMS_ABSENCES_PERSONS ,   
                                      PERSON_DATA ,   
                                      ABSCENCE_CATEGORIES ,   
                                      ABSENCE_TYPES,
                                      RANKS ,
                                      FIRMS    
                               WHERE (  PERSON_DATA . PERSON_CODE  =  FIRMS_ABSENCES_PERSONS . PERSON_CODE  ) and  
                                FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 and
                                     (RANKS.RANK_ID=PERSON_DATA.RANK_ID) AND
                                     (  ABSENCE_TYPES . ABSCENCE_CATEGORY_ID  =  ABSCENCE_CATEGORIES . ABSCENCE_CATEGORY_ID  ) and  
                                     (  ABSENCE_TYPES . ABSENCE_TYPE_ID  =  FIRMS_ABSENCES_PERSONS . ABSENCE_TYPE_ID  ) and  
                                     (FIRMS_ABSENCES_PERSONS.FIRM_CODE = FIRMS.FIRM_CODE  )and 
                                     ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FIRM + @" ) AND  
                                     ( person_data.rank_cat_id = 1 ) AND  
                                     ( to_number(to_char(firms_absences_persons.from_date , 'mm')) = " + MON + @" ) AND  
                                     ( abscence_categories.abscence_category_id = 1 ) AND  
                                     ( firms_absences_persons.absence_type_id = " + TYP + @" ) AND  
                                     ( firms_absences_persons.fin_year = '" + TY + @"' ) AND  
                                     ( FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID = " + PROID + @" ) )
                                ORDER BY FIRMS_ABSENCES_PERSONS.FROM_DATE    ";

            OracleCommand cmd = new OracleCommand(query);

            DataTable data = GetData(cmd);

            data = TO_ARABIC(data);

            ReportDataSource RDS = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            this.ReportViewer1.LocalReport.Refresh();
            this.ReportViewer1.AsyncRendering = true;

        }

        // query to get data of report
        public void bind_khedma_off_wait(string FIRM, string TYP, string monthplus, string TY, string PROID)
        {
            string query = @"SELECT  FIRMS.NAME as nameUnit , ABSENCE_TYPES.NAME as nameKhedma , PERSON_DATA.RANK_ID,
                                     RANKS.RANK,
                                     PERSON_DATA.PERSON_NAME,
                                     FIRMS_ABSENCES_PERSONS.FIRM_CODE,
                                     to_char(FIRMS_ABSENCES_PERSONS.FROM_DATE,'yyyy/mm/dd')FROM_DATE,
                                      to_char(FIRMS_ABSENCES_PERSONS.TO_DATE,'yyyy/mm/dd')TO_DATE,
                                     --TO_CHAR(FIRMS_ABSENCES_PERSONS . TO_DATE, 'D'),  
                                     DECODE(TO_CHAR(FIRMS_ABSENCES_PERSONS . FROM_DATE, 'D'),1,'الأحد',2,'الأثنين',3,'الثلاثاء',4,'الأربعاء',5,'الخميس',6,'الجمعة','السبت') AS DY,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_NOTES,
                                     PERSON_DATA.ID_NO,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CODE,
                                     FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG,
                                     FIRMS_ABSENCES_PERSONS.FIN_YEAR,
                                     FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID,
                                     FIRMS_ABSENCES_PERSONS.RANK_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.PERSON_CAT_ID,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_STATUS,
                                     FIRMS_ABSENCES_PERSONS.ABSENCE_TYPE_ID,
                                     PERSON_DATA.CATEGORY_ID,
                                     PERSON_DATA.SORT_NO,
                                     PERSON_DATA.CURRENT_RANK_DATE,
                                     FIRMS_ABSENCES_PERSONS.FORCE_DELETE_DATE,
                                     FIRMS_ABSENCES_PERSONS.ESCAPE_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.RETURN_ORDER_NO,
                                     FIRMS_ABSENCES_PERSONS.DAY_STATUS,
                                     DECODE(FIRMS_ABSENCES_PERSONS.DAY_STATUS,2,'عطلة',1,'قبل العطلة','عادية') AS DAY_STAT
                              FROM  FIRMS_ABSENCES_PERSONS ,   
                                      PERSON_DATA ,   
                                      ABSCENCE_CATEGORIES ,   
                                      ABSENCE_TYPES,
                                      RANKS ,
                                      FIRMS    
                               WHERE (  PERSON_DATA . PERSON_CODE  =  FIRMS_ABSENCES_PERSONS . PERSON_CODE  ) and  
                                          FIRMS_ABSENCES_PERSONS.COMMANDER_FLAG=1 and
                                     (RANKS.RANK_ID=PERSON_DATA.RANK_ID) AND
                                     (  ABSENCE_TYPES . ABSCENCE_CATEGORY_ID  =  ABSCENCE_CATEGORIES . ABSCENCE_CATEGORY_ID  ) and  
                                     (  ABSENCE_TYPES . ABSENCE_TYPE_ID  =  FIRMS_ABSENCES_PERSONS . ABSENCE_TYPE_ID  ) and  
                                     (FIRMS_ABSENCES_PERSONS.FIRM_CODE = FIRMS.FIRM_CODE  )and 
                                     ( ( FIRMS_ABSENCES_PERSONS.FIRM_CODE = " + FIRM + @" ) AND  
                                     ( person_data.rank_cat_id = 1 ) AND  
                                     ( to_number(to_char(firms_absences_persons.from_date , 'mm')) = " + monthplus + @" ) AND  
                                     ( abscence_categories.abscence_category_id = 1 ) AND  
                                     ( firms_absences_persons.absence_type_id = " + TYP + @" ) AND  
                                     ( firms_absences_persons.fin_year = '" + TY + @"' ) AND  
                                     ( FIRMS_ABSENCES_PERSONS.TRAINING_PERIOD_ID = " + PROID + @" ) )
                                ORDER BY FIRMS_ABSENCES_PERSONS . TO_DATE    ";

            OracleCommand cmd = new OracleCommand(query);

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