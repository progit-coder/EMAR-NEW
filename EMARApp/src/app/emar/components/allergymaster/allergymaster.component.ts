
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AllergyInfoMaster } from '../../../models/allergyandicd.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { SharedService } from '../../../services/shared/shared.service';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-allergymaster',
  templateUrl: './allergymaster.component.html',
  styleUrls: ['./allergymaster.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]
})
export class AllergymasterComponent implements OnInit {
  public template;
  classform: FormGroup;
  drugform: FormGroup;
  importform: FormGroup;
  public allergyClassObj: AllergyInfoMaster;
  public allergyDrugObj: AllergyInfoMaster;
  public classAllergyID: number = 0;
  private drugAllergyID: number = 0;
  private searchByClass: string;
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  private searchByDrug: string;
  public getSearchByClassData: AllergyInfoMaster[] = [];
  public checkAllergy: AllergyInfoMaster[] = [];
  public getSearchByDrugData: AllergyInfoMaster[] = [];
  public getAllClassData: AllergyInfoMaster[];
  public getAllDrugData: AllergyInfoMaster[];
  public drugMaster: any[] = [];
  public classMaster: any[] = [];
  allergyClassFile: File;
  loading = false;
  public classM: any[] = [];
  public classList: any[] = [];
  public inactivecheckbox: boolean = false;
  public drugList: any[] = [];
  public inactivedrugcheckbox: boolean = false;
  pageConfig = {};
  pc: number = 1;
  pd: number = 1;
  gridPagination = this.config.gridPagination;
  errorMessage: string = '';
  drugErrorMessage: string = '';
  public selectedRecords:any[]=[];
  CheckAllClass: boolean = false;
  public UpdateStatusClass: boolean = true;
  CheckAllDrug: boolean = false;
  public UpdateStatusDrug: boolean = true;
  constructor(private dataservice: DataService, private config: APIConfiguration, private exceldownload: ExceldownloadService,
    private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, public sharedService: SharedService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AllergyMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.getClassMaster();

    this.classform = new FormGroup({
      classID: new FormControl('', [Validators.required, Validators.pattern(this.config.numeric)]),
      className: new FormControl('', Validators.required),
      status: new FormControl(true, Validators.required),
      searchTextClass: new FormControl()
    });
    this.drugform = new FormGroup({
      drugID: new FormControl('', [Validators.required, Validators.pattern(this.config.numeric)]),
      drugName: new FormControl('', Validators.required),
      status: new FormControl(true, Validators.required),
      searchTextDrug: new FormControl()
    });
    this.importform = new FormGroup({
      impFile: new FormControl('', Validators.required),
      impType: new FormControl(true, Validators.required)
    });
    if (this.pageConfig["AccessWrite"] == 0) {
      // this.classform.disable();
      // this.drugform.disable();
      // this.importform.disable();
    }
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.AllergyMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  insertClassAllegry() {
    this.ng4LoadingSpinnerService.show();
    this.allergyClassObj = {
      Allergy_Id: this.classAllergyID,
      AllergyDesc_Id: this.classform.value.classID,
      AllergyDesc: this.classform.value.className,
      Allergy_Status: (this.classform.value.status == true ? 1 : 0),
      Allergy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Allergy_CreatedOn: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      AllergyTypeMaster_Id: 1,
      CreatedBy: ''
    };
    this.dataservice.post(this.config.Emar_Allergy_InsertUpdateAllergy, this.allergyClassObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.success("Save successful");
          this.getClassMaster();
          this.inactivecheckbox = false;
        }
        else if (res == 2) {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.warn("Allergy Class already exist");
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.reSetClass();
  }
  reSetClass() {
    this.ng4LoadingSpinnerService.show();
    this.classAllergyID = 0;
    this.classform.reset();
    this.classform.patchValue({
      status: 1,
    });
    this.ng4LoadingSpinnerService.hide();
    // this.classform.patchValue({
    //   classID: '',
    //   className: '',
    //   status: true,
    //   searchTextClass: ''
    // });
    this.getSearchByClassData = [];
    // this.CheckAllClass = false;
    // this.UpdateStatusClass=true;
    // this.selectedRecords=[];
  }
  insertDrugAllegry() {
    this.ng4LoadingSpinnerService.show();
    this.allergyDrugObj = {
      Allergy_Id: this.drugAllergyID,
      AllergyDesc_Id: this.drugform.value.drugID,
      AllergyDesc: this.drugform.value.drugName,
      Allergy_Status: (this.drugform.value.status == true ? 1 : 0),
      Allergy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Allergy_CreatedOn: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      AllergyTypeMaster_Id: 2,
      CreatedBy: ''
    };
    this.dataservice.post(this.config.Emar_Allergy_InsertUpdateAllergy, this.allergyDrugObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getDrugMaster();
          this.reSetDrug();
          this.inactivedrugcheckbox = false;

        }
        else if (res == 2) {
          this.alertService.warn("Allergy Drug already exist");
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.ng4LoadingSpinnerService.hide();
  }
  reSetDrug() {
    this.ng4LoadingSpinnerService.show();
    this.drugform.reset();
    this.drugAllergyID = 0;
    this.drugform.patchValue({
      status: 1,
    });
    this.ng4LoadingSpinnerService.hide();
    // this.drugform.patchValue({
    //   drugID: '',
    //   drugName: '',
    //   status: true,
    //   searchTextDrug: ''
    // });
    this.getSearchByDrugData = [];
    // this.CheckAllDrug = false;
    // this.UpdateStatusDrug=true;
    // this.selectedRecords=[];
  }
  GetAllergyByClassByName() {
    this.errorMessage = '';
    this.drugErrorMessage = '';
    this.searchByClass = this.classform.value.searchTextClass;
    let searchValue = '';
    this.searchByClass = this.searchByClass != undefined && this.searchByClass != null ? this.searchByClass.replace(/[&\\\#,+()$~%'":.*?<>{}\s]/g, '') : '';
    if (this.searchByClass.trim() != '') {
      if (this.searchByClass.indexOf('.') == this.searchByClass.length - 1) {
        this.errorMessage = 'Please enter valid data';
        this.classform.patchValue({
          searchTextClass: ''
        });
        this.classList = [];
      }
      else {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.get<AllergyInfoMaster[]>(this.config.Emar_Allergy_GetAllergiesByName + this.searchByClass + "/" + 1)
          .subscribe(res => {
            this.classList = res;
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          })
      }
    }
    else {
      // this.errorMessage = 'Please enter valid data';
      // this.classform.patchValue({
      //   searchTextClass: ''
      // });
      // this.classList = [];
      this.getClassMaster();
    }
  }
  GetAllergyByClassByNameEdit(Allergy_Id: number, AllergyDesc_Id: string, AllergyDesc: string, Allergy_Status: number) {
    this.ng4LoadingSpinnerService.show();
    this.classAllergyID = Allergy_Id;
    // this.classAllergyID = CAllergy_Id;
    this.classform.patchValue({
      classID: AllergyDesc_Id,
      className: AllergyDesc,
      status: Allergy_Status
    });
    window.scroll(0, 0);
    this.ng4LoadingSpinnerService.hide();
  }

  GetAllergyByDrugByName() {
    this.errorMessage = '';
    this.drugErrorMessage = '';
    this.searchByDrug = this.drugform.value.searchTextDrug;
    let searchValue = '';
    this.searchByDrug = this.searchByDrug != undefined && this.searchByDrug != null ? this.searchByDrug.replace(/[&\\\#,+()$~%'":.*?<>{}\s ]/g, '') : '';
    if (this.searchByDrug.trim() != '') {
      if (this.searchByDrug.indexOf('.') == this.searchByDrug.length - 1) {
        this.drugErrorMessage = 'Please enter valid data';
        this.drugform.patchValue({
          searchTextDrug: ''
        });
        this.drugList = [];
      }
      else {
        this.ng4LoadingSpinnerService.show();
        this.dataservice.get<AllergyInfoMaster[]>(this.config.Emar_Allergy_GetAllergiesByName + this.searchByDrug + "/" + 2)
          .subscribe(res => {
            this.drugList = res;
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
      }
    }
    else {
      // this.drugErrorMessage = 'Please enter valid data';
      // this.drugform.patchValue({
      //   searchTextDrug: ''
      // });
      // this.drugList = [];
      this.getDrugMaster();
    }
  }
  getDrugMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Allergy_GetAllAllergiesList + 2)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.drugMaster = [];
        this.drugMaster = res.filter(item => item.AllergyTypeMaster_Id == 2);
        this.drugList = this.drugMaster.filter(d => d.Allergy_Status == 1);
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  showInactiveDrugRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.drugList = this.drugMaster.filter(c => c.Allergy_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getDrugMaster();
    }
  }
  SearchEmpty1(event) {
    if (event.key === "Enter") {
      this.GetAllergyByDrugByName();
    }
  }

  getClassMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Allergy_GetAllAllergiesList + 1)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.classMaster = [];
        this.classMaster = res.filter(item => item.AllergyTypeMaster_Id == 1);
        this.classList = this.classMaster.filter(c => c.Allergy_Status == 1);
        this.getDrugMaster();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  SearchEmpty(event) {
    if (event.key === "Enter") {
      this.GetAllergyByClassByName();
    }
  }
  showInactiveClassRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.classList = this.classMaster.filter(c => c.Allergy_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getClassMaster();
    }
  }
  GetAllergyByDrugByNameEdit(Allergy_Id: number, AllergyDesc_Id: string, AllergyDesc: string, Allergy_Status: number) {
    this.ng4LoadingSpinnerService.show();
    this.drugAllergyID = Allergy_Id;
    this.drugform.patchValue({
      drugID: AllergyDesc_Id,
      drugName: AllergyDesc,
      status: Allergy_Status
    });
    window.scroll(0, 0);
    this.ng4LoadingSpinnerService.hide();
  }
  InsertAllergyFromFile() {
    let type: number = (this.importform.value.impType == true ? 1 : 0);
    if (type == 1) {
      this.importAllergyClassData();
    }
    else {
      this.importAllergyDrugData();
    }
    this.importform.patchValue({
      impFile: '',
      impType: true
    });
  }
  fileUploadChange(event: any): void {
    this.allergyClassFile = null;
    if (event.target.files && event.target.files[0]) {
      this.allergyClassFile = event.target.files[0];
    }
  }
  importAllergyClassData() {
    this.allergyClassObj = {
      AllergyTypeMaster_Id: 1,
      Allergy_Id: 0,
      AllergyDesc_Id: '',
      AllergyDesc: '',
      Allergy_Status: 1,
      Allergy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Allergy_CreatedOn: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      CreatedBy: ''
    };
    this.ng4LoadingSpinnerService.show();
    let formData: FormData = new FormData();
    formData.append('excel', this.allergyClassFile);

    this.dataservice.postFormData(this.config.Emar_Allergy_InsertAllergyByClassFromFile, this.allergyClassObj, formData)
      .subscribe(res => {
        console.log('Inserted AllergyByClass count:' + res);
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

  importAllergyDrugData() {
    this.allergyClassObj = {
      AllergyTypeMaster_Id: 2,
      Allergy_Id: 0,
      AllergyDesc_Id: '',
      AllergyDesc: '',
      Allergy_Status: 1,
      Allergy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Allergy_CreatedOn: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      CreatedBy: ''
    };
    this.ng4LoadingSpinnerService.show();
    let formData: FormData = new FormData();
    formData.append('excel', this.allergyClassFile);

    this.dataservice.postFormData(this.config.Emar_Allergy_InsertAllergyByDrugFromFile, this.allergyDrugObj, formData)
      .subscribe(res => {
        console.log('Inserted AllergyByDrug count:' + res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  downloadExcelForAllergyClass() {
    this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetAllergyMasterExcel + 1 )
        .subscribe(res => {
          if(res.length!=0)
          res.forEach(item => item["Allergy Created On"] = this.dateFormatPipe.transform(item["Allergy Created On"]));
          this.exceldownload.excelDownload(res, "AllergyByClass");
          this.ng4LoadingSpinnerService.hide();
          //this.getSearchByClassData = [];
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  downloadExcelForAllergyDrug() {
    this.ng4LoadingSpinnerService.show();
      this.dataservice.get<any[]>(this.config.Emar_Reports_GetAllergyMasterExcel + 2)
        .subscribe(res => {
          if(res.length!=0)
          res.forEach(item => item["Allergy Created On"] = this.dateFormatPipe.transform(item["Allergy Created On"]));
          this.exceldownload.excelDownload(res, "AllergyByDrug");
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  exportAllergyDrugToPdf() {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_ICD_ExportAllergyDrugToPDF + userId +"/"+dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "AllergiesByDrug_" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  exportAllergyClassToPdf() {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_ICD_ExportAllergyClassToPDF + userId +"/"+ dateTime)
      .subscribe((res) => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "AllergiesByClass_" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  checkAllergyClassID(): any {
    let AllergyDesc_Id = this.classform.value.classID.toLowerCase();
    let result1 = this.classMaster.find(x => (x.AllergyDesc_Id).toLowerCase() === AllergyDesc_Id);
    if (result1) {
      this.alertService.error("ClassID Already Exists");
      this.classform.patchValue({
        classID: ''
      });
    }
    else { }
  }
  //   omit_special_char(event)
  // {   
  //    var k;  
  //    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
  //    return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57)); 
  // }

  checkAllergyDrugID(): any {
    let AllergyDesc_Id = this.drugform.value.drugID.toLowerCase();
    let result = this.drugMaster.find(x => (x.AllergyDesc_Id).toLowerCase() === AllergyDesc_Id);
    if (result) {
      this.alertService.error("DrugID Already Exists");
      this.drugform.patchValue({
        drugID: ''
      });
    }
    else { }
  }
  getHistoryById(Allergy_Id: number) {
    this.auditTable = {
      "tableName": "AllergyInfoMaster",
      "recordId": Allergy_Id
    }
    this.modalHistoryIsOpen = true;
  }
  // excelDownloadA(objArray, name) {
  //   if (objArray.length != 0)
  //     objArray.forEach(function (x) {
  //       x.Allergy_CreatedOn = x.Allergy_CreatedOn.substring(0, 10);
  //       x.AllergyDesc = x.AllergyDesc.replace(/,/g, ':');
  //       x.AllergyTypeMaster_Id = x.AllergyTypeMaster_Id == 1 ? "Allergy By Class" : "Allergy By Drug";
  //       delete x.Allergy_Status, delete x.Allergy_CreatedBy

  //     });

  //   var csvData = this.exceldownload.ConvertToCSV(objArray);
  //   var a = document.createElement("a");
  //   a.setAttribute('style', 'display:none;');
  //   document.body.appendChild(a);
  //   var blob = new Blob([csvData], { type: 'text/csv' });
  //   var url = window.URL.createObjectURL(blob);
  //   a.href = url;
  //   var x: Date = new Date();
  //   var link: string = name + x.getMonth() + "_" + x.getDay() + '.csv';
  //   a.download = link.toLocaleLowerCase();
  //   a.click();
  // }
  // excelDownload(objArray, name) {
  //   if (objArray.length != 0)
  //     objArray.forEach(function (x) {
  //       x.Allergy_CreatedOn = x.Allergy_CreatedOn.substring(0, 10);
  //       x.AllergyTypeMaster_Id = x.AllergyTypeMaster_Id == 2 ? "Allergy By Drug" : "Allergy By Class";
  //       delete x.Allergy_Status, delete x.Allergy_CreatedBy

  //     });
  //   var csvData = this.ConvertToCSV(objArray);
  //   var a = document.createElement("a");
  //   a.setAttribute('style', 'display:none;');
  //   document.body.appendChild(a);
  //   var blob = new Blob([csvData], { type: 'text/csv' });
  //   var url = window.URL.createObjectURL(blob);
  //   a.href = url;
  //   var x: Date = new Date();
  //   var link: string = name + x.getMonth() + "_" + x.getDay() + '.csv';
  //   a.download = link.toLocaleLowerCase();
  //   a.click();

  // }
  // ConvertToCSV(objArray) {

  //   var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
  //   var str = '';
  //   var row = "";

  //   for (var index in objArray[0]) {
  //     //Now convert each value to string and comma-separated
  //     row += index + ',';
  //   }
  //   row = row.slice(0, -1);
  //   //append Label row with line break
  //   str += row + '\r\n';

  //   for (var i = 0; i < array.length; i++) {
  //     var line = '';
  //     for (var index in array[i]) {
  //       if (line != '') line += ','

  //       line += array[i][index];
  //     }
  //     str += line + '\r\n';
  //   }
  //   return str;
  // }

  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  onCheckAllClass(event) {
    if (event == true) {
      this.UpdateStatusClass = false;
      this.CheckAllClass = true;
      this.classList.forEach(element => {
        element.Allergy_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Allergy_CreatedOn=this.dateFormatPipe.dateWithTime(new Date());
        //element.Allergy_Status=1;
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAllClass = false;
      this.UpdateStatusClass = true;
      this.selectedRecords = [];
    }
  }
  onselectClassRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatusClass = false;
      item.Allergy_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Allergy_CreatedOn=this.dateFormatPipe.dateWithTime(new Date());
      //item.Allergy_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Allergy_Id == item.Allergy_Id );
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatusClass = true;
      }
      else {
        this.UpdateStatusClass = false;
      }
    }
  }
  updateClassStatus()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatusClass = true;
    }
    else {
      this.dataservice.post(this.config.Emar_ICD_UpdateAllergysStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatusClass = true;
            this.inactivecheckbox=false;
            this.CheckAllClass = false;
            this.getClassMaster();
            this.selectedRecords = [];
            
          }
          else if(res == 0)
          {
            this.alertService.error("Status Updated Failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  onCheckAllDrug(event) {
    if (event == true) {
      this.UpdateStatusDrug = false;
      this.CheckAllDrug = true;
      this.drugList.forEach(element => {
        element.Allergy_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Allergy_CreatedOn=this.dateFormatPipe.dateWithTime(new Date());
        //element.Allergy_Status=1;
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAllDrug = false;
      this.UpdateStatusDrug = true;
      this.selectedRecords = [];
    }
  }
  onselectDrugRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatusDrug = false;
      item.Allergy_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Allergy_CreatedOn=this.dateFormatPipe.dateWithTime(new Date());
      //item.Allergy_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Allergy_Id == item.Allergy_Id );
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatusDrug = true;
      }
      else {
        this.UpdateStatusDrug = false;
      }
    }
  }
  updateDrugStatus()
  {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatusDrug = true;
    }
    else {
      this.dataservice.post(this.config.Emar_ICD_UpdateAllergysStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatusDrug = true;
            this.inactivedrugcheckbox=false;
            this.CheckAllDrug = false;
            this.getDrugMaster();
            this.selectedRecords = [];
            
          }
          else if(res == 0)
          {
            this.alertService.error("Status Updated Failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
}