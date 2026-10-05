import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({
  name: 'gridFilter'
})
export class GridFilterPipe implements PipeTransform {

  transform(data: any, searchText?: any, screenName?: any): any {
    // if(searchText == null) return data;

    // return data.filter(function(category){
    //   return category.File_Name.toLowerCase().indexOf(searchText.toLowerCase()) > -1;
    // })
    searchText=searchText.toLowerCase();
    if (!searchText) {
      return data;
    }
    if (screenName == "Inbound") {
      return data.filter((val) => {
        let result = (val.File_Name!=null?val.File_Name.toLowerCase().includes(searchText):"")
          || (val.Event!=null?val.Event.toLowerCase().includes(searchText):"")
          ||(val.File_ErrorDesc!=null?val.File_ErrorDesc.toLowerCase().includes(searchText):"")
          || (val.Event!=null?val.Event.toLowerCase().includes(searchText):"")
          || (val.File_CreatedDate!=null?  new DatePipe('en-US').transform(val.File_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Outbound") {
      return data.filter((val) => {
        let result = (val.File_Name!=null?val.File_Name.toLowerCase().includes(searchText):"")
        || (val.Event!=null?val.Event.toLowerCase().includes(searchText):"")
        ||(val.File_ErrorDesc!=null?val.File_ErrorDesc.toLowerCase().includes(searchText):"")
        || (val.Category!=null?val.Category.toLowerCase().includes(searchText):"")
        || (val.File_CreatedDate!=null?  new DatePipe('en-US').transform(val.File_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Orders") {
      return data.filter((val) => {
        let result = (val.ResidentName!=null?val.ResidentName.toLowerCase().includes(searchText):"")
        || (val.Drug!=null?val.Drug.toLowerCase().includes(searchText):"")
        || (val.Dosage_Form!=null?val.Dosage_Form.toLowerCase().includes(searchText):"")
        || (val.Quantity!=null?val.Quantity.toString().includes(searchText):"")
        ||(val.Physician_Name!=null?val.Physician_Name.toLowerCase().includes(searchText):"")
        ||(val.Directions!=null?val.Directions.toLowerCase().includes(searchText):"")
        ||(val.POrder_Status!=null?val.POrder_Status.toLowerCase().includes(searchText):"")
        ||(val.DOB!=null?val.DOB.toLowerCase().includes(searchText):"")
        ||(val.Gender!=null?val.Gender.toLowerCase().includes(searchText):"")
        || (val.Date!=null?  new DatePipe('en-US').transform(val.Date,'MM/dd/yyyy').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "PendingOrders") {
      return data.filter((val) => {
        let result = (val.ResidentName!=null?val.ResidentName.toLowerCase().includes(searchText):"")
        || (val.Drug!=null?val.Drug.toLowerCase().includes(searchText):"")
        || (val.Dosage_Form!=null?val.Dosage_Form.toLowerCase().includes(searchText):"")
        || (val.Quantity!=null?val.Quantity.toString().includes(searchText):"")
        ||(val.Physician_Name!=null?val.Physician_Name.toLowerCase().includes(searchText):"")
        ||(val.Directions!=null?val.Directions.toLowerCase().includes(searchText):"")
        ||(val.POrder_Status!=null?val.POrder_Status.toLowerCase().includes(searchText):"")
        || (val.Date!=null?  new DatePipe('en-US').transform(val.Date,'MM/dd/yyyy').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Company") {
      return data.filter((val) => {
        let result = (val.Company_Name!=null?val.Company_Name.toLowerCase().includes(searchText):"")
        || (val.Company_UniqueId!=null? val.Company_UniqueId.toLowerCase().includes(searchText):"")
          || (val.Company_State!=null? val.Company_State.toLowerCase().includes(searchText):"")
          || (val.Company_City!=null? val.Company_City.toLowerCase().includes(searchText):"")
          || (val.Com_ContactPerson!=null? val.Com_ContactPerson.toLowerCase().includes(searchText):"")
          || (val.Com_ContactPhone!=null? val.Com_ContactPhone.toLowerCase().includes(searchText):"")
          || (val.UserName!=null? val.UserName.toLowerCase().includes(searchText):"")
          || (val.Company_CreatedDate!=null?  new DatePipe('en-US').transform(val.Company_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }


    if (screenName == "Facility") {
      return data.filter((val) => {
        let result = (val.Facility_Name!=null?val.Facility_Name.toLowerCase().includes(searchText):"")
        || (val.Facility_Addr1!=null?val.Facility_Addr1.toLowerCase().includes(searchText):"")
        || (val.Facility_ShortName!=null?val.Facility_ShortName.toLowerCase().includes(searchText):"")
          || (val.Company_Name!=null?val.Company_Name.toLowerCase().includes(searchText):"")
          || (val.Facility_City!=null?val.Facility_City.toLowerCase().includes(searchText):"")
          || (val.Facility_State!=null?val.Facility_State.toLowerCase().includes(searchText):"")
          || (val.Facility_Zip!=null?val.Facility_Zip.toLowerCase().includes(searchText):"")
          || (val.Facility_ContactPhone!=null? val.Facility_ContactPhone.toLowerCase().includes(searchText):"")
          || (val.Facility_ContactName!=null?val.Facility_ContactName.toLowerCase().includes(searchText):"")
          || (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
          || (val.Facility_CreatedDate!=null? new DatePipe('en-US').transform(val.Facility_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "NurseStation") {
      return data.filter((val) => {
        let result = (val.NurseStation_Code!=null?val.NurseStation_Code.toLowerCase().includes(searchText):"")
        || (val.FacilityName!=null?val.FacilityName.toLowerCase().includes(searchText):"")
        || (val.PhysicianName!=null?val.PhysicianName.toLowerCase().includes(searchText):"")
        || (val.NurseStation_Name!=null?val.NurseStation_Name.toLowerCase().includes(searchText):"")
        || (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.NurseStationShifts!=null?val.NurseStationShifts.toLowerCase().includes(searchText):"")
        || (val.NurseStation_CreatedDate!=null? new DatePipe('en-US').transform(val.NurseStation_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Floor") {
      return data.filter((val) => {
        let result = (val.Floor_Name!=null?val.Floor_Name.toLowerCase().includes(searchText):"")
        || (val.User.User_DisplayName!=null?val.User.User_DisplayName.toLowerCase().includes(searchText):"")
        || (val.Floor_CreatedDate!=null? new DatePipe('en-US').transform(val.Floor_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Wing") {
      return data.filter((val) => {
        let result = (val.Wing_Desc!=null?val.Wing_Desc.toLowerCase().includes(searchText):"")
        || (val.User.User_DisplayName!=null?val.User.User_DisplayName.toLowerCase().includes(searchText):"")
        || (val.Wing_CreatedDate!=null? new DatePipe('en-US').transform(val.Wing_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

        return result;
      });
    }
    if (screenName == "EkitMedsCheckIn") {
      return data.filter((val) => {
        let result = (val.DrugName!=null?val.DrugName.toLowerCase().includes(searchText):"")
        || (val.InHand!=null?val.InHand.toLowerCase().includes(searchText):"")
        || (val.LotNumber!=null?val.LotNumber.toLowerCase().includes(searchText):"")
        || (val.ExpiryDate!=null? new DatePipe('en-US').transform(val.ExpiryDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
        || (val.Drugname!=null?val.Drugname.toLowerCase().includes(searchText):"")
        || (val.Inhand!=null?val.Inhand.toLowerCase().includes(searchText):"")
        || (val.BarcodeDetail!=null?val.BarcodeDetail.toLowerCase().includes(searchText):"")
        || (val.inputValue!=null?val.inputValue.toLowerCase().includes(searchText):"")
        || (val.reason!=null?val.reason.toLowerCase().includes(searchText):"")

        
        return result;
      });
    }
    if (screenName == "orderFav") {
      return data.filter((val) => {
        let result = (val.Facility_Name!=null?val.Facility_Name.toLowerCase().includes(searchText):"")
        || (val.OrderFavCode!=null?val.OrderFavCode.toLowerCase().includes(searchText):"")
        || (val.OrderFavDesc!=null?val.OrderFavDesc.toLowerCase().includes(searchText):"")

        return result;
      });
    }
    if (screenName == "Room") {
      return data.filter((val) => {
        let result = (val.Room_Name!=null?val.Room_Name.toLowerCase().includes(searchText):"")
        ||(val.Room_Code!=null?val.Room_Code.toLowerCase().includes(searchText):"")
        || (val.User.User_DisplayName!=null?val.User.User_DisplayName.toLowerCase().includes(searchText):"")
        || (val.Room_CreatedDate!=null? new DatePipe('en-US').transform(val.Room_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

        return result;
      });
    }

    if (screenName == "Bed") {
      return data.filter((val) => {
        let result = (val.Bed_Name!=null?val.Bed_Name.toLowerCase().includes(searchText):"")
        ||(val.Bed_Code!=null?val.Bed_Code.toLowerCase().includes(searchText):"")
        || (val.User.User_DisplayName!=null?val.User.User_DisplayName.toLowerCase().includes(searchText):"")
        || (val.Bed_CreatedDate!=null? new DatePipe('en-US').transform(val.Bed_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Users") {
      return data.filter((val) => {
        let result = (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        ||(val.DisplayName!=null?val.DisplayName.toLowerCase().includes(searchText):"")
        ||(val.RoleName!=null?val.RoleName.toLowerCase().includes(searchText):"")
        ||(val.Company_Facility_Nursestation!=null?val.Company_Facility_Nursestation.toLowerCase().includes(searchText):"")
        || (val.EmailId!=null?val.EmailId.toString().includes(searchText):"")
        || (val.PhoneNumber!=null?val.PhoneNumber.toString().includes(searchText):"")
        || (val.User_CreatedDate!=null? new DatePipe('en-US').transform(val.User_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }

    if (screenName == "RoleMaster") {
      return data.filter((val) => {
        let result = (val.Role_Description!=null?val.Role_Description.toLowerCase().includes(searchText):"")
        || (val.Superior_Role!=null?val.Superior_Role.toLowerCase().includes(searchText):"")
        || (val.IsAdmin!=null?val.IsAdmin.toLowerCase().includes(searchText):"")
        || (val.Screen_Desc!=null?val.Screen_Desc.toLowerCase().includes(searchText):"");
        return result;
      });
    }
    if (screenName == "RoleConfig") {
      return data.filter((val) => {
        let result = (val.Screen!=null?val.Screen.toLowerCase().includes(searchText):"")
        || (val.Role_Name!=null?val.Role_Name.toLowerCase().includes(searchText):"")
        || (val.Read!=null?val.Read.toLowerCase().includes(searchText):"")
        || (val.Write!=null?val.Write.toLowerCase().includes(searchText):"")
        || (val.PDF!=null?val.PDF.toLowerCase().includes(searchText):"")
        || (val.Excel!=null?val.Excel.toLowerCase().includes(searchText):"");
        return result;
      });
    }
    if (screenName == "UserFacilityRoleConfig") {
      return data.filter((val) => {
        let result = (val.DisplayName!=null?val.DisplayName.toLowerCase().includes(searchText):"")
        ||(val.RoleName!=null?val.RoleName.toLowerCase().includes(searchText):"")
        ||(val.CompanyName!=null?val.CompanyName.toLowerCase().includes(searchText):"")
        ||(val.FacilityName!=null?val.FacilityName.toLowerCase().includes(searchText):"")
        ||(val.NurseStationNames!=null?val.NurseStationNames.toLowerCase().includes(searchText):"")
        ||(val.User_DisplayName!=null?val.User_DisplayName.toLowerCase().includes(searchText):"")
        || (val.UserRole_CreatedDate!=null? new DatePipe('en-US').transform(val.UserRole_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "HLSegmentFieldConfig") {
      return data.filter((val) => {
        let result = (val.SegDetail_Desc.toLowerCase().includes(searchText));

        return result;
      });
    }
    if (screenName == "FTPConfig") {
      return data.filter((val) => {
        let result = (val.Port!=null?val.Port.toLowerCase().includes(searchText):"")
        ||(val.Company.Company_Name!=null?val.Company.Company_Name.toLowerCase().includes(searchText):"")
        ||(val.FTECategory.FteCategory_Desc!=null?val.FTECategory.FteCategory_Desc.toLowerCase().includes(searchText):"")
        ||(val.FteConnection.FteConn_Desc!=null?val.FteConnection.FteConn_Desc.toLowerCase().includes(searchText):"")
        ||(val.ServerIp!=null?val.ServerIp.toLowerCase().includes(searchText):"")
        ||(val.Port!=null?val.Port.toString().includes(searchText):"")
        || (val.User.UserName!=null?val.User.UserName.toLowerCase().includes(searchText):"")
        || (val.FteConfig_CreatedDate!=null? new DatePipe('en-US').transform(val.FteConfig_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "CompanyBedConf") {
      return data.filter((val) => {
        let result = (val.Company_Name!=null?val.Company_Name.toLowerCase().includes(searchText):"")
        ||(val.Facility_Name!=null?val.Facility_Name.toLowerCase().includes(searchText):"")
        ||(val.NurseStation_Name!=null?val.NurseStation_Name.toLowerCase().includes(searchText):"")
        ||(val.Floor_Name!=null?val.Floor_Name.toLowerCase().includes(searchText):"")
        ||(val.Wing_Desc!=null?val.Wing_Desc.toLowerCase().includes(searchText):"")
        ||(val.Room_Name!=null?val.Room_Name.toLowerCase().includes(searchText):"")
        ||(val.Bed_Name!=null?val.Bed_Name.toLowerCase().includes(searchText):"")
        || (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.BedConfig_CreatedDate!=null? new DatePipe('en-US').transform(val.BedConfig_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "FrequencyMaping") {
      return data.filter((val) => {
        let result = (val.Frequency!=null?val.Frequency.toLowerCase().includes(searchText):"")
        ||(val.NurseStationName!=null?val.NurseStationName.toLowerCase().includes(searchText):"")
        ||(val.FacilityName!=null?val.FacilityName.toLowerCase().includes(searchText):"")
        ||(val.Start_Time!=null?val.Start_Time.toLowerCase().includes(searchText):"")
        || (val.Hours!=null?val.Hours.toString().toLowerCase().includes(searchText):"")
        return result;
      });
    }
    if (screenName == "ResidentGrid") {
      return data.filter((val) => {
        let result = (val.PatientLastName!=null?val.PatientLastName.toLowerCase().includes(searchText):"")
        ||(val.PatientFirstName.toLowerCase().includes(searchText))
        ||(val.PatientMiddleInitial!=null?val.PatientMiddleInitial.toLowerCase().includes(searchText):"")
        ||(val.PatientAddress1!=null?val.PatientAddress1.toLowerCase().includes(searchText):"")
        ||(val.PatientCity!=null?val.PatientCity.toLowerCase().includes(searchText):"")
        ||(val.PatientState!=null?val.PatientState.toLowerCase().includes(searchText):"")
        ||(val.PatientZipCode!=null?val.PatientZipCode.toString().includes(searchText):"")
        ||(val.PhoneHome!=null?val.PhoneHome.toString().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toString().includes(searchText):"")

        return result;
      });
    }
    if (screenName == "ResidentMedication") {
      return data.filter((val) => {
        let result = (val.RequestedGiveCode!=null?val.RequestedGiveCode.toLowerCase().includes(searchText):"")
        ||(val.NumberOfRefills!=null?val.NumberOfRefills.toString().includes(searchText):"")
        ||(val.RequestedDosageForm!=null?val.RequestedDosageForm.toLowerCase().includes(searchText):"")
        ||(val.ProvidersTreatmentInstructions!=null?val.ProvidersTreatmentInstructions.toLowerCase().includes(searchText):"")
        ||(val.OrderingProviderDEANumber!=null?val.OrderingProviderDEANumber.toLowerCase().includes(searchText):"")
        || (val.PTreatment_CreatedDate!=null? new DatePipe('en-US').transform(val.PTreatment_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "ResidentAllergies") {
      return data.filter((val) => {
        let result = (val.ClassDrug_Name!=null?val.ClassDrug_Name.toLowerCase().includes(searchText):"")
        ||(val.AllergyReactionCode!=null?val.AllergyReactionCode.toLowerCase().includes(searchText):"")
        || (val.PAllergy_CreatedDate!=null? new DatePipe('en-US').transform(val.PAllergy_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

        return result;
      });
    }
    if (screenName == "LiteralOrders") {
      return data.filter((val) => {
        let result = (val.FillerType!=null?val.FillerType.toLowerCase().includes(searchText):"")
        || (val.OrderControl!=null?val.OrderControl.toLowerCase().includes(searchText):"")
        ||(val.PatientId!=null?val.PatientId.toString().includes(searchText):"")
        ||(val.Room!=null?val.Room.toLowerCase().includes(searchText):"")
        || (val.TransactionDate!=null? new DatePipe('en-US').transform(val.TransactionDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
        ||(val.EnteredBy!=null?val.EnteredBy.toLowerCase().includes(searchText):"")
        ||(val.EnteredBy!=null?val.EnteredBy.toLowerCase().includes(searchText):"")
        || (val.VEffectivedate!=null? new DatePipe('en-US').transform(val.VEffectivedate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
        ||(val.OPhysicianLname!=null?val.OPhysicianLname.toLowerCase().includes(searchText):"")
        ||(val.OPhysicianFname!=null?val.OPhysicianFname.toLowerCase().includes(searchText):"")
        ||(val.RequestedGiveCode!=null?val.RequestedGiveCode.toLowerCase().includes(searchText):"")
        || (val.OrderEffectiveDate!=null? new DatePipe('en-US').transform(val.OrderEffectiveDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "Diagnosis") {
      return data.filter((val) => {
        let result = (val.DiagnosisDescription!=null?val.DiagnosisDescription.toLowerCase().includes(searchText):"")
        || (val.PDiagnosis_CreatedDate!=null? new DatePipe('en-US').transform(val.PDiagnosis_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

        return result;
      });
    }
    if (screenName == "Behaviour") {
      return data.filter((val) => {
        let result = (val.Initials!=null?val.Initials.toLowerCase().includes(searchText):"")
           ||(val.PhysicalbehavioralDesc!=null?val.PhysicalbehavioralDesc.toLowerCase().includes(searchText):"")
          ||(val.VerbalbehavioralDesc!=null?val.VerbalbehavioralDesc.toLowerCase().includes(searchText):"")
           ||(val.OtherbehavioralDesc!=null?val.OtherbehavioralDesc.toLowerCase().includes(searchText):"")
           || (val.rejectevaluationDesc!=null?val.rejectevaluationDesc.toLowerCase().includes(searchText):"")
           ||(val.ResisdentwanderedDesc!=null?val.ResisdentwanderedDesc.toLowerCase().includes(searchText):"")
           ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
           || (val.Date!=null? new DatePipe('en-US').transform(val.Date,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
           || (val.VisitBehaviour_CreatedDate!=null? new DatePipe('en-US').transform(val.VisitBehaviour_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
           return result;
      });
    }
    if (screenName == "NurseNotes") {
      return data.filter((val) => {
        let result = (val.NurseName!=null?val.NurseName.toLowerCase().includes(searchText):"")
        ||(val.Notes!=null?val.Notes.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.VisitNursingNotes_CreatedOn!=null? new DatePipe('en-US').transform(val.VisitNursingNotes_CreatedOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
        return result;
      });
    }
    if (screenName == "FoodIntake") {
      return data.filter((val) => {
        let result = (val.Fluidsb!=null?val.Fluidsb.toLowerCase().includes(searchText):"")
        ||(val.Alternateb!=null?val.Alternateb.toLowerCase().includes(searchText):"")
        ||(val.supplementb!=null?val.supplementb.toLowerCase().includes(searchText):"")
        ||(val.Fluidsl!=null?val.Fluidsl.toLowerCase().includes(searchText):"")
        ||(val.Alternatel!=null?val.Alternatel.toLowerCase().includes(searchText):"")
        ||(val.supplementl!=null?val.supplementl.toLowerCase().includes(searchText):"")
        ||(val.Fluidss!=null?val.Fluidss.toLowerCase().includes(searchText):"")
        ||(val.Alternates!=null?val.Alternates.toLowerCase().includes(searchText):"")
        ||(val.supplements!=null?val.supplements.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.VisitFoodintake_CreatedDate!=null? new DatePipe('en-US').transform(val.VisitFoodintake_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "Weight") {
      return data.filter((val) => {
        let result = (val.IBW!=null?val.IBW.toLowerCase().includes(searchText):"")
        ||(val.Remarks!=null?val.Remarks.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        ||(val.HeightFeet!=null?val.HeightFeet.toString().includes(searchText):"")
        ||(val.HeightInc!=null?val.HeightInc.toString().includes(searchText):"")
        ||(val.Weight!=null?val.Weight.toString().includes(searchText):"")
        ||(val.Type!=null?val.Type.toLowerCase().includes(searchText):"")
        || (val.WeightLog_CreatedOn!=null? new DatePipe('en-US').transform(val.WeightLog_CreatedOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "Vitals") {
      return data.filter((val) => {
        let result = (val.Remark!=null?val.Remark.toLowerCase().includes(searchText):"")
       ||(val.UserName.toLowerCase().includes(searchText))
       ||(val.BloodSugar!=null?val.BloodSugar.includes(searchText):"")
       ||(val.Temperature!=null?val.Temperature.toString().includes(searchText):"")
       ||(val.PulseRate!=null?val.PulseRate.toString().includes(searchText):"")
       ||(val.HeartRate!=null?val.HeartRate.toString().includes(searchText):"")
       ||(val.RespiratoryRate!=null?val.RespiratoryRate.toString().includes(searchText):"")
       ||(val.Remark!=null?val.Remark.toLowerCase().includes(searchText):"")
       ||(val.CistolicBP!=null?val.CistolicBP.toString().includes(searchText):"")
       ||(val.DiastolicBP!=null?val.DiastolicBP.toString().includes(searchText):"")
       || (val.VitalSigns_CreatedOn!=null?  new DatePipe('en-US').transform(val.VitalSigns_CreatedOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "Administration") {
      return data.filter((val) => {
        let result = (val.Resident_Name!=null?val.Resident_Name.toLowerCase().includes(searchText):"")
      return result;
      });
    }
    if (screenName == "Integrations") {
      return data.filter((val) => {
         let result =
         (val.ApiPath!=null?val.ApiPath.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        ||(val.Password!=null?val.Password.toLowerCase().includes(searchText):"")
        ||(val.CratedUserName!=null?val.CratedUserName.toLowerCase().includes(searchText):"");
      return result;
      });
    }
    if (screenName == "Approval") {
      return data.filter((val) => {
         let result =
         (val.Patient_Name!=null?val.Patient_Name.toLowerCase().includes(searchText):"")
        ||(val.Category!=null?val.Category.toLowerCase().includes(searchText):"")
        || (val.CreatedDate!=null?  new DatePipe('en-US').transform(val.CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")

      return result;
      });
    }
    if (screenName == "72hours") {
      return data.filter((val) => {
         let result =
         (val.GiveCodeText!=null?val.GiveCodeText.toLowerCase().includes(searchText):"")
        ||(val.ResidentName!=null?val.PatientFirstName.toLowerCase().includes(searchText):"")
        || (val.AdministerOn!=null?  new DatePipe('en-US').transform(val.AdministerOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

      return result;
      });
    }
    if (screenName == "PRNdocument") {
      return data.filter((val) => {
         let result =
        (val.PatientName!=null?val.PatientName.toLowerCase().includes(searchText):"")
        || (val.GiveCodeText!=null?val.GiveCodeText.toLowerCase().includes(searchText):"")
        || (val.MedicationReason_Desc!=null?val.MedicationReason_Desc.toLowerCase().includes(searchText):"")
        ||(val.AdministerComment!=null?val.AdministerComment.toLowerCase().includes(searchText):"")
        || (val.AdminsterOn!=null?  new DatePipe('en-US').transform(val.AdminsterOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");

      return result;
      });
    }
    if (screenName == "ControlSubCount") {
      return data.filter((val) => {
         let result =
        (val.ResidentName!=null?val.ResidentName.toLowerCase().includes(searchText):"")
        || (val.Order!=null?val.Order.toLowerCase().includes(searchText):"")
        ||(val.LastCertified!=null?val.LastCertified.toLowerCase().includes(searchText):"")
        ||(val.NurseStationName!=null?val.NurseStationName.toLowerCase().includes(searchText):"")

      return result;
      });
    }
    if (screenName == "ControlSub") {
      return data.filter((val) => {
         let result =
        (val.ResName!=null?val.ResName.toLowerCase().includes(searchText):"")
        || (val.GiveCodeText!=null?val.GiveCodeText.toLowerCase().includes(searchText):"")
        || (val.GiveDosageForm!=null?val.GiveDosageForm.toLowerCase().includes(searchText):"")
        || (val.Quantity!=null?val.Quantity.toString().includes(searchText):"")
        ||(val.PhyName!=null?val.PhyName.toLowerCase().includes(searchText):"")
        ||(val.ProviderAdminDrugInsText!=null?val.ProviderAdminDrugInsText.toLowerCase().includes(searchText):"")
        || (val.OrderEffectiveDate!=null?  new DatePipe('en-US').transform(val.OrderEffectiveDate,'MM/dd/yyyy').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "CensusDashboard") {
      return data.filter((val) => {
         let result =
        (val.name!=null?val.name.toLowerCase().includes(searchText):"")
        || (val.nsname!=null?val.nsname.toLowerCase().includes(searchText):"")
      return result;
      });
    }
    if (screenName == "DrFirstOrder") {
      return data.filter((val) => {
         let result =
        (val.GiveCodeIdentifier!=null?val.GiveCodeIdentifier.toLowerCase().includes(searchText):"")
        || (val.Drug!=null?val.Drug.toLowerCase().includes(searchText):"");
      return result;
      });
    }
    if (screenName == "HL7DrFirstOrder") {
      return data.filter((val) => {
         let result =
        (val.GiveCodeIdentifier!=null?val.GiveCodeIdentifier.toLowerCase().includes(searchText):"")
        || (val.Drug!=null?val.Drug.toLowerCase().includes(searchText):"");
      return result;
      });
    }
    if (screenName == "MailBox") {
      return data.filter((val) => {
         let result =
        (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.Subject!=null?val.Subject.toLowerCase().includes(searchText):"")
        || (val.MailBoxDate!=null?new DatePipe('en-US').transform(val.MailBoxDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "ReadMail") {
      return data.filter((val) => {
         let result =
        (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.Subject!=null?val.Subject.toLowerCase().includes(searchText):"")
        || (val.MailBox_Date!=null?new DatePipe('en-US').transform(val.MailBox_Date,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "ComposeMail") {
      return data.filter((val) => {
         let result =
        (val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.Subject!=null?val.Subject.toLowerCase().includes(searchText):"")
        || (val.MailBox_Date!=null?new DatePipe('en-US').transform(val.MailBox_Date,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName =="ResidentColor") {
      return data.filter((val) => {
         let result =
        (val.Company_Name!=null?val.Company_Name.toLowerCase().includes(searchText):"")
        || (val.Color_Description!=null?val.Color_Description.toLowerCase().includes(searchText):"")
        || (val.PatientType_CreatedBy!=null?val.PatientType_CreatedBy.toLowerCase().includes(searchText):"")
        || (val.PatientType_CreatedDate!=null?new DatePipe('en-US').transform(val.PatientType_CreatedDate,'MM/dd/yyyy').includes(searchText):"");
      return result;
      });
    }
    if (screenName =="DocumentManager") {
      return data.filter((val) => {
         let result =
        (val.DocName!=null?val.DocName.toLowerCase().includes(searchText):"")
        || (val.FolderName!=null?val.FolderName.toLowerCase().includes(searchText):"")
        || (val.DocDescription!=null?val.DocDescription.toLowerCase().includes(searchText):"");
      return result;
      });
    }
    if (screenName =="Stock") {
      return data.filter((val) => {
         let result =
        (val.DrugName!=null?val.DrugName.toLowerCase().includes(searchText):"")
        || (val.FacilityName!=null?val.FacilityName.toLowerCase().includes(searchText):"")
        || (val.InHand!=null?val.InHand.toLowerCase().includes(searchText):"")
        || (val.NurseStationName!=null?val.NurseStationName.toLowerCase().includes(searchText):"")
        || (val.Barcode!=null?val.Barcode.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        || (val.Stock_CreatedDate!=null?  new DatePipe('en-US').transform(val.Stock_CreatedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName =="Ekit") {
      return data.filter((val) => {
         let result =
        (val.DrugName!=null?val.DrugName.toLowerCase().includes(searchText):"")
        || (val.InHand!=null?val.InHand.toLowerCase().includes(searchText):"")
        || (val.NurseStationName!=null?val.NurseStationName.toLowerCase().includes(searchText):"")
        || (val.Barcode!=null?val.Barcode.toLowerCase().includes(searchText):"")
        ||(val.UserName!=null?val.UserName.toLowerCase().includes(searchText):"")
        ||(val.LotNumber!=null?val.LotNumber.toLowerCase().includes(searchText):"")
        || (val.ExpiryDate!=null?  new DatePipe('en-US').transform(val.ExpiryDate,'MM/dd/yyyy').includes(searchText):"")
        || (val.Ekit_CreatedOn!=null?  new DatePipe('en-US').transform(val.Ekit_CreatedOn,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName =="AcknowledgeOrders") {
      return data.filter((val) => {
         let result =
        (val.PatientName!=null?val.PatientName.toLowerCase().includes(searchText):"")
        || (val.OrderType!=null?val.OrderType.toLowerCase().includes(searchText):"")
        || (val.DrugName!=null?val.DrugName.toLowerCase().includes(searchText):"")
        || (val.StartDate!=null?  new DatePipe('en-US').transform(val.StartDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"")
        || (val.ReceivedDate!=null?  new DatePipe('en-US').transform(val.ReceivedDate,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "Alerts") {
      return data.filter((val) => {
         let result =
        (val.ResidentName!=null?val.ResidentName.toLowerCase().includes(searchText):"")
        || (val.TypeName!=null?val.TypeName.toLowerCase().includes(searchText):"")
        || (val.DateTime!=null?new DatePipe('en-US').transform(val.DateTime,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "DrFistFilesGrid") {
      return data.filter((val) => {
         let result =
        (val.PatientMRNumber !=null?val.PatientMRNumber .toLowerCase().includes(searchText):"")
        || (val.CompanyName !=null?val.CompanyName .toLowerCase().includes(searchText):"")
        || (val.PatientName  !=null?val.PatientName  .toLowerCase().includes(searchText):"")
        || (val.ReceivedOn !=null?new DatePipe('en-US').transform(val.ReceivedOn ,'MM/dd/yyyy hh:mm:ss a').includes(searchText):"");
      return result;
      });
    }
    if (screenName == "CompanyConfiguration") {
      return data.filter((val) => {
         let result =
        (val.Company_Name !=null?val.Company_Name .toLowerCase().includes(searchText):"")
        || (val.FingersDesc1 !=null?val.FingersDesc1 .toLowerCase().includes(searchText):"")
        || (val.StockReportFor !=null?val.StockReportFor .toLowerCase().includes(searchText):"")
        || (val.HLDirectionalWaysDesc  !=null?val.HLDirectionalWaysDesc  .toLowerCase().includes(searchText):"")
        || (val.FteCategory_Desc !=null?val.FteCategory_Desc .toLowerCase().includes(searchText):"")
        || (val.EventCat_Desc  !=null?val.EventCat_Desc  .toLowerCase().includes(searchText):"")
      return result;
      });
    }
    if (screenName =="PhysicianDetails") {
      return data.filter((val) => {
         let result =
        (val.Facility_Name!=null?val.Facility_Name.toString().toLowerCase().includes(searchText):"")
        || (val.NurseStation_Name!=null?val.NurseStation_Name.toString().toLowerCase().includes(searchText):"")
        || (val.PhysicianNPI!=null?val.PhysicianNPI.toString().toLowerCase().includes(searchText):"")
        || (val.PhysicianName!=null?val.PhysicianName.toString().toLowerCase().includes(searchText):"")
        ||(val.PhysicianCity!=null?val.PhysicianCity.toString().toLowerCase().includes(searchText):"")
        || (val.PhysicianState!=null?val.PhysicianState.toString().toLowerCase().includes(searchText):"")
        || (val.Country_Name!=null?val.Country_Name.toString().toLowerCase().includes(searchText):"")
        ||(val.PhysicianZip!=null?val.PhysicianZip.toString().toLowerCase().includes(searchText):"")
      return result;
      });
    }
    if (screenName =="PharmacyDetails") {
      return data.filter((val) => {
         let result =
        (val.Facility_Name!=null?val.Facility_Name.toString().toLowerCase().includes(searchText):"")
        || (val.NurseStation_Name!=null?val.NurseStation_Name.toString().toLowerCase().includes(searchText):"")
        //|| (val.Pharmacy_Options!=null?val.Pharmacy_Options.toString().toLowerCase().includes(searchText):"")
        || (val.PharmacyName!=null?val.PharmacyName.toString().toLowerCase().includes(searchText):"")
        ||(val.Pharmacy_City!=null?val.Pharmacy_City.toString().toLowerCase().includes(searchText):"")
        || (val.Pharmacy_State!=null?val.Pharmacy_State.toString().toLowerCase().includes(searchText):"")
        || (val.Pharamcy_Type!=null?val.Pharamcy_Type.toString().toLowerCase().includes(searchText):"")
        ||(val.Pharmacy_Zip!=null?val.Pharmacy_Zip.toString().toLowerCase().includes(searchText):"")
        ||(val.NCPDP!=null?val.NCPDP.toString().toLowerCase().includes(searchText):"")
        ||(val.NPI!=null?val.NPI.toString().toLowerCase().includes(searchText):"")


      return result;
      });
    }
    if (screenName =="CompanyToBedMapp") {
      return data.filter((val) => {
         let result =
        (val.CompanyName!=null?val.CompanyName.toString().toLowerCase().includes(searchText):"")
        || (val.FacilityName!=null?val.FacilityName.toString().toLowerCase().includes(searchText):"")
        || (val.NursingStationName!=null?val.NursingStationName.toString().toLowerCase().includes(searchText):"")
        || (val.FloorPrior!=null?val.FloorPrior.toString().toLowerCase().includes(searchText):"")
        || (val.WingPrior!=null?val.WingPrior.toString().toLowerCase().includes(searchText):"")
        || (val.RoomPrior!=null?val.RoomPrior.toString().toLowerCase().includes(searchText):"")
        || (val.BedPrior!=null?val.BedPrior.toString().toLowerCase().includes(searchText):"")
      return result;
      });
    }

  if(screenName=="certificationOrders")
  {
    return data.filter((val) => {
      let result =
     (val.PatientName!=null?val.PatientName.toString().toLowerCase().includes(searchText):"")
     || (val.PatientDOB!=null?val.PatientDOB.toString().toLowerCase().includes(searchText):"")
     || (val.PatientGender!=null?val.PatientGender.toString().toLowerCase().includes(searchText):"")
     || (val.NurseStationName!=null?val.NurseStationName.toString().toLowerCase().includes(searchText):"")
   return result;
   });
  }
  if(screenName=="RefillMailConfigMaster")
  {
    return data.filter((val) => {
      let result =
     (val.FacilityName!=null?val.FacilityName.toString().toLowerCase().includes(searchText):"")
     || (val.NursingStationNames!=null?val.NursingStationNames.toString().toLowerCase().includes(searchText):"")
     || (val.TimeText!=null?val.TimeText.toString().toLowerCase().includes(searchText):"")
     || (val.MailTo!=null?val.MailTo.toString().toLowerCase().includes(searchText):"")
     || (val.MailCC!=null?val.MailCC.toString().toLowerCase().includes(searchText):"")
   return result;
   });
  }
    if(screenName=="EmarHTMLBarcodes")
    {
      let barcodes= data.split('|');
      let barcodevalue =searchText.split('/');
      let findText = barcodevalue[0];
      if(barcodes.length>0)
      {
        return barcodes.filter((val) => {
          let result = (val!=null?val.toLowerCase()==(findText.toLowerCase()):"")
          return result;
        });
    }
  }
  if(screenName=="EmarHTMLCertified")
    {
      debugger
      let certifiers= data.split(',');
      if(certifiers.length>0)
      {
        return certifiers.filter((val) => {
          let result = (val!=null?val.toLowerCase()==(searchText.toLowerCase()):"")
          return result;
        });
    }
  }
  if(screenName=="searchAllStock")
  {
    return data.filter((val) => {
      let result =
     (val.DrugName!=null?val.DrugName.toString().toLowerCase().includes(searchText):"")
     || (val.InHand!=null?val.InHand.toString().toLowerCase().includes(searchText):"")
     || (val.Barcode!=null?val.Barcode.toString().toLowerCase().includes(searchText):"")
   return result;
   });
  }
  }
  splitData(val:string):string[] {
    return val.split('|');
  }
}
