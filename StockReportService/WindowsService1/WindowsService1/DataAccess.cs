using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WindowsService1
{
    class DataAccess
    {
        public DataTable GetHeaderLogo(int facilityId)
        {
            //int FacilityID = Convert.ToInt32(ConfigurationManager.AppSettings["FacilityId"]);
            DataTable dt = new DataTable();
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcGetHeaderLogo]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FacilityId", SqlDbType.Int).Value = facilityId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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

        public DataTable GetFooterLogo()
        {
            DataTable dt = new DataTable();
            string queryEmar = "[Admin].[PrcGetFooterLogo]";
            string constrEmar = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd1 = new SqlCommand(queryEmar))
                {
                    cmd1.Connection = con;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd1))
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
        public DataTable GetUserAndEmail()
        {
            DataTable dt = new DataTable();
            string query = "select User_Id,User_Email from admin.[User] where StkReportReq=1 and User_Status=1";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
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

        public DataTable GetStockReportsData(int UserID,int companyID)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcReportsGetStockData]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserID;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyID;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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

        public DataTable GetMailCredentials()
        {
            DataTable dt = new DataTable();
            string query = "select Port,UserName,Pwd,Host from [Admin].[MailConfig] where Status=1";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
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

        internal DataTable GetStockReportsCompanyId(int userId)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcGetCompanyStockReport]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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

        public DataTable GetRefilMailDetailsByTimme(DateTime time)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            string dateTime = time.ToString("MM/dd/yyyy hh:mm:ss tt");
            //DateTime date= new DateTime(2012, 12, 31, 00, 00, 0);
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcGetNurseStationForMailService]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Datetime", SqlDbType.DateTime).Value = dateTime;//time;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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
        public int PrcUpdateMailsendStatus(int File_Id)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcUpdateMailsendStatus]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@File_Id", SqlDbType.Int).Value = File_Id;
                    cmd.CommandTimeout = 200;
                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
        }
        public DataTable GetRefilMailConfigInfo(int nsId,int MailId,int HourId)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetOutBoundErrorAck_Mail]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NurseStation_Id", SqlDbType.Int).Value = nsId;
                    cmd.Parameters.Add("@Rdc_Id", SqlDbType.Int).Value = MailId;
                    cmd.Parameters.Add("@Hour_Id", SqlDbType.Int).Value = HourId;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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
        public int RefilStatusUpdate(string fileIds)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("prcRefilStatusModified", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@fileids", SqlDbType.VarChar).Value = fileIds;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 1;
                    }

                }
            }
        }

        // Order Change Report Methods Start
        public DataTable GetOrderChangeReportMailDetailsByTimme(DateTime time)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
           // time = time.AddMinutes(-17);
            string dateTime = time.ToString("MM/dd/yyyy hh:mm:ss tt");
            DateTime date = time;
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                //[Admin].[PrcGetNurseStationForOrderChangeReportMailService]
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcGetNurseStationForOrderChangeReportMailService]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Datetime", SqlDbType.DateTime).Value = dateTime;// time;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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

        public DataTable GetOrderChangeReportRecords(int nsId, int MailId, int HourId,DateTime date)
        {
            string constrFac = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrFac))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[Admin].[PrcreportOrderChangeMailService]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NursingstationId", SqlDbType.Int).Value = nsId;
                    cmd.Parameters.Add("@Rdc_Id", SqlDbType.Int).Value = MailId;
                    cmd.Parameters.Add("@Hour_Id", SqlDbType.Int).Value = HourId;
                    cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;
                    cmd.CommandTimeout = 200;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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

        public DataTable GetFacilityIdByNsId(int nsId)
        {
            DataTable dt = new DataTable();
            string query = "select Facility_Id from Admin.NursingStation where NurseStation_Id="+nsId;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
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
        public int insertUpdateOrderChangeMailSent(int nsId, DateTime sentdateTime)
        {
            DataTable dt = new DataTable();
            string query = "select * from [Admin].[tblMailLastSentDetails] where NurseStation_Id=" + nsId;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd2 = new SqlCommand(query))
                {
                    cmd2.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd2))
                    {
                        sda.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        con.Open();
                        string insertqry = "update [Admin].[tblMailLastSentDetails] set MailSent= @MailSent where NurseStation_Id= @NurseStation_Id";
                        using (SqlCommand cmd = new SqlCommand(insertqry))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@NurseStation_Id", nsId);
                            cmd.Parameters.AddWithValue("@MailSent", sentdateTime);
                            cmd.ExecuteNonQuery();
                            return 1;
                        }
                    }
                    else
                    {
                        con.Open();
                        string insertqry = "Insert into [Admin].[tblMailLastSentDetails] (NurseStation_Id,MailSent)values(@NurseStation_Id, @MailSent)";
                        using (SqlCommand cmd = new SqlCommand(insertqry))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@NurseStation_Id", nsId);
                            cmd.Parameters.AddWithValue("@MailSent", sentdateTime);
                            cmd.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
        }
    }
}
