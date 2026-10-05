import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ICD10 } from '../../../models/allergyandicd.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { stringify } from '@angular/core/src/util';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-icdmaster',
  templateUrl: './icdmaster.component.html',
  styleUrls: ['./icdmaster.component.css'],
  providers: [ExceldownloadService, DataService, APIConfiguration]
})
export class IcdmasterComponent implements OnInit {
  public template;
  icdFile: File;
  icdObj: ICD10;
  errorMessage: string;
  icdform: FormGroup;
  icdImport: FormGroup;
  private icd10ID: number = 0;
  public icdList: any[] = [];
  public inactivecheckbox: boolean = false;
  public getIcdsearchData: ICD10[] = [];
  private getAllIcdsData: ICD10[];
  p: number = 1;
  gridPagination = this.config.gridPagination;
  icd10rawformat: string;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  pageConfig: {};
  totalRecords: number = 0;
  public savedisable=false;
  constructor(private dataservice: DataService, private config: APIConfiguration, private exceldownload: ExceldownloadService, private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, public sharedService: SharedService, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ICD10Master");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.icdform = new FormGroup({
          rawFormat: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.pattern(this.config.alphaNumeric)]),
          formatted: new FormControl('', Validators.required),
          description: new FormControl('', Validators.required),
          status: new FormControl(true, Validators.required),
          searchText: new FormControl()
        });
        this.icdImport = new FormGroup({
          ICDfile: new FormControl('', Validators.required)
        });
        this.userActivity();
        //this.GetICDGrid();
        this.GetICD10ByRawFormat(1, 1);
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ICD10Master, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  InsertICD10() {
    this.savedisable=true;
    this.ng4LoadingSpinnerService.show();
    this.icdObj = {
      ICD10_Id: this.icd10ID,
      ICD10_RawFormat: this.icdform.value.rawFormat,
      ICD10_Formatted: this.icdform.value.formatted,
      ICD10_Description: this.icdform.value.description,
      ICD10_Status: (this.icdform.value.status == true ? 1 : 0),
      ICD10_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      ICD10_CreatedDate: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      CreatedBy: ""
    };
    this.dataservice.post(this.config.Emar_ICD_InsertICD10, this.icdObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          //this.GetICDGrid();
          this.GetICD10ByRawFormat(1, 1);
          this.inactivecheckbox = false;
          this.reSet();
          this.savedisable=false;
        }
        else if (res == 2) {
          this.alertService.warn("Record already exist");
          this.savedisable=false;
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.savedisable=false;
        this.alertService.error(error.message)
      });
  }
  reSet() {
    this.ng4LoadingSpinnerService.show();
    this.icdform.reset();
    this.icdform.patchValue({
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.icdform.patchValue({
    //   rawFormat: '',
    //   formatted: '',
    //   description: '',
    //   status: true
    // });
  }
  // GetICDGrid() {
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<any[]>(this.config.Emar_ICD_GetICDGrid)
  //     .subscribe(res => {
  //       this.getIcdsearchData = res;
  //       this.icdList = this.getIcdsearchData.filter(i => i.ICD10_Status == 1);
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  // }
  getHistoryById(ICD10Id: number) {
    this.auditTable = {
      "tableName": "ICD",
      "recordId": ICD10Id
    }
    this.modalHistoryIsOpen = true;

  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  SearchEmpty(event) {
    if (event.key === "Enter") {
      //this.GetICDGrid();
      this.inactivecheckbox == true ? this.GetICD10ByRawFormat(1, 0) : this.GetICD10ByRawFormat(1, 1);
    }
  }
  showInactiveRecords(value: any) {
    debugger;
    if (value == true) {
      this.GetICD10ByRawFormat(1, 0);
    }
    else if (value == false) {
      //this.GetICDGrid();
      this.GetICD10ByRawFormat(1, 1);
    }
  }
  searchICD10() {
    this.inactivecheckbox == true ? this.GetICD10ByRawFormat(1, 0) : this.GetICD10ByRawFormat(1, 1);
  }
  GetICD10ByRawFormat(pageNumber: number, status: number, pageClick?: number) {
    this.errorMessage = '';
    this.p = pageNumber;
    pageClick == 1 ? (this.inactivecheckbox == true ? status = 0 : 1) : status = status;
    let searchText = this.icdform.value.searchText == '' ? null : this.icdform.value.searchText;
    if ((searchText != null && searchText.trim() != '') || (searchText == null || searchText == undefined)) {
      if (searchText != null && searchText.trim() != '' && searchText.indexOf('.') == searchText.length - 1) {
        this.errorMessage = 'Please enter valid data';
        this.icdform.patchValue({
          searchText: ''
        });
        this.icdList = [];
      }
      else if (searchText == null || searchText == undefined || (searchText != null && searchText.trim() != '' && searchText.indexOf('.') != searchText.length - 1)) {
        let obj =
        {
          CurrentPage: pageNumber,
          PageSize: this.gridPagination,
          SearchText: searchText,
          status: status
        }
        this.ng4LoadingSpinnerService.show();
        this.dataservice.post(this.config.Emar_ICD_GetICD10ByRawFormat, obj)
          .subscribe(res => {
            //this.getIcdsearchData = res.Data;
            this.totalRecords = res.TotalRecords;
            //this.icdList = this.getIcdsearchData.filter(i => i.ICD10_Status == 1);
            this.icdList = res.Data;
            this.ng4LoadingSpinnerService.hide();
          }, error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          })
      }
    }
    else {
      this.errorMessage = 'Please enter valid data';
      this.icdform.patchValue({
        searchText: ''
      });
      this.icdList = [];
    }
  }
  GetICD10Edit(ICD10_Id: number, ICD10_RawFormat: string, ICD10_Formatted: string, ICD10_Description: string, ICD10_Status: number) {
    this.ng4LoadingSpinnerService.show();
    this.icd10ID = ICD10_Id;
    this.icdform.patchValue({
      rawFormat: ICD10_RawFormat,
      formatted: ICD10_Formatted,
      description: ICD10_Description,
      status: ICD10_Status
    });

    window.scroll(0, 0);
    this.ng4LoadingSpinnerService.hide();
  }


  importICDData() {
    this.icdObj = {
      ICD10_Id: 0,
      ICD10_RawFormat: '',
      ICD10_Formatted: '',
      ICD10_Description: '',
      ICD10_Status: 1,
      ICD10_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      ICD10_CreatedDate: this.dateFormatPipe.dateWithTimeFormat(new Date()),
      CreatedBy: ''
    };

    this.ng4LoadingSpinnerService.show();
    let formData: FormData = new FormData();
    formData.append('excel', this.icdFile);

    this.dataservice.postFormData(this.config.Emar_ICD_InsertICD10FromFile, this.icdObj, formData)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }

  fileUploadChange(event: any): void {
    this.icdFile = null;
    if (event.target.files && event.target.files[0]) {
      this.icdFile = event.target.files[0];
    }
  }
  downloadExcelForAllergyDrug() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Reports_GetICDMasterExcel)
      .subscribe(res => {
          if(res.length!=0)
          res.forEach(item => item["Created On"] = this.dateFormatPipe.transform(item["Created On"]));
          this.exceldownload.excelDownload(res, "ICD10");
          this.ng4LoadingSpinnerService.hide();
          //this.alertService.success('Exported successfully');
        //}
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }

  // exportICD10ToPdf() {

  //this.ng4LoadingSpinnerService.show();
  //this.dataservice.getFile(this.config.Emar_ICD_ExportAllICD10ToPdf)
  //.subscribe(res => {
  //this.ng4LoadingSpinnerService.hide();
  //this.alertService.success('Exported successfully');
  // }, error => {
  // this.ng4LoadingSpinnerService.hide();
  //this.alertService.error(error.message);
  // });
  //}
  exportICD10ToPdf() {

    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.config.loggedInUserKey);
    let dateTime=this.dateFormatPipe.dateWithTimeFormatForApi(new Date());
    this.dataservice.getFile(this.config.Emar_ICD_ExportAllICD10ToPdf + userId +"/"+ dateTime)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/pdf' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "ICD10_" + x.getMonth() + "_" + x.getDay() + '.pdf';
        a.download = link.toLocaleLowerCase();
        a.click();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.icdList.forEach(element => {
        element.ICD10_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.ICD10_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.ICD10_Status=1;
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.UpdateStatus = true;
      this.selectedRecords = [];
    }
  }
  onselectRecord(event, item: any) {
    if (event == true) {
      this.UpdateStatus = false;
      item.ICD10_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.ICD10_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.ICD10_Status=1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.ICD10_Id == item.ICD10_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateICDStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
    }
    else {
      this.dataservice.post(this.config.Emar_ICD_UpdateICDsStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status Updated Successfully");
            this.inactivecheckbox == true ? this.GetICD10ByRawFormat(1, 1) : this.GetICD10ByRawFormat(1, 0);
            this.UpdateStatus = true;
            this.inactivecheckbox == true ? this.inactivecheckbox = false : this.inactivecheckbox = true;
            this.CheckAll = false;
            this.selectedRecords = [];

          }
          else if (res == 0) {
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
