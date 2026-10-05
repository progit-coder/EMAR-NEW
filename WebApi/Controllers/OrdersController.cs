using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using System.Web.Http.Results;
using LTCPro.Entities;
using WebApi.Filters;
using System.Collections;
using System.Data;
using System.Web.Http.Filters;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Orders")]
    [CustomAuthorizationFilter]
    public class OrdersController : ApiController
    {
        private readonly IOrdersService _ordersService;
        private readonly IResidentDemographicService _residentDemographicService;
        private readonly ILogger _log;
        public OrdersController(IOrdersService ordersService, ILogger log , IResidentDemographicService residentDemographicService)
        {
            this._residentDemographicService = residentDemographicService;
            this._ordersService = ordersService;
            this._log = log;
        }
        [Route("GetOrdersGridData")]
        [HttpPost]
        public JsonResult<List<OrdersGridEntity>> GetOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {

            try
            {
                this._log.Debug("---Executing GetOrdersGridData() in OrdersController----");
                var filterData = this._ordersService.GetOrdersGridData(orderFilter).Result;
                this._log.Debug("---Executed Successfully GetOrdersGridData() in OrdersController----");

                return Json<List<OrdersGridEntity>>(filterData);
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        [Route("GetImage/{PatientId}")]
        [HttpGet]
        public JsonResult<byte[]> GetImage(int PatientId)
        {

            try
            {
                this._log.Debug("---Executing GetOrdersGridData() in OrdersController----");
                var filterData = this._ordersService.GetImage(PatientId).Result;
                this._log.Debug("---Executed Successfully GetOrdersGridData() in OrdersController----");

                return Json<byte[]>(filterData);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

      //  Task<byte[]> GetImage(int PatientId)
        //[Route("GetOrderDetails/{OrderID}/{patientID}")]
        //[HttpGet]
        //public JsonResult<OrderDetailsEntity> GetOrderDetails(Int64 OrderID, int patientID)
        //{
        //    this._log.Debug("---Executing GetOrderDetails() in OrdersController----");
        //    var filterData = this._ordersService.GetOrderDetails(OrderID, patientID).Result;
        //    this._log.Debug("---Executed Successfully GetOrderDetails() in OrdersController----");
        //    return Json<OrderDetailsEntity>(filterData);
        //}
        //[Route("GetAllOrdersByPatient/{PatientID}")]
        //[HttpGet]
        //public JsonResult<List<OrdersGridCustomEntity>> GetAllOrdersByPatient(Int64 PatientID)
        //{
        //    this._log.Debug("---Executing GetAllOrdersByPatient() in OrdersController----");
        //    var ordersList = this._ordersService.GetAllOrdersByPatient(PatientID).Result;
        //    this._log.Debug("---Executed Successfully GetAllOrdersByPatient() in OrdersController----");
        //    return Json<List<OrdersGridCustomEntity>>(ordersList);
        //}
        [Route("GetPhysicianDropData/{facilityId?}/{nurseStatioId?}/{orderId?}")]
        [HttpGet]
        public JsonResult<List<PhysicianDropEntity>> GetPhysicianDropData(int? facilityId = null, int? nurseStatioId = null, int? orderId=null)
        {
            try
            {
                
                this._log.Debug("---Executing GetPhysicianDropData() in OrdersController----");
                var ordersList = this._ordersService.GetPhysicianDropData(facilityId, nurseStatioId, orderId).Result;
                this._log.Debug("---Executed Successfully GetPhysicianDropData() in OrdersController----");
                return Json<List<PhysicianDropEntity>>(ordersList);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        //[Route("ControlTypeUpdate")]
        //[HttpPost]
        //public int ControlTypeUpdate(OrderControlUpdateEntity orderControl)
        //{
        //    this._log.Debug("---Executing ControlTypeUpdate() in OrdersController----");
        //    var orderCnt = this._ordersService.ControlTypeUpdate(orderControl).Result;
        //    this._log.Debug("---Executed Successfully ControlTypeUpdate() in OrdersController----");
        //    return orderCnt;
        //}
        [Route("GetFrequencyMasterData")]
        [HttpGet]
        public JsonResult<List<FrequencyMasterEntity>> GetFrequencyMasterData()
        {
            this._log.Debug("---Executing GetFrequencyMasterData() in OrdersController----");
            var frequencyData = this._ordersService.GetFrequencyMasterData().Result;
            this._log.Debug("---Executed Successfully GetFrequencyMasterData() in OrdersController----");
            return Json<List<FrequencyMasterEntity>>(frequencyData);
        }
        [Route("GetFrequencyMasterDataWithShifts/{StationId}")]
        [HttpGet]
        public JsonResult<List<FrequencyMasterEntityWithShifts>> GetFrequencyMasterDataWithShifts(int stationId)
        {
            this._log.Debug("---Executing GetFrequencyMasterDataWithShifts() in OrdersController----");
            var frequencyData = this._ordersService.GetFrequencyMasterDataWithShifts(stationId).Result;
            this._log.Debug("---Executed Successfully GetFrequencyMasterDataWithShifts() in OrdersController----");
            return Json<List<FrequencyMasterEntityWithShifts>>(frequencyData);
        }
        [Route("GetWeekMasterData")]
        [HttpGet]
        public JsonResult<List<WeekEntity>> GetWeekMasterData()
        {
            this._log.Debug("---Executing GetWeekMasterData() in OrdersController----");
            var weekData = this._ordersService.GetWeekMasterData().Result;
            this._log.Debug("---Executed Successfully GetWeekMasterData() in OrdersController----");
            return Json<List<WeekEntity>>(weekData);
        }
        [Route("GetMonthMasterData")]
        [HttpGet]
        public JsonResult<List<MonthEntity>> GetMonthMasterData()
        {
            this._log.Debug("---Executing GetMonthMasterData() in OrdersController----");
            var monthData = this._ordersService.GetMonthMasterData().Result;
            this._log.Debug("---Executed Successfully GetMonthMasterData() in OrdersController----");
            return Json<List<MonthEntity>>(monthData);
        }
        [Route("GetHoursDataByNSId/{nurseStationId}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<HourEntity>> GetHoursDataByNSId(int nurseStationId, int facilityId)
        {
            this._log.Debug("---Executing GetHoursMasterData() in OrdersController----");
            var hoursData = this._ordersService.GetHoursDataByNSId(nurseStationId,facilityId).Result;
            this._log.Debug("---Executed Successfully GetHoursMasterData() in OrdersController----");
            return Json<List<HourEntity>>(hoursData);
        }
        [Route("GetHoursMasterData")]
        [HttpGet]
        public JsonResult<List<HourEntity>> GetHoursMasterData()
        {
            this._log.Debug("---Executing GetHoursMasterData() in OrdersController----");
            var hoursData = this._ordersService.GetHoursMasterData().Result;
            this._log.Debug("---Executed Successfully GetHoursMasterData() in OrdersController----");
            return Json<List<HourEntity>>(hoursData);
        }
        //[Route("GetTimeFormatMasterData")]
        //[HttpGet]
        //public JsonResult<List<TimeFormatEntity>> GetTimeFormatMasterData()
        //{
        //    this._log.Debug("---Executing GetTimeFormatMasterData() in OrdersController----");
        //    var timeFormatData = this._ordersService.GetTimeFormatMasterData().Result;
        //    this._log.Debug("---Executed Successfully GetTimeFormatMasterData() in OrdersController----");
        //    return Json<List<TimeFormatEntity>>(timeFormatData);
        //}
        //[Route("InsertDrugAdministrationTime")]
        //[HttpPost]
        //public int InsertDrugAdministrationTime(DrugAdministrationTimeEntity DrugAdmTime)
        //{
        //    this._log.Debug("---Executing InsertDrugAdministrationTime() in OrdersController----");
        //    var drugCnt = this._ordersService.InsertDrugAdministrationTime(DrugAdmTime).Result;
        //    this._log.Debug("---Executed Successfully InsertDrugAdministrationTime() in OrdersController----");
        //    return drugCnt;
        //}
        //[Route("GetScheduleTimeDetails/{OrderID}")]
        //[HttpGet]
        //public JsonResult<DrugAdministrationTimeEntity> GetScheduleTimeDetails(int OrderID)
        //{
        //    this._log.Debug("---Executing GetScheduleTimeDetails() in OrdersController----");
        //    var DrugTimeDetails = this._ordersService.GetScheduleTimeDetails(OrderID).Result;
        //    this._log.Debug("---Executed Successfully GetScheduleTimeDetails() in OrdersController----");
        //    return Json<DrugAdministrationTimeEntity>(DrugTimeDetails);
        //}
        //[Route("GetOrdersTypeCounts/{userId}")]
        //[HttpGet]
        //public JsonResult<OrdersTypeCountEntity> GetOrdersTypeCounts(int userId)
        //{
        //    this._log.Debug("---Executing GetOrdersTypeCounts() in OrdersController----");
        //    var DrugTimeDetails = this._ordersService.GetOrdersTypeCounts(userId).Result;
        //    this._log.Debug("---Executed Successfully GetOrdersTypeCounts() in OrdersController----");
        //    return Json<OrdersTypeCountEntity>(DrugTimeDetails);
        //}
        [Route("GetDiagnosisDetails/{PatientID}")]
        [HttpGet]
        public JsonResult<OrdersinfoCustomEntity> GetDiagnosisDetails(int PatientID)
        {
            this._log.Debug("---Executing GetDiagnosisDetails() in OrdersController----");
            var diagnosisData = this._ordersService.GetDiagnosisDetails(PatientID).Result;
            this._log.Debug("---Executed Successfully GetDiagnosisDetails() in OrdersController----");
            return Json<OrdersinfoCustomEntity>(diagnosisData);
        }
        //[Route("InsertUpdateResidentOrders")]
        //[HttpPost]
        //public int InsertUpdateResidentOrders(ResidentOrderEntity ResOrders)
        //{
        //    this._log.Debug("---Executing InsertUpdateResidentOrders() in OrdersController----");
        //    var result = this._ordersService.InsertUpdateResidentOrders(ResOrders).Result;
        //    this._log.Debug("---Executed Successfully InsertUpdateResidentOrders() in OrdersController----");
        //    return result;
        //}
        //[Route("GetAllResidentOrders/{PatientID}")]
        //[HttpGet]
        //public JsonResult<List<ResidentOrderEntity>> GetAllResidentOrders(int PatientID)
        //{
        //    this._log.Debug("---Executing GetAllResidentOrders() in OrdersController----");
        //    var ordersData = this._ordersService.GetAllResidentOrders(PatientID).Result;
        //    this._log.Debug("---Executed Successfully GetAllResidentOrders() in OrdersController----");
        //    return Json<List<ResidentOrderEntity>>(ordersData);
        //}
        //[Route("GetResidentOrderDetails/{ResOrderID}")]
        //[HttpGet]
        //public JsonResult<ResidentOrderEntity> GetResidentOrderDetails(int ResOrderID)
        //{
        //    this._log.Debug("---Executing GetResidentOrderDetails() in OrdersController----");
        //    var orderDetail = this._ordersService.GetResidentOrderDetails(ResOrderID).Result;
        //    this._log.Debug("---Executed Successfully GetResidentOrderDetails() in OrdersController----");
        //    return Json<ResidentOrderEntity>(orderDetail);
        //}
        //[Route("RemoveResidentOrderbyID/{ResOrderID}")]
        //[HttpGet]
        //public int RemoveResidentOrderbyID(int ResOrderID)
        //{
        //    this._log.Debug("---Executing RemoveResidentOrderbyID() in OrdersController----");
        //    var result = this._ordersService.RemoveResidentOrderbyID(ResOrderID).Result;
        //    this._log.Debug("---Executed Successfully RemoveResidentOrderbyID() in OrdersController----");
        //    return result;
        //}
        //[Route("InsertOrder")]
        //[HttpPost]
        //public int InsertOrder(InsertOrdersEntity orderDetails)
        //{
        //    this._log.Debug("---Executing InsertOrder() in OrdersController----");
        //    var result = this._ordersService.InsertOrder(orderDetails).Result;
        //    this._log.Debug("---Executed Successfully InsertOrder() in OrdersController----");
        //    return result;
        //}
        //[Route("InsertBarcodeDetails")]
        //[HttpPost]
        //public int InsertBarcodeDetails(BarcodeDetailEntity barcodes)
        //{
        //    this._log.Debug("---Executing InsertBarcodeDetails() in OrdersController----");
        //    var result = this._ordersService.InsertBarcodeDetails(barcodes).Result;
        //    this._log.Debug("---Executed Successfully InsertBarcodeDetails() in OrdersController----");
        //    return result;
        //}
        //[Route("GetBarcodeData/{OrderID}")]
        //[HttpGet]
        //public JsonResult<List<BarcodeDetailEntity>> GetBarcodeData(int OrderID)
        //{
        //    this._log.Debug("---Executing GetBarcodeData() in OrdersController----");
        //    var barCodeData = this._ordersService.GetBarcodeData(OrderID).Result;
        //    this._log.Debug("---Executed Successfully GetBarcodeData() in OrdersController----");
        //    return Json<List<BarcodeDetailEntity>>(barCodeData);
        //}

        //[Route("DeleteBarcode/{BarcodeID}")]
        //[HttpGet]
        //public int DeleteBarcode(int BarcodeID)
        //{
        //    this._log.Debug("---Executing DeleteBarcode() in OrdersController----");
        //    var result = this._ordersService.DeleteBarcode(BarcodeID).Result;
        //    this._log.Debug("---Executed Successfully DeleteBarcode() in OrdersController----");
        //    return result;
        //}
        [Route("GetFavouritesMasterData/{QuantityId}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<OrderFavouriteCustomEntity>> GetFavouritesMasterData(int quantityId, int facilityId)
        {
            this._log.Debug("---Executing GetFavouritesMasterData() in OrdersController----");
            var favouritesdata = this._ordersService.GetFavouritesMasterData(quantityId, facilityId).Result;
            this._log.Debug("---Executed Successfully GetFavouritesMasterData() in OrdersController----");
            return Json<List<OrderFavouriteCustomEntity>>(favouritesdata);
        }
        [Route("InsertOrderFavourities")]
        [HttpPost]
        public int InsertOrderFavourities(List<OrderFavouriteEntity> OrderFavouroties)
        {
            this._log.Debug("---Executing InsertOrderFavourities() in OrdersController----");
            var result = this._ordersService.InsertOrderFavourities(OrderFavouroties).Result;
            this._log.Debug("---Executed Successfully InsertOrderFavourities() in OrdersController----");
            return result;
        }
        [Route("OrderFavouritiesOrderChange/{OrderId}/{UserID}")]
        [HttpGet]
        public int OrderFavouritiesOrderChange(int? OrderId, string UserID)
        {
            this._log.Debug("---Executing OrderFavouritiesOrderChange() in OrdersController----");
            var result = this._ordersService.OrderFavouritiesOrderChange(OrderId ,Convert.ToInt32(UserID)).Result;
            this._log.Debug("---Executed Successfully OrderFavouritiesOrderChange() in OrdersController----");
            return result;
        }


        //[Route("GetFavouritesByOrderID/{OrderID}")]
        //[HttpGet]
        //public JsonResult<List<OrderFavouriteEntity>> GetFavouritesByOrderID(int OrderID)
        //{
        //    this._log.Debug("---Executing GetFavouritesByOrderID() in OrdersController----");
        //    var favouriteData = this._ordersService.GetFavouritesByOrderID(OrderID).Result;
        //    this._log.Debug("---Executed Successfully GetFavouritesByOrderID() in OrdersController----");
        //    return Json<List<OrderFavouriteEntity>>(favouriteData);
        //}
        [Route("InsertOrderHoldDetails")]
        [HttpPost]
        public int InsertOrderHoldDetails(OrderHoldEntity orderHold)
        {
            this._log.Debug("---Executing InsertOrderHoldDetails() in OrdersController----");
            var result = this._ordersService.InsertOrderHoldDetails(orderHold).Result;
            this._log.Debug("---Executed Successfully InsertOrderHoldDetails() in OrdersController----");
            return result;
        }
        [Route("GetMergeOrdersByPatientId/{PatientId}/{OrderId}/{QuantityId}")]
        [HttpGet]
        public IList GetMergeOrdersByPatientId(int PatientId, int OrderId, int quantityId)
        {
            this._log.Debug("---Executing GetMergeOrdersByPatientId() in OrdersController----");
            var result = this._ordersService.GetMergeOrdersByPatientId(PatientId, OrderId, quantityId).Result;
            this._log.Debug("---Executed Successfully GetMergeOrdersByPatientId() in OrdersController----");
            return result;
        }
        [Route("MergeTwoOrders/{orderQtyId1}/{orderQtyId2}/{orderQtyId3}/{endDateMerge}/{userID}/{startDateMerge}")]
        [HttpGet]
        public int MergeTwoOrders(int orderQtyId1, int orderQtyId2, int orderQtyId3, string endDateMerge, int userID, string startDateMerge)
        {
            this._log.Debug("---Executing MergeTwoOrders() in OrdersController----");
            var result = this._ordersService.MergeTwoOrders(orderQtyId1, orderQtyId2, orderQtyId3, endDateMerge, userID, startDateMerge).Result;
            this._log.Debug("---Executed Successfully MergeTwoOrders() in OrdersController----");
            return result;
        }

        [Route("GetNurseFrequencyDropSelect/{frequencyId}/{nurseStationId}")]
        [HttpGet]
        public JsonResult<NursingFrequencyConfigEntity> GetNurseFrequencyDropSelect(int frequencyId, int nurseStationId)
        {
            this._log.Debug("---Executing GetNurseFrequencyDropSelect() in OrdersController----");
            var freqData = this._ordersService.GetNurseFrequencyDropSelect(frequencyId, nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetNurseFrequencyDropSelect() in OrdersController----");
            return Json<NursingFrequencyConfigEntity>(freqData);
        }
        [Route("InsertOrderdestroy")]
        [HttpPost]
        public JsonResult<string> InsertOrderdestroy(OrderDestroyEntity orderDestroy)
        {
            this._log.Debug("---Executing InsertOrderdestroy() in OrdersController----");
            var destroy = this._ordersService.InsertOrderdestroy(orderDestroy).Result;
            this._log.Debug("---Executed Successfully InsertOrderdestroy() in OrdersController----");
            return Json<string>(destroy);
        }
        //[Route("CheckDischargeInterval/{patientId}")]
        //[HttpGet]
        //public JsonResult<string> CheckDischargeInterval(int patientId)
        //{
        //    this._log.Debug("---Executing CheckDischargeInterval() in OrdersController----");
        //    var result = this._ordersService.CheckDischargeInterval(patientId).Result;
        //    this._log.Debug("---Executed Successfully CheckDischargeInterval() in OrdersController----");
        //    return Json<string>(result);
        //}
        [Route("GetControlSubstanceGridData")]
        [HttpPost]
        public JsonResult<List<ControlSubstanceGridEntity>> GetControlSubstanceGridData(ControlSubstanceFilter filter)
        {
            try
            {
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersController----");
                // Convert to JSON
                string jsonString = JsonConvert.SerializeObject(filter);

                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersController input data----"+ jsonString);
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersController Input data----");
                var filterData = this._ordersService.GetControlSubstanceGridData(filter).Result;
                this._log.Debug("---Executed Successfully GetControlSubstanceGridData() in OrdersController----");
                return Json<List<ControlSubstanceGridEntity>>(filterData);
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersController Exception Pro----"+ ex.Message.ToString());
                this._log.Debug("---Executing GetControlSubstanceGridData() in OrdersController Inner Exception Pro----" + ex.InnerException.Message.ToString());
                return null;
            }
          
        }
        [Route("CheckCertifyAndApprovals")]
        [HttpPost]
        public JsonResult<string> CheckCertifyAndApprovals(CertifyAndApprovalCheckEntity credentials)
        {
            this._log.Debug("---Executing CheckCertifyAndApprovals() in OrdersController----");
            var filterData = this._ordersService.CheckCertifyAndApprovals(credentials).Result;
            this._log.Debug("---Executed Successfully CheckCertifyAndApprovals() in OrdersController----");
            return Json<string>(filterData);
        }
        //[Route("GetOrderScheduleText/{porderId}")]
        //[HttpGet]
        //public JsonResult<string> GetOrderScheduleText(int porderId)
        //{
        //    this._log.Debug("---Executing GetOrderScheduleText() in OrdersController----");
        //    var result = this._ordersService.GetOrderScheduleText(porderId).Result;
        //    this._log.Debug("---Executed Successfully GetOrderScheduleText() in OrdersController----");
        //    return Json<string>(result);
        //}
        [Route("GetAckOrders/{userId}")]
        [HttpGet]
        public JsonResult<List<AcknowledgeOrdersCustomEntity>> GetAckOrders(int userId)
        {
            this._log.Debug("---Executing GetAckOrders() in OrdersController----");
            var result = this._ordersService.GetAckOrders(userId).Result;
            this._log.Debug("---Executed Successfully GetAckOrders() in OrdersController----");
            return Json<List<AcknowledgeOrdersCustomEntity>>(result);
        }
        [Route("AcceptAckOrders")]
        [HttpPost]
        public JsonResult<int> AcceptAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            this._log.Debug("---Executing AcceptAckOrders() in OrdersController----");
            var result = this._ordersService.AcceptAckOrders(entity).Result;
            this._log.Debug("---Executed Successfully AcceptAckOrders() in OrdersController----");
            return Json<int>(result);
        }
        [Route("RejectAckOrders")]
        [HttpPost]
        public JsonResult<int> RejectAckOrders(List<AcknowledgeOrdersCustomEntity> entity)
        {
            this._log.Debug("---Executing RejectAckOrders() in OrdersController----");
            var result = this._ordersService.RejectAckOrders(entity).Result;
            this._log.Debug("---Executed Successfully RejectAckOrders() in OrdersController----");
            return Json<int>(result);
        }
        [Route("GetAckOrdersResidentDrop/{userId}")]
        [HttpGet]
        public JsonResult<List<DemographicResidentDropEnity>> GetAckOrdersResidentDrop(int userId)
        {
            this._log.Debug("---Executing GetAckOrdersResidentDrop() in OrdersController----");
            var result = this._ordersService.GetAckOrdersResidentDrop(userId).Result;
            this._log.Debug("---Executed Successfully GetAckOrdersResidentDrop() in OrdersController----");
            return Json<List<DemographicResidentDropEnity>>(result);
        }
        [Route("GetAckOrdersByPatientId/{PatientId}")]
        [HttpGet]
        public JsonResult<List<AcknowledgeOrdersCustomEntity>> GetAckOrdersByPatientId(int PatientId)
        {
            this._log.Debug("---Executing GetAckOrdersByPatientId() in OrdersController----");
            var result = this._ordersService.GetAckOrdersByPatientId(PatientId).Result;
            this._log.Debug("---Executed Successfully GetAckOrdersByPatientId() in OrdersController----");
            return Json<List<AcknowledgeOrdersCustomEntity>>(result);
        }
        [Route("GetDrFirstFilesData")]
        [HttpGet]
        public JsonResult<List<DrFirstFileDataEntity>> GetDrFirstFilesData()
        {
            this._log.Debug("---Executing GetDrFirstFilesData() in OrdersController----");
            var result = this._ordersService.GetDrFirstFilesData().Result;
            this._log.Debug("---Executed Successfully GetDrFirstFilesData() in OrdersController----");
            return Json<List<DrFirstFileDataEntity>>(result);
        }
        #region
        //created by:sampath
        [Route("GetOrderRoutes")]
        [HttpGet]
        public JsonResult<List<OrderRouteEntity>> GetOrderRoutes()
        {
            this._log.Debug("---Executing GetOrderRoutes() in OrdersController----");
            var result = this._ordersService.GetOrderRoutes().Result;
            this._log.Debug("---Executed Successfully GetOrderRoutes() in OrdersController----");
            return Json<List<OrderRouteEntity>>(result);
        }
        [Route("GetOrderGridData/{patientId}/{status}")]
        [HttpGet]
        public IHttpActionResult GetOrderGridData(int patientId, string status)
        {
            this._log.Debug("---Executing GetOrderGridData() in OrdersController----");
            var result = this._ordersService.GetOrderGridData(patientId, status).Result;
            this._log.Debug("---Executed Successfully GetOrderGridData() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetOrdersData/{orderId}/{quantityId}/{userId}")]
        [HttpGet]
        public JsonResult<OrdersDataEntity> GetOrdersData(int orderId, int quantityId, int userId)
        {
            try
            {
                
                this._log.Debug("---Executing GetOrdersData() in OrdersController----");
               // var json = new JavaScriptSerializer().Serialize(orderId.ToString() , quantityId);
                //this._log.Debug("---Executing GetOrdersData() in OrdersController Data----" + json.ToString());
                var result = this._ordersService.GetOrdersData(orderId, quantityId,userId).Result;
                this._log.Debug("---Executed Successfully GetOrdersData() in OrdersController----");
                return Json<OrdersDataEntity>(result);
            }
            catch(Exception ex)
            {
                
                return null;
            }
           
        }

        [Route("CheckBarcodeAlert/{BarcodeData}/{GpiNum}/{patientId}/{facilityId}/{orderId}")]
        [HttpGet]
        public int CheckBarcodeAlert(string BarcodeData, string GpiNum, int patientId, int facilityId, int orderId)
        {
            this._log.Debug("---Executing CheckBarcodeAlert() in OrdersController----");
            var result = this._ordersService.CheckBarcodeAlert(BarcodeData, GpiNum, patientId, facilityId, orderId).Result;
            this._log.Debug("---Executed Successfully CheckBarcodeAlert() in OrdersController----");
            return result;
        }

        private void WriteLog(Exception Exception)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string fromMail = WebConfigurationManager.AppSettings["EmarFromMail"];
                string toMail = WebConfigurationManager.AppSettings["EmarTeamMail"];



                if (Exception.InnerException != null)
                {
                    _log.Error("============= Inner Exception  Message  ===========");
                    _log.Error(" Inner Exception Message : = " + Exception.InnerException.Message);
                    _log.Error("============= Inner Exception  Message End  ===========");
                }
                using (MailMessage mail = new MailMessage(fromMail, toMail))
                {
                   

                    


                    string Body = "";
                    if (Exception.InnerException != null)
                    {
                        string mes = "";
                        if (Exception.Message == null)
                            mes = "";
                        else
                            mes = Exception.Message.ToString();

                        mail.Subject = "Emar Application Issues :  " + mes + " , " + Exception.InnerException.Message;
                        Body = " Message : " + Request;
                    }
                    else
                    {
                        mail.Subject = "Emar Application Issues :  " + Exception.Message;
                        Body = " Message : " + Request;
                    }
                    mail.Body = Body;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp1-mke.securence.com"; //Or Your SMTP Server Address


                    smtp.EnableSsl = false;
                    smtp.Credentials = new System.Net.NetworkCredential("","");
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    if (Exception.InnerException != null)
                        _log.Error(" InnerException Message : = " + Exception.InnerException.Message);
                    else
                        _log.Error(" InnerException Message : = " + Exception.Message);
                    _log.Error("===================================");
                }
            }
            catch (Exception ex)
            {

            }



        }
        [Route("InsertOrderCommonStatus")]
        [HttpPost]
        public int InsertOrderCommonStatus(OrdersCommonStatusEntity obj)
        {
            this._log.Debug("---Executing InsertOrderCommonStatus() in OrdersController----");
            var result = this._ordersService.InsertOrderCommonStatus(obj).Result;
            this._log.Debug("---Executed Successfully InsertOrderCommonStatus() in OrdersController----");
            return result;
        }
        [Route("UpdateOrdersDatabyOrderId")]
        [HttpPost]
        public int UpdateOrdersDatabyOrderId(OrderUpdateEntity obj)
        {
            //Srikar
            try
            {
                this._log.Debug("---Executing UpdateOrdersDatabyOrderId() in OrdersController----");
                var json = new JavaScriptSerializer().Serialize(obj);
                this._log.Debug("---Executing UpdateOrdersDatabyOrderId() in OrdersController Data----" + json.ToString());
                var result = this._ordersService.UpdateOrdersDatabyOrderId(obj).Result;
                this._log.Debug("---Executed Successfully UpdateOrdersDatabyOrderId() in OrdersController----");
                return result;
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing Exception  UpdateOrdersDatabyOrderId() in OrdersController Srikarv----");
                this._log.Debug("-------");
                this._log.Debug(ex.Message.ToString());
                this._log.Debug(ex.InnerException.Message.ToString());
                this._log.Debug("Alerttext : " + obj.Alerttext.ToString());
                this._log.Debug("Barcode : " + obj.Barcode.ToString());
                this._log.Debug("controlSubstance : " + obj.controlSubstance.ToString());
                this._log.Debug("Createdby : " + obj.Createdby.ToString());
                this._log.Debug("DAdminId : " +  obj.DAdminId.ToString());
                this._log.Debug("Directions : " + obj.Directions.ToString());
                this._log.Debug("DrugName : " +  obj.DrugName.ToString());
                this._log.Debug("EndDate : " + obj.EndDate.ToString());                
                this._log.Debug("EndDate : " +  obj.Inhand.ToString());
                this._log.Debug("InsulinComments : " + obj.InsulinComments.ToString());
                this._log.Debug("MaxPerdays : " + obj.MaxPerdays.ToString());
                this._log.Debug("NumberofRefills : " +  obj.NumberofRefills.ToString());
                this._log.Debug("OrderstockFlag : " + obj.OrderstockFlag.ToString());
                this._log.Debug("orderTypeID : " + obj.orderTypeID.ToString());
                this._log.Debug("OrderUpdatedOn : " + obj.OrderUpdatedOn.ToString());
                this._log.Debug("PatientId : " + obj.PatientId.ToString());
                this._log.Debug("PhysicianId : " + obj.PhysicianId.ToString());
                this._log.Debug("POrderId : " +  obj.POrderId.ToString());
                this._log.Debug("PQuantityId : " + obj.PQuantityId.ToString());
                this._log.Debug("PRNFlag : " + obj.PRNFlag.ToString());
                this._log.Debug("Quantity : " + obj.Quantity.ToString());
                this._log.Debug("RequestedGiveCode : " + obj.RequestedGiveCode.ToString());
                this._log.Debug("Route : " + obj.Route.ToString());
                this._log.Debug("ScheduleText : " + obj.ScheduleText.ToString());
                this._log.Debug("SelfAdministeredFlag : " + obj.SelfAdministeredFlag.ToString());
                this._log.Debug("StartDate : " + obj.StartDate.ToString());
                this._log.Debug("TreatmentFlag : " + obj.TreatmentFlag.ToString());
                return 0;
            }
           
        }
        [Route("UpdateOrderHoldStatus")]
        [HttpPost]
        public int UpdateOrderHoldStatus(OrderHoldEntity obj)
        {
            this._log.Debug("---Executing UpdateOrderHoldStatus() in OrdersController----");
            var result = this._ordersService.UpdateOrderHoldStatus(obj).Result;
            this._log.Debug("---Executed Successfully UpdateOrderHoldStatus() in OrdersController----");
            return result;
        }
        [Route("InsertupdateHOA")]
        [HttpPost]
        public string InsertupdateHOA(HOAEntity obj)
        {
            try
            {
                //Srikar
                this._log.Debug("---Executing InsertupdateHOA() in OrdersController----" + obj.dadminId.ToString() + "," + obj.porderId.ToString());
                this._log.Debug("---Executing InsertupdateHOA() in OrdersController----");
                var result = this._ordersService.InsertupdateHOA(obj).Result;
                this._log.Debug("---Executed Successfully InsertupdateHOA() in OrdersController----");
                return result;
                //return "";
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing InsertupdateHOA() in OrdersController----"+ex.Message.ToString()+"-- - "+ex.InnerException.Message.ToString());
                return "";

            }
           
        }
        [Route("GetStockQtyonHand")]
        [HttpPost]
        public IHttpActionResult GetStockQtyonHand(StockSearchEntity stockEntity)
        {
            this._log.Debug("---Executing GetStockQtyonHand() in OrdersController----");
            var result = this._ordersService.GetStockQtyonHand(stockEntity.DrugName, stockEntity.NurseStationId).Result;
            this._log.Debug("---Executed Successfully GetStockQtyonHand() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetStockQtyonHandData")]
        [HttpPost]
        public JsonResult<IList> GetStockQtyonHandData(StockSearchEntity modal)
        {
            this._log.Debug("---Executing GetStockQtyonHand() in OrdersController----");
            var json = new JavaScriptSerializer().Serialize(modal);
            this._log.Debug("---Executing GetStockQtyonHand() in OrdersController Data----" + json.ToString());
            var result = this._ordersService.GetStockQtyonHand(modal.DrugName,modal.NurseStationId).Result;
            this._log.Debug("---Executed Successfully GetStockQtyonHand() in OrdersController----");
            return Json<IList>(result);
        }
        //[Route("SearchDrugNameData/{DrugName}")]
        //[HttpGet]
        //public JsonResult<IList> SearchDrugNameData(string DrugName)
        // {
        //    this._log.Debug("---Executing SearchDrugName() in OrdersController----");
        //    var json = new JavaScriptSerializer().Serialize(DrugName);
        //    this._log.Debug("---Executing SearchDrugName() in OrdersController Data----" + json.ToString());
        //    var result = this._ordersService.SearchDrugName(DrugName).Result;
        //    this._log.Debug("---Executed Successfully SearchDrugName() in OrdersController----");
        //    return Json<IList>(result);
        //}
        //DTMS 
        [Route("SearchDrugNameData/{DrugName}")]
        [HttpGet]
        public JsonResult<List<DrugNameSearch>> SearchDrugNameData(string DrugName)
        {
            this._log.Debug("---Executing SearchDrugName() in OrdersController----");
            var result = this._ordersService.SearchDrugName(DrugName).Result;
            this._log.Debug("---Executed Successfully SearchDrugName() in OrdersController----");
            return Json<List<DrugNameSearch>>(result);
        }
        [Route("GetHoaDetails/{orderId}/{quantityId}/{nurseStatioId}")]
        [HttpGet]
        public JsonResult<DrugAdministrationTimeEntity> GetHoaDetails(int orderId, int quantityId, int nurseStatioId)
        {
            this._log.Debug("---Executing GetHoaDetails() in OrdersController----");
            var result = this._ordersService.GetHoaDetails(orderId, quantityId,nurseStatioId).Result;
            this._log.Debug("---Executed Successfully GetHoaDetails() in OrdersController----");
            return Json<DrugAdministrationTimeEntity>(result);
        }
        [Route("GetHoaDetailsDef/{orderId}/{quantityId}/{nurseStatioId}/{userID}")]
        [HttpGet]
        public string GetHoaDetailsDef(int orderId, int quantityId, int nurseStatioId,int userID)
        {
            this._log.Debug("---Executing GetHoaDetails() in OrdersController----");
            var result = this._ordersService.GetHoaDetails(orderId, quantityId, nurseStatioId).Result;
            HOAEntity obj = new HOAEntity();

            obj.createdby = userID;
            obj.sunday = result.Sunday;
            obj.monday = result.Monday;
            obj.tuesday = result.Tuesday;
            obj.wednesday = result.Wednesday;
            obj.thursday = result.Thursday;
            obj.friday = result.Friday;
            obj.saturday = result.Saturday;

            if (result.HourId != null && result.HourId !="")
            {
                obj.hourId = Convert.ToInt16(result.HourId[0]);
            }
            
            obj.hourIds = result.HourId;
            obj.NurseStationId = nurseStatioId;
            obj.dadminId = result.DAdminId;
            obj.freqId = result.NursingFreqId;
            obj.porderId = result.POrderId;
            obj.pquantityId = quantityId;



            var result1 = this._ordersService.InsertupdateHOA(obj).Result;

            return result1;
            
        }
        [Route("GetEmarpreviewdetailslegend/{month}/{year}/{patientId}")]
        [HttpGet]
        public JsonResult<string> GetEmarpreviewdetailslegend(int month, int year, int patientId)
        {
            this._log.Debug("---Executing GetEmarpreviewdetails() in OrdersController----");
            var result = this._ordersService.GetEmarpreviewdetailslegend(month, year, patientId).Result;
            this._log.Debug("---Executed Successfully GetEmarpreviewdetails() in OrdersController----");
            return Json<string>(result);
        }
        [Route("GetEmarpreviewdetails/{month}/{year}/{patientId}")]
        [HttpGet]
        public JsonResult<DataTable> GetEmarpreviewdetails(int month, int year, int patientId)
        {
            this._log.Debug("---Executing GetEmarpreviewdetails() in OrdersController----");
            var result = this._ordersService.GetEmarpreviewdetails(month, year, patientId).Result;
            this._log.Debug("---Executed Successfully GetEmarpreviewdetails() in OrdersController----");
            return Json<DataTable>(result.Tables[0]);
        }
        [Route("GetOrderStockDetails/{OrderId}")]
        [HttpGet]
        public JsonResult<OrderStockEntity> GetOrderStockDetails(int OrderId)
        {
            this._log.Debug("---Executing GetOrderStockDetails() in OrdersController----");
            var result = this._ordersService.GetOrderStockDetails(OrderId).Result;
       
            this._log.Debug("---Executed Successfully GetOrderStockDetails() in OrdersController----");
            return Json<OrderStockEntity>(result);
        }
        
        [Route("UpdateDrFirstOrderAcknowledge")]
        [HttpPost]
        public int UpdateDrFirstOrderAcknowledge(DrFirstOrderXMLTransEntity entity)
        {
            this._log.Debug("---Executing UpdateDrFirstOrderAcknowledge() in OrdersController----");
            var result = this._ordersService.UpdateDrFirstOrderAcknowledge(entity).Result;
            this._log.Debug("---Executed Successfully UpdateDrFirstOrderAcknowledge() in OrdersController----");
            return result;
        }
        [Route("GetControlSubstanceResDrop")]
        [HttpPost]
        public List<DemographicResidentDropEnity> GetControlSubstanceResDrop(ControlSubstanceFilter filters)
        {
            this._log.Debug("---Executing GetControlSubstanceResDrop() in OrdersController----");
            var result = this._ordersService.GetControlSubstanceResDrop(filters).Result;
            this._log.Debug("---Executed Successfully GetControlSubstanceResDrop() in OrdersController----");
            return result;
        }
        [Route("GetControlSubstanceGridDataByPid/{patientId}/{gpi}/{ConsolidateFlag}")]
        [HttpGet]
        public List<ControlSubstanceGridEntity> GetControlSubstanceGridDataByPid(int patientId, string gpi,int ConsolidateFlag)
        {
            this._log.Debug("---Executing GetControlSubstanceGridDataByPid() in OrdersController----");
            var result = this._ordersService.GetControlSubstanceGridDataByPid(patientId, gpi, ConsolidateFlag).Result;
            this._log.Debug("---Executed Successfully GetControlSubstanceGridDataByPid() in OrdersController----");
            return result;
        }
        #endregion

        [Route("GetOrdersEndingSoon/{nursingStationId}")]
        [HttpGet]
        public Tuple<IList, int> GetOrdersEndingSoon(string nursingStationId)
        {
            this._log.Debug("---Executing GetOrdersEndingSoon() in OrdersController----");
            var result = this._ordersService.GetOrdersEndingSoon(nursingStationId).Result;
            this._log.Debug("---Executed Successfully GetOrdersEndingSoon() in OrdersController----");
            return result;
        }
        [Route("ConfirmOrdersEndingSoon")]
        [HttpPost]
        public int ConfirmOrdersEndingSoon(List<ConfirmOrdersEndDateEntity> orders)
        {
            this._log.Debug("---Executing ConfirmOrdersEndingSoon() in OrdersController----");
            var result = this._ordersService.ConfirmOrdersEndingSoon(orders).Result;
            this._log.Debug("---Executed Successfully ConfirmOrdersEndingSoon() in OrdersController----");
            return result;
        }
        [Route("SearchDrugName")]
        [HttpPost]
        public IList SearchDrugName(HttpRequestMessage request)
        {
            this._log.Debug("---Executing SearchDrugName() in OrdersController----");
            string drugName = request.Content.ReadAsStringAsync().Result;
            if(drugName == null)
            {
                return null;
            }
            var result = this._ordersService.SearchDrugName(drugName).Result;
            this._log.Debug("---Executed Successfully SearchDrugName() in OrdersController----");
            return result;
        }
        [Route("GetEmarPreviewYearDrop/{patientId}")]
        [HttpGet]
        public JsonResult<List<string>> GetEmarPreviewYearDrop(int patientId)
        {
            this._log.Debug("---Executing GetEmarPreviewYearDrop() in OrdersController----");
            var result = this._ordersService.GetEmarPreviewYearDrop(patientId).Result;
            this._log.Debug("---Executed Successfully GetEmarPreviewYearDrop() in OrdersController----");
            return Json<List<string>>(result);
        }
        [Route("GetScheduledTimeText")]
        [HttpPost]
        public JsonResult<string> GetScheduledTimeText(HOAEntity obj)
        {
            this._log.Debug("---Executing GetScheduledTimeText() in OrdersController----");
            var result = this._ordersService.GetScheduledTimeText(1, obj.freqId, obj.NurseShiftId, obj.hourIds, obj.hours, obj.monday, obj.tuesday, obj.wednesday, obj.thursday, obj.friday, obj.saturday, obj.sunday, obj.weekId, obj.monthId, obj.days, obj.activedays, obj.holddays, obj.NurseStationId).Result;
            this._log.Debug("---Executed Successfully GetScheduledTimeText() in OrdersController----");
            return Json<string>(result);
        }

        [Route("GetNurseNotes/{pQuantityId}")]
        [HttpGet]
        public JsonResult<List<NurseCommentsEntity>> GetNurseNotes(int pQuantityId)
        {
            this._log.Debug("---Executing GetNurseNotes() in OrdersController----");
            var result = this._ordersService.GetNurseNotes(pQuantityId).Result;
            this._log.Debug("---Executed Successfully GetNurseNotes() in OrdersController----");
            return Json<List<NurseCommentsEntity>>(result);
        }
        [Route("InsertUpdateNurseNotes")]
        [HttpPost]
        public JsonResult<int> InsertUpdateNurseNotes(NurseCommentsEntity notes)
        {
            this._log.Debug("---Executing InsertUpdateNurseNotes() in OrdersController----");

            var json = new JavaScriptSerializer().Serialize(notes);
            this._log.Debug("---Executing InsertUpdateNurseNotes() in OrdersController Data----"+ json.ToString());

            var result = this._ordersService.InsertUpdateNurseNotes(notes).Result;
            this._log.Debug("---Executed Successfully InsertUpdateNurseNotes() in OrdersController----");
            return Json<int>(result);
        }
        [Route("OrderEndingSoonStatus/{userId}/{screenId}")]
        [HttpGet]
        public JsonResult<int> OrderEndingSoonStatus(int userId, int screenId)
        {
            this._log.Debug("---Executing OrderEndingSoonStatus() in OrdersController----");
            var result = this._ordersService.OrderEndingSoonStatus(userId, screenId).Result;
            this._log.Debug("---Executed Successfully OrderEndingSoonStatus() in OrdersController----");
            return Json<int>(result);
        }
        [Route("GetPasstimeShiftsData/{nurseStationIds}/{facilityId}")]
        [HttpGet]
        public JsonResult<List<CustomPassShiftTimeEntity>> GetPasstimeShiftsData(string nurseStationIds, int facilityId)
        {
            this._log.Debug("---Executing GetPasstimeShiftsData() in OrdersController----");
            var result = this._ordersService.GetPasstimeShiftsData(nurseStationIds, facilityId).Result;
            this._log.Debug("---Executed Successfully GetPasstimeShiftsData() in OrdersController----");
            return Json<List<CustomPassShiftTimeEntity>>(result);
        }
        [Route("GetLiteralOrderGridData/{patientId}")]
        [HttpGet]
        public JsonResult<List<LiteralOrderEntity>> GetLiteralOrderGridData(int patientId)
        {
            this._log.Debug("---Executing GetLiteralOrderGridData() in OrdersController----");
            var result = this._ordersService.GetLiteralOrderGridData(patientId).Result;
            this._log.Debug("---Executed Successfully GetLiteralOrderGridData() in OrdersController----");
            return Json<List<LiteralOrderEntity>>(result);
        }
        [Route("GetOrderHoldData/{pQuantityId}")]
        [HttpGet]
        public JsonResult<OrderHoldEntity> GetOrderHoldData(int pQuantityId)
        {
            this._log.Debug("---Executing GetOrderHoldData() in OrdersController----");
            var result = this._ordersService.GetOrderHoldData(pQuantityId).Result;
            this._log.Debug("---Executed Successfully GetOrderHoldData() in OrdersController----");
            return Json<OrderHoldEntity>(result);
        }
        [Route("DiscontinueCertifyOrder")]
        [HttpPost]
        public int DiscontinueCertifyOrders(List<OrdersCommonStatusEntity> list)
        {
            this._log.Debug("---Executing DiscontinueCertifyOrders() in OrdersController----");
            var result = this._ordersService.DiscontinueCertifyOrders(list).Result;
            this._log.Debug("---Executed Successfully DiscontinueCertifyOrders() in OrdersController----");
            return result;
        }
        [Route("InsertOrdersCertification")]
        [HttpPost]
        public string InsertOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            this._log.Debug("---Executing InsertOrdersCertification() in OrdersController----");
            var result = this._ordersService.InsertOrdersCertification(obj).Result;
            this._log.Debug("---Executed Successfully InsertOrdersCertification() in OrdersController----");
            return result;
        }
        [Route("GetDefultNursingstationPrescriber/{residentId}")]
        [HttpGet]
        public string GetDefultNursingstationPrescriber(string residentId)

        {
            this._log.Debug("---Executing GetResidentMedications() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetFacilityNSResidentsDataByPId(Convert.ToInt32(residentId)).Result;
            this._log.Debug("---Executed Successfully GetResidentMedications() in ResidentDemographicController----");


            this._log.Debug("---Executing GetDefultNursingstationPrescriber() in OrdersController----");
            var result1 = this._ordersService.GetDefultNursingstationPrescriber(result.FacilityName ,Convert.ToString( result.NursingStationId)).Result;
            this._log.Debug("---Executed Successfully GetDefultNursingstationPrescriber() in OrdersController----");
            return result1;
        }
        [Route("GetCertifiedDatesDrop/{userId}/{patientId}/{fromdate}/{todate}/{phynpi}")]
        [HttpGet]
        public JsonResult<List<CertifiedDatesDropEntity>> GetCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate,string phynpi)
        {
            this._log.Debug("---Executing GetCertifiedDatesDrop() in OrdersController----");
            var result = this._ordersService.GetCertifiedDatesDrop(userId, patientId, fromdate, todate, phynpi).Result;
            this._log.Debug("---Executed Successfully GetCertifiedDatesDrop() in OrdersController----");
            return Json<List<CertifiedDatesDropEntity>>(result);
        }
        [Route("GetAllcertifiedOrderByDate/{userId}/{certifiedTimeId}/{patientId}")]
        [HttpGet]
        public JsonResult<IList> GetAllcertifiedOrderByDate(int userId, int certifiedTimeId,int patientId)
        {
            this._log.Debug("---Executing GetAllcertifiedOrderByDate() in OrdersController----");
            var result = this._ordersService.GetAllcertifiedOrderByDate(userId, certifiedTimeId, patientId).Result;
            this._log.Debug("---Executed Successfully GetAllcertifiedOrderByDate() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetPhysicianDropCertifyOrders/{nurseStationIds}")]
        [HttpGet]
        public JsonResult<List<PhysicianDropEntity>> GetPhysicianDropCertifyOrders(string nurseStationIds)
        {
            this._log.Debug("---Executing GetPhysicianDropCertifyOrders() in OrdersController----");
            var result = this._ordersService.GetPhysicianDropCertifyOrders(nurseStationIds).Result;
            this._log.Debug("---Executed Successfully GetAllcertifiedOrderByDate() in OrdersController----");
            return Json<List<PhysicianDropEntity>>(result);
        }
        [Route("GetPhysicianCredentialsByNPI/{phyNpi}")]
        [HttpGet]
        public string GetPhysicianCredentialsByNPI(string phyNpi)
        {
            this._log.Debug("---Executing GetPhysicianCredentialsByNPI() in OrdersController----");
            var result = this._ordersService.GetPhysicianCredentialsByNPI(phyNpi).Result;
            this._log.Debug("---Executed Successfully GetPhysicianCredentialsByNPI() in OrdersController----");
            return result;
        }
        [Route("GetAllOrderStockQtyonHand/{nurseStationId}")]
        [HttpGet]
        public JsonResult<IList> GetAllOrderStockQtyonHand(int nurseStationId)
        {
            this._log.Debug("---Executing GetAllOrderStockQtyonHand() in OrdersController----");
            var result = this._ordersService.GetAllOrderStockQtyonHand(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetAllOrderStockQtyonHand() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetOrdersGridDataByPatientId")]
        [HttpPost]
        public JsonResult<List<OrdersGridEntity>> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetOrdersGridDataByPatientId() in OrdersController----");
            var result = this._ordersService.GetOrdersGridDataByPatientId(orderFilter).Result;
            this._log.Debug("---Executed Successfully GetOrdersGridDataByPatientId() in OrdersController----");
            return Json<List<OrdersGridEntity>>(result);
        }
        [Route("GetPendingOrdersGridData")]
        [HttpPost]
        public JsonResult<List<OrdersGridEntity>> GetPendingOrdersGridData(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetPendingOrdersGridData() in OrdersController----");
            var result = this._ordersService.GetPendingOrdersGridData(orderFilter).Result;
            this._log.Debug("---Executed Successfully GetPendingOrdersGridData() in OrdersController----");
            return Json<List<OrdersGridEntity>>(result);
        }

        [Route("GetPendingOrdersGridDataInside")]
        [HttpPost]
        public JsonResult<List<OrdersGridEntity>> GetPendingOrdersGridDataInside(OrdersCustomFilterEntity orderFilter)
        {
            this._log.Debug("---Executing GetPendingOrdersGridData() in OrdersController----");
            var result = this._ordersService.GetPendingOrdersGridData(orderFilter).Result;
            var res = result.Where(x => x.Patient_Id == orderFilter.CompanyID).ToList();
            this._log.Debug("---Executed Successfully GetPendingOrdersGridData() in OrdersController----");
            return Json<List<OrdersGridEntity>>(res);
        }

        


        [Route("InsertConsolidateOrders")]
        [HttpPost]
        public JsonResult<string> InsertConsolidateOrders(ConsolidateCustomEntity entity)
        {
            this._log.Debug("---Executing InsertConsolidateOrders() in OrdersController----");
            var result = this._ordersService.InsertConsolidateOrders(entity).Result;
            this._log.Debug("---Executed Successfully InsertConsolidateOrders() in OrdersController----");
            return Json<string>(result);
        }
        [Route("UpdateHoldDcMultipleOrders")]
        [HttpPost]
        public JsonResult<int> UpdateHoldDcMultipleOrders(MultipleOrdersCustomEntity obj)
        {
            this._log.Debug("---Executing UpdateHoldDcMultipleOrders() in OrdersController----");
            var result = this._ordersService.UpdateHoldDcMultipleOrders(obj).Result;
            this._log.Debug("---Executed Successfully UpdateHoldDcMultipleOrders() in OrdersController----");
            return Json<int>(result);
        }
        [Route("GetPhysicianNPIByRoleRes/{userId}/{residentId}/{nurseStationId}")]
        [HttpGet]
        public JsonResult<string> GetPhysicianNPIByRoleRes(int userId, int residentId, int nurseStationId)
        {
            this._log.Debug("---Executing GetPhysicianNPIByRoleRes() in OrdersController----");
            var result = this._ordersService.GetPhysicianNPIByRoleRes(userId, residentId,nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetPhysicianNPIByRoleRes() in OrdersController----");
            return Json<string>(result);
        }
        [Route("InsertProfileOrdersCertification")]
        [HttpPost]
        public string InsertProfileOrdersCertification(OrdersCertifyCustomEntity obj)
        {
            this._log.Debug("---Executing InsertProfileOrdersCertification() in OrdersController----");
            var result = this._ordersService.InsertProfileOrdersCertification(obj).Result;
            this._log.Debug("---Executed Successfully InsertProfileOrdersCertification() in OrdersController----");
            return result;
        }
        [Route("GetProfileCertifiedDatesDrop/{userId}/{patientId}/{fromdate}/{todate}")]
        [HttpGet]
        public JsonResult<List<ProfileCertifiedDatesDropEntity>> GetProfileCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate)
        {
            this._log.Debug("---Executing GetProfileCertifiedDatesDrop() in OrdersController----");
            var result = this._ordersService.GetProfileCertifiedDatesDrop(userId, patientId, fromdate,todate).Result;
            this._log.Debug("---Executed Successfully GetProfileCertifiedDatesDrop() in OrdersController----");
            return Json<List<ProfileCertifiedDatesDropEntity>>(result);
        }
        [Route("GetAllProfilecertifiedOrderByDate/{userId}/{certifiedTimeId}/{patientId}")]
        [HttpGet]
        public JsonResult<IList> GetAllProfilecertifiedOrderByDate(int userId, int certifiedTimeId, int patientId)
        {
            this._log.Debug("---Executing GetAllProfilecertifiedOrderByDate() in OrdersController----");
            var result = this._ordersService.GetAllProfilecertifiedOrderByDate(userId, certifiedTimeId, patientId).Result;
            this._log.Debug("---Executed Successfully GetAllProfilecertifiedOrderByDate() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("RemoveConsolidateOrders")]
        [HttpPost]
        public JsonResult<string> RemoveConsolidateOrders(ConsolidateCustomEntity entity)
        {
            this._log.Debug("---Executing RemoveConsolidateOrders() in OrdersController----");
            var result = this._ordersService.RemoveConsolidateOrders(entity).Result;
            this._log.Debug("---Executed Successfully RemoveConsolidateOrders() in OrdersController----");
            return Json<string>(result);
        }
        [Route("GetCpoeSourceDrop/{userId}/{sourceId?}")]
        [HttpGet]
        public JsonResult<IList> GetCpoeSourceDrop(int userId, int? sourceId = null)
        {
            this._log.Debug("---Executing GetCpoeSourceDrop() in OrdersController----");
            var result = this._ordersService.GetCpoeSourceDrop(userId, sourceId).Result;
            this._log.Debug("---Executed Successfully GetCpoeSourceDrop() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetQuantityDoseDrop")]
        [HttpGet]
        public JsonResult<IList> GetQuantityDoseDrop()
        {
            this._log.Debug("---Executing GetQuantityDoseDrop() in OrdersController----");
            var result = this._ordersService.GetQuantityDoseDrop().Result;
            this._log.Debug("---Executed Successfully GetQuantityDoseDrop() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetUnitMeasurementsDrop")]
        [HttpGet]
        public JsonResult<IList> GetUnitMeasurementsDrop()
        {
            this._log.Debug("---Executing GetUnitMeasurementsDrop() in OrdersController----");
            var result = this._ordersService.GetUnitMeasurementsDrop().Result;
            this._log.Debug("---Executed Successfully GetUnitMeasurementsDrop() in OrdersController----");
            return Json<IList>(result);
        }
        [Route("GetDoseUomDrop")]
        [HttpGet]
        public JsonResult<IList> GetDoseUomDrop()
        {
            this._log.Debug("---Executing GetDoseUomDrop() in OrdersController----");
            var result = this._ordersService.GetDoseUom().Result;
            this._log.Debug("---Executed Successfully GetDoseUomDrop() in OrdersController----");
            return Json<IList>(result);
        }
    }
}