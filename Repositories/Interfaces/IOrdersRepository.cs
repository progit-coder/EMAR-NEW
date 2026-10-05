using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;
using System.Data;

namespace LTCPro.Repositories
{
    public interface IOrdersRepository
    {
        List<OrdersGridEntity> GetOrdersGridData(OrdersCustomFilterEntity orderFilter);
        //OrderDetailsEntity GetOrderDetails(Int64 OrderID, int patientID);
        //List<OrdersGridCustomEntity> GetAllOrdersByPatient(Int64 PatientID);
        List<PhysicianDropEntity> GetPhysicianDropData(int? facilityId = null,int? nurseStatioId=null,int? orderId=null);
        int ControlTypeUpdate(OrderControlUpdateEntity orderControl);
        List<FrequencyMasterEntity> GetFrequencyMasterData();
        List<FrequencyMasterEntityWithShifts> GetFrequencyMasterDataWithShifts(int stationId);
        List<WeekEntity> GetWeekMasterData();
        List<MonthEntity> GetMonthMasterData();
        List<HourEntity> GetHoursDataByNSId(int nurseStationId, int facilityId);
        List<HourEntity> GetHoursMasterData();
       // List<TimeFormatEntity> GetTimeFormatMasterData();
      //  int InsertDrugAdministrationTime(DrugAdministrationTimeEntity DrugAdmTime);
        //DrugAdministrationTimeEntity GetScheduleTimeDetails(int OrderID);
        //OrdersTypeCountEntity GetOrdersTypeCounts(int userId);
        OrdersinfoCustomEntity GetDiagnosisDetails(int PatientID);
        //int InsertUpdateResidentOrders(ResidentOrderEntity ResOrders);
        //List<ResidentOrderEntity> GetAllResidentOrders(int PatientID);
        //ResidentOrderEntity GetResidentOrderDetails(int ResOrderID);
        //int RemoveResidentOrderbyID(int ResOrderID);
        //int InsertOrder(InsertOrdersEntity orderDetails);
        //int InsertBarcodeDetails(BarcodeDetailEntity barcodes);
        //List<BarcodeDetailEntity> GetBarcodeData(int OrderID);
        //int DeleteBarcode(int BarcodeID);
        List<OrderFavouriteCustomEntity> GetFavouritesMasterData(int quantityId, int facilityId);
        int InsertOrderFavourities(List<OrderFavouriteEntity> OrderFavouroties);
        //List<OrderFavouriteEntity> GetFavouritesByOrderID(int OrderID);
        int InsertOrderHoldDetails(OrderHoldEntity orderHold);
        IList GetMergeOrdersByPatientId(int patientId,int orderId, int quantityId);
        int MergeTwoOrders(int orderQtyId1, int orderQtyId2, int orderQtyId3, string endDateMerge,int userID, string startDateMerge);
        NursingFrequencyConfigEntity GetNurseFrequencyDropSelect(int frequencyId, int nurseStationId);
        string InsertOrderdestroy(OrderDestroyEntity orderDestroy);
        //string CheckDischargeInterval(int patientId, int interval);
        List<ControlSubstanceGridEntity> GetControlSubstanceGridData(ControlSubstanceFilter filter);
        int SaveControlSubstance(ControlSubstanceSave controlObj,int createdby,int approvedby, DateTime approvedOn);
        string GetOrderScheduleText(int porderId);
        List<AcknowledgeOrdersCustomEntity> GetAckOrders(int userId);
        int AcceptAckOrders(List<AcknowledgeOrdersCustomEntity> entity);
        int RejectAckOrders(List<AcknowledgeOrdersCustomEntity> entity);
        List<DemographicResidentDropEnity> GetAckOrdersResidentDrop(int userId);
        List<AcknowledgeOrdersCustomEntity> GetAckOrdersByPatientId(int PatientId);
        List<DrFirstFileDataEntity> GetDrFirstFilesData();

        #region 
        //created by:sampath
        List<OrderRouteEntity> GetOrderRoutes();
        IList GetOrderGridData(int patientId,string status);
        OrdersDataEntity GetOrdersData(int orderId, int quantityId, int userId);
        int CheckBarcodeAlert(string BarcodeData, string GpiNum, int patientId, int facilityId, int orderId);

