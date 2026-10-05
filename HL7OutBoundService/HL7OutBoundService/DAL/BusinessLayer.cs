using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7OutBoundService
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
                return dt;
            }
        }
        public string OutboundAckUpdate(string fileID, string ackData)
        {
            try
            {
                query = "[Admin].[PrcUPdateOutBoundAckData]";
                var parameter = new SqlParameter[2];
                dt = new DataTable();

                parameter[0] = new SqlParameter("@File_Id", fileID);
                parameter[1] = new SqlParameter("@ackData", ackData);

                dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
            //string constr = ConfigurationManager.ConnectionStrings["HL7ConnectionString"].ConnectionString;
            //string commandText = "update Patient.OutBoundFileInformation set File_Acknowledge=@ackData where File_Name=@fileName";

            //using (SqlConnection connection = new SqlConnection(constr))
            //{
            //    SqlCommand command = new SqlCommand(commandText, connection);
            //    command.Parameters.Add("@fileName", SqlDbType.VarChar);
            //    command.Parameters["@fileName"].Value = fileName;
            //    command.Parameters.Add("@ackData", SqlDbType.VarChar);
            //    command.Parameters["@ackData"].Value = ackData;
            //    try
            //    {
            //        connection.Open();
            //        Int64 dr = Convert.ToInt64(command.ExecuteNonQuery());
            //        connection.Close();
            //        if (dr > 0)
            //            return 1;
            //        else
            //            return 0;
            //    }
            //    catch (Exception ex)
            //    {
            //        connection.Close();
            //        return 0;
            //    }
            //}
        }
    }
}
