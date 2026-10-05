using HL7FilesImportService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;


/// <summary>
/// Summary description for BusinessLayer
/// </summary>
public class BusinessLayer
{
    String query = "";
    DataTable dt;
    public BusinessLayer()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int GetUserID()
    {
        try {
            query = "[Admin].[PrcGetUserID]";
            dt = new DataTable();
            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query,null);
            if(dt.Rows.Count>0)
            {
                return Convert.ToInt16(dt.Rows[0][0]);
            }
            else
            {
                return 1;
            }
        }
        catch(Exception ex)
        {
            Library.ExceptionLog(ex);
            return 1;
        }
    }
    public Int64 InsertFileInformation(int CompanyId, string FileName, byte[] FileData, int CreatedBy)
    {
        try
        {
            query = "[Admin].[PrcInsertFileInformation]";
            dt = new DataTable();
            var parameter = new SqlParameter[4];

            parameter[0] = new SqlParameter("@CompanyId", DbType.Int32) { Value = CompanyId };
            parameter[1] = new SqlParameter("@FileName", FileName);
            parameter[2] = new SqlParameter("@FileData", Encoding.UTF8.GetString(FileData, 0, FileData.Length - 1));
            parameter[3] = new SqlParameter("@CreatedBy", DbType.Int32) { Value = CreatedBy };

            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt64(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            Library.ExceptionLog(ex);
            return 0;
        }

    }
    public Int64 PrcFileAckSave(string FileName, string FileData)
    {
        try
        {
            query = "[dbo].[PrcFileAckSave]";            
            var parameter = new SqlParameter[2];
            
            parameter[0] = new SqlParameter("@File_Name", FileName);
            parameter[1] = new SqlParameter("@FileAck_Data", FileData);
            SQLHelper.GetInstance().ExecuteStoredProcedure(query,parameter);
                return 1;
        }
        catch (Exception ex)
        {
            Library.ExceptionLog(ex);
            return 0;
        }

    }
    //public Int64 InsertFileInformation(int CompanyId, string FileName, string FileData, int CreatedBy)
    //{
    //    try
    //    {
    //        //query = @"Insert into Admin.FileInformation(Company_Id,File_Category,[File_Name],File_CreatedDate,File_CreatedBy,File_Data) values('" + FileData + "',@Category,@FileName,@CreatedDate,@CreatedBy,@FileData)";
    //        string constr = ConfigurationManager.ConnectionStrings["HL7ConnectionString"].ConnectionString;
    //        string commandText = "Insert into Admin.FileInformation(Company_Id,File_Category,[File_Name],File_CreatedDate,File_CreatedBy,File_Data) values(@CompanyId,@Category,@FileName,@CreatedDate,@CreatedBy,@FileData)";

    //        using (SqlConnection connection = new SqlConnection(constr))
    //        {
    //            SqlCommand command = new SqlCommand(commandText, connection);
    //            command.CommandTimeout = 50000;
    //            command.Parameters.Add("@CompanyId", SqlDbType.Int);
    //            command.Parameters["@CompanyId"].Value = CompanyId;

    //            command.Parameters.Add("@Category", SqlDbType.Int);
    //            command.Parameters["@Category"].Value = 1;

    //            command.Parameters.Add("@FileName", SqlDbType.VarChar);
    //            command.Parameters["@FileName"].Value = FileName;

    //            command.Parameters.Add("@CreatedDate", SqlDbType.VarChar);
    //            command.Parameters["@CreatedDate"].Value = DateTime.Now.ToShortDateString();

    //            command.Parameters.Add("@CreatedBy", SqlDbType.Int);
    //            command.Parameters["@CreatedBy"].Value = CreatedBy;

    //            command.Parameters.Add("@FileData", SqlDbType.NVarChar);
    //            command.Parameters["@FileData"].Value = FileData;                

    //            try
    //            {
    //                connection.Open();
    //                Int32 rowsAffected = command.ExecuteNonQuery();
    //                if(rowsAffected>0)
    //                {
    //                    connection.Close();
    //                    return GetMaxFileID(CompanyId);
    //                }
    //                else
    //                {
    //                    connection.Close();

    //                    return 0;
    //                }                    
    //            }
    //            catch (Exception ex)
    //            {
    //                connection.Close();
    //                return 0;
    //            }
    //        }            
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //    }
    //}
    public Int64 GetMaxFileID(int CompanyId)
    {
        string constr = ConfigurationManager.ConnectionStrings["HL7ConnectionString"].ConnectionString;
        //string constr = "Data Source = 10.20.4.52; Initial Catalog = Emar_Testing; User Id = sa; Password = ABC123abc";
        string commandText = "select MAX(File_Id) from Admin.FileInformation where Company_Id=@CompanyId";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@CompanyId", SqlDbType.Int);
            command.Parameters["@CompanyId"].Value = CompanyId;
            try
            {
                connection.Open();
                Int64 dr = Convert.ToInt64(command.ExecuteScalar());
                connection.Close();
                return dr;                
            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);
                connection.Close();
                return 0;
            }
        }
    }
    public Int64 GetFileInfoCheck(string FileName)
    {
        string constr = ConfigurationManager.ConnectionStrings["HL7ConnectionString"].ConnectionString;
        //string constr = "Data Source = 10.20.4.52; Initial Catalog = Emar_Testing; User Id = sa; Password = ABC123abc";
        string commandText = "select count(*) from Admin.FileInformation where FILE_NAME=@FileName";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@FileName", SqlDbType.VarChar);
            command.Parameters["@FileName"].Value = FileName;
            try
            {
                connection.Open();
                Int64 dr = Convert.ToInt64(command.ExecuteScalar());
                connection.Close();
                return dr;
            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);
                connection.Close();
                return 0;
            }
        }
    }
    public int InsertDataAfterUpload(string fileId,int CreatedBy)
    {
        try {
            query = "[dbo].[PrcInsertDataAfterFileUpload]";
            var parameter = new SqlParameter[2];

            parameter[0] = new SqlParameter("@File", fileId);
            parameter[1] = new SqlParameter("@CreatedBy", DbType.Int32) { Value = CreatedBy };

            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
            if(dt.Rows.Count>0)
            {
                return Convert.ToInt16(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        catch(Exception ex)
        {
            Library.ExceptionLog(ex);
            return 0;
        }
    }

    public DataTable GetOutBoundAwatingFileData()
    {
        try
        {
            query = "[Admin].[PrcGetOutBoundAwatingFileData]";
            //var parameter = new SqlParameter[1];

            //parameter[0] = new SqlParameter("@File_Name", FileName);

            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, null);
            return dt;
        }
        catch (Exception ex)
        {
            Library.ExceptionLog(ex);
            throw new Exception(ex.Message);
        }
    }
    public int UpdateOutBoundAwatingFile(string FileName)
    {
        try
        {
            query = "[Admin].[PrcUpdateOutBoundAwatingFile]";
            var parameter = new SqlParameter[1];

            parameter[0] = new SqlParameter("@File_Name", FileName);

            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, parameter);
            return 1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //protected void ExecuteScalar(object sender, EventArgs e)
    //{
    //    //string name = txtName2.Text;
    //    //string country = txtCountry2.Text;
    //    string constr = ConfigurationManager.ConnectionStrings["HL7ConnectionString"].ConnectionString;
    //    string query = "INSERT INTO Customers (Name, Country) VALUES (@Name, @Country);SELECT SCOPE_IDENTITY();";
    //    List<SqlParameter> parameters = new List<SqlParameter>();
    //    parameters.Add(new SqlParameter("@Name", name));
    //    parameters.Add(new SqlParameter("@Country", country));
    //    int customerId = Convert.ToInt32(SqlHelper.GetInstance().(constr, CommandType.Text, query, parameters.ToArray()));

    //}
}