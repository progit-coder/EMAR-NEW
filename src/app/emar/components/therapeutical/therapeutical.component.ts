import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { chart } from 'highcharts';
import * as Highcharts from 'highcharts';
import { FormGroup, FormControl } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NurseStation } from '../../../models/facility.model';
import { ResidentDemographic } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { MedRefdata } from '../../../models/assesments.model';


@Component({ 
  selector: 'app-therapeutical',
  templateUrl: './therapeutical.component.html',
  styleUrls: ['./therapeutical.component.css']
})
export class TherapeuticalComponent implements OnInit {
  toDate: string;
  fromDate: string;
  public comparedropdownList = [];
  public residents: ResidentDemographic[];
  gridData: any[] = [];
  gridColumns: any;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  dashboardForm: FormGroup;
  public module: string;
  userId: number;
  iscollapsed = true;
  showDates = false;
  month: number;
  year: number;
  public template;
  public nursestationid: string = "";
  public patientid: string = "";
  backButton = false;
  public nurseStations: NurseStation[];
  dropdownSettings_ID: any = {};
  public selectedItems = [];
  ShowFilter = true;
  modulenamereport: any;
  searchText: string = "";
  dropdownSettings_ResID: any = {};
  dropdownSettings_FacID: any = {};
  public selectedFacItems = [];
  public facilities: any[];

  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedfaItems = [];
  public selectednItems = [];
  public selectednItemsNew = [];
  public selectedResidents= [];
  pageConfig = {};
  public selectedResItem: any[];
  public totalRecords: any;
  public selecteGPIdItems = [];
  public gpitext : string = "";
  public gpilist: any =[];
  dropdownSettings_gpiID:any = {};
  public isPageLaod : number = 0;
  selectedLevel;
  public MedRefdata: MedRefdata[];
  MedRefList: MedRefdata;
  public legend: string;

  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private sharedService: SharedService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("MARReport");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();

    let date = new Date();
    this.fromDate = new Date(date.setMonth(date.getMonth() - 11)).toISOString().substring(0, 10);
    this.toDate = new Date().toISOString().substring(0, 10);
    // this.month = 2;
    // this.year = 2019;

    this.comparedropdownList = [];
    let today = new Date();
    let year = today.getFullYear();
    let i: number = 2010;
    while (year >= i) {
      this.comparedropdownList.push({ item_id: year, item_text: year });
      year--;
    }