        int InsertOrderCommonStatus(OrdersCommonStatusEntity obj);
        int UpdateOrdersDatabyOrderId(OrderUpdateEntity obj);
        int UpdateOrderHoldStatus(OrderHoldEntity obj);
        string InsertupdateHOA(HOAEntity obj);
        IList GetStockQtyonHand(string drugName, int nurseStationId);
        DrugAdministrationTimeEntity GetHoaDetails(int orderId, int quantityId, int nurseStatioId);
        DataSet GetEmarpreviewdetails(int month, int year, int patientId);
        string GetEmarpreviewdetailslegend(int month, int year, int patientId);
        OrderStockEntity GetOrderStockDetails(int OrderId);
        int UpdateDrFirstOrderAcknowledge(DrFirstOrderXMLTransEntity entity);
        List<LiteralOrderEntity> GetLiteralOrderGridData(int patientId);
        #endregion
        List<DemographicResidentDropEnity> GetControlSubstanceResDrop(ControlSubstanceFilter filters);
        List<ControlSubstanceGridEntity> GetControlSubstanceGridDataByPid(int patientId, string gpi, int ConsolidateFlag);
        Tuple<IList, int> GetOrdersEndingSoon(string nursingStationId);
        int ConfirmOrdersEndingSoon(List<ConfirmOrdersEndDateEntity> orders);
        //IList SearchDrugName(string drugName);
        List<string> GetEmarPreviewYearDrop(int patientId);
        int InsertUpdateNurseNotes(NurseCommentsEntity notes);
        List<NurseCommentsEntity> GetNurseNotes(int pQuantityId);
        string GetScheduledTimeText(Nullable<int> administrationType, Nullable<int> nursingFreq_Id, string nurseshifts_Id, string hour_Id, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string week_Id, string month_Id, string days, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> nurseStationId);
        int OrderEndingSoonStatus(int userId, int screenId);
        List<CustomPassShiftTimeEntity> GetPasstimeShiftsData(string nurseStationIds, int facilityId);
        OrderHoldEntity GetOrderHoldData(int pQuantityId);
        string InsertOrdersCertification(OrdersCertifyCustomEntity obj);
        List<CertifiedDatesDropEntity> GetCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate,string phynpi);
        IList GetAllcertifiedOrderByDate(int userId, int certifiedTimeId,int patientId);
        List<PhysicianDropEntity> GetPhysicianDropCertifyOrders(string nurseStationIds);
        string GetPhysicianCredentialsByNPI(string phyNpi);
        IList GetAllOrderStockQtyonHand(int nurseStationId);
        List<OrdersGridEntity> GetOrdersGridDataByPatientId(OrdersCustomFilterEntity orderFilter);
        List<OrdersGridEntity> GetPendingOrdersGridData(OrdersCustomFilterEntity orderFilter);
        int InsertScheduleTimeText(int Porder_Id, int Pquantity_Id, string ScheduleText);
        int InsertConsolidateOrders(ConsolidateCustomEntity entity);
        string GetPhysicianNPIByRoleRes(int userId, int residentId, int nurseStationId);
        string InsertProfileOrdersCertification(OrdersCertifyCustomEntity obj);
        List<ProfileCertifiedDatesDropEntity> GetProfileCertifiedDatesDrop(int userId, int patientId, string fromdate, string todate);
        IList GetAllProfilecertifiedOrderByDate(int userId, int certifiedTimeId, int patientId);
        int RemoveConsolidateOrders(ConsolidateCustomEntity entity);
        IList GetCpoeSourceDrop(int userId, int? sourceId = null);
        IList GetQuantityDoseDrop();
        IList GetUnitMeasurementsDrop();
        IList GetDoseUom();
        string GetDefultNursingstationPrescriber(string FacilityId, string NurseId);
        int OrderFavouritiesOrderChange(int? OrderId, int? UserID);
        byte[] GetImage(int PatientId);
        List<DrugNameSearch> SearchDrugName(string drugName);
    }
}
