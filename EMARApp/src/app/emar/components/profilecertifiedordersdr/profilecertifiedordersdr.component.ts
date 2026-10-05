import { Component, OnInit } from '@angular/core';
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
import { Screens, Activity } from '../../../models/useractivity.model';
import { ExceldownloadService } from 'src/app/services/shared/exceldownload.service';
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-profilecertifiedordersdr',
  templateUrl: './profilecertifiedordersdr.component.html',
  styleUrls: ['./profilecertifiedordersdr.component.css']
})
export class ProfilecertifiedordersdrComponent implements OnInit {

  myform: FormGroup;
  public template;
  iscollapsed = true;
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  public selectedfaItems = [];
  public selectednItems = [];
  public facilities: any[];
  public nurseStations: NurseStation[];
  public userId: number;
  public residents: any[] = [];
  pageConfig = {};
  p: number = 1;
  gridPagination = this.config.gridPagination;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public selectedFacItems = [];
  public modalUserCredential:boolean=false;
  public certifierObj:any;
  public certifiedDates: any[] = [];
  public certDate:any[]=[];
  public dropdownSettings_Date: any = {};
  public certifiedOrdersList:any[]=[];
  public physiciansdrop:any[]=[];
  public selectedphyItems:any[]=[];
  public dropdownSettings_Physician:any={};
  public selectedResItem: any[]=[];
  public dropdownSettings_ResID: any = {};
  public selectednItemsNew = [];
  
  constructor(private dataservice: DataService, private config: APIConfiguration, private sharedService: SharedService,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe ,) { }

