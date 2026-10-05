using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.IO;
using System.Globalization;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Security.Claims;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace LTCPro.Repositories
{
    public class EmarRepository : IEmarRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly object todate;
        private readonly object id;
        private object datetime;
        private object emarGridData;

        public EmarRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, FacilityRepository facilityRepository, CommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._facilityRepository = facilityRepository;
            this._commonRepository = commonRepository;
        }
        public EmarData GetEmarResidentGridData(string time, string dateValue, int nurseStationId, int NurseStationShiftId, int showTwoHours)
        {
            if (nurseStationId != 0)
            {
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity()
                    {
                        User_Id = userId,
                        Facility_Id = (int)facilityId,
                        NurseStation_Id = nurseStationId.ToString()
                    };
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }

            time = time == "null" ? null : time.Replace('-', ':');
            dateValue = dateValue.Replace('-', '/') + " " + time;
            DateTime date = Convert.ToDateTime(dateValue);

           


            string query = "[Patient].[PrcEmargriddetails]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@NursingStationId", SqlDbType.Int).Value = nurseStationId;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
                    cmd.Parameters.Add("@PassTime", SqlDbType.VarChar).Value = (object)time ?? DBNull.Value;
                    cmd.Parameters.Add("@ShiftId", SqlDbType.Int).Value = NurseStationShiftId == 0 ? (object)DBNull.Value : NurseStationShiftId;
                    cmd.Parameters.Add("@window", SqlDbType.Int).Value = showTwoHours;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }


                }
            }

            var emarGridData = (from d in ds.Tables[0].AsEnumerable()
                           select new
                           {
                            
                               Patient_Id = Convert.ToInt32(d["Patient_Id"]),
                               PatientMRNumber = d["PatientMRNumber"].ToString(),
                               ExternalPatientId = d["ExternalPatientId"].ToString(),
                               DOB = d["DOB"].ToString(),

                               Resident_Name = d["Resident Name"].ToString(),
                               Pending_count = Convert.ToInt32(d["Pending count"]),
                               PRN_Drugs = Convert.ToInt32(d["PRN Drugs"]),

                               //Prnadministered = Convert.ToInt32(d["Prnadministered"]),
                               //Prnadministered = 0,
                               ImgPath = d["ImageLocation"] != DBNull.Value ? "Yes" : "No",
                               MedReasonCount = Convert.ToInt32(d["MedReasonCount"]),
                               ReviewFlag = d["ReviewFlag"].ToString(),
                               Total_Count = Convert.ToInt32(d["Total Count"]),
                               Administered = Convert.ToInt32(d["Administered"]),
                               Prnadministered = Convert.ToInt32(d["PrnAdministered"]),
                               PrnPending = Convert.ToInt32(d["PrnPending"]),
                               Prnmedreasoncount = Convert.ToInt32(d["PrnMedReasonCount"]),

                           }).ToList();

            var pendingCount = emarGridData.Select(i => i.Pending_count).Sum();
            var totalCount = emarGridData.Select(i => i.Total_Count).Sum();
            var attemptedCount = emarGridData.Select(i => i.MedReasonCount).Sum();
            //var PRNCount = emarGridData.Select(i => i.PRN_Drugs).Sum();
            var Prnadministered = emarGridData.Select(i => i.Prnadministered).Sum();

            var xaxisdata = new List<string> { "Administered", "Not Administered", "Attempted", "PRN Administered" };
            var data = new List<PieSeriesDataEntity>();
            foreach (string type in xaxisdata)
            {
                if (type == "Not Administered")
                    data.Add(new PieSeriesDataEntity { name = type, y = (int)pendingCount - (int)attemptedCount });
                else if (type == "Administered")
                    data.Add(new PieSeriesDataEntity { name = type, y = (int)totalCount - (int)pendingCount });
                else if (type == "Attempted")
                    data.Add(new PieSeriesDataEntity { name = type, y = (int)attemptedCount });

               // else if (type == "PRN Administered")
                   // data.Add(new PieSeriesDataEntity { name = type, y = (int)Prnadministered });
            }
            //else if (type == "PRN")
            //  data.Add(new PieSeriesDataEntity { name = type, y = (int)PRNCount });
            var PRNpendingCount = emarGridData.Select(i => i.PrnPending).Sum();
            var PRNtotalCount = emarGridData.Select(i => i.PRN_Drugs).Sum();
            var PRNattemptedCount = emarGridData.Select(i => i.Prnmedreasoncount).Sum();
            var PRNPRNCount = emarGridData.Select(i => i.PRN_Drugs).Sum();
            var PRNPrnadministered = emarGridData.Select(i => i.Prnadministered).Sum();
            var Pxaxisdata = new List<string> { "Administered (PRN)", "Not Administered (PRN)", "Attempted (PRN)" };
            var data1 = new List<PieSeriesDataEntity>();
            foreach (string type in Pxaxisdata)
            {
                if (type == "Not Administered (PRN)")
                    data1.Add(new PieSeriesDataEntity { name = type, y = (int)PRNpendingCount - (int)PRNattemptedCount });
                else if (type == "Administered (PRN)")
                    data1.Add(new PieSeriesDataEntity { name = type, y = (int)PRNtotalCount - (int)PRNpendingCount });
                else if (type == "Attempted (PRN)")
                    data1.Add(new PieSeriesDataEntity { name = type, y = (int)PRNattemptedCount });


            }
            var yaxisData = data;
            var PyaxisData = data1; 

            var emarData = new EmarData
            {
                GridData = emarGridData,
                XAxisData = xaxisdata,
                YAxisData = yaxisData,
                PXAxisData = Pxaxisdata,
                PYAxisData = PyaxisData
            };

            return emarData;
        }
        public IList GetEmarResidentBarcodes(string dateValue, int nurseStationId)
        {


            dateValue = dateValue.Replace('-', '/');
            DateTime date = Convert.ToDateTime(dateValue);


            string query = "[Patient].[Prc_BarcodeValidationAlert]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@patient_id", SqlDbType.Int).Value = nurseStationId;
                    cmd.Parameters.Add("@EmarTime", SqlDbType.DateTime).Value = date;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }


                }
            }

            var BarcodeListData = (from d in dt.AsEnumerable()
                                   select new
                                   {

                                       Patient_Id = Convert.ToInt32(d["patient_id"]),
                                       HoldStatus = Convert.ToInt32(d["holdstatus"]),
                                       PQuantity_Id = Convert.ToInt32(d["PQuantity_Id"]),
                                       BarcodeDetails = Convert.ToString(d["barcodedetail"]),
                                       OrderStatus = Convert.ToString(d["OrderStatus"])

                                   }).ToList();


            return BarcodeListData;
        }

        public List<NursingScheduleDropEntity> GetNursingScheduleData(int nurseStationId, string scheduleDate)
        {
            //Insert Recent FacNs
            //var facId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();

            //RecentFacEntity userRecentFacObj = new RecentFacEntity()
            //{
            //    User_Id = 0,
            //    Facility_Id = (int)facId,
            //    NurseStation_Id = nurseStationId.ToString()
            //};
            //this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);

            //scheduleDate = scheduleDate.Replace('-', '/');
            //DateTime dtScheduleDate = Convert.ToDateTime(scheduleDate);
            //var nurseschedule = (from dr in this.dbContext.DrugAdministers
            //                     join co in this.dbContext.CommonOrderInfoes on dr.Porder_Id equals co.POrder_Id
            //                     join vi in this.dbContext.VisitInfoes on co.Patient_Id equals vi.Patient_Id
            //                     join qu in this.dbContext.QuantityDetails on co.POrder_Id equals qu.POrder_Id
            //                     where DbFunctions.TruncateTime(dr.AdminsterSchedule) == DbFunctions.TruncateTime(dtScheduleDate) && vi.NursingStationId == nurseStationId && vi.PVisit_Status == 1 && qu.OrderStatus == 1 && qu.PQuantity_Status == 1 && qu.ReviewFlag != 0 && qu.ReviewFlag != null && dr.NurseShifts_Id == null
            //                     select new
            //                     {
            //                         time = dr.AdminsterSchedule,
            //                         OrderId = co.POrder_Id,
            //                         QuantityId = qu.PQuantity_Id
            //                     }).Distinct().ToList();
            //if (nurseschedule.Count() > 0)
            //{
            //    List<string> scheduleTimes = new List<string>();
            //    var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
            //    int cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)facilityId);

            //    foreach (var item in nurseschedule.OrderBy(t => Convert.ToDateTime(t.time).TimeOfDay))
            //    {
            //        OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == item.QuantityId && oh.OrderHold_Status == 1).FirstOrDefault();
            //        if (orderHold == null)
            //        {
            //            if (cmpTimeFormate == 1)
            //                scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("HH:mm"));
            //            else
            //                scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("hh:mm tt"));
            //        }
            //        else
            //        {
            //            string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            //            string date1 = Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            //            string date2 = Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            //            if (DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            //            {

            //            }
            //            else
            //            {
            //                if (cmpTimeFormate == 1)
            //                    scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("HH:mm"));
            //                else
            //                    scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("hh:mm tt"));
            //            }
            //        }
            //    }

            //    List<NursingScheduleDropEntity> scheduleTimesList = new List<NursingScheduleDropEntity>();
            //    NursingScheduleDropEntity record;
            //    int i = 0;
            //    foreach (var item in scheduleTimes.Distinct())
            //    {
            //        record = new NursingScheduleDropEntity();
            //        record.NursingSchedule_Id = i++;
            //        record.ScheduleTime = item;
            //        scheduleTimesList.Add(record);
            //    }


            //    return scheduleTimesList;
            //}
            //else
            //    return null;
            //var nurseschedule = this.dbContext.NursingSchedules.Where(ns => ns.NursingStation_Id == NurseStationID).ToList();
            //return this.autoMapper.Map<List<NursingSchedule>, List<NursingScheduleEntity>>(nurseschedule);
            scheduleDate = scheduleDate.Replace('-', '/');
            DateTime dtScheduleDate = Convert.ToDateTime(scheduleDate);
            string query = "[dbo].[GetAdminsterScheduleTime]";
            string connectEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@nurseStationId", SqlDbType.Int).Value = nurseStationId;
                    cmd.Parameters.Add("@scheduleDate", SqlDbType.DateTime).Value = dtScheduleDate;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            var scheduleTimesList = (from d in dt.AsEnumerable()
                                     select new NursingScheduleDropEntity
                                     {
                                         ScheduleTime = d["time"].ToString(),
                                     }).ToList();
            return scheduleTimesList;
        }
        //time drop
        public List<NursingScheduleDropEntity> GetNursingScheduleDataByPatientID(int patientId, string scheduleDate, int nursingStationId)
        {
            var nurseStationId = this.dbContext.VisitInfoes.Where(v => v.Patient_Id == patientId).Select(v => v.NursingStationId).FirstOrDefault();
            scheduleDate = scheduleDate.Replace('-', '/');
            List<string> scheduleTimes = new List<string>();
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
            int cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)facilityId);
            DateTime dtScheduleDate = Convert.ToDateTime(scheduleDate);
            var nurseschedule = (from dr in this.dbContext.DrugAdministers
                                 join co in this.dbContext.CommonOrderInfoes on dr.Porder_Id equals co.POrder_Id
                                 join vi in this.dbContext.VisitInfoes on co.Patient_Id equals vi.Patient_Id
                                 join qu in this.dbContext.QuantityDetails on co.POrder_Id equals qu.POrder_Id
                                 where DbFunctions.TruncateTime(dr.AdminsterSchedule) == DbFunctions.TruncateTime(dtScheduleDate) && vi.Patient_Id == patientId && vi.NursingStationId == nurseStationId && qu.PQuantity_Status == 1 && qu.ReviewFlag != 0 && qu.ReviewFlag != null && dr.NurseShifts_Id == null
                                 && (vi.PVisit_Status != 2 || dtScheduleDate <= vi.DischargeDate) && dr.is_deleted == null
                                 select new
                                 {
                                     time = dr.AdminsterSchedule,
                                     OrderId = co.POrder_Id,
                                     QuantityId = qu.PQuantity_Id
                                 }).Distinct().ToList();
            if (nurseschedule.Count() > 0)
            {

                foreach (var item in nurseschedule.OrderBy(t => Convert.ToDateTime(t.time).TimeOfDay))
                {
                    OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == item.QuantityId).FirstOrDefault();
                    if (orderHold == null)
                    {
                        if (cmpTimeFormate == 1)
                            scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("HH:mm"));
                        else
                            scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("hh:mm tt"));
                    }
                    else
                    {
                        string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date1 = Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date2 = Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        if (DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {

                        }
                        else
                        {
                            if (cmpTimeFormate == 1)
                                scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("HH:mm"));
                            else
                                scheduleTimes.Add(Convert.ToDateTime(item.time).ToString("hh:mm tt"));
                        }
                    }
                }
            }

            List<NursingScheduleDropEntity> scheduleTimesList = new List<NursingScheduleDropEntity>();
            NursingScheduleDropEntity record;
            int i = 0;
            if (scheduleTimes.Count > 0)
            {
                foreach (var item in scheduleTimes.Distinct())
                {
                    record = new NursingScheduleDropEntity();
                    record.NursingSchedule_Id = i++;
                    record.NurseShiftTime = item;
                    record.ScheduleTime = i.ToString();
                    scheduleTimesList.Add(record);
                }
            }

            var nurseShifts = (from dr in this.dbContext.DrugAdministers
                               join co in this.dbContext.CommonOrderInfoes on dr.Porder_Id equals co.POrder_Id
                               join vi in this.dbContext.VisitInfoes on co.Patient_Id equals vi.Patient_Id
                               join qu in this.dbContext.QuantityDetails on co.POrder_Id equals qu.POrder_Id
                               where DbFunctions.TruncateTime(dr.AdminsterSchedule) == DbFunctions.TruncateTime(dtScheduleDate) && vi.Patient_Id == patientId && vi.NursingStationId == nursingStationId && vi.PVisit_Status == 1 && qu.OrderStatus == 1 && qu.PQuantity_Status == 1 && qu.ReviewFlag != 0 && qu.ReviewFlag != null && dr.NurseShifts_Id != null
                               && dr.is_deleted == null
                               select new
                               {
                                   NurseShiftId = dr.NurseShifts_Id,
                                   QuantityId = qu.PQuantity_Id
                               }).Distinct().ToList();
            if (nurseShifts.Count() > 0)
            {
                if (cmpTimeFormate == 0)
                {
                    foreach (var shiftsList in nurseShifts)
                    {
                        OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == shiftsList.QuantityId).FirstOrDefault();
                        string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date1 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date2 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        if (orderHold != null && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {

                        }
                        else
                        {
                            var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseShifts_Id == shiftsList.NurseShiftId && nr.NurseStation_Id == nursingStationId && nr.NurseShifts_Status == 1).Select(nr => new { nr.NurseShifts_Id, nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                            var shiftexist = scheduleTimesList.Where(sc => sc.NursingSchedule_Id == shiftsList.NurseShiftId).FirstOrDefault();
                            if (shiftexist == null && shiftHours.Length != 0 && shiftHours.Length > 0)
                            {
                                NursingScheduleDropEntity shiftObj = new NursingScheduleDropEntity();
                                foreach (var hour in shiftHours)
                                {
                                    int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                                    int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                                    if (from != null && to != null)
                                    {
                                        shiftObj = new NursingScheduleDropEntity();
                                        string shift = hour.NurseShifts_Name + " (" + from.RegularTime + from.RegularTimeFormat + " - " + to.RegularTime + to.RegularTimeFormat + ")";
                                        shiftObj.NursingSchedule_Id = hour.NurseShifts_Id;
                                        shiftObj.NurseShiftTime = shift;
                                        shiftObj.ScheduleTime = "s" + hour.NurseShifts_Id;
                                        scheduleTimesList.Add(shiftObj);
                                    }
                                }

                            }
                        }
                    }

                }
                else if (cmpTimeFormate == 1)
                {
                    foreach (var shiftsList in nurseShifts)
                    {
                        OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == shiftsList.QuantityId).FirstOrDefault();
                        string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date1 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date2 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        if (orderHold != null && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {

                        }
                        else
                        {
                            var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseShifts_Id == shiftsList.NurseShiftId && nr.NurseStation_Id == nursingStationId && nr.NurseShifts_Status == 1).Select(nr => new { nr.NurseShifts_Id, nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                            var shiftexist = scheduleTimesList.Where(sc => sc.NursingSchedule_Id == shiftsList.NurseShiftId).FirstOrDefault();
                            if (shiftexist == null && shiftHours.Length != 0 && shiftHours.Length > 0)
                            {
                                NursingScheduleDropEntity shiftObj = new NursingScheduleDropEntity();
                                foreach (var hour in shiftHours)
                                {
                                    int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                                    int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                                    if (from != null && to != null)
                                    {
                                        shiftObj = new NursingScheduleDropEntity();
                                        string shift = hour.NurseShifts_Name + " (" + from.Hour_Desc + " - " + to.Hour_Desc + ")";
                                        shiftObj.NursingSchedule_Id = hour.NurseShifts_Id;
                                        shiftObj.NurseShiftTime = shift;
                                        shiftObj.ScheduleTime = "s" + hour.NurseShifts_Id;
                                        scheduleTimesList.Add(shiftObj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return scheduleTimesList.OrderBy(item => item.NurseShiftTime).ThenBy(item => item.ScheduleTime).ToList();
            //var nurseschedule = this.dbContext.NursingSchedules.Where(ns => ns.NursingStation_Id == NurseStationID).ToList();
            //return this.autoMapper.Map<List<NursingSchedule>, List<NursingScheduleEntity>>(nurseschedule);
        }



        public string GetResidentBiometricInfo(int PatientId)
        {
            var biometricDetails = this.dbContext.Demographics.Where(dm => dm.Patient_Id == PatientId).FirstOrDefault();
            return biometricDetails.BiometricInfo;
        }
        public List<EmarOrdersListEntity> GetEmarOrdersList(int patientId, string time, string dateValue, int shiftId, int window, int userId)
        {
            time = time == "null" ? null : time.Replace("-", ":");
            DateTime? DateValues3 = null;
            if (dateValue == "null")
            {
                DateValues3 = null;
            }
            else
            {
                DateValues3 = Convert.ToDateTime(dateValue);

            }
            int? shift = null;
            if (shiftId != 0)
                shift = shiftId;
            //var ordersList_old = this.dbContext.PrcReportsGetEmarDetails(patientId , DateValues3, time, shift, window, userId).ToList();
            DataTable dt = new DataTable();
            string query = "[Patient].[PrcReportsGetEmarDetails]";
            //string query = "[Patient].[PrcReportsGetEmarDetails_Brad]";
            //string query = "[Patient].[PrcReportsGetEmarDetails]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateValues3;
                    cmd.Parameters.Add("@PassTime", SqlDbType.VarChar).Value = time == null ? (object)DBNull.Value : time;
                    cmd.Parameters.Add("@ShiftId", SqlDbType.Int).Value = shift == null ? (object)DBNull.Value : shift;
                    cmd.Parameters.Add("@window", SqlDbType.Int).Value = window;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    //Commented by Taj on 02202024 to test the executereader performance
                    //using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    //{
                    //    sda.Fill(dt);
                    //}

                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{
                    //    // DataTable dt1 = new DataTable();
                    //    dt.Load(reader);
                    //    // Use dt as needed
                    //}
                    using (SqlDataReader sda = cmd.ExecuteReader())
                    {
                        dt.Load(sda);
                    }
                }
            }
            var ordersList = (from d in dt.AsEnumerable()
                              select new PrcReportsGetEmarDetails
                              {
                                  POrder_Id = Convert.ToInt32(d["POrder_Id"]),
                                  pquantity_Id = Convert.ToInt32(d["pquantity_Id"]),
                                  Drug = d["Drug"].ToString(),
                                  Quantity = d["Quantity"].ToString(),
                                  Route = d["Route"].ToString(),
                                  AdditionalInst = d["AdditionalInst"].ToString(),
                                  Inhand = d["Inhand"].ToString(),
                                  NumberOfRefills = d["NumberOfRefills"].ToString(),
                                  NumberOfRefillsRemaining = d["NumberOfRefillsRemaining"].ToString(),
                                  MaxPerday = Convert.ToDecimal(d["MaxPerday"]),
                                  Diagnosis = d["Diagnosis"].ToString(),
                                  Barcode = d["Barcode"].ToString(),
                                  AlertText = d["AlertText"].ToString(),
                                  Last_ModifiedDate = Convert.ToDateTime(d["Last_ModifiedDate"]),
                                  Last_Modified = d["Last_Modified"].ToString(),
                                  Pass_Time = d["Pass_Time"].ToString(),
                                  Last_Passed = d["Last_Passed"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(d["Last_Passed"]),
                                  Last_PassedBy = d["Last_PassedBy"].ToString(),
                                  AdministerStatus = d["AdministerStatus"].ToString(),
                                  Refill_Request = Convert.ToInt32(d["Refill Request"]),
                                  PsychiatricFlag = Convert.ToInt32(d["PsychiatricFlag"]),
                                  AllergyFlag = Convert.ToInt32(d["AllergyFlag"]),
                                  PRNFlag = d["PRNFlag"].ToString() == "" ? false : Convert.ToBoolean(d["PRNFlag"]),
                                  OrderStockFlag = d["OrderStockFlag"].ToString() == "" ? false : Convert.ToBoolean(d["OrderStockFlag"]),
                                  MedicationReason_Desc = d["MedicationReason_Desc"].ToString(),
                                  ControlSubstanceFlag = Convert.ToInt32(d["ControlSubstanceFlag"]),
                                  PRNAdministered = Convert.ToInt32(d["PRNAdministered"]),
                                  ControlSubstanceCertifiedBy = d["ControlSubstanceCertifiedBy"].ToString(),
                                  ReviewFlag = Convert.ToInt32(d["ReviewFlag"]),
                                  dueflag = Convert.ToInt32(d["dueflag"]),
                                  InputTime = d["InputTime"].ToString(),
                                  ShiftId = d["ShiftId"].ToString() == "" ? 0 : Convert.ToInt32(d["ShiftId"]),
                                  Window = d["Window"].ToString() == "" ? 0 : Convert.ToInt32(d["Window"]),
                                  Ekit = Convert.ToInt32(d["Ekit"]),
                                  Undo = Convert.ToInt32(d["Undo"]),
                                  GPI = d["GPI"].ToString(),
                                  DiscardDays = d["DiscardDays"].ToString() == "" ? 0 : Convert.ToInt32(d["DiscardDays"]),
                                  DiscardDate = d["DiscardDate"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(d["DiscardDate"]),
                                  SideEffectFlag = d["SideEffectFlag"].ToString() == "" ? 0 : Convert.ToInt32(d["SideEffectFlag"]),
                                  AdministerSites = d["AdministerSites"].ToString(),
                                  ABarcode = d["ABarcode"].ToString(),
                                  Refill_Note = d["Refill Note"].ToString(),
                                  DUOM = d["Dose_Desc"].ToString()
                              }).ToList();
            //if (ordersList.Count > 0)
            //{
            //    foreach (var item in ordersList)
            //    {
            //        item.Refill_Note = GetEmarRefillNotesText(item.POrder_Id);
            //    }
            //}
            return this.autoMapper.Map<List<PrcReportsGetEmarDetails>, List<EmarOrdersListEntity>>(ordersList);

        }

        public string GetEmarRefillNotesText(int orderId)
        {

            DataTable dt = new DataTable();
            string query = "[patient].[GetRefillNote]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@POrder_Id", SqlDbType.Int).Value = orderId == null ? (object)DBNull.Value : orderId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            var data = (from d in dt.AsEnumerable()
                        select new RefillDataEntity
                        {

                            Refill_Note = Convert.ToString(d["Refill_Note"])

                        }).FirstOrDefault();



            return data.Refill_Note.ToString();

        }

        public RefillDataEntity GetEmarRefillNotes(int orderId)
        {

            DataTable dt = new DataTable();
            string query = "[patient].[GetRefillNote]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@POrder_Id", SqlDbType.Int).Value = orderId == null ? (object)DBNull.Value : orderId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            var data = (from d in dt.AsEnumerable()
                        select new RefillDataEntity
                        {
                         
                            Refill_Note = Convert.ToString(d["Refill_Note"])

                        }).FirstOrDefault();



            return data;

        }
        public int InsertDrugAdminister(DrugAdministerEntity drugAdministerEntity)
        {
            //Add Window check
            DrugAdminister record = null;
            DrugAdminister prnDrugAdminiter = new DrugAdminister();
            drugAdministerEntity.AdminsterOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (drugAdministerEntity.PRNFlag == true && drugAdministerEntity.UndoFlag == false)
            {
                int dAdmin_id = this.dbContext.DrugAdministrationTimes.Where(d => d.POrder_Id == drugAdministerEntity.POrder_Id && d.PQuantity_Id == drugAdministerEntity.pquantity_Id).Select(d => d.DAdmin_Id).FirstOrDefault();
                prnDrugAdminiter.DAdmin_Id = dAdmin_id;
                prnDrugAdminiter.Porder_Id = drugAdministerEntity.POrder_Id;
                prnDrugAdminiter.PQuantity_Id = drugAdministerEntity.pquantity_Id;
                prnDrugAdminiter.AdminsterSchedule = (DateTime)drugAdministerEntity.AdminsterSchedule;
                prnDrugAdminiter.AdminsterStatus = 1;
                if (drugAdministerEntity.ShiftId != 0 && drugAdministerEntity.ShiftId != null)
                    prnDrugAdminiter.NurseShifts_Id = drugAdministerEntity.ShiftId;
                //    this.dbContext.DrugAdministers.Add(prnDrugAdminiter);
                //this.dbContext.SaveChanges();
                drugAdministerEntity.DrugAdminister_Id = prnDrugAdminiter.DrugAdminister_Id;
                //record = this.dbContext.DrugAdministers.Where(d => d.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id).FirstOrDefault();
                //Commented By Anusha on 11-02-2020
                //drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                //record = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == (int)drugAdministerEntity.POrder_Id && d.PQuantity_Id == drugAdministerEntity.pquantity_Id).FirstOrDefault();
                record = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == (int)drugAdministerEntity.POrder_Id && d.is_deleted == null && d.PQuantity_Id == drugAdministerEntity.pquantity_Id && d.AdminsterSchedule == drugAdministerEntity.AdminsterSchedule).FirstOrDefault();

            }
            //Commented By Anusha on 11-02-2020
            //else if(drugAdministerEntity.PRNFlag == true && drugAdministerEntity.UndoFlag == true && drugAdministerEntity.Last_Passed != null)
            else if (drugAdministerEntity.PRNFlag == true && drugAdministerEntity.UndoFlag == true)
            {
                //var dateTime = drugAdministerEntity.Last_Passed.Replace('-', '/');
                //DateTime lastPassed = Convert.ToDateTime(dateTime);
                //Commented By Anusha on 11-02-2020
                //drugAdministerEntity.DrugAdminister_Id = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == (int)drugAdministerEntity.POrder_Id && d.PQuantity_Id == drugAdministerEntity.pquantity_Id && DbFunctions.TruncateTime(d.AdminsterOn) == DbFunctions.TruncateTime(lastPassed)).Select(d=>d.DrugAdminister_Id).FirstOrDefault();
                if (drugAdministerEntity.Window == 0)
                {


                    drugAdministerEntity.DrugAdminister_Id = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == (int)drugAdministerEntity.POrder_Id && d.PQuantity_Id == drugAdministerEntity.pquantity_Id && d.AdminsterSchedule == drugAdministerEntity.AdminsterSchedule && d.is_deleted == null).Select(d => d.DrugAdminister_Id).FirstOrDefault();
                    record = this.dbContext.DrugAdministers.Where(d => d.DrugAdminister_Id == drugAdministerEntity.DrugAdminister_Id && d.is_deleted == null).FirstOrDefault();

                }
                else
                {
                    drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                    var startTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(-1);
                    var endTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(1);
                    record = (from da in this.dbContext.DrugAdministers
                              where (da.AdminsterSchedule >= startTime && da.AdminsterSchedule <= endTime) && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                              && da.NurseShifts_Id == null && da.is_deleted == null
                              select da).OrderBy(d => d.AdminsterSchedule).FirstOrDefault();
                }
                //if(record !=null)
                //record.AdminsterSchedule = drugAdministerEntity.AdminsterSchedule;
            }

            if ((drugAdministerEntity.ShiftId == 0 || drugAdministerEntity.ShiftId == null) && drugAdministerEntity.InputTime != null)
            {
                if (drugAdministerEntity.Window == 0)
                {
                    drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                    record = (from da in this.dbContext.DrugAdministers
                              where da.AdminsterSchedule == drugAdministerEntity.AdminsterSchedule && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                              && da.NurseShifts_Id == null && da.is_deleted == null
                              select da).FirstOrDefault();

                }
                else if (drugAdministerEntity.Window == 1)
                {
                    if (drugAdministerEntity.PRNFlag == true)
                    {
                        drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                        record = (from da in this.dbContext.DrugAdministers
                                  where da.AdminsterSchedule == drugAdministerEntity.AdminsterSchedule && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                                  && da.NurseShifts_Id == null && da.is_deleted == null
                                  select da).FirstOrDefault();
                        if (record == null)
                        {
                            drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                            var startTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(-1);
                            var endTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(1);
                            record = (from da in this.dbContext.DrugAdministers
                                      where (da.AdminsterSchedule >= startTime && da.AdminsterSchedule <= endTime) && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                                      && da.NurseShifts_Id == null && da.is_deleted == null
                                      select da).OrderBy(d => d.AdminsterSchedule).FirstOrDefault();
                        }
                    }
                    else
                    {
                        drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                        var startTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(-1);
                        var endTime = ((DateTime)drugAdministerEntity.AdminsterSchedule).AddHours(1);
                        record = (from da in this.dbContext.DrugAdministers
                                  where (da.AdminsterSchedule >= startTime && da.AdminsterSchedule <= endTime) && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                                  && da.NurseShifts_Id == null && da.is_deleted == null
                                  select da).OrderBy(d => d.AdminsterSchedule).FirstOrDefault();
                    }
                }
            }
            else if (drugAdministerEntity.ShiftId != 0 && drugAdministerEntity.ShiftId != null && drugAdministerEntity.PRNFlag == false)
            {
                record = (from da in this.dbContext.DrugAdministers
                          where DbFunctions.TruncateTime(da.AdminsterSchedule) == DbFunctions.TruncateTime(drugAdministerEntity.AdminsterSchedule)
                          && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                          && da.NurseShifts_Id == drugAdministerEntity.ShiftId && da.is_deleted == null
                          select da).FirstOrDefault();
            }
            else if (drugAdministerEntity.MedicationReason_ID != null && drugAdministerEntity.AdministerComment != "")
            {
                drugAdministerEntity.AdminsterSchedule = ((DateTime)drugAdministerEntity.AdminsterSchedule).Date.Add(DateTime.Parse(drugAdministerEntity.InputTime).TimeOfDay);
                record = (from da in this.dbContext.DrugAdministers
                          where da.AdminsterSchedule == drugAdministerEntity.AdminsterSchedule && da.Porder_Id == drugAdministerEntity.POrder_Id && da.PQuantity_Id == drugAdministerEntity.pquantity_Id
                          select da).FirstOrDefault();
            }

            if (record != null)
            {
                //var DrugDetails = this.autoMapper.Map<DrugAdministerEntity, DrugAdminister>(drugAdministar);
                // var record = this.dbContext.DrugAdministers.Find(DrugDetails.DrugAdminister_Id);
                record.BCScanner = drugAdministerEntity.BCScanner;
                if (drugAdministerEntity.BCScanner == 1)
                {
                    NurseComment nurseComment = new NurseComment()
                    {
                        DrugAdminister_Id = record.DrugAdminister_Id,
                        NurseCommentType_Id = 4,
                        Comment = drugAdministerEntity.BCScannerText,
                        comment_CreatedBy = drugAdministerEntity.AdminsterBy,
                        Comment_CreatedOn = (DateTime)drugAdministerEntity.AdminsterOn,
                        comment_Status = 1,
                        Comments_Id = 0
                    };
                    this.dbContext.NurseComments.Add(nurseComment);
                    List<string> barcodesList = drugAdministerEntity.AdministeredBarcode.Split('|').ToList();
                    List<int?> stockIds = this.dbContext.BarcodeDetails.Where(s => barcodesList.Contains(s.BarcodeDetail1) && s.Stock_Id != null).Select(s => s.Stock_Id).Distinct().ToList();
                    int patientId = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == drugAdministerEntity.POrder_Id).Select(co => co.Patient_Id).FirstOrDefault();
                    var visitInfo = this.dbContext.VisitInfoes.Where(v => v.Patient_Id == patientId && v.PVisit_Status == 1).FirstOrDefault();
                    if (stockIds.Count > 0)
                    {
                        var stocks = this.dbContext.Stocks.Where(st => stockIds.Contains(st.Stock_Id) && st.InHand != "").Distinct().ToList();
                        if (stocks.Count > 0 && stocks.Where(s => (s.InHand != null ? Convert.ToDouble(s.InHand) : 0) > 0 && ((s.NurseStation_Id != null && s.NurseStation_Id == visitInfo.NursingStationId) || (s.Facility_Id != null && s.Facility_Id == visitInfo.FacilityId))).Count() != 0)
                        {
                            List<int> temp = stocks.Where(s => (s.InHand != null ? Convert.ToDouble(s.InHand) : 0) > 0 && ((s.NurseStation_Id != null && s.NurseStation_Id == visitInfo.NursingStationId && s.Facility_Id == null) || (s.Facility_Id != null && s.Facility_Id == visitInfo.FacilityId && s.NurseStation_Id == null) || (s.NurseStation_Id == visitInfo.NursingStationId && s.Facility_Id == visitInfo.FacilityId))).Select(s => s.Stock_Id).ToList();
                            drugAdministerEntity.AdministeredBarcode = this.dbContext.BarcodeDetails.Where(br => temp.Contains((int)br.Stock_Id)).Select(br => br.BarcodeDetail1).FirstOrDefault();
                        }
                        else
                        {
                            drugAdministerEntity.AdministeredBarcode = barcodesList[0];
                        }

                    }
                    else
                    {
                        List<int?> orderIds = this.dbContext.BarcodeDetails.Where(s => barcodesList.Contains(s.BarcodeDetail1) && s.POrder_Id != null).Select(s => s.POrder_Id).Distinct().ToList();
                        if (orderIds.Count > 0)
                        {
                            var orders = this.dbContext.OrderStocks.Where(st => orderIds.Contains(st.Porder_Id)).Distinct().ToList();
                            if (orders.Count > 0 && orders.Where(s => (s.Inhand != null ? Convert.ToDouble(s.Inhand) : 0) > 0).Count() != 0)
                            {
                                List<int?> temp = orders.Where(s => (s.Inhand != null ? Convert.ToDouble(s.Inhand) : 0) > 0).Select(s => s.Porder_Id).ToList();
                                drugAdministerEntity.AdministeredBarcode = this.dbContext.BarcodeDetails.Where(br => temp.Contains((int)br.POrder_Id)).Select(br => br.BarcodeDetail1).FirstOrDefault();
                            }
                            else
                            {
                                drugAdministerEntity.AdministeredBarcode = barcodesList[0];
                            }
                        }
                    }
                }
                record.Barcode = drugAdministerEntity.AdministeredBarcode;
                record.additionalcomments = drugAdministerEntity.AdditionalComments;
                record.AdminsterStatus = drugAdministerEntity.AdminsterStatus;
                record.AdminsterBy = drugAdministerEntity.AdminsterBy;
                record.AdminsterOn = drugAdministerEntity.AdminsterOn;
                record.MedicationReason_ID = drugAdministerEntity.MedicationReason_ID;
                if (drugAdministerEntity.DrugQuantity.ToString() != "")
                    record.DrugQuantity = Convert.ToDecimal(drugAdministerEntity.DrugQuantity);
                //if (drugAdministerEntity.MedicationReason_ID != 7)
                record.AdministerComment = drugAdministerEntity.AdministerComment;
                //else 
                if (drugAdministerEntity.MedicationReason_ID == 7)
                {
                    //record.AdministerComment = string.Empty;
                    NurseComment nurseComment = new NurseComment()
                    {
                        DrugAdminister_Id = record.DrugAdminister_Id,
                        NurseCommentType_Id = 1,
                        Comment = drugAdministerEntity.AdministerComment,
                        comment_CreatedBy = drugAdministerEntity.AdminsterBy,
                        Comment_CreatedOn = (DateTime)drugAdministerEntity.AdminsterOn,
                        comment_Status = 1,
                        Comments_Id = 0
                    };
                    this.dbContext.NurseComments.Add(nurseComment);
                }
                if (drugAdministerEntity.Ekit_Id != 0 || drugAdministerEntity.Ekit_AllIds.Count > 0)
                {
                    if (drugAdministerEntity.Ekit_AllIds.Count > 0)
                    {

                        var ekitIds = drugAdministerEntity.Ekit_AllIds;
                        foreach (var ekitItem in ekitIds)
                        {
                            var ekit = this.dbContext.EkitAdministers.Where(e => e.DrugAdminister_Id == record.DrugAdminister_Id).FirstOrDefault();
                            EkitAdminister ekitAdminster = new EkitAdminister()
                            {
                                EkitAdminister_Id = 0,
                                Patient_Id = drugAdministerEntity.Patient_Id,
                                DrugAdminister_Id = record.DrugAdminister_Id,
                                Ekit_Id = ekitItem.Ekit_Id,
                                quantity = ekitItem.InHandQty,
                                EkitAdministerBy = drugAdministerEntity.AdminsterBy,
                                EkitAdministerOn = (DateTime)drugAdministerEntity.AdminsterOn,
                            };
                            this.dbContext.EkitAdministers.Add(ekitAdminster);
                            record.EkitAdminister = 1;
                            record.Ekit_Id = ekitItem.Ekit_Id;
                            if (ekitItem.InHandQty.ToString() != "")
                                record.DrugQuantity = Convert.ToDecimal(ekitItem.InHandQty);
                        }

                    }

                }
                //else if(drugAdministerEntity.Ekit_Id == 0 && drugAdministerEntity.UndoFlag)
                //{

                //}
                else if (drugAdministerEntity.AdminsterStatus == 1 && record.EkitAdminister != null && (drugAdministerEntity.Ekit_Id == 0 || drugAdministerEntity.Ekit_Id == null))
                {
                    record.EkitAdminister = 0;
                    record.Ekit_Id = null;
                }
                record.BiometricBypass = drugAdministerEntity.ByPassReason == "" ? 1 : 2;
                record.BiometricBypassReason = drugAdministerEntity.ByPassReason != "" ? drugAdministerEntity.ByPassReason : null;
                record.ManualDocumentedBy = null;
                this.dbContext.SaveChanges();
                //Discard Date Insert/Update
                if (drugAdministerEntity.DiscardDate != null && record.DAdmin_Id != null)
                {
                    var checkExist = this.dbContext.tblDrugDiscardInfoes.Where(td => td.DAdmin_Id == record.DAdmin_Id).FirstOrDefault();
                    if (checkExist == null)
                    {
                        tblDrugDiscardInfo discardObj = new tblDrugDiscardInfo();
                        discardObj.DAdmin_Id = record.DAdmin_Id;
                        discardObj.DiscardDate = drugAdministerEntity.DiscardDate;
                        discardObj.CreatedDate = (DateTime)drugAdministerEntity.AdminsterOn;
                        discardObj.CreatedBy = drugAdministerEntity.AdminsterBy;
                        this.dbContext.tblDrugDiscardInfoes.Add(discardObj);
                        this.dbContext.SaveChanges();
                    }
                    else
                    {
                        checkExist.DiscardDate = drugAdministerEntity.DiscardDate;
                        checkExist.CreatedDate = (DateTime)drugAdministerEntity.AdminsterOn;
                        checkExist.CreatedBy = drugAdministerEntity.AdminsterBy;
                        this.dbContext.SaveChanges();
                    }
                }
                //Insulin Sites
                if (drugAdministerEntity.UndoFlag == true)
                {
                    var insulinRecords = this.dbContext.tblAdministeredSites.Where(t => t.DrugAdminister_Id == record.DrugAdminister_Id).ToList();
                    if (insulinRecords.Count() > 0)
                    {
                        this.dbContext.tblAdministeredSites.RemoveRange(insulinRecords);
                        this.dbContext.SaveChanges();
                    }
                }
                if (record.DAdmin_Id != null && drugAdministerEntity.AdministerInsulinSites != null && drugAdministerEntity.AdministerInsulinSites != "null" && drugAdministerEntity.AdministerInsulinSites != "")
                {
                    List<string> siteId = drugAdministerEntity.AdministerInsulinSites.Split(',').ToList();
                    if (siteId.Count() > 0)
                    {
                        foreach (var id in siteId)
                        {
                            int insulinSite = Convert.ToInt32(id);
                            var checkRecord = this.dbContext.tblAdministeredSites.Where(t => t.DrugAdminister_Id == record.DrugAdminister_Id && t.Site_Id == insulinSite).FirstOrDefault();
                            if (checkRecord == null)
                            {
                                tblAdministeredSite siteObj = new tblAdministeredSite();
                                siteObj.DrugAdminister_Id = record.DrugAdminister_Id;
                                siteObj.DAdmin_Id = record.DAdmin_Id;
                                siteObj.Route = drugAdministerEntity.RouteCode;
                                siteObj.Site_Id = insulinSite;
                                siteObj.AdministeredDate = (DateTime)record.AdminsterSchedule;
                                siteObj.CreatedDate = (DateTime)drugAdministerEntity.AdminsterOn;
                                siteObj.CreatedBy = drugAdministerEntity.AdminsterBy;
                                this.dbContext.tblAdministeredSites.Add(siteObj);
                                this.dbContext.SaveChanges();
                            }
                            else
                            {
                                checkRecord.DrugAdminister_Id = record.DrugAdminister_Id;
                                checkRecord.DAdmin_Id = record.DAdmin_Id;
                                checkRecord.Route = drugAdministerEntity.RouteCode;
                                checkRecord.Site_Id = insulinSite;
                                checkRecord.AdministeredDate = (DateTime)record.AdminsterSchedule;
                                checkRecord.CreatedDate = (DateTime)drugAdministerEntity.AdminsterOn;
                                checkRecord.CreatedBy = drugAdministerEntity.AdminsterBy;
                                this.dbContext.SaveChanges();
                            }
                        }
                    }
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.EMAR,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = record.DAdmin_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };
                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;
            }
            return 0;
        }

        public int InsertNursingFrequencyConfig(NursingFrequencyConfigCustomEntity frequencyconfig)
        {
            if (frequencyconfig.NursingFreq_Id != 0)
            {
                string[] nsIds = frequencyconfig.NursingStations.Split(',');
                if (frequencyconfig.OldFrequencyId != frequencyconfig.Frequency_Id || frequencyconfig.Facility_Id != frequencyconfig.Facility_Id || !nsIds.Contains(frequencyconfig.OldNursingStationId.ToString()))
                {
                    NursingFrequencyConfig data = this.dbContext.NursingFrequencyConfigs.Where(n => n.Frequency_Id == frequencyconfig.OldFrequencyId && n.Facility_Id == frequencyconfig.OldFacilityId && n.NursingStation_Id == frequencyconfig.OldNursingStationId).FirstOrDefault();
                    NursingFCTime timeRecord = this.dbContext.NursingFCTimes.Where(n => n.NursingFreq_Id == data.NursingFreq_Id).FirstOrDefault();
                    if (timeRecord != null)
                    {
                        this.dbContext.NursingFCTimes.Remove(timeRecord);
                    }
                    if (data != null)
                    {
                        this.dbContext.NursingFrequencyConfigs.Remove(data);
                    }
                    this.dbContext.SaveChanges();
                }
            }

            int record = this.dbContext.PrcInsertUpdateFreqMapping(frequencyconfig.Frequency_Id, frequencyconfig.Facility_Id, frequencyconfig.NursingStations, frequencyconfig.Times, frequencyconfig.Hours, frequencyconfig.Monday == true ? 1 : 0, frequencyconfig.Tuesday == true ? 1 : 0, frequencyconfig.Wednesday == true ? 1 : 0, frequencyconfig.Thursday == true ? 1 : 0, frequencyconfig.Friday == true ? 1 : 0, frequencyconfig.Saturday == true ? 1 : 0, frequencyconfig.Sunday == true ? 1 : 0, frequencyconfig.Week_Id, frequencyconfig.Month_Id, frequencyconfig.ActiveDays, frequencyconfig.HoldDays, frequencyconfig.NursingFreq_CreatedBy);

            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.FrequencyMapping,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = frequencyconfig.NursingFreq_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (frequencyconfig.NursingStations != "")
                {
                    //var selectedNurseStationsSave = string.Join(",", frequencyconfig.NursingStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = (int)frequencyconfig.Facility_Id;
                    userRecentFacObj.NurseStation_Id = frequencyconfig.NursingStations;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public List<PrcGetFreqMappingData_ResultEntity> GetNursingFrequencyConfigsData(int userId)
        {
            var records = this.dbContext.PrcGetFreqMappingData(userId).OrderBy(item => item.FacilityName).ThenBy(item => item.NurseStationName).ThenBy(item => item.Frequency).ToList();
            return this.autoMapper.Map<List<PrcGetFreqMappingData_Result>, List<PrcGetFreqMappingData_ResultEntity>>(records);
        }
        public NursingFrequencyConfigCustomEntity GetNursingFrequencyDetailsByID(int nursingFreqId)
        {
            List<HourEntity> list = new List<HourEntity>();
            HourEntity data = new HourEntity();
            var configs = (from n in this.dbContext.NursingFrequencyConfigs
                           join fa in this.dbContext.Facilities on n.Facility_Id equals fa.Facility_Id
                           join ns in this.dbContext.NursingStations on n.NursingStation_Id equals ns.NurseStation_Id
                           join u in this.dbContext.Users on n.NursingFreq_CreatedBy equals u.User_Id
                           join fr in this.dbContext.FrequencyMasters on n.Frequency_Id equals fr.Frequency_Id
                           where n.NursingFreq_Id == nursingFreqId
                           select new NursingFrequencyConfigCustomEntity
                           {
                               NursingFreq_Id = n.NursingFreq_Id,
                               NursingStation_Id = (int)n.NursingStation_Id,
                               Facility_Id = n.Facility_Id,
                               Frequency_Id = (int)n.Frequency_Id,
                               //Hours = n.Hours,
                               Hours = fr.Freq_Times,
                               Monday = n.Monday,
                               Tuesday = n.Tuesday,
                               Wednesday = n.Wednesday,
                               Thursday = n.Thursday,
                               Friday = n.Friday,
                               Saturday = n.Saturday,
                               Sunday = n.Sunday,
                               Week_Id = n.Week_Id,
                               Month_Id = n.Month_Id,
                               OnlyOnDay = n.OnlyOnDay,
                               ThroughDay = n.ThroughDay,
                               ActiveDays = n.ActiveDays,
                               HoldDays = n.HoldDays,
                               NursingFreq_Status = n.NursingFreq_Status,
                               NursingFreq_CreatedDate = n.NursingFreq_CreatedDate,
                               NursingStation_Name = ns.NurseStation_Name,
                               Frequency_Name = fr.Frequency_Shortname,
                           }).FirstOrDefault();
            if (configs != null)
            {
                int cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)configs.Facility_Id);
                if (cmpTimeFormate == 0)
                {
                    var hourIds = this.dbContext.NursingFCTimes.Where(nf => nf.NursingFreq_Id == configs.NursingFreq_Id).ToList();
                    if (hourIds.Count > 0)
                    {
                        foreach (var item in hourIds)
                        {
                            data = new HourEntity();
                            int hourId = (int)item.hour_Id;
                            var time = this.dbContext.Hours.Where(h => h.Hour_Id == hourId).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                            data.Hour_Id = hourId;
                            data.Hour_Desc = time.RegularTime + " " + time.RegularTimeFormat;
                            list.Add(data);
                        }
                    }
                    configs.HoursList = list.OrderBy(item => item.Hour_Id).ToList();
                }
                if (cmpTimeFormate == 1)
                {
                    var hourIds = this.dbContext.NursingFCTimes.Where(nf => nf.NursingFreq_Id == configs.NursingFreq_Id).ToList();
                    if (hourIds.Count > 0)
                    {
                        foreach (var item in hourIds)
                        {
                            data = new HourEntity();
                            int hourId = (int)item.hour_Id;
                            var time = this.dbContext.Hours.Where(h => h.Hour_Id == hourId).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                            data.Hour_Id = hourId;
                            data.Hour_Desc = time.Hour_Desc;
                            list.Add(data);
                        }
                        configs.HoursList = list.OrderBy(item => item.Hour_Id).ToList();
                    }
                }
            }
            return configs;
        }
        public List<MedicationReasonEntity> GetMedicationReason()
        {
            var reason = this.dbContext.MedicationReasons.Where(item => item.MedicationReason_ID != 1).ToList();
             return this.autoMapper.Map<List<MedicationReason>, List<MedicationReasonEntity>>(reason);

        }
        public List<PRNDetailsCustomEntity> GetPRNData(PRNFilterCustomEntity configs)
        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (configs.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", configs.Facilities);
            if (configs.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", configs.NurseStations);
            if (configs.Floors.Count() > 0)
                selectedFloors = string.Join(",", configs.Floors);
            if (configs.Wings.Count() > 0)
                selectedWings = string.Join(",", configs.Wings);
            if (configs.Rooms.Count() > 0)
                selectedRooms = string.Join(",", configs.Rooms);
            if (configs.Beds.Count() > 0)
                selectedBeds = string.Join(",", configs.Beds);

            var records = this.dbContext.PrcPRNDrugAdministerSchedule(configs.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds).Select(e => new PRNDetailsCustomEntity
            {

                PatientName = e.ResidentName,
                MedicationReason_Desc = e.MedicationReason_Desc,
                GiveCodeText = e.GiveCodeText,
                AdminsterBy = e.AdminsterBy,
                AdministerComment = e.AdministerComment,
                AdminsterOn = e.AdminsterOn,
                Patient_Id = e.Patient_Id,
                POrder_Id = (int)e.Porder_Id,
                DrugAdminister_Id = (int)e.DrugAdminister_Id


            }).OrderBy(item => item.PatientName).ToList();
            return records;
        }
        public List<SeventyTwoHourCheckEntity> GetSeventyTwoHourCheckDetails(SeventyTwoHourCustomEntity configs)

        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (configs.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", configs.Facilities);
            if (configs.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", configs.NurseStations);
            if (configs.Floors.Count() > 0)
                selectedFloors = string.Join(",", configs.Floors);
            if (configs.Wings.Count() > 0)
                selectedWings = string.Join(",", configs.Wings);
            if (configs.Rooms.Count() > 0)
                selectedRooms = string.Join(",", configs.Rooms);
            if (configs.Beds.Count() > 0)
                selectedBeds = string.Join(",", configs.Beds);

            var records = this.dbContext.PrcFirstDrugAdministerSchedule(configs.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds).Select(e => new SeventyTwoHourCheckEntity
            {
                ResidentName = e.ResidentName,
                Porder_Id = Convert.ToInt32(e.Porder_Id),
                DrugAdminister_Id = Convert.ToInt32(e.DrugAdministerId),
                Patient_Id = e.Patient_Id,
                AdminsterSchedule = e.AdminsterSchedule,
                AdminsterOn = e.AdminsterOn,
                GiveCodeText = e.GiveCodeText,
                AdministerOn = e.AdminsterOn,
                PatientFirstName = e.ResidentName,
                PatientLastName = e.ResidentName,


            }).OrderBy(item => item.ResidentName).ThenBy(item => item.GiveCodeText).ToList();


            return records;

        }
        public int InsertPRNData(List<PRNInsertCustomEntity> prnData)
        {
            foreach (var item in prnData)
            {
                item.PRNCommentOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                DrugAdminister record = this.dbContext.DrugAdministers.Find(item.DrugAdminister_Id);
                if (record != null)
                {
                    record.DrugAdminister_Id = Convert.ToInt32(item.DrugAdminister_Id);
                    record.PRNComment = item.PRNComment;
                    record.PRNCommentBy = item.PRNCommentBy;
                    record.PRNCommentOn = item.PRNCommentOn;
                    this.dbContext.SaveChanges();
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.PRNDocumentation,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertUpdateSeventyTwoHourChecks(List<SeventyTwoHourInsertEntity> data)
        {

            foreach (var item in data)
            {
                item.SeventyTwoCommentOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                DrugAdminister record = this.dbContext.DrugAdministers.Find(item.DrugAdminister_Id);


                if (record != null)
                {
                    record.DrugAdminister_Id = item.DrugAdminister_Id;
                    record.SeventyTwoComment = item.SeventyTwoComment;
                    record.SeventyTwoCommentBy = item.SeventyTwoCommentBy;
                    record.SeventyTwoCommentOn = item.SeventyTwoCommentOn;
                    this.dbContext.SaveChanges();
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.SeventyTwoHourChecks,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public List<SeventyTwoHourCheckEntity> GetSeventyTwoHourDetailsByfilter(SeventyTwoHourCustomEntity configs)
        {
            //PrcFirstDrugAdministerSchedule_Result
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (configs.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", configs.Facilities);
            if (configs.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", configs.NurseStations);
            if (configs.Floors.Count() > 0)
                selectedFloors = string.Join(",", configs.Floors);
            if (configs.Wings.Count() > 0)
                selectedWings = string.Join(",", configs.Wings);
            if (configs.Rooms.Count() > 0)
                selectedRooms = string.Join(",", configs.Rooms);
            if (configs.Beds.Count() > 0)
                selectedBeds = string.Join(",", configs.Beds);
            List<int> facilityIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                     where us.User_Id == configs.User_Id && us.UserRole_Status == 1
                                     select us.Facility_id).Distinct().ToList();


            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (configs.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", configs.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = configs.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }

            var q1 = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId)).ToList();
            var q2 = q1;
            if (configs.NurseStations.Count != 0)
            {
                q2 = q1.Where(p => configs.NurseStations.Contains(Convert.ToInt32(p.NursingStationId))).ToList();
            }
            var q3 = q2;
            if (configs.Floors.Count != 0)
            {
                q3 = q2.Where(p => configs.Floors.Contains(Convert.ToInt32(p.Floor))).ToList();
            }
            var q4 = q3;
            if (configs.Wings.Count != 0)
            {
                q4 = q3.Where(w => configs.Wings.Contains(Convert.ToInt32(w.Wing))).ToList();
            }
            var q5 = q4;
            if (configs.Rooms.Count != 0)
            {
                q5 = q4.Where(p => configs.Rooms.Contains(Convert.ToInt32(p.Room))).ToList();
            }
            var q6 = q5;
            if (configs.Beds.Count != 0)
            {
                q6 = q5.Where(p => configs.Beds.Contains(Convert.ToInt32(p.Bed))).ToList();
            }

            if (q6.Count() > 0)
            {

                var patientIds = q6.Select(p => p.Patient_Id);

                var records = this.dbContext.PrcFirstDrugAdministerSchedule(configs.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds).Where(p => patientIds.Contains(p.Patient_Id)).Select(e => new SeventyTwoHourCheckEntity
                {
                    ResidentName = e.ResidentName,
                    Porder_Id = Convert.ToInt32(e.Porder_Id),
                    DrugAdminister_Id = Convert.ToInt32(e.DrugAdministerId),
                    Patient_Id = e.Patient_Id,
                    AdminsterSchedule = e.AdminsterSchedule,
                    AdminsterOn = e.AdminsterOn,
                    GiveCodeText = e.GiveCodeText,
                    AdministerOn = e.AdminsterOn,
                    PatientFirstName = e.ResidentName,
                    PatientLastName = e.ResidentName,


                }).OrderBy(item => item.ResidentName).ToList();

                return records;
            }

            return null;
        }

        public List<PRNDetailsCustomEntity> GetPRNDetailsByfilter(PRNFilterCustomEntity configs)
        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (configs.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", configs.Facilities);
            if (configs.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", configs.NurseStations);
            if (configs.Floors.Count() > 0)
                selectedFloors = string.Join(",", configs.Floors);
            if (configs.Wings.Count() > 0)
                selectedWings = string.Join(",", configs.Wings);
            if (configs.Rooms.Count() > 0)
                selectedRooms = string.Join(",", configs.Rooms);
            if (configs.Beds.Count() > 0)
                selectedBeds = string.Join(",", configs.Beds);
            List<int> facilityIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                     where us.User_Id == configs.User_Id && us.UserRole_Status == 1
                                     select us.Facility_id).Distinct().ToList();


            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (configs.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", configs.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = configs.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }


            var q1 = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId)).ToList();
            var q2 = q1;
            if (configs.NurseStations.Count != 0)
            {
                q2 = q1.Where(p => configs.NurseStations.Contains(Convert.ToInt32(p.NursingStationId))).ToList();
            }
            var q3 = q2;
            if (configs.Floors.Count != 0)
            {
                q3 = q2.Where(p => configs.Floors.Contains(Convert.ToInt32(p.Floor))).ToList();
            }
            var q4 = q3;
            if (configs.Wings.Count != 0)
            {
                q4 = q3.Where(w => configs.Wings.Contains(Convert.ToInt32(w.Wing))).ToList();
            }
            var q5 = q4;
            if (configs.Rooms.Count != 0)
            {
                q5 = q4.Where(p => configs.Rooms.Contains(Convert.ToInt32(p.Room))).ToList();
            }
            var q6 = q5;
            if (configs.Beds.Count != 0)
            {
                q6 = q5.Where(p => configs.Beds.Contains(Convert.ToInt32(p.Bed))).ToList();
            }

            if (q6.Count() > 0)
            {

                var patientIds = q6.Select(p => p.Patient_Id);

                var records = this.dbContext.PrcPRNDrugAdministerSchedule(configs.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds).Where(p => patientIds.Contains(p.Patient_Id)).Select(e => new PRNDetailsCustomEntity
                {
                    PatientName = e.ResidentName,
                    MedicationReason_Desc = e.MedicationReason_Desc,
                    GiveCodeText = e.GiveCodeText,
                    AdminsterBy = e.AdminsterBy,
                    AdministerComment = e.AdministerComment,
                    AdminsterOn = e.AdminsterOn,
                    Patient_Id = e.Patient_Id,
                    POrder_Id = Convert.ToInt32(e.Porder_Id),
                    DrugAdminister_Id = Convert.ToInt32(e.DrugAdminister_Id)



                }).OrderBy(item => item.PatientName).ToList();


                return records;
            }

            return null;
        }
        //Remove - Not needed
        public int ByPassBiometric(BypassBiometricEntity byPass)
        {
            //var timeStamp = this.dbContext.NursingSchedules.Find(byPass.Time);
            //string date = DateTime.Now.ToShortDateString() + " " + timeStamp.ScheduleTime;
            string date = byPass.dateValue + " " + byPass.Time.Replace("-", ":");//timeStamp.ScheduleTime;
            DateTime dt1 = Convert.ToDateTime(date).AddHours(-1);
            DateTime dt2 = Convert.ToDateTime(date).AddHours(1);
            var ordersList = (from da in this.dbContext.DrugAdministers
                              join co in this.dbContext.CommonOrderInfoes on da.Porder_Id equals co.POrder_Id
                              join dm in this.dbContext.Demographics on co.Patient_Id equals dm.Patient_Id
                              join en in this.dbContext.EncodedOrderDetails on co.POrder_Id equals en.POrder_Id
                              join qd in this.dbContext.QuantityDetails on co.POrder_Id equals qd.POrder_Id
                              where da.AdminsterSchedule <= dt2 && da.AdminsterSchedule >= dt1 && dm.Patient_Id == byPass.PatientID && da.is_deleted == null
                              select new
                              {
                                  da.DrugAdminister_Id
                              }).ToList();
            //if (ordersList.Count() > 0)
            //{
            //    for (int i = 0; i < ordersList.Count; i++)
            //    {
            //        var record = this.dbContext.DrugAdministers.Find(ordersList[i].DrugAdminister_Id);
            //        record.BiometricBypass = byPass.byPass;
            //        record.BiometricBypassReason = byPass.reason;
            //        this.dbContext.SaveChanges();
            //    }
            //}
            return 1;
        }
        public int InsertAdministredwithoutscanning(NurseCommentsEntity nurseobj)
        {
            //ToDo: Anitha - change this
            var data = this.autoMapper.Map<NurseCommentsEntity, NurseComment>(nurseobj);
            var comments = this.dbContext.NurseComments.Where(c => c.DrugAdminister_Id == nurseobj.DrugAdminister_Id).FirstOrDefault();
            if (comments == null)
            {
                this.dbContext.NurseComments.Add(data);
                this.dbContext.SaveChanges();
            }
            else
            {
                NurseComment records = this.dbContext.NurseComments.Find(comments.DrugAdminister_Id);
                records.DrugAdminister_Id = data.DrugAdminister_Id;
                records.NurseCommentType_Id = data.NurseCommentType_Id;
                records.Comment = data.Comment;
                records.comment_Status = data.comment_Status;
                records.comment_CreatedBy = data.comment_CreatedBy;
                records.Comment_CreatedOn = data.Comment_CreatedOn;
                this.dbContext.SaveChanges();

            }
            return 1;
        }
        public List<VitalsCheckEntity> GetVitalsCheckList(int PQuantityId)
        {
            var records = (from orderfav in this.dbContext.OrderFavourites
                           join favmaster in this.dbContext.OrderFavouriteMasters on orderfav.OrderFavMaster_ID equals favmaster.OrderFavMaster_ID
                           where orderfav.PQuantity_Id == PQuantityId
                           select new VitalsCheckEntity
                           {
                               OrderFavMaster_ID = favmaster.OrderFavMaster_ID,
                               OrderFavDesc = favmaster.OrderFavDesc,
                               PQuantity_Id = PQuantityId
                           }).ToList();
            return records;

        }
        public List<VitalsCheckEntity> GetVitalsChecksListbyQuantityIds(string PQuantityIds)
        {
            List<string> ids = PQuantityIds.Split(',').ToList();
            var records = (from orderfav in this.dbContext.OrderFavourites
                           join favmaster in this.dbContext.OrderFavouriteMasters on orderfav.OrderFavMaster_ID equals favmaster.OrderFavMaster_ID
                           where ids.Contains(orderfav.PQuantity_Id.ToString())
                           select new VitalsCheckEntity
                           {
                               OrderFavMaster_ID = favmaster.OrderFavMaster_ID,
                               OrderFavDesc = favmaster.OrderFavDesc,
                               PQuantity_Id = orderfav.PQuantity_Id
                           }).ToList();
            return records;

        }
        public int InsertOrderFavouritesData(List<OrderFavouriteDataEntity> entity)
        {
            if (entity.Count() > 0)
            {
                var orderId = entity[0].POrder_Id;
                var quantityId = entity[0].pquantity_Id;
                var inputTime = entity[0].InputTime;
                var shiftId = entity[0].ShiftId;
                var window = entity[0].Window;
                var drugAdminsterSchedule = entity[0].AdminsterSchedule;
                var dateTime = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());

                DrugAdminister record = null;

                if ((shiftId == 0 || shiftId == null) && inputTime != null)
                {
                    drugAdminsterSchedule = ((DateTime)drugAdminsterSchedule).Date.Add(DateTime.Parse(inputTime).TimeOfDay);
                    record = (from da in this.dbContext.DrugAdministers
                              where da.AdminsterSchedule == drugAdminsterSchedule && da.Porder_Id == orderId && da.PQuantity_Id == quantityId
                              && da.NurseShifts_Id == null && da.is_deleted == null
                              select da).FirstOrDefault();

                }
                else if (shiftId != 0 && shiftId != null)
                {
                    record = (from da in this.dbContext.DrugAdministers
                              where da.AdminsterSchedule == drugAdminsterSchedule && da.Porder_Id == orderId && da.PQuantity_Id == quantityId
                              && da.NurseShifts_Id == shiftId && da.is_deleted == null
                              select da).FirstOrDefault();
                }
                if (record != null)
                {
                    foreach (OrderFavouriteDataEntity item in entity)
                    {
                        var favRecord = this.dbContext.OrderFavouriteDatas.Where(of => of.DrugAdminister_Id == record.DrugAdminister_Id && of.OrderFavMaster_ID == item.OrderFavMaster_ID).FirstOrDefault();
                        if (favRecord == null)
                        {
                            favRecord = new OrderFavouriteData()
                            {
                                DrugAdminister_Id = record.DrugAdminister_Id,
                                OrderFavMaster_ID = item.OrderFavMaster_ID,
                                value = item.value,
                                FavouriteData_Status = item.FavouriteData_Status,
                                FavouriteData_CreatedBy = item.FavouriteData_CreatedBy,
                                Favourite_CreatedOn = dateTime //item.Favourite_CreatedOn
                            };
                            this.dbContext.OrderFavouriteDatas.Add(favRecord);
                            this.dbContext.SaveChanges();
                        }
                        else
                        {
                            favRecord.value = item.value;
                            favRecord.FavouriteData_Status = item.FavouriteData_Status;
                            favRecord.FavouriteData_CreatedBy = item.FavouriteData_CreatedBy;
                            favRecord.Favourite_CreatedOn = dateTime;//item.Favourite_CreatedOn;
                            this.dbContext.SaveChanges();
                        }
                    }
                    return 1;
                }
            }
            return 0;
        }
        public int GetDueMARAlertData(int userId)
        {
            int? i = this.dbContext.PrcDueMARAlert(userId).FirstOrDefault();
            return (int)i;

        }
        public List<EkitDropEntity> GetEkitDropData(int nursestaionId, int orderId, int userId)
        {
            string gpiCode = this.dbContext.EncodedOrderDetails.Where(c => c.POrder_Id == orderId).Select(c => c.AGiveCodeIdentifier).FirstOrDefault();
            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nursestaionId).Select(n => n.Facility_Id).FirstOrDefault();
            List<Ekit> ekits = this.dbContext.Ekits.AsEnumerable().Where(c => c.Facility_Id == facilityId && c.NurseStation_Id == nursestaionId && c.GPICode == gpiCode && c.Ekit_Status == 1 && c.InHand != null && c.InHand != "" && Convert.ToDecimal(c.InHand) != 0 && (c.ControlSubstance != 1 || (c.ControlSubstance == 1 && c.CertifiedBy != null && c.CheckInFlag != 1))).ToList();
            if (ekits.Count == 0)
            {
                ekits.AddRange(this.dbContext.Ekits.AsEnumerable().Where(c => c.Facility_Id == facilityId && c.NurseStation_Id != nursestaionId && c.GPICode == gpiCode && c.Ekit_Status == 1 && c.Sharedekitbit == 1 && c.InHand != null && c.InHand != "" && Convert.ToDecimal(c.InHand) != 0 && (c.ControlSubstance != 1 || (c.ControlSubstance == 1 && c.CertifiedBy != null && c.CheckInFlag != 1))).ToList());
            }
            List<Ekit> ekitDrop = new List<Ekit>();
            if (ekits.Count() > 0)
            {
                foreach (var item in ekits)
                {
                    if (item.ControlSubstance == 1)
                    {
                        var ekitCertified = this.dbContext.EKitControlSubstanceTrans.Where(e => e.Ekit_Id == item.Ekit_Id).Select(e => e.CertifiedBy).ToList();
                        if (ekitCertified.Contains(userId) == true)
                        {
                            ekitDrop.Add(item);
                        }
                    }
                    else
                    {
                        ekitDrop.Add(item);
                    }
                }
            }
            return this.autoMapper.Map<List<Ekit>, List<EkitDropEntity>>(ekitDrop);
        }
        public List<NurseShiftsEntity> GetNurseShiftDrop(int nursestationId, string scheduleDate)
        {
            List<NurseShiftsEntity> shiftTime = new List<NurseShiftsEntity>();
            NurseShiftsEntity shiftObj = new NurseShiftsEntity();
            int cmpTimeFormate = 0;

            scheduleDate = scheduleDate.Replace('-', '/');
            DateTime dtScheduleDate = Convert.ToDateTime(scheduleDate);
            var nurseShifts = (from dr in this.dbContext.DrugAdministers
                               join co in this.dbContext.CommonOrderInfoes on dr.Porder_Id equals co.POrder_Id
                               join vi in this.dbContext.VisitInfoes on co.Patient_Id equals vi.Patient_Id
                               join qu in this.dbContext.QuantityDetails on co.POrder_Id equals qu.POrder_Id
                               where DbFunctions.TruncateTime(dr.AdminsterSchedule) == DbFunctions.TruncateTime(dtScheduleDate) && vi.NursingStationId == nursestationId && vi.PVisit_Status == 1 && qu.OrderStatus == 1 && qu.PQuantity_Status == 1 && qu.ReviewFlag != 0 && qu.ReviewFlag != null && dr.NurseShifts_Id != null && dr.is_deleted == null
                               select new
                               {
                                   NurseShiftId = dr.NurseShifts_Id,
                                   QuantityId = qu.PQuantity_Id
                               }).Distinct().ToList();
            if (nurseShifts.Count() > 0)
            {
                var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nursestationId).Select(n => n.Facility_Id).FirstOrDefault();
                cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)facilityId);

                if (cmpTimeFormate == 0)
                {
                    foreach (var shiftsList in nurseShifts)
                    {
                        OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == shiftsList.QuantityId).FirstOrDefault();
                        string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date1 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date2 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        if (orderHold != null && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {

                        }
                        else
                        {
                            var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseShifts_Id == shiftsList.NurseShiftId && nr.NurseStation_Id == nursestationId && nr.NurseShifts_Status == 1).Select(nr => new { nr.NurseShifts_Id, nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                            var shiftexist = shiftTime.Where(sc => sc.NurseShifts_Id == shiftsList.NurseShiftId).FirstOrDefault();
                            if (shiftexist == null && shiftHours.Length != 0 && shiftHours.Length > 0)
                            {
                                foreach (var hour in shiftHours)
                                {
                                    int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                                    int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                                    if (from != null && to != null)
                                    {
                                        shiftObj = new NurseShiftsEntity();
                                        string shift = hour.NurseShifts_Name + " (" + from.RegularTime + from.RegularTimeFormat + " - " + to.RegularTime + to.RegularTimeFormat + ")";
                                        shiftObj.NurseShifts_Id = hour.NurseShifts_Id;
                                        shiftObj.NurseShifts_Name = shift;
                                        shiftTime.Add(shiftObj);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (cmpTimeFormate == 1)
                {
                    foreach (var shiftsList in nurseShifts)
                    {
                        OrderHold orderHold = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == shiftsList.QuantityId).FirstOrDefault();
                        string date = dtScheduleDate.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date1 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldFrom).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        string date2 = orderHold == null ? "" : Convert.ToDateTime(orderHold.HoldTo).ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        if (orderHold != null && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {

                        }
                        else
                        {
                            var shiftHours = this.dbContext.NurseShifts.Where(nr => nr.NurseShifts_Id == shiftsList.NurseShiftId && nr.NurseStation_Id == nursestationId && nr.NurseShifts_Status == 1).Select(nr => new { nr.NurseShifts_Id, nr.NurseShifts_Name, nr.Fromtime_hoursId, nr.Totime_hoursId }).ToArray();
                            var shiftexist = shiftTime.Where(sc => sc.NurseShifts_Id == shiftsList.NurseShiftId).FirstOrDefault();
                            if (shiftexist == null && shiftHours.Length != 0 && shiftHours.Length > 0)
                            {
                                foreach (var hour in shiftHours)
                                {
                                    int fromHour = hour.Fromtime_hoursId == null ? 0 : (int)hour.Fromtime_hoursId;
                                    int toHour = hour.Totime_hoursId == null ? 0 : (int)hour.Totime_hoursId;
                                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                                    if (from != null && to != null)
                                    {
                                        shiftObj = new NurseShiftsEntity();
                                        string shift = hour.NurseShifts_Name + " (" + from.Hour_Desc + " - " + to.Hour_Desc + ")";
                                        shiftObj.NurseShifts_Id = hour.NurseShifts_Id;
                                        shiftObj.NurseShifts_Name = shift;
                                        shiftTime.Add(shiftObj);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return shiftTime.OrderBy(item => item.NurseShifts_Id).ToList();
        }
        public List<ControlSubstanceTransEntity> GetControlSubstanceTrans(int quantityId)
        {
            var orderId = this.dbContext.QuantityDetails.Where(q => q.PQuantity_Id == quantityId).Select(q => q.POrder_Id).FirstOrDefault();
            var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == orderId).Select(c => c.Patient_Id).FirstOrDefault();
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var records = (from ct in this.dbContext.ControlSubstanceTrans
                           join cs in this.dbContext.ControlSubstanceCounts on ct.ControlSubstance_Id equals cs.ControlSubstance_Id
                           join r in this.dbContext.ControlSubstanceReasons on ct.TransCS_Id equals r.ControlSubstance_Id into res
                           from r in res.DefaultIfEmpty()
                           join en in this.dbContext.EncodedOrderDetails on cs.Porder_Id equals en.POrder_Id
                           join us in this.dbContext.Users on ct.CertifiedBy equals us.User_Id
                           where cs.PQuantity_Id == quantityId
                           select new //ControlSubstanceTransEntity
                           {
                               controlTrans = ct,
                               encoded = en,
                               user = us,
                               reason = r,
                               //Order = en.GiveCodeText,
                               //LastCertified = us.UserName,
                               //CertifiedDate = ct.CertifiedDate,
                               //InitialQuantity = ct.Quantity,
                               //Reason = r.Reason,
                           }).OrderByDescending(item => item.controlTrans.CertifiedDate).ToList()
                           .Select(x => new ControlSubstanceTransEntity
                           {
                               Order = x.encoded.GiveCodeText,
                               LastCertified = x.user.UserName,
                               CertifiedDate = GetTimeZoneDateTime(x.controlTrans.CertifiedDate, companyId),
                               InitialQuantity = x.controlTrans.Quantity,
                               Reason = x.reason != null ? x.reason.Reason : "",
                           }).ToList();
            return records;
        }
        public List<ControlSubstanceTransEntity> GetEkitControlSubstanceTrans(int ekitId)
        {
            var facId = this.dbContext.Ekits.Where(e => e.Ekit_Id == ekitId).Select(e => e.Facility_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == facId).Select(f => f.Facility_Id).FirstOrDefault();
            var records = (from et in this.dbContext.EKitControlSubstanceTrans
                           join ek in this.dbContext.Ekits on et.Ekit_Id equals ek.Ekit_Id
                           join r in this.dbContext.EkitControlSubstanceReasons on et.EkTransCS_Id equals r.Ekit_Id into res
                           from r in res.DefaultIfEmpty()
                           join us in this.dbContext.Users on et.CertifiedBy equals us.User_Id
                           where ek.Ekit_Id == ekitId
                           select new //ControlSubstanceTransEntity
                           {
                               ekcontrolTrans = et,
                               ekit = ek,
                               user = us,
                               reason = r,
                           }).OrderByDescending(item => item.ekcontrolTrans.CertifiedDate).ToList()
                           .Select(x => new ControlSubstanceTransEntity
                           {
                               Order = x.ekit.DrugName,
                               LastCertified = x.user.UserName,
                               CertifiedDate = GetTimeZoneDateTime(x.ekcontrolTrans.CertifiedDate, companyId),
                               InitialQuantity = x.ekcontrolTrans.Quantity,
                               Reason = x.reason != null ? x.reason.Reason : "",
                           }).ToList();
            return records;
        }
        public int GetPRNAdministerCountByDate(int orderId, int quantityd, string dateValue)
        {

            var Date = Convert.ToDateTime(dateValue.Substring(0, 10));
            int count = this.dbContext.DrugAdministers.Where(d => d.Porder_Id == orderId && d.PQuantity_Id == quantityd && d.is_deleted == null && EntityFunctions.TruncateTime(d.AdminsterOn) == Date && d.AdminsterStatus != 0).ToList().Count();
            return count;
        }
        public string GetInsulinCommentsByQuantityId(int quantityId)
        {
            string insulinComments = this.dbContext.QuantityDetails.Where(q => q.PQuantity_Id == quantityId).Select(q => q.InsulinComments).FirstOrDefault();
            return insulinComments;
        }
        public int InsertUpdateNurseComments(NurseCommentsEntity notes)
        {
            notes.Comment_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<NurseCommentsEntity, NurseComment>(notes);
            var check = this.dbContext.NurseComments.Where(dr => dr.Comments_Id == notes.Comments_Id).FirstOrDefault();
            if (check == null)
            {
                record.DrugAdminister_Id = null;
                record.PQuantity_Id = null;
                this.dbContext.NurseComments.Add(record);
                this.dbContext.SaveChanges();
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = record.Comments_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;

        }
        public List<NurseCommentsEntity> GetNurseComments(int patientId)
        {

            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var records = (from nc in this.dbContext.NurseComments
                           join u in this.dbContext.Users on nc.comment_CreatedBy equals u.User_Id
                           where nc.Patient_Id == patientId
                           select new //NurseCommentsEntity
                           {
                               comments = nc,
                               user = u,
                               //Comment = nc.Comment,
                               //UserName = u.UserName,
                               //Comment_CreatedOn = GetTimeZoneDateTime(nc.Comment_CreatedOn,companyId)
                           }).OrderByDescending(item => item.comments.Comment_CreatedOn).ToList()
                           .Select(x => new NurseCommentsEntity
                           {
                               Comment = x.comments.Comment,
                               UserName = x.user.UserName,
                               Comment_CreatedOn = GetTimeZoneDateTime(x.comments.Comment_CreatedOn, companyId)
                           }).ToList();
            return records;
        }
        public string GetSideEffectsByGPICode(string GPICode)
        {
            var records = this.dbContext.tblSideEffects.Where(s => s.GPI == GPICode).Select(s => s.SideEffects).FirstOrDefault();
            return records;

        }
        public string AdministerOdersVitalsInfo(VitalsEntity obj)
        {
            List<string> Ids = obj.AdminsIds.Split(',').ToList();
            List<string> DIds = new List<string>();
            List<DrugAdminister> data = this.dbContext.DrugAdministers.Where(s => Ids.Contains(s.DrugAdminister_Id.ToString()) && s.is_deleted == null).Distinct().ToList();
            foreach (var item in data)
            {
                List<int> favIds = this.dbContext.OrderFavourites.Where(p => p.PQuantity_Id == item.PQuantity_Id).Select(o => o.OrderFavMaster_ID).ToList();
                List<int> id = obj.checkedVitals.Select(s => s.IdValue).ToList();
                List<checkedList> lst = obj.checkedVitals.Where(s => favIds.Contains(s.IdValue)).ToList();
                var allOfList1IsInList2 = favIds.Intersect(id).Count() == favIds.Count();

                if (allOfList1IsInList2 == true && favIds.Count > 0)
                {
                    foreach (var ids in lst)
                    {
                        var favRecord = this.dbContext.OrderFavouriteDatas.Where(of => of.DrugAdminister_Id == item.DrugAdminister_Id && of.OrderFavMaster_ID == ids.IdValue).FirstOrDefault();

                        if (favRecord == null)
                        {
                            favRecord = new OrderFavouriteData()
                            {
                                DrugAdminister_Id = item.DrugAdminister_Id,
                                OrderFavMaster_ID = ids.IdValue,
                                value = ids.value,
                                FavouriteData_Status = 1,
                                FavouriteData_CreatedBy = obj.CreatedBy,
                                Favourite_CreatedOn = obj.CreatedDate
                            };
                            this.dbContext.OrderFavouriteDatas.Add(favRecord);
                            this.dbContext.SaveChanges();
                        }
                        else
                        {
                            favRecord.value = ids.value;
                            favRecord.FavouriteData_Status = 1;
                            favRecord.FavouriteData_CreatedBy = obj.CreatedBy;
                            favRecord.Favourite_CreatedOn = obj.CreatedDate;
                            this.dbContext.SaveChanges();
                        }

                    }
                    DIds.Add(item.PQuantity_Id.ToString());
                }
                if (favIds.Count() == 0)
                {
                    DIds.Add(item.PQuantity_Id.ToString());
                }
            }
            string result = string.Join(",", DIds);
            return result;
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
            var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(dateTime, companyId).FirstOrDefault());
            return convetedDate;
        }
        public int GetDate2hrsDiff(TimezoneEntity obj)
        {

            var dateValue = obj.LastPassedDate.Replace('-', '/');
            DateTime date = Convert.ToDateTime(dateValue);


            string query = "[Admin].[PrcGetDate2hrsDiff]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@facilityID", SqlDbType.Int).Value = obj.FacilityId;
                    cmd.Parameters.Add("@lastdatemodified", SqlDbType.DateTime).Value = date;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }


                }
            }

            var result = (from d in dt.AsEnumerable()
                          select d["AlertMsg"]).FirstOrDefault();

            return (int)result;
        }
    }
}
