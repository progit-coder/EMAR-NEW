import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { AlertService } from '../../../_services';
import { Floor, Wing, NurseStation, Room, Bed } from '../../../models/facility.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { ControlSubstanceFilter, ControlSubstanceGrid, CertifyAndApprovalCheck, ControlSubstanceSave } from '../../../models/emar.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ControlledsubstancediscrepancyreasonComponent } from '../controlledsubstancediscrepancyreason/controlledsubstancediscrepancyreason.component';
import * as $ from 'jquery';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
@Component({
  selector: 'app-controlsubstancecount',
  templateUrl: './controlsubstancecount.component.html',
  styleUrls: ['./controlsubstancecount.component.css'],
  providers: [DataService, APIConfiguration]
})
export class ControlsubstancecountComponent implements OnInit {
  public template;
  ShowFilter = true;
  errorMessage: string;
  toolTipMes: string;
  public selectedResItem = [];
  dropdownSettings_Resident: any = {};
  public filterData: any = [];
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  dropdownSettings_Facilities: any = {};
  arfloor = []; arfacility = []; arnstation = []; arwing = []; arroom = []; arbed = [];
  public floors: Floor[];
  public nurseStations: NurseStation[];
  public wings: Wing[];
  public rooms: Room[];
  public beds: Bed[];
  public facilities: any[];
  ddlfloors: any;
  ddlwings: any;
  history: boolean = false;
  public filterConfigs: ControlSubstanceFilter;
  saveform: FormGroup;
  showHistroyform: FormGroup;
  controlGridData: ControlSubstanceGrid[] = [];
  modalHistroy: boolean = false;
  quantityflag: boolean = false;
  modalSave: boolean = false;
  modalDiscrepancyReason: boolean = false;
  public p: number = 1;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  userCheckObj: CertifyAndApprovalCheck;
  ControlSubstanceSaveObj: ControlSubstanceSave[] = [];
  myform: FormGroup;
  filterform: FormGroup;
  public residentddlform;
  public companyToBed: number = 0;
  public userId: number;
  public residents: any[];
  public qtyAlert: any;
  public orderId: number;
  public form: FormGroup;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public nstations: string = "";
  pageConfig = {};
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  public modalControlledSubstanceHistoryIsOpen:boolean=false;
  public certifyControlledMedsHistory:any[]=[];
  public historyPagination=10;
  public h:number=1;
  public saveDisable=false;
  public consolidateOrders=[];
  public modalConsolidateIsOpen:boolean=false;
  public g:number=1;
  public CheckAll:boolean=false;
  public consolidateOrderSave=[];
  public consolidateBtn:boolean=false;
  public consolidateQuantity:any;
  public consolidateConfirmationModal:boolean=false;
  public removeconsolidateOrders=[];
  public removeconsolidateQuantity:string="";
  public modalRemoveConsolidate:boolean=false;
  public removeconsolidateOrdersSave=[];
  public conDiscrepencyReason:string="";
  public consolidateInitialQty:any;
  public consolidateAministerQty:any;
  @ViewChild('quantityFocus') quantityFocus:ElementRef;
  @ViewChild('inputFocus') inputFocus:ElementRef;

