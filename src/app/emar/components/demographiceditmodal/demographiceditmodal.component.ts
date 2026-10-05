import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Gender } from '../../../models/common.model';
import { DemographicInfo, ResidentDemographicMaster } from '../../../models/residentdemographic.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { validateConfig } from '@angular/router/src/config';

@Component({
  selector: 'app-demographiceditmodal',
  templateUrl: './demographiceditmodal.component.html',
  styleUrls: ['./demographiceditmodal.component.css']
})
export class DemographiceditmodalComponent implements OnInit {

  @Output() residetData: EventEmitter<any> = new EventEmitter();
  @Input() selectedResident: any;
  pageConfig = {};
  public demographicInfoData = {} as DemographicInfo;
  public ResidentdemographicData: ResidentDemographicMaster[];
  private demographicsObj: ResidentDemographicMaster;
  displayfield: any = {};
  public genders: Gender[];
  public checkMiddleName: boolean = false;
  public residentId: number;
  public segmentDesc: string = "Demographics";
  public template;
  myform: FormGroup;
  public approval: any;
  public errorMessage: any;
  public MiddileIntialPattern = '^[a-zA-Z0-9-_` \x27]*$';
  public lastUpdate: string = "";
  public lastfacUpdate: string = "";
  public Hlsevenconfig: any[];
  public InternalIdStatusFlag: number = 0;
  public InternalIdStatus: any;
  public CheckedInternalId:number =0;
  public zipcodeisvalid=true;
  public zipcodeValue:string="";
  @ViewChild('inputFocus') inputFocus:ElementRef

