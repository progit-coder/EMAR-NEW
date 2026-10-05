import { AlertService } from './../../../_services/index';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Company } from '../../../models/company.model';
import { Hlsevensegment, HLSevenClone } from '../../../models/hlsevensegment.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
@Component({
  selector: 'app-hlsegmentfieldsconfig',
  templateUrl: './hlsegmentfieldsconfig.component.html',
  styleUrls: ['./hlsegmentfieldsconfig.component.css'],
  providers: [DataService, APIConfiguration]
})
export class HlsegmentfieldsconfigComponent implements OnInit {

  public template;
  public companyId: number = 0;
  errorMessage: string;
  ShowFilter = true;
  public selectedHlItem = [];
  dropdownSettings_HlSegment: any = {};
  public companyMaster: any[];
  public CloneComp: any[];
  public hlSegmentObj: Hlsevensegment;
  public hlCloneobj: HLSevenClone;
  myform: FormGroup;
  cloneform: FormGroup;
  private url: string;
  public hlSegmentDetails: any[] = [];
  public hlSegment: any[];
  p: number = 1;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  public cloneButton: number = 0;
  public modalHistoryIsOpen: boolean = false;
  
  dropdownSettings_Company: any = {};
  public selectedComItems = [];

  constructor(private dataservice: DataService,
    private persistanceService: PersistanceService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, public sharedService: SharedService) { }

  ngOnInit() {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getCompanyMaster();
    this.GetHLSevenSegmentData();
    this.myform = new FormGroup({
      ddlcompany: new FormControl('', Validators.required),
      ddlsegment: new FormControl('', Validators.required),
      hlsevenstatus: new FormControl(),
      displaystatus: new FormControl()

    });
    this.cloneform = new FormGroup({
      ddlCloneComp: new FormControl('', Validators.required),
    })
    this.dropdownSettings_HlSegment = {
      singleSelection: true,
      idField: "Segment_Id",
      textField: "Segment_Desc",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };

    this.dropdownSettings_Company = {
      singleSelection: true,
      idField: "Company_Id",
      textField: "Company_Name",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };

    this.userActivity();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.HLSegmentFieldsConfig, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {
        this.companyMaster = res;
        this.ng4LoadingSpinnerService.hide();

        if(this.companyMaster.length==1)
        {
          this.myform.patchValue({
            ddlcompany:this.companyMaster
          })
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getCloneComp() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetUserActiveCompanyDrop)
      .subscribe(res => {

        for (let i = 0; i < res.length; i++) {
          if (res[i].Company_Id == this.myform.value.ddlcompany[0].Company_Id) {
            res.splice(i, 1);
          }
          this.CloneComp = res;
          this.ng4LoadingSpinnerService.hide();
          //res.splice(res[i].Company_Id.indexOf(this.myform.value.ddlcompany),1);
        }
      },
        error => {
          this.errorMessage = <any>error.message;
          this.alertService.error(this.errorMessage);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetHLSevenSegmentData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_RoleMaster_GetHLSevenSegmentData)
      .subscribe(res => {
        this.hlSegment = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  inserthlsevenstatus(HLConfig_Id: number, SegDisplay_Id: number) {
    this.hlSegmentObj = {

      HLConfig_Id: HLConfig_Id,
      Company_Id: this.myform.value.ddlcompany[0].Company_Id,
      SegDetail_Id: this.myform.value.ddlsegment[0].SegDetail_Id,
      SegDetailConfig_Id: (this.myform.value.hlsevenstatus == true ? 1 : 0),
      SegDisplay_Id: SegDisplay_Id,
      HLConfig_Status: 1,
      HLConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      HLConfig_CreatedDate: new Date().toISOString()
    };

    this.dataservice.post(this.config.Emar_HlSevenSegment_InsertHlSegment, this.hlSegmentObj)
      .subscribe(res => {
        this.GetHLSevenSegmentData();
      }, error => {
        this.alertService.error(error.message);
         this.ng4LoadingSpinnerService.hide();
      });
  }
  insertdisplaystatus(HLConfig_Id: number, SegDetailConfig_Id: number) {

    this.hlSegmentObj = {

      HLConfig_Id: HLConfig_Id,
      Company_Id: this.myform.value.ddlcompany[0].Company_Id,
      SegDetail_Id: this.myform.value.ddlsegment[0].SegDetail_Id,
      SegDetailConfig_Id: SegDetailConfig_Id,
      SegDisplay_Id: (this.myform.value.displaystatus == true ? 1 : 0),
      HLConfig_Status: 1,
      HLConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      HLConfig_CreatedDate: new Date().toISOString()
    };

    this.dataservice.post(this.config.Emar_HlSevenSegment_InsertHlSegment, this.hlSegmentObj)
      .subscribe(res => {
        this.GetHLSevenSegmentData()
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
    this.cloneform.patchValue({
      ddlCloneComp:'',
    });
  }
  modalpop() {

    this.modalHistoryIsOpen = true;
    this.getCloneComp();
  }
  InsertHLSevenClone() {
    this.ng4LoadingSpinnerService.show();
    this.hlCloneobj = {
      InputCompanyId: this.myform.value.ddlcompany[0].Company_Id,
      OutputCompanyId: this.cloneform.value.ddlCloneComp,
      SegmentId: this.myform.value.ddlsegment[0].SegDetail_Id,
      CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
    };
    this.dataservice.post(this.config.Emar_HLSevenSegment_CloneHlSevenCompanyConfig, this.hlCloneobj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.cloneform.patchValue({
          ddlCloneComp:'',
        });
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.modalHistoryIsOpen = false;
    // this.cloneform.reset();
  }

  GetHLSevenSegmentDetailData() {
    if (this.myform.value.ddlcompany == '' || this.myform.value.ddlcompany == null || this.myform.value.ddlsegment.length == 0) {
      if (this.myform.value.ddlcompany == '' || this.myform.value.ddlcompany == null)
        this.alertService.error("Please Select Company");
        this.hlSegmentDetails=[];
    }
    else if (this.myform.value.ddlcompany != '' && this.myform.value.ddlsegment.length != 0) {
      this.template = this.dataservice.template;
      this.ng4LoadingSpinnerService.show();
      this.url = this.config.Emar_RoleMaster_GetHLSevenCompanyConfigEntity + "/" + this.myform.value.ddlcompany[0].Company_Id + "/" + this.myform.value.ddlsegment[0].Segment_Id;
      this.dataservice.get<any[]>(this.url)
        .subscribe(res => {
          this.hlSegmentDetails = res;
          //removed clone button
          // if (this.hlSegmentDetails.length > 0)
          //   this.cloneButton = 1;
          this.ng4LoadingSpinnerService.hide();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  resetScreen() {
    this.myform.reset();
    this.myform.patchValue({
      ddlcompany:'',
      ddlsegment:''
    });
    this.cloneButton = 0;
    this.hlSegmentDetails = [];    
  }
  onSegmentSelect(item: any) {
    this.GetHLSevenSegmentDetailData();
  }
  onSegmentDeSelect(item: any) {
    this.GetHLSevenSegmentDetailData();
  }

}
