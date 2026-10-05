import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Company, HLDesc, companyconfig } from '../../../models/company.model';
import { APIConfiguration } from '../../../models/app.constants';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { PhysicianDetails } from '../../../models/orders.model';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';

@Component({
  selector: 'app-companyconfiguration',
  templateUrl: './companyconfiguration.component.html',
  styleUrls: ['./companyconfiguration.component.css']
})
export class CompanyconfigurationComponent implements OnInit {
  public template;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  public arEDesc: any[];
  public arEvent: any[];
  public companies: Company[];
  public direction: HLDesc[];
  public ftecategories: string = "";
  public eventdesc: string = "";
  myform: FormGroup;
  public fingerDescData: any[] = [];
  public gridData: any[] = [];
  public HLFlag: number = 0;
  public selectedItems = [];
  public descItems = [];
  searchText: string = "";
  dropdownSettings_UID: any = {};
  public selectedCompanyItem = [];
  eventdropdownSettings_UID: any = {};
  columnsList: any[];
  descList: any[];
  ShowFilter = true;
  private companyObj: companyconfig;
  dropdownSettings_Company: any = {};
  public configId: number = 0;
  public selectedCatgeriesList: any[] = [];
  public selectedEventsList: any[] = [];
  public directionalId: number = 0;
  public categoryId: number = 0;
  public stockReportFor: any[] = [];
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  pageConfig: {};
  public selectedPhtsicianItem: any[] = [];
  public physiciansdrop: PhysicianDetails[];
  public dropdownSettings_Physician: any = {};
  errorMessage: string;
  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe,public sharedService: SharedService, ) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("CompanyConfiguration");
    //this.GetPhysicianDropData();

    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.myform = new FormGroup({
          company: new FormControl('', Validators.required),
          fingerDesc: new FormControl(''),
          approval: new FormControl(''),
          timeformat: new FormControl('1'),
          hl7configure: new FormControl(''),
          hldirection: new FormControl('0'),
          categoryid: new FormControl(''),
          eventid: new FormControl(''),
          stockreport: new FormControl('', Validators.required),
          drfirst: new FormControl(''),
         // defaultphysician: new FormControl('', Validators.required),
        });

        this.dropdownSettings_Physician = {
          singleSelection: true,
          idField: "Physician_Id",
          textField: "PhysicianFullName",
          text: "Select",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true,
        };

        this.dropdownSettings_Company = {
          singleSelection: true,
          idField: "Company_Id",
          textField: "Company_Name",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        };
        this.dropdownSettings_UID = {
          singleSelection: true,
          idField: "FteCategory_Id",
          textField: "FteCategory_Desc",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        };
        this.eventdropdownSettings_UID = {
          singleSelection: false,
          idField: "EventCat_Id",
          textField: "EventCat_Desc",
          itemsShowLimit: 1,
          allowSearchFilter: this.ShowFilter
        };
        if (this.myform.value.hl7configure == true) {
          this.HLFlag = 1;
        }
        else {
          this.HLFlag = 0;
        }
        this.userActivity();
        this.getCompanies();
        this.getCategory();
        this.getFingerDescDrop();
        this.getHLDirectionDrop();
        this.getEventCategory();
        this.getStockReportDrop();
        this.configGrid();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.CompanyConfiguration, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  GetPhysicianDropData() {
    this.dataservice.get<PhysicianDetails[]>(this.config.Emar_Orders_GetPhysicianDropData)
      .subscribe(res => {
        this.physiciansdrop = res;
      }, error => {
        this.errorMessage = <any>error;
      });
  }


  hlCongifigureDrop() {
    if (this.myform.value.hl7configure == true) {
      this.HLFlag = 1;
      this.myform.patchValue({
        hldirection: '0',
        categoryid: '',
        eventid: ''
      });
    }
    else {
      this.HLFlag = 0;
      this.myform.patchValue({
        hldirection: '0',
        categoryid: '',
        eventid: ''
      });
    }
  }

  getCompanies() {
    this.dataservice.get<Company[]>(this.config.Emar_CompanyMaster_GetAllActiveCompanyDrop)
      .subscribe(res => {
        this.companies = res;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getHistoryById(companyConfigId: number) {
    this.auditTable = {
      "tableName": "CompanyConfig",
      "recordId": companyConfigId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  getCategory() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllFTECategory)
      .subscribe(res => {
        this.columnsList = res;
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getEventCategory(fetchRecord?:any) {
    let HLdirectionalId = this.directionalId;
    let HLcategoryId =fetchRecord==undefined? this.myform.value.categoryid.length == 2 && this.directionalId == 2 ? 0 : this.categoryId:fetchRecord.FteCategory_Id;
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllEventCategroy + HLdirectionalId + "/" + HLcategoryId)
      .subscribe(res => {
        this.descList = res;
        if(fetchRecord!=undefined)
        {
          this.fetchdata(fetchRecord);
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getHLDirectionDrop() {
    this.dataservice.get<HLDesc[]>(this.config.Emar_CompanyMaster_GetHLDirectionList)
      .subscribe(res => {
        this.direction = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getStockReportDrop() {
    this.dataservice.get<HLDesc[]>(this.config.Emar_Company_GetStockReportForDropData)
      .subscribe(res => {
        this.stockReportFor = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      timeformat: '1',
      categoryid: '',
    });
    this.ng4LoadingSpinnerService.hide();
    this.eventdesc = "";
    this.ftecategories = "";
    this.configId = 0;
    this.directionalId = 0;
    this.categoryId = 0;
    this.getEventCategory();
    this.hlCongifigureDrop();
  }
  getFingerDescDrop() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetFingersDescDrop)
      .subscribe(res => {
        this.fingerDescData = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  configGrid() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllCompanyConfigList)
      .subscribe(res => {
        this.gridData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  insertCompany() {
    if (this.HLFlag == 1 && ((this.myform.value.categoryid == '' || this.myform.value.categoryid == null || this.myform.value.categoryid == undefined) || (this.myform.value.eventid == '' || this.myform.value.eventid == null || this.myform.value.eventid == undefined) || (this.myform.value.hldirection == 0 || this.myform.value.hldirection == null || this.myform.value.hldirection == undefined))) {
      this.alertService.warn("Please select all config details.")
    }
    else {
      this.ng4LoadingSpinnerService.show();
      if (this.HLFlag == 1) {
        this.arEvent = this.myform.value.categoryid;
        this.arEvent.forEach(element => {
          this.ftecategories += element.FteCategory_Id + ",";
        });
        this.ftecategories = this.ftecategories.substring(0, this.ftecategories.length - 1);
        this.arEDesc = this.myform.value.eventid;
        this.arEDesc.forEach(element => {
          this.eventdesc += element.EventCat_Id + ",";
        });
        this.eventdesc = this.eventdesc.substring(0, this.eventdesc.length - 1);
      }
      this.companyObj = {
        CompanyConfig_Id: this.configId,
        Company_Id: this.myform.value.company[0].Company_Id,
        ApprovalFlag: (this.myform.value.approval == true ? 1 : 0),
        CompanyConfig_Status: 1,
        CompanyConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        CompanyConfig_CreatedOn: this.dateFormatPipe.transform(new Date()),
        Fingersdesc_Id: this.myform.value.fingerDesc,
        Hl7Configured: (this.myform.value.hl7configure == true ? 1 : 0),
        TimeFormat: (this.myform.value.timeformat == true ? 1 : 0),
        HLDirectionalWay_Id: this.HLFlag == 1 ? this.myform.value.hldirection : null,
        Category: this.HLFlag == 1 ? this.ftecategories : null,
        Events: this.HLFlag == 1 ? this.eventdesc : null,
        StockReport_Id: this.myform.value.stockreport,
        DrFirstRequired: (this.myform.value.drfirst == true ? 1 : 0),
       // Physician_Id: this.myform.value.defaultphysician[0] == undefined ? null : this.myform.value.defaultphysician[0].Physician_Id,
      };
      this.dataservice.post(this.config.Emar_CompanyMaster_InsertComapnyConfigDetails, this.companyObj)
        .subscribe(res => {
          if (res == 1) {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.success("Save successful");
            this.configGrid();
            this.resetScreen();
          }
          else {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error("This company already configured");
          }

        }, error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    }
  }
  getCompanyConfigById(configId: number, Hl7Configured: number, FteCategoryId: number,companyStatus:number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyItem = [];
    this.getCategory();
    this.HLFlag = 0;
    this.descItems = [];
    if(companyStatus==1)
    {
    this.dataservice.get<any>(this.config.Emar_Company_GetCompanyConfigById + configId + "/" + Hl7Configured + "/" + FteCategoryId)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.categoryId = res.FteCategory_Id;
        if (res.Hl7Configured === 1) {
          this.getCatageryDrop(res.HLDirectionalWay_Id);
          this.getEventCategory(res);
        }
        this.fetchdata(res);
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }
      else
      {
        this.alertService.error("Selected Company is Inactive you can't update");
        this.ng4LoadingSpinnerService.hide();
      }
    window.scroll(0, 0);
  }
  fetchdata(fetchRecord: any) {
    this.selectedPhtsicianItem = [];
    this.selectedCompanyItem = [];
    this.configId = fetchRecord.CompanyConfig_Id;
    this.selectedItems = [];
    this.descItems = [];
    if (fetchRecord.Hl7Configured == 1) {
      this.HLFlag = 1;
    }
    if (fetchRecord.Physician_Id != null) {
      this.selectedPhtsicianItem.push(this.physiciansdrop.filter(p => p.Physician_Id == fetchRecord.Physician_Id)[0]);
    }
    this.selectedCompanyItem.push(this.companies.filter(c => c.Company_Id == fetchRecord.Company_Id)[0]);
    this.myform.patchValue({
      company: this.selectedCompanyItem,
      fingerDesc: fetchRecord.Fingersdesc_Id == null ? '' : fetchRecord.Fingersdesc_Id,
      approval: fetchRecord.ApprovalFlag,
      timeformat: fetchRecord.TimeFormat,
      hl7configure: fetchRecord.Hl7Configured,
      hldirection: fetchRecord.HLDirectionalWay_Id,
      stockreport: fetchRecord.StockReport_Id,
      drfirst: fetchRecord.DrFirstRequired,
    // defaultphysician: this.selectedPhtsicianItem,
    });
    this.selectedItems.push(this.columnsList.filter(r => r.FteCategory_Id == fetchRecord.FteCategory_Id)[0]);
    this.selectedEventsList = fetchRecord.Events.split(',');
    if (this.selectedEventsList.length > 0) {
      for (let i = 0; i < this.selectedEventsList.length; i++) {

        this.descItems.push(this.descList.filter(r => r.EventCat_Id == parseInt(this.selectedEventsList[i]))[0]);
      }
      return this.descItems;
    }
  }
  getCatageryDrop(value: number) {
    if (this.myform.value.categoryid != '' || this.myform.value.categoryid != null || this.myform.value.categoryid != undefined) {
      this.directionalId = value;
      this.getEventCategory();
    }
    this.directionalId = value;
    this.getCategory();
    if (value == 1) {
      this.myform.patchValue({
        eventid: '',
      });
      this.dropdownSettings_UID = {
        singleSelection: true,
        idField: "FteCategory_Id",
        textField: "FteCategory_Desc",
        itemsShowLimit: 1,
        closeDropDownOnSelection:true,
        allowSearchFilter: this.ShowFilter
      };
    }
    else if (value == 2) {
      this.categoryId = 0;
      this.myform.patchValue({
        eventid: '',
      });
      this.dropdownSettings_UID = {
        singleSelection: false,
        idField: "FteCategory_Id",
        textField: "FteCategory_Desc",
        itemsShowLimit: 1,
        allowSearchFilter: this.ShowFilter
      };
    }
    if(this.myform.value.categoryid!=null && this.myform.value.categoryid!=undefined && this.myform.value.categoryid.length!=0)
    {
      if(value==1 ||(value==2 && this.myform.value.categoryid.length==1))
      {
      this.categoryId=this.myform.value.categoryid[0].FteCategory_Id;
      }
      this.getEventCategory();
    }
  }
  onCategorySelect(item: any) {
    if (this.myform.value.hldirection == 0 || this.myform.value.hldirection == undefined || this.myform.value.hldirection == null) {
      this.alertService.warn("Please select Hl7DirectionalWay");
    }
    this.categoryId = item.FteCategory_Id;
    this.myform.patchValue
    ({
      eventid: '',
    });
    this.getEventCategory();
  }
  onCategorySelectAll(item: any) {
    if (this.myform.value.hldirection == 0 || this.myform.value.hldirection == undefined || this.myform.value.hldirection == null) {
      this.alertService.warn("Please select Hl7DirectionalWay")
    }
    this.selectedEventsList = [];
    this.categoryId = 0;
    this.getEventCategory();
  }
  onCategoryDeSelect(item: any) {
    if (this.myform.value.hldirection == 2 && (this.myform.value.categoryid.length == 0 || this.myform.value.categoryid == null || this.myform.value.categoryid == undefined)) {
      this.categoryId = 0;
    }
    else {
      if (this.myform.value.categoryid.length ==1)
        this.categoryId = this.myform.value.categoryid[0].FteCategory_Id;
        else
        this.categoryId=0;
    }
    this.myform.patchValue
      ({
        eventid: '',
      });
    this.getEventCategory();
  }
  onCategoryDeSelectAll(item: any) {
    this.categoryId = 0;
    this.myform.patchValue
      ({
        eventid: '',
      })
    this.getEventCategory();
  }
  onEventSelect(item: any) {
    if (this.myform.value.categoryid == '' || this.myform.value.categoryid == null || this.myform.value.categoryid == undefined) {
      this.alertService.warn("Please select category");
      this.myform.patchValue
        ({
          eventid: ''
        })
    }
  }
  onEventSelectAll(item: any) {
    if (this.myform.value.categoryid == '' || this.myform.value.categoryid == null || this.myform.value.categoryid == undefined) {
      this.alertService.warn("Please select category");
      this.myform.patchValue
        ({
          eventid: ''
        })
    }
  }
}
