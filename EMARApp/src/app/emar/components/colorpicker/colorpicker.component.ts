import { AlertService } from './../../../_services/index';
import { Component, OnInit, ChangeDetectorRef, HostBinding } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { PatientType } from '../../../models/common.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { ColorEvent } from 'ngx-color';

@Component({
  selector: 'app-colorpicker',
  templateUrl: './colorpicker.component.html',
  styleUrls: ['./colorpicker.component.css'],
  providers: [DataService, APIConfiguration]
})
export class ColorpickerComponent implements OnInit {
  myform: FormGroup;
  public companyMaster: any[];
  public PatientTypeData: any[] = [];
  public searchText: string = "";
  public patienttypeObj: PatientType;
  public colorcode: any;
  public template;
  errorMessage: any;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  public color2: any;
  public patientTypeId: number = 0;
  auditTable: any;
  public colorList: any[] = [];
  public inactivecheckbox: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  public selectedRecords: any[] = [];
  CheckAll: boolean = false;
  public UpdateStatus: boolean = true;
  pageConfig = {};

  dropdownSettings_Company: any = {};
  public selectedComItems = [];

  color: Object;
  hexColor: String;
  @HostBinding('class') headerClass: SafeStyle;
  iscollapsed = true;
  colors: any[] = ['#FFBF00',
    '#9966CC',
    '#FBCEB1',
    '#7FFFD4',
    '#CCCCFF',
    '#007FFF',
    '#89CFF0',
    '#f80c0c',
    '#006b76',
    '#50C878',
    '#4B0082',
    '#FF007F',
    '#FF6600',
    '#DA70D6',
    '#FA8072',
    '#964B00',
  ];
  constructor(private dataservice: DataService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, public sharedService: SharedService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ColorPicker");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.getCompanyMaster();
        this.getPatientTypeData();
        this.myform = new FormGroup({
          ddlcompany: new FormControl('', Validators.required),
          colorcode: new FormControl(''),
          colordescription: new FormControl('', [Validators.required,Validators.maxLength(75)]),
          Status: new FormControl('1'),
        });
        this.userActivity();
        this.color2 = '';

        this.dropdownSettings_Company = {
          singleSelection: true,
          idField: "Company_Id",
          textField: "Company_Name",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };

      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserCompanyMaster)
      .subscribe(res => {
        this.companyMaster = res;
        this.ng4LoadingSpinnerService.hide();
        if (this.companyMaster.length == 1) {
          this.myform.patchValue({
            ddlcompany: this.companyMaster
          })
        }
      },
        error => {
          this.alertService.error(error.message);
        });
  }
  handleBlockChange($event: any) {
    this.color = $event.color;
    this.color2 = $event.color.hex;
    this.colorcode = this.color2;
    this.headerClass = this.sanitizer.bypassSecurityTrustStyle('background-color:' + this.hexColor + ';');
    let colorcodevalue = this.colorcode;
    if (this.myform.value.ddlcompany == undefined || this.myform.value.ddlcompany == null || this.myform.value.ddlcompany.length == 0) {
      this.alertService.warn("Please select Company");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      let companyId = this.myform.value.ddlcompany[0].Company_Id;
      let result = this.patientTypeId == 0 ? this.PatientTypeData.find(x => x.Company_Id == companyId && x.Color_Code.replace(/\s/g, '').toLowerCase() == colorcodevalue.toLowerCase().replace(/\s/g, '')) : this.PatientTypeData.find(x => x.Company_Id == companyId && x.PatientType_Id != this.patientTypeId && x.Color_Code.replace(/\s/g, '').toLowerCase() == colorcodevalue.toLowerCase().replace(/\s/g, ''));
      if (result) {
        let recordStatus = result.PatientType_Status == 0 ? 'Inactive' : 'Active';
        this.alertService.error("Same color code already exists for an " + recordStatus + " record of same company");
        this.color2 = '';
        this.ng4LoadingSpinnerService.hide();
      }
    }
  }
  insertColor() {
    this.ng4LoadingSpinnerService.show();
    if (this.color2 == '' || this.color2 == undefined) {
      this.alertService.warn("Please select color code");
      this.ng4LoadingSpinnerService.hide();
    }
    else if (this.myform.value.ddlcompany == undefined || this.myform.value.ddlcompany == null || this.myform.value.ddlcompany.length == 0) {
      this.alertService.warn("Please select Company");
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      let result = this.patientTypeId == 0 ? this.PatientTypeData.find(x => x.Company_Id == this.myform.value.ddlcompany[0].Company_Id && x.Color_Code.replace(/\s/g, '').toLowerCase() == this.colorcode.toLowerCase().replace(/\s/g, '')) : this.PatientTypeData.find(x => x.Company_Id == this.myform.value.ddlcompany[0].Company_Id && x.PatientType_Id != this.patientTypeId && x.Color_Code.replace(/\s/g, '').toLowerCase() == this.colorcode.toLowerCase().replace(/\s/g, ''));
      if (result) {
        this.alertService.error("Color Code already exists");
        this.color2 = '';
        this.ng4LoadingSpinnerService.hide();
      }
      else {
        this.patienttypeObj = {
          PatientType_Id: this.patientTypeId,
          Company_Id: this.myform.value.ddlcompany[0].Company_Id,
          Color_Code: this.colorcode,
          Color_Description: this.myform.value.colordescription,
          PatientType_Status: (this.myform.value.Status == true ? 1 : 0),
          PatientType_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
          PatientType_CreatedDate: this.dateFormatPipe.transform(new Date())
        }
        this.dataservice.post(this.config.Emar_ColorPicker_InsertUpdatePatientType, this.patienttypeObj)
          .subscribe(res => {
            this.getPatientTypeData();
            this.inactivecheckbox = false;
            this.ng4LoadingSpinnerService.hide();
            this.resetScreen();
            this.color2 = '';
            this.alertService.success("Save successful");
          },
            error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
      }
    }
  }
  saveColor(color: string) {
    this.colorcode = color;
  }
  getPatientTypeData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_ColorPicker_GetPatientTypeData)
      .subscribe(res => {
        this.PatientTypeData = res;
        this.colorList = this.PatientTypeData.filter(c => c.PatientType_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.colorList = this.PatientTypeData.filter(c => c.PatientType_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getPatientTypeData();
    }
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      ddlcompany: '',
      Status: '1',
    });
    this.ng4LoadingSpinnerService.hide();
    this.color2 = '';
    this.patientTypeId = 0;
  }
  getpatientTypeByID(PatientTypeId: number) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_ColorPicker_GetPatientTypeByID + PatientTypeId)
      .subscribe(res => {
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    window.scroll(0, 0);
  }
  fetchData(res: PatientType) {
    this.selectedComItems = [];
    this.selectedComItems.push(this.companyMaster.filter(c => c.Company_Id == res.Company_Id)[0])
    this.color2 = res.Color_Code;
    this.colorcode = res.Color_Code;
    this.myform.patchValue({
      ddlcompany: this.selectedComItems,
      colordescription: res.Color_Description,
      Status: res.PatientType_Status,
    });
    this.patientTypeId = res.PatientType_Id;
  }
  getHistoryById(PatientTypeId: number) {
    this.auditTable = {
      "tableName": "ResidentColorConfig",
      "recordId": PatientTypeId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ColorPicker, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  checkColorDesc(): any {
    let colorDesc = this.myform.value.colordescription;
    let companyId = this.myform.value.ddlcompany[0].Company_Id;
    let result = this.patientTypeId == 0 ? this.PatientTypeData.find(x => x.Company_Id == companyId && x.Color_Description.replace(/\s/g, '').toLowerCase() === colorDesc.toLowerCase().replace(/\s/g, '')) : this.PatientTypeData.find(x => x.Company_Id == companyId && x.PatientType_Id != this.patientTypeId && x.Color_Description.replace(/\s/g, '').toLowerCase() === colorDesc.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.alertService.error("Color Description already exists for this Company");
      this.myform.patchValue({
        colordescription: ''
      });
    }
  }
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.colorList.forEach(element => {
        element.CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.PatientType_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.PatientType_Status =1;
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
      item.CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.PatientType_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      //item.PatientType_Status =1;
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.PatientType_Id == item.PatientType_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  updateColorPickerStatus() {

    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.ng4LoadingSpinnerService.hide();
      this.alertService.warn("Please select data to update status.");
      this.UpdateStatus = true;
    }
    else {
      this.dataservice.post(this.config.Emar_Common_UpdatePatientTypeStatus, this.selectedRecords)
        .subscribe(res => {

          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Status updated successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getPatientTypeData();
            this.selectedRecords = [];

          }
          else if (res == 0) {
            this.alertService.error("Status updated failed.");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
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
