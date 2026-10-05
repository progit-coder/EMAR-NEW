import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import {OrderFavConfig,OrderDetailsBYId } from '../../../models/common.model'
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-measurementsuserinputmaster',
  templateUrl: './measurementsuserinputmaster.component.html',
  styleUrls: ['./measurementsuserinputmaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class MeasurementsuserinputmasterComponent implements OnInit {
  myform: FormGroup;
  public events:number =1;
  public orderFavDetails: any[] = [];
  private OrderFavConfig = new OrderFavConfig();
  private OrderDetailsBYId = new OrderDetailsBYId();
  public selectedfaItems = [];
  private OfConfig_Id: number = 0;
  public selectedOrderItems =[];
  public template;
  errorMessage: string;
  searchText: string = "";
  dropdownSettings_Facilities: any = {};
  dropdownSettings_OrderFav:any ={};
  ShowFilter = true;
  p: number = 1;
  public facilities: any[];
  public OrderFav:any[];
  orderFavIDs:string ="";
  orderFavIdsData: any[];
  gridPagination = this.config.gridPagination;
  auditTable: any;
  pageConfig: {};
  public userId:number;
  constructor(private dataservice: DataService,private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }


  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("MeasurementandOtherChecksMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.myform = new FormGroup({
      ddlfacilities: new FormControl('', [Validators.required]),
      ddlorderfav: new FormControl('', [Validators.required]),
      code: new FormControl('',[Validators.required,Validators.maxLength(2),Validators.minLength(2), Validators.pattern(this.config.alphabets)])

    });
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.getFacilityData(this.userId);
    this.getOrderFavData();
    this.getOrderFavInfo(0);
    this.dropdownSettings_Facilities = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
      text: "Facilities",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.dropdownSettings_OrderFav = {
      singleSelection: false,
      idField: "OrderFavMaster_ID",
      textField: "OrderFavDesc",
      text: "Measurements",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.MeasurementandOtherChecksMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  insertOrderFavInfo() {
    this.orderFavIDs = '';
    this.orderFavIdsData = this.myform.value.ddlorderfav;
    this.orderFavIdsData.forEach(element => { this.orderFavIDs += element.OrderFavMaster_ID + ',' });
    this.orderFavIDs = this.orderFavIDs.substring(0, this.orderFavIDs.length - 1);
    this.ng4LoadingSpinnerService.show();
    this.OrderFavConfig = {
      facility_Id: this.myform.value.ddlfacilities[0].Facility_Id,
      orderFavMaster: this.orderFavIDs,
     // Room_Status: (this.myform.value.status == true ? 1 : 0),
     orderFavCode:this.myform.value.code,
     oFConfig_Status:1,
     oFConfig_CreatedBy:this.persistanceService.get(this.config.loggedInUserKey),
     oFConfig_CreatedDate: new Date().toISOString(),
     events:this.events
    };
    this.dataservice.post(this.config.Emar_Common_InsertUpdateMeasurementsandUserInputs, this.OrderFavConfig)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if(res ==1)
        {
        this.alertService.success("Save successful"); 
        this.resetScreen();
       }
       else if(res == 2)
       {
        this.alertService.warn("Code Already Exist.Please try with another Code");  
        this.myform.patchValue({
          code:""
        });
       }
        this.events=1;
        this.getOrderFavInfo(0);
        //this.getRooms();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    
  }
  onFacilitySelect(item: any)
  {
 this.getOrderFavInfo(item.Facility_Id);
  }
  onFacilityDeSelect(item: any)
  {
    this.getOrderFavInfo(0);
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();

    this.ng4LoadingSpinnerService.hide();
    this.OfConfig_Id = 0;
    this.events =1;
  }
  getFacilityData(userId: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        debugger;
        this.facilities = res.Facilities;
       this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getOrderFavData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Common_GetOderFavInfo)
      .subscribe((res: any) => {
        debugger;
        this.OrderFav = res;
       this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getOrderFavInfo(FacilityId:number) {
    debugger;
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Common_GetOrderFavDetails + FacilityId)
      .subscribe(res => {
        this.orderFavDetails = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getOrderFavDetailsByID(FacilityId:number,orderFavCode:string,orderFavDate:any)
{
  debugger;
  this.ng4LoadingSpinnerService.show();
  this.OrderDetailsBYId = {
    FacilityId: FacilityId,
    Code: orderFavCode,
   // Room_Status: (this.myform.value.status == true ? 1 : 0),
   CreatedDate:orderFavDate,
  };
  this.dataservice.post(this.config.Emar_Common_GetOrderFavConfigDetailsById ,this.OrderDetailsBYId)
    .subscribe(res => {
      debugger;
      this.fetchData(res);
      this.events =2;
      this.ng4LoadingSpinnerService.hide();
    }, error => {
      this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)
      this.ng4LoadingSpinnerService.hide();
    });
}
fetchData(res: any) {
  if (res.OrderFavDesc != undefined) {
    let orderFavList = res.OrderFavDesc.split(',');
    if (orderFavList.length > 0) {
      this.selectedOrderItems = [];
      for (let i = 0; i < orderFavList.length; i++) {
        let checkOrderFavExist = this.OrderFav.find(r => r.OrderFavMaster_ID === parseInt(orderFavList[i]));
        if (checkOrderFavExist != undefined) {
          this.selectedOrderItems.push(checkOrderFavExist);
        }
      }
  }
}
this.selectedfaItems=[];
if(res.Facility_ID !=undefined && res.Facility_ID!=null)
{
 let checkrecord= this.facilities.find(r=>r.Facility_Id==res.Facility_ID);
 if(checkrecord !=undefined)
 {
   this.selectedfaItems.push(checkrecord);
 }
}
  this.myform.patchValue({
    code: res.OrderFavCode,
    ddlfacilities:this.selectedfaItems,
    ddlorderfav:this.selectedOrderItems
  });
}
}
