
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/29/2023 09:11:27
-- Generated from EDMX file: D:\old system Data 04042021\Emar Service Latest 1\DAL\EMAR.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EMAR];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[Admin].[FK_ActivityMaster_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[ActivityMaster] DROP CONSTRAINT [FK_ActivityMaster_User];
GO
IF OBJECT_ID(N'[Patient].[FK_AddlInstructionDetails_CommonOrder]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AddlInstructionDetails] DROP CONSTRAINT [FK_AddlInstructionDetails_CommonOrder];
GO
IF OBJECT_ID(N'[Patient].[FK_AddlInstructionDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AddlInstructionDetails] DROP CONSTRAINT [FK_AddlInstructionDetails_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Alert_AlertText]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AlertStatus] DROP CONSTRAINT [FK_Alert_AlertText];
GO
IF OBJECT_ID(N'[Admin].[FK_Alert_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AlertStatus] DROP CONSTRAINT [FK_Alert_User];
GO
IF OBJECT_ID(N'[Admin].[FK_AlertText_AlertType]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AlertText] DROP CONSTRAINT [FK_AlertText_AlertType];
GO
IF OBJECT_ID(N'[Admin].[FK_AlertText_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AlertText] DROP CONSTRAINT [FK_AlertText_Demographics];
GO
IF OBJECT_ID(N'[Admin].[FK_AlertText_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AlertText] DROP CONSTRAINT [FK_AlertText_NursingStation];
GO
IF OBJECT_ID(N'[Patient].[FK_AllergyInfo_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AllergyInfo] DROP CONSTRAINT [FK_AllergyInfo_Demographics];
GO
IF OBJECT_ID(N'[Admin].[FK_AllergyInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AllergyInfoMaster] DROP CONSTRAINT [FK_AllergyInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_AllergyInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AllergyInfo] DROP CONSTRAINT [FK_AllergyInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_AllergyInfo_User11]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AllergyInfo] DROP CONSTRAINT [FK_AllergyInfo_User11];
GO
IF OBJECT_ID(N'[Admin].[FK_AllergyTypeCode_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[AllergyTypeCode] DROP CONSTRAINT [FK_AllergyTypeCode_User];
GO
IF OBJECT_ID(N'[Patient].[FK_AncillaryDetails_CommonOrder]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AncillaryDetails] DROP CONSTRAINT [FK_AncillaryDetails_CommonOrder];
GO
IF OBJECT_ID(N'[Patient].[FK_AncillaryDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AncillaryDetails] DROP CONSTRAINT [FK_AncillaryDetails_User];
GO
IF OBJECT_ID(N'[Patient].[FK_AncillaryDetails_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[AncillaryDetails] DROP CONSTRAINT [FK_AncillaryDetails_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_ApiIntegrations_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[ApiIntegrations] DROP CONSTRAINT [FK_ApiIntegrations_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalAllergyInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalAllergyInfo] DROP CONSTRAINT [FK_ApprovalAllergyInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalAllergyInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalAllergyInfo] DROP CONSTRAINT [FK_ApprovalAllergyInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalDemographics_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalDemographics] DROP CONSTRAINT [FK_ApprovalDemographics_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalDemographics_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalDemographics] DROP CONSTRAINT [FK_ApprovalDemographics_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalDemographics_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalDemographics] DROP CONSTRAINT [FK_ApprovalDemographics_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalDiagnosisInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalDiagnosisInfo] DROP CONSTRAINT [FK_ApprovalDiagnosisInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalDiagnosisInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalDiagnosisInfo] DROP CONSTRAINT [FK_ApprovalDiagnosisInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalOrders_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalOrders] DROP CONSTRAINT [FK_ApprovalOrders_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalOrders_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalOrders] DROP CONSTRAINT [FK_ApprovalOrders_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalRefill_OutBoundFileInformation]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalRefill] DROP CONSTRAINT [FK_ApprovalRefill_OutBoundFileInformation];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalVisitInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalVisitInfo] DROP CONSTRAINT [FK_ApprovalVisitInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalVisitInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalVisitInfo] DROP CONSTRAINT [FK_ApprovalVisitInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ApprovalVisitInfo_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ApprovalVisitInfo] DROP CONSTRAINT [FK_ApprovalVisitInfo_VisitInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_BarcodeDetails_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[BarcodeDetails] DROP CONSTRAINT [FK_BarcodeDetails_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_BarcodeDetails_Ekit]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[BarcodeDetails] DROP CONSTRAINT [FK_BarcodeDetails_Ekit];
GO
IF OBJECT_ID(N'[Patient].[FK_BarcodeDetails_Stock]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[BarcodeDetails] DROP CONSTRAINT [FK_BarcodeDetails_Stock];
GO
IF OBJECT_ID(N'[Patient].[FK_BarcodeDetails_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[BarcodeDetails] DROP CONSTRAINT [FK_BarcodeDetails_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_Bed_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Bed] DROP CONSTRAINT [FK_Bed_User];
GO
IF OBJECT_ID(N'[Admin].[FK_BehavioralSymptomsMaster_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[BehavioralSymptomsMaster] DROP CONSTRAINT [FK_BehavioralSymptomsMaster_User];
GO
IF OBJECT_ID(N'[Admin].[FK_ClassDrugCodingType_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[ClassDrugCodingType] DROP CONSTRAINT [FK_ClassDrugCodingType_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ColourType_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ColourType] DROP CONSTRAINT [FK_ColourType_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_ColourType_PatientType]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ColourType] DROP CONSTRAINT [FK_ColourType_PatientType];
GO
IF OBJECT_ID(N'[Patient].[FK_ColourType_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ColourType] DROP CONSTRAINT [FK_ColourType_User];
GO
IF OBJECT_ID(N'[Patient].[FK_CommonOrderInfo_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CommonOrderInfo] DROP CONSTRAINT [FK_CommonOrderInfo_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_CommonOrderInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CommonOrderInfo] DROP CONSTRAINT [FK_CommonOrderInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_CommonOrderInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CommonOrderInfo] DROP CONSTRAINT [FK_CommonOrderInfo_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_Company_Country]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Company] DROP CONSTRAINT [FK_Company_Country];
GO
IF OBJECT_ID(N'[Admin].[FK_Company_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Company] DROP CONSTRAINT [FK_Company_User];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Bed]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Bed];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Floor]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Floor];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Room]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Room];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyBedConfig_Wing]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyBedConfig] DROP CONSTRAINT [FK_CompanyBedConfig_Wing];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_Fingersdesc]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_Fingersdesc];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_HLDirectionalWay]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_HLDirectionalWay];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_PhysicianDetails]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_PhysicianDetails];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_StockReport]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_StockReport];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyConfig] DROP CONSTRAINT [FK_CompanyConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyHlCategory_CompanyConfig]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyHlCategory] DROP CONSTRAINT [FK_CompanyHlCategory_CompanyConfig];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyHlCategory_FTECategory]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyHlCategory] DROP CONSTRAINT [FK_CompanyHlCategory_FTECategory];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyHLEvent_CompanyHlCategory]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyHLEvent] DROP CONSTRAINT [FK_CompanyHLEvent_CompanyHlCategory];
GO
IF OBJECT_ID(N'[Admin].[FK_CompanyHLEvent_EventCategory]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[CompanyHLEvent] DROP CONSTRAINT [FK_CompanyHLEvent_EventCategory];
GO
IF OBJECT_ID(N'[Patient].[FK_CompoundOrder_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CompoundOrder] DROP CONSTRAINT [FK_CompoundOrder_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_CompoundOrder_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CompoundOrder] DROP CONSTRAINT [FK_CompoundOrder_User];
GO
IF OBJECT_ID(N'[Patient].[FK_CompoundOrder_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[CompoundOrder] DROP CONSTRAINT [FK_CompoundOrder_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ControlSubstanceCount_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceCount] DROP CONSTRAINT [FK_ControlSubstanceCount_NursingStation];
GO
IF OBJECT_ID(N'[Patient].[FK_ControlSubstanceCount_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceCount] DROP CONSTRAINT [FK_ControlSubstanceCount_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ControlSubstanceCount_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceCount] DROP CONSTRAINT [FK_ControlSubstanceCount_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_ControlSubstanceReason_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceReason] DROP CONSTRAINT [FK_ControlSubstanceReason_User];
GO
IF OBJECT_ID(N'[Chat].[FK_Conversation_Users]', 'F') IS NOT NULL
    ALTER TABLE [Chat].[Conversation] DROP CONSTRAINT [FK_Conversation_Users];
GO
IF OBJECT_ID(N'[Admin].[FK_Country_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Country] DROP CONSTRAINT [FK_Country_User];
GO
IF OBJECT_ID(N'[Admin].[FK_DefaultScreens_Screens]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[DefaultScreens] DROP CONSTRAINT [FK_DefaultScreens_Screens];
GO
IF OBJECT_ID(N'[Admin].[FK_DefaultScreens_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[DefaultScreens] DROP CONSTRAINT [FK_DefaultScreens_User];
GO
IF OBJECT_ID(N'[Patient].[FK_Demographics_PatientType]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[Demographics] DROP CONSTRAINT [FK_Demographics_PatientType];
GO
IF OBJECT_ID(N'[Patient].[FK_Demographics_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[Demographics] DROP CONSTRAINT [FK_Demographics_User];
GO
IF OBJECT_ID(N'[Patient].[FK_Demographics_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[Demographics] DROP CONSTRAINT [FK_Demographics_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_DiagnosisCodingType_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[DiagnosisCodingType] DROP CONSTRAINT [FK_DiagnosisCodingType_User];
GO
IF OBJECT_ID(N'[Patient].[FK_DiagnosisInfo_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DiagnosisInfo] DROP CONSTRAINT [FK_DiagnosisInfo_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_DiagnosisInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DiagnosisInfo] DROP CONSTRAINT [FK_DiagnosisInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_DiagnosisInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DiagnosisInfo] DROP CONSTRAINT [FK_DiagnosisInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_DocAdministerTrans_DrugAdminister]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DocAdministerTrans] DROP CONSTRAINT [FK_DocAdministerTrans_DrugAdminister];
GO
IF OBJECT_ID(N'[Patient].[FK_DocAdministerTrans_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DocAdministerTrans] DROP CONSTRAINT [FK_DocAdministerTrans_User];
GO
IF OBJECT_ID(N'[Admin].[FK_DocFolder_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[DocFolder] DROP CONSTRAINT [FK_DocFolder_User];
GO
IF OBJECT_ID(N'[Admin].[FK_DrFirstFileData_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[DrFirstFileData] DROP CONSTRAINT [FK_DrFirstFileData_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_Drug_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Drug] DROP CONSTRAINT [FK_Drug_User];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugActiveday_DrugAdministrationTime]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugActiveday] DROP CONSTRAINT [FK_DrugActiveday_DrugAdministrationTime];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_MedicationReason]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_MedicationReason];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_NurseShifts]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_NurseShifts];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_User];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_User2]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_User2];
GO
IF OBJECT_ID(N'[Patient].[FK_DrugAdminister_User3]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[DrugAdminister] DROP CONSTRAINT [FK_DrugAdminister_User3];
GO
IF OBJECT_ID(N'[Admin].[FK_EkControlSubstanceReason_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[EkitControlSubstanceReason] DROP CONSTRAINT [FK_EkControlSubstanceReason_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Ekit_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Ekit] DROP CONSTRAINT [FK_Ekit_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_Ekit_Facility1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Ekit] DROP CONSTRAINT [FK_Ekit_Facility1];
GO
IF OBJECT_ID(N'[Admin].[FK_Ekit_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Ekit] DROP CONSTRAINT [FK_Ekit_User];
GO
IF OBJECT_ID(N'[Patient].[FK_EkitAdminister_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EkitAdminister] DROP CONSTRAINT [FK_EkitAdminister_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_EkitAdminister_DrugAdminister]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EkitAdminister] DROP CONSTRAINT [FK_EkitAdminister_DrugAdminister];
GO
IF OBJECT_ID(N'[Patient].[FK_EkitAdminister_Ekit]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EkitAdminister] DROP CONSTRAINT [FK_EkitAdminister_Ekit];
GO
IF OBJECT_ID(N'[Patient].[FK_EkitAdminister_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EkitAdminister] DROP CONSTRAINT [FK_EkitAdminister_User];
GO
IF OBJECT_ID(N'[Admin].[FK_EkTransCSCount_ControlSubstanceCount]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[EKitControlSubstanceTrans] DROP CONSTRAINT [FK_EkTransCSCount_ControlSubstanceCount];
GO
IF OBJECT_ID(N'[Admin].[FK_EkTransCSCount_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[EKitControlSubstanceTrans] DROP CONSTRAINT [FK_EkTransCSCount_User];
GO
IF OBJECT_ID(N'[Admin].[FK_EkTransCSCount_User1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[EKitControlSubstanceTrans] DROP CONSTRAINT [FK_EkTransCSCount_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_EncodedOrderDetails_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EncodedOrderDetails] DROP CONSTRAINT [FK_EncodedOrderDetails_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_EncodedOrderDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EncodedOrderDetails] DROP CONSTRAINT [FK_EncodedOrderDetails_User];
GO
IF OBJECT_ID(N'[Patient].[FK_EncodedOrderDetails_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[EncodedOrderDetails] DROP CONSTRAINT [FK_EncodedOrderDetails_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_EventCategory_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[EventCategory] DROP CONSTRAINT [FK_EventCategory_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Facility_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Facility] DROP CONSTRAINT [FK_Facility_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_Facility_Country]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Facility] DROP CONSTRAINT [FK_Facility_Country];
GO
IF OBJECT_ID(N'[Admin].[FK_Facility_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Facility] DROP CONSTRAINT [FK_Facility_User];
GO
IF OBJECT_ID(N'[Patient].[FK_FavouriteData_FavouriteData]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavouriteData] DROP CONSTRAINT [FK_FavouriteData_FavouriteData];
GO
IF OBJECT_ID(N'[Patient].[FK_FavouriteData_OrderFavouriteMaster]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavouriteData] DROP CONSTRAINT [FK_FavouriteData_OrderFavouriteMaster];
GO
IF OBJECT_ID(N'[Patient].[FK_FavouriteData_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavouriteData] DROP CONSTRAINT [FK_FavouriteData_User];
GO
IF OBJECT_ID(N'[Admin].[FK_FileAckInformation_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FileAckInformation] DROP CONSTRAINT [FK_FileAckInformation_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_FileAckInformation_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FileAckInformation] DROP CONSTRAINT [FK_FileAckInformation_User];
GO
IF OBJECT_ID(N'[dbo].[FK_FileInfoError_FileInformation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FileInfoError] DROP CONSTRAINT [FK_FileInfoError_FileInformation];
GO
IF OBJECT_ID(N'[Admin].[FK_FileInformation_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FileInformation] DROP CONSTRAINT [FK_FileInformation_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_FileInformation_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FileInformation] DROP CONSTRAINT [FK_FileInformation_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Fingersdesc_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Fingersdesc] DROP CONSTRAINT [FK_Fingersdesc_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Floor_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Floor] DROP CONSTRAINT [FK_Floor_User];
GO
IF OBJECT_ID(N'[Admin].[FK_FrequencyMaster_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FrequencyMaster] DROP CONSTRAINT [FK_FrequencyMaster_User];
GO
IF OBJECT_ID(N'[Admin].[FK_FTECategory_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FTECategory] DROP CONSTRAINT [FK_FTECategory_User];
GO
IF OBJECT_ID(N'[Admin].[FK_FTEConfiguration_FTECategory]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FTEConfiguration] DROP CONSTRAINT [FK_FTEConfiguration_FTECategory];
GO
IF OBJECT_ID(N'[Admin].[FK_FTEConfiguration_FteConnection]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FTEConfiguration] DROP CONSTRAINT [FK_FTEConfiguration_FteConnection];
GO
IF OBJECT_ID(N'[Admin].[FK_FTEConfiguration_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FTEConfiguration] DROP CONSTRAINT [FK_FTEConfiguration_User];
GO
IF OBJECT_ID(N'[Admin].[FK_FteConnection_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[FteConnection] DROP CONSTRAINT [FK_FteConnection_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Gender_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Gender] DROP CONSTRAINT [FK_Gender_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HL7DirectionalWay_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLDirectionalWay] DROP CONSTRAINT [FK_HL7DirectionalWay_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenCompanyConfig_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenCompanyConfig] DROP CONSTRAINT [FK_HLSevenCompanyConfig_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenCompanyConfig_HLSevenSegmentDetails]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenCompanyConfig] DROP CONSTRAINT [FK_HLSevenCompanyConfig_HLSevenSegmentDetails];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenCompanyConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenCompanyConfig] DROP CONSTRAINT [FK_HLSevenCompanyConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplayCompanyConfig_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplayCompanyConfig] DROP CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplayCompanyConfig_HLSevenOutboundDisplaySegmentDetails]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplayCompanyConfig] DROP CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_HLSevenOutboundDisplaySegmentDetails];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplayCompanyConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplayCompanyConfig] DROP CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplaySegmentDetails_HLSevenOutboundDisplaySegments1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplaySegmentDetails] DROP CONSTRAINT [FK_HLSevenOutboundDisplaySegmentDetails_HLSevenOutboundDisplaySegments1];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplaySegmentDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplaySegmentDetails] DROP CONSTRAINT [FK_HLSevenOutboundDisplaySegmentDetails_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenOutboundDisplaySegments_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenOutboundDisplaySegments] DROP CONSTRAINT [FK_HLSevenOutboundDisplaySegments_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenSegmentDetails_HLSevenSegments]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenSegmentDetails] DROP CONSTRAINT [FK_HLSevenSegmentDetails_HLSevenSegments];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenSegmentDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenSegmentDetails] DROP CONSTRAINT [FK_HLSevenSegmentDetails_User];
GO
IF OBJECT_ID(N'[Admin].[FK_HLSevenSegments_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[HLSevenSegments] DROP CONSTRAINT [FK_HLSevenSegments_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Hours_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Hours] DROP CONSTRAINT [FK_Hours_User];
GO
IF OBJECT_ID(N'[Admin].[FK_ICD10_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[ICD10] DROP CONSTRAINT [FK_ICD10_User];
GO
IF OBJECT_ID(N'[Admin].[FK_ImportFile_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[ImportFile] DROP CONSTRAINT [FK_ImportFile_User];
GO
IF OBJECT_ID(N'[Patient].[FK_InsuranceInfo_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[InsuranceInfo] DROP CONSTRAINT [FK_InsuranceInfo_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_InsuranceInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[InsuranceInfo] DROP CONSTRAINT [FK_InsuranceInfo_User];
GO
IF OBJECT_ID(N'[Admin].[FK_IntegrationType_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[IntegrationType] DROP CONSTRAINT [FK_IntegrationType_User];
GO
IF OBJECT_ID(N'[Admin].[FK_MailConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[MailConfig] DROP CONSTRAINT [FK_MailConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_MailFavourite_MailBox]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[MailFavourite] DROP CONSTRAINT [FK_MailFavourite_MailBox];
GO
IF OBJECT_ID(N'[Admin].[FK_MailFavourite_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[MailFavourite] DROP CONSTRAINT [FK_MailFavourite_User];
GO
IF OBJECT_ID(N'[Admin].[FK_MaritalStatus_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[MaritalStatus] DROP CONSTRAINT [FK_MaritalStatus_User];
GO
IF OBJECT_ID(N'[Admin].[FK_MedicationReason_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[MedicationReason] DROP CONSTRAINT [FK_MedicationReason_User];
GO
IF OBJECT_ID(N'[Chat].[FK_Messages_Conversation]', 'F') IS NOT NULL
    ALTER TABLE [Chat].[Messages] DROP CONSTRAINT [FK_Messages_Conversation];
GO
IF OBJECT_ID(N'[Chat].[FK_Messages_Users]', 'F') IS NOT NULL
    ALTER TABLE [Chat].[Messages] DROP CONSTRAINT [FK_Messages_Users];
GO
IF OBJECT_ID(N'[Admin].[FK_Months_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Months] DROP CONSTRAINT [FK_Months_User];
GO
IF OBJECT_ID(N'[Patient].[FK_NotesInfo_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[NotesInfo] DROP CONSTRAINT [FK_NotesInfo_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_NotesInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[NotesInfo] DROP CONSTRAINT [FK_NotesInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_NurseComments_DrugAdminister]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[NurseComments] DROP CONSTRAINT [FK_NurseComments_DrugAdminister];
GO
IF OBJECT_ID(N'[Patient].[FK_NurseComments_QuantityDetails]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[NurseComments] DROP CONSTRAINT [FK_NurseComments_QuantityDetails];
GO
IF OBJECT_ID(N'[Patient].[FK_NurseComments_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[NurseComments] DROP CONSTRAINT [FK_NurseComments_User];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_Hours]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_Hours];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_Hours1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_Hours1];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_TimeFormat]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_TimeFormat];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_TimeFormat1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_TimeFormat1];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseShifts_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseShifts] DROP CONSTRAINT [FK_NurseShifts_User];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseStationHierarchy_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseStationHierarchy] DROP CONSTRAINT [FK_NurseStationHierarchy_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_NurseStationHierarchy_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NurseStationHierarchy] DROP CONSTRAINT [FK_NurseStationHierarchy_User];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFCTime_Hours]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFCTime] DROP CONSTRAINT [FK_NursingFCTime_Hours];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFCTime_NursingFrequencyConfig]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFCTime] DROP CONSTRAINT [FK_NursingFCTime_NursingFrequencyConfig];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Floor]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Floor];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_FrequencyMaster]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_FrequencyMaster];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Hours]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Hours];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Months]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Months];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_NursingStation1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_NursingStation1];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_TimeFormat]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_TimeFormat];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Weeks]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Weeks];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingFrequencyConfig_Wing]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingFrequencyConfig] DROP CONSTRAINT [FK_NursingFrequencyConfig_Wing];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingSchedule_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingSchedule] DROP CONSTRAINT [FK_NursingSchedule_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingSchedule_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingSchedule] DROP CONSTRAINT [FK_NursingSchedule_User];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingStation_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingStation] DROP CONSTRAINT [FK_NursingStation_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingStation_PhysicianDetails]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingStation] DROP CONSTRAINT [FK_NursingStation_PhysicianDetails];
GO
IF OBJECT_ID(N'[Admin].[FK_NursingStation_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[NursingStation] DROP CONSTRAINT [FK_NursingStation_User];
GO
IF OBJECT_ID(N'[Patient].[FK_Observation_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[Observation] DROP CONSTRAINT [FK_Observation_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_Observation_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[Observation] DROP CONSTRAINT [FK_Observation_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OnLeave_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OnLeave] DROP CONSTRAINT [FK_OnLeave_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OnLeave_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OnLeave] DROP CONSTRAINT [FK_OnLeave_VisitInfo];
GO
IF OBJECT_ID(N'[Admin].[FK_OrderControlMaster_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[OrderControlMaster] DROP CONSTRAINT [FK_OrderControlMaster_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderDestroy_QuantityDetails]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderDestroy] DROP CONSTRAINT [FK_OrderDestroy_QuantityDetails];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderDestroy_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderDestroy] DROP CONSTRAINT [FK_OrderDestroy_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderDestroy_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderDestroy] DROP CONSTRAINT [FK_OrderDestroy_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderDestroy_User2]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderDestroy] DROP CONSTRAINT [FK_OrderDestroy_User2];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderFavourite_OrderFavouriteMaster]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavourite] DROP CONSTRAINT [FK_OrderFavourite_OrderFavouriteMaster];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderFavourite_QuantityDetails]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavourite] DROP CONSTRAINT [FK_OrderFavourite_QuantityDetails];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderFavourite_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderFavourite] DROP CONSTRAINT [FK_OrderFavourite_User];
GO
IF OBJECT_ID(N'[Admin].[FK_OrderFavouriteMaster_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[OrderFavouriteMaster] DROP CONSTRAINT [FK_OrderFavouriteMaster_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderHold_QuantityDetails]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderHold] DROP CONSTRAINT [FK_OrderHold_QuantityDetails];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderHold_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderHold] DROP CONSTRAINT [FK_OrderHold_User];
GO
IF OBJECT_ID(N'[Admin].[FK_OrderInactiveReason_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[OrderInactiveReason] DROP CONSTRAINT [FK_OrderInactiveReason_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OrderStock_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderStock] DROP CONSTRAINT [FK_OrderStock_User];
GO
IF OBJECT_ID(N'[Admin].[FK_OrderType_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[OrderType] DROP CONSTRAINT [FK_OrderType_User];
GO
IF OBJECT_ID(N'[Patient].[FK_OutBoundFileInformation_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OutBoundFileInformation] DROP CONSTRAINT [FK_OutBoundFileInformation_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_OutBoundFileInformation_Company]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OutBoundFileInformation] DROP CONSTRAINT [FK_OutBoundFileInformation_Company];
GO
IF OBJECT_ID(N'[Patient].[FK_OutBoundFileInformation_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OutBoundFileInformation] DROP CONSTRAINT [FK_OutBoundFileInformation_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_OutBoundFileInformation_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OutBoundFileInformation] DROP CONSTRAINT [FK_OutBoundFileInformation_User];
GO
IF OBJECT_ID(N'[Chat].[FK_Participants_Conversation]', 'F') IS NOT NULL
    ALTER TABLE [Chat].[Participants] DROP CONSTRAINT [FK_Participants_Conversation];
GO
IF OBJECT_ID(N'[Chat].[FK_Participants_Users]', 'F') IS NOT NULL
    ALTER TABLE [Chat].[Participants] DROP CONSTRAINT [FK_Participants_Users];
GO
IF OBJECT_ID(N'[Admin].[FK_PatientType_Company]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[PatientType] DROP CONSTRAINT [FK_PatientType_Company];
GO
IF OBJECT_ID(N'[Admin].[FK_PatientType_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[PatientType] DROP CONSTRAINT [FK_PatientType_User];
GO
IF OBJECT_ID(N'[Admin].[FK_PhysicianDetails_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[PhysicianDetails] DROP CONSTRAINT [FK_PhysicianDetails_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_PhysicianDetails_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[PhysicianDetails] DROP CONSTRAINT [FK_PhysicianDetails_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_PhysicianDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[PhysicianDetails] DROP CONSTRAINT [FK_PhysicianDetails_User];
GO
IF OBJECT_ID(N'[Patient].[FK_QuantityDetails_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[QuantityDetails] DROP CONSTRAINT [FK_QuantityDetails_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_QuantityDetails_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[QuantityDetails] DROP CONSTRAINT [FK_QuantityDetails_User];
GO
IF OBJECT_ID(N'[Admin].[FK_RecentFac_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[RecentFac] DROP CONSTRAINT [FK_RecentFac_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_RecentFac_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[RecentFac] DROP CONSTRAINT [FK_RecentFac_User];
GO
IF OBJECT_ID(N'[Patient].[FK_ResidentOrder_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ResidentOrder] DROP CONSTRAINT [FK_ResidentOrder_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_ResidentOrder_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ResidentOrder] DROP CONSTRAINT [FK_ResidentOrder_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Role_DefaultScreens]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Role] DROP CONSTRAINT [FK_Role_DefaultScreens];
GO
IF OBJECT_ID(N'[Admin].[FK_Role_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Role] DROP CONSTRAINT [FK_Role_User];
GO
IF OBJECT_ID(N'[Admin].[FK_RoleConfig_Role]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[RoleConfig] DROP CONSTRAINT [FK_RoleConfig_Role];
GO
IF OBJECT_ID(N'[Admin].[FK_RoleConfig_Screens]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[RoleConfig] DROP CONSTRAINT [FK_RoleConfig_Screens];
GO
IF OBJECT_ID(N'[Admin].[FK_RoleConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[RoleConfig] DROP CONSTRAINT [FK_RoleConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Room_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Room] DROP CONSTRAINT [FK_Room_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Route_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Route] DROP CONSTRAINT [FK_Route_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Screens_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Screens] DROP CONSTRAINT [FK_Screens_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Stock_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Stock] DROP CONSTRAINT [FK_Stock_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_Stock_Facility1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Stock] DROP CONSTRAINT [FK_Stock_Facility1];
GO
IF OBJECT_ID(N'[Admin].[FK_Stock_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Stock] DROP CONSTRAINT [FK_Stock_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Suffix_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Suffix] DROP CONSTRAINT [FK_Suffix_User];
GO
IF OBJECT_ID(N'[Admin].[FK_tblRDCMailConfig_Facility]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[tblRDCMailConfig] DROP CONSTRAINT [FK_tblRDCMailConfig_Facility];
GO
IF OBJECT_ID(N'[Admin].[FK_tblRDCMailConfig_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[tblRDCMailConfig] DROP CONSTRAINT [FK_tblRDCMailConfig_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_tblRdcMailTime_tblRDCMailConfig]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[tblRdcMailTime] DROP CONSTRAINT [FK_tblRdcMailTime_tblRDCMailConfig];
GO
IF OBJECT_ID(N'[Admin].[FK_TimeFormat_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[TimeFormat] DROP CONSTRAINT [FK_TimeFormat_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TransCSCount_ControlSubstanceCount]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceTrans] DROP CONSTRAINT [FK_TransCSCount_ControlSubstanceCount];
GO
IF OBJECT_ID(N'[Patient].[FK_TransCSCount_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceTrans] DROP CONSTRAINT [FK_TransCSCount_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TransCSCount_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[ControlSubstanceTrans] DROP CONSTRAINT [FK_TransCSCount_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_TransOS_OrderStock]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderStockTrans] DROP CONSTRAINT [FK_TransOS_OrderStock];
GO
IF OBJECT_ID(N'[Patient].[FK_TransOS_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[OrderStockTrans] DROP CONSTRAINT [FK_TransOS_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentDispenseInfo_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentDispenseInfo] DROP CONSTRAINT [FK_TreatmentDispenseInfo_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentDispenseInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentDispenseInfo] DROP CONSTRAINT [FK_TreatmentDispenseInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentDispenseInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentDispenseInfo] DROP CONSTRAINT [FK_TreatmentDispenseInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentInfo_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentInfo] DROP CONSTRAINT [FK_TreatmentInfo_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentInfo] DROP CONSTRAINT [FK_TreatmentInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentInfo] DROP CONSTRAINT [FK_TreatmentInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentRouteInfo_CommonOrderInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentRouteInfo] DROP CONSTRAINT [FK_TreatmentRouteInfo_CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentRouteInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentRouteInfo] DROP CONSTRAINT [FK_TreatmentRouteInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_TreatmentRouteInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[TreatmentRouteInfo] DROP CONSTRAINT [FK_TreatmentRouteInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_UploadedDocuments_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[UploadedDocuments] DROP CONSTRAINT [FK_UploadedDocuments_User];
GO
IF OBJECT_ID(N'[Admin].[FK_User_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[User] DROP CONSTRAINT [FK_User_User];
GO
IF OBJECT_ID(N'[Admin].[FK_UserActivityDetails_ActivityMaster]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserActivityDetails] DROP CONSTRAINT [FK_UserActivityDetails_ActivityMaster];
GO
IF OBJECT_ID(N'[Admin].[FK_UserActivityDetails_UserSession]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserActivityDetails] DROP CONSTRAINT [FK_UserActivityDetails_UserSession];
GO
IF OBJECT_ID(N'[Admin].[FK_UserRoleFacilityConfig_NursingStation]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserRoleFacilityConfig] DROP CONSTRAINT [FK_UserRoleFacilityConfig_NursingStation];
GO
IF OBJECT_ID(N'[Admin].[FK_UserRoleFacilityConfig_Role]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserRoleFacilityConfig] DROP CONSTRAINT [FK_UserRoleFacilityConfig_Role];
GO
IF OBJECT_ID(N'[Admin].[FK_UserRoleFacilityConfig_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserRoleFacilityConfig] DROP CONSTRAINT [FK_UserRoleFacilityConfig_User];
GO
IF OBJECT_ID(N'[Admin].[FK_UserRoleFacilityConfig_User1]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserRoleFacilityConfig] DROP CONSTRAINT [FK_UserRoleFacilityConfig_User1];
GO
IF OBJECT_ID(N'[Admin].[FK_UserSession_Role]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserSession] DROP CONSTRAINT [FK_UserSession_Role];
GO
IF OBJECT_ID(N'[Admin].[FK_UserSession_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[UserSession] DROP CONSTRAINT [FK_UserSession_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitBehaviour_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitBehaviour] DROP CONSTRAINT [FK_VisitBehaviour_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitBehaviour_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitBehaviour] DROP CONSTRAINT [FK_VisitBehaviour_VisitInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitFoodintake_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitFoodintake] DROP CONSTRAINT [FK_VisitFoodintake_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitFoodintake_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitFoodintake] DROP CONSTRAINT [FK_VisitFoodintake_VisitInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitInfo_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitInfo] DROP CONSTRAINT [FK_VisitInfo_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitInfo_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitInfo] DROP CONSTRAINT [FK_VisitInfo_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitInfo_User1]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitInfo] DROP CONSTRAINT [FK_VisitInfo_User1];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitNursingNotes_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitNursingNotes] DROP CONSTRAINT [FK_VisitNursingNotes_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VisitNursingNotes_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitNursingNotes] DROP CONSTRAINT [FK_VisitNursingNotes_VisitInfo];
GO
IF OBJECT_ID(N'[Patient].[FK_VitalSigns_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitVitals] DROP CONSTRAINT [FK_VitalSigns_User];
GO
IF OBJECT_ID(N'[Patient].[FK_VitalSigns_VisitInfo]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[VisitVitals] DROP CONSTRAINT [FK_VitalSigns_VisitInfo];
GO
IF OBJECT_ID(N'[Admin].[FK_WeekDays_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[WeekDays] DROP CONSTRAINT [FK_WeekDays_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Weeks_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Weeks] DROP CONSTRAINT [FK_Weeks_User];
GO
IF OBJECT_ID(N'[Patient].[FK_WeightLog_Demographics]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[WeightLog] DROP CONSTRAINT [FK_WeightLog_Demographics];
GO
IF OBJECT_ID(N'[Patient].[FK_WeightLog_User]', 'F') IS NOT NULL
    ALTER TABLE [Patient].[WeightLog] DROP CONSTRAINT [FK_WeightLog_User];
GO
IF OBJECT_ID(N'[Admin].[FK_Wing_User]', 'F') IS NOT NULL
    ALTER TABLE [Admin].[Wing] DROP CONSTRAINT [FK_Wing_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[Admin].[ActivityMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ActivityMaster];
GO
IF OBJECT_ID(N'[Admin].[AlertStatus]', 'U') IS NOT NULL
    DROP TABLE [Admin].[AlertStatus];
GO
IF OBJECT_ID(N'[Admin].[AlertText]', 'U') IS NOT NULL
    DROP TABLE [Admin].[AlertText];
GO
IF OBJECT_ID(N'[Admin].[AlertType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[AlertType];
GO
IF OBJECT_ID(N'[Admin].[AllergyInfoMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[AllergyInfoMaster];
GO
IF OBJECT_ID(N'[Admin].[AllergyTypeCode]', 'U') IS NOT NULL
    DROP TABLE [Admin].[AllergyTypeCode];
GO
IF OBJECT_ID(N'[Admin].[ApiIntegrations]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ApiIntegrations];
GO
IF OBJECT_ID(N'[Admin].[Bed]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Bed];
GO
IF OBJECT_ID(N'[Admin].[BehavioralSymptomsMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[BehavioralSymptomsMaster];
GO
IF OBJECT_ID(N'[Admin].[CertifyDates]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CertifyDates];
GO
IF OBJECT_ID(N'[Admin].[CertifyOrders]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CertifyOrders];
GO
IF OBJECT_ID(N'[Admin].[ClassDrugCodingType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ClassDrugCodingType];
GO
IF OBJECT_ID(N'[Admin].[Company]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Company];
GO
IF OBJECT_ID(N'[Admin].[CompanyBedConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CompanyBedConfig];
GO
IF OBJECT_ID(N'[Admin].[CompanyConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CompanyConfig];
GO
IF OBJECT_ID(N'[Admin].[CompanyHlCategory]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CompanyHlCategory];
GO
IF OBJECT_ID(N'[Admin].[CompanyHLEvent]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CompanyHLEvent];
GO
IF OBJECT_ID(N'[Admin].[Country]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Country];
GO
IF OBJECT_ID(N'[Admin].[CpoeSource]', 'U') IS NOT NULL
    DROP TABLE [Admin].[CpoeSource];
GO
IF OBJECT_ID(N'[Admin].[DefaultScreens]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DefaultScreens];
GO
IF OBJECT_ID(N'[Admin].[DiagnosisCodingType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DiagnosisCodingType];
GO
IF OBJECT_ID(N'[Admin].[DocFolder]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DocFolder];
GO
IF OBJECT_ID(N'[Admin].[DoseUom]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DoseUom];
GO
IF OBJECT_ID(N'[Admin].[DrFirstFileData]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DrFirstFileData];
GO
IF OBJECT_ID(N'[Admin].[DrFirstOrderXMLTrans]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DrFirstOrderXMLTrans];
GO
IF OBJECT_ID(N'[Admin].[DrFirstXMLTrans]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DrFirstXMLTrans];
GO
IF OBJECT_ID(N'[Admin].[Drug]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Drug];
GO
IF OBJECT_ID(N'[Admin].[DrugOrderMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[DrugOrderMaster];
GO
IF OBJECT_ID(N'[Admin].[Ekit]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Ekit];
GO
IF OBJECT_ID(N'[Admin].[EkitControlSubstanceReason]', 'U') IS NOT NULL
    DROP TABLE [Admin].[EkitControlSubstanceReason];
GO
IF OBJECT_ID(N'[Admin].[EKitControlSubstanceTrans]', 'U') IS NOT NULL
    DROP TABLE [Admin].[EKitControlSubstanceTrans];
GO
IF OBJECT_ID(N'[Admin].[EventCategory]', 'U') IS NOT NULL
    DROP TABLE [Admin].[EventCategory];
GO
IF OBJECT_ID(N'[Admin].[Facility]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Facility];
GO
IF OBJECT_ID(N'[Admin].[FileAckInformation]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FileAckInformation];
GO
IF OBJECT_ID(N'[Admin].[FileInformation]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FileInformation];
GO
IF OBJECT_ID(N'[Admin].[Fingersdesc]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Fingersdesc];
GO
IF OBJECT_ID(N'[Admin].[Floor]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Floor];
GO
IF OBJECT_ID(N'[Admin].[FooterLogo]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FooterLogo];
GO
IF OBJECT_ID(N'[Admin].[FrequencyMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FrequencyMaster];
GO
IF OBJECT_ID(N'[Admin].[FTECategory]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FTECategory];
GO
IF OBJECT_ID(N'[Admin].[FTEConfiguration]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FTEConfiguration];
GO
IF OBJECT_ID(N'[Admin].[FteConnection]', 'U') IS NOT NULL
    DROP TABLE [Admin].[FteConnection];
GO
IF OBJECT_ID(N'[Admin].[Gender]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Gender];
GO
IF OBJECT_ID(N'[Admin].[HLDirectionalWay]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLDirectionalWay];
GO
IF OBJECT_ID(N'[Admin].[HLSevenCompanyConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenCompanyConfig];
GO
IF OBJECT_ID(N'[Admin].[HLSevenOutboundDisplayCompanyConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenOutboundDisplayCompanyConfig];
GO
IF OBJECT_ID(N'[Admin].[HLSevenOutboundDisplaySegmentDetails]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenOutboundDisplaySegmentDetails];
GO
IF OBJECT_ID(N'[Admin].[HLSevenOutboundDisplaySegments]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenOutboundDisplaySegments];
GO
IF OBJECT_ID(N'[Admin].[HLSevenSegmentDetails]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenSegmentDetails];
GO
IF OBJECT_ID(N'[Admin].[HLSevenSegments]', 'U') IS NOT NULL
    DROP TABLE [Admin].[HLSevenSegments];
GO
IF OBJECT_ID(N'[Admin].[Hours]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Hours];
GO
IF OBJECT_ID(N'[Admin].[ICD10]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ICD10];
GO
IF OBJECT_ID(N'[Admin].[ImportFile]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ImportFile];
GO
IF OBJECT_ID(N'[Admin].[IntegrationType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[IntegrationType];
GO
IF OBJECT_ID(N'[Admin].[MailBox]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MailBox];
GO
IF OBJECT_ID(N'[Admin].[MailConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MailConfig];
GO
IF OBJECT_ID(N'[Admin].[MailFavourite]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MailFavourite];
GO
IF OBJECT_ID(N'[Admin].[MailRead]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MailRead];
GO
IF OBJECT_ID(N'[Admin].[MaritalStatus]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MaritalStatus];
GO
IF OBJECT_ID(N'[Admin].[MedicationReason]', 'U') IS NOT NULL
    DROP TABLE [Admin].[MedicationReason];
GO
IF OBJECT_ID(N'[Admin].[Months]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Months];
GO
IF OBJECT_ID(N'[Admin].[NurseCommentType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NurseCommentType];
GO
IF OBJECT_ID(N'[Admin].[NurseShifts]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NurseShifts];
GO
IF OBJECT_ID(N'[Admin].[NurseStationHierarchy]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NurseStationHierarchy];
GO
IF OBJECT_ID(N'[Admin].[NursingFCTime]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NursingFCTime];
GO
IF OBJECT_ID(N'[Admin].[NursingFrequencyConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NursingFrequencyConfig];
GO
IF OBJECT_ID(N'[Admin].[NursingSchedule]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NursingSchedule];
GO
IF OBJECT_ID(N'[Admin].[NursingStation]', 'U') IS NOT NULL
    DROP TABLE [Admin].[NursingStation];
GO
IF OBJECT_ID(N'[Admin].[OrderControlMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[OrderControlMaster];
GO
IF OBJECT_ID(N'[Admin].[OrderFavConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[OrderFavConfig];
GO
IF OBJECT_ID(N'[Admin].[OrderFavouriteMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[OrderFavouriteMaster];
GO
IF OBJECT_ID(N'[Admin].[OrderInactiveReason]', 'U') IS NOT NULL
    DROP TABLE [Admin].[OrderInactiveReason];
GO
IF OBJECT_ID(N'[Admin].[OrderType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[OrderType];
GO
IF OBJECT_ID(N'[Admin].[PatientType]', 'U') IS NOT NULL
    DROP TABLE [Admin].[PatientType];
GO
IF OBJECT_ID(N'[Admin].[PCertifyDates]', 'U') IS NOT NULL
    DROP TABLE [Admin].[PCertifyDates];
GO
IF OBJECT_ID(N'[Admin].[PCertifyOrders]', 'U') IS NOT NULL
    DROP TABLE [Admin].[PCertifyOrders];
GO
IF OBJECT_ID(N'[Admin].[PhysicianDetails]', 'U') IS NOT NULL
    DROP TABLE [Admin].[PhysicianDetails];
GO
IF OBJECT_ID(N'[Admin].[ProcessKeyMaster]', 'U') IS NOT NULL
    DROP TABLE [Admin].[ProcessKeyMaster];
GO
IF OBJECT_ID(N'[Admin].[QuantityDose]', 'U') IS NOT NULL
    DROP TABLE [Admin].[QuantityDose];
GO
IF OBJECT_ID(N'[Admin].[RecentFac]', 'U') IS NOT NULL
    DROP TABLE [Admin].[RecentFac];
GO
IF OBJECT_ID(N'[Admin].[Role]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Role];
GO
IF OBJECT_ID(N'[Admin].[RoleConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[RoleConfig];
GO
IF OBJECT_ID(N'[Admin].[Room]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Room];
GO
IF OBJECT_ID(N'[Admin].[Route]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Route];
GO
IF OBJECT_ID(N'[Admin].[Screens]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Screens];
GO
IF OBJECT_ID(N'[Admin].[Stock]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Stock];
GO
IF OBJECT_ID(N'[Admin].[StockReport]', 'U') IS NOT NULL
    DROP TABLE [Admin].[StockReport];
GO
IF OBJECT_ID(N'[Admin].[Suffix]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Suffix];
GO
IF OBJECT_ID(N'[Admin].[tblRDCMailConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[tblRDCMailConfig];
GO
IF OBJECT_ID(N'[Admin].[tblRdcMailTime]', 'U') IS NOT NULL
    DROP TABLE [Admin].[tblRdcMailTime];
GO
IF OBJECT_ID(N'[Admin].[tblSideEffect]', 'U') IS NOT NULL
    DROP TABLE [Admin].[tblSideEffect];
GO
IF OBJECT_ID(N'[Admin].[tblTimeZone]', 'U') IS NOT NULL
    DROP TABLE [Admin].[tblTimeZone];
GO
IF OBJECT_ID(N'[Admin].[TimeFormat]', 'U') IS NOT NULL
    DROP TABLE [Admin].[TimeFormat];
GO
IF OBJECT_ID(N'[Admin].[UnitMeasurement]', 'U') IS NOT NULL
    DROP TABLE [Admin].[UnitMeasurement];
GO
IF OBJECT_ID(N'[Admin].[User]', 'U') IS NOT NULL
    DROP TABLE [Admin].[User];
GO
IF OBJECT_ID(N'[Admin].[UserActivityDetails]', 'U') IS NOT NULL
    DROP TABLE [Admin].[UserActivityDetails];
GO
IF OBJECT_ID(N'[Admin].[UserOTP]', 'U') IS NOT NULL
    DROP TABLE [Admin].[UserOTP];
GO
IF OBJECT_ID(N'[Admin].[UserRoleFacilityConfig]', 'U') IS NOT NULL
    DROP TABLE [Admin].[UserRoleFacilityConfig];
GO
IF OBJECT_ID(N'[Admin].[UserSession]', 'U') IS NOT NULL
    DROP TABLE [Admin].[UserSession];
GO
IF OBJECT_ID(N'[Admin].[WeekDays]', 'U') IS NOT NULL
    DROP TABLE [Admin].[WeekDays];
GO
IF OBJECT_ID(N'[Admin].[Weeks]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Weeks];
GO
IF OBJECT_ID(N'[Admin].[Wing]', 'U') IS NOT NULL
    DROP TABLE [Admin].[Wing];
GO
IF OBJECT_ID(N'[Chat].[Conversation]', 'U') IS NOT NULL
    DROP TABLE [Chat].[Conversation];
GO
IF OBJECT_ID(N'[Chat].[Messages]', 'U') IS NOT NULL
    DROP TABLE [Chat].[Messages];
GO
IF OBJECT_ID(N'[Chat].[Participants]', 'U') IS NOT NULL
    DROP TABLE [Chat].[Participants];
GO
IF OBJECT_ID(N'[Chat].[Users]', 'U') IS NOT NULL
    DROP TABLE [Chat].[Users];
GO
IF OBJECT_ID(N'[dbo].[FileInfoError]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileInfoError];
GO
IF OBJECT_ID(N'[dbo].[tmptblSegmentCheck]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tmptblSegmentCheck];
GO
IF OBJECT_ID(N'[Patient].[AddlInstructionDetails]', 'U') IS NOT NULL
    DROP TABLE [Patient].[AddlInstructionDetails];
GO
IF OBJECT_ID(N'[Patient].[AllergyInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[AllergyInfo];
GO
IF OBJECT_ID(N'[Patient].[AncillaryDetails]', 'U') IS NOT NULL
    DROP TABLE [Patient].[AncillaryDetails];
GO
IF OBJECT_ID(N'[Patient].[ApprovalAllergyInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalAllergyInfo];
GO
IF OBJECT_ID(N'[Patient].[ApprovalDemographics]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalDemographics];
GO
IF OBJECT_ID(N'[Patient].[ApprovalDiagnosisInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalDiagnosisInfo];
GO
IF OBJECT_ID(N'[Patient].[ApprovalOrders]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalOrders];
GO
IF OBJECT_ID(N'[Patient].[ApprovalRefill]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalRefill];
GO
IF OBJECT_ID(N'[Patient].[ApprovalVisitInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ApprovalVisitInfo];
GO
IF OBJECT_ID(N'[Patient].[BarcodeDetails]', 'U') IS NOT NULL
    DROP TABLE [Patient].[BarcodeDetails];
GO
IF OBJECT_ID(N'[Patient].[ColourType]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ColourType];
GO
IF OBJECT_ID(N'[Patient].[CommonOrderInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[CommonOrderInfo];
GO
IF OBJECT_ID(N'[Patient].[CompoundOrder]', 'U') IS NOT NULL
    DROP TABLE [Patient].[CompoundOrder];
GO
IF OBJECT_ID(N'[Patient].[ControlSubstanceCount]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ControlSubstanceCount];
GO
IF OBJECT_ID(N'[Patient].[ControlSubstanceReason]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ControlSubstanceReason];
GO
IF OBJECT_ID(N'[Patient].[ControlSubstanceTrans]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ControlSubstanceTrans];
GO
IF OBJECT_ID(N'[Patient].[Demographics]', 'U') IS NOT NULL
    DROP TABLE [Patient].[Demographics];
GO
IF OBJECT_ID(N'[Patient].[DiagnosisInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[DiagnosisInfo];
GO
IF OBJECT_ID(N'[Patient].[DocAdministerTrans]', 'U') IS NOT NULL
    DROP TABLE [Patient].[DocAdministerTrans];
GO
IF OBJECT_ID(N'[Patient].[DrugActiveday]', 'U') IS NOT NULL
    DROP TABLE [Patient].[DrugActiveday];
GO
IF OBJECT_ID(N'[Patient].[DrugAdminister]', 'U') IS NOT NULL
    DROP TABLE [Patient].[DrugAdminister];
GO
IF OBJECT_ID(N'[Patient].[DrugAdministrationTime]', 'U') IS NOT NULL
    DROP TABLE [Patient].[DrugAdministrationTime];
GO
IF OBJECT_ID(N'[Patient].[EkitAdminister]', 'U') IS NOT NULL
    DROP TABLE [Patient].[EkitAdminister];
GO
IF OBJECT_ID(N'[Patient].[EncodedOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [Patient].[EncodedOrderDetails];
GO
IF OBJECT_ID(N'[Patient].[InsuranceInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[InsuranceInfo];
GO
IF OBJECT_ID(N'[Patient].[NotesInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[NotesInfo];
GO
IF OBJECT_ID(N'[Patient].[NurseComments]', 'U') IS NOT NULL
    DROP TABLE [Patient].[NurseComments];
GO
IF OBJECT_ID(N'[Patient].[Observation]', 'U') IS NOT NULL
    DROP TABLE [Patient].[Observation];
GO
IF OBJECT_ID(N'[Patient].[OnLeave]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OnLeave];
GO
IF OBJECT_ID(N'[Patient].[OrderDestroy]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderDestroy];
GO
IF OBJECT_ID(N'[Patient].[OrderFavourite]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderFavourite];
GO
IF OBJECT_ID(N'[Patient].[OrderFavouriteData]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderFavouriteData];
GO
IF OBJECT_ID(N'[Patient].[OrderHOAInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderHOAInfo];
GO
IF OBJECT_ID(N'[Patient].[OrderHold]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderHold];
GO
IF OBJECT_ID(N'[Patient].[OrderStock]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderStock];
GO
IF OBJECT_ID(N'[Patient].[OrderStockTrans]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OrderStockTrans];
GO
IF OBJECT_ID(N'[Patient].[OutBoundFileInformation]', 'U') IS NOT NULL
    DROP TABLE [Patient].[OutBoundFileInformation];
GO
IF OBJECT_ID(N'[Patient].[QuantityDetails]', 'U') IS NOT NULL
    DROP TABLE [Patient].[QuantityDetails];
GO
IF OBJECT_ID(N'[Patient].[ResidentOrder]', 'U') IS NOT NULL
    DROP TABLE [Patient].[ResidentOrder];
GO
IF OBJECT_ID(N'[Patient].[tblAdministeredSites]', 'U') IS NOT NULL
    DROP TABLE [Patient].[tblAdministeredSites];
GO
IF OBJECT_ID(N'[Patient].[tblDrugDiscardInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[tblDrugDiscardInfo];
GO
IF OBJECT_ID(N'[Patient].[TreatmentDispenseInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[TreatmentDispenseInfo];
GO
IF OBJECT_ID(N'[Patient].[TreatmentInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[TreatmentInfo];
GO
IF OBJECT_ID(N'[Patient].[TreatmentRouteInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[TreatmentRouteInfo];
GO
IF OBJECT_ID(N'[Patient].[UploadedDocuments]', 'U') IS NOT NULL
    DROP TABLE [Patient].[UploadedDocuments];
GO
IF OBJECT_ID(N'[Patient].[VisitBehaviour]', 'U') IS NOT NULL
    DROP TABLE [Patient].[VisitBehaviour];
GO
IF OBJECT_ID(N'[Patient].[VisitFoodintake]', 'U') IS NOT NULL
    DROP TABLE [Patient].[VisitFoodintake];
GO
IF OBJECT_ID(N'[Patient].[VisitInfo]', 'U') IS NOT NULL
    DROP TABLE [Patient].[VisitInfo];
GO
IF OBJECT_ID(N'[Patient].[VisitNursingNotes]', 'U') IS NOT NULL
    DROP TABLE [Patient].[VisitNursingNotes];
GO
IF OBJECT_ID(N'[Patient].[VisitVitals]', 'U') IS NOT NULL
    DROP TABLE [Patient].[VisitVitals];
GO
IF OBJECT_ID(N'[Patient].[WeightLog]', 'U') IS NOT NULL
    DROP TABLE [Patient].[WeightLog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AllergyTypeCodes'
CREATE TABLE [dbo].[AllergyTypeCodes] (
    [AllergyType_Id] int IDENTITY(1,1) NOT NULL,
    [AllergyType_Code] varchar(50)  NULL,
    [AllergyType_Desc] varchar(50)  NULL,
    [AllergyType_Status] int  NOT NULL,
    [AllergyType_CreatedBy] int  NULL,
    [AllergyType_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'ApiIntegrations'
CREATE TABLE [dbo].[ApiIntegrations] (
    [Integration_Id] int IDENTITY(1,1) NOT NULL,
    [Integration_TypeId] int  NULL,
    [Company_Id] int  NULL,
    [ApiPath] varchar(500)  NULL,
    [UserName] varchar(50)  NULL,
    [Password] varchar(50)  NULL,
    [Integration_Status] int  NOT NULL,
    [Integration_CreatedBy] int  NULL,
    [Integration_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Beds'
CREATE TABLE [dbo].[Beds] (
    [Bed_Id] int IDENTITY(1,1) NOT NULL,
    [Bed_Name] varchar(50)  NULL,
    [Bed_Code] varchar(20)  NULL,
    [Bed_Status] int  NOT NULL,
    [Bed_CreatedBy] int  NULL,
    [Bed_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'BehavioralSymptomsMasters'
CREATE TABLE [dbo].[BehavioralSymptomsMasters] (
    [BehavioralSym_ID] int IDENTITY(1,1) NOT NULL,
    [BehavioralSym_Desc] varchar(150)  NULL,
    [BehavioralSym_Status] int  NOT NULL,
    [BehavioralSym_CreatedBy] int  NULL,
    [BehavioralSym_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'ClassDrugCodingTypes'
CREATE TABLE [dbo].[ClassDrugCodingTypes] (
    [CDCodingType_Id] int IDENTITY(1,1) NOT NULL,
    [CDCodingType_Name] varchar(50)  NULL,
    [CDCodingType_Desc] varchar(50)  NULL,
    [CDCodingType_Status] int  NOT NULL,
    [CDCodingType_CreatedBy] int  NULL,
    [CDCodingType_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'CompanyBedConfigs'
CREATE TABLE [dbo].[CompanyBedConfigs] (
    [BedConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [Facility_Id] int  NULL,
    [Floor_Id] int  NULL,
    [NurseStation_Id] int  NULL,
    [Wing_Id] int  NULL,
    [Room_Id] int  NULL,
    [Bed_Id] int  NULL,
    [BedConfig_Status] int  NOT NULL,
    [BedConfig_CreatedBy] int  NULL,
    [BedConfig_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [Country_Id] int IDENTITY(1,1) NOT NULL,
    [Country_Name] varchar(100)  NULL,
    [Country_Code] varchar(10)  NULL,
    [Country_Status] int  NOT NULL,
    [Country_CreatedBy] int  NULL,
    [Country_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'DiagnosisCodingTypes'
CREATE TABLE [dbo].[DiagnosisCodingTypes] (
    [DCodingType_Id] int IDENTITY(1,1) NOT NULL,
    [DCodingType_Code] varchar(50)  NULL,
    [DCodingType_Desc] varchar(50)  NULL,
    [DCodingType_Status] int  NOT NULL,
    [DCodingType_CreatedBy] int  NULL,
    [DCodingType_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'DocFolders'
CREATE TABLE [dbo].[DocFolders] (
    [FolderID] int IDENTITY(1,1) NOT NULL,
    [FolderName] varchar(75)  NULL,
    [FolderParentID] int  NULL,
    [Folder_Status] int  NOT NULL,
    [Folder_CreatedBy] int  NULL,
    [Folder_CreatedDate] datetime  NOT NULL,
    [FolderLocation] varchar(500)  NULL
);
GO

-- Creating table 'EventCategories'
CREATE TABLE [dbo].[EventCategories] (
    [EventCat_Id] int IDENTITY(1,1) NOT NULL,
    [Category_Id] int  NULL,
    [EventCat_Code] varchar(50)  NULL,
    [EventCat_Desc] varchar(100)  NULL,
    [EventCat_ShortCode] varchar(10)  NULL,
    [EventCat_Type] int  NULL,
    [EventCat_Status] int  NOT NULL,
    [EventCat_CreatedBy] int  NULL,
    [EventCat_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'FileAckInformations'
CREATE TABLE [dbo].[FileAckInformations] (
    [FileAckInformation_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [File_Name] varchar(100)  NULL,
    [FileAck_Data] nvarchar(max)  NULL,
    [File_Id] int  NULL,
    [FileAck_Status] int  NOT NULL,
    [FileAck_CreatedBy] int  NULL,
    [File_CreatedDate] datetime  NOT NULL,
    [File_Status] int  NULL,
    [File_StatusSent] int  NULL,
    [PatientMrNumber] varchar(50)  NULL
);
GO

-- Creating table 'FileInformations'
CREATE TABLE [dbo].[FileInformations] (
    [File_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [File_Category] int  NOT NULL,
    [File_Name] varchar(100)  NULL,
    [File_Data] nvarchar(max)  NULL,
    [Event] varchar(50)  NULL,
    [File_Status] int  NOT NULL,
    [File_Error] varchar(50)  NULL,
    [File_CreatedDate] datetime  NOT NULL,
    [File_CreatedBy] int  NULL
);
GO

-- Creating table 'Floors'
CREATE TABLE [dbo].[Floors] (
    [Floor_Id] int IDENTITY(1,1) NOT NULL,
    [Floor_Name] varchar(50)  NULL,
    [Floor_Status] int  NOT NULL,
    [Floor_CreatedBy] int  NULL,
    [Floor_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'FrequencyMasters'
CREATE TABLE [dbo].[FrequencyMasters] (
    [Frequency_Id] int IDENTITY(1,1) NOT NULL,
    [Frequency_Code] varchar(50)  NULL,
    [Frequency_Shortname] varchar(50)  NULL,
    [Frequency_Description] varchar(100)  NULL,
    [Frequency_Status] int  NOT NULL,
    [Frequency_CreatedBy] int  NULL,
    [Frequency_CreatedDate] datetime  NOT NULL,
    [Frequency_PRN] int  NULL,
    [Freq_Group] varchar(50)  NULL,
    [Freq_Times] int  NULL,
    [Freq_Descalert] varchar(250)  NULL,
    [Freq_Descalert1] varchar(250)  NULL
);
GO

-- Creating table 'FTECategories'
CREATE TABLE [dbo].[FTECategories] (
    [FteCategory_Id] int IDENTITY(1,1) NOT NULL,
    [FteCategory_Desc] varchar(50)  NOT NULL,
    [FteCategory_Status] int  NOT NULL,
    [FteCategory_CreatedBy] int  NULL,
    [FteCategory_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'FteConnections'
CREATE TABLE [dbo].[FteConnections] (
    [FteConn_Id] int IDENTITY(1,1) NOT NULL,
    [FteConn_Desc] varchar(50)  NULL,
    [FteConn_Status] int  NOT NULL,
    [FteConn_CreatedBy] int  NULL,
    [FteConn_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Genders'
CREATE TABLE [dbo].[Genders] (
    [Gender_Id] int IDENTITY(1,1) NOT NULL,
    [Gender_Desc] varchar(15)  NULL,
    [Gender_Code] char(1)  NULL,
    [Gender_Status] int  NOT NULL,
    [Gender_CreatedBy] int  NULL,
    [Gender_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'HLSevenCompanyConfigs'
CREATE TABLE [dbo].[HLSevenCompanyConfigs] (
    [HLConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NOT NULL,
    [SegDetail_Id] int  NOT NULL,
    [SegDetailConfig_Id] int  NOT NULL,
    [SegDisplay_Id] int  NULL,
    [HLConfig_Status] int  NOT NULL,
    [HLConfig_CreatedBy] int  NULL,
    [HLConfig_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'HLSevenSegmentDetails'
CREATE TABLE [dbo].[HLSevenSegmentDetails] (
    [SegDetail_Id] int IDENTITY(1,1) NOT NULL,
    [Segment_Id] int  NOT NULL,
    [SegDetail_Desc] varchar(75)  NULL,
    [SegDetail_Length] int  NULL,
    [SegDetail_Sequence] int  NULL,
    [SegDetail_SubSequence] int  NULL,
    [SegDetail_Status] int  NOT NULL,
    [SegDetail_CreatedBy] int  NULL,
    [SegDetail_CreatedDate] datetime  NOT NULL,
    [SegDetail_ColumnID] varchar(50)  NULL,
    [File_Values] varchar(250)  NULL,
    [HlSequence] int  NULL
);
GO

-- Creating table 'HLSevenSegments'
CREATE TABLE [dbo].[HLSevenSegments] (
    [Segment_Id] int IDENTITY(1,1) NOT NULL,
    [Segment_Desc] varchar(50)  NULL,
    [Segment_Code] varchar(50)  NULL,
    [Segment_Table] varchar(50)  NULL,
    [Segment_Status] int  NOT NULL,
    [Segment_CreatedBy] int  NULL,
    [Segment_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Hours'
CREATE TABLE [dbo].[Hours] (
    [Hour_Id] int IDENTITY(1,1) NOT NULL,
    [Hour_Desc] varchar(5)  NULL,
    [Hour_Status] int  NOT NULL,
    [Hour_CreatedBy] int  NULL,
    [Hour_Createddate] datetime  NOT NULL,
    [RegularTime] varchar(5)  NULL,
    [RegularTimeFormat] varchar(2)  NULL
);
GO

-- Creating table 'ICD10'
CREATE TABLE [dbo].[ICD10] (
    [ICD10_Id] int IDENTITY(1,1) NOT NULL,
    [ICD10_RawFormat] varchar(50)  NOT NULL,
    [ICD10_Formatted] varchar(50)  NOT NULL,
    [ICD10_Description] varchar(1000)  NULL,
    [ICD10_Status] int  NOT NULL,
    [ICD10_CreatedBy] int  NULL,
    [ICD10_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'ImportFiles'
CREATE TABLE [dbo].[ImportFiles] (
    [ImportFile_Id] bigint IDENTITY(1,1) NOT NULL,
    [ImportFile_Name] varchar(150)  NULL,
    [EncryptedDate] datetime  NULL,
    [DecryptedDate] datetime  NULL,
    [ImportFile_Status] int  NULL,
    [ImportFile_CreatedBy] int  NULL,
    [ImportFile_CreatedDate] datetime  NULL
);
GO

-- Creating table 'IntegrationTypes'
CREATE TABLE [dbo].[IntegrationTypes] (
    [Integration_TypeId] int IDENTITY(1,1) NOT NULL,
    [IntegrationType_Desc] varchar(50)  NULL,
    [IntegrationType_Status] int  NOT NULL,
    [IntegrationType_CreatedBy] int  NULL,
    [IntegrationType_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'MaritalStatus'
CREATE TABLE [dbo].[MaritalStatus] (
    [Marital_id] int IDENTITY(1,1) NOT NULL,
    [Marital_Desc] varchar(50)  NULL,
    [Marital_Code] varchar(10)  NULL,
    [Marital_Status] int  NOT NULL,
    [Marital_CreatedBy] int  NULL,
    [Marital_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'MedicationReasons'
CREATE TABLE [dbo].[MedicationReasons] (
    [MedicationReason_ID] int IDENTITY(1,1) NOT NULL,
    [MedicationReason_Desc] varchar(250)  NULL,
    [MedicationReason_Status] int  NOT NULL,
    [MedicationReason_CreatedBy] int  NULL,
    [MedicationReason_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'Months'
CREATE TABLE [dbo].[Months] (
    [Month_Id] int IDENTITY(1,1) NOT NULL,
    [Month_Name] varchar(50)  NULL,
    [Month_Status] int  NOT NULL,
    [Month_CreatedBy] int  NULL,
    [Month_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'NursingSchedules'
CREATE TABLE [dbo].[NursingSchedules] (
    [NursingSchedule_Id] int IDENTITY(1,1) NOT NULL,
    [NursingStation_Id] int  NOT NULL,
    [ScheduleTime] varchar(50)  NULL,
    [NursingSchedule_Status] int  NOT NULL,
    [NursingSchedule_CreatedBy] int  NULL,
    [NursingSchedule_CreatedDate] datetime  NULL
);
GO

-- Creating table 'NursingStations'
CREATE TABLE [dbo].[NursingStations] (
    [NurseStation_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Code] varchar(50)  NULL,
    [NurseStation_Name] varchar(75)  NULL,
    [NurseStation_Status] int  NOT NULL,
    [NurseStation_CreatedBy] int  NULL,
    [NurseStation_CreatedDate] datetime  NOT NULL,
    [Facility_Id] int  NULL,
    [DefaultPhysician_Id] int  NULL
);
GO

-- Creating table 'OrderControlMasters'
CREATE TABLE [dbo].[OrderControlMasters] (
    [OrderControl_ID] int IDENTITY(1,1) NOT NULL,
    [OrderControlValue] varchar(5)  NULL,
    [OrderControlDesc] varchar(50)  NULL,
    [OrderControlStatus] int  NULL,
    [OrderControlCretedBy] int  NULL,
    [OrderControlCreatedOn] datetime  NULL
);
GO

-- Creating table 'OrderFavouriteMasters'
CREATE TABLE [dbo].[OrderFavouriteMasters] (
    [OrderFavMaster_ID] int IDENTITY(1,1) NOT NULL,
    [OrderFavDesc] varchar(100)  NULL,
    [OrderFavMaster_status] int  NOT NULL,
    [OrderFavMaster_CreatedBy] int  NULL,
    [OrderFavMaster_CreatedOn] datetime  NOT NULL,
    [OrderFavCode] varchar(10)  NULL
);
GO

-- Creating table 'OrderTypes'
CREATE TABLE [dbo].[OrderTypes] (
    [OrderTypeID] int IDENTITY(1,1) NOT NULL,
    [OrderType_Desc] varchar(50)  NULL,
    [OrderType_Status] int  NOT NULL,
    [OrderType_CreatedBy] int  NULL,
    [OrderType_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'RoleConfigs'
CREATE TABLE [dbo].[RoleConfigs] (
    [RoleConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Role_Id] int  NOT NULL,
    [Screen_Id] int  NULL,
    [AccessRead] int  NULL,
    [AccessWrite] int  NULL,
    [PrintPdf] int  NULL,
    [PrintExcel] int  NULL,
    [RoleConfig_Status] int  NOT NULL,
    [RoleConfig_CreatedBy] int  NULL,
    [RoleConfig_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Room_Id] int IDENTITY(1,1) NOT NULL,
    [Room_Name] varchar(50)  NULL,
    [Room_Code] varchar(20)  NULL,
    [Room_Status] int  NOT NULL,
    [Room_CreatedBy] int  NULL,
    [Room_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Screens'
CREATE TABLE [dbo].[Screens] (
    [Screen_Id] int IDENTITY(1,1) NOT NULL,
    [Screen_Desc] varchar(50)  NULL,
    [Screen_Order] int  NULL,
    [Screen_Status] int  NOT NULL,
    [Screen_CreatedBy] int  NULL,
    [Screen_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Suffixes'
CREATE TABLE [dbo].[Suffixes] (
    [Suffix_Id] int IDENTITY(1,1) NOT NULL,
    [Suffix_Desc] varchar(50)  NULL,
    [Suffix_Status] int  NOT NULL,
    [Suffix_CreatedBy] int  NULL,
    [Suffix_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'TimeFormats'
CREATE TABLE [dbo].[TimeFormats] (
    [TimeFormat_Id] int IDENTITY(1,1) NOT NULL,
    [TimeFormat_Desc] varchar(5)  NULL,
    [TimeFormat_Status] int  NOT NULL,
    [TimeFormat_CreatedBy] int  NULL,
    [TimeFormat_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [User_Id] int IDENTITY(1,1) NOT NULL,
    [User_Suffix] varchar(5)  NULL,
    [User_Fname] varchar(50)  NULL,
    [User_Lname] varchar(50)  NULL,
    [User_Mname] varchar(50)  NULL,
    [UserName] varchar(50)  NULL,
    [Password] varchar(50)  NULL,
    [User_Gender] int  NULL,
    [User_MaritalStatus] int  NULL,
    [User_DisplayName] varchar(50)  NULL,
    [User_Email] varchar(50)  NULL,
    [User_Phone] varchar(20)  NULL,
    [User_PwdCount] int  NOT NULL,
    [User_Lock] int  NOT NULL,
    [User_Status] int  NOT NULL,
    [User_CreatedBy] int  NULL,
    [User_CreatedDate] datetime  NOT NULL,
    [User_Token] varchar(max)  NULL,
    [StkReportReq] int  NULL,
    [NewUserFlag] int  NULL,
    [ProcessKey] int  NULL,
    [PhysicianNPI] varchar(10)  NULL,
    [PastDueAlertFlag] int  NULL,
    [User_ShortName] varchar(10)  NULL
);
GO

-- Creating table 'UserRoleFacilityConfigs'
CREATE TABLE [dbo].[UserRoleFacilityConfigs] (
    [UserRole_Id] int IDENTITY(1,1) NOT NULL,
    [Role_ID] int  NOT NULL,
    [User_Id] int  NOT NULL,
    [Facility_id] int  NOT NULL,
    [UserRole_Status] int  NOT NULL,
    [UserRole_CreatedBy] int  NULL,
    [UserRole_CreatedDate] datetime  NOT NULL,
    [NurseStation_Id] int  NULL
);
GO

-- Creating table 'WeekDays'
CREATE TABLE [dbo].[WeekDays] (
    [Weekdays_ID] int IDENTITY(1,1) NOT NULL,
    [Weekdays_Desc] varchar(50)  NULL,
    [Weekdays_Status] int  NOT NULL,
    [Weekdays_CreatedBy] int  NULL,
    [Weekdays_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'Weeks'
CREATE TABLE [dbo].[Weeks] (
    [Week_Id] int IDENTITY(1,1) NOT NULL,
    [Week_Desc] varchar(50)  NULL,
    [Week_Status] int  NOT NULL,
    [Week_CreatedBy] int  NULL,
    [Week_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Wings'
CREATE TABLE [dbo].[Wings] (
    [Wing_Id] int IDENTITY(1,1) NOT NULL,
    [Wing_Desc] varchar(10)  NOT NULL,
    [Wing_Status] int  NOT NULL,
    [Wing_CreatedBy] int  NOT NULL,
    [Wing_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'AddlInstructionDetails'
CREATE TABLE [dbo].[AddlInstructionDetails] (
    [PAddl_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [PackageType] varchar(10)  NULL,
    [NumberOfLabels] varchar(5)  NULL,
    [LabelQuantity] varchar(250)  NULL,
    [Zone] varchar(10)  NULL,
    [Bin] varchar(10)  NULL,
    [TotalQuantityWritten] varchar(18)  NULL,
    [TotalQuantityDispensed] varchar(18)  NULL,
    [FillQuantityWritten] varchar(18)  NULL,
    [MaxDailyQuantity] varchar(18)  NULL,
    [DaysSupply] varchar(5)  NULL,
    [TimesPerDay] varchar(5)  NULL,
    [DateWritten] datetime  NULL,
    [PrePack] char(1)  NULL,
    [CycleFill] char(1)  NULL,
    [MARGroupLevel] char(1)  NULL,
    [MARGroup] varchar(10)  NULL,
    [PartialStatus] char(1)  NULL,
    [IntendedQuantity] varchar(9)  NULL,
    [IntendedDaysSupply] varchar(5)  NULL,
    [OriginCode] char(1)  NULL,
    [RxGuid] varchar(50)  NULL,
    [ToteId] varchar(10)  NULL,
    [CustomFieldID] varchar(80)  NULL,
    [CustomFieldValue] varchar(80)  NULL,
    [CustomFieldName] varchar(90)  NULL,
    [ACustomFieldID] varchar(80)  NULL,
    [ACustomFieldValue] varchar(80)  NULL,
    [ACustomFieldName] varchar(80)  NULL,
    [RxType] varchar(max)  NULL,
    [LinkedReorderNumber] varchar(10)  NULL,
    [ExtraDoseIndicator] char(1)  NULL,
    [LeaveOfAbsenceIndicator] char(1)  NULL,
    [DeliveryID] varchar(10)  NULL,
    [PartialTabletIndicator] char(1)  NULL,
    [RawAdministrationTimes] varchar(max)  NULL,
    [ProductType] char(1)  NULL,
    [Treatment] char(1)  NULL,
    [PhRxExpireDate] datetime  NULL,
    [RxNumber] varchar(20)  NULL,
    [PAddl_Status] int  NOT NULL,
    [PAddl_CreatedBy] int  NULL,
    [PAddl_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'AncillaryDetails'
CREATE TABLE [dbo].[AncillaryDetails] (
    [PAnc_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [LiteralOrderCode] varchar(250)  NULL,
    [LiteralOrderDesc] varchar(250)  NULL,
    [MAROrderCatSeq] varchar(16)  NULL,
    [POOrderCatSeq] varchar(16)  NULL,
    [TAROrderCatSeq] varchar(16)  NULL,
    [Filler] char(1)  NULL,
    [IncludeOnMAR] char(1)  NULL,
    [IncludeOnPO] char(1)  NULL,
    [IncludeOnTAR] char(1)  NULL,
    [RawAdministrationTimes] varchar(250)  NULL,
    [PAnc_Status] int  NOT NULL,
    [PAnc_CreatedBy] int  NULL,
    [PAnc_CreatedDate] datetime  NOT NULL,
    [PAncOutBoundFileStatus] int  NULL,
    [PAncOutBoundApproval] int  NULL,
    [PAncOutBoundApprovalBy] int  NULL,
    [PAncOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'CompoundOrders'
CREATE TABLE [dbo].[CompoundOrders] (
    [PComp_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [ComponentType] char(1)  NULL,
    [ComponentId] varchar(50)  NULL,
    [DrugName] varchar(50)  NULL,
    [CodingSystem] varchar(50)  NULL,
    [CAlternateId] varchar(50)  NULL,
    [CAlternateText] varchar(50)  NULL,
    [CACodingSystem] varchar(50)  NULL,
    [ComponentAmount] varchar(20)  NULL,
    [UnitsId] varchar(50)  NULL,
    [UnitsText] varchar(50)  NULL,
    [UCodingSystem] varchar(50)  NULL,
    [UAlternateId] varchar(50)  NULL,
    [UAlternateText] varchar(50)  NULL,
    [UACodingSystem] varchar(50)  NULL,
    [ComponentStrength] varchar(250)  NULL,
    [CSUnitsId] varchar(50)  NULL,
    [CSUnitsText] varchar(50)  NULL,
    [CSUCodingSystem] varchar(50)  NULL,
    [CSUAlternateId] varchar(50)  NULL,
    [CSUAlternateText] varchar(50)  NULL,
    [CSUACodingSystem] varchar(50)  NULL,
    [SCodeId] varchar(50)  NULL,
    [SCodeText] varchar(50)  NULL,
    [SCCodingSystem] varchar(50)  NULL,
    [SCAlternateId] varchar(50)  NULL,
    [SCAlternateText] varchar(50)  NULL,
    [SCACodingSystem] varchar(50)  NULL,
    [DrugStrengthVolume] varchar(5)  NULL,
    [CDSUnitsId] varchar(50)  NULL,
    [CDSUnitsText] varchar(50)  NULL,
    [CDSUnitsCodingSystem] varchar(50)  NULL,
    [CDSVAlternateId] varchar(50)  NULL,
    [CDSVAlternateText] varchar(50)  NULL,
    [CDSVACodingSystem] varchar(50)  NULL,
    [CodingSystemVersion] varchar(50)  NULL,
    [ACodingSystemVersion] varchar(50)  NULL,
    [OriginalText] varchar(500)  NULL,
    [PComp_Status] int  NOT NULL,
    [PComp_CreatedBy] int  NULL,
    [PComp_CreatedDate] datetime  NOT NULL,
    [PCOutBoundFileStatus] int  NULL,
    [PCOutBoundApproval] int  NULL,
    [PCOutBoundApprovalBy] int  NULL,
    [PCOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'EncodedOrderDetails'
CREATE TABLE [dbo].[EncodedOrderDetails] (
    [PEncOrder_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NULL,
    [Quantity] varchar(200)  NULL,
    [GiveCodeIdentifier] varchar(50)  NULL,
    [GiveCodeText] varchar(60)  NULL,
    [AGiveCodeIdentifier] varchar(50)  NULL,
    [GiveAmountMin] varchar(20)  NULL,
    [GiveAmountMax] varchar(20)  NULL,
    [GiveUnits] varchar(250)  NULL,
    [GiveDosageForm] varchar(250)  NULL,
    [ProviderAdminDrugIdentifier] varchar(250)  NULL,
    [ProviderAdminDrugInsText] nvarchar(max)  NULL,
    [DeliverToLocation] varchar(200)  NULL,
    [SubstitutionStatus] char(1)  NULL,
    [DispenseAmount] varchar(20)  NULL,
    [DispenseUnits] varchar(250)  NULL,
    [NumberOfRefills] varchar(3)  NULL,
    [PhysicianDEANumber] varchar(80)  NULL,
    [PhysicianLastName] varchar(50)  NULL,
    [PhysicianFirstName] varchar(50)  NULL,
    [TreatmentSupplierVerifierID] varchar(250)  NULL,
    [PrescriptionNumber] varchar(20)  NULL,
    [NumberOfRefillsRemaining] varchar(20)  NULL,
    [NumberOfRefillsDispensed] varchar(20)  NULL,
    [RecentRefillDate] datetime  NULL,
    [TotalDailyDose] varchar(10)  NULL,
    [NeedsHumanReview] char(1)  NULL,
    [SpecialDispensingInstruction] varchar(250)  NULL,
    [GivePer] varchar(20)  NULL,
    [GiveRateAmount] varchar(6)  NULL,
    [GiveRateUnits] varchar(250)  NULL,
    [GiveStrength] varchar(20)  NULL,
    [GiveStrengthUnits] varchar(250)  NULL,
    [GiveIndication] varchar(250)  NULL,
    [DispensePackageSize] varchar(20)  NULL,
    [DispensePackageSizeUnit] varchar(250)  NULL,
    [DispensePackageMethod] varchar(2)  NULL,
    [SupplementaryCode] varchar(250)  NULL,
    [OriginalOrderDate] datetime  NULL,
    [GiveDrugStrengthVolume] varchar(5)  NULL,
    [GiveDrugStrengthVolUnits] varchar(250)  NULL,
    [ControlledSubstanceSchedule] varchar(60)  NULL,
    [FormularyStatus] char(1)  NULL,
    [PharmaceuticalSubstance] varchar(60)  NULL,
    [PharmacyRecentFill] varchar(250)  NULL,
    [InitialDispenseAmount] varchar(250)  NULL,
    [DispensingPharmacyID] varchar(100)  NULL,
    [DispensingPharmacyName] varchar(150)  NULL,
    [DispensingPharmacyAddr1] varchar(80)  NULL,
    [DispensingPharmacyAddr2] varchar(80)  NULL,
    [DispensingPharmacyCity] varchar(50)  NULL,
    [DispensingPharmacyState] varchar(50)  NULL,
    [DispensingPharmacyZip] varchar(50)  NULL,
    [DeliverToPatientLocation] varchar(80)  NULL,
    [DeliverToAddress] varchar(250)  NULL,
    [PharmacyOrderType] char(1)  NULL,
    [PEncOrder_Status] int  NOT NULL,
    [PEncOrder_CreatedBy] int  NULL,
    [PEncOrder_CreatedDate] datetime  NOT NULL,
    [PEOutBoundFileStatus] int  NULL,
    [PEOutBoundApproval] int  NULL,
    [PEOutBoundApprovalBy] int  NULL,
    [PEOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'InsuranceInfoes'
CREATE TABLE [dbo].[InsuranceInfoes] (
    [PatIns_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NULL,
    [InsIdentifier] varchar(80)  NULL,
    [InsPlanName] varchar(80)  NULL,
    [CodingSystemName] varchar(50)  NULL,
    [InsCompanyID] varchar(250)  NULL,
    [InsCompanyName] varchar(250)  NULL,
    [InsCompanyAddress1] varchar(100)  NULL,
    [InsCompanyAddress2] varchar(100)  NULL,
    [InsCompanyCity] varchar(50)  NULL,
    [InsCompanyState] varchar(50)  NULL,
    [InsCompanyZipCode] varchar(20)  NULL,
    [InsContactPerson] varchar(250)  NULL,
    [InsPhoneNumber] varchar(250)  NULL,
    [InsGroupNumber] varchar(20)  NULL,
    [InsGroupName] varchar(250)  NULL,
    [InsGroupEmpId] varchar(250)  NULL,
    [InsGroupEmpName] varchar(250)  NULL,
    [PlanEffDate] datetime  NULL,
    [PlanExpDate] datetime  NULL,
    [AuthorizationInfo] varchar(239)  NULL,
    [PlanType] varchar(3)  NULL,
    [FamilyName] varchar(80)  NULL,
    [GivenName] varchar(80)  NULL,
    [InsRelationIdentifier] varchar(80)  NULL,
    [InsRelationText] varchar(50)  NULL,
    [InsuredDob] datetime  NULL,
    [InsuredAddress] varchar(250)  NULL,
    [BenefitAssignment] varchar(2)  NULL,
    [BenefitCoordination] varchar(2)  NULL,
    [BenefitCoordinationPriority] varchar(2)  NULL,
    [AdmissionFlag] varchar(50)  NULL,
    [AdmissionDate] datetime  NULL,
    [EligibilityFlag] varchar(50)  NULL,
    [EligibilityDate] datetime  NULL,
    [ReleaseInfoCode] varchar(26)  NULL,
    [PAC] varchar(15)  NULL,
    [VerificationDate] datetime  NULL,
    [VerificationBy] varchar(250)  NULL,
    [AgreementCode] varchar(2)  NULL,
    [BillingStatus] varchar(2)  NULL,
    [ReserveDays] int  NULL,
    [DelayReserveDays] int  NULL,
    [CompanyPlanCode] varchar(8)  NULL,
    [PolicyNumber] varchar(15)  NULL,
    [PolicyDeductibles] varchar(26)  NULL,
    [PolicyLimitAmount] varchar(26)  NULL,
    [PolicyLimitDays] int  NULL,
    [RoomRateSemiPrivate] varchar(26)  NULL,
    [RoomRatePrivate] varchar(26)  NULL,
    [InsuredEmpStatus] varchar(250)  NULL,
    [InsuredSex] varchar(50)  NULL,
    [InsuredEmpAddress] varchar(250)  NULL,
    [VerificationStatus] varchar(2)  NULL,
    [PriorInsPlanId] varchar(8)  NULL,
    [CoverageType] varchar(3)  NULL,
    [Handicap] varchar(2)  NULL,
    [InsuredIdNumber] varchar(250)  NULL,
    [SignatureCode] varchar(50)  NULL,
    [SignatureCodeDate] datetime  NULL,
    [InsuredBirthPlace] varchar(250)  NULL,
    [VipIndicator] varchar(2)  NULL,
    [PatIns_Status] int  NOT NULL,
    [PatIns_CreatedBy] int  NULL,
    [PatIns_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'NotesInfoes'
CREATE TABLE [dbo].[NotesInfoes] (
    [PNote_id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [SourceOfComment] varchar(1)  NULL,
    [Comment] varchar(max)  NULL,
    [CommentType] varchar(250)  NULL,
    [PNote_Status] int  NOT NULL,
    [PNote_CreatedBy] int  NULL,
    [PNote_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Observations'
CREATE TABLE [dbo].[Observations] (
    [PObs_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [ValueType] varchar(50)  NULL,
    [ObservationId] varchar(50)  NULL,
    [ObservationText] varchar(50)  NULL,
    [ObsCodingSystem] varchar(50)  NULL,
    [OAlternateId] varchar(50)  NULL,
    [OAlternateText] varchar(50)  NULL,
    [OACodingSystem] varchar(50)  NULL,
    [ObservationSubId] varchar(50)  NULL,
    [ObservationValue] varchar(50)  NULL,
    [UnitsId] varchar(50)  NULL,
    [UnitText] varchar(50)  NULL,
    [UnitCodingSystem] varchar(50)  NULL,
    [UAlternateId] varchar(50)  NULL,
    [UAlternateText] varchar(50)  NULL,
    [UACodingSystem] varchar(50)  NULL,
    [ReferenceRange] varchar(50)  NULL,
    [AbnormalFlags] varchar(50)  NULL,
    [Probability] varchar(50)  NULL,
    [NatureOfAbnormalTest] varchar(50)  NULL,
    [ObservationResultStatus] varchar(50)  NULL,
    [ReferenceDate] datetime  NULL,
    [AccessChecks] varchar(50)  NULL,
    [ObservationDate] datetime  NULL,
    [ProducerID] varchar(50)  NULL,
    [ProducerText] varchar(50)  NULL,
    [ProducerCodingSystem] varchar(50)  NULL,
    [PAlternateId] varchar(50)  NULL,
    [PAlternateText] varchar(50)  NULL,
    [PACodingSystem] varchar(50)  NULL,
    [ResponsibleObserver] varchar(50)  NULL,
    [ObservationMethod] varchar(50)  NULL,
    [EquipmentInstance] varchar(50)  NULL,
    [AnalysisDate] datetime  NULL,
    [PObs_Status] int  NOT NULL,
    [PObs_CreatedBy] int  NULL,
    [PObs_CreatedDate] datetime  NULL
);
GO

-- Creating table 'OutBoundFileInformations'
CREATE TABLE [dbo].[OutBoundFileInformations] (
    [File_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [File_Category] int  NOT NULL,
    [File_Name] varchar(100)  NULL,
    [File_Data] varchar(max)  NULL,
    [Event] varchar(50)  NULL,
    [File_Status] int  NOT NULL,
    [File_Error] varchar(50)  NULL,
    [File_CreatedDate] datetime  NOT NULL,
    [File_CreatedBy] int  NULL,
    [File_Acknowledge] nvarchar(500)  NULL,
    [File_AcknowledgeDate] datetime  NULL,
    [Patient_Id] int  NULL,
    [Porder_Id] int  NULL,
    [MailStatus] int  NULL,
    [MessageId] varchar(20)  NULL
);
GO

-- Creating table 'ResidentOrders'
CREATE TABLE [dbo].[ResidentOrders] (
    [ResOrder_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_ID] int  NOT NULL,
    [ResOrderType] varchar(50)  NULL,
    [ResOrderDate] datetime  NULL,
    [ResPhysician] varchar(150)  NULL,
    [ResOrderText] varchar(250)  NULL,
    [ResOrder_Status] int  NOT NULL,
    [ResOrder_CreatedBy] int  NULL,
    [ResOrder_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'TreatmentInfoes'
CREATE TABLE [dbo].[TreatmentInfoes] (
    [PTreatment_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [ReqGiveCodeIdentifier] varchar(125)  NULL,
    [RequestedGiveCode] varchar(125)  NULL,
    [RequestedGiveAmtMin] varchar(20)  NULL,
    [RequestedGiveAmtMax] varchar(20)  NULL,
    [RequestedGiveUnits] varchar(250)  NULL,
    [RequestedDosageForm] varchar(250)  NULL,
    [ProvidersTreatmentInstructions] varchar(250)  NULL,
    [ProvidersAdministrationInstructions] varchar(250)  NULL,
    [DeliverToLocation] varchar(200)  NULL,
    [AllowSubstitutions] varchar(1)  NULL,
    [RequestedDispenseCode] varchar(250)  NULL,
    [RequestedDispenseAmount] varchar(20)  NULL,
    [RequestedDispenseUnits] varchar(250)  NULL,
    [NumberOfRefills] varchar(3)  NULL,
    [OrderingProviderDEANumber] varchar(250)  NULL,
    [TreatmentSupplierVerifierID] varchar(250)  NULL,
    [NeedsHumanReview] varchar(1)  NULL,
    [RequestedGivePer] varchar(20)  NULL,
    [RequestedGiveStrength] varchar(20)  NULL,
    [RequestedGiveStrengthUnits] varchar(250)  NULL,
    [IndicationIdentifier] varchar(10)  NULL,
    [IndicationText] varchar(150)  NULL,
    [IndicationCodingSystem] varchar(10)  NULL,
    [AIndicationIdentifier] varchar(10)  NULL,
    [AIndicationText] varchar(150)  NULL,
    [AIndicationCodingSystem] varchar(50)  NULL,
    [RequestedGiveRateAmount] varchar(6)  NULL,
    [RequestedGiveRateUnits] varchar(250)  NULL,
    [TotalDailyDose] varchar(10)  NULL,
    [SupplementaryCode] varchar(250)  NULL,
    [RequestedDrugStrengthVol] varchar(5)  NULL,
    [RequestedDrugStrengthVolUnits] varchar(250)  NULL,
    [PharmacyOrderType] varchar(5)  NULL,
    [DispensingInterval] varchar(20)  NULL,
    [PTreatment_Status] int  NOT NULL,
    [PTreatment_CreatedBy] int  NULL,
    [PTreatment_CreatedDate] datetime  NOT NULL,
    [PTIOutBoundFileStatus] int  NULL,
    [PTIOutBoundApproval] int  NULL,
    [PTIOutBoundApprovalBy] int  NULL,
    [PTIOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'VisitBehaviours'
CREATE TABLE [dbo].[VisitBehaviours] (
    [VisitBehaviour_ID] bigint IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NOT NULL,
    [HallucinationsID] int  NULL,
    [DelusionsID] int  NULL,
    [Physicalbehavioral] int  NULL,
    [Verbalbehavioral] int  NULL,
    [Otherbehavioral] int  NULL,
    [rejectevaluation] int  NULL,
    [Resisdentwandered] int  NULL,
    [Comments] nvarchar(4000)  NULL,
    [Initials] varchar(50)  NULL,
    [Date] datetime  NULL,
    [WeeklyStatus] int  NULL,
    [VisitBehaviour_Status] int  NOT NULL,
    [VisitBehaviour_CreatedBy] int  NULL,
    [VisitBehaviour_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'VisitFoodintakes'
CREATE TABLE [dbo].[VisitFoodintakes] (
    [VisitFoodintake_ID] bigint IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NOT NULL,
    [AssessmentDate] datetime  NULL,
    [Initials] varchar(50)  NULL,
    [Attendingphysician] varchar(50)  NULL,
    [Fluidsb] varchar(50)  NULL,
    [Alternateb] varchar(50)  NULL,
    [supplementb] varchar(50)  NULL,
    [Fluidsl] varchar(50)  NULL,
    [Alternatel] varchar(50)  NULL,
    [supplementl] varchar(50)  NULL,
    [Fluidss] varchar(50)  NULL,
    [Alternates] varchar(50)  NULL,
    [supplements] varchar(50)  NULL,
    [VisitFoodintake_Status] int  NOT NULL,
    [VisitFoodintake_CreatedBy] int  NULL,
    [VisitFoodintake_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'VisitInfoes'
CREATE TABLE [dbo].[VisitInfoes] (
    [PVisit_Id] bigint IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [PatientClass] varchar(1)  NULL,
    [NursingStationId] int  NULL,
    [Room] varchar(50)  NULL,
    [Bed] varchar(50)  NULL,
    [FacilityId] int  NULL,
    [Floor] varchar(50)  NULL,
    [AdmissionType] varchar(50)  NULL,
    [PreAdmitNumber] varchar(250)  NULL,
    [PriorNursingStationId] int  NULL,
    [PriorRoom] varchar(50)  NULL,
    [PriorBed] varchar(50)  NULL,
    [PriorFacilityId] int  NULL,
    [PriorFloor] varchar(50)  NULL,
    [Physician_Id] int  NULL,
    [PrimaryPhysicianNPI] varchar(10)  NULL,
    [PrimaryPhysicianLName] varchar(75)  NULL,
    [PrimaryPhysicianFName] varchar(75)  NULL,
    [ReferringDoctor] varchar(250)  NULL,
    [ConsultingDoctor] varchar(250)  NULL,
    [HospitalService] varchar(50)  NULL,
    [TemporaryLocation] varchar(80)  NULL,
    [PreAdmitTestIndicator] varchar(50)  NULL,
    [ReAdmissionIndicator] varchar(5)  NULL,
    [AdmitSource] varchar(6)  NULL,
    [AmbulatoryStatus] varchar(2)  NULL,
    [VIPIndicator] varchar(2)  NULL,
    [AdmittingDoctor] varchar(250)  NULL,
    [PatientType] varchar(2)  NULL,
    [VisitNumber] varchar(250)  NULL,
    [FinancialClass] varchar(50)  NULL,
    [ChargePriceIndicator] varchar(2)  NULL,
    [CourtesyCode] varchar(2)  NULL,
    [CreditRating] varchar(2)  NULL,
    [ContractCode] varchar(2)  NULL,
    [ContractEffDate] datetime  NULL,
    [ContractAmount] varchar(20)  NULL,
    [ContractPeriod] varchar(20)  NULL,
    [InterestCode] varchar(2)  NULL,
    [BadDebtCode] varchar(4)  NULL,
    [BadDebtDate] datetime  NULL,
    [BadDebtAgencyCode] varchar(10)  NULL,
    [BadDebtTransferAmt] varchar(20)  NULL,
    [BadDebtRecoveryAmt] varchar(20)  NULL,
    [DeleteAccIndicator] varchar(1)  NULL,
    [DeleteAccDate] datetime  NULL,
    [DischargeDisposition] varchar(3)  NULL,
    [DischargedLocation] varchar(47)  NULL,
    [DietType] varchar(250)  NULL,
    [ServicingFacility] varchar(2)  NULL,
    [BedStatus] varchar(1)  NULL,
    [AccStatus] varchar(5)  NULL,
    [PendingLocation] varchar(80)  NULL,
    [PriorTemporaryLocation] varchar(80)  NULL,
    [AdmitDate] datetime  NULL,
    [DischargeDate] datetime  NULL,
    [CurrentPatientBalance] varchar(20)  NULL,
    [TotalCharges] varchar(20)  NULL,
    [TotalAdjustments] varchar(20)  NULL,
    [TotalPayments] varchar(20)  NULL,
    [AlternateVisitId] varchar(250)  NULL,
    [VisitIndicator] varchar(5)  NULL,
    [OtherHealthProvider] varchar(250)  NULL,
    [Wing] int  NULL,
    [PVisit_Status] int  NOT NULL,
    [PVisit_CreatedBy] int  NULL,
    [PVisit_CreatedDate] datetime  NOT NULL,
    [PVOutBoundFileStatus] int  NULL,
    [PVOutBoundApproval] int  NULL,
    [PVOutBoundApprovalBy] int  NULL,
    [PVOutBoundApprovalOn] datetime  NULL,
    [File_Id] int  NULL,
    [HomeFlag] int  NULL
);
GO

-- Creating table 'VisitNursingNotes'
CREATE TABLE [dbo].[VisitNursingNotes] (
    [VisitNursingNotes_ID] bigint IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NOT NULL,
    [NoteDate] datetime  NULL,
    [NoteTime] varchar(10)  NULL,
    [NurseName] varchar(50)  NULL,
    [Notes] varchar(500)  NOT NULL,
    [PDAID] varchar(15)  NULL,
    [VisitNursingNotes_Status] int  NOT NULL,
    [VisitNursingNotes_CreatedBy] int  NULL,
    [VisitNursingNotes_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'VisitVitals'
CREATE TABLE [dbo].[VisitVitals] (
    [Vitals_ID] bigint IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NOT NULL,
    [VitalDate] datetime  NULL,
    [VitalTime] varchar(10)  NULL,
    [CistolicBP] varchar(50)  NULL,
    [DiastolicBP] int  NULL,
    [HeartRate] int  NULL,
    [RespiratoryRate] int  NULL,
    [OxygenRate] int  NULL,
    [Temperature] varchar(10)  NULL,
    [Pain] varchar(500)  NULL,
    [Remark] varchar(8000)  NULL,
    [PulseRate] int  NULL,
    [Initials] varchar(15)  NULL,
    [BloodSugar] varchar(100)  NULL,
    [PDAID] varchar(15)  NULL,
    [VitalSigns_status] int  NOT NULL,
    [VitalSigns_CreatedBy] int  NULL,
    [VitalSigns_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'AllergyInfoes'
CREATE TABLE [dbo].[AllergyInfoes] (
    [PAllergy_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [AllergyType_Id] int  NULL,
    [ClassDrugType] int  NULL,
    [ClassDrug_Id] int  NULL,
    [ClassDrug_Name] varchar(100)  NULL,
    [NameOfCoding] varchar(50)  NULL,
    [AllergySeverityCode] varchar(50)  NULL,
    [AllergyReactionCode] varchar(50)  NULL,
    [AllergyIdentificationDate] datetime  NULL,
    [PAllergy_Status] int  NOT NULL,
    [PAllergy_CreatedBy] int  NULL,
    [PAllergy_CreatedDate] datetime  NOT NULL,
    [PAOutBoundFileStatus] int  NULL,
    [PAOutBoundApproval] int  NULL,
    [PAOutBoundApprovalBy] int  NULL,
    [PAOutBoundApprovalOn] datetime  NULL,
    [OldPatient_Id] int  NULL
);
GO

-- Creating table 'DiagnosisInfoes'
CREATE TABLE [dbo].[DiagnosisInfoes] (
    [PDiagnosis_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [CodingMethod] varchar(50)  NULL,
    [ICD10_Id] int  NULL,
    [DCodingType_Id] int  NULL,
    [AltCodingId] varchar(10)  NULL,
    [AltCodingText] varchar(150)  NULL,
    [AltCodingMethod] varchar(5)  NULL,
    [DiagnosisDescription] varchar(100)  NULL,
    [DiagnosisDate] datetime  NULL,
    [DiagnosisType] varchar(2)  NULL,
    [MajorDiagnosticId] varchar(50)  NULL,
    [MajorDiagnosticText] varchar(50)  NULL,
    [MDCodingSystem] varchar(50)  NULL,
    [MDAlternateId] varchar(50)  NULL,
    [MDAlternateText] varchar(50)  NULL,
    [MDAltCodingSystem] varchar(50)  NULL,
    [DiagnosticGroupId] varchar(50)  NULL,
    [DiagnosticGroupText] varchar(50)  NULL,
    [DGCodingSystem] varchar(50)  NULL,
    [DGAlternateId] varchar(50)  NULL,
    [DGAlternateText] varchar(50)  NULL,
    [DGAltCodingSystem] varchar(50)  NULL,
    [DRGApprovalIndicator] varchar(1)  NULL,
    [DRGGrouperReviewCode] varchar(2)  NULL,
    [OutlierId] varchar(50)  NULL,
    [OutlierText] varchar(50)  NULL,
    [OutlierCodingSystem] varchar(50)  NULL,
    [OAlternateId] varchar(50)  NULL,
    [OAlternateText] varchar(50)  NULL,
    [OAltCodingSystem] varchar(50)  NULL,
    [OutlierDays] varchar(50)  NULL,
    [OutlierQuantity] varchar(50)  NULL,
    [OutlierDenomination] varchar(50)  NULL,
    [PriceType] varchar(50)  NULL,
    [FromValue] varchar(50)  NULL,
    [ToValue] varchar(50)  NULL,
    [RangeId] varchar(50)  NULL,
    [RangeText] varchar(50)  NULL,
    [RangeCodingSystem] varchar(50)  NULL,
    [RangeAltId] varchar(50)  NULL,
    [RangeAltText] varchar(50)  NULL,
    [RangeAltCodingSystem] varchar(50)  NULL,
    [RangeType] varchar(50)  NULL,
    [GrouperVersion] varchar(50)  NULL,
    [DiagnosisPriority] varchar(2)  NULL,
    [DiagnosisClassification] varchar(3)  NULL,
    [ConfidentialIndicator] varchar(50)  NULL,
    [AttestationDate] datetime  NULL,
    [DiagnosisIdentifier] varchar(50)  NULL,
    [DiagnosisActionCode] varchar(1)  NULL,
    [PDiagnosis_Status] int  NOT NULL,
    [PDiagnosis_CreatedBy] int  NULL,
    [PDiagnosis_CreatedDate] datetime  NOT NULL,
    [PDGOutBoundFileStatus] int  NULL,
    [PDGOutBoundApproval] int  NULL,
    [PDGOutBoundApprovalBy] int  NULL,
    [PDGOutBoundApprovalOn] datetime  NULL,
    [PhysicianNPI] varchar(50)  NULL,
    [PhysicianLName] varchar(50)  NULL,
    [PhysicianFName] varchar(50)  NULL,
    [OldPatient_Id] int  NULL
);
GO

-- Creating table 'tmptblSegmentChecks'
CREATE TABLE [dbo].[tmptblSegmentChecks] (
    [Segmentcheck_Id] bigint IDENTITY(1,1) NOT NULL,
    [FileId] int  NULL,
    [SegmentCode] varchar(50)  NULL,
    [SegmentValue] varchar(100)  NULL,
    [SegmentDesc] varchar(75)  NULL,
    [SegError] int  NULL
);
GO

-- Creating table 'MailConfigs'
CREATE TABLE [dbo].[MailConfigs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Port] int  NULL,
    [UserName] varchar(50)  NULL,
    [Pwd] varchar(50)  NULL,
    [Host] varchar(50)  NULL,
    [Status] int  NULL,
    [CreatedBy] int  NULL,
    [Createddate] datetime  NOT NULL
);
GO

-- Creating table 'OrderDestroys'
CREATE TABLE [dbo].[OrderDestroys] (
    [OrderDestroy_Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] varchar(20)  NULL,
    [Reason] varchar(250)  NULL,
    [DestroyerUserId] int  NULL,
    [ApprovalUserId] int  NULL,
    [OrderDestroy_Status] int  NOT NULL,
    [OrderDestroy_CreatedBy] int  NULL,
    [OrderDestroy_CreatedOn] datetime  NOT NULL,
    [PQuantity_Id] int  NULL,
    [QtyHand] varchar(20)  NULL
);
GO

-- Creating table 'ApprovalAllergyInfoes'
CREATE TABLE [dbo].[ApprovalAllergyInfoes] (
    [ApprovalPAllergy_Id] int IDENTITY(1,1) NOT NULL,
    [PAllergy_Id] int  NULL,
    [Patient_Id] int  NULL,
    [AllergyType_Id] int  NULL,
    [ClassDrugType] int  NULL,
    [ClassDrug_Id] int  NULL,
    [ClassDrug_Name] varchar(100)  NULL,
    [NameOfCoding] varchar(50)  NULL,
    [AllergySeverityCode] varchar(50)  NULL,
    [AllergyReactionCode] varchar(50)  NULL,
    [AllergyIdentificationDate] datetime  NULL,
    [PAllergy_Status] int  NOT NULL,
    [PAllergy_CreatedBy] int  NULL,
    [PAllergy_CreatedDate] datetime  NOT NULL,
    [PAOutBoundFileStatus] int  NULL,
    [PAOutBoundApproval] int  NULL,
    [PAOutBoundApprovalBy] int  NULL,
    [PAOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'ApprovalDiagnosisInfoes'
CREATE TABLE [dbo].[ApprovalDiagnosisInfoes] (
    [ApprovalPDiagnosis_Id] int IDENTITY(1,1) NOT NULL,
    [PDiagnosis_Id] int  NULL,
    [Patient_Id] int  NULL,
    [CodingMethod] varchar(50)  NULL,
    [ICD10_Id] int  NULL,
    [DCodingType_Id] int  NULL,
    [AltCodingId] varchar(10)  NULL,
    [AltCodingText] varchar(150)  NULL,
    [AltCodingMethod] varchar(5)  NULL,
    [DiagnosisDescription] varchar(100)  NULL,
    [DiagnosisDate] datetime  NULL,
    [DiagnosisType] varchar(2)  NULL,
    [MajorDiagnosticId] varchar(50)  NULL,
    [MajorDiagnosticText] varchar(50)  NULL,
    [MDCodingSystem] varchar(50)  NULL,
    [MDAlternateId] varchar(50)  NULL,
    [MDAlternateText] varchar(50)  NULL,
    [MDAltCodingSystem] varchar(50)  NULL,
    [DiagnosticGroupId] varchar(50)  NULL,
    [DiagnosticGroupText] varchar(50)  NULL,
    [DGCodingSystem] varchar(50)  NULL,
    [DGAlternateId] varchar(50)  NULL,
    [DGAlternateText] varchar(50)  NULL,
    [DGAltCodingSystem] varchar(50)  NULL,
    [DRGApprovalIndicator] varchar(1)  NULL,
    [DRGGrouperReviewCode] varchar(2)  NULL,
    [OutlierId] varchar(50)  NULL,
    [OutlierText] varchar(50)  NULL,
    [OutlierCodingSystem] varchar(50)  NULL,
    [OAlternateId] varchar(50)  NULL,
    [OAlternateText] varchar(50)  NULL,
    [OAltCodingSystem] varchar(50)  NULL,
    [OutlierDays] varchar(50)  NULL,
    [OutlierQuantity] varchar(50)  NULL,
    [OutlierDenomination] varchar(50)  NULL,
    [PriceType] varchar(50)  NULL,
    [FromValue] varchar(50)  NULL,
    [ToValue] varchar(50)  NULL,
    [RangeId] varchar(50)  NULL,
    [RangeText] varchar(50)  NULL,
    [RangeCodingSystem] varchar(50)  NULL,
    [RangeAltId] varchar(50)  NULL,
    [RangeAltText] varchar(50)  NULL,
    [RangeAltCodingSystem] varchar(50)  NULL,
    [RangeType] varchar(50)  NULL,
    [GrouperVersion] varchar(50)  NULL,
    [DiagnosisPriority] varchar(2)  NULL,
    [DiagnosisClassification] varchar(3)  NULL,
    [ConfidentialIndicator] varchar(50)  NULL,
    [AttestationDate] datetime  NULL,
    [DiagnosisIdentifier] varchar(50)  NULL,
    [DiagnosisActionCode] varchar(1)  NULL,
    [PDiagnosis_Status] int  NOT NULL,
    [PDiagnosis_CreatedBy] int  NULL,
    [PDiagnosis_CreatedDate] datetime  NOT NULL,
    [PDGOutBoundFileStatus] int  NULL,
    [PDGOutBoundApproval] int  NULL,
    [PDGOutBoundApprovalBy] int  NULL,
    [PDGOutBoundApprovalOn] datetime  NULL,
    [PhysicianNPI] varchar(50)  NULL,
    [PhysicianLName] varchar(50)  NULL,
    [PhysicianFName] varchar(50)  NULL
);
GO

-- Creating table 'ApprovalVisitInfoes'
CREATE TABLE [dbo].[ApprovalVisitInfoes] (
    [ApprovalPVisit_Id] int IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NULL,
    [Patient_Id] int  NULL,
    [PatientClass] varchar(1)  NULL,
    [NursingStationId] int  NULL,
    [Room] varchar(50)  NULL,
    [Bed] varchar(50)  NULL,
    [FacilityId] int  NULL,
    [Floor] varchar(50)  NULL,
    [AdmissionType] varchar(50)  NULL,
    [PreAdmitNumber] varchar(250)  NULL,
    [PriorNursingStationId] int  NULL,
    [PriorRoom] varchar(50)  NULL,
    [PriorBed] varchar(50)  NULL,
    [PriorFacilityId] int  NULL,
    [PriorFloor] varchar(50)  NULL,
    [Physician_Id] int  NULL,
    [PrimaryPhysicianNPI] varchar(10)  NULL,
    [PrimaryPhysicianLName] varchar(75)  NULL,
    [PrimaryPhysicianFName] varchar(75)  NULL,
    [ReferringDoctor] varchar(250)  NULL,
    [ConsultingDoctor] varchar(250)  NULL,
    [HospitalService] varchar(50)  NULL,
    [TemporaryLocation] varchar(80)  NULL,
    [PreAdmitTestIndicator] varchar(50)  NULL,
    [ReAdmissionIndicator] varchar(5)  NULL,
    [AdmitSource] varchar(6)  NULL,
    [AmbulatoryStatus] varchar(2)  NULL,
    [VIPIndicator] varchar(2)  NULL,
    [AdmittingDoctor] varchar(250)  NULL,
    [PatientType] varchar(2)  NULL,
    [VisitNumber] varchar(250)  NULL,
    [FinancialClass] varchar(50)  NULL,
    [ChargePriceIndicator] varchar(2)  NULL,
    [CourtesyCode] varchar(2)  NULL,
    [CreditRating] varchar(2)  NULL,
    [ContractCode] varchar(2)  NULL,
    [ContractEffDate] datetime  NULL,
    [ContractAmount] varchar(20)  NULL,
    [ContractPeriod] varchar(20)  NULL,
    [InterestCode] varchar(2)  NULL,
    [BadDebtCode] varchar(4)  NULL,
    [BadDebtDate] datetime  NULL,
    [BadDebtAgencyCode] varchar(10)  NULL,
    [BadDebtTransferAmt] varchar(20)  NULL,
    [BadDebtRecoveryAmt] varchar(20)  NULL,
    [DeleteAccIndicator] varchar(1)  NULL,
    [DeleteAccDate] datetime  NULL,
    [DischargeDisposition] varchar(3)  NULL,
    [DischargedLocation] varchar(47)  NULL,
    [DietType] varchar(250)  NULL,
    [ServicingFacility] varchar(2)  NULL,
    [BedStatus] varchar(1)  NULL,
    [AccStatus] varchar(5)  NULL,
    [PendingLocation] varchar(80)  NULL,
    [PriorTemporaryLocation] varchar(80)  NULL,
    [AdmitDate] datetime  NULL,
    [DischargeDate] datetime  NULL,
    [CurrentPatientBalance] varchar(20)  NULL,
    [TotalCharges] varchar(20)  NULL,
    [TotalAdjustments] varchar(20)  NULL,
    [TotalPayments] varchar(20)  NULL,
    [AlternateVisitId] varchar(250)  NULL,
    [VisitIndicator] varchar(5)  NULL,
    [OtherHealthProvider] varchar(250)  NULL,
    [Wing] int  NULL,
    [PVisit_Status] int  NOT NULL,
    [PVisit_CreatedBy] int  NULL,
    [PVisit_CreatedDate] datetime  NOT NULL,
    [PVOutBoundFileStatus] int  NULL,
    [PVOutBoundApproval] int  NULL,
    [PVOutBoundApprovalBy] int  NULL,
    [PVOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'ControlSubstanceCounts'
CREATE TABLE [dbo].[ControlSubstanceCounts] (
    [ControlSubstance_Id] int IDENTITY(1,1) NOT NULL,
    [Porder_Id] int  NULL,
    [Quantity] varchar(20)  NULL,
    [CertifiedBy] int  NULL,
    [ApprovedBy] int  NULL,
    [CertifiedDate] datetime  NULL,
    [ControlSubstance_Status] int  NOT NULL,
    [ControlSubstance_CreatedDate] datetime  NOT NULL,
    [NurseStation_Id] int  NULL,
    [InitialQuantity] varchar(20)  NULL,
    [ConsolidateFlag] int  NULL,
    [PQuantity_Id] int  NULL,
    [CheckInFlag] int  NULL
);
GO

-- Creating table 'Conversations'
CREATE TABLE [dbo].[Conversations] (
    [Conversation_Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(40)  NULL,
    [Creator_Id] int  NULL,
    [Channel_Id] varchar(50)  NULL,
    [Created_Date] datetime  NULL,
    [Updated_Date] datetime  NULL,
    [Deleted_Date] datetime  NULL,
    [Receiver_Id] int  NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [Messages_Id] int IDENTITY(1,1) NOT NULL,
    [Conversation_Id] int  NULL,
    [Sender_Id] int  NULL,
    [Message_Type] nvarchar(max)  NULL,
    [Message1] varchar(255)  NULL,
    [Attachment_Thumb_Url] varchar(255)  NULL,
    [Attachment_Url] varchar(255)  NULL,
    [Created_Date] datetime  NULL,
    [Guid] varchar(100)  NULL,
    [Deleted_Date] datetime  NULL
);
GO

-- Creating table 'Participants'
CREATE TABLE [dbo].[Participants] (
    [Participant_Id] int IDENTITY(1,1) NOT NULL,
    [Conversation_Id] int  NULL,
    [User_Id] int  NULL,
    [Participant_Type] nvarchar(max)  NULL
);
GO

-- Creating table 'Users1'
CREATE TABLE [dbo].[Users1] (
    [User_Id] int IDENTITY(1,1) NOT NULL,
    [Phone] varchar(16)  NULL,
    [Email] varchar(255)  NULL,
    [Password] varchar(40)  NULL,
    [First_Name] varchar(50)  NULL,
    [Last_Name] varchar(50)  NULL,
    [Middle_Name] varchar(50)  NULL,
    [Verification_Code] varchar(6)  NULL,
    [Is_Active] int  NULL,
    [Is_Reported] int  NULL,
    [Is_Blocked] int  NULL,
    [Created_Date] datetime  NULL,
    [socket_Id] varchar(max)  NULL
);
GO

-- Creating table 'UploadedDocuments'
CREATE TABLE [dbo].[UploadedDocuments] (
    [PatientDoc_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NULL,
    [DocName] varchar(max)  NULL,
    [DocLocation] varchar(max)  NULL,
    [DocDescription] varchar(50)  NULL,
    [FolderID] int  NULL,
    [UDocuments_CreatedDate] datetime  NULL,
    [UDocuments_CreatedBy] int  NULL,
    [DocType] varchar(500)  NULL
);
GO

-- Creating table 'DrFirstXMLTrans'
CREATE TABLE [dbo].[DrFirstXMLTrans] (
    [DrFirstXMLTrans_Id] int IDENTITY(1,1) NOT NULL,
    [RcopiaID] varchar(50)  NULL,
    [CreatedDate] varchar(50)  NULL,
    [CompletedDate] varchar(50)  NULL,
    [SignedDate] varchar(50)  NULL,
    [StopDate] varchar(50)  NULL,
    [LastModifiedBy] varchar(50)  NULL,
    [LastModifiedDate] varchar(50)  NULL,
    [Height] varchar(50)  NULL,
    [Weight] varchar(50)  NULL,
    [IntendedUse] varchar(50)  NULL,
    [Deleted] varchar(50)  NULL,
    [Voided] varchar(50)  NULL,
    [Denied] varchar(50)  NULL,
    [ExternalID] varchar(50)  NULL,
    [NeedsReview] varchar(50)  NULL,
    [PatientID] varchar(20)  NULL,
    [PatientExternalID] varchar(20)  NULL,
    [RcopiaPracticeID] varchar(50)  NULL,
    [FirstName] varchar(50)  NULL,
    [LastName] varchar(50)  NULL,
    [ProviderID] varchar(20)  NULL,
    [Username] varchar(20)  NULL,
    [ProviderExternalID] varchar(50)  NULL,
    [ProviderNPI] varchar(50)  NULL,
    [ProviderFirstName] varchar(50)  NULL,
    [ProviderLastName] varchar(50)  NULL,
    [ProviderDEA] varchar(50)  NULL,
    [PreparerID] varchar(20)  NULL,
    [PreparerName] varchar(20)  NULL,
    [PreparerExternalID] varchar(50)  NULL,
    [PreparerFirstName] varchar(50)  NULL,
    [PreparerLastName] varchar(50)  NULL,
    [PharmacyID] varchar(20)  NULL,
    [PharmacyMasterID] varchar(20)  NULL,
    [PharmacyDeleted] varchar(2)  NULL,
    [NCPDPID] varchar(50)  NULL,
    [PharmacyName] varchar(50)  NULL,
    [PharmacyAddress1] varchar(50)  NULL,
    [PharmacyAddress2] varchar(50)  NULL,
    [PharmacyCity] varchar(50)  NULL,
    [PharmacyState] varchar(50)  NULL,
    [PharmacyZip] varchar(50)  NULL,
    [PharmacyPhone] varchar(50)  NULL,
    [PharmacyFax] varchar(50)  NULL,
    [PharmacyIs24Hour] varchar(2)  NULL,
    [PharmacyLevel3] varchar(2)  NULL,
    [PharmacyElectronic] varchar(2)  NULL,
    [PharmacyMailOrder] varchar(2)  NULL,
    [PharmacyRequiresEligibility] varchar(2)  NULL,
    [PharmacyRetail] varchar(2)  NULL,
    [PharmacyLongTermCare] varchar(2)  NULL,
    [PharmacySpecialty] varchar(2)  NULL,
    [PharmacyCanReceiveControlledSubstance] varchar(2)  NULL,
    [PharmacyInHouseDispensing] varchar(2)  NULL,
    [PharmacyCompounding] varchar(2)  NULL,
    [PharmacyDurableMedicalEquipment] varchar(2)  NULL,
    [PharmacyKiosk] varchar(2)  NULL,
    [NDCID] varchar(20)  NULL,
    [FirstDataBankMedID] varchar(20)  NULL,
    [DrugId] varchar(50)  NULL,
    [PrescriptionID] varchar(50)  NULL,
    [DrugDescription] varchar(500)  NULL,
    [BrandName] varchar(250)  NULL,
    [GenericName] varchar(250)  NULL,
    [Schedule] varchar(50)  NULL,
    [BrandType] varchar(50)  NULL,
    [LegendStatus] varchar(50)  NULL,
    [RouteInfo] varchar(50)  NULL,
    [Form] varchar(50)  NULL,
    [Strength] varchar(50)  NULL,
    [DoAction] varchar(50)  NULL,
    [Dose] varchar(50)  NULL,
    [DoseUnit] varchar(50)  NULL,
    [TakeBy] varchar(50)  NULL,
    [DoseTiming] varchar(150)  NULL,
    [DoseOther] varchar(150)  NULL,
    [Duration] varchar(2)  NULL,
    [Quantity] varchar(5)  NULL,
    [QuantityUnit] varchar(50)  NULL,
    [Refills] varchar(50)  NULL,
    [SubstitutionPermitted] varchar(50)  NULL,
    [OtherNotes] varchar(250)  NULL,
    [PatientNotes] varchar(250)  NULL,
    [MaximumDailyDose] varchar(50)  NULL,
    [MaximumDailyDoseUnit] varchar(50)  NULL,
    [Comments] varchar(50)  NULL,
    [FormularyAction] varchar(200)  NULL,
    [DrFirstXMLTrans_Approval] int  NULL,
    [DrFirstXMLTrans_ApprovalBy] int  NULL,
    [DrFirstXMLTrans_ApprovalOn] datetime  NULL
);
GO

-- Creating table 'DrFirstOrderXMLTrans'
CREATE TABLE [dbo].[DrFirstOrderXMLTrans] (
    [DrFirstOrder_Id] int IDENTITY(1,1) NOT NULL,
    [RcopiaID] varchar(50)  NULL,
    [PatientExternalID] varchar(20)  NULL,
    [NDCID] varchar(20)  NULL,
    [FirstDataBankMedID] varchar(20)  NULL,
    [DrugId] varchar(50)  NULL,
    [PrescriptionID] varchar(50)  NULL,
    [DrugDescription] varchar(500)  NULL,
    [BrandName] varchar(250)  NULL,
    [GenericName] varchar(250)  NULL,
    [Schedule] varchar(50)  NULL,
    [BrandType] varchar(50)  NULL,
    [LegendStatus] varchar(50)  NULL,
    [RouteInfo] varchar(50)  NULL,
    [Form] varchar(50)  NULL,
    [Strength] varchar(50)  NULL,
    [DoAction] varchar(50)  NULL,
    [Dose] varchar(50)  NULL,
    [DoseUnit] varchar(50)  NULL,
    [TakeBy] varchar(50)  NULL,
    [DoseTiming] varchar(150)  NULL,
    [DoseOther] varchar(150)  NULL,
    [Duration] varchar(2)  NULL,
    [Quantity] varchar(5)  NULL,
    [QuantityUnit] varchar(50)  NULL,
    [Refills] varchar(50)  NULL,
    [SubstitutionPermitted] varchar(50)  NULL,
    [OtherNotes] varchar(250)  NULL,
    [PatientNotes] varchar(250)  NULL,
    [MaximumDailyDose] varchar(50)  NULL,
    [MaximumDailyDoseUnit] varchar(50)  NULL,
    [Comments] varchar(50)  NULL,
    [DrFirstOrder_CreatedDate] datetime  NULL,
    [DrFirstOrderXMLTrans_Approval] int  NULL,
    [DrFirstOrderXMLTrans_ApprovalBy] int  NULL,
    [DrFirstOrderXMLTrans_ApprovalOn] datetime  NULL
);
GO

-- Creating table 'AllergyInfoMasters'
CREATE TABLE [dbo].[AllergyInfoMasters] (
    [Allergy_Id] int IDENTITY(1,1) NOT NULL,
    [AllergyTypeMaster_Id] int  NULL,
    [AllergyDesc_Id] varchar(50)  NULL,
    [AllergyDesc] varchar(150)  NULL,
    [Allergy_Status] int  NOT NULL,
    [Allergy_CreatedBy] int  NULL,
    [Allergy_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'NurseCommentTypes'
CREATE TABLE [dbo].[NurseCommentTypes] (
    [NurseCommentType_Id] int IDENTITY(1,1) NOT NULL,
    [NurseCommentType_Desc] varchar(100)  NULL,
    [NurseCommentType_Status] int  NULL,
    [NurseCommentType_CreatedBy] int  NULL,
    [NurseCommentType_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'ActivityMasters'
CREATE TABLE [dbo].[ActivityMasters] (
    [Activity_Id] int IDENTITY(1,1) NOT NULL,
    [Activity_Desc] varchar(50)  NULL,
    [Activity_Status] int  NULL,
    [Activity_CreatedBy] int  NULL,
    [Activity_CreatedOn] datetime  NULL
);
GO

-- Creating table 'UserActivityDetails'
CREATE TABLE [dbo].[UserActivityDetails] (
    [UserActivity_Id] bigint IDENTITY(1,1) NOT NULL,
    [Session_Id] bigint  NULL,
    [Screen_Id] int  NULL,
    [Time] datetime  NULL,
    [Activity_Id] int  NULL,
    [Comments] varchar(500)  NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [Session_Id] bigint IDENTITY(1,1) NOT NULL,
    [user_Id] int  NULL,
    [LoginTime] datetime  NULL,
    [LogOutTime] datetime  NULL,
    [Session_Status] int  NULL,
    [SystemIP] varchar(500)  NULL,
    [BrowserName] varchar(100)  NULL,
    [role_Id] int  NULL
);
GO

-- Creating table 'UserOTPs'
CREATE TABLE [dbo].[UserOTPs] (
    [TransID] bigint IDENTITY(1,1) NOT NULL,
    [UserID] bigint  NULL,
    [OTP] varchar(16)  NULL,
    [LoginDate] datetime  NULL,
    [LoginType] int  NULL,
    [Attempts] int  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'FileInfoErrors'
CREATE TABLE [dbo].[FileInfoErrors] (
    [FileInfoError_Id] int IDENTITY(1,1) NOT NULL,
    [File_Id] int  NULL,
    [Error_Desc] varchar(500)  NULL
);
GO

-- Creating table 'ApprovalRefills'
CREATE TABLE [dbo].[ApprovalRefills] (
    [Refill_Id] bigint IDENTITY(1,1) NOT NULL,
    [Porder_Id] int  NULL,
    [Patient_Id] int  NULL,
    [NumberOfRefillsRemaining] nchar(10)  NULL,
    [Refill_Status] int  NULL,
    [Refill_CreatedBy] int  NULL,
    [Refill_CreatedDate] datetime  NULL,
    [POOutBoundFileStatus] int  NULL,
    [POOutBoundApproval] int  NULL,
    [POOutBoundApprovalBy] int  NULL,
    [POOutBoundApprovalOn] datetime  NULL,
    [File_Id] int  NULL
);
GO

-- Creating table 'OrderFavouriteDatas'
CREATE TABLE [dbo].[OrderFavouriteDatas] (
    [FavouriteData_Id] bigint IDENTITY(1,1) NOT NULL,
    [DrugAdminister_Id] bigint  NULL,
    [OrderFavMaster_ID] int  NULL,
    [value] varchar(500)  NULL,
    [FavouriteData_Status] int  NULL,
    [FavouriteData_CreatedBy] int  NULL,
    [Favourite_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'NurseComments'
CREATE TABLE [dbo].[NurseComments] (
    [Comments_Id] bigint IDENTITY(1,1) NOT NULL,
    [DrugAdminister_Id] bigint  NULL,
    [NurseCommentType_Id] int  NULL,
    [Comment] varchar(500)  NULL,
    [comment_Status] int  NULL,
    [comment_CreatedBy] int  NULL,
    [Comment_CreatedOn] datetime  NOT NULL,
    [PQuantity_Id] int  NULL,
    [Patient_Id] int  NULL
);
GO

-- Creating table 'PatientTypes'
CREATE TABLE [dbo].[PatientTypes] (
    [PatientType_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [Color_Code] varchar(50)  NULL,
    [Color_Description] varchar(75)  NULL,
    [PatientType_Status] int  NULL,
    [PatientType_CreatedBy] int  NULL,
    [PatientType_CreatedDate] datetime  NULL
);
GO

-- Creating table 'ColourTypes'
CREATE TABLE [dbo].[ColourTypes] (
    [ColourType_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NULL,
    [PatientType_Id] int  NULL,
    [ColourType_Status] int  NULL,
    [ColourType_CreatedBy] int  NULL,
    [ColourType_CreatedOn] datetime  NULL
);
GO

-- Creating table 'Fingersdescs'
CREATE TABLE [dbo].[Fingersdescs] (
    [Fingersdesc_Id] int IDENTITY(1,1) NOT NULL,
    [FingersDesc1] varchar(100)  NULL,
    [Fingersdesc_Status] int  NULL,
    [Fingersdesc_CreatedBy] int  NULL,
    [Fingersdesc_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'BarcodeDetails'
CREATE TABLE [dbo].[BarcodeDetails] (
    [PBarcode_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NULL,
    [Stock_Id] int  NULL,
    [Ekit_Id] int  NULL,
    [BarcodeDetail1] varchar(150)  NULL,
    [PBarcode_Status] int  NOT NULL,
    [PBarcode_CreatedBy] int  NULL,
    [PBarcode_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'MailFavourites'
CREATE TABLE [dbo].[MailFavourites] (
    [MailFavourite_Id] int IDENTITY(1,1) NOT NULL,
    [MailBox_Id] int  NULL,
    [user_Id] int  NULL,
    [MailFavourite_Status] int  NULL,
    [MailFavourite_date] datetime  NULL
);
GO

-- Creating table 'MailReads'
CREATE TABLE [dbo].[MailReads] (
    [MailRead_Id] bigint IDENTITY(1,1) NOT NULL,
    [MailBox_Id] int  NULL,
    [user_Id] int  NULL,
    [MailRead_Status] int  NULL,
    [MailRead_Date] datetime  NULL,
    [MailFav_Status] int  NULL,
    [Mail_Status] int  NULL,
    [FromMail_Status] int  NULL
);
GO

-- Creating table 'AlertStatus'
CREATE TABLE [dbo].[AlertStatus] (
    [Alert_Id] bigint IDENTITY(1,1) NOT NULL,
    [user_Id] int  NULL,
    [AlertText_Id] bigint  NULL,
    [Read_Flag] int  NULL,
    [Favourite_Flag] int  NULL,
    [Alert_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'AlertTexts'
CREATE TABLE [dbo].[AlertTexts] (
    [AlertText_Id] bigint IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [Type_Id] int  NULL,
    [Patient_Id] int  NULL,
    [AlertText_Status] int  NULL,
    [AlertText_CreatedOn] datetime  NULL,
    [File_Id] bigint  NULL
);
GO

-- Creating table 'AlertTypes'
CREATE TABLE [dbo].[AlertTypes] (
    [Type_Id] int IDENTITY(1,1) NOT NULL,
    [Type_Desc] varchar(50)  NULL,
    [Type_Status] int  NULL,
    [Type_CreatedBy] int  NULL,
    [Type_CreatedOn] datetime  NULL
);
GO

-- Creating table 'Routes'
CREATE TABLE [dbo].[Routes] (
    [Route_Id] int IDENTITY(1,1) NOT NULL,
    [Route_Code] varchar(5)  NULL,
    [Route_Desc] varchar(50)  NULL,
    [Route_Status] int  NULL,
    [Route_CreatedBy] int  NULL,
    [Route_CreatedOn] datetime  NULL,
    [Route_DirDesc] varchar(50)  NULL
);
GO

-- Creating table 'Ekits'
CREATE TABLE [dbo].[Ekits] (
    [Ekit_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [DrugName] varchar(250)  NULL,
    [InHand] varchar(50)  NULL,
    [NDC] varchar(50)  NULL,
    [LotNumber] varchar(50)  NULL,
    [ExpiryDate] datetime  NULL,
    [Ekit_Status] int  NULL,
    [Ekit_CreatedBy] int  NULL,
    [Ekit_CreatedOn] datetime  NULL,
    [GPICode] varchar(20)  NULL,
    [TrackableBit] int  NULL,
    [Facility_Id] int  NULL,
    [Sharedekitbit] int  NULL,
    [InitialQuantity] varchar(50)  NULL,
    [CertifiedBy] int  NULL,
    [ApprovedBy] int  NULL,
    [CertifiedDate] datetime  NULL,
    [CheckInFlag] int  NULL,
    [ControlSubstance] int  NULL
);
GO

-- Creating table 'Stocks'
CREATE TABLE [dbo].[Stocks] (
    [Stock_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [DrugName] varchar(250)  NULL,
    [InHand] varchar(50)  NULL,
    [Stock_Status] int  NULL,
    [Stock_CreatedBy] int  NULL,
    [Stock_CreatedDate] datetime  NULL,
    [GPICode] varchar(20)  NULL,
    [TrackableBit] int  NULL,
    [Facility_Id] int  NULL,
    [Sharedstockbit] int  NULL
);
GO

-- Creating table 'Drugs'
CREATE TABLE [dbo].[Drugs] (
    [Drug_Id] int IDENTITY(1,1) NOT NULL,
    [TradeName] nvarchar(255)  NULL,
    [GenericName] nvarchar(255)  NULL,
    [Form] nvarchar(255)  NULL,
    [Route] nvarchar(255)  NULL,
    [Dosage] nvarchar(255)  NULL,
    [Indication] nvarchar(255)  NULL,
    [PsychiatricIndication] int  NULL,
    [Drug_Status] int  NULL,
    [Drug_CreatedBy] int  NULL,
    [Drug_CreatedDate] datetime  NULL
);
GO

-- Creating table 'EkitAdministers'
CREATE TABLE [dbo].[EkitAdministers] (
    [EkitAdminister_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NULL,
    [DrugAdminister_Id] bigint  NULL,
    [Ekit_Id] int  NULL,
    [quantity] decimal(19,4)  NULL,
    [EkitAdministerBy] int  NULL,
    [EkitAdministerOn] datetime  NULL
);
GO

-- Creating table 'DrugActivedays'
CREATE TABLE [dbo].[DrugActivedays] (
    [Activeday_Id] bigint IDENTITY(1,1) NOT NULL,
    [DAdmin_Id] int  NULL,
    [ActiveDay] int  NULL,
    [Activeday_CreatedDate] datetime  NULL
);
GO

-- Creating table 'DrFirstFileDatas'
CREATE TABLE [dbo].[DrFirstFileDatas] (
    [DrFirstFile_Id] bigint IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [PatientMRNumber] varchar(50)  NULL,
    [FilePath] nvarchar(max)  NULL,
    [ReceivedOn] datetime  NULL
);
GO

-- Creating table 'OrderStocks'
CREATE TABLE [dbo].[OrderStocks] (
    [OrderStock_Id] int IDENTITY(1,1) NOT NULL,
    [Stock_Id] int  NULL,
    [Porder_Id] int  NULL,
    [Inhand] varchar(50)  NULL,
    [Remaining] varchar(50)  NULL,
    [OrderStock_Status] int  NULL,
    [OrderStock_CreatedBy] int  NULL,
    [OrderStock_CreatedDate] datetime  NULL,
    [LotNumber] varchar(150)  NULL,
    [ExpirationDate] datetime  NULL,
    [PQuantity_Id] int  NULL
);
GO

-- Creating table 'NurseShifts'
CREATE TABLE [dbo].[NurseShifts] (
    [NurseShifts_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [NurseShifts_Name] varchar(50)  NULL,
    [Fromtime_hoursId] int  NULL,
    [FromTime_TimeFormatId] int  NULL,
    [Totime_hoursId] int  NULL,
    [ToTime_TimeFormatId] int  NULL,
    [NurseShifts_Status] int  NULL,
    [NurseShifts_CreatedBy] int  NULL,
    [NurseShifts_CreatedOn] datetime  NULL
);
GO

-- Creating table 'CompanyConfigs'
CREATE TABLE [dbo].[CompanyConfigs] (
    [CompanyConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [ApprovalFlag] int  NULL,
    [Fingersdesc_Id] int  NULL,
    [TimeFormat] int  NULL,
    [Hl7Configured] int  NULL,
    [HLDirectionalWay_Id] int  NULL,
    [CompanyConfig_Status] int  NULL,
    [CompanyConfig_CreatedBy] int  NULL,
    [CompanyConfig_CreatedOn] datetime  NULL,
    [StockReport_Id] int  NULL,
    [DrFirstRequired] int  NULL,
    [Physician_Id] int  NULL
);
GO

-- Creating table 'CompanyHlCategories'
CREATE TABLE [dbo].[CompanyHlCategories] (
    [CompanyHlCategory_Id] int IDENTITY(1,1) NOT NULL,
    [CompanyConfig_Id] int  NULL,
    [FteCategory_Id] int  NULL
);
GO

-- Creating table 'CompanyHLEvents'
CREATE TABLE [dbo].[CompanyHLEvents] (
    [CompanyHLEvent_Id] int IDENTITY(1,1) NOT NULL,
    [CompanyHlCategory_Id] int  NULL,
    [EventCat_Id] int  NULL
);
GO

-- Creating table 'HLDirectionalWays'
CREATE TABLE [dbo].[HLDirectionalWays] (
    [HLDirectionalWay_Id] int IDENTITY(1,1) NOT NULL,
    [HLDirectionalWaysDesc] varchar(100)  NULL,
    [HLDirectionalWay_Status] int  NULL,
    [HLDirectionalWay_CreatedBy] int  NULL,
    [HLDirectionalWay_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'StockReports'
CREATE TABLE [dbo].[StockReports] (
    [StockReport_Id] int IDENTITY(1,1) NOT NULL,
    [StockReportFor] varchar(50)  NULL,
    [StockReport_Status] int  NULL,
    [StockReport_CreatedBy] int  NULL,
    [StockReport_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'OnLeaves'
CREATE TABLE [dbo].[OnLeaves] (
    [Onleave_Id] int IDENTITY(1,1) NOT NULL,
    [PVisit_Id] bigint  NULL,
    [LeaveFrom] datetime  NULL,
    [LeaveTo] datetime  NULL,
    [Reason] varchar(250)  NULL,
    [OnLeave_Status] int  NULL,
    [OnLeave_CreatedBy] int  NULL,
    [OnLeave_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'NursingFrequencyConfigs'
CREATE TABLE [dbo].[NursingFrequencyConfigs] (
    [NursingFreq_Id] int IDENTITY(1,1) NOT NULL,
    [Facility_Id] int  NULL,
    [NursingStation_Id] int  NULL,
    [Floor_Id] int  NULL,
    [Wing_Id] int  NULL,
    [Frequency_Id] int  NULL,
    [StartTime] int  NULL,
    [TimeFormat_ID] int  NULL,
    [Hours] int  NULL,
    [Monday] bit  NULL,
    [Tuesday] bit  NULL,
    [Wednesday] bit  NULL,
    [Thursday] bit  NULL,
    [Friday] bit  NULL,
    [Saturday] bit  NULL,
    [Sunday] bit  NULL,
    [Week_Id] int  NULL,
    [Month_Id] int  NULL,
    [OnlyOnDay] int  NULL,
    [ThroughDay] int  NULL,
    [ActiveDays] int  NULL,
    [HoldDays] int  NULL,
    [NursingFreq_Status] int  NOT NULL,
    [NursingFreq_CreatedBy] int  NULL,
    [NursingFreq_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Role_Id] int IDENTITY(1,1) NOT NULL,
    [Role_Desc] varchar(50)  NULL,
    [DefaultScreen_Id] int  NULL,
    [IsAdmin] int  NULL,
    [Role_Status] int  NOT NULL,
    [Role_CreatedBy] int  NULL,
    [Role_CreatedDate] datetime  NOT NULL,
    [Parent_Id] int  NULL
);
GO

-- Creating table 'DefaultScreens'
CREATE TABLE [dbo].[DefaultScreens] (
    [DefaultScreen_Id] int IDENTITY(1,1) NOT NULL,
    [Screen_Id] int  NULL,
    [DefaultScreen_Status] int  NULL,
    [DefaultScreen_CreatedBy] int  NULL,
    [DefaultScreen_CreatedOn] datetime  NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Company_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Name] varchar(50)  NULL,
    [Company_Addr1] varchar(100)  NULL,
    [Company_Addr2] varchar(100)  NULL,
    [Company_Zip] varchar(50)  NULL,
    [Company_City] varchar(50)  NULL,
    [Company_State] varchar(50)  NULL,
    [Company_CountryId] int  NULL,
    [Company_Phone] varchar(20)  NULL,
    [Company_Fax] varchar(20)  NULL,
    [Company_Email] varchar(150)  NULL,
    [Com_ContactPerson] varchar(50)  NULL,
    [Com_ContactPhone] varchar(20)  NULL,
    [Company_EIN] varchar(20)  NULL,
    [Company_Logo] varbinary(max)  NULL,
    [Company_UniqueId] varchar(50)  NULL,
    [ApprovalFlag] int  NULL,
    [Fingersdesc_Id] int  NULL,
    [TimeFormat] int  NULL,
    [Company_Status] int  NOT NULL,
    [Company_CreatedBy] int  NULL,
    [Company_CreatedDate] datetime  NOT NULL,
    [Company_FooterLogo] varbinary(max)  NULL
);
GO

-- Creating table 'Facilities'
CREATE TABLE [dbo].[Facilities] (
    [Facility_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NOT NULL,
    [Facility_Name] varchar(100)  NULL,
    [Facility_Addr1] varchar(150)  NULL,
    [Facility_Addr2] varchar(150)  NULL,
    [Facility_Zip] varchar(50)  NULL,
    [Facility_City] varchar(50)  NULL,
    [Facility_State] varchar(50)  NULL,
    [Facility_CountryId] int  NULL,
    [Facility_Phone] varchar(20)  NULL,
    [Facility_Fax] varchar(20)  NULL,
    [Facility_ContactName] varchar(50)  NULL,
    [Facility_ContactPhone] varchar(20)  NULL,
    [Facility_ShortName] varchar(20)  NULL,
    [Facility_Logo] varbinary(max)  NULL,
    [Facility_Status] int  NOT NULL,
    [Facility_CreatedBy] int  NULL,
    [Facility_CreatedDate] datetime  NOT NULL,
    [TZ_Id] int  NULL
);
GO

-- Creating table 'PhysicianDetails'
CREATE TABLE [dbo].[PhysicianDetails] (
    [Physician_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [PhysicianNPI] varchar(50)  NULL,
    [PhysicianDEANumber] varchar(50)  NULL,
    [PhysicianLName] varchar(50)  NULL,
    [PhysicianFName] varchar(50)  NULL,
    [PhysicianAddress1] varchar(250)  NULL,
    [PhysicianAddress2] varchar(250)  NULL,
    [PhysicianCity] varchar(50)  NULL,
    [PhysicianState] varchar(50)  NULL,
    [PhysicianCountryID] int  NULL,
    [PhysicianZip] varchar(50)  NULL,
    [Physician_Status] int  NOT NULL,
    [Physician_CreatedBy] int  NULL,
    [Physician_CreatedDate] datetime  NOT NULL,
    [Facility_Id] int  NULL,
    [PhysicianCredentials] varchar(50)  NULL
);
GO

-- Creating table 'NursingFCTimes'
CREATE TABLE [dbo].[NursingFCTimes] (
    [FCTime_Id] bigint IDENTITY(1,1) NOT NULL,
    [NursingFreq_Id] int  NULL,
    [hour_Id] int  NULL
);
GO

-- Creating table 'ApprovalDemographics'
CREATE TABLE [dbo].[ApprovalDemographics] (
    [ApprovalPatient_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NULL,
    [ExternalPatientId] varchar(20)  NULL,
    [ExternalFacShortName] varchar(50)  NULL,
    [ExternalFacPatientId] varchar(50)  NULL,
    [AlternatePatientId] int  NULL,
    [PatientLastName] varchar(100)  NULL,
    [PatientFirstName] varchar(100)  NULL,
    [PatientMiddleInitial] varchar(1)  NULL,
    [NameTypeCode] varchar(1)  NULL,
    [MotherMaidenName] varchar(250)  NULL,
    [DOB] datetime  NULL,
    [AdministrativeSex] varchar(1)  NULL,
    [PatientAlias] varchar(250)  NULL,
    [Race] varchar(250)  NULL,
    [PatientAddress1] varchar(100)  NULL,
    [PatientAddress2] varchar(100)  NULL,
    [PatientCity] varchar(50)  NULL,
    [PatientState] varchar(50)  NULL,
    [PatientZipCode] varchar(20)  NULL,
    [CountyCode] varchar(10)  NULL,
    [PhoneHome] varchar(50)  NULL,
    [PhoneBusiness] varchar(50)  NULL,
    [PrimaryLanguage] varchar(50)  NULL,
    [MaritalStatus] varchar(50)  NULL,
    [Religion] varchar(50)  NULL,
    [PatientMRNumber] varchar(50)  NULL,
    [SSN] varchar(20)  NULL,
    [DriverLicense] varchar(50)  NULL,
    [MotherIdentifier] varchar(250)  NULL,
    [EthnicGroup] varchar(50)  NULL,
    [BirthPlace] varchar(50)  NULL,
    [MultipleBirthIndicator] varchar(50)  NULL,
    [BirthOrder] varchar(50)  NULL,
    [Citizenship] varchar(50)  NULL,
    [MilitaryStatus] varchar(50)  NULL,
    [Nationality] varchar(50)  NULL,
    [DeathDateTime] datetime  NULL,
    [DeathIndicator] varchar(1)  NULL,
    [IdentityIndicator] varchar(1)  NULL,
    [IdentityReliability] varchar(1)  NULL,
    [LastUpdate] varchar(26)  NULL,
    [LastFacilityUpdate] varchar(26)  NULL,
    [SpeciesCode] varchar(50)  NULL,
    [BreedCode] varchar(50)  NULL,
    [Strain] varchar(50)  NULL,
    [ProductionClassCode] varchar(50)  NULL,
    [TribalCitizenship] varchar(50)  NULL,
    [ImageLocation] varchar(max)  NULL,
    [Patient_Status] int  NOT NULL,
    [Patient_CreatedBy] int  NULL,
    [Patient_CreatedDate] datetime  NOT NULL,
    [PDOutBoundFileStatus] int  NULL,
    [PDOutBoundApproval] int  NULL,
    [PDOutBoundApprovalBy] int  NULL,
    [PDOutBoundApprovalOn] datetime  NULL,
    [Alert] varchar(150)  NULL,
    [Diet] varchar(150)  NULL,
    [BiometricInfo] varchar(8000)  NULL
);
GO

-- Creating table 'FTEConfigurations'
CREATE TABLE [dbo].[FTEConfigurations] (
    [FteConfig_Id] int IDENTITY(1,1) NOT NULL,
    [ServerIp] varchar(50)  NULL,
    [Category] int  NOT NULL,
    [ConnectionType] int  NULL,
    [Port] varchar(50)  NULL,
    [UserName] varchar(50)  NULL,
    [Password] varchar(50)  NULL,
    [FteConfig_Status] int  NOT NULL,
    [FteConfig_CreatedBy] int  NULL,
    [FteConfig_CreatedDate] datetime  NOT NULL,
    [FteConnectionStatus] varchar(50)  NULL
);
GO

-- Creating table 'RecentFacs'
CREATE TABLE [dbo].[RecentFacs] (
    [RecFac_Id] int IDENTITY(1,1) NOT NULL,
    [User_Id] int  NOT NULL,
    [Facility_Id] int  NOT NULL,
    [NurseStation_Id] varchar(500)  NOT NULL
);
GO

-- Creating table 'HLSevenOutboundDisplayCompanyConfigs'
CREATE TABLE [dbo].[HLSevenOutboundDisplayCompanyConfigs] (
    [OHLConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Company_Id] int  NULL,
    [OSegDetail_Id] int  NOT NULL,
    [DisplayConfigId] int  NULL,
    [OHLConfig_Status] int  NULL,
    [OHLConfig_CreatedBy] int  NULL,
    [OHLConfig_CreatedDate] datetime  NULL
);
GO

-- Creating table 'HLSevenOutboundDisplaySegmentDetails'
CREATE TABLE [dbo].[HLSevenOutboundDisplaySegmentDetails] (
    [OSegDetail_Id] int IDENTITY(1,1) NOT NULL,
    [OSegment_Id] int  NOT NULL,
    [OSegDetail_Desc] varchar(50)  NULL,
    [HlSequence] int  NULL,
    [OSegDetail_Status] int  NULL,
    [OSegDetail_CreatedBy] int  NULL,
    [OSegDetail_CreatedDate] datetime  NULL
);
GO

-- Creating table 'HLSevenOutboundDisplaySegments'
CREATE TABLE [dbo].[HLSevenOutboundDisplaySegments] (
    [OSegment_Id] int IDENTITY(1,1) NOT NULL,
    [OSegment_Desc] varchar(50)  NULL,
    [OSegment_Status] int  NOT NULL,
    [OSegment_CreatedBy] int  NULL,
    [OSegment_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'DrugOrderMasters'
CREATE TABLE [dbo].[DrugOrderMasters] (
    [DrugOrderMaster_Id] int IDENTITY(1,1) NOT NULL,
    [NDC] varchar(20)  NULL,
    [GPI] varchar(20)  NULL,
    [DrugName] varchar(50)  NULL,
    [Strength] varchar(50)  NULL,
    [DosageForm] varchar(10)  NULL,
    [DrugOrderMaster_Status] int  NULL,
    [DrugOrderMaster_CreatedBy] int  NULL,
    [DrugOrderMaster_CreatedDate] datetime  NULL,
    [ControlledSubstanceSchedule] varchar(5)  NULL
);
GO

-- Creating table 'DrugAdministrationTimes'
CREATE TABLE [dbo].[DrugAdministrationTimes] (
    [DAdmin_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [PQuantity_Id] int  NULL,
    [AdministrationType] int  NULL,
    [NursingFreq_Id] int  NULL,
    [NurseShifts_Id] varchar(50)  NULL,
    [Hour_Id] int  NULL,
    [TimeFormat_Id] int  NULL,
    [Hours] int  NULL,
    [Monday] bit  NULL,
    [Tuesday] bit  NULL,
    [Wednesday] bit  NULL,
    [Thursday] bit  NULL,
    [Friday] bit  NULL,
    [Saturday] bit  NULL,
    [Sunday] bit  NULL,
    [Week_Id] int  NULL,
    [Month_Id] int  NULL,
    [OnlyOnDay] int  NULL,
    [ThroughDay] int  NULL,
    [ActiveDays] int  NULL,
    [HoldDays] int  NULL,
    [DAdmin_Status] int  NOT NULL,
    [DAdmin_CreatedBy] int  NULL,
    [DAdmin_CreatedDate] datetime  NULL,
    [DUom] int  NULL
);
GO

-- Creating table 'DocAdministerTrans'
CREATE TABLE [dbo].[DocAdministerTrans] (
    [DATrans_Id] bigint IDENTITY(1,1) NOT NULL,
    [DrugAdminister_Id] bigint  NULL,
    [Dacd_Id] int  NULL,
    [ManualDocumentedBy] int  NULL,
    [ManualDocumentedDate] datetime  NULL,
    [AdminsterBy] int  NULL,
    [AdminsterOn] datetime  NULL,
    [Reason] varchar(100)  NULL,
    [EnteredBy] int  NULL,
    [EnteredDate] datetime  NULL
);
GO

-- Creating table 'OrderFavourites'
CREATE TABLE [dbo].[OrderFavourites] (
    [OrderFavourite_ID] int IDENTITY(1,1) NOT NULL,
    [PQuantity_Id] int  NOT NULL,
    [OrderFavMaster_ID] int  NOT NULL,
    [OrderFavourite_Status] int  NOT NULL,
    [OrderFavourite_Createby] int  NULL,
    [OrderFavourite_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'OrderHolds'
CREATE TABLE [dbo].[OrderHolds] (
    [OrderHold_Id] int IDENTITY(1,1) NOT NULL,
    [PQuantity_Id] int  NULL,
    [HoldFrom] datetime  NULL,
    [HoldTo] datetime  NULL,
    [HoldReason] varchar(max)  NULL,
    [OrderHold_Status] int  NOT NULL,
    [OrderHold_CreatedBy] int  NULL,
    [OrderHold_CreatedDate] datetime  NULL
);
GO

-- Creating table 'TreatmentDispenseInfoes'
CREATE TABLE [dbo].[TreatmentDispenseInfoes] (
    [PDispense_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [DispenseCounter] varchar(4)  NULL,
    [DispenseCodeID] varchar(80)  NULL,
    [DispenseCodeText] varchar(80)  NULL,
    [ADispenseCodeID] varchar(90)  NULL,
    [DispensedDate] datetime  NULL,
    [ActualDispenseAmt] varchar(20)  NULL,
    [ActualDispenseUnits] varchar(250)  NULL,
    [ActualDosageForm] varchar(250)  NULL,
    [PrescriptionNumber] varchar(20)  NULL,
    [NumberOfRefillsRemaining] varchar(20)  NULL,
    [DispenseNotes] varchar(200)  NULL,
    [DispensingProvider] varchar(200)  NULL,
    [SubstitutionStatus] char(1)  NULL,
    [TotalDailyDose] varchar(10)  NULL,
    [DispenseToLocation] varchar(200)  NULL,
    [NeedsHumanReview] char(1)  NULL,
    [SpecialDispensingInstruction] varchar(250)  NULL,
    [ActualStrength] varchar(20)  NULL,
    [ActualStrengthUnit] varchar(250)  NULL,
    [SubstanceLotNumber] varchar(20)  NULL,
    [SubstanceExpirationDate] datetime  NULL,
    [SubstanceManufacturer] varchar(250)  NULL,
    [Indication] varchar(250)  NULL,
    [DispensePackageSize] varchar(20)  NULL,
    [DispensePackageSizeUnits] varchar(250)  NULL,
    [DispensePackageMethod] char(2)  NULL,
    [SupplementaryCode] varchar(250)  NULL,
    [InitiatingLocation] varchar(250)  NULL,
    [AssemblyLocation] varchar(250)  NULL,
    [ActualDrugStrengthVolume] varchar(5)  NULL,
    [ActualDrugStrengthVolUnits] varchar(250)  NULL,
    [DispenseToPharmacy] varchar(180)  NULL,
    [DispenseToPharmacyAddress] varchar(106)  NULL,
    [PharmacyOrderType] char(1)  NULL,
    [DispenceType] varchar(250)  NULL,
    [PDispense_Status] int  NOT NULL,
    [PDispense_CreatedBy] int  NULL,
    [PDispense_CreatedDate] datetime  NOT NULL,
    [PTOutBoundFileStatus] int  NULL,
    [PTOutBoundApproval] int  NULL,
    [PTOutBoundApprovalBy] int  NULL,
    [PTOutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'TreatmentRouteInfoes'
CREATE TABLE [dbo].[TreatmentRouteInfoes] (
    [PRoute_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [RouteCode] varchar(100)  NULL,
    [RouteText] varchar(150)  NULL,
    [AdministrationSite] varchar(250)  NULL,
    [AdministrationDevice] varchar(250)  NULL,
    [AdministrationMethod] varchar(250)  NULL,
    [RoutingInstruction] varchar(250)  NULL,
    [AdminstrationSiteModifier] varchar(250)  NULL,
    [PRoute_Status] int  NOT NULL,
    [PRoute_CreatedBy] int  NULL,
    [PRoute_CreatedDate] datetime  NOT NULL,
    [PTROutBoundFileStatus] int  NULL,
    [PTROutBoundApproval] int  NULL,
    [PTROutBoundApprovalBy] int  NULL,
    [PTROutBoundApprovalOn] datetime  NULL
);
GO

-- Creating table 'OrderInactiveReasons'
CREATE TABLE [dbo].[OrderInactiveReasons] (
    [OIAReason_Id] int IDENTITY(1,1) NOT NULL,
    [OIAReason_Type] varchar(100)  NULL,
    [OIAReason_Status] int  NULL,
    [OIAReason_CreatedBy] int  NULL,
    [OIAReason_CreatedOn] datetime  NULL
);
GO

-- Creating table 'NurseStationHierarchies'
CREATE TABLE [dbo].[NurseStationHierarchies] (
    [NSHierarchy_Id] int IDENTITY(1,1) NOT NULL,
    [NurseStation_Id] int  NULL,
    [FloorPrior] int  NULL,
    [WingPrior] int  NULL,
    [RoomPrior] int  NULL,
    [BedPrior] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'FooterLogoes'
CREATE TABLE [dbo].[FooterLogoes] (
    [Footer_Id] int IDENTITY(1,1) NOT NULL,
    [Footer_Image] varbinary(max)  NULL,
    [Footer_Status] int  NOT NULL,
    [Footer_CreatedOn] datetime  NULL,
    [Footer_CreatedBy] int  NULL
);
GO

-- Creating table 'ControlSubstanceTrans'
CREATE TABLE [dbo].[ControlSubstanceTrans] (
    [TransCS_Id] int IDENTITY(1,1) NOT NULL,
    [ControlSubstance_Id] int  NULL,
    [Quantity] varchar(20)  NULL,
    [CertifiedBy] int  NULL,
    [ApprovedBy] int  NULL,
    [CertifiedDate] datetime  NULL,
    [TransCS_Status] int  NOT NULL,
    [TransCS_CreatedDate] datetime  NOT NULL,
    [ConsolidateFlag] int  NULL
);
GO

-- Creating table 'OrderStockTrans'
CREATE TABLE [dbo].[OrderStockTrans] (
    [TransOS_Id] int IDENTITY(1,1) NOT NULL,
    [OrderStock_Id] int  NULL,
    [Inhand] varchar(50)  NULL,
    [TransOS_CreatedBy] int  NULL,
    [TransOS_Status] int  NOT NULL,
    [TransOS_CreatedDate] datetime  NOT NULL,
    [LotNumber] varchar(50)  NULL,
    [ExpirationDate] datetime  NULL
);
GO

-- Creating table 'ControlSubstanceReasons'
CREATE TABLE [dbo].[ControlSubstanceReasons] (
    [CSReason_Id] bigint IDENTITY(1,1) NOT NULL,
    [ControlSubstance_Id] int  NULL,
    [Reason] varchar(500)  NULL,
    [CSReason_CreatedBy] int  NULL,
    [CSReason_CreatedDate] datetime  NULL
);
GO

-- Creating table 'ProcessKeyMasters'
CREATE TABLE [dbo].[ProcessKeyMasters] (
    [ProcessID] int IDENTITY(1,1) NOT NULL,
    [ProcessKey] varchar(50)  NULL,
    [ComputerName] varchar(50)  NULL,
    [Status] int  NULL,
    [ProcessID_CreatedBy] int  NULL,
    [ProcessID_CreatedOn] datetime  NULL,
    [NursingStation_Id] int  NULL
);
GO

-- Creating table 'QuantityDetails'
CREATE TABLE [dbo].[QuantityDetails] (
    [PQuantity_Id] int IDENTITY(1,1) NOT NULL,
    [POrder_Id] int  NOT NULL,
    [Quantity] varchar(20)  NULL,
    [RepeatPattern] varchar(540)  NULL,
    [ExplicitTime] varchar(60)  NULL,
    [RelativeTimeUnits] varchar(20)  NULL,
    [ServiceDuration] varchar(20)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Priority] varchar(250)  NULL,
    [ConditionText] varchar(250)  NULL,
    [TextInstruction] varchar(max)  NULL,
    [Conjunction] varchar(10)  NULL,
    [OccuranceDuration] varchar(20)  NULL,
    [TotalOccurances] varchar(10)  NULL,
    [PQuantity_Status] int  NOT NULL,
    [PQuantity_CreatedBy] int  NULL,
    [PQuantity_CreatedDate] datetime  NOT NULL,
    [EndDateStatus] int  NULL,
    [PRNFlag] bit  NULL,
    [ReviewFlag] int  NULL,
    [AlertText] varchar(500)  NULL,
    [MaxPerdays] int  NULL,
    [SelfAdministeredFlag] bit  NULL,
    [TreatmentFlag] bit  NULL,
    [InsulinComments] varchar(max)  NULL,
    [OrderStatus] int  NULL,
    [DiscontinueFlag] int  NULL,
    [DiscontinueReason] varchar(500)  NULL
);
GO

-- Creating table 'CertifyDates'
CREATE TABLE [dbo].[CertifyDates] (
    [CertifyTime_ID] int IDENTITY(1,1) NOT NULL,
    [CertifyDate1] datetime  NULL,
    [User_Id] int  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'CertifyOrders'
CREATE TABLE [dbo].[CertifyOrders] (
    [Certify_ID] bigint IDENTITY(1,1) NOT NULL,
    [CertifyTime_ID] int  NULL,
    [Porder_Id] int  NULL,
    [months] int  NULL,
    [status] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'MailBoxes'
CREATE TABLE [dbo].[MailBoxes] (
    [MailBox_Id] int IDENTITY(1,1) NOT NULL,
    [Fromuser_Id] int  NULL,
    [Touser_Id] varchar(50)  NULL,
    [ToMalId] varchar(50)  NULL,
    [CcUser_Id] varchar(50)  NULL,
    [Subject] varchar(500)  NULL,
    [MailBody] varchar(8000)  NULL,
    [AttachmentPath] varchar(8000)  NULL,
    [MailBox_Status] int  NULL,
    [MailBox_Date] datetime  NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'OrderFavConfigs'
CREATE TABLE [dbo].[OrderFavConfigs] (
    [OFConfig_Id] int IDENTITY(1,1) NOT NULL,
    [Facility_Id] int  NULL,
    [OrderFavCode] varchar(10)  NULL,
    [OrderFavMaster_ID] int  NULL,
    [OFConfig_Status] int  NULL,
    [OFConfig_CreatedBy] int  NULL,
    [OFConfig_CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblSideEffects'
CREATE TABLE [dbo].[tblSideEffects] (
    [SE_Id] int IDENTITY(1,1) NOT NULL,
    [GPI] varchar(50)  NULL,
    [SideEffects] nvarchar(max)  NULL,
    [Status] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblDrugDiscardInfoes'
CREATE TABLE [dbo].[tblDrugDiscardInfoes] (
    [DDiscard_Id] bigint IDENTITY(1,1) NOT NULL,
    [DAdmin_Id] bigint  NULL,
    [DiscardDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblAdministeredSites'
CREATE TABLE [dbo].[tblAdministeredSites] (
    [ASites_Id] bigint IDENTITY(1,1) NOT NULL,
    [DAdmin_Id] bigint  NULL,
    [Route] varchar(50)  NULL,
    [Site_Id] int  NULL,
    [AdministeredDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL,
    [DrugAdminister_Id] bigint  NULL
);
GO

-- Creating table 'tblRDCMailConfigs'
CREATE TABLE [dbo].[tblRDCMailConfigs] (
    [Rdc_Id] int IDENTITY(1,1) NOT NULL,
    [Facility_Id] int  NULL,
    [NurseStation_Id] int  NULL,
    [MailTo] varchar(500)  NULL,
    [MailCC] varchar(max)  NULL,
    [Status] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'tblRdcMailTimes'
CREATE TABLE [dbo].[tblRdcMailTimes] (
    [RdcTime_Id] int IDENTITY(1,1) NOT NULL,
    [Rdc_Id] int  NULL,
    [Time_Id] int  NULL
);
GO

-- Creating table 'OrderHOAInfoes'
CREATE TABLE [dbo].[OrderHOAInfoes] (
    [Hoa_Id] int IDENTITY(1,1) NOT NULL,
    [Porder_Id] int  NULL,
    [PQuantity_Id] int  NULL,
    [HoaMessage] varchar(max)  NULL
);
GO

-- Creating table 'tblTimeZones'
CREATE TABLE [dbo].[tblTimeZones] (
    [TZ_Id] int IDENTITY(1,1) NOT NULL,
    [ZoneDesc] varchar(50)  NULL,
    [ZoneCode] varchar(50)  NULL,
    [OffSetTime] varchar(50)  NULL,
    [Dst] int  NULL,
    [Status] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'PCertifyDates'
CREATE TABLE [dbo].[PCertifyDates] (
    [PCertifyTime_ID] int IDENTITY(1,1) NOT NULL,
    [PCertifyDate1] datetime  NULL,
    [PUser_Id] int  NULL,
    [PStatus] int  NULL
);
GO

-- Creating table 'PCertifyOrders'
CREATE TABLE [dbo].[PCertifyOrders] (
    [PCertify_ID] bigint IDENTITY(1,1) NOT NULL,
    [PCertifyTime_ID] int  NULL,
    [PPorder_Id] int  NULL,
    [Pmonths] int  NULL,
    [Pstatus] int  NULL,
    [PCreatedDate] datetime  NULL
);
GO

-- Creating table 'EkitControlSubstanceReasons'
CREATE TABLE [dbo].[EkitControlSubstanceReasons] (
    [EkCSReason_Id] bigint IDENTITY(1,1) NOT NULL,
    [Ekit_Id] int  NULL,
    [Reason] varchar(500)  NULL,
    [EkCSReason_CreatedBy] int  NULL,
    [EkCSReason_CreatedDate] datetime  NULL
);
GO

-- Creating table 'EKitControlSubstanceTrans'
CREATE TABLE [dbo].[EKitControlSubstanceTrans] (
    [EkTransCS_Id] int IDENTITY(1,1) NOT NULL,
    [Ekit_Id] int  NULL,
    [Quantity] varchar(20)  NULL,
    [CertifiedBy] int  NULL,
    [ApprovedBy] int  NULL,
    [CertifiedDate] datetime  NULL,
    [EkTransCS_Status] int  NOT NULL,
    [EkTransCS_CreatedDate] datetime  NOT NULL
);
GO

-- Creating table 'CpoeSources'
CREATE TABLE [dbo].[CpoeSources] (
    [Source_Id] int IDENTITY(1,1) NOT NULL,
    [Source_Desc] varchar(50)  NULL,
    [Source_Status] int  NULL,
    [Source_CreatedBy] int  NULL,
    [Source_CreatedDate] datetime  NULL
);
GO

-- Creating table 'QuantityDoses'
CREATE TABLE [dbo].[QuantityDoses] (
    [DQty_Id] int IDENTITY(1,1) NOT NULL,
    [DQty_Desc] varchar(50)  NULL,
    [DQty_Status] int  NULL,
    [DQty_CreatedBy] int  NULL,
    [DQty_CreatedDate] datetime  NULL
);
GO

-- Creating table 'UnitMeasurements'
CREATE TABLE [dbo].[UnitMeasurements] (
    [Uom_Id] int IDENTITY(1,1) NOT NULL,
    [Uom_Desc] varchar(50)  NULL,
    [Uom_Status] int  NULL,
    [Uom_CreatedBy] int  NULL,
    [Uom_CreatedDate] datetime  NULL
);
GO

-- Creating table 'DoseUoms'
CREATE TABLE [dbo].[DoseUoms] (
    [Dose_Id] int IDENTITY(1,1) NOT NULL,
    [Dose_Desc] varchar(50)  NULL,
    [Status] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'DrugAdministers'
CREATE TABLE [dbo].[DrugAdministers] (
    [DrugAdminister_Id] bigint IDENTITY(1,1) NOT NULL,
    [DAdmin_Id] int  NULL,
    [Porder_Id] int  NULL,
    [PQuantity_Id] int  NULL,
    [AdminsterSchedule] datetime  NULL,
    [NurseShifts_Id] int  NULL,
    [AdministerComment] varchar(max)  NULL,
    [PRNComment] varchar(max)  NULL,
    [SeventyTwoComment] varchar(max)  NULL,
    [MedicationReason_ID] int  NULL,
    [AdminsterStatus] int  NULL,
    [AdminsterBy] int  NULL,
    [AdminsterOn] datetime  NULL,
    [PRNCommentBy] int  NULL,
    [PRNCommentOn] datetime  NULL,
    [SeventyTwoCommentBy] int  NULL,
    [SeventyTwoCommentOn] datetime  NULL,
    [BiometricBypass] int  NULL,
    [BiometricBypassReason] varchar(max)  NULL,
    [BCScanner] int  NULL,
    [DrugQuantity] decimal(19,4)  NULL,
    [ManualDocumentedBy] int  NULL,
    [ManualDocumentedDate] datetime  NULL,
    [NotificationStat] int  NULL,
    [EkitAdminister] int  NULL,
    [additionalcomments] nvarchar(max)  NULL,
    [Barcode] varchar(50)  NULL,
    [Ekit_Id] int  NULL
);
GO

-- Creating table 'CommonOrderInfoes'
CREATE TABLE [dbo].[CommonOrderInfoes] (
    [POrder_Id] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [OrderingPhysicianID] int  NULL,
    [OrderControl] varchar(2)  NULL,
    [PlacerOrderNumber] varchar(22)  NULL,
    [FacilityId] varchar(50)  NULL,
    [PatientId] varchar(50)  NULL,
    [Room] varchar(50)  NULL,
    [OrderTypeID] int  NULL,
    [PlacerGroupNumber] varchar(22)  NULL,
    [OrderStatus] varchar(2)  NULL,
    [ResponseFlag] varchar(1)  NULL,
    [QuantityTiming] varchar(200)  NULL,
    [Parent] varchar(200)  NULL,
    [TransactionDate] datetime  NULL,
    [EnteredBy] varchar(50)  NULL,
    [EPharmacistLName] varchar(50)  NULL,
    [EPharmacistFName] varchar(50)  NULL,
    [VerifiedBy] varchar(50)  NULL,
    [VPharmacistLName] varchar(50)  NULL,
    [VPharmacistFName] varchar(50)  NULL,
    [VEffectivedate] datetime  NULL,
    [OrderingPhysicianNPI] varchar(20)  NULL,
    [OPhysicianLname] varchar(50)  NULL,
    [OPhysicianFname] varchar(50)  NULL,
    [EntererLocation] varchar(80)  NULL,
    [CallBackPhoneNumber] varchar(250)  NULL,
    [OrderEffectiveDate] datetime  NULL,
    [OrderControlCodeReason] varchar(250)  NULL,
    [EnteringOrganisation] varchar(250)  NULL,
    [EnteringDevice] varchar(250)  NULL,
    [AltCodingSystem] varchar(250)  NULL,
    [AdvBeneficiaryNoticeCode] varchar(250)  NULL,
    [OrderingFacilityName] varchar(250)  NULL,
    [OrderingFacilityAddress1] varchar(100)  NULL,
    [OrderingFacilityAddress2] varchar(100)  NULL,
    [OrderingFacilityCity] varchar(50)  NULL,
    [OrderingFacilityState] varchar(50)  NULL,
    [OrderingFacilityZip] varchar(20)  NULL,
    [OrderingFacilityPhone] varchar(250)  NULL,
    [OrderingPhysicianAddress1] varchar(100)  NULL,
    [OrderingPhysicianAddress2] varchar(100)  NULL,
    [OrderingPhysicianCity] varchar(50)  NULL,
    [OrderingPhysicianState] varchar(50)  NULL,
    [OrderingProviderZip] varchar(20)  NULL,
    [OrderStatusModifier] varchar(250)  NULL,
    [AdvBeneficiaryNoticeOverrideReason] varchar(60)  NULL,
    [ExpectedAvailabilityDate] datetime  NULL,
    [ConfidentialityCode] varchar(250)  NULL,
    [OrderType] varchar(250)  NULL,
    [EntererAuthorizationMode] varchar(250)  NULL,
    [POrder_Status] int  NOT NULL,
    [POrder_CreatedBy] int  NULL,
    [POrder_CreatedDate] datetime  NOT NULL,
    [POOutBoundFileStatus] int  NULL,
    [POOutBoundApproval] int  NULL,
    [POOutBoundApprovalBy] int  NULL,
    [POOutBoundApprovalOn] datetime  NULL,
    [OrderStockFlag] bit  NULL,
    [DrFirstPrescriptionId] varchar(50)  NULL,
    [File_Id] int  NULL,
    [OldPatient_Id] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [Daw] int  NULL,
    [DispenseQty] decimal(18,2)  NULL,
    [WrittenDate] datetime  NULL,
    [DiagIndication] int  NULL,
    [WaitforPharmacy] int  NULL,
    [Hospice] int  NULL,
    [Source] int  NULL,
    [UOM] int  NULL,
    [DiagIndicationText] varchar(250)  NULL
);
GO

-- Creating table 'WeightLogs'
CREATE TABLE [dbo].[WeightLogs] (
    [WeightLog_ID] int IDENTITY(1,1) NOT NULL,
    [Patient_Id] int  NOT NULL,
    [Weight] decimal(10,2)  NULL,
    [Type] varchar(50)  NULL,
    [HeightFeet] varchar(10)  NULL,
    [HeightInc] varchar(10)  NULL,
    [DateTime] datetime  NULL,
    [PreDialysis] int  NULL,
    [PostDialysis] int  NULL,
    [Remarks] varchar(8000)  NULL,
    [IBW] varchar(50)  NULL,
    [initials] varchar(50)  NULL,
    [WeightLog_Status] int  NOT NULL,
    [WeightLog_CreatedBy] int  NULL,
    [WeightLog_CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'ApprovalOrders'
CREATE TABLE [dbo].[ApprovalOrders] (
    [PApprovalOrder_Id] bigint IDENTITY(1,1) NOT NULL,
    [Porder_Id] int  NULL,
    [Patient_Id] int  NULL,
    [OrderingPhysicianID] int  NULL,
    [OrderControl] varchar(2)  NULL,
    [OrderTypeID] int  NULL,
    [TransactionDate] datetime  NULL,
    [OrderEffectiveDate] datetime  NULL,
    [POrder_Status] int  NULL,
    [POrder_CreatedBy] int  NULL,
    [POrder_CreatedDate] datetime  NULL,
    [AlertText] varchar(500)  NULL,
    [MaxPerdays] int  NULL,
    [OrderStockFlag] bit  NULL,
    [PRNFlag] bit  NULL,
    [SelfAdministeredFlag] bit  NULL,
    [TreatmentFlag] bit  NULL,
    [Maysubstitute] bit  NULL,
    [InsulinComments] varchar(max)  NULL,
    [RouteCode] int  NULL,
    [Quantity] varchar(20)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [RequestedGiveCode] varchar(125)  NULL,
    [GiveCodeText] varchar(60)  NULL,
    [ProviderAdminDrugInsText] varchar(250)  NULL,
    [NumberOfRefills] varchar(3)  NULL,
    [NumberOfRefillsRemaining] varchar(20)  NULL,
    [POOutBoundFileStatus] int  NULL,
    [POOutBoundApproval] int  NULL,
    [POOutBoundApprovalBy] int  NULL,
    [POOutBoundApprovalOn] datetime  NULL,
    [AdministrationType] int  NULL,
    [NursingFreq_Id] int  NULL,
    [NurseShifts_Id] varchar(50)  NULL,
    [Hour_Id] varchar(250)  NULL,
    [TimeFormat_Id] int  NULL,
    [Hours] int  NULL,
    [Monday] bit  NULL,
    [Tuesday] bit  NULL,
    [Wednesday] bit  NULL,
    [Thursday] bit  NULL,
    [Friday] bit  NULL,
    [Saturday] bit  NULL,
    [Sunday] bit  NULL,
    [Week_Id] int  NULL,
    [Month_Id] int  NULL,
    [OnlyOnDay] int  NULL,
    [ThroughDay] int  NULL,
    [ActiveDays] int  NULL,
    [HoldDays] int  NULL,
    [InHand] int  NULL,
    [Stock_Id] int  NULL,
    [Barcode] varchar(150)  NULL,
    [Days] varchar(500)  NULL,
    [Favourites] varchar(500)  NULL,
    [ControlSubstanceBit] int  NULL,
    [Notes] nvarchar(500)  NULL,
    [Daw] int  NULL,
    [DispenseQty] decimal(18,2)  NULL,
    [WrittenDate] datetime  NULL,
    [DiagIndication] int  NULL,
    [WaitforPharmacy] int  NULL,
    [Hospice] int  NULL,
    [Source] int  NULL,
    [UOM] int  NULL,
    [Sig2Quantity] varchar(50)  NULL,
    [Sig2NursingFreq_Id] int  NULL,
    [Sig2NurseShifts_Id] varchar(150)  NULL,
    [Sig2AddInsText] varchar(250)  NULL,
    [Sig2MaxPerdays] int  NULL,
    [Sig2PRNFlag] int  NULL,
    [Sig3Quantity] varchar(50)  NULL,
    [Sig3NursingFreq_Id] int  NULL,
    [Sig3NurseShifts_Id] varchar(150)  NULL,
    [Sig3AddInsText] varchar(250)  NULL,
    [Sig3MaxPerdays] int  NULL,
    [Sig3PRNFlag] int  NULL,
    [Sig4Quantity] varchar(50)  NULL,
    [Sig4NursingFreq_Id] int  NULL,
    [Sig4NurseShifts_Id] varchar(150)  NULL,
    [Sig4AddInsText] varchar(250)  NULL,
    [Sig4MaxPerdays] int  NULL,
    [Sig4PRNFlag] int  NULL,
    [DiagIndicationText] varchar(250)  NULL,
    [DUom] int  NULL,
    [Sig2DUom] int  NULL,
    [Sig3DUom] int  NULL,
    [Sig4DUom] int  NULL
);
GO

-- Creating table 'Demographics'
CREATE TABLE [dbo].[Demographics] (
    [Patient_Id] int IDENTITY(1,1) NOT NULL,
    [ExternalPatientId] varchar(20)  NULL,
    [ExternalFacShortName] varchar(50)  NULL,
    [ExternalFacPatientId] varchar(50)  NULL,
    [AlternatePatientId] int  NULL,
    [PatientLastName] varchar(100)  NULL,
    [PatientFirstName] varchar(100)  NULL,
    [PatientMiddleInitial] varchar(1)  NULL,
    [NameTypeCode] varchar(1)  NULL,
    [MotherMaidenName] varchar(250)  NULL,
    [DOB] datetime  NULL,
    [AdministrativeSex] varchar(1)  NULL,
    [PatientAlias] varchar(250)  NULL,
    [Race] varchar(250)  NULL,
    [PatientAddress1] varchar(100)  NULL,
    [PatientAddress2] varchar(100)  NULL,
    [PatientCity] varchar(50)  NULL,
    [PatientState] varchar(50)  NULL,
    [PatientZipCode] varchar(20)  NULL,
    [CountyCode] varchar(10)  NULL,
    [PhoneHome] varchar(50)  NULL,
    [PhoneBusiness] varchar(50)  NULL,
    [PrimaryLanguage] varchar(50)  NULL,
    [MaritalStatus] varchar(50)  NULL,
    [Religion] varchar(50)  NULL,
    [PatientMRNumber] varchar(50)  NULL,
    [SSN] varchar(20)  NULL,
    [DriverLicense] varchar(50)  NULL,
    [MotherIdentifier] varchar(250)  NULL,
    [EthnicGroup] varchar(50)  NULL,
    [BirthPlace] varchar(50)  NULL,
    [MultipleBirthIndicator] varchar(50)  NULL,
    [BirthOrder] varchar(50)  NULL,
    [Citizenship] varchar(50)  NULL,
    [MilitaryStatus] varchar(50)  NULL,
    [Nationality] varchar(50)  NULL,
    [DeathDateTime] datetime  NULL,
    [DeathIndicator] varchar(1)  NULL,
    [IdentityIndicator] varchar(1)  NULL,
    [IdentityReliability] varchar(1)  NULL,
    [LastUpdate] varchar(26)  NULL,
    [LastFacilityUpdate] varchar(26)  NULL,
    [SpeciesCode] varchar(50)  NULL,
    [BreedCode] varchar(50)  NULL,
    [Strain] varchar(50)  NULL,
    [ProductionClassCode] varchar(50)  NULL,
    [TribalCitizenship] varchar(50)  NULL,
    [ImageLocation] varchar(max)  NULL,
    [Patient_Status] int  NOT NULL,
    [Patient_CreatedBy] int  NULL,
    [Patient_CreatedDate] datetime  NOT NULL,
    [PDOutBoundFileStatus] int  NULL,
    [PDOutBoundApproval] int  NULL,
    [PDOutBoundApprovalBy] int  NULL,
    [PDOutBoundApprovalOn] datetime  NULL,
    [Alert] varchar(150)  NULL,
    [Diet] varchar(150)  NULL,
    [BiometricInfo] varchar(8000)  NULL,
    [PatientType_Id] int  NULL,
    [MergeID] int  NULL,
    [AliasName] varchar(150)  NULL,
    [MergeDate] datetime  NULL,
    [Pregnant] int  NULL,
    [BreastFeeding] int  NULL
);
GO

-- Creating table 'ApprovalOrder1'
CREATE TABLE [dbo].[ApprovalOrder1] (
    [PApprovalOrder_Id] bigint IDENTITY(1,1) NOT NULL,
    [Porder_Id] int  NULL,
    [Patient_Id] int  NULL,
    [OrderingPhysicianID] int  NULL,
    [OrderControl] varchar(2)  NULL,
    [OrderTypeID] int  NULL,
    [TransactionDate] datetime  NULL,
    [OrderEffectiveDate] datetime  NULL,
    [POrder_Status] int  NULL,
    [POrder_CreatedBy] int  NULL,
    [POrder_CreatedDate] datetime  NULL,
    [AlertText] varchar(500)  NULL,
    [MaxPerdays] decimal(18,3)  NULL,
    [OrderStockFlag] bit  NULL,
    [PRNFlag] bit  NULL,
    [SelfAdministeredFlag] bit  NULL,
    [TreatmentFlag] bit  NULL,
    [Maysubstitute] bit  NULL,
    [InsulinComments] varchar(max)  NULL,
    [RouteCode] int  NULL,
    [Quantity] varchar(20)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [RequestedGiveCode] varchar(125)  NULL,
    [GiveCodeText] varchar(60)  NULL,
    [ProviderAdminDrugInsText] varchar(250)  NULL,
    [NumberOfRefills] varchar(3)  NULL,
    [NumberOfRefillsRemaining] varchar(20)  NULL,
    [POOutBoundFileStatus] int  NULL,
    [POOutBoundApproval] int  NULL,
    [POOutBoundApprovalBy] int  NULL,
    [POOutBoundApprovalOn] datetime  NULL,
    [AdministrationType] int  NULL,
    [NursingFreq_Id] int  NULL,
    [NurseShifts_Id] varchar(50)  NULL,
    [Hour_Id] varchar(250)  NULL,
    [TimeFormat_Id] int  NULL,
    [Hours] int  NULL,
    [Monday] bit  NULL,
    [Tuesday] bit  NULL,
    [Wednesday] bit  NULL,
    [Thursday] bit  NULL,
    [Friday] bit  NULL,
    [Saturday] bit  NULL,
    [Sunday] bit  NULL,
    [Week_Id] int  NULL,
    [Month_Id] int  NULL,
    [OnlyOnDay] int  NULL,
    [ThroughDay] int  NULL,
    [ActiveDays] int  NULL,
    [HoldDays] int  NULL,
    [InHand] int  NULL,
    [Stock_Id] int  NULL,
    [Barcode] varchar(150)  NULL,
    [Days] varchar(500)  NULL,
    [Favourites] varchar(500)  NULL,
    [ControlSubstanceBit] int  NULL,
    [Notes] nvarchar(500)  NULL,
    [Daw] int  NULL,
    [DispenseQty] decimal(18,2)  NULL,
    [WrittenDate] datetime  NULL,
    [DiagIndication] int  NULL,
    [WaitforPharmacy] int  NULL,
    [Hospice] int  NULL,
    [Source] int  NULL,
    [UOM] int  NULL,
    [Sig2Quantity] varchar(50)  NULL,
    [Sig2NursingFreq_Id] int  NULL,
    [Sig2NurseShifts_Id] varchar(150)  NULL,
    [Sig2AddInsText] varchar(250)  NULL,
    [Sig2MaxPerdays] int  NULL,
    [Sig2PRNFlag] int  NULL,
    [Sig3Quantity] varchar(50)  NULL,
    [Sig3NursingFreq_Id] int  NULL,
    [Sig3NurseShifts_Id] varchar(150)  NULL,
    [Sig3AddInsText] varchar(250)  NULL,
    [Sig3MaxPerdays] int  NULL,
    [Sig3PRNFlag] int  NULL,
    [Sig4Quantity] varchar(50)  NULL,
    [Sig4NursingFreq_Id] int  NULL,
    [Sig4NurseShifts_Id] varchar(150)  NULL,
    [Sig4AddInsText] varchar(250)  NULL,
    [Sig4MaxPerdays] int  NULL,
    [Sig4PRNFlag] int  NULL,
    [DiagIndicationText] varchar(250)  NULL,
    [DUom] int  NULL,
    [Sig2DUom] int  NULL,
    [Sig3DUom] int  NULL,
    [Sig4DUom] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AllergyType_Id] in table 'AllergyTypeCodes'
ALTER TABLE [dbo].[AllergyTypeCodes]
ADD CONSTRAINT [PK_AllergyTypeCodes]
    PRIMARY KEY CLUSTERED ([AllergyType_Id] ASC);
GO

-- Creating primary key on [Integration_Id] in table 'ApiIntegrations'
ALTER TABLE [dbo].[ApiIntegrations]
ADD CONSTRAINT [PK_ApiIntegrations]
    PRIMARY KEY CLUSTERED ([Integration_Id] ASC);
GO

-- Creating primary key on [Bed_Id] in table 'Beds'
ALTER TABLE [dbo].[Beds]
ADD CONSTRAINT [PK_Beds]
    PRIMARY KEY CLUSTERED ([Bed_Id] ASC);
GO

-- Creating primary key on [BehavioralSym_ID] in table 'BehavioralSymptomsMasters'
ALTER TABLE [dbo].[BehavioralSymptomsMasters]
ADD CONSTRAINT [PK_BehavioralSymptomsMasters]
    PRIMARY KEY CLUSTERED ([BehavioralSym_ID] ASC);
GO

-- Creating primary key on [CDCodingType_Id] in table 'ClassDrugCodingTypes'
ALTER TABLE [dbo].[ClassDrugCodingTypes]
ADD CONSTRAINT [PK_ClassDrugCodingTypes]
    PRIMARY KEY CLUSTERED ([CDCodingType_Id] ASC);
GO

-- Creating primary key on [BedConfig_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [PK_CompanyBedConfigs]
    PRIMARY KEY CLUSTERED ([BedConfig_Id] ASC);
GO

-- Creating primary key on [Country_Id] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([Country_Id] ASC);
GO

-- Creating primary key on [DCodingType_Id] in table 'DiagnosisCodingTypes'
ALTER TABLE [dbo].[DiagnosisCodingTypes]
ADD CONSTRAINT [PK_DiagnosisCodingTypes]
    PRIMARY KEY CLUSTERED ([DCodingType_Id] ASC);
GO

-- Creating primary key on [FolderID] in table 'DocFolders'
ALTER TABLE [dbo].[DocFolders]
ADD CONSTRAINT [PK_DocFolders]
    PRIMARY KEY CLUSTERED ([FolderID] ASC);
GO

-- Creating primary key on [EventCat_Id] in table 'EventCategories'
ALTER TABLE [dbo].[EventCategories]
ADD CONSTRAINT [PK_EventCategories]
    PRIMARY KEY CLUSTERED ([EventCat_Id] ASC);
GO

-- Creating primary key on [FileAckInformation_Id] in table 'FileAckInformations'
ALTER TABLE [dbo].[FileAckInformations]
ADD CONSTRAINT [PK_FileAckInformations]
    PRIMARY KEY CLUSTERED ([FileAckInformation_Id] ASC);
GO

-- Creating primary key on [File_Id] in table 'FileInformations'
ALTER TABLE [dbo].[FileInformations]
ADD CONSTRAINT [PK_FileInformations]
    PRIMARY KEY CLUSTERED ([File_Id] ASC);
GO

-- Creating primary key on [Floor_Id] in table 'Floors'
ALTER TABLE [dbo].[Floors]
ADD CONSTRAINT [PK_Floors]
    PRIMARY KEY CLUSTERED ([Floor_Id] ASC);
GO

-- Creating primary key on [Frequency_Id] in table 'FrequencyMasters'
ALTER TABLE [dbo].[FrequencyMasters]
ADD CONSTRAINT [PK_FrequencyMasters]
    PRIMARY KEY CLUSTERED ([Frequency_Id] ASC);
GO

-- Creating primary key on [FteCategory_Id] in table 'FTECategories'
ALTER TABLE [dbo].[FTECategories]
ADD CONSTRAINT [PK_FTECategories]
    PRIMARY KEY CLUSTERED ([FteCategory_Id] ASC);
GO

-- Creating primary key on [FteConn_Id] in table 'FteConnections'
ALTER TABLE [dbo].[FteConnections]
ADD CONSTRAINT [PK_FteConnections]
    PRIMARY KEY CLUSTERED ([FteConn_Id] ASC);
GO

-- Creating primary key on [Gender_Id] in table 'Genders'
ALTER TABLE [dbo].[Genders]
ADD CONSTRAINT [PK_Genders]
    PRIMARY KEY CLUSTERED ([Gender_Id] ASC);
GO

-- Creating primary key on [HLConfig_Id] in table 'HLSevenCompanyConfigs'
ALTER TABLE [dbo].[HLSevenCompanyConfigs]
ADD CONSTRAINT [PK_HLSevenCompanyConfigs]
    PRIMARY KEY CLUSTERED ([HLConfig_Id] ASC);
GO

-- Creating primary key on [SegDetail_Id] in table 'HLSevenSegmentDetails'
ALTER TABLE [dbo].[HLSevenSegmentDetails]
ADD CONSTRAINT [PK_HLSevenSegmentDetails]
    PRIMARY KEY CLUSTERED ([SegDetail_Id] ASC);
GO

-- Creating primary key on [Segment_Id] in table 'HLSevenSegments'
ALTER TABLE [dbo].[HLSevenSegments]
ADD CONSTRAINT [PK_HLSevenSegments]
    PRIMARY KEY CLUSTERED ([Segment_Id] ASC);
GO

-- Creating primary key on [Hour_Id] in table 'Hours'
ALTER TABLE [dbo].[Hours]
ADD CONSTRAINT [PK_Hours]
    PRIMARY KEY CLUSTERED ([Hour_Id] ASC);
GO

-- Creating primary key on [ICD10_Id] in table 'ICD10'
ALTER TABLE [dbo].[ICD10]
ADD CONSTRAINT [PK_ICD10]
    PRIMARY KEY CLUSTERED ([ICD10_Id] ASC);
GO

-- Creating primary key on [ImportFile_Id] in table 'ImportFiles'
ALTER TABLE [dbo].[ImportFiles]
ADD CONSTRAINT [PK_ImportFiles]
    PRIMARY KEY CLUSTERED ([ImportFile_Id] ASC);
GO

-- Creating primary key on [Integration_TypeId] in table 'IntegrationTypes'
ALTER TABLE [dbo].[IntegrationTypes]
ADD CONSTRAINT [PK_IntegrationTypes]
    PRIMARY KEY CLUSTERED ([Integration_TypeId] ASC);
GO

-- Creating primary key on [Marital_id] in table 'MaritalStatus'
ALTER TABLE [dbo].[MaritalStatus]
ADD CONSTRAINT [PK_MaritalStatus]
    PRIMARY KEY CLUSTERED ([Marital_id] ASC);
GO

-- Creating primary key on [MedicationReason_ID] in table 'MedicationReasons'
ALTER TABLE [dbo].[MedicationReasons]
ADD CONSTRAINT [PK_MedicationReasons]
    PRIMARY KEY CLUSTERED ([MedicationReason_ID] ASC);
GO

-- Creating primary key on [Month_Id] in table 'Months'
ALTER TABLE [dbo].[Months]
ADD CONSTRAINT [PK_Months]
    PRIMARY KEY CLUSTERED ([Month_Id] ASC);
GO

-- Creating primary key on [NursingSchedule_Id] in table 'NursingSchedules'
ALTER TABLE [dbo].[NursingSchedules]
ADD CONSTRAINT [PK_NursingSchedules]
    PRIMARY KEY CLUSTERED ([NursingSchedule_Id] ASC);
GO

-- Creating primary key on [NurseStation_Id] in table 'NursingStations'
ALTER TABLE [dbo].[NursingStations]
ADD CONSTRAINT [PK_NursingStations]
    PRIMARY KEY CLUSTERED ([NurseStation_Id] ASC);
GO

-- Creating primary key on [OrderControl_ID] in table 'OrderControlMasters'
ALTER TABLE [dbo].[OrderControlMasters]
ADD CONSTRAINT [PK_OrderControlMasters]
    PRIMARY KEY CLUSTERED ([OrderControl_ID] ASC);
GO

-- Creating primary key on [OrderFavMaster_ID] in table 'OrderFavouriteMasters'
ALTER TABLE [dbo].[OrderFavouriteMasters]
ADD CONSTRAINT [PK_OrderFavouriteMasters]
    PRIMARY KEY CLUSTERED ([OrderFavMaster_ID] ASC);
GO

-- Creating primary key on [OrderTypeID] in table 'OrderTypes'
ALTER TABLE [dbo].[OrderTypes]
ADD CONSTRAINT [PK_OrderTypes]
    PRIMARY KEY CLUSTERED ([OrderTypeID] ASC);
GO

-- Creating primary key on [RoleConfig_Id] in table 'RoleConfigs'
ALTER TABLE [dbo].[RoleConfigs]
ADD CONSTRAINT [PK_RoleConfigs]
    PRIMARY KEY CLUSTERED ([RoleConfig_Id] ASC);
GO

-- Creating primary key on [Room_Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([Room_Id] ASC);
GO

-- Creating primary key on [Screen_Id] in table 'Screens'
ALTER TABLE [dbo].[Screens]
ADD CONSTRAINT [PK_Screens]
    PRIMARY KEY CLUSTERED ([Screen_Id] ASC);
GO

-- Creating primary key on [Suffix_Id] in table 'Suffixes'
ALTER TABLE [dbo].[Suffixes]
ADD CONSTRAINT [PK_Suffixes]
    PRIMARY KEY CLUSTERED ([Suffix_Id] ASC);
GO

-- Creating primary key on [TimeFormat_Id] in table 'TimeFormats'
ALTER TABLE [dbo].[TimeFormats]
ADD CONSTRAINT [PK_TimeFormats]
    PRIMARY KEY CLUSTERED ([TimeFormat_Id] ASC);
GO

-- Creating primary key on [User_Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([User_Id] ASC);
GO

-- Creating primary key on [UserRole_Id] in table 'UserRoleFacilityConfigs'
ALTER TABLE [dbo].[UserRoleFacilityConfigs]
ADD CONSTRAINT [PK_UserRoleFacilityConfigs]
    PRIMARY KEY CLUSTERED ([UserRole_Id] ASC);
GO

-- Creating primary key on [Weekdays_ID] in table 'WeekDays'
ALTER TABLE [dbo].[WeekDays]
ADD CONSTRAINT [PK_WeekDays]
    PRIMARY KEY CLUSTERED ([Weekdays_ID] ASC);
GO

-- Creating primary key on [Week_Id] in table 'Weeks'
ALTER TABLE [dbo].[Weeks]
ADD CONSTRAINT [PK_Weeks]
    PRIMARY KEY CLUSTERED ([Week_Id] ASC);
GO

-- Creating primary key on [Wing_Id] in table 'Wings'
ALTER TABLE [dbo].[Wings]
ADD CONSTRAINT [PK_Wings]
    PRIMARY KEY CLUSTERED ([Wing_Id] ASC);
GO

-- Creating primary key on [PAddl_Id] in table 'AddlInstructionDetails'
ALTER TABLE [dbo].[AddlInstructionDetails]
ADD CONSTRAINT [PK_AddlInstructionDetails]
    PRIMARY KEY CLUSTERED ([PAddl_Id] ASC);
GO

-- Creating primary key on [PAnc_Id] in table 'AncillaryDetails'
ALTER TABLE [dbo].[AncillaryDetails]
ADD CONSTRAINT [PK_AncillaryDetails]
    PRIMARY KEY CLUSTERED ([PAnc_Id] ASC);
GO

-- Creating primary key on [PComp_Id] in table 'CompoundOrders'
ALTER TABLE [dbo].[CompoundOrders]
ADD CONSTRAINT [PK_CompoundOrders]
    PRIMARY KEY CLUSTERED ([PComp_Id] ASC);
GO

-- Creating primary key on [PEncOrder_Id] in table 'EncodedOrderDetails'
ALTER TABLE [dbo].[EncodedOrderDetails]
ADD CONSTRAINT [PK_EncodedOrderDetails]
    PRIMARY KEY CLUSTERED ([PEncOrder_Id] ASC);
GO

-- Creating primary key on [PatIns_Id] in table 'InsuranceInfoes'
ALTER TABLE [dbo].[InsuranceInfoes]
ADD CONSTRAINT [PK_InsuranceInfoes]
    PRIMARY KEY CLUSTERED ([PatIns_Id] ASC);
GO

-- Creating primary key on [PNote_id] in table 'NotesInfoes'
ALTER TABLE [dbo].[NotesInfoes]
ADD CONSTRAINT [PK_NotesInfoes]
    PRIMARY KEY CLUSTERED ([PNote_id] ASC);
GO

-- Creating primary key on [PObs_Id] in table 'Observations'
ALTER TABLE [dbo].[Observations]
ADD CONSTRAINT [PK_Observations]
    PRIMARY KEY CLUSTERED ([PObs_Id] ASC);
GO

-- Creating primary key on [File_Id] in table 'OutBoundFileInformations'
ALTER TABLE [dbo].[OutBoundFileInformations]
ADD CONSTRAINT [PK_OutBoundFileInformations]
    PRIMARY KEY CLUSTERED ([File_Id] ASC);
GO

-- Creating primary key on [ResOrder_Id] in table 'ResidentOrders'
ALTER TABLE [dbo].[ResidentOrders]
ADD CONSTRAINT [PK_ResidentOrders]
    PRIMARY KEY CLUSTERED ([ResOrder_Id] ASC);
GO

-- Creating primary key on [PTreatment_Id] in table 'TreatmentInfoes'
ALTER TABLE [dbo].[TreatmentInfoes]
ADD CONSTRAINT [PK_TreatmentInfoes]
    PRIMARY KEY CLUSTERED ([PTreatment_Id] ASC);
GO

-- Creating primary key on [VisitBehaviour_ID] in table 'VisitBehaviours'
ALTER TABLE [dbo].[VisitBehaviours]
ADD CONSTRAINT [PK_VisitBehaviours]
    PRIMARY KEY CLUSTERED ([VisitBehaviour_ID] ASC);
GO

-- Creating primary key on [VisitFoodintake_ID] in table 'VisitFoodintakes'
ALTER TABLE [dbo].[VisitFoodintakes]
ADD CONSTRAINT [PK_VisitFoodintakes]
    PRIMARY KEY CLUSTERED ([VisitFoodintake_ID] ASC);
GO

-- Creating primary key on [PVisit_Id] in table 'VisitInfoes'
ALTER TABLE [dbo].[VisitInfoes]
ADD CONSTRAINT [PK_VisitInfoes]
    PRIMARY KEY CLUSTERED ([PVisit_Id] ASC);
GO

-- Creating primary key on [VisitNursingNotes_ID] in table 'VisitNursingNotes'
ALTER TABLE [dbo].[VisitNursingNotes]
ADD CONSTRAINT [PK_VisitNursingNotes]
    PRIMARY KEY CLUSTERED ([VisitNursingNotes_ID] ASC);
GO

-- Creating primary key on [Vitals_ID] in table 'VisitVitals'
ALTER TABLE [dbo].[VisitVitals]
ADD CONSTRAINT [PK_VisitVitals]
    PRIMARY KEY CLUSTERED ([Vitals_ID] ASC);
GO

-- Creating primary key on [PAllergy_Id] in table 'AllergyInfoes'
ALTER TABLE [dbo].[AllergyInfoes]
ADD CONSTRAINT [PK_AllergyInfoes]
    PRIMARY KEY CLUSTERED ([PAllergy_Id] ASC);
GO

-- Creating primary key on [PDiagnosis_Id] in table 'DiagnosisInfoes'
ALTER TABLE [dbo].[DiagnosisInfoes]
ADD CONSTRAINT [PK_DiagnosisInfoes]
    PRIMARY KEY CLUSTERED ([PDiagnosis_Id] ASC);
GO

-- Creating primary key on [Segmentcheck_Id] in table 'tmptblSegmentChecks'
ALTER TABLE [dbo].[tmptblSegmentChecks]
ADD CONSTRAINT [PK_tmptblSegmentChecks]
    PRIMARY KEY CLUSTERED ([Segmentcheck_Id] ASC);
GO

-- Creating primary key on [ID] in table 'MailConfigs'
ALTER TABLE [dbo].[MailConfigs]
ADD CONSTRAINT [PK_MailConfigs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [OrderDestroy_Id] in table 'OrderDestroys'
ALTER TABLE [dbo].[OrderDestroys]
ADD CONSTRAINT [PK_OrderDestroys]
    PRIMARY KEY CLUSTERED ([OrderDestroy_Id] ASC);
GO

-- Creating primary key on [ApprovalPAllergy_Id] in table 'ApprovalAllergyInfoes'
ALTER TABLE [dbo].[ApprovalAllergyInfoes]
ADD CONSTRAINT [PK_ApprovalAllergyInfoes]
    PRIMARY KEY CLUSTERED ([ApprovalPAllergy_Id] ASC);
GO

-- Creating primary key on [ApprovalPDiagnosis_Id] in table 'ApprovalDiagnosisInfoes'
ALTER TABLE [dbo].[ApprovalDiagnosisInfoes]
ADD CONSTRAINT [PK_ApprovalDiagnosisInfoes]
    PRIMARY KEY CLUSTERED ([ApprovalPDiagnosis_Id] ASC);
GO

-- Creating primary key on [ApprovalPVisit_Id] in table 'ApprovalVisitInfoes'
ALTER TABLE [dbo].[ApprovalVisitInfoes]
ADD CONSTRAINT [PK_ApprovalVisitInfoes]
    PRIMARY KEY CLUSTERED ([ApprovalPVisit_Id] ASC);
GO

-- Creating primary key on [ControlSubstance_Id] in table 'ControlSubstanceCounts'
ALTER TABLE [dbo].[ControlSubstanceCounts]
ADD CONSTRAINT [PK_ControlSubstanceCounts]
    PRIMARY KEY CLUSTERED ([ControlSubstance_Id] ASC);
GO

-- Creating primary key on [Conversation_Id] in table 'Conversations'
ALTER TABLE [dbo].[Conversations]
ADD CONSTRAINT [PK_Conversations]
    PRIMARY KEY CLUSTERED ([Conversation_Id] ASC);
GO

-- Creating primary key on [Messages_Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([Messages_Id] ASC);
GO

-- Creating primary key on [Participant_Id] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [PK_Participants]
    PRIMARY KEY CLUSTERED ([Participant_Id] ASC);
GO

-- Creating primary key on [User_Id] in table 'Users1'
ALTER TABLE [dbo].[Users1]
ADD CONSTRAINT [PK_Users1]
    PRIMARY KEY CLUSTERED ([User_Id] ASC);
GO

-- Creating primary key on [PatientDoc_Id] in table 'UploadedDocuments'
ALTER TABLE [dbo].[UploadedDocuments]
ADD CONSTRAINT [PK_UploadedDocuments]
    PRIMARY KEY CLUSTERED ([PatientDoc_Id] ASC);
GO

-- Creating primary key on [DrFirstXMLTrans_Id] in table 'DrFirstXMLTrans'
ALTER TABLE [dbo].[DrFirstXMLTrans]
ADD CONSTRAINT [PK_DrFirstXMLTrans]
    PRIMARY KEY CLUSTERED ([DrFirstXMLTrans_Id] ASC);
GO

-- Creating primary key on [DrFirstOrder_Id] in table 'DrFirstOrderXMLTrans'
ALTER TABLE [dbo].[DrFirstOrderXMLTrans]
ADD CONSTRAINT [PK_DrFirstOrderXMLTrans]
    PRIMARY KEY CLUSTERED ([DrFirstOrder_Id] ASC);
GO

-- Creating primary key on [Allergy_Id] in table 'AllergyInfoMasters'
ALTER TABLE [dbo].[AllergyInfoMasters]
ADD CONSTRAINT [PK_AllergyInfoMasters]
    PRIMARY KEY CLUSTERED ([Allergy_Id] ASC);
GO

-- Creating primary key on [NurseCommentType_Id] in table 'NurseCommentTypes'
ALTER TABLE [dbo].[NurseCommentTypes]
ADD CONSTRAINT [PK_NurseCommentTypes]
    PRIMARY KEY CLUSTERED ([NurseCommentType_Id] ASC);
GO

-- Creating primary key on [Activity_Id] in table 'ActivityMasters'
ALTER TABLE [dbo].[ActivityMasters]
ADD CONSTRAINT [PK_ActivityMasters]
    PRIMARY KEY CLUSTERED ([Activity_Id] ASC);
GO

-- Creating primary key on [UserActivity_Id] in table 'UserActivityDetails'
ALTER TABLE [dbo].[UserActivityDetails]
ADD CONSTRAINT [PK_UserActivityDetails]
    PRIMARY KEY CLUSTERED ([UserActivity_Id] ASC);
GO

-- Creating primary key on [Session_Id] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [PK_UserSessions]
    PRIMARY KEY CLUSTERED ([Session_Id] ASC);
GO

-- Creating primary key on [TransID] in table 'UserOTPs'
ALTER TABLE [dbo].[UserOTPs]
ADD CONSTRAINT [PK_UserOTPs]
    PRIMARY KEY CLUSTERED ([TransID] ASC);
GO

-- Creating primary key on [FileInfoError_Id] in table 'FileInfoErrors'
ALTER TABLE [dbo].[FileInfoErrors]
ADD CONSTRAINT [PK_FileInfoErrors]
    PRIMARY KEY CLUSTERED ([FileInfoError_Id] ASC);
GO

-- Creating primary key on [Refill_Id] in table 'ApprovalRefills'
ALTER TABLE [dbo].[ApprovalRefills]
ADD CONSTRAINT [PK_ApprovalRefills]
    PRIMARY KEY CLUSTERED ([Refill_Id] ASC);
GO

-- Creating primary key on [FavouriteData_Id] in table 'OrderFavouriteDatas'
ALTER TABLE [dbo].[OrderFavouriteDatas]
ADD CONSTRAINT [PK_OrderFavouriteDatas]
    PRIMARY KEY CLUSTERED ([FavouriteData_Id] ASC);
GO

-- Creating primary key on [Comments_Id] in table 'NurseComments'
ALTER TABLE [dbo].[NurseComments]
ADD CONSTRAINT [PK_NurseComments]
    PRIMARY KEY CLUSTERED ([Comments_Id] ASC);
GO

-- Creating primary key on [PatientType_Id] in table 'PatientTypes'
ALTER TABLE [dbo].[PatientTypes]
ADD CONSTRAINT [PK_PatientTypes]
    PRIMARY KEY CLUSTERED ([PatientType_Id] ASC);
GO

-- Creating primary key on [ColourType_Id] in table 'ColourTypes'
ALTER TABLE [dbo].[ColourTypes]
ADD CONSTRAINT [PK_ColourTypes]
    PRIMARY KEY CLUSTERED ([ColourType_Id] ASC);
GO

-- Creating primary key on [Fingersdesc_Id] in table 'Fingersdescs'
ALTER TABLE [dbo].[Fingersdescs]
ADD CONSTRAINT [PK_Fingersdescs]
    PRIMARY KEY CLUSTERED ([Fingersdesc_Id] ASC);
GO

-- Creating primary key on [PBarcode_Id] in table 'BarcodeDetails'
ALTER TABLE [dbo].[BarcodeDetails]
ADD CONSTRAINT [PK_BarcodeDetails]
    PRIMARY KEY CLUSTERED ([PBarcode_Id] ASC);
GO

-- Creating primary key on [MailFavourite_Id] in table 'MailFavourites'
ALTER TABLE [dbo].[MailFavourites]
ADD CONSTRAINT [PK_MailFavourites]
    PRIMARY KEY CLUSTERED ([MailFavourite_Id] ASC);
GO

-- Creating primary key on [MailRead_Id] in table 'MailReads'
ALTER TABLE [dbo].[MailReads]
ADD CONSTRAINT [PK_MailReads]
    PRIMARY KEY CLUSTERED ([MailRead_Id] ASC);
GO

-- Creating primary key on [Alert_Id] in table 'AlertStatus'
ALTER TABLE [dbo].[AlertStatus]
ADD CONSTRAINT [PK_AlertStatus]
    PRIMARY KEY CLUSTERED ([Alert_Id] ASC);
GO

-- Creating primary key on [AlertText_Id] in table 'AlertTexts'
ALTER TABLE [dbo].[AlertTexts]
ADD CONSTRAINT [PK_AlertTexts]
    PRIMARY KEY CLUSTERED ([AlertText_Id] ASC);
GO

-- Creating primary key on [Type_Id] in table 'AlertTypes'
ALTER TABLE [dbo].[AlertTypes]
ADD CONSTRAINT [PK_AlertTypes]
    PRIMARY KEY CLUSTERED ([Type_Id] ASC);
GO

-- Creating primary key on [Route_Id] in table 'Routes'
ALTER TABLE [dbo].[Routes]
ADD CONSTRAINT [PK_Routes]
    PRIMARY KEY CLUSTERED ([Route_Id] ASC);
GO

-- Creating primary key on [Ekit_Id] in table 'Ekits'
ALTER TABLE [dbo].[Ekits]
ADD CONSTRAINT [PK_Ekits]
    PRIMARY KEY CLUSTERED ([Ekit_Id] ASC);
GO

-- Creating primary key on [Stock_Id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [PK_Stocks]
    PRIMARY KEY CLUSTERED ([Stock_Id] ASC);
GO

-- Creating primary key on [Drug_Id] in table 'Drugs'
ALTER TABLE [dbo].[Drugs]
ADD CONSTRAINT [PK_Drugs]
    PRIMARY KEY CLUSTERED ([Drug_Id] ASC);
GO

-- Creating primary key on [EkitAdminister_Id] in table 'EkitAdministers'
ALTER TABLE [dbo].[EkitAdministers]
ADD CONSTRAINT [PK_EkitAdministers]
    PRIMARY KEY CLUSTERED ([EkitAdminister_Id] ASC);
GO

-- Creating primary key on [Activeday_Id] in table 'DrugActivedays'
ALTER TABLE [dbo].[DrugActivedays]
ADD CONSTRAINT [PK_DrugActivedays]
    PRIMARY KEY CLUSTERED ([Activeday_Id] ASC);
GO

-- Creating primary key on [DrFirstFile_Id] in table 'DrFirstFileDatas'
ALTER TABLE [dbo].[DrFirstFileDatas]
ADD CONSTRAINT [PK_DrFirstFileDatas]
    PRIMARY KEY CLUSTERED ([DrFirstFile_Id] ASC);
GO

-- Creating primary key on [OrderStock_Id] in table 'OrderStocks'
ALTER TABLE [dbo].[OrderStocks]
ADD CONSTRAINT [PK_OrderStocks]
    PRIMARY KEY CLUSTERED ([OrderStock_Id] ASC);
GO

-- Creating primary key on [NurseShifts_Id] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [PK_NurseShifts]
    PRIMARY KEY CLUSTERED ([NurseShifts_Id] ASC);
GO

-- Creating primary key on [CompanyConfig_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [PK_CompanyConfigs]
    PRIMARY KEY CLUSTERED ([CompanyConfig_Id] ASC);
GO

-- Creating primary key on [CompanyHlCategory_Id] in table 'CompanyHlCategories'
ALTER TABLE [dbo].[CompanyHlCategories]
ADD CONSTRAINT [PK_CompanyHlCategories]
    PRIMARY KEY CLUSTERED ([CompanyHlCategory_Id] ASC);
GO

-- Creating primary key on [CompanyHLEvent_Id] in table 'CompanyHLEvents'
ALTER TABLE [dbo].[CompanyHLEvents]
ADD CONSTRAINT [PK_CompanyHLEvents]
    PRIMARY KEY CLUSTERED ([CompanyHLEvent_Id] ASC);
GO

-- Creating primary key on [HLDirectionalWay_Id] in table 'HLDirectionalWays'
ALTER TABLE [dbo].[HLDirectionalWays]
ADD CONSTRAINT [PK_HLDirectionalWays]
    PRIMARY KEY CLUSTERED ([HLDirectionalWay_Id] ASC);
GO

-- Creating primary key on [StockReport_Id] in table 'StockReports'
ALTER TABLE [dbo].[StockReports]
ADD CONSTRAINT [PK_StockReports]
    PRIMARY KEY CLUSTERED ([StockReport_Id] ASC);
GO

-- Creating primary key on [Onleave_Id] in table 'OnLeaves'
ALTER TABLE [dbo].[OnLeaves]
ADD CONSTRAINT [PK_OnLeaves]
    PRIMARY KEY CLUSTERED ([Onleave_Id] ASC);
GO

-- Creating primary key on [NursingFreq_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [PK_NursingFrequencyConfigs]
    PRIMARY KEY CLUSTERED ([NursingFreq_Id] ASC);
GO

-- Creating primary key on [Role_Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Role_Id] ASC);
GO

-- Creating primary key on [DefaultScreen_Id] in table 'DefaultScreens'
ALTER TABLE [dbo].[DefaultScreens]
ADD CONSTRAINT [PK_DefaultScreens]
    PRIMARY KEY CLUSTERED ([DefaultScreen_Id] ASC);
GO

-- Creating primary key on [Company_Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Company_Id] ASC);
GO

-- Creating primary key on [Facility_Id] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [PK_Facilities]
    PRIMARY KEY CLUSTERED ([Facility_Id] ASC);
GO

-- Creating primary key on [Physician_Id] in table 'PhysicianDetails'
ALTER TABLE [dbo].[PhysicianDetails]
ADD CONSTRAINT [PK_PhysicianDetails]
    PRIMARY KEY CLUSTERED ([Physician_Id] ASC);
GO

-- Creating primary key on [FCTime_Id] in table 'NursingFCTimes'
ALTER TABLE [dbo].[NursingFCTimes]
ADD CONSTRAINT [PK_NursingFCTimes]
    PRIMARY KEY CLUSTERED ([FCTime_Id] ASC);
GO

-- Creating primary key on [ApprovalPatient_Id] in table 'ApprovalDemographics'
ALTER TABLE [dbo].[ApprovalDemographics]
ADD CONSTRAINT [PK_ApprovalDemographics]
    PRIMARY KEY CLUSTERED ([ApprovalPatient_Id] ASC);
GO

-- Creating primary key on [FteConfig_Id] in table 'FTEConfigurations'
ALTER TABLE [dbo].[FTEConfigurations]
ADD CONSTRAINT [PK_FTEConfigurations]
    PRIMARY KEY CLUSTERED ([FteConfig_Id] ASC);
GO

-- Creating primary key on [RecFac_Id] in table 'RecentFacs'
ALTER TABLE [dbo].[RecentFacs]
ADD CONSTRAINT [PK_RecentFacs]
    PRIMARY KEY CLUSTERED ([RecFac_Id] ASC);
GO

-- Creating primary key on [OHLConfig_Id] in table 'HLSevenOutboundDisplayCompanyConfigs'
ALTER TABLE [dbo].[HLSevenOutboundDisplayCompanyConfigs]
ADD CONSTRAINT [PK_HLSevenOutboundDisplayCompanyConfigs]
    PRIMARY KEY CLUSTERED ([OHLConfig_Id] ASC);
GO

-- Creating primary key on [OSegDetail_Id] in table 'HLSevenOutboundDisplaySegmentDetails'
ALTER TABLE [dbo].[HLSevenOutboundDisplaySegmentDetails]
ADD CONSTRAINT [PK_HLSevenOutboundDisplaySegmentDetails]
    PRIMARY KEY CLUSTERED ([OSegDetail_Id] ASC);
GO

-- Creating primary key on [OSegment_Id] in table 'HLSevenOutboundDisplaySegments'
ALTER TABLE [dbo].[HLSevenOutboundDisplaySegments]
ADD CONSTRAINT [PK_HLSevenOutboundDisplaySegments]
    PRIMARY KEY CLUSTERED ([OSegment_Id] ASC);
GO

-- Creating primary key on [DrugOrderMaster_Id] in table 'DrugOrderMasters'
ALTER TABLE [dbo].[DrugOrderMasters]
ADD CONSTRAINT [PK_DrugOrderMasters]
    PRIMARY KEY CLUSTERED ([DrugOrderMaster_Id] ASC);
GO

-- Creating primary key on [DAdmin_Id] in table 'DrugAdministrationTimes'
ALTER TABLE [dbo].[DrugAdministrationTimes]
ADD CONSTRAINT [PK_DrugAdministrationTimes]
    PRIMARY KEY CLUSTERED ([DAdmin_Id] ASC);
GO

-- Creating primary key on [DATrans_Id] in table 'DocAdministerTrans'
ALTER TABLE [dbo].[DocAdministerTrans]
ADD CONSTRAINT [PK_DocAdministerTrans]
    PRIMARY KEY CLUSTERED ([DATrans_Id] ASC);
GO

-- Creating primary key on [PQuantity_Id], [OrderFavMaster_ID] in table 'OrderFavourites'
ALTER TABLE [dbo].[OrderFavourites]
ADD CONSTRAINT [PK_OrderFavourites]
    PRIMARY KEY CLUSTERED ([PQuantity_Id], [OrderFavMaster_ID] ASC);
GO

-- Creating primary key on [OrderHold_Id] in table 'OrderHolds'
ALTER TABLE [dbo].[OrderHolds]
ADD CONSTRAINT [PK_OrderHolds]
    PRIMARY KEY CLUSTERED ([OrderHold_Id] ASC);
GO

-- Creating primary key on [PDispense_Id] in table 'TreatmentDispenseInfoes'
ALTER TABLE [dbo].[TreatmentDispenseInfoes]
ADD CONSTRAINT [PK_TreatmentDispenseInfoes]
    PRIMARY KEY CLUSTERED ([PDispense_Id] ASC);
GO

-- Creating primary key on [PRoute_Id] in table 'TreatmentRouteInfoes'
ALTER TABLE [dbo].[TreatmentRouteInfoes]
ADD CONSTRAINT [PK_TreatmentRouteInfoes]
    PRIMARY KEY CLUSTERED ([PRoute_Id] ASC);
GO

-- Creating primary key on [OIAReason_Id] in table 'OrderInactiveReasons'
ALTER TABLE [dbo].[OrderInactiveReasons]
ADD CONSTRAINT [PK_OrderInactiveReasons]
    PRIMARY KEY CLUSTERED ([OIAReason_Id] ASC);
GO

-- Creating primary key on [NSHierarchy_Id] in table 'NurseStationHierarchies'
ALTER TABLE [dbo].[NurseStationHierarchies]
ADD CONSTRAINT [PK_NurseStationHierarchies]
    PRIMARY KEY CLUSTERED ([NSHierarchy_Id] ASC);
GO

-- Creating primary key on [Footer_Id] in table 'FooterLogoes'
ALTER TABLE [dbo].[FooterLogoes]
ADD CONSTRAINT [PK_FooterLogoes]
    PRIMARY KEY CLUSTERED ([Footer_Id] ASC);
GO

-- Creating primary key on [TransCS_Id] in table 'ControlSubstanceTrans'
ALTER TABLE [dbo].[ControlSubstanceTrans]
ADD CONSTRAINT [PK_ControlSubstanceTrans]
    PRIMARY KEY CLUSTERED ([TransCS_Id] ASC);
GO

-- Creating primary key on [TransOS_Id] in table 'OrderStockTrans'
ALTER TABLE [dbo].[OrderStockTrans]
ADD CONSTRAINT [PK_OrderStockTrans]
    PRIMARY KEY CLUSTERED ([TransOS_Id] ASC);
GO

-- Creating primary key on [CSReason_Id] in table 'ControlSubstanceReasons'
ALTER TABLE [dbo].[ControlSubstanceReasons]
ADD CONSTRAINT [PK_ControlSubstanceReasons]
    PRIMARY KEY CLUSTERED ([CSReason_Id] ASC);
GO

-- Creating primary key on [ProcessID] in table 'ProcessKeyMasters'
ALTER TABLE [dbo].[ProcessKeyMasters]
ADD CONSTRAINT [PK_ProcessKeyMasters]
    PRIMARY KEY CLUSTERED ([ProcessID] ASC);
GO

-- Creating primary key on [PQuantity_Id] in table 'QuantityDetails'
ALTER TABLE [dbo].[QuantityDetails]
ADD CONSTRAINT [PK_QuantityDetails]
    PRIMARY KEY CLUSTERED ([PQuantity_Id] ASC);
GO

-- Creating primary key on [CertifyTime_ID] in table 'CertifyDates'
ALTER TABLE [dbo].[CertifyDates]
ADD CONSTRAINT [PK_CertifyDates]
    PRIMARY KEY CLUSTERED ([CertifyTime_ID] ASC);
GO

-- Creating primary key on [Certify_ID] in table 'CertifyOrders'
ALTER TABLE [dbo].[CertifyOrders]
ADD CONSTRAINT [PK_CertifyOrders]
    PRIMARY KEY CLUSTERED ([Certify_ID] ASC);
GO

-- Creating primary key on [MailBox_Id] in table 'MailBoxes'
ALTER TABLE [dbo].[MailBoxes]
ADD CONSTRAINT [PK_MailBoxes]
    PRIMARY KEY CLUSTERED ([MailBox_Id] ASC);
GO

-- Creating primary key on [OFConfig_Id] in table 'OrderFavConfigs'
ALTER TABLE [dbo].[OrderFavConfigs]
ADD CONSTRAINT [PK_OrderFavConfigs]
    PRIMARY KEY CLUSTERED ([OFConfig_Id] ASC);
GO

-- Creating primary key on [SE_Id] in table 'tblSideEffects'
ALTER TABLE [dbo].[tblSideEffects]
ADD CONSTRAINT [PK_tblSideEffects]
    PRIMARY KEY CLUSTERED ([SE_Id] ASC);
GO

-- Creating primary key on [DDiscard_Id] in table 'tblDrugDiscardInfoes'
ALTER TABLE [dbo].[tblDrugDiscardInfoes]
ADD CONSTRAINT [PK_tblDrugDiscardInfoes]
    PRIMARY KEY CLUSTERED ([DDiscard_Id] ASC);
GO

-- Creating primary key on [ASites_Id] in table 'tblAdministeredSites'
ALTER TABLE [dbo].[tblAdministeredSites]
ADD CONSTRAINT [PK_tblAdministeredSites]
    PRIMARY KEY CLUSTERED ([ASites_Id] ASC);
GO

-- Creating primary key on [Rdc_Id] in table 'tblRDCMailConfigs'
ALTER TABLE [dbo].[tblRDCMailConfigs]
ADD CONSTRAINT [PK_tblRDCMailConfigs]
    PRIMARY KEY CLUSTERED ([Rdc_Id] ASC);
GO

-- Creating primary key on [RdcTime_Id] in table 'tblRdcMailTimes'
ALTER TABLE [dbo].[tblRdcMailTimes]
ADD CONSTRAINT [PK_tblRdcMailTimes]
    PRIMARY KEY CLUSTERED ([RdcTime_Id] ASC);
GO

-- Creating primary key on [Hoa_Id] in table 'OrderHOAInfoes'
ALTER TABLE [dbo].[OrderHOAInfoes]
ADD CONSTRAINT [PK_OrderHOAInfoes]
    PRIMARY KEY CLUSTERED ([Hoa_Id] ASC);
GO

-- Creating primary key on [TZ_Id] in table 'tblTimeZones'
ALTER TABLE [dbo].[tblTimeZones]
ADD CONSTRAINT [PK_tblTimeZones]
    PRIMARY KEY CLUSTERED ([TZ_Id] ASC);
GO

-- Creating primary key on [PCertifyTime_ID] in table 'PCertifyDates'
ALTER TABLE [dbo].[PCertifyDates]
ADD CONSTRAINT [PK_PCertifyDates]
    PRIMARY KEY CLUSTERED ([PCertifyTime_ID] ASC);
GO

-- Creating primary key on [PCertify_ID] in table 'PCertifyOrders'
ALTER TABLE [dbo].[PCertifyOrders]
ADD CONSTRAINT [PK_PCertifyOrders]
    PRIMARY KEY CLUSTERED ([PCertify_ID] ASC);
GO

-- Creating primary key on [EkCSReason_Id] in table 'EkitControlSubstanceReasons'
ALTER TABLE [dbo].[EkitControlSubstanceReasons]
ADD CONSTRAINT [PK_EkitControlSubstanceReasons]
    PRIMARY KEY CLUSTERED ([EkCSReason_Id] ASC);
GO

-- Creating primary key on [EkTransCS_Id] in table 'EKitControlSubstanceTrans'
ALTER TABLE [dbo].[EKitControlSubstanceTrans]
ADD CONSTRAINT [PK_EKitControlSubstanceTrans]
    PRIMARY KEY CLUSTERED ([EkTransCS_Id] ASC);
GO

-- Creating primary key on [Source_Id] in table 'CpoeSources'
ALTER TABLE [dbo].[CpoeSources]
ADD CONSTRAINT [PK_CpoeSources]
    PRIMARY KEY CLUSTERED ([Source_Id] ASC);
GO

-- Creating primary key on [DQty_Id] in table 'QuantityDoses'
ALTER TABLE [dbo].[QuantityDoses]
ADD CONSTRAINT [PK_QuantityDoses]
    PRIMARY KEY CLUSTERED ([DQty_Id] ASC);
GO

-- Creating primary key on [Uom_Id] in table 'UnitMeasurements'
ALTER TABLE [dbo].[UnitMeasurements]
ADD CONSTRAINT [PK_UnitMeasurements]
    PRIMARY KEY CLUSTERED ([Uom_Id] ASC);
GO

-- Creating primary key on [Dose_Id] in table 'DoseUoms'
ALTER TABLE [dbo].[DoseUoms]
ADD CONSTRAINT [PK_DoseUoms]
    PRIMARY KEY CLUSTERED ([Dose_Id] ASC);
GO

-- Creating primary key on [DrugAdminister_Id] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [PK_DrugAdministers]
    PRIMARY KEY CLUSTERED ([DrugAdminister_Id] ASC);
GO

-- Creating primary key on [POrder_Id] in table 'CommonOrderInfoes'
ALTER TABLE [dbo].[CommonOrderInfoes]
ADD CONSTRAINT [PK_CommonOrderInfoes]
    PRIMARY KEY CLUSTERED ([POrder_Id] ASC);
GO

-- Creating primary key on [WeightLog_ID] in table 'WeightLogs'
ALTER TABLE [dbo].[WeightLogs]
ADD CONSTRAINT [PK_WeightLogs]
    PRIMARY KEY CLUSTERED ([WeightLog_ID] ASC);
GO

-- Creating primary key on [PApprovalOrder_Id] in table 'ApprovalOrders'
ALTER TABLE [dbo].[ApprovalOrders]
ADD CONSTRAINT [PK_ApprovalOrders]
    PRIMARY KEY CLUSTERED ([PApprovalOrder_Id] ASC);
GO

-- Creating primary key on [Patient_Id] in table 'Demographics'
ALTER TABLE [dbo].[Demographics]
ADD CONSTRAINT [PK_Demographics]
    PRIMARY KEY CLUSTERED ([Patient_Id] ASC);
GO

-- Creating primary key on [PApprovalOrder_Id] in table 'ApprovalOrder1'
ALTER TABLE [dbo].[ApprovalOrder1]
ADD CONSTRAINT [PK_ApprovalOrder1]
    PRIMARY KEY CLUSTERED ([PApprovalOrder_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AllergyType_CreatedBy] in table 'AllergyTypeCodes'
ALTER TABLE [dbo].[AllergyTypeCodes]
ADD CONSTRAINT [FK_AllergyTypeCode_User]
    FOREIGN KEY ([AllergyType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AllergyTypeCode_User'
CREATE INDEX [IX_FK_AllergyTypeCode_User]
ON [dbo].[AllergyTypeCodes]
    ([AllergyType_CreatedBy]);
GO

-- Creating foreign key on [Integration_CreatedBy] in table 'ApiIntegrations'
ALTER TABLE [dbo].[ApiIntegrations]
ADD CONSTRAINT [FK_ApiIntegrations_User]
    FOREIGN KEY ([Integration_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApiIntegrations_User'
CREATE INDEX [IX_FK_ApiIntegrations_User]
ON [dbo].[ApiIntegrations]
    ([Integration_CreatedBy]);
GO

-- Creating foreign key on [Bed_CreatedBy] in table 'Beds'
ALTER TABLE [dbo].[Beds]
ADD CONSTRAINT [FK_Bed_User]
    FOREIGN KEY ([Bed_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Bed_User'
CREATE INDEX [IX_FK_Bed_User]
ON [dbo].[Beds]
    ([Bed_CreatedBy]);
GO

-- Creating foreign key on [Bed_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Bed]
    FOREIGN KEY ([Bed_Id])
    REFERENCES [dbo].[Beds]
        ([Bed_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Bed'
CREATE INDEX [IX_FK_CompanyBedConfig_Bed]
ON [dbo].[CompanyBedConfigs]
    ([Bed_Id]);
GO

-- Creating foreign key on [BehavioralSym_CreatedBy] in table 'BehavioralSymptomsMasters'
ALTER TABLE [dbo].[BehavioralSymptomsMasters]
ADD CONSTRAINT [FK_BehavioralSymptomsMaster_User]
    FOREIGN KEY ([BehavioralSym_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BehavioralSymptomsMaster_User'
CREATE INDEX [IX_FK_BehavioralSymptomsMaster_User]
ON [dbo].[BehavioralSymptomsMasters]
    ([BehavioralSym_CreatedBy]);
GO

-- Creating foreign key on [CDCodingType_CreatedBy] in table 'ClassDrugCodingTypes'
ALTER TABLE [dbo].[ClassDrugCodingTypes]
ADD CONSTRAINT [FK_ClassDrugCodingType_User]
    FOREIGN KEY ([CDCodingType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassDrugCodingType_User'
CREATE INDEX [IX_FK_ClassDrugCodingType_User]
ON [dbo].[ClassDrugCodingTypes]
    ([CDCodingType_CreatedBy]);
GO

-- Creating foreign key on [Floor_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Floor]
    FOREIGN KEY ([Floor_Id])
    REFERENCES [dbo].[Floors]
        ([Floor_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Floor'
CREATE INDEX [IX_FK_CompanyBedConfig_Floor]
ON [dbo].[CompanyBedConfigs]
    ([Floor_Id]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_NursingStation'
CREATE INDEX [IX_FK_CompanyBedConfig_NursingStation]
ON [dbo].[CompanyBedConfigs]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Room_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Room]
    FOREIGN KEY ([Room_Id])
    REFERENCES [dbo].[Rooms]
        ([Room_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Room'
CREATE INDEX [IX_FK_CompanyBedConfig_Room]
ON [dbo].[CompanyBedConfigs]
    ([Room_Id]);
GO

-- Creating foreign key on [BedConfig_CreatedBy] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_User]
    FOREIGN KEY ([BedConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_User'
CREATE INDEX [IX_FK_CompanyBedConfig_User]
ON [dbo].[CompanyBedConfigs]
    ([BedConfig_CreatedBy]);
GO

-- Creating foreign key on [Wing_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Wing]
    FOREIGN KEY ([Wing_Id])
    REFERENCES [dbo].[Wings]
        ([Wing_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Wing'
CREATE INDEX [IX_FK_CompanyBedConfig_Wing]
ON [dbo].[CompanyBedConfigs]
    ([Wing_Id]);
GO

-- Creating foreign key on [Country_CreatedBy] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [FK_Country_User]
    FOREIGN KEY ([Country_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Country_User'
CREATE INDEX [IX_FK_Country_User]
ON [dbo].[Countries]
    ([Country_CreatedBy]);
GO

-- Creating foreign key on [DCodingType_CreatedBy] in table 'DiagnosisCodingTypes'
ALTER TABLE [dbo].[DiagnosisCodingTypes]
ADD CONSTRAINT [FK_DiagnosisCodingType_User]
    FOREIGN KEY ([DCodingType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiagnosisCodingType_User'
CREATE INDEX [IX_FK_DiagnosisCodingType_User]
ON [dbo].[DiagnosisCodingTypes]
    ([DCodingType_CreatedBy]);
GO

-- Creating foreign key on [Folder_CreatedBy] in table 'DocFolders'
ALTER TABLE [dbo].[DocFolders]
ADD CONSTRAINT [FK_DocFolder_User]
    FOREIGN KEY ([Folder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocFolder_User'
CREATE INDEX [IX_FK_DocFolder_User]
ON [dbo].[DocFolders]
    ([Folder_CreatedBy]);
GO

-- Creating foreign key on [EventCat_CreatedBy] in table 'EventCategories'
ALTER TABLE [dbo].[EventCategories]
ADD CONSTRAINT [FK_EventCategory_User]
    FOREIGN KEY ([EventCat_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventCategory_User'
CREATE INDEX [IX_FK_EventCategory_User]
ON [dbo].[EventCategories]
    ([EventCat_CreatedBy]);
GO

-- Creating foreign key on [FileAck_CreatedBy] in table 'FileAckInformations'
ALTER TABLE [dbo].[FileAckInformations]
ADD CONSTRAINT [FK_FileAckInformation_User]
    FOREIGN KEY ([FileAck_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileAckInformation_User'
CREATE INDEX [IX_FK_FileAckInformation_User]
ON [dbo].[FileAckInformations]
    ([FileAck_CreatedBy]);
GO

-- Creating foreign key on [File_CreatedBy] in table 'FileInformations'
ALTER TABLE [dbo].[FileInformations]
ADD CONSTRAINT [FK_FileInformation_User]
    FOREIGN KEY ([File_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileInformation_User'
CREATE INDEX [IX_FK_FileInformation_User]
ON [dbo].[FileInformations]
    ([File_CreatedBy]);
GO

-- Creating foreign key on [Floor_CreatedBy] in table 'Floors'
ALTER TABLE [dbo].[Floors]
ADD CONSTRAINT [FK_Floor_User]
    FOREIGN KEY ([Floor_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Floor_User'
CREATE INDEX [IX_FK_Floor_User]
ON [dbo].[Floors]
    ([Floor_CreatedBy]);
GO

-- Creating foreign key on [Frequency_CreatedBy] in table 'FrequencyMasters'
ALTER TABLE [dbo].[FrequencyMasters]
ADD CONSTRAINT [FK_FrequencyMaster_User]
    FOREIGN KEY ([Frequency_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrequencyMaster_User'
CREATE INDEX [IX_FK_FrequencyMaster_User]
ON [dbo].[FrequencyMasters]
    ([Frequency_CreatedBy]);
GO

-- Creating foreign key on [FteCategory_CreatedBy] in table 'FTECategories'
ALTER TABLE [dbo].[FTECategories]
ADD CONSTRAINT [FK_FTECategory_User]
    FOREIGN KEY ([FteCategory_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FTECategory_User'
CREATE INDEX [IX_FK_FTECategory_User]
ON [dbo].[FTECategories]
    ([FteCategory_CreatedBy]);
GO

-- Creating foreign key on [FteConn_CreatedBy] in table 'FteConnections'
ALTER TABLE [dbo].[FteConnections]
ADD CONSTRAINT [FK_FteConnection_User]
    FOREIGN KEY ([FteConn_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FteConnection_User'
CREATE INDEX [IX_FK_FteConnection_User]
ON [dbo].[FteConnections]
    ([FteConn_CreatedBy]);
GO

-- Creating foreign key on [Gender_CreatedBy] in table 'Genders'
ALTER TABLE [dbo].[Genders]
ADD CONSTRAINT [FK_Gender_User]
    FOREIGN KEY ([Gender_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Gender_User'
CREATE INDEX [IX_FK_Gender_User]
ON [dbo].[Genders]
    ([Gender_CreatedBy]);
GO

-- Creating foreign key on [User_Gender] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_Gender]
    FOREIGN KEY ([User_Gender])
    REFERENCES [dbo].[Genders]
        ([Gender_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Gender'
CREATE INDEX [IX_FK_User_Gender]
ON [dbo].[Users]
    ([User_Gender]);
GO

-- Creating foreign key on [SegDetail_Id] in table 'HLSevenCompanyConfigs'
ALTER TABLE [dbo].[HLSevenCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenCompanyConfig_HLSevenSegmentDetails]
    FOREIGN KEY ([SegDetail_Id])
    REFERENCES [dbo].[HLSevenSegmentDetails]
        ([SegDetail_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenCompanyConfig_HLSevenSegmentDetails'
CREATE INDEX [IX_FK_HLSevenCompanyConfig_HLSevenSegmentDetails]
ON [dbo].[HLSevenCompanyConfigs]
    ([SegDetail_Id]);
GO

-- Creating foreign key on [HLConfig_CreatedBy] in table 'HLSevenCompanyConfigs'
ALTER TABLE [dbo].[HLSevenCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenCompanyConfig_User]
    FOREIGN KEY ([HLConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenCompanyConfig_User'
CREATE INDEX [IX_FK_HLSevenCompanyConfig_User]
ON [dbo].[HLSevenCompanyConfigs]
    ([HLConfig_CreatedBy]);
GO

-- Creating foreign key on [Segment_Id] in table 'HLSevenSegmentDetails'
ALTER TABLE [dbo].[HLSevenSegmentDetails]
ADD CONSTRAINT [FK_HLSevenSegmentDetails_HLSevenSegments]
    FOREIGN KEY ([Segment_Id])
    REFERENCES [dbo].[HLSevenSegments]
        ([Segment_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenSegmentDetails_HLSevenSegments'
CREATE INDEX [IX_FK_HLSevenSegmentDetails_HLSevenSegments]
ON [dbo].[HLSevenSegmentDetails]
    ([Segment_Id]);
GO

-- Creating foreign key on [SegDetail_CreatedBy] in table 'HLSevenSegmentDetails'
ALTER TABLE [dbo].[HLSevenSegmentDetails]
ADD CONSTRAINT [FK_HLSevenSegmentDetails_User]
    FOREIGN KEY ([SegDetail_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenSegmentDetails_User'
CREATE INDEX [IX_FK_HLSevenSegmentDetails_User]
ON [dbo].[HLSevenSegmentDetails]
    ([SegDetail_CreatedBy]);
GO

-- Creating foreign key on [Segment_CreatedBy] in table 'HLSevenSegments'
ALTER TABLE [dbo].[HLSevenSegments]
ADD CONSTRAINT [FK_HLSevenSegments_User]
    FOREIGN KEY ([Segment_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenSegments_User'
CREATE INDEX [IX_FK_HLSevenSegments_User]
ON [dbo].[HLSevenSegments]
    ([Segment_CreatedBy]);
GO

-- Creating foreign key on [Hour_CreatedBy] in table 'Hours'
ALTER TABLE [dbo].[Hours]
ADD CONSTRAINT [FK_Hours_User]
    FOREIGN KEY ([Hour_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Hours_User'
CREATE INDEX [IX_FK_Hours_User]
ON [dbo].[Hours]
    ([Hour_CreatedBy]);
GO

-- Creating foreign key on [ICD10_CreatedBy] in table 'ICD10'
ALTER TABLE [dbo].[ICD10]
ADD CONSTRAINT [FK_ICD10_User]
    FOREIGN KEY ([ICD10_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ICD10_User'
CREATE INDEX [IX_FK_ICD10_User]
ON [dbo].[ICD10]
    ([ICD10_CreatedBy]);
GO

-- Creating foreign key on [ImportFile_CreatedBy] in table 'ImportFiles'
ALTER TABLE [dbo].[ImportFiles]
ADD CONSTRAINT [FK_ImportFile_User]
    FOREIGN KEY ([ImportFile_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ImportFile_User'
CREATE INDEX [IX_FK_ImportFile_User]
ON [dbo].[ImportFiles]
    ([ImportFile_CreatedBy]);
GO

-- Creating foreign key on [IntegrationType_CreatedBy] in table 'IntegrationTypes'
ALTER TABLE [dbo].[IntegrationTypes]
ADD CONSTRAINT [FK_IntegrationType_User]
    FOREIGN KEY ([IntegrationType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IntegrationType_User'
CREATE INDEX [IX_FK_IntegrationType_User]
ON [dbo].[IntegrationTypes]
    ([IntegrationType_CreatedBy]);
GO

-- Creating foreign key on [Marital_CreatedBy] in table 'MaritalStatus'
ALTER TABLE [dbo].[MaritalStatus]
ADD CONSTRAINT [FK_MaritalStatus_User]
    FOREIGN KEY ([Marital_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaritalStatus_User'
CREATE INDEX [IX_FK_MaritalStatus_User]
ON [dbo].[MaritalStatus]
    ([Marital_CreatedBy]);
GO

-- Creating foreign key on [User_MaritalStatus] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_MaritalStatus]
    FOREIGN KEY ([User_MaritalStatus])
    REFERENCES [dbo].[MaritalStatus]
        ([Marital_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_MaritalStatus'
CREATE INDEX [IX_FK_User_MaritalStatus]
ON [dbo].[Users]
    ([User_MaritalStatus]);
GO

-- Creating foreign key on [MedicationReason_CreatedBy] in table 'MedicationReasons'
ALTER TABLE [dbo].[MedicationReasons]
ADD CONSTRAINT [FK_MedicationReason_User]
    FOREIGN KEY ([MedicationReason_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicationReason_User'
CREATE INDEX [IX_FK_MedicationReason_User]
ON [dbo].[MedicationReasons]
    ([MedicationReason_CreatedBy]);
GO

-- Creating foreign key on [Month_CreatedBy] in table 'Months'
ALTER TABLE [dbo].[Months]
ADD CONSTRAINT [FK_Months_User]
    FOREIGN KEY ([Month_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Months_User'
CREATE INDEX [IX_FK_Months_User]
ON [dbo].[Months]
    ([Month_CreatedBy]);
GO

-- Creating foreign key on [NursingStation_Id] in table 'NursingSchedules'
ALTER TABLE [dbo].[NursingSchedules]
ADD CONSTRAINT [FK_NursingSchedule_NursingStation]
    FOREIGN KEY ([NursingStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingSchedule_NursingStation'
CREATE INDEX [IX_FK_NursingSchedule_NursingStation]
ON [dbo].[NursingSchedules]
    ([NursingStation_Id]);
GO

-- Creating foreign key on [NursingSchedule_CreatedBy] in table 'NursingSchedules'
ALTER TABLE [dbo].[NursingSchedules]
ADD CONSTRAINT [FK_NursingSchedule_User]
    FOREIGN KEY ([NursingSchedule_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingSchedule_User'
CREATE INDEX [IX_FK_NursingSchedule_User]
ON [dbo].[NursingSchedules]
    ([NursingSchedule_CreatedBy]);
GO

-- Creating foreign key on [NurseStation_CreatedBy] in table 'NursingStations'
ALTER TABLE [dbo].[NursingStations]
ADD CONSTRAINT [FK_NursingStation_User]
    FOREIGN KEY ([NurseStation_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingStation_User'
CREATE INDEX [IX_FK_NursingStation_User]
ON [dbo].[NursingStations]
    ([NurseStation_CreatedBy]);
GO

-- Creating foreign key on [OrderControlCretedBy] in table 'OrderControlMasters'
ALTER TABLE [dbo].[OrderControlMasters]
ADD CONSTRAINT [FK_OrderControlMaster_User]
    FOREIGN KEY ([OrderControlCretedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderControlMaster_User'
CREATE INDEX [IX_FK_OrderControlMaster_User]
ON [dbo].[OrderControlMasters]
    ([OrderControlCretedBy]);
GO

-- Creating foreign key on [OrderFavMaster_CreatedBy] in table 'OrderFavouriteMasters'
ALTER TABLE [dbo].[OrderFavouriteMasters]
ADD CONSTRAINT [FK_OrderFavouriteMaster_User]
    FOREIGN KEY ([OrderFavMaster_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderFavouriteMaster_User'
CREATE INDEX [IX_FK_OrderFavouriteMaster_User]
ON [dbo].[OrderFavouriteMasters]
    ([OrderFavMaster_CreatedBy]);
GO

-- Creating foreign key on [OrderType_CreatedBy] in table 'OrderTypes'
ALTER TABLE [dbo].[OrderTypes]
ADD CONSTRAINT [FK_OrderType_User]
    FOREIGN KEY ([OrderType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderType_User'
CREATE INDEX [IX_FK_OrderType_User]
ON [dbo].[OrderTypes]
    ([OrderType_CreatedBy]);
GO

-- Creating foreign key on [Screen_Id] in table 'RoleConfigs'
ALTER TABLE [dbo].[RoleConfigs]
ADD CONSTRAINT [FK_RoleConfig_Screens]
    FOREIGN KEY ([Screen_Id])
    REFERENCES [dbo].[Screens]
        ([Screen_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleConfig_Screens'
CREATE INDEX [IX_FK_RoleConfig_Screens]
ON [dbo].[RoleConfigs]
    ([Screen_Id]);
GO

-- Creating foreign key on [RoleConfig_CreatedBy] in table 'RoleConfigs'
ALTER TABLE [dbo].[RoleConfigs]
ADD CONSTRAINT [FK_RoleConfig_User]
    FOREIGN KEY ([RoleConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleConfig_User'
CREATE INDEX [IX_FK_RoleConfig_User]
ON [dbo].[RoleConfigs]
    ([RoleConfig_CreatedBy]);
GO

-- Creating foreign key on [Room_CreatedBy] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [FK_Room_User]
    FOREIGN KEY ([Room_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Room_User'
CREATE INDEX [IX_FK_Room_User]
ON [dbo].[Rooms]
    ([Room_CreatedBy]);
GO

-- Creating foreign key on [Screen_CreatedBy] in table 'Screens'
ALTER TABLE [dbo].[Screens]
ADD CONSTRAINT [FK_Screens_User]
    FOREIGN KEY ([Screen_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Screens_User'
CREATE INDEX [IX_FK_Screens_User]
ON [dbo].[Screens]
    ([Screen_CreatedBy]);
GO

-- Creating foreign key on [Suffix_CreatedBy] in table 'Suffixes'
ALTER TABLE [dbo].[Suffixes]
ADD CONSTRAINT [FK_Suffix_User]
    FOREIGN KEY ([Suffix_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Suffix_User'
CREATE INDEX [IX_FK_Suffix_User]
ON [dbo].[Suffixes]
    ([Suffix_CreatedBy]);
GO

-- Creating foreign key on [TimeFormat_CreatedBy] in table 'TimeFormats'
ALTER TABLE [dbo].[TimeFormats]
ADD CONSTRAINT [FK_TimeFormat_User]
    FOREIGN KEY ([TimeFormat_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TimeFormat_User'
CREATE INDEX [IX_FK_TimeFormat_User]
ON [dbo].[TimeFormats]
    ([TimeFormat_CreatedBy]);
GO

-- Creating foreign key on [PAddl_CreatedBy] in table 'AddlInstructionDetails'
ALTER TABLE [dbo].[AddlInstructionDetails]
ADD CONSTRAINT [FK_AddlInstructionDetails_User]
    FOREIGN KEY ([PAddl_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddlInstructionDetails_User'
CREATE INDEX [IX_FK_AddlInstructionDetails_User]
ON [dbo].[AddlInstructionDetails]
    ([PAddl_CreatedBy]);
GO

-- Creating foreign key on [PAnc_CreatedBy] in table 'AncillaryDetails'
ALTER TABLE [dbo].[AncillaryDetails]
ADD CONSTRAINT [FK_AncillaryDetails_User]
    FOREIGN KEY ([PAnc_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AncillaryDetails_User'
CREATE INDEX [IX_FK_AncillaryDetails_User]
ON [dbo].[AncillaryDetails]
    ([PAnc_CreatedBy]);
GO

-- Creating foreign key on [PAncOutBoundApprovalBy] in table 'AncillaryDetails'
ALTER TABLE [dbo].[AncillaryDetails]
ADD CONSTRAINT [FK_AncillaryDetails_User1]
    FOREIGN KEY ([PAncOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AncillaryDetails_User1'
CREATE INDEX [IX_FK_AncillaryDetails_User1]
ON [dbo].[AncillaryDetails]
    ([PAncOutBoundApprovalBy]);
GO

-- Creating foreign key on [PComp_CreatedBy] in table 'CompoundOrders'
ALTER TABLE [dbo].[CompoundOrders]
ADD CONSTRAINT [FK_CompoundOrder_User]
    FOREIGN KEY ([PComp_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompoundOrder_User'
CREATE INDEX [IX_FK_CompoundOrder_User]
ON [dbo].[CompoundOrders]
    ([PComp_CreatedBy]);
GO

-- Creating foreign key on [PCOutBoundApprovalBy] in table 'CompoundOrders'
ALTER TABLE [dbo].[CompoundOrders]
ADD CONSTRAINT [FK_CompoundOrder_User1]
    FOREIGN KEY ([PCOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompoundOrder_User1'
CREATE INDEX [IX_FK_CompoundOrder_User1]
ON [dbo].[CompoundOrders]
    ([PCOutBoundApprovalBy]);
GO

-- Creating foreign key on [PEncOrder_CreatedBy] in table 'EncodedOrderDetails'
ALTER TABLE [dbo].[EncodedOrderDetails]
ADD CONSTRAINT [FK_EncodedOrderDetails_User]
    FOREIGN KEY ([PEncOrder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EncodedOrderDetails_User'
CREATE INDEX [IX_FK_EncodedOrderDetails_User]
ON [dbo].[EncodedOrderDetails]
    ([PEncOrder_CreatedBy]);
GO

-- Creating foreign key on [PEOutBoundApprovalBy] in table 'EncodedOrderDetails'
ALTER TABLE [dbo].[EncodedOrderDetails]
ADD CONSTRAINT [FK_EncodedOrderDetails_User1]
    FOREIGN KEY ([PEOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EncodedOrderDetails_User1'
CREATE INDEX [IX_FK_EncodedOrderDetails_User1]
ON [dbo].[EncodedOrderDetails]
    ([PEOutBoundApprovalBy]);
GO

-- Creating foreign key on [PatIns_CreatedBy] in table 'InsuranceInfoes'
ALTER TABLE [dbo].[InsuranceInfoes]
ADD CONSTRAINT [FK_InsuranceInfo_User]
    FOREIGN KEY ([PatIns_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InsuranceInfo_User'
CREATE INDEX [IX_FK_InsuranceInfo_User]
ON [dbo].[InsuranceInfoes]
    ([PatIns_CreatedBy]);
GO

-- Creating foreign key on [PNote_CreatedBy] in table 'NotesInfoes'
ALTER TABLE [dbo].[NotesInfoes]
ADD CONSTRAINT [FK_NotesInfo_User]
    FOREIGN KEY ([PNote_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotesInfo_User'
CREATE INDEX [IX_FK_NotesInfo_User]
ON [dbo].[NotesInfoes]
    ([PNote_CreatedBy]);
GO

-- Creating foreign key on [PObs_CreatedBy] in table 'Observations'
ALTER TABLE [dbo].[Observations]
ADD CONSTRAINT [FK_Observation_User]
    FOREIGN KEY ([PObs_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Observation_User'
CREATE INDEX [IX_FK_Observation_User]
ON [dbo].[Observations]
    ([PObs_CreatedBy]);
GO

-- Creating foreign key on [File_CreatedBy] in table 'OutBoundFileInformations'
ALTER TABLE [dbo].[OutBoundFileInformations]
ADD CONSTRAINT [FK_OutBoundFileInformation_User]
    FOREIGN KEY ([File_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutBoundFileInformation_User'
CREATE INDEX [IX_FK_OutBoundFileInformation_User]
ON [dbo].[OutBoundFileInformations]
    ([File_CreatedBy]);
GO

-- Creating foreign key on [ResOrder_CreatedBy] in table 'ResidentOrders'
ALTER TABLE [dbo].[ResidentOrders]
ADD CONSTRAINT [FK_ResidentOrder_User]
    FOREIGN KEY ([ResOrder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResidentOrder_User'
CREATE INDEX [IX_FK_ResidentOrder_User]
ON [dbo].[ResidentOrders]
    ([ResOrder_CreatedBy]);
GO

-- Creating foreign key on [PTreatment_CreatedBy] in table 'TreatmentInfoes'
ALTER TABLE [dbo].[TreatmentInfoes]
ADD CONSTRAINT [FK_TreatmentInfo_User]
    FOREIGN KEY ([PTreatment_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentInfo_User'
CREATE INDEX [IX_FK_TreatmentInfo_User]
ON [dbo].[TreatmentInfoes]
    ([PTreatment_CreatedBy]);
GO

-- Creating foreign key on [PTIOutBoundApprovalBy] in table 'TreatmentInfoes'
ALTER TABLE [dbo].[TreatmentInfoes]
ADD CONSTRAINT [FK_TreatmentInfo_User1]
    FOREIGN KEY ([PTIOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentInfo_User1'
CREATE INDEX [IX_FK_TreatmentInfo_User1]
ON [dbo].[TreatmentInfoes]
    ([PTIOutBoundApprovalBy]);
GO

-- Creating foreign key on [User_CreatedBy] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_User]
    FOREIGN KEY ([User_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_User'
CREATE INDEX [IX_FK_User_User]
ON [dbo].[Users]
    ([User_CreatedBy]);
GO

-- Creating foreign key on [UserRole_CreatedBy] in table 'UserRoleFacilityConfigs'
ALTER TABLE [dbo].[UserRoleFacilityConfigs]
ADD CONSTRAINT [FK_UserRoleFacilityConfig_User]
    FOREIGN KEY ([UserRole_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleFacilityConfig_User'
CREATE INDEX [IX_FK_UserRoleFacilityConfig_User]
ON [dbo].[UserRoleFacilityConfigs]
    ([UserRole_CreatedBy]);
GO

-- Creating foreign key on [User_Id] in table 'UserRoleFacilityConfigs'
ALTER TABLE [dbo].[UserRoleFacilityConfigs]
ADD CONSTRAINT [FK_UserRoleFacilityConfig_User1]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleFacilityConfig_User1'
CREATE INDEX [IX_FK_UserRoleFacilityConfig_User1]
ON [dbo].[UserRoleFacilityConfigs]
    ([User_Id]);
GO

-- Creating foreign key on [VisitBehaviour_CreatedBy] in table 'VisitBehaviours'
ALTER TABLE [dbo].[VisitBehaviours]
ADD CONSTRAINT [FK_VisitBehaviour_User]
    FOREIGN KEY ([VisitBehaviour_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitBehaviour_User'
CREATE INDEX [IX_FK_VisitBehaviour_User]
ON [dbo].[VisitBehaviours]
    ([VisitBehaviour_CreatedBy]);
GO

-- Creating foreign key on [VisitFoodintake_CreatedBy] in table 'VisitFoodintakes'
ALTER TABLE [dbo].[VisitFoodintakes]
ADD CONSTRAINT [FK_VisitFoodintake_User]
    FOREIGN KEY ([VisitFoodintake_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitFoodintake_User'
CREATE INDEX [IX_FK_VisitFoodintake_User]
ON [dbo].[VisitFoodintakes]
    ([VisitFoodintake_CreatedBy]);
GO

-- Creating foreign key on [PVisit_CreatedBy] in table 'VisitInfoes'
ALTER TABLE [dbo].[VisitInfoes]
ADD CONSTRAINT [FK_VisitInfo_User]
    FOREIGN KEY ([PVisit_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitInfo_User'
CREATE INDEX [IX_FK_VisitInfo_User]
ON [dbo].[VisitInfoes]
    ([PVisit_CreatedBy]);
GO

-- Creating foreign key on [PVOutBoundApprovalBy] in table 'VisitInfoes'
ALTER TABLE [dbo].[VisitInfoes]
ADD CONSTRAINT [FK_VisitInfo_User1]
    FOREIGN KEY ([PVOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitInfo_User1'
CREATE INDEX [IX_FK_VisitInfo_User1]
ON [dbo].[VisitInfoes]
    ([PVOutBoundApprovalBy]);
GO

-- Creating foreign key on [VisitNursingNotes_CreatedBy] in table 'VisitNursingNotes'
ALTER TABLE [dbo].[VisitNursingNotes]
ADD CONSTRAINT [FK_VisitNursingNotes_User]
    FOREIGN KEY ([VisitNursingNotes_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitNursingNotes_User'
CREATE INDEX [IX_FK_VisitNursingNotes_User]
ON [dbo].[VisitNursingNotes]
    ([VisitNursingNotes_CreatedBy]);
GO

-- Creating foreign key on [VitalSigns_CreatedBy] in table 'VisitVitals'
ALTER TABLE [dbo].[VisitVitals]
ADD CONSTRAINT [FK_VitalSigns_User]
    FOREIGN KEY ([VitalSigns_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VitalSigns_User'
CREATE INDEX [IX_FK_VitalSigns_User]
ON [dbo].[VisitVitals]
    ([VitalSigns_CreatedBy]);
GO

-- Creating foreign key on [Weekdays_CreatedBy] in table 'WeekDays'
ALTER TABLE [dbo].[WeekDays]
ADD CONSTRAINT [FK_WeekDays_User]
    FOREIGN KEY ([Weekdays_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WeekDays_User'
CREATE INDEX [IX_FK_WeekDays_User]
ON [dbo].[WeekDays]
    ([Weekdays_CreatedBy]);
GO

-- Creating foreign key on [Week_CreatedBy] in table 'Weeks'
ALTER TABLE [dbo].[Weeks]
ADD CONSTRAINT [FK_Weeks_User]
    FOREIGN KEY ([Week_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Weeks_User'
CREATE INDEX [IX_FK_Weeks_User]
ON [dbo].[Weeks]
    ([Week_CreatedBy]);
GO

-- Creating foreign key on [Wing_CreatedBy] in table 'Wings'
ALTER TABLE [dbo].[Wings]
ADD CONSTRAINT [FK_Wing_User]
    FOREIGN KEY ([Wing_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Wing_User'
CREATE INDEX [IX_FK_Wing_User]
ON [dbo].[Wings]
    ([Wing_CreatedBy]);
GO

-- Creating foreign key on [PVisit_Id] in table 'VisitBehaviours'
ALTER TABLE [dbo].[VisitBehaviours]
ADD CONSTRAINT [FK_VisitBehaviour_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitBehaviour_VisitInfo'
CREATE INDEX [IX_FK_VisitBehaviour_VisitInfo]
ON [dbo].[VisitBehaviours]
    ([PVisit_Id]);
GO

-- Creating foreign key on [PVisit_Id] in table 'VisitFoodintakes'
ALTER TABLE [dbo].[VisitFoodintakes]
ADD CONSTRAINT [FK_VisitFoodintake_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitFoodintake_VisitInfo'
CREATE INDEX [IX_FK_VisitFoodintake_VisitInfo]
ON [dbo].[VisitFoodintakes]
    ([PVisit_Id]);
GO

-- Creating foreign key on [PVisit_Id] in table 'VisitNursingNotes'
ALTER TABLE [dbo].[VisitNursingNotes]
ADD CONSTRAINT [FK_VisitNursingNotes_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitNursingNotes_VisitInfo'
CREATE INDEX [IX_FK_VisitNursingNotes_VisitInfo]
ON [dbo].[VisitNursingNotes]
    ([PVisit_Id]);
GO

-- Creating foreign key on [PVisit_Id] in table 'VisitVitals'
ALTER TABLE [dbo].[VisitVitals]
ADD CONSTRAINT [FK_VitalSigns_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VitalSigns_VisitInfo'
CREATE INDEX [IX_FK_VitalSigns_VisitInfo]
ON [dbo].[VisitVitals]
    ([PVisit_Id]);
GO

-- Creating foreign key on [PAllergy_CreatedBy] in table 'AllergyInfoes'
ALTER TABLE [dbo].[AllergyInfoes]
ADD CONSTRAINT [FK_AllergyInfo_User]
    FOREIGN KEY ([PAllergy_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AllergyInfo_User'
CREATE INDEX [IX_FK_AllergyInfo_User]
ON [dbo].[AllergyInfoes]
    ([PAllergy_CreatedBy]);
GO

-- Creating foreign key on [PAOutBoundApprovalBy] in table 'AllergyInfoes'
ALTER TABLE [dbo].[AllergyInfoes]
ADD CONSTRAINT [FK_AllergyInfo_User1]
    FOREIGN KEY ([PAOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AllergyInfo_User1'
CREATE INDEX [IX_FK_AllergyInfo_User1]
ON [dbo].[AllergyInfoes]
    ([PAOutBoundApprovalBy]);
GO

-- Creating foreign key on [PDiagnosis_CreatedBy] in table 'DiagnosisInfoes'
ALTER TABLE [dbo].[DiagnosisInfoes]
ADD CONSTRAINT [FK_DiagnosisInfo_User]
    FOREIGN KEY ([PDiagnosis_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiagnosisInfo_User'
CREATE INDEX [IX_FK_DiagnosisInfo_User]
ON [dbo].[DiagnosisInfoes]
    ([PDiagnosis_CreatedBy]);
GO

-- Creating foreign key on [PDGOutBoundApprovalBy] in table 'DiagnosisInfoes'
ALTER TABLE [dbo].[DiagnosisInfoes]
ADD CONSTRAINT [FK_DiagnosisInfo_User1]
    FOREIGN KEY ([PDGOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiagnosisInfo_User1'
CREATE INDEX [IX_FK_DiagnosisInfo_User1]
ON [dbo].[DiagnosisInfoes]
    ([PDGOutBoundApprovalBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'MailConfigs'
ALTER TABLE [dbo].[MailConfigs]
ADD CONSTRAINT [FK_MailConfig_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MailConfig_User'
CREATE INDEX [IX_FK_MailConfig_User]
ON [dbo].[MailConfigs]
    ([CreatedBy]);
GO

-- Creating foreign key on [OrderDestroy_CreatedBy] in table 'OrderDestroys'
ALTER TABLE [dbo].[OrderDestroys]
ADD CONSTRAINT [FK_OrderDestroy_User]
    FOREIGN KEY ([OrderDestroy_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDestroy_User'
CREATE INDEX [IX_FK_OrderDestroy_User]
ON [dbo].[OrderDestroys]
    ([OrderDestroy_CreatedBy]);
GO

-- Creating foreign key on [DestroyerUserId] in table 'OrderDestroys'
ALTER TABLE [dbo].[OrderDestroys]
ADD CONSTRAINT [FK_OrderDestroy_User1]
    FOREIGN KEY ([DestroyerUserId])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDestroy_User1'
CREATE INDEX [IX_FK_OrderDestroy_User1]
ON [dbo].[OrderDestroys]
    ([DestroyerUserId]);
GO

-- Creating foreign key on [ApprovalUserId] in table 'OrderDestroys'
ALTER TABLE [dbo].[OrderDestroys]
ADD CONSTRAINT [FK_OrderDestroy_User2]
    FOREIGN KEY ([ApprovalUserId])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDestroy_User2'
CREATE INDEX [IX_FK_OrderDestroy_User2]
ON [dbo].[OrderDestroys]
    ([ApprovalUserId]);
GO

-- Creating foreign key on [PAllergy_CreatedBy] in table 'ApprovalAllergyInfoes'
ALTER TABLE [dbo].[ApprovalAllergyInfoes]
ADD CONSTRAINT [FK_ApprovalAllergyInfo_User]
    FOREIGN KEY ([PAllergy_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalAllergyInfo_User'
CREATE INDEX [IX_FK_ApprovalAllergyInfo_User]
ON [dbo].[ApprovalAllergyInfoes]
    ([PAllergy_CreatedBy]);
GO

-- Creating foreign key on [PAOutBoundApprovalBy] in table 'ApprovalAllergyInfoes'
ALTER TABLE [dbo].[ApprovalAllergyInfoes]
ADD CONSTRAINT [FK_ApprovalAllergyInfo_User1]
    FOREIGN KEY ([PAOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalAllergyInfo_User1'
CREATE INDEX [IX_FK_ApprovalAllergyInfo_User1]
ON [dbo].[ApprovalAllergyInfoes]
    ([PAOutBoundApprovalBy]);
GO

-- Creating foreign key on [PDiagnosis_CreatedBy] in table 'ApprovalDiagnosisInfoes'
ALTER TABLE [dbo].[ApprovalDiagnosisInfoes]
ADD CONSTRAINT [FK_ApprovalDiagnosisInfo_User]
    FOREIGN KEY ([PDiagnosis_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalDiagnosisInfo_User'
CREATE INDEX [IX_FK_ApprovalDiagnosisInfo_User]
ON [dbo].[ApprovalDiagnosisInfoes]
    ([PDiagnosis_CreatedBy]);
GO

-- Creating foreign key on [PDGOutBoundApprovalBy] in table 'ApprovalDiagnosisInfoes'
ALTER TABLE [dbo].[ApprovalDiagnosisInfoes]
ADD CONSTRAINT [FK_ApprovalDiagnosisInfo_User1]
    FOREIGN KEY ([PDGOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalDiagnosisInfo_User1'
CREATE INDEX [IX_FK_ApprovalDiagnosisInfo_User1]
ON [dbo].[ApprovalDiagnosisInfoes]
    ([PDGOutBoundApprovalBy]);
GO

-- Creating foreign key on [PVisit_CreatedBy] in table 'ApprovalVisitInfoes'
ALTER TABLE [dbo].[ApprovalVisitInfoes]
ADD CONSTRAINT [FK_ApprovalVisitInfo_User]
    FOREIGN KEY ([PVisit_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalVisitInfo_User'
CREATE INDEX [IX_FK_ApprovalVisitInfo_User]
ON [dbo].[ApprovalVisitInfoes]
    ([PVisit_CreatedBy]);
GO

-- Creating foreign key on [PVOutBoundApprovalBy] in table 'ApprovalVisitInfoes'
ALTER TABLE [dbo].[ApprovalVisitInfoes]
ADD CONSTRAINT [FK_ApprovalVisitInfo_User1]
    FOREIGN KEY ([PVOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalVisitInfo_User1'
CREATE INDEX [IX_FK_ApprovalVisitInfo_User1]
ON [dbo].[ApprovalVisitInfoes]
    ([PVOutBoundApprovalBy]);
GO

-- Creating foreign key on [PVisit_Id] in table 'ApprovalVisitInfoes'
ALTER TABLE [dbo].[ApprovalVisitInfoes]
ADD CONSTRAINT [FK_ApprovalVisitInfo_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalVisitInfo_VisitInfo'
CREATE INDEX [IX_FK_ApprovalVisitInfo_VisitInfo]
ON [dbo].[ApprovalVisitInfoes]
    ([PVisit_Id]);
GO

-- Creating foreign key on [ApprovedBy] in table 'ControlSubstanceCounts'
ALTER TABLE [dbo].[ControlSubstanceCounts]
ADD CONSTRAINT [FK_ControlSubstanceCount_User]
    FOREIGN KEY ([ApprovedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlSubstanceCount_User'
CREATE INDEX [IX_FK_ControlSubstanceCount_User]
ON [dbo].[ControlSubstanceCounts]
    ([ApprovedBy]);
GO

-- Creating foreign key on [CertifiedBy] in table 'ControlSubstanceCounts'
ALTER TABLE [dbo].[ControlSubstanceCounts]
ADD CONSTRAINT [FK_ControlSubstanceCount_User1]
    FOREIGN KEY ([CertifiedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlSubstanceCount_User1'
CREATE INDEX [IX_FK_ControlSubstanceCount_User1]
ON [dbo].[ControlSubstanceCounts]
    ([CertifiedBy]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'UserRoleFacilityConfigs'
ALTER TABLE [dbo].[UserRoleFacilityConfigs]
ADD CONSTRAINT [FK_UserRoleFacilityConfig_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleFacilityConfig_NursingStation'
CREATE INDEX [IX_FK_UserRoleFacilityConfig_NursingStation]
ON [dbo].[UserRoleFacilityConfigs]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Creator_Id] in table 'Conversations'
ALTER TABLE [dbo].[Conversations]
ADD CONSTRAINT [FK_Conversation_Users]
    FOREIGN KEY ([Creator_Id])
    REFERENCES [dbo].[Users1]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Conversation_Users'
CREATE INDEX [IX_FK_Conversation_Users]
ON [dbo].[Conversations]
    ([Creator_Id]);
GO

-- Creating foreign key on [Conversation_Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Messages_Conversation]
    FOREIGN KEY ([Conversation_Id])
    REFERENCES [dbo].[Conversations]
        ([Conversation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Messages_Conversation'
CREATE INDEX [IX_FK_Messages_Conversation]
ON [dbo].[Messages]
    ([Conversation_Id]);
GO

-- Creating foreign key on [Conversation_Id] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_Participants_Conversation]
    FOREIGN KEY ([Conversation_Id])
    REFERENCES [dbo].[Conversations]
        ([Conversation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Participants_Conversation'
CREATE INDEX [IX_FK_Participants_Conversation]
ON [dbo].[Participants]
    ([Conversation_Id]);
GO

-- Creating foreign key on [Sender_Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Messages_Users]
    FOREIGN KEY ([Sender_Id])
    REFERENCES [dbo].[Users1]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Messages_Users'
CREATE INDEX [IX_FK_Messages_Users]
ON [dbo].[Messages]
    ([Sender_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_Participants_Users]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users1]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Participants_Users'
CREATE INDEX [IX_FK_Participants_Users]
ON [dbo].[Participants]
    ([User_Id]);
GO

-- Creating foreign key on [UDocuments_CreatedBy] in table 'UploadedDocuments'
ALTER TABLE [dbo].[UploadedDocuments]
ADD CONSTRAINT [FK_UploadedDocuments_User]
    FOREIGN KEY ([UDocuments_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UploadedDocuments_User'
CREATE INDEX [IX_FK_UploadedDocuments_User]
ON [dbo].[UploadedDocuments]
    ([UDocuments_CreatedBy]);
GO

-- Creating foreign key on [Allergy_CreatedBy] in table 'AllergyInfoMasters'
ALTER TABLE [dbo].[AllergyInfoMasters]
ADD CONSTRAINT [FK_AllergyInfo_User2]
    FOREIGN KEY ([Allergy_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AllergyInfo_User2'
CREATE INDEX [IX_FK_AllergyInfo_User2]
ON [dbo].[AllergyInfoMasters]
    ([Allergy_CreatedBy]);
GO

-- Creating foreign key on [Activity_CreatedBy] in table 'ActivityMasters'
ALTER TABLE [dbo].[ActivityMasters]
ADD CONSTRAINT [FK_ActivityMaster_User]
    FOREIGN KEY ([Activity_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivityMaster_User'
CREATE INDEX [IX_FK_ActivityMaster_User]
ON [dbo].[ActivityMasters]
    ([Activity_CreatedBy]);
GO

-- Creating foreign key on [Activity_Id] in table 'UserActivityDetails'
ALTER TABLE [dbo].[UserActivityDetails]
ADD CONSTRAINT [FK_UserActivityDetails_ActivityMaster]
    FOREIGN KEY ([Activity_Id])
    REFERENCES [dbo].[ActivityMasters]
        ([Activity_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserActivityDetails_ActivityMaster'
CREATE INDEX [IX_FK_UserActivityDetails_ActivityMaster]
ON [dbo].[UserActivityDetails]
    ([Activity_Id]);
GO

-- Creating foreign key on [Screen_Id] in table 'UserActivityDetails'
ALTER TABLE [dbo].[UserActivityDetails]
ADD CONSTRAINT [FK_UserActivityDetails_Screens]
    FOREIGN KEY ([Screen_Id])
    REFERENCES [dbo].[Screens]
        ([Screen_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserActivityDetails_Screens'
CREATE INDEX [IX_FK_UserActivityDetails_Screens]
ON [dbo].[UserActivityDetails]
    ([Screen_Id]);
GO

-- Creating foreign key on [user_Id] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSession_User]
    FOREIGN KEY ([user_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSession_User'
CREATE INDEX [IX_FK_UserSession_User]
ON [dbo].[UserSessions]
    ([user_Id]);
GO

-- Creating foreign key on [Session_Id] in table 'UserActivityDetails'
ALTER TABLE [dbo].[UserActivityDetails]
ADD CONSTRAINT [FK_UserActivityDetails_UserSession]
    FOREIGN KEY ([Session_Id])
    REFERENCES [dbo].[UserSessions]
        ([Session_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserActivityDetails_UserSession'
CREATE INDEX [IX_FK_UserActivityDetails_UserSession]
ON [dbo].[UserActivityDetails]
    ([Session_Id]);
GO

-- Creating foreign key on [File_Id] in table 'FileInfoErrors'
ALTER TABLE [dbo].[FileInfoErrors]
ADD CONSTRAINT [FK_FileInfoError_FileInformation]
    FOREIGN KEY ([File_Id])
    REFERENCES [dbo].[FileInformations]
        ([File_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileInfoError_FileInformation'
CREATE INDEX [IX_FK_FileInfoError_FileInformation]
ON [dbo].[FileInfoErrors]
    ([File_Id]);
GO

-- Creating foreign key on [OrderFavMaster_ID] in table 'OrderFavouriteDatas'
ALTER TABLE [dbo].[OrderFavouriteDatas]
ADD CONSTRAINT [FK_FavouriteData_OrderFavouriteMaster]
    FOREIGN KEY ([OrderFavMaster_ID])
    REFERENCES [dbo].[OrderFavouriteMasters]
        ([OrderFavMaster_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FavouriteData_OrderFavouriteMaster'
CREATE INDEX [IX_FK_FavouriteData_OrderFavouriteMaster]
ON [dbo].[OrderFavouriteDatas]
    ([OrderFavMaster_ID]);
GO

-- Creating foreign key on [FavouriteData_CreatedBy] in table 'OrderFavouriteDatas'
ALTER TABLE [dbo].[OrderFavouriteDatas]
ADD CONSTRAINT [FK_FavouriteData_User]
    FOREIGN KEY ([FavouriteData_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FavouriteData_User'
CREATE INDEX [IX_FK_FavouriteData_User]
ON [dbo].[OrderFavouriteDatas]
    ([FavouriteData_CreatedBy]);
GO

-- Creating foreign key on [FavouriteData_Id] in table 'OrderFavouriteDatas'
ALTER TABLE [dbo].[OrderFavouriteDatas]
ADD CONSTRAINT [FK_FavouriteData_FavouriteData]
    FOREIGN KEY ([FavouriteData_Id])
    REFERENCES [dbo].[OrderFavouriteDatas]
        ([FavouriteData_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [comment_CreatedBy] in table 'NurseComments'
ALTER TABLE [dbo].[NurseComments]
ADD CONSTRAINT [FK_NurseComments_User]
    FOREIGN KEY ([comment_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseComments_User'
CREATE INDEX [IX_FK_NurseComments_User]
ON [dbo].[NurseComments]
    ([comment_CreatedBy]);
GO

-- Creating foreign key on [PatientType_CreatedBy] in table 'PatientTypes'
ALTER TABLE [dbo].[PatientTypes]
ADD CONSTRAINT [FK_PatientType_User]
    FOREIGN KEY ([PatientType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientType_User'
CREATE INDEX [IX_FK_PatientType_User]
ON [dbo].[PatientTypes]
    ([PatientType_CreatedBy]);
GO

-- Creating foreign key on [PatientType_Id] in table 'ColourTypes'
ALTER TABLE [dbo].[ColourTypes]
ADD CONSTRAINT [FK_ColourType_PatientType]
    FOREIGN KEY ([PatientType_Id])
    REFERENCES [dbo].[PatientTypes]
        ([PatientType_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ColourType_PatientType'
CREATE INDEX [IX_FK_ColourType_PatientType]
ON [dbo].[ColourTypes]
    ([PatientType_Id]);
GO

-- Creating foreign key on [ColourType_CreatedBy] in table 'ColourTypes'
ALTER TABLE [dbo].[ColourTypes]
ADD CONSTRAINT [FK_ColourType_User]
    FOREIGN KEY ([ColourType_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ColourType_User'
CREATE INDEX [IX_FK_ColourType_User]
ON [dbo].[ColourTypes]
    ([ColourType_CreatedBy]);
GO

-- Creating foreign key on [Fingersdesc_CreatedBy] in table 'Fingersdescs'
ALTER TABLE [dbo].[Fingersdescs]
ADD CONSTRAINT [FK_Fingersdesc_User]
    FOREIGN KEY ([Fingersdesc_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Fingersdesc_User'
CREATE INDEX [IX_FK_Fingersdesc_User]
ON [dbo].[Fingersdescs]
    ([Fingersdesc_CreatedBy]);
GO

-- Creating foreign key on [PBarcode_CreatedBy] in table 'BarcodeDetails'
ALTER TABLE [dbo].[BarcodeDetails]
ADD CONSTRAINT [FK_BarcodeDetails_User1]
    FOREIGN KEY ([PBarcode_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BarcodeDetails_User1'
CREATE INDEX [IX_FK_BarcodeDetails_User1]
ON [dbo].[BarcodeDetails]
    ([PBarcode_CreatedBy]);
GO

-- Creating foreign key on [user_Id] in table 'MailFavourites'
ALTER TABLE [dbo].[MailFavourites]
ADD CONSTRAINT [FK_MailFavourite_User]
    FOREIGN KEY ([user_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MailFavourite_User'
CREATE INDEX [IX_FK_MailFavourite_User]
ON [dbo].[MailFavourites]
    ([user_Id]);
GO

-- Creating foreign key on [user_Id] in table 'MailReads'
ALTER TABLE [dbo].[MailReads]
ADD CONSTRAINT [FK_MailRead_User]
    FOREIGN KEY ([user_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MailRead_User'
CREATE INDEX [IX_FK_MailRead_User]
ON [dbo].[MailReads]
    ([user_Id]);
GO

-- Creating foreign key on [AlertText_Id] in table 'AlertStatus'
ALTER TABLE [dbo].[AlertStatus]
ADD CONSTRAINT [FK_Alert_AlertText]
    FOREIGN KEY ([AlertText_Id])
    REFERENCES [dbo].[AlertTexts]
        ([AlertText_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Alert_AlertText'
CREATE INDEX [IX_FK_Alert_AlertText]
ON [dbo].[AlertStatus]
    ([AlertText_Id]);
GO

-- Creating foreign key on [user_Id] in table 'AlertStatus'
ALTER TABLE [dbo].[AlertStatus]
ADD CONSTRAINT [FK_Alert_User]
    FOREIGN KEY ([user_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Alert_User'
CREATE INDEX [IX_FK_Alert_User]
ON [dbo].[AlertStatus]
    ([user_Id]);
GO

-- Creating foreign key on [Type_Id] in table 'AlertTexts'
ALTER TABLE [dbo].[AlertTexts]
ADD CONSTRAINT [FK_AlertText_AlertType]
    FOREIGN KEY ([Type_Id])
    REFERENCES [dbo].[AlertTypes]
        ([Type_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlertText_AlertType'
CREATE INDEX [IX_FK_AlertText_AlertType]
ON [dbo].[AlertTexts]
    ([Type_Id]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'AlertTexts'
ALTER TABLE [dbo].[AlertTexts]
ADD CONSTRAINT [FK_AlertText_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlertText_NursingStation'
CREATE INDEX [IX_FK_AlertText_NursingStation]
ON [dbo].[AlertTexts]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Route_CreatedBy] in table 'Routes'
ALTER TABLE [dbo].[Routes]
ADD CONSTRAINT [FK_Route_User]
    FOREIGN KEY ([Route_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Route_User'
CREATE INDEX [IX_FK_Route_User]
ON [dbo].[Routes]
    ([Route_CreatedBy]);
GO

-- Creating foreign key on [Ekit_Id] in table 'BarcodeDetails'
ALTER TABLE [dbo].[BarcodeDetails]
ADD CONSTRAINT [FK_BarcodeDetails_Ekit1]
    FOREIGN KEY ([Ekit_Id])
    REFERENCES [dbo].[Ekits]
        ([Ekit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BarcodeDetails_Ekit1'
CREATE INDEX [IX_FK_BarcodeDetails_Ekit1]
ON [dbo].[BarcodeDetails]
    ([Ekit_Id]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'Ekits'
ALTER TABLE [dbo].[Ekits]
ADD CONSTRAINT [FK_Ekit_Facility]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ekit_Facility'
CREATE INDEX [IX_FK_Ekit_Facility]
ON [dbo].[Ekits]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Ekit_CreatedBy] in table 'Ekits'
ALTER TABLE [dbo].[Ekits]
ADD CONSTRAINT [FK_Ekit_User]
    FOREIGN KEY ([Ekit_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ekit_User'
CREATE INDEX [IX_FK_Ekit_User]
ON [dbo].[Ekits]
    ([Ekit_CreatedBy]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [FK_Stock_Facility]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stock_Facility'
CREATE INDEX [IX_FK_Stock_Facility]
ON [dbo].[Stocks]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Stock_Id] in table 'BarcodeDetails'
ALTER TABLE [dbo].[BarcodeDetails]
ADD CONSTRAINT [FK_BarcodeDetails_Stock1]
    FOREIGN KEY ([Stock_Id])
    REFERENCES [dbo].[Stocks]
        ([Stock_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BarcodeDetails_Stock1'
CREATE INDEX [IX_FK_BarcodeDetails_Stock1]
ON [dbo].[BarcodeDetails]
    ([Stock_Id]);
GO

-- Creating foreign key on [Stock_CreatedBy] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [FK_Stock_User]
    FOREIGN KEY ([Stock_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stock_User'
CREATE INDEX [IX_FK_Stock_User]
ON [dbo].[Stocks]
    ([Stock_CreatedBy]);
GO

-- Creating foreign key on [Drug_CreatedBy] in table 'Drugs'
ALTER TABLE [dbo].[Drugs]
ADD CONSTRAINT [FK_Drug_User]
    FOREIGN KEY ([Drug_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Drug_User'
CREATE INDEX [IX_FK_Drug_User]
ON [dbo].[Drugs]
    ([Drug_CreatedBy]);
GO

-- Creating foreign key on [Ekit_Id] in table 'EkitAdministers'
ALTER TABLE [dbo].[EkitAdministers]
ADD CONSTRAINT [FK_EkitAdminister_Ekit]
    FOREIGN KEY ([Ekit_Id])
    REFERENCES [dbo].[Ekits]
        ([Ekit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkitAdminister_Ekit'
CREATE INDEX [IX_FK_EkitAdminister_Ekit]
ON [dbo].[EkitAdministers]
    ([Ekit_Id]);
GO

-- Creating foreign key on [EkitAdministerBy] in table 'EkitAdministers'
ALTER TABLE [dbo].[EkitAdministers]
ADD CONSTRAINT [FK_EkitAdminister_User]
    FOREIGN KEY ([EkitAdministerBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkitAdminister_User'
CREATE INDEX [IX_FK_EkitAdminister_User]
ON [dbo].[EkitAdministers]
    ([EkitAdministerBy]);
GO

-- Creating foreign key on [Fromtime_hoursId] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_Hours]
    FOREIGN KEY ([Fromtime_hoursId])
    REFERENCES [dbo].[Hours]
        ([Hour_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_Hours'
CREATE INDEX [IX_FK_NurseShifts_Hours]
ON [dbo].[NurseShifts]
    ([Fromtime_hoursId]);
GO

-- Creating foreign key on [Totime_hoursId] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_Hours1]
    FOREIGN KEY ([Totime_hoursId])
    REFERENCES [dbo].[Hours]
        ([Hour_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_Hours1'
CREATE INDEX [IX_FK_NurseShifts_Hours1]
ON [dbo].[NurseShifts]
    ([Totime_hoursId]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_NursingStation'
CREATE INDEX [IX_FK_NurseShifts_NursingStation]
ON [dbo].[NurseShifts]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [FromTime_TimeFormatId] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_TimeFormat]
    FOREIGN KEY ([FromTime_TimeFormatId])
    REFERENCES [dbo].[TimeFormats]
        ([TimeFormat_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_TimeFormat'
CREATE INDEX [IX_FK_NurseShifts_TimeFormat]
ON [dbo].[NurseShifts]
    ([FromTime_TimeFormatId]);
GO

-- Creating foreign key on [ToTime_TimeFormatId] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_TimeFormat1]
    FOREIGN KEY ([ToTime_TimeFormatId])
    REFERENCES [dbo].[TimeFormats]
        ([TimeFormat_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_TimeFormat1'
CREATE INDEX [IX_FK_NurseShifts_TimeFormat1]
ON [dbo].[NurseShifts]
    ([ToTime_TimeFormatId]);
GO

-- Creating foreign key on [NurseShifts_CreatedBy] in table 'NurseShifts'
ALTER TABLE [dbo].[NurseShifts]
ADD CONSTRAINT [FK_NurseShifts_User]
    FOREIGN KEY ([NurseShifts_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseShifts_User'
CREATE INDEX [IX_FK_NurseShifts_User]
ON [dbo].[NurseShifts]
    ([NurseShifts_CreatedBy]);
GO

-- Creating foreign key on [Fingersdesc_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_Fingersdesc]
    FOREIGN KEY ([Fingersdesc_Id])
    REFERENCES [dbo].[Fingersdescs]
        ([Fingersdesc_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_Fingersdesc'
CREATE INDEX [IX_FK_CompanyConfig_Fingersdesc]
ON [dbo].[CompanyConfigs]
    ([Fingersdesc_Id]);
GO

-- Creating foreign key on [HLDirectionalWay_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_HLDirectionalWay]
    FOREIGN KEY ([HLDirectionalWay_Id])
    REFERENCES [dbo].[HLDirectionalWays]
        ([HLDirectionalWay_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_HLDirectionalWay'
CREATE INDEX [IX_FK_CompanyConfig_HLDirectionalWay]
ON [dbo].[CompanyConfigs]
    ([HLDirectionalWay_Id]);
GO

-- Creating foreign key on [CompanyConfig_CreatedBy] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_User]
    FOREIGN KEY ([CompanyConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_User'
CREATE INDEX [IX_FK_CompanyConfig_User]
ON [dbo].[CompanyConfigs]
    ([CompanyConfig_CreatedBy]);
GO

-- Creating foreign key on [CompanyConfig_Id] in table 'CompanyHlCategories'
ALTER TABLE [dbo].[CompanyHlCategories]
ADD CONSTRAINT [FK_CompanyHlCategory_CompanyConfig]
    FOREIGN KEY ([CompanyConfig_Id])
    REFERENCES [dbo].[CompanyConfigs]
        ([CompanyConfig_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyHlCategory_CompanyConfig'
CREATE INDEX [IX_FK_CompanyHlCategory_CompanyConfig]
ON [dbo].[CompanyHlCategories]
    ([CompanyConfig_Id]);
GO

-- Creating foreign key on [FteCategory_Id] in table 'CompanyHlCategories'
ALTER TABLE [dbo].[CompanyHlCategories]
ADD CONSTRAINT [FK_CompanyHlCategory_FTECategory]
    FOREIGN KEY ([FteCategory_Id])
    REFERENCES [dbo].[FTECategories]
        ([FteCategory_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyHlCategory_FTECategory'
CREATE INDEX [IX_FK_CompanyHlCategory_FTECategory]
ON [dbo].[CompanyHlCategories]
    ([FteCategory_Id]);
GO

-- Creating foreign key on [CompanyHlCategory_Id] in table 'CompanyHLEvents'
ALTER TABLE [dbo].[CompanyHLEvents]
ADD CONSTRAINT [FK_CompanyHLEvent_CompanyHlCategory]
    FOREIGN KEY ([CompanyHlCategory_Id])
    REFERENCES [dbo].[CompanyHlCategories]
        ([CompanyHlCategory_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyHLEvent_CompanyHlCategory'
CREATE INDEX [IX_FK_CompanyHLEvent_CompanyHlCategory]
ON [dbo].[CompanyHLEvents]
    ([CompanyHlCategory_Id]);
GO

-- Creating foreign key on [EventCat_Id] in table 'CompanyHLEvents'
ALTER TABLE [dbo].[CompanyHLEvents]
ADD CONSTRAINT [FK_CompanyHLEvent_EventCategory]
    FOREIGN KEY ([EventCat_Id])
    REFERENCES [dbo].[EventCategories]
        ([EventCat_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyHLEvent_EventCategory'
CREATE INDEX [IX_FK_CompanyHLEvent_EventCategory]
ON [dbo].[CompanyHLEvents]
    ([EventCat_Id]);
GO

-- Creating foreign key on [HLDirectionalWay_CreatedBy] in table 'HLDirectionalWays'
ALTER TABLE [dbo].[HLDirectionalWays]
ADD CONSTRAINT [FK_HL7DirectionalWay_User]
    FOREIGN KEY ([HLDirectionalWay_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HL7DirectionalWay_User'
CREATE INDEX [IX_FK_HL7DirectionalWay_User]
ON [dbo].[HLDirectionalWays]
    ([HLDirectionalWay_CreatedBy]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'ControlSubstanceCounts'
ALTER TABLE [dbo].[ControlSubstanceCounts]
ADD CONSTRAINT [FK_ControlSubstanceCount_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlSubstanceCount_NursingStation'
CREATE INDEX [IX_FK_ControlSubstanceCount_NursingStation]
ON [dbo].[ControlSubstanceCounts]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [File_Id] in table 'ApprovalRefills'
ALTER TABLE [dbo].[ApprovalRefills]
ADD CONSTRAINT [FK_ApprovalRefill_OutBoundFileInformation]
    FOREIGN KEY ([File_Id])
    REFERENCES [dbo].[OutBoundFileInformations]
        ([File_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalRefill_OutBoundFileInformation'
CREATE INDEX [IX_FK_ApprovalRefill_OutBoundFileInformation]
ON [dbo].[ApprovalRefills]
    ([File_Id]);
GO

-- Creating foreign key on [StockReport_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_StockReport]
    FOREIGN KEY ([StockReport_Id])
    REFERENCES [dbo].[StockReports]
        ([StockReport_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_StockReport'
CREATE INDEX [IX_FK_CompanyConfig_StockReport]
ON [dbo].[CompanyConfigs]
    ([StockReport_Id]);
GO

-- Creating foreign key on [OnLeave_CreatedBy] in table 'OnLeaves'
ALTER TABLE [dbo].[OnLeaves]
ADD CONSTRAINT [FK_OnLeave_User]
    FOREIGN KEY ([OnLeave_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OnLeave_User'
CREATE INDEX [IX_FK_OnLeave_User]
ON [dbo].[OnLeaves]
    ([OnLeave_CreatedBy]);
GO

-- Creating foreign key on [PVisit_Id] in table 'OnLeaves'
ALTER TABLE [dbo].[OnLeaves]
ADD CONSTRAINT [FK_OnLeave_VisitInfo]
    FOREIGN KEY ([PVisit_Id])
    REFERENCES [dbo].[VisitInfoes]
        ([PVisit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OnLeave_VisitInfo'
CREATE INDEX [IX_FK_OnLeave_VisitInfo]
ON [dbo].[OnLeaves]
    ([PVisit_Id]);
GO

-- Creating foreign key on [Floor_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Floor]
    FOREIGN KEY ([Floor_Id])
    REFERENCES [dbo].[Floors]
        ([Floor_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Floor'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Floor]
ON [dbo].[NursingFrequencyConfigs]
    ([Floor_Id]);
GO

-- Creating foreign key on [Frequency_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_FrequencyMaster]
    FOREIGN KEY ([Frequency_Id])
    REFERENCES [dbo].[FrequencyMasters]
        ([Frequency_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_FrequencyMaster'
CREATE INDEX [IX_FK_NursingFrequencyConfig_FrequencyMaster]
ON [dbo].[NursingFrequencyConfigs]
    ([Frequency_Id]);
GO

-- Creating foreign key on [StartTime] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Hours]
    FOREIGN KEY ([StartTime])
    REFERENCES [dbo].[Hours]
        ([Hour_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Hours'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Hours]
ON [dbo].[NursingFrequencyConfigs]
    ([StartTime]);
GO

-- Creating foreign key on [Month_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Months]
    FOREIGN KEY ([Month_Id])
    REFERENCES [dbo].[Months]
        ([Month_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Months'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Months]
ON [dbo].[NursingFrequencyConfigs]
    ([Month_Id]);
GO

-- Creating foreign key on [NursingFreq_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_NursingFrequencyConfig]
    FOREIGN KEY ([NursingFreq_Id])
    REFERENCES [dbo].[NursingFrequencyConfigs]
        ([NursingFreq_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [NursingStation_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_NursingStation1]
    FOREIGN KEY ([NursingStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_NursingStation1'
CREATE INDEX [IX_FK_NursingFrequencyConfig_NursingStation1]
ON [dbo].[NursingFrequencyConfigs]
    ([NursingStation_Id]);
GO

-- Creating foreign key on [NursingFreq_CreatedBy] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_User]
    FOREIGN KEY ([NursingFreq_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_User'
CREATE INDEX [IX_FK_NursingFrequencyConfig_User]
ON [dbo].[NursingFrequencyConfigs]
    ([NursingFreq_CreatedBy]);
GO

-- Creating foreign key on [Week_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Weeks]
    FOREIGN KEY ([Week_Id])
    REFERENCES [dbo].[Weeks]
        ([Week_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Weeks'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Weeks]
ON [dbo].[NursingFrequencyConfigs]
    ([Week_Id]);
GO

-- Creating foreign key on [Wing_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Wing]
    FOREIGN KEY ([Wing_Id])
    REFERENCES [dbo].[Wings]
        ([Wing_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Wing'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Wing]
ON [dbo].[NursingFrequencyConfigs]
    ([Wing_Id]);
GO

-- Creating foreign key on [Role_CreatedBy] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_Role_User]
    FOREIGN KEY ([Role_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Role_User'
CREATE INDEX [IX_FK_Role_User]
ON [dbo].[Roles]
    ([Role_CreatedBy]);
GO

-- Creating foreign key on [Role_Id] in table 'RoleConfigs'
ALTER TABLE [dbo].[RoleConfigs]
ADD CONSTRAINT [FK_RoleConfig_Role]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[Roles]
        ([Role_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleConfig_Role'
CREATE INDEX [IX_FK_RoleConfig_Role]
ON [dbo].[RoleConfigs]
    ([Role_Id]);
GO

-- Creating foreign key on [Role_ID] in table 'UserRoleFacilityConfigs'
ALTER TABLE [dbo].[UserRoleFacilityConfigs]
ADD CONSTRAINT [FK_UserRoleFacilityConfig_Role]
    FOREIGN KEY ([Role_ID])
    REFERENCES [dbo].[Roles]
        ([Role_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleFacilityConfig_Role'
CREATE INDEX [IX_FK_UserRoleFacilityConfig_Role]
ON [dbo].[UserRoleFacilityConfigs]
    ([Role_ID]);
GO

-- Creating foreign key on [role_Id] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSession_Role]
    FOREIGN KEY ([role_Id])
    REFERENCES [dbo].[Roles]
        ([Role_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSession_Role'
CREATE INDEX [IX_FK_UserSession_Role]
ON [dbo].[UserSessions]
    ([role_Id]);
GO

-- Creating foreign key on [Screen_Id] in table 'DefaultScreens'
ALTER TABLE [dbo].[DefaultScreens]
ADD CONSTRAINT [FK_DefaultScreens_Screens]
    FOREIGN KEY ([Screen_Id])
    REFERENCES [dbo].[Screens]
        ([Screen_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DefaultScreens_Screens'
CREATE INDEX [IX_FK_DefaultScreens_Screens]
ON [dbo].[DefaultScreens]
    ([Screen_Id]);
GO

-- Creating foreign key on [DefaultScreen_CreatedBy] in table 'DefaultScreens'
ALTER TABLE [dbo].[DefaultScreens]
ADD CONSTRAINT [FK_DefaultScreens_User]
    FOREIGN KEY ([DefaultScreen_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DefaultScreens_User'
CREATE INDEX [IX_FK_DefaultScreens_User]
ON [dbo].[DefaultScreens]
    ([DefaultScreen_CreatedBy]);
GO

-- Creating foreign key on [DefaultScreen_Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_Role_DefaultScreens]
    FOREIGN KEY ([DefaultScreen_Id])
    REFERENCES [dbo].[DefaultScreens]
        ([DefaultScreen_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Role_DefaultScreens'
CREATE INDEX [IX_FK_Role_DefaultScreens]
ON [dbo].[Roles]
    ([DefaultScreen_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Company'
CREATE INDEX [IX_FK_CompanyBedConfig_Company]
ON [dbo].[CompanyBedConfigs]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_Company'
CREATE INDEX [IX_FK_CompanyConfig_Company]
ON [dbo].[CompanyConfigs]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'DrFirstFileDatas'
ALTER TABLE [dbo].[DrFirstFileDatas]
ADD CONSTRAINT [FK_DrFirstFileData_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrFirstFileData_Company'
CREATE INDEX [IX_FK_DrFirstFileData_Company]
ON [dbo].[DrFirstFileDatas]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [FK_Facility_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Facility_Company'
CREATE INDEX [IX_FK_Facility_Company]
ON [dbo].[Facilities]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'FileAckInformations'
ALTER TABLE [dbo].[FileAckInformations]
ADD CONSTRAINT [FK_FileAckInformation_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileAckInformation_Company'
CREATE INDEX [IX_FK_FileAckInformation_Company]
ON [dbo].[FileAckInformations]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'FileInformations'
ALTER TABLE [dbo].[FileInformations]
ADD CONSTRAINT [FK_FileInformation_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileInformation_Company'
CREATE INDEX [IX_FK_FileInformation_Company]
ON [dbo].[FileInformations]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'HLSevenCompanyConfigs'
ALTER TABLE [dbo].[HLSevenCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenCompanyConfig_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenCompanyConfig_Company'
CREATE INDEX [IX_FK_HLSevenCompanyConfig_Company]
ON [dbo].[HLSevenCompanyConfigs]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'OutBoundFileInformations'
ALTER TABLE [dbo].[OutBoundFileInformations]
ADD CONSTRAINT [FK_OutBoundFileInformation_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutBoundFileInformation_Company'
CREATE INDEX [IX_FK_OutBoundFileInformation_Company]
ON [dbo].[OutBoundFileInformations]
    ([Company_Id]);
GO

-- Creating foreign key on [Company_Id] in table 'PatientTypes'
ALTER TABLE [dbo].[PatientTypes]
ADD CONSTRAINT [FK_PatientType_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientType_Company'
CREATE INDEX [IX_FK_PatientType_Company]
ON [dbo].[PatientTypes]
    ([Company_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'CompanyBedConfigs'
ALTER TABLE [dbo].[CompanyBedConfigs]
ADD CONSTRAINT [FK_CompanyBedConfig_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBedConfig_Facility'
CREATE INDEX [IX_FK_CompanyBedConfig_Facility]
ON [dbo].[CompanyBedConfigs]
    ([Facility_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_Facility'
CREATE INDEX [IX_FK_NursingFrequencyConfig_Facility]
ON [dbo].[NursingFrequencyConfigs]
    ([Facility_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'NursingStations'
ALTER TABLE [dbo].[NursingStations]
ADD CONSTRAINT [FK_NursingStation_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingStation_Facility'
CREATE INDEX [IX_FK_NursingStation_Facility]
ON [dbo].[NursingStations]
    ([Facility_Id]);
GO

-- Creating foreign key on [Physician_Id] in table 'CompanyConfigs'
ALTER TABLE [dbo].[CompanyConfigs]
ADD CONSTRAINT [FK_CompanyConfig_PhysicianDetails]
    FOREIGN KEY ([Physician_Id])
    REFERENCES [dbo].[PhysicianDetails]
        ([Physician_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyConfig_PhysicianDetails'
CREATE INDEX [IX_FK_CompanyConfig_PhysicianDetails]
ON [dbo].[CompanyConfigs]
    ([Physician_Id]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'PhysicianDetails'
ALTER TABLE [dbo].[PhysicianDetails]
ADD CONSTRAINT [FK_PhysicianDetails_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PhysicianDetails_NursingStation'
CREATE INDEX [IX_FK_PhysicianDetails_NursingStation]
ON [dbo].[PhysicianDetails]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Physician_CreatedBy] in table 'PhysicianDetails'
ALTER TABLE [dbo].[PhysicianDetails]
ADD CONSTRAINT [FK_PhysicianDetails_User]
    FOREIGN KEY ([Physician_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PhysicianDetails_User'
CREATE INDEX [IX_FK_PhysicianDetails_User]
ON [dbo].[PhysicianDetails]
    ([Physician_CreatedBy]);
GO

-- Creating foreign key on [hour_Id] in table 'NursingFCTimes'
ALTER TABLE [dbo].[NursingFCTimes]
ADD CONSTRAINT [FK_NursingFCTime_Hours]
    FOREIGN KEY ([hour_Id])
    REFERENCES [dbo].[Hours]
        ([Hour_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFCTime_Hours'
CREATE INDEX [IX_FK_NursingFCTime_Hours]
ON [dbo].[NursingFCTimes]
    ([hour_Id]);
GO

-- Creating foreign key on [NursingFreq_Id] in table 'NursingFCTimes'
ALTER TABLE [dbo].[NursingFCTimes]
ADD CONSTRAINT [FK_NursingFCTime_NursingFrequencyConfig]
    FOREIGN KEY ([NursingFreq_Id])
    REFERENCES [dbo].[NursingFrequencyConfigs]
        ([NursingFreq_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFCTime_NursingFrequencyConfig'
CREATE INDEX [IX_FK_NursingFCTime_NursingFrequencyConfig]
ON [dbo].[NursingFCTimes]
    ([NursingFreq_Id]);
GO

-- Creating foreign key on [Patient_CreatedBy] in table 'ApprovalDemographics'
ALTER TABLE [dbo].[ApprovalDemographics]
ADD CONSTRAINT [FK_ApprovalDemographics_User]
    FOREIGN KEY ([Patient_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalDemographics_User'
CREATE INDEX [IX_FK_ApprovalDemographics_User]
ON [dbo].[ApprovalDemographics]
    ([Patient_CreatedBy]);
GO

-- Creating foreign key on [PDOutBoundApprovalBy] in table 'ApprovalDemographics'
ALTER TABLE [dbo].[ApprovalDemographics]
ADD CONSTRAINT [FK_ApprovalDemographics_User1]
    FOREIGN KEY ([PDOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalDemographics_User1'
CREATE INDEX [IX_FK_ApprovalDemographics_User1]
ON [dbo].[ApprovalDemographics]
    ([PDOutBoundApprovalBy]);
GO

-- Creating foreign key on [Company_CountryId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_Company_Country]
    FOREIGN KEY ([Company_CountryId])
    REFERENCES [dbo].[Countries]
        ([Country_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Company_Country'
CREATE INDEX [IX_FK_Company_Country]
ON [dbo].[Companies]
    ([Company_CountryId]);
GO

-- Creating foreign key on [Company_CreatedBy] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_Company_User]
    FOREIGN KEY ([Company_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Company_User'
CREATE INDEX [IX_FK_Company_User]
ON [dbo].[Companies]
    ([Company_CreatedBy]);
GO

-- Creating foreign key on [Facility_CountryId] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [FK_Facility_Country]
    FOREIGN KEY ([Facility_CountryId])
    REFERENCES [dbo].[Countries]
        ([Country_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Facility_Country'
CREATE INDEX [IX_FK_Facility_Country]
ON [dbo].[Facilities]
    ([Facility_CountryId]);
GO

-- Creating foreign key on [Facility_CreatedBy] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [FK_Facility_User]
    FOREIGN KEY ([Facility_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Facility_User'
CREATE INDEX [IX_FK_Facility_User]
ON [dbo].[Facilities]
    ([Facility_CreatedBy]);
GO

-- Creating foreign key on [Category] in table 'FTEConfigurations'
ALTER TABLE [dbo].[FTEConfigurations]
ADD CONSTRAINT [FK_FTEConfiguration_FTECategory]
    FOREIGN KEY ([Category])
    REFERENCES [dbo].[FTECategories]
        ([FteCategory_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FTEConfiguration_FTECategory'
CREATE INDEX [IX_FK_FTEConfiguration_FTECategory]
ON [dbo].[FTEConfigurations]
    ([Category]);
GO

-- Creating foreign key on [ConnectionType] in table 'FTEConfigurations'
ALTER TABLE [dbo].[FTEConfigurations]
ADD CONSTRAINT [FK_FTEConfiguration_FteConnection]
    FOREIGN KEY ([ConnectionType])
    REFERENCES [dbo].[FteConnections]
        ([FteConn_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FTEConfiguration_FteConnection'
CREATE INDEX [IX_FK_FTEConfiguration_FteConnection]
ON [dbo].[FTEConfigurations]
    ([ConnectionType]);
GO

-- Creating foreign key on [FteConfig_CreatedBy] in table 'FTEConfigurations'
ALTER TABLE [dbo].[FTEConfigurations]
ADD CONSTRAINT [FK_FTEConfiguration_User]
    FOREIGN KEY ([FteConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FTEConfiguration_User'
CREATE INDEX [IX_FK_FTEConfiguration_User]
ON [dbo].[FTEConfigurations]
    ([FteConfig_CreatedBy]);
GO

-- Creating foreign key on [Facility_Id] in table 'PhysicianDetails'
ALTER TABLE [dbo].[PhysicianDetails]
ADD CONSTRAINT [FK_PhysicianDetails_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PhysicianDetails_Facility'
CREATE INDEX [IX_FK_PhysicianDetails_Facility]
ON [dbo].[PhysicianDetails]
    ([Facility_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'RecentFacs'
ALTER TABLE [dbo].[RecentFacs]
ADD CONSTRAINT [FK_RecentFac_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecentFac_Facility'
CREATE INDEX [IX_FK_RecentFac_Facility]
ON [dbo].[RecentFacs]
    ([Facility_Id]);
GO

-- Creating foreign key on [User_Id] in table 'RecentFacs'
ALTER TABLE [dbo].[RecentFacs]
ADD CONSTRAINT [FK_RecentFac_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecentFac_User'
CREATE INDEX [IX_FK_RecentFac_User]
ON [dbo].[RecentFacs]
    ([User_Id]);
GO

-- Creating foreign key on [DefaultPhysician_Id] in table 'NursingStations'
ALTER TABLE [dbo].[NursingStations]
ADD CONSTRAINT [FK_NursingStation_PhysicianDetails]
    FOREIGN KEY ([DefaultPhysician_Id])
    REFERENCES [dbo].[PhysicianDetails]
        ([Physician_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingStation_PhysicianDetails'
CREATE INDEX [IX_FK_NursingStation_PhysicianDetails]
ON [dbo].[NursingStations]
    ([DefaultPhysician_Id]);
GO

-- Creating foreign key on [TimeFormat_ID] in table 'NursingFrequencyConfigs'
ALTER TABLE [dbo].[NursingFrequencyConfigs]
ADD CONSTRAINT [FK_NursingFrequencyConfig_TimeFormat]
    FOREIGN KEY ([TimeFormat_ID])
    REFERENCES [dbo].[TimeFormats]
        ([TimeFormat_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NursingFrequencyConfig_TimeFormat'
CREATE INDEX [IX_FK_NursingFrequencyConfig_TimeFormat]
ON [dbo].[NursingFrequencyConfigs]
    ([TimeFormat_ID]);
GO

-- Creating foreign key on [OrderStock_CreatedBy] in table 'OrderStocks'
ALTER TABLE [dbo].[OrderStocks]
ADD CONSTRAINT [FK_OrderStock_User]
    FOREIGN KEY ([OrderStock_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderStock_User'
CREATE INDEX [IX_FK_OrderStock_User]
ON [dbo].[OrderStocks]
    ([OrderStock_CreatedBy]);
GO

-- Creating foreign key on [Company_Id] in table 'HLSevenOutboundDisplayCompanyConfigs'
ALTER TABLE [dbo].[HLSevenOutboundDisplayCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_Company]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Company_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplayCompanyConfig_Company'
CREATE INDEX [IX_FK_HLSevenOutboundDisplayCompanyConfig_Company]
ON [dbo].[HLSevenOutboundDisplayCompanyConfigs]
    ([Company_Id]);
GO

-- Creating foreign key on [OSegDetail_Id] in table 'HLSevenOutboundDisplayCompanyConfigs'
ALTER TABLE [dbo].[HLSevenOutboundDisplayCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_HLSevenOutboundDisplaySegmentDetails]
    FOREIGN KEY ([OSegDetail_Id])
    REFERENCES [dbo].[HLSevenOutboundDisplaySegmentDetails]
        ([OSegDetail_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplayCompanyConfig_HLSevenOutboundDisplaySegmentDetails'
CREATE INDEX [IX_FK_HLSevenOutboundDisplayCompanyConfig_HLSevenOutboundDisplaySegmentDetails]
ON [dbo].[HLSevenOutboundDisplayCompanyConfigs]
    ([OSegDetail_Id]);
GO

-- Creating foreign key on [OHLConfig_CreatedBy] in table 'HLSevenOutboundDisplayCompanyConfigs'
ALTER TABLE [dbo].[HLSevenOutboundDisplayCompanyConfigs]
ADD CONSTRAINT [FK_HLSevenOutboundDisplayCompanyConfig_User]
    FOREIGN KEY ([OHLConfig_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplayCompanyConfig_User'
CREATE INDEX [IX_FK_HLSevenOutboundDisplayCompanyConfig_User]
ON [dbo].[HLSevenOutboundDisplayCompanyConfigs]
    ([OHLConfig_CreatedBy]);
GO

-- Creating foreign key on [OSegment_Id] in table 'HLSevenOutboundDisplaySegmentDetails'
ALTER TABLE [dbo].[HLSevenOutboundDisplaySegmentDetails]
ADD CONSTRAINT [FK_HLSevenOutboundDisplaySegmentDetails_HLSevenOutboundDisplaySegments1]
    FOREIGN KEY ([OSegment_Id])
    REFERENCES [dbo].[HLSevenOutboundDisplaySegments]
        ([OSegment_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplaySegmentDetails_HLSevenOutboundDisplaySegments1'
CREATE INDEX [IX_FK_HLSevenOutboundDisplaySegmentDetails_HLSevenOutboundDisplaySegments1]
ON [dbo].[HLSevenOutboundDisplaySegmentDetails]
    ([OSegment_Id]);
GO

-- Creating foreign key on [OSegDetail_CreatedBy] in table 'HLSevenOutboundDisplaySegmentDetails'
ALTER TABLE [dbo].[HLSevenOutboundDisplaySegmentDetails]
ADD CONSTRAINT [FK_HLSevenOutboundDisplaySegmentDetails_User]
    FOREIGN KEY ([OSegDetail_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplaySegmentDetails_User'
CREATE INDEX [IX_FK_HLSevenOutboundDisplaySegmentDetails_User]
ON [dbo].[HLSevenOutboundDisplaySegmentDetails]
    ([OSegDetail_CreatedBy]);
GO

-- Creating foreign key on [OSegment_CreatedBy] in table 'HLSevenOutboundDisplaySegments'
ALTER TABLE [dbo].[HLSevenOutboundDisplaySegments]
ADD CONSTRAINT [FK_HLSevenOutboundDisplaySegments_User]
    FOREIGN KEY ([OSegment_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HLSevenOutboundDisplaySegments_User'
CREATE INDEX [IX_FK_HLSevenOutboundDisplaySegments_User]
ON [dbo].[HLSevenOutboundDisplaySegments]
    ([OSegment_CreatedBy]);
GO

-- Creating foreign key on [DAdmin_Id] in table 'DrugActivedays'
ALTER TABLE [dbo].[DrugActivedays]
ADD CONSTRAINT [FK_DrugActiveday_DrugAdministrationTime]
    FOREIGN KEY ([DAdmin_Id])
    REFERENCES [dbo].[DrugAdministrationTimes]
        ([DAdmin_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugActiveday_DrugAdministrationTime'
CREATE INDEX [IX_FK_DrugActiveday_DrugAdministrationTime]
ON [dbo].[DrugActivedays]
    ([DAdmin_Id]);
GO

-- Creating foreign key on [ManualDocumentedBy] in table 'DocAdministerTrans'
ALTER TABLE [dbo].[DocAdministerTrans]
ADD CONSTRAINT [FK_DocAdministerTrans_User]
    FOREIGN KEY ([ManualDocumentedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocAdministerTrans_User'
CREATE INDEX [IX_FK_DocAdministerTrans_User]
ON [dbo].[DocAdministerTrans]
    ([ManualDocumentedBy]);
GO

-- Creating foreign key on [OrderFavMaster_ID] in table 'OrderFavourites'
ALTER TABLE [dbo].[OrderFavourites]
ADD CONSTRAINT [FK_OrderFavourite_OrderFavouriteMaster]
    FOREIGN KEY ([OrderFavMaster_ID])
    REFERENCES [dbo].[OrderFavouriteMasters]
        ([OrderFavMaster_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderFavourite_OrderFavouriteMaster'
CREATE INDEX [IX_FK_OrderFavourite_OrderFavouriteMaster]
ON [dbo].[OrderFavourites]
    ([OrderFavMaster_ID]);
GO

-- Creating foreign key on [OrderFavourite_Createby] in table 'OrderFavourites'
ALTER TABLE [dbo].[OrderFavourites]
ADD CONSTRAINT [FK_OrderFavourite_User]
    FOREIGN KEY ([OrderFavourite_Createby])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderFavourite_User'
CREATE INDEX [IX_FK_OrderFavourite_User]
ON [dbo].[OrderFavourites]
    ([OrderFavourite_Createby]);
GO

-- Creating foreign key on [OrderHold_CreatedBy] in table 'OrderHolds'
ALTER TABLE [dbo].[OrderHolds]
ADD CONSTRAINT [FK_OrderHold_User]
    FOREIGN KEY ([OrderHold_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderHold_User'
CREATE INDEX [IX_FK_OrderHold_User]
ON [dbo].[OrderHolds]
    ([OrderHold_CreatedBy]);
GO

-- Creating foreign key on [PDispense_CreatedBy] in table 'TreatmentDispenseInfoes'
ALTER TABLE [dbo].[TreatmentDispenseInfoes]
ADD CONSTRAINT [FK_TreatmentDispenseInfo_User]
    FOREIGN KEY ([PDispense_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentDispenseInfo_User'
CREATE INDEX [IX_FK_TreatmentDispenseInfo_User]
ON [dbo].[TreatmentDispenseInfoes]
    ([PDispense_CreatedBy]);
GO

-- Creating foreign key on [PTOutBoundApprovalBy] in table 'TreatmentDispenseInfoes'
ALTER TABLE [dbo].[TreatmentDispenseInfoes]
ADD CONSTRAINT [FK_TreatmentDispenseInfo_User1]
    FOREIGN KEY ([PTOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentDispenseInfo_User1'
CREATE INDEX [IX_FK_TreatmentDispenseInfo_User1]
ON [dbo].[TreatmentDispenseInfoes]
    ([PTOutBoundApprovalBy]);
GO

-- Creating foreign key on [PRoute_CreatedBy] in table 'TreatmentRouteInfoes'
ALTER TABLE [dbo].[TreatmentRouteInfoes]
ADD CONSTRAINT [FK_TreatmentRouteInfo_User]
    FOREIGN KEY ([PRoute_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentRouteInfo_User'
CREATE INDEX [IX_FK_TreatmentRouteInfo_User]
ON [dbo].[TreatmentRouteInfoes]
    ([PRoute_CreatedBy]);
GO

-- Creating foreign key on [PTROutBoundApprovalBy] in table 'TreatmentRouteInfoes'
ALTER TABLE [dbo].[TreatmentRouteInfoes]
ADD CONSTRAINT [FK_TreatmentRouteInfo_User1]
    FOREIGN KEY ([PTROutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentRouteInfo_User1'
CREATE INDEX [IX_FK_TreatmentRouteInfo_User1]
ON [dbo].[TreatmentRouteInfoes]
    ([PTROutBoundApprovalBy]);
GO

-- Creating foreign key on [Facility_Id] in table 'Ekits'
ALTER TABLE [dbo].[Ekits]
ADD CONSTRAINT [FK_Ekit_Facility1]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ekit_Facility1'
CREATE INDEX [IX_FK_Ekit_Facility1]
ON [dbo].[Ekits]
    ([Facility_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [FK_Stock_Facility1]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stock_Facility1'
CREATE INDEX [IX_FK_Stock_Facility1]
ON [dbo].[Stocks]
    ([Facility_Id]);
GO

-- Creating foreign key on [OIAReason_CreatedBy] in table 'OrderInactiveReasons'
ALTER TABLE [dbo].[OrderInactiveReasons]
ADD CONSTRAINT [FK_OrderInactiveReason_User]
    FOREIGN KEY ([OIAReason_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderInactiveReason_User'
CREATE INDEX [IX_FK_OrderInactiveReason_User]
ON [dbo].[OrderInactiveReasons]
    ([OIAReason_CreatedBy]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'NurseStationHierarchies'
ALTER TABLE [dbo].[NurseStationHierarchies]
ADD CONSTRAINT [FK_NurseStationHierarchy_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseStationHierarchy_NursingStation'
CREATE INDEX [IX_FK_NurseStationHierarchy_NursingStation]
ON [dbo].[NurseStationHierarchies]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [CreatedBy] in table 'NurseStationHierarchies'
ALTER TABLE [dbo].[NurseStationHierarchies]
ADD CONSTRAINT [FK_NurseStationHierarchy_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseStationHierarchy_User'
CREATE INDEX [IX_FK_NurseStationHierarchy_User]
ON [dbo].[NurseStationHierarchies]
    ([CreatedBy]);
GO

-- Creating foreign key on [ApprovedBy] in table 'ControlSubstanceTrans'
ALTER TABLE [dbo].[ControlSubstanceTrans]
ADD CONSTRAINT [FK_TransCSCount_User]
    FOREIGN KEY ([ApprovedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransCSCount_User'
CREATE INDEX [IX_FK_TransCSCount_User]
ON [dbo].[ControlSubstanceTrans]
    ([ApprovedBy]);
GO

-- Creating foreign key on [CertifiedBy] in table 'ControlSubstanceTrans'
ALTER TABLE [dbo].[ControlSubstanceTrans]
ADD CONSTRAINT [FK_TransCSCount_User1]
    FOREIGN KEY ([CertifiedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransCSCount_User1'
CREATE INDEX [IX_FK_TransCSCount_User1]
ON [dbo].[ControlSubstanceTrans]
    ([CertifiedBy]);
GO

-- Creating foreign key on [ControlSubstance_Id] in table 'ControlSubstanceTrans'
ALTER TABLE [dbo].[ControlSubstanceTrans]
ADD CONSTRAINT [FK_TransCSCount_ControlSubstanceCount]
    FOREIGN KEY ([ControlSubstance_Id])
    REFERENCES [dbo].[ControlSubstanceCounts]
        ([ControlSubstance_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransCSCount_ControlSubstanceCount'
CREATE INDEX [IX_FK_TransCSCount_ControlSubstanceCount]
ON [dbo].[ControlSubstanceTrans]
    ([ControlSubstance_Id]);
GO

-- Creating foreign key on [TransOS_CreatedBy] in table 'OrderStockTrans'
ALTER TABLE [dbo].[OrderStockTrans]
ADD CONSTRAINT [FK_TransOS_User]
    FOREIGN KEY ([TransOS_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransOS_User'
CREATE INDEX [IX_FK_TransOS_User]
ON [dbo].[OrderStockTrans]
    ([TransOS_CreatedBy]);
GO

-- Creating foreign key on [OrderStock_Id] in table 'OrderStockTrans'
ALTER TABLE [dbo].[OrderStockTrans]
ADD CONSTRAINT [FK_TransOS_OrderStock]
    FOREIGN KEY ([OrderStock_Id])
    REFERENCES [dbo].[OrderStocks]
        ([OrderStock_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransOS_OrderStock'
CREATE INDEX [IX_FK_TransOS_OrderStock]
ON [dbo].[OrderStockTrans]
    ([OrderStock_Id]);
GO

-- Creating foreign key on [CSReason_CreatedBy] in table 'ControlSubstanceReasons'
ALTER TABLE [dbo].[ControlSubstanceReasons]
ADD CONSTRAINT [FK_ControlSubstanceReason_User]
    FOREIGN KEY ([CSReason_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlSubstanceReason_User'
CREATE INDEX [IX_FK_ControlSubstanceReason_User]
ON [dbo].[ControlSubstanceReasons]
    ([CSReason_CreatedBy]);
GO

-- Creating foreign key on [ControlSubstance_Id] in table 'ControlSubstanceReasons'
ALTER TABLE [dbo].[ControlSubstanceReasons]
ADD CONSTRAINT [FK_ControlSubstanceReason_ControlSubstanceTrans]
    FOREIGN KEY ([ControlSubstance_Id])
    REFERENCES [dbo].[ControlSubstanceTrans]
        ([TransCS_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlSubstanceReason_ControlSubstanceTrans'
CREATE INDEX [IX_FK_ControlSubstanceReason_ControlSubstanceTrans]
ON [dbo].[ControlSubstanceReasons]
    ([ControlSubstance_Id]);
GO

-- Creating foreign key on [PQuantity_CreatedBy] in table 'QuantityDetails'
ALTER TABLE [dbo].[QuantityDetails]
ADD CONSTRAINT [FK_QuantityDetails_User]
    FOREIGN KEY ([PQuantity_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuantityDetails_User'
CREATE INDEX [IX_FK_QuantityDetails_User]
ON [dbo].[QuantityDetails]
    ([PQuantity_CreatedBy]);
GO

-- Creating foreign key on [PQuantity_Id] in table 'NurseComments'
ALTER TABLE [dbo].[NurseComments]
ADD CONSTRAINT [FK_NurseComments_QuantityDetails]
    FOREIGN KEY ([PQuantity_Id])
    REFERENCES [dbo].[QuantityDetails]
        ([PQuantity_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseComments_QuantityDetails'
CREATE INDEX [IX_FK_NurseComments_QuantityDetails]
ON [dbo].[NurseComments]
    ([PQuantity_Id]);
GO

-- Creating foreign key on [PQuantity_Id] in table 'OrderDestroys'
ALTER TABLE [dbo].[OrderDestroys]
ADD CONSTRAINT [FK_OrderDestroy_QuantityDetails]
    FOREIGN KEY ([PQuantity_Id])
    REFERENCES [dbo].[QuantityDetails]
        ([PQuantity_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDestroy_QuantityDetails'
CREATE INDEX [IX_FK_OrderDestroy_QuantityDetails]
ON [dbo].[OrderDestroys]
    ([PQuantity_Id]);
GO

-- Creating foreign key on [PQuantity_Id] in table 'OrderFavourites'
ALTER TABLE [dbo].[OrderFavourites]
ADD CONSTRAINT [FK_OrderFavourite_QuantityDetails]
    FOREIGN KEY ([PQuantity_Id])
    REFERENCES [dbo].[QuantityDetails]
        ([PQuantity_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PQuantity_Id] in table 'OrderHolds'
ALTER TABLE [dbo].[OrderHolds]
ADD CONSTRAINT [FK_OrderHold_QuantityDetails]
    FOREIGN KEY ([PQuantity_Id])
    REFERENCES [dbo].[QuantityDetails]
        ([PQuantity_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderHold_QuantityDetails'
CREATE INDEX [IX_FK_OrderHold_QuantityDetails]
ON [dbo].[OrderHolds]
    ([PQuantity_Id]);
GO

-- Creating foreign key on [MailBox_Id] in table 'MailFavourites'
ALTER TABLE [dbo].[MailFavourites]
ADD CONSTRAINT [FK_MailFavourite_MailBox]
    FOREIGN KEY ([MailBox_Id])
    REFERENCES [dbo].[MailBoxes]
        ([MailBox_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MailFavourite_MailBox'
CREATE INDEX [IX_FK_MailFavourite_MailBox]
ON [dbo].[MailFavourites]
    ([MailBox_Id]);
GO

-- Creating foreign key on [MailBox_Id] in table 'MailReads'
ALTER TABLE [dbo].[MailReads]
ADD CONSTRAINT [FK_MailRead_MailBox]
    FOREIGN KEY ([MailBox_Id])
    REFERENCES [dbo].[MailBoxes]
        ([MailBox_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MailRead_MailBox'
CREATE INDEX [IX_FK_MailRead_MailBox]
ON [dbo].[MailReads]
    ([MailBox_Id]);
GO

-- Creating foreign key on [Facility_Id] in table 'tblRDCMailConfigs'
ALTER TABLE [dbo].[tblRDCMailConfigs]
ADD CONSTRAINT [FK_tblRDCMailConfig_Facility]
    FOREIGN KEY ([Facility_Id])
    REFERENCES [dbo].[Facilities]
        ([Facility_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblRDCMailConfig_Facility'
CREATE INDEX [IX_FK_tblRDCMailConfig_Facility]
ON [dbo].[tblRDCMailConfigs]
    ([Facility_Id]);
GO

-- Creating foreign key on [NurseStation_Id] in table 'tblRDCMailConfigs'
ALTER TABLE [dbo].[tblRDCMailConfigs]
ADD CONSTRAINT [FK_tblRDCMailConfig_NursingStation]
    FOREIGN KEY ([NurseStation_Id])
    REFERENCES [dbo].[NursingStations]
        ([NurseStation_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblRDCMailConfig_NursingStation'
CREATE INDEX [IX_FK_tblRDCMailConfig_NursingStation]
ON [dbo].[tblRDCMailConfigs]
    ([NurseStation_Id]);
GO

-- Creating foreign key on [Rdc_Id] in table 'tblRdcMailTimes'
ALTER TABLE [dbo].[tblRdcMailTimes]
ADD CONSTRAINT [FK_tblRdcMailTime_tblRDCMailConfig]
    FOREIGN KEY ([Rdc_Id])
    REFERENCES [dbo].[tblRDCMailConfigs]
        ([Rdc_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblRdcMailTime_tblRDCMailConfig'
CREATE INDEX [IX_FK_tblRdcMailTime_tblRDCMailConfig]
ON [dbo].[tblRdcMailTimes]
    ([Rdc_Id]);
GO

-- Creating foreign key on [Ekit_Id] in table 'EKitControlSubstanceTrans'
ALTER TABLE [dbo].[EKitControlSubstanceTrans]
ADD CONSTRAINT [FK_EkTransCSCount_ControlSubstanceCount]
    FOREIGN KEY ([Ekit_Id])
    REFERENCES [dbo].[Ekits]
        ([Ekit_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkTransCSCount_ControlSubstanceCount'
CREATE INDEX [IX_FK_EkTransCSCount_ControlSubstanceCount]
ON [dbo].[EKitControlSubstanceTrans]
    ([Ekit_Id]);
GO

-- Creating foreign key on [EkCSReason_CreatedBy] in table 'EkitControlSubstanceReasons'
ALTER TABLE [dbo].[EkitControlSubstanceReasons]
ADD CONSTRAINT [FK_EkControlSubstanceReason_User]
    FOREIGN KEY ([EkCSReason_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkControlSubstanceReason_User'
CREATE INDEX [IX_FK_EkControlSubstanceReason_User]
ON [dbo].[EkitControlSubstanceReasons]
    ([EkCSReason_CreatedBy]);
GO

-- Creating foreign key on [ApprovedBy] in table 'EKitControlSubstanceTrans'
ALTER TABLE [dbo].[EKitControlSubstanceTrans]
ADD CONSTRAINT [FK_EkTransCSCount_User]
    FOREIGN KEY ([ApprovedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkTransCSCount_User'
CREATE INDEX [IX_FK_EkTransCSCount_User]
ON [dbo].[EKitControlSubstanceTrans]
    ([ApprovedBy]);
GO

-- Creating foreign key on [CertifiedBy] in table 'EKitControlSubstanceTrans'
ALTER TABLE [dbo].[EKitControlSubstanceTrans]
ADD CONSTRAINT [FK_EkTransCSCount_User1]
    FOREIGN KEY ([CertifiedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkTransCSCount_User1'
CREATE INDEX [IX_FK_EkTransCSCount_User1]
ON [dbo].[EKitControlSubstanceTrans]
    ([CertifiedBy]);
GO

-- Creating foreign key on [MedicationReason_ID] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_MedicationReason]
    FOREIGN KEY ([MedicationReason_ID])
    REFERENCES [dbo].[MedicationReasons]
        ([MedicationReason_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_MedicationReason'
CREATE INDEX [IX_FK_DrugAdminister_MedicationReason]
ON [dbo].[DrugAdministers]
    ([MedicationReason_ID]);
GO

-- Creating foreign key on [NurseShifts_Id] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_NurseShifts]
    FOREIGN KEY ([NurseShifts_Id])
    REFERENCES [dbo].[NurseShifts]
        ([NurseShifts_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_NurseShifts'
CREATE INDEX [IX_FK_DrugAdminister_NurseShifts]
ON [dbo].[DrugAdministers]
    ([NurseShifts_Id]);
GO

-- Creating foreign key on [AdminsterBy] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_User]
    FOREIGN KEY ([AdminsterBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_User'
CREATE INDEX [IX_FK_DrugAdminister_User]
ON [dbo].[DrugAdministers]
    ([AdminsterBy]);
GO

-- Creating foreign key on [PRNCommentBy] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_User1]
    FOREIGN KEY ([PRNCommentBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_User1'
CREATE INDEX [IX_FK_DrugAdminister_User1]
ON [dbo].[DrugAdministers]
    ([PRNCommentBy]);
GO

-- Creating foreign key on [SeventyTwoCommentBy] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_User2]
    FOREIGN KEY ([SeventyTwoCommentBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_User2'
CREATE INDEX [IX_FK_DrugAdminister_User2]
ON [dbo].[DrugAdministers]
    ([SeventyTwoCommentBy]);
GO

-- Creating foreign key on [ManualDocumentedBy] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_User3]
    FOREIGN KEY ([ManualDocumentedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_User3'
CREATE INDEX [IX_FK_DrugAdminister_User3]
ON [dbo].[DrugAdministers]
    ([ManualDocumentedBy]);
GO

-- Creating foreign key on [DrugAdminister_Id] in table 'DocAdministerTrans'
ALTER TABLE [dbo].[DocAdministerTrans]
ADD CONSTRAINT [FK_DocAdministerTrans_DrugAdminister]
    FOREIGN KEY ([DrugAdminister_Id])
    REFERENCES [dbo].[DrugAdministers]
        ([DrugAdminister_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocAdministerTrans_DrugAdminister'
CREATE INDEX [IX_FK_DocAdministerTrans_DrugAdminister]
ON [dbo].[DocAdministerTrans]
    ([DrugAdminister_Id]);
GO

-- Creating foreign key on [DrugAdminister_Id] in table 'EkitAdministers'
ALTER TABLE [dbo].[EkitAdministers]
ADD CONSTRAINT [FK_EkitAdminister_DrugAdminister]
    FOREIGN KEY ([DrugAdminister_Id])
    REFERENCES [dbo].[DrugAdministers]
        ([DrugAdminister_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkitAdminister_DrugAdminister'
CREATE INDEX [IX_FK_EkitAdminister_DrugAdminister]
ON [dbo].[EkitAdministers]
    ([DrugAdminister_Id]);
GO

-- Creating foreign key on [DrugAdminister_Id] in table 'NurseComments'
ALTER TABLE [dbo].[NurseComments]
ADD CONSTRAINT [FK_NurseComments_DrugAdminister]
    FOREIGN KEY ([DrugAdminister_Id])
    REFERENCES [dbo].[DrugAdministers]
        ([DrugAdminister_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NurseComments_DrugAdminister'
CREATE INDEX [IX_FK_NurseComments_DrugAdminister]
ON [dbo].[NurseComments]
    ([DrugAdminister_Id]);
GO

-- Creating foreign key on [POrder_CreatedBy] in table 'CommonOrderInfoes'
ALTER TABLE [dbo].[CommonOrderInfoes]
ADD CONSTRAINT [FK_CommonOrderInfo_User]
    FOREIGN KEY ([POrder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommonOrderInfo_User'
CREATE INDEX [IX_FK_CommonOrderInfo_User]
ON [dbo].[CommonOrderInfoes]
    ([POrder_CreatedBy]);
GO

-- Creating foreign key on [POOutBoundApprovalBy] in table 'CommonOrderInfoes'
ALTER TABLE [dbo].[CommonOrderInfoes]
ADD CONSTRAINT [FK_CommonOrderInfo_User1]
    FOREIGN KEY ([POOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommonOrderInfo_User1'
CREATE INDEX [IX_FK_CommonOrderInfo_User1]
ON [dbo].[CommonOrderInfoes]
    ([POOutBoundApprovalBy]);
GO

-- Creating foreign key on [POrder_Id] in table 'AddlInstructionDetails'
ALTER TABLE [dbo].[AddlInstructionDetails]
ADD CONSTRAINT [FK_AddlInstructionDetails_CommonOrder]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddlInstructionDetails_CommonOrder'
CREATE INDEX [IX_FK_AddlInstructionDetails_CommonOrder]
ON [dbo].[AddlInstructionDetails]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'AncillaryDetails'
ALTER TABLE [dbo].[AncillaryDetails]
ADD CONSTRAINT [FK_AncillaryDetails_CommonOrder]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AncillaryDetails_CommonOrder'
CREATE INDEX [IX_FK_AncillaryDetails_CommonOrder]
ON [dbo].[AncillaryDetails]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'BarcodeDetails'
ALTER TABLE [dbo].[BarcodeDetails]
ADD CONSTRAINT [FK_BarcodeDetails_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BarcodeDetails_CommonOrderInfo'
CREATE INDEX [IX_FK_BarcodeDetails_CommonOrderInfo]
ON [dbo].[BarcodeDetails]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'CompoundOrders'
ALTER TABLE [dbo].[CompoundOrders]
ADD CONSTRAINT [FK_CompoundOrder_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompoundOrder_CommonOrderInfo'
CREATE INDEX [IX_FK_CompoundOrder_CommonOrderInfo]
ON [dbo].[CompoundOrders]
    ([POrder_Id]);
GO

-- Creating foreign key on [Porder_Id] in table 'DrugAdministers'
ALTER TABLE [dbo].[DrugAdministers]
ADD CONSTRAINT [FK_DrugAdminister_CommonOrderInfo]
    FOREIGN KEY ([Porder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrugAdminister_CommonOrderInfo'
CREATE INDEX [IX_FK_DrugAdminister_CommonOrderInfo]
ON [dbo].[DrugAdministers]
    ([Porder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'EncodedOrderDetails'
ALTER TABLE [dbo].[EncodedOrderDetails]
ADD CONSTRAINT [FK_EncodedOrderDetails_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EncodedOrderDetails_CommonOrderInfo'
CREATE INDEX [IX_FK_EncodedOrderDetails_CommonOrderInfo]
ON [dbo].[EncodedOrderDetails]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'NotesInfoes'
ALTER TABLE [dbo].[NotesInfoes]
ADD CONSTRAINT [FK_NotesInfo_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotesInfo_CommonOrderInfo'
CREATE INDEX [IX_FK_NotesInfo_CommonOrderInfo]
ON [dbo].[NotesInfoes]
    ([POrder_Id]);
GO

-- Creating foreign key on [Porder_Id] in table 'OutBoundFileInformations'
ALTER TABLE [dbo].[OutBoundFileInformations]
ADD CONSTRAINT [FK_OutBoundFileInformation_CommonOrderInfo]
    FOREIGN KEY ([Porder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutBoundFileInformation_CommonOrderInfo'
CREATE INDEX [IX_FK_OutBoundFileInformation_CommonOrderInfo]
ON [dbo].[OutBoundFileInformations]
    ([Porder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'QuantityDetails'
ALTER TABLE [dbo].[QuantityDetails]
ADD CONSTRAINT [FK_QuantityDetails_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuantityDetails_CommonOrderInfo'
CREATE INDEX [IX_FK_QuantityDetails_CommonOrderInfo]
ON [dbo].[QuantityDetails]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'TreatmentDispenseInfoes'
ALTER TABLE [dbo].[TreatmentDispenseInfoes]
ADD CONSTRAINT [FK_TreatmentDispenseInfo_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentDispenseInfo_CommonOrderInfo'
CREATE INDEX [IX_FK_TreatmentDispenseInfo_CommonOrderInfo]
ON [dbo].[TreatmentDispenseInfoes]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'TreatmentInfoes'
ALTER TABLE [dbo].[TreatmentInfoes]
ADD CONSTRAINT [FK_TreatmentInfo_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentInfo_CommonOrderInfo'
CREATE INDEX [IX_FK_TreatmentInfo_CommonOrderInfo]
ON [dbo].[TreatmentInfoes]
    ([POrder_Id]);
GO

-- Creating foreign key on [POrder_Id] in table 'TreatmentRouteInfoes'
ALTER TABLE [dbo].[TreatmentRouteInfoes]
ADD CONSTRAINT [FK_TreatmentRouteInfo_CommonOrderInfo]
    FOREIGN KEY ([POrder_Id])
    REFERENCES [dbo].[CommonOrderInfoes]
        ([POrder_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TreatmentRouteInfo_CommonOrderInfo'
CREATE INDEX [IX_FK_TreatmentRouteInfo_CommonOrderInfo]
ON [dbo].[TreatmentRouteInfoes]
    ([POrder_Id]);
GO

-- Creating foreign key on [WeightLog_CreatedBy] in table 'WeightLogs'
ALTER TABLE [dbo].[WeightLogs]
ADD CONSTRAINT [FK_WeightLog_User]
    FOREIGN KEY ([WeightLog_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WeightLog_User'
CREATE INDEX [IX_FK_WeightLog_User]
ON [dbo].[WeightLogs]
    ([WeightLog_CreatedBy]);
GO

-- Creating foreign key on [POOutBoundApprovalBy] in table 'ApprovalOrders'
ALTER TABLE [dbo].[ApprovalOrders]
ADD CONSTRAINT [FK_ApprovalOrders_User]
    FOREIGN KEY ([POOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalOrders_User'
CREATE INDEX [IX_FK_ApprovalOrders_User]
ON [dbo].[ApprovalOrders]
    ([POOutBoundApprovalBy]);
GO

-- Creating foreign key on [POrder_CreatedBy] in table 'ApprovalOrders'
ALTER TABLE [dbo].[ApprovalOrders]
ADD CONSTRAINT [FK_ApprovalOrders_User1]
    FOREIGN KEY ([POrder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalOrders_User1'
CREATE INDEX [IX_FK_ApprovalOrders_User1]
ON [dbo].[ApprovalOrders]
    ([POrder_CreatedBy]);
GO

-- Creating foreign key on [Patient_Id] in table 'AlertTexts'
ALTER TABLE [dbo].[AlertTexts]
ADD CONSTRAINT [FK_AlertText_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlertText_Demographics'
CREATE INDEX [IX_FK_AlertText_Demographics]
ON [dbo].[AlertTexts]
    ([Patient_Id]);
GO

-- Creating foreign key on [PatientType_Id] in table 'Demographics'
ALTER TABLE [dbo].[Demographics]
ADD CONSTRAINT [FK_Demographics_PatientType]
    FOREIGN KEY ([PatientType_Id])
    REFERENCES [dbo].[PatientTypes]
        ([PatientType_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Demographics_PatientType'
CREATE INDEX [IX_FK_Demographics_PatientType]
ON [dbo].[Demographics]
    ([PatientType_Id]);
GO

-- Creating foreign key on [Patient_CreatedBy] in table 'Demographics'
ALTER TABLE [dbo].[Demographics]
ADD CONSTRAINT [FK_Demographics_User]
    FOREIGN KEY ([Patient_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Demographics_User'
CREATE INDEX [IX_FK_Demographics_User]
ON [dbo].[Demographics]
    ([Patient_CreatedBy]);
GO

-- Creating foreign key on [PDOutBoundApprovalBy] in table 'Demographics'
ALTER TABLE [dbo].[Demographics]
ADD CONSTRAINT [FK_Demographics_User1]
    FOREIGN KEY ([PDOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Demographics_User1'
CREATE INDEX [IX_FK_Demographics_User1]
ON [dbo].[Demographics]
    ([PDOutBoundApprovalBy]);
GO

-- Creating foreign key on [Patient_Id] in table 'AllergyInfoes'
ALTER TABLE [dbo].[AllergyInfoes]
ADD CONSTRAINT [FK_AllergyInfo_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AllergyInfo_Demographics'
CREATE INDEX [IX_FK_AllergyInfo_Demographics]
ON [dbo].[AllergyInfoes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'ApprovalDemographics'
ALTER TABLE [dbo].[ApprovalDemographics]
ADD CONSTRAINT [FK_ApprovalDemographics_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalDemographics_Demographics'
CREATE INDEX [IX_FK_ApprovalDemographics_Demographics]
ON [dbo].[ApprovalDemographics]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'ColourTypes'
ALTER TABLE [dbo].[ColourTypes]
ADD CONSTRAINT [FK_ColourType_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ColourType_Demographics'
CREATE INDEX [IX_FK_ColourType_Demographics]
ON [dbo].[ColourTypes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'CommonOrderInfoes'
ALTER TABLE [dbo].[CommonOrderInfoes]
ADD CONSTRAINT [FK_CommonOrderInfo_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommonOrderInfo_Demographics'
CREATE INDEX [IX_FK_CommonOrderInfo_Demographics]
ON [dbo].[CommonOrderInfoes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'DiagnosisInfoes'
ALTER TABLE [dbo].[DiagnosisInfoes]
ADD CONSTRAINT [FK_DiagnosisInfo_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiagnosisInfo_Demographics'
CREATE INDEX [IX_FK_DiagnosisInfo_Demographics]
ON [dbo].[DiagnosisInfoes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'EkitAdministers'
ALTER TABLE [dbo].[EkitAdministers]
ADD CONSTRAINT [FK_EkitAdminister_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EkitAdminister_Demographics'
CREATE INDEX [IX_FK_EkitAdminister_Demographics]
ON [dbo].[EkitAdministers]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'InsuranceInfoes'
ALTER TABLE [dbo].[InsuranceInfoes]
ADD CONSTRAINT [FK_InsuranceInfo_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InsuranceInfo_Demographics'
CREATE INDEX [IX_FK_InsuranceInfo_Demographics]
ON [dbo].[InsuranceInfoes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'Observations'
ALTER TABLE [dbo].[Observations]
ADD CONSTRAINT [FK_Observation_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Observation_Demographics'
CREATE INDEX [IX_FK_Observation_Demographics]
ON [dbo].[Observations]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'OutBoundFileInformations'
ALTER TABLE [dbo].[OutBoundFileInformations]
ADD CONSTRAINT [FK_OutBoundFileInformation_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutBoundFileInformation_Demographics'
CREATE INDEX [IX_FK_OutBoundFileInformation_Demographics]
ON [dbo].[OutBoundFileInformations]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_ID] in table 'ResidentOrders'
ALTER TABLE [dbo].[ResidentOrders]
ADD CONSTRAINT [FK_ResidentOrder_Demographics]
    FOREIGN KEY ([Patient_ID])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResidentOrder_Demographics'
CREATE INDEX [IX_FK_ResidentOrder_Demographics]
ON [dbo].[ResidentOrders]
    ([Patient_ID]);
GO

-- Creating foreign key on [Patient_Id] in table 'VisitInfoes'
ALTER TABLE [dbo].[VisitInfoes]
ADD CONSTRAINT [FK_VisitInfo_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitInfo_Demographics'
CREATE INDEX [IX_FK_VisitInfo_Demographics]
ON [dbo].[VisitInfoes]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'WeightLogs'
ALTER TABLE [dbo].[WeightLogs]
ADD CONSTRAINT [FK_WeightLog_Demographics]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Demographics]
        ([Patient_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WeightLog_Demographics'
CREATE INDEX [IX_FK_WeightLog_Demographics]
ON [dbo].[WeightLogs]
    ([Patient_Id]);
GO

-- Creating foreign key on [POOutBoundApprovalBy] in table 'ApprovalOrder1'
ALTER TABLE [dbo].[ApprovalOrder1]
ADD CONSTRAINT [FK_ApprovalOrders_User2]
    FOREIGN KEY ([POOutBoundApprovalBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalOrders_User2'
CREATE INDEX [IX_FK_ApprovalOrders_User2]
ON [dbo].[ApprovalOrder1]
    ([POOutBoundApprovalBy]);
GO

-- Creating foreign key on [POrder_CreatedBy] in table 'ApprovalOrder1'
ALTER TABLE [dbo].[ApprovalOrder1]
ADD CONSTRAINT [FK_ApprovalOrders_User11]
    FOREIGN KEY ([POrder_CreatedBy])
    REFERENCES [dbo].[Users]
        ([User_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApprovalOrders_User11'
CREATE INDEX [IX_FK_ApprovalOrders_User11]
ON [dbo].[ApprovalOrder1]
    ([POrder_CreatedBy]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------