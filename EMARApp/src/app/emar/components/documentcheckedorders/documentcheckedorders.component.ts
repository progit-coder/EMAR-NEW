import { Component, OnInit,ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ResidentDemographic, ResidentsCount } from '../../../models/residentdemographic.model';
import { Router } from '@angular/router';
import { Floor, NurseStation, Wing, Bed, Room, FiltersConfig } from '../../../models/facility.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DrugAdminister, MedicationReason } from '../../../models/emar.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { RemovemarmodalComponent } from '../removemarmodal/removemarmodal.component';
//import { DatePipe } from '@angular/common';
import { OrderFavouriteData } from '../../../models/emar.model';
import { DragScrollComponent } from 'ngx-drag-scroll';

@Component({
  selector: 'app-documentcheckedorders',
  templateUrl: './documentcheckedorders.component.html',
  styleUrls: ['./documentcheckedorders.component.css']
})
export class DocumentcheckedordersComponent implements OnInit {

  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent;
  myform: FormGroup;
  public template;
  dropdownSettings_Resident: any = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  public dropdownSettings_Time = {};
  public selectedfaItems = [];
  public selectednItems = [];
  public residents: ResidentDemographic[];
  public facilities: any[];
  public nurseStations: NurseStation[];
  public userId: number;
  public timeDropList: any[];
  public userDrop: any[];
  public dropdownSettings_User: {};
  public selecteduserItem = [];
  public medicationReasonList: MedicationReason[] = [];
  public noShowFlag:boolean=false;
  public Orders: any[] = [];
  public OrderList: any[] = [];
  public OrderAuditList:any=[];
  logoErrorMessage: string = '';
  private companyId: number = 0;
  medicationForm: FormGroup;
  public modalNoShowAdministerIsOpen:boolean=false;
  public undoOrderDetails: any;
  

