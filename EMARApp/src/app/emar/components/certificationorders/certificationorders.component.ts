import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { Router } from '@angular/router';
import { Floor, NurseStation, Wing, Bed, Room, FiltersConfig } from '../../../models/facility.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { DomSanitizer } from '@angular/platform-browser';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { OrderInfoAlert ,CommonDcOrderStatus} from '../../../models/orders.model';
import { Screens, Activity } from '../../../models/useractivity.model';
@Component({
  selector: 'app-certificationorders',
  templateUrl: './certificationorders.component.html',
  styleUrls: ['./certificationorders.component.css']
})
export class CertificationordersComponent implements OnInit {
  myform: FormGroup;
  public template;
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  public selectedfaItems = [];
  public selectednItems = [];
  public facilities: any[];
  public nurseStations: NurseStation[];
  public userId: number;
  public residentsList: any[] = [];
  pageConfig = {};
  p: number = 1;
  searchText: string = "";
  gridPagination = this.config.gridPagination;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedFacItems = [];
  public selectstyle: number = 1;
  residentId: any;
  public demographicInfoData: DemographicInfo;
  public ordersInfo: OrderInfoAlert = {} as any;
  public patientTypeList: any[] = [];
  public ordersList: any[] = [];
  CheckAll: boolean = false;
  public selectedRecords = [];
  public MyImages:any;
  public orderStatusDCObj:CommonDcOrderStatus;
  public credentialform:FormGroup;
  public modalUserCredential:boolean=false;
  public certifierObj:any;
  public certifiedDates: any[] = [];
  public certDate:any[]=[];
  public dropdownSettings_Date: any = {};
  public certifiedOrdersList:any[]=[];
  public physiciansdrop:any[]=[];
  public isUserAdmin:boolean=false;
  public selectedphyItems:any[]=[];
  public dropdownSettings_Physician:any={};
  @ViewChild('inputFocus') inputFocus:ElementRef

  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, public sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("OrderCertification");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.template=this.dataservice.template;
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
          ddlnursestations: new FormControl(''),
          physician:new FormControl(''),
          check: new FormControl('')
        });
        this.credentialform=new FormGroup({
          certUsername:new FormControl('',Validators.required),
          certPass:new FormControl('',Validators.required),
          Profcre:new FormControl('',Validators.maxLength(50)),
          months:new FormControl(6,[Validators.required,Validators.min(1),Validators.max(12)]),
        });
        this.dropdownSettings_Facilities = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          text: "Facilities",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        this.dropdownSettings_NurseStations = {
          singleSelection: false,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          noDataAvailablePlaceholderText: 'Please select Facility',
        };
        this.dropdownSettings_Date={
          singleSelection: true,
          idField: "CertifyTime_ID",
          textField: "CertifyedPhysicianWithDate",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true,
        };
        this.dropdownSettings_Physician = {
          singleSelection: true,
          idField: "PhysicianNPI",
          textField: "PhysicianFullName",
          text: "Select",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true,
        };
        if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
          this.isUserAdmin = true;
        }
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.CertificationOrders, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getFiltersData(userId: number): any {
    this.ng4LoadingSpinnerService.show();

    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.facilities = res.Facilities;
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedFacItems = [];
            if (checkFacExist != undefined) {
              this.selectedFacItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacilities: this.selectedFacItems,
            });
          }
        }
        else if (this.facilities.length > 0) {
            this.selectedFacItems = [];
            this.selectedFacItems.push(this.facilities[0]);
            this.getNurseStationByFacilityID(this.facilities[0].Facility_Id);
          
          this.myform.patchValue({
            ddlfacilities: this.selectedFacItems,
          });
        }
        else
        {
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
        if (this.nurseStations.length > 0) {
          this.selectednItems = this.nurseStations;
          this.myform.patchValue({
            ddlnursestations: this.selectednItems,
          });
          if(this.isUserAdmin==true)
          {
            this.getPhysicianDrop();
          }
          else
          {
          this.getFiltersDataBySelection();
          }
        }
        else{
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.physiciansdrop=[];
    this.residentsList = [];
    this.myform.patchValue({
      ddlnursestations: '',
      physician:'',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.physiciansdrop=[];
    this.residentsList = [];
    this.myform.patchValue({
      ddlnursestations: '', 
      physician:'',
    });
  }
  onNurseStationSelect(item: any) {
    this.residentsList = [];
    this.physiciansdrop=[];
    if(this.isUserAdmin==true)
    {
      this.getPhysicianDrop();
    }
    else
    {
      this.getFiltersDataBySelection();
    }
  }
  onNurseStationDeSelect(item: any) {
    this.residentsList = [];
    this.physiciansdrop=[];
    if(this.isUserAdmin==true)
    {
      this.getPhysicianDrop();
    }
    else
    {
      this.getFiltersDataBySelection();
    }
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestations = item;
    this.residentsList = [];
    this.physiciansdrop=[];
    if(this.isUserAdmin==true)
    {
      this.getPhysicianDrop();
    }
    else
    {
      this.getFiltersDataBySelection();
    }
  }
  onNurseStationDeSelectAll(item: any) {
    this.residentsList = [];
    this.selectednItems = [];
    this.physiciansdrop=[];
    this.selectedphyItems=[],
    this.myform.patchValue({
      ddlnursestations: this.selectednItems,
      physician:this.selectedphyItems
    })
  }
  getFiltersDataBySelection() {
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.residentsList = [];
      this.alertService.error("Use filters to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.isUserAdmin==true && this.myform.value.physician.length==0)
    {
      this.residentsList = [];
      this.alertService.error("Select physician to display order list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.loadGrid();
    }
  }
  loadGrid() {
    this.ng4LoadingSpinnerService.show();
    let nsList = this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(',');
    let phyNpi=this.isUserAdmin==true?this.myform.value.physician.length!=0 && this.myform.value.physician!=undefined && this.myform.value.physician!=null && this.myform.value.physician!=""?this.myform.value.physician[0].PhysicianNPI:null:null;
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetCertifyOrderResidentGridData + nsList + "/" + this.userId+"/"+phyNpi)
      .subscribe((res: any) => {
        this.residentsList = res;
        if (this.residentsList.length == 0)
          this.alertService.warn("No data available.");
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getPhysicianDrop() {
    this.ng4LoadingSpinnerService.show();
    let nsList = this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(',');
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetPhysicianDropCertifyOrders + nsList)
      .subscribe(res => {
        this.physiciansdrop = res;
        if(res.length>0)
        {
        this.selectedphyItems = [];
        this.selectedphyItems.push(this.physiciansdrop[0]);
        this.myform.patchValue({
          physician:this.selectedphyItems,
        });
        this.getFiltersDataBySelection();
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  mouseEnter(Id:any)
  {
    
    this.MyImages = Id;
   
  }
  mouseLeave()
  {
    this.MyImages =null;
 
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  onRowSelect(patientId: any) {
    this.ng4LoadingSpinnerService.show();
    this.selectstyle = 2;
    this.residentId = patientId;
    this.certDate=[];
    this.certifiedOrdersList=[];
    this.getDemographicInfoData(this.residentId);
  }
  getDemographicInfoData(PatientID: number) {
    this.ng4LoadingSpinnerService.show();
    this.residentId = PatientID;
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.demographicInfoData = res;
        this.GetDiagnosisDetails();
        this.getPatientType();
        this.getOrderGridData();
        this.getCertifiedDatesDrop();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetDiagnosisDetails() {
    this.dataservice.get<OrderInfoAlert>(this.config.Emar_Orders_GetDiagnosisDetails + this.residentId)
      .subscribe(res => {

        this.ordersInfo = {
          Allergy: res.Allergy,
          Diet: res.Diet,
          Diagnosis: res.Diagnosis,
        };
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getPatientType() {
    this.dataservice.get<any[]>(this.config.Resident_Demographic_GetPatientType + this.residentId)
      .subscribe(res => {
        this.patientTypeList = res;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  back() {
    this.CheckAll = false;
    this.selectedRecords = [];
    this.selectstyle = 1;
    this.isUserAdmin = false;
    if (this.persistanceService.get('userRole') == '\"SuperAdmin\"') {
      this.isUserAdmin = true;
    }
    // if(this.isUserAdmin==true)
    // {
    //   this.getPhysicianDrop();
    // }
    // else
     this.getFiltersDataBySelection();
  }
  getOrderGridData() {
    this.ng4LoadingSpinnerService.show();
    let phyNpi=this.isUserAdmin==true?this.myform.value.physician!=undefined&&this.myform.value.physician!=null&& this.myform.value.physician!=undefined && this.myform.value.physician!=""? this.myform.value.physician[0].PhysicianNPI:null:null
    this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetCertifyOrderGridData + this.residentId+"/"+phyNpi)
      .subscribe(res => {
        this.ordersList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onCheckAll(event) {
    if (event == true) {
      this.CheckAll = true;
      this.selectedRecords = [];
      this.ordersList.forEach(element => {
        this.orderStatusDCObj = {
          PatientId: this.residentId,
          OrderId: element.porder_Id,
          DAdminId: element.DAdmin_Id,
          QuantityId: element.PQuantity_Id,
          POrderCreatedBy: this.userId,
          POrderStatus: 2,
          POOutBoundApproval: 0,
          POOutBoundApprovalBy: 0,
          OrderType: 'Discontinue',
          DiscontinueFlag: 1,
          DiscontinueReason: "",
          DiscontinueAllSplits: 0,
          Split: 0,
          DiscontinuedOn: this.dateFormatPipe.dateWithTime(new Date()),
        }
        this.selectedRecords.push(this.orderStatusDCObj);
      });
    }
    else {
      this.CheckAll = false;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.orderStatusDCObj = {
        PatientId: this.residentId,
        OrderId: item.porder_Id,
        DAdminId: item.DAdmin_Id,
        QuantityId: item.PQuantity_Id,
        POrderCreatedBy: this.userId,
        POrderStatus: 2,
        POOutBoundApproval: 0,
        POOutBoundApprovalBy: 0,
        OrderType: 'Discontinue',
        DiscontinueFlag: 1,
        DiscontinueReason: "",
        DiscontinueAllSplits: 0,
        Split: 0,
        DiscontinuedOn: this.dateFormatPipe.dateWithTime(new Date()),
      }
      this.selectedRecords.push(this.orderStatusDCObj);;
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.QuantityId == item.PQuantity_Id);
      this.selectedRecords.splice(index, 1);
    }
  }
  discontinueCheckedOrders() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_Orders_DiscontinueCertifyOrder, this.selectedRecords)
      .subscribe(res => {
        if (res == 1) {
          this.alertService.success("Orders discontinued successfully");
          this.CheckAll = false;
          this.getOrderGridData();
          this.getCertifiedDatesDrop();
          this.selectedRecords = [];
          window.scroll(0, 0);
        }
        else if (res == 0) {
          this.alertService.error("Something went wrong");
          this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  openCertifyOrdersModal()
  {
    this.credentialform.controls['certUsername'].reset();
    this.credentialform.controls['certPass'].reset();
    this.credentialform.controls['Profcre'].reset();
    this.credentialform.controls['months'].patchValue(6);
    this.getPhysicianCredentials();
  }
  closeModal()
  {
    this.modalUserCredential=false;
  }
  certifyOrders() {
    let orderIds=this.ordersList.map(o => o.porder_Id).join(',');
    this.certifierObj={
        Cert_UserName: this.credentialform.value.certUsername,
        Cert_Password: this.credentialform.value.certPass,
        CertifyOrders: orderIds,
        Month:this.credentialform.value.months,
        ProfessionalCredentials:this.credentialform.value.Profcre==undefined || this.credentialform.value.Profcre==""||this.credentialform.value.Profcre==null?null:this.credentialform.value.Profcre,
        User_Id: this.persistanceService.get(this.config.loggedInUserKey),
        CertifiedOn: this.dateFormatPipe.dateWithTime(new Date())
     }
     this.dataservice.post(this.config.Emar_Orders_InsertOrdersCertification ,this.certifierObj)
     .subscribe((res: any) => {
       if (res == "Success")
       {
         this.alertService.success("Orders Certified Successfully");
         this.modalUserCredential=false;
         this.getCertifiedDatesDrop();
       }
       else 
       {
        this.alertService.error(res);
       }
       this.ng4LoadingSpinnerService.hide();
     }, error => {
       this.alertService.error(error.message);
       this.ng4LoadingSpinnerService.hide();
     });
  }
  getCertifiedDatesDrop() {
     
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    let fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 11))).setHours(0)).toString().substring(0, 10));
    let toDate = this.dateFormatPipe.transformISODate(new Date());
    let phyNpi=this.isUserAdmin==true?this.myform.value.physician.length!=0 && this.myform.value.physician!=undefined && this.myform.value.physician!=null && this.myform.value.physician!=""?this.myform.value.physician[0].PhysicianNPI:null:null;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetCertifiedDatesDrop + this.userId+"/"+this.residentId+"/"+ fromDate +"/"+toDate+"/"+phyNpi)
      .subscribe(res => {
        this.certifiedDates = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getCertifiedOrdersList() {
    this.ng4LoadingSpinnerService.show();
    let cert_TimeId=this.certDate[0].CertifyTime_ID;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetAllcertifiedOrderByDate + this.userId+"/" +cert_TimeId+"/"+this.residentId)
      .subscribe(res => {
        this.certifiedOrdersList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onDateSelect(item:any)
  {
    this.certifiedOrdersList=[];
    this.getCertifiedOrdersList();
  }
  onDateDeSelect()
  {
    this.certifiedOrdersList=[];
  }
  onPhysicianSelect(item:any)
  {
    this.residentsList=[];
    this.getFiltersDataBySelection();
  }
  onPhysicianDeSelect(item:any)
  {
    this.residentsList=[];
    this.alertService.error("Select physician to display order list.");
    this.myform.patchValue({
      physician:'',
    });
  }
  exportCertifiedOrdersToPdf()
  {
    this.ng4LoadingSpinnerService.show();
    if(this.certDate.length==0)
    {
      this.alertService.warn("Please select last certified date");
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      this.ng4LoadingSpinnerService.show();
      let cert_TimeId=this.certDate[0].CertifyTime_ID;
      let facilityId=this.myform.value.ddlfacilities[0].Facility_Id;
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      this.dataservice.getFile(this.config.Emar_Reports_GetCertifiedOrderReport + this.userId +"/"+ cert_TimeId+"/"+this.residentId +"/"+facilityId+"/"+this.demographicInfoData.NurseStation_Name+"/"+dateTime)
      .subscribe((res) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Certified_Orders" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  getPhysicianCredentials() {
    this.ng4LoadingSpinnerService.show();
    let phyNpi=this.isUserAdmin==true?this.myform.value.physician!=undefined&&this.myform.value.physician!=null && this.myform.value.physician!=""? this.myform.value.physician[0].PhysicianNPI:null:null
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetPhysicianCredentialsByNPI +phyNpi)
      .subscribe(res => {
        if(res!=null)
        {
        this.credentialform.patchValue({
          Profcre:res,
        });
        }
        this.modalUserCredential=true;
        setTimeout(() => {
          this.inputFocus.nativeElement.focus()
        }, 300);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  
}
