import { NurseStation, } from './../../../models/facility.model';
import { Router } from '@angular/router';
import { Component, OnInit, Input, ChangeDetectorRef,ViewChild,ElementRef } from '@angular/core';
import { Facility } from '../../../models/facility.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ResidentDemographic, ResidentAdmitDischarge, ColorCode } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DomSanitizer } from '@angular/platform-browser';
import { AlertService } from '../../../_services';
import { Screens, Activity } from '../../../models/useractivity.model';
import { PatientType } from '../../../models/common.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UpdatevisitinfoComponent } from '../updatevisitinfo/updatevisitinfo.component';
import { ResidentmergeComponent } from '../residentmerge/residentmerge.component';
import { WeightMaster } from 'src/app/models/assesments.model';
declare function fileclear(): any;
@Component({
  selector: 'app-residentinformation',
  templateUrl: './residentinformation.component.html',
  styleUrls: ['./residentinformation.component.css']
})
export class ResidentinformationComponent implements OnInit {
  @ViewChild('imageModelClose') imageModelClose : ElementRef
  public facilities: Facility[];
  public nurseStations: NurseStation[];
  public residents: ResidentDemographic[];
  public residentAdmits: ResidentAdmitDischarge[];
  public residentId: number=0;
  private resImage: File;
  myform: FormGroup=new FormGroup({});
  public demographicInfoData = {} as DemographicInfo;
  columnsList: any[];
  pageConfig = {};
  residentImage: any;
  public PatientTypeId: number;
  public PatientTypeData: PatientType[];
  public dropdownSettings_ColorCode: any = {};
  public patientType: string = '';
  public colorCodeObj: ColorCode;
  public NurseStatioId: any;
  dropdownSettings_Nuresestation: any = {};
  dropdownSettings_Resident: any = {};
  ShowFilter = true;
  public colorArray: any[] = [];
  public selectedNursestation = [];
  public selectedResItem = [];
  selectedColorCodes = [];
  DemographicpageConfig ={};
  VisitInfopageConfig ={};
  AllergiespageConfig ={};
  DiagnosispageConfig ={};
  residentStatus: { "residentId": number; "residentStatus": number; };
  private nurseStationId: number = 0;
  facilityName: any;
  public reAdmitDate:string="";
  public reAdmitModal:boolean=false;
  public isUserAdmin:boolean=false;
  public errormessage:string="";
  public checkDischargeDate:any;
  public weightDetails:any;
  public isWeight:boolean =false;
  public isHeight:boolean =false;
  public getID:number;
  MyImages: any;
  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private sharedService: SharedService,
    private alertService: AlertService, private persistanceService: PersistanceService, private sanitizer: DomSanitizer, private dateFormatPipe: CustomdatePipe, private route: Router, private modalService: NgbModal) {
    //this.route.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    window.scroll(0,0);
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);

    //this.getNurseStations();
    this.myform.valueChanges.subscribe(newValues => {
         console.log('input values:', newValues);
       });
    if(this.residentId!=0)
    {
      if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
        this.isUserAdmin = true;
      }
    this.getFacilityNSResidentsDataByPId();

    this.dropdownSettings_ColorCode = {
      singleSelection: false,
      idField: "PatientType_Id",
      textField: "Color_Description",
      text: "PatientTypeData",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      limitSelection: 3,
      noDataAvailablePlaceholderText: 'Please Select Nursing Station',
    };
    this.dropdownSettings_Nuresestation = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      itemsShowLimit: 3,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_Resident = {
      singleSelection: true,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter,
      noDataAvailablePlaceholderText: 'Please Select Nursing Station',
    };
    this.getResidentAdmitDischargeDate();
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ResidentGrid");
    this.DemographicpageConfig = this.persistanceService.getPermissionsByScreen("DemographicInformation");
    this.VisitInfopageConfig = this.persistanceService.getPermissionsByScreen("AdmitVisitInfo");
    this.AllergiespageConfig = this.persistanceService.getPermissionsByScreen("Allergies");
    this.DiagnosispageConfig = this.persistanceService.getPermissionsByScreen("Diagnosis");
    if (this.VisitInfopageConfig == undefined) {
      this.VisitInfopageConfig =0;
    }
    if (this.DemographicpageConfig == undefined) {
      this.DemographicpageConfig =0;
    }
    if (this.AllergiespageConfig == undefined) {
      this.AllergiespageConfig =0;
    }
    if (this.DiagnosispageConfig == undefined) {
      this.DiagnosispageConfig =0;
    }
    this.myform = new FormGroup({
      ddlresidents: new FormControl(this.residentId),
      nursestationName: new FormControl('0'),
      ddlColorCode: new FormControl(''),
    });
    this.userActivity();
  }
  else
  this.route.navigate(['/home/residentgrid']);
  //calling height and weight

  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.DemographicInformation, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error('ua');
      });
  }
  ngAfterViewInit() {

  }
  getResident(newResidentId: any) {
    this.sharedService.changePatientId(newResidentId);
    this.residentId = newResidentId;
    //this.getResidentDropData();
    this.getFacilityNSResidentsDataByPId();
  }
  getAllPatientTypes() {
    this.dataservice.get<any[]>(this.config.Resident_Demographic_GetAllPatientTypesByPId + this.residentId)
      .subscribe(res => {

        if (res.length > 0) {
          this.myform.patchValue({
            ddlColorCode: res,
          })
          this.selectedColorCodes = res;
          this.colorArray = [];
          res.forEach(item => {
            let filter=this.PatientTypeData.find(i => i.PatientType_Id == item.PatientType_Id);
            if(filter!=undefined)
            {
            this.colorArray.push(filter.Color_Code)
            }
          });
        }
        else {
          this.colorArray = [];
          this.selectedColorCodes = [];
        }
      }, error => {
        this.alertService.error('error in getAllPatientTypes');
        this.ng4LoadingSpinnerService.hide();
      });
  }
  // getNurseStations() {
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
  //     .subscribe(res => {
  //       this.nurseStations = res;
  //       this.getResidentDropData();
  //     },
  //       error => {
  //         this.alertService.error('error in getNurseStations');
  //       });
  // }
  // this.registrationForm.valueChanges.subscribe(newValues => {
    //   console.log('New values:', newValues);
    // });
  getDemographicInfoData() {
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {

        this.demographicInfoData = res;
        // console.log(res,"data check");
        this.residentStatus = {
          "residentId": this.residentId,
          "residentStatus": res.PVisit_Status
        }
        this.getID =res.Patient_Id;
        this.getWeightHeight();
        this.getPatientTypeData();
        this.selectedResItem.push(this.residents.filter(f => f.Patient_Id == this.residentId)[0]);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error('getDemographicInfoData');
          this.ng4LoadingSpinnerService.hide();
        });
  }
