using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;
using System.Data.Entity;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web;
using System.Security.Claims;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Data.Entity.Core.Objects;
using log4net.Core;

namespace LTCPro.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmarRepository _emarRepository;
        private readonly ICommonRepository _commonRepository;

        public OrdersRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IUserActivityRepository userActivityRepository, FacilityRepository facilityRepository, CompanyRepository companyRepository, EmarRepository emarRepository, CommonRepository commonRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            _userActivityRepository = userActivityRepository;
            this._facilityRepository = facilityRepository;
            this._companyRepository = companyRepository;
            this._emarRepository = emarRepository;
            this._commonRepository = commonRepository;

        }
        public List<OrdersGridEntity> GetOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (orderFilter.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", orderFilter.Facilities);
            if (orderFilter.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", orderFilter.NurseStations);
            if (orderFilter.Floors.Count() > 0)
                selectedFloors = string.Join(",", orderFilter.Floors);
            if (orderFilter.Wings.Count() > 0)
                selectedWings = string.Join(",", orderFilter.Wings);
            if (orderFilter.Rooms.Count() > 0)
                selectedRooms = string.Join(",", orderFilter.Rooms);
            if (orderFilter.Beds.Count() > 0)
                selectedBeds = string.Join(",", orderFilter.Beds);

            var records = this.dbContext.PrcGetOrderGrid(orderFilter.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds, orderFilter.VisitStatus).ToList();

            var data =
              records.Select(item => new OrdersGridEntity()
              {
                  Patient_Id = item.Patient_Id,
                  porder_Id = item.porder_Id,
                  PQuantity_Id = item.PQuantity_Id,
                  Date = item.Date,
                  ResidentName = item.ResidentName,
                  Drug = item.Drug,
                  Dosage_Form = item.Dosage_Form,
                  Quantity = item.Quantity,
                  Directions = item.Directions,
                  Physician_Name = item.Physician_Name,
                  POrder_Status = item.POrder_Status == 3 ? "" : item.POrder_Status == 1 ? "Completed" : "Pending",
                  ScheduleTimeFlag = item.scheduleTimeflag,
                  SplitFlag = item.split,
                  //DOB = Convert.ToDateTime(item.DOB).ToString("MM/dd/yyyy"),
                  DOB = item.DOB,
                  Gender = item.Gender != "" ? item.Gender == "M" ? "Male" : item.Gender == "F" ? "Female" : "Unknown" : "",
                  //Livee
                  //ImageLocation = CheckResImageinBiometricGrid(this.dbContext.Demographics.Where(p => p.Patient_Id == item.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault()) == "" ? ConvertImage(item.ImageLocation) : ConvertBiometricImage(CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == item.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault())),
                  //ImgPath = CheckResImageinBiometricGrid(this.dbContext.Demographics.Where(p => p.Patient_Id == item.Patient_Id).Select(p => p.PatientMRNumber).FirstOrDefault()),
                  ImgPath = item.ImageLocation != null ? "Yes" : "No",
                  ImageLocation = null,
                  OrderStatus = item.PQuantity_Id == 0 ? 0 : (records.Where(re => re.Patient_Id == item.Patient_Id && re.Order_Status == 1).Count() > 0 ? 1 : 2),
                  EndDate = item.EndDate,
                  PRN = item.PRN,
                  HoldUntill = item.On_Hold_Until,
              }).OrderBy(item => item.ResidentName).ThenBy(item => item.Drug).Distinct().ToList();
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (orderFilter.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", orderFilter.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = orderFilter.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            List<OrdersGridEntity> allOrderRecords1 = data.GroupBy(m => new { m.Patient_Id }).ToList().Select(U => U.OrderByDescending(U1 => U1.OrderStatus).FirstOrDefault()).ToList();
            // List<OrdersGridEntity> allOrderRecords = data.GroupBy(m => new { m.Patient_Id }).Select(group => group.FirstOrDefault()).ToList();
            return allOrderRecords1;
            //return this.autoMapper.Map<List<PrcGetOrderGrid_Result>, List<OrdersGridEntity>>(records);
        }
        /*  public OrderDetailsEntity GetOrderDetails(Int64 OrderID, int patientID)
          {

              OrderDetailsEntity ordersDetails = new OrderDetailsEntity();
              DateTime? nullDate = null;
              if (OrderID != 0)
              {
                  ordersDetails = (from dm in this.dbContext.Demographics
                                   join vs in this.dbContext.VisitInfoes on dm.Patient_Id equals vs.Patient_Id
                                   join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                                   join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id into join1
                                   from j1 in join1.DefaultIfEmpty()
                                   join qd in this.dbContext.QuantityDetails on cm.POrder_Id equals qd.POrder_Id into join2
                                   from j2 in join2.DefaultIfEmpty()
                                   join tr in this.dbContext.TreatmentRouteInfoes on cm.POrder_Id equals tr.POrder_Id into join3
                                   from j3 in join3.DefaultIfEmpty()
                                   join cs in this.dbContext.ControlSubstanceCounts on cm.POrder_Id equals cs.Porder_Id into join4
                                   from j4 in join4.DefaultIfEmpty()
                                   where cm.POrder_Id == OrderID
                                   select new OrderDetailsEntity
                                   {
                                       ResName = dm.PatientLastName + " " + dm.PatientFirstName,
                                       patientID = dm.Patient_Id,
                                       DOB = (DateTime)dm.DOB,
                                       AdmitDate = (DateTime)vs.AdmitDate,
                                       DischargeDate = vs.DischargeDate == null ? "" : ((DateTime)vs.DischargeDate).ToString(),
                                       PhyName = cm.OrderingPhysicianID.ToString(),//cm.OPhysicianLname + " " + cm.OPhysicianFname,
                                       Drug = cm.TreatmentFlag == true ? (this.dbContext.TreatmentInfoes.Where(ti => ti.POrder_Id == cm.POrder_Id).Select(ti => ti.RequestedGiveCode)).FirstOrDefault() : j1.GiveCodeText,
                                       Quantity = j2.Quantity,
                                       RouteCode = j3.RouteCode,
                                       AdditionalInst = j1.ProviderAdminDrugInsText,
                                       NumberofRefill = j1.NumberOfRefills,
                                       Inhand = j4.Quantity != null ? j4.Quantity : j1.NumberOfRefillsRemaining,
                                       Diagnosis = string.Empty,
                                       StartDate = j2.StartDate == null ? nullDate : (DateTime)j2.StartDate,
                                       EndDate = j2.EndDate == null ? nullDate : (DateTime)j2.EndDate,
                                       AlertText = cm.AlertText,
                                       MaxPerDay = (cm.MaxPerdays == null ? 0 : cm.MaxPerdays),
                                       Prn = (cm.PRNFlag == null ? false : cm.PRNFlag),
                                       Self = (cm.SelfAdministeredFlag == null ? false : cm.SelfAdministeredFlag),
                                       Treatment = (cm.TreatmentFlag == null ? false : cm.TreatmentFlag),
                                       MaySubtitude = (cm.Maysubstitute == null ? false : cm.Maysubstitute),
                                       OrderStock = (cm.OrderStockFlag == null ? false : cm.OrderStockFlag),
                                       ControlsubFlag = j4.Quantity != null ? true : false
                                   }).FirstOrDefault();

              }
              else
              {
                  ordersDetails = (from dm in this.dbContext.Demographics
                                   join vs in this.dbContext.VisitInfoes on dm.Patient_Id equals vs.Patient_Id
                                   join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id into join4
                                   from j4 in join4.DefaultIfEmpty()
                                   join en in this.dbContext.EncodedOrderDetails on j4.POrder_Id equals en.POrder_Id into join1
                                   from j1 in join1.DefaultIfEmpty()
                                   join qd in this.dbContext.QuantityDetails on j4.POrder_Id equals qd.POrder_Id into join2
                                   from j2 in join2.DefaultIfEmpty()
                                   join tr in this.dbContext.TreatmentRouteInfoes on j4.POrder_Id equals tr.POrder_Id into join3
                                   from j3 in join3.DefaultIfEmpty()
                                   where dm.Patient_Id == patientID
                                   select new OrderDetailsEntity
                                   {
                                       ResName = dm.PatientLastName + " " + dm.PatientFirstName,
                                       patientID = dm.Patient_Id,
                                       DOB = (DateTime)dm.DOB,
                                       AdmitDate = (DateTime)vs.AdmitDate,
                                       PhyName = j4.OPhysicianLname + " " + j4.OPhysicianFname,
                                       Drug = j1.GiveCodeText,
                                       Quantity = j2.Quantity,
                                       RouteCode = j3.RouteCode,
                                       AdditionalInst = j1.ProviderAdminDrugInsText,
                                       NumberofRefill = j1.NumberOfRefills,
                                       Inhand = j1.NumberOfRefillsRemaining,
                                       Diagnosis = string.Empty,
                                       StartDate = j2.StartDate == null ? nullDate : (DateTime)j2.StartDate,
                                       EndDate = j2.EndDate != null ? (DateTime)j2.EndDate : nullDate
                                   }).FirstOrDefault();
              }

              return ordersDetails;
          }
          public List<OrdersGridCustomEntity> GetAllOrdersByPatient(Int64 PatientID)
          {
              //var commonOrders = this.dbContext.CommonOrderInfoes.Where(cm => cm.Patient_Id == PatientID).ToList();
              //List<int> OrderIDs = new List<int>();
              //commonOrders.ForEach(co => {
              //    OrderIDs.Add(co.POrder_Id);
              //});
              //var encodedList = this.dbContext.EncodedOrderDetails.Where(en => OrderIDs.Contains(en.POrder_Id)).ToList();
              //return this.autoMapper.Map<List<EncodedOrderDetail>, List<EncodedOrderDetailEntity>>(encodedList);
              var commonOrders = (from cm in this.dbContext.CommonOrderInfoes
                                  join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                                  where cm.Patient_Id == PatientID
                                  select new OrdersGridCustomEntity
                                  {
                                      POrder_Id = cm.POrder_Id,
                                      GiveCodeText = en.GiveCodeText,
                                      GiveDosageForm = en.GiveDosageForm,
                                      ProviderAdminDrugInsText = en.ProviderAdminDrugInsText,
                                      OrderControl = cm.OrderControl,
                                      OrderStockFlag = cm.OrderStockFlag == false ? "Stock" : "Order"
                                  }).ToList();
              return commonOrders;
          }
          */
        //public List<PhysicianDropEntity> GetPhysicianDropData(int? facilityId = null, int? nurseStatioId = null, int? orderId = null)
        //{
        //    int? userID = facilityId;
        //    var nurseStation = this.dbContext.NursingStations.Where(ns => ns.NurseStation_Id == nurseStatioId).FirstOrDefault();
        //    if (nurseStation != null)
        //        facilityId = nurseStation.Facility_Id;
        //    var records = this.dbContext.PrcGetPhysicianDetailsatorder(nurseStatioId, facilityId, orderId, userID);
        //    var physicianData = (from phy in records
        //                         select new PhysicianDropEntity
        //                         {
        //                             Physician_Id = phy.Physician_Id.Value,
        //                             PhysicianNPI = phy.PhysicianNPI,
        //                             PhysicianLName = phy.PhysicianLName,
        //                             PhysicianFName = phy.PhysicianFName,
        //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
        //                         }).OrderBy(item => item.PhysicianLName).ToList();
        //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
        //    return physicianRecords;
        //    //if (facilityId == null && nurseStatioId == null)
        //    //{
        //    //    var physicianData = (from phy in this.dbContext.PhysicianDetails
        //    //                         where phy.Physician_Status == 1
        //    //                         select new PhysicianDropEntity
        //    //                         {
        //    //                             Physician_Id = phy.Physician_Id,
        //    //                             PhysicianNPI = phy.PhysicianNPI,
        //    //                             PhysicianLName = phy.PhysicianLName,
        //    //                             PhysicianFName = phy.PhysicianFName,
        //    //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
        //    //                         }).OrderBy(item => item.PhysicianLName).ToList();
        //    //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
        //    //    return physicianRecords;
        //    //}
        //    //else if (nurseStatioId == null)
        //    //{
        //    //    var physicianData = (from phy in this.dbContext.PhysicianDetails
        //    //                         where phy.Physician_Status == 1 && phy.Facility_Id == facilityId
        //    //                         select new PhysicianDropEntity
        //    //                         {
        //    //                             Physician_Id = phy.Physician_Id,
        //    //                             PhysicianNPI = phy.PhysicianNPI,
        //    //                             PhysicianLName = phy.PhysicianLName,
        //    //                             PhysicianFName = phy.PhysicianFName,
        //    //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
        //    //                         }).OrderBy(item => item.PhysicianLName).ToList();

        //    //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
        //    //    return physicianRecords;
        //    //}
        //    //else
        //    //{
        //    //    var physicianData = (from phy in this.dbContext.PhysicianDetails
        //    //                         where phy.Physician_Status == 1 && phy.NurseStation_Id == nurseStatioId || phy.Facility_Id == facilityId
        //    //                         select new PhysicianDropEntity
        //    //                         {
        //    //                             Physician_Id = phy.Physician_Id,
        //    //                             PhysicianNPI = phy.PhysicianNPI,
        //    //                             PhysicianLName = phy.PhysicianLName,
        //    //                             PhysicianFName = phy.PhysicianFName,
        //    //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
        //    //                         }).OrderBy(item => item.PhysicianLName).ToList();

        //    //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
        //    //    return physicianRecords;
        //    //}
        //}

        public List<PhysicianDropEntity> GetPhysicianDropData(int? facilityId = null, int? nurseStatioId = null, int? orderId = null)
        {
            int? userID = facilityId;
            var nurseStation = this.dbContext.NursingStations.Where(ns => ns.NurseStation_Id == nurseStatioId).FirstOrDefault();
            if (nurseStation != null)
                facilityId = nurseStation.Facility_Id;
            // var records = this.dbContext.PrcGetPhysicianDetailsatorder(nurseStatioId, facilityId, orderId, userID);



            DataTable dt = new DataTable();
            string query = "[Patient].[PrcGetPhysicianDetailsatorder]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@NurseStation_Id", SqlDbType.Int).Value = nurseStatioId == null ? (object)DBNull.Value : nurseStatioId;
                    cmd.Parameters.Add("@Facility_Id", SqlDbType.Int).Value = facilityId == null ? (object)DBNull.Value : facilityId;
                    cmd.Parameters.Add("@POrder_Id", SqlDbType.BigInt).Value = orderId == null ? (object)DBNull.Value : orderId;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID == null ? (object)DBNull.Value : userID;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }



            var physicianData = (from d in dt.AsEnumerable()
                                 select new PhysicianDropEntity
                                 {
                                     Physician_Id = Convert.ToInt32(d["Physician_Id"]),
                                     PhysicianNPI = d["PhysicianNPI"].ToString(),
                                     PhysicianLName = d["PhysicianLName"].ToString(),
                                     PhysicianFName = d["PhysicianFName"].ToString(),
                                     PhysicianFullName = d["PhysicianLName"].ToString() + ", " + d["PhysicianFName"].ToString(),
                                     // Credentials = phy.Credentials != null ? 1 : 0,
                                     Credentials = Convert.ToInt32(d["Credentials"]),
                                     PStatus = Convert.ToInt32(d["PhysicianStatus"]),
                                     SPhy = d["SupervisingPhyNPI"].ToString(),
                                     SPhyStatus = Convert.ToInt32(d["SupervisingPhyStatus"]),
                                     SphyName = d["SupervisingPhyName"].ToString(),
                                     CredeValue = d["CredeValue"].ToString()
                                 }).OrderBy(item => item.PhysicianLName).ToList();
            List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
            return physicianRecords;
            //if (facilityId == null && nurseStatioId == null)
            //{
            //    var physicianData = (from phy in this.dbContext.PhysicianDetails
            //                         where phy.Physician_Status == 1
            //                         select new PhysicianDropEntity
            //                         {
            //                             Physician_Id = phy.Physician_Id,
            //                             PhysicianNPI = phy.PhysicianNPI,
            //                             PhysicianLName = phy.PhysicianLName,
            //                             PhysicianFName = phy.PhysicianFName,
            //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
            //                         }).OrderBy(item => item.PhysicianLName).ToList();
            //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
            //    return physicianRecords;
            //}
            //else if (nurseStatioId == null)
            //{
            //    var physicianData = (from phy in this.dbContext.PhysicianDetails
            //                         where phy.Physician_Status == 1 && phy.Facility_Id == facilityId
            //                         select new PhysicianDropEntity
            //                         {
            //                             Physician_Id = phy.Physician_Id,
            //                             PhysicianNPI = phy.PhysicianNPI,
            //                             PhysicianLName = phy.PhysicianLName,
            //                             PhysicianFName = phy.PhysicianFName,
            //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
            //                         }).OrderBy(item => item.PhysicianLName).ToList();

            //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
            //    return physicianRecords;
            //}
            //else
            //{
            //    var physicianData = (from phy in this.dbContext.PhysicianDetails
            //                         where phy.Physician_Status == 1 && phy.NurseStation_Id == nurseStatioId || phy.Facility_Id == facilityId
            //                         select new PhysicianDropEntity
            //                         {
            //                             Physician_Id = phy.Physician_Id,
            //                             PhysicianNPI = phy.PhysicianNPI,
            //                             PhysicianLName = phy.PhysicianLName,
            //                             PhysicianFName = phy.PhysicianFName,
            //                             PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
            //                         }).OrderBy(item => item.PhysicianLName).ToList();

            //    List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
            //    return physicianRecords;
            //}
        }
        public int ControlTypeUpdate(OrderControlUpdateEntity orderControl)
        {
            var orderDetail = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == orderControl.OrderID).FirstOrDefault();
            orderDetail.OrderControl = orderControl.ControlType;
            orderDetail.POOutBoundFileStatus = 1;
            this.dbContext.SaveChanges();
            return 1;
        }
        public List<FrequencyMasterEntity> GetFrequencyMasterData()
        {
            var records = (from fr in this.dbContext.FrequencyMasters
                           where fr.Frequency_Status == 1 && fr.Frequency_Shortname != "QShift"
                           select new FrequencyMasterEntity
                           {
                               Frequency_Id = fr.Frequency_Id,
                               Frequency_Shortname = fr.Frequency_Shortname,
                               Frequency_PRN = fr.Frequency_PRN,
                               Frequency_Name = fr.Frequency_Description == null ? fr.Frequency_Shortname : fr.Frequency_Shortname + " - " + fr.Frequency_Description,
                               Freq_Times = fr.Freq_Times,
                               Freq_Descalert = fr.Freq_Descalert == null ? "" : fr.Freq_Descalert,
                               Freq_Descalert1 = fr.Freq_Descalert1 == null ? "" : fr.Freq_Descalert1
                           }).OrderBy(item => item.Frequency_Shortname).ToList();
            return records;
        }
        public List<FrequencyMasterEntityWithShifts> GetFrequencyMasterDataWithShifts(int stationId)
        {
            var records = (from fr in this.dbContext.FrequencyMasters
                           where fr.Frequency_Status == 1
                           select new FrequencyMasterEntityWithShifts
                           {
                               Frequency_Id = fr.Frequency_Id.ToString(),
                               Frequency_PRN = fr.Frequency_PRN,
                               Freq_Group = fr.Freq_Group,
                               Frequency_Name = fr.Frequency_Description == null ? fr.Frequency_Shortname : fr.Frequency_Shortname + " - " + fr.Frequency_Description,
                               Freq_Times = fr.Freq_Times,
                               Freq_Descalert = fr.Freq_Descalert == null ? "" : fr.Freq_Descalert,
                               Freq_Descalert1 = fr.Freq_Descalert1 == null ? "" : fr.Freq_Descalert1
                           }).OrderBy(item => item.Frequency_Name).ToList();
            var nsShifts = (from ns in this.dbContext.NurseShifts
                            where ns.NurseStation_Id == stationId && ns.NurseShifts_Status == 1
                            select ns).ToList();
            if (nsShifts.Count() > 0)
            {
                var FacilityID = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == stationId).Select(n => n.Facility_Id).FirstOrDefault();
                var cmpTimeFormat = this._facilityRepository.GetCompanyTimeFormat((int)FacilityID);

                if (cmpTimeFormat == 0)
                {
                    foreach (var item in nsShifts)
                    {
                        int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                        int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                        var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                        var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                        if (from != null && to != null)
                        {
                            string shiftTime = item.NurseShifts_Name + " (" + from.RegularTime + from.RegularTimeFormat + " - " + to.RegularTime + to.RegularTimeFormat + ")";
                            records.Add(new FrequencyMasterEntityWithShifts()
                            {
                                Frequency_Id = "s" + item.NurseShifts_Id,
                                Frequency_Name = shiftTime
                            });
                        }
                    }
                }
                else if (cmpTimeFormat == 1)
                {
                    foreach (var item in nsShifts)
                    {
                        int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                        int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                        var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                        var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                        if (from != null && to != null)
                        {

                            string shiftTime = item.NurseShifts_Name + " (" + from.Hour_Desc + " - " + to.Hour_Desc + ")";
                            records.Add(new FrequencyMasterEntityWithShifts()
                            {
                                Frequency_Id = "s" + item.NurseShifts_Id,
                                Frequency_Name = shiftTime
                            });
                        }
                    }

                }
            }
            else
            {
                //Don't show QShift
                var item = records.Where(f => f.Frequency_Id == "28").FirstOrDefault();
                records.Remove(item);
            }
            return records;
        }
        public List<WeekEntity> GetWeekMasterData()
        {
            var weekData = this.dbContext.Weeks.Where(wk => wk.Week_Status == 1).ToList();
            return this.autoMapper.Map<List<Week>, List<WeekEntity>>(weekData);
        }
        public List<MonthEntity> GetMonthMasterData()
        {
            var monthData = this.dbContext.Months.Where(mn => mn.Month_Status == 1).OrderByDescending(item => item.Month_Id == 13).ToList();
            return this.autoMapper.Map<List<Month>, List<MonthEntity>>(monthData);
        }
        public List<HourEntity> GetHoursDataByNSId(int nurseStationId, int facilityId)
        {
            List<HourEntity> hours = new List<HourEntity>();
            int cmpTimeFormate = 0;
            if (facilityId == 0 && nurseStationId == 0)
            {
                return null;
            }
            else
            {
                if (facilityId == 0)
                {
                    var FacilityID = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();
                    cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)FacilityID);
                }
                else if (nurseStationId == 0)
                {
                    cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)facilityId);
                }
                if (cmpTimeFormate == 1)
                {
                    hours = (from h in this.dbContext.Hours
                             where h.Hour_Status == 1
                             orderby h.Hour_Desc
                             select new HourEntity
                             {
                                 Hour_Id = h.Hour_Id,
                                 Hour_Desc = h.Hour_Desc,
                             }).ToList();
                }
                else if (cmpTimeFormate == 0)
                {
                    hours = (from h in this.dbContext.Hours
                             where h.Hour_Status == 1
                             orderby h.Hour_Desc
                             select new HourEntity
                             {
                                 Hour_Id = h.Hour_Id,
                                 Hour_Desc = h.RegularTime + " " + h.RegularTimeFormat,
                             }).ToList();
                }
            }
            return hours;
        }
        public List<CustomPassShiftTimeEntity> GetPasstimeShiftsData(string nurseStationIds, int facilityId)
        {
            List<string> nsIds = nurseStationIds.Split(',').ToList();
            List<CustomPassShiftTimeEntity> shiftTimes = new List<CustomPassShiftTimeEntity>();
            var cmpTimeFormate = this._facilityRepository.GetCompanyTimeFormat((int)facilityId);
            var nsShifts = (from ns in this.dbContext.NurseShifts
                            where nsIds.Contains(ns.NurseStation_Id.ToString()) && ns.NurseShifts_Status == 1
                            select ns).ToList();
            if (cmpTimeFormate == 1)
            {
                shiftTimes = (from h in this.dbContext.Hours
                              where h.Hour_Status == 1
                              orderby h.Hour_Desc
                              select new CustomPassShiftTimeEntity
                              {
                                  Hour_Id = h.Hour_Id.ToString(),
                                  Hour_Desc = h.Hour_Desc,
                              }).ToList();
                foreach (var item in nsShifts)
                {
                    int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                    int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                    if (from != null && to != null)
                    {

                        string shiftTime = item.NurseShifts_Name + " (" + from.Hour_Desc + " - " + to.Hour_Desc + ")";
                        shiftTimes.Add(new CustomPassShiftTimeEntity()
                        {
                            Hour_Id = "s" + item.NurseShifts_Id,
                            Hour_Desc = shiftTime
                        });
                    }
                }
            }
            else if (cmpTimeFormate == 0)
            {
                shiftTimes = (from h in this.dbContext.Hours
                              where h.Hour_Status == 1
                              orderby h.Hour_Desc
                              select new CustomPassShiftTimeEntity
                              {
                                  Hour_Id = h.Hour_Id.ToString(),
                                  Hour_Desc = h.RegularTime + " " + h.RegularTimeFormat,
                              }).ToList();
                foreach (var item in nsShifts)
                {
                    int fromHour = item.Fromtime_hoursId == null ? 0 : (int)item.Fromtime_hoursId;
                    int toHour = item.Totime_hoursId == null ? 0 : (int)item.Totime_hoursId;
                    var from = this.dbContext.Hours.Where(h => h.Hour_Id == fromHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                    var to = this.dbContext.Hours.Where(h => h.Hour_Id == toHour).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                    if (from != null && to != null)
                    {
                        string shiftTime = item.NurseShifts_Name + " (" + from.RegularTime + from.RegularTimeFormat + " - " + to.RegularTime + to.RegularTimeFormat + ")";
                        shiftTimes.Add(new CustomPassShiftTimeEntity()
                        {
                            Hour_Id = "s" + item.NurseShifts_Id,
                            Hour_Desc = shiftTime
                        });
                    }
                }
            }
            return shiftTimes;
        }
        public List<HourEntity> GetHoursMasterData()
        {
            var hours = this.dbContext.Hours.Where(h => h.Hour_Status == 1).ToList();
            return this.autoMapper.Map<List<Hour>, List<HourEntity>>(hours);
        }
        public List<TimeFormatEntity> GetTimeFormatMasterData()
        {
            var timeFormatData = this.dbContext.TimeFormats.Where(tf => tf.TimeFormat_Status == 1).ToList();
            return this.autoMapper.Map<List<TimeFormat>, List<TimeFormatEntity>>(timeFormatData);
        }
        //public int InsertDrugAdministrationTime(DrugAdministrationTimeEntity DrugAdmTime)
        //{
        //    var record = this.autoMapper.Map<DrugAdministrationTimeEntity, DrugAdministrationTime>(DrugAdmTime);
        //    var quantity = this.dbContext.QuantityDetails.Where(qa => qa.POrder_Id == DrugAdmTime.POrder_Id).FirstOrDefault();
        //    var drugAdminister = this.dbContext.DrugAdministrationTimes.Where(da => da.POrder_Id == DrugAdmTime.POrder_Id).FirstOrDefault();
        //    if (drugAdminister == null)
        //    {
        //        record.PQuantity_Id = quantity.PQuantity_Id;
        //        this.dbContext.DrugAdministrationTimes.Add(record);
        //        this.dbContext.SaveChanges();

        //        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //        {
        //            Screen_Id = (int)ScreenEntity.Screens.Orders,
        //            Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
        //            Comments = record.POrder_Id.ToString(),
        //            Session_Id = 0,
        //            Time = DateTime.Now,
        //            UserActivity_Id = 0,

        //        };

        //        _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //        return 1;
        //    }
        //    else
        //    {
        //        //DrugAdministrationTime drugAdminister = this.dbContext.DrugAdministrationTimes.Where(d=>d.POrder_Id == DrugAdmTime.POrder_Id).FirstOrDefault();
        //        //drugAdminister.DAdmin_Id = record.DAdmin_Id;
        //        drugAdminister.POrder_Id = record.POrder_Id;
        //        drugAdminister.PQuantity_Id = quantity.PQuantity_Id;
        //        drugAdminister.AdministrationType = record.AdministrationType;
        //        drugAdminister.NursingFreq_Id = record.NursingFreq_Id;
        //        drugAdminister.Hour_Id = record.Hour_Id;
        //        drugAdminister.TimeFormat_Id = record.TimeFormat_Id;
        //        drugAdminister.Hours = record.Hours;
        //        drugAdminister.Monday = record.Monday;
        //        drugAdminister.Tuesday = record.Tuesday;
        //        drugAdminister.Wednesday = record.Wednesday;
        //        drugAdminister.Thursday = record.Thursday;
        //        drugAdminister.Friday = record.Friday;
        //        drugAdminister.Saturday = record.Saturday;
        //        drugAdminister.Sunday = record.Sunday;
        //        drugAdminister.Week_Id = record.Week_Id;
        //        drugAdminister.Month_Id = record.Month_Id;
        //        drugAdminister.OnlyOnDay = record.OnlyOnDay;
        //        drugAdminister.ThroughDay = record.ThroughDay;
        //        drugAdminister.ActiveDays = record.ActiveDays;
        //        drugAdminister.HoldDays = record.HoldDays;
        //        drugAdminister.DAdmin_Status = record.DAdmin_Status;
        //        drugAdminister.DAdmin_CreatedBy = record.DAdmin_CreatedBy;
        //        drugAdminister.DAdmin_CreatedDate = record.DAdmin_CreatedDate;
        //        this.dbContext.SaveChanges();
        //        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
        //        {
        //            Screen_Id = (int)ScreenEntity.Screens.Orders,
        //            Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
        //            Comments = record.POrder_Id.ToString(),
        //            Session_Id = 0,
        //            Time = DateTime.Now,
        //            UserActivity_Id = 0,

        //        };

        //        _userActivityRepository.InsertUserActivityDetails(activityEntity);
        //        return 1;
        //    }
        //    //var record = this.autoMapper.Map<DrugAdministrationTimeEntity, DrugAdministrationTime>(DrugAdmTime);
        //    //this.dbContext.DrugAdministrationTimes.Add(record);
        //    //this.dbContext.SaveChanges();
        //    //return 1;
        //}
        public DrugAdministrationTimeEntity GetScheduleTimeDetails(int OrderID)
        {
            var DrugTimeDetails = this.dbContext.DrugAdministrationTimes.Where(da => da.POrder_Id == OrderID).FirstOrDefault();
            return this.autoMapper.Map<DrugAdministrationTime, DrugAdministrationTimeEntity>(DrugTimeDetails);
        }
        /* 
         public OrdersTypeCountEntity GetOrdersTypeCounts(int userId)
        {
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();
            List<int> patientIds = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();

            int destroy = (from cm in this.dbContext.CommonOrderInfoes
                           join od in this.dbContext.OrderDestroys on cm.POrder_Id equals od.POrder_Id
                           where patientIds.Contains((int)cm.Patient_Id)
                           select od.OrderDestroy_Id).Count();
            int hold = (from cm in this.dbContext.CommonOrderInfoes
                        join oh in this.dbContext.OrderHolds on cm.POrder_Id equals oh.POrder_Id
                        where patientIds.Contains((int)cm.Patient_Id)
                        select oh.OrderHold_Id).Count();

            //List<int> DphysicianIds = destroyHold.Select(f => f.DphysicianId).Distinct().ToList();
            //List<int> OphysicianIds = destroyHold.Select(f => f.OphysicianId).Distinct().ToList();


            int active = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "NW" && patientIds.Contains((int)cm.Patient_Id)).Count();
            int dC = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "DC" && patientIds.Contains((int)cm.Patient_Id)).Count();
            int refill = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "RF" && patientIds.Contains((int)cm.Patient_Id)).Count();
            //int destroy = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "CA").Count();
            // int destroy = this.dbContext.OrderDestroys.Count();



            //int hold = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "RE").Count();
            //int hold = this.dbContext.OrderHolds.Count();
            int update = this.dbContext.CommonOrderInfoes.Where(cm => cm.OrderControl == "XO" && patientIds.Contains((int)cm.Patient_Id)).Count();
            OrdersTypeCountEntity ordercount = new OrdersTypeCountEntity();
            ordercount.Active = active;
            ordercount.Discontinue = dC;
            ordercount.Refill = refill;
            ordercount.Destroy = destroy;
            ordercount.Hold = hold;
            ordercount.Update = update;
            return ordercount;
        }
        */
        public OrdersinfoCustomEntity GetDiagnosisDetails(int PatientID)
        {
            OrdersinfoCustomEntity ordersinfo = new OrdersinfoCustomEntity();
            string result = string.Empty;
            var diagnosisdetails = (from dia in this.dbContext.DiagnosisInfoes
                                    join icd in this.dbContext.ICD10 on dia.ICD10_Id equals icd.ICD10_Id
                                    where dia.Patient_Id == PatientID
                                    select new { code = icd.ICD10_Description }).ToList();
            var allergydetails = (from al in this.dbContext.AllergyInfoes
                                  where al.Patient_Id == PatientID
                                  select new { alcode = al.ClassDrug_Name }).ToList();

            var resOrders = this.dbContext.ResidentOrders.Where(ro => ro.Patient_ID == PatientID && ro.ResOrderType == "4").ToList();
            if (resOrders.Count > 0)
            {
                resOrders.ForEach(di =>
                {
                    result = result + di.ResOrderText + ", ";
                });
                //result = string.Join(",", diagnosisdetails);
                ordersinfo.Diet = result.Substring(0, result.Length - 2);
            }
            else
            {
                ordersinfo.Diet = string.Empty;
            }
            result = "";
            if (allergydetails.Count() > 0)
            {
                allergydetails.ForEach(di =>
                {
                    result = result + di.alcode + ", ";
                });
                //result = string.Join(",", diagnosisdetails);
                ordersinfo.Allergy = result.Substring(0, result.Length - 2);
            }
            else
            {
                ordersinfo.Allergy = string.Empty;
            }
            result = "";
            if (diagnosisdetails.Count() > 0)
            {
                diagnosisdetails.ForEach(di =>
                {
                    result = result + di.code + ", ";
                });
                //result = string.Join(",", diagnosisdetails);
                ordersinfo.Diagnosis = result.Substring(0, result.Length - 2);
            }
            else
            {
                ordersinfo.Diagnosis = string.Empty;
            }
            return ordersinfo;
        }
        /*
        public int InsertUpdateResidentOrders(ResidentOrderEntity ResOrders)
        {
            var ResidentOrders = this.autoMapper.Map<ResidentOrderEntity, ResidentOrder>(ResOrders);
            if (ResidentOrders.ResOrder_Id == 0)
            {
                this.dbContext.ResidentOrders.Add(ResidentOrders);
            }
            else
            {
                var record = this.dbContext.ResidentOrders.Find(ResidentOrders.ResOrder_Id);
                record.ResOrderType = ResidentOrders.ResOrderType;
                record.ResOrderDate = ResidentOrders.ResOrderDate;
                record.ResPhysician = ResidentOrders.ResPhysician;
                record.ResOrderText = ResidentOrders.ResOrderText;
                record.ResOrder_CreatedBy = ResidentOrders.ResOrder_CreatedBy;
                record.ResOrder_CreatedDate = ResidentOrders.ResOrder_CreatedDate;
            }
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = ResOrders.ResOrder_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public List<ResidentOrderEntity> GetAllResidentOrders(int PatientID)
        {
            //var ResOrders = this.dbContext.ResidentOrders.Where(ro=>ro.Patient_ID==PatientID && ro.ResOrder_Status==1).ToList();
            var ResOrders = (from ro in this.dbContext.ResidentOrders
                             join phy in this.dbContext.PhysicianDetails on ro.ResPhysician equals phy.Physician_Id.ToString()
                             where ro.Patient_ID == PatientID && ro.ResOrder_Status == 1
                             select new ResidentOrderEntity
                             {
                                 ResOrder_Id = ro.ResOrder_Id,
                                 Patient_ID = ro.Patient_ID,
                                 ResOrderType = ro.ResOrderType,
                                 ResOrderDate = ro.ResOrderDate,
                                 ResPhysician = ro.ResPhysician,
                                 ResOrderText = ro.ResOrderText,
                                 ResOrder_Status = ro.ResOrder_Status,
                                 ResOrder_CreatedBy = ro.ResOrder_CreatedBy,
                                 ResOrder_CreatedDate = ro.ResOrder_CreatedDate,
                                 PhysicianName = phy.PhysicianLName + " " + phy.PhysicianFName
                             }).ToList();
            //return this.autoMapper.Map<List<ResidentOrder>, List<ResidentOrderEntity>>(ResOrders);
            return ResOrders;
        }
        public ResidentOrderEntity GetResidentOrderDetails(int ResOrderID)
        {
            var ResOrder = this.dbContext.ResidentOrders.Find(ResOrderID);
            return this.autoMapper.Map<ResidentOrder, ResidentOrderEntity>(ResOrder);
        }
        public int RemoveResidentOrderbyID(int ResOrderID)
        {
            var resOrder = this.dbContext.ResidentOrders.Find(ResOrderID);
            resOrder.ResOrder_Status = 2;
            this.dbContext.SaveChanges();
            return 1;
        }
        public int InsertOrder(InsertOrdersEntity orderDetails)
        {
            int result = 0;
            if (orderDetails.newOrderFlag == 1)
            {
                CommonOrderInfoEntity commonobj = new CommonOrderInfoEntity();
                commonobj.Patient_Id = orderDetails.ResidentID;
                commonobj.OrderingPhysicianID = Convert.ToInt16(orderDetails.PhyName);
                commonobj.OrderControl = "NW";
                commonobj.OrderTypeID = 1;
                commonobj.TransactionDate = DateTime.Now;
                commonobj.OrderEffectiveDate = orderDetails.StartDate;
                commonobj.POrder_Status = orderDetails.Status;
                commonobj.POrder_CreatedBy = orderDetails.CreatedBy;
                commonobj.POrder_CreatedDate = DateTime.Now;
                commonobj.AlertText = orderDetails.AlertText;
                commonobj.MaxPerdays = orderDetails.MaxPerdays;
                commonobj.OrderStockFlag = orderDetails.OrderStockFlag == 1 ? true : false;
                commonobj.PRNFlag = orderDetails.PRNFlag == 1 ? true : false;
                commonobj.SelfAdministeredFlag = orderDetails.SelfAdministeredFlag == 1 ? true : false;
                commonobj.TreatmentFlag = orderDetails.TreatmentFlag == 1 ? true : false;
                commonobj.Maysubstitute = orderDetails.Maysubstitute == 1 ? true : false;
                var commondata = this.autoMapper.Map<CommonOrderInfoEntity, CommonOrderInfo>(commonobj);
                this.dbContext.CommonOrderInfoes.Add(commondata);
                this.dbContext.SaveChanges();
                int ID = commondata.POrder_Id;

                EncodedOrderDetailEntity encodeObj = new EncodedOrderDetailEntity();
                encodeObj.POrder_Id = ID;
                encodeObj.GiveCodeText = orderDetails.TreatmentFlag == 0 ? orderDetails.Drug : null;
                encodeObj.ProviderAdminDrugInsText = orderDetails.AdditionalInst;
                encodeObj.NumberOfRefills = orderDetails.NumberofRefill;
                encodeObj.NumberOfRefillsRemaining = orderDetails.Inhand;
                encodeObj.PEncOrder_Status = orderDetails.Status;
                encodeObj.PEncOrder_CreatedBy = orderDetails.CreatedBy;
                encodeObj.PEncOrder_CreatedDate = DateTime.Now;
                var encodedata = this.autoMapper.Map<EncodedOrderDetailEntity, EncodedOrderDetail>(encodeObj);
                this.dbContext.EncodedOrderDetails.Add(encodedata);
                this.dbContext.SaveChanges();

                QuantityDetailEntity quantityObj = new QuantityDetailEntity();
                quantityObj.POrder_Id = ID;
                quantityObj.Quantity = orderDetails.Quantity;
                quantityObj.StartDate = orderDetails.StartDate;
                quantityObj.EndDate = orderDetails.EndDate;
                quantityObj.PQuantity_Status = orderDetails.Status;
                quantityObj.PQuantity_CreatedBy = orderDetails.CreatedBy;
                quantityObj.PQuantity_CreatedDate = DateTime.Now;
                var quantitydata = this.autoMapper.Map<QuantityDetailEntity, QuantityDetail>(quantityObj);
                this.dbContext.QuantityDetails.Add(quantitydata);
                this.dbContext.SaveChanges();

                TreatmentRouteInfoEntity treatmentroute = new TreatmentRouteInfoEntity();
                treatmentroute.POrder_Id = ID;
                treatmentroute.RouteCode = orderDetails.RouteCode;
                treatmentroute.PRoute_Status = orderDetails.Status;
                treatmentroute.PRoute_CreatedBy = orderDetails.CreatedBy;
                treatmentroute.PRoute_CreatedDate = DateTime.Now;
                var treatmentdata = this.autoMapper.Map<TreatmentRouteInfoEntity, TreatmentRouteInfo>(treatmentroute);
                this.dbContext.TreatmentRouteInfoes.Add(treatmentdata);
                this.dbContext.SaveChanges();
                if (orderDetails.TreatmentFlag == 1)
                {
                    TreatmentInfoEntity treatmentInfo = new TreatmentInfoEntity();
                    treatmentInfo.POrder_Id = ID;
                    treatmentInfo.RequestedGiveCode = orderDetails.Drug;
                    treatmentInfo.PTreatment_Status = 1;
                    treatmentInfo.PTreatment_CreatedBy = orderDetails.CreatedBy;
                    treatmentInfo.PTreatment_CreatedDate = DateTime.Now;
                    var treatmentInfoData = this.autoMapper.Map<TreatmentInfoEntity, TreatmentInfo>(treatmentInfo);
                    this.dbContext.TreatmentInfoes.Add(treatmentInfoData);
                    this.dbContext.SaveChanges();
                }
                result = ID;
            }
            else
            {
                var commonobj = this.dbContext.CommonOrderInfoes.Find(orderDetails.OrderID);
                commonobj.Patient_Id = orderDetails.ResidentID;
                commonobj.OrderingPhysicianID = Convert.ToInt16(orderDetails.PhyName);
                commonobj.OrderControl = "XO";
                commonobj.OrderTypeID = 1;
                commonobj.TransactionDate = DateTime.Now;
                commonobj.VEffectivedate = orderDetails.StartDate;
                commonobj.POrder_Status = orderDetails.Status;
                commonobj.POrder_CreatedBy = orderDetails.CreatedBy;
                commonobj.POrder_CreatedDate = DateTime.Now;
                commonobj.AlertText = orderDetails.AlertText;
                commonobj.MaxPerdays = orderDetails.MaxPerdays;
                commonobj.OrderStockFlag = orderDetails.OrderStockFlag == 1 ? true : false;
                commonobj.PRNFlag = orderDetails.PRNFlag == 1 ? true : false;
                commonobj.SelfAdministeredFlag = orderDetails.SelfAdministeredFlag == 1 ? true : false;
                commonobj.TreatmentFlag = orderDetails.TreatmentFlag == 1 ? true : false;
                commonobj.Maysubstitute = orderDetails.Maysubstitute == 1 ? true : false;
                commonobj.POOutBoundFileStatus = 1;
                this.dbContext.SaveChanges();

                var encodeObj = this.dbContext.EncodedOrderDetails.Where(en => en.POrder_Id == orderDetails.OrderID).FirstOrDefault();
                encodeObj.POrder_Id = orderDetails.OrderID;
                encodeObj.GiveCodeText = orderDetails.Drug;
                encodeObj.ProviderAdminDrugInsText = orderDetails.AdditionalInst;
                encodeObj.NumberOfRefills = orderDetails.NumberofRefill;
                encodeObj.NumberOfRefillsRemaining = orderDetails.Inhand;
                encodeObj.PEncOrder_Status = orderDetails.Status;
                encodeObj.PEncOrder_CreatedBy = orderDetails.CreatedBy;
                encodeObj.PEncOrder_CreatedDate = DateTime.Now;
                encodeObj.PEOutBoundFileStatus = 1;
                this.dbContext.SaveChanges();

                var quantityObj = this.dbContext.QuantityDetails.Where(qd => qd.POrder_Id == orderDetails.OrderID).FirstOrDefault();
                if (quantityObj != null)
                {
                    quantityObj.POrder_Id = orderDetails.OrderID;
                    quantityObj.Quantity = orderDetails.Quantity;
                    quantityObj.StartDate = orderDetails.StartDate;
                    quantityObj.EndDate = orderDetails.EndDate;
                    quantityObj.PQuantity_Status = orderDetails.Status;
                    quantityObj.PQuantity_CreatedBy = orderDetails.CreatedBy;
                    quantityObj.PQuantity_CreatedDate = DateTime.Now;

                    this.dbContext.SaveChanges();
                }

                var treatmentroute = this.dbContext.TreatmentRouteInfoes.Where(tr => tr.POrder_Id == orderDetails.OrderID).FirstOrDefault();
                if (treatmentroute != null)
                {
                    treatmentroute.POrder_Id = orderDetails.OrderID;
                    treatmentroute.RouteCode = orderDetails.RouteCode;
                    treatmentroute.PRoute_Status = orderDetails.Status;
                    treatmentroute.PRoute_CreatedBy = orderDetails.CreatedBy;
                    treatmentroute.PRoute_CreatedDate = DateTime.Now;
                    treatmentroute.PTROutBoundFileStatus = 1;
                    this.dbContext.SaveChanges();
                }
                var treatmentinfo = this.dbContext.TreatmentInfoes.Where(tr => tr.POrder_Id == orderDetails.OrderID).FirstOrDefault();
                if (treatmentinfo != null)
                {
                    treatmentinfo.POrder_Id = orderDetails.OrderID;
                    treatmentinfo.RequestedGiveCode = orderDetails.Drug;
                    treatmentinfo.PTreatment_Status = orderDetails.Status;
                    treatmentinfo.PTreatment_CreatedBy = orderDetails.CreatedBy;
                    treatmentinfo.PTreatment_CreatedDate = DateTime.Now;
                    treatmentinfo.PTIOutBoundFileStatus = 1;
                    this.dbContext.SaveChanges();
                }
                result = orderDetails.OrderID;
            }
            //using (EMAREntities context = new EMAREntities())
            //{
            //    using (DbContextTransaction transcation = context.Database.BeginTransaction())
            //    {
            //        try {                        
            //            result = InsertOrderRecord(orderDetails, context);
            //        }
            //        catch(Exception ex)
            //        {
            //            transcation.Rollback();
            //            throw ex;
            //        }
            //    }
            //}
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = orderDetails.OrderID.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return result;
        }
        public int InsertOrderRecord(InsertOrdersEntity orderDetails, EMAREntities context)
        {
            int? i = context.PrcOrderInsert(orderDetails.FacilityID, Convert.ToInt16(orderDetails.PhyName), orderDetails.ResName, orderDetails.ResidentID, Convert.ToDateTime(orderDetails.DOB), Convert.ToDateTime(orderDetails.AdmitDate),
                orderDetails.Drug, orderDetails.Quantity, orderDetails.RouteCode, orderDetails.AdditionalInst, orderDetails.NumberofRefill,
                orderDetails.Inhand, orderDetails.StartDate, orderDetails.EndDate,
                orderDetails.Diagnosis, orderDetails.OrderID, orderDetails.Status, orderDetails.CreatedBy, orderDetails.AlertText, orderDetails.MaxPerdays, orderDetails.OrderStockFlag,
                orderDetails.PRNFlag, orderDetails.SelfAdministeredFlag, orderDetails.TreatmentFlag, orderDetails.Maysubstitute).FirstOrDefault();
            return 1;
        }
        public int InsertBarcodeDetails(BarcodeDetailEntity barcodes)
        {
            var barcodedetail = this.autoMapper.Map<BarcodeDetailEntity, BarcodeDetail>(barcodes);
            this.dbContext.BarcodeDetails.Add(barcodedetail);
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = barcodedetail.POrder_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public List<BarcodeDetailEntity> GetBarcodeData(int OrderID)
        {
            var barcodedata = this.dbContext.BarcodeDetails.Where(or => or.POrder_Id == OrderID && or.PBarcode_Status == 1).ToList();
            return this.autoMapper.Map<List<BarcodeDetail>, List<BarcodeDetailEntity>>(barcodedata);
        }
        public int DeleteBarcode(int BarcodeID)
        {
            var barcodeRecord = this.dbContext.BarcodeDetails.Find(BarcodeID);
            barcodeRecord.PBarcode_Status = 2;
            this.dbContext.SaveChanges();
            return 1;
        }

        */
        //public List<OrderFavouriteCustomEntity> GetFavouritesMasterData(int quantityId)
        //{
        //    //var favouritesdata = this.dbContext.OrderFavouriteMasters.Where(fa => fa.OrderFavMaster_status == 1).ToList();
        //    var favouritesdata = (from fam in this.dbContext.OrderFavouriteMasters
        //                              //join fa in this.dbContext.OrderFavourites on fam.OrderFavMaster_ID equals fa.OrderFavMaster_ID into join1
        //                              //from j1 in join1.DefaultIfEmpty() where j1.Porder_ID== OrderID
        //                          select new OrderFavouriteCustomEntity
        //                          {
        //                              OrderFavourite_ID = this.dbContext.OrderFavourites.Where(fa => fa.PQuantity_Id == quantityId && fa.OrderFavMaster_ID == fam.OrderFavMaster_ID).Select(fa => fa.OrderFavourite_ID).FirstOrDefault(),
        //                              OrderFavMaster_ID = fam.OrderFavMaster_ID,
        //                              OrderFavDesc = fam.OrderFavDesc,
        //                              OrderFavMaster_status = fam.OrderFavMaster_status,
        //                              OrderFavMaster_CreatedBy = fam.OrderFavMaster_CreatedBy,
        //                              OrderFavMaster_CreatedOn = fam.OrderFavMaster_CreatedOn,
        //                              //check=(j1.OrderFavourite_ID.ToString()==""?0:1)
        //                              check = this.dbContext.OrderFavourites.Where(fa => fa.PQuantity_Id == quantityId && fa.OrderFavMaster_ID == fam.OrderFavMaster_ID).Select(fa => fa.OrderFavMaster_ID).FirstOrDefault()
        //                          }).ToList();
        //    //return this.autoMapper.Map<List<OrderFavouriteMaster>, List<OrderFavouriteMasterEntity>>(favouritesdata);
        //    return favouritesdata;
        //}
        public List<OrderFavouriteCustomEntity> GetFavouritesMasterData(int quantityId, int facilityId)
        {
            DataTable dt = new DataTable();
            string query = "[Patient].[Prc_Userinputs]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId;
                    cmd.Parameters.Add("@pquantityid", SqlDbType.Int).Value = quantityId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            var result = (from d in dt.AsEnumerable()
                          select new OrderFavouriteCustomEntity
                          {
                              OrderFavourite_ID = Convert.ToInt32(d["OrderFavourite_ID"]),
                              OrderFavMaster_ID = Convert.ToInt32(d["orderfavmaster_ID"]),
                              OrderFavDesc = d["OrderFavDesc"].ToString(),
                              OrderFavMaster_status = Convert.ToInt32(d["OrderFavMaster_status"]),
                              OrderFavMaster_CreatedBy = Convert.ToInt32(d["OrderFavMaster_CreatedBy"]),
                              OrderFavMaster_CreatedOn = (DateTime)(string.IsNullOrEmpty(d["OrderFavMaster_CreatedOn"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["OrderFavMaster_CreatedOn"])),
                              check = Convert.ToInt32(d["check"]),
                          }).ToList();

            return result;

        }

        //public List<OrderFavouriteCustomEntity> GetFavouritesMasterData(int quantityId)
        //{
        //    // Step 1: Get the top four OrderFavMaster_IDs based on their count
        //    var topOrderFavMasterIds = this.dbContext.OrderFavourites
        //        .GroupBy(fa => fa.OrderFavMaster_ID)
        //        .Select(group => new
        //        {
        //            OrderFavMaster_ID = group.Key,
        //            Count = group.Count()
        //        })
        //        .OrderByDescending(x => x.Count)
        //        .Take(4)
        //        .Select(x => x.OrderFavMaster_ID)
        //        .ToList();

        //    // Step 2: Retrieve the data and order it
        //    var favouritesdata = (from fam in this.dbContext.OrderFavouriteMasters
        //                          select new OrderFavouriteCustomEntity
        //                          {
        //                              OrderFavourite_ID = this.dbContext.OrderFavourites
        //                                  .Where(fa => fa.PQuantity_Id == quantityId && fa.OrderFavMaster_ID == fam.OrderFavMaster_ID)
        //                                  .Select(fa => fa.OrderFavourite_ID)
        //                                  .FirstOrDefault(),
        //                              OrderFavMaster_ID = fam.OrderFavMaster_ID,
        //                              OrderFavDesc = fam.OrderFavDesc,
        //                              OrderFavMaster_status = fam.OrderFavMaster_status,
        //                              OrderFavMaster_CreatedBy = fam.OrderFavMaster_CreatedBy,
        //                              OrderFavMaster_CreatedOn = fam.OrderFavMaster_CreatedOn,
        //                              check = this.dbContext.OrderFavourites
        //                                  .Where(fa => fa.PQuantity_Id == quantityId && fa.OrderFavMaster_ID == fam.OrderFavMaster_ID)
        //                                  .Select(fa => fa.OrderFavMaster_ID)
        //                                  .FirstOrDefault()
        //                          })
        //                          .ToList()
        //                          .OrderByDescending(x => topOrderFavMasterIds.Contains(x.OrderFavMaster_ID))
        //                          .ThenBy(x => x.OrderFavDesc)  // Assuming you want to sort alphabetically by OrderFavDesc
        //                          .ToList();
        //    return favouritesdata;

        //}
        public int InsertOrderFavourities(List<OrderFavouriteEntity> OrderFavouroties)
        {
            var orderFav = this.autoMapper.Map<List<OrderFavouriteEntity>, List<OrderFavourite>>(OrderFavouroties).ToList();
            //var order = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == OrderFavouroties[0].PQuantity_Id).FirstOrDefault().POrder_Id;
            DateTime dateTime = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            if (OrderFavouroties[0].FavListCheckFlag == 0)
            {
                if (orderFav.Count > 0)
                {
                    for (int i = 0; i < orderFav.Count; i++)
                    {
                        OrderFavourite favObj = orderFav[i];
                        favObj.OrderFavourite_CreatedOn = dateTime;
                        if (favObj.OrderFavourite_ID == 0)
                        {
                            this.dbContext.OrderFavourites.Add(favObj);
                            this.dbContext.SaveChanges();
                        }
                        else
                        {
                            var record = this.dbContext.OrderFavourites.Where(of => of.OrderFavourite_ID == favObj.OrderFavourite_ID).FirstOrDefault();
                            this.dbContext.OrderFavourites.Attach(record);
                            //this.dbContext.OrderFavourites.Remove(record);
                            this.dbContext.SaveChanges();
                        }
                    }
                    var checkFavId = orderFav.Where(o => o.OrderFavourite_ID != 0).Select(o => o.OrderFavourite_ID).FirstOrDefault();
                    var qtyId = orderFav.Any(o => o.PQuantity_Id != 0) ? orderFav.Where(o => o.PQuantity_Id != 0).Select(o => o.PQuantity_Id).FirstOrDefault() : this.dbContext.OrderFavourites.Where(of => of.OrderFavourite_ID == checkFavId).Select(of => of.PQuantity_Id).FirstOrDefault();

                    var existingRecordIds = OrderFavouroties.Select(f => f.OrderFavMaster_ID).ToList();
                    var exisitngrecords = this.dbContext.OrderFavourites.Where(of => of.PQuantity_Id == qtyId).ToList();
                    var tobedeletedrecords = exisitngrecords.Where(e => !existingRecordIds.Contains(e.OrderFavMaster_ID)).ToList();
                    if (tobedeletedrecords.Count() > 0)
                    {
                        this.dbContext.OrderFavourites.RemoveRange(tobedeletedrecords);
                        this.dbContext.SaveChanges();
                    }
                    var order = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == qtyId).FirstOrDefault();
                    if (order != null)
                    {
                        this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
                        //var recordUpdate = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == order.POrder_Id).FirstOrDefault();
                        //if (recordUpdate != null)
                        //{
                        //    recordUpdate.POrder_CreatedBy = OrderFavouroties[0].OrderFavourite_Createby;
                        //    recordUpdate.POrder_CreatedDate = DateTime.Now;
                        //    this.dbContext.SaveChanges();
                        //}
                    }
                }
            }
            if (OrderFavouroties[0].FavListCheckFlag == 1)
            {
                int qtyId = OrderFavouroties[0].PQuantity_Id;
                var exisitngrecords = this.dbContext.OrderFavourites.Where(of => of.PQuantity_Id == qtyId).ToList();
                if (exisitngrecords.Count() > 0)
                {
                    this.dbContext.OrderFavourites.RemoveRange(exisitngrecords);
                    this.dbContext.SaveChanges();
                }
                var order = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == qtyId).FirstOrDefault();
                if (order != null)
                {
                    this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
                    //var recordUpdate = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == order.POrder_Id).FirstOrDefault();
                    //if (recordUpdate != null)
                    //{
                    //    recordUpdate.POrder_CreatedBy = OrderFavouroties[0].OrderFavourite_Createby;
                    //    recordUpdate.POrder_CreatedDate = DateTime.Now;
                    //    this.dbContext.SaveChanges();
                    //}
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };



            _userActivityRepository.InsertUserActivityDetails(activityEntity);

            return 1;
        }
        //public List<OrderFavouriteEntity> GetFavouritesByOrderID(int OrderID)
        //{
        //    var favouriteData = this.dbContext.OrderFavourites.Where(fav => fav.Porder_ID == OrderID).ToList();
        //    return this.autoMapper.Map<List<OrderFavourite>, List<OrderFavouriteEntity>>(favouriteData);
        //}
        public int InsertOrderHoldDetails(OrderHoldEntity orderHold)
        {
            orderHold.OrderHold_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<OrderHoldEntity, OrderHold>(orderHold);
            var check = this.dbContext.OrderHolds.Where(rh => rh.PQuantity_Id == orderHold.PQuantity_Id).FirstOrDefault();
            var order = this.dbContext.QuantityDetails.Where(qd => qd.PQuantity_Id == orderHold.PQuantity_Id).FirstOrDefault();
            if (order != null)
            {
                var comOrder = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == order.POrder_Id).FirstOrDefault();
                comOrder.POrder_CreatedBy = orderHold.OrderHold_CreatedBy;
                comOrder.POrder_CreatedDate = orderHold.OrderHold_CreatedDate.Value;
                this.dbContext.SaveChanges();
            }
            if (check == null)
            {
                this.dbContext.OrderHolds.Add(record);
                this.dbContext.SaveChanges();
                //if(order!=null)
                //this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
            }
            else
            {
                check.HoldFrom = orderHold.HoldFrom;
                check.HoldTo = orderHold.HoldTo;
                check.HoldReason = orderHold.HoldReason;
                check.OrderHold_Status = orderHold.OrderHold_Status;
                check.OrderHold_CreatedBy = orderHold.OrderHold_CreatedBy;
                check.OrderHold_CreatedDate = orderHold.OrderHold_CreatedDate;
                this.dbContext.SaveChanges();
                //if (order != null)
                //this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = record.PQuantity_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public NursingFrequencyConfigEntity GetNurseFrequencyDropSelect(int frequencyId, int nurseStationId)
        {
            var record = this.dbContext.NursingFrequencyConfigs.Where(nf => nf.Frequency_Id == frequencyId && nf.NursingStation_Id == nurseStationId).FirstOrDefault();
            if (record != null)
            {
                var data = this.autoMapper.Map<NursingFrequencyConfig, NursingFrequencyConfigEntity>(record);
                data.HoursList = this._emarRepository.GetNursingFrequencyDetailsByID(record.NursingFreq_Id).HoursList;
                return data;
            }
            else
            {
                return null;
            }
        }

        public string InsertOrderdestroy(OrderDestroyEntity orderDestroy)
        {
            orderDestroy.OrderDestroy_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<OrderDestroyEntity, OrderDestroy>(orderDestroy);
            var check = this.dbContext.OrderDestroys.Where(dr => dr.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault();
            var qtydetails = this.dbContext.QuantityDetails.Where(ds => ds.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault();
            var order = this.dbContext.QuantityDetails.Where(qd => qd.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault();

            //if (check == null)
            //{
            this.dbContext.OrderDestroys.Add(record);
            this.dbContext.SaveChanges();
            //if (order != null)
            //this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
            //}
            //else
            //{
            //check.Quantity = orderDestroy.Quantity;
            //check.Reason = orderDestroy.Reason;
            //check.DestroyerUserId = orderDestroy.DestroyerUserId;
            //check.ApprovalUserId = orderDestroy.ApprovalUserId;
            //check.OrderDestroy_CreatedBy = orderDestroy.OrderDestroy_CreatedBy;
            //check.OrderDestroy_CreatedOn = orderDestroy.OrderDestroy_CreatedOn;
            //this.dbContext.SaveChanges();
            //if (order != null)
            //this.dbContext.InsertOrderChangesforReport(order.POrder_Id);
            //}
            qtydetails.DiscontinueFlag = 5;
            if (decimal.Parse(orderDestroy.Quantity) != 0)
            {
                var drugGpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == qtydetails.POrder_Id).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                if (drugGpi != null && this.dbContext.Stocks.Where(s => s.GPICode == drugGpi).FirstOrDefault() != null)
                {
                    var stockRecord = this.dbContext.Stocks.Where(s => s.GPICode == drugGpi).FirstOrDefault();
                    if (stockRecord.InHand != null && stockRecord.InHand != "")
                        stockRecord.InHand = (decimal.Parse(stockRecord.InHand) - decimal.Parse(orderDestroy.Quantity)) < 0 ? "0" : (decimal.Parse(stockRecord.InHand) - decimal.Parse(orderDestroy.Quantity)).ToString();
                }
                if (this.dbContext.OrderStocks.Where(o => o.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault() != null)
                {
                    var orderStock = this.dbContext.OrderStocks.Where(o => o.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault();
                    record.QtyHand = orderStock.Remaining;
                    if (orderStock.Remaining != null && orderStock.Remaining != "")
                        orderStock.Remaining = (decimal.Parse(orderStock.Remaining) - decimal.Parse(orderDestroy.Quantity)) < 0 ? "0" : (decimal.Parse(orderStock.Remaining) - decimal.Parse(orderDestroy.Quantity)).ToString();
                }
                if (this.dbContext.ControlSubstanceCounts.Where(c => c.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault() != null)
                {
                    var controlsubstanceCountQty = this.dbContext.ControlSubstanceCounts.Where(c => c.PQuantity_Id == orderDestroy.PQuantity_Id).FirstOrDefault();
                    record.QtyHand = controlsubstanceCountQty.Quantity;
                    if (controlsubstanceCountQty.Quantity != null && controlsubstanceCountQty.Quantity != "")
                        controlsubstanceCountQty.Quantity = (decimal.Parse(controlsubstanceCountQty.Quantity) - decimal.Parse(orderDestroy.Quantity)) < 0 ? "0" : (decimal.Parse(controlsubstanceCountQty.Quantity) - decimal.Parse(orderDestroy.Quantity)).ToString();
                }
            }
            this.dbContext.SaveChanges();
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = record.PQuantity_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return "Done";
        }
        /* 
        public string CheckDischargeInterval(int patientId, int interval)
        {
            string outPut = string.Empty;
            var resInfo = this.dbContext.VisitInfoes.Where(vs => vs.Patient_Id == patientId).FirstOrDefault();
            if (resInfo.DischargeDate != null)
            {
                DateTime dd1 = Convert.ToDateTime(resInfo.DischargeDate);
                DateTime dd2 = DateTime.Now;
                int result = Convert.ToInt16((dd2 - dd1).TotalDays);
                if (result > interval)
                {
                    outPut = "Not Allowed";
                }
                else
                {
                    outPut = "Allowed";
                }
            }
            else
            {
                outPut = "Allowed";
            }
            return outPut;
        }
        */

        public Int32 checkinteger(object obj)
        {
            try
            {
                if (obj != DBNull.Value)
                    return Convert.ToInt32(obj);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }

        }


        public List<ControlSubstanceGridEntity> GetControlSubstanceGridData(ControlSubstanceFilter filter)
        {
            string orderid = "";
            try
            {

                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                List<int> facilityIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                         where us.User_Id == filter.User_Id && us.UserRole_Status == 1
                                         select us.Facility_id).Distinct().ToList();

                //t
                var q1 = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId)).ToList();
                var q2 = q1;
                if (filter.NurseStations.Count != 0)
                {
                    q2 = q1.Where(p => filter.NurseStations.Contains(Convert.ToInt32(p.NursingStationId))).ToList();
                }
                var q3 = q2;
                if (filter.Floors.Count != 0)
                {
                    q3 = q2.Where(p => filter.Floors.Contains(Convert.ToInt32(p.Floor))).ToList();
                }
                var q4 = q3;
                if (filter.Wings.Count != 0)
                {
                    q4 = q3.Where(w => filter.Wings.Contains(Convert.ToInt32(w.Wing))).ToList();
                }
                var q5 = q4;
                if (filter.Rooms.Count != 0)
                {
                    q5 = q4.Where(p => filter.Rooms.Contains(Convert.ToInt32(p.Room))).ToList();
                }
                var q6 = q5;
                if (filter.Beds.Count != 0)
                {
                    q6 = q5.Where(p => filter.Beds.Contains(Convert.ToInt32(p.Bed))).ToList();
                }
                List<ControlSubstanceGridEntity> controlData = new List<ControlSubstanceGridEntity>();
                if (q6.Count > 0)
                {
                    var patientIds = q6.Select(p => p.Patient_Id).Distinct();
                    string[] controlIds = { "I", "II", "III", "IV", "V" };

                    string Pids = String.Join(",", patientIds.ToArray());
                    if (filter.History == 0)
                    {



                        DataSet dspat = new DataSet();
                        using (SqlConnection con = new SqlConnection(constrEmar))
                        {
                            SqlCommand objSqlCommand = new SqlCommand("select * from [Patient].[VGetorderinfoforcontrolsubs] where Patient_Id in(" + Pids + ")", con);
                            objSqlCommand.CommandType = CommandType.Text;
                            objSqlCommand.CommandTimeout = 180;
                            SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);

                            objSqlDataAdapter.Fill(dspat);


                        }

                        var list = (from d in dspat.Tables[0].AsEnumerable()
                                    select new
                                    {
                                        POrder_Id = checkinteger(d["POrder_Id"]),
                                        Patient_Id = checkinteger(d["Patient_Id"]),
                                        ResidentName = d["ResidentName"].ToString(),
                                        DOB = Convert.ToDateTime(d["DOB"]),
                                        PQuantity_Id = checkinteger(d["PQuantity_Id"]),
                                        PQuantity_Status = checkinteger(d["PQuantity_Status"]),
                                        TextInstruction = d["TextInstruction"].ToString(),
                                        ControlledSubstanceSchedule = d["ControlledSubstanceSchedule"],
                                        AGiveCodeIdentifier = Convert.ToString(d["AGiveCodeIdentifier"]),
                                        GiveCodeText = d["GiveCodeText"].ToString(),
                                        ConsolidateFlag = checkinteger(d["ConsolidateFlag"]),
                                        Quantity = d["Quantity"].ToString(),
                                        CheckInFlag = checkinteger(d["CheckInFlag"]),
                                        NurseStation_Id = checkinteger(d["NurseStation_Id"]),
                                        CertifiedBy = checkinteger(d["CertifiedBy"]),
                                        CertifiedDate = d["CertifiedDate"],
                                        InitialQuantity = checkinteger(d["InitialQuantity"]),
                                        OrderStatus = checkinteger(d["OrderStatus"]),

                                    }).Distinct().ToList();

                        var orders = list.Where(li => li.ConsolidateFlag == 1).Select(li => li.PQuantity_Id).ToList();
                        var consolidatedOrders = list.Where(li => orders.Contains(li.PQuantity_Id)).ToList().GroupBy(m => new { m.Patient_Id, m.AGiveCodeIdentifier }).Select(group => group.FirstOrDefault()).ToList().Select(re => re.PQuantity_Id).ToList();
                        var data = list.Where(li => (li.ConsolidateFlag != 1) || consolidatedOrders.Contains(li.PQuantity_Id)).ToList();

                        ControlSubstanceGridEntity controlSubstance;
                        foreach (var item in data)
                        {


                            orderid = item.POrder_Id.ToString();
                            if(item.POrder_Id.ToString() == "67246")
                            {
                                string a = "";
                            }

                            DataSet dsquery = new DataSet();
                            using (SqlConnection con = new SqlConnection(constrEmar))
                            {
                                SqlCommand objSqlCommand = new SqlCommand("select * from [Patient].[VGetVisitInfoByVisit]  where Patient_Id = " + item.Patient_Id + "", con);
                                objSqlCommand.CommandType = CommandType.Text;
                                objSqlCommand.CommandTimeout = 180;
                                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);

                                objSqlDataAdapter.Fill(dsquery);


                            }

                            var listquery = (from d in dsquery.Tables[0].AsEnumerable()
                                             select new
                                             {
                                                 NursingStationId = checkinteger(d["NursingStationId"]),
                                                 FacilityId = checkinteger(d["FacilityId"]),
                                                 PVisit_Status = checkinteger(d["PVisit_Status"]),

                                             }).FirstOrDefault();




                            var nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == listquery.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();
                            //12/09/2022 changed comapany id to facility id
                            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == listquery.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
                            decimal administeredQty = 0;
                            controlSubstance = new ControlSubstanceGridEntity();
                            controlSubstance.PorderId = item.POrder_Id;
                            controlSubstance.ResidentName = item.ResidentName;
                            controlSubstance.DOB = item.DOB.ToString();
                            controlSubstance.Patient_Id = item.Patient_Id;
                            controlSubstance.GPI = item.AGiveCodeIdentifier;
                            controlSubstance.QuantityId = item.PQuantity_Id;
                            controlSubstance.ConsolidatedAllow = 0;
                            controlSubstance.ConsolidateFlag = item.ConsolidateFlag;
                            controlSubstance.Directions = item.TextInstruction;
                            controlSubstance.AdministeredQty = administeredQty + (item.Quantity == "" ? 0 : Convert.ToDecimal(item.Quantity));
                            controlSubstance.Order = item.GiveCodeText;
                            controlSubstance.NurseStationName = nsName;
                            controlSubstance.CheckInFlag = item.CheckInFlag;
                            controlSubstance.NsChangeFlag = item.NurseStation_Id != listquery.NursingStationId ? 1 : 0;
                            controlSubstance.LastCertified = item.NurseStation_Id != listquery.NursingStationId ? "None" : (item.CertifiedBy != null ? (this.dbContext.Users.Where(us => us.User_Id == item.CertifiedBy).Select(us => us.User_DisplayName).FirstOrDefault()) : "None");
                            controlSubstance.CertifiedDate = item.NurseStation_Id != listquery.NursingStationId ? "None" : (item.CertifiedDate != DBNull.Value ? GetTimeZoneDateTime(Convert.ToDateTime(item.CertifiedDate), companyId).ToString() : "None");
                            controlSubstance.Porder_Status = list.Where(li => li.Patient_Id == item.Patient_Id && li.AGiveCodeIdentifier == item.AGiveCodeIdentifier && item.ConsolidateFlag == 1).Count() > 1 ? list.Where(li => li.Patient_Id == item.Patient_Id && li.AGiveCodeIdentifier == item.AGiveCodeIdentifier && item.ConsolidateFlag == 1 && li.OrderStatus == 1).Count() == 0 ? 2 : 1 : (int)item.OrderStatus;
                            controlSubstance.eKitFlag = 0;
                            controlSubstance.Ekit_Id = 0;
                            if (item.ConsolidateFlag == 1)
                            {
                                List<ControlSubstanceGridEntity> lst = GetControlSubstanceGridDataByPid(item.Patient_Id, item.AGiveCodeIdentifier, item.ConsolidateFlag);

                                string Text = "";

                                foreach (var a in lst)
                                {
                                    if (a.DFalg <= 0)
                                    {
                                        if (Text == "")
                                        {
                                            Text = a.Directions;
                                        }
                                        else
                                        {
                                            Text = Text + "\n" + a.Directions;
                                        }
                                    }


                                }

                                controlSubstance.Directions = Text;


                            }
                            if (item.Quantity != "")
                            {
                                controlSubstance.Quantity = Convert.ToDecimal(item.Quantity);
                                controlSubstance.InitialQuantity = Convert.ToDecimal(item.InitialQuantity);
                            }
                            else
                            {
                                controlSubstance.Quantity = null;
                                controlSubstance.InitialQuantity = null;
                            }
                            if (controlSubstance.Quantity != 0)
                            {
                                if ((controlSubstance.Porder_Status == 2 && controlSubstance.Quantity > 0) || ((listquery.PVisit_Status == 2 || listquery.PVisit_Status == 3) && controlSubstance.Quantity > 0))
                                {
                                    controlSubstance.QuantityZeroStatus = 1;
                                    controlData.Add(controlSubstance);
                                }
                                else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (listquery.PVisit_Status == 1 || (listquery.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                                {
                                    controlSubstance.QuantityZeroStatus = 0;
                                    controlData.Add(controlSubstance);
                                }
                            }
                            else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (listquery.PVisit_Status == 1 || (listquery.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                            {
                                controlSubstance.QuantityZeroStatus = 0;
                                controlData.Add(controlSubstance);
                            }
                        }
                    }
                    else if (filter.History == 1)
                    {

                        DataSet dspat = new DataSet();
                        using (SqlConnection con = new SqlConnection(constrEmar))
                        {
                            SqlCommand objSqlCommand = new SqlCommand("select * from [Patient].[VGetorderinfoforcontrolsubstwo]  where Patient_Id in(" + Pids + ")", con);
                            objSqlCommand.CommandType = CommandType.Text;
                            objSqlCommand.CommandTimeout = 180;
                            SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);

                            objSqlDataAdapter.Fill(dspat);


                        }

                        var list = (from d in dspat.Tables[0].AsEnumerable()
                                    select new
                                    {
                                        POrder_Id = checkinteger(d["POrder_Id"]),
                                        Patient_Id = checkinteger(d["Patient_Id"]),
                                        ResidentName = d["ResidentName"].ToString(),
                                        DOB = Convert.ToDateTime(d["DOB"]),
                                        PQuantity_Id = checkinteger(d["PQuantity_Id"]),
                                        PQuantity_Status = checkinteger(d["PQuantity_Status"]),
                                        TextInstruction = d["TextInstruction"].ToString(),
                                        ControlledSubstanceSchedule = d["ControlledSubstanceSchedule"],
                                        AGiveCodeIdentifier = Convert.ToString(d["AGiveCodeIdentifier"]),
                                        GiveCodeText = d["GiveCodeText"].ToString(),
                                        ConsolidateFlag = checkinteger(d["ConsolidateFlag"]),
                                        Quantity = d["Quantity"].ToString(),
                                        CheckInFlag = checkinteger(d["CheckInFlag"]),
                                        NurseStation_Id = checkinteger(d["NurseStation_Id"]),
                                        CertifiedBy = checkinteger(d["CertifiedBy"]),
                                        CertifiedDate = d["CertifiedDate"],
                                        InitialQuantity = checkinteger(d["InitialQuantity"]),
                                        OrderStatus = checkinteger(d["OrderStatus"]),

                                    }).Distinct().ToList();
                        var orders = list.Where(li => li.ConsolidateFlag == 1).Select(li => li.PQuantity_Id).ToList();
                        var consolidatedOrders = list.Where(li => orders.Contains(li.PQuantity_Id)).ToList().GroupBy(m => new { m.Patient_Id, m.AGiveCodeIdentifier }).Select(group => group.FirstOrDefault()).ToList().Select(re => re.PQuantity_Id).ToList();
                        var data = list.Where(li => (li.ConsolidateFlag != 1) || consolidatedOrders.Contains(li.PQuantity_Id)).ToList();

                        ControlSubstanceGridEntity controlSubstance;
                        foreach (var item in data)
                        {
                            DataSet dsquery = new DataSet();
                            using (SqlConnection con = new SqlConnection(constrEmar))
                            {
                                SqlCommand objSqlCommand = new SqlCommand("select * from [Patient].[VGetVisitInfoByVisit]  where Patient_Id = " + item.Patient_Id + "", con);
                                objSqlCommand.CommandType = CommandType.Text;
                                objSqlCommand.CommandTimeout = 180;
                                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);

                                objSqlDataAdapter.Fill(dsquery);


                            }

                            var listquery = (from d in dsquery.Tables[0].AsEnumerable()
                                             select new
                                             {
                                                 NursingStationId = checkinteger(d["NursingStationId"]),
                                                 FacilityId = checkinteger(d["FacilityId"]),
                                                 PVisit_Status = checkinteger(d["PVisit_Status"]),

                                             }).FirstOrDefault();




                            var nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == listquery.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();
                            //12/09/2022 changed comapany id to facility id
                            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == listquery.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
                            decimal administeredQty = 0;
                            controlSubstance = new ControlSubstanceGridEntity();

                            controlSubstance.PorderId = item.POrder_Id;
                            controlSubstance.ResidentName = item.ResidentName;
                            controlSubstance.DOB = item.DOB.ToString();
                            controlSubstance.Patient_Id = item.Patient_Id;
                            controlSubstance.GPI = item.AGiveCodeIdentifier;
                            controlSubstance.QuantityId = item.PQuantity_Id;
                            controlSubstance.ConsolidatedAllow = 0;
                            controlSubstance.ConsolidateFlag = item.ConsolidateFlag;
                            controlSubstance.Directions = item.TextInstruction;
                            controlSubstance.AdministeredQty = administeredQty + (item.Quantity == "" ? 0 : Convert.ToDecimal(item.Quantity));
                            controlSubstance.Order = item.GiveCodeText;
                            controlSubstance.NurseStationName = nsName;
                            controlSubstance.CheckInFlag = item.CheckInFlag;
                            controlSubstance.NsChangeFlag = item.NurseStation_Id != listquery.NursingStationId ? 1 : 0;
                            controlSubstance.LastCertified = item.NurseStation_Id != listquery.NursingStationId ? "None" : (item.CertifiedBy != null ? (this.dbContext.Users.Where(us => us.User_Id == item.CertifiedBy).Select(us => us.User_DisplayName).FirstOrDefault()) : "None");
                            controlSubstance.CertifiedDate = item.NurseStation_Id != listquery.NursingStationId ? "None" : (item.CertifiedDate != DBNull.Value ? GetTimeZoneDateTime(Convert.ToDateTime(item.CertifiedDate), companyId).ToString() : "None");
                            controlSubstance.Porder_Status = list.Where(li => li.Patient_Id == item.Patient_Id && li.AGiveCodeIdentifier == item.AGiveCodeIdentifier && item.ConsolidateFlag == 1).Count() > 1 ? list.Where(li => li.Patient_Id == item.Patient_Id && li.AGiveCodeIdentifier == item.AGiveCodeIdentifier && item.ConsolidateFlag == 1 && li.OrderStatus == 1).Count() == 0 ? 2 : 1 : (int)item.OrderStatus;
                            controlSubstance.eKitFlag = 0;
                            controlSubstance.Ekit_Id = 0;
                            if (item.ConsolidateFlag == 1)
                            {
                                List<ControlSubstanceGridEntity> lst = GetControlSubstanceGridDataByPid(item.Patient_Id, item.AGiveCodeIdentifier, item.ConsolidateFlag);

                                string Text = "";

                                foreach (var a in lst)
                                {
                                    if (a.DFalg <= 0)
                                    {
                                        if (Text == "")
                                        {
                                            Text = a.Directions;
                                        }
                                        else
                                        {
                                            Text = Text + "\n" + a.Directions;
                                        }
                                    }

                                }

                                controlSubstance.Directions = Text;


                            }
                            if (item.Quantity != "")
                            {
                                controlSubstance.Quantity = Convert.ToDecimal(item.Quantity);
                                controlSubstance.InitialQuantity = Convert.ToDecimal(item.InitialQuantity);
                            }
                            else
                            {
                                controlSubstance.Quantity = null;
                                controlSubstance.InitialQuantity = null;
                            }
                            if (controlSubstance.Quantity != 0)
                            {
                                if ((controlSubstance.Porder_Status == 2 && controlSubstance.Quantity > 0) || ((listquery.PVisit_Status == 2 || listquery.PVisit_Status == 3) && controlSubstance.Quantity > 0))
                                {
                                    controlSubstance.QuantityZeroStatus = 1;
                                    controlData.Add(controlSubstance);
                                }
                                else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (listquery.PVisit_Status == 1 || (listquery.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                                {
                                    controlSubstance.QuantityZeroStatus = 0;
                                    controlData.Add(controlSubstance);
                                }
                            }
                            else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (listquery.PVisit_Status == 1 || (listquery.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                            {
                                controlSubstance.QuantityZeroStatus = 0;
                                controlData.Add(controlSubstance);
                            }
                        }
                    }
                }

                var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                if (claimsIdentity.FindFirst("UserId").Value != "")
                {
                    int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                    if (filter.NurseStations.Count() > 0)
                    {
                        var selectedNurseStations = string.Join(",", filter.NurseStations);
                        RecentFacEntity userRecentFacObj = new RecentFacEntity();
                        userRecentFacObj.User_Id = userId;
                        userRecentFacObj.Facility_Id = filter.Facilities[0];
                        userRecentFacObj.NurseStation_Id = selectedNurseStations;
                        this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                    }
                }
                //eKit Controlsubstance Records
                int facId = filter.Facilities[0];
                //12/09/2022 changed comapany id to facility id
                var companyID = this.dbContext.Facilities.Where(f => f.Facility_Id == facId).Select(f => f.Facility_Id).FirstOrDefault();
                List<ControlSubstanceGridEntity> ekitRecords = (from a in this.dbContext.Ekits
                                                                    //join user in this.dbContext.Users on a.Ekit_CreatedBy equals user.User_Id
                                                                join nur in this.dbContext.NursingStations on a.NurseStation_Id equals nur.NurseStation_Id into ns
                                                                from nur in ns.DefaultIfEmpty()
                                                                join fc in this.dbContext.Facilities on a.Facility_Id equals fc.Facility_Id
                                                                where a.Ekit_Status == 1 && a.ControlSubstance == 1 && ((filter.Facilities.Contains(fc.Facility_Id) && a.NurseStation_Id == null) || filter.NurseStations.Contains(nur.NurseStation_Id))
                                                                orderby nur.NurseStation_Name, a.DrugName
                                                                select new
                                                                {
                                                                    Ekit = a,
                                                                    NurseStation = nur,
                                                                    //User = user,
                                                                }).ToList()
                                  .Select(x => new ControlSubstanceGridEntity()
                                  {
                                      eKitFlag = 1,
                                      Ekit_Id = x.Ekit.Ekit_Id,
                                      PorderId = 0,
                                      ResidentName = "eKit",
                                      DOB = "",
                                      Order = x.Ekit.DrugName,
                                      LastCertified = x.Ekit.CertifiedBy != null ? ((this.dbContext.Users.Where(us => us.User_Id == x.Ekit.CertifiedBy).Select(us => us.User_DisplayName).FirstOrDefault())) : "None",
                                      CertifiedDate = x.Ekit.CertifiedDate != null ? GetTimeZoneDateTime(x.Ekit.CertifiedDate, companyID).ToString() : "None",
                                      Quantity = x.Ekit.InHand != null && x.Ekit.InHand != "" ? Convert.ToDecimal(x.Ekit.InHand) : 0,
                                      NurseStationName = x.NurseStation != null ? x.NurseStation.NurseStation_Name : "",
                                      Porder_Status = (int)x.Ekit.Ekit_Status,
                                      QuantityZeroStatus = 0,
                                      InitialQuantity = x.Ekit.InitialQuantity != null && x.Ekit.InitialQuantity != "" ? Convert.ToDecimal(x.Ekit.InitialQuantity) : 0,
                                      Patient_Id = 0,
                                      QuantityId = 0,
                                      GPI = "",
                                      ConsolidatedAllow = 0,
                                      ConsolidateFlag = 0,
                                      CheckInFlag = x.Ekit.CheckInFlag,
                                      AdministeredQty = x.Ekit.InHand != null && x.Ekit.InHand != "" ? Convert.ToDecimal(x.Ekit.InHand) : 0,
                                  }).ToList();

                controlData.AddRange(ekitRecords);
                var resultSet = controlData.Distinct();
                return resultSet.OrderBy(item => item.ResidentName).ThenBy(item => item.Order).ThenBy(item => item.ResidentName != "eKit").ThenBy(item => item.ResidentName == "eKit").ToList();

            }

            catch (Exception ex)
            {
                string errptid = orderid;
                return null;
            }
        }

        //old code

        public int SaveControlSubstance(ControlSubstanceSave controlObj, int createdby, int approvedby, DateTime approvedOn)
        {
            if (controlObj.eKitFlag == 0)
            {
                string[] controlIds = { "I", "II", "III", "IV", "V" };
                List<int> records = new List<int>();
                approvedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                var isOrderConsolidated = this.dbContext.ControlSubstanceCounts.Where(c => c.Porder_Id == controlObj.POrderId && c.PQuantity_Id == controlObj.PQuantity_Id).Select(c => c.ConsolidateFlag).FirstOrDefault();
                var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == controlObj.POrderId).Select(c => c.Patient_Id).FirstOrDefault();
                if (isOrderConsolidated == 1)
                {
                    var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == controlObj.POrderId).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                    var list = (from dm in this.dbContext.Demographics
                                join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                                join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                                //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                                join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                                join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                                //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                                where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1
                                //&& pq.ReviewFlag == 1
                                select new
                                {
                                    Controlsub = cs,
                                }).Distinct().ToList();
                    records = list.Select(l => (int)l.Controlsub.PQuantity_Id).ToList();
                }
                else
                {
                    records.Add(controlObj.PQuantity_Id);
                }

                if (records.Count() > 0)
                {
                    foreach (var item in records)
                    {
                        int PQuantity_Id = (int)item;
                        var VisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
                        var controlCheck = this.dbContext.ControlSubstanceCounts.Where(cs => cs.PQuantity_Id == PQuantity_Id).FirstOrDefault();
                        if (controlCheck == null)
                        {
                            ControlSubstanceCount record = new ControlSubstanceCount();
                            record.Porder_Id = controlObj.POrderId;
                            record.PQuantity_Id = controlObj.PQuantity_Id;
                            record.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
                            record.Quantity = controlObj.Quantity;
                            record.InitialQuantity = controlObj.Quantity;
                            record.CertifiedBy = createdby;
                            record.ApprovedBy = approvedby;
                            record.CertifiedDate = approvedOn;
                            record.ControlSubstance_Status = 1;
                            record.ControlSubstance_CreatedDate = approvedOn;
                            record.CheckInFlag = 0;
                            this.dbContext.ControlSubstanceCounts.Add(record);
                            this.dbContext.SaveChanges();

                            ControlSubstanceTran csTran = new ControlSubstanceTran();
                            csTran.ControlSubstance_Id = record.ControlSubstance_Id;
                            csTran.ConsolidateFlag = record.ConsolidateFlag;
                            csTran.Quantity = controlObj.Quantity;
                            csTran.CertifiedBy = createdby;
                            csTran.ApprovedBy = approvedby;
                            csTran.CertifiedDate = approvedOn;
                            csTran.TransCS_Status = 1;
                            csTran.TransCS_CreatedDate = approvedOn;
                            this.dbContext.ControlSubstanceTrans.Add(csTran);
                            this.dbContext.SaveChanges();

                            if (controlObj.DiscrepancyReason != "")
                            {
                                ControlSubstanceReason reasonRecord = new ControlSubstanceReason();
                                reasonRecord.ControlSubstance_Id = csTran.TransCS_Id;
                                reasonRecord.Reason = controlObj.DiscrepancyReason;
                                reasonRecord.CSReason_CreatedBy = createdby;
                                reasonRecord.CSReason_CreatedDate = approvedOn;
                                this.dbContext.ControlSubstanceReasons.Add(reasonRecord);
                                this.dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            //controlCheck.Porder_Id = controlObj.POrderId;
                            //controlCheck.PQuantity_Id = controlObj.PQuantity_Id;
                            controlCheck.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
                            controlCheck.Quantity = controlObj.Quantity;
                            controlCheck.InitialQuantity = controlObj.Quantity;
                            controlCheck.CertifiedBy = createdby;
                            controlCheck.ApprovedBy = approvedby;
                            controlCheck.CertifiedDate = approvedOn;
                            controlCheck.ControlSubstance_Status = 1;
                            controlCheck.ControlSubstance_CreatedDate = approvedOn;
                            controlCheck.CheckInFlag = 0;
                            this.dbContext.SaveChanges();

                            ControlSubstanceTran csTran = new ControlSubstanceTran();
                            csTran.ControlSubstance_Id = controlCheck.ControlSubstance_Id;
                            csTran.Quantity = controlObj.Quantity;
                            csTran.CertifiedBy = createdby;
                            csTran.ApprovedBy = approvedby;
                            csTran.CertifiedDate = approvedOn;
                            csTran.TransCS_Status = 1;
                            csTran.TransCS_CreatedDate = approvedOn;
                            this.dbContext.ControlSubstanceTrans.Add(csTran);
                            this.dbContext.SaveChanges();
                            if (controlObj.DiscrepancyReason != "")
                            {

                                ControlSubstanceReason reasonRecord = new ControlSubstanceReason();
                                reasonRecord.ControlSubstance_Id = csTran.TransCS_Id;
                                reasonRecord.Reason = controlObj.DiscrepancyReason;
                                reasonRecord.CSReason_CreatedBy = createdby;
                                reasonRecord.CSReason_CreatedDate = approvedOn;
                                this.dbContext.ControlSubstanceReasons.Add(reasonRecord);
                                this.dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            else if (controlObj.eKitFlag == 1)
            {
                approvedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                Ekit ekitRecord = this.dbContext.Ekits.Find(controlObj.Ekit_Id);
                if (ekitRecord != null)
                {
                    ekitRecord.CheckInFlag = 0;
                    ekitRecord.InHand = controlObj.Quantity;
                    ekitRecord.InitialQuantity = controlObj.Quantity;
                    ekitRecord.CertifiedBy = createdby;
                    ekitRecord.ApprovedBy = approvedby;
                    ekitRecord.CertifiedDate = approvedOn;
                    ekitRecord.Ekit_CreatedOn = approvedOn;
                    ekitRecord.CheckInFlag = 0;
                    this.dbContext.SaveChanges();

                    EKitControlSubstanceTran ekitTran = new EKitControlSubstanceTran();
                    ekitTran.Ekit_Id = ekitRecord.Ekit_Id;
                    ekitTran.Quantity = controlObj.Quantity;
                    ekitTran.CertifiedBy = createdby;
                    ekitTran.ApprovedBy = approvedby;
                    ekitTran.CertifiedDate = approvedOn;
                    ekitTran.EkTransCS_Status = 1;
                    ekitTran.EkTransCS_CreatedDate = approvedOn;
                    this.dbContext.EKitControlSubstanceTrans.Add(ekitTran);
                    this.dbContext.SaveChanges();
                    if (controlObj.DiscrepancyReason != "")
                    {

                        EkitControlSubstanceReason ekiReasonRecord = new EkitControlSubstanceReason();
                        ekiReasonRecord.Ekit_Id = ekitTran.EkTransCS_Id;
                        ekiReasonRecord.Reason = controlObj.DiscrepancyReason;
                        ekiReasonRecord.EkCSReason_CreatedBy = createdby;
                        ekiReasonRecord.EkCSReason_CreatedDate = approvedOn;
                        this.dbContext.EkitControlSubstanceReasons.Add(ekiReasonRecord);
                        this.dbContext.SaveChanges();
                    }
                }

            }
            this.dbContext.InsertOrderChangesforReport(controlObj.POrderId);
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.ControlSubstance,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = controlObj.POrderId.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public string GetOrderScheduleText(int porderId)
        {
            string scheduleText = string.Empty;
            var scheduleData = this.dbContext.DrugAdministrationTimes.Where(da => da.POrder_Id == porderId).FirstOrDefault();
            if (scheduleData != null)
            {
                var hour = (scheduleData.Hour_Id != null ? (this.dbContext.Hours.Where(hr => hr.Hour_Id == scheduleData.Hour_Id).Select(hr => hr.Hour_Desc).FirstOrDefault()) : "");
                var time = scheduleData.TimeFormat_Id == 1 ? "AM" : "PM";

                if (scheduleData.Monday == true)
                {
                    scheduleText = scheduleText + "Monday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Tuesday == true)
                {
                    scheduleText = scheduleText + "Tuesday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Wednesday == true)
                {
                    scheduleText = scheduleText + "Wednesday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Thursday == true)
                {
                    scheduleText = scheduleText + "Thursday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Friday == true)
                {
                    scheduleText = scheduleText + "Friday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Saturday == true)
                {
                    scheduleText = scheduleText + "Saturday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours, ";
                }
                if (scheduleData.Sunday == true)
                {
                    scheduleText = scheduleText + "Sunday ";
                    if (hour != null)
                        scheduleText = scheduleText + hour + " " + time + ", ";
                    else if (scheduleData.Hours > 0)
                        scheduleText = scheduleText + " Every " + scheduleData.Hours + " Hours";
                }
                return scheduleText;
            }
            else
            {
                return "";
            }
        }
        public List<AcknowledgeOrdersCustomEntity> GetAckOrders(int userId)
        {
            List<int> patientIds = null;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();


            patientIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate == null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
            if (patientIds != null)
            {
                var records = (from co in this.dbContext.CommonOrderInfoes
                               join pq in this.dbContext.QuantityDetails on co.POrder_Id equals pq.POrder_Id
                               join en in this.dbContext.EncodedOrderDetails on co.POrder_Id equals en.POrder_Id
                               join de in this.dbContext.Demographics on co.Patient_Id equals de.Patient_Id
                               join qa in this.dbContext.QuantityDetails on co.POrder_Id equals qa.POrder_Id
                               join ot in this.dbContext.OrderTypes on co.OrderTypeID equals ot.OrderTypeID
                               join fi in this.dbContext.FileInformations on co.File_Id equals fi.File_Id
                               where pq.OrderStatus == 1 && (co.POOutBoundApproval == 0 || co.POOutBoundApproval == null) &&
                               patientIds.Distinct().Contains(co.Patient_Id)
                               select new AcknowledgeOrdersCustomEntity
                               {
                                   POrder_Id = co.POrder_Id,
                                   FileId = (int)co.File_Id,
                                   FileName = fi.File_Name,
                                   PatientName = de.PatientLastName + " " + de.PatientFirstName,
                                   DrugName = en.GiveCodeText,
                                   OrderType = ot.OrderType_Desc,
                                   StartDate = qa.StartDate,
                                   ReceivedDate = co.POrder_CreatedDate,
                               }).ToList();
                return records;
            }
            return null;
        }
        public int AcceptAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            int PorderId = 0;
            foreach (var item in entity)
            {
                PorderId = item.POrder_Id;
                CommonOrderInfo record = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == PorderId).FirstOrDefault();
                if (record != null)
                {
                    record.POOutBoundApproval = item.ApprovalStatus;
                    record.POOutBoundApprovalBy = item.ApprovedBy;
                    record.POOutBoundApprovalOn = item.ApprovedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int RejectAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            int PorderId = 0;
            foreach (var item in entity)
            {
                PorderId = item.POrder_Id;
                CommonOrderInfo record = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == PorderId).FirstOrDefault();
                if (record != null)
                {
                    record.POOutBoundApproval = item.ApprovalStatus;
                    record.POOutBoundApprovalBy = item.ApprovedBy;
                    record.POOutBoundApprovalOn = item.ApprovedDate;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public List<DemographicResidentDropEnity> GetAckOrdersResidentDrop(int userId)
        {
            List<int> patientIds = null;
            var facility_nurseStationIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                            join ns in this.dbContext.NursingStations on us.NurseStation_Id equals ns.NurseStation_Id
                                            where us.User_Id == userId && ns.NurseStation_Status == 1
                                            select new
                                            {
                                                NurseStationId = ns.NurseStation_Id,
                                                FacilityId = us.Facility_id
                                            }).Distinct().ToList();

            List<int> facilityIds = facility_nurseStationIds.Select(f => f.FacilityId).Distinct().ToList();
            List<int> stationIds = facility_nurseStationIds.Select(f => f.NurseStationId).Distinct().ToList();


            patientIds = this.dbContext.VisitInfoes.Where(p => p.DischargeDate == null && facilityIds.Contains((int)p.FacilityId) && stationIds.Contains((int)p.NursingStationId)).Select(p => p.Patient_Id).ToList();
            if (patientIds != null)
            {
                var records = (from co in this.dbContext.CommonOrderInfoes
                               join pq in this.dbContext.QuantityDetails on co.POrder_Id equals pq.POrder_Id
                               join de in this.dbContext.Demographics on co.Patient_Id equals de.Patient_Id
                               where pq.OrderStatus == 1 && (co.POOutBoundApproval == 0 || co.POOutBoundApproval == null) && patientIds.Distinct().Contains(co.Patient_Id) && co.File_Id != null

                               select new DemographicResidentDropEnity
                               {
                                   Patient_Id = co.Patient_Id,
                                   PatientFirstName = de.PatientFirstName,
                                   PatientLastName = de.PatientLastName,
                                   PatientMiddleInitial = de.PatientMiddleInitial
                               }).Distinct().ToList();
                return records;
            }
            return null;
        }
        public List<AcknowledgeOrdersCustomEntity> GetAckOrdersByPatientId(int PatientId)
        {
            var records = (from co in this.dbContext.CommonOrderInfoes
                           join en in this.dbContext.EncodedOrderDetails on co.POrder_Id equals en.POrder_Id
                           join de in this.dbContext.Demographics on co.Patient_Id equals de.Patient_Id
                           join qa in this.dbContext.QuantityDetails on co.POrder_Id equals qa.POrder_Id
                           join ot in this.dbContext.OrderTypes on co.OrderTypeID equals ot.OrderTypeID
                           join fi in this.dbContext.FileInformations on co.File_Id equals fi.File_Id
                           where co.Patient_Id == PatientId
                           select new AcknowledgeOrdersCustomEntity
                           {
                               POrder_Id = co.POrder_Id,
                               FileId = (int)co.File_Id,
                               FileName = fi.File_Name,
                               PatientName = de.PatientLastName + " " + de.PatientFirstName,
                               DrugName = en.GiveCodeText,
                               OrderType = ot.OrderType_Desc,
                               StartDate = qa.StartDate,
                               ReceivedDate = co.POrder_CreatedDate,
                           }).ToList();
            return records;
        }
        public List<DrFirstFileDataEntity> GetDrFirstFilesData()
        {
            var records = (from dr in this.dbContext.DrFirstFileDatas
                           join de in this.dbContext.Demographics on dr.PatientMRNumber equals de.PatientMRNumber
                           join co in this.dbContext.Companies on dr.Company_Id equals co.Company_Id
                           orderby dr.ReceivedOn descending
                           select new DrFirstFileDataEntity
                           {
                               DrFirstFile_Id = dr.DrFirstFile_Id,
                               Company_Id = dr.Company_Id,
                               PatientMRNumber = dr.PatientMRNumber,
                               FilePath = dr.FilePath,
                               ReceivedOn = dr.ReceivedOn,
                               CompanyName = co.Company_Name,
                               PatientName = de.PatientLastName + ", " + de.PatientFirstName + " " + (de.PatientMiddleInitial != null ? de.PatientMiddleInitial : ""),

                           }).OrderBy(item => item.CompanyName).ThenBy(item => item.PatientName).ToList();
            return records;
        }

        #region
        //created by:sampath
        public List<OrderRouteEntity> GetOrderRoutes()
        {
            var data = (from ro in this.dbContext.Routes
                        where ro.Route_Status == 1
                        select new OrderRouteEntity
                        {
                            Route_Id = ro.Route_Id,
                            Route = ro.Route_Code + " -- " + ro.Route_Desc,
                        }).OrderBy(r => r.Route).ToList();
            return data;
        }

        public IList GetOrderGridData(int patientId, string status)
        {
            //if (status == "Active")
            //{
            //    var record = this.dbContext.PrcGetOrderGridData(patientId).Where(e => e.POrder_Status == 1).OrderBy(item => item.OrderType == "Literal").ThenBy(item => item.OrderType == "Literal(Treatments)").ThenBy(item => item.OrderType == "Drug").ToList();
            //    return record;
            //}
            //else if (status == "InActive")
            //{
            //    var record = this.dbContext.PrcGetOrderGridData(patientId).Where(e => e.POrder_Status != 1).OrderBy(item => item.OrderType == "Literal").ThenBy(item => item.OrderType == "Literal(Treatments)").ThenBy(item => item.OrderType == "Drug").ToList();
            //    return record;
            //}
            //else if (status == "AllActive")
            //{
            //    var record = this.dbContext.PrcGetOrderGridData(patientId).Where(e => e.POrder_Status == 1).OrderBy(item => item.OrderType == "Literal").ThenBy(item => item.OrderType == "Literal(Treatments)").ThenBy(item => item.OrderType == "Drug").ToList();
            //    return record;
            //}
            //return null;
            if ((status == "Active") || (status == "InActive") || (status == "AllActive"))
            {
                DataTable dt = new DataTable();
                string query = "[Patient].[PrcGetOrderGridData]";
                string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constrEmar))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId == null ? (object)DBNull.Value : patientId;


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                var records = (from d in dt.AsEnumerable()

                               where (status == "Active" && Convert.ToInt32(d["POrder_Status"]) == 1) ||
                               (status == "InActive" && Convert.ToInt32(d["POrder_Status"]) != 1) ||
                               (status == "AllActive" && Convert.ToInt32(d["POrder_Status"]) == 1)

                               select new
                               {
                                   porder_Id = Convert.ToInt32(d["porder_Id"]),
                                   PQuantity_Id = Convert.ToInt32(d["PQuantity_Id"]),
                                   //DAdmin_Id = Convert.ToInt32(d["DAdmin_Id"]),
                                   DAdmin_Id = string.IsNullOrEmpty(d["DAdmin_Id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DAdmin_Id"]),
                                   OrderType = d["OrderType"].ToString(),
                                   DrugName = d["DrugName"].ToString(),
                                   Directions = d["Directions"].ToString(),
                                   StartDate = string.IsNullOrEmpty(d["StartDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["StartDate"]),//date  //na
                                   EndDate = string.IsNullOrEmpty(d["EndDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["EndDate"]),//date  //na
                                   POrder_Status = Convert.ToInt32(d["POrder_Status"]),
                                   ReviewFlag = Convert.ToInt32(d["ReviewFlag"]),
                                   HoldStatus = Convert.ToInt32(d["HoldStatus"]),
                                   scheduleTimeflag = Convert.ToInt32(d["scheduleTimeflag"]),
                                   DiscontinueReason = d["DiscontinueReason"].ToString(),
                                   DiscontinuedBy = d["DiscontinuedBy"].ToString(),
                                   //DiscontinueFlag = Convert.ToInt32(d["DiscontinueFlag"]),
                                   DiscontinueFlag = string.IsNullOrEmpty(d["DiscontinueFlag"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DiscontinueFlag"]),
                                   OrderCreatedBy = d["OrderCreatedBy"].ToString(),
                                   OrderCreatedDate = string.IsNullOrEmpty(d["StartDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["OrderCreatedDate"]),//date  //na

                                   //OrderCreatedDate = d["OrderCreatedDate"].ToString(),
                                   HoldFrom = d["Hold From"].ToString(),
                                   On_Hold_Until = d["On Hold Until"].ToString(),

                                   PRNFlag = Convert.ToInt32(d["PRNFlag"]),
                                   Schedule = d["Schedule"].ToString(),
                                   split = Convert.ToInt32(d["split"]),
                                   NumberOfRefillsRemaining = d["NumberOfRefillsRemaining"].ToString(),





                               }).OrderBy(item => item.OrderType == "Literal").ThenBy(item => item.OrderType == "Literal(Treatments)").ThenBy(item => item.OrderType == "Drug").ToList();

                return records;
            }
            else
            {
                return null;
            }
        }
        public List<LiteralOrderEntity> GetLiteralOrderGridData(int patientId)
        {
            List<LiteralOrderEntity> orderslist = new List<LiteralOrderEntity>();
            var record = this.dbContext.PrcGetOrderGridData(patientId).Where(e => e.POrder_Status == 1 && e.OrderType == "Literal").ToList();
            for (int i = 0; i < record.Count; i++)
            {
                int physicianId = record[i].porder_Id;
                var directions = record[i].Directions;
                var startdate = record[i].StartDate;
                var createdBy = record[i].OrderCreatedBy;
                var createdDate = record[i].OrderCreatedDate;
                var data = (from pd in this.dbContext.PhysicianDetails
                            join co in this.dbContext.CommonOrderInfoes on pd.Physician_Id equals co.OrderingPhysicianID
                            where co.POrder_Id == physicianId
                            select new LiteralOrderEntity
                            {
                                Directions = directions,
                                StartDate = startdate,
                                PhysicianName = pd.PhysicianLName + ", " + pd.PhysicianFName,
                                OrderCreatedBy = createdBy,
                                OrderCreatedDate = createdDate,
                            }).FirstOrDefault();
                if (data != null)
                    orderslist.Add(data);
                else
                {
                    LiteralOrderEntity liObj = new LiteralOrderEntity();
                    liObj.Directions = directions;
                    liObj.StartDate = startdate;
                    liObj.PhysicianName = "";
                    liObj.OrderCreatedBy = createdBy;
                    liObj.OrderCreatedDate = Convert.ToDateTime(createdDate);
                    orderslist.Add(liObj);
                }
            }
            return orderslist;
        }
        public OrdersDataEntity GetOrdersData(int orderId, int quantityId, int userId)
        {

            //var data = this.dbContext.PrcGetOrdersdata(orderId, quantityId).FirstOrDefault();
            //return this.autoMapper.Map<PrcGetOrdersdata_Result, OrdersDataEntity>(data);

            DataTable dt = new DataTable();
            string query = "[Patient].[PrcGetOrdersdata]";
            //string query = "[Admin].[PrcGetPhysicianData_Sv]";

            //OrdersDataEntity data = new OrdersDataEntity();
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@PorderId", SqlDbType.Int).Value = orderId == null ? (object)DBNull.Value : orderId;
                    cmd.Parameters.Add("@PquantityId", SqlDbType.Int).Value = quantityId == null ? (object)DBNull.Value : quantityId;
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId == null ? (object)DBNull.Value : userId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                    /*using (SqlDataReader d = cmd.ExecuteReader())
                    {
                        while (d.Read())
                        {
                            data.porder_Id = Convert.ToInt32(d["porder_Id"]);

                            data.StartDate = string.IsNullOrEmpty(d["StartDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["StartDate"]);//date  //na
                            data.EndDate = string.IsNullOrEmpty(d["EndDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["EndDate"]); //date  //na
                            data.WrittenDate = string.IsNullOrEmpty(d["WrittenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["WrittenDate"]); //nullable date //na

                            data.DispenseQty = string.IsNullOrEmpty(d["DispenseQty"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["DispenseQty"]); //nullable decimal //na
                            data.MaxPerdays = string.IsNullOrEmpty(d["MaxPerdays"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["MaxPerdays"]); //nullable decimal
                            data.Dayssupply = string.IsNullOrEmpty(d["Dayssupply"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["Dayssupply"]); //nullable decimal


                            data.OrderStockFlag = string.IsNullOrEmpty(d["OrderStockFlag"].ToString()) ? false : Convert.ToBoolean(d["OrderStockFlag"]); //bool
                            data.PRNFlag = string.IsNullOrEmpty(d["PRNFlag"].ToString()) ? false : Convert.ToBoolean(d["PRNFlag"]); //bool
                            data.TreatmentFlag = string.IsNullOrEmpty(d["TreatmentFlag"].ToString()) ? false : Convert.ToBoolean(d["TreatmentFlag"]); //bool
                            data.SelfAdministeredFlag = string.IsNullOrEmpty(d["SelfAdministeredFlag"].ToString()) ? false : Convert.ToBoolean(d["SelfAdministeredFlag"]); //bool



                            data.split = Convert.ToInt32(d["split"]);
                            data.PQuantity_Id = Convert.ToInt32(d["PQuantity_Id"]);
                            data.Route_Id = Convert.ToInt32(d["Route_Id"]);
                            data.CpoeFlag = Convert.ToInt32(d["CpoeFlag"].ToString());
                            data.HoldStatus = Convert.ToInt32(d["HoldStatus"]);
                            data.POrder_Status = Convert.ToInt32(d["POrder_Status"]);
                            data.DestroyStatus = Convert.ToInt32(d["DestroyStatus"]);
                            data.ReviewFlag = Convert.ToInt32(d["ReviewFlag"]);
                            data.MergeFlag = Convert.ToInt32(d["MergeFlag"]);
                            data.ControlSubstanceBit = Convert.ToInt32(d["ControlSubstanceBit"]);
                            data.scheduleTimeflag = Convert.ToInt32(d["scheduleTimeflag"]);
                            data.Favouriteflag = Convert.ToInt32(d["Favouriteflag"]);
                            data.MedispanControlSubBit = Convert.ToInt32(d["MedispanControlSubBit"]);
                            data.Refill_Request = Convert.ToInt32(d["Refill Request"]);


                            ////
                            data.ControlSubCreatedBy = (d["ControlSubCreatedBy"] == null || d["ControlSubCreatedBy"] == "") ? (Int32?)null : Convert.ToInt32(d["ControlSubCreatedBy"]); //nullable int
                           data.Stock_Id = string.IsNullOrEmpty(d["Stock_Id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Stock_Id"]); //nullable int

                            data.OrderTypeID = string.IsNullOrEmpty(d["OrderTypeID"].ToString()) ? (Int32?)null : Convert.ToInt32(d["OrderTypeID"]); //nullable int



                            data.DiscontinueFlag = string.IsNullOrEmpty(d["DiscontinueFlag"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DiscontinueFlag"]); //nullable int
                           data.DiagIndication = string.IsNullOrEmpty(d["DiagIndication"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DiagIndication"]); //nullable int
                            data.Daw = string.IsNullOrEmpty(d["Daw"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Daw"]); //nullable int
                           data.WaitforPharmacy = string.IsNullOrEmpty(d["WaitforPharmacy"].ToString()) ? (Int32?)null : Convert.ToInt32(d["WaitforPharmacy"]); //nullable int
                            data.Hospice = string.IsNullOrEmpty(d["Hospice"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Hospice"]); //nullable int

                            data.Source = string.IsNullOrEmpty(d["Source"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Source"]); //nullable int
                            data.UOM = string.IsNullOrEmpty(d["UOM"].ToString()) ? (Int32?)null : Convert.ToInt32(d["UOM"]); //nullable int
                            data.DUom = string.IsNullOrEmpty(d["DUom"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DUom"]); //nullable int


                            data.SchFlag = string.IsNullOrEmpty(d["SchFlag"].ToString()) ? (Int32?)null : Convert.ToInt32(d["SchFlag"]); //nullable int



                            data.DiagIndicationText = Convert.ToString(d["DiagIndicationText"]);
                            data.Inhand = Convert.ToString(d["Inhand"]);
                            data.NursingFreq_Id = Convert.ToString(d["NursingFreq_Id"]); //nullable int
                            data.NurseShifts_Id = Convert.ToString(d["NurseShifts_Id"]);
                            data.DrugName = Convert.ToString(d["DrugName"]);
                            data.Quantity = Convert.ToString(d["Quantity"]);
                            data.Directions = Convert.ToString(d["Directions"]);
                            data.OrderingPhysicianNPI = Convert.ToString(d["OrderingPhysicianNPI"]);
                            data.Refill = Convert.ToString(d["Refill"]);
                            data.AlertText = Convert.ToString(d["AlertText"]);
                            data.InsulinComments = Convert.ToString(d["InsulinComments"]);
                            data.PharmacyName = Convert.ToString(d["PharmacyName"]);
                            data.OrderOrigin = Convert.ToString(d["OrderOrigin"]);
                            data.AGiveCodeIdentifier = Convert.ToString(d["AGiveCodeIdentifier"]);
                            data.Barcode = Convert.ToString(d["Barcode"]);
                            data.Schedule = Convert.ToString(d["Schedule"]);
                            data.Notes = Convert.ToString(d["Notes"]);
                            data.Refill_Note = Convert.ToString(d["Refill Note"]);
                        }
                    }*/
                }
            }

            var data = (from d in dt.AsEnumerable()
                        select new OrdersDataEntity
                        {
                            porder_Id = Convert.ToInt32(d["porder_Id"]),

                            StartDate = string.IsNullOrEmpty(d["StartDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["StartDate"]),//date  //na
                            EndDate = string.IsNullOrEmpty(d["EndDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["EndDate"]), //date  //na
                            WrittenDate = string.IsNullOrEmpty(d["WrittenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["WrittenDate"]), //nullable date //na

                            DispenseQty = string.IsNullOrEmpty(d["DispenseQty"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["DispenseQty"]), //nullable decimal //na
                            MaxPerdays = string.IsNullOrEmpty(d["MaxPerdays"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["MaxPerdays"]), //nullable decimal
                            Dayssupply = string.IsNullOrEmpty(d["Dayssupply"].ToString()) ? (decimal?)null : Convert.ToDecimal(d["Dayssupply"]), //nullable decimal


                            OrderStockFlag = string.IsNullOrEmpty(d["OrderStockFlag"].ToString()) ? false : Convert.ToBoolean(d["OrderStockFlag"]), //bool
                            PRNFlag = string.IsNullOrEmpty(d["PRNFlag"].ToString()) ? false : Convert.ToBoolean(d["PRNFlag"]), //bool
                            TreatmentFlag = string.IsNullOrEmpty(d["TreatmentFlag"].ToString()) ? false : Convert.ToBoolean(d["TreatmentFlag"]), //bool
                            SelfAdministeredFlag = string.IsNullOrEmpty(d["SelfAdministeredFlag"].ToString()) ? false : Convert.ToBoolean(d["SelfAdministeredFlag"]), //bool



                            split = Convert.ToInt32(d["split"]),
                            PQuantity_Id = Convert.ToInt32(d["PQuantity_Id"]),
                            Route_Id = Convert.ToInt32(d["Route_Id"]),
                            CpoeFlag = Convert.ToInt32(d["CpoeFlag"].ToString()),
                            HoldStatus = Convert.ToInt32(d["HoldStatus"]),
                            POrder_Status = Convert.ToInt32(d["POrder_Status"]),
                            DestroyStatus = Convert.ToInt32(d["DestroyStatus"]),
                            ReviewFlag = Convert.ToInt32(d["ReviewFlag"]),
                            MergeFlag = Convert.ToInt32(d["MergeFlag"]),
                            ControlSubstanceBit = Convert.ToInt32(d["ControlSubstanceBit"]),
                            scheduleTimeflag = Convert.ToInt32(d["scheduleTimeflag"]),
                            Favouriteflag = Convert.ToInt32(d["Favouriteflag"]),
                            MedispanControlSubBit = Convert.ToInt32(d["MedispanControlSubBit"]),
                            Refill_Request = Convert.ToInt32(d["Refill Request"]),


                            ////
                            ControlSubCreatedBy = (d["ControlSubCreatedBy"] == null || d["ControlSubCreatedBy"] == "") ? (Int32?)null : Convert.ToInt32(d["ControlSubCreatedBy"]), //nullable int
                            Stock_Id = string.IsNullOrEmpty(d["Stock_Id"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Stock_Id"]), //nullable int

                            OrderTypeID = string.IsNullOrEmpty(d["OrderTypeID"].ToString()) ? (Int32?)null : Convert.ToInt32(d["OrderTypeID"]), //nullable int



                            DiscontinueFlag = string.IsNullOrEmpty(d["DiscontinueFlag"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DiscontinueFlag"]), //nullable int
                            DiagIndication = string.IsNullOrEmpty(d["DiagIndication"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DiagIndication"]), //nullable int
                            Daw = string.IsNullOrEmpty(d["Daw"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Daw"]), //nullable int
                            WaitforPharmacy = string.IsNullOrEmpty(d["WaitforPharmacy"].ToString()) ? (Int32?)null : Convert.ToInt32(d["WaitforPharmacy"]), //nullable int
                            Hospice = string.IsNullOrEmpty(d["Hospice"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Hospice"]), //nullable int

                            Source = string.IsNullOrEmpty(d["Source"].ToString()) ? (Int32?)null : Convert.ToInt32(d["Source"]), //nullable int
                            UOM = string.IsNullOrEmpty(d["UOM"].ToString()) ? (Int32?)null : Convert.ToInt32(d["UOM"]), //nullable int
                            DUom = string.IsNullOrEmpty(d["DUom"].ToString()) ? (Int32?)null : Convert.ToInt32(d["DUom"]), //nullable int


                            SchFlag = string.IsNullOrEmpty(d["SchFlag"].ToString()) ? (Int32?)null : Convert.ToInt32(d["SchFlag"]), //nullable int



                            DiagIndicationText = Convert.ToString(d["DiagIndicationText"]),
                            Inhand = Convert.ToString(d["Inhand"]),
                            NursingFreq_Id = Convert.ToString(d["NursingFreq_Id"]), //nullable int
                            NurseShifts_Id = Convert.ToString(d["NurseShifts_Id"]),
                            DrugName = Convert.ToString(d["DrugName"]),
                            Quantity = Convert.ToString(d["Quantity"]),
                            Directions = Convert.ToString(d["Directions"]),
                            OrderingPhysicianNPI = Convert.ToString(d["OrderingPhysicianNPI"]),
                            Refill = Convert.ToString(d["Refill"]),
                            AlertText = Convert.ToString(d["AlertText"]),
                            InsulinComments = Convert.ToString(d["InsulinComments"]),
                            PharmacyName = Convert.ToString(d["PharmacyName"]),
                            OrderOrigin = Convert.ToString(d["OrderOrigin"]),
                            AGiveCodeIdentifier = Convert.ToString(d["AGiveCodeIdentifier"]),
                            Barcode = Convert.ToString(d["Barcode"]),
                            Schedule = Convert.ToString(d["Schedule"]),
                            Notes = Convert.ToString(d["Notes"]),
                            Refill_Note = Convert.ToString(d["Refill Note"]),
                            AutoBarcode = Convert.ToString(d["AutoBarcode"]),





                        }).FirstOrDefault();

            return data;

        }

        public int CheckBarcodeAlert(string BarcodeData, string GpiNum, int patientId, int facilityId, int orderId)
        {
            int gpimatch = 0; // Default value in case of no match
            string query = "[dbo].[Prc_BarCodeMatchWithGPIAlerts]";

            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = BarcodeData == null ? (object)DBNull.Value : BarcodeData;
                    cmd.Parameters.Add("@RequestedGiveCode", SqlDbType.VarChar).Value = GpiNum == null ? (object)DBNull.Value : GpiNum;
                    cmd.Parameters.Add("@patientid", SqlDbType.Int).Value = patientId == null ? (object)DBNull.Value : patientId;
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId == null ? (object)DBNull.Value : facilityId;
                    cmd.Parameters.Add("@porder_id", SqlDbType.Int).Value = orderId == null ? (object)DBNull.Value : orderId;



                    object result = cmd.ExecuteScalar(); // Execute scalar as we expect a single value
                    if (result != DBNull.Value && result != null)
                    {
                        gpimatch = Convert.ToInt32(result); // Convert result to integer
                    }
                }
            }

            return gpimatch;
        }
        public int InsertOrderCommonStatus(OrdersCommonStatusEntity obj)
        {
            obj.UpdatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());

            if (obj.OrderType == "Review")
            {
                var data = this.dbContext.QuantityDetails.Where(e => e.POrder_Id == obj.OrderId && e.PQuantity_Id == obj.QuantityId).FirstOrDefault();
                if (data != null)
                {
                    //data.POOutBoundApproval = obj.POOutBoundApproval;
                    //data.POOutBoundApprovalBy = obj.POOutBoundApprovalBy;
                    //data.POOutBoundApprovalOn = DateTime.Now;
                    data.DiscontinueFlag = null;
                    data.ReviewFlag = 1;
                    this.dbContext.SaveChanges();
                    if (this.dbContext.DrugAdministers.Where(d => d.DAdmin_Id == obj.DAdminId && d.is_deleted == null).Count() == 0)
                        return this.dbContext.PrcOrderScheduleTime(obj.OrderId, obj.DAdminId);
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Orders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = obj.OrderId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            else if (obj.OrderType == "Destroy")
            {
                var data = this.dbContext.QuantityDetails.Where(e => e.POrder_Id == obj.OrderId && e.PQuantity_Id == obj.QuantityId).FirstOrDefault();
                if (data != null)
                {
                    data.OrderStatus = obj.POrderStatus;
                    data.PQuantity_CreatedBy = obj.POrderCreatedBy;
                    data.PQuantity_CreatedDate = (DateTime)obj.UpdatedOn;
                    this.dbContext.SaveChanges();
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Orders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = obj.OrderId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            else if (obj.OrderType == "Reactivate")
            {
                var data = this.dbContext.QuantityDetails.Where(e => e.POrder_Id == obj.OrderId && e.PQuantity_Id == obj.QuantityId).FirstOrDefault();
                if (data != null)
                {
                    data.OrderStatus = obj.POrderStatus;
                    data.EndDate = null;
                    data.DiscontinueFlag = null;
                    data.PQuantity_CreatedBy = obj.POrderCreatedBy;
                    data.PQuantity_CreatedDate = (DateTime)obj.UpdatedOn;
                    this.dbContext.SaveChanges();
                    // Remove shared barcode
                    var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == obj.OrderId).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                    var patient_Id = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == obj.OrderId).Select(c => c.Patient_Id).FirstOrDefault();
                    var patientOrders = (from en in this.dbContext.EncodedOrderDetails
                                         join co in this.dbContext.CommonOrderInfoes on en.POrder_Id equals co.POrder_Id
                                         join de in this.dbContext.Demographics on co.Patient_Id equals de.Patient_Id
                                         where en.AGiveCodeIdentifier == gpi && co.Patient_Id == patient_Id && en.POrder_Id != obj.OrderId
                                         select new OrderStockEntity
                                         {
                                             PorderId = en.POrder_Id,
                                         }).Distinct().ToList();
                    if (patientOrders.Count() > 0)
                    {
                        var resOrders = patientOrders.Select(s => s.PorderId).Distinct().ToList();
                        var orderBarcodes = this.dbContext.BarcodeDetails.Where(b => b.POrder_Id == obj.OrderId).Select(b => b.BarcodeDetail1).Distinct().ToList();
                        var barcodes = this.dbContext.BarcodeDetails.Where(br => resOrders.Contains(br.POrder_Id) && orderBarcodes.Contains(br.BarcodeDetail1)).Select(br => br.BarcodeDetail1).Distinct().ToList();
                        var removeBarcodes = this.dbContext.BarcodeDetails.Where(ba => ba.POrder_Id == obj.OrderId && barcodes.Contains(ba.BarcodeDetail1)).Distinct().ToList();
                        if (removeBarcodes.Count() > 0)
                        {
                            this.dbContext.BarcodeDetails.RemoveRange(removeBarcodes);
                            this.dbContext.SaveChanges();
                        }
                        var record = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == obj.OrderId).FirstOrDefault();
                        //if(record!=null)
                        //{
                        //    record.POrder_CreatedBy = obj.POrderCreatedBy;
                        //    record.POrder_CreatedDate = DateTime.Now;
                        //    this.dbContext.SaveChanges();
                        //}
                        if (record != null && record.OrderStockFlag == false)
                        {
                            var stock = this.dbContext.Stocks.Where(st => st.GPICode == gpi).FirstOrDefault();
                            var stockBarcodes = this.dbContext.BarcodeDetails.Where(br => br.Stock_Id == stock.Stock_Id && br.PBarcode_Status == 1).ToList();
                            if (stockBarcodes.Count() > 0)
                            {
                                foreach (var item in stockBarcodes)
                                {
                                    if (this.dbContext.BarcodeDetails.Where(br => br.POrder_Id == obj.OrderId && br.BarcodeDetail1.ToLower() == item.BarcodeDetail1.ToLower()).FirstOrDefault() == null)
                                    {
                                        BarcodeDetail brObj = new BarcodeDetail();
                                        brObj.POrder_Id = obj.OrderId;
                                        brObj.BarcodeDetail1 = item.BarcodeDetail1;
                                        brObj.PBarcode_Status = 1;
                                        brObj.PBarcode_CreatedBy = obj.POrderCreatedBy;
                                        brObj.PBarcode_CreatedDate = (DateTime)obj.UpdatedOn;
                                        this.dbContext.BarcodeDetails.Add(brObj);
                                        this.dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Orders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = obj.OrderId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            //var recordUpdate = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == obj.OrderId).FirstOrDefault();
            //if (recordUpdate != null)
            //{
            //    recordUpdate.POrder_CreatedBy = obj.POrderCreatedBy;
            //    recordUpdate.POrder_CreatedDate = DateTime.Now;
            //    this.dbContext.SaveChanges();
            //}
            return 1;
        }
        public int UpdateOrdersDatabyOrderId(OrderUpdateEntity obj)
        {
            //var data = this.dbContext.QuantityDetails.Where(e => e.POrder_Id == obj.POrderId && e.PQuantity_Id == obj.PQuantityId).FirstOrDefault();
            //var existingEndDate = data.EndDate;

            var record = this.dbContext.PrcOrderUpdate(obj.POrderId, obj.PQuantityId, obj.PhysicianId, obj.DrugName, obj.Quantity,
            obj.Directions, obj.StartDate, obj.EndDate, obj.NumberofRefills, obj.MaxPerdays, obj.Alerttext,
            obj.InsulinComments, obj.OrderstockFlag, obj.PRNFlag, obj.TreatmentFlag, obj.SelfAdministeredFlag,

            obj.Route, obj.Inhand, obj.Createdby, obj.Barcode, obj.controlSubstance, obj.orderTypeID, obj.RequestedGiveCode).FirstOrDefault();

            if (record == 2)
            {
                return 2;
            }

            else
            {
                this.InsertScheduleTimeText(Convert.ToInt32(obj.POrderId), Convert.ToInt32(obj.PQuantityId), obj.ScheduleText);
                //if (data.EndDate != obj.EndDate)
                //    return this.dbContext.PrcOrderScheduleTime(obj.POrderId, obj.DAdminId);

                var orderUpdate = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == obj.POrderId).FirstOrDefault();
                if (orderUpdate != null)
                {
                    orderUpdate.POrder_CreatedBy = obj.Createdby;
                    orderUpdate.POrder_CreatedDate = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Orders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = obj.POrderId.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return 1;

            }
        }
        public int UpdateOrderHoldStatus(OrderHoldEntity obj)
        {
            //var record = this.autoMapper.Map<OrderHoldEntity, OrderHold>(obj);
            //var porder = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == obj.PQuantity_Id).FirstOrDefault().POrder_Id;
            obj.OrderHold_CreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var data = this.dbContext.OrderHolds.OrderByDescending(e => e.OrderHold_CreatedDate).Where(e => e.PQuantity_Id == obj.PQuantity_Id).FirstOrDefault();
            if (data != null)
            {

                var porder = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == obj.PQuantity_Id).FirstOrDefault();
                if (porder != null)
                {
                    var comOrder = this.dbContext.CommonOrderInfoes.Where(cm => cm.POrder_Id == porder.POrder_Id).FirstOrDefault();
                    comOrder.POrder_CreatedBy = obj.OrderHold_CreatedBy;
                    comOrder.POrder_CreatedDate = obj.OrderHold_CreatedDate.Value;
                    this.dbContext.SaveChanges();
                    this.dbContext.InsertOrderChangesforReport(porder.POrder_Id);

                }
                data.OrderHold_Status = obj.OrderHold_Status;
                data.OrderHold_CreatedBy = obj.OrderHold_CreatedBy;
                data.OrderHold_CreatedDate = obj.OrderHold_CreatedDate;
                this.dbContext.SaveChanges();
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = obj.PQuantity_Id.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public string InsertupdateHOA(HOAEntity obj)
        {
            if (obj.hours == 0)
                obj.hours = null;
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Orders,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                Comments = obj.porderId.ToString(),
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };
            //int weekId = obj.weekId!=null? (Convert.ToInt32(obj.weekId.Split(',')[0])):0;
            //int monthId = obj.monthId != null ? Convert.ToInt32(obj.monthId.Split(',')[0]):0;

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            var result = this.dbContext.PrcInsertupdateHOA(obj.dadminId, obj.porderId, obj.pquantityId, obj.freqId, obj.NurseShiftId, obj.hourIds,
                               obj.hours, obj.monday, obj.tuesday, obj.wednesday, obj.thursday, obj.friday, obj.saturday, obj.sunday, obj.weekId, obj.monthId, obj.days, obj.createdby, obj.activedays, obj.holddays).FirstOrDefault();

            if (result == "Critical")
            {
                return "2";
            }
            else
            {
                this.InsertScheduleTimeText(Convert.ToInt32(obj.porderId), Convert.ToInt32(obj.pquantityId), result);
                return result;
            }

        }

        public IList GetStockQtyonHand(string drugName, int nurseStationId)
        {
            string[] controlIds = { "I", "II", "III", "IV", "V" };

            var data = (from i in this.dbContext.Stocks
                            //join s in this.dbContext.NursingStations on i.NurseStation_Id equals s.NurseStation_Id
                        join f in this.dbContext.BarcodeDetails on i.Stock_Id equals f.Stock_Id into barcodes
                        where i.DrugName.StartsWith(drugName) && i.Stock_Status == 1 && i.NurseStation_Id == nurseStationId
                        select new
                        {
                            stock = i,
                            bc = barcodes,
                            //ns = s
                        }).AsEnumerable().
                        Select(sb => new StockEntity
                        {
                            Stock_Id = sb.stock.Stock_Id,
                            //NurseStationName = sb.ns.NurseStation_Name,
                            InHand = sb.stock.InHand,
                            DrugName = sb.stock.DrugName,
                            Barcode = string.Join(", ", sb.bc.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).ToList()),
                            GPICode = sb.stock.GPICode,
                            //  ControlledSubstanceSchedule = controlIds.Contains(this.dbContext.DrugOrderMasters.Where(d => d.GPI == sb.stock.GPICode).Select(d => d.ControlledSubstanceSchedule).FirstOrDefault()) == true ? 1 : 0,
                        }).ToList();

            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();

            var shareddata = (from i in this.dbContext.Stocks
                                  //join s in this.dbContext.NursingStations on i.NurseStation_Id equals s.NurseStation_Id
                              join f in this.dbContext.BarcodeDetails on i.Stock_Id equals f.Stock_Id into barcodes
                              where i.DrugName.StartsWith(drugName) && i.Stock_Status == 1 && i.NurseStation_Id == null && i.Facility_Id == facilityId && i.Sharedstockbit == 1
                              select new
                              {
                                  stock = i,
                                  bc = barcodes,
                                  //ns = s
                              }).AsEnumerable().
                        Select(sb => new StockEntity
                        {
                            Stock_Id = sb.stock.Stock_Id,
                            //NurseStationName = sb.ns.NurseStation_Name,
                            InHand = sb.stock.InHand,
                            DrugName = sb.stock.DrugName,
                            Barcode = string.Join(", ", sb.bc.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).ToList()),
                            GPICode = sb.stock.GPICode,
                            // ControlledSubstanceSchedule = controlIds.Contains(this.dbContext.DrugOrderMasters.Where(d => d.GPI == sb.stock.GPICode).Select(d => d.ControlledSubstanceSchedule).FirstOrDefault()) == true ? 1 : 0,
                        }).ToList();
            if (shareddata.Count() > 0)
                data.AddRange(shareddata);

            if (data.Count > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    string gpicheck = data[i].GPICode.ToString();
                    var barcodeString = (from st in this.dbContext.Stocks
                                         join br in this.dbContext.BarcodeDetails on st.Stock_Id equals br.Stock_Id
                                         where st.GPICode == gpicheck && ((st.NurseStation_Id != null && st.NurseStation_Id == nurseStationId && st.Facility_Id == null) || (st.Facility_Id != null && st.Facility_Id == facilityId && st.NurseStation_Id == null) || (st.NurseStation_Id == nurseStationId && st.Facility_Id == facilityId))
                                         select new { br.BarcodeDetail1 }).ToList();
                    if (barcodeString.Count > 0)
                        data[i].Barcode = string.Join(", ", barcodeString.Select(b => b.BarcodeDetail1).ToList());
                }
            }
            return data;
        }
        //public IList SearchDrugName(string drugName)
        //{
        //    string[] controlIds = { "I", "II", "III", "IV", "V" };
        //    var data = (from i in this.dbContext.DrugOrderMasters
        //                    //join f in this.dbContext.BarcodeDetails on i.Stock_Id equals f.Stock_Id
        //                where i.DrugName.StartsWith(drugName) && i.DrugOrderMaster_Status == 1
        //                select new
        //                {
        //                    Drug_Id = i.DrugOrderMaster_Id,
        //                    DrugName = i.DrugName,
        //                    DosageForm = i.DosageForm,
        //                    Strength = i.Strength,
        //                    GPICode = i.GPI,
        //                    ControlledSubstanceSchedule = controlIds.Contains(i.ControlledSubstanceSchedule) == true ? 1 : 0,
        //                    Route = i.Route,
        //                    RouteDescription = i.RouteDescription
        //                }).ToList();
        //    return data;
        //    //string[] controlIds = { "I", "II", "III", "IV", "V" };

        //    //// Query to fetch records without grouping
        //    //var query = from i in this.dbContext.DrugOrderMasters
        //    //            where i.DrugName.StartsWith(drugName) && i.DrugOrderMaster_Status == 1
        //    //            select new
        //    //            {
        //    //                Drug_Id = i.DrugOrderMaster_Id,
        //    //                DrugName = i.DrugName,
        //    //                DosageForm = i.DosageForm,
        //    //                Strength = i.Strength,
        //    //                GPICode = i.GPI,
        //    //                ControlledSubstanceSchedule = controlIds.Contains(i.ControlledSubstanceSchedule) ? 1 : 0,
        //    //                Route = i.Route,
        //    //                RouteDescription = i.RouteDescription
        //    //            };


        //    //var data = query.ToList();


        //    //var distinctData = data
        //    //    .GroupBy(d => new { d.DrugName, d.GPICode })
        //    //    .Select(g => g.First())
        //    //    .ToList();

        //    //return distinctData;
        //}
        //DTMS (12/9/2025) start
        
          
        public List<DrugNameSearch> SearchDrugName(string drugName)
        {
            DataTable dt = new DataTable();
            string query = "[Admin].[Prc_GetMedispanDrugOrderMaster]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@searchDrugname", SqlDbType.VarChar).Value = drugName;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            var result = (from d in dt.AsEnumerable()
                          select new DrugNameSearch
                          {
                              Drug_Id = d["Drug_Id"] != DBNull.Value ? Convert.ToInt32(d["Drug_Id"]) : (int?)null,
                              DrugName = d["DrugName"] != DBNull.Value ? d["DrugName"].ToString() : null,
                              DosageForm = d["DosageForm"] != DBNull.Value ? d["DosageForm"].ToString() : null,
                              Strength = d["Strength"] != DBNull.Value ? d["Strength"].ToString() : null,
                              GPICode = d["GPICode"] != DBNull.Value ? d["GPICode"].ToString() : null,
                              ControlledSubstanceSchedule = d["ControlledSubstanceSchedule"] != DBNull.Value ? Convert.ToInt32(d["ControlledSubstanceSchedule"]) : (int?)null,
                              Route = d["Route"] != DBNull.Value ? d["Route"].ToString() : null,
                              RouteDescription = d["RouteDescription"] != DBNull.Value ? d["RouteDescription"].ToString() : null,
                          }).ToList();

            return result;
        }
        //end
        public DrugAdministrationTimeEntity GetHoaDetails(int orderId, int quantityId, int nurseStatioId)
        {
            List<HourEntity> hourList = new List<HourEntity>();
            HourEntity hourObj = new HourEntity();
            var result = this.dbContext.PrcgetHOAdata(orderId, quantityId).FirstOrDefault();
            var data = this.autoMapper.Map<PrcgetHOAdata_Result, DrugAdministrationTimeEntity>(result);
            if (data != null)
            {
                if (data.HourId == "")
                {
                    data.HoursList = null;
                }
                else
                {
                    var scheduleHours = data.HourId.Split(',');
                    if (scheduleHours.Length > 0)
                    {
                        int cmpTimeFormate = (int)(this._companyRepository.GetAllFlagsForCompanyByNSId(nurseStatioId).TimeFormat);
                        if (cmpTimeFormate == 0)
                        {
                            foreach (var item in scheduleHours)
                            {
                                int hourId = Convert.ToInt32(item);
                                hourObj = new HourEntity();
                                var time = this.dbContext.Hours.Where(h => h.Hour_Id == hourId).Select(h => new { h.RegularTime, h.RegularTimeFormat }).FirstOrDefault();
                                hourObj.Hour_Id = hourId;
                                hourObj.Hour_Desc = time.RegularTime + " " + time.RegularTimeFormat;
                                hourList.Add(hourObj);
                            }
                            data.HoursList = hourList.OrderBy(item => item.Hour_Id).ToList();
                        }
                        else if (cmpTimeFormate == 1)
                        {
                            foreach (var item in scheduleHours)
                            {
                                int hourId = Convert.ToInt32(item);
                                hourObj = new HourEntity();
                                var time = this.dbContext.Hours.Where(h => h.Hour_Id == hourId).Select(h => new { h.Hour_Desc }).FirstOrDefault();
                                hourObj.Hour_Id = hourId;
                                hourObj.Hour_Desc = time.Hour_Desc;
                                hourList.Add(hourObj);
                            }
                            data.HoursList = hourList.OrderBy(item => item.Hour_Id).ToList();
                        }
                    }
                }
            }
            return data;
        }
        public string GetEmarpreviewdetailslegend(int month, int year, int patientId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetEmarpreviewdetailsinitials", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 360;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@PatientId"].Value = patientId;
                DataSet ds = new DataSet();
                adapt.Fill(ds);
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
        public DataSet GetEmarpreviewdetails(int month, int year, int patientId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ToString()))
            using (SqlCommand cmd = new SqlCommand("Patient.PrcGetEmarpreviewdetails", conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 360;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Month"].Value = month;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@Year"].Value = year;
                adapt.SelectCommand.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int));
                adapt.SelectCommand.Parameters["@PatientId"].Value = patientId;
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

        public OrderStockEntity GetOrderStockDetails(int OrderId)
        {
            var data = this.dbContext.OrderStocks.Where(o => o.Porder_Id == OrderId).FirstOrDefault();
            return this.autoMapper.Map<OrderStock, OrderStockEntity>(data);
        }

        public int UpdateDrFirstOrderAcknowledge(DrFirstOrderXMLTransEntity entity)
        {
            //var record = this.autoMapper.Map<DrFirstOrderXMLTransEntity, DrFirstOrderXMLTran>(entity);
            var data = this.dbContext.DrFirstOrderXMLTrans.Where(e => e.DrFirstOrder_Id == entity.DrFirstOrderId).FirstOrDefault();
            if (data != null)
            {
                data.DrFirstOrderXMLTrans_Approval = entity.DrFirstOrderXMLTransApproval;
                data.DrFirstOrderXMLTrans_ApprovalBy = entity.DrFirstOrderXMLTransApprovalBy;
                data.DrFirstOrderXMLTrans_ApprovalOn = entity.DrFirstOrderXMLTransApprovalOn;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public List<DemographicResidentDropEnity> GetControlSubstanceResDrop(ControlSubstanceFilter filter)
        {
            List<int> facilityIds = (from us in this.dbContext.UserRoleFacilityConfigs
                                     where us.User_Id == filter.User_Id && us.UserRole_Status == 1
                                     select us.Facility_id).Distinct().ToList();


            var q1 = this.dbContext.VisitInfoes.Where(p => facilityIds.Contains((int)p.FacilityId)).ToList();
            var q2 = q1;
            if (filter.NurseStations.Count != 0)
            {
                q2 = q1.Where(p => filter.NurseStations.Contains(Convert.ToInt32(p.NursingStationId))).ToList();
            }
            var q3 = q2;
            if (filter.Floors.Count != 0)
            {
                q3 = q2.Where(p => filter.Floors.Contains(Convert.ToInt32(p.Floor))).ToList();
            }
            var q4 = q3;
            if (filter.Wings.Count != 0)
            {
                q4 = q3.Where(w => filter.Wings.Contains(Convert.ToInt32(w.Wing))).ToList();
            }
            var q5 = q4;
            if (filter.Rooms.Count != 0)
            {
                q5 = q4.Where(p => filter.Rooms.Contains(Convert.ToInt32(p.Room))).ToList();
            }
            var q6 = q5;
            if (filter.Beds.Count != 0)
            {
                q6 = q5.Where(p => filter.Beds.Contains(Convert.ToInt32(p.Bed))).ToList();
            }
            List<DemographicResidentDropEnity> residentDropData = new List<DemographicResidentDropEnity>();
            if (q6.Count > 0)
            {
                var patientIds = q6.Select(p => p.Patient_Id).Distinct();
                string[] controlIds = { "I", "II", "III", "IV", "V" };
                var list = (from de in this.dbContext.Demographics
                            join cm in this.dbContext.CommonOrderInfoes on de.Patient_Id equals cm.Patient_Id
                            join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                            //join vi in this.dbContext.VisitInfoes on de.Patient_Id equals vi.Patient_Id
                            join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                            join cs in this.dbContext.ControlSubstanceCounts on cm.POrder_Id equals cs.Porder_Id
                            //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                            where patientIds.Contains(de.Patient_Id) && controlIds.Contains(en.ControlledSubstanceSchedule)
                            //&& pq.ReviewFlag == 1
                            select new
                            {
                                CommomOrder = cm,
                                Demographics = de,
                                //NurseStation = ns,
                                ControlSub = cs,
                                Encoded = en,
                                QuantityDetails = pq,
                                //Visit = vi,
                            }).Distinct().ToList();
                DemographicResidentDropEnity residentDrop;
                foreach (var item in list)
                {
                    var checkPatientId = residentDropData.Count == 0 ? false : residentDropData.Any(r => r.Patient_Id == item.Demographics.Patient_Id);
                    if (checkPatientId == false)
                    {
                        var query = (from vi in this.dbContext.VisitInfoes
                                     join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                                     group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                                     from g in grouped
                                     where g.date == grouped.Max(recentVisit => recentVisit.date) && g.dm.Patient_Id == item.Demographics.Patient_Id
                                     select g.vis).FirstOrDefault();
                        var nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == query.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();

                        var qty = Convert.ToDecimal(item.ControlSub.Quantity);
                        residentDrop = new DemographicResidentDropEnity();

                        residentDrop.Patient_Id = item.Demographics.Patient_Id;
                        residentDrop.PatientName = item.Demographics.PatientLastName + ", " + item.Demographics.PatientFirstName + " " + (item.Demographics.PatientMiddleInitial != null ? item.Demographics.PatientMiddleInitial : "");
                        if (qty != 0)
                        {
                            if ((item.QuantityDetails.OrderStatus == 2 && qty > 0) || ((query.PVisit_Status == 2 || query.PVisit_Status == 3) && qty > 0) || (item.ControlSub.NurseStation_Id != query.NursingStationId && qty > 0))
                            {
                                residentDropData.Add(residentDrop);
                            }
                            else if ((item.QuantityDetails.OrderStatus == 1 || (item.QuantityDetails.OrderStatus == 1 && qty > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && qty > 0)) && (item.ControlSub.NurseStation_Id == query.NursingStationId))
                            {
                                residentDropData.Add(residentDrop);
                            }
                        }
                        else if ((item.QuantityDetails.OrderStatus == 1 || (item.QuantityDetails.OrderStatus == 1 && qty > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && qty > 0)) && (item.ControlSub.NurseStation_Id == query.NursingStationId))
                        {
                            residentDropData.Add(residentDrop);
                        }
                    }
                }
            }

            // return residentDropData;
            return residentDropData.Distinct().OrderBy(item => item.PatientName).ToList();
        }
        public List<ControlSubstanceGridEntity> GetControlSubstanceGridDataByPid(int patientId, string gpi, int ConsolidateFlag)
        {
            List<ControlSubstanceGridEntity> controlData = new List<ControlSubstanceGridEntity>();
            string[] controlIds = { "I", "II", "III", "IV", "V" };
            if (ConsolidateFlag == 0)
            {
                var list = (from dm in this.dbContext.Demographics
                            join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                            join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                            //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                            join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                            join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                            //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                            where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi
                            //&& pq.ReviewFlag == 1
                            select new
                            {
                                CommomOrder = cm,
                                Demographics = dm,
                                //NurseStation = ns,
                                ControlSub = cs,
                                Encoded = en,
                                QuantityDetails = pq,
                                //Visit = vi,
                            }).Distinct().ToList();
                var orders = list.Where(li => li.ControlSub.ConsolidateFlag == 1).Select(li => li.QuantityDetails.PQuantity_Id).ToList();
                var consolidatedOrders = list.Where(li => orders.Contains(li.QuantityDetails.PQuantity_Id)).ToList().GroupBy(m => new { m.Demographics.Patient_Id, m.Encoded.AGiveCodeIdentifier }).Select(group => group.FirstOrDefault()).ToList().Select(re => re.ControlSub.PQuantity_Id).ToList();
                var data = list.Where(li => (li.ControlSub.ConsolidateFlag != 1) || consolidatedOrders.Contains(li.ControlSub.PQuantity_Id)).ToList();

                ControlSubstanceGridEntity controlSubstance;
                foreach (var item in data)
                {
                    var query = (from vi in this.dbContext.VisitInfoes
                                 join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                                 group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                                 from g in grouped
                                 where g.date == grouped.Max(recentVisit => recentVisit.date) && g.dm.Patient_Id == item.Demographics.Patient_Id
                                 select g.vis).FirstOrDefault();
                    var nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == query.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();
                    decimal administeredQty = 0;
                    controlSubstance = new ControlSubstanceGridEntity();

                    controlSubstance.PorderId = item.CommomOrder.POrder_Id;
                    controlSubstance.ResidentName = item.Demographics.PatientLastName + ", " + item.Demographics.PatientFirstName + " " + (item.Demographics.PatientMiddleInitial != null ? item.Demographics.PatientMiddleInitial : "");
                    controlSubstance.DOB = item.Demographics.DOB.ToString();
                    controlSubstance.Patient_Id = item.Demographics.Patient_Id;
                    controlSubstance.GPI = item.Encoded.AGiveCodeIdentifier;
                    controlSubstance.QuantityId = item.QuantityDetails.PQuantity_Id;
                    controlSubstance.GPI = item.Encoded.AGiveCodeIdentifier;
                    controlSubstance.QtyPerDose = item.QuantityDetails.Quantity;
                    controlSubstance.Directions = item.QuantityDetails.TextInstruction;
                    controlSubstance.AdministeredQty = administeredQty + (item.ControlSub.Quantity == null ? 0 : Convert.ToDecimal(item.ControlSub.Quantity));
                    controlSubstance.Order = item.Encoded.GiveCodeText;
                    controlSubstance.NurseStationName = nsName;
                    controlSubstance.CheckInFlag = item.ControlSub.CheckInFlag;
                    controlSubstance.NsChangeFlag = item.ControlSub.NurseStation_Id != query.NursingStationId ? 1 : 0;
                    controlSubstance.LastCertified = item.ControlSub.NurseStation_Id != query.NursingStationId ? "None" : (item.ControlSub.CertifiedBy != null ? (this.dbContext.Users.Where(us => us.User_Id == item.ControlSub.CertifiedBy).Select(us => us.User_DisplayName).FirstOrDefault()) : "None");
                    controlSubstance.CertifiedDate = item.ControlSub.NurseStation_Id != query.NursingStationId ? "None" : (item.ControlSub.CertifiedDate != null ? item.ControlSub.CertifiedDate.ToString() : "None");
                    controlSubstance.Porder_Status = list.Where(li => li.Demographics.Patient_Id == item.Demographics.Patient_Id && li.Encoded.AGiveCodeIdentifier == item.Encoded.AGiveCodeIdentifier && item.ControlSub.ConsolidateFlag == 1).Count() > 1 ? list.Where(li => li.Demographics.Patient_Id == item.Demographics.Patient_Id && li.Encoded.AGiveCodeIdentifier == item.Encoded.AGiveCodeIdentifier && item.ControlSub.ConsolidateFlag == 1 && li.QuantityDetails.OrderStatus == 1).Count() == 0 ? 2 : 1 : (int)item.QuantityDetails.OrderStatus;
                    controlSubstance.DFalg = item.QuantityDetails.DiscontinueFlag ?? 0;

                    if (item.ControlSub.Quantity != null)
                    {
                        //if empty bring last certifed value
                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                        DataSet dspat = new DataSet();
                        using (SqlConnection con = new SqlConnection(constrEmar))
                        {
                            if (item.ControlSub.Quantity == "")
                            {
                                SqlCommand cmdQuantity = new SqlCommand(@"SELECT TOP 1 Quantity FROM [Audit].[ControlSubstanceCount] WHERE ControlSubstance_Id = @ControlSubstance_Id
            AND Quantity IS NOT NULL AND Quantity <> '' 
          ORDER BY CertifiedDate DESC", con);

                                cmdQuantity.CommandType = CommandType.Text;
                                cmdQuantity.Parameters.AddWithValue("@ControlSubstance_Id", item.ControlSub.ControlSubstance_Id); // Replace 4775 dynamically if needed

                                SqlDataAdapter daQuantity = new SqlDataAdapter(cmdQuantity);
                                daQuantity.Fill(dspat, "LatestQuantity");


                                var listlcc = (from d in dspat.Tables[0].AsEnumerable()
                                               select new
                                               {
                                                   Quantity = d["Quantity"].ToString()
                                               }).FirstOrDefault();

                                if (listlcc != null)
                                {
                                    if (listlcc.Quantity != null && listlcc.Quantity != "")
                                    {
                                        controlSubstance.Quantity = Convert.ToDecimal(listlcc.Quantity);
                                    }
                                    else
                                    {
                                        controlSubstance.Quantity = null;
                                    }

                                    controlSubstance.InitialQuantity = null;
                                }
                                else
                                {
                                    controlSubstance.Quantity = null;
                                    controlSubstance.InitialQuantity = null;
                                }
                            }
                            else
                            {
                                controlSubstance.Quantity = Convert.ToDecimal(item.ControlSub.Quantity);
                                controlSubstance.InitialQuantity = Convert.ToDecimal(item.ControlSub.InitialQuantity);
                            }
                        }
                    }
                    else
                    {
                        controlSubstance.Quantity = null;
                        controlSubstance.InitialQuantity = null;
                    }
                    if (controlSubstance.Quantity != 0)
                    {
                        if ((controlSubstance.Porder_Status == 2 && controlSubstance.Quantity > 0) || ((query.PVisit_Status == 2 || query.PVisit_Status == 3) && controlSubstance.Quantity > 0))
                        {
                            controlSubstance.QuantityZeroStatus = 1;
                            controlData.Add(controlSubstance);
                        }
                        else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                        {
                            controlSubstance.QuantityZeroStatus = 0;
                            controlData.Add(controlSubstance);
                        }
                    }
                    else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                    {
                        controlSubstance.QuantityZeroStatus = 0;
                        controlData.Add(controlSubstance);
                    }
                }
            }
            else if (ConsolidateFlag == 1)
            {
                var list = (from dm in this.dbContext.Demographics
                            join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                            join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                            //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                            join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                            join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                            //join ns in this.dbContext.NursingStations on cs.NurseStation_Id equals ns.NurseStation_Id
                            where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1
                            // && pq.ReviewFlag == 1
                            select new
                            {
                                CommomOrder = cm,
                                Demographics = dm,
                                //NurseStation = ns,
                                ControlSub = cs,
                                Encoded = en,
                                QuantityDetails = pq,
                                //Visit = vi,
                            }).Distinct().ToList();
                ControlSubstanceGridEntity controlSubstance;
                foreach (var item in list)
                {
                    var query = (from vi in this.dbContext.VisitInfoes
                                 join de in dbContext.Demographics on vi.Patient_Id equals de.Patient_Id
                                 group new { dm = de, vis = vi, date = vi.PVisit_CreatedDate } by vi.Patient_Id into grouped
                                 from g in grouped
                                 where g.date == grouped.Max(recentVisit => recentVisit.date) && g.dm.Patient_Id == item.Demographics.Patient_Id
                                 select g.vis).FirstOrDefault();
                    var nsName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == query.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();

                    controlSubstance = new ControlSubstanceGridEntity();
                    controlSubstance.PorderId = item.CommomOrder.POrder_Id;
                    controlSubstance.ResidentName = item.Demographics.PatientLastName + ", " + item.Demographics.PatientFirstName + " " + (item.Demographics.PatientMiddleInitial == null ? "" : item.Demographics.PatientMiddleInitial);
                    controlSubstance.DOB = item.Demographics.DOB.ToString();
                    controlSubstance.Patient_Id = item.Demographics.Patient_Id;
                    controlSubstance.GPI = item.Encoded.AGiveCodeIdentifier;
                    controlSubstance.QuantityId = item.QuantityDetails.PQuantity_Id;
                    controlSubstance.GPI = item.Encoded.AGiveCodeIdentifier;
                    controlSubstance.QtyPerDose = item.QuantityDetails.Quantity;
                    controlSubstance.Directions = item.QuantityDetails.TextInstruction;
                    controlSubstance.Order = item.Encoded.GiveCodeText;
                    controlSubstance.NurseStationName = nsName;
                    controlSubstance.LastCertified = item.ControlSub.CertifiedBy != null ? (this.dbContext.Users.Where(us => us.User_Id == item.ControlSub.CertifiedBy).Select(us => us.User_DisplayName).FirstOrDefault()) : "None";
                    controlSubstance.CertifiedDate = item.ControlSub.CertifiedDate != null ? item.ControlSub.CertifiedDate.ToString() : "None";
                    controlSubstance.Porder_Status = list.Where(li => li.Demographics.Patient_Id == item.Demographics.Patient_Id && li.Encoded.AGiveCodeIdentifier == item.Encoded.AGiveCodeIdentifier && item.ControlSub.ConsolidateFlag == 1).Count() > 1 ? list.Where(li => li.Demographics.Patient_Id == item.Demographics.Patient_Id && li.Encoded.AGiveCodeIdentifier == item.Encoded.AGiveCodeIdentifier && item.ControlSub.ConsolidateFlag == 1 && li.QuantityDetails.OrderStatus == 1).Count() == 0 ? 2 : 1 : (int)item.QuantityDetails.OrderStatus;
                    controlSubstance.DFalg = item.QuantityDetails.DiscontinueFlag ?? 0;

                    if (item.ControlSub.Quantity != null)
                    {
                        //if empty bring last certifed value
                        string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
                        DataSet dspat = new DataSet();
                        using (SqlConnection con = new SqlConnection(constrEmar))
                        {
                            if (item.ControlSub.Quantity == "")
                            {
                                SqlCommand cmdQuantity = new SqlCommand(@"SELECT TOP 1 Quantity FROM [Audit].[ControlSubstanceCount] WHERE ControlSubstance_Id = @ControlSubstance_Id
            AND Quantity IS NOT NULL AND Quantity <> '' 
          ORDER BY CertifiedDate DESC", con);

                                cmdQuantity.CommandType = CommandType.Text;
                                cmdQuantity.Parameters.AddWithValue("@ControlSubstance_Id", item.ControlSub.ControlSubstance_Id); // Replace 4775 dynamically if needed

                                SqlDataAdapter daQuantity = new SqlDataAdapter(cmdQuantity);
                                daQuantity.Fill(dspat, "LatestQuantity");


                                var listlcc = (from d in dspat.Tables[0].AsEnumerable()
                                               select new
                                               {
                                                   Quantity = d["Quantity"].ToString()
                                               }).FirstOrDefault();

                                if (listlcc != null)
                                {
                                    if (listlcc.Quantity != null && listlcc.Quantity != "")
                                    {
                                        controlSubstance.Quantity = Convert.ToDecimal(listlcc.Quantity);
                                    }
                                    else
                                    {
                                        controlSubstance.Quantity = null;
                                    }

                                    controlSubstance.InitialQuantity = null;
                                }
                                else
                                {
                                    controlSubstance.Quantity = null;
                                    controlSubstance.InitialQuantity = null;
                                }
                            }
                            else
                            {
                                controlSubstance.Quantity = Convert.ToDecimal(item.ControlSub.Quantity);
                                controlSubstance.InitialQuantity = Convert.ToDecimal(item.ControlSub.InitialQuantity);
                            }
                        }


                    }

                    else
                    {
                        controlSubstance.Quantity = null;
                        controlSubstance.InitialQuantity = null;
                    }
                    if (controlSubstance.Quantity != 0)
                    {
                        if ((controlSubstance.Porder_Status == 2 && controlSubstance.Quantity > 0) || ((query.PVisit_Status == 2 || query.PVisit_Status == 3) && controlSubstance.Quantity > 0))
                        {
                            controlSubstance.QuantityZeroStatus = 1;
                            controlData.Add(controlSubstance);
                        }
                        else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                        {
                            controlSubstance.QuantityZeroStatus = 0;
                            controlData.Add(controlSubstance);
                        }
                    }
                    else if ((controlSubstance.Porder_Status == 1 || (controlSubstance.Porder_Status == 1 && controlSubstance.Quantity > 0)) && (query.PVisit_Status == 1 || (query.PVisit_Status == 1 && controlSubstance.Quantity > 0)))
                    {
                        controlSubstance.QuantityZeroStatus = 0;
                        controlData.Add(controlSubstance);
                    }
                }
            }
            return controlData.OrderBy(item => item.NurseStationName).ThenBy(item => item.ResidentName).ThenBy(item => item.PorderId).Distinct().ToList();
        }
        #endregion
        #region MergeOrders
        public IList GetMergeOrdersByPatientId(int patientId, int orderId, int quantityId)
        {
            return this.dbContext.PrcMergeOrderData(patientId, orderId, quantityId).ToList();
        }
        public int MergeTwoOrders(int orderQtyId1, int orderQtyId2, int orderQtyId3, string endDateMerge, int userID, string startDateMerge)
        {
            //Nullable<DateTime> endDate = null;
            //if (endDateMerge != "null")
            //{
            //    endDate = Convert.ToDateTime(endDateMerge);
            //}
            //int? i = this.dbContext.PrcMergeOrders(orderQtyId1, orderQtyId2, orderQtyId3, endDate, userID).FirstOrDefault();
            //return (int)i;

           // saikiran  04/28/2026
            DateTime? endDate = null;
            DateTime? startDate = null;

            // Apply timezone-based date if dates or null
            if (endDateMerge == "null" || endDateMerge == null || endDateMerge =="")
            {
                endDate = null;
            }
            else
            {
                endDate = Convert.ToDateTime(endDateMerge);
            }

            if (startDateMerge == "null" || startDateMerge == null || startDateMerge == "")
            {
                startDate = null;
            }
            else
            {
                startDate = Convert.ToDateTime(startDateMerge);
            }

            string query = "[Patient].[PrcMergeOrders]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;

                    cmd.Parameters.Add("@QuantityId1", SqlDbType.Int).Value = orderQtyId1;
                    cmd.Parameters.Add("@QuantityId2", SqlDbType.Int).Value = orderQtyId2;
                    cmd.Parameters.Add("@QuantityId3", SqlDbType.Int).Value = orderQtyId3;
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;

                    // End Date
                    if (endDate.HasValue)
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate.Value;
                    else
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = DBNull.Value;

                    // Start Date
                    if (startDate.HasValue)
                        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate.Value;
                    else
                        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = DBNull.Value;

                    // Best approach instead of DataTable
                    object result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result);
                }
            }
        }

        #endregion
        #region OrdersEndingSoon
        public Tuple<IList, int> GetOrdersEndingSoon(string nursingStationId)
        {
            // int skipRows = (currentPage - 1) * pageSize;
            var count = this.dbContext.PrcGetOrderEndDate(nursingStationId).Count();
            var records = this.dbContext.PrcGetOrderEndDate(nursingStationId).OrderBy(item => item.EndDate).ToList();
            return new Tuple<IList, int>(this.autoMapper.Map<List<PrcGetOrderEndDate_Result>, List<OrdersEndDateEntity>>(records), count);
        }
        public int ConfirmOrdersEndingSoon(List<ConfirmOrdersEndDateEntity> orders)
        {
            //ToDo: Antha Work on this later
            //this.dbContext.QuantityDetails.Where(item => orders.Contains(new ConfirmOrdersEndDateEntity { PQuantity_Id = item.PQuantity_Id, porder_Id = item.POrder_Id }))
            //     .ToList()
            //     .ForEach(a => a.EndDateStatus = 1);
            //return this.dbContext.SaveChanges();

            foreach (var item in orders)
            {
                this.dbContext.QuantityDetails.Where(i => i.PQuantity_Id == item.PQuantity_Id && i.POrder_Id == item.porder_Id)
                    .FirstOrDefault()
                    .EndDateStatus = 1;
            }
            return this.dbContext.SaveChanges();
        }

        public List<string> GetEmarPreviewYearDrop(int patientId)
        {
            var list = (from co in this.dbContext.CommonOrderInfoes
                        join qa in this.dbContext.QuantityDetails on co.POrder_Id equals qa.POrder_Id
                        join dr in this.dbContext.DrugAdministers on co.POrder_Id equals dr.Porder_Id
                        where qa.PQuantity_Status == 1 && co.Patient_Id == patientId && dr.is_deleted == null
                        select dr.AdminsterSchedule.Value.Year.ToString()
                        ).Distinct().ToList();
            return list;
        }
        #endregion

        public string GetScheduledTimeText(Nullable<int> administrationType, Nullable<int> nursingFreq_Id, string nurseshifts_Id, string hour_Id, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string week_Id, string month_Id, string days, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> nurseStationId)
        {
            var facilityId = this.dbContext.NursingStations.Where(s => s.NurseStation_Id == nurseStationId).Select(f => f.Facility_Id).FirstOrDefault();
            int weekId = week_Id != null ? Convert.ToInt32(week_Id.Split(',')[0]) : 0;
            int monthId = month_Id != null ? Convert.ToInt32(month_Id.Split(',')[0]) : 0;
            if (facilityId != null)
            {
                var companyId = this.dbContext.Facilities.Where(fc => fc.Facility_Id == facilityId).Select(c => c.Company_Id).FirstOrDefault();
                if (hours == 0)
                    hours = null;
                return this.dbContext.TempGetOrderHOAMessage(administrationType, nursingFreq_Id, nurseshifts_Id, hour_Id, hours, monday, tuesday, wednesday, thursday, friday, saturday, sunday, week_Id, month_Id, days, activeDays, holdDays, companyId, nurseStationId).FirstOrDefault();
                //week_Id, month_Id,
            }
            return null;

        }
        public int InsertUpdateNurseNotes(NurseCommentsEntity notes)
        {
            notes.Comment_CreatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var record = this.autoMapper.Map<NurseCommentsEntity, NurseComment>(notes);
            //var porderId = this.dbContext.QuantityDetails.Where(qu => qu.PQuantity_Id == notes.PQuantity_Id).FirstOrDefault().POrder_Id;
            var check = this.dbContext.NurseComments.Where(dr => dr.Comments_Id == notes.Comments_Id).FirstOrDefault();
            if (check == null)
            {
                record.DrugAdminister_Id = null;
                this.dbContext.NurseComments.Add(record);
                this.dbContext.SaveChanges();
            }
            else
            {
                check.DrugAdminister_Id = notes.DrugAdminister_Id;
                check.NurseCommentType_Id = notes.NurseCommentType_Id;
                check.Comment = notes.Comment;
                check.comment_Status = notes.comment_Status;
                check.comment_CreatedBy = notes.comment_CreatedBy;
                check.Comment_CreatedOn = Convert.ToDateTime(notes.Comment_CreatedOn);
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
        public List<NurseCommentsEntity> GetNurseNotes(int pQuantityId)
        {
            var orderId = this.dbContext.QuantityDetails.Where(q => q.PQuantity_Id == pQuantityId).Select(q => q.POrder_Id).FirstOrDefault();
            var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == orderId).Select(c => c.Patient_Id).FirstOrDefault();
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //12/09/2022 changed company id to facility id
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var records = (from nc in this.dbContext.NurseComments
                           join u in this.dbContext.Users on nc.comment_CreatedBy equals u.User_Id
                           where nc.PQuantity_Id == pQuantityId
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
        public int OrderEndingSoonStatus(int userId, int screenId)
        {
            var count = this.dbContext.PrcgetOrderEndingSoonVisitStatus(userId, screenId, DateTime.Today).FirstOrDefault();
            if (count == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            //return 3;
        }
        public OrderHoldEntity GetOrderHoldData(int pQuantityId)
        {
            var record = this.dbContext.OrderHolds.Where(oh => oh.PQuantity_Id == pQuantityId && oh.OrderHold_Status == 1).FirstOrDefault();
            return this.autoMapper.Map<OrderHold, OrderHoldEntity>(record);
        }
        public string InsertOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            if (obj.ProfessionalCredentials == "null")
            {
                obj.ProfessionalCredentials = null;
            }
            obj.CertifiedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var result = this.dbContext.PrcInsertOrderCertification(obj.CertifiedOn, obj.User_Id, obj.CertifyOrders, obj.Month, obj.ProfessionalCredentials);
            if (result != 0)
            {
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.CertificationOrders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return "Success";
            }
            return "Cerification Failed";
        }
        public List<CertifiedDatesDropEntity> GetCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate, string phynpi)
        {
            var fromDate = Convert.ToDateTime(fromdate.Substring(0, 10));
            var toDate = Convert.ToDateTime(todate.Substring(0, 10));
            phynpi = phynpi == "null" || phynpi == null ? this.dbContext.Users.Where(us => us.User_Id == userId).Select(us => us.PhysicianNPI).FirstOrDefault() : phynpi;
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //12/09/2022 changed companyid to facility id
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();
            var records = (from cd in this.dbContext.CertifyDates
                           join co in this.dbContext.CertifyOrders on cd.CertifyTime_ID equals co.CertifyTime_ID
                           join pcom in this.dbContext.CommonOrderInfoes on co.Porder_Id equals pcom.POrder_Id
                           join us in this.dbContext.Users on cd.User_Id equals us.User_Id
                           join phy in this.dbContext.PhysicianDetails on pcom.OrderingPhysicianNPI equals phy.PhysicianNPI
                           where pcom.Patient_Id == patientId && phy.PhysicianNPI == phynpi && DbFunctions.TruncateTime(cd.CertifyDate1) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(cd.CertifyDate1) <= DbFunctions.TruncateTime(toDate)
                           // us.User_Id == userId &&
                           select new //CertifiedDatesDropEntity
                           {
                               certifiedDates = cd,
                               physician = phy,
                               //CertifyTime_ID = cd.CertifyTime_ID,
                               //CertifyedPhysician = phy.PhysicianLName + ", " + phy.PhysicianFName,
                               //CertifyedDate = SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1),
                               //CertifyedPhysicianWithDate = phy.PhysicianLName + ", " + phy.PhysicianFName+ " (" + SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1) + ")",
                           }).OrderByDescending(item => item.certifiedDates.CertifyDate1).GroupBy(m => new { m.certifiedDates.CertifyTime_ID }).Select(group => group.FirstOrDefault()).ToList()
                           .Select(x => new CertifiedDatesDropEntity
                           {
                               CertifyTime_ID = x.certifiedDates.CertifyTime_ID,
                               CertifyedPhysician = x.physician.PhysicianLName + ", " + x.physician.PhysicianFName,
                               ConvertedCerifiedDate = GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId),
                               CertifyedDate = string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)), //SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)),
                               CertifyedPhysicianWithDate = x.physician.PhysicianLName + ", " + x.physician.PhysicianFName + " (" + string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + ")", //+ " (" + SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + ")",
                           }).Distinct().ToList();
            return records;
        }
        public IList GetAllcertifiedOrderByDate(int userId, int certifiedTimeId, int patientId)
        {
            var records = this.dbContext.PrcReportGetCertifyedOrders(userId, certifiedTimeId, patientId).ToList();
            return records;
        }
        public List<PhysicianDropEntity> GetPhysicianDropCertifyOrders(string nurseStationIds)
        {
            List<string> nsIds = nurseStationIds.Split(',').ToList();
            var physicianData = (from phy in this.dbContext.PhysicianDetails
                                 where phy.Physician_Status == 1 && nsIds.Contains(phy.NurseStation_Id.ToString())
                                 select new PhysicianDropEntity
                                 {
                                     Physician_Id = phy.Physician_Id,
                                     PhysicianNPI = phy.PhysicianNPI,
                                     PhysicianLName = phy.PhysicianLName,
                                     PhysicianFName = phy.PhysicianFName,
                                     PhysicianFullName = phy.PhysicianLName + ", " + phy.PhysicianFName,
                                 }).OrderBy(item => item.PhysicianFullName).ToList();

            List<PhysicianDropEntity> physicianRecords = physicianData.GroupBy(m => new { m.PhysicianNPI }).Select(group => group.FirstOrDefault()).ToList();
            return physicianRecords;
        }
        public string GetPhysicianCredentialsByNPI(string phyNpi)
        {
            int loginUserId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
            }
            if (phyNpi == null)
            {
                var physicianNpi = this.dbContext.Users.Where(u => u.User_Id == loginUserId).Select(u => u.PhysicianNPI).ToString();
                return this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == physicianNpi).Select(p => p.PhysicianCredentials).FirstOrDefault();
            }
            else
            {
                return this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == phyNpi).Select(p => p.PhysicianCredentials).FirstOrDefault();
            }
        }
        public IList GetAllOrderStockQtyonHand(int nurseStationId)
        {
            string[] controlIds = { "I", "II", "III", "IV", "V" };

            var data = (from i in this.dbContext.Stocks
                            //join s in this.dbContext.NursingStations on i.NurseStation_Id equals s.NurseStation_Id
                        join f in this.dbContext.BarcodeDetails on i.Stock_Id equals f.Stock_Id into barcodes
                        where i.Stock_Status == 1 && i.NurseStation_Id == nurseStationId
                        select new
                        {
                            stock = i,
                            bc = barcodes,
                            //ns = s
                        }).AsEnumerable().
                        Select(sb => new StockEntity
                        {
                            Stock_Id = sb.stock.Stock_Id,
                            //NurseStationName = sb.ns.NurseStation_Name,
                            InHand = sb.stock.InHand,
                            DrugName = sb.stock.DrugName,
                            Barcode = string.Join(", ", sb.bc.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).ToList()),
                            GPICode = sb.stock.GPICode,
                            //  ControlledSubstanceSchedule = controlIds.Contains(this.dbContext.DrugOrderMasters.Where(d => d.GPI == sb.stock.GPICode).Select(d => d.ControlledSubstanceSchedule).FirstOrDefault()) == true ? 1 : 0,
                        }).ToList();

            var facilityId = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == nurseStationId).Select(n => n.Facility_Id).FirstOrDefault();

            var shareddata = (from i in this.dbContext.Stocks
                                  //join s in this.dbContext.NursingStations on i.NurseStation_Id equals s.NurseStation_Id
                              join f in this.dbContext.BarcodeDetails on i.Stock_Id equals f.Stock_Id into barcodes
                              where i.Stock_Status == 1 && i.NurseStation_Id == null && i.Facility_Id == facilityId && i.Sharedstockbit == 1
                              select new
                              {
                                  stock = i,
                                  bc = barcodes,
                                  //ns = s
                              }).AsEnumerable().
                        Select(sb => new StockEntity
                        {
                            Stock_Id = sb.stock.Stock_Id,
                            //NurseStationName = sb.ns.NurseStation_Name,
                            InHand = sb.stock.InHand,
                            DrugName = sb.stock.DrugName,
                            Barcode = string.Join(", ", sb.bc.Where(b => b.PBarcode_Status == 1).Select(b => b.BarcodeDetail1).ToList()),
                            GPICode = sb.stock.GPICode,
                            // ControlledSubstanceSchedule = controlIds.Contains(this.dbContext.DrugOrderMasters.Where(d => d.GPI == sb.stock.GPICode).Select(d => d.ControlledSubstanceSchedule).FirstOrDefault()) == true ? 1 : 0,
                        }).ToList();
            if (shareddata.Count() > 0)
                data.AddRange(shareddata);
            if (data.Count > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    string gpicheck = data[i].GPICode.ToString();
                    var barcodeString = (from st in this.dbContext.Stocks
                                         join br in this.dbContext.BarcodeDetails on st.Stock_Id equals br.Stock_Id
                                         where st.GPICode == gpicheck && ((st.NurseStation_Id != null && st.NurseStation_Id == nurseStationId && st.Facility_Id == null) || (st.Facility_Id != null && st.Facility_Id == facilityId && st.NurseStation_Id == null) || (st.NurseStation_Id == nurseStationId && st.Facility_Id == facilityId))
                                         select new { br.BarcodeDetail1 }).ToList();
                    if (barcodeString.Count > 0)
                        data[i].Barcode = string.Join(", ", barcodeString.Select(b => b.BarcodeDetail1).ToList());
                }
            }
            return data;
        }
        //public List<OrdersGridEntity> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter)
        //{
        //    string selectedFacilities = null;
        //    string selectedNurseStations = null;
        //    string selectedFloors = null;
        //    string selectedWings = null;
        //    string selectedRooms = null;
        //    string selectedBeds = null;

        //    if (orderFilter.Facilities.Count() > 0)
        //        selectedFacilities = string.Join(",", orderFilter.Facilities);
        //    if (orderFilter.NurseStations.Count() > 0)
        //        selectedNurseStations = string.Join(",", orderFilter.NurseStations);
        //    if (orderFilter.Floors.Count() > 0)
        //        selectedFloors = string.Join(",", orderFilter.Floors);
        //    if (orderFilter.Wings.Count() > 0)
        //        selectedWings = string.Join(",", orderFilter.Wings);
        //    if (orderFilter.Rooms.Count() > 0)
        //        selectedRooms = string.Join(",", orderFilter.Rooms);
        //    if (orderFilter.Beds.Count() > 0)
        //        selectedBeds = string.Join(",", orderFilter.Beds);

        //    var ordersList = this.dbContext.PrcGetOrderGrid(orderFilter.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds, orderFilter.VisitStatus).Where(item => item.Patient_Id == orderFilter.patientId).Where(item => item.Order_Status == 1).ToList();
        //    var records =
        //        //this.dbContext.PrcGetOrderGrid(orderFilter.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds, orderFilter.VisitStatus)
        //        ordersList.Select(item => new OrdersGridEntity()
        //        {
        //            Patient_Id = item.Patient_Id,
        //            porder_Id = item.porder_Id,
        //            PQuantity_Id = item.PQuantity_Id,
        //            Date = item.Date,
        //            ResidentName = item.ResidentName,
        //            Drug = item.Drug,
        //            Dosage_Form = item.Dosage_Form,
        //            Quantity = item.Quantity,
        //            Directions = item.Directions,
        //            Physician_Name = item.Physician_Name,
        //            POrder_Status = item.POrder_Status == 3 ? "" : item.POrder_Status == 1 ? "Completed" : "Pending",
        //            ScheduleTimeFlag = item.scheduleTimeflag,
        //            SplitFlag = item.split,
        //            DOB = Convert.ToDateTime(item.DOB).ToString("MM/dd/yyyy"),
        //            EndDate = item.EndDate,
        //            PRN = item.PRN,
        //            HoldUntill = item.On_Hold_Until,
        //            NumberOfRefillsRemaining = item.NumberOfRefillsRemaining
        //        }).OrderBy(item => item.ResidentName).ThenBy(item => item.Drug).Distinct().ToList();
        //    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    if (claimsIdentity.FindFirst("UserId").Value != "")
        //    {
        //        int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
        //        if (orderFilter.NurseStations.Count() > 0)
        //        {
        //            var selectedNurseStationsSave = string.Join(",", orderFilter.NurseStations);
        //            RecentFacEntity userRecentFacObj = new RecentFacEntity();
        //            userRecentFacObj.User_Id = userId;
        //            userRecentFacObj.Facility_Id = orderFilter.Facilities[0];
        //            userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
        //            this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
        //        }
        //    }
        //    return records;
        //    //return this.autoMapper.Map<List<PrcGetOrderGrid_Result>, List<OrdersGridEntity>>(records);
        //}
        public List<OrdersGridEntity> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter)
        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (orderFilter.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", orderFilter.Facilities);
            if (orderFilter.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", orderFilter.NurseStations);
            if (orderFilter.Floors.Count() > 0)
                selectedFloors = string.Join(",", orderFilter.Floors);
            if (orderFilter.Wings.Count() > 0)
                selectedWings = string.Join(",", orderFilter.Wings);
            if (orderFilter.Rooms.Count() > 0)
                selectedRooms = string.Join(",", orderFilter.Rooms);
            if (orderFilter.Beds.Count() > 0)
                selectedBeds = string.Join(",", orderFilter.Beds);

            DataTable dt = new DataTable();
            string query = "[Patient].[PrcGetOrderGrid]";
            string constrEmar = ConfigurationManager.ConnectionStrings["EMARReportsEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constrEmar))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = orderFilter.User_Id == null ? (object)DBNull.Value : orderFilter.User_Id;
                    cmd.Parameters.Add("@FacilityId", SqlDbType.VarChar).Value = selectedFacilities == null ? (object)DBNull.Value : selectedFacilities;
                    cmd.Parameters.Add("@NursingstationId", SqlDbType.VarChar).Value = selectedNurseStations == null ? (object)DBNull.Value : selectedNurseStations;
                    cmd.Parameters.Add("@Floor", SqlDbType.VarChar).Value = selectedFloors == null ? (object)DBNull.Value : selectedFloors;
                    cmd.Parameters.Add("@wing", SqlDbType.VarChar).Value = selectedWings == null ? (object)DBNull.Value : selectedWings;
                    cmd.Parameters.Add("@Room", SqlDbType.VarChar).Value = selectedRooms == null ? (object)DBNull.Value : selectedRooms;
                    cmd.Parameters.Add("@Bed", SqlDbType.VarChar).Value = selectedBeds == null ? (object)DBNull.Value : selectedBeds;
                    cmd.Parameters.Add("@VisitStatus", SqlDbType.Int).Value = orderFilter.VisitStatus == null ? (object)DBNull.Value : orderFilter.VisitStatus;


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            var records = (from d in dt.AsEnumerable()
                           where Convert.ToInt32(d["Patient_Id"]) == orderFilter.patientId && Convert.ToInt32(d["Order_Status"]) == 1

                           select new OrdersGridEntity
                           {
                               Patient_Id = Convert.ToInt32(d["Patient_Id"]),
                               porder_Id = Convert.ToInt32(d["porder_Id"]),
                               PQuantity_Id = Convert.ToInt32(d["PQuantity_Id"]),
                               Date = string.IsNullOrEmpty(d["Date"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["Date"]),//date  //na
                               ResidentName = d["ResidentName"].ToString(),
                               Drug = d["Drug"].ToString(),
                               Dosage_Form = d["Dosage Form"].ToString(),
                               Quantity = d["Quantity"].ToString(),
                               Directions = d["Directions"].ToString(),
                               Physician_Name = d["Physician Name"].ToString(),
                               POrder_Status = Convert.ToInt32(d["POrder_Status"]) == 3 ? "" : Convert.ToInt32(d["POrder_Status"]) == 1 ? "Completed" : "Pending",
                               ScheduleTimeFlag = Convert.ToInt32(d["scheduleTimeflag"]),
                               SplitFlag = Convert.ToInt32(d["split"]),
                               DOB = Convert.ToDateTime(d["DOB"]).ToString("MM/dd/yyyy"),
                               EndDate = string.IsNullOrEmpty(d["EndDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(d["EndDate"]),//date  //na
                               PRN = d["PRN"].ToString(),
                               HoldUntill = d["On Hold Until"].ToString(),
                               HoldFrom = d["Hold From"].ToString(),
                               NumberOfRefillsRemaining = d["NumberOfRefillsRemaining"].ToString(),
                               OrderStatus = string.IsNullOrEmpty(d["Order_Status"].ToString()) ? 0 : Convert.ToInt32(d["Order_Status"]), //nullable int
                                                                                                                                          //OrderStatus = Convert.ToInt32(d["Order_Status"]),
                               ImgPath = d["ImageLocation"] != null ? "Yes" : "No",
                               HoldStatus = Convert.ToInt32(d["HoldStatus"])

                           }).OrderBy(item => item.ResidentName).ThenBy(item => item.Drug).Distinct().ToList();

            return records;

            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (orderFilter.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", orderFilter.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = orderFilter.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return records;
            //return this.autoMapper.Map<List<PrcGetOrderGrid_Result>, List<OrdersGridEntity>>(records);
        }
        public List<OrdersGridEntity> GetPendingOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {
            string selectedFacilities = null;
            string selectedNurseStations = null;
            string selectedFloors = null;
            string selectedWings = null;
            string selectedRooms = null;
            string selectedBeds = null;

            if (orderFilter.Facilities.Count() > 0)
                selectedFacilities = string.Join(",", orderFilter.Facilities);
            if (orderFilter.NurseStations.Count() > 0)
                selectedNurseStations = string.Join(",", orderFilter.NurseStations);
            if (orderFilter.Floors.Count() > 0)
                selectedFloors = string.Join(",", orderFilter.Floors);
            if (orderFilter.Wings.Count() > 0)
                selectedWings = string.Join(",", orderFilter.Wings);
            if (orderFilter.Rooms.Count() > 0)
                selectedRooms = string.Join(",", orderFilter.Rooms);
            if (orderFilter.Beds.Count() > 0)
                selectedBeds = string.Join(",", orderFilter.Beds);

            var ordersList = this.dbContext.PrcGetOrderGrid(orderFilter.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds, orderFilter.VisitStatus).Where(item => item.porder_Id != 0 && item.PQuantity_Id != 0 && item.Order_Status == 1 && item.POrder_Status != 1 && item.POrder_Status != 3).ToList();
            var records =
                //this.dbContext.PrcGetOrderGrid(orderFilter.User_Id, selectedFacilities, selectedNurseStations, selectedFloors, selectedWings, selectedRooms, selectedBeds, orderFilter.VisitStatus)
                ordersList.Select(item => new OrdersGridEntity()
                {
                    Patient_Id = item.Patient_Id,
                    porder_Id = item.porder_Id,
                    PQuantity_Id = item.PQuantity_Id,
                    Date = item.Date,
                    ResidentName = item.ResidentName,
                    Drug = item.Drug,
                    Dosage_Form = item.Dosage_Form,
                    Quantity = item.Quantity,
                    Directions = item.Directions,
                    Physician_Name = item.Physician_Name,
                    POrder_Status = item.POrder_Status == 3 ? "" : item.POrder_Status == 1 ? "Completed" : "Pending",
                    ScheduleTimeFlag = item.scheduleTimeflag,
                    SplitFlag = item.split,
                    //DOB = Convert.ToDateTime(item.DOB).ToString("MM/dd/yyyy"),
                    DOB = item.DOB,
                    EndDate = item.EndDate,
                    PRN = item.PRN,
                    HoldUntill = item.On_Hold_Until,
                }).OrderBy(item => item.ResidentName).ThenBy(item => item.Drug).Distinct().ToList();
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
                if (orderFilter.NurseStations.Count() > 0)
                {
                    var selectedNurseStationsSave = string.Join(",", orderFilter.NurseStations);
                    RecentFacEntity userRecentFacObj = new RecentFacEntity();
                    userRecentFacObj.User_Id = userId;
                    userRecentFacObj.Facility_Id = orderFilter.Facilities[0];
                    userRecentFacObj.NurseStation_Id = selectedNurseStationsSave;
                    this._commonRepository.InsertUpdateUserRecentFacNs(userRecentFacObj);
                }
            }
            return records;
            //return this.autoMapper.Map<List<PrcGetOrderGrid_Result>, List<OrdersGridEntity>>(records);
        }
        public byte[] ConvertImage(string url)
        {
            if (url != null)
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath("~/" + url);
                if (path != null && File.Exists(path))
                    buffer = File.ReadAllBytes(path);
                else
                {
                    path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                    buffer = File.ReadAllBytes(path);
                }
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                if (path != null)
                    buffer = File.ReadAllBytes(path);
                return buffer;
            }
        }

        public byte[] GetImage(int PatientId)
        {
            return CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == PatientId).Select(p => p.PatientMRNumber).FirstOrDefault()) == "" ? ConvertImage(this.dbContext.Demographics.Where(p => p.Patient_Id == PatientId).Select(p => p.ImageLocation).FirstOrDefault()) : ConvertBiometricImage(CheckResImageinBiometric(this.dbContext.Demographics.Where(p => p.Patient_Id == PatientId).Select(p => p.PatientMRNumber).FirstOrDefault()));
            //this.dbContext.Demographics.Where(p => p.Patient_Id == PatientId).Select(p => p.PatientMRNumber).FirstOrDefault();
        }
        public string CheckResImageinBiometric(string mrnumber)
        {

            string imageLocation = ConfigurationManager.AppSettings.GetValues("ResidentImages")[0].ToString() + mrnumber + ".jpg";
            if (File.Exists(imageLocation))
            {
                return imageLocation;
            }
            else
            {
                return string.Empty;
            }
            return imageLocation;
        }

        public string CheckResImageinBiometricGrid(string mrnumber)
        {

            string imageLocation = ConfigurationManager.AppSettings.GetValues("ResidentImages")[0].ToString() + mrnumber + ".jpg";
            if (File.Exists(imageLocation))
            {


                string path = HttpContext.Current.Server.MapPath(imageLocation);

                return path;
            }
            else
            {
                return imageLocation;
            }
            return imageLocation;
        }
        public byte[] ConvertBiometricImage(string url)
        {
            if (url != null)
            {
                byte[] buffer = new byte[16 * 1024];
                //string path = HttpContext.Current.Server.MapPath("~/" + url);
                if (url != null && File.Exists(url))
                    buffer = File.ReadAllBytes(url);
                else
                {
                    string path = HttpContext.Current.Server.MapPath("~/" + url);
                    path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                    buffer = File.ReadAllBytes(path);
                }
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[16 * 1024];
                string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.GetValues("DefaultResidentImage")[0].ToString());
                if (path != null)
                    buffer = File.ReadAllBytes(path);
                return buffer;
            }
        }
        public int InsertScheduleTimeText(int Porder_Id, int Pquantity_Id, string ScheduleText)
        {
            var record = this.dbContext.OrderHOAInfoes.Where(o => o.Porder_Id == Porder_Id && o.PQuantity_Id == Pquantity_Id).FirstOrDefault();
            if (record == null)
            {
                OrderHOAInfo obj = new OrderHOAInfo();
                obj.Porder_Id = Porder_Id;
                obj.PQuantity_Id = Pquantity_Id;
                obj.HoaMessage = ScheduleText;
                this.dbContext.OrderHOAInfoes.Add(obj);
                this.dbContext.SaveChanges();
            }
            else
            {
                record.HoaMessage = ScheduleText;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
        public int InsertConsolidateOrders(ConsolidateCustomEntity entity)
        {
            if (entity.ConsolidateOrders.Count() > 0)
            {
                entity.ConsolidatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                foreach (var item in entity.ConsolidateOrders)
                {
                    string[] controlIds = { "I", "II", "III", "IV", "V" };
                    List<int> records = new List<int>();
                    var isOrderConsolidated = this.dbContext.ControlSubstanceCounts.Where(c => c.Porder_Id == item.POrderId && c.PQuantity_Id == item.PQuantity_Id).Select(c => c.ConsolidateFlag).FirstOrDefault();
                    var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == item.POrderId).Select(c => c.Patient_Id).FirstOrDefault();
                    if (isOrderConsolidated == 1)
                    {
                        List<int> qtyIds = entity.ConsolidateOrders.Select(cs => cs.PQuantity_Id).ToList();
                        var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == item.POrderId).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
                        var list = (from dm in this.dbContext.Demographics
                                    join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
                                    join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
                                    //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
                                    join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
                                    join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
                                    //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
                                    where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1 && !qtyIds.Contains((int)cs.PQuantity_Id)
                                    //&& pq.ReviewFlag == 1
                                    select new
                                    {
                                        Controlsub = cs,
                                    }).Distinct().ToList();
                        records = list.Select(l => (int)l.Controlsub.PQuantity_Id).ToList();
                        records.Add(item.PQuantity_Id);
                    }
                    else
                    {
                        records.Add(item.PQuantity_Id);
                    }
                    if (records.Count() > 0)
                    {
                        foreach (var data in records)
                        {
                            int qty = Convert.ToInt32(data);
                            var VisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
                            var record = this.dbContext.ControlSubstanceCounts.Where(c => c.PQuantity_Id == qty).FirstOrDefault();
                            if (record != null)
                            {
                                record.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
                                record.Quantity = entity.Quantity;
                                record.InitialQuantity = entity.Quantity;
                                record.ConsolidateFlag = 1;
                                record.CertifiedBy = entity.CertifiedUserId;
                                record.ApprovedBy = entity.ApprovedUserId;
                                record.CertifiedDate = entity.ConsolidatedOn;
                                record.ControlSubstance_Status = 1;
                                record.ControlSubstance_CreatedDate = entity.ConsolidatedOn;
                                record.CheckInFlag = 0;
                                this.dbContext.SaveChanges();

                                ControlSubstanceTran csTran = new ControlSubstanceTran();
                                csTran.ControlSubstance_Id = record.ControlSubstance_Id;
                                csTran.ConsolidateFlag = record.ConsolidateFlag;
                                csTran.Quantity = entity.Quantity;
                                csTran.CertifiedBy = entity.CertifiedUserId;
                                csTran.ApprovedBy = entity.ApprovedUserId;
                                csTran.CertifiedDate = entity.ConsolidatedOn;
                                csTran.TransCS_Status = 1;
                                csTran.TransCS_CreatedDate = entity.ConsolidatedOn;
                                this.dbContext.ControlSubstanceTrans.Add(csTran);
                                this.dbContext.SaveChanges();

                                if (entity.Reason != "")
                                {
                                    ControlSubstanceReason reasonRecord = new ControlSubstanceReason();
                                    reasonRecord.ControlSubstance_Id = csTran.TransCS_Id;
                                    reasonRecord.Reason = entity.Reason;
                                    reasonRecord.CSReason_CreatedBy = entity.CertifiedUserId;
                                    reasonRecord.CSReason_CreatedDate = entity.ConsolidatedOn;
                                    this.dbContext.ControlSubstanceReasons.Add(reasonRecord);
                                    this.dbContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return 1;
        }
        public Nullable<DateTime> GetTimeZoneDateTime(DateTime? dateTime, int? companyId)
        {
            var convetedDate = (this.dbContext.GetTimeZoneConvertedDateTime(dateTime, companyId).FirstOrDefault());
            return convetedDate;
        }
        public string GetPhysicianNPIByRoleRes(int userId, int residentId, int nurseStationId)
        {
            var user = this.dbContext.UserRoleFacilityConfigs.Where(u => u.User_Id == userId).FirstOrDefault();
            var userRole = this.dbContext.Roles.Where(r => r.Role_Id == user.Role_ID).FirstOrDefault();
            var physicianNpi = this.dbContext.Users.Where(use => use.User_Id == userId).Select(us => us.PhysicianNPI).FirstOrDefault();



            if (userRole.Role_Desc.ToString() == "PHYSICIAN" || userRole.Role_Desc.ToString() == "PRESCRIBER")
            {
                return physicianNpi;
            }
            else
            {
                var npiID = this.dbContext.NursingStations.Where(ns => ns.NurseStation_Id == nurseStationId).Select(ns => ns.DefaultPhysician_Id).FirstOrDefault();
                var npi = this.dbContext.PhysicianDetails.Where(pd => pd.Physician_Id == npiID).Select(pd => pd.PhysicianNPI).FirstOrDefault();
                //var VisitsLatestNpi = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == residentId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault().PrimaryPhysicianNPI;
                //return VisitsLatestNpi;
                return npi;
            }

            return null;
        }
        public string InsertProfileOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            if (obj.ProfessionalCredentials == "null")
            {
                obj.ProfessionalCredentials = null;
            }
            obj.CertifiedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            var result = this.dbContext.PrcInsertProfileOrderCertification(obj.CertifiedOn, obj.User_Id, obj.CertifyOrders, obj.Month, obj.ProfessionalCredentials);
            if (result != 0)
            {
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.ResidentProfileCertificationOrders,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                    Comments = "",
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
                return "Success";
            }
            return "Cerification Failed";
        }
        public List<ProfileCertifiedDatesDropEntity> GetProfileCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate)
        {
            var fromDate = Convert.ToDateTime(fromdate.Substring(0, 10));
            var toDate = Convert.ToDateTime(todate.Substring(0, 10));
            var query = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //Changed on 12/09/2022 company id to facility id
            var companyId = this.dbContext.Facilities.Where(f => f.Facility_Id == query.FacilityId).Select(f => f.Facility_Id).FirstOrDefault();

            var records = (from cd in this.dbContext.PCertifyDates
                           join co in this.dbContext.PCertifyOrders on cd.PCertifyTime_ID equals co.PCertifyTime_ID
                           join pcom in this.dbContext.CommonOrderInfoes on co.PPorder_Id equals pcom.POrder_Id
                           join us in this.dbContext.Users on cd.PUser_Id equals us.User_Id
                           //join phy in this.dbContext.PhysicianDetails on us.PhysicianNPI equals phy.PhysicianNPI
                           where pcom.Patient_Id == patientId && DbFunctions.TruncateTime(cd.PCertifyDate1) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(cd.PCertifyDate1) <= DbFunctions.TruncateTime(toDate)
                           select new //ProfileCertifiedDatesDropEntity
                           {
                               certifiedDates = cd,
                               user = us,
                               //physician = phy,
                               //CertifyTime_ID = cd.CertifyTime_ID,
                               //CertifyedPhysician = phy.PhysicianLName + ", " + phy.PhysicianFName,
                               //CertifyedDate = SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1),
                               //CertifyedPhysicianWithDate = phy.PhysicianLName + ", " + phy.PhysicianFName+ " (" + SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1) + ")",
                           }).ToList();
            var list = records.OrderByDescending(item => item.certifiedDates.PCertifyDate1).GroupBy(m => new { m.certifiedDates.PCertifyTime_ID }).Select(group => group.FirstOrDefault()).ToList()
            .Select(x => new ProfileCertifiedDatesDropEntity
            {
                CertifyTime_ID = "P" + x.certifiedDates.PCertifyTime_ID,
                CertifyedPhysician = x.user.PhysicianNPI != null && x.user.PhysicianNPI != "" ? (this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == x.user.PhysicianNPI).FirstOrDefault().PhysicianLName) + ", " + (this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == x.user.PhysicianNPI).FirstOrDefault().PhysicianFName) : "",
                ConvertedCerifiedDate = GetTimeZoneDateTime(x.certifiedDates.PCertifyDate1, companyId),
                CertifyedDate = string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.PCertifyDate1, companyId)), //SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)),
                CertifyedPhysicianWithDate = (x.user.PhysicianNPI != null && x.user.PhysicianNPI != "" ? (this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == x.user.PhysicianNPI).FirstOrDefault().PhysicianLName) + ", " + (this.dbContext.PhysicianDetails.Where(p => p.PhysicianNPI == x.user.PhysicianNPI).FirstOrDefault().PhysicianFName) : (x.user.User_DisplayName)) + " (" + string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.PCertifyDate1, companyId)) + ")", //+ " (" + SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + ")",
            }).Distinct().ToList();
            var certifiedOrders = (from cd in this.dbContext.CertifyDates
                                   join co in this.dbContext.CertifyOrders on cd.CertifyTime_ID equals co.CertifyTime_ID
                                   join pcom in this.dbContext.CommonOrderInfoes on co.Porder_Id equals pcom.POrder_Id
                                   join us in this.dbContext.Users on cd.User_Id equals us.User_Id
                                   join phy in this.dbContext.PhysicianDetails on pcom.OrderingPhysicianNPI equals phy.PhysicianNPI
                                   where pcom.Patient_Id == patientId
                                   select new //ProfileCertifiedDatesDropEntity
                                   {
                                       certifiedDates = cd,
                                       physician = phy,
                                       //CertifyTime_ID = cd.CertifyTime_ID,
                                       //CertifyedPhysician = phy.PhysicianLName + ", " + phy.PhysicianFName,
                                       //CertifyedDate = SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1),
                                       //CertifyedPhysicianWithDate = phy.PhysicianLName + ", " + phy.PhysicianFName+ " (" + SqlFunctions.DatePart("mm", cd.CertifyDate1) + "/" + SqlFunctions.DateName("day", cd.CertifyDate1) + "/" + SqlFunctions.DateName("year", cd.CertifyDate1) + ")",
                                   }).OrderByDescending(item => item.certifiedDates.CertifyDate1).GroupBy(m => new { m.certifiedDates.CertifyTime_ID }).Select(group => group.FirstOrDefault()).ToList()
                           .Select(x => new ProfileCertifiedDatesDropEntity
                           {
                               CertifyTime_ID = x.certifiedDates.CertifyTime_ID.ToString(),
                               CertifyedPhysician = x.physician.PhysicianLName + ", " + x.physician.PhysicianFName,
                               ConvertedCerifiedDate = GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId),
                               CertifyedDate = string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)), //SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)),
                               CertifyedPhysicianWithDate = x.physician.PhysicianLName + ", " + x.physician.PhysicianFName + " (" + string.Format("{0:MM/dd/yyyy}", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + ")", //+ " (" + SqlFunctions.DatePart("mm", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("day", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + "/" + SqlFunctions.DateName("year", GetTimeZoneDateTime(x.certifiedDates.CertifyDate1, companyId)) + ")",
                           }).Distinct().ToList();
            list.AddRange(certifiedOrders);
            return list.ToList().OrderBy(item => item.CertifyedDate).ToList();
        }
        public IList GetAllProfilecertifiedOrderByDate(int userId, int certifiedTimeId, int patientId)
        {
            var records = this.dbContext.PrcReportGetPrifileCertifyedOrders(userId, certifiedTimeId, patientId).ToList();
            return records;
        }
        public int RemoveConsolidateOrders(ConsolidateCustomEntity entity)
        {
            //if (entity.ConsolidateOrders.Count() > 0)
            //{
            //    entity.ConsolidatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            //    foreach (var item in entity.ConsolidateOrders)
            //    {
            //        string[] controlIds = { "I", "II", "III", "IV", "V" };
            //        List<int> records = new List<int>();
            //        var isOrderConsolidated = this.dbContext.ControlSubstanceCounts.Where(c => c.Porder_Id == item.POrderId && c.PQuantity_Id == item.PQuantity_Id).Select(c => c.ConsolidateFlag).FirstOrDefault();
            //        var patientId = this.dbContext.CommonOrderInfoes.Where(c => c.POrder_Id == item.POrderId).Select(c => c.Patient_Id).FirstOrDefault();
            //        if (isOrderConsolidated == 1)
            //        {
            //            List<int> qtyIds = entity.ConsolidateOrders.Select(cs => cs.PQuantity_Id).ToList();
            //            var gpi = this.dbContext.EncodedOrderDetails.Where(e => e.POrder_Id == item.POrderId).Select(e => e.AGiveCodeIdentifier).FirstOrDefault();
            //            var list = (from dm in this.dbContext.Demographics
            //                        join cm in this.dbContext.CommonOrderInfoes on dm.Patient_Id equals cm.Patient_Id
            //                        join pq in this.dbContext.QuantityDetails on cm.POrder_Id equals pq.POrder_Id
            //                        //join vi in this.dbContext.VisitInfoes on dm.Patient_Id equals vi.Patient_Id
            //                        join en in this.dbContext.EncodedOrderDetails on cm.POrder_Id equals en.POrder_Id
            //                        join cs in this.dbContext.ControlSubstanceCounts on pq.PQuantity_Id equals cs.PQuantity_Id
            //                        //join ns in this.dbContext.NursingStations on vi.NursingStationId equals ns.NurseStation_Id
            //                        where dm.Patient_Id == patientId && controlIds.Contains(en.ControlledSubstanceSchedule) && en.AGiveCodeIdentifier == gpi && cs.ConsolidateFlag == 1 && !qtyIds.Contains((int)cs.PQuantity_Id)
            //                        //&& pq.ReviewFlag == 1
            //                        select new
            //                        {
            //                            Controlsub = cs,
            //                        }).Distinct().ToList();
            //            records = list.Select(l => (int)l.Controlsub.PQuantity_Id).ToList();
            //            records.Add(item.PQuantity_Id);
            //        }
            //        else
            //        {
            //            records.Add(item.PQuantity_Id);
            //        }
            //        if (records.Count() > 0)
            //        {
            //foreach (var data in records)
            //{
            //int qty = Convert.ToInt32(data);
            //var VisitsLatest = this.dbContext.VisitInfoes.Where(mvs => mvs.Patient_Id == patientId).OrderByDescending(mvs => mvs.PVisit_Id).FirstOrDefault();
            //var record = this.dbContext.ControlSubstanceCounts.Where(c => c.PQuantity_Id == qty).FirstOrDefault();
            //if (record != null)
            //{
            //record.NurseStation_Id = Convert.ToInt32(VisitsLatest.NursingStationId);
            //record.Quantity = entity.Quantity;
            //record.InitialQuantity = entity.Quantity;
            //record.ConsolidateFlag = 1;
            //record.CertifiedBy = entity.CertifiedUserId;
            //record.ApprovedBy = entity.ApprovedUserId;
            //record.CertifiedDate = entity.ConsolidatedOn;
            //record.ControlSubstance_Status = 1;
            //record.ControlSubstance_CreatedDate = entity.ConsolidatedOn;
            //record.CheckInFlag = 0;
            //this.dbContext.SaveChanges();

            //ControlSubstanceTran csTran = new ControlSubstanceTran();
            //csTran.ControlSubstance_Id = record.ControlSubstance_Id;
            //csTran.ConsolidateFlag = record.ConsolidateFlag;
            //csTran.Quantity = entity.Quantity;
            //csTran.CertifiedBy = entity.CertifiedUserId;
            //csTran.ApprovedBy = entity.ApprovedUserId;
            //csTran.CertifiedDate = entity.ConsolidatedOn;
            //csTran.TransCS_Status = 1;
            //csTran.TransCS_CreatedDate = entity.ConsolidatedOn;
            //this.dbContext.ControlSubstanceTrans.Add(csTran);
            //this.dbContext.SaveChanges();

            //if (entity.Reason != "")
            //{
            //    ControlSubstanceReason reasonRecord = new ControlSubstanceReason();
            //    reasonRecord.ControlSubstance_Id = csTran.TransCS_Id;
            //    reasonRecord.Reason = entity.Reason;
            //    reasonRecord.CSReason_CreatedBy = entity.CertifiedUserId;
            //    reasonRecord.CSReason_CreatedDate = entity.ConsolidatedOn;
            //    this.dbContext.ControlSubstanceReasons.Add(reasonRecord);
            //    this.dbContext.SaveChanges();
            //}
            //}
            //}
            //}
            //}
            //}
            return 1;
        }
        public IList GetCpoeSourceDrop(int userId, int? sourceId = null)
        {
            if (sourceId == null)
            {
                var user = this.dbContext.UserRoleFacilityConfigs.Where(u => u.User_Id == userId).FirstOrDefault();
                var userRole = this.dbContext.Roles.Where(r => r.Role_Id == user.Role_ID).FirstOrDefault();
                if (userRole.Role_Desc.ToString() == "PHYSICIAN" || userRole.Role_Desc.ToString() == "PRESCRIBER")
                {
                    var source = this.dbContext.CpoeSources.Where(cp => cp.Source_Status == 1 && cp.Source_Id == 5).ToList();
                    return source;
                }
                else
                {
                    var source = this.dbContext.CpoeSources.Where(cp => cp.Source_Status == 1 && cp.Source_Id != 5).ToList();
                    return source;
                }
            }
            else
            {
                var source = this.dbContext.CpoeSources.Where(cp => cp.Source_Status == 1 && cp.Source_Id == sourceId).ToList();
                return source;
            }
        }
        public IList GetQuantityDoseDrop()
        {
            var qtyDose = this.dbContext.QuantityDoses.Where(cp => cp.DQty_Status == 1).ToList();
            return qtyDose;
        }
        public IList GetUnitMeasurementsDrop()
        {
            var unitMeasurements = this.dbContext.UnitMeasurements.Where(cp => cp.Uom_Status == 1).ToList();
            return unitMeasurements;
        }

        public IList GetDoseUom()
        {
            var DoseUom = (from ro in this.dbContext.DoseUoms
                           where ro.Status == 1
                           select new DoseUomEntity
                           {
                               Dose_Id = ro.Dose_Id,
                               Dose_Desc = ro.Dose_Desc
                           }).ToList();
            return DoseUom;



            return DoseUom;
        }

        public string GetDefultNursingstationPrescriber(string FacilityId, string NurseId)
        {
            string NPI = "";

            var b = this.dbContext.Facilities.Where(U => U.Facility_Name == FacilityId && U.Facility_Status == 1).Select(U => U.Facility_Id).FirstOrDefault();
            int FacilityIdS = b;
            int NursIdS = Convert.ToInt32(NurseId);
            var Phy = this.dbContext.NursingStations.Where(U => U.Facility_Id == FacilityIdS && U.NurseStation_Id == NursIdS && U.NurseStation_Status == 1).Select(U => U.DefaultPhysician_Id).FirstOrDefault();
            if (Phy != null)
            {


                NPI = this.dbContext.PhysicianDetails.Where(U => U.Physician_Id == Phy && U.Physician_Status == 1).Select(U => U.PhysicianNPI).FirstOrDefault();
            }
            else
            {
                NPI = "";
            }
            return Convert.ToString(NPI);
        }

        public int OrderFavouritiesOrderChange(int? OrderId, int? UserID)
        {
            var recordUpdate = this.dbContext.CommonOrderInfoes.Where(co => co.POrder_Id == OrderId).FirstOrDefault();
            if (recordUpdate != null)
            {
                recordUpdate.POrder_CreatedBy = UserID;
                recordUpdate.POrder_CreatedDate = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
    }
}