using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;
using System.Security.Claims;
using System.Data.Entity.SqlServer;

namespace LTCPro.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        public ReportsRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._userActivityRepository = userActivityRepository;
        }
        //public List<NurseCommentTypeDropEntity> GetNurseComments()
        //{
        //    var nurseComments = this.dbContext.NurseCommentTypes.ToList();
        //    return this.autoMapper.Map<List<NurseCommentType>, List<NurseCommentTypeDropEntity>>(nurseComments);
        //}
        public IList GetReports()
        {
            var reports = this.dbContext.PrcDashboardVitalsdata().ToList();
            return reports;
        }
        public IList GetCompanyReports()
        {
            var reports = this.dbContext.PrcGetReportsCompanyData().ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }

        public IList GetFacilityReports(int companyId)
        {
            var reports = this.dbContext.PrcGetReportsFacilityData(companyId).ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }

        public IList GetNursestationReports(int companyId, int facilityId)
        {
            var reports = this.dbContext.PrcGetReportsNurseStationData(companyId, facilityId).ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }
        public IList GetCensusReports(int userId, string fromDate, string toDate)
        {

            if (fromDate == "null" && toDate == "null")
            {

                var reports = (from a in this.dbContext.PrcgetReportsCensusData(null, null, null, userId, null, null, null, null, null)
                               select new
                               {
                                   AdmitDate = a.AdmitDate,
                                   Census = a.Census == null ? 0 : (int)a.Census,
                                   NurseStation_Name = a.NurseStation_Name,
                                   Floor_Name = a.Floor_Name,
                                   Bed_Name = a.Bed_Name,
                                   Wing_Desc = a.Wing_Desc,
                                   Resident_Name = a.Resident_Name
                               }).ToList();
                return reports;
            }
            else
            {
                var reports = (from a in this.dbContext.PrcgetReportsCensusData(null, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), userId, null, null, null, null, null)
                               select new
                               {
                                   AdmitDate = a.AdmitDate,
                                   Census = a.Census == null ? 0 : (int)a.Census,
                                   NurseStation_Name = a.NurseStation_Name,
                                   Floor_Name = a.Floor_Name,
                                   Bed_Name = a.Bed_Name,
                                   Wing_Desc = a.Wing_Desc,
                                   Resident_Name = a.Resident_Name
                               }).ToList();
                return reports;
            }


        }

        public IList GetFloorReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            var reports = this.dbContext.PrcGetReportsFloorData(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId).Distinct().ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;

        }

        public IList GetEmarlogo(int Id = 0)
        {
         //   HttpContext.Current.Session["SomeData"] = "";
            var reports = (from a in this.dbContext.PrcGetFooterLogo()
                           select new LogoEntity
                           {
                               Footerlogo = a
                           }).ToList();
            return reports;
        }

        public IList GetHeaderlogo(Nullable<int> companyId, Nullable<int> facilityId)
        {
            if (companyId != null && facilityId == null)
            {
                var reports = this.dbContext.PrcGetHeaderLogo(companyId, null).ToList();
                return reports;
            }
            else if (facilityId != null && companyId == null)
            {
                var reports = this.dbContext.PrcGetHeaderLogo(null, facilityId).ToList();
                return reports;
            }
            else if (companyId == null && facilityId == null)
            {
                var reports = this.dbContext.PrcGetHeaderLogo(null, null).ToList();
                return reports;
            }
            return null;
        }

        public IList GetWingReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            var reports = this.dbContext.PrcGetReportsWingData(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId).Distinct().ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }

        public IList GetRoomReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            var reports = this.dbContext.PrcGetReportsRoomData(companyId, facilityId, nursestationId, floorId, wingId,roomId, bedId ).Distinct().ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }

        public IList GetBedReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            var reports = this.dbContext.PrcGetReportsBedData(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId ).Distinct().ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Dashboard,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return reports;
        }

        public DataSet GetCensusReportByfilter(string fromdate, string todate, int userId, string nursestationId, int type, int reportType)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcgetReportsCensusDataReport", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Type"].Value = type;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@ReportType"].Value = reportType;

                // fill the data table - no need to explicitly call `conn.Open()` - 
                // the SqlDataAdapter automatically does this (and closes the connection, too)
                //DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetCensusReportByDates(string fromdate, string todate, int userId, string nursestationname, int type, int reportType, string value)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcgetReportsCensusPivotDetail", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationName", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationName"].Value = nursestationname;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Type"].Value = type;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@ReportType"].Value = reportType;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@value"].Value = value;
                // fill the data table - no need to explicitly call `conn.Open()` - 
                // the SqlDataAdapter automatically does this (and closes the connection, too)
                //DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public IList GetAllAllergiesByClassReport()
        {
            var allergies = (from a in this.dbContext.AllergyInfoMasters
                             where a.AllergyTypeMaster_Id == 1
                             select new
                             {
                                 AllergyClassId = a.AllergyDesc_Id,
                                 AllergyClassName = a.AllergyDesc,
                                 CreatedBy = (this.dbContext.Users.Where(us => us.User_Id == a.Allergy_CreatedBy).Select(usr => usr.UserName).FirstOrDefault()),
                                 CAllergy_CreatedDate = a.Allergy_CreatedOn,
                                 CAllergy_Id = a.Allergy_Id,
                                 Status = a.Allergy_Status == 1 ? "Active" : "Inactive",

                             }).ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.AllergyMaster,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return allergies;

        }
        //public IList GetAllAllergiesByClassReport()
        //{

        //                    var allergies = (from a in this.dbContext.AllergyByClasses
        //                    select new
        //                       {
        //                         AllergyClassId = a.AllergyClassId,
        //                         AllergyClassName = a.AllergyClassName,
        //                         CreatedBy = (this.dbContext.Users.Where(us => us.User_Id == a.CAllergy_CreatedBy).Select(usr => usr.UserName).FirstOrDefault()),
        //                          CAllergy_CreatedDate = a.CAllergy_CreatedDate,
        //                          CAllergy_Id = a.CAllergy_Id,
        //                        Status = a.CAllergy_Status == 1 ? "Active" : "Inactive",

        //                        }).ToList();
        //    return allergies;
        //}

        //public IList GetAllAllergiesByDrugReport()
        //{
        //    var allergiesdrug = (from a in this.dbContext.AllergyByDrugs
        //                         select new
        //                         {
        //                             AllergyDrugId = a.AllergyDrugId,
        //                             AllergyDrugName = a.AllergyDrugName,
        //                             CreatedBy = (this.dbContext.Users.Where(us => us.User_Id == a.DAllergy_CreatedBy).Select(usr => usr.UserName).FirstOrDefault()),
        //                             DAllergy_CreatedDate = a.DAllergy_CreatedDate,
        //                             DAllergy_Id = a.DAllergy_Id,
        //                             Status = a.DAllergy_Status == 1 ? "Active" : "Inactive",

        //                         }).ToList();
        //    return allergiesdrug;
        //}
        public IList GetAllAllergiesByDrugReport()
        {
            var allergiesdrug = (from a in this.dbContext.AllergyInfoMasters
                                 where a.AllergyTypeMaster_Id == 2
                                 select new
                                 {
                                     AllergyDrugId = a.AllergyDesc_Id,
                                     AllergyDrugName = a.AllergyDesc,
                                     CreatedBy = (this.dbContext.Users.Where(us => us.User_Id == a.Allergy_CreatedBy).Select(usr => usr.UserName).FirstOrDefault()),
                                     DAllergy_CreatedDate = a.Allergy_CreatedOn,
                                     DAllergy_Id = a.Allergy_Id,
                                     Status = a.Allergy_Status == 1 ? "Active" : "Inactive",

                                 }).ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.AllergyMaster,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return allergiesdrug;
        }

        public IList GetAllICD10Report()
        {
            var records = (from i in this.dbContext.ICD10
                           join u in this.dbContext.Users on i.ICD10_CreatedBy equals u.User_Id
                           select new
                           {
                               ICD10_Id = i.ICD10_Id,
                               ICD10_Formatted = i.ICD10_Formatted,
                               ICD10_RawFormat = i.ICD10_RawFormat,
                               ICD10_Description = i.ICD10_Description,
                               CreatedBy = u.UserName,
                               ICD10_CreatedDate = i.ICD10_CreatedDate,
                               Status = i.ICD10_Status == 1 ? "Active" : "Inactive",
                           }).ToList();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.ICD10Master,
                Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return records;
        }

        public DataSet Get72HoursReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcGetReports72HoursData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.SeventyTwoHoursDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetPRNDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetPRNDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PRNDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderDetailsReport(string patientIds, string fromdate, string todate, int ordertype, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetScheduleOrderDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Resident", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Resident"].Value = patientIds == "null" ? (object)DBNull.Value : patientIds;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@OrderType"].Value = ordertype == 0 ? (object)DBNull.Value : ordertype;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetWithoutBarcodeDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetWithoutBarcodeDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BarcodeDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetBiometricsDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetWithoutBiometricsDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.BiometericDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderControlSignoffReport(string fromdate, string todate, string nursestationId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetOrderControlSignoff", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NurseStation", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NurseStation"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userId"].Value = userId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderSignoffDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderControlSubstanceReport(string nursestationId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetOrderControlSubstance", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NurseStation", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NurseStation"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userId"].Value = userId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderControlSubstanceDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderHoldDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetOrderHoldDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderHoldDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderWithFavouritesReport(string passtime, string fromdate, string todate, int userId, string nursestationId, int shiftTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetOrderWithFavourites", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PassTime", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PassTime"].Value = passtime == "null" ? (object)DBNull.Value : passtime;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ShiftId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@ShiftId"].Value = shiftTime;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderwithFavouritesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetCompareCensusDataReport(string year, int userId, string nursestationId, int type, int reporttype)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcgetCompareCensusDataReport", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Years", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Years"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingstationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingstationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Type"].Value = type;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@ReportType"].Value = reporttype;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetRefusedByResidentDetailsReport(string fromdate, string todate, int userId, string nursestationId,string gpi,string datetime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetTherapeuticDrugData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Res", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Res"].Value = datetime == "null" ? (object)DBNull.Value : datetime;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@GPI", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@GPI"].Value = gpi == "null" ? (object)DBNull.Value : gpi;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                ds.Tables[0].Columns["TherapeuticDrugType"].ColumnName = "Order Type";
                ds.Tables[0].Columns["EndDate"].ColumnName = "AdminsterSchedule";

                // string query = "[Patient].[PrcReportsGetTherapeuticDrugData]";
                //string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                //DataSet ds = new DataSet();
                //using (SqlConnection con = new SqlConnection(constrEmar))
                //{
                //    con.Open();
                //    using (SqlCommand cmd = new SqlCommand(query))
                //    {
                //        cmd.Connection = con;
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.CommandTimeout = 180;
                //        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                //        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                //        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                //        cmd.Parameters.Add("@NursingStationId", SqlDbType.VarChar).Value = nursingstationId;
                //        cmd.Parameters.Add("@Res", SqlDbType.VarChar).Value = datetime;
                //        cmd.Parameters.Add("@GPI", SqlDbType.VarChar).Value = passTime;
                //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        {
                //            sda.Fill(ds);
                //        }
                //    }
                //}


                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefusedByResidentDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetMedrefReport(string fromdate, string todate, int userId, string nursestationId, string gpi)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetRefusedByResidentDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@GPI", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@GPI"].Value = gpi == "null" ? (object)DBNull.Value : gpi;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefusedByResidentDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetOrderChangeDetailsReport(string fromdate, string todate, int userId, string nursestationId, string residentId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcreportsGetOrderchange", conn))
            {
                cmd.CommandTimeout = 500;
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@patientId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@patientId"].Value = residentId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Fromdate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int));
                //adapt.SelectCommand.Parameters["@PatientId"].Value = patientId == 0 ? (object)DBNull.Value : patientId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.OrderChangeDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetWithoutScanningDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetWithoutScanningDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ScanningBypassDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetDestructionDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetDestructionDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DestructionDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetFloorStockDetailsReport(int userId, string nursestationId,int facilityId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsNewGetFloorStockDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FacilityId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@FacilityId"].Value = facilityId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.FloorStockDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetPharmacyMedsDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetPharmacyMedsDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PharmacyMedsDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetAverageCensusDataReport(int year, int month, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcGetAverageCensusData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStation", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStation"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Census,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetPsychiatricDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetPsychiatricDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PsychiatricDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetOutBoundErrorDetails(string fromdate, string todate)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetOutBoundErrorDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.HL7OutboundErrorDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetNurseNotesDetailsReport(string fromdate, string todate, int userId, string nursestationId, string commentType,string MedicationReason)
        {
            

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetNurseNotesDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CommentType", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@CommentType"].Value = commentType == "null" ? (object)DBNull.Value : commentType;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@MedicationReason", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@MedicationReason"].Value = commentType == "" ? (object)DBNull.Value : MedicationReason;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.NurseNotesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetMARHistorydetails(int month, int year, string nursingStationId, int userId, string patientID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetMARHistorydetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursingStationId == null ? (object)DBNull.Value : nursingStationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PatientID"].Value = patientID == null ? (object)DBNull.Value : patientID;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetMARHoledetails(int month, int year, string nursingStationId, int userId, string patientID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetMARHoledetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursingStationId == null ? (Object)DBNull.Value : nursingStationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PatientID"].Value = patientID == null ? (Object)DBNull.Value : patientID;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetInboundDetails(string fromdate, string todate, string residentName, int inboundStatus)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetInboundDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ResidentName", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@ResidentName"].Value = residentName == "null" ? (object)DBNull.Value : residentName;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Status"].Value = inboundStatus;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.HL7InboundDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetPrescriberNotesReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetPrescriberNotesDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.PrescriberNotesDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        

        public string GetEmarResidentlegendReport(int month, int year, string nursestationId, string patientId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetMarUserInfo", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PatientId"].Value = patientId == null ? (object)DBNull.Value : patientId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    return ds.Tables[0].Rows[0]["displayname"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        public DataSet GetEmarResidentdetailsReport(int month, int year, string nursestationId, string patientId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetEmarResidentdetails", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PatientId"].Value = patientId == null ? (object)DBNull.Value : patientId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetTherapeuticaltwo(int month, int year, string nursestationId, string patientId, string commentType)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetEmardetailsbydrugclassification]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@DrugClassification", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@DrugClassification"].Value = commentType == null ? (object)DBNull.Value : commentType;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Residents", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Residents"].Value = patientId == null ? (object)DBNull.Value : patientId;

                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        //MAR by Therapeutic Category Legend
        public string GetTherapeuticaltwolegend(int month, int year, string nursestationId, string patientId, string commentType)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetEmardetailsbydrugclassificationInitials]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@DrugClassification", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@DrugClassification"].Value = commentType == null ? (object)DBNull.Value : commentType;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Residents", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Residents"].Value = patientId == null ? (object)DBNull.Value : patientId;

                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["displayname"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        //GetTherapeuticaltwo
        public DataSet GetTherapeuticalMedication(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetMedicationReasonEmardetails]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@DrugClassification", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@DrugClassification"].Value = commentType == null ? (object)DBNull.Value : commentType;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Residents", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Residents"].Value = patientId == null ? (object)DBNull.Value : patientId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@MedicationReason", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@MedicationReason"].Value = shiftTime == null ? (object)DBNull.Value : shiftTime;

                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        //Medical Refusal Legend
        public string GetTherapeuticalMedicationlegend(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetMedicationReasonEmardetailsInitials]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@DrugClassification", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@DrugClassification"].Value = commentType == null ? (object)DBNull.Value : commentType;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Residents", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@Residents"].Value = patientId == null ? (object)DBNull.Value : patientId;

                adapt.SelectCommand.Parameters.Add(new SqlParameter("@MedicationReason", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@MedicationReason"].Value = shiftTime == null ? (object)DBNull.Value : shiftTime;

                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MARDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["displayname"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }


        //GetTherapeuticalMedication
        public DataSet GetEmarResidentdetailsReportAllergyAndDiagnosis(int month, int year, string nursestationId, string patientId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetMarResidentConditions]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == null ? (object)DBNull.Value : nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PatientId"].Value = patientId == null ? (object)DBNull.Value : patientId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                cmd.CommandTimeout = 100000;

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

        public string GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE(int month, int year, string nursestationId, string patientId, int userId)
        {
            //User -- Order ID
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("[Patient].[PrcGetMarResidentConditionsCPOE]", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@OrderID"].Value = userId;
                
                cmd.CommandTimeout = 100000;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Allergyinfo"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public IList GetResidentnamesmarReport(string nursingStationIds , int? ResidentStatus)
        {
            string[] nursestations = nursingStationIds.Split(',');
            if (ResidentStatus == 1)
            {


                var records = (from a in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on a.Patient_Id equals vi.Patient_Id
                               join c in this.dbContext.CommonOrderInfoes on a.Patient_Id equals c.Patient_Id
                              // join s in this.dbContext.DrugAdministers on c.POrder_Id equals s.Porder_Id
                                
                               where (vi.PVisit_Status == 1 || vi.PVisit_Status == 3) && nursestations.Contains(vi.NursingStationId.ToString())
                               select new
                               {
                                   patientId = a.Patient_Id,
                                   patientname = a.PatientLastName + ", " + a.PatientFirstName + " " + (a.PatientMiddleInitial != null ? a.PatientMiddleInitial : "") + (a.DOB != null ? "(" + SqlFunctions.DatePart("mm", a.DOB) + "/" + SqlFunctions.DateName("day", a.DOB) + "/" + SqlFunctions.DateName("year", a.DOB) + ")" : ""),
                                   patinetStatus = a.Patient_Status
                               }).Distinct().OrderBy(item => item.patientname).ToList();
                return records;
            }
            else if (ResidentStatus == 2)
            {


                var records = (from a in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on a.Patient_Id equals vi.Patient_Id
                               join c in this.dbContext.CommonOrderInfoes on a.Patient_Id equals c.Patient_Id
                             //  join s in this.dbContext.DrugAdministers on c.POrder_Id equals s.Porder_Id
                               where vi.PVisit_Status == 2 && nursestations.Contains(vi.NursingStationId.ToString())
                               select new
                               {
                                   patientId = a.Patient_Id,
                                   patientname = a.PatientLastName + ", " + a.PatientFirstName + " " + (a.PatientMiddleInitial != null ? a.PatientMiddleInitial : "") + (a.DOB != null ? "(" + SqlFunctions.DatePart("mm", a.DOB) + "/" + SqlFunctions.DateName("day", a.DOB) + "/" + SqlFunctions.DateName("year", a.DOB) + ")" : ""),
                                   patinetStatus = a.Patient_Status
                               }).Distinct().OrderBy(item => item.patientname).ToList();
                return records;
            }
            else if(ResidentStatus == null)
            {


                var records = (from a in this.dbContext.Demographics
                               join vi in this.dbContext.VisitInfoes on a.Patient_Id equals vi.Patient_Id
                               join c in this.dbContext.CommonOrderInfoes on a.Patient_Id equals c.Patient_Id
                             //  join s in this.dbContext.DrugAdministers on c.POrder_Id equals s.Porder_Id
                               where nursestations.Contains(vi.NursingStationId.ToString())
                               select new
                               {
                                   patientId = a.Patient_Id,
                                   patientname = a.PatientLastName + ", " + a.PatientFirstName + " " + (a.PatientMiddleInitial != null ? a.PatientMiddleInitial : "") + (a.DOB != null ? "(" + SqlFunctions.DatePart("mm", a.DOB) + "/" + SqlFunctions.DateName("day", a.DOB) + "/" + SqlFunctions.DateName("year", a.DOB) + ")" : ""),
                                   patinetStatus = a.Patient_Status

                               }).Distinct().OrderBy(item => item.patientname).ToList();
                return records;
            }
            return null;
        }

        public DataSet GetEmarResidentReportSecurity(int nursingStationId, string date, string passTime, int shiftId, int window)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcEmarResidentReport", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursingStationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@date"].Value = date;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PassTime", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@PassTime"].Value = passTime == null ? (object)DBNull.Value : passTime;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ShiftId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@ShiftId"].Value = shiftId == 0 ? (object)DBNull.Value : Convert.ToInt32(shiftId);
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@window", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@window"].Value = window == 0 ? (object)DBNull.Value : Convert.ToInt32(window);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EMAR,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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

        public DataSet GetUserActivityDetailsReport(string userId, string fromdate, string todate)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcGetUserActivityDetails", conn))
            {

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Fromdate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.UserActivityDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetEkitDetailsReport(/*string fromdate, string todate,*/ int userId, string nursestationId,int ekitType)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsGetEKitDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                //adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                //adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Status"].Value = ekitType;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int));
                //adapt.SelectCommand.Parameters["@PatientId"].Value = patientId == 0 ? (object)DBNull.Value : patientId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetStockDetailsReport(int userId, int companyId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsGetStockData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@CompanyId"].Value = companyId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.StockDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetAdminUsersDetailsReport(string companyId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsAdminUsers", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@CompanyId"].Value = companyId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.AdminUsersReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0, 

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetSetUpConfigDetailsReport(string companyId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsSetUpConfigData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@CompanyId"].Value = companyId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.SetupConfigReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetRefillDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetRefillDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.RefillReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public string GetNurseStationShiftTime(int nsShiftId, int nurseStationId)
        {
            var nurseShiftObj = this.dbContext.NurseShifts.Where(nf => nf.NurseShifts_Id == nsShiftId && nf.NurseStation_Id == nurseStationId && nf.NurseShifts_Status == 1).FirstOrDefault();
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
            var cmpId = this.dbContext.Facilities.Where(f => f.Facility_Id == facilityId).Select(f => f.Company_Id).FirstOrDefault();
            var timeType = this.dbContext.CompanyConfigs.Where(c => c.Company_Id == cmpId).Select(c => c.TimeFormat).FirstOrDefault();
            if (timeType == 0 && nurseShiftObj != null)
            {
                var from = this.dbContext.Hours.Where(h => h.Hour_Id == nurseShiftObj.Fromtime_hoursId).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                var to = this.dbContext.Hours.Where(h => h.Hour_Id == nurseShiftObj.Totime_hoursId).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                string nurseShiftTime = from.RegularTime + " " + from.RegularTimeFormat + " - " + to.RegularTime + " " + to.RegularTimeFormat;
                return nurseShiftTime;
            }
            else if (timeType == 1 && nurseShiftObj != null)
            {
                var from = this.dbContext.Hours.Where(h => h.Hour_Id == nurseShiftObj.Fromtime_hoursId).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                var to = this.dbContext.Hours.Where(h => h.Hour_Id == nurseShiftObj.Totime_hoursId).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                string nurseShiftTime = from.Hour_Desc + " - " + to.Hour_Desc;
                return nurseShiftTime;
            }
            return null;
        }
        public DataSet GetAllergyMasterExcel(int type)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT a.AllergyDesc_Id,a.AllergyDesc,case  a.Allergy_Status  when 1 then 'Active' else 'Inactive' end as 'Allergy Status',u.UserName as 'Allergy Created By', a.Allergy_CreatedOn as 'Allergy Created On'  FROM Admin.AllergyInfoMaster a INNER JOIN Admin.[User] u ON a.Allergy_CreatedBy = u.User_Id WHERE a.AllergyTypeMaster_Id = @Type;", conn);
                da.SelectCommand.Parameters.AddWithValue("@Type", type);
                da.TableMappings.Add("Table", "AllergyInfoMaster");
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        public DataSet GetICDMasterExcel()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT i.ICD10_RawFormat as 'ICD10 RawFormat',i.ICD10_Formatted as 'ICD10 Formatted',i.ICD10_Description as'ICD10 Description',case  i.ICD10_Status  when 1 then 'Active' else 'Inactive' end as 'Status',u.UserName as 'Created By', i.ICD10_CreatedDate as 'Created On' FROM Admin.ICD10 i INNER JOIN Admin.[User] u ON i.ICD10_CreatedBy  = u.User_Id;", conn);
                da.TableMappings.Add("Table", "ICD");
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        public DataSet GetRoleConfigExcel(int userId, int roleId)
        {
            if (roleId == 0)
            {
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                {
                    roleId = Convert.ToInt32(claimsIdentity.FindFirst("RoleId").Value);
                }
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcGetRoleconfigData", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userId"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@role_Id", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@role_Id"].Value = roleId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                return ds;
            }
        }
        public DataSet GetEkitMedsDispensingReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsGetEKitDispensingDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetDocAdministerOrderReport(string fromdate, string todate, int OrderType, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetDocAdminOrderDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@ToDate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@OrderType"].Value = OrderType;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.DocumentAdminReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetCertifiedOrderReport(int userId, int certTimeId, int patientId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportGetCertifyedOrders", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@certifiedTimeId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@certifiedTimeId"].Value = certTimeId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@patientId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@patientId"].Value = patientId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.CertifiedOrdersReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetPharmacyMedsExpiryReport(string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, string nursestationId,int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportsGetPharmacyExpirationDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CheckinFromDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@CheckinFromDate"].Value = checkinFromDate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@CheckinToDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@CheckinToDate"].Value = checkinToDate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ExpireFromDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@ExpireFromDate"].Value = expireFromDate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@ExpireToDate", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@ExpireToDate"].Value = expireTodate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = userId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.MedicationExpirationDateReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetEkitMedsExpiryReport(/*string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate,*/ string nursestationId, int facilityId, int userId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.PrcReportsGetEkitExpirationDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@CheckinFromDate", SqlDbType.VarChar));
                //adapt.SelectCommand.Parameters["@CheckinFromDate"].Value = checkinFromDate;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@CheckinToDate", SqlDbType.VarChar));
                //adapt.SelectCommand.Parameters["@CheckinToDate"].Value = checkinToDate;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@ExpireFromDate", SqlDbType.VarChar));
                //adapt.SelectCommand.Parameters["@ExpireFromDate"].Value = expireFromDate;
                //adapt.SelectCommand.Parameters.Add(new SqlParameter("@ExpireToDate", SqlDbType.VarChar));
                //adapt.SelectCommand.Parameters["@ExpireToDate"].Value = expireTodate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FacilityId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@FacilityId"].Value = facilityId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@UserId"].Value = 1;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EKitMedsExpirationDateReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetCPOEOrderDetailsReport(int porder_id, int quantityId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetCpoeorderInfoDetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@POrder_Id", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@POrder_Id"].Value = porder_id;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.CPOE,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetProfileCertifiedOrderReport(int userId, int certTimeId, int patientId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcReportGetPrifileCertifyedOrders", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@certifiedTimeId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@certifiedTimeId"].Value = certTimeId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@patientId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@patientId"].Value = patientId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ResidentProfileCertifiedOrdersReport,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetEkitMedicationCheckInReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.Prc_GetEkitMedCheckInReport", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;



                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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
        public DataSet GetMedicationQtyonhandUpdateReport(string fromdate, string todate, int userId, string nursestationId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Admin.Prc_ReportGetEkitQtyOnHandUpdate", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@FromDate"].Value = fromdate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime));
                adapt.SelectCommand.Parameters["@Todate"].Value = todate;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@userid"].Value = userId;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@NursingStationId", SqlDbType.VarChar));
                adapt.SelectCommand.Parameters["@NursingStationId"].Value = nursestationId == "null" ? (object)DBNull.Value : nursestationId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EkitDashboard,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.PDF,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
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


    }
}
