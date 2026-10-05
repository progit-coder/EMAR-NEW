import { AlertService } from './../../../_services/index';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Company } from '../../../models/company.model';
import { Hlsevensegment, OutboundHLSegmentFieldDisplayConfigs,HLSevenOutboundDisplayCompanyConfigs } from '../../../models/hlsevensegment.model';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from 'src/app/services/shared/customdate.pipe';
@Component({
  selector: 'app-outboundhlsegmentfielddisplayconfig',
  templateUrl: './outboundhlsegmentfielddisplayconfig.component.html',
  styleUrls: ['./outboundhlsegmentfielddisplayconfig.component.css'],
  providers: [DataService, APIConfiguration]
})
export class OutboundhlsegmentfielddisplayconfigComponent implements OnInit {
  public template;
  errorMessage: string;
  ShowFilter = true;
  myform: FormGroup;
  private url: string;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  searchText: string = "";
  pageConfig: {};
  public selectedOutboundHlItem = [];
  dropdownSettings_OutboundHlSegmentdisplay: any = {};
  public outboundsettingsFlag:number =0;

  public hlSegmentOutboundDisplayconfigObj: OutboundHLSegmentFieldDisplayConfigs[];
  public HLSevenOutboundDisplayCompanyConfigsObj:HLSevenOutboundDisplayCompanyConfigs;
  public outboundHlsegmentconfigs:any=[];
  public Companies:any=[];
  public selectedCompanyItem = [];
  dropdownSettings_Company: any = {};
  public displaysegments:any[]=[];
  public OSegDetailId:number;
  public OSDisplayId:number;
  constructor(private dataservice: DataService,
    private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("ADTFieldsDisplayConfiguration");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.myform = new FormGroup({
      ddlsegment: new FormControl('', Validators.required),
      ddlcompany: new FormControl('', Validators.required),
    });
    this.userActivity();
    this.GetHLSevenSegmentData();
    this.GetHLSevenEnabledCompanyData();
    this.dropdownSettings_OutboundHlSegmentdisplay = {
      singleSelection: true,
      idField: "OSegment_Id",
      textField: "OSegment_Desc",
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
      allowSearchFilter: this.ShowFilter
    };
       } 
     }
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.ADTFieldsDisplayConfig, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  GetHLSevenSegmentData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_HLSevenOutboundDisplaySegment_GetHLSevenOutboundDisplaySegmentsData)
      .subscribe(res => {
        this.outboundHlsegmentconfigs = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  GetHLSevenEnabledCompanyData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_HlsevenEnabledCompanyDetails_GetHlsevenEnabledCompanyDetails)
      .subscribe(res => {
        this.Companies = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  onSelectionChange(id,displayId,OHId,HlSeq) {
    //Need to do later for Outbound file generation for this Segment
   // if(this.outboundsettingsFlag ==1)
   // {
     let transferFacility=HlSeq==3 && id==63?this.displaysegments.find(d=>d.OSegDetail_Id==66).DisplayConfigId:null
     if((HlSeq == 33 || HlSeq == 34) && displayId == 1)
     {
     this.alertService.warn("This field cannot be Editable");
     this.GetOutboundDisplaySegmentConfigs();
     this.ng4LoadingSpinnerService.hide();
     }
     else if(HlSeq ==3 && id==63 && transferFacility!=null && transferFacility==1 && (displayId==0||displayId==2))
     {
      this.alertService.warn("Facility display status is Editable.So Nursing Station display status cannot be Restricted Or Non-Editable");
      this.GetOutboundDisplaySegmentConfigs();
      this.ng4LoadingSpinnerService.hide();
     }
     else if((HlSeq==5 && (id==32 || id==33)) ||(HlSeq==7 && id==36) || (HlSeq==8 && id==37) ||(HlSeq==3 && (id==53 || id==56)) || (HlSeq==7 && (id==58 || id==59 ||id==60))||(HlSeq==44 && id==62))
     {
      this.alertService.warn("This field display status cannot be Restricted Or Non-Editable");
      this.GetOutboundDisplaySegmentConfigs();
      this.ng4LoadingSpinnerService.hide();
     }
     else 
     {
    this.ng4LoadingSpinnerService.show();
    this.OSDisplayId = displayId;
    this.OSegDetailId =id;
    this.HLSevenOutboundDisplayCompanyConfigsObj = {
      OHLConfig_Id:OHId,
      Company_Id:this.myform.value.ddlcompany[0].Company_Id,
      OSegDetail_Id:id,
      DisplayConfigId: displayId,
      OHLConfig_Status:1,
      OHLConfig_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      OHLConfig_CreatedDate:this.dateFormatPipe.transform(new Date()),
      OSegment_Id:this.myform.value.ddlsegment[0].OSegment_Id,
      HlSequence:HlSeq
    };

    this.dataservice.post(this.config.Emar_HlSevenOutboundConfigs_InsertUpdateHlsevenOutboundDisplayConfigsData, this.HLSevenOutboundDisplayCompanyConfigsObj)
      .subscribe(res => {
        this.GetOutboundDisplaySegmentConfigs();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
    //}
   // else 
   // {
   //   this.alertService.error("This Company Doesn't Have Outbound file generation for this Segment");
     // this.ng4LoadingSpinnerService.hide();
   // }
  }
  GetOutboundDisplaySegmentConfigs() {
    if (this.myform.value.ddlsegment == '' || this.myform.value.ddlsegment == null || this.myform.value.ddlsegment.length == 0) {
      if (this.myform.value.ddlsegment == '' || this.myform.value.ddlsegment == null)
        this.alertService.error("Please Select Segment");
        this.displaysegments=[];
    }
    else if (this.myform.value.ddlsegment != '' && this.myform.value.ddlcompany.length != 0) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_HlsevenOutboundDisplaysegments_GetHLSevenOutboundDisplaySegmentsGridData +this.myform.value.ddlsegment[0].OSegment_Id +"/"+this.myform.value.ddlcompany[0].Company_Id)
      .subscribe(res => {
        this.displaysegments = res;
     //  this.outboundsettingsFlag = this.displaysegments[0].OutboundHlsevenSettingsResult;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  }
  onPermissionsSelect(item: any) {
    this.GetOutboundDisplaySegmentConfigs();
  }
  onPermissionsDeSelect(item: any) {
    this.GetOutboundDisplaySegmentConfigs();
  }
  resetScreen() {
    this.myform.reset();
    this.myform.patchValue({
      ddlcompany:'',
      ddlsegment:''
    });
    this.displaysegments = [];    
  }
}