    this.dashboardForm = new FormGroup({
      years: new FormControl(today.getFullYear()),
      months: new FormControl(today.getMonth() + 1),
      ddlresidents: new FormControl(''),
      residentstatus: new FormControl(1),
      martype: new FormControl('3'),
      RefType : new FormControl(''),
      gpidata : new FormControl(''),
      nursestationName: new FormControl(''),
      facilityName: new FormControl(''),
    });
    this.dropdownSettings_ID = {
      singleSelection: true,
      idField: "NurseStation_Id",
      textField: "NurseStation_Name",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      allowSearchFilter: true

    }; 
    this.dropdownSettings_FacID = {
      singleSelection: true,
      idField: "Facility_Id",
      textField: "Facility_Name",
     // text: "Facilities",
      selectAllText: "Select All",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.dropdownSettings_ResID =
      {
        singleSelection: false,
        idField: "patientId",
        textField: "patientname",
        selectAllText: "Select All",
        itemsShowLimit: 1,
        allowSearchFilter: true
      };
      this.dropdownSettings_gpiID = {
        singleSelection: false,
        idField: "ID",
        textField: "GPI",
        selectAllText: "Select All",
        noDataAvailablePlaceholderText: "Please Select",
        itemsShowLimit: 1,
        allowSearchFilter: true
  
      };
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.month = this.dashboardForm.value.months;
    this.year = this.dashboardForm.value.years;
    //this.getFacilities(); 
    this.getUserRecentFacilityNurseStations();  
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getUserRecentFacilityNurseStations() {
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
         
        if (res != undefined) {
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFacilities();
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getFacilities() {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res:any) => {
        this.facilities = res.Facilities;
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedFacItems = [];
            if (checkFacExist != undefined) {
              this.selectedFacItems.push(checkFacExist);
              this.getNurseStations(this.loginUserReceFacility);
            }
            this.dashboardForm.patchValue({
              facilityName: this.selectedFacItems,
            });
          }
        }
        // this.ng4LoadingSpinnerService.hide();
        // this.dashboardForm.patchValue({
        //   facilityName:this.facilities
        // });
        // this.getNurseStations(res.Facilities[0].Facility_Id);
        // res.forEach(item => this.selectedFacItems.push(item.Facilities[0].Facility_Id));
       
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
    });
    this.loginUserReceNurseStation = undefined;
    this.getNurseStations(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.dashboardForm.patchValue({
      nursestationName: '',
    });
    this.loginUserReceNurseStation = undefined;
    this.alertService.error("Please select at least one facility for nursing stations");
  //  this.getResidentDropData();
  this.getGpiData();
  }
  getGpiData() {
    debugger;
     this.dataservice.get<any>(this.config.Emar_Facility_GpiList+this.nursestationid+"/"+this.fromDate + "/" + this.toDate+"/"+this.selectedLevel)
       .subscribe(res => {
         this.gpilist = res;
         //this.resMreason.push('1,2,3');
 
         //this.gpitext = "";
         //this.selecteddItems=['1,2,3,4,5'];
         //this.selecteddItems = [res];
           
         this.dashboardForm.patchValue({
           gpidata : res,
         });
         this.getSelectedgpiLoad();
         //this.dashboardForm.value.ddlNotestype = this.selecteddItems;
         this.ng4LoadingSpinnerService.hide();
         
       },
         error => {
           this.alertService.error(error.message);
           this.ng4LoadingSpinnerService.hide();
         });
   }
   getSelectedgpiLoad()
   {
     
     if (this.dashboardForm.value.gpidata.length != 0) {
       this.selecteGPIdItems.length = 0;
       this.dashboardForm.value.gpidata.forEach(item => this.selecteGPIdItems.push(item.ID));
       this.gpitext = "";
       this.gpitext = this.selecteGPIdItems.join(',');
 
     this.loadChart();
     }
     else if (this.dashboardForm.value.gpidata.length == 0) {
       this.selecteGPIdItems.length = 0;
      // this.alertService.error("Please select gpi to display data");
     }
   }
   getSelectedgpi()
   {
     
     if (this.dashboardForm.value.gpidata.length != 0) {
       this.selecteGPIdItems.length = 0;
       this.dashboardForm.value.gpidata.forEach(item => this.selecteGPIdItems.push(item.ID));
       this.gpitext = "";
       this.gpitext = this.selecteGPIdItems.join(',');
 
      // this.getGridData(this.module, this.fromDate, this.toDate);
     }
     else if (this.dashboardForm.value.gpidata.length == 0) {
       this.selecteGPIdItems.length = 0;
       //this.alertService.error("Please select gpi to display data");
     }
   }
   onGpiSelect(item: any) {
    this.getSelectedgpi();
  }
  onGpiSelectAll(item: any) {
    this.dashboardForm.value.gpidata = item;
    this.getSelectedgpi();
  }
  onGpiDeSelect(item: any) {
    this.getSelectedgpi();
  }
  onGpiDeSelectAll(item: any) {
    this.dashboardForm.value.gpidata.length = 0;
    this.gpitext = "";
    this.selecteGPIdItems = [];
    this.alertService.error("Please select therapeutical classification to display data");
  }
  getResidentDropData() {
  
    this.dataservice.get<any[]>(this.config.Emar_Report_GetResidentnamesmarReport+this.nursestationid+"/"+this.dashboardForm.value.residentstatus)
      .subscribe(res => {
            
        this.residents = res;
        
        this.selectedResItem = [] 
        if(res.length != 0)
        {

          res.forEach(itemr => {
            this.selectedResItem.push(itemr);
       // this.selectedResItem.push(this.residents)
          });
        }
        this.dashboardForm.patchValue({
          ddlresidents: this.selectedResItem
        });
        this.selectedResidents=[];
        if(this.selectedResItem.length != 0)
        {

        
        this.dashboardForm.value.ddlresidents.forEach(item => {
        this.selectedResidents.push(item.patientId);
       });
      }
      //   this.dashboardForm.value.ddlresidents.forEach(item => {
      //   this.selectedResidents.push(item.patientId);
      //  });
      this.patientid = "";
      
      this.patientid = this.selectedResidents.join(',');
      // this.module = "emarhole";
    
      this.getGridData(this.module, this.month, this.year);
      }, error => this.alertService.error(error.message));
  }
  getNurseStations(facilityId: any) {
  debugger;
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId +"/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.selectedItems=[];
        this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            this.selectednItemsNew = [];
              
            // for (let i = 0; i < userReceNSList.length; i++) {
            //   let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
            //   if (checkNsExist != undefined) {
            //     this.selectednItems.push(checkNsExist);
            //   }
            // }
    let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[0]));
    this.selectednItems.push(checkNsExist);

            this.dashboardForm.patchValue({
              nursestationName: this.selectednItems,
            });
            this.nursestationid = "";
            this.selectednItems.forEach(item => this.selectednItemsNew.push(item.NurseStation_Id));
            this.nursestationid = this.selectednItemsNew.join(',');
            this.module = "emarresidents";
             this.getResidentDropData();
            this.getGpiData();
          }
        }
        else if (this.loginUserReceNurseStation == undefined) {

          let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === res[0].NurseStation_Id);
         var B = [];
          B.push(checkNsExist);


          this.dashboardForm.patchValue({
            nursestationName: B,
          });
          this.selectedItems = [];
         // res.forEach(item => this.selectedItems.push(item.NurseStation_Id));
         this.selectedItems.push(res[0].NurseStation_Id);
       // this.selectedItems.push(res[0]);
          this.nursestationid = "";
          this.nursestationid = this.selectedItems.join(',');
          this.module = "refill";
           this.getResidentDropData();
          this.getGpiData();
        
        }
      }, 
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  showHideToggle(cls) {
    if (this.iscollapsed) {
      $(cls).addClass('show');
      this.iscollapsed = false;
    }
    else {
      this.iscollapsed = true;
      $(cls).removeClass('show');
    }
  }
  close(cls) {
    this.iscollapsed = true;
    $(cls).removeClass('show');
  }
  getGridData(moduleName: string, month: number, year: number,currentPage?:any) {
    this.gridData = [];
    this.gridColumns = [];
    this.totalRecords=0;
      
    if ((this.dashboardForm.value.facilityName.length == 0 || this.dashboardForm.value.facilityName == undefined || this.dashboardForm.value.facilityName == null) || (this.dashboardForm.value.nursestationName.length == 0 || this.dashboardForm.value.nursestationName == undefined || this.dashboardForm.value.nursestationName == null)) {
    //  this.alertService.warn("Select facility and nursing station");
      this.ng4LoadingSpinnerService.hide();
      this.residents = [];
      this.dashboardForm.patchValue({
        ddlresidents: '',
        gpidata:''
      });
      // this.gridData = [];
      // this.gridColumns = [];
      // this.totalRecords=0;
    }
    else if ((this.dashboardForm.value.years == 0 || this.dashboardForm.value.years==null || this.dashboardForm.value.years==undefined) || (this.dashboardForm.value.months == 0 || this.dashboardForm.value.months==null || this.dashboardForm.value.months==undefined)) {
      this.alertService.warn("Enter valid date range");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dashboardForm.value.martype == 0 || this.dashboardForm.value.martype==null || this.dashboardForm.value.martype==undefined)
    {
      this.alertService.warn("Please select type");
      this.ng4LoadingSpinnerService.hide();
    }

    else if (this.dashboardForm.value.ddlresidents.length == 0 || this.dashboardForm.value.ddlresidents == undefined || this.dashboardForm.value.ddlresidents == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
    }

     else if (this.dashboardForm.value.gpidata.length == 0 || this.dashboardForm.value.gpidata == undefined || this.dashboardForm.value.gpidata == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
     }

    else
    {
         
       
      
      let emarObj=
      {
        DashboardName:this.module,
        FromDate:this.fromDate,
        ToDate:this.toDate,
        UserId:this.userId,
        NursingstationId:this.nursestationid,
        currentPage:currentPage==undefined?1:currentPage,
        pageSize:this.gridPagination,
        PassTime:null,
        OrderType:1,
        Month:this.month,
        Year:this.year,
        PatientId:0,
        patientName:this.patientid,
        commentType:this.gpitext,
        userIds:null,
        shiftTime:null,
        facilityid:0
      }
    if (this.module == 'emarresidents') {
        
      this.ng4LoadingSpinnerService.show();
      
      this.dataservice.post(this.config.Emar_Dashboard_Therepeutical ,emarObj)
        .subscribe(res => {
              
          if (res != null) {
            this.gridData = res.GridData;
            this.gridColumns = res.ColumnNames;
            this.legend = res.Legend;
            this.totalRecords =res.TotalRecordsCount;
            this.p=currentPage;               
          }
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
    else {
      this.ng4LoadingSpinnerService.show();
      this.dataservice.post(this.config.Emar_Dashboard_GetEmarHoleHistoryDashboard , emarObj)
        .subscribe(res => {
            
          
          if (res != null) {
            this.gridData = res.GridData;
            this.gridColumns = res.ColumnNames;
            this.totalRecords =res.TotalRecordsCount;
            this.p=currentPage;           
          }
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
}
  loadChart() {
    debugger;
    if ((this.dashboardForm.value.facilityName.length == 0 || this.dashboardForm.value.facilityName == undefined || this.dashboardForm.value.facilityName == null) || (this.dashboardForm.value.nursestationName.length == 0 || this.dashboardForm.value.nursestationName == undefined || this.dashboardForm.value.nursestationName == null)) {
      this.alertService.warn("Select facility and nursing station");
      this.ng4LoadingSpinnerService.hide();
      this.gridData = [];
      this.gridColumns = [];
    }
    else if ((this.dashboardForm.value.years == 0 || this.dashboardForm.value.years==null || this.dashboardForm.value.years==undefined) || (this.dashboardForm.value.months == 0 || this.dashboardForm.value.months==null || this.dashboardForm.value.months==undefined)) {
      this.alertService.warn("Enter valid date range");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dashboardForm.value.martype == 0 || this.dashboardForm.value.martype==null || this.dashboardForm.value.martype==undefined)
    {
      this.alertService.warn("Please select type");
      this.ng4LoadingSpinnerService.hide();
    }

    else if (this.dashboardForm.value.ddlresidents.length == 0 || this.dashboardForm.value.ddlresidents == undefined || this.dashboardForm.value.ddlresidents == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
    }

      else if (this.dashboardForm.value.gpidata.length == 0 || this.dashboardForm.value.gpidata == undefined || this.dashboardForm.value.gpidata == null) {
       this.alertService.warn("Select resident");
       this.ng4LoadingSpinnerService.hide();
     }
    else
    {
        
    this.month = this.dashboardForm.value.months;
    this.year = this.dashboardForm.value.years;
    this.close('.test');
    if (this.dashboardForm.value.martype == 3) {
      this.module = 'emarresidents'
      this.getGridData(this.module, this.month, this.year);
    }
  }
}
  //biometeric
  getReport() {
    if ((this.dashboardForm.value.facilityName.length == 0 || this.dashboardForm.value.facilityName == undefined || this.dashboardForm.value.facilityName == null) || (this.dashboardForm.value.nursestationName.length == 0 || this.dashboardForm.value.nursestationName == undefined || this.dashboardForm.value.nursestationName == null)) {
      this.alertService.warn("Select facility and nursing station");
      this.ng4LoadingSpinnerService.hide();
      this.gridData = [];
      this.gridColumns = [];
      this.totalRecords=0;
    }
    else if ((this.dashboardForm.value.years == 0 || this.dashboardForm.value.years==null || this.dashboardForm.value.years==undefined) || (this.dashboardForm.value.months == 0 || this.dashboardForm.value.months==null || this.dashboardForm.value.months==undefined)) {
      this.alertService.warn("Enter valid date range");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.dashboardForm.value.martype == 0 || this.dashboardForm.value.martype==null || this.dashboardForm.value.martype==undefined)
    {
      this.alertService.warn("Please select type");
      this.ng4LoadingSpinnerService.hide();
    }

    else if (this.dashboardForm.value.ddlresidents.length == 0 || this.dashboardForm.value.ddlresidents == undefined || this.dashboardForm.value.ddlresidents == null) {
      this.alertService.warn("Select resident");
      this.ng4LoadingSpinnerService.hide();
    }

      else if (this.dashboardForm.value.gpidata.length == 0 || this.dashboardForm.value.gpidata == undefined || this.dashboardForm.value.gpidata == null) {
       this.alertService.warn("Select resident");
       this.ng4LoadingSpinnerService.hide();
     }
    else{
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      let emarObj={
        UserId :this.userId,
        NursingstationId :this.nursestationid,
        Month :this.dashboardForm.value.months,
        Year :this.dashboardForm.value.years,
        patientName :this.patientid,
        FacilityId :this.dashboardForm.value.facilityName[0].Facility_Id,
        DateTime :dateTime,
        druglassification:this.gpitext
      }
      
    this.ng4LoadingSpinnerService.show(); 
      if (this.dashboardForm.value.martype == 3) { 
      this.ng4LoadingSpinnerService.show();
      this.dataservice.getReport(this.config.Emar_Report_TherepeuticalReport,emarObj)
        .subscribe((res) => {
           
          if( this.gridData.length == 0)
          {
            this.alertService.warn("No data available"); 
            this.ng4LoadingSpinnerService.hide();
            return false;
          }
          this.ng4LoadingSpinnerService.hide();
          var a = document.createElement("a");
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          var file = new Blob([res], { type: 'application/pdf' });
          var url = window.URL.createObjectURL(file);
          a.href = url;
          var x: Date = new Date();
          var link: string = "therapeuticalorderadministration" + x.getMonth() + "_" + x.getDay() + '.pdf';
          a.download = link.toLocaleLowerCase();
          a.click();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  }

  //Emar_Dashboard_Therepeutical

  // getReport1() {
  //   this.ng4LoadingSpinnerService.show();
  //   if((this.dashboardForm.value.facilityName.length==0 || this.dashboardForm.value.facilityName==undefined || this.dashboardForm.value.facilityName==null) || (this.dashboardForm.value.nursestationName.length==0|| this.dashboardForm.value.nursestationName==undefined ||this.dashboardForm.value.nursestationName==null))
  //   {
  //     this.alertService.warn("Please select facility and nursing station to display data");
  //     this.ng4LoadingSpinnerService.hide();
  //     this.gridData=[];
  //     this.gridColumns=[];
  //     this.totalRecords=0;
  //   }
  //   else if(this.dashboardForm.value.txtFromDate=="" || this.dashboardForm.value.txtToDate=="")
  //   {
  //     this.alertService.warn("Please select proper dates");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
  //     this.alertService.warn("“TO” date cannot be prior to “FROM” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
  //   {
  //     this.alertService.warn("“FROM” date cannot be after “TO” date");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else if(this.gpitext == "")
  //   {
  //     this.alertService.warn("Please select therapeutical classification");
  //     this.ng4LoadingSpinnerService.hide();
  //   }
  //   else
  //   {
  //     //Srikar
  //     let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
  //     this.MedRefList = {

  //       dashboardName : "",
  //       fromdate:this.fromDate,
  //       todate : this.toDate,
  //       userId : this.userId,
  //       nursingstationId :this.nursestationid,
  //       currentPage:0,
  //       pageSize:0,
  //       passTime :this.gpitext,
  //       facilityId : this.dashboardForm.value.facilityName[0].Facility_Id,
  //       dateTime : dateTime
  //     }
   
  //     //this.dataservice.getFile(this.config.Emar_Report_GetRefusedByResidentDetailsReport + this.fromDate + "/" + 
  //    // this.toDate+ "/" + this.userId + "/" + this.nursestationid +"/"+this.dashboardForm.value.facilityName[0].Facility_Id
  //    // +"/"+ dateTime+"/"+ this.gpitext)
  //     this.dataservice.getFile1(this.config.Emar_Report_GetRefusedByResidentDetailsReport,this.MedRefList)
  //     .subscribe((res) => {
  //       this.ng4LoadingSpinnerService.hide();
  //       var a = document.createElement("a");
  //       a.setAttribute('style', 'display:none;');
  //       document.body.appendChild(a);
  //       var file = new Blob([res], { type: 'application/pdf' });
  //       var url = window.URL.createObjectURL(file);
  //       a.href = url;
  //       var x: Date = new Date();
  //       var link: string = "Refused by res" + x.getMonth() + "_" + x.getDay() + '.pdf';
  //       a.download = link.toLocaleLowerCase();
  //       a.click();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  //   }
  // }



  
  getSelectedNurseStations() {
    if (this.dashboardForm.value.nursestationName.length != 0) {
      this.selectedItems.length = 0;
      this.dashboardForm.value.nursestationName.forEach(item => this.selectedItems.push(item.NurseStation_Id));
      this.nursestationid = "";
      this.nursestationid = this.selectedItems.join(',');
      this.getResidentDropData();
      this.getGpiData();
    }
    else if (this.dashboardForm.value.nursestationName.length == 0) {
      this.nursestationid = "";
      this.selectedItems = [];
      this.selectedResItem = [];
      this.residents=[];
      this.selectedItems.length = 0;
      this.gridData = [];
    this.gridColumns = [];
    this.totalRecords=0;
      this.alertService.error("Select at least one nursing station");
    }
  }
  onNurseStationSelect(item: any) {
    
    this.getSelectedNurseStations();
     this.getResidentDropData();
    this.getGpiData();
  }
  onNurseStationSelectAll(item: any) {
    this.dashboardForm.value.nursestationName = item;
    this.getSelectedNurseStations();
     this.getResidentDropData();
    this.getGpiData();
  }
  onNurseStationDeSelect(item: any) {

    this.getSelectedNurseStations();
    
     this.getResidentDropData();
    this.getGpiData();
  }
  onNurseStationDeSelectAll(item: any) {
    this.dashboardForm.value.nursestationName.length = 0;
    this.nursestationid = "";
    this.selectedItems = [];
    this.selectedResItem = [];
    this.residents=[];
    this.alertService.error("Select at least one nursing station");
  }

  onResidentSelect(item: any) {
    this.getSelectedResidents();
  }
  onResidentSelectAll(item: any) {
    this.dashboardForm.value.ddlresidents = item;
    this.getSelectedResidents();
  }
  onResidentDeSelect(item: any) {
    this.getSelectedResidents();
  }
  onResidentDeSelectAll(item: any) {
    this.dashboardForm.value.ddlresidents.length = 0;
    this.patientid = "";
    this.selectedResItem = [];
  }
  getSelectedResidents() { 
   if (this.dashboardForm.value.ddlresidents.length != 0) {
      this.selectedResidents=[];
      this.dashboardForm.value.ddlresidents.forEach(item => {
        this.selectedResidents.push(item.patientId);
      });
      this.patientid = "";
      this.patientid = this.selectedResidents.join(',');
    }
  }




  getGridDataOnPageChange(currentPage:any)
  {
    this.getGridData(this.module, this.month, this.year,currentPage);
  }
  getGpiDatainchange() {
    debugger;
     this.dataservice.get<any>(this.config.Emar_Facility_GpiList+this.nursestationid+"/"+this.fromDate + "/" + this.toDate+"/"+this.selectedLevel)
       .subscribe(res => {
         this.gpilist = res;
         //this.resMreason.push('1,2,3');
 
         this.gpitext = "";
         //this.selecteddItems=['1,2,3,4,5'];
         //this.selecteddItems = [res];
           
         this.dashboardForm.patchValue({
           gpidata : res,
         });
         this.getSelectedgpi();
        // this.getSelectedgpiLoad();
         //this.dashboardForm.value.ddlNotestype = this.selecteddItems;
         this.ng4LoadingSpinnerService.hide();
         
       }, 
         error => {
           this.alertService.error(error.message);
           this.ng4LoadingSpinnerService.hide();
         });
   }
   forChange()
{
  debugger;
  if(this.isPageLaod != 0 && this.isPageLaod >1)
  {

    this.fromDate = this.dashboardForm.value.txtFromDate;
    this.toDate = this.dashboardForm.value.txtToDate;
  
  this.ng4LoadingSpinnerService.show();
   if(this.fromDate=="" || this.toDate=="")
    {
      this.alertService.warn("Please select proper dates");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.fromDate!="" && this.toDate!="" && this.fromDate > this.toDate) {
      this.alertService.warn("“TO” date cannot be prior to “FROM” date");
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.fromDate!="" && this.toDate!="" && this.toDate < this.fromDate)
    {
      this.alertService.warn("“FROM” date cannot be after “TO” date");
      this.ng4LoadingSpinnerService.hide();
    }

    else
    {
      this.getGpiDatainchange();
    }
      
   
     
    }
    else{
      this.isPageLaod = this.isPageLaod+1;
    }
}

RefTypeChange()
{
  this.getGpiDatainchange();
  //console.log(this.selectedLevel +"testing");
}
  
}
 