  constructor(private dataservice: DataService, public config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService, public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DemographicInformation");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.residentId = this.selectedResident;
        this.myform = new FormGroup({
          ExternalPatientId: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumeric), Validators.maxLength(20), Validators.minLength(5)]),
          ExternalFacShortName: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumericFewSpecialCharacters1), Validators.maxLength(10), Validators.minLength(3)]),
          ExternalFacPatientId: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumeric), Validators.maxLength(20), Validators.minLength(4)]),
          AlternatePatientId: new FormControl('', [Validators.pattern(this.config.alphaNumeric), Validators.maxLength(20), Validators.minLength(8)]),
          PatientLastName: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50), Validators.minLength(2)]),
          PatientFirstName: new FormControl('', [Validators.required, Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50)]),
          PatientMiddleInitial: new FormControl('', [Validators.pattern(this.MiddileIntialPattern), Validators.maxLength(1)]),
          MotherMaidenName: new FormControl('', [Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50)]),
          DOB: new FormControl('', Validators.required),
          AdministrativeSex: new FormControl('', Validators.required),
          PatientAlias: new FormControl('', [Validators.pattern(this.config.alphaNumericFewSpecialCharacters3), Validators.maxLength(50)]),
          Race: new FormControl('', [Validators.pattern(this.config.alphabetsWithSpaces), Validators.maxLength(20), Validators.minLength(3)]),
          PatientAddress1: new FormControl('', [Validators.maxLength(100)]),
          PatientAddress2: new FormControl('', [Validators.maxLength(100)]),
          PatientCity: new FormControl('', [Validators.pattern(this.config.alphaNumericFewSpecialCharacters5), Validators.maxLength(50)]),
          PatientState: new FormControl('', [Validators.minLength(2),Validators.maxLength(2), Validators.pattern(this.config.alphabets)]),
          PatientZipCode: new FormControl('', [Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
          PhoneHome: new FormControl('', [Validators.minLength(13)]),
          PrimaryLanguage: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(20)]),
          MaritalStatus: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(20)]),
          Religion: new FormControl('', [Validators.pattern(this.config.alphabets), Validators.maxLength(20)]),
          PatientMRNumber: new FormControl('', [Validators.pattern(this.config.alphaNumeric), Validators.maxLength(50)]),
          SSN: new FormControl('', [Validators.maxLength(11)]),
          DeathDateTime: new FormControl(''),
          DeathIndicator: new FormControl('', [Validators.required, Validators.pattern(this.config.alphabets), Validators.maxLength(1)]),
          LastUpdate: new FormControl('', [Validators.maxLength(25)]),
          LastFacilityUpdate: new FormControl('', [Validators.maxLength(25)]),
        });
        if (this.residentId != 0 && this.residentId != undefined) {
          this.getDemographicInfoData();
          this.getResidentDemographicData(this.residentId);
          this.getHlsevenconfigData();
        }
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  ngAfterViewInit(){
    setTimeout(()=>{
     this.inputFocus.nativeElement.focus()
    },300)
   }
  getHlsevenconfigData() {
    this.dataservice.get<any[]>(this.config.Emar_HlSevenConfigs_GetOutboundHLSevenConfigs + this.residentId + "/" + 1)
      .subscribe(res => {
        this.Hlsevenconfig = res;
        for (let index = 0; index < this.Hlsevenconfig.length; index++) {
          this.displayfield[this.Hlsevenconfig[index].OSegDetail_Desc] = this.Hlsevenconfig[index].DisplayConfigId;
        }
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  checkResidentInternalId(data: string) {
    if(data !="")
    {
    this.dataservice.get<any>(this.config.Emar_ResidentDemographic_CheckResidentInternalId + this.myform.value.ExternalPatientId)
      .subscribe(res => {
        if (res == 1) {
          this.CheckedInternalId =1;
          this.InternalIdStatus = "This InternalId already exist for another resident."
        
        }
        else {
          this.InternalIdStatus = "";
          this.CheckedInternalId =0;
          if (this.InternalIdStatusFlag == 1) {
            this.myform.patchValue({
              PatientMRNumber: this.myform.value.ExternalPatientId
            });
          }
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    }
  }
  MRNumberChange(data:string)
  {
    if(data !="")
    {
      if (this.InternalIdStatusFlag == 1) {
        this.myform.patchValue({
          ExternalPatientId: this.myform.value.PatientMRNumber
        });
      }
    }
  }
  getResidentDemographicData(patientId: number) {
    this.dataservice.get<ResidentDemographicMaster>(this.config.Emar_Demographic_GetResidentDemographicData + patientId)
      .subscribe(res => {
        this.approval = res.PDOutBoundApproval;
        if (this.approval == 0) {
          this.errorMessage = "This resident record is pending for admin approval."
        }
        this.getGenders();
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  fetchData(res: ResidentDemographicMaster) {
    if (res != null && res != undefined) {
      this.lastUpdate = res.LastUpdate;
      this.lastfacUpdate = res.LastFacilityUpdate;
      if (res.ExternalPatientId == res.PatientMRNumber) {
        this.InternalIdStatusFlag = 1;
      }
      this.myform.patchValue({
        // SetID:res.SetID,
        ExternalPatientId: res.ExternalPatientId,
        ExternalFacShortName: res.ExternalFacShortName,
        ExternalFacPatientId: res.ExternalFacPatientId,
        AlternatePatientId: res.AlternatePatientId,
        PatientLastName: res.PatientLastName,
        PatientFirstName: res.PatientFirstName,
        PatientMiddleInitial: res.PatientMiddleInitial,
        MotherMaidenName: res.MotherMaidenName,
        DOB: res.DOB.substring(0, 10),
        AdministrativeSex: res.AdministrativeSex,
        PatientAlias: res.PatientAlias,
        Race: res.Race,
        PatientAddress1: res.PatientAddress1,
        PatientAddress2: res.PatientAddress2,
        PatientCity: res.PatientCity,
        PatientState: res.PatientState,
        PatientZipCode: res.PatientZipCode,
        PhoneHome: res.PhoneHome,
        PrimaryLanguage: res.PrimaryLanguage,
        MaritalStatus: res.MaritalStatus,
        Religion: res.Religion,
        PatientMRNumber: res.PatientMRNumber,
        SSN: res.SSN,
        DeathDateTime: (res.DeathDateTime == null ? '' : res.DeathDateTime.substring(0, 10)),
        DeathIndicator: res.DeathIndicator,
      });
    if(res.PhoneHome!=undefined&& res.PhoneHome!=null && res.PhoneHome!="")
    {
      this.myform.controls['PhoneHome'].markAsTouched();
      this.myform.get('PhoneHome').markAsPristine();
    }
    if(res.PatientState!=undefined&& res.PatientState!=null && res.PatientState!="")
    {
       this.myform.controls['PatientState'].markAsTouched();
    }
    if( res.PatientZipCode !=null &&  res.PatientZipCode!="")
    {
    this.myform.controls['PatientZipCode'].markAsTouched();
    if(res.PatientZipCode.length<=5){
    this.zipcodeValue =res.PatientZipCode.replace('-','');
    if(res.PatientZipCode.length<5)
    {
      this.myform.controls['PatientZipCode'].setErrors({'incorrect': true});
    }
    }
    else if(res.PatientZipCode.length==6){
    if(res.PatientZipCode.indexOf("-")==-1)
    {
      this.zipcodeValue =res.PatientZipCode.match(/.{1,5}/g).join("-");
    }    
    }
    if(this.myform.value.PatientZipCode!="" && this.myform.value.PatientZipCode!=undefined && this.myform.value.PatientZipCode!=null && this.myform.value.PatientZipCode.length==5 && this.myform.value.PatientZipCode.indexOf("-")!=5)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.PatientZipCode!="" && this.myform.value.PatientZipCode!=undefined && this.myform.value.PatientZipCode!=null && this.myform.value.PatientZipCode.length>5 && this.myform.value.PatientZipCode.length==10 && (this.myform.value.PatientZipCode.substr(6,10).split('0').length-1!=4) && (this.myform.value.PatientZipCode.split('-').length-1)==1 && this.myform.value.PatientZipCode.indexOf("-")==5)
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['PatientZipCode'].setErrors({'incorrect': true});
    }
  }
    }
  }
  insertDemographicDetails() {
    this.demographicsObj =
      {
        Patient_Id: this.residentId,
        ExternalPatientId: this.myform.value.ExternalPatientId,
        ExternalFacShortName: this.myform.value.ExternalFacShortName,
        ExternalFacPatientId: this.myform.value.ExternalFacPatientId,
        AlternatePatientId: this.myform.value.AlternatePatientId,
        PatientLastName:this.myform.value.PatientLastName!=undefined && this.myform.value.PatientLastName!=null? (this.myform.value.PatientLastName).toString().toUpperCase():this.myform.value.PatientLastName,
        PatientFirstName:this.myform.value.PatientFirstName!=undefined && this.myform.value.PatientFirstName!=null? (this.myform.value.PatientFirstName).toString().toUpperCase():this.myform.value.PatientFirstName,
        PatientMiddleInitial:this.myform.value.PatientMiddleInitial!=undefined && this.myform.value.PatientMiddleInitial!=null? (this.myform.value.PatientMiddleInitial).toString().toUpperCase():this.myform.value.PatientMiddleInitial,
        NameTypeCode: 'D',
        MotherMaidenName: this.myform.value.MotherMaidenName,
        DOB: this.myform.value.DOB,
        AdministrativeSex: this.myform.value.AdministrativeSex,
        PatientAlias: this.myform.value.PatientAlias,
        Race: this.myform.value.Race,
        PatientAddress1: this.myform.value.PatientAddress1,
        PatientAddress2: this.myform.value.PatientAddress2,
        PatientCity: this.myform.value.PatientCity,
        PatientState: this.myform.value.PatientState,
        PatientZipCode: this.myform.value.PatientZipCode,
        CountyCode: null,
        PhoneHome: this.myform.value.PhoneHome,
        PhoneBusiness: null,
        PrimaryLanguage: this.myform.value.PrimaryLanguage,
        MaritalStatus: this.myform.value.MaritalStatus,
        Religion: this.myform.value.Religion,
        PatientMRNumber: this.myform.value.PatientMRNumber,
        SSN: this.myform.value.SSN,
        DriverLicense: null,
        MotherIdentifier: null,
        EthnicGroup: null,
        BirthPlace: null,
        MultipleBirthIndicator: null,
        BirthOrder: null,
        Citizenship: null,
        MilitaryStatus: null,
        Nationality: null,
        DeathDateTime: this.myform.value.DeathDateTime,
        DeathIndicator: this.myform.value.DeathIndicator,
        IdentityIndicator: null,
        IdentityReliability: null,
        LastUpdate: this.lastUpdate,
        LastFacilityUpdate: this.lastfacUpdate,
        SpeciesCode: null,
        BreedCode: null,
        Strain: null,
        ProductionClassCode: null,
        TribalCitizenship: null,
        ImageLocation: this.myform.value.ImageLocation,
        Patient_Status: 1,
        Patient_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        Patient_CreatedDate: new Date().toISOString(),
        PDOutBoundFileStatus: 1,
        PDOutBoundApproval: null,
        PDOutBoundApprovalBy: null,
        PDOutBoundApprovalOn: null,
        Alert: null,
        Diet: null,
      }
    this.dataservice.post(this.config.Emar_AdminApproval_InsertResidentDemographicData, this.demographicsObj)
      .subscribe(res => {
        this.getResidentDemographicData(this.residentId);
        this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.demographicInfoData.Patient_Id)
          .subscribe(res => {
            if (res == 1) {
              this.residetData.emit(1);
            }
            else {
              this.demographicInfoData = null;
              this.residetData.emit(0);
              this.getDemographicInfoData();
            }
          }, error => {
            this.alertService.error(error.message)
          });

      }, error => {
        this.alertService.error(error.message);

      });
  }
  getGenders() {
    this.dataservice.get<Gender[]>(this.config.Common_GetGenders)
      .subscribe(res => this.genders = res, error => {
        this.alertService.error(error.message)
      });
  }
  checkPatientMiddleName() {
    let text = this.myform.value.PatientMiddleInitial;
    if (text.length > 1) {
      this.checkMiddleName = true;
    }
    else {
      this.checkMiddleName = false;
    }
  }
  getDemographicInfoData() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.demographicInfoData = res;
        this.getResidentDemographicData(this.residentId);
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  zipcodeChange(value:any)
  {
    if(value.trim() == '')
    {
      const zipcodevalidation = this.myform.get('PatientZipCode');
      zipcodevalidation.setValidators(null);
      zipcodevalidation.clearValidators();
      zipcodevalidation.updateValueAndValidity();
    }
    else
    {
    const zipcodevalidation = this.myform.get('PatientZipCode');
    zipcodevalidation.setValidators([Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]);
    zipcodevalidation.updateValueAndValidity();
    if(value.length<=5){
    this.zipcodeValue =value.replace('-','');
    if(value.length<5 || (value.length>6 && value.length!=10))
    {
      this.myform.controls['PatientZipCode'].setErrors({'incorrect': true});
    }
    }
    else if(value.length==6){
    if(value.indexOf("-")==-1)
    {
      this.zipcodeValue =value.match(/.{1,5}/g).join("-");
    }     
    }
    if(this.myform.value.PatientZipCode!="" && this.myform.value.PatientZipCode!=undefined && this.myform.value.PatientZipCode!=null && this.myform.value.PatientZipCode.length==5 && this.myform.value.PatientZipCode.indexOf("-")==-1)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.PatientZipCode!="" && this.myform.value.PatientZipCode!=undefined && this.myform.value.PatientZipCode!=null && this.myform.value.PatientZipCode.length>6 && this.myform.value.PatientZipCode.length==10 && (this.myform.value.PatientZipCode.substr(6,10).split('0').length-1!=4) && (this.myform.value.PatientZipCode.split('-').length-1)==1 && this.myform.value.PatientZipCode.indexOf("-")==5)
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['PatientZipCode'].setErrors({'incorrect': true});
    }
  }
  }
}
