namespace LTCPro.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class ScreenEntity
    {
        public enum Screens
        {
            Dashboard = 1,
            CompanyMaster = 2,
            FacilityMaster = 3,
            NurseStationMaster = 4,
            FloorMaster = 5,
            WingMaster = 6,
            RoomMaster = 7,
            BedMaster = 8,
            Users = 10,
            RoleMaster = 11,
            RoleConfig = 12,
            UserRoleConfig = 12,
            HLSegmentFieldsConfig = 14,
            FTPConfiguration = 15,
            AllergyMaster = 16,
            ICD10Master = 17,
            CompanyBedConfig = 9,
            FrequencyMapping = 18,
            Inbound = 19,
            Outbound = 20,
            ResidentGrid = 21,
            DemographicInformation = 22,
            AdmitVisitInfo = 23,
            ResidentGridOrders = 24,
            Medication = 25,
            Allergies = 26,
            Diagnosis = 27,
            Integrations = 28,
            PhysicianDetails = 28,
            Orders = 29,
            AdminApproval = 30,
            DocumentManager = 31,
            EMAR = 32,
            PRNDocumentation = 33,
            SeventyTwoHourChecks = 34,
            ControlSubstance = 35,
            DrControlSubstance = 36,
            Vitals = 37,
            WeightLog = 38,
            FoodIntake = 39,
            NurseNotes = 40,
            Behaviour = 41,
            Census = 42,
            SeventyTwoHoursDashboard = 43,
            PRNDashboard = 44,
            OrderDashboard = 45,
            BarcodeDashboard = 46,
            BiometericDashboard = 47,
            OrderControlSubstanceDashboard = 48,
            OrderwithFavouritesDashboard = 49,
            OrderSignoffDashboard = 50,
            OrderHoldDashboard = 51,
            RefusedByResidentDashboard = 52,
            OrderChangeDashboard = 53,
            ScanningBypassDashboard = 54,
            DestructionDashboard = 55,
            FloorStockDashboard = 56,
            PharmacyMedsDashboard = 57,
            PsychiatricDashboard = 58,
            NurseNotesDashboard = 59,
            PrescriberNotesDashboard = 60,
            HL7InboundDashboard = 61,
            HL7OutboundErrorDashboard = 62,
            MARDashboard = 63,
            Mailbox = 64,
            UserActivityDashboard = 65,
            EkitDashboard=1064,
            StockDashboard=1065,
            AdminUsersReport=1067,
            SetupConfigReport=1068,
            RefillReport=2067,
            Stock=1060,
            Ekit=2070,
            CompanyConfiguration=1069,
            CompanyBedHierarchy=2077,
            ADTFieldsDisplayConfig=2071,
            NewResident = 2069,
            Transfer = 2072,
            Discharge = 2073,
            AcknowledgeOrders=1063,
            CheckInMeds=2074,
            CertificationOrders=3077,
            CertifiedOrdersReport=3078,
            DocumentAdminReport=2076,
            DocumentAdministeredOrders=2075,
            Alerts = 1062,
            ColorPicker = 1059,
            MedicationExpirationDateReport = 3080,
            EKitMedsExpirationDateReport = 3082,
            RefillRejectMailConfig=3083,
            CPOE=3084,
            ResidentProfileCertificationOrders = 3085, 
            ResidentProfileCertifiedOrdersReport = 3086,
            MailConfig = 3087,
        }
        public int Screen_Id { get; set; }
        public string Screen_Desc { get; set; }
        public int Screen_Status { get; set; }
        public Nullable<int> Screen_CreatedBy { get; set; }
        public System.DateTime Screen_CreatedDate { get; set; }
        public Nullable<int> Screen_Order { get; set; }
        public int DefaultScreen_Id { get; set; }

        public virtual UserEntity User { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleConfigEntity> RoleConfigs { get; set; }

        //public enum SubScreens
        //{
        //    UserRoleConfig =13,
        //    DemographicInformation =22,
        //    AdmitVisitInfo =23,
        //    ResidentGridOrders =24,
        //    Medication =25,
        //    Allergies =26,
        //    Diagnosis =27,
        //    DrControlSubstance =36,
        //    ReadMail =1061,
        //    AcknowledgeOrders =1063
        //}
    }
}