  public notestatus: number = 0;
  public ShowFilter = true;
  dropdownSettings_MedicationReason = {};
  public drugAdminsterObj: any;
  public inactivecheckbox: boolean = false;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  public RemoveStatus: boolean = true;
  public CheckedStatus: boolean = false;
  public modalAudit:boolean=false;
  CheckAll: boolean;
  public modalUnlockIsOpen: boolean = false;
  public modalRemoveIsOpen: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  public auditTable: any;
  modalOption: NgbModalOptions = {};
  pageConfig = {};
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedFacItems = [];
  public CheckRecord:boolean=true;
  public modalAdministerWithoutScannerVitalsIsOpen:boolean=false;
  favouritesForm: FormGroup;
  public withoutScannerDisplayVitals=[];
  public withoutScannerOrderVitals=[];
  public CheckedQuantityIds =[];
  public drugAdminIds =[];
  public checkedVitalsList:any[]=[];
  public vitalsave:any[]=[];
  public WsDiscardDate:FormGroup;
  public wsDiscard:number=1;
  public ordersGrid:number=1;
  public WsPRN:FormGroup;
  public modalAdministerInsulinSiteIsOpen:boolean=false;
  public insulinSiteIdsWS=[];
  public porderIdForInsulin:any;
  public routeForInsulin:any;
  public wsOrderInsulinSites=[];
  public displayInsuliSitesForWS=[];
  public lastUsedSiteWS:string="";
  public lastUsedSites:string="";
  public sitesType:string="";
  public withoutScannerTempVitals=[];
  public vitalsCheckList: any[] = [];
  public CheckVitalsObj: OrderFavouriteData[] = [];
  public vitalsStatus: number = 0;
  public timeFormatId:number=0;
  public checkAllDisplay:number=0;
  public cllick:any;
  public nursingStationZoneCurrentDate:any;
  public allInsulinSites = [
    { item_id: 1, item_text: 'Thigh, Left (Quadricep)',item_type:1 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 2, item_text: 'Thigh, Right (Quadricep)' ,item_type:1,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 3, item_text: 'Arm, Left (Deltoid)' ,item_type:1 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 4, item_text: 'Arm, Right (Deltoid)' ,item_type:1,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 5, item_text: 'Abdomen, RUQ' ,item_type:1,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 6, item_text: 'Abdomen, RLQ',item_type:1 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 7, item_text: 'Abdomen, LUQ' ,item_type:1 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 8, item_text: 'Abdomen, LLQ' ,item_type:1 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 9, item_text: 'Buttocks, Left (Gluteus)' ,item_type:1,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 10, item_text: 'Buttocks, Right (Gluteus)' ,item_type:1,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 11, item_text: 'Chest, Left',item_type:2 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 12, item_text: 'Chest, Right' ,item_type:2,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 13, item_text: 'Back, Left' ,item_type:2 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 14, item_text: 'Back, Right' ,item_type:2,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 15, item_text: 'Arm, Left' ,item_type:2 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 16, item_text: 'Arm, Right',item_type:2 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 17, item_text: 'Ear, behind Left' ,item_type:2,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
    { item_id: 18, item_text: 'Ear, behind Right' ,item_type:2 ,lastUsed:0,lastUsedDate:"",IsChacked:false,IsDisabled:false},
  ];

  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private route: Router, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private modalService: NgbModal,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DocumentAdministeredOrders");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.template = this.dataservice.template;
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
          ddlnursestations: new FormControl(''),
          ddlresidents: new FormControl(''),
          dateCheck: new FormControl(new Date().toISOString().substring(0, 10), Validators.required),
          user: new FormControl('', Validators.required),
          nurseSheduleTime: new FormControl('', Validators.required),
          AdminDate: new FormControl('', Validators.required)
        });
        this.favouritesForm = new FormGroup({
          comments: new FormControl('', [Validators.required, Validators.maxLength(50)]),
        });
        this.medicationForm = new FormGroup({
          medicationReason: new FormControl('0', Validators.required),
          note: new FormControl('', Validators.maxLength(50)),
          undoQuantity: new FormControl('0', Validators.required)
        });
        this.WsDiscardDate =new FormGroup({
          WSDiscard:new FormControl('',)
          });
          this.WsPRN =new FormGroup({
            WSPrnReason:new FormControl('',)
            })
        this.dropdownSettings_Resident = {
          singleSelection: true,
          idField: "Patient_Id",
          textField: "PatientName",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please select Nursing Station',
        };
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
          singleSelection: true,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please select Facility',
        };
        this.dropdownSettings_MedicationReason = {
          singleSelection: true,
          idField: "MedicationReason_ID",
          textField: "MedicationReason_Desc",
          text: "Medication Reason",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        };
        this.dropdownSettings_Time = {
          //singleSelection: true,
          idField: 'ScheduleTime',
          textField: 'NurseShiftTime',
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'No orders due',
        }
        this.dropdownSettings_User = {
          singleSelection: true,
          idField: "User_Id",
          textField: "User_DisplayName",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
        //this.getFiltersData(this.userId);
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.DocumentAdministeredOrders, Activity.View, '')
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
  getMedicationReason() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<MedicationReason[]>(this.config.Emar_GetMedicationReason)
      .subscribe(res => {
        this.medicationReasonList = res;
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  onMedicationReasonSelect(item: any) {
    if ((item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators([Validators.required, Validators.maxLength(50)]);
      notevalidation.updateValueAndValidity();
      this.notestatus = 1;
    }
    else {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
  }
  onMedicationReasonDeSelect(item: any) {
    if ( (item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
    this.medicationForm.patchValue({
      note: ''
    });
  }
  openNoShowAdministerModal(i: number, item: any)
  {

if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
{

  this.alertService.warn("Please select Administer By & Date/Time");
  this.ng4LoadingSpinnerService.hide();
  return false;
 
}
else if(( item.ControlSubstanceBit==1 && (item.ControlSubstanceCertifiedBy==null))|| (item.ControlSubstanceBit==2))
{
 
  this.alertService.warn("Controlled substance count certification required prior to administration");
  this.ng4LoadingSpinnerService.hide();
  return false;
}
else
{


this.getMedicationReason();
    this.medicationForm.reset();
    const undoQuantityValidations = this.medicationForm.get('undoQuantity');
    undoQuantityValidations.clearValidators();
    undoQuantityValidations.updateValueAndValidity();
    this.medicationForm.patchValue({
      undoQuantity: '0'
    });
  this.undoOrderDetails = item;
    this.noShowFlag=true;
   
 
    this.modalNoShowAdministerIsOpen = true;
    this.ng4LoadingSpinnerService.hide();
  }
  }
  orderGivenStatus1()
  {
       
  //  this.modalNoShowAdministerIsOpen = true;
  }
  orderGivenStatus(item)
  {
       
    this.ng4LoadingSpinnerService.show();
    //let list=this.OrderList.filter(r=>r.Type!=0 && r.PQuantity_Id==parseInt(item.PQuantity_Id));
   
    this.drugAdminsterObj = {
      DrugAdminister_Id: item.DrugAdminister_Id,
      POrder_Id: item.Porder_Id,
      pquantity_Id: item.PQuantity_Id,
      AdminsterSchedule:item.AdminsterSchedule == null ? null : item.AdminsterSchedule.split('/').join('-'),//this.dateFormatPipe.transform(this.myform.value.dateCheck),
      AdministerComment: this.medicationForm.value.note,
      AdminsterStatus: 0,
      AdminsterBy:  this.selecteduserItem[0].User_Id,
      AdminsterOn: this.myform.value.dateCheck.split('/').join('-'),
      MedicationReason_ID: this.medicationForm.value.medicationReason[0].MedicationReason_ID,
      BCScanner: null,
      BCScannerText: null,
      Ekit_Id: null,
      Quantity: item.Quantity,
      Patient_Id: this.myform.value.ddlresidents[0].Patient_Id,
      DrugQuantity: 0,
      ByPassReason: null,
      InputTime: this.myform.value.AdminDate,
      ShiftId: null,
      Window: 0,
      PRNFlag: item.PRNFlag,
      UndoFlag:true,
      Last_Passed:null,
      AdditionalComments:'',
      AdministeredBarcode:'',
      DiscardDate:null,
      AdministerInsulinSites:null,
      RouteCode:true,
      Reason:item.Reason ,
      ManualDocumentedBy: this.userId,
      ManualDocumentedDate: this.dateFormatPipe.dateWithTime(new Date()),
    };
          
    if (this.drugAdminsterObj != null) {
      this.dataservice.post(this.config.Emar_Common_NoAdminitration, this.drugAdminsterObj)
        .subscribe(res => {
            
        
         //// this.getEmarOrdersList();
          //this.getEkitDrop(this.nurseStationvalue);
          //this.getEmarResidentGridData();
          // this.persistanceService.getDueMARAlert();
          // this.listChage=0;
          if (res==1 )
          {
            this.alertService.success('Order administration successful');
          }
          this.modalNoShowAdministerIsOpen = false;
          this.ng4LoadingSpinnerService.hide();
        this.medicationForm.reset();
        this.drugAdminsterObj = [];
    
       this. getOrders();
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
    }




     
   // this.modalNoShowAdministerIsOpen = true;
  //  this.drugAdminsterObj = {
  //   //DrugAdminister_Id: drugAdministor_Id,
  //   POrder_Id: orderId,
  //   pquantity_Id: quantityId,
  //   AdminsterSchedule: rightAdministerPassTime,//this.dateFormatPipe.transform(this.myform.value.dateCheck),
  //   AdministerComment: this.medicationForm.value.note,
  //   AdminsterStatus: 0,
  //   AdminsterBy: this.userId,
  //   AdminsterOn: this.dateFormatPipe.dateWithTime(new Date()),
  //   MedicationReason_ID: this.medicationForm.value.medicationReason[0].MedicationReason_ID,
  //   BCScanner: this.barcodeScanCheckbox == true ? 1 : 0,
  //   BCScannerText: this.withoutAdminsterForm.value.reasonForWithoutAdministred,
  //   Ekit_Id: this.ekit == null || this.ekit == undefined || this.ekit.length == 0 ? 0 : this.ekit[0].Ekit_Id,
  //   Quantity: this.quantity,
  //   Patient_Id: this.residentId,
  //   DrugQuantity: this.undoFlag == true && this.medicationForm.value.undoQuantity != '' ? this.medicationForm.value.undoQuantity : 0,
  //   ByPassReason: this.byPassReason,
  //   InputTime: inputTime,
  //   ShiftId: shiftId,
  //   Window: window,
  //   PRNFlag: PRNFlag,
  //   UndoFlag:this.undoFlag,
  //   Last_Passed:this.ordersList.find(o=>o.POrder_Id==orderId && o.pquantity_Id==quantityId).Last_Passed!=null?this.ordersList.find(o=>o.POrder_Id==orderId && o.pquantity_Id==quantityId).Last_Passed.split('/').join('-'):null,
  //   AdditionalComments:this.additionalComForm.value.addCom,
  //   AdministeredBarcode:this.barcodeScanCheckbox == false?this.barcodevalue:barcodesList,
  //   DiscardDate:this.undoFlag==true?null:(this.discardform.value.discardDate!=undefined && this.discardform.value.discardDate!=null && this.discardform.value.discardDate!=""?this.dateFormatPipe.dateFormat(this.discardform.value.discardDate):null),
  //   AdministerInsulinSites:this.undoFlag==true?null:this.insulinSites.join(','),
  //   RouteCode:this.undoFlag==true?null:this.ordersList.find(o=>o.POrder_Id==orderId).Route,
  // };
  }
  closeNoShowModel()
  {
   // this.undoOrderDetails = null;
   // this.undoFlag = false;
    this.noShowFlag=false;
    this.modalNoShowAdministerIsOpen = false;
  }
  getDemographicInfoByNurseStation(stationId: number) {
    this.dataservice.get<ResidentDemographic[]>(this.config.Emar_ResidentDemographic_GetResidentsListByNSId + stationId)
      .subscribe(res => {
        this.residents = res;
        this.getNursingStationTimeZone(stationId);
      },
        error => {
          this.alertService.error(error)
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNursingStationTimeZone(stationId: number) {
    this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
      .subscribe(res => {
        
        this.nursingStationZoneCurrentDate=res;
        this.myform.patchValue({
          dateCheck:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
        });
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
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    ///let backClick = JSON.parse(localStorage.getItem("BackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
              if(this.selectednItems.length==1)
              break;
            }
            if (this.selectednItems.length != 0) {
            this.myform.patchValue({
              nurseStations: this.selectednItems,
            });
            
            this.getDemographicInfoByNurseStation(this.selectednItems[0].NurseStation_Id);
            this.getUsers(this.selectednItems[0].NurseStation_Id);
          }
          }
        }
          else if (this.facilities.length == 1) {
            this.selectednItems.push(this.nurseStations[0]);
            this.myform.patchValue({
              nurseStations: this.selectednItems,
            });
            this.getDemographicInfoByNurseStation(this.selectednItems[0].NurseStation_Id);
            this.getUsers(this.selectednItems[0].NurseStation_Id);
          }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.timeDropList = [];
    this.userDrop = [];
    this.OrderList=[];
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
      nurseSheduleTime: '',
      user:'',
    });
    this.RemoveStatus=true;
    this.checkAllDisplay=0;
    this.getNurseStationByFacilityID(item.Facility_Id);
  }
  onNurseStationSelect(item: any) {
    this.residents = [];
    this.timeDropList = [];
    this.userDrop = [];
    this.OrderList=[];
    this.myform.patchValue({
      ddlresidents: '',
      nurseSheduleTime: ''
    });
    this.RemoveStatus=true;
    this.checkAllDisplay=0;
    this.getDemographicInfoByNurseStation(item.NurseStation_Id);
    // this.getNursingScheduleData();
    this.getUsers(item.NurseStation_Id);
  }
  getNursingScheduleData() {
    let scheduleDate = this.myform.value.dateCheck.split('/').join('-');
    let pId=this.myform.value.ddlresidents[0].Patient_Id;
    let nsId=this.myform.value.ddlnursestations[0].NurseStation_Id;
    this.dataservice.get<any[]>(this.config.Emar_Emar_GetNursingScheduleDataByPatientID + pId + "/" + nsId + "/" + scheduleDate)
      .subscribe(res => {
        
        if (res == null) {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.warn('No orders due');
        }
        else {
             
          this.timeDropList = res;
        }
        //this.RemoveStatus=true;
        //this.CheckedStatus = true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.timeDropList = [];
    this.userDrop = [];
    this.OrderList=[];
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
      user: '',
      nurseSheduleTime: ''
    });
    this.RemoveStatus=true;
    this.checkAllDisplay=0;
  }

  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.timeDropList = [];
    this.userDrop = [];
    this.OrderList=[];
    this.myform.patchValue({
      ddlresidents: '',
      user: '',
      nurseSheduleTime: ''
    });
    this.RemoveStatus=true;
    this.checkAllDisplay=0;
  }
  getUsers(nS_Id: number) {
    this.dataservice.get<any[]>(this.config.User_GetUsersByNSID + nS_Id)
      .subscribe(res => {
        this.userDrop = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

  resetScreen() {
    this.myform.patchValue({
      AdminDate:'',
      user:'',
      nurseSheduleTime:'',
      ddlresidents: '',
      dateCheck: new Date().toISOString().substring(0, 10),
    });    
    this.OrderList=[];
    this.checkAllDisplay=0;
    this.selectedRecords=[];
    this.RemoveStatus=true;
    this.CheckAll=false;
  }
getOrders() {
  
    this.ng4LoadingSpinnerService.show();
    this.CheckRecord=true;
    let date = this.myform.value.dateCheck.split('/').join('-');
    if(this.myform.value.nurseSheduleTime!=undefined && this.myform.value.nurseSheduleTime!=null && this.myform.value.nurseSheduleTime)
    {
    let times=[];
    let shifts=[];
    this.myform.value.nurseSheduleTime.forEach(element => {
      if(element.ScheduleTime.startsWith('s'))
      {
        shifts.push(parseInt(element.ScheduleTime.split('s')[1]));
      }
      else{
        times.push(element.NurseShiftTime.replace(':', '-'));
      }
    });
    if(times.length!=0 || shifts.length!=0)
    {
    //let time = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime.length == 0 || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined ? null :
    //this.myform.value.nurseSheduleTime[0].ScheduleTime.startsWith('s')?null :Array.prototype.map.call(this.myform.value.nurseSheduleTime, s => s.NurseShiftTime).toString().replace(':', '-').split(",");
    //  let time1 = this.myform.value.nurseSheduleTime == "" || this.myform.value.nurseSheduleTime.length == 0 || this.myform.value.nurseSheduleTime == null || this.myform.value.nurseSheduleTime == undefined ? null :this.myform.value.nurseSheduleTime[0].ScheduleTime.startsWith('s')?null :this.myform.value.nurseSheduleTime[0].NurseShiftTime.replace(':', '-');
    // let time = this.myform.value.nurseSheduleTime.length == 0?null:Array.prototype.map.call(this.myform.value.nurseSheduleTime, s => s.NurseShiftTime).toString().replace(':', '-').split(",");
    //let shiftId= null;
    //this.myform.value.nurseSheduleTime[0].ScheduleTime.startsWith('s')?parseInt(this.myform.value.nurseSheduleTime[0].ScheduleTime.split('s')[1]):null;
    let obj={
      residentId:this.myform.value.ddlresidents[0].Patient_Id,
      shiftId:shifts.length>0?shifts.join(','):null,
      dateValue:date,
      time:times.length>0?times:null
    }
    //this.dataservice.get<any[]>(this.config.Emar_Common_GetOrders + this.myform.value.ddlresidents[0].Patient_Id +"/"+ shiftId + "/" + date + "/" + time )
    this.dataservice.post(this.config.Emar_Common_GetOrders,obj)
      .subscribe(res => {
        if(res.length==0)
        {
          this.alertService.warn("No data available");
          this.OrderList=[];
          this.selectedRecords=[];
          this.CheckAll=false;
          this.withoutScannerDisplayVitals=[];
          this.checkAllDisplay=0;
          this.ng4LoadingSpinnerService.hide();
        }
        else
        {
          console.log("Grid Binding");
          console.log(res);
        this.OrderList = res;
        this.ng4LoadingSpinnerService.hide();
        this.checkAllDisplay=res.filter(r=>r.Type!=0).length==0?1:0;
        this.getAllFlagsForCompanyByNSId();
        let QuantityIds =[];
        this.ordersGrid=1;
        this.insulinSiteIdsWS=[];
        this.withoutScannerDisplayVitals=[];
        this.favouritesForm.reset();
        this.CheckVitalsObj=[];
        this.withoutScannerVitals();
        // this.OrderList.forEach((element, index) => {
        //   QuantityIds.push(element.PQuantity_Id);
        //   this.drugAdminIds.push(element.DrugAdminister_Id);
        // });
        // this.GetVitalsDataByIds(QuantityIds.toString());
        // if(this.OrderList.find(element=>element.Type==0)==undefined)
        // {
        // this.CheckRecord=false;
        // }
        this.selectedRecords=[];
        this.CheckAll=false;
      }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
    else
    {
      this.alertService.error("Please select pass time");
      this.ng4LoadingSpinnerService.hide();
    }
    }
    else
    {
      this.alertService.error("Please select pass time");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  getOrdersAll() {
    
      this.ng4LoadingSpinnerService.show();
      this.CheckRecord=true;
      let date = this.myform.value.dateCheck.split('/').join('-');
      let time = null;
      let shiftId= this.myform.value.nurseSheduleTime[0].ScheduleTime.startsWith('s')?parseInt(this.myform.value.nurseSheduleTime[0].ScheduleTime.split('s')[1]):null;
      let obj={
        residentId:this.myform.value.ddlresidents[0].Patient_Id,
        shiftId:shiftId,
        dateValue:date,
        time:time
      }
      //this.dataservice.get<any[]>(this.config.Emar_Common_GetOrders + this.myform.value.ddlresidents[0].Patient_Id +"/"+ shiftId + "/" + date + "/" + time )
      this.dataservice.post(this.config.Emar_Common_GetOrders,obj)
        .subscribe(res => {
          
          if(res.length==0)
          {
            this.alertService.warn("No data available.");
            this.OrderList=[];
            this.selectedRecords=[];
            this.CheckAll=false;
            this.withoutScannerDisplayVitals=[];
            this.checkAllDisplay=0;
            this.ng4LoadingSpinnerService.hide();
          }
          else
          {
          this.OrderList = res;
          this.checkAllDisplay=res.filter(r=>r.Type!=0).length==0?1:0;
          this.getAllFlagsForCompanyByNSId();
          let QuantityIds =[];
          this.ordersGrid=1;
          this.insulinSiteIdsWS=[];
          this.withoutScannerDisplayVitals=[];
          this.favouritesForm.reset();
          this.CheckVitalsObj=[];
          this.withoutScannerVitals();
          // this.OrderList.forEach((element, index) => {
          //   QuantityIds.push(element.PQuantity_Id);
          //   this.drugAdminIds.push(element.DrugAdminister_Id);
          // });
          // this.GetVitalsDataByIds(QuantityIds.toString());
          // if(this.OrderList.find(element=>element.Type==0)==undefined)
          // {
          // this.CheckRecord=false;
          // }
          this.selectedRecords=[];
          this.CheckAll=false;
        }
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  insertDAO() {
    this.ng4LoadingSpinnerService.show();
    this.logoErrorMessage = '';
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select order(s) to document");
      //this.RemoveStatus=   true;
      this.UpdateStatus =true;
      
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      debugger;
      this.dataservice.post(this.config.Emar_Common_InsertDAO, this.selectedRecords)

        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res == 1) {
            this.alertService.success("Save successful");
            // this.getOrders();
            this.RemoveStatus=false;
            this.CheckedStatus = true;
            //this.resetScreen();
            this.selectedRecords=[];
            // this.inactivecheckbox = false;
            this.getOrders();
            this.WsPRN.reset();
            this.getDosesDetails(0,null);
          }
          else if (res == 2)
            this.alertService.error("Order administered");
          else
            this.alertService.error("System error, please try again");

        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  onCheckAll(event) {
          debugger
    let checkControlledMed=this.OrderList.length>1?this.OrderList.find(i=>i.ControlSubstanceBit==1)==undefined?true:false:true;
    if(checkControlledMed==true && event==true)
    {
      if(this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate))
    {
      this.alertService.warn("Future orders cannot be administered");
      let ordercheckBoxId = "#chkselectall";
      $(ordercheckBoxId).prop("checked",false);
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
      {
        this.CheckAll=false;
        let ordercheckBoxId = "#chkselectall";
        $(ordercheckBoxId).prop("checked",false);
        this.alertService.warn("Please select Administer By & Date/Time");
        this.ng4LoadingSpinnerService.hide();
       
      }
     // ||(this.OrderList[0].ControlSubstanceCertifiedBy!=null&&this.OrderList[0].ControlSubstanceCertifiedBy.toString().split(',').includes(this.selecteduserItem[0].User_Id.toString())==false) 
    else if((checkControlledMed==true && this.OrderList.length==1 && this.OrderList[0].ControlSubstanceBit==1 && (this.OrderList[0].ControlSubstanceCertifiedBy==null)) || (checkControlledMed==true && this.OrderList[0].ControlSubstanceBit==2))
    {
      this.CheckAll = false;
      let ordercheckBoxId = "#chkselectall";
      $(ordercheckBoxId).prop("checked",false);
      this.selectedRecords = [];
      this.alertService.warn("Controlled substance count certification required prior to administration");
    }
    else
    {
      debugger;
    let date = this.myform.value.dateCheck.split('/').join('-');
    let time = this.myform.value.AdminDate == "" || this.myform.value.AdminDate.length == 0 || this.myform.value.AdminDate == null || this.myform.value.AdminDate == undefined ? null : this.myform.value.AdminDate;
    let dateWithTimeon = date + " " + time ;
    if (event == true) {
    this.selectedRecords = [];
      this.CheckAll = true;
      this.OrderList.forEach(element => {
        if(element.Type==0)
        {
        this.drugAdminsterObj = {
          DrugAdminister_Id: element.DrugAdminister_Id,
          POrder_Id: element.Porder_Id,
          pquantity_Id: element.PQuantity_Id,
          DrugName:element.Order,
          AdminsterSchedule: null,
          AdministerComment: element.PRNFlag==true?this.getPrnReason(element.DrugAdminister_Id):null,
          AdminsterStatus: null,
          AdminsterBy: this.selecteduserItem[0].User_Id,
          AdminsterOnDate:date,
          AdminsterOnTime:time,
          MedicationReason_ID: 1,
          BCScanner: null,
          BCScannerText: null,
          Ekit_Id: null,
          Quantity: null,
          Patient_Id: null,
          DrugQuantity: null,
          ByPassReason: null,
          ManualDocumentedBy: this.userId,
          ManualDocumentedDate: this.dateFormatPipe.dateWithTime(new Date()),
          PRNFlag:element.PRNFlag,
          Route:element.Route,
          DiscardDate:this.getDiscardDateForAdminister(element.DrugAdminister_Id)!=undefined && this.getDiscardDateForAdminister(element.DrugAdminister_Id)!=null && this.getDiscardDateForAdminister(element.DrugAdminister_Id)!=""?this.dateFormatPipe.dateFormat(this.getDiscardDateForAdminister(element.DrugAdminister_Id)):null,
          AdministerInsulinSites:this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==element.DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==element.DrugAdminister_Id).SiteIds:null,
          RouteCode:this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==element.DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==element.DrugAdminister_Id).RouteCode:null,
        };
        this.selectedRecords.push(this.drugAdminsterObj);
      }
      });
    }
  }
} 
    }
  else if( checkControlledMed!=true && event==true)
    {
      if(this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate))
      {
        this.alertService.warn("Future orders cannot be administered");
        let ordercheckBoxId = "#chkselectall";
        $(ordercheckBoxId).prop("checked",false);
        this.ng4LoadingSpinnerService.hide();
      }
      else
      {
      this.CheckAll = false;
      this.selectedRecords = [];
      let ordercheckBoxId = "#chkselectall";
      $(ordercheckBoxId).prop("checked",false);
      this.alertService.warn("Controlled and non-controlled meds cannot be documented at the same time");
      }
    }
  else if(event==false) {
      this.CheckAll = false;
      this.selectedRecords = [];
  }
  }
  onselectRecord(event, item: any) {
    
    if(this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate))
      {
        this.alertService.warn("Future orders cannot be administered");
        let ordercheckBoxId = "#"+item.DrugAdminister_Id;
        $(ordercheckBoxId).prop("checked",false);
        this.ng4LoadingSpinnerService.hide();
      }
    else
    { 
    if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
    {
      //this.getOrders();
      this.CheckAll=false;
      const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
      if(index>=0)
      {
      this.selectedRecords.splice(index, 1);
      }
      let ordercheckBoxId = "#" + item.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      this.alertService.warn("Please select Administer By & Date/Time");
      this.ng4LoadingSpinnerService.hide();
      
    }
    // && item.ControlSubstanceCertifiedBy.toString().split(',').includes(this.selecteduserItem[0].User_Id.toString())==true
    else if ((event == true && item.ControlSubstanceBit==0) || (event == true && item.ControlSubstanceBit==1 && item.ControlSubstanceCertifiedBy!=null)) {
    //else if(event == true){ 
    let date = this.myform.value.dateCheck.split('/').join('-');
      let time = this.myform.value.AdminDate == "" || this.myform.value.AdminDate.length == 0 || this.myform.value.AdminDate == null || this.myform.value.AdminDate == undefined ? null : this.myform.value.AdminDate;
      var dateWithTimeon = date + " " + time ;
      this.drugAdminsterObj = {
        DrugAdminister_Id: item.DrugAdminister_Id,
        POrder_Id: item.Porder_Id,
        pquantity_Id: item.PQuantity_Id,
        DrugName:item.Order,
        AdminsterSchedule: null,
        AdministerComment: item.PRNFlag==true?this.getPrnReason(item.DrugAdminister_Id):null,
        AdminsterStatus: null,
        AdminsterBy: this.selecteduserItem[0].User_Id,
        AdminsterOnDate:date,
        AdminsterOnTime:time,
       // AdminsterOn: this.dateFormatPipe.dateWithTimeFormat(dateWithTimeon),
       //AdminsterOn: Administerdate,
        MedicationReason_ID: 1,
        BCScanner: null,
        BCScannerText: null,
        Ekit_Id: null,
        Quantity: null,
        Patient_Id: null,
        DrugQuantity: null,
        ByPassReason: null,
        ManualDocumentedBy: this.userId,
        ManualDocumentedDate: this.dateFormatPipe.dateWithTime(new Date()),
        Route:item.Route,
        PRNFlag:item.PRNFlag,
        DiscardDate:this.getDiscardDateForAdminister(item.DrugAdminister_Id)!=undefined && this.getDiscardDateForAdminister(item.DrugAdminister_Id)!=null && this.getDiscardDateForAdminister(item.DrugAdminister_Id)!=""?this.dateFormatPipe.dateFormat(this.getDiscardDateForAdminister(item.DrugAdminister_Id)):null,
        AdministerInsulinSites:this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==item.DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==item.DrugAdminister_Id).SiteIds:null,
        RouteCode:this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==item.DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==item.DrugAdminister_Id).RouteCode:null,
      };
      this.selectedRecords.push(this.drugAdminsterObj);
      // let QuantityIds =[];
      // this.selectedRecords.forEach((element, index) => {
      //   QuantityIds.push(element.pquantity_Id);
      //   this.drugAdminIds.push(element.DrugAdminister_Id);
      // });
      // this.GetVitalsDataByIds(QuantityIds.toString());
    }
    //|| || (item.ControlSubstanceCertifiedBy!=null&&item.ControlSubstanceCertifiedBy.toString().split(',').includes(this.selecteduserItem[0].User_Id.toString())==false)
    else if((event == true && item.ControlSubstanceBit==1 && (item.ControlSubstanceCertifiedBy==null))|| (event==true && item.ControlSubstanceBit==2))
    {
      this.alertService.warn("Controlled substance count certification required prior to administration");
      this.ng4LoadingSpinnerService.hide();
      //this.getOrders();
      const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
      if(index>=0)
      {
      this.selectedRecords.splice(index, 1);
      }
      let ordercheckBoxId = "#" + item.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
    }
    else if(event==false) {
      const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
      this.selectedRecords.splice(index, 1);
      // let QuantityIds =[];
      // this.selectedRecords.forEach((element, index) => {
      //   QuantityIds.push(element.pquantity_Id);
      // });
      // this.GetVitalsDataByIds(QuantityIds.toString());
      // if (this.selectedRecords.length == 0) {
      // }
      // else {
      // }
    }
    }
  }
  onTimeSelect(item: any) {
    
    this.selectedRecords=[];
    this.CheckAll=false;
    this.OrderList=[];
    this.getOrders();
  }
  onTimeSelectAll(item: any) {
    debugger;
    this.myform.patchValue({
      nurseSheduleTime:item
    });
    //.nurseSheduleTime
    this.selectedRecords=[];
    this.CheckAll=false;
    this.OrderList=[];
    this.getOrders();
    //this.getOrdersAll();
  }
  onDateChange() {
  //   if(this.myform.value.dateCheck !="")
  //   {
  //   this.getOrders();
  // }
  // else
  // {
  //   this.OrderList=[];
  // }
  this.OrderList=[];
  this.selectedRecords=[];
  this.CheckAll=false;
  this.timeDropList=[];
  this.checkAllDisplay=0;
  this.myform.patchValue({
    nurseSheduleTime:''
  });
  if(this.myform.value.dateCheck !="" && (this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents.length!=0))
  {
    this.RemoveStatus=false;
  this.getNursingScheduleData();
  }
}
  onTimeDeSelect(item:any)
  {
    
    this.selectedRecords=[];
    this.CheckAll=false;
    this.OrderList=[];
    this.checkAllDisplay=0;
    if(this.myform.value.nurseSheduleTime != undefined && this.myform.value.nurseSheduleTime != null && this.myform.value.nurseSheduleTime.length != 0 && this.myform.value.nurseSheduleTime != "" )
    {
      this.getOrders();
    }
  }
  onTimeDeSelectAll(item:any)
  {
    this.selectedRecords=[];
    this.CheckAll=false;
    this.OrderList=[];
    this.checkAllDisplay=0;
    this.myform.patchValue({
      nurseSheduleTime:[]
    });
  }
  closeUnlockModel() {
    this.modalUnlockIsOpen = false;
  }
  closeRemoveModel() {
    this.modalRemoveIsOpen = false;
  }
  modalUnlockopen() {
    
    let prnCommentsCheck=this.selectedRecords.filter(pc=>pc.PRNFlag==true);
    let insulinSitesRequireOrder=this.selectedRecords.filter(o=>o.Route=="SC" || o.Route=="IM" || o.Route=="TD")
    if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
    {
      this.CheckAll=false;
      this.alertService.warn("Please select Administer By & Date/Time");
      this.ng4LoadingSpinnerService.hide();
     
    }
    else 
    {
      let checkFlag=0;
    for(let k=0;k<prnCommentsCheck.length;k++)
    {
      let prnText="#PRN"+prnCommentsCheck[k].DrugAdminister_Id;
      let prnReason= $(prnText).val();
      if(prnReason!=null && prnReason!="")
      checkFlag=0;
      else
      {
      checkFlag=1;
      break;
      }
    }
    if(checkFlag==0 && insulinSitesRequireOrder.length==this.insulinSiteIdsWS.length)
    {
      this.modalUnlockIsOpen = true;
    }
    else if(insulinSitesRequireOrder.length!=this.insulinSiteIdsWS.length)
    {
      this.alertService.warn("Administration site required");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(checkFlag!=0){
      this.alertService.warn("Reason for administration required on PRN orders");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  }
  modalRemoveopen() {
    let id=this.myform.value.ddlresidents[0].Patient_Id;
    this.modalRemoveIsOpen = true;
    // this.RemoveStatus=false;
    // this.UpdateStatus =false;
    this.modalOption.size = 'lg';
    const modalRef = this.modalService.open(RemovemarmodalComponent, this.modalOption);
    modalRef.componentInstance.selectedResident = this.myform.value.ddlresidents[0].Patient_Id;
    modalRef.componentInstance.selectedNsId = this.myform.value.ddlnursestations[0].NurseStation_Id;
    // modalRef.componentInstance.selectedDrugs = this.selectedRecords;
    modalRef.componentInstance.RemovedResult.subscribe((receivedResult) => {
      if (receivedResult == 1)
      {
        this.alertService.success("Removed successfully");
       this.getOrders();
       this.CheckedStatus=false;
       this.getDosesDetails(0,null);
      }
      modalRef.close();
    });

  }
  unlockUser() {
    this.insertDAO();
    this.modalUnlockIsOpen = false;
  }
  RemoveEntry() {
    this.modalRemoveIsOpen = false;
  }
  getHistoryById(DrugAdminister_Id: number) {
    this.auditTable = {
      "tableName": "DrugAdminister",
      "recordId": DrugAdminister_Id
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  onResidentSelect(item:any)
  {
    this.OrderList=[];
    this.timeDropList=[];
    this.RemoveStatus=false;
    this.myform.patchValue({
      nurseSheduleTime:''
    })
    this.getNursingScheduleData();
    this.checkAllDisplay=0;
    this.selectedRecords=[];
    this.CheckAll=false;
  }
  onResidentDeSelect(item:any)
  {
    this.myform.patchValue({
      AdminDate:'',
      user:'',
      nurseSheduleTime:''
    });
    this.RemoveStatus=true;
    this.CheckedStatus = true;
    this.OrderList=[];
    this.timeDropList=[];
    this.checkAllDisplay=0;
    this.selectedRecords=[];
    this.CheckAll=false;
  }
  RefreshGrid()
  {
    if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
    {
      this.CheckAll=false;
      this.alertService.warn("Please select Administer By & Date/Time");
      this.ng4LoadingSpinnerService.hide();
     
    }
    else
    {
      this.OrderList=[];
    this.getOrders();
    }
  }
  GetOrderAuditData(DrugAdministerID)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Common_GetDocAdminOrderAudit + DrugAdministerID)
      .subscribe(res => {
        this.OrderAuditList = res;
        this.modalAudit=true;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  closeAudit()
  {
    this.OrderAuditList=[];
    this.modalAudit=false;
  }



  userInputsOpen()
{
  
    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered");
      this.modalAdministerWithoutScannerVitalsIsOpen=false;
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
  if(this.CheckVitalsObj.length==0)
  {
  this.modalAdministerWithoutScannerVitalsIsOpen=true;
  this.favouritesForm.reset();
  }
  else{
    this.modalAdministerWithoutScannerVitalsIsOpen=true;
    //this.favouritesForm.reset();
  }
  }
}

RowClickwithoutScannerVitals(item:any)
{

  debugger;  
  this.withoutScannerTempVitals=[];
 // let records= item.split(',');

  
  var vitalCheckRecordsrc=this.OrderList.filter(r=>r.Type==0 && r.DrugAdminister_Id == item.DrugAdminister_Id);
    if((this.myform.value.user.length==0 || this.myform.value.user==null ||this.myform.value.user==undefined)||(this.myform.value.dateCheck==""||this.myform.value.dateCheck==null)||(this.myform.value.AdminDate==""||this.myform.value.AdminDate==null)||(this.myform.value.nurseSheduleTime==""||this.myform.value.nurseSheduleTime==null))
    {
      //this.getOrders();
      this.CheckAll=false;
      const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
      if(index>=0)
      {
      this.selectedRecords.splice(index, 1);
      }
      let ordercheckBoxId = "#" + item.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      this.alertService.warn("Please select Administer By & Date/Time");
      this.ng4LoadingSpinnerService.hide();
      
    }
    else{
  if(vitalCheckRecordsrc.length>0)
  {
    vitalCheckRecordsrc.forEach((element, index) => {
      //let checkBoxId = "#" + element.DrugAdminister_Id;
     // $(checkBoxId).prop("checked",false);
      this.withoutScannerOrderVitals=[];
      this.WsPRN.reset();
      this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + item.PQuantity_Id)
      .subscribe(res => {
        if(res.length>0)
        {
          let vitalIds=[];
          let vitalDescs=[];
          res.forEach(item => vitalIds.push(item.OrderFavMaster_ID));
         res.forEach(item => vitalDescs.push(item.OrderFavDesc));
          let obj=
          {
            orderId:element.Porder_Id,
            qtyId:element.PQuantity_Id,
            Vitals:vitalIds.join(","),
            DadminId:element.DrugAdminister_Id,
            Drug:element.Order +" ("+ vitalDescs.join(", ")+")",
          }
       this.withoutScannerTempVitals.push(res);
        if(this.withoutScannerOrderVitals.length==0 || (this.withoutScannerOrderVitals.find(w=>w.qtyId==element.PQuantity_Id)==undefined))
        {
        this.withoutScannerOrderVitals.push(obj);
        }
        this.afterGetVitalsList();
       // let checkBoxId = "#" + element.DrugAdminister_Id;
       // $(checkBoxId).prop("disabled",true);
        }
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
    });
    this.userInputsOpen();  
  }}
}
closeWithoutScannerVitalsModel()
{
  this.modalAdministerWithoutScannerVitalsIsOpen=false;
  this.favouritesForm.reset();
}
// checkVitalsComment(Id:any,event)
// {
// 

// const foundIndex = this.checkedVitalsList.findIndex(({ IdValue }) => IdValue === Id);
// this.checkedVitalsList = this.checkedVitalsList.filter((_, index) => index !== foundIndex);

// this.checkedVitalsList.push({IdValue:Id,value:event});
// }
// GetVitalsDataByIds(PQuantity_Id :string)
// {
 
//   this.dataservice.get<any[]>(this.config.Emar_GetVitalsChecksListbyQuantityIds + PQuantity_Id)
//   .subscribe(res => {
//     
//     const distinctThings = res.filter(
//       (thing, i, arr) => arr.findIndex(t => t.OrderFavMaster_ID === thing.OrderFavMaster_ID) === i
//     );
//     res.forEach((element, index) => {
//       let checkBoxId = "#" + element.PQuantity_Id;
//     $(checkBoxId).prop("disabled",true);
//     });
   
//     this.withoutScannerDisplayVitals =distinctThings;
//   },
//     error => {
//       this.ng4LoadingSpinnerService.hide();
//       this.alertService.error(error.message);
//     });
// }
withoutScannervitalsCheckSave()
{
let data ={
AdminsIds :this.drugAdminIds.join(',').toString(),
checkedVitals:this.checkedVitalsList,
CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
CreatedDate: this.dateFormatPipe.transform(new Date()),
}
this.dataservice.post(this.config.Emar_AdministerOdersVitalsInfo,data)
.subscribe(res => {
  if(res!="")
  {
  let resData = res;
  let records= resData.split(',');
  if(records.length>0)
  {
    
  records.forEach(element=>{
  let list=this.OrderList.filter(r=>r.Type!=0 && r.PQuantity_Id==parseInt(element));
  if(list.length>0)
  {  
    list.forEach(ele => {
      let CheckBoxId = "#" + ele.DrugAdminister_Id;

      $(CheckBoxId).prop("disabled",false);
      
    }); 
  }
  });
  }
}
this.checkedVitalsList =[];
 this.modalAdministerWithoutScannerVitalsIsOpen =false;
  this.favouritesForm.reset();
});

}
// Discard date Region
checkDiscardDate(discardDate:any,DrugAdminister_Id:any,discardDays:any,index:any)
{
  
  if(this.ordersGrid==1)
  {
  //let ordercheckBoxId = "#" + pquantity_Id;
  //$(ordercheckBoxId).prop("checked",false);
  if(discardDays==null || discardDays==0)
  {
    //No Discard days
    //.filter(o=>o.Type==0)
    if (this.OrderList.length == (index + 1)) {
      this.ordersGrid = 2;
    }
    return 0;
  }
  else
  {
  if(discardDate==undefined || discardDate==null || discardDate=="")
  {
    let manSetUnCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    let newPrtUnCheckBoxId = "#newpr" +DrugAdminister_Id;
    $(newPrtUnCheckBoxId).prop("checked",false);
    let manSetCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",true);
    let discardDate="#disca"+DrugAdminister_Id;
    $(discardDate).attr("disabled","disabled");
    let ordercheckBoxId = "#" + DrugAdminister_Id;
    $(ordercheckBoxId).css("display","none"); 
    if (this.OrderList.length == (index + 1)) {
      this.ordersGrid = 2;
    }
    return 2;
  }
  else if(discardDate!=undefined && discardDate!=null && discardDate!=""){
  if( ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))>=0 && ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<=3)
  {
    //Drug  must be discarded soon, reorder promptly
    let manSetUnCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    let newPrtUnCheckBoxId = "#newpr" + DrugAdminister_Id;
    $(newPrtUnCheckBoxId).prop("checked",false);
    let manSetCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",false);
    let discardDate="#disca"+DrugAdminister_Id;
    $(discardDate).attr("disabled","disabled");
    let ordercheckBoxId = "#" + DrugAdminister_Id;
    $(ordercheckBoxId).prop("checked",false);
    $(ordercheckBoxId).css("display","block");
   if (this.OrderList.length == (index + 1)) {
     this.ordersGrid = 2;
   }
    return 1;
  }
  else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))<0)
  {
    //This drug package should no longer be used, open a new package
    let manSetUnCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    let newPrtUnCheckBoxId = "#newpr" + DrugAdminister_Id;
    $(newPrtUnCheckBoxId).prop("checked",false);
    let manSetCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",false);
    let discardDate="#disca"+DrugAdminister_Id;
    $(discardDate).attr("disabled","disabled");
    let ordercheckBoxId = "#" + DrugAdminister_Id;
    $(ordercheckBoxId).prop("checked",false);
    $(ordercheckBoxId).css("display","none");
   if (this.OrderList.length == (index + 1)) {
     this.ordersGrid = 2;
   }
    return 2;
  }
  else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))>3)
  {
    let manSetUnCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    let newPrtUnCheckBoxId = "#newpr" + DrugAdminister_Id;
    $(newPrtUnCheckBoxId).prop("checked",false);
    let manSetCheckBoxId = "#manset" + DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",false);
    let discardDate="#disca"+DrugAdminister_Id;
    $(discardDate).attr("disabled","disabled");
    let ordercheckBoxId = "#" + DrugAdminister_Id;
    $(ordercheckBoxId).prop("checked",false);
    $(ordercheckBoxId).css("display","block");
   if (this.OrderList.length == (index + 1)) {
     this.ordersGrid = 2;
   }
    return 0;
  }
}
}
}
}
onselectNewProductOpenWS(event:any,orderItem:any,index:any)
  {
    
    this.ordersGrid=2;
  if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
    this.alertService.warn("Future orders cannot be administered");
    let ordercheckBoxId = "#newpr" + orderItem.DrugAdminister_Id;
    $(ordercheckBoxId).prop("checked", false);
    this.ng4LoadingSpinnerService.hide();
  }
  else
  {
    var orderItemRecords=this.OrderList.filter(or=>or.Type==0 && or.Porder_Id==orderItem.Porder_Id);
    orderItemRecords.forEach((listItem,len) => {
      if(event==true)
    {
    let newPrCheckBoxId = "#newpr" + listItem.DrugAdminister_Id;
    $(newPrCheckBoxId).prop("checked",true); 
    if (orderItem != undefined && orderItem.DiscardDays!=null && orderItem.DiscardDays!=0) {
    let discardDate=new Date( new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck)).setDate(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck)).getDate()+(orderItem.DiscardDays-1))).toISOString().substring(0, 10);
    let discardDateId="#disca"+listItem.DrugAdminister_Id;
    $(discardDateId).val(discardDate);
    let manSetCheckBoxId = "#manset" + listItem.DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",false);
    let manSetUnCheckBoxId = "#manset" + listItem.DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    $(discardDateId).attr("disabled","disabled");
    if( ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))>=0 && ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<=3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ade39d"); 
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))<0)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ffdf62");
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).css("display","none");
      //this.wsCheckAllButtonDisplay();
      let objIndex = this.selectedRecords.findIndex(cs => cs.DrugAdminister_Id == listItem.DrugAdminister_Id);
      if(objIndex>=0)
      {
      this.selectedRecords.splice(objIndex, 1);
      }
    }
    else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))>3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "white");
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    }
    }
    else if(event==false)
    {
      let newPrCheckBoxId = "#newpr" + listItem.DrugAdminister_Id;
    $(newPrCheckBoxId).prop("checked",false); 
    if(orderItem.DiscardDays!=null && orderItem.DiscardDays!=0 && (orderItem.DiscardDate==null || orderItem.DiscardDate==""))
    {
    let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
    let manSetUnCheckBoxId = "#manset" + listItem.DrugAdminister_Id;
    $(manSetUnCheckBoxId).prop("checked",false);
    let manSetCheckBoxId = "#manset" + listItem.DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",true);
    let discardDate="#disca"+listItem.DrugAdminister_Id;
    $(discardDate).attr("disabled","disabled");
    let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
    $(ordercheckBoxId).css("display","none");
    let discardDateId="#disca"+listItem.DrugAdminister_Id;
    $(discardDateId).val(orderItem.DiscardDate);
    let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
    $(rowBgColor).css("background-color", "#ffdf62");
    //this.wsCheckAllButtonDisplay();
    let objIndex = this.selectedRecords.findIndex(cs => cs.DrugAdminister_Id == listItem.DrugAdminister_Id);
      if(objIndex>=0)
      {
      this.selectedRecords.splice(objIndex, 1);
      }
    }
    else if(orderItem.DiscardDays!=null && orderItem.DiscardDays!=0 && orderItem.DiscardDate!=null && orderItem.DiscardDate!="")
    {
    let manSetCheckBoxId = "#manset" + listItem.DrugAdminister_Id;
    $(manSetCheckBoxId).prop("disabled",false);
    var check=$(manSetCheckBoxId).prop("checked");
    let discardDate="#disca"+listItem.DrugAdminister_Id;
    if(check=false)
    {
    $(discardDate).attr("disabled","disabled");
    }
    else if(check==true)
    {
    $(discardDate).removeAttr("disabled");
    }
    let discardDateId="#disca"+listItem.DrugAdminister_Id;
    $(discardDateId).val(this.dateFormatPipe.transformISODate(orderItem.DiscardDate));
    if( ((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))>=0 && ((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<=3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ade39d");
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    else if(((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))<0)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ffdf62");
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).css("display","none");
      //this.wsCheckAllButtonDisplay();
      let objIndex = this.selectedRecords.findIndex(cs => cs.DrugAdminister_Id == listItem.DrugAdminister_Id);
      if(objIndex>=0)
      {
      this.selectedRecords.splice(objIndex, 1);
      }
    }
    else if(((new Date(this.dateFormatPipe.transform(orderItem.DiscardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))>3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==listItem.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ listItem.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "white");
      let ordercheckBoxId = "#" + listItem.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    }
    }
    if(orderItemRecords.length==(len+1))
    {
      this.wsCheckAllButtonDisplay(); 
    }
    });
  }
  }
  onselectManSetDiscardWS(event:any,orderItem:any,index:any)
  {
    this.ordersGrid=2;
    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered");
      let ordercheckBoxId = "#manset" + orderItem.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked", false);
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
    var orderItemRecords=this.OrderList.filter(or=>or.Type==0 && or.Porder_Id==orderItem.Porder_Id);
    orderItemRecords.forEach((orderItemList,len) => {
      if(event==true)
    {
      let ordercheckBoxId = "#manset" + orderItemList.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked", true);
      let discardDate="#disca"+orderItemList.DrugAdminister_Id;
      $(discardDate).removeAttr("disabled");
    }
    else if(event==false)
    {
      let ordercheckBoxId = "#manset" + orderItemList.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked", false);
      let discardDate="#disca"+orderItemList.DrugAdminister_Id;
      $(discardDate).attr("disabled","disabled");
      let newPrtCheckBoxId = "#newpr" + orderItemList.DrugAdminister_Id;
      if($(newPrtCheckBoxId).prop("checked") == false){
        this.onselectNewProductOpenWS(false,orderItem,index)
      } 
     else if($(newPrtCheckBoxId).prop("checked") == true){
        this.onselectNewProductOpenWS(true,orderItem,index)
      } 
      
    }
    });
    }
  }
  changeDiscardDateWS(date:any,orderItem:any,index:any)
  {
    this.ordersGrid=2;
    if(date!=undefined && date!=null)
    {
    var orderItemRecords=this.OrderList.filter(or=>or.Type==0 && or.Porder_Id==orderItem.Porder_Id);
    orderItemRecords.forEach((Orderelement,len) => {
    let num=(new Date(this.dateFormatPipe.transform(date)).setHours(0,0,0,0)-new Date().setHours(0,0,0,0))/ (1000 * 60 * 60 * 24);
    let discardDateId="#disca"+Orderelement.DrugAdminister_Id;
    $(discardDateId).val(this.dateFormatPipe.transformISODate(date));
    if( ((new Date(this.dateFormatPipe.transform(date)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))>=0 && ((new Date(this.dateFormatPipe.transform(date)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<=3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==Orderelement.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ Orderelement.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ade39d");
      let ordercheckBoxId = "#" + Orderelement.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    else if(((new Date(this.dateFormatPipe.transform(date)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))<0)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==Orderelement.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ Orderelement.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "#ffdf62");
      let ordercheckBoxId = "#" + Orderelement.DrugAdminister_Id;
      $(ordercheckBoxId).css("display","none");
      //this.wsCheckAllButtonDisplay();
      let objIndex = this.selectedRecords.findIndex(cs => cs.DrugAdminister_Id == orderItem.DrugAdminister_Id);
      if(objIndex>=0)
      {
      this.selectedRecords.splice(objIndex, 1);
      }
    }
    else if(((new Date(this.dateFormatPipe.transform(date)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))>3)
    {
      let rowIn=this.OrderList.findIndex(or=>or.DrugAdminister_Id==Orderelement.DrugAdminister_Id);
      let rowBgColor = "#WSOrderRow"+rowIn+ Orderelement.DrugAdminister_Id;
      $(rowBgColor).css("background-color", "white");
      let ordercheckBoxId = "#" + Orderelement.DrugAdminister_Id;
      $(ordercheckBoxId).prop("checked",false);
      $(ordercheckBoxId).css("display","block");
      //this.wsCheckAllButtonDisplay();
    }
    if(orderItemRecords.length==(len+1))
    {
      this.wsCheckAllButtonDisplay(); 
    }
    });
  }
  }
  getDiscardDateForAdminister(DrugAdminister_Id:any)
  {
    let discardDateId="#disca"+DrugAdminister_Id;
    let discardValue=$(discardDateId).val();
    return discardValue;
  }
  wsCheckAllButtonDisplay()
  {
    
    var ordersRecords=this.OrderList.filter(o=>o.Type==0);
    if(ordersRecords.length>0)
    {
    for (let i = 0; i < ordersRecords.length; i++) {
      if(ordersRecords[i].DiscardDays!=null && ordersRecords[i].DiscardDays!=0)
      {
      let rowBgColor = "#WSOrderRow"+i+ ordersRecords[i].DrugAdminister_Id;
      if($(rowBgColor).css("background-color")==="rgb(255, 223, 98)")
      {
        this.wsDiscard=0;
        break;
      }
      else{
        //this.wsDiscard=1;
      }
      }
      if(ordersRecords.length==(i+1))
      {
        this.wsDiscard=1;
        break;
      }
    }
  }
  }
  getTextColorForDiscardDate(discardDate:any,discardDays:any)
  {
    if(discardDays==null || discardDays==0)
  {
    //No Discard days
    return 'white';
  }
  else
  {
    if(discardDate==undefined || discardDate==null || discardDate=="")
    {
      this.wsDiscard=0;
     return "#ffdf62";
    }
    else if(discardDate!=undefined && discardDate!=null && discardDate!=""){
   if( ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))>=0 && ((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-(new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<=3)
    {
      //Drug  must be discarded soon, reorder promptly
      return '#ade39d';
    }
    else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))<0)
    {
      //This drug package should no longer be used, open a new package
      this.wsDiscard=0;
      return '#ffdf62';
    }
    else if(((new Date(this.dateFormatPipe.transform(discardDate)).setHours(0,0,0,0)-((new Date(this.dateFormatPipe.transform(this.myform.value.dateCheck))).setHours(0,0,0,0)))/ (1000 * 60 * 60 * 24))>3)
    {
      return 'white';
    }
  }
  }
}

//Insulin Sites Region

openInsulinSiteWithoutScannerModal(DrugAdminister_Id:any,route:any,administerSites:any)
  {
    
    if (this.dateFormatPipe.transformISODate(this.myform.value.dateCheck) > this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)) {
      this.alertService.warn("Future orders cannot be administered");
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
    this.lastUsedSiteWS=administerSites!=null && administerSites!=""?administerSites:"";
    this.porderIdForInsulin=DrugAdminister_Id;
    this.routeForInsulin=route;
    this.displayInsuliSitesForWS=[];
    this.displayInsuliSitesForWS=route=="TD"?this.allInsulinSites.filter(f=>f.item_type==2):this.allInsulinSites.filter(f=>f.item_type==1);
    this.sitesType=route=="TD"?"Patch Sites":"Administration Sites";
    this.wsOrderInsulinSites=[];
    this.displayInsulinSitesChecksResetWS();
    }
  }
  displayInsulinSitesChecksResetWS()
  {
    
    this.displayInsuliSitesForWS.forEach((element,index)=>{
      if(this.insulinSiteIdsWS.length==0 ||  this.insulinSiteIdsWS.find(i=>i.DrugAdminister_Id==this.porderIdForInsulin)==undefined)
      {
        let siteCheckBoxId = "#insulin" + element.item_id;
        $(siteCheckBoxId).prop("disabled",false);
        $(siteCheckBoxId).prop("checked",false);
      }
      element.lastUsed=0;
      element.lastUsedDate="";
      this.displayInsuliSitesForWS.push();
        if(this.lastUsedSiteWS!="")
        {
        let insulinSites=this.lastUsedSiteWS.split(",");
        if(insulinSites.length>0)
        {
        insulinSites.forEach(value => {
        
        var site = value.split(" | ");
        if(site[0]==(element.item_id.toString()))
        {
          element.lastUsed=1;
          element.lastUsedDate=this.timeFormatId==1? this.dateFormatPipe.get24HourDateTime(site[1]) :site[1];
          this.displayInsuliSitesForWS.push();
        }
        });
        }
      }
       if(this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(i=>i.DrugAdminister_Id==this.porderIdForInsulin)!=undefined)
        {
          
          let Ids=this.insulinSiteIdsWS.find(i=>i.DrugAdminister_Id==this.porderIdForInsulin).SiteIds.split(",");
          element.IsChacked=Ids.includes(element.item_id.toString())==true?true:false;
          element.IsDisabled=(this.routeForInsulin!="TD"&&Ids.includes(element.item_id.toString())==false)?true:false;
          this.displayInsuliSitesForWS.push();
          let siteCheckBoxId = "#insulin" + element.item_id;
          $(siteCheckBoxId).prop("disabled", element.IsDisabled);
          $(siteCheckBoxId).prop("checked", element.IsChacked);
          if(element.IsChacked==true)
          {
            this.wsOrderInsulinSites.push(element.item_id);
          }
        }
        if(this.displayInsuliSitesForWS.length==(index+1))
        {
          this.modalAdministerInsulinSiteIsOpen=true;
        }
    });
  }
  insulinSiteCheckWS(event:any,siteId:any)
  {
    if(event==true)
    {
      if(this.wsOrderInsulinSites.find(id => id == siteId)==undefined)
      {
      this.wsOrderInsulinSites.push(siteId);
      }
      if (this.routeForInsulin != undefined && this.routeForInsulin!=null && (this.routeForInsulin=="SC" || this.routeForInsulin=="IM")) {
      this.displayInsuliSitesForWS.forEach(element=>{
        if(element.item_id!=siteId)
        {
          let siteCheckBoxId = "#insulin" + element.item_id;
          $(siteCheckBoxId).prop("disabled",true);
        }
      });
      }
    }
   else if(event==false)
    {
      let index = this.wsOrderInsulinSites.findIndex(id => id == siteId);
      if (index >= 0) {
      this.wsOrderInsulinSites.splice(index, 1);
      }
      if (this.routeForInsulin != undefined && this.routeForInsulin!=null && (this.routeForInsulin=="SC" || this.routeForInsulin=="IM")) {
      this.displayInsuliSitesForWS.forEach(element=>{
        if(element.item_id!=siteId)
        {
          let siteCheckBoxId = "#insulin" + element.item_id;
          $(siteCheckBoxId).prop("disabled",false);
        }
      });
      }
    }
  }
  insertInsulinSitesForWS()
  {
    if(this.insulinSiteIdsWS.length==0)
    {
    let obj={
      DrugAdminister_Id:this.porderIdForInsulin,
      RouteCode:this.routeForInsulin,
      SiteIds:this.wsOrderInsulinSites.join(','),
    }
    this.insulinSiteIdsWS.push(obj);
    if(this.selectedRecords.length>0 && this.selectedRecords.find(s=>s.DrugAdminister_Id==obj.DrugAdminister_Id)!=undefined)
    {
      this.insulinSitesForSelectedRecords(obj.DrugAdminister_Id);
    }
   }
   else if(this.insulinSiteIdsWS.length>0)
   {
     let record=this.insulinSiteIdsWS.find(i=>i.DrugAdminister_Id==this.porderIdForInsulin);
     if(record==undefined)
     {
      let obj={
        DrugAdminister_Id:this.porderIdForInsulin,
        RouteCode:this.routeForInsulin,
        SiteIds:this.wsOrderInsulinSites.join(','),
      }
      this.insulinSiteIdsWS.push(obj);
      if(this.selectedRecords.length>0 && this.selectedRecords.find(s=>s.DrugAdminister_Id==obj.DrugAdminister_Id)!=undefined)
    {
      this.insulinSitesForSelectedRecords(obj.DrugAdminister_Id);
    }
     }
     else
     {
      let index=this.insulinSiteIdsWS.findIndex(i=>i.DrugAdminister_Id==this.porderIdForInsulin);
      this.insulinSiteIdsWS.splice(index,1);
      let obj={
        DrugAdminister_Id:this.porderIdForInsulin,
        RouteCode:this.routeForInsulin,
        SiteIds:this.wsOrderInsulinSites.join(','),
      }
      this.insulinSiteIdsWS.push(obj);
      if(this.selectedRecords.length>0 && this.selectedRecords.find(s=>s.DrugAdminister_Id==obj.DrugAdminister_Id)!=undefined)
    {
      this.insulinSitesForSelectedRecords(obj.DrugAdminister_Id);
    }
     }
   }
    this.modalAdministerInsulinSiteIsOpen=false;
  }
  closeAdministerSitesModel()
  {
    this.modalAdministerInsulinSiteIsOpen=false;
  }
  insulinSitesForSelectedRecords(DrugAdminister_Id:any)
  {
    
    let record=this.selectedRecords.find(s=>s.DrugAdminister_Id==DrugAdminister_Id);
    if(record!=undefined)
    {
    record.AdministerInsulinSites=this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==DrugAdminister_Id).SiteIds:null;
    record.RouteCode=this.insulinSiteIdsWS.length>0 && this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==DrugAdminister_Id)!=undefined?this.insulinSiteIdsWS.find(o=>o.DrugAdminister_Id==DrugAdminister_Id).RouteCode:null;
    this.selectedRecords.push();
    }
  }
  prnReasonUpdate(porderId:any,pquantity_Id:any,value:any,DrugAdminister_Id:any)
  {
    let record=this.selectedRecords.find(s=>s.POrder_Id==porderId && pquantity_Id==s.pquantity_Id && s.DrugAdminister_Id==DrugAdminister_Id && s.PRNFlag==true);
    if(this.selectedRecords.length>0 && record!=undefined)
    {
      record.AdministerComment=value;
    }
  }
  getPrnReason(DrugAdminister_Id:any)
  {
    let prnText="#PRN"+DrugAdminister_Id;
    let prnReasonText= $(prnText).val()!=undefined?$(prnText).val():"";
    return prnReasonText;
  }
  getDosesDetails(FacilityId: any, NsId: string) {
    this.dataservice.get<any>(this.config.Emar_Common_GetPendingDosesList + this.persistanceService.get(this.config.loggedInUserKey) + "/" + NsId + "/" + FacilityId)
      .subscribe(res => {
        this.sharedService.updateDosesList(res);
      });
  }
  //User Inputs Logic Begin
  withoutScannerVitals()
{
  
  debugger;
  this.withoutScannerTempVitals=[];
  var vitalCheckRecords=this.OrderList.filter(r=>r.Type==0);
  if(vitalCheckRecords.length>0)
  {
    vitalCheckRecords.forEach((element, index) => {

      let checkBoxId = "#" + element.DrugAdminister_Id;
      $(checkBoxId).prop("checked",false);

      //let BoxId = "#" + element.PQuantity_Id+element.DrugAdminister_Id;
      //$(BoxId).css("display","none");

      this.withoutScannerOrderVitals=[];
      this.WsPRN.reset();
      this.dataservice.get<any[]>(this.config.Emar_GetVitalsCheckList + element.PQuantity_Id)
      .subscribe(res => {
        if(res.length>0)
        {
         
        let checkBoxId = "#" + element.DrugAdminister_Id;
        $(checkBoxId).prop("disabled",true);
        //let clk = "#" + element.PQuantity_Id;
        //$(clk).css("color","blue");
        }
        else
        {
let clk = "#" + element.PQuantity_Id+element.DrugAdminister_Id;
      //  $(clk).prop("disabled",true);
     $(clk).css("display","none");

        }
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
    });  
  }
  window.scroll(0, 0);
  this.favouritesForm.reset();
}
afterGetVitalsList()
{
  if(this.withoutScannerTempVitals.length>0)
  {
    
    this.withoutScannerDisplayVitals=[];
    this.withoutScannerTempVitals.filter(el => {
      if (this.withoutScannerDisplayVitals.length==0 ||(this.withoutScannerDisplayVitals.length>0 && this.withoutScannerDisplayVitals.find(wi=>wi.OrderFavMaster_ID==el.OrderFavMaster_ID)==null)) {
          // If not present in array, then add it
          this.withoutScannerDisplayVitals.push(el);
          console.log("withoutScannerDisplayVitals001");
  console.log(this.withoutScannerDisplayVitals);
      } else {
          // Already present in array, don't add it
      }
  });
 let vitalsChecks= this.withoutScannerDisplayVitals.sort((a, b) => {
    if(a.PQuantity_Id > b.PQuantity_Id) {
      return 1;
    } else if(a.PQuantity_Id < b.PQuantity_Id) {
      return -1;
    } else {
      return 0;
    }
    
  });
  this.withoutScannerDisplayVitals=vitalsChecks;
  console.log("withoutScannerDisplayVitals");
  console.log(this.withoutScannerDisplayVitals);
  }
}
checkVitalsComment(OrderFavMaster_ID: number, Value: string) {
  this.ng4LoadingSpinnerService.show();
  if (Value != "") {
    let favObj = new OrderFavouriteData();
    favObj.FavouriteData_Id = 0;
    favObj.POrder_Id = null;
    favObj.pquantity_Id = null;
    favObj.AdminsterSchedule = null;
    favObj.InputTime = null;
    favObj.ShiftId = null;
    favObj.Window = null;
    favObj.OrderFavMaster_ID = OrderFavMaster_ID;
    favObj.value = Value;
    favObj.FavouriteData_Status = 1;
    favObj.FavouriteData_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
    favObj.Favourite_CreatedOn = this.dateFormatPipe.dateWithTime(new Date());
    let index = this.CheckVitalsObj.findIndex(cs => cs.OrderFavMaster_ID == OrderFavMaster_ID);
    if (index >= 0) {
      this.CheckVitalsObj.splice(index, 1);
      this.CheckVitalsObj.push(favObj);
    }
    else
      this.CheckVitalsObj.push(favObj);
  }
  else if (Value == "") {
    let index = this.CheckVitalsObj.findIndex(cs => cs.OrderFavMaster_ID == OrderFavMaster_ID);
    this.CheckVitalsObj.splice(index, 1);
  }
  if (this.CheckVitalsObj.length != this.vitalsCheckList.length) {
    const vitalsvalidation = this.favouritesForm.get('comments');
    //barcodevalidation.setValidators(null);
    vitalsvalidation.setValidators([Validators.required]);
    vitalsvalidation.updateValueAndValidity();
    this.vitalsStatus = 1;
  }
  else {
    const vitalsvalidation = this.favouritesForm.get('comments');
    //barcodevalidation.setValidators(null);
    vitalsvalidation.setValidators(null);
    vitalsvalidation.clearValidators();
    vitalsvalidation.updateValueAndValidity();
    this.vitalsStatus = 0;
  }
  this.ng4LoadingSpinnerService.hide();
}
vitalsCheckSave(index:any) {
  debugger;
  //this.cllick = index;
    this.ng4LoadingSpinnerService.show();
    if (this.CheckVitalsObj.length!=0) {
      let vitalsCompletedOrder=[];
      this.withoutScannerOrderVitals.forEach((record,len)=>{
        let tempVitals=[];
        let vitals=[];
        this.CheckVitalsObj.forEach(item => tempVitals.push(item.OrderFavMaster_ID.toString()));
        let vitalsByOrder =record.Vitals.split(',');
        //based order all inputs given or not 
        let test=vitalsByOrder.map(x=> tempVitals.includes(x.toString()));
        if((test.every( (val, i, arr) => val === arr[0] && val==true))==true)
        {
        var vitalsRecords=this.CheckVitalsObj.filter(ch=>vitalsByOrder.includes((ch.OrderFavMaster_ID).toString()));
        vitalsRecords.forEach(re=>vitals.push({IdValue:re.OrderFavMaster_ID,value:re.value}));
        let data ={
          AdminsIds :record.DadminId.toString(),
          checkedVitals:vitals,
          CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          CreatedDate: this.dateFormatPipe.transform(new Date()),
          }
        this.dataservice.post(this.config.Emar_AdministerOdersVitalsInfo, data)
        .subscribe(res => {
          
          //this.alertService.success("Save successful");
          //this.vitalCheckStatus = "Vitals Check Completed";
          // let checkBoxId = "#" + record.qtyId;
          // $(checkBoxId).prop("disabled",false);
          // vitalsCompletedOrder.push(record.qtyId);
          if (res!=null && res != "") {
            let resData = res;
            let records = resData.split(',');
            if (records.length > 0) {

              records.forEach((element,i) => {
                let list = this.OrderList.filter(r => r.DrugAdminister_Id == data.AdminsIds);
                if (list.length > 0) {
                  list.forEach(ele => {
                    let CheckBoxId = "#" + ele.DrugAdminister_Id;
                    $(CheckBoxId).prop("disabled", false);
                    let clicck = "#" + ele.PQuantity_Id+ ele.DrugAdminister_Id;
                    $(clicck).css("color","green");
                  });
                }
                // let CheckBoxId = "#" + element;
                // $(CheckBoxId).prop("disabled", false);
                vitalsCompletedOrder.push(element);
                if(records.length==(i+1))
                {
                  if (vitalsCompletedOrder.length == this.withoutScannerOrderVitals.length) {
                    this.withoutScannerDisplayVitals = [];
                  }
                  //this.modalAdministerWithoutScannerVitalsIsOpen=false;
                }
              });
              
            }
          else{
          if(vitalsCompletedOrder.length==this.withoutScannerOrderVitals.length)
          {
            this.withoutScannerDisplayVitals=[];
          }
          //this.modalAdministerWithoutScannerVitalsIsOpen=false;
        }
        }
        else
        {
          this.modalAdministerWithoutScannerVitalsIsOpen=false;
        }
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
        }
        if(this.withoutScannerOrderVitals.length==(len+1))
        {
          this.modalAdministerWithoutScannerVitalsIsOpen=false;
        }
        });
        this.ng4LoadingSpinnerService.hide();
        //this.CheckVitalsObj=[];
        //this.modalAdministerWithoutScannerVitalsIsOpen=false;
    }
    else {
      this.alertService.warn("Please enter required user inputs")
      this.ng4LoadingSpinnerService.hide();
    }
    window.scroll(0, 0);
    this.favouritesForm.reset();
  }
  getAllFlagsForCompanyByNSId() {
    let stationId=this.selectednItems[0].NurseStation_Id;
    this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId)
      .subscribe(res => {
        this.timeFormatId = res.TimeFormat;;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  timeFormate(time: any): any {
    let hour = (time.split(':'))[0]
    let min = (time.split(':'))[1]
    let part = hour > 12 ? 'PM' : 'AM';
    min = (min + '').length == 1 ? `0${min}` : min;
    hour = hour > 12 ? hour - 12 : hour;
    hour = (hour + '').length == 1 ? `0${hour}` : hour;
    return `${hour}:${min} ${part}`
  }
  timeSubString(time: any)
  {
    
    let hour = (time.split(':'))[0]
    let min = (time.split(':'))[1]
    return `${hour}:${min}`
  }
}