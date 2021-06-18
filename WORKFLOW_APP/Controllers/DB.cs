using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace WORKFLOW_APP.Controllers
{
    public class DB
    {

        public static DataSet GetData(OracleCommand cmd)
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
        }


        //update and delete method
        //connect db and insert rows
        public static int excutecommand(String query)
        {
            try
            {
                //OracleConnection conn = new OracleConnection("DATA SOURCE=WF;PASSWORD=FIRM_WORK;PERSIST SECURITY INFO=True;USER ID=FIRM_WORK");
               OracleConnection conn = new OracleConnection( @"Data Source=wf;User ID=firm_work;Password=firm_work;Unicode=True");
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
                conn.Close();
                if (r == 0)
                    return 0;
                else
                    return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        
    }
}