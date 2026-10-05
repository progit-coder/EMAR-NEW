using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using LTCPro.DAL;
using LTCPro.Repositories;
using System;
using LTCPro.ServiceLayer;

namespace WebApi
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion
        public static void RegisterTypes(IUnityContainer container)
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAutoMapper, AutoMapping>();
            container.RegisterType<IFacilityRepository, FacilityRepository>();
            container.RegisterType<IFacilityService, FacilityService>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<ICompanyService, CompanyService>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<ICommonRepository, CommonRepository>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IHLSevenSegmentRepository,HLSevenSegmentRepository>();
            container.RegisterType<IHLSevenSegmentService, HLSevenSegmentService>();
            container.RegisterType<IFTEConfigurationRepository, FTEConfigurationRepository>();
            container.RegisterType<IFTEConfigurationService, FTEConfigurationService>();
            container.RegisterType<IFileInformationRepository, FileInformationRepository>();
            container.RegisterType<IInboundFilesService, InboundFilesService>();
            container.RegisterType<IAllergiesICDService, AllergiesICDService>();
            container.RegisterType<IAllergiesICDRepository, AllergiesICDRepository>();
            container.RegisterType<IResidentDemographicService, ResidentDemographicService>();
            container.RegisterType<IResidentDemographicRepository, ResidentDemographicRepository>();
            container.RegisterType<IDrFirstIntegrationRepository, DrFirstIntegrationRepository>();
            container.RegisterType<IDrFirstIntegrationService, DrFirstIntegrationService>();
            container.RegisterType<IOrdersRepository, OrdersRepository>();
            container.RegisterType<IOrdersService, OrdersService>();
            container.RegisterType<IOutboundFilesService, OutboundFilesService>();
            container.RegisterType<IAllergyInfoRepository, AllergyInfoRepository>();
            container.RegisterType<IDiagnosisInfoRepository, DiagnosisInfoRepository>();
            container.RegisterType<IDiagnosisInfoService, DiagnosisInfoService>();
            container.RegisterType<IEmarRepository, EmarRepository>();
            container.RegisterType<IEmarService, EmarService>();
            container.RegisterType<IAllergyInfoService, AllergyInfoService>();
            container.RegisterType<IOutboundFileInformationRepository, OutboundFileInformationRepository>();
            container.RegisterType<IAssessmentsService, AssessmentsService>();
            container.RegisterType<IAssessmentsRepository, AssessmentsRepository>();
            container.RegisterType<IAuditRepository, AuditRepository>();
            container.RegisterType<IAuditService, AuditService>();
            container.RegisterType<IAdminApprovalService, AdminApprovalService>();
            container.RegisterType<IAdminApprovalRepository, AdminApprovalRepository>();
            container.RegisterType<IDocumentManagerRepository, DocumentManagerRepository>();
            container.RegisterType<IDocumentManagerService, DocumentManagerService>();
            container.RegisterType<IChatRepository, ChatRepository>();
            container.RegisterType<IChatService, ChatService>();
            container.RegisterType<IDashboardsRepository, DashboardsRepository>();
            container.RegisterType<IDashboardsService, DashboardsService>();
            container.RegisterType<IReportsRepository, ReportsRepository>();
            container.RegisterType<IReportsService, ReportsService>();
            container.RegisterType<IMailboxRepository, MailboxRepository>();
            container.RegisterType<IMailboxService, MailboxService>();
            container.RegisterType<IUserActivityRepository, UserActivityRepository>();
            container.RegisterType<IUserActivityService, UserActivityService>();
            container.RegisterType<IStockEkitRepository, StockEkitRepository>();
            container.RegisterType<IStockEkitService, StockEkitService>();


            container.RegisterType<ICheckInPharmacyMedsRepository, CheckInPharmacyMedsRepository>();
            container.RegisterType<ICheckInPharmacyMedsService, CheckInPharmacyMedsService>();


            container.RegisterType<IRcopiaResository, RcopiaResository>();
            container.RegisterType<IRcopiaService, RcopiaService>();

            container.RegisterType<ILogger, Log4net>();
            container.RegisterType<IAngularLog, AngularLogs>();
            container.RegisterType<IDbContextEmar, EMAREntities>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}