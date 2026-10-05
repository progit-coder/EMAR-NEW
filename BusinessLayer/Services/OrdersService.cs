using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Repositories;
using LTCPro.Entities;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data;
using System.Security.Claims;
using LTCPro.DAL;
using System.Data.Entity;

namespace LTCPro.ServiceLayer
{
    public class OrdersService : IOrdersService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _log;
        private readonly IAdminApprovalService _adminApprovalService;
        private readonly ICommonRepository _commonRepository;
        public OrdersService(IAutoMapper autoMapper, IOrdersRepository ordersRepository, UserRepository userRepository, IAdminApprovalService adminApprovalService, ILogger log, CommonRepository commonRepository)
        {
            this._autoMapper = autoMapper;
            this._ordersRepository = ordersRepository;
            this._userRepository = userRepository;
            this._adminApprovalService = adminApprovalService;
            this._log = log;
            this._commonRepository = commonRepository;
        }
        public async Task<List<OrdersGridEntity>> GetOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetOrdersGridData() in OrdersService----");
            return await Task.FromResult<List<OrdersGridEntity>>(this._ordersRepository.GetOrdersGridData(orderFilter));
        }

        public async Task<byte[]> GetImage(int PatientId)
        {
            this._log.Debug("---Executing GetOrdersGridData() in OrdersService----");
            return await Task.FromResult<byte[]>(this._ordersRepository.GetImage(PatientId));
        }

        
        /* public async Task<OrderDetailsEntity> GetOrderDetails(Int64 OrderID, int patientID)
         {
             this._log.Debug("---Executing GetOrderDetails() in OrdersService----");
             return await Task.FromResult<OrderDetailsEntity>(this._ordersRepository.GetOrderDetails(OrderID, patientID));
         }
         public async Task<List<OrdersGridCustomEntity>> GetAllOrdersByPatient(Int64 PatientID)
         {
             this._log.Debug("---Executing GetOrderDetails() in OrdersService----");
             return await Task.FromResult<List<OrdersGridCustomEntity>>(this._ordersRepository.GetAllOrdersByPatient(PatientID));
         }
         public async Task<int> ControlTypeUpdate(OrderControlUpdateEntity orderControl)
         {
             this._log.Debug("---Executing ControlTypeUpdate() in OrdersService----");
             return await Task.FromResult<int>(this._ordersRepository.ControlTypeUpdate(orderControl));
         } */
        public async Task<List<PhysicianDropEntity>> GetPhysicianDropData(int? facilityId = null, int? nurseStatioId = null,int? orderId=null)
        {
            this._log.Debug("---Executing GetPhysicianDropData() in OrdersService----");
            return await Task.FromResult<List<PhysicianDropEntity>>(this._ordersRepository.GetPhysicianDropData(facilityId, nurseStatioId, orderId));
        }
        
        public async Task<List<FrequencyMasterEntity>> GetFrequencyMasterData()
        {
            this._log.Debug("---Executing GetFrequencyMasterData() in OrdersService----");
            return await Task.FromResult<List<FrequencyMasterEntity>>(this._ordersRepository.GetFrequencyMasterData());
        }
        public async Task<List<FrequencyMasterEntityWithShifts>> GetFrequencyMasterDataWithShifts(int stationId)
        {
            this._log.Debug("---Executing GetFrequencyMasterDataWithShifts() in OrdersService----");
            return await Task.FromResult<List<FrequencyMasterEntityWithShifts>>(this._ordersRepository.GetFrequencyMasterDataWithShifts(stationId));
        }
        public async Task<List<WeekEntity>> GetWeekMasterData()
        {
            this._log.Debug("---Executing GetWeekMasterData() in OrdersService----");
            return await Task.FromResult<List<WeekEntity>>(this._ordersRepository.GetWeekMasterData());
        }
        public async Task<List<MonthEntity>> GetMonthMasterData()
        {
            this._log.Debug("---Executing GetMonthMasterData() in OrdersService----");
            return await Task.FromResult<List<MonthEntity>>(this._ordersRepository.GetMonthMasterData());
        }
        public async Task<List<HourEntity>> GetHoursDataByNSId(int nurseStationId, int facilityId)
        {
            this._log.Debug("---Executing GetHoursMasterData() in OrdersService----");
            return await Task.FromResult<List<HourEntity>>(this._ordersRepository.GetHoursDataByNSId(nurseStationId,facilityId));
        }
        public async Task<List<HourEntity>> GetHoursMasterData()
        {
            this._log.Debug("---Executing GetHoursMasterData() in OrdersService----");
            return await Task.FromResult<List<HourEntity>>(this._ordersRepository.GetHoursMasterData());
        }
        //public async Task<List<TimeFormatEntity>> GetTimeFormatMasterData()
        //{
        //    this._log.Debug("---Executing GetTimeFormatMasteInsertOrderCommonStatusrData() in OrdersService----");
        //    return await Task.FromResult<List<TimeFormatEntity>>(this._ordersRepository.GetTimeFormatMasterData());
        //}
        //public async Task<int> InsertDrugAdministrationTime(DrugAdministrationTimeEntity DrugAdmTime)
        //{
        //    this._log.Debug("---Executing InsertDrugAdministrationTime() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.InsertDrugAdministrationTime(DrugAdmTime));
        //}
        //public async Task<DrugAdministrationTimeEntity> GetScheduleTimeDetails(int OrderID)
        //{
        //    this._log.Debug("---Executing GetScheduleTimeDetails() in OrdersService----");
        //    return await Task.FromResult<DrugAdministrationTimeEntity>(this._ordersRepository.GetScheduleTimeDetails(OrderID));
        //}
        //public async Task<OrdersTypeCountEntity> GetOrdersTypeCounts(int userId)
        //{
        //    this._log.Debug("---Executing GetOrdersTypeCounts() in OrdersService----");
        //    return await Task.FromResult<OrdersTypeCountEntity>(this._ordersRepository.GetOrdersTypeCounts(userId));
        //}
        public async Task<OrdersinfoCustomEntity> GetDiagnosisDetails(int PatientID)
        {
            this._log.Debug("---Executing GetDiagnosisDetails() in OrdersService----");
            return await Task.FromResult<OrdersinfoCustomEntity>(this._ordersRepository.GetDiagnosisDetails(PatientID));
        }
        //public async Task<int> InsertUpdateResidentOrders(ResidentOrderEntity ResOrders)
        //{
        //    this._log.Debug("---Executing InsertUpdateResidentOrders() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.InsertUpdateResidentOrders(ResOrders));
        //}
        //public async Task<List<ResidentOrderEntity>> GetAllResidentOrders(int PatientID)
        //{
        //    this._log.Debug("---Executing GetAllResidentOrders() in OrdersService----");
        //    return await Task.FromResult<List<ResidentOrderEntity>>(this._ordersRepository.GetAllResidentOrders(PatientID));
        //}
        //public async Task<ResidentOrderEntity> GetResidentOrderDetails(int ResOrderID)
        //{
        //    this._log.Debug("---Executing GetResidentOrderDetails() in OrdersService----");
        //    return await Task.FromResult<ResidentOrderEntity>(this._ordersRepository.GetResidentOrderDetails(ResOrderID));
        //}
        //public async Task<int> RemoveResidentOrderbyID(int ResOrderID)
        //{
        //    this._log.Debug("---Executing RemoveResidentOrderbyID() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.RemoveResidentOrderbyID(ResOrderID));
        //}
        //public async Task<int> InsertOrder(InsertOrdersEntity orderDetails)
        //{
        //    this._log.Debug("---Executing InsertOrder() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.InsertOrder(orderDetails));
        //}
        //public async Task<int> InsertBarcodeDetails(BarcodeDetailEntity barcodes)
        //{
        //    this._log.Debug("---Executing InsertBarcodeDetails() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.InsertBarcodeDetails(barcodes));
        //}
        //public async Task<List<BarcodeDetailEntity>> GetBarcodeData(int OrderID)
        //{
        //    this._log.Debug("---Executing GetBarcodeData() in OrdersService----");
        //    return await Task.FromResult<List<BarcodeDetailEntity>>(this._ordersRepository.GetBarcodeData(OrderID));
        //}
        //public async Task<int> DeleteBarcode(int BarcodeID)
        //{
        //    this._log.Debug("---Executing DeleteBarcode() in OrdersService----");
        //    return await Task.FromResult<int>(this._ordersRepository.DeleteBarcode(BarcodeID));
        //}
        public async Task<List<OrderFavouriteCustomEntity>> GetFavouritesMasterData(int quantityId, int facilityId)
        {
            this._log.Debug("---Executing GetFavouritesMasterData() in OrdersService----");
            return await Task.FromResult<List<OrderFavouriteCustomEntity>>(this._ordersRepository.GetFavouritesMasterData(quantityId, facilityId));
        }
        public async Task<int> InsertOrderFavourities(List<OrderFavouriteEntity> OrderFavouroties)
        {
            this._log.Debug("---Executing InsertOrderFavourities() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.InsertOrderFavourities(OrderFavouroties));
        }
        //public async Task<List<OrderFavouriteEntity>> GetFavouritesByOrderID(int OrderID)
        //{
        //    this._log.Debug("---Executing GetFavouritesByOrderID() in OrdersService----");
        //    return await Task.FromResult<List<OrderFavouriteEntity>>(this._ordersRepository.GetFavouritesByOrderID(OrderID));
        //}
        public async Task<int> InsertOrderHoldDetails(OrderHoldEntity orderHold)
        {
            this._log.Debug("---Executing InsertOrderHoldDetails() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.InsertOrderHoldDetails(orderHold));
        }
        public async Task<IList> GetMergeOrdersByPatientId(int patientId, int orderId, int quantityId)
        {
            this._log.Debug("---Executing GetMergeOrdersByPatientId() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetMergeOrdersByPatientId(patientId,orderId, quantityId));
        }
        public async Task<int> MergeTwoOrders(int orderQtyId1, int orderQtyId2, int orderQtyId3, string endDateMerge,int userID, string startDateMerge)
        {
            this._log.Debug("---Executing MergeTwoOrders() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.MergeTwoOrders(orderQtyId1, orderQtyId2, orderQtyId3, endDateMerge, userID, startDateMerge));
        }
        public async Task<NursingFrequencyConfigEntity> GetNurseFrequencyDropSelect(int frequencyId, int nurseStationId)
        {
            this._log.Debug("---Executing GetNurseFrequencyDropSelect() in OrdersService----");
            return await Task.FromResult<NursingFrequencyConfigEntity>(this._ordersRepository.GetNurseFrequencyDropSelect(frequencyId, nurseStationId));
        }
        public async Task<string> InsertOrderdestroy(OrderDestroyEntity orderDestroy)
        {
            var dUserId = this._userRepository.GetUserId(orderDestroy.DUserName, orderDestroy.DPassword);
            int loginUserId = 0;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity.FindFirst("UserId").Value != "")
            {
                loginUserId = Convert.ToInt32(claimsIdentity.FindFirst("UserId").Value);
            }
            if (dUserId == 0)
            {
                return "Invalid Destroyer Credentials";
            }
            else
            {
                orderDestroy.DestroyerUserId = dUserId;
            }
            //var aUserId = this._userRepository.GetUserId(orderDestroy.DUserName, orderDestroy.DPassword);
            var aUserId = this._userRepository.GetUserId(orderDestroy.AUserName, orderDestroy.APassword);
            if (aUserId == 0)
            {
                return "Invalid Approval Credentials";
            }
            else
            {
                orderDestroy.ApprovalUserId = aUserId;
            }
            var destroy = this._ordersRepository.InsertOrderdestroy(orderDestroy);
            this._log.Debug("---Executing InsertOrderdestroy() in OrdersService----");
            return await Task.FromResult<string>(destroy);
        }
        //public async Task<string> CheckDischargeInterval(int patientId)
        //{
        //    this._log.Debug("---Executing CheckDischargeInterval() in OrdersService----");
        //    int interval = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("DischargeInterval")[0]);
        //    return await Task.FromResult<string>(this._ordersRepository.CheckDischargeInterval(patientId, interval));
        //}
        public async Task<List<ControlSubstanceGridEntity>> GetControlSubstanceGridData(ControlSubstanceFilter filter)
        {
            if (filter.UserName == "" && filter.Password == "")
            {
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersService 1----");
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersService----");
              //  if (filter.ResidentId == 0)
                    return await Task.FromResult<List<ControlSubstanceGridEntity>>(this._ordersRepository.GetControlSubstanceGridData(filter));
              //  else
               //     return await Task.FromResult<List<ControlSubstanceGridEntity>>(this._ordersRepository.GetControlSubstanceGridDataByPid(filter.ResidentId));
            }
            else if (filter.UserName != "" && filter.Password != "")
            {
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersService 2----");

                var dUserId = this._userRepository.GetUserId(filter.UserName, filter.Password);
                if (dUserId == 0)
                {
                    return null;
                }
                UserEntity user = this._userRepository.GetUserDetailsByID(filter.User_Id);

              if( user.UserName.ToUpper() ==filter.UserName.ToUpper() && user.Password==filter.Password)
                {
                    this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersService 3----");

                    this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersService----");
                    //if (filter.ResidentId == 0)
                    //{
                        return await Task.FromResult<List<ControlSubstanceGridEntity>>(this._ordersRepository.GetControlSubstanceGridData(filter));
                    //}
                    //else
                    //{
                    //    return await Task.FromResult<List<ControlSubstanceGridEntity>>(this._ordersRepository.GetControlSubstanceGridDataByPid(filter.ResidentId,filter.History));
                    //}
                }
                else { return null; }
            }
            else
            {
                return null;
            }
        }
        public async Task<string> CheckCertifyAndApprovals(CertifyAndApprovalCheckEntity credentials)
        {
            var dUserId = this._userRepository.GetUserId(credentials.Cert_UserName, credentials.Cert_Password);
            if (dUserId == 0)
            {
                return "Invalid Certifier Credentials";
            }
            //else
            //{
            //    var facilityId = this._userRepository.GetFacilityOfUser(dUserId,credentials.Facility_Id);
            //    if (facilityId==0)
            //    {
            //        return " Certifier Does not have permission for this Facility";
            //    }
                
            //}
            var aUserId = this._userRepository.GetUserId(credentials.Approval_UserName, credentials.Approval_Password);
            if (aUserId == 0)
            {
                return "Invalid Approval Credentials";

            }
            else
            {
                var facilityId = this._userRepository.GetFacilityOfUser(aUserId, credentials.Facility_Id);
                if (facilityId == 0)
                {
                    return " Approval Does not have permission for this Facility";
                }
            }

            var checkUser = this._userRepository.GetUserDetailsByUserId(credentials.User_Id);

            
            if(checkUser.UserName.ToUpper() == credentials.Cert_UserName.ToUpper() && checkUser.Password== credentials.Cert_Password)
            {
                for (int i = 0; i < credentials.ControlOrders.Count; i++)
                {
                    await Task.FromResult<int>(this._ordersRepository.SaveControlSubstance(credentials.ControlOrders[i], dUserId, aUserId, credentials.ApprovedOn));
                }
                return "Success";
            }
            else
            { return "Invalid Credentials"; }
        }
        //public async Task<string> GetOrderScheduleText(int porderId)
        //{
        //    this._log.Debug("---Executing GetOrderScheduleText() in OrdersService----");
        //    return await Task.FromResult<string>(this._ordersRepository.GetOrderScheduleText(porderId));
        //}
        public async Task<List<AcknowledgeOrdersCustomEntity>> GetAckOrders(int userId)
        {
            this._log.Debug("---Executing GetAckOrders() in OrdersService----");
            return await Task.FromResult<List<AcknowledgeOrdersCustomEntity>>(this._ordersRepository.GetAckOrders(userId));
        }
        public async Task<int> AcceptAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            this._log.Debug("---Executing AcceptAckOrders() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.AcceptAckOrders(entity));
        }
        public async Task<int> RejectAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            this._log.Debug("---Executing AcceptAckOrders() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.AcceptAckOrders(entity));
        }
        public async Task<List<DemographicResidentDropEnity>> GetAckOrdersResidentDrop(int userId)
        {
            this._log.Debug("---Executing GetAckOrdersResidentDrop() in OrdersService----");
            return await Task.FromResult<List<DemographicResidentDropEnity>>(this._ordersRepository.GetAckOrdersResidentDrop(userId));
        }
        public async Task<List<AcknowledgeOrdersCustomEntity>> GetAckOrdersByPatientId(int PatientId)
        {
            this._log.Debug("---Executing GetAckOrdersByPatientId() in OrdersService----");
            return await Task.FromResult<List<AcknowledgeOrdersCustomEntity>>(this._ordersRepository.GetAckOrdersByPatientId(PatientId));
        }
        public async Task<List<DrFirstFileDataEntity>> GetDrFirstFilesData()
        {
            this._log.Debug("---Executing GetDrFirstFilesData() in OrdersService----");
            return await Task.FromResult<List<DrFirstFileDataEntity>>(this._ordersRepository.GetDrFirstFilesData());
        }
        public async Task<List<DemographicResidentDropEnity>> GetControlSubstanceResDrop(ControlSubstanceFilter filters)
        {
            this._log.Debug("---Executing GetControlSubstanceResDrop() in OrdersService----");
            return await Task.FromResult<List<DemographicResidentDropEnity>>(this._ordersRepository.GetControlSubstanceResDrop(filters));
        }
        public async Task<List<ControlSubstanceGridEntity>> GetControlSubstanceGridDataByPid(int patientId, string gpi, int ConsolidateFlag)
        {
            this._log.Debug("---Executing GetControlSubstanceGridDataByPid() in OrdersService----");
            return await Task.FromResult<List<ControlSubstanceGridEntity>>(this._ordersRepository.GetControlSubstanceGridDataByPid(patientId, gpi, ConsolidateFlag));
        }
        #region
        //created by:sampath
        public async Task<List<OrderRouteEntity>> GetOrderRoutes()
        {
            this._log.Debug("---Executing GetOrderRoutes() in OrdersService----");
            return await Task.FromResult<List<OrderRouteEntity>>(this._ordersRepository.GetOrderRoutes());
        }

        public async Task<IList> GetOrderGridData(int patientId, string status)
        {
            this._log.Debug("---Executing GetOrderGridData() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetOrderGridData(patientId, status));
        }

        public async Task<OrdersDataEntity> GetOrdersData(int orderId, int quantityId, int userId)
        {
            this._log.Debug("---Executing GetOrdersData() in OrdersService----");
            return await Task.FromResult<OrdersDataEntity>(this._ordersRepository.GetOrdersData(orderId, quantityId, userId));
        }
        public async Task<int> CheckBarcodeAlert(string BarcodeData, string GpiNum, int patientId, int facilityId, int orderId)
        {
            this._log.Debug("---Executing CheckBarcodeAlert() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.CheckBarcodeAlert(BarcodeData, GpiNum, patientId, facilityId, orderId));
        }
        public async Task<int> InsertOrderCommonStatus(OrdersCommonStatusEntity obj)
        {
            var result = 0;
            this._log.Debug("---Executing InsertOrderCommonStatus() in OrdersService----");
            this._log.Debug("---Executing InsertOrderCommonStatus() in OrdersService----DAdminId, OrderType" + obj.DAdminId + " , " + obj.OrderType);
            using (EMAREntities context = new EMAREntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        result = this._ordersRepository.InsertOrderCommonStatus(obj);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            return result;
            //this._log.Debug("---Executing InsertOrderCommonStatus() in OrdersService----");
            //this._log.Debug("---Executing InsertOrderCommonStatus() in OrdersService----DAdminId, OrderType"+obj.DAdminId+", "+obj.OrderType);
            //return await Task.FromResult<int>(this._ordersRepository.InsertOrderCommonStatus(obj));
        }

        public async Task<int> UpdateOrdersDatabyOrderId(OrderUpdateEntity obj)
        {
            this._log.Debug("---Executing UpdateOrdersDatabyOrderId() in OrdersService----");
            this._log.Debug("---Executing UpdateOrdersDatabyOrderId() in OrdersService---- OrderID, UserID " + obj.POrderId + "," + obj.Createdby);
            obj.OrderUpdatedOn = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDateOrderByPatient(obj.PatientId));
            var result = this._ordersRepository.UpdateOrdersDatabyOrderId(obj);



            if ((obj.EndDate!=null && Convert.ToDateTime(obj.EndDate).Date <= Convert.ToDateTime(obj.OrderUpdatedOn).Date))
            {
                OrdersCommonStatusEntity discontinueOrder = new OrdersCommonStatusEntity();
                discontinueOrder.PatientId = obj.PatientId;
                discontinueOrder.DAdminId =(int) obj.DAdminId;
                discontinueOrder.OrderId = (int)obj.POrderId;
                discontinueOrder.QuantityId = (int)obj.PQuantityId;
                discontinueOrder.POrderCreatedBy = obj.Createdby;
                discontinueOrder.POrderStatus = 2;
                discontinueOrder.POOutBoundApproval = 0;
                discontinueOrder.POOutBoundApprovalBy = 0;
                discontinueOrder.OrderType = "Discontinue";
                discontinueOrder.DiscontinueFlag = 2;
                discontinueOrder.DiscontinueReason = "";
                discontinueOrder.DiscontinueAllSplits =0;
                discontinueOrder.DiscontinuedOn = obj.OrderUpdatedOn;
                discontinueOrder.Split =0;
                await this._adminApprovalService.DiscontinueOrder(discontinueOrder);
            }
            return await Task.FromResult<int>(result);
        }
        public async Task<int> UpdateOrderHoldStatus(OrderHoldEntity obj)
        {
            this._log.Debug("---Executing UpdateOrderHoldStatus() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.UpdateOrderHoldStatus(obj));
        }
        public async Task<string> InsertupdateHOA(HOAEntity obj)
        {
            this._log.Debug("---Executing InsertupdateHOA() in OrdersService----");
            return await Task.FromResult<string>(this._ordersRepository.InsertupdateHOA(obj));
        }

        public async Task<IList> GetStockQtyonHand(string drugName, int nurseStationId)
        {
            this._log.Debug("---Executing GetStockQtyonHand() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetStockQtyonHand(drugName, nurseStationId));
        }

        public async Task<DrugAdministrationTimeEntity> GetHoaDetails(int orderId, int quantityId, int nurseStatioId)
        {
            this._log.Debug("---Executing GetHoaDetails() in OrdersService----");
            return await Task.FromResult<DrugAdministrationTimeEntity>(this._ordersRepository.GetHoaDetails(orderId, quantityId,nurseStatioId));
        }
        public async Task<DataSet> GetEmarpreviewdetails(int month, int year, int patientId)
        {
            this._log.Debug("---Executing GetEmarpreviewdetails() in OrdersService----");
            var entity =this._ordersRepository.GetEmarpreviewdetails(month,year,patientId);
            return await Task.FromResult(entity);
        }
        public async Task<string> GetEmarpreviewdetailslegend(int month, int year, int patientId)
        {
            this._log.Debug("---Executing GetEmarpreviewdetails() in OrdersService----");
            var entity = this._ordersRepository.GetEmarpreviewdetailslegend(month, year, patientId);
            return await Task.FromResult(entity);
        }

        public async Task<OrderStockEntity> GetOrderStockDetails(int OrderId)
        {
            this._log.Debug("---Executing GetOrderStockDetails() in OrdersService----");
            return await Task.FromResult<OrderStockEntity>(this._ordersRepository.GetOrderStockDetails(OrderId));
        }

       

        public async Task<int> UpdateDrFirstOrderAcknowledge(DrFirstOrderXMLTransEntity entity)
        {
            this._log.Debug("---Executing UpdateDrFirstOrderAcknowledge() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.UpdateDrFirstOrderAcknowledge(entity));
        }

        public async Task<Tuple<IList, int>> GetOrdersEndingSoon(string nursingStationId)
        {
            this._log.Debug("---Executing GetOrdersEndingSoon() in OrdersService----");
            return await Task.FromResult<Tuple<IList, int>>(this._ordersRepository.GetOrdersEndingSoon(nursingStationId));
        }

        public async Task<int> ConfirmOrdersEndingSoon(List<ConfirmOrdersEndDateEntity> orders)
        {
            this._log.Debug("---Executing ConfirmOrdersEndingSoon() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.ConfirmOrdersEndingSoon(orders));
        }

        #endregion
        //public async Task<IList> SearchDrugName(string drugName)
        //{
        //    this._log.Debug("---Executing SearchDrugName() in OrdersService----");
        //    return await Task.FromResult<IList>(this._ordersRepository.SearchDrugName(drugName));
        //}
        public async Task<List<string>> GetEmarPreviewYearDrop(int patientId)
        {
            this._log.Debug("---Executing GetEmarPreviewYearDrop() in OrdersService----");
            return await Task.FromResult<List<string>>(this._ordersRepository.GetEmarPreviewYearDrop(patientId));
        }
        public async Task<string> GetScheduledTimeText(Nullable<int> administrationType, Nullable<int> nursingFreq_Id, string nurseshifts_Id, string hour_Id, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string week_Id, string month_Id, string days, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> nurseStationId)
        {
            this._log.Debug("---Executing GetScheduledTimeText() in OrdersService----");
            return await Task.FromResult<string>(this._ordersRepository.GetScheduledTimeText(administrationType, nursingFreq_Id, nurseshifts_Id, hour_Id, hours, monday, tuesday, wednesday, thursday, friday, saturday, sunday, week_Id, month_Id, days, activeDays, holdDays, nurseStationId));
        }
        public async Task<List<NurseCommentsEntity>> GetNurseNotes(int pQuantityId)
        {
            this._log.Debug("---Executing GetNurseNotes() in OrdersService----");
            return await Task.FromResult<List<NurseCommentsEntity>>(this._ordersRepository.GetNurseNotes(pQuantityId));
        }
        public async Task<int> InsertUpdateNurseNotes(NurseCommentsEntity notes)    
        {
            this._log.Debug("---Executing InsertUpdateNurseNotes() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.InsertUpdateNurseNotes(notes));
        }

        public async Task<int> OrderEndingSoonStatus(int userId, int screenId)
        {
            this._log.Debug("---Executing OrderEndingSoonStatus() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.OrderEndingSoonStatus(userId, screenId));
        }
        public async Task<List<CustomPassShiftTimeEntity>> GetPasstimeShiftsData(string nurseStationIds, int facilityId)
        {
            this._log.Debug("---Executing GetPasstimeShiftsData() in OrdersService----");
            return await Task.FromResult<List<CustomPassShiftTimeEntity>>(this._ordersRepository.GetPasstimeShiftsData(nurseStationIds, facilityId));
        }
        public async Task<List<LiteralOrderEntity>> GetLiteralOrderGridData(int patientId)
        {
            this._log.Debug("---Executing GetLiteralOrderGridData() in OrdersService----");
            return await Task.FromResult<List<LiteralOrderEntity>>(this._ordersRepository.GetLiteralOrderGridData(patientId));
        }
        public async Task<OrderHoldEntity> GetOrderHoldData(int pQuantityId)
        {
            this._log.Debug("---Executing GetOrderHoldData() in OrdersService----");
            return await Task.FromResult<OrderHoldEntity>(this._ordersRepository.GetOrderHoldData(pQuantityId));
        }
        public async Task<int> DiscontinueCertifyOrders(List<OrdersCommonStatusEntity> list)
        {
            this._log.Debug("---Executing DiscontinueCertifyOrders() in OrdersService----");
            foreach (var item in list)
            {
                item.POrderCreatedDate = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
                item.DiscontinuedOn = item.POrderCreatedDate;
                this._adminApprovalService.DiscontinueOrder(item);
            }
            return 1;
        }
        public async Task<string> InsertOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            this._log.Debug("---Executing Check Certifyer Credentials in OrdersService----");
            var cUserId = this._userRepository.GetUserId(obj.Cert_UserName, obj.Cert_Password);
            if (cUserId == 0 || obj.User_Id!=cUserId)
            {
                return "Invalid Certifier Credentials";
            }
            var certify = this._ordersRepository.InsertOrdersCertification(obj);
            this._log.Debug("---Executing InsertOrdersCertification() in OrdersService----");
            return await Task.FromResult<string>(certify);
        }
        public async Task<List<CertifiedDatesDropEntity>> GetCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate,string phynpi)
        {
            this._log.Debug("---Executing GetCertifiedDatesDrop() in OrdersService----");
            return await Task.FromResult<List<CertifiedDatesDropEntity>>(this._ordersRepository.GetCertifiedDatesDrop(userId, patientId, fromdate, todate, phynpi));
        }
        public async Task<IList> GetAllcertifiedOrderByDate(int userId, int certifiedTimeId, int patientId)
        {
            this._log.Debug("---Executing GetAllcertifiedOrderByDate() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetAllcertifiedOrderByDate(userId, certifiedTimeId,patientId));
        }
        public async Task<List<PhysicianDropEntity>> GetPhysicianDropCertifyOrders(string nurseStationIds)
        {
            this._log.Debug("---Executing GetPhysicianDropCertifyOrders() in OrdersService----");
            return await Task.FromResult<List<PhysicianDropEntity>>(this._ordersRepository.GetPhysicianDropCertifyOrders(nurseStationIds));
        }
        public async Task<string> GetPhysicianCredentialsByNPI(string phyNpi)
        {
            this._log.Debug("---Executing GetPhysicianCredentialsByNPI() in OrdersService----");
            return await Task.FromResult<string>(this._ordersRepository.GetPhysicianCredentialsByNPI(phyNpi));
        }
        public async Task<IList> GetAllOrderStockQtyonHand(int nurseStationId)
        {
            this._log.Debug("---Executing GetAllOrderStockQtyonHand() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetAllOrderStockQtyonHand(nurseStationId));
        }
        public async Task<List<OrdersGridEntity>> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetOrdersGridDataByPatientId() in OrdersService----");
            return await Task.FromResult<List<OrdersGridEntity>>(this._ordersRepository.GetOrdersGridDataByPatientId(orderFilter));
        }
        public async Task<List<OrdersGridEntity>> GetPendingOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetPendingOrdersGridData() in OrdersService----");
            return await Task.FromResult<List<OrdersGridEntity>>(this._ordersRepository.GetPendingOrdersGridData(orderFilter));
        }
        public async Task<string> InsertConsolidateOrders(ConsolidateCustomEntity entity)
        {
            this._log.Debug("---Executing InsertConsolidateOrders() in OrdersService----");
            var dUserId = this._userRepository.GetUserId(entity.Cert_UserName, entity.Cert_Password);
            if (dUserId == 0)
            {
                return "Invalid Certifier Credentials";
            }
            var aUserId = this._userRepository.GetUserId(entity.Approval_UserName, entity.Approval_Password);
            if (aUserId == 0)
            {
                return "Invalid Approval Credentials";

            }
            else
            {
                var facilityId = this._userRepository.GetFacilityOfUser(aUserId, entity.Facility_Id);
                if (facilityId == 0)
                {
                    return " Approval Does not have permission for this Facility";
                }
            }

            var checkUser = this._userRepository.GetUserDetailsByUserId(entity.User_Id);
            if (checkUser.UserName == entity.Cert_UserName && checkUser.Password == entity.Cert_Password)
            {
                entity.CertifiedUserId = dUserId;
                entity.ApprovedUserId = aUserId;
                var result = this._ordersRepository.InsertConsolidateOrders(entity);
                return "Success";
            }
            else
            {
                { return "Invalid Credentials"; }
            }
        }
        public async Task<int> UpdateHoldDcMultipleOrders(MultipleOrdersCustomEntity obj)
        {
            this._log.Debug("---Executing UpdateHoldDcMultipleOrders() in OrdersService----");
            var list = obj.records;
            var dateTime = Convert.ToDateTime(this._commonRepository.GetNursingStationTimeZoneDate());
            foreach (var item in list)
            {
                if(item.HoldChangeFlag==1)
                {
                    //Hold Order
                    if(item.HoldStatus==1)
                    {
                        OrderHoldEntity entity = new OrderHoldEntity();
                        entity.OrderHold_Id = 0;
                        entity.PQuantity_Id = item.PQuantityId;
                        entity.HoldFrom = obj.HoldDC.HoldFrom;
                        entity.HoldTo = obj.HoldDC.HoldTo;
                        entity.HoldReason = obj.HoldDC.HoldReason;
                        entity.OrderHold_Status = 1;
                        entity.OrderHold_CreatedBy = item.UpdatedBy;
                        entity.OrderHold_CreatedDate = dateTime;
                        this._ordersRepository.InsertOrderHoldDetails(entity);


                    }
                    //Relase Hold
                    else if(item.HoldStatus==2)
                    {
                        OrderHoldEntity entity = new OrderHoldEntity();
                        entity.OrderHold_Id = 0;
                        entity.PQuantity_Id = item.PQuantityId;
                        entity.HoldFrom = null;
                        entity.HoldTo = null;
                        entity.HoldReason = "";
                        entity.OrderHold_Status = 2;
                        entity.OrderHold_CreatedBy = item.UpdatedBy;
                        entity.OrderHold_CreatedDate = dateTime;
                        this._ordersRepository.UpdateOrderHoldStatus(entity);
                    }
                }
                if(item.DcChangeFlag==1)
                {
                    //Discontinue Order
                    if(item.DcStatus==1)
                    {
                        OrdersCommonStatusEntity discontinueOrder = new OrdersCommonStatusEntity();
                        discontinueOrder.PatientId = item.PatientId;
                        discontinueOrder.DAdminId = 0;
                        discontinueOrder.OrderId = item.PorderId;
                        discontinueOrder.QuantityId = item.PQuantityId;
                        discontinueOrder.POrderCreatedBy = item.UpdatedBy;
                        discontinueOrder.POrderStatus = 2;
                        discontinueOrder.POOutBoundApproval = 0;
                        discontinueOrder.POOutBoundApprovalBy = 0;
                        discontinueOrder.OrderType = "Discontinue";
                        discontinueOrder.DiscontinueFlag = 1;
                        discontinueOrder.DiscontinueReason = obj.HoldDC.DcReason;
                        discontinueOrder.DiscontinueAllSplits = item.DcAllSplits;
                        discontinueOrder.DiscontinuedOn = dateTime;
                        discontinueOrder.Split = item.DcsPlit;
                        this._adminApprovalService.DiscontinueOrder(discontinueOrder);
                    }
                    //Reactivate Order
                    else if(item.DcStatus==2)
                    {
                        OrdersCommonStatusEntity entity = new OrdersCommonStatusEntity();
                        entity.PatientId = item.PatientId;
                        entity.OrderId = item.PorderId;
                        entity.QuantityId = item.PQuantityId;
                        entity.POrderStatus = 1;
                        entity.POOutBoundApproval = 0;
                        entity.OrderType = "Reactivate";
                        entity.POOutBoundApprovalBy = item.UpdatedBy;
                        entity.UpdatedOn = dateTime;
                        this._ordersRepository.InsertOrderCommonStatus(entity);
                    }
                }
            }
            this._log.Debug("---Executing UpdateHoldDcMultipleOrders() in OrdersService----");
            return 1;
        }
        
        public async Task<string> GetPhysicianNPIByRoleRes(int userId, int residentId, int nurseStationId)
        {
            this._log.Debug("---Executing GetPhysicianNPIByRoleRes() in OrdersService----");
            return await Task.FromResult<string>(this._ordersRepository.GetPhysicianNPIByRoleRes(userId, residentId,nurseStationId));
        }
        public async Task<string> InsertProfileOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            this._log.Debug("---Executing Check Certifyer Credentials in OrdersService----");
            var cUserId = this._userRepository.GetUserId(obj.Cert_UserName, obj.Cert_Password);
            if (cUserId == 0 || obj.User_Id != cUserId)
            {
                return "Invalid Certifier Credentials";
            }
            var certify = this._ordersRepository.InsertProfileOrdersCertification(obj);
            this._log.Debug("---Executing InsertProfileOrdersCertification() in OrdersService----");
            return await Task.FromResult<string>(certify);
        }
        public async Task<List<ProfileCertifiedDatesDropEntity>> GetProfileCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate)
        {
            this._log.Debug("---Executing GetProfileCertifiedDatesDrop() in OrdersService----");
            return await Task.FromResult<List<ProfileCertifiedDatesDropEntity>>(this._ordersRepository.GetProfileCertifiedDatesDrop(userId, patientId, fromdate,todate));
        }
        public async Task<IList> GetAllProfilecertifiedOrderByDate(int userId, int certifiedTimeId, int patientId)
        {
            this._log.Debug("---Executing GetAllProfilecertifiedOrderByDate() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetAllProfilecertifiedOrderByDate(userId, certifiedTimeId, patientId));
        }
        public async Task<string> RemoveConsolidateOrders(ConsolidateCustomEntity entity)
        {
            this._log.Debug("---Executing RemoveConsolidateOrders() in OrdersService----");
            var dUserId = this._userRepository.GetUserId(entity.Cert_UserName, entity.Cert_Password);
            if (dUserId == 0)
            {
                return "Invalid Certifier Credentials";
            }
            var aUserId = this._userRepository.GetUserId(entity.Approval_UserName, entity.Approval_Password);
            if (aUserId == 0)
            {
                return "Invalid Approval Credentials";

            }
            else
            {
                var facilityId = this._userRepository.GetFacilityOfUser(aUserId, entity.Facility_Id);
                if (facilityId == 0)
                {
                    return " Approval Does not have permission for this Facility";
                }
            }

            var checkUser = this._userRepository.GetUserDetailsByUserId(entity.User_Id);
            if (checkUser.UserName == entity.Cert_UserName && checkUser.Password == entity.Cert_Password)
            {
                entity.CertifiedUserId = dUserId;
                entity.ApprovedUserId = aUserId;
                var result = this._ordersRepository.RemoveConsolidateOrders(entity);
                return "Success";
            }
            else
            {
                { return "Invalid Credentials"; }
            }
        }
        public async Task<IList> GetCpoeSourceDrop(int userId, int? sourceId = null)
        {
            this._log.Debug("---Executing GetCpoeSourceDrop() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetCpoeSourceDrop(userId, sourceId));
        }
        public async Task<IList> GetQuantityDoseDrop()
        {
            this._log.Debug("---Executing GetQuantityDoseDrop() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetQuantityDoseDrop());
        }
        public async Task<IList> GetUnitMeasurementsDrop()
        {
            this._log.Debug("---Executing GetUnitMeasurementsDrop() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetUnitMeasurementsDrop());
        }

        public async Task< IList> GetDoseUom()
        {
            this._log.Debug("---Executing GetDoseUom() in OrdersService----");
            return await Task.FromResult<IList>(this._ordersRepository.GetDoseUom());
        }

        public async  Task<string> GetDefultNursingstationPrescriber(string FacilityId, string NurseId)
        {
            this._log.Debug("---Executing GetDefultNursingstationPrescriber() in OrdersService----");
            return await Task.FromResult<string>(this._ordersRepository.GetDefultNursingstationPrescriber(FacilityId , NurseId));
        }
        public async Task<int> OrderFavouritiesOrderChange(int? OrderId, int? UserID)
        {
            this._log.Debug("---Executing OrderFavouritiesOrderChange() in OrdersService----");
            return await Task.FromResult<int>(this._ordersRepository.OrderFavouritiesOrderChange(OrderId , UserID));
        }
        public async Task<List<DrugNameSearch>> SearchDrugName(string drugName)
        {
            this._log.Debug("---Executing SearchDrugName() in OrdersService----");
            return await Task.FromResult<List<DrugNameSearch>>(this._ordersRepository.SearchDrugName(drugName));
        }

    }
}
