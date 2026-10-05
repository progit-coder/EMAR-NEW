using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;
using System.Data;

namespace LTCPro.ServiceLayer
{
    public interface IOrdersService
    {
        Task<List<OrdersGridEntity>> GetOrdersGridData(OrdersCustomFilterEntity orderFilter);
        //Task<OrderDetailsEntity> GetOrderDetails(Int64 OrderID, int patientID);
        //Task<List<OrdersGridCustomEntity>> GetAllOrdersByPatient(Int64 PatientID);
        Task<List<PhysicianDropEntity>> GetPhysicianDropData(int? facilityId = null, int? nurseStatioId = null, int? orderId = null);
        //Task<int> ControlTypeUpdate(OrderControlUpdateEntity orderControl);
        Task<List<FrequencyMasterEntity>> GetFrequencyMasterData();
        Task<List<FrequencyMasterEntityWithShifts>> GetFrequencyMasterDataWithShifts(int stationId);
        Task<List<WeekEntity>> GetWeekMasterData();
        Task<List<MonthEntity>> GetMonthMasterData();
        Task<List<HourEntity>> GetHoursDataByNSId(int nurseStationId, int facilityId);
        Task<List<HourEntity>> GetHoursMasterData();
        // Task<List<TimeFormatEntity>> GetTimeFormatMasterData();
        // Task<int> InsertDrugAdministrationTime(DrugAdministrationTimeEntity DrugAdmTime);
        //Task<DrugAdministrationTimeEntity> GetScheduleTimeDetails(int OrderID);
        //Task<OrdersTypeCountEntity> GetOrdersTypeCounts(int userId);
        Task<OrdersinfoCustomEntity> GetDiagnosisDetails(int PatientID);
        //Task<int> InsertUpdateResidentOrders(ResidentOrderEntity ResOrders);
        //Task<List<ResidentOrderEntity>> GetAllResidentOrders(int PatientID);
        //Task<ResidentOrderEntity> GetResidentOrderDetails(int ResOrderID);
        //Task<int> RemoveResidentOrderbyID(int ResOrderID);
        //Task<int> InsertOrder(InsertOrdersEntity orderDetails);
        //Task<int> InsertBarcodeDetails(BarcodeDetailEntity barcodes);
        //Task<List<BarcodeDetailEntity>> GetBarcodeData(int OrderID);
        //Task<int> DeleteBarcode(int BarcodeID);
        Task<List<OrderFavouriteCustomEntity>> GetFavouritesMasterData(int quantityId, int facilityId);
        Task<int> InsertOrderFavourities(List<OrderFavouriteEntity> OrderFavouroties);
        //Task<List<OrderFavouriteEntity>> GetFavouritesByOrderID(int OrderID);
        Task<int> InsertOrderHoldDetails(OrderHoldEntity orderHold);
        Task<IList> GetMergeOrdersByPatientId(int patientId, int orderId, int quantityId);
        Task<int> MergeTwoOrders(int orderQtyId1, int orderQtyId2, int orderQtyId3, string endDateMerge, int userID, string startDateMerge);
        Task<NursingFrequencyConfigEntity> GetNurseFrequencyDropSelect(int frequencyId, int nurseStationId);
        Task<string> InsertOrderdestroy(OrderDestroyEntity orderDestroy);
        //Task<string> CheckDischargeInterval(int patientId);
        Task<List<ControlSubstanceGridEntity>> GetControlSubstanceGridData(ControlSubstanceFilter filter);
        Task<string> CheckCertifyAndApprovals(CertifyAndApprovalCheckEntity credentials);
        //Task<string> GetOrderScheduleText(int porderId);
        Task<List<AcknowledgeOrdersCustomEntity>> GetAckOrders(int userId);
        Task<int> AcceptAckOrders(List<AcknowledgeOrdersCustomEntity> entity);
        Task<int> RejectAckOrders(List<AcknowledgeOrdersCustomEntity> entity);
        Task<List<DemographicResidentDropEnity>> GetAckOrdersResidentDrop(int userId);
        Task<List<AcknowledgeOrdersCustomEntity>> GetAckOrdersByPatientId(int PatientId);
        Task<List<DrFirstFileDataEntity>> GetDrFirstFilesData();
        Task<List<DemographicResidentDropEnity>> GetControlSubstanceResDrop(ControlSubstanceFilter filters);
        Task<List<ControlSubstanceGridEntity>> GetControlSubstanceGridDataByPid(int patientId, string gpi, int ConsolidateFlag);
        #region
        //created by:sampath
        Task<List<OrderRouteEntity>> GetOrderRoutes();
        Task<IList> GetOrderGridData(int patientId, string status);
        Task<OrdersDataEntity> GetOrdersData(int orderId, int quantityId, int userId);
        Task<int> CheckBarcodeAlert(string BarcodeData, string GpiNum, int patientId, int facilityId, int orderId);

