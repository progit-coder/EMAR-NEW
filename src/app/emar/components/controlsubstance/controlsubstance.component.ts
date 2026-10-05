import { User } from 'ng-chat';
import { Component, OnInit } from '@angular/core';
import { NurseStation, Floor, Wing } from '../../../models/facility.model';
import { DrFirstIntegration, HLSeven } from '../../../models/drfirstintegration.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { AlertService } from '../../../_services/index';
import { numberFormat } from 'highcharts';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';

@Component({
  selector: 'app-controlsubstance',
  templateUrl: './controlsubstance.component.html',
  styleUrls: ['./controlsubstance.component.css']
})
export class ControlsubstanceComponent implements OnInit {
  PrescriptionID: string;
  selectedResident: number;
  collapse: boolean = false;
  residents: any[];
  private residentID: number;
  facilityId: any;
  myform: FormGroup;
  buttonform: FormGroup;
  public template;
  public nurseStations: NurseStation[];
  public defaultNurstationId: number = 0;
  arfloor = []; arwing = [];
  arnstation = [];
  private filterConfigs: any = [];
  private controlType: string = "NW";
  private type: string;
  public ordersList: any[] = [];
  public filterData: any = [];
  public floors: Floor[];
  public wings: Wing[];
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_NurseStations: any = {};
  ShowFilter = false;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  i: number = 0;
  j: number;
  oPatient_Id: number;
  oGiveCodeidentifier: any;
  oDrug: string;
  oPrescriptionID: string;
  oRouteText: string;
  oGiveDosageForm: string;
  oAdditionalInst: string;
  oQuantity: string;
  oNumberOfRefill: string;
  oStrength: string;
  dPatient_Id: number;
  dGiveCodeidentifier: any;
  dDrug: string;
  dPrescriptionID: string;
  dRouteText: string;
  dGiveDosageForm: string;
  dAdditionalInst: string;
  dQuantity: string;
  dNumberOfRefill: string;
  dStrength: string;
  user: string;
  pwd: string;
  date: string = new Date().toISOString();
  public userId: number;
  public drFirstOrderInfo: any[] = [];
  public hl7DrFirstOrderInfo: any[] = [];
  public MapButton: number = 0;
  public historyData: any[] = [];
  public history: number = 0;
  public hlObj: HLSeven;
  historyCheck;

  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService, private persistanceService: PersistanceService
    , private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService) { }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.myform = new FormGroup({
      ddlnursestations: new FormControl(''),
      ddlfloors: new FormControl(''),
      ddlwings: new FormControl(''),
      ddlresidents: new FormControl('')
    });
    this.buttonform = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),



    });
    this.sharedService.currentPatientId.subscribe(patientId => this.residentID = patientId);
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    //this.getNurseStations();
    //this.getOrdersGirddata();
    this.getFiltersData(this.userId);
    this.getResidentDropData(this.userId);
    this.user = this.buttonform.value.username;
    this.pwd = this.buttonform.value.password;
    if (this.myform.value.ddlresidents == '') {
      this.alertService.error("Please Select Resident");
    }
    this.userActivity();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.DrControlSubstance, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getResidentDropData(userId:number) {
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstResidentDrop + userId)
      .subscribe(res => this.residents = res, error => this.alertService.error(error.message)
      );
  }
  // getResidentDropData(userId: number) {
  //   this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentDropData + userId)
  //     .subscribe(res => {
  //       this.residents = res;
  //     }, error => this.alertService.error(error.message));
  // }
  selectedRecords: any[] = [];
  maxNo = false;
  amt = 0;
  onDrFirstSelect(event, type: number, item: any) {
    if (event == true && type == 1)
      this.amt++
    else if (event == false && type == 1)
      this.amt--
    if (type == 1)
      this.amt === 1 ? this.maxNo = true : this.maxNo = false;
    if (event == true) {
      //if (type == 1) {
      //const index = this.selectedRecords.findIndex(i => i.item == item.item);
      //if(index!=-1)
      //this.selectedRecords.splice(index, 1);
      //this.selectedRecords.push({ "type": type, "Prescription": item, "ApprovalBy": this.userId });
      //}
      //else
      this.selectedRecords.push({ "type": type, "Prescription": item, "ApprovalBy": this.userId });
    }
    if (event == false) {
      const index = this.selectedRecords.findIndex(i => i.item == item.item);
      this.selectedRecords.splice(index, 1);
    }
    this.selectedRecords.sort((t1, t2) => {
      const data1 = t1.type;
      const data2 = t2.type;
      if (data1 > data2) { return 1; }
      if (data1 < data2) { return -1; }
      return 0;
    });
  }
  ResetFlag() {
    this.maxNo = false;
    this.amt = 0;
  }
  InsertDrFirstMaping() {

    if (this.selectedRecords.length == 0 || this.selectedRecords.length < 2) {
      this.alertService.warn("Please Select Proper Data To Map");
    }
    else if (this.selectedRecords[0].type != 1) {
      this.alertService.warn("Please Select Dr.First Order.")
    }
    else if (this.selectedRecords[1].type != 2) {
      this.alertService.warn("Please Select Pharmacy Order");
    }
    else {
      this.dataservice.post(this.config.Emar_DrFirstIntegration_InsertDrFirstMap, this.selectedRecords)
        .subscribe(res => {
          this.selectedRecords = [];
          this.ResetFlag();
          this.alertService.success("Maped Successfully");
          this.getDrFirstOrderCheck();
          this.getHL7DrFirstOrderCheck(this.myform.value.ddlresidents);
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      this.selectedRecords = [];
    }
  }
  // getResidentDropData() {
  //   let userId = this.persistanceService.get(this.config.loggedInUserKey);
  //   this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetResidentDropData + userId)
  //     .subscribe(res => {
  //       this.residents = res;
  //       if (this.residentID == 0) {
  //         this.residentID = this.residents[0].Patient_Id;
  //       }
  //     }, error => this.alertService.error(error.message));
  // }
  getOrdersGirddata() {
    if (this.myform.value.ddlfloors.length != 0) {
      this.arfloor.length = 0;
      this.myform.value.ddlfloors.forEach(item => this.arfloor.push(item.Floor_Id));
    }
    else if (this.myform.value.ddlfloors.length == 0) {
      this.arfloor.length = 0;
    }

    if (this.myform.value.ddlwings.length != 0) {
      this.arwing.length = 0;
      this.myform.value.ddlwings.forEach(item => this.arwing.push(item.Wing_Id));
    }
    else if (this.myform.value.ddlwings.length == 0) {
      this.arwing.length = 0;
    }
    this.filterConfigs = {
      CompanyID: 0,
      NurseStations: this.arnstation,
      Floors: this.arfloor,
      Wings: this.arwing,
      ControlType: this.controlType,
      Type: this.type,
      User_Id: this.persistanceService.get(this.config.loggedInUserKey),
    }

    this.dataservice.post(this.config.Emar_Orders_GetOrdersGridData, this.filterConfigs)
      .subscribe((res: any) => {

        this.ordersList = res;
      }, error => {
      });
  }
  getNurseStations() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserNurseStations + userId)
      .subscribe(res => {
        this.nurseStations = res;
        this.defaultNurstationId = this.nurseStations[0].NurseStation_Id;
        this.ng4LoadingSpinnerService.hide()
      },
        error => {

        });
  }
  getFiltersConfig(filterType: string): any {
    if (this.myform.value.ddlnursestations.length != 0) {
      this.arnstation.length = 0;
      this.myform.value.ddlnursestations.forEach(item => this.arnstation.push(item.NurseStation_Id));
    }
    else if (this.myform.value.ddlnursestations.length == 0) {
      this.arnstation.length = 0;
    }

    if (this.myform.value.ddlfloors.length != 0) {
      this.arfloor.length = 0;
      this.myform.value.ddlfloors.forEach(item => this.arfloor.push(item.Floor_Id));
    }
    else if (this.myform.value.ddlfloors.length == 0) {
      this.arfloor.length = 0;
    }

    if (this.myform.value.ddlwings.length != 0) {
      this.arwing.length = 0;
      this.myform.value.ddlwings.forEach(item => this.arwing.push(item.Wing_Id));
    }
    else if (this.myform.value.ddlwings.length == 0) {
      this.arwing.length = 0;
    }
    this.filterConfigs = {
      CompanyID: 0,
      Facility_Id: this.facilityId,
      NurseStations: this.arnstation,
      Floors: this.arfloor,
      Wings: this.arwing,
      ControlType: this.controlType,
      Type: this.type,
      User_Id: this.persistanceService.get(this.config.loggedInUserKey),
    }
    this.dataservice.post(this.config.Emar_Orders_GetOrdersGridData, this.filterConfigs)
      .subscribe((res: any) => {
        this.ordersList = res;
      }, error => {
      });
  }
  getFiltersData(userId: number): any {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetCompanyToBedData + userId)
      .subscribe((res: any) => {

        this.filterData = res;
        this.nurseStations = res.NurseStations;
        this.floors = res.Floors;
        this.wings = res.Wings;
      }, error => { });
    this.dropdownSettings_NurseStations = {
      singleSelection: false,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      text: "NurseStations",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };

    this.dropdownSettings_Floors = {
      singleSelection: false,
      idField: "Floor_Id",
      textField: "Floor_Name",
      text: "Floors",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };

    this.dropdownSettings_Wings = {
      singleSelection: false,
      idField: "Wing_Id",
      textField: "Wing_Desc",
      text: "Wings",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: this.ShowFilter
    };
    this.ng4LoadingSpinnerService.hide();
  }
  onNurseStationSelect(item: any) {
    this.getFiltersConfig('nstation');
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestations = item;
    this.getFiltersConfig('nstation');
  }
  onNurseStationDeSelect(item: any) {
    this.getFiltersConfig('nstation');
  }
  onNurseStationDeSelectAll(item: any) {
    this.myform.value.ddlnursestations.length = 0;
    this.getFiltersConfig('nstation');
  }
  onFloorSelect(item: any) {
    this.getFiltersConfig('floor');
  }
  onFloorSelectAll(item: any) {
    this.myform.value.ddlfloors = item;
    this.getFiltersConfig('floor');
  }
  onFloorDeSelect(item: any) {
    this.getFiltersConfig('floor');
  }
  onFloorDeSelectAll(item: any) {
    this.myform.value.ddlfloors.length = 0;
    this.getFiltersConfig('floor');
  }
  onWingSelect(item: any) {
    this.getFiltersConfig('wing');
  }
  onWingSelectAll(item: any) {
    this.myform.value.ddlwings = item;
    this.getFiltersConfig('wing');
  }
  onWingDeSelect(item: any) {
    this.getFiltersConfig('wing');
  }
  onWingDeSelectAll(item: any) {
    this.myform.value.ddlwings.length = 0;
    this.getFiltersConfig('wing');
  }
  // onRowSelect(porderId,patientId)
  // {
  //   
  // }
  // onRowSelect(orderId: number) {
  //   this.dataservice.get<DrFirstIntegration[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderInfo + orderId)
  //     .subscribe(res => {
  //       this.fetchDetail(res);

  //     },
  //        error => {
  //       this.alertService.error(error.message);
  //     });
  // }
  getDrFirstOrderCheck() {
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderCheck + this.residentID)
      .subscribe(res => {
        this.drFirstOrderInfo = [];
        this.drFirstOrderInfo = res.filter(item => item.DrFirstOrderXMLTrans_Approval == 0);
        this.getHL7DrFirstOrderCheck(this.residentID);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
          this.MapButton = 0;
        });
  }
  getHL7DrFirstOrderCheck(patientId: number) {
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetHL7DrFirstOrderCheck + patientId)
      .subscribe(res => {
        this.hl7DrFirstOrderInfo = res;
        if (this.drFirstOrderInfo.length > 0 && this.hl7DrFirstOrderInfo.length > 0) {
          this.MapButton = 1;
        }
        else {
          this.MapButton = 0;
        }
      },
        error => {
          this.alertService.error(error.message);
        });
  }

  // fetchDetail(res:DrFirstIntegration[])
  // {

  //   if(res.length>0)
  //   {
  //     if(res[0])
  //     {
  //       this.oGiveCodeidentifier=res[0].GiveCodeIdentifier;
  //       this.oPatient_Id=res[0].Patient_Id;
  //       this.oDrug=res[0].Drug;
  //       this.oPrescriptionID=res[0].PrescriptionID;
  //       this.oRouteText=res[0].RouteText;
  //       this.oGiveDosageForm=res[0].GiveDosageForm;
  //       this.oAdditionalInst=res[0].AdditionalInst
  //       this.oQuantity=res[0].Quantity;
  //       this.oNumberOfRefill=res[0].NumberofRefill;
  //       this.oStrength=res[0].Strength;


  //     }
  //     if(res[1])
  //     {
  //       this.dGiveCodeidentifier=res[1].GiveCodeIdentifier;
  //       this.dPatient_Id=res[1].Patient_Id;
  //       this.dDrug=res[1].Drug;
  //       this.dPrescriptionID=res[1].PrescriptionID;
  //       this.dRouteText=res[1].RouteText;
  //       this.dGiveDosageForm=res[1].GiveDosageForm;
  //       this.dAdditionalInst=res[1].AdditionalInst
  //       this.dQuantity=res[1].Quantity;
  //       this.dNumberOfRefill=res[1].NumberofRefill;
  //       this.dStrength=res[1].Strength;


  //     }
  //   }
  // }

  updateApprovalStatus() {

    this.dataservice.get<any>(this.config.Emar_DrFirstIntegration_UpdateApprovalStatus + this.oPrescriptionID + "/" + this.buttonform.value.username + "/" + this.buttonform.value.password)
      .subscribe(res => {

        this.alertService.success("Save successful");

      }, error => {
        this.alertService.error(error.message)
      });
  }
  changeResident() {
    if (this.myform.value.ddlresidents != "") {
      this.MapButton = 0;
      this.history = 0;
      this.historyCheck = 0;
      this.selectedResident = this.myform.value.ddlresidents;
      this.sharedService.changePatientId(this.selectedResident);
      this.getDrFirstOrderCheck();
      this.ResetFlag();
    }
    else {
      this.drFirstOrderInfo = [];
      this.hl7DrFirstOrderInfo = [];
      this.MapButton = 0;
    }
  }
  getHistory(event) {
    if (event == true) {
      if (this.myform.value.ddlresidents == "") {
        this.residentID = 0;
      }
      else {
        this.residentID = this.myform.value.ddlresidents;
      }
      this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_PrcGetReportDrFirst + this.residentID)
        .subscribe(res => {

          this.historyData = res;
          this.history = 1;
          this.MapButton = 0;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message)

        });
    }
    else if (event == false) {
      this.changeResident();
      // this.getDrFirstOrderCheck(this.residentID);
      this.history = 0;
      //this.MapButton=1;
    }
  }
  ApproveHlRecord(porderId: number, dadminId: number) {
    this.hlObj = {
      ApprovalStatus: 3,
      ApprovedBy: this.persistanceService.get(this.config.loggedInUserKey),
      ApprovedDate: new Date().toISOString(),
      POrderId: porderId,
      DAdminId: dadminId,
    }
    this.dataservice.post(this.config.Emar_DrFirstIntegration_InsertHlSevenApprove, this.hlObj)
      .subscribe(res => {
        this.alertService.success("Saved Succesfully");
        this.getHL7DrFirstOrderCheck(this.residentID);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  ApproveDrFirstRecord(DrFirstOrderId: number) {
    this.dataservice.get(this.config.Emar_DrFirstIntegration_InsertDrFirstApprove + this.userId + "/" + DrFirstOrderId)
      .subscribe(res => {
        this.alertService.success("Saved Succesfully");
        this.getDrFirstOrderCheck();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getActiveDrFirst() {
    this.dataservice.get<any[]>(this.config.Emar_DrFirstIntegration_GetDrFirstOrderCheck + this.residentID)
      .subscribe(res => {
        this.drFirstOrderInfo = [];
        this.drFirstOrderInfo = res.filter(item => item.DrFirstOrderXMLTrans_Approval == 1);
        this.getHL7DrFirstOrderCheck(this.residentID);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
          this.MapButton = 0;
        });

  }
}
