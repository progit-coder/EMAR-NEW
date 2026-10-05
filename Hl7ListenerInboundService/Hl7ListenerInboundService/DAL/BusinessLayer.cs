using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7ListenerInboundService
{
    class BusinessLayer
    {
        String query = "";
        DataTable dt;
        public DataTable getServerDetailsByCompany(int category, int connectionType)
        {
            try
            {
                query = "[Admin].[PrcGetServerDetailsByCompany]";
                var parameter = new SqlParameter[2];
                dt = new DataTable();
                //parameter[0] = new SqlParameter("@Company_Id", DbType.Int32) { Value = companyId };
                parameter[0] = new SqlParameter("@Category", DbType.Int32) { Value = category };
                parameter[1] = new SqlParameter("@connectionType", DbType.Int32) { Value = connectionType };

                dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("DB Exception ---------------------");
                Logger.ErrorLog(ex);
                return dt;
            }
        }
        public List<string> PrcFileAckStatusList(string FileData)
        {
            List<string> ackMessage = new List<string>();
            try
            {
                DataTable dtAck = new DataTable();
                query = "[Admin].[PrcFileInformationErrorCheckdetails]";
                var parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@Input", FileData);
                // parameter[1] = new SqlParameter("@CompanyId", DbType.Int32) { Value = CompanyId };
                dtAck = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
                if (dtAck.Rows.Count > 0)
                {
                    foreach (DataColumn item in dtAck.Columns)
                        ackMessage.Add(dtAck.Rows[0][item].ToString());
                }
                return ackMessage;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("DB Exception ---------------------");
                Logger.ErrorLog(ex);
                return ackMessage;
            }

        }
        public Int64 PrcFileAckSave(string FileName, string FileData, int actStatus, string PatientMRNumber)
        {
            try
            {
                query = "[dbo].[PrcFileAckSave]";
                var parameter = new SqlParameter[4];

                parameter[0] = new SqlParameter("@File_Name", FileName);
                parameter[1] = new SqlParameter("@FileAck_Data", FileData);
                parameter[2] = new SqlParameter("@File_Status", actStatus);
                parameter[3] = new SqlParameter("@PatientMRNumber", PatientMRNumber);
                SQLHelper.GetInstance().ExecuteStoredProcedure(query, parameter);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("DB Exception ---------------------");
                Logger.ErrorLog(ex);
                return 0;
            }

        }
        public string GetPatientMrNumber(string FileData)
        {
            try
            {
                query = "[Admin].[PrcGetPatientMrNumber]";
                var parameter = new SqlParameter[1];
                dt = new DataTable();
                parameter[0] = new SqlParameter("@Input", FileData);

                dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("DB Exception ---------------------");
                Logger.ErrorLog(ex);
                return "";
            }
        }

        public DataTable GetMailCredentials()
        {
            DataTable dt = new DataTable();
            string query = "select Port,UserName,Pwd,Host from [Admin].[MailConfig] where Status=1";
            string constr = ConfigurationManager.ConnectionStrings["BiometricConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd2 = new SqlCommand(query))
                {
                    cmd2.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd2))
                    {
                        sda.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return dt;
                    }
                }
            }
        }
    }
}