        Task<int> InsertOrderCommonStatus(OrdersCommonStatusEntity obj);
        Task<int> UpdateOrdersDatabyOrderId(OrderUpdateEntity obj);
        Task<int> UpdateOrderHoldStatus(OrderHoldEntity obj);
        Task<string> InsertupdateHOA(HOAEntity obj);
        Task<IList> GetStockQtyonHand(string drugName, int nurseStationId);
        Task<DrugAdministrationTimeEntity> GetHoaDetails(int orderId, int quantityId, int nurseStatioId);
        Task<DataSet> GetEmarpreviewdetails(int month, int year, int patientId);
        Task<string> GetEmarpreviewdetailslegend(int month, int year, int patientId);
        Task<OrderStockEntity> GetOrderStockDetails(int OrderId);
        Task<int> UpdateDrFirstOrderAcknowledge(DrFirstOrderXMLTransEntity entity);
        Task<List<LiteralOrderEntity>> GetLiteralOrderGridData(int patientId);
        #endregion

        Task<Tuple<IList, int>> GetOrdersEndingSoon(string nursingStationId);
        Task<int> ConfirmOrdersEndingSoon(List<ConfirmOrdersEndDateEntity> orders);
        //Task<IList> SearchDrugName(string drugName);
        Task<List<string>> GetEmarPreviewYearDrop(int patentId);
        Task<int> InsertUpdateNurseNotes(NurseCommentsEntity notes);
        Task<List<NurseCommentsEntity>> GetNurseNotes(int pQuantityId);
        Task<string> GetScheduledTimeText(Nullable<int> administrationType, Nullable<int> nursingFreq_Id, string nurseshifts_Id, string hour_Id, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string week_Id, string month_Id, string days, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> nurseStationId);
        Task<int> OrderEndingSoonStatus(int userId, int screenId);
        Task<List<CustomPassShiftTimeEntity>> GetPasstimeShiftsData(string nurseStationIds, int facilityId);
        Task<OrderHoldEntity> GetOrderHoldData(int pQuantityId);
        Task<int> DiscontinueCertifyOrders(List<OrdersCommonStatusEntity> list);
        Task<string> InsertOrdersCertification(OrdersCertifyCustomEntity obj);
        Task<List<CertifiedDatesDropEntity>> GetCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate, string phynpi);
        Task<IList> GetAllcertifiedOrderByDate(int userId, int certifiedTimeId, int patientId);
        Task<List<PhysicianDropEntity>> GetPhysicianDropCertifyOrders(string nurseStationIds);
        Task<string> GetPhysicianCredentialsByNPI(string phyNpi);
        Task<IList> GetAllOrderStockQtyonHand(int nurseStationId);
        Task<List<OrdersGridEntity>> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter);
        Task<List<OrdersGridEntity>> GetPendingOrdersGridData(OrdersCustomFilterEntity orderFilter);
        Task<string> InsertConsolidateOrders(ConsolidateCustomEntity entity);
        Task<int> UpdateHoldDcMultipleOrders(MultipleOrdersCustomEntity obj);
        Task<string> GetPhysicianNPIByRoleRes(int userId, int residentId, int nurseStationId);
        Task<string> InsertProfileOrdersCertification(OrdersCertifyCustomEntity obj);
        Task<List<ProfileCertifiedDatesDropEntity>> GetProfileCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate);
        Task<IList> GetAllProfilecertifiedOrderByDate(int userId, int certifiedTimeId, int patientId);
        Task<string> RemoveConsolidateOrders(ConsolidateCustomEntity entity);
        Task<IList> GetCpoeSourceDrop(int userId, int? sourceId = null);
        Task<IList> GetQuantityDoseDrop();
        Task<IList> GetUnitMeasurementsDrop();
        Task<IList> GetDoseUom();
        Task<string> GetDefultNursingstationPrescriber(string FacilityId, string NurseId);
        Task<int> OrderFavouritiesOrderChange(int? OrderId, int? UserID);
        Task<byte[]> GetImage(int PatientId);
        Task<List<DrugNameSearch>> SearchDrugName(string drugName);
    }
}
