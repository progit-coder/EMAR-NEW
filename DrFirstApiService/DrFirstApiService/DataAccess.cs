using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrFirstApiService
{
    public class DataAccess
    {
        public DataSet GetResidentsList(int companyId)
        {
            //int companyId = CompanyID;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMAREntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcDrFirstPrescriptionPatientList", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@CompanyId"].Value = companyId;                

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return ds;
                }
            }
        }

        public DataTable GetCompanyID()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMAREntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("admin.prcGetCompanyIDDrFirst", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;               

             
                adapt.Fill(dt);
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

        public int SaveDownloadedPrescriptionsDetails(int companyId, string patientMRNumber, string filePath, DateTime receivedOn)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMAREntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Insert into Admin.DrFirstFileData(Company_Id,PatientMRNumber,FilePath,ReceivedOn) Values(@Company_Id,@PatientMRNumber,@FilePath,@ReceivedOn)", conn)) {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@PatientMRNumber", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@FilePath", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@ReceivedOn", SqlDbType.DateTime));
                cmd.Parameters["@Company_Id"].Value = companyId;
                cmd.Parameters["@PatientMRNumber"].Value = patientMRNumber;
                cmd.Parameters["@FilePath"].Value = filePath;
                cmd.Parameters["@ReceivedOn"].Value = receivedOn;
                
                conn.Open();
                int result = cmd.ExecuteNonQuery();                
                conn.Close();
                return result;
            }

        }
        public string SavePrescriptions(string xmlData)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMAREntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcInsertDrFirstData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@xml", SqlDbType.NVarChar));
                cmd.Parameters["@xml"].Value = xmlData;
                var param = new SqlParameter("@Count", SqlDbType.Int);
                param.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(param);
                conn.Open();
                cmd.ExecuteNonQuery();
                string val = param.Value.ToString();
                conn.Close();
                return val;
            }
        }
    }
}
