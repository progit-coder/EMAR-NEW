import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { Floor, Wing, NurseStation, Bed, Room, } from '../../../models/facility.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services/index';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Screens, Activity } from '../../../models/useractivity.model';
import { OrdersendingsoonComponent } from '../ordersendingsoon/ordersendingsoon.component';
import { OrdersgridprndocumentationComponent } from '../ordersgridprndocumentation/ordersgridprndocumentation.component';
import { Ordersgrid72hourscheckComponent } from '../ordersgrid72hourscheck/ordersgrid72hourscheck.component';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs-compat';
export interface Food {
  value: string;
  viewValue: string;
  img: string;
}

@Component({
  selector: 'app-ordergrid',
  templateUrl: './ordergrid.component.html',
  styleUrls: ['./ordergrid.component.css'],
  providers: [DataService, APIConfiguration]
})
export class OrdergridComponent implements OnInit {
  public form: FormGroup;
  visitStatusForm: FormGroup;
  myform: FormGroup;
  public template;
  errorMessage: string;
  public ordersList: any[] = [];
  public filterData: any = [];
  private filterConfigs: any = [];
  public floors: Floor[];
  public wings: Wing[];
  public rooms: Room[];
  public beds: Bed[];
  public facilities: any[];
  public nurseStations: NurseStation[];
  private controlType: string = "NW";
  private type: string;
  dropdownSettings_Floors: any = {};
  dropdownSettings_Wings: any = {};
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Rooms: any = {};
  dropdownSettings_Beds: any = {};
  dropdownSettings_Facilities: any = {};
  public selectedfaItems = [];
  public selectednItems = [];
  public selectedflItems = [];
  public selectedwItems = [];
  public selectedrItems = [];
  public selectedbItems = [];
  arfloor = []; arfacility = []; arnstation = []; arwing = []; arroom = []; arbed = [];
  disabled = false;
  ShowFilter = true;
  limitSelection = false;
  gridCount: any;
  p: number = 1;
  q: number = 1;
  r:number = 1;
  public MyImages:any;
  searchText: string = "";
  pendingOrderSearch:string="";
  public companyToBed: number = 0;
  public userId: number;
  endingSoonOrdersList: any[] = [];
  modalOption: NgbModalOptions = {};
  pageConfig = {};
  cpoepageConfig={};
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public nstations: string = "";
  public visitStatusFlag: number = 1;
  public VisitViewFlag: number;
  public orderEndingCount:number =1;
  public fields: any[];
  public dataFields:any[];
  public floorIndex:number =0;
  public wingIndex:number=0;
  public roomIndex:number =0;
  public bedIndex:number =0;
  public seventyTwoHoursDiplayFlag:boolean=false;
  public prnDiplayFlag:boolean=false;
  public prnFilterConfigs: any = [];
  public seventyTwoHoursFilterConfigs: any = [];
  public modalResidentAllOrderIsOpen:boolean=false;
  public modalPendingOrderIsOpen:boolean=false;
  public residentOrdersList:any[]=[];
  public pendingOrdersList:any[]=[];
 
  public MouseX:number = 0;
public MouseY:number= 0;

  foods: Food[] = [
    { value: 'steak-0', viewValue: 'Steak', img: 'https://www.akberiqbal.com/favicon-32x32.png' },
    { value: 'pizza-1', viewValue: 'Pizza', img: 'https://www.akberiqbal.com/favicon-16x16.png' },
    { value: 'tacos-2', viewValue: 'Tacos', img: 'https://www.akberiqbal.com/favicon-96x96.png' }
  ];

  @ViewChild('orderFocus') orderfocus:ElementRef;