  constructor(private dataservice: DataService, private config: APIConfiguration, private alertService: AlertService
    , private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService
    , private modalService: NgbModal, private dateFormatPipe: CustomdatePipe, ) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ControlledSubstance");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.filterform = new FormGroup({
          ddlfacilities: new FormControl(''),
       //   ddlfloors: new FormControl(''),
          ddlnursestations: new FormControl(''),
         // ddlwings: new FormControl(''),
       //   ddlrooms: new FormControl(''),
        //  ddlbeds: new FormControl(''),
        });
        this.saveform = new FormGroup({
          certUsername: new FormControl('', Validators.required),
          certPass: new FormControl('', Validators.required),
          approvalUsername: new FormControl('', Validators.required),
          approvalPass: new FormControl('', Validators.required),
        });
        this.showHistroyform = new FormGroup({
          showUsername: new FormControl('', Validators.required),
          showPass: new FormControl('', Validators.required),
        });
        this.form = new FormGroup({
          fields: new FormControl(JSON.stringify(this.fields))
        });

        this.myform = new FormGroup({
          quantity: new FormControl('', Validators.required),
        });
        this.residentddlform = new FormGroup({
          ddlresidents: new FormControl('0')
        })
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        //this.getFiltersData(this.userId);
        //this.getCompanyToBedData();
        //this.getControlSubstanceGirddata();
        //this.getResidentDropData();
        this.getUserRecentFacilityNurseStations();
        this.userActivity();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ControlSubstance, Activity.View, '')
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
  getCompanyToBedData(facilityId: number) {
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" + facilityId)
      .subscribe(res => {
        if (res.companyBedFlag == 1) {
          this.companyToBed = res.companyBedFlag;
          this.floors = res.Floors;
          this.wings = res.Wings;
          this.rooms = res.Rooms;
          this.beds = res.Beds;
        }
        else if (res.companyBedFlag == 0) {
          this.companyToBed = 0;
          this.floors = [];
          this.wings = [];
          this.rooms = [];
          this.beds = [];
        }
        // if (this.facilities.length == 1) {
        //   this.filterform.patchValue({
        //     ddlfloors: this.floors,
        //     ddlwings:this.wings,
        //     ddlrooms:this.rooms,
        //     ddlbeds:this.beds,
        //   });
        // }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getFiltersData(userId: number): any {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.facilities = res.Facilities;
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.filterform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        } else if (res.Facilities.length == 1) {
         // this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.filterform.patchValue({
            ddlfacilities: this.facilities,
          });
        }

        // if(res.Facilities.length==1)
        // {
        //   this.getCompanyToBedData(res.Facilities[0].Facility_Id);
        //   this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
        //   this.filterform.patchValue({
        //     ddlfacilities:this.facilities,
        //   });
        // }
        else
        {
        this.ng4LoadingSpinnerService.hide();
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter,
    };
    // this.dropdownSettings_Floors = {
    //   singleSelection: false,
    //   idField: "Floor_Id",
    //   textField: "Floor_Name",
    //   text: "Floors",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter,
    // };
    this.dropdownSettings_NurseStations = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "Nursing Stations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      noDataAvailablePlaceholderText: 'Please Select Facility',
      allowSearchFilter: this.ShowFilter,
      closeOnSelect: true,
    };

    // this.dropdownSettings_Wings = {
    //   singleSelection: false,
    //   idField: "Wing_Id",
    //   textField: "Wing_Desc",
    //   text: "Wings",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    // this.dropdownSettings_Rooms = {
    //   singleSelection: false,
    //   idField: "Room_Id",
    //   textField: "Room_Name",
    //   text: "Rooms",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    // this.dropdownSettings_Beds = {
    //   singleSelection: false,
    //   idField: "Bed_Id",
    //   textField: "Bed_Name",
    //   text: "Beds",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
    // };
    this.dropdownSettings_Resident = {
      singleSelection: true,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
  }
  getFiltersConfig() {
    this.residentddlform.patchValue({
      ddlresidents: '',
    });
    this.myform.patchValue({ quantity: "" });
    if (this.filterform.value.ddlfacilities.length == 0 || this.filterform.value.ddlnursestations.length == 0) {
      this.controlGridData = [];
      this.alertService.error("Use filters to display controlled substance list.");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.getControlledMedsData();
      // if (result.length == 0)
      //   this.alertService.warn("No data available.");
      // else
      //   this.getResidentDropData();
    }
  }
  getControlledMedsData(residentId?: any, userName?: any, passWord?: any) {
    this.ng4LoadingSpinnerService.show();
    this.filterConfigs = {
      CompanyId: 0,
      CompanyTobedFlag: this.companyToBed,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
      Facilities: this.filterform.value.ddlfacilities.length != 0 ? this.filterform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.filterform.value.ddlnursestations.length != 0 ? this.filterform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0) ? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
      History: this.history == false ? 0 : 1,
      UserName: userName != undefined ? userName : "",
      Password: passWord != undefined ? passWord : "",
      User_Id: this.userId,
      ResidentId: residentId
    };
    this.dataservice.post(this.config.Emar_Orders_GetControlSubstanceGridData, this.filterConfigs)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res != null) {
          if (res.length > 0) {
            this.controlGridData = res;
            //if ((userName == undefined && userName == '') && (passWord == undefined && passWord == '') && (residentId == undefined && residentId == 0))
            if ((userName == undefined || userName == '') && (passWord == undefined || passWord == '') && (residentId == undefined || residentId == 0))
            {  
            //this.getResidentDropData();
            }
            else {
              if (userName != undefined && userName != '' && passWord != undefined && passWord != '') {
                this.quantityflag = true;
                this.modalHistroy = false;
                this.showHistroyform.reset();
              }
              else if (residentId != undefined && residentId != 0) {
              }
              else {
                this.alertService.error("Invalid Certifier");
              }
            }
          }
          else {
            if ((userName == undefined || userName == '') && (passWord == undefined || passWord == '')) {
              this.alertService.warn("No data available.");
              this.controlGridData=[];
            }
          }
        }
        else if (userName != '' && passWord != '') {
          this.alertService.error("Invalid Certifier");
        }
      }, error => {
        this.alertService.error(this.errorMessage);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  ShowGrid() {
    let residentId = 0;
    this.ControlSubstanceSaveObj=[];
    if (this.residentddlform.value.ddlresidents.length == 1) {
      residentId = this.residentddlform.value.ddlresidents[0].Patient_Id;
    }
    this.getControlledMedsData(residentId, this.showHistroyform.value.showUsername, this.showHistroyform.value.showPass);
    // if (result != null) {
    //   this.quantityflag = true;
    //   this.modalHistroy = false;
    //   this.showHistroyform.reset();
    // }
    // else {
    //   this.alertService.error("Invalid Certifier");
    //   // this.modalHistroy = false;
    //   // this.quantityflag = false;
    // }
  }
  ShowChange(check) {
    //this.residentddlform.reset();
    if (check == true) {
      this.modalHistroy = true;
      setTimeout(() => {
        this.inputFocus.nativeElement.focus()
      }, 300);
      this.history = true;
      this.saveform.reset();
      this.myform.reset();
      //this.showHistroyform.value.showUsername
    }
    else {
      this.modalHistroy = false;
      this.myform.reset();
      this.history = false;
      this.quantityflag = false;
      this.ControlSubstanceSaveObj=[];
      this.getControlledMedsData();
      //this.getFiltersConfig("");
    }
  }
  SaveChanges() {
     
    let check = this.ControlSubstanceSaveObj.filter(i => i.ReasonRequired == true && i.DiscrepancyReason == '');
    let checkCertifyRecords = this.ControlSubstanceSaveObj.filter(i => i.ReasonRequired == false);
    if (check.length != 0) {
      //this.alertService.warn("Reason must be entered for Quantity Discrepancy to proceed further.");
      this.alertService.warn("Reason for quantity discrepancy must be entered ");
    }
    else if(checkCertifyRecords.length > 0)
    {
      this.modalSave = true;
      this.saveform.reset();
      //this.showHistroyform.reset();
      this.saveform.patchValue({
        certUsername: '',
        certPass: ''
      });
    }
    else {
      this.alertService.error("No data found to certify");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  SaveClose() {
    this.modalSave = false;

    this.saveform.reset();
    //this.showHistroyform.reset();
    this.saveform.patchValue({
      certUsername: '',
      certPass: ''
    });
  }
  ShowClose() {
    this.showHistroyform.reset();
    this.modalHistroy = false;
    this.history = false;
  }
  SaveControlData() {
    this.saveDisable=true;
    let checkCertifyRecords = this.ControlSubstanceSaveObj.filter(i => i.ReasonRequired == false);
    if (checkCertifyRecords.length > 0) {
      this.ng4LoadingSpinnerService.show();
      this.userCheckObj = {
        Cert_UserName: this.saveform.value.certUsername,
        Cert_Password: this.saveform.value.certPass,
        Approval_UserName: this.saveform.value.approvalUsername,
        Approval_Password: this.saveform.value.approvalPass,
        ControlOrders: checkCertifyRecords,
        User_Id: this.persistanceService.get(this.config.loggedInUserKey),
        Facility_Id: this.filterform.value.ddlfacilities[0].Facility_Id,
        ApprovedOn: this.dateFormatPipe.dateWithTime(new Date())
      };
      this.dataservice.post(this.config.Emar_Orders_CheckCertifyAndApprovals, this.userCheckObj)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res != "Success") {
            this.alertService.error(res);
            this.saveDisable=false;
          }
          else {
            this.modalSave = false;
            this.saveform.reset();
            this.saveDisable=false;
            this.saveform.patchValue(
              {
                certUsername: '',
                certPass: ''
              }
            );
            this.alertService.success("Controlled Substance Saved");
            this.ControlSubstanceSaveObj = [];
            this.myform.reset();
            let residentId = 0;
            if (this.residentddlform.value.ddlresidents.length == 1) {
              residentId = this.residentddlform.value.ddlresidents[0].Patient_Id;
            }
            this.getControlledMedsData(residentId);

            this.myform.patchValue({ quantity: "" });
          }
          //this.getFiltersConfig("");
          //this.ControlSubstanceSaveObj = [];
          // this.myform.reset();
          // this.getControlSubstanceGirddata();
          // this.myform.patchValue({ quantity: "" });
          //this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.errorMessage = <any>error.message;
          this.saveDisable=true;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else {
      //this.modalSave = false;
      //this.saveform.reset();
      this.alertService.error("No data found");
      this.saveDisable=false;
    }
  }
  UpdateArray(item: any, Value: any,ekitflag:number) {
    this.orderId = item.PorderId;
    let check = this.ControlSubstanceSaveObj.filter(i => i.ReasonRequired == true && i.DiscrepancyReason == '');
    if (Value != "" && check.length != 0) {
      //this.alertService.warn("Reason must be entered for Quantity Discrepancy to proceed further.");
     // this.alertService.warn("Discrepancy has been identified. Please explain");
     check.forEach(element => {
      if(element.eKitFlag==0)
      {
      this.clear(element.PQuantity_Id,0);
      let orderTextBoxId = "#reason" + element.PQuantity_Id;
      $(orderTextBoxId).val("");
      $(orderTextBoxId).css("display","none");
      this.qtyAlert = 0;
      let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == element.PQuantity_Id);
      this.ControlSubstanceSaveObj.splice(index, 1);
      let data = this.controlGridData.find(cs => cs.QuantityId == element.PQuantity_Id);
      data.Color='';
      }
      else if(element.eKitFlag==1)
      {
        this.clear(element.Ekit_Id,1);
      let orderTextBoxId = "#ekreason" + element.Ekit_Id;
      $(orderTextBoxId).val("");
      $(orderTextBoxId).css("display","none");
      this.qtyAlert = 0;
      let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.Ekit_Id == element.Ekit_Id);
      this.ControlSubstanceSaveObj.splice(index, 1);
      let data = this.controlGridData.find(cs => cs.Ekit_Id == element.Ekit_Id);
      data.Color='';
      }
     });
    }
    if(ekitflag==0)
    {
    if (Value != "") {
      this.qtyAlert = parseFloat(Value).toFixed(2);
      let svObj = new ControlSubstanceSave();
      svObj.POrderId = item.PorderId;
      svObj.PQuantity_Id=item.QuantityId;
      svObj.Quantity = Value;
      svObj.DiscrepancyReason = '';
      svObj.ReasonRequired = false;
      svObj.eKitFlag=0;
      let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == item.QuantityId);
      if (index >= 0) {
        this.ControlSubstanceSaveObj.splice(index, 1);
        this.ControlSubstanceSaveObj.push(svObj);
      }
      else
        this.ControlSubstanceSaveObj.push(svObj);
         
       if ((item.LastCertified == 'None' && item.CheckInFlag==1 && item.AdministeredQty != this.qtyAlert && item.QuantityZeroStatus==0) || (item.NsChangeFlag == 1 && item.QuantityZeroStatus==0)||(item.AdministeredQty != this.qtyAlert && item.QuantityZeroStatus==0)) {
        item.Color = '#ffdf62';
        this.addDiscrepencyReason(item, this.qtyAlert);
      }
      else if(item.QuantityZeroStatus==1 && this.qtyAlert!=0)
      {
        //this.clear(item.QuantityId);
        //this.alertService.warn("Enter Zero(0) as the order is InActive");
        this.alertService.warn("Order inactive. Please destroy remaining quantity");
        item.Color = '';
        let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == item.QuantityId);
        this.ControlSubstanceSaveObj.splice(index, 1);
      }
     else if ((item.AdministeredQty == this.qtyAlert) || (item.QuantityZeroStatus==1 && this.qtyAlert==0))
     {
        item.Color = '#ade39d';
        let orderTextBoxId = "#reason" + item.QuantityId;
        $(orderTextBoxId).val("");
        $(orderTextBoxId).css("display","none");
        let orderqtyId="#"+item.QuantityId;
        $(orderqtyId).val(Value);
      // else if (item.LastCertified == 'None') {
      //   item.Color = '#ade39d';
      // }
     }
      else
        item.Color = '';
    }
    else if (Value == "") {
      let orderTextBoxId = "#reason" + item.QuantityId;
      $(orderTextBoxId).val("");
      $(orderTextBoxId).css("display","none");
      this.qtyAlert = 0;
      let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == item.QuantityId);
      this.ControlSubstanceSaveObj.splice(index, 1);
      item.Color = '';
    }
    }
    else if(ekitflag==1)
    {
       
      if (Value != "") {
        this.qtyAlert = parseFloat(Value).toFixed(2);
        let svObj = new ControlSubstanceSave();
        svObj.POrderId = 0;
        svObj.PQuantity_Id=0;
        svObj.Quantity = Value;
        svObj.DiscrepancyReason = '';
        svObj.ReasonRequired = false;
        svObj.eKitFlag=1;
        svObj.Ekit_Id=item.Ekit_Id;
        let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.Ekit_Id == item.Ekit_Id);
        if (index >= 0) {
          this.ControlSubstanceSaveObj.splice(index, 1);
          this.ControlSubstanceSaveObj.push(svObj);
        }
        else
          this.ControlSubstanceSaveObj.push(svObj);
           
         if ((item.LastCertified == 'None' && item.CheckInFlag==1 && item.AdministeredQty != this.qtyAlert && item.QuantityZeroStatus==0) || (item.NsChangeFlag == 1 && item.QuantityZeroStatus==0)||(item.AdministeredQty != this.qtyAlert && item.QuantityZeroStatus==0)) {
          item.Color = '#ffdf62';
          this.addDiscrepencyReason(item, this.qtyAlert);
        }
        else if(item.QuantityZeroStatus==1 && this.qtyAlert!=0)
        {
          //this.clear(item.QuantityId);
          //this.alertService.warn("Enter Zero(0) as the order is InActive");
          this.alertService.warn("Order inactive. Please destroy remaining quantity");
          item.Color = '';
          let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.Ekit_Id == item.Ekit_Id);
          this.ControlSubstanceSaveObj.splice(index, 1);
        }
       else if ((item.AdministeredQty == this.qtyAlert) || (item.QuantityZeroStatus==1 && this.qtyAlert==0))
       {
          item.Color = '#ade39d';
          let orderTextBoxId = "#ekreason" + item.Ekit_Id;
          $(orderTextBoxId).val("");
          $(orderTextBoxId).css("display","none");
          let orderqtyId="#ek"+item.Ekit_Id;
          $(orderqtyId).val(Value);
        // else if (item.LastCertified == 'None') {
        //   item.Color = '#ade39d';
        // }
       }
        else
          item.Color = '';
      }
      else if (Value == "") {
        let orderTextBoxId = "#ekreason" + item.Ekit_Id;
        $(orderTextBoxId).val("");
        $(orderTextBoxId).css("display","none");
        this.qtyAlert = 0;
        let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.Ekit_Id == item.Ekit_Id);
        this.ControlSubstanceSaveObj.splice(index, 1);
        item.Color = '';
      }
    }
  }
  addDiscrepencyReason(item: any, enteredQty: any) {
    if(item.eKitFlag==0)
    {
    let orderTextBoxId = "#reason" + item.QuantityId;
    $(orderTextBoxId).val("");
    $(orderTextBoxId).css("display","block");
    let record = this.ControlSubstanceSaveObj.find(o => o.PQuantity_Id == item.QuantityId);
      if (record != null) {
          record.ReasonRequired = true;
          record.DiscrepancyReason="";
          this.ControlSubstanceSaveObj.push();
      }
    }
    else if(item.eKitFlag==1)
    {
      let orderTextBoxId = "#ekreason" + item.Ekit_Id;
      $(orderTextBoxId).val("");
      $(orderTextBoxId).css("display","block");
      let record = this.ControlSubstanceSaveObj.find(o => o.Ekit_Id == item.Ekit_Id);
        if (record != null) {
            record.ReasonRequired = true;
            record.DiscrepancyReason="";
            this.ControlSubstanceSaveObj.push();
        } 
    }
    // const modalRef = this.modalService.open(ControlledsubstancediscrepancyreasonComponent, { size: 'lg', windowClass: '' });
    // modalRef.componentInstance.title = 'Merge OrdersEnter Reason for incorrect count';
    // let orderData = {
    //   "ResidentName": item.ResidentName,
    //   "DrugName": item.Order, //+ '(' + enteredQty + ')',
    // }
    // modalRef.componentInstance.orderDetails = orderData;
    // modalRef.componentInstance.reason.subscribe((receivedResult) => {
    //   let record = this.ControlSubstanceSaveObj.find(o => o.PQuantity_Id == item.QuantityId);
    //   if (record != null) {
    //     if (receivedResult != "") {
    //       record.DiscrepancyReason = receivedResult;
    //     }
    //     else {
    //       record.ReasonRequired = true;
    //       this.clear(item.QuantityId);
    //       // this.myform.patchValue({ quantity: "" });
    //       item.Color = '';
    //       let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == item.QuantityId);
    //       this.ControlSubstanceSaveObj.splice(index, 1);
    //     }
    //   }
    //   modalRef.close();
    // })
  }
  UpdateReasonArray(item:any,reason:any)
  {
      let record =item.eKitFlag==0? this.ControlSubstanceSaveObj.find(o => o.PQuantity_Id == item.QuantityId):this.ControlSubstanceSaveObj.find(o => o.Ekit_Id == item.Ekit_Id);
      if (record != null) {
        if (reason != undefined && reason!=null && reason!="") {
          record.DiscrepancyReason = reason;
          record.ReasonRequired = false;
          this.ControlSubstanceSaveObj.push();
        }
        else {
          record.ReasonRequired = true;
          record.DiscrepancyReason = "";
          this.ControlSubstanceSaveObj.push();
          //this.clear(item.QuantityId);
          // this.myform.patchValue({ quantity: "" });
          //item.Color = '';
          //let index = this.ControlSubstanceSaveObj.findIndex(cs => cs.PQuantity_Id == item.QuantityId);
          //this.ControlSubstanceSaveObj.splice(index, 1);
        }
      }
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.controlGridData = [];
    this.residents = [];
    this.companyToBed = 0;
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.filterform.patchValue({
      ddlnursestations: '',
    //  ddlfloors: '',
     // ddlwings: '',
    //  ddlrooms: '',
     // ddlbeds: '',
    });
    this.residentddlform.patchValue({
      ddlresidents: '',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
    //this.getCompanyToBedData(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.controlGridData = [];
    this.residents = [];
    this.companyToBed = 0;
    this.filterform.patchValue({
      ddlnursestations: '',
     // ddlfloors: '',
   //   ddlwings: '',
    //  ddlrooms: '',
   //   ddlbeds: '',
    });
    this.residentddlform.patchValue({
      ddlresidents: '',
    });
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
  }
  onNurseStationSelect(item: any) {
    this.controlGridData=[];
    this.getFiltersConfig();
    this.getCompanyToBedByNurseStation();
  }
  onNurseStationSelectAll(item: any) {
    this.controlGridData=[];
    this.filterform.value.ddlnursestations = item;
    this.getFiltersConfig();
  }
  onNurseStationDeSelect(item: any) {
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.floorIndex =0;
    this.wingIndex =0;
    this.roomIndex =0;
    this.bedIndex =0;
    this.form.reset();
    this.fields =[];
    this.controlGridData=[];
    this.getFiltersConfig();
  }
  onNurseStationDeSelectAll(item: any) {
    this.controlGridData = [];
    this.alertService.error("Please select at least one nursing station to display the data.")
  }
  onCommonSelectItems(item:any) {
    this.controlGridData=[];
    this.getFiltersConfig();
  }
  onCommonSelectAllItems(item:any) {
    this.controlGridData=[];
    if(item[0].Floor_Id >0)
    {
      this.form.value.ddlfloors = item;
    }
    else if(item[0].Wing_Id >0)
    {
      this.form.value.ddlwings = item;
    }
    else if(item[0].Room_Id >0)
    {
      this.form.value.ddlrooms = item;
    }
    else if(item[0].Bed_Id >0)
    {
      this.form.value.ddlbeds = item;
    }
    this.getFiltersConfig();
  }
  onCommonDeSelectItems(item:any) {
    this.controlGridData=[];
    this.getFiltersConfig();
  }
  onCommonDeSelectAllItems(item:any) {
    this.controlGridData=[];
    if(item == "ddlfloors")
    {
      this.form.value.ddlfloors.length = 0;
    }
    else if(item == "ddlwings")
    {
      this.form.value.ddlwings.length = 0;
    }
    else if(item == "ddlrooms")
    {
      this.form.value.ddlrooms.length = 0;
    }
    else if(item == "ddlbeds")
    {
      this.form.value.ddlbeds.length = 0;
    }
    this.getFiltersConfig();
  }
  // onFloorSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onFloorSelectAll(item: any) {
  //   this.filterform.value.ddlfloors = item;
  //   this.getFiltersConfig();
  // }
  // onFloorDeSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onFloorDeSelectAll(item: any) {
  //   this.filterform.value.ddlfloors.length = 0;
  //   this.getFiltersConfig();
  // }

  // onWingSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onWingSelectAll(item: any) {
  //   this.filterform.value.ddlwings = item;
  //   this.getFiltersConfig();
  // }
  // onWingDeSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onWingDeSelectAll(item: any) {
  //   this.filterform.value.ddlwings.length = 0;
  //   this.getFiltersConfig();
  // }
  // onRoomSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onRoomSelectAll(item: any) {
  //   this.filterform.value.ddlrooms = item;
  //   this.getFiltersConfig();
  // }
  // onRoomDeSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onRoomDeSelectAll(item: any) {
  //   this.filterform.value.ddlrooms.length = 0;
  //   this.getFiltersConfig();
  // }
  // onBedSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onBedSelectAll(item: any) {
  //   this.filterform.value.ddlbeds = item;
  //   this.getFiltersConfig();
  // }
  // onBedDeSelect(item: any) {
  //   this.getFiltersConfig();
  // }
  // onBedDeSelectAll(item: any) {
  //   this.filterform.value.ddlbeds.length = 0;
  //   this.getFiltersConfig();
  // }
  getResidentDropData() {
    this.filterConfigs = {
      CompanyId: 0,
      CompanyTobedFlag: this.companyToBed,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
      Facilities: this.filterform.value.ddlfacilities.length != 0 ? this.filterform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.filterform.value.ddlnursestations.length != 0 ? this.filterform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0) ? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
      History: this.history == false ? 0 : 1,
      UserName: this.showHistroyform.value.showUsername,
      Password: this.showHistroyform.value.showPass,
      User_Id: this.persistanceService.get(this.config.loggedInUserKey),
      ResidentId: 0
    }
    this.dataservice.post(this.config.Emar_Orders_GetControlSubstanceResDrop, this.filterConfigs)
      .subscribe(res =>
        this.residents = res,
        error => this.alertService.error(error.message)
      );
  }
  getConsolidateOrders(residentId: number,gpi:any,ekitflag:number) {
     debugger
    if(ekitflag==0)
    {
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetControlSubstanceGridDataByPid + residentId+"/"+(gpi==""?null:gpi) +"/"+0)
      .subscribe(res => {
         
        this.consolidateOrders = res;
        if (this.consolidateOrders.length > 1) {
          this.consolidateQuantity="";
          this.CheckAll = true;
          this.consolidateOrderSave=[];
          this.consolidateInitialQty=0;
          this.consolidateAministerQty=0;
          this.consolidateOrders.forEach(element => {
            this.consolidateOrderSave.push(element);
            //this.consolidateInitialQty=this.consolidateInitialQty+(element.LastCertified=='None'?0: parseFloat(element.InitialQuantity));
            //this.consolidateAministerQty=this.consolidateAministerQty+(element.LastCertified=='None'?0:parseFloat(element.AdministeredQty));
          });
          // this.consolidateQuantity=this.consolidateAministerQty;
          this.modalConsolidateIsOpen = true;
          if (this.consolidateOrderSave.length <= 1) {
            this.consolidateBtn = true;
          }
          else {
            this.consolidateBtn = false;
          }
        this.ng4LoadingSpinnerService.hide();
        }
        else
        {
          //this.alertService.warn("No orders to combine inventory");
          this.ng4LoadingSpinnerService.hide();
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
}
  changeResident() {
    //this.ng4LoadingSpinnerService.show();
    if (this.residentddlform.value.ddlresidents.length == 0) {
      this.getControlledMedsData();
    }
    else {
      //this.getControlSubstanceGridByPId(this.residentddlform.value.ddlresidents[0].Patient_Id);
    }
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
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
              else if(this.nurseStations!=undefined && this.nurseStations.length>0)
          {
            this.selectednItems.push(this.nurseStations[0]);
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
          }
            }
            this.filterform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            //this.getFiltersDataBySelection(this.userId);
            this.getCompanyToBedByNurseStation();
            this.getFiltersConfig();
          }
        }

        else if (this.facilities.length == 1) {
          this.filterform.patchValue({
            ddlnursestations: this.nurseStations,
          });
          this.getFiltersConfig();
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
  onResidentSelect(item: any) {
    this.changeResident();
  }
  onResidentDeSelect(item: any) {
    this.changeResident();
  }

  getCompanyToBedByNurseStation() {
    this.nstations = "";
    let facilityId = this.filterform.value.ddlfacilities[0].Facility_Id;
    let arNurseStations = this.filterform.value.ddlnursestations;
    if (arNurseStations.length != 0) {
      arNurseStations.forEach(element => {
        this.nstations += element.NurseStation_Id + ",";
      });
      this.nstations = this.nstations.substring(0, this.nstations.length - 1);
      this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" + facilityId + "/" + this.nstations)
        .subscribe(res => {
          if (res.companyBedFlag == 1) {
            this.companyToBed = res.companyBedFlag;
            this.floors = res.Floors;
            this.wings = res.Wings;
            this.rooms = res.Rooms;
            this.beds = res.Beds;
          }
          else if (res.companyBedFlag == 0) {
            this.companyToBed = 0;
            this.floors = [];
            this.wings = [];
            this.rooms = [];
            this.beds = [];
          }
          this.getNurseStationHierarchyDetailsbyNsId(this.filterform.value.ddlnursestations[0].NurseStation_Id);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (arNurseStations.length == 0) {
     // this.getCompanyToBedData(facilityId);
    }
  }
  getNurseStationHierarchyDetailsbyNsId(nsId:any)
  {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyBedMapping_GetNurseStationHierarchyDetailsbyNsId + nsId)
      .subscribe((res: any) => {
    
            if(res == null && this.companyToBed !=0)
            {
              this.floorIndex =1;
              this.wingIndex =2;
              this.roomIndex =3;
              this.bedIndex =4;  
            }
            else if(res != null && this.companyToBed!=0){
            this.floorIndex =res.FloorPrior;
            this.wingIndex =res.WingPrior;
            this.roomIndex =res.RoomPrior;
            this.bedIndex =res.BedPrior;
           }
           else if((res == null && this.companyToBed ==0) || this.companyToBed==0)
           {
            this.floorIndex =0;
            this.wingIndex =0;
            this.roomIndex =0;
            this.bedIndex =0;  
           }
            this.fields = [
              {
                label: 'Floor',
                name: 'ddlfloors',
                data: this.floors,
                settings: {
                  singleSelection: false,
                  idField: "Floor_Id",
                  textField: "Floor_Name",
                  text: "Floors",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedflItems,
                placeholder: 'Floors',
                index:this.floorIndex
              },
              {
                label: 'Wing',
                name: 'ddlwings',
                data: this.wings,
                settings: {
                  singleSelection: false,
                  idField: "Wing_Id",
                  textField: "Wing_Desc",
                  text: "Wings",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedwItems,
                placeholder: 'Wings',
                index:this.wingIndex
              },
              {
                label: 'Room',
                name: 'ddlrooms',
                data: this.rooms,
                settings: {
                  singleSelection: false,
                  idField: "Room_Id",
                  textField: "Room_Name",
                  text: 'Rooms',
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedflItems,
                placeholder: 'Rooms',
                index:this.roomIndex
              },
              {
                label: 'Bed',
                name: 'ddlbeds',
                data: this.beds,
                settings: {
                  singleSelection: false,
                  idField: "Bed_Id",
                  textField: "Bed_Name",
                  text: "Beds",
                  selectAllText: "Select All",
                  unSelectAllText: "UnSelect All",
                  itemsShowLimit: 1,
                  allowSearchFilter: this.ShowFilter
                },
                ngModel: this.selectedbItems,
                placeholder: 'Beds',
                index:this.bedIndex
              }
            ];
       
             let fieldsCtrls = {};
             for (let f of this.fields) {
                 fieldsCtrls[f.name] = new FormControl(f.value)
             }
            this.form = new FormGroup(fieldsCtrls);
            this.dataFields=this.fields.filter(s=>s.index != 0); 
            this.dataFields.sort((a, b) => {
              if(a.index > b.index) {
                return 1;
              } else if(a.index < b.index) {
                return -1;
              } else {
                return 0;
              }
              
            });
            this.fields =this.dataFields;
          
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  clear(Id: any,ekitflag:number) {
    if(ekitflag==0)
    {
    let element = "#" + Id;
    $(element).val("");
    }
    else if(ekitflag==1)
    {
    let element = "#ek" + Id;
    $(element).val("");
    }
  }
  getControlSubstaceHistoryById(qtyId:any,ekitflag:number,ekitId:number)
  {
    debugger;
    if(ekitflag==0)
    {
    this.dataservice.get<any[]>(this.config.Emar_GetControlSubstanceTrans+ qtyId)
      .subscribe(res => {
        this.certifyControlledMedsHistory=res;
      this.modalControlledSubstanceHistoryIsOpen=true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
    else if(ekitflag==1)
    {
      this.dataservice.get<any[]>(this.config.Emar_GetEkitControlSubstanceTrans+ ekitId)
      .subscribe(res => {
        this.certifyControlledMedsHistory=res;
      this.modalControlledSubstanceHistoryIsOpen=true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
  }
  getControlSubstaceHistoryByIdtoolTip(qtyId:any,ekitflag:number,ekitId:number)
  {
    if(ekitflag==0)
    {
    this.dataservice.get<any[]>(this.config.Emar_GetControlSubstanceTrans+ qtyId)
      .subscribe(res => {
        this.certifyControlledMedsHistory=res;
      //this.modalControlledSubstanceHistoryIsOpen=true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
    else if(ekitflag==1)
    {
      this.dataservice.get<any[]>(this.config.Emar_GetEkitControlSubstanceTrans+ ekitId)
      .subscribe(res => {
        this.certifyControlledMedsHistory=res;
      //this.modalControlledSubstanceHistoryIsOpen=true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
    }
  }
  closeModel()
  {
    this.modalControlledSubstanceHistoryIsOpen=false;
  }
  onselectRecord(event:any,item:any)
  {
     
    if (event == true) {
      this.consolidateQuantity="";
      this.consolidateOrderSave.push(item);
      if (this.consolidateOrderSave.length <= 1) {
        this.consolidateBtn = true;
      }
      else {
        this.consolidateBtn = false;
      }
    }
    else {
      this.consolidateQuantity="";
      const index = this.consolidateOrderSave.findIndex(i => i.QuantityId == item.QuantityId);
      this.consolidateOrderSave.splice(index, 1);
      this.removeconsolidateQuantity="";
      if (this.consolidateOrderSave.length <= 1) {
        this.consolidateBtn = true;
      }
      else {
        this.consolidateBtn = false;
      }
    }
  }
  confirmConsolidate()
  {
    if(this.consolidateQuantity=="")
    {
      this.alertService.warn("Please enter numeric quantity");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.consolidateOrderSave.length<=1)
    {
      this.alertService.warn("Select at least two orders to consolidate");
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      //this.consolidateConfirmationModal=true;
      this.consolidateConfirmationModal=false;
      this.modalSave = true;
      this.saveform.reset();
      //this.showHistroyform.reset();
      this.saveform.patchValue({
        certUsername: '',
        certPass: ''
      });
      this.ng4LoadingSpinnerService.hide();
    }
  }
  saveConsolidate()
  {
    let objects=[];
    this.consolidateOrderSave.forEach(element => {
      let obj=
      {
        POrderId:element.PorderId,
        PQuantity_Id:element.QuantityId,
      }
      objects.push(obj);
    });
    let obj=
    {
      Quantity:this.consolidateQuantity,
      Cert_UserName: this.saveform.value.certUsername,
      Cert_Password: this.saveform.value.certPass,
      Approval_UserName: this.saveform.value.approvalUsername,
      Approval_Password: this.saveform.value.approvalPass,
      ConsolidateOrders:objects,
      User_Id: this.persistanceService.get(this.config.loggedInUserKey),
      Facility_Id: this.filterform.value.ddlfacilities[0].Facility_Id,
      ConsolidatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      Reason:this.conDiscrepencyReason,
    }
    this.dataservice.post(this.config.Emar_Orders_InsertConsolidateOrders,obj)

    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      if (res == "Success") {
        this.alertService.success("Save successful");
        this.consolidateOrderSave=[];
        this.consolidateOrders=[];
        this.consolidateQuantity="";
        this.modalConsolidateIsOpen = false;
        this.modalSave=false;
        this.getControlledMedsData();
        this.myform.patchValue({ quantity: "" });
      }
      else{
        this.alertService.error(res);
        this.ng4LoadingSpinnerService.hide();
      }

    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
  closeConsolidateModel()
  {
    this.modalConsolidateIsOpen=false;
    this.modalRemoveConsolidate=false;
  }
  closeConfirmation()
  {
  this.consolidateConfirmationModal=false;
  }
  openWitnessModal()
  {
    this.consolidateConfirmationModal=false;
      this.modalSave = true;
      this.saveform.reset();
      //this.showHistroyform.reset();
      this.saveform.patchValue({
        certUsername: '',
        certPass: ''
      });
  }
  openRemoveConsolidateModal(residentId:any,gpi:any)
  {
    debugger;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetControlSubstanceGridDataByPid + residentId+"/"+gpi +"/"+1)
    .subscribe(res => {
      this.removeconsolidateOrders = res;
      if (this.removeconsolidateOrders.length > 0) {
        this.removeconsolidateQuantity="";
        this.CheckAll = true;
        this.removeconsolidateOrdersSave=[];
        this.removeconsolidateOrders.forEach(element => {
          this.removeconsolidateOrdersSave.push(element);
        });
        this.modalRemoveConsolidate = true;
        setTimeout(() => {
          this.quantityFocus.nativeElement.focus()
        }, 300);
      this.ng4LoadingSpinnerService.hide();
      }
      else
      {
        //this.alertService.warn("No orders to combine inventory");
        this.ng4LoadingSpinnerService.hide();
      }
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

  openRemoveConsolidateModaltooltip(residentId:any,gpi:any)
  {
    debugger;
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetControlSubstanceGridDataByPid + residentId+"/"+gpi +"/"+1)
    .subscribe(res => {
      this.removeconsolidateOrders = res;
      if (this.removeconsolidateOrders.length > 0) {
        this.removeconsolidateQuantity="";
        this.CheckAll = true;
        this.removeconsolidateOrdersSave=[];

        this.toolTipMes = '';
        this.removeconsolidateOrders.forEach(element => {
          //this.removeconsolidateOrdersSave.push(element);
          this.toolTipMes = this.toolTipMes +''+element.Directions ;
        });
        this.modalRemoveConsolidate = true;
        setTimeout(() => {
          this.quantityFocus.nativeElement.focus()
        }, 300);
      this.ng4LoadingSpinnerService.hide();
      }
      else
      {
        //this.alertService.warn("No orders to combine inventory");
        this.ng4LoadingSpinnerService.hide();
      }
    },
      error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  checkConsolidateQty(conQty:any)
  {
     
    if(this.consolidateQuantity=="")
    {
      //this.alertService.warn("Please enter quantity");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.consolidateOrderSave.length<=1)
    {
      this.alertService.warn("Select at least two orders to consolidate");
      this.consolidateQuantity="";
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.consolidateQuantity!="")
    { 
    this.consolidateQuantity=parseFloat(this.consolidateQuantity).toFixed(2); 
    this.consolidateInitialQty=0;
    this.consolidateAministerQty=0;
    if(this.consolidateOrderSave.filter(co=>co.LastCertified=='None').length==this.consolidateOrderSave.length)
    {
      this.consolidateInitialQty=0;
      this.consolidateAministerQty=0;
    }
    else{
      this.consolidateOrderSave.forEach(element => {
        //this.consolidateInitialQty=this.consolidateInitialQty+(element.LastCertified=='None'?0: parseFloat(element.InitialQuantity));
        this.consolidateAministerQty=this.consolidateAministerQty+(element.LastCertified=='None'?0:parseFloat(element.AdministeredQty));
      });
    }
    if(this.consolidateAministerQty==0 || ((this.consolidateAministerQty)== this.consolidateQuantity))
    {
      this.conDiscrepencyReason="";
      //Nothing
    }
    else
    {
      const modalRef = this.modalService.open(ControlledsubstancediscrepancyreasonComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.title = 'Merge OrdersEnter Reason for incorrect count';
      let orderData = {
        "ResidentName": this.consolidateOrderSave[0].ResidentName,
        "DrugName": this.consolidateOrderSave[0].Order, //+ '(' + this.consolidateQuantity + ')',
      }
      modalRef.componentInstance.orderDetails = orderData;
      modalRef.componentInstance.reason.subscribe((receivedResult) => {
        if (receivedResult != "") {
          this.conDiscrepencyReason = receivedResult;
        }
        else {
          this.consolidateQuantity='';
        }
    modalRef.close();
    });
  }
  }
  }
  onselectRemoveConsolidateRecord(event:any, item:any){
    if (event == true) {
      this.removeconsolidateQuantity="";
      this.removeconsolidateOrdersSave.push(item);
    }
    else {
      const index = this.removeconsolidateOrdersSave.findIndex(i => i.QuantityId == item.QuantityId);
      this.removeconsolidateOrdersSave.splice(index, 1);
      this.removeconsolidateQuantity="";
    }
  }
  removeCombineQty()
  {
    this.modalSave = true;
      this.saveform.reset();
      //this.showHistroyform.reset();
      this.saveform.patchValue({
        certUsername: '',
        certPass: ''
      });
      this.ng4LoadingSpinnerService.hide();
  }
  removeConsolidate()
  {
    let objects=[];
    this.removeconsolidateOrdersSave.forEach(element => {
      let obj=
      {
        POrderId:element.PorderId,
        PQuantity_Id:element.QuantityId,
      }
      objects.push(obj);
    });
    let obj=
    {
      Quantity:this.removeconsolidateQuantity,
      Cert_UserName: this.saveform.value.certUsername,
      Cert_Password: this.saveform.value.certPass,
      Approval_UserName: this.saveform.value.approvalUsername,
      Approval_Password: this.saveform.value.approvalPass,
      ConsolidateOrders:objects,
      User_Id: this.persistanceService.get(this.config.loggedInUserKey),
      Facility_Id: this.filterform.value.ddlfacilities[0].Facility_Id,
      ConsolidatedOn: this.dateFormatPipe.dateWithTime(new Date()),
      Reason:'',
    }
    this.dataservice.post(this.config.Emar_Orders_RemoveConsolidateOrders,obj)

    .subscribe(res => {
      this.ng4LoadingSpinnerService.hide();
      if (res == "Success") {
        this.alertService.success("Removed successfully");
        this.removeconsolidateOrdersSave=[];
        this.removeconsolidateOrders=[];
        this.removeconsolidateQuantity="";
        this.modalRemoveConsolidate = false;
        this.modalSave=false;
        this.getControlledMedsData();
        this.myform.patchValue({ quantity: "" });
      }
      else{
        this.alertService.error(res);
        this.ng4LoadingSpinnerService.hide();
      }

    }, error => {
      this.alertService.error(error.message);
      this.ng4LoadingSpinnerService.hide();
    });
  }
}
