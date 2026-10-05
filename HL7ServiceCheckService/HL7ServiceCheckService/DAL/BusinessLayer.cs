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
    public DataTable GetFTEConfigurationReport()
    {
        try
        {
            query = "[Admin].[PrcGetFTEConfigurationList]";
            dt = new DataTable();
            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, null);
            return dt;
            
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public DataTable GetSuperAdminUsersEmail()
    {
        try
        {
            query = "[Admin].[PrcGetSuperUsersEmails]";
            dt = new DataTable();
            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, null);
            return dt;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public DataTable GetMailConfigData()
    {
        try
        {
            query = "[Admin].[PrcGetMailConfigData]";
            dt = new DataTable();
            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, null);
            return dt;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public DataTable GetFileACKAEARData()
    {
        try
        {
            query = "[Admin].[PrcGetFileACKAEARData]";
            dt = new DataTable();
            dt = SQLHelper.GetInstance().ExecuteStoredProcedureReturnDataTable(query, null);
            return dt;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public int UpdateFTEConnectionStatus(int FteConfig_Id, string Status)
    {
        try
        {
            query = "[Admin].[PrcUpdateFteConnectionStatus]";
            var parameter = new SqlParameter[2];

            parameter[0] = new SqlParameter("@FteConfig_Id", FteConfig_Id);
            parameter[1] = new SqlParameter("@Status", Status);
            SQLHelper.GetInstance().ExecuteStoredProcedure(query, parameter);
            return 1;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public int UpdateAckSent(string FileAckIDS)
    {
        try
        {
            query = "[Admin].[PrcUpdateAckSent]";
            var parameter = new SqlParameter[1];

            parameter[0] = new SqlParameter("@FileAckIds", FileAckIDS);
            SQLHelper.GetInstance().ExecuteStoredProcedure(query, parameter);
            return 1;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

}