  constructor(private dataservice: DataService, private sharedService: SharedService, private config: APIConfiguration, private route: Router, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private alertService: AlertService,
    private modalService: NgbModal, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    window.scroll(0,0);
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
    this.cpoepageConfig = this.persistanceService.getPermissionsByScreen("CPOE");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
         // ddlfloors: new FormControl(''),
          ddlnursestations: new FormControl(''),
          //ddlwings: new FormControl(''),
        //  ddlrooms: new FormControl(''),
         // ddlbeds: new FormControl(''),
        });
        this.visitStatusForm = new FormGroup({
          visitStatus: new FormControl('1'),
        });
        this.form = new FormGroup({
          fields: new FormControl(JSON.stringify(this.fields))
        });

        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        //this.getCompanyToBedData();
        //this.getFiltersData(this.userId);
        this.getUserRecentFacilityNurseStations();
        this.getOrderEndingSoonStatus();
        this.userActivity();

      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Orders, Activity.View, '')
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
    //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        //this.ng4LoadingSpinnerService.hide();
        this.facilities = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
              this.sharedService.changeFacilityId(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        }

       else if (res.Facilities.length == 1) {
        //  this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.sharedService.changeFacilityId(res.Facilities[0].Facility_Id)

          this.myform.patchValue({
            ddlfacilities: this.facilities,
          });
        }
        else
        {
          this.ng4LoadingSpinnerService.hide();
        }
        // if(backClick==true)
        // {
        // this.getBackClickFilterData();
        // }
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
      allowSearchFilter: this.ShowFilter
    };
    // this.dropdownSettings_Floors = {
    //   singleSelection: false,
    //   idField: "Floor_Id",
    //   textField: "Floor_Name",
    //   text: "Floors",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   allowSearchFilter: this.ShowFilter
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
      allowSearchFilter: this.ShowFilter
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
  }
  getCompanyToBedData(facilityId: number) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + userId + "/" + facilityId)
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
        // if (this.facilities.length == 1 && backClick != true) {
        //   this.myform.patchValue({
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
  getDataByVisitStatus() {
    this.visitStatusFlag = this.visitStatusForm.value.visitStatus;
    this.getFiltersDataBySelection(this.userId);
  }
  getFiltersDataBySelection(userId: number, category?: string, result?: any[]): any {
    this.ng4LoadingSpinnerService.show();
    
    let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    if(backClick!=null && backClick==true)
    {
      let visitFlag= JSON.parse(localStorage.getItem("OrdersGridResType"));
      this.visitStatusForm.patchValue({
        visitStatus:visitFlag,
      });
    }
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.ordersList = [];
      this.pendingOrdersList=[];
      this.alertService.error("please use filters to display orders list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.filterConfigs = {
        CompanyID: 0,
        Floors: (this.form.value.ddlfloors !=null &&this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
        Facilities: this.myform.value.ddlfacilities.length != 0 ? this.myform.value.ddlfacilities.map(item => item.Facility_Id) : [],
        NurseStations: this.myform.value.ddlnursestations.length != 0 ? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
        Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
        Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
        Beds: (this.form.value.ddlbeds !=null &&this.form.value.ddlbeds.length != 0) ? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
        ControlType: this.controlType,
        Type: this.type,
        User_Id: this.userId,
        VisitStatus: this.visitStatusForm.value.visitStatus
      }
      
      this.dataservice.post(this.config.Emar_Orders_GetOrdersGridData, this.filterConfigs)
        .subscribe((res: any) => {
          this.ng4LoadingSpinnerService.hide();
          debugger;
          this.ordersList = res;
          //this.getOrdersEndingSoon();
          this.getPendingOrders();
          localStorage.removeItem('ordersBackClick');
          if (this.ordersList.length == 0)
            this.alertService.warn("No data available.");

        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.ordersList = [];
    this.pendingOrdersList=[];
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
    this.myform.patchValue({
      ddlnursestations: '',
      ddlfloors: '',
      ddlwings: '',
      ddlrooms: '',
      ddlbeds: '',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
    this.sharedService.changeFacilityId(item.Facility_Id);

    //this.getCompanyToBedData(item.Facility_Id);
  }
  mouseEnter(Id:any)
  {
    
    //this.MyImages = Id;

   //let userId: number = this.persistanceService.get(this.config.loggedInUserKey);
   this.MyImages= "";
    this.dataservice.get<any>(this.config.Emar_Orders_GetOrdersGetImage + Id )
      .subscribe(res => {
        //this.VisitViewFlag = res;
        //this.ng4LoadingSpinnerService.hide();
        this.MyImages = res
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
  }
  mouseLeave()
  {
    this.MyImages =null;
 
  }
 
  onFacilityDeSelect(item: any) {
    this.orderEndingCount =1;
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.nurseStations = [];
    this.companyToBed = 0;
    this.floors = [];
    this.wings = [];
    this.rooms = [];
    this.beds = [];
    this.ordersList = [];
    this.pendingOrdersList=[];
    this.myform.patchValue({
      ddlnursestations: '',
   //   ddlfloors: '',
     // ddlwings: '',
    //  ddlrooms: '',
    //  ddlbeds: '',
    })
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
    this.getFiltersDataBySelection(this.userId);
    this.getCompanyToBedByNurseStation();
    this.modalService.dismissAll();
    this.getOrdersEndingSoon();
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();

  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestations = item;
    this.getFiltersDataBySelection(this.userId);
    this.getOrdersEndingSoon();
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();
  }
  onNurseStationDeSelect(item: any) {
    this.orderEndingCount =1;
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
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
    this.getFiltersDataBySelection(this.userId);
    //this.getOrdersEndingSoon();
  }
  onNurseStationDeSelectAll(item: any) {
    
    this.ordersList = [];
    this.pendingOrdersList=[];
    this.alertService.error("Please select at least one nursing station to display the data.")
    
  }
  onCommonSelectItems(item:any) {
    this.getFiltersDataBySelection(this.userId);
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();
  }
  onCommonSelectAllItems(item:any) {
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
    this.getFiltersDataBySelection(this.userId);
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();
  }
  onCommonDeSelectItems(item:any) {
    this.getFiltersDataBySelection(this.userId);
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();
  }
  onCommonDeSelectAllItems(item:any) {
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
    this.getFiltersDataBySelection(this.userId);
    this.prnDiplayFlag=false;
    this.seventyTwoHoursDiplayFlag=false;
    this.getSeventyTwoHourCheckDetails();
    this.getPRNCheckDetails();
  }
  // onFloorSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorSelectAll(item: any) {
  //   this.myform.value.ddlfloors = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onFloorDeSelectAll(item: any) {
  //   this.myform.value.ddlfloors.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }

  // onWingSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingSelectAll(item: any) {
  //   this.myform.value.ddlwings = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onWingDeSelectAll(item: any) {
  //   this.myform.value.ddlwings.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomSelectAll(item: any) {
  //   this.myform.value.ddlrooms = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onRoomDeSelectAll(item: any) {
  //   this.myform.value.ddlrooms.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedSelectAll(item: any) {
  //   this.myform.value.ddlbeds = item;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedDeSelect(item: any) {
  //   this.getFiltersDataBySelection(this.userId);
  // }
  // onBedDeSelectAll(item: any) {
  //   this.myform.value.ddlbeds.length = 0;
  //   this.getFiltersDataBySelection(this.userId);
  // }
  getOrderEndingSoonStatus() {
    //this.ng4LoadingSpinnerService.show();
    let userId: number = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any>(this.config.Emar_Orders_OrderEndingSoonStatus + userId + "/" + 29)
      .subscribe(res => {
        this.VisitViewFlag = res;
        //this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getOrdersEndingSoon() {
      if (this.myform.value.ddlfacilities.length != 0 && this.myform.value.ddlnursestations.length != 0) {
        this.ng4LoadingSpinnerService.show();
        this.nstations = "";
        let arNurseStations = this.myform.value.ddlnursestations;
        arNurseStations.forEach(element => {
          this.nstations += element.NurseStation_Id + ",";
        });
        this.nstations = this.nstations.substring(0, this.nstations.length - 1);
        this.dataservice.get<any>(this.config.Emar_Orders_GetOrdersEndingSoon + this.nstations)
        .subscribe(res => {
          
          this.orderEndingCount =0;
             if(res.m_Item2 <=0)
             {
               this.orderEndingCount =1;
             }
             
            else if(res.m_Item2 > 0)
            {
              if (this.VisitViewFlag == 0) {
              this.modalOption.size = 'lg';
              const modalRef = this.modalService.open(OrdersendingsoonComponent, this.modalOption);
              modalRef.componentInstance.nsStationsList =this.nstations;
              modalRef.componentInstance.leaveRequestResult.subscribe((receivedResult) => {
               if (receivedResult.responce == 1) {
                  this.alertService.success("Confirmed End Dates successfully");
                  this.orderEndingCount=receivedResult.count<=0?1:0;
                }
              else if (receivedResult.responce == 2) {
                  this.alertService.error("Something went wrong");
                  this.orderEndingCount=receivedResult.count<=0?1:0;
                }
            modalRef.close();
            this.ng4LoadingSpinnerService.hide();
             });
              }
              //this.ng4LoadingSpinnerService.hide();
            } 
            //this.ng4LoadingSpinnerService.hide();
      });
      }
      else
      {
        this.orderEndingCount =1;
      }
    
    //this.ng4LoadingSpinnerService.hide();
  }
  getOrderSendingSoon() {
    this.VisitViewFlag = 0;
    this.getOrdersEndingSoon();
  }
  // GetTopBoxesFilter(type: string) {
  //   this.controlType = type;
  //   this.getOrdersGirddata();
  // }
  onRowSelect(orderId: number, patientId: number, quantityId: number) {
    
    // localStorage.setItem("OrdersGridFacility", JSON.stringify(this.filterConfigs.Facilities));
    // localStorage.setItem("OrdersGridNurseStations", JSON.stringify(this.filterConfigs.NurseStations));
    // localStorage.setItem("OrdersGridFloors", JSON.stringify(this.filterConfigs.Floors));
    // localStorage.setItem("OrdersGridWings", JSON.stringify(this.filterConfigs.Wings));
    // localStorage.setItem("OrdersGridRooms", JSON.stringify(this.filterConfigs.Rooms));
    localStorage.setItem("OrdersGridResType", JSON.stringify(this.visitStatusForm.value.visitStatus));
    this.sharedService.changePatientId(patientId);
    this.sharedService.changeOrderId(orderId);
    this.sharedService.changeQuantityId(quantityId);
    this.route.navigate(['/home/orderinfo']);
  }
  onResidentSelect(orderId: number, patientId: number, quantityId: number,orderStatus:number)
  {
    debugger;
    if(orderId==0 && quantityId==0)
    {
      this.onRowSelect(orderId,patientId,quantityId);
    }
    else if(orderStatus!=1)
    {
      this.onRowSelect(orderId,patientId,quantityId);
    }
    else
    {
      this.filterConfigs = {
        CompanyID: 0,
        Floors: (this.form.value.ddlfloors !=null &&this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
        Facilities: this.myform.value.ddlfacilities.length != 0 ? this.myform.value.ddlfacilities.map(item => item.Facility_Id) : [],
        NurseStations: this.myform.value.ddlnursestations.length != 0 ? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
        Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
        Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
        Beds: (this.form.value.ddlbeds !=null &&this.form.value.ddlbeds.length != 0) ? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
        ControlType: this.controlType,
        Type: this.type,
        User_Id: this.userId,
        VisitStatus: this.visitStatusForm.value.visitStatus,
        PatientId:patientId
      }
      this.dataservice.post(this.config.Emar_Orders_GetOrdersGridDataByPatientId, this.filterConfigs)
        .subscribe((res: any) => {
          
          this.ng4LoadingSpinnerService.hide();
          this.q=1;

          this.residentOrdersList = res;
          //this.getOrdersEndingSoon();
          localStorage.removeItem('ordersBackClick');
          this.modalResidentAllOrderIsOpen=true;

        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  // GetOrdersTypeCounts(userId:number)
  // {
  //   this.dataservice.get(this.config.Emar_Orders_GetOrdersTypeCounts+userId)
  //   .subscribe(res=>{
  //     this.gridCount=res;
  //   },error=>{
  //     this.errorMessage = <any>error.message;
  //           //this.alertService.error(this.errorMessage);
  //   });
  // }

  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    //let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
        if (res.length == 1) {
          debugger
          this.selectednItems = [];
          console.log(this.nurseStations)
          this.selectednItems.push(this.nurseStations[0]);
          console.log(this.selectednItems)
          this.myform.patchValue({
            ddlnursestations: this.selectednItems,
          });
          this.getCompanyToBedByNurseStation();
          //this.getFiltersDataBySelection(this.userId);
          this.getOrderEndingSoonStatus();
          this.getOrdersEndingSoon();
          this.getPRNCheckDetails();
          this.getSeventyTwoHourCheckDetails();
          this.getFiltersDataBySelection(this.userId);
        }
        else{
          if (this.loginUserReceNurseStation != undefined) {
            let userReceNSList = this.loginUserReceNurseStation.split(',');
            if (userReceNSList.length > 0) {
              this.selectednItems = [];
              for (let i = 0; i < userReceNSList.length; i++) {
                if(this.nurseStations!=null && this.nurseStations!=undefined)
                {
                let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
                if (checkNsExist != undefined) {
                  this.selectednItems.push(checkNsExist);
                }
            //     else if(this.nurseStations!=undefined && this.nurseStations.length>0)
            // {
            //   this.selectednItems.push(this.nurseStations[0]);
            //   this.myform.patchValue({
            //     ddlnursestations: this.selectednItems,
            //   });
            // }
                if(this.selectednItems.length==1)
                break;
                }
              }
              this.myform.patchValue({
                ddlnursestations: this.selectednItems,
              });
              this.getCompanyToBedByNurseStation();
              //this.getFiltersDataBySelection(this.userId);
              this.getOrderEndingSoonStatus();
              this.getOrdersEndingSoon();
              this.getPRNCheckDetails();
              this.getSeventyTwoHourCheckDetails();
              this.getFiltersDataBySelection(this.userId);
            }
          }
        }

        // if(this.facilities.length==1 && backClick!=true)
        // {
        //   this.myform.patchValue({
        //     ddlnursestations:this.nurseStations,
        //   });
        //   this.getFiltersDataBySelection(this.userId);
        // }
        // if (this.ResNsfilterData != null && this.ResNsfilterData.length != 0 && backClick == true) {
        //   this.fetchData();
        // }
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
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
  getCompanyToBedByNurseStation() {
    this.nstations = "";
    let facilityId = this.myform.value.ddlfacilities[0].Facility_Id;
    let arNurseStations = this.myform.value.ddlnursestations;
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
          this.getNurseStationHierarchyDetailsbyNsId(this.myform.value.ddlnursestations[0].NurseStation_Id);
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
    else if (arNurseStations.length == 0) {
      //this.getCompanyToBedData(facilityId);
    }
  }

  // getCompanyToBedDataForBackClickFetch(facilityId)
  // {
  //   this.dataservice.get<any>(this.config.Emar_Facility_GetCompanyToBedFlagByFacId + this.userId + "/" +facilityId)
  //   .subscribe(res => {
  //     if (res.companyBedFlag == 1) {
  //       this.companyToBed = res.companyBedFlag;
  //       this.floors = res.Floors;
  //       this.wings = res.Wings;
  //       this.rooms = res.Rooms;
  //       this.beds = res.Beds;
  //     }
  //     else if (res.companyBedFlag == 0) {
  //       this.companyToBed = 0;
  //       this.floors =[];
  //       this.wings = [];
  //       this.rooms = [];
  //       this.beds = [];
  //     }
  //     this.getNurseStationByFacilityID(facilityId);
  //   }, error => {
  //     this.alertService.error(error.message);
  //     this.ng4LoadingSpinnerService.hide();
  //   });

  // }
  // getBackClickFilterData() {
  //   this.ng4LoadingSpinnerService.show();
  //   let backClick = JSON.parse(localStorage.getItem("ordersBackClick"));
  //   if (backClick == true) {
  //     this.ResFacfilterData = JSON.parse(localStorage.getItem("OrdersGridFacility"));
  //     this.ResNsfilterData = JSON.parse(localStorage.getItem("OrdersGridNurseStations"));
  //     this.ResFloorfilterData = JSON.parse(localStorage.getItem("OrdersGridFloors"));
  //     this.ResWingfilterData = JSON.parse(localStorage.getItem("OrdersGridWings"));
  //     this.ResRoomfilterData = JSON.parse(localStorage.getItem("OrdersGridRooms"));
  //     this.ResBedfilterData = JSON.parse(localStorage.getItem("OrdersGridBeds"));
  //     if (this.ResFacfilterData != null && this.ResNsfilterData != null) {
  //       //this.getNurseStationByFacilityID(this.ResFacfilterData);
  //       this.getCompanyToBedDataForBackClickFetch(this.ResFacfilterData);
  //       this.selectedfaItems=[];
  //       this.selectedfaItems.push(this.facilities.filter(f => f.Facility_Id == this.ResFacfilterData)[0]);
  //     }
  //   }
  //   this.ng4LoadingSpinnerService.hide();
  // }
  // fetchData() {
  //   this.selectedbItems=[];
  //   this.selectedflItems=[];
  //   this.selectednItems=[];
  //   this.selectedrItems=[];
  //   this.selectedwItems=[];
  //   this.ng4LoadingSpinnerService.show();
  //   if (this.ResNsfilterData.length > 0) {
  //     for (let i = 0; i < this.ResNsfilterData.length; i++) {
  //       this.selectednItems.push(this.nurseStations.filter(r => r.NurseStation_Id == parseInt(this.ResNsfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResFloorfilterData.length > 0) {
  //     for (let i = 0; i < this.ResFloorfilterData.length; i++) {
  //       this.selectedflItems.push(this.floors.filter(f => f.Floor_Id == parseInt(this.ResFloorfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResWingfilterData.length > 0) {
  //     for (let i = 0; i < this.ResWingfilterData.length; i++) {
  //       this.selectedwItems.push(this.wings.filter(w => w.Wing_Id == parseInt(this.ResWingfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResRoomfilterData.length > 0) {
  //     for (let i = 0; i < this.ResRoomfilterData.length; i++) {
  //       this.selectedrItems.push(this.rooms.filter(r => r.Room_Id == parseInt(this.ResRoomfilterData[i]))[0]);
  //     }
  //   }
  //   if (this.ResBedfilterData.length > 0) {
  //     for (let i = 0; i < this.ResBedfilterData.length; i++) {
  //       this.selectedbItems.push(this.beds.filter(b => b.Bed_Id == parseInt(this.ResBedfilterData[i]))[0]);
  //     }
  //   }
  //   this.myform.patchValue({
  //     ddlfacilities: this.selectedfaItems,
  //     ddlnursestations: this.selectednItems,
  //     ddlfloors: this.selectedflItems,
  //     ddlwings: this.selectedwItems,
  //     ddlrooms: this.selectedrItems,
  //     ddlbeds: this.selectedbItems
  //   });
  //   this.getFiltersDataBySelection(this.userId);
  //   this.ng4LoadingSpinnerService.hide();
  // }
  getSeventyTwoHourCheckDetails(event?:any) {
    
    if (this.myform.value.ddlfacilities.length != 0 && this.myform.value.ddlnursestations.length != 0) {
    this.seventyTwoHoursFilterConfigs = {
      User_Id: this.userId,
      Company_Id: 0,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ?this.form.value.ddlfloors.map(item => item.Floor_Id):[],
      Facilities: this.myform.value.ddlfacilities.length != 0? this.myform.value.ddlfacilities.map(item => item.Facility_Id): [],
      NurseStations: this.myform.value.ddlnursestations.length != 0? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id): [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0)? this.form.value.ddlwings.map(item => item.Wing_Id): [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0)? this.form.value.ddlrooms.map(item => item.Room_Id): [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0)? this.form.value.ddlbeds.map(item => item.Bed_Id): [],
    }
    this.dataservice.post(this.config.Emar_GetSeventyTwoHourDetailsByfilter ,this.seventyTwoHoursFilterConfigs)
      .subscribe(res => {
        //this.ng4LoadingSpinnerService.hide();
        if( res !=null && res.length>0)
        {
          this.seventyTwoHoursDiplayFlag=true;
        }
        else
        {
          this.seventyTwoHoursDiplayFlag=false;
        }
        if(event!=undefined && res !=null && res.length>0)
        {
          this.modalOption.size = 'lg';
          const modalRef = this.modalService.open(Ordersgrid72hourscheckComponent, this.modalOption);
          modalRef.componentInstance.orderGridFilter =this.seventyTwoHoursFilterConfigs;
          modalRef.componentInstance.leaveRequestResult.subscribe((receivedResult) => {
           if (receivedResult.responce == 1) {
              this.alertService.success("Save successful");
              this.seventyTwoHoursDiplayFlag=receivedResult.count<=0?false:true;
            }
          else if (receivedResult.responce == 2) {
              this.alertService.error("Something went wrong");
              this.seventyTwoHoursDiplayFlag=receivedResult.count<=0?false:true;
            }
        modalRef.close();
         });
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else
      {
        this.seventyTwoHoursDiplayFlag=false;
      }
  }
  getPRNCheckDetails(event?:any): any{
    if (this.myform.value.ddlfacilities.length != 0 && this.myform.value.ddlnursestations.length != 0) {
    this.ng4LoadingSpinnerService.show();
    this.prnFilterConfigs = {
      Company_Id: 0,
      Floors: (this.form.value.ddlfloors !=null && this.form.value.ddlfloors.length != 0) ?this.form.value.ddlfloors.map(item => item.Floor_Id):[],
      Facilities: this.myform.value.ddlfacilities.length != 0? this.myform.value.ddlfacilities.map(item => item.Facility_Id): [],
      NurseStations: this.myform.value.ddlnursestations.length != 0? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id): [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id): [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id): [],
      Beds: (this.form.value.ddlbeds !=null && this.form.value.ddlbeds.length != 0)? this.form.value.ddlbeds.map(item => item.Bed_Id): [],
      User_Id: this.userId,

    }
    this.dataservice.post(this.config.Emar_GetPRNDetailsByfilter, this.prnFilterConfigs)
      .subscribe((res: any[]) => {
        //this.ng4LoadingSpinnerService.hide();
        if( res !=null && res.length>0)
        {
          this.prnDiplayFlag=true;
        }
        else
        {
          this.prnDiplayFlag=false;
        }
        if(event!=undefined &&  res !=null && res.length>0)
        {
          this.modalOption.size = 'lg';
          const modalRef = this.modalService.open(OrdersgridprndocumentationComponent, this.modalOption);
          modalRef.componentInstance.orderGridFilter =this.prnFilterConfigs;
          modalRef.componentInstance.leaveRequestResult.subscribe((receivedResult) => {
           if (receivedResult.responce == 1) {
              this.alertService.success("Save successful");
              this.prnDiplayFlag=receivedResult.count<=0?false:true;
            }
          else if (receivedResult.responce == 2) {
              this.alertService.error("Something went wrong");
              this.prnDiplayFlag=receivedResult.count<=0?false:true;
            }
        modalRef.close();
         });
        }
      
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else
    {
      this.prnDiplayFlag=false;
    }
  }
  closeModel()
  {
  this.modalResidentAllOrderIsOpen=false;
  this.modalPendingOrderIsOpen=false;
  }
  openPendingOrdersModal()
  {
    this.r=1;
    this.modalPendingOrderIsOpen=true;
    setTimeout(() => {
      this.orderfocus.nativeElement.focus()
    }, 300);
  }
  getPendingOrders()
  {
    this.filterConfigs = {
      CompanyID: 0,
      Floors: (this.form.value.ddlfloors !=null &&this.form.value.ddlfloors.length != 0) ? this.form.value.ddlfloors.map(item => item.Floor_Id) : [],
      Facilities: this.myform.value.ddlfacilities.length != 0 ? this.myform.value.ddlfacilities.map(item => item.Facility_Id) : [],
      NurseStations: this.myform.value.ddlnursestations.length != 0 ? this.myform.value.ddlnursestations.map(item => item.NurseStation_Id) : [],
      Wings: (this.form.value.ddlwings !=null && this.form.value.ddlwings.length != 0) ? this.form.value.ddlwings.map(item => item.Wing_Id) : [],
      Rooms: (this.form.value.ddlrooms !=null && this.form.value.ddlrooms.length != 0) ? this.form.value.ddlrooms.map(item => item.Room_Id) : [],
      Beds: (this.form.value.ddlbeds !=null &&this.form.value.ddlbeds.length != 0) ? this.form.value.ddlbeds.map(item => item.Bed_Id) : [],
      ControlType: this.controlType,
      Type: this.type,
      User_Id: this.userId,
      VisitStatus: this.visitStatusForm.value.visitStatus,
    }
    this.dataservice.post(this.config.Emar_Orders_GetPendingOrdersGridData, this.filterConfigs)
      .subscribe((res: any) => {
        this.ng4LoadingSpinnerService.hide();
        this.pendingOrdersList = res;

      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl("data:image/jpeg;base64," + url);
  }
  NewOrder(orderId: number, patientId: number, quantityId: number) {
    localStorage.setItem("OrdersGridResType", JSON.stringify(this.visitStatusForm.value.visitStatus));
    localStorage.setItem("FromScreen", JSON.stringify("Orders"));
    this.sharedService.changePatientId(patientId);
    this.sharedService.changeOrderId(orderId);
    this.sharedService.changeQuantityId(quantityId);
    this.route.navigate(['/home/orderinfocpoe']);
  }
}
