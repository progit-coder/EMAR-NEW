using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public class RcopiaResository : IRcopiaResository
    {

        private readonly SQLHelper dbHelper;
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        string storedprocedure = "";
        public RcopiaResository(IAutoMapper autoMapper, IDbContextEmar dbContext, SQLHelper _dbContext)
        {
            this.dbHelper = _dbContext;
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
           
        }


        public RcopiaEntity GetXMLData()
        {
            storedprocedure = "[DrFirst].[Prc_Send_Patient_Request]";
            var parameter = new SqlParameter[1];
            string PatientID = "5,17";
            parameter[0] = new SqlParameter("@Patient_Id", PatientID);

            DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);

            var records = (from d in dt.AsEnumerable()
                           select new RcopiaEntity
                           {
                               RequestID = Convert.ToString(d["Request_Id"]),
                               XMLRequest = d["xmlrequest"].ToString(),
                             


                           }).FirstOrDefault();

            return records;

            // return records.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

        }

        public string InsertXMLData(Int32 Id, string Response)
        {
            try
            {
                storedprocedure = "[DrFirst].[CaptureXMLResponse]";
                var parameter = new SqlParameter[2];

                parameter[0] = new SqlParameter("@Request_id", Id);
                parameter[1] = new SqlParameter("@Response", Response);

                DataTable dt = this.dbHelper.ExecuteStoredProcedureReturnDataTable(storedprocedure, parameter);


                return "1";
            }
            catch(Exception ex)
            {
                return "0";
            }
            

            // return records.OrderBy(item => item.DisplayName).ThenBy(item => item.Company_Facility_Nursestation).ToList();

        }


    }
}
