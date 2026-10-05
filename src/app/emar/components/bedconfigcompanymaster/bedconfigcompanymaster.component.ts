import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { State, ZipCode, Country, City } from '../../../models/common.model';
import { Company } from '../../../models/company.model';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { TextMaskModule } from 'angular2-text-mask';
//import { EventEmitter } from 'events';

import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ImageCroppedEvent } from '../../interfaces/image-cropped-event.interface';
import { ImageCropperComponent } from '../../image-cropper/image-cropper/image-cropper.component';
@Component({
  selector: 'app-bedconfigcompanymaster',
  templateUrl: './bedconfigcompanymaster.component.html',
  styleUrls: ['./bedconfigcompanymaster.component.css']
})
export class BedconfigcompanymasterComponent implements OnInit {
  @ViewChild('closeAddCompanyModal') closeCompanyModal: ElementRef;
  p: number = 1;
  gridPagination = this.config.gridPagination;
  private companyId: number = 0;
  public template;
  imgurl: any = null;
  footerimgurl: any = null;
  private test: string;
  companyLogo: File;
  myform: FormGroup;
  public countries: Country[];
  public states: State[];
  public cities: City[];
  public companyMaster: any[] = [];
  private companyObj: Company;
  private zipCodes: ZipCode[];
  public selectedstateItems = [];
  public selectedcityItems = [];
  dropdownSettings_States: any = {};
  dropdownSettings_City: any = {};
  logoErrorMessage: string = '';
  footerlogoErrorMessage:string='';
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public CompanyLogoModalOpen: boolean = false;
  public CompanyFooterLogoModalOpen: boolean = false;
  public selectedItems = [];
  @Output() company: EventEmitter<string> = new EventEmitter<string>();
  radioSelectedString: string;
  radioSelected: 1;
  getLogo = null;
  getfooterLogo=null;
  public mobileNumberMask = this.config.mobileNumberMask;
  public companyein = this.config.companyein;
  public zipcode = this.config.zipcode;
  dropdownSettings_UID: any = {};
  columnsList: any[];
  ShowFilter = true;
  sessionId: number;
  @ViewChild('inputLogo') myInputLogo: ElementRef;
  @Output()
  NewCompany = new EventEmitter();
  public fingerDescData: any[] = [];
  pageConfig: {};
  public zipcodemask:any;  
  public zipcodeisvalid=false;
  public zipcodeValue:string="";
  @ViewChild(ImageCropperComponent) imageCropper: ImageCropperComponent;
  @ViewChild('cropHeaderlogofile') myCompanyHeaderLogo: ElementRef;
  @ViewChild('cropFooterlogofile') myCompanyFooterLogo: ElementRef;
  imageChangedEvent: any = '';
  croppedImage: any = '';
  FooterimageChangedEvent: any = '';
  FootercroppedImage: any = '';
  showCropper = false;
  companyFooterLogo: File;
  @ViewChild('inputFocus', {read:ElementRef}) inputFocus: ElementRef<HTMLInputElement>;

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private persistanceService: PersistanceService, private sanitizer: DomSanitizer) {
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("CompanyMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    //this.sessionId = this.persistanceService.get('sessionId');
    this.getAllCountries();
    this.getCompanyMaster();
    this.getCompanyUID();
    this.getFingerDescDrop();
    this.zipcodemask=this.config.zipcode;
    this.myform = new FormGroup({
      companyName: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      address1: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      address2: new FormControl('', Validators.maxLength(100)),
      city: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters5)]),
      state: new FormControl('', [Validators.required,Validators.minLength(2),Validators.maxLength(2), Validators.pattern(this.config.alphabets)]),
      country: new FormControl('', Validators.required),
      zipcode: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
      contactNumber: new FormControl('', [Validators.minLength(14)]),
      fax: new FormControl('', [Validators.minLength(14)]),
      email: new FormControl('', Validators.pattern(this.config.eMail)),
      contactPerson: new FormControl('', [Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
      contactPersonNo: new FormControl('', Validators.minLength(14)),
      companyEIN: new FormControl('', Validators.maxLength(10)),
      logo: new FormControl(),
      status: new FormControl('1'),
      approval: new FormControl(0),
      companyuid: new FormControl('', Validators.required),
      fingerDesc: new FormControl(''),
      timeformat: new FormControl('')
    });
    this.dropdownSettings_UID = {
      singleSelection: true,
      idField: "UIDValue",
      textField: "UIDText",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  getCompanyUID() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetCompanyUIDDrop)
      .subscribe(res => {
        this.columnsList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getHistoryById(companyId: number) {
    this.auditTable = {
      "tableName": "Company",
      "recordId": companyId
    }
    this.modalHistoryIsOpen = true;
  }
  // closeModel() {
  //   this.modalHistoryIsOpen = false;
  // }
  insertCompany() {
    this.ng4LoadingSpinnerService.show();
    this.logoErrorMessage = '';
    this.companyObj = {
      Company_Id: this.companyId,
      Company_Name: this.myform.value.companyName,
      Company_Addr1: this.myform.value.address1,
      Company_Addr2: this.myform.value.address2,
      Company_Zip: this.myform.value.zipcode,
      Company_City: this.myform.value.city,
      Company_State: this.myform.value.state,
      Company_CountryId: this.myform.value.country,
      Company_Phone: this.myform.value.contactNumber,
      Company_Fax: this.myform.value.fax,
      Company_Email: this.myform.value.email,
      Com_ContactPerson: this.myform.value.contactPerson,
      Com_ContactPhone: this.myform.value.contactPersonNo,
      Company_EIN: this.myform.value.companyEIN,
      Company_Logo: null,
      Company_Footerlogo:null,
      Company_Status: (this.myform.value.status == true ? 1 : 0),
      ApprovalFlag: (this.myform.value.status == false ? 0 : 1),
      Company_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Company_CreatedDate: new Date().toISOString(),
      Company_UniqueId: this.myform.value.companyuid[0],
      //Session_Id:this.sessionId,
      //Fingersdesc_Id:this.myform.value.fingerDesc,
      //TimeFormat:(this.myform.value.timeformat == true ? 1 : 0),

    };
    let formData: FormData = new FormData();
    formData.append('Image', this.companyLogo);
    formData.append('FooterImage',this.companyFooterLogo);

    this.dataservice.postFormData(this.config.Emar_CompanyMaster_InsertCompany, this.companyObj, formData)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getCompanyMaster();
          this.NewCompany.emit();
          this.closeCompanyModal.nativeElement.click();
          this.resetScreen();
        }
        else if (res == 2)
          this.alertService.error("CompanyName already exists");
        else
          this.alertService.error("Something went wrong. Please try again");

      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      country: '1',
      state: '',
      city: '',
      status: '1',
    })
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   companyName: '',
    //   address1: '',
    //   address2: '',
    //   zipcode: '',
    //   state: '',
    //   city: '',
    //   country: '',
    //   contactNumber: '',
    //   email: '',
    //   fax: '',
    //   logo: '',
    //   contactPerson: '',
    //   contactPersonNo: '',
    //   companyEIN: '',
    //   status: '1'
    // });

    this.imgurl = null;
    this.footerimgurl=null;
    this.companyId = 0;
    this.companyLogo = null;
    this.companyFooterLogo=null;
    this.getLogo = null;
    this.getfooterLogo=null;
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;
    this.zipcodeisvalid=false;
    this.logoErrorMessage="";
    this.footerlogoErrorMessage="";
    this.NewCompany.emit();
  }

  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllCompanyMaster)
      .subscribe(res => {
        this.companyMaster = res;
        this.sharedService.changeCompany(this.companyMaster);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getAllCountries() {
    this.dataservice.get<Country[]>(this.config.Common_GetAllCountries)
      .subscribe(res => {
        this.countries = res;
        if (res != null) {
          this.myform.patchValue({
            country: res[0].Country_Id
          });
        }
      }, error => {
        this.alertService.error(error.message);
      });
  }
  getZipCodes() {
    this.dataservice.get<ZipCode[]>(this.config.Emar_CompanyMaster_GetZipCodes)
      .subscribe(res => this.zipCodes = res, error => {
        this.alertService.error(error.message);
      });
  }
  // getCompanyDetailsByID(ID: number) {

  //   this.selectedItems=[];
  //   this.ng4LoadingSpinnerService.show();
  //   this.dataservice.get<Company>(this.config.Emar_CompanyMaster_GetCompanyDetailsByID + ID)
  //     .subscribe(res => {
  //       this.fetchData(res);
  //       this.ng4LoadingSpinnerService.hide();
  //     }, error => {
  //       this.alertService.error(error.message);
  //       this.ng4LoadingSpinnerService.hide();
  //     });
  //   window.scroll(0, 0);
  // }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }
  fetchData(res: any) {

    this.companyLogo = null;
    this.companyId = res.Company_Id;
    // this.getCitiesByStateId(res.Company_StateId);
    //this.getLogo = res.Company_Logo;
    //let blob = new Blob([res.Company_Logo], { type: 'image/jpeg' });
    //Create a url to the blob 
    //this.getLogo = window.URL.createObjectURL(blob);
    if (res.Company_Logo != "")
      this.getLogo = this.sanitize("data:image/jpeg;base64," + res.Company_Logo);
    else
      this.getLogo = null;
    this.selectedItems.push(res.Company_UniqueId);
    this.selectedItems.push(this.columnsList.filter(e => e.UIDValue = res.Company_UniqueId)[0]);
    this.myform.patchValue({
      companyName: res.Company_Name,
      address1: res.Company_Addr1,
      address2: res.Company_Addr2,
      zipcode: res.Company_Zip,
      country: res.Company_CountryId,
      state: res.Company_State,
      city: res.Company_City,
      email: res.Company_Email,
      contactNumber: res.Company_Phone,
      fax: res.Company_Fax,
      logo: "",
      contactPerson: res.Com_ContactPerson,
      contactPersonNo: res.Com_ContactPhone,
      companyEIN: res.Company_EIN,
      companyuid: this.selectedItems,
      status: res.Company_Status,
      approval: res.ApprovalFlag,
      //fingerDesc:res.Fingersdesc_Id,
      // timeformat:res.TimeFormat
    });
  }
  getcompanyselected(item: Company) {
    this.fetchData(item);
    this.imgurl = item.Company_Logo;
  }
  fileUploadChange(event: any): void {
    this.imgurl = null;
    this.getLogo = null;
    this.companyLogo = event.target.files[0];
    if (event.target.files && event.target.files[0]) {
      if (event.target.files[0].size > '204800') {
        this.logoErrorMessage = 'File size must be less than 200KB';
        if(this.myInputLogo!=undefined && this.myInputLogo!=null)
        {
        this.myInputLogo.nativeElement.value = '';
        }
        this.myform.value.logo = '';
        this.imgurl = null;
        this.getLogo = null;
        this.companyLogo = null;
      }
      else {
        this.logoErrorMessage = '';
        let myReader = new FileReader();
        myReader.readAsDataURL(this.companyLogo);
        myReader.onload = () => {
          this.imgurl = myReader.result;
        }
      }
    }
  }
  getFingerDescDrop() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetFingersDescDrop)
      .subscribe(res => {
        this.fingerDescData = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  zipcodeChange(value:any)
  {
    this.zipcodemask=this.config.zipcode;
    if(value.length<=5){
    this.zipcodeValue =value.replace('-','');
    if(value.length<5)
    {
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode5digit;
    }
    else if(value.length==6){
    if(value.indexOf("-")==-1)
    {
      this.zipcodeValue =value.match(/.{1,5}/g).join("-");
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;      
    }
    if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length==5 && this.myform.value.zipcode.indexOf("-")!=5)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length>5 && this.myform.value.zipcode.length==10 && (this.myform.value.zipcode.substr(6,10).split('0').length-1!=4) && (this.myform.value.zipcode.split('-').length-1)==1 && this.myform.value.zipcode.indexOf("-")==5)
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
  }
  UploadCompanyLogo() {
    this.logoErrorMessage="";
    this.companyLogo=null;
    this.CompanyLogoModalOpen = true;
  }
  UploadCompanyFooterLogo() {
    this.footerlogoErrorMessage="";
    this.companyFooterLogo=null;
    this.CompanyFooterLogoModalOpen = true;
  }
  closeCompanyLogoModel(){
    if(this.myCompanyHeaderLogo!=undefined && this.myCompanyHeaderLogo!=null)
    {
    this.myCompanyHeaderLogo.nativeElement.value='';
    }
    this.showCropper = false;
    this.imgurl=null;
    this.companyLogo=null;
    this.logoErrorMessage="";
    this.CompanyLogoModalOpen = false;
    if(this.myInputLogo!=undefined && this.myInputLogo!=null)
    {
    this.myInputLogo.nativeElement.value = '';
    }
    this.myform.value.logo = '';
    this.croppedImage=null;
  } 
  closeModel() {
    this.showCropper = false;
    if(this.myCompanyHeaderLogo!=undefined && this.myCompanyHeaderLogo!=null)
    {
    this.myCompanyHeaderLogo.nativeElement.value='';
    }
    this.CompanyLogoModalOpen = false;
    this.modalHistoryIsOpen = false;
    this.CompanyFooterLogoModalOpen = false;
  }
  closeCompanyFooterLogoModel(){
    if(this.myCompanyFooterLogo!=undefined && this.myCompanyFooterLogo!=null)
    {
    this.myCompanyFooterLogo.nativeElement.value='';
    }
    this.showCropper = false;
    //this.imgurl=null;
    this.companyFooterLogo=null;
    this.logoErrorMessage="";
    this.footerlogoErrorMessage="";
    this.CompanyFooterLogoModalOpen = false;
    if(this.myInputLogo!=undefined && this.myInputLogo!=null)
    {
    this.myInputLogo.nativeElement.value = '';
    }
    this.myform.value.logo = '';
    this.FootercroppedImage=null;
    this.footerimgurl=null;
  } 
  closeFooterModel() {
    if(this.myCompanyFooterLogo!=undefined && this.myCompanyFooterLogo!=null)
    {
    this.myCompanyFooterLogo.nativeElement.value='';
    }
    this.showCropper = false;
    this.footerlogoErrorMessage="";
    this.CompanyFooterLogoModalOpen = false;
    this.CompanyLogoModalOpen = false;
    this.modalHistoryIsOpen = false;
  }
  fileChangeEvent(event: any): void {
    this.showCropper=true;
    this.imageChangedEvent = event;
    }
  FooterfileChangeEvent(event: any): void {
    this.showCropper=true;
    this.imageChangedEvent = event;
    }
  imageCropped(event: ImageCroppedEvent) {
      this.croppedImage = event.base64;
      this.imgurl = null;
    this.companyLogo=null;
    this.getLogo = null;
     let contentType = event.base64.split(';')[0];   
        var file=this.getBlob(event.base64);
        var files = new File([file], 'test.png', {type: contentType, lastModified: Date.now()});        
        var fileSize=this.imageChangedEvent.target.files[0].size;
        //if (files.size>204800) {
          if(fileSize>204800){
          this.logoErrorMessage = 'File size must be less than 200KB';
          if(this.myInputLogo!=undefined && this.myInputLogo!=null)
          {
          this.myInputLogo.nativeElement.value = '';
          }
          this.myform.value.logo = '';
          this.imgurl = null;
          this.getLogo = null;
          this.companyLogo = null;
          this.croppedImage=null;
        } 
        else
        {
          this.logoErrorMessage ="";
          this.imgurl = [event.base64];
          this.companyLogo=files;     
        } 
      
    }
    FooterimageCropped(event: ImageCroppedEvent) {
      this.FootercroppedImage=event.base64;
      this.footerimgurl = null;
      this.getfooterLogo = null;
      let contentType = event.base64.split(';')[0];   
      var file=this.getBlob(event.base64);
      var files = new File([file], 'test.png', {type: contentType, lastModified: Date.now()});        
      var fileSize=this.imageChangedEvent.target.files[0].size;
      //if (files.size>204800) {
        if(fileSize>204800){
        this.footerlogoErrorMessage = 'File size must be less than 200KB';
        if(this.myInputLogo!=undefined && this.myInputLogo!=null)
        {
          this.myInputLogo.nativeElement.value = '';
        }
          this.myform.value.footerlogo = '';
          this.footerimgurl = null;
          this.getfooterLogo = null;
          this.companyFooterLogo = null;
        }
        else {
          this.footerlogoErrorMessage ="";
          this.footerimgurl = [event.base64];
          this.companyFooterLogo=files;              
      }
     }
  imageLoaded() {
    this.showCropper = true;
    // console.log('Image loaded')
    }
  rotateLeft() {
    this.imageCropper.rotateLeft();
    }
  rotateRight() {
    this.imageCropper.rotateRight();
  }
  flipHorizontal() {
    this.imageCropper.flipHorizontal();
  }
  flipVertical() {
    this.imageCropper.flipVertical();
  }  
  getBlob (b64Data) {
    let contentType = '';
    let sliceSize = 512;
    b64Data = b64Data.replace(/data\:image\/(jpeg|jpg|png)\;base64\,/gi, '');
    let byteCharacters = atob(b64Data);
    let byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      let slice = byteCharacters.slice(offset, offset + sliceSize);
      let byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
          byteNumbers[i] = slice.charCodeAt(i);
      }
      let byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    let blob = new Blob(byteArrays, {type: contentType});
    return blob;
}
}