getMerge()
{
    const modalRef = this.modalService.open(ResidentmergeComponent, { size: 'lg', windowClass: 'my-class' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.result.subscribe((receivedResult) => {
      if (receivedResult == 2) {
        this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
        //this.getDemographicInfoData();
        this.getFacilityNSResidentsDataByPId();
        modalRef.close();
      }
      modalRef.close();
    });
}

  // getResidentDropData() {
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentDropData + userId)
  //     .subscribe(res => {
  //       this.residents = res;
  //       if (res != null) {
  //         if (this.residentId == 0) {
  //           this.residentId = this.residents[0].Patient_Id;
  //           this.sharedService.changePatientId(this.residentId);
  //         }
  //         this.getDemographicInfoData();
  //         this.getNurseStationByPId();
  //         this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
  //         this.myform.patchValue({
  //           ddlresidents: this.selectedResItem
  //         });
  //       }
  //       else {

  //       }
  //     }, error => this.alertService.error('in res'));
  // }
   Validate(oForm) {
     debugger;

      var fileName =oForm.name;
      var idxDot = fileName.lastIndexOf(".") + 1;
      var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
      if (extFile!="jpg" && extFile!="jpeg" && extFile!="png"){
        this.alertService.warn("Only jpg/jpeg and png files are allowed");
        fileclear();
        return false;
      }else{
        return true;
      }

}
  getResidentAdmitDischargeDate() {
    this.dataservice.get<ResidentAdmitDischarge[]>(this.config.Emar_ResidentDemographic_GetResidentAdmitDischargeData + this.residentId)
      .subscribe(res => {
        this.residentAdmits = res;
        this.checkDischargeDate=res!=null && res.length>0?res[res.length-1].DischargeDate!=null?res[res.length-1].DischargeDate:"":"";
      }, error => this.alertService.error('ad dis'));
  }

  fileUploadChange(event: any): void {

    this.resImage = null;

    if (event.target.files && event.target.files[0]) {
      this.Validate( event.target.files[0]);
      this.resImage = event.target.files[0];
    }
  }

  UploadResidentImage() {

    if(this.resImage!=undefined &&this.resImage!=null)
    {

    let formData: FormData = new FormData();
    formData.append('Image', this.resImage);
    this.dataservice.postFormData(this.config.Emar_ResidentDemographic_UploadResidentImage, this.residentId, formData).subscribe(res => {
      debugger;
      if(res == 10)
      {
        this.alertService.warn("you are uploaded corrupted image");
      }
      else
      {
        this.resImage = null;
        this.alertService.success('Image uploaded successfully');
      fileclear();
        if(this.imageModelClose!=undefined && this.imageModelClose!=null)
        {
        this.imageModelClose.nativeElement.click();
        }
        this.getDemographicInfoData();

      }

    }, error => this.alertService.error('upl'));
    }
    else{
      this.alertService.warn("Please select image to upload");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  changeResident() {
    let residents = this.myform.value.ddlresidents;
    debugger;
    if (residents.length != 0) {
      this.ng4LoadingSpinnerService.show();
      this.residentId = this.myform.value.ddlresidents[0].Patient_Id;
      this.sharedService.changePatientId(this.residentId);
      //this.getNurseStationByPId();
      this.getDemographicInfoData();

    }
    else {
      this.residentId = 0;
      this.sharedService.changePatientId(this.residentId);
      let nursestations = this.myform.value.nursestationName
      if (nursestations.length == 0) {
        this.getDemographicInfoByNurseStation(0);
      }
      else {
        let nurseStationId = this.selectedNursestation[0].NurseStation_Id;
        this.getDemographicInfoByNurseStation(nurseStationId);
      }
    }
  }
  changeNurseStation(item:any) {

    if (item.NurseStation_Id == 0) {
      this.residents = [];
      this.selectedResItem = [];
      this.PatientTypeData=[];
      this.selectedColorCodes=[];
      this.alertService.warn("Select nursing station to get residents list");
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.getDemographicInfoByNurseStation(item.NurseStation_Id);
      this.ng4LoadingSpinnerService.hide();
    }

  }
  getDemographicInfoByNurseStation(stationId: number) {
    this.dataservice.get<ResidentDemographic[]>(this.config.Emar_ResidentDemographic_GetResidentsByNurseStationId + stationId)
      .subscribe(res => {
        this.residents = res;
        if (res.length > 0) {
          this.residentId = this.residents[0].Patient_Id;
          this.sharedService.changePatientId(this.residentId);
          this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
          this.getDemographicInfoData();
          this.getPatientTypeData();
        }
        else {
          this.selectedResItem = [];
          this.residentId = 0;
           if(res.length == 0)
      {


      this.route.navigate(['/home/residentgrid']);
      return false;


      }
        }
        this.myform.patchValue({
          ddlresidents: this.selectedResItem,
        })
      },
        error => {
          this.alertService.error('Error while loading Demographic Information')
          this.ng4LoadingSpinnerService.hide();
        });
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  getPatientTypeData() {
    this.dataservice.get<any[]>(this.config.Resident_Common_GetPatientTypeDrop + this.residentId)
      .subscribe(res => {
        this.PatientTypeData = res;
        this.getAllPatientTypes();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error('pat type');
          this.ng4LoadingSpinnerService.hide();
        });
  }

  SaveColorCode() {
    if (this.demographicInfoData.PVisit_Status != 1) {
      this.alertService.warn("Resident is inactive");
    }
    else {

      let selectedColorCodes = this.myform.value.ddlColorCode;
      selectedColorCodes.forEach(element => { this.patientType += element.PatientType_Id + ',' });
      this.patientType = this.patientType.substring(0, this.patientType.length - 1);
      // if (this.myform.value.ddlColorCode == '')
      //   this.alertService.warn("Please Select color code");
      // else {
        this.colorCodeObj = new ColorCode();
        this.colorCodeObj.ColourType_Id = 0;
        this.colorCodeObj.Patient_Id = this.residentId;
        this.colorCodeObj.PatientType_Id = 0;
        this.colorCodeObj.PatientTypeId = this.patientType;
        this.colorCodeObj.ColourType_Status = 1;
        this.colorCodeObj.ColourType_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        this.colorCodeObj.ColourType_CreatedOn = this.dateFormatPipe.transform(new Date()),
          this.dataservice.post(this.config.Emar_Colorpicker_InsertPatientType, this.colorCodeObj)
            .subscribe(res => {
              this.ng4LoadingSpinnerService.hide();
              this.alertService.success("Save successful");
              this.patientType = '';
              //this.selectedItems = [];
            },
              error => {
                this.alertService.error(error.message);
                this.ng4LoadingSpinnerService.hide();
              });

    }
  }

  getNurseStationByPId() {
    this.selectedNursestation = [];
    this.dataservice.get<any>(this.config.Resident_Demographic_GetNurseStationByPId + this.residentId)
      .subscribe(res => {
        let id = res;
        //this.selectedNursestation=res;
        this.selectedNursestation = this.nurseStations.filter(item => item.NurseStation_Id == id);
        this.myform.patchValue({
          nursestationName: this.selectedNursestation,
        });
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error('Error while loading Nursestations');
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onColorcodeSelect(item: any) {
    let selectedItems = this.myform.value.ddlColorCode;
    this.colorArray = [];
    selectedItems.forEach(item => {
      this.colorArray.push(this.PatientTypeData.find(i => i.PatientType_Id == item.PatientType_Id).Color_Code)
    });
  }
  onColorcodeDeSelect(item: any) {
    let selectedItems = this.myform.value.ddlColorCode;
    this.colorArray = [];
    selectedItems.forEach(item => {
      this.colorArray.push(this.PatientTypeData.find(i => i.PatientType_Id == item.PatientType_Id).Color_Code)
    });
  }
  back() {
    localStorage.setItem("BackClick", JSON.stringify(true));
    this.route.navigate(['/home/residentgrid']);
  }
  onNurseStationSelect(item: any) {
    this.changeNurseStation(item);
  }
  onNurseStationDeSelect(item: any) {
    this.clearData();
    this.alertService.warn("select nursing station to get residents list");
  }
  onResidentSelect(item: any) {
    this.changeResident();
  }
  onResidentDeSelect(item: any) {
    this.changeResident();
  }
  openUpdateVisitInfoModal() {
    const modalRef = this.modalService.open(UpdatevisitinfoComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.updateVisitInfo.subscribe((receivedResult) => {
      if (receivedResult == 1) {
        this.alertService.success("Save successful.");
      }
      modalRef.close();
    });
  }
  getFacilityNSResidentsDataByPId() {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetFacilityNSResidentsDataByPId + this.residentId)
      .subscribe(res => {
        this.nurseStations = res.NSDrop;
        this.nurseStationId = res.NursingStationId;
        this.residents = res.ResidentDrop;
        this.facilityName = res.FacilityName;
        this.selectedNursestation = this.nurseStations.filter(item => item.NurseStation_Id == this.nurseStationId);
        this.selectedResItem = this.residents.filter(item => item.Patient_Id == this.residentId);
        this.myform.patchValue({
          nursestationName: this.selectedNursestation,
          ddlresidents: this.selectedResItem
        });
        this.getDemographicInfoData();
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  clearData() {
    this.residents = [];
    this.selectedResItem = [];
    this.residentId = 0;
    this.residentStatus=null;
    this.PatientTypeData=[];
    this.selectedColorCodes=[];
    this.demographicInfoData = {} as DemographicInfo;
  }
  PatientReAdmission()
  {

    this.errormessage = "";
    if (this.checkDischargeDate!="" && this.reAdmitDate < (this.checkDischargeDate.split('T')[0])) {
      this.errormessage = "Re-Admit date cannot be less than Discharge date.!"
    }
    else
    {
this.ng4LoadingSpinnerService.show();
let objPreadmit={
  PatientId:this.residentId,
  AdmitDate:this.reAdmitDate
}
this.dataservice.post(this.config.Emar_AdminApproval_PatientReAdmission,objPreadmit)
.subscribe(res=>{

  this.alertService.success("Re-admission successful");
  this.ng4LoadingSpinnerService.hide();
  this.reAdmitModal=false;
  this.getDemographicInfoData();
//   this.route.routeReuseStrategy.shouldReuseRoute = function(){return false;};
//   let url=this.route.url+'?';
//   this.route.navigateByUrl(url)
// .then(() => {
// this.route.navigated = false;
// this.route.navigate([this.route.url]);
//});
},
  error=>{
    this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
  });
  }
}
  ReAdmit()
  {
    this.reAdmitModal=true;
    this.errormessage="";
  }
  closeReadmitModel()
  {
    this.reAdmitModal=false;
    this.reAdmitDate='';
  }
  getToday(): string {
    return new Date().toISOString().split('T')[0]
  }
  mouseEnter(Id:any)
  {

    this.MyImages = Id;

  }
  mouseLeave()
  {
    this.MyImages =null;

  }
  updatePregnancy(isChecked: boolean){
    if(this.demographicInfoData){
      const type = 1;
      const Pregnant = isChecked ? 1 : 0;
      let Data = {
        PatientID: this.demographicInfoData.Patient_Id,
        Type: type,
        IScheck: Pregnant
      };
      this.dataservice.post(this.config.Emar_ResidentDemographic_UpdatePregnecyFeedingEntity , Data).subscribe(
         res =>{}
      )
      this.demographicInfoData.Pregnant = isChecked ? 1 : 0;

    }

  }

   updateFeeding(isChecked: boolean){
    if(this.demographicInfoData){
      const type = 2;
      const BreastFeeding = isChecked ? 1 : 0;
      let dataFeed = {
        PatientID: this.demographicInfoData.Patient_Id,
        Type: type,
        IScheck: BreastFeeding
      };
      this.dataservice.post(this.config.Emar_ResidentDemographic_UpdatePregnecyFeedingEntity , dataFeed).subscribe(
         res =>{}
      )
      this.demographicInfoData.BreastFeeding = isChecked ? 1 : 0;

    }

  }
  getWeightHeight(){
    //console.log(this.residentId,"from orders")
    if(this.getID >0){
     let id = this.getID;
     this.dataservice.get<any>(this.config.Weight_GetWeightList + id )
     .subscribe(res => {
      if(res.length>=1){
        this.weightDetails = res[res.length-1];
        //console.log(this.weightDetails,"response");
        if(this.weightDetails.Patient_Id ==id){
          if(this.weightDetails.HeightFeet > 0){
            this.isHeight = true;
          }
          if(this.weightDetails.Weight>0){
            this.isWeight = true;
          }
        }
      } else{
        this.isWeight = false;
        this.isHeight = false;
      }

     })
    }


   }
}
