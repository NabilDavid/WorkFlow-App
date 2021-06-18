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

namespace WORKFLOW_APP.Views.Shared
{
    public class General
    {

       /* public static DataSet GetData(OracleCommand cmd)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OracleConnection con = new OracleConnection(strConnString))
            {
                using (OracleDataAdapter sda = new OracleDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }*/

       /* public static DataSet get_data(string query)
        {
            OracleConnection conn = new OracleConnection("Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True");
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter(query, conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            conn.Close();
            return ds;
        }*/
        public static List<Dictionary<string, object>> GetData_New(OracleCommand cmd)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            
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

                        List<Dictionary<string, object>>
                        lstRows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> dictRow = null;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dictRow = new Dictionary<string, object>();
                            foreach (DataColumn col in ds.Tables[0].Columns)
                            {
                                dictRow.Add(col.ColumnName, dr[col]);
                            }
                            lstRows.Add(dictRow);
                        }
                        return lstRows;
                    }
                }

            }

        }
        public static DataSet returnqueryresult(String query)
        {
            OracleConnection conn = new OracleConnection("DATA SOURCE=wf;PASSWORD=firm_work;PERSIST SECURITY INFO=True;USER ID=firm_work");
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter(query, conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            conn.Close();
            return ds;
        }

        public static bool check_ABS (string PERSON_CODE, string fin_year, string period_id, string firms, string RANK_CAT_ID, string person_rank_cat, string FullDateFrom, string FullDateTo){
        
             var V_query = @"
         SELECT PERSON_CODE,
               FIN_YEAR,
               TRAINING_PERIOD_ID,
               FIRM_CODE,
               FROM_DATE,
               TO_DATE,
               ABSENCE_TYPE_ID,
               RANK_CAT_ID,
               PERSON_CAT_ID
          FROM FIRM_WORK.FIRMS_ABSENCES_PERSONS
         WHERE     PERSON_CODE = '"+PERSON_CODE+@"'
               AND FIN_YEAR = '"+fin_year+@"'
               AND TRAINING_PERIOD_ID = '"+period_id+ @"'
and COMMANDER_FLAG=1
               AND FIRM_CODE = '" + firms+@"'
               AND (
               (
                (TO_DATE ('" +FullDateFrom+@"', 'dd/mm/yyyy hh24:mi') <= FROM_DATE )
                 AND
                (TO_DATE ('" + FullDateTo + @"' , 'dd/mm/yyyy hh24:mi') > FROM_DATE )
               )
               or
               (
                (TO_DATE ('" + FullDateFrom + @"' , 'dd/mm/yyyy hh24:mi') >= FROM_DATE )
                 AND 
                (TO_DATE ('" + FullDateTo + @"' , 'dd/mm/yyyy hh24:mi') <= TO_DATE )
               )
               or
               (
                (TO_DATE ('" + FullDateFrom + @"', 'dd/mm/yyyy hh24:mi') < TO_DATE )
                AND 
                (TO_DATE ('" + FullDateTo + @"' , 'dd/mm/yyyy hh24:mi') >= TO_DATE )
               )
              )
               AND RANK_CAT_ID = '"+RANK_CAT_ID+@"'
               AND PERSON_CAT_ID = '"+person_rank_cat+@"' ";
              var V_data = returnqueryresult(V_query);
            int a = V_data.Tables[0].Rows.Count;

            if(a == 0){

            return true;

            }else{

            return false;

            }


        } 

        public static string[] bind_scaler_value(string query)
        {


            OracleCommand cmd = new OracleCommand(query);
            var data = GetData_New(cmd);
            string[] x = new string[4];
           // x[0] = data.Tables[0].Rows[0].ItemArray[0].ToString();
           // x[1] = data.Tables[0].Rows[0].ItemArray[1].ToString();
            //x[2] = data.Tables[0].Rows[0].ItemArray[2].ToString();
            var mn = DateTime.Now.Month.ToString();
            x[3] = mn;
            return x;

        }

        public string Max_Id(string id_col,string tb_name)
        {
            string query = @"select  TO_CHAR(NVL(MAX(TO_NUMBER(SUBSTR(" + id_col + ",   INSTR(" + id_col + ",'-') +1    ))),0)+1) MAX_CODE  from " + tb_name;

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data[0]["MAX_CODE"].ToString();

        }


        public static string TRAINING_PERIODS()
        {
            string query = @"SELECT TRAINING_PERIODS.TRAINING_PERIOD,TRAINING_PERIODS.FIN_YEAR,TRAINING_PERIODS.TRAINING_PERIOD_ID FROM TRAINING_PERIODS WHERE   SYSDATE BETWEEN TRAINING_PERIODS.PERIOD_FROM AND TRAINING_PERIODS.PERIOD_TO + 1" ;

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data.ToString();

        }
        public static string TRAINING_PERIODS1()
        {
            string query = @"SELECT TRAINING_PERIODS.TRAINING_PERIOD,TRAINING_PERIODS.FIN_YEAR,TRAINING_PERIODS.TRAINING_PERIOD_ID FROM TRAINING_PERIODS WHERE   SYSDATE BETWEEN TRAINING_PERIODS.PERIOD_FROM AND TRAINING_PERIODS.PERIOD_TO + 1";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            var period = data[0]["TRAINING_PERIOD_ID"];
            var finyear = data[0]["FIN_YEAR"];
         var training=   period.ToString() + "_" + finyear.ToString();
         return training;

        }

        public static string TRAINING_YEARS()
        {
            string query = @"SELECT FIN_YEAR FROM TRAINING_YEARS ORDER BY 1 DESC";

            OracleCommand cmd = new OracleCommand(query);
            var data = General.GetData_New(cmd);
            //return Json(data, JsonRequestBehavior.AllowGet);
            return data.ToString();

        }
        public static void exec_q(string qry, string CON)
        {
            //string strConnString = @"Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True";
            if (CON == "")
            {
                CON = "Data Source=WORKFLOW;User ID=COMMAND;Password=COMMAND;Unicode=True";
            }
            else if(CON == "1")
            {
                CON = "DATA SOURCE=wf;PASSWORD=firm_work;PERSIST SECURITY INFO=True;USER ID=firm_work";
            }
            OracleConnection con = new OracleConnection(CON);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = qry;
            con.Open();
            cmd.ExecuteScalar();
            con.Close();
            //return x.ToString();
        }

        public static string exec_f(string qry, string CON)
        {
            if (CON == "")
            {
                CON = "Data Source=WF;User ID=FIRM_WORK;Password=FIRM_WORK;Unicode=True";
            }
            OracleConnection con = new OracleConnection(CON);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = qry;
            con.Open();
            var x = cmd.ExecuteScalar();
            con.Close();
            return x.ToString();
        }

      
    }
}