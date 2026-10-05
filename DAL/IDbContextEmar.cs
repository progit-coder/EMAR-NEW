using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.DAL
{
    public interface IDbContextEmar : IDisposable
    {
        int SaveChanges();
        DbSet<AllergyTypeCode> AllergyTypeCodes { get; set; }
        DbSet<ApiIntegration> ApiIntegrations { get; set; }
        DbSet<Bed> Beds { get; set; }
        DbSet<BehavioralSymptomsMaster> BehavioralSymptomsMasters { get; set; }
        DbSet<ClassDrugCodingType> ClassDrugCodingTypes { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<CompanyBedConfig> CompanyBedConfigs { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<DiagnosisCodingType> DiagnosisCodingTypes { get; set; }
        DbSet<DocFolder> DocFolders { get; set; }
        DbSet<EventCategory> EventCategories { get; set; }
        DbSet<Facility> Facilities { get; set; }
        DbSet<FileAckInformation> FileAckInformations { get; set; }
        DbSet<FileInformation> FileInformations { get; set; }
        DbSet<Floor> Floors { get; set; }
        DbSet<FrequencyMaster> FrequencyMasters { get; set; }
        DbSet<FTECategory> FTECategories { get; set; }
        DbSet<FTEConfiguration> FTEConfigurations { get; set; }
        DbSet<FteConnection> FteConnections { get; set; }
        DbSet<Gender> Genders { get; set; }
        DbSet<HLSevenCompanyConfig> HLSevenCompanyConfigs { get; set; }
        DbSet<HLSevenSegmentDetail> HLSevenSegmentDetails { get; set; }
        DbSet<HLSevenSegment> HLSevenSegments { get; set; }
        DbSet<Hour> Hours { get; set; }
        DbSet<ICD10> ICD10 { get; set; }
        DbSet<ImportFile> ImportFiles { get; set; }
        DbSet<IntegrationType> IntegrationTypes { get; set; }
        DbSet<MailConfig> MailConfigs { get; set; }
        DbSet<MaritalStatu> MaritalStatus { get; set; }
        DbSet<MedicationReason> MedicationReasons { get; set; }
        DbSet<Month> Months { get; set; }
        DbSet<NursingFrequencyConfig> NursingFrequencyConfigs { get; set; }
        DbSet<NursingSchedule> NursingSchedules { get; set; }
        DbSet<NursingStation> NursingStations { get; set; }
        DbSet<OrderControlMaster> OrderControlMasters { get; set; }
        DbSet<OrderFavouriteMaster> OrderFavouriteMasters { get; set; }
        DbSet<OrderType> OrderTypes { get; set; }
        DbSet<PhysicianDetail> PhysicianDetails { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RoleConfig> RoleConfigs { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Screen> Screens { get; set; }
        DbSet<Suffix> Suffixes { get; set; }
        DbSet<TimeFormat> TimeFormats { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRoleFacilityConfig> UserRoleFacilityConfigs { get; set; }
        DbSet<WeekDay> WeekDays { get; set; }
        DbSet<Week> Weeks { get; set; }
        DbSet<Wing> Wings { get; set; }
        DbSet<AddlInstructionDetail> AddlInstructionDetails { get; set; }
        DbSet<AllergyInfo> AllergyInfoes { get; set; }
        DbSet<AncillaryDetail> AncillaryDetails { get; set; }
        DbSet<BarcodeDetail> BarcodeDetails { get; set; }
        DbSet<CommonOrderInfo> CommonOrderInfoes { get; set; }
        DbSet<CompoundOrder> CompoundOrders { get; set; }
        DbSet<Demographic> Demographics { get; set; }
        DbSet<DiagnosisInfo> DiagnosisInfoes { get; set; }
        DbSet<DrugAdminister> DrugAdministers { get; set; }
        DbSet<DrugAdministrationTime> DrugAdministrationTimes { get; set; }
        DbSet<EncodedOrderDetail> EncodedOrderDetails { get; set; }
        DbSet<InsuranceInfo> InsuranceInfoes { get; set; }
        DbSet<NotesInfo> NotesInfoes { get; set; }
        DbSet<Observation> Observations { get; set; }
        DbSet<OrderFavourite> OrderFavourites { get; set; }
        DbSet<OrderHold> OrderHolds { get; set; }
        DbSet<OutBoundFileInformation> OutBoundFileInformations { get; set; }
        DbSet<QuantityDetail> QuantityDetails { get; set; }
        DbSet<ResidentOrder> ResidentOrders { get; set; }
        DbSet<TreatmentDispenseInfo> TreatmentDispenseInfoes { get; set; }
        DbSet<TreatmentInfo> TreatmentInfoes { get; set; }
        DbSet<TreatmentRouteInfo> TreatmentRouteInfoes { get; set; }
        DbSet<VisitBehaviour> VisitBehaviours { get; set; }
        DbSet<VisitFoodintake> VisitFoodintakes { get; set; }
        DbSet<VisitInfo> VisitInfoes { get; set; }
        DbSet<VisitNursingNote> VisitNursingNotes { get; set; }
        DbSet<VisitVital> VisitVitals { get; set; }
        DbSet<WeightLog> WeightLogs { get; set; }
        DbSet<UploadedDocument> UploadedDocuments { get; set; }
        DbSet<AllergyInfoMaster> AllergyInfoMasters { get; set; }
        DbSet<MailBox> MailBoxes { get; set; }
        DbSet<ApprovalOrder> ApprovalOrders { get; set; }
        DbSet<tmptblSegmentCheck> tmptblSegmentChecks { get; set; }
        DbSet<OrderDestroy> OrderDestroys { get; set; }
        DbSet<ApprovalAllergyInfo> ApprovalAllergyInfoes { get; set; }
        DbSet<ApprovalDemographic> ApprovalDemographics { get; set; }
        DbSet<ApprovalDiagnosisInfo> ApprovalDiagnosisInfoes { get; set; }
        DbSet<ApprovalVisitInfo> ApprovalVisitInfoes { get; set; }
        DbSet<ControlSubstanceCount> ControlSubstanceCounts { get; set; }
        DbSet<NurseCommentType> NurseCommentTypes { get; set; }
        DbSet<ApprovalRefill> ApprovalRefills { get; set; }
        DbSet<OrderFavouriteData> OrderFavouriteDatas { get; set; }
        DbSet<Conversation> Conversations { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Participant> Participants { get; set; }
        DbSet<User2> Users1 { get; set; }
        DbSet<FileInfoError> FileInfoErrors { get; set; }
        DbSet<NurseComment> NurseComments { get; set; }
        DbSet<PatientType> PatientTypes { get; set; }
        DbSet<ColourType> ColourTypes { get; set; }
        DbSet<ActivityMaster> ActivityMasters { get; set; }
        DbSet<UserActivityDetail> UserActivityDetails { get; set; }
        DbSet<UserSession> UserSessions { get; set; }
        DbSet<UserOTP> UserOTPs { get; set; }
        DbSet<MailFavourite> MailFavourites { get; set; }
        DbSet<MailRead> MailReads { get; set; }
        DbSet<Fingersdesc> Fingersdescs { get; set; }
        DbSet<Ekit> Ekits { get; set; }
        DbSet<Stock> Stocks { get; set; }
        DbSet<DrFirstXMLTran> DrFirstXMLTrans { get; set; }
        DbSet<DrFirstOrderXMLTran> DrFirstOrderXMLTrans { get; set; }
        DbSet<AlertStatu> AlertStatus { get; set; }
        DbSet<AlertText> AlertTexts { get; set; }
        DbSet<AlertType> AlertTypes { get; set; }
        DbSet<Route> Routes { get; set; }
        DbSet<Drug> Drugs { get; set; }
        DbSet<EkitAdminister> EkitAdministers { get; set; }
        DbSet<DrugActiveday> DrugActivedays { get; set; }
        DbSet<DrFirstFileData> DrFirstFileDatas { get; set; }
        DbSet<OrderStock> OrderStocks { get; set; }
        DbSet<NurseShift> NurseShifts { get; set; }
        DbSet<CompanyConfig> CompanyConfigs { get; set; }
        DbSet<CompanyHlCategory> CompanyHlCategories { get; set; }
        DbSet<CompanyHLEvent> CompanyHLEvents { get; set; }
        DbSet<HLDirectionalWay> HLDirectionalWays { get; set; }
        DbSet<StockReport> StockReports { get; set; }
        DbSet<OnLeave> OnLeaves { get; set; }
        DbSet<DefaultScreen> DefaultScreens { get; set; }
        DbSet<NursingFCTime> NursingFCTimes { get; set; }
        DbSet<RecentFac> RecentFacs { get; set; }
        DbSet<HLSevenOutboundDisplayCompanyConfig> HLSevenOutboundDisplayCompanyConfigs { get; set; }
        DbSet<HLSevenOutboundDisplaySegmentDetail> HLSevenOutboundDisplaySegmentDetails { get; set; }
        DbSet<HLSevenOutboundDisplaySegment> HLSevenOutboundDisplaySegments { get; set; }
        DbSet<DrugOrderMaster> DrugOrderMasters { get; set; }
        DbSet<ControlSubstanceReason> ControlSubstanceReasons { get; set; }
        DbSet<DocAdministerTran> DocAdministerTrans { get; set; }
        DbSet<NurseStationHierarchy> NurseStationHierarchies { get; set; }
        DbSet<FooterLogo> FooterLogoes { get; set; }
        DbSet<ControlSubstanceTran> ControlSubstanceTrans { get; set; }
        DbSet<OrderStockTran> OrderStockTrans { get; set; }
        DbSet<ProcessKeyMaster> ProcessKeyMasters { get; set; }
        DbSet<CertifyDate> CertifyDates { get; set; }
        DbSet<CertifyOrder> CertifyOrders { get; set; }
        DbSet<OrderFavConfig> OrderFavConfigs { get; set; }
        DbSet<tblSideEffect> tblSideEffects { get; set; }
        DbSet<tblDrugDiscardInfo> tblDrugDiscardInfoes { get; set; }
        DbSet<tblAdministeredSite> tblAdministeredSites { get; set; }
        DbSet<tblRDCMailConfig> tblRDCMailConfigs { get; set; }
        DbSet<tblRdcMailTime> tblRdcMailTimes { get; set; }
        DbSet<OrderHOAInfo> OrderHOAInfoes { get; set; }
        DbSet<tblTimeZone> tblTimeZones { get; set; }
        DbSet<PCertifyDate> PCertifyDates { get; set; }
        DbSet<PCertifyOrder> PCertifyOrders { get; set; }
        DbSet<EkitControlSubstanceReason> EkitControlSubstanceReasons { get; set; }
        DbSet<EKitControlSubstanceTran> EKitControlSubstanceTrans { get; set; }
        DbSet<CpoeSource> CpoeSources { get; set; }
        DbSet<QuantityDose> QuantityDoses { get; set; }
        DbSet<UnitMeasurement> UnitMeasurements { get; set; }
        DbSet<DoseUom> DoseUoms { get; set; }
        ObjectResult<PrcFirstDrugAdministerSchedule_Result> PrcFirstDrugAdministerSchedule(Nullable<int> userId, string facilityId, string nursingstationId, string floor, string wing, string room, string bed);
        ObjectResult<PrcPRNDrugAdministerSchedule_Result> PrcPRNDrugAdministerSchedule(Nullable<int> userId, string facilityId, string nursingstationId, string floor, string wing, string room, string bed);
        ObjectResult<PrcGetDempographicsChanges_Result> PrcGetDempographicsChanges(Nullable<int> patientID, Nullable<int> approvalID);
        ObjectResult<PrcGetVisitInfoChanges_Result> PrcGetVisitInfoChanges(Nullable<int> patientID, Nullable<int> approvalID);
        ObjectResult<PrcGetOrderFavConfigByCode_Result> PrcGetOrderFavConfigByCode(Nullable<int> facility_Id, string orderFavCode, Nullable<System.DateTime> oFConfig_CreatedDate);
        ObjectResult<PrcGetOrderFavConfigInfo_Result> PrcGetOrderFavConfigInfo(Nullable<int> facility_Id);
        int PrcInsertOrderFavFacilityConfig(Nullable<int> facility_Id, string orderFavCode, string orderFavMaster, Nullable<int> oFConfig_Status, Nullable<int> oFConfig_CreatedBy, Nullable<System.DateTime> oFConfig_CreatedDate, Nullable<int> @event);

        ObjectResult<Nullable<int>> PrcInsertDataAfterFileUpload(string file, Nullable<int> createdBy);

        ObjectResult<string> PrcGenerateHL7forPatientDiscontinueOrder(Nullable<int> patientId, Nullable<int> porderID, string messageID);
        ObjectResult<string> PrcGenerateHL7forPatientUpdate(Nullable<int> patientId, string category, string messageID);
        ObjectResult<string> PrcGenerateHL7forPatientNewOrder(Nullable<int> patientId, Nullable<int> porderID, string messageID);
        ObjectResult<string> PrcGenerateHL7forPatientRefillOrder(Nullable<int> patientId, Nullable<int> porderID, string messageID);
        ObjectResult<Nullable<int>> PrcMergeOrders(Nullable<int> quantityId1, Nullable<int> quantityId2, Nullable<int> quantityId3, Nullable<System.DateTime> enddate, Nullable<int> userid);
        ObjectResult<PrcMergeOrderData_Result> PrcMergeOrderData(Nullable<int> patientId, Nullable<int> porderId, Nullable<int> pquantityId);
        ObjectResult<PrcGetBedChangesDetails_Result> PrcGetBedChangesDetails(Nullable<int> nursingStationId);

        ObjectResult<PrcGetCompanyChangesdetails_Result> PrcGetCompanyChangesdetails(Nullable<int> companyId);

        ObjectResult<PrcGetFacilityChangesDetails_Result> PrcGetFacilityChangesDetails(Nullable<int> facilityId);
        ObjectResult<PrcGetFloorChangesDetails_Result> PrcGetFloorChangesDetails(Nullable<int> nursingStationId);

        ObjectResult<PrcGetNursingStationChangesDetails_Result> PrcGetNursingStationChangesDetails(Nullable<int> nursingStationId);

        ObjectResult<PrcGetRoomChangesDetails_Result> PrcGetRoomChangesDetails(Nullable<int> nursingStationId);

        ObjectResult<PrcGetUserChangesDetails_Result> PrcGetUserChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetWingChangesDetails_Result> PrcGetWingChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetFreqMappChangesDetails_Result> PrcGetFreqMappChangesDetails(Nullable<int> nursingStationId);

        ObjectResult<PrcGetFTConfigChangesDetails_Result> PrcGetFTConfigChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetRoleChangesDetails_Result> PrcGetRoleChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetRoleConfigChangesDetails_Result> PrcGetRoleConfigChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetUserRoleConfigChangesDetails_Result> PrcGetUserRoleConfigChangesDetails(Nullable<int> userId, Nullable<int> roleId, Nullable<int> facilityId);
        ObjectResult<PrcGetAllergyInfoChanges_Result> PrcGetAllergyInfoChanges(Nullable<int> patientID, Nullable<int> approvalID);
        ObjectResult<PrcGetDiagnosisInfoChanges_Result> PrcGetDiagnosisInfoChanges(Nullable<int> patientID, Nullable<int> approvalID);
        ObjectResult<PrcDashboardVitalsdata_Result> PrcDashboardVitalsdata();
        ObjectResult<PrcgetReportsAllergyData_Result> PrcgetReportsAllergyData(string patientId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId, string floorId, string wingId, string roomId, string bedId);

        ObjectResult<PrcgetReportsInActiveData_Result> PrcgetReportsInActiveData(string patientId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId, string floorId, string wingId, string roomId, string bedId);

        ObjectResult<PrcgetReports72HrscheckData_Result> PrcgetReports72HrscheckData(string patientId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId, string floorId, string wingId, string roomId, string bedId);

        ObjectResult<PrcgetReportsCensusData_Result> PrcgetReportsCensusData(string patientId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId, string floorId, string wingId, string roomId, string bedId);
        ObjectResult<PrcGetReportsCompanyData_Result> PrcGetReportsCompanyData();
        ObjectResult<PrcGetReportsFacilityData_Result> PrcGetReportsFacilityData(Nullable<int> companyId);

        ObjectResult<PrcGetReportsNurseStationData_Result> PrcGetReportsNurseStationData(Nullable<int> companyId, Nullable<int> facilityId);
        ObjectResult<PrcInsertDrFirstData_Result> PrcInsertDrFirstData(string xml);
        ObjectResult<PrcGetReportsBedData_Result> PrcGetReportsBedData(Nullable<int> companyId, Nullable<int> facilityId, Nullable<int> nurseStationId, Nullable<int> floorId, Nullable<int> wingId, Nullable<int> roomId, Nullable<int> bedId);

        ObjectResult<PrcGetReportsFloorData_Result> PrcGetReportsFloorData(Nullable<int> companyId, Nullable<int> facilityId, Nullable<int> nurseStationId, Nullable<int> floorId, Nullable<int> wingId, Nullable<int> roomId, Nullable<int> bedId);

        ObjectResult<PrcGetReportsRoomData_Result> PrcGetReportsRoomData(Nullable<int> companyId, Nullable<int> facilityId, Nullable<int> nurseStationId, Nullable<int> floorId, Nullable<int> wingId, Nullable<int> roomId, Nullable<int> bedId);

        ObjectResult<PrcGetReportsWingData_Result> PrcGetReportsWingData(Nullable<int> companyId, Nullable<int> facilityId, Nullable<int> nurseStationId, Nullable<int> floorId, Nullable<int> wingId, Nullable<int> roomId, Nullable<int> bedId);
        ObjectResult<byte[]> PrcGetFooterLogo();

        ObjectResult<PrcGetHeaderLogo_Result> PrcGetHeaderLogo(Nullable<int> companyId, Nullable<int> facilityId);
        ObjectResult<PrcgetReportsCensusDatainfo_Result> PrcgetReportsCensusDatainfo(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId, Nullable<int> type, Nullable<int> reportType);
        ObjectResult<DrFirstOrderCheck_Result> DrFirstOrderCheck(Nullable<int> patientId);
        ObjectResult<PrcgetReportsCensusDetail_Result> PrcgetReportsCensusDetail(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationName, Nullable<int> type, Nullable<int> reportType, string value);
        ObjectResult<PrcgetCensusCompareDatainfo_Result> PrcgetCensusCompareDatainfo(string years, Nullable<int> userId, Nullable<int> nursingstationId, Nullable<int> type, Nullable<int> reportType);
        ObjectResult<PrcGetReports72HoursData_Result> PrcGetReports72HoursData(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate, Nullable<int> userId, string nursingstationId);
        ObjectResult<PrcReportsGetOrderControlSignoff_Result> PrcReportsGetOrderControlSignoff(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, string nurseStation, Nullable<int> userId);
        ObjectResult<PrcReportsGetOrderDetails_Result> PrcReportsGetOrderDetails(string passTime, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> orderType, Nullable<int> userid, string nursingStationId, Nullable<int> shiftId);
        ObjectResult<PrcReportsGetOrderHoldDetails_Result> PrcReportsGetOrderHoldDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetOrderWithFavourites_Result> PrcReportsGetOrderWithFavourites(string passTime, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId, Nullable<int> shiftId);
        ObjectResult<PrcReportsGetPRNDetails_Result> PrcReportsGetPRNDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetWithoutBarcodeDetails_Result> PrcReportsGetWithoutBarcodeDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetWithoutBiometricsDetails_Result> PrcReportsGetWithoutBiometricsDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetOrderControlSubstance_Result> PrcReportsGetOrderControlSubstance(string nurseStation, Nullable<int> userId);
        ObjectResult<PrcReportsGetDestructionDetails_Result> PrcReportsGetDestructionDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetFloorStockDetails_Result> PrcReportsGetFloorStockDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetOrderChangeDetails_Result> PrcReportsGetOrderChangeDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetPharmacyMedsDetails_Result> PrcReportsGetPharmacyMedsDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetRefusedByResidentDetails_Result> PrcReportsGetRefusedByResidentDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId,string gPI);
        ObjectResult<Prcgetgpidropdown_Result> Prcgetgpidropdown(string nursingStation, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> type);
        ObjectResult<PrcReportsGetWithoutScanningDetails_Result> PrcReportsGetWithoutScanningDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcGetDemographicChangesDetails_Result> PrcGetDemographicChangesDetails(Nullable<int> patientId);
        ObjectResult<PrcGetVisitChangesDetails_Result> PrcGetVisitChangesDetails(Nullable<int> pVisitId);
        ObjectResult<PrcDrFirstOrderHL7_Result> PrcDrFirstOrderHL7(Nullable<int> patientId);
        ObjectResult<PrcGetAverageCensusData_Result> PrcGetAverageCensusData(Nullable<int> year, Nullable<int> month, Nullable<int> userId, string nursingStation);
        ObjectResult<string> PrcGetEmarResidentdetails(Nullable<int> month, Nullable<int> year, string nursingstationId, string patientId, Nullable<int> userId);
        ObjectResult<PrcGetMARHistorydetails_Result> PrcGetMARHistorydetails(Nullable<int> month, Nullable<int> year, string nursingStationId, Nullable<int> userId, string patientID);
        ObjectResult<PrcGetMARHoledetails_Result> PrcGetMARHoledetails(Nullable<int> month, Nullable<int> year, string nursingStationId, Nullable<int> userId, string patientID);
        ObjectResult<PrcReportsGetInboundDetails_Result> PrcReportsGetInboundDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, string residentName, Nullable<int> status);
        ObjectResult<PrcReportsGetNurseNotesDetails_Result> PrcReportsGetNurseNotesDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId, string commentType, string medicationReason);
       // ObjectResult<PrcReportsGetNurseNotesDetails_Result> PrcReportsGetNurseNotesDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId, string commentType);
        ObjectResult<PrcReportsGetOutBoundErrorDetails_Result> PrcReportsGetOutBoundErrorDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate);
        ObjectResult<PrcReportsGetPrescriberNotesDetails_Result> PrcReportsGetPrescriberNotesDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsGetPsychiatricDetails_Result> PrcReportsGetPsychiatricDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcGetAllergyChangesDetails_Result> PrcGetAllergyChangesDetails(Nullable<int> pAllergyId);

        ObjectResult<PrcGetDiagnosisChangesDetails_Result> PrcGetDiagnosisChangesDetails(Nullable<int> pDiagnosisId);
        ObjectResult<Nullable<int>> PrcInsertOrderData(Nullable<long> approvalOrderId);
        ObjectResult<string> PrcGetAllergyInfoByPatient(string patientName);
        ObjectResult<PrcGetReportDrFirst_Result> PrcGetReportDrFirst(string patientId);
        ObjectResult<PrcGetUserActivityDetails_Result> PrcGetUserActivityDetails(string userId, Nullable<System.DateTime> fromdate, Nullable<System.DateTime> todate);
        ObjectResult<PrcEmarResidentReport_Result> PrcEmarResidentReport(Nullable<int> nursingStationId, Nullable<System.DateTime> date, string passTime, Nullable<int> shiftId, Nullable<int> window);
        ObjectResult<PrcReportsGetEmarDetails_Result> PrcReportsGetEmarDetails(Nullable<int> patientId, Nullable<System.DateTime> date, string passTime, Nullable<int> shiftId, Nullable<int> window, Nullable<int> userId);
        ObjectResult<PrcGetAllergyMasterChangesDetails_Result> PrcGetAllergyMasterChangesDetails(Nullable<int> allergyId);
        ObjectResult<PrcGetCompanyBedConfigChangesdetails_Result> PrcGetCompanyBedConfigChangesdetails(Nullable<int> companyId);
        ObjectResult<PrcGetOrderInfoChanges_Result> PrcGetOrderInfoChanges(Nullable<int> approvalID);
        ObjectResult<PrcGetResientOrderData_Result> PrcGetResientOrderData(Nullable<int> patientId);
        ObjectResult<PrcEmargriddetails_Result> PrcEmargriddetails(Nullable<int> nursingStationId, Nullable<System.DateTime> date, string passTime, Nullable<int> shiftId, Nullable<int> window);
        ObjectResult<PrcApprovalRefillDetails_Result> PrcApprovalRefillDetails(Nullable<long> refillId, Nullable<int> porderId);
        ObjectResult<PrcGetPatientTypeChangesdetails_Result> PrcGetPatientTypeChangesdetails(Nullable<int> patientType_Id);
        ObjectResult<Nullable<int>> PrcDueMARAlert(Nullable<int> userId);
        int PrcOrderScheduleTime(Nullable<int> pOrderId, Nullable<int> dAdmin_Id);
        ObjectResult<PrcGetUserRoleConfigData_Result> PrcGetUserRoleConfigData(Nullable<int> userId);
        ObjectResult<AlertTypeDataCount_Result> AlertTypeDataCount(Nullable<int> userId);
        ObjectResult<PrcGetOrderGridData_Result> PrcGetOrderGridData(Nullable<int> patientId);
       // ObjectResult<PrcGetOrdersdata_Result> PrcGetOrdersdata(Nullable<int> porderId, Nullable<int> pquantityId);
        //ObjectResult<Nullable<int>> PrcOrderUpdate(Nullable<int> porderId, Nullable<int> pquantityId, Nullable<int> physicianId, string drugName, string quantity, string directions, Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate, string numberofRefills, Nullable<int> maxPerdays, string alerttext, string insulinComments, Nullable<bool> orderstockFlag, Nullable<bool> pRNFlag, Nullable<bool> treatmentFlag, Nullable<bool> selfAdministeredFlag, Nullable<int> route, string inhand, Nullable<int> createdby, string barcode, Nullable<int> controlledSubstanceBit, Nullable<int> orderTypeID, string requestedGiveCode);
        ObjectResult<Nullable<int>> PrcOrderUpdate(Nullable<int> porderId, Nullable<int> pquantityId, Nullable<int> physicianId, string drugName, string quantity, string directions, Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate, string numberofRefills, Nullable<decimal> maxPerdays, string alerttext, string insulinComments, Nullable<bool> orderstockFlag, Nullable<bool> pRNFlag, Nullable<bool> treatmentFlag, Nullable<bool> selfAdministeredFlag, Nullable<int> route, string inhand, Nullable<int> createdby, string barcode, Nullable<int> controlledSubstanceBit, Nullable<int> orderTypeID, string requestedGiveCode);
        ObjectResult<PrcGetOrderGrid_Result> PrcGetOrderGrid(Nullable<int> userId, string facilityId, string nursingstationId, string floor, string wing, string room, string bed, Nullable<int> visitStatus);
        ObjectResult<string> PrcInsertupdateHOA(Nullable<int> dadminId, Nullable<int> porderId, Nullable<int> pquantityId, Nullable<int> freqId, string nurseShiftId, string hourId, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string weekId, string monthId, string days, Nullable<int> createdby, Nullable<int> activedays, Nullable<int> holddays);
        int PrcGetEmarpreviewdetails(Nullable<int> month, Nullable<int> year, Nullable<int> patientId);
        ObjectResult<PrcReportsGetEKitDetails_Result> PrcReportsGetEKitDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId, Nullable<int> status);
        ObjectResult<PrcReportsGetStockData_Result> PrcReportsGetStockData(Nullable<int> userId, Nullable<int> companyId);
        ObjectResult<PrcgetHOAdata_Result> PrcgetHOAdata(Nullable<int> porderId, Nullable<int> pquantityId);
        ObjectResult<PrcReportsAdminUsers_Result> PrcReportsAdminUsers(string companyId, Nullable<int> userId);
        ObjectResult<PrcReportsSetUpConfigData_Result> PrcReportsSetUpConfigData(string companyId);
        int PrcInsertUpdateCompanyConfig(Nullable<int> companyConfigId, Nullable<int> companyId, Nullable<int> approvalflag, Nullable<int> fingerDescId, Nullable<int> timeFormat, Nullable<int> stockReportId, Nullable<int> drFirstRequired, Nullable<int> hLConfigured, Nullable<int> hLDirectionalWayId, string fteCategory, string @event, Nullable<int> configStatus, Nullable<int> createdBy);
        ObjectResult<PrcReportsGetRefillDetails_Result> PrcReportsGetRefillDetails(Nullable<int> userid, string nursingStationId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> todate);
        ObjectResult<PrcGetOrderEndDate_Result> PrcGetOrderEndDate(string nursingStationId);
        ObjectResult<PrcGetICD10ChangesDetails_Result> PrcGetICD10ChangesDetails(Nullable<int> nursingStationId);
        ObjectResult<PrcGetCompanyConfigChanges_Result> PrcGetCompanyConfigChanges(Nullable<int> companyId);
        ObjectResult<PrcGetPhysicianChangesdetails_Result> PrcGetPhysicianChangesdetails(Nullable<int> physicianId);
        ObjectResult<PrcGetCompanyStockReport_Result> PrcGetCompanyStockReport(Nullable<int> userId);
        ObjectResult<string> PrcGenerateHL7forNewPatient(Nullable<int> patientId, string messageID);
        int PrcInsertUpdateFreqMapping(Nullable<int> frequencyId, Nullable<int> facilityId, string nursingStationId, string hourId, Nullable<int> hours, Nullable<int> monday, Nullable<int> tuesday, Nullable<int> wednesday, Nullable<int> thursday, Nullable<int> friday, Nullable<int> saturday, Nullable<int> sunday, Nullable<int> weekId, Nullable<int> monthId, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> nursingFreq_CreatedBy);
        ObjectResult<PrcGetFreqMappingData_Result> PrcGetFreqMappingData(Nullable<int> userId);
        int PrcInsertUpdateUser(Nullable<int> userId, Nullable<int> suffix, string lname, string fname, string mname, string userName, string password, string displayName, string email, string userPhone, Nullable<int> stkReportReq, Nullable<int> userStatus, Nullable<int> userCreatedBy, Nullable<int> newUserFlag, Nullable<int> roleId, string facilityId, string nurseStationId, Nullable<int> processkey, string physicianNPI, Nullable<int> pastDueAlertFlag);
        ObjectResult<PrcGetRoleData_Result> PrcGetRoleData(Nullable<int> userId);
        ObjectResult<PrcGetRoleconfigData_Result> PrcGetRoleconfigData(Nullable<int> userId, Nullable<int> role_Id);
        ObjectResult<PrcGetPhysicianData_Result> PrcGetPhysicianData(Nullable<int> userId);
        ObjectResult<string> TempGetOrderHOAMessage(Nullable<int> administrationType, Nullable<int> nursingFreq_Id, string nurseshifts_Id, string hour_Id, Nullable<int> hours, Nullable<bool> monday, Nullable<bool> tuesday, Nullable<bool> wednesday, Nullable<bool> thursday, Nullable<bool> friday, Nullable<bool> saturday, Nullable<bool> sunday, string week_Id, string month_Id, string days, Nullable<int> activeDays, Nullable<int> holdDays, Nullable<int> company_id, Nullable<int> nurseStation_Id);
        int PrcreportsGetOrderchange(string patientId, Nullable<System.DateTime> fromdate, Nullable<System.DateTime> todate);
        ObjectResult<PrcReportsGetEKitDispensingDetails_Result> PrcReportsGetEKitDispensingDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcReportsNewGetFloorStockDetails_Result> PrcReportsNewGetFloorStockDetails(Nullable<int> userid, string nursingStationId, Nullable<int> facilityId);
        ObjectResult<PrcGetDocumentCheckData_Result> PrcGetDocumentCheckData(Nullable<int> patientId, Nullable<System.DateTime> date, string passTime, string shiftId);
        ObjectResult<Nullable<int>> PrcgetOrderEndingSoonVisitStatus(Nullable<int> userId, Nullable<int> screenId, Nullable<System.DateTime> datetime);
        ObjectResult<PrcReportsGetDocAdminOrderDetails_Result> PrcReportsGetDocAdminOrderDetails(Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> orderType, Nullable<int> userid, string nursingStationId);
        ObjectResult<PrcGetRemoveDocumentOrderData_Result> PrcGetRemoveDocumentOrderData(Nullable<int> patientId, Nullable<System.DateTime> fromdate, Nullable<System.DateTime> toDate);
        ObjectResult<prcgetDosesDetails_Result> prcgetDosesDetails(Nullable<int> userId, string nurseStation_Id, Nullable<int> facility_Id);
        ObjectResult<PrcGetDocAdminOrderAudit_Result> PrcGetDocAdminOrderAudit(Nullable<long> drugAdminister_Id);
        ObjectResult<PrcGetOutBoundErrorAck_Result> PrcGetOutBoundErrorAck(Nullable<int> userId);
        ObjectResult<Nullable<int>> PrcInsertStockEkitClone(Nullable<int> input, string clone, Nullable<int> sharedstockbit, Nullable<int> stockekit, Nullable<int> createdBy);
        int prcInsertControlSubstanceTrans();
        ObjectResult<PrcGetCertifyOrderResidentGridData_Result> PrcGetCertifyOrderResidentGridData(string nurseStations, Nullable<int> userId, string physicianNPI);
        ObjectResult<PrcGetCertifyOrderGridData_Result> PrcGetCertifyOrderGridData(Nullable<int> patientId, Nullable<int> userId, string physicianNPI);
        int PrcInsertOrderCertification(Nullable<System.DateTime> certifyDate, Nullable<int> user_Id, string porder_Id, Nullable<int> months, string physicianCredentials);
        ObjectResult<PrcReportGetCertifyedOrders_Result> PrcReportGetCertifyedOrders(Nullable<int> userId, Nullable<int> certifiedTimeId, Nullable<int> patientId);
        ObjectResult<PrcReportsGetEkitExpirationDetails_Result> PrcReportsGetEkitExpirationDetails(Nullable<System.DateTime> checkinFromDate, Nullable<System.DateTime> checkinToDate, Nullable<System.DateTime> expireFromDate, Nullable<System.DateTime> expireToDate, string nursingStationId, Nullable<int> facilityId, Nullable<int> userId);
        ObjectResult<PrcReportsGetPharmacyExpirationDetails_Result> PrcReportsGetPharmacyExpirationDetails(Nullable<System.DateTime> checkinFromDate, Nullable<System.DateTime> checkinToDate, Nullable<System.DateTime> expireFromDate, Nullable<System.DateTime> expireToDate, string nursingStationId, Nullable<int> userId);
        ObjectResult<PrcReportsGetScheduleOrderDetails_Result> PrcReportsGetScheduleOrderDetails(string resident, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> orderType, Nullable<int> userid, string nursingStationId);
        ObjectResult<string> PrcGetTimeZoneDate(Nullable<int> nursingStation_Id);
        ObjectResult<Nullable<System.DateTime>> GetTimeZoneConvertedDateTime(Nullable<System.DateTime> datetime, Nullable<int> facilityId);
        ObjectResult<Nullable<System.DateTime>> GetFileTimeZoneConvertedDateTime(Nullable<System.DateTime> datetime, Nullable<int> facilityId, Nullable<int> type);
        ObjectResult<string> PrcGenerateHL7forPatientNewCPOEOrder(Nullable<int> patientId, Nullable<int> porderID, string uniqueid);
        ObjectResult<PrcGetProfieOrderResidentGridData_Result> PrcGetProfieOrderResidentGridData(string nurseStations);
        ObjectResult<PrcGetProfileOrderGridData_Result> PrcGetProfileOrderGridData(Nullable<int> patientId);
        int PrcInsertProfileOrderCertification(Nullable<System.DateTime> certifyDate, Nullable<int> user_Id, string porder_Id, Nullable<int> months, string physicianCredentials);
        ObjectResult<PrcReportGetPrifileCertifyedOrders_Result> PrcReportGetPrifileCertifyedOrders(Nullable<int> userId, Nullable<int> certifiedTimeId, Nullable<int> patientId);
        ObjectResult<string> PrcGetMessageId();
        int InsertOrderChangesforReport(Nullable<long> pOrder_Id);
        ObjectResult<PrcGetPhysicianDetailsatorder_Result> PrcGetPhysicianDetailsatorder(Nullable<int> nurseStation_Id, Nullable<int> facility_Id, Nullable<long> pOrder_Id, Nullable<int> userID);
        int PrcInsertRoleConfig(Nullable<int> roleId, string screen, Nullable<int> userid);
        int PrcUpdateRoleConfig(Nullable<int> roleId, string screen, Nullable<int> userid, Nullable<int> read, Nullable<int> write, Nullable<int> pdf, Nullable<int> excel, Nullable<int> active);
        int PrcNoAdministrationUpdate(Nullable<long> drugAdminister_Id, Nullable<int> porder_Id, Nullable<int> pQuantity_Id, string additionalcomments, Nullable<int> adminsterStatus, Nullable<int> adminsterBy, Nullable<System.DateTime> adminsterOn, Nullable<int> medicationReason_ID, string administerComment, string reason, Nullable<int> manualDocumentedBy, Nullable<System.DateTime> manualDocumentedDate, Nullable<int> enteredBy, Nullable<System.DateTime> enteredDate, string time);
    }
}