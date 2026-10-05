using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;

namespace LTCPro.Repositories
{
    public class AutoMapping : IAutoMapper
    {

        private static IMapper _mapper;
        private static MapperConfiguration _config;

        public IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    AutoMapperRegistration();
                }
                return _mapper;
            }
        }

        public AutoMapping() { AutoMapperRegistration(); }


        private void AutoMapperRegistration()
        {
            if (_config != null)
                return;
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Gender, GenderEntity>();
                cfg.CreateMap<GenderEntity, Gender>();
                cfg.CreateMap<Suffix, SuffixEntity>();
                cfg.CreateMap<SuffixEntity, Suffix>();
                cfg.CreateMap<Country, CountryEntity>();
                cfg.CreateMap<CountryEntity, Country>();
                cfg.CreateMap<CompanyEntity, Company>();
                cfg.CreateMap<Company, CompanyEntity>();
                cfg.CreateMap<Facility, FacilityEntity>();
                cfg.CreateMap<FacilityEntity, Facility>();
                cfg.CreateMap<Floor, FloorEntity>();
                cfg.CreateMap<FloorEntity, Floor>();
                cfg.CreateMap<NursingStation, NursingStationEntity>();
                cfg.CreateMap<NursingStationEntity, NursingStation>();
                cfg.CreateMap<Role, RoleEntity>();
                cfg.CreateMap<RoleEntity, Role>();
                cfg.CreateMap<Wing, WingEntity>();
                cfg.CreateMap<WingEntity, Wing>();
                cfg.CreateMap<Room, RoomEntity>();
                cfg.CreateMap<RoomEntity, Room>();
                cfg.CreateMap<Bed, BedEntity>();
                cfg.CreateMap<BedEntity, Bed>();
                cfg.CreateMap<RoleConfig, RoleConfigEntity>();
                cfg.CreateMap<RoleConfigEntity, RoleConfig>();
                cfg.CreateMap<MaritalStatu, MaritalStatusEntity>();
                cfg.CreateMap<MaritalStatusEntity, MaritalStatu>();
                cfg.CreateMap<User, UserEntity>();
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<UserRoleFacilityConfig, UserRoleFacilityConfigEntity>();
                cfg.CreateMap<UserRoleFacilityConfigEntity, UserRoleFacilityConfig>();
                cfg.CreateMap<HLSevenSegment, HLSevenSegmentEntity>();
                cfg.CreateMap<HLSevenSegmentEntity, HLSevenSegment>();
                cfg.CreateMap<HLSevenSegmentDetail, HLSevenSegmentDetailEntity>();
                cfg.CreateMap<HLSevenSegmentDetailEntity, HLSevenSegmentDetail>();
                cfg.CreateMap<FTEConfiguration, FTEConfigurationEntity>();
                cfg.CreateMap<FTEConfigurationEntity, FTEConfiguration>();
                cfg.CreateMap<FTECategory, FTECategoryEntity>();
                cfg.CreateMap<FTECategoryEntity, FTECategory>();
                cfg.CreateMap<FteConnection, FteConnectionEntity>();
                cfg.CreateMap<FteConnectionEntity, FteConnection>();
                cfg.CreateMap<CompanyBedConfig, CompanyBedConfigEntity>();
                cfg.CreateMap<CompanyBedConfigEntity, CompanyBedConfig>();
                cfg.CreateMap<ICD10, ICD10Entity>();
                cfg.CreateMap<ICD10Entity, ICD10>();
                cfg.CreateMap<HLSevenCompanyConfig, HLSevenCompanyConfigEntity>();
                cfg.CreateMap<HLSevenCompanyConfigEntity, HLSevenCompanyConfig>();
                cfg.CreateMap<Demographic, DemographicEntity>();
                cfg.CreateMap<DemographicEntity, Demographic>();
                cfg.CreateMap<AllergyInfo, AllergyInfoEntity>();
                cfg.CreateMap<AllergyInfoEntity, AllergyInfo>();
                cfg.CreateMap<DiagnosisInfo, DiagnosisInfoEntity>();
                cfg.CreateMap<DiagnosisInfoEntity, DiagnosisInfo>();
                cfg.CreateMap<DocFolder, DocFolderEntity>();
                cfg.CreateMap<DocFolderEntity, DocFolder>();
                cfg.CreateMap<UploadedDocument, UploadedDocumentEntity>();
                cfg.CreateMap<UploadedDocumentEntity, UploadedDocument>();
                cfg.CreateMap<ApiIntegration, ApiIntegrationEntity>();
                cfg.CreateMap<ApiIntegrationEntity, ApiIntegration>();
                cfg.CreateMap<EncodedOrderDetail, EncodedOrderDetailEntity>();
                cfg.CreateMap<EncodedOrderDetailEntity, EncodedOrderDetail>();
                cfg.CreateMap<OutBoundFileInformation, OutBoundFileInformationEntity>();
                cfg.CreateMap<OutBoundFileInformationEntity, OutBoundFileInformation>();
                cfg.CreateMap<CompanyBedConfig, CompanyBedConfigEntity>();
                cfg.CreateMap<CompanyBedConfigEntity, CompanyBedConfig>();
                cfg.CreateMap<TreatmentInfo, TreatmentInfoEntity>();
                cfg.CreateMap<TreatmentInfoEntity, TreatmentInfo>();
                cfg.CreateMap<VisitInfo, VisitInfoEntity>();
                cfg.CreateMap<VisitInfoEntity, VisitInfo>();
                cfg.CreateMap<CommonOrderInfoEntity, CommonOrderInfo>();
                cfg.CreateMap<CommonOrderInfo, CommonOrderInfoEntity>();
                cfg.CreateMap<DrugAdministrationTime, DrugAdministrationTimeEntity>();
                cfg.CreateMap<DrugAdministrationTimeEntity, DrugAdministrationTime>();
                cfg.CreateMap<FrequencyMaster, FrequencyMasterEntity>();
                cfg.CreateMap<FrequencyMasterEntity, FrequencyMaster>();
                cfg.CreateMap<Week, WeekEntity>();
                cfg.CreateMap<WeekEntity, Week>();
                cfg.CreateMap<Month, MonthEntity>();
                cfg.CreateMap<MonthEntity, Month>();
                cfg.CreateMap<Hour, HourEntity>();
                cfg.CreateMap<HourEntity, Hour>();
                cfg.CreateMap<TimeFormat, TimeFormatEntity>();
                cfg.CreateMap<TimeFormatEntity, TimeFormat>();
                cfg.CreateMap<ResidentOrder, ResidentOrderEntity>();
                cfg.CreateMap<ResidentOrderEntity, ResidentOrder>();
                cfg.CreateMap<BarcodeDetail, BarcodeDetailEntity>();
                cfg.CreateMap<BarcodeDetailEntity, BarcodeDetail>();
                cfg.CreateMap<OrderFavouriteMaster, OrderFavouriteMasterEntity>();
                cfg.CreateMap<OrderFavouriteMasterEntity, OrderFavouriteMaster>();
                cfg.CreateMap<OrderFavourite, OrderFavouriteEntity>();
                cfg.CreateMap<OrderFavouriteEntity, OrderFavourite>();
                cfg.CreateMap<OrderHold, OrderHoldEntity>();
                cfg.CreateMap<OrderHoldEntity, OrderHold>();
                cfg.CreateMap<NursingSchedule, NursingScheduleEntity>();
                cfg.CreateMap<NursingScheduleEntity, NursingSchedule>();
                cfg.CreateMap<Screen, ScreenEntity>();
                cfg.CreateMap<ScreenEntity, Screen>();
                cfg.CreateMap<PhysicianDetail, PhysicianDetailsEntity>();
                cfg.CreateMap<PhysicianDetailsEntity, PhysicianDetail>();
                cfg.CreateMap<DrugAdminister, DrugAdministerEntity>();
                cfg.CreateMap<DrugAdministerEntity, DrugAdminister>();
                cfg.CreateMap<ImportFileEntity, ImportFile>();
                cfg.CreateMap<ImportFile, ImportFileEntity>();
                cfg.CreateMap<NursingFrequencyConfig, NursingFrequencyConfigEntity>();
                cfg.CreateMap<NursingFrequencyConfigEntity, NursingFrequencyConfig>();
                cfg.CreateMap<VisitBehaviour, VisitBehaviourEntity>();
                cfg.CreateMap<VisitBehaviourEntity, VisitBehaviour>();
                cfg.CreateMap<VisitFoodintake, VisitFoodintakeEntity>();
                cfg.CreateMap<VisitFoodintakeEntity, VisitFoodintake>();
                cfg.CreateMap<VisitNursingNote, VisitNursingNoteEntity>();
                cfg.CreateMap<VisitNursingNoteEntity, VisitNursingNote>();
                cfg.CreateMap<VisitVital, VisitVitalEntity>();
                cfg.CreateMap<VisitVitalEntity, VisitVital>();
                cfg.CreateMap<WeightLog, WeightLogEntity>();
                cfg.CreateMap<WeightLogEntity, WeightLog>();
                cfg.CreateMap<BehavioralSymptomsMaster, BehavioralSymptomsMasterEntity>();
                cfg.CreateMap<BehavioralSymptomsMasterEntity, BehavioralSymptomsMaster>();
                cfg.CreateMap<MailConfig, MailConfigEntity>();
                cfg.CreateMap<MailConfigEntity, MailConfig>();
                cfg.CreateMap<OrderDestroy, OrderDestroyEntity>();
                cfg.CreateMap<OrderDestroyEntity, OrderDestroy>();
                cfg.CreateMap<ApprovalAllergyInfo, ApprovalAllergyInfoEntity>();
                cfg.CreateMap<ApprovalAllergyInfoEntity, ApprovalAllergyInfo>();
                cfg.CreateMap<ApprovalDiagnosisInfo, ApprovalDiagnosisInfoEntity>();
                cfg.CreateMap<ApprovalDiagnosisInfoEntity, ApprovalDiagnosisInfo>();
                cfg.CreateMap<ApprovalDemographic, ApprovalDemographicEntity>();
                cfg.CreateMap<ApprovalDemographicEntity, ApprovalDemographic>();
                cfg.CreateMap<ApprovalVisitInfo, ApprovalVisitInfoEntity>();
                cfg.CreateMap<ApprovalVisitInfoEntity, ApprovalVisitInfo>();
                cfg.CreateMap<Message, MessageEntity>();
                cfg.CreateMap<MessageEntity, Message>();
                cfg.CreateMap<DrFirstOrderCheck_Result, DrFirstOrderCheck_ResultEntity>();
                cfg.CreateMap<DrFirstOrderCheck_ResultEntity, DrFirstOrderCheck_Result>();
                cfg.CreateMap<PrcDrFirstOrderHL7_Result, PrcDrFirstOrderHL7_ResultEntity>();
                cfg.CreateMap<PrcDrFirstOrderHL7_ResultEntity, PrcDrFirstOrderHL7_Result>();
                cfg.CreateMap<AllergyInfoMaster, AllergyInfoMastersEntity>();
                cfg.CreateMap<AllergyInfoMastersEntity, AllergyInfoMaster>();
                cfg.CreateMap<ApprovalOrderEntity, ApprovalOrder>();
                cfg.CreateMap<ApprovalOrder, ApprovalOrderEntity>();

                cfg.CreateMap<NurseCommentType, NurseCommentTypeDropEntity>();
                cfg.CreateMap<NurseCommentTypeDropEntity, NurseCommentType>();

                cfg.CreateMap<MedicationReason, MedicationReasonEntity>();
                cfg.CreateMap<MedicationReasonEntity, MedicationReason>();


                cfg.CreateMap<UserActivityDetail, UserActivityDetailEntity>();
                cfg.CreateMap<UserActivityDetailEntity, UserActivityDetail>();
                cfg.CreateMap<VisitInfo, VisitUpdateEntity>();
                cfg.CreateMap<VisitUpdateEntity, VisitInfo>();
                cfg.CreateMap<MailBox, MailComposeEntity>();
                cfg.CreateMap<MailComposeEntity, MailBox>();
                cfg.CreateMap<StockCustomEntity, Stock>();
                cfg.CreateMap<Stock, StockCustomEntity>();
                cfg.CreateMap<EkitCustomEntity, Ekit>();
                cfg.CreateMap<Ekit, EkitCustomEntity>();
                cfg.CreateMap<MailFavouritesEntity, MailFavourite>();
                cfg.CreateMap<MailFavourite, MailFavouritesEntity>();
                cfg.CreateMap<PrcGetUserRoleConfigData_Result, UserRoleFacilityConfigGridEntity>();
                cfg.CreateMap<UserRoleFacilityConfigGridEntity, PrcGetUserRoleConfigData_Result>();
                cfg.CreateMap<AlertStatusEntity, AlertStatu>();
                cfg.CreateMap<AlertStatu, AlertStatusEntity>();
                cfg.CreateMap<OrdersCommonStatusEntity, CommonOrderInfo>();
                cfg.CreateMap<CommonOrderInfo, OrdersCommonStatusEntity>();
                cfg.CreateMap<OrderStockEntity, OrderStock>();
                cfg.CreateMap<OrderStock, OrderStockEntity>();
                cfg.CreateMap<DrFirstOrderXMLTransEntity, DrFirstOrderXMLTran>();
                cfg.CreateMap<DrFirstOrderXMLTran, DrFirstOrderXMLTransEntity>();
                cfg.CreateMap<NurseShiftEntity, NurseShift>();
                cfg.CreateMap<NurseShift, NurseShiftEntity>();
                cfg.CreateMap<HLDirectionEntity, HLDirectionalWay>();
                cfg.CreateMap<HLDirectionalWay, HLDirectionEntity>();
                cfg.CreateMap<CompanyConfigEntity, CompanyConfig>();
                cfg.CreateMap<CompanyConfig, CompanyConfigEntity>();
                cfg.CreateMap<PrcgetHOAdata_Result, DrugAdministrationTimeEntity>();
                cfg.CreateMap<DrugAdministrationTimeEntity, PrcgetHOAdata_Result>();
                cfg.CreateMap<ApprovalRefillEntity, ApprovalRefill>();
                cfg.CreateMap<ApprovalRefill, ApprovalRefillEntity>();
                cfg.CreateMap<OnLeaveEntity, OnLeave>();
                cfg.CreateMap<OnLeave, OnLeaveEntity>();
                cfg.CreateMap<PrcGetOrdersdata_Result, OrdersDataEntity>();
                cfg.CreateMap<OrdersDataEntity, PrcGetOrdersdata_Result>();
                cfg.CreateMap<PatientTypeCustomEntity, PatientType>();
                cfg.CreateMap<PatientType, PatientTypeCustomEntity>();
                cfg.CreateMap<PatientTypeEntity, PatientType>();
                cfg.CreateMap<PatientType, PatientTypeEntity>();
                cfg.CreateMap<PrcGetOrderEndDate_Result, OrdersEndDateEntity>();
                cfg.CreateMap<OrdersEndDateEntity, PrcGetOrderEndDate_Result>();
                cfg.CreateMap<QuantityDetailEntity, QuantityDetail>();
                cfg.CreateMap<QuantityDetail, QuantityDetailEntity>();
                cfg.CreateMap<DefaultScreenEntity, DefaultScreen>();
                cfg.CreateMap<DefaultScreen, DefaultScreenEntity>();

                cfg.CreateMap<DemographicCustomEntity, ApprovalDemographic>();
                cfg.CreateMap<ApprovalDemographic, DemographicCustomEntity>();
                cfg.CreateMap<VisitInfoCustomEntity, ApprovalVisitInfo>();
                cfg.CreateMap<ApprovalVisitInfo, VisitInfoCustomEntity>();
                cfg.CreateMap<DemographicCustomEntity, Demographic>();
                cfg.CreateMap<Demographic, DemographicCustomEntity>();
                cfg.CreateMap<VisitInfoCustomEntity, VisitInfo>();
                cfg.CreateMap<VisitInfo, VisitInfoCustomEntity>();
                cfg.CreateMap<ColourType, ColourTypeEntity>();
                cfg.CreateMap<ColourTypeEntity, ColourType>();
                cfg.CreateMap<RecentFac, RecentFacEntity>();
                cfg.CreateMap<RecentFacEntity, RecentFac>();
                cfg.CreateMap<Role, RoleDropdownEntity>();
                cfg.CreateMap<RoleDropdownEntity, Role>();
                cfg.CreateMap<RoleEntity, PrcGetRoleData_Result>();
                cfg.CreateMap<PrcGetRoleData_Result, RoleEntity>();

                cfg.CreateMap<RoleConfigGridEntity, PrcGetRoleconfigData_Result>();
                cfg.CreateMap<PrcGetRoleconfigData_Result, RoleConfigGridEntity>();

                cfg.CreateMap<PhysicianGridEntity, PrcGetPhysicianData_Result>();
                cfg.CreateMap<PrcGetPhysicianData_Result, PhysicianGridEntity>();

                cfg.CreateMap<HLSevenOutboundDisplaySegment, HLSevenOutboundDisplaySegmentEntity>();
                cfg.CreateMap<HLSevenOutboundDisplaySegmentEntity, HLSevenOutboundDisplaySegment>();

                cfg.CreateMap<HLSevenOutboundDisplayCompanyConfig, CustomHLSevenOutboundDisplayCompanyConfigEntity>();
                cfg.CreateMap<CustomHLSevenOutboundDisplayCompanyConfigEntity, HLSevenOutboundDisplayCompanyConfig>();

                cfg.CreateMap<PrcGetDocumentCheckData_Result, GetDocumentCheckDataEntity>();
                cfg.CreateMap<GetDocumentCheckDataEntity, PrcGetDocumentCheckData_Result>();

                cfg.CreateMap<NurseCommentsEntity, NurseComment>();
                cfg.CreateMap<NurseComment, NurseCommentsEntity>();

                cfg.CreateMap<DosesDetailsEntity, prcgetDosesDetails_Result>();
                cfg.CreateMap<prcgetDosesDetails_Result, DosesDetailsEntity>();

                cfg.CreateMap<OutboundErrorDetailsEntity, PrcGetOutBoundErrorAck_Result>();
                cfg.CreateMap<PrcGetOutBoundErrorAck_Result, OutboundErrorDetailsEntity>();

                cfg.CreateMap<PrcGetRemoveDocumentOrderData_Result, GetDocumentCheckDataEntity>();
                cfg.CreateMap<GetDocumentCheckDataEntity, PrcGetRemoveDocumentOrderData_Result>();
                cfg.CreateMap<NurseStationHierarchyEntity, NurseStationHierarchy>();
                cfg.CreateMap<NurseStationHierarchy, NurseStationHierarchyEntity>();
                cfg.CreateMap<ApprovalCPOEOrderEntity, ApprovalOrder>();
                cfg.CreateMap<ApprovalOrder, ApprovalCPOEOrderEntity>();
            });
            _mapper = _config.CreateMapper();
        }

        /// <summary>
        /// Auto Mapping Attributes
        /// </summary>
        /// <typeparam name="T1">From</typeparam>
        /// <typeparam name="T2">To</typeparam>
        /// <param name="input">Source</param>
        /// <returns>T2</returns>
        public T2 Map<T1, T2>(T1 input)
        {
            AutoMapperRegistration();
            return Mapper.Map<T1, T2>(input);
        }

        /// <summary>
        /// Auto Mapping Attributes
        /// </summary>
        /// <typeparam name="T1">From</typeparam>
        /// <typeparam name="T2">To</typeparam>
        /// <param name="input">Collection</param>
        /// <returns>IEnumerable<T2></returns>
        public IEnumerable<T2> Map<T1, T2>(IEnumerable<T1> input)
        {
            AutoMapperRegistration();
            return Mapper.Map<IEnumerable<T1>, IEnumerable<T2>>(input);
        }
    }
}