  ngOnInit() {
    debugger;
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ProfileCertificationReport");
    debugger;
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
          ddlresidents:new FormControl(''),
          certifiedDate:new FormControl('')
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
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please select Resident',
        };
        this.dropdownSettings_Physician = {
          singleSelection: true,
          idField: "PhysicianNPI",
          textField: "PhysicianFullName",
          text: "Select",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please select Nursing Station',
        };
        this.dropdownSettings_ResID =
        {
          singleSelection: true,
          idField: "Patient_Id",
          textField: "PatientName",
          selectAllText: "Select All",
          itemsShowLimit: 1,
          allowSearchFilter: true,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please select Physician',
        };
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.userActivity();
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
        this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedFacItems = [];
            if (checkFacExist != undefined) {
              this.selectedFacItems.push(checkFacExist);
              this.getNurseStations(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacilities: this.selectedFacItems,
            });
          }
        }
       
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getNurseStations(facilityId: any) {
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    this.dataservice.get<NurseStation[]>(this.config.Emar_Facility_GetNurseStationsByFacilityId + userId +"/" + facilityId)
      .subscribe(res => {
        this.nurseStations = res;
        this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            this.selectednItemsNew = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
            }
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            this.selectednItems.forEach(item => this.selectednItemsNew.push(item.NurseStation_Id));
            this.getResidentDrop();
            //this.getPhysicianDrop();
          }
        }
        else if (this.loginUserReceNurseStation == undefined) {
          this.selectednItems = [];
          this.selectednItems=res;
          this.myform.patchValue({
            ddlnursestations: this.selectednItems
          });
          //res.forEach(item => this.selectednItems.push(item.NurseStation_Id));
          //this.getPhysicianDrop();
          this.getResidentDrop();
        }
      },
        error => {
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
        this.getResidentDrop();
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getResidentDrop() {
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.residents = [];
      this.selectedResItem=[];
      this.alertService.error("Use filters to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // else if(this.myform.value.physician.length==0)
    // {
    //   this.residents = [];
    //   this.selectedResItem=[];
    //   this.alertService.error("Select physician to display order list.")
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else {
      debugger
      let nsList = this.myform.value.ddlnursestations.map(n => n.NurseStation_Id).join(',');
      let phyNpi=this.myform.value.physician.length!=0 && this.myform.value.physician!=undefined && this.myform.value.physician!=null && this.myform.value.physician!=""?this.myform.value.physician[0].PhysicianNPI:null;
      this.dataservice.get<any[]>(this.config.Emar_ResidentDemographic_GetProfileCertifyOrderResidentGridData + nsList)
      .subscribe((res: any) => {
        this.residents = res;
        if (this.residents.length>0)
        {
          this.selectedResItem = [];
          this.selectedResItem.push(this.residents[0]);
          this.myform.patchValue({
            ddlresidents:this.selectedResItem,
          });
          this.getCertifiedDatesDrop();
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  getCertifiedDatesDrop() {
    this.ng4LoadingSpinnerService.show();
    let date = new Date();
    let resId=this.myform.value.ddlresidents.length!=0 && this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents!=""?this.myform.value.ddlresidents[0].Patient_Id:null;
    let fromDate = this.dateFormatPipe.transformISODate(this.dateFormatPipe.transform((new Date(date.setMonth(date.getMonth() - 11))).setHours(0)).toString().substring(0, 10));
    let toDate = this.dateFormatPipe.transformISODate(new Date());
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetProfileCertifiedDatesDrop + this.userId+"/"+resId+"/"+fromDate+"/"+toDate)
      .subscribe(res => {
        this.certifiedDates = res;
        if (this.certifiedDates.length>0)
        {
          this.certDate = [];
          this.certDate.push(this.certifiedDates[0]);
          this.myform.patchValue({
            certifiedDate:this.certDate,
          });
           this.getCertifiedOrdersList();
        }
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getCertifiedOrdersList() {
    this.ng4LoadingSpinnerService.show();
    let TimeId=this.certDate[0].CertifyTime_ID;
    let resId=this.myform.value.ddlresidents.length!=0 && this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents!=""?this.myform.value.ddlresidents[0].Patient_Id:null;
    let api =TimeId.startsWith('P')?(this.config.Emar_Orders_GetAllProfilecertifiedOrderByDate + this.userId+"/" +parseInt(TimeId.substring(1))+"/"+resId):(this.config.Emar_Orders_GetAllcertifiedOrderByDate + this.userId+"/" +parseInt(TimeId)+"/"+resId)
    this.dataservice.get<any[]>(api)
      .subscribe(res => {
        this.certifiedOrdersList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.selectednItems=[];
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.certifiedOrdersList=[];
    this.myform.patchValue({
      ddlnursestations: this.selectednItems,
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.getNurseStations(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.selectednItems=[];
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      ddlnursestations: this.selectednItems,
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.certifiedOrdersList=[];
    this.alertService.warn("Please select facility and nursing station to display data");
    this.ng4LoadingSpinnerService.hide();
  }
  onNurseStationSelect(item: any) {
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.certifiedOrdersList=[];
    this.getSelectedNurseStations();
  }
  onNurseStationSelectAll(item: any) {
    this.myform.value.ddlnursestations = item;
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.certifiedOrdersList=[];
    this.getSelectedNurseStations();
  }
  onNurseStationDeSelect(item: any) {
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.certifiedOrdersList=[];
    this.getSelectedNurseStations();
  }
  onNurseStationDeSelectAll(item: any) {
    this.physiciansdrop=[];
    this.selectedphyItems=[];
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.loginUserReceNurseStation = undefined;
    this.certifiedOrdersList=[];
    this.myform.value.ddlnursestations.length = 0;
    this.selectednItems = [];
    this.alertService.error("Select at least one nursing station to display data");
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ResidentProfileCertifiedOrdersReport, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getSelectedNurseStations() {
    if (this.myform.value.ddlnursestations.length != 0) {
      this.getResidentDrop();
      //this.getPhysicianDrop();
    }
    else if (this.myform.value.ddlnursestations.length == 0) {
      this.selectednItems=[];
      this.alertService.error("Select at least one nursing station to display data");
    }
  }
  onPhysicianSelect(item:any)
  {
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.certifiedOrdersList=[];
    this.getResidentDrop();
  }
  onPhysicianDeSelect(item:any)
  {
    this.residents=[];
    this.selectedResItem=[];
    this.certifiedDates=[];
    this.certDate=[];
    this.selectedphyItems=[];
    this.myform.patchValue({
      physician: this.selectedphyItems,
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.certifiedOrdersList=[];
    this.alertService.error("please select physician to display residents list.");
  }
  onResidentSelect(item:any)
  {
    this.certifiedDates=[];
    this.certDate=[];
    this.myform.patchValue({
      certifiedDate:this.certDate
    });
    this.certifiedOrdersList=[];
    this.getCertifiedDatesDrop();
  }
  onResidentDeSelect(item:any)
  {
    this.certifiedDates=[];
    this.certDate=[];
    this.selectedResItem=[];
    this.myform.patchValue({
      ddlresidents:this.selectedResItem,
      certifiedDate:this.certDate
    });
    this.certifiedOrdersList=[];
    this.alertService.error("please select resident to display last certified times.");
  }
  onDateSelect(item:any)
  {
    this.certifiedOrdersList=[];
    this.getCertifiedOrdersList();
  }
  onDateDeSelect(item:any)
  {
    this.certifiedOrdersList=[];
    this.certDate=[];
    this.myform.patchValue({
      certifiedDate:this.certDate
    });
    this.alertService.error("please select last certified time to display Certified Orders.");
  }
  loadChart()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.certifiedOrdersList=[];
      this.alertService.error("Use filters to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // else if(this.myform.value.physician.length==0)
    // {
    //   this.certifiedOrdersList=[];
    //   this.alertService.error("Select physician to display order list.")
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else if(this.myform.value.ddlresidents.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("please select resident to display orders list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.myform.value.certifiedDate.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("Please select last certified date to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    else
    {
      this.close('.test');
      this.getCertifiedOrdersList();
    }
  }
  exportCertifiedOrdersToPdf()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.certifiedOrdersList=[];
      this.alertService.error("Use filters to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // else if(this.myform.value.physician.length==0)
    // {
    //   this.certifiedOrdersList=[];
    //   this.alertService.error("Select physician to display order list.")
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else if(this.myform.value.ddlresidents.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("please select resident to display orders list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.myform.value.certifiedDate.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("Please select last certified date to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // if(this.certDate.length==0)
    // {
    //   this.alertService.warn("Please select last certified date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else
    {
      this.ng4LoadingSpinnerService.show();
      let cert_TimeId=this.certDate[0].CertifyTime_ID;
      let facilityId=this.myform.value.ddlfacilities[0].Facility_Id;
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      let resId=this.myform.value.ddlresidents.length!=0 && this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents!=""?this.myform.value.ddlresidents[0].Patient_Id:null;
      this.dataservice.getFile(this.config.Emar_Reports_GetProfileCertifiedOrderReport + this.userId +"/"+ cert_TimeId+"/"+resId +"/"+facilityId+"/"+null+"/"+dateTime)
      .subscribe((res) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "Profile_Certification" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  getExcel()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.myform.value.ddlfacilities.length == 0 || this.myform.value.ddlnursestations.length == 0) {
      this.certifiedOrdersList=[];
      this.alertService.error("Use filters to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // else if(this.myform.value.physician.length==0)
    // {
    //   this.certifiedOrdersList=[];
    //   this.alertService.error("Select physician to display order list.")
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else if(this.myform.value.ddlresidents.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("please select resident to display orders list.")
      this.ng4LoadingSpinnerService.hide();
    }
    else if(this.myform.value.certifiedDate.length==0)
    {
      this.certifiedOrdersList=[];
      this.alertService.error("Please select last certified date to display order list")
      this.ng4LoadingSpinnerService.hide();
    }
    // if(this.certDate.length==0)
    // {
    //   this.alertService.warn("Please select last certified date");
    //   this.ng4LoadingSpinnerService.hide();
    // }
    else
    {
      this.ng4LoadingSpinnerService.show();
      let cert_TimeId=this.certDate[0].CertifyTime_ID;
      let facilityId=this.myform.value.ddlfacilities[0].Facility_Id;
      let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
      let resId=this.myform.value.ddlresidents.length!=0 && this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents!=""?this.myform.value.ddlresidents[0].Patient_Id:null;
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetProfileCertifiedOrderExcel + this.userId +"/"+ cert_TimeId+"/"+resId +"/"+facilityId+"/"+null+"/"+dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        // this.excelforuseractivity = res;
        if (res.length != 0)
        {
          res.forEach(item => (item["Start Date"] = this.dateFormatPipe.transform(item["Start Date"])));
          res.forEach(item => (item["End Date"] = this.dateFormatPipe.transform(item["End Date"])));
          this.excelDownload(res, "ProfileCertification");
        }
        else{
          this.alertService.warn("No data available");
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  
  excelDownload(objArray,name)
  {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(objArray);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
      var a = document.createElement("a");
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      var blob = new Blob([excelBuffer], { type: 'EXCEL_TYPE' });
      var url= window.URL.createObjectURL(blob);
      a.href = url;
      var x:Date = new Date();
      var link:string =name + x.getMonth() +  "_" +  x.getDay() + '.csv';
      a.download = link.toLocaleLowerCase();
      a.click();
  }
  // convert Json to CSV data
  ConvertToCSV(objArray) {
      
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';
    var row = "";
  
    for (var index in objArray[0]) {
        //Now convert each value to string and comma-separated
        row += index + ',';
    }
    row = row.slice(0, -1);
    //append Label row with line break
    str += row + '\r\n';
  
    for (var i = 0; i < array.length; i++) {
        var line = '';
        for (var index in array[i]) {
            if (line != '') line += ','
  
            line += array[i][index];
        }
        str += line + '\r\n';
    }
    return str;
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
}
