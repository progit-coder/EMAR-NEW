import { Component, HostListener, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/shared/dataservice.service';
import { APIConfiguration } from 'src/app/models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from 'src/app/services/shared/persistance.service';
import { AlertService } from 'src/app/_services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharedService } from 'src/app/services/shared/shared.service';
import { NurseStation } from 'src/app/models/facility.model';
import { CheckInMeds } from 'src/app/models/checkinmeds.model';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
import { parse } from 'querystring';

@Component({
  selector: 'app-checkinmeds',
  templateUrl: './checkinmeds.component.html',
  styleUrls: ['./checkinmeds.component.css'],
})
export class CheckinmedsComponent implements OnInit {

  pageConfig = {};
  template;
  myform: FormGroup;
  userId: number;
  loginUserReceFacility: any;
  loginUserReceNurseStation: any;
  facilities: any[];
  nurseStations: NurseStation[];
  selectedfaItems = [];
  selectednItems = [];
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  //dropdownSettings_Residents: any = {};
  ShowFilter = true;
  residents: any[];
  //selectedResItem: any[];
  ordersList: any[] = [];
  currentPageNo: number = 1;
  gridPagination = this.config.gridPagination;
  //totalRecords: number = 0;
  checkInMedsObj: any[] = [];
  textform: FormGroup;
  textform1: FormGroup;
  residentId: number = 0;
  statusFlag: boolean=false;
  BarcodeValue: string = "";
  public p:number=1;
  public modalCheckinHistoryIsOpen:boolean=false;
  public CheckinMedsHistory:any[]=[];
  public historyPagination=10;
  public checkAllOrders:any[]=[];
  textform2:FormGroup;
  public minDate:string=this.dateFormatPipe.dateFormat(new Date());
  public yearDate= new Date(new Date().setMonth(new Date().getMonth() + 11)).toISOString().substring(0, 10);

  constructor(private dataservice: DataService, private sharedService: SharedService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private alertService: AlertService, private dateFormatPipe: CustomdatePipe) { }
  // @HostListener('paste', ['$event']) blockPaste(e: KeyboardEvent) {
  //    
  // var b = e.target.attributes.formcontrolname.value
  //   if(b=="myNumber")
  // {

  
  //   e.preventDefault();
  // }
  // }
  
  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("PharmacyCheck-inMedication");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
          //ddlfloors: new FormControl(''),
          ddlnursestations: new FormControl(''),
          // ddlwings: new FormControl(''),
          // ddlrooms: new FormControl(''),
          // ddlbeds: new FormControl(''),
          //ddlresidents: new FormControl(''),
          barcode: new FormControl(''),
          showOrders:new FormControl(false),
        });
        this.textform = new FormGroup({
          quantity: new FormControl('0', Validators.required),
        });
        this.textform1 = new FormGroup({
          barcode: new FormControl('')
        });
        let date=new Date();
        this.textform2=new FormGroup({
          lotnumber:new FormControl(''),
          expdate:new FormControl(),
        });
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        //this.getCompanyToBedData();
        //this.getFiltersData(this.userId);
        this.getUserRecentFacilityNurseStations();
        this.userActivity();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.CheckInMeds,Activity.View,'')
    .subscribe(res=>{},error=>{
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
        this.ng4LoadingSpinnerService.hide();
        this.facilities = res.Facilities;

        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacilities: this.selectedfaItems,
            });
          }
        }

        if (res.Facilities.length == 1) {
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.myform.patchValue({
            ddlfacilities: this.facilities,
          });
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
      allowSearchFilter: this.ShowFilter
    };
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
    // this.dropdownSettings_Residents = {
    //   singleSelection: true,
    //   idField: "Patient_Id",
    //   textField: "PatientName",
    //   text: "Residents",
    //   selectAllText: "Select All",
    //   unSelectAllText: "UnSelect All",
    //   itemsShowLimit: 1,
    //   noDataAvailablePlaceholderText: 'Please Select Nursing Station',
    //   allowSearchFilter: this.ShowFilter
    // };
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
      // if( this.nurseStations.length==1){
      //   debugger
      //   this.myform.patchValue({
      //     ddlnursestations: this.nurseStations,
      //   });
      //   this.selectednItems.push(this.nurseStations);
      //   if (this.selectednItems.length > 0 && this.myform.value.barcode != '')
      //   this.getOrdersList(this.selectednItems[0].NurseStation_Id, this.myform.value.barcode);
      // }
      // else{
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
            }
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            if (this.selectednItems.length > 0 && this.myform.value.barcode != '')
              this.getOrdersList(this.selectednItems[0].NurseStation_Id, this.myform.value.barcode);
          }
        // }
      }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  // getResidentDropData(stationIds: string) {
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<any>(this.config.Emar_ResidentDemographic_GetResidentsByNurseStationIds + stationIds)
  //     .subscribe(res => {
  //       this.residents = res;
  //       if (res != null) {
  //         //if (this.residentId == 0) {
  //           this.residentId = this.residents[0].Patient_Id;
  //           //this.sharedService.changePatientId(this.residentId);
  //         //}
  //        let selectedResItem = this.residents.filter(item => item.Patient_Id == this.residents[0].Patient_Id);
  //         this.myform.patchValue({
  //           ddlresidents: selectedResItem
  //         });
  //         this.getOrdersList(1, this.residentId);
  //       }
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  onResidentSelect(item: any) {
    // this.selectedResItem = this.residents.filter(item => item.Patient_Id == item.Patient_Id);
    // this.myform.patchValue({
    //   ddlresidents: this.selectedResItem
    // });
    this.residentId = item.Patient_Id;
    this.getOrdersList(1, item.Patient_Id);
  }
  onResidentDeSelect(item: any) {
    //this.selectedResItem = [];
    this.residentId = 0;
    // this.myform.patchValue({
    //   ddlresidents: ''
    // });
    this.alertService.warn('Select at least one resident');
    this.ordersList = [];
  }
  getOrdersList(nsId: number, barcode: string) {
    //barcode = barcode == undefined ? '' : barcode;
    if (barcode != "") {
      let barcodevalue = barcode.split('/');
      this.BarcodeValue = barcodevalue[0];
    }
    if(this.BarcodeValue!="")
    {
      this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_CheckInPharmacyMeds_GetCheckInMedsList + nsId + '/' + this.BarcodeValue)
      .subscribe(res => {
        debugger;
        //this.ordersList = res.Data;
        let record = res.Data;
         console.log(record);
        let checkBarcodeOrders=record.filter(r=>r.barcodeCheck==1);
        if(res.Data.length==0 || checkBarcodeOrders.length==0)
        {
          this.alertService.warn("Barcode not found");
          this.ng4LoadingSpinnerService.hide();
        }
        if (this.ordersList.length > 0 || record.length > 0 && checkBarcodeOrders.length>0) 
        {
          record.forEach(element => {
            if (this.ordersList.find(or => or.PQuantity_Id === element.PQuantity_Id) == undefined) {
              this.ordersList.push(element);
            }
          });
          checkBarcodeOrders.forEach(element => {
            if (this.ordersList.find(or => or.PQuantity_Id === element.PQuantity_Id) == undefined) {
              this.ordersList.push(element);
            }
            else{
             let index=this.ordersList.findIndex(or => or.PQuantity_Id === element.PQuantity_Id);
             this.ordersList.splice(index,1);
             this.ordersList.push(element);
            }
          });
        }
        else {
          this.ordersList.push(record);
        }
        this.myform.patchValue({
          barcode: ''
        });
        //this.statusFlag=this.ordersList.length>0 && this.ordersList.find(ol=>ol.ResidentNSFlag==0 ||ol.OrderStatus==2)?true:false;
        this.ng4LoadingSpinnerService.hide();
        // this.errorMessage = res.residentNSFlag == 0 ? 'Resident is No Longer present here.' : '';
        // this.errorMessage = this.ordersList.length > 1 ? 'More than one Order has Same barcode which is wrong' : '';
        // this.errorMessage = this.ordersList.filter(p => p.OrderStatus == 2).length > 0 ? 'Order is Inactive' : '';

      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  loadgrid() {

  }
  scanBarcode(value: any) {
    if(value != null)
    {

    
    value = value.trim();
    }
    if (value != '') {
    
      if (this.selectednItems.length > 0)
        this.getOrdersList(this.selectednItems[0].NurseStation_Id, value);
      else
        this.alertService.warn('Select nursing station to display data');
    }
    else {
      this.alertService.warn('Scan barcode to proceed');
    }
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.ordersList = [];
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
      barcode: ''
    });
    this.statusFlag=false;
    this.getNurseStationByFacilityID(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.ordersList = [];
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
      barcode: ''
    });
    this.statusFlag=false;
  }
  onNurseStationSelect(item: any) {
    this.ordersList = [];
    this.statusFlag=false;
    this.myform.patchValue({
      barcode: ''
    });
    //this.getResidentDropData(this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(','));
  }
  onNurseStationSelectAll(item: any) {
    this.ordersList = [];
    this.myform.value.ddlnursestations = item;
    this.statusFlag=false;
    this.myform.patchValue({
      barcode: ''
    });
    //this.getResidentDropData(this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(','));
  }
  onNurseStationDeSelect(item: any) {
    this.ordersList = [];
    this.alertService.error("Please select nursing station")
    //this.getResidentDropData(this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(','));
  }
  onNurseStationDeSelectAll(item: any) {
    this.ordersList = [];
    this.myform.patchValue({
      ddlresidents: ''
    })
    this.alertService.error("Select at least one nursing station to display data")
  }

  // QuntitiyCHeckOnLeve(qty:any)
  // {
  //  ;
  // let qqunt = new String( qty);
  //   if(qqunt.qq>10)
  //   {
  //     return alert('Hello');
  //   }
  // }

  readUserInputData(orderId: number,quantityId:number, qty: number, barcode: string,type:string,lot:string,expdate:string) {
     if(barcode != null)
     {

     
    barcode = barcode.trim();
     }
     let qqunt = new String(qty);
    if( qqunt.length > 10)
    {
      this.alertService.warn("Quantity Received must not exceed 10 characters. ");
      const inputElement = document.getElementById(quantityId.toString()) as HTMLInputElement;
      inputElement.value = '';
      inputElement.focus();
      // this.textform.patchValue({
      //   quantity:"",
      // });
      
    
    }
    if (barcode != null && barcode != '' && barcode != undefined) {
        this.ng4LoadingSpinnerService.show();
        let barcodeValue=(barcode.split("/"))[0];
        if(barcodeValue!="")
        {
      this.dataservice.get<any>(this.config.Emar_CheckInPharmacyMeds_CheckBarcode + barcodeValue +"/"+orderId)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          if (res != 0) {
             
            if(res==orderId)
            this.alertService.warn('Barcode already exists for this resident');
            else
            this.alertService.warn('This barcode is assigned to an item/drug with a different GPI');
            let exist=this.checkInMedsObj.find(e=>e.PQuantity_Id==quantityId);
            if(exist!=undefined)
            {
              let index=this.checkInMedsObj.findIndex(e=>e.PQuantity_Id==quantityId);
              if(this.checkInMedsObj[index].OnHand==null)
              {
                this.checkInMedsObj.splice(index,1);
              }
              else
              {
              this.checkInMedsObj[index].Barcode="";
              }
            }
            this.clearData(quantityId);
          }
          else {
            this.addDataToArray(orderId,quantityId, qty, barcodeValue,lot,expdate);
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
    }
      else if(barcode=="" && type=="barcode")
      {
        let exist=this.checkInMedsObj.find(e=>e.PQuantity_Id==quantityId);
            if(exist!=undefined)
            {
              let index=this.checkInMedsObj.findIndex(e=>e.PQuantity_Id==quantityId);
              if(this.checkInMedsObj[index].OnHand==null)
              {
                this.checkInMedsObj.splice(index,1);
              }
              else
              {
              this.checkInMedsObj[index].Barcode="";
              }
            }
      }
    else {
      this.addDataToArray(orderId,quantityId, qty, barcode,lot,expdate);
    }
  }
  addDataToArray(orderId: number,quantityId:number, qty: number, barcode: string,lot:string,expdate:string) {
    if(expdate!=null && expdate!="" && ((new Date(this.dateFormatPipe.transform(expdate)).setHours(0,0,0,0)-new Date().setHours(0,0,0,0))/ (1000 * 60 * 60 * 24))<0)
    {
      this.alertService.warn("Expiration date cannot be earlier than today’s date");
      this.setExpDateByOrderId(orderId);
    }
    else
    {
    if (this.checkInMedsObj.length > 0) {
      let record = this.checkInMedsObj.find(o => o.PQuantity_Id == quantityId);
      if (record != null) {
        if (barcode != null)
          record.Barcode = barcode;
        if (qty != null)
          record.OnHand = qty;
        if(lot!=null)
          record.LotNumber=this.getLotNumberByOrderId(quantityId);
        if(expdate!=null)
          record.ExpirationDate=this.dateFormatPipe.transform(this.getExpDateByOrderId(quantityId)); 
      }
      else {
        let record = new CheckInMeds();
        record.POrder_Id = orderId;
        record.PQuantity_Id=quantityId;
        record.OnHand = qty;
        record.Barcode = barcode;
        record.CheckInDate = this.dateFormatPipe.dateWithTime(new Date());
        record.LotNumber=lot;
        record.ExpirationDate=this.dateFormatPipe.transform(this.getExpDateByOrderId(quantityId));
        this.checkInMedsObj.push(record);
      };
    }
    else {
      let record = new CheckInMeds();
      record.POrder_Id = orderId;
      record.PQuantity_Id=quantityId;
      record.OnHand = qty;
      record.Barcode = barcode;
      record.CheckInDate = this.dateFormatPipe.dateWithTime(new Date());
      record.LotNumber=this.getLotNumberByOrderId(quantityId);
      record.ExpirationDate=this.dateFormatPipe.transform(this.getExpDateByOrderId(quantityId));
      this.checkInMedsObj.push(record);
    };
  }
  }
  checkInSelectedMeds() {
    debugger;
    this.ng4LoadingSpinnerService.show();
    if(this.ordersList.length==1 &&  this.textform1.value.barcode!="")
    {
      let barcodeValue=((this.textform1.value.barcode).split("/"))[0];
      if(barcodeValue!="")
      {
      this.dataservice.get<any>(this.config.Emar_CheckInPharmacyMeds_CheckBarcode + barcodeValue+"/"+this.ordersList[0].POrder_Id)
        .subscribe(res => {
           
          this.ng4LoadingSpinnerService.hide();
          if (res != 0) {
            if(res==this.ordersList[0].POrder_Id)
            this.alertService.warn('Barcode already exists for this resident');
            else
            this.alertService.warn('This barcode is assigned to an item/drug with a different GPI');
            let exist=this.checkInMedsObj.find(e=>e.PQuantity_Id==this.ordersList[0].PQuantity_Id);
            if(exist!=undefined)
            {
              let index=this.checkInMedsObj.findIndex(e=>e.PQuantity_Id==this.ordersList[0].PQuantity_Id);
              if(this.checkInMedsObj[index].OnHand==null)
              {
                this.checkInMedsObj.splice(index,1);
              }
              else
              {
              this.checkInMedsObj[index].Barcode="";
              }
            }
            this.clearData(this.ordersList[0].PQuantity_Id);
          }
          else {
            if (this.checkInMedsObj.length > 0) {
              let record = this.checkInMedsObj.find(o => o.PQuantity_Id == this.ordersList[0].PQuantity_Id);
              if (record != null) {
                record.Barcode = barcodeValue;
              }
            }
            else
            {
            let record = new CheckInMeds();
            record.POrder_Id = this.ordersList[0].POrder_Id;
            record.PQuantity_Id=this.ordersList[0].PQuantity_Id;
            record.OnHand = this.ordersList[0].OnHand;
            record.Barcode = this.textform1.value.barcode;
            record.CheckInDate = this.dateFormatPipe.dateWithTime(new Date());
            record.LotNumber=this.getLotNumberByOrderId(this.ordersList[0].PQuantity_Id);
            record.ExpirationDate=this.dateFormatPipe.transform(this.getExpDateByOrderId(this.ordersList[0].PQuantity_Id));
            this.checkInMedsObj.push(record);
            }
            this.dataservice.post(this.config.Emar_CheckInPharmacyMeds_CheckInSelectedMeds, this.checkInMedsObj)
            .subscribe(res => {
              this.ng4LoadingSpinnerService.hide();
              if (res == 1) {
                this.alertService.success('Check-in successful');
                //this.textform.reset();
                this.textform.reset({
                  quantity: '0',
                });
                this.textform1.reset({
                  barcode:'',
                });
                let date = new Date();
                this.textform2.reset({
                  lotnumber:'',
                  expdate:new Date(date.setMonth(date.getMonth() + 11)).toISOString().substring(0, 10)
                });
                this.ordersList = [];
                this.checkInMedsObj = [];
                this.statusFlag=false;
                this.myform.patchValue({
                  barcode: '',
                  showOrders:false
                });
              }
              else {
                this.alertService.warn('Something went wrong');
              }
            }, error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
          }
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
    }
   else if(this.checkInMedsObj.length>0)
    {
      // this.ordersList.forEach((element) => {
      //   let checkOrder = this.checkInMedsObj.length > 0 ? this.checkInMedsObj.find(o => o.POrder_Id == element.POrder_Id) : undefined;
      //   if (checkOrder == undefined) {
      //     let record = new CheckInMeds();
      //     record.POrder_Id = element.POrder_Id;
      //     record.OnHand = element.OnHand == undefined ? null : element.OnHand;
      //     record.Barcode = null;
      //     this.checkInMedsObj.push(element);
      //   }
      // });
    this.dataservice.post(this.config.Emar_CheckInPharmacyMeds_CheckInSelectedMeds, this.checkInMedsObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success('Check-in successful');
          //this.textform.reset();
          this.textform.reset({
            quantity: '0'
          });
          this.ordersList = [];
          this.checkInMedsObj = [];
          this.statusFlag=false;
          this.myform.patchValue({
            barcode: '',
            showOrders:false
          });
          let date = new Date();
          this.textform2.reset({
            lotnumber: '',
            expdate:''
          });
          //this.getOrdersList(this.selectednItems[0].NurseStation_Id, this.myform.value.barcode);
        }
        else {
          this.alertService.warn('Something went wrong');
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    else
    {
      this.alertService.warn("At least one order detail must be entered to proceed");
      this.ng4LoadingSpinnerService.hide();
    }
  }
  removeCheckInMed(id: any) {
    const index = this.ordersList.findIndex(i => i.PQuantity_Id == id);
    this.ordersList.splice(index, 1);
    let chMedIndex=this.checkInMedsObj.findIndex(c=>c.PQuantity_Id==id);
    if(chMedIndex!=-1)
    {
      this.checkInMedsObj.splice(chMedIndex, 1);
    }
  }
  clearData(qtyId:any)
  {
      let control = "#br"+qtyId;
      $(control).val("")
  }
  clearLotData(qtyId:any)
  {
      let control = "#lot"+qtyId;
      $(control).val("")
  }
  getExpDateByOrderId(qtyId:any)
  {
     
    let control = "#exp"+qtyId;
    var date=$(control).val();
    return  date;
  }
  getLotNumberByOrderId(qtyId:any)
  {
     
    let control = "#lot"+qtyId;
    var lotnumber=$(control).val().toString();
    return  lotnumber;
  }
  setExpDateByOrderId(qtyId:any)
  {
     
    var item=this.ordersList.find(o=>o.PQuantity_Id == qtyId);
    if(item!=undefined && item.ExpirationDate!=null && item.ExpirationDate!="")
    {
      let control = "#exp"+qtyId;
      var date=$(control).val(this.dateFormatPipe.transformISODate(item.ExpirationDate));
      return  date;
    }
    else if(item!=undefined && (item.ExpirationDate==null || item.ExpirationDate==""))
    {
      let control = "#exp"+qtyId;
      var date=$(control).val(this.dateFormatPipe.transformISODate(this.yearDate));
      return  date;
    }
  }
  getCheckinHistoryById(quantityId:any)
  {
    this.dataservice.get<any[]>(this.config.Emar_CheckInPharmacyMeds_GetOrderStockTrans+ quantityId)
      .subscribe(res => {
        this.CheckinMedsHistory=res;
      this.modalCheckinHistoryIsOpen=true;
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  closeModel()
  {
    this.modalCheckinHistoryIsOpen=false;
  }
}
