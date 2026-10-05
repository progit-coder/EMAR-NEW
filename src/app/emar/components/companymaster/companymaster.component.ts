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
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Router } from '@angular/router';
import { ImageCroppedEvent } from '../../interfaces/image-cropped-event.interface';
import { ImageCropperComponent } from '../../image-cropper/image-cropper/image-cropper.component';
@Component({
  selector: 'app-companymaster',
  templateUrl: './companymaster.component.html',
  styleUrls: ['./companymaster.component.css'],
  providers: [DataService, APIConfiguration]
})

export class CompanymasterComponent implements OnInit {
  @ViewChild(ImageCropperComponent) imageCropper: ImageCropperComponent;
   p: number = 1;
  gridPagination = this.config.gridPagination;
  private companyId: number = 0;
  public template;
  imgurl: any = null;
  footerimgurl: any = null;
  private test: string;
  companyLogo: File;
  companyFooterLogo: File;
  myform: FormGroup;
  public countries: Country[];
  public selectedstateItems=[];
  public selectedcityItems=[];
  public companyMaster: any[] = [];
  private companyObj: Company;
  private zipCodes: ZipCode[];
  logoErrorMessage: string = '';
  footerlogoErrorMessage:string='';
  auditTable: any;
  public modalHistoryIsOpen: boolean = false;
  public CompanyLogoModalOpen: boolean = false;
  public CompanyFooterLogoModalOpen: boolean = false;
  searchText: string = "";
  public companyList: any[] = [];
  public inactivecheckbox: boolean = false;
  public selectedItems = [];
  @Output() company: EventEmitter<string> = new EventEmitter<string>();
  radioSelectedString: string;
  radioSelected: 1;
  getLogo = null;
  getfooterLogo=null;
  CheckAll: boolean = false;
  public mobileNumberMask = this.config.mobileNumberMask;
  public companyein = this.config.companyein;
  public zipcodemask:any;  
  dropdownSettings_UID: any = {};
  dropdownSettings_States:any={};
  dropdownSettings_City:any={};
  columnsList: any[];
  ShowFilter = true;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  public fingerDescData: any[] = [];
  @ViewChild('inputLogo') myInputLogo: ElementRef;
  @ViewChild('cropHeaderlogofile') myCompanyHeaderLogo: ElementRef;
  @ViewChild('cropFooterlogofile') myCompanyFooterLogo: ElementRef;
  pageConfig: {};
  imageChangedEvent: any = '';
  croppedImage: any = '';
  FooterimageChangedEvent: any = '';
  FootercroppedImage: any = '';
  showCropper = false;
  public zipcodeisvalid=false;
  public zipcodeValue:string="";
  @ViewChild('companyFocus') companyFocus:ElementRef

  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService, private dateFormatPipe: CustomdatePipe, private persistanceService: PersistanceService,
    private sanitizer: DomSanitizer
    ) {
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
        setTimeout(()=>{
          this.companyFocus.nativeElement.focus()
        },400)
        this.getAllCountries();
        this.getCompanyMaster();
        this.getCompanyUID();
        this.myform = new FormGroup({
          companyName: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          address1: new FormControl('', [Validators.required, Validators.maxLength(100)]),
          address2: new FormControl('', [Validators.maxLength(100)]),
          city: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters5)]),
          state: new FormControl('', [Validators.required,Validators.minLength(2),Validators.maxLength(2), Validators.pattern(this.config.alphabets)]),
          country: new FormControl('', Validators.required),
          zipcode: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
          contactNumber: new FormControl('', [Validators.minLength(14)]),
          fax: new FormControl('', [Validators.minLength(14)]),
          email: new FormControl('', Validators.pattern(this.config.eMail)),
          contactPerson: new FormControl('', [Validators.maxLength(50),Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
          contactPersonNo: new FormControl('', [Validators.minLength(14)]),
          companyEIN: new FormControl('', [Validators.maxLength(10)]),
          logo: new FormControl(),
          companyFooterLogo:new FormControl(),
          status: new FormControl('1'),
          approval: new FormControl(0),
          companyuid: new FormControl('', Validators.required),
          //fingerDesc: new FormControl('', Validators.required),
          timeformat: new FormControl(''),
        });
        this.dropdownSettings_UID = {
          singleSelection: true,
          idField: "UIDValue",
          textField: "UIDText",
          itemsShowLimit: 3,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        };
        this.userActivity();
        this.getFingerDescDrop();
        this.zipcodemask=this.config.zipcode;
      }
    }
    else
    this.persistanceService.redirectToHomePage();
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
  onReset() {
    this.companyLogo=null;
  }
  getHistoryById(companyId: number) {
    this.auditTable = {
      "tableName": "Company",
      "recordId": companyId
    }
    this.modalHistoryIsOpen = true;

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
    this.footerlogoErrorMessage="";
    this.showCropper = false;
    this.CompanyFooterLogoModalOpen = false;
    this.CompanyLogoModalOpen = false;
    this.modalHistoryIsOpen = false;
  }
  insertCompany() {
    debugger
    this.ng4LoadingSpinnerService.show();
    this.logoErrorMessage = '';
  //   let checkzip=this.myform.value.zipcode.split("-",2);
  //   if(checkzip[1]=="0"||checkzip[1]=="00"||checkzip[1]=="000"||checkzip[1]=="0000")
  //   {
  //     this.myform.value.zipcode=checkzip[0];
  //   }
  //   if(this.myform.value.zipcode.length>6 && this.myform.value.zipcode.length<10 )
  //   {
  //   if(this.myform.value.zipcode.length!=10){
  //     this.myform.patchValue({
  //       zipcode:''
  //     })   
  //     this.zipcodeisvalid=true;
  //   this.ng4LoadingSpinnerService.hide();
  //   }
  // } 
  //   else
  //   {
  //   this.zipcodeisvalid=false;
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
      ApprovalFlag: (this.myform.value.approval == false ? 0 : 1),
      Company_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Company_CreatedDate: this.dateFormatPipe.transform(new Date()),
      Company_UniqueId: this.myform.value.companyuid[0],
      //Fingersdesc_Id: this.myform.value.fingerDesc,
      //TimeFormat: (this.myform.value.timeformat == true ? 1 : 0),
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
          this.resetScreen();
          this.inactivecheckbox = false;
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
      //state: '',
     // city: '',
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

  }
  showInactiveRecords(value: any) {
    if (value == true) {

      this.companyList = this.companyMaster.filter(c => c.Company_Status == 0);
    }
    if (value == false) {
      this.getCompanyMaster();
    }
  }

  getCompanyMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllCompanyMaster)
      .subscribe(res => {
        this.companyMaster = res;
        this.companyList = this.companyMaster.filter(c => c.Company_Status == 1);
        this.sharedService.changeCompany(this.companyMaster);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  updateCompanyStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Company_UpdateCompaniesStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getCompanyMaster();
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
  onCheckAll(event) {
    if (event == true) {
      this.UpdateStatus = false;
      this.CheckAll = true;
      this.companyList.forEach(element => {
        element.Company_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Company_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
        //element.NurseStation_Status=1;
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
      item.Company_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Company_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Company_Id == item.Company_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
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
  getCompanyDetailsByID(ID: number) {
    this.selectedItems = [];
    this.selectedcityItems=[];
    this.selectedstateItems=[];
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any>(this.config.Emar_CompanyMaster_GetCompanyDetailsByID + ID)
      .subscribe(res => {        
        this.zipcodemask=null;
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    window.scroll(0, 0);
  }
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
    if (res.Company_FooterLogo != null)
    this.getfooterLogo = this.sanitize("data:image/jpeg;base64," + res.Company_FooterLogo);
  else
  this.getfooterLogo = null;
    this.selectedItems.push(res.Company_UniqueId);
    this.selectedItems.push(this.columnsList.filter(e => e.UIDValue = res.Company_UniqueId)[0]);
    this.myform.patchValue({
      companyName: res.Company_Name,
      address1: res.Company_Addr1,
      address2: res.Company_Addr2,
      zipcode: res.Company_Zip,
      country: res.Company_CountryId,
      state:res.Company_State,
      city: res.Company_City,
      email: res.Company_Email,
      contactNumber: res.Company_Phone,
      fax: res.Company_Fax,
      logo: "",
      footerlogo:"",
      contactPerson: res.Com_ContactPerson,
      contactPersonNo: res.Com_ContactPhone,
      companyEIN: res.Company_EIN,
      companyuid: this.selectedItems,
      status: res.Company_Status,
      approval: res.ApprovalFlag,
      //fingerDesc: res.Fingersdesc_Id,
      // timeformat: res.TimeFormat
    });
    this.myform.controls['zipcode'].markAsTouched();
    this.myform.controls['state'].markAsTouched();
    if(res.Company_Zip.length<=5){
      this.zipcodeValue =res.Company_Zip.replace('-','');
      if(res.Company_Zip.length<5)
      {
        this.myform.controls['zipcode'].setErrors({'incorrect': true});
      }
      this.zipcodemask=null;
      this.zipcodemask=this.config.zipcode5digit;
      }
      else if(res.Company_Zip.length==6){
      if(res.Company_Zip.indexOf("-")==-1)
      {
        this.zipcodeValue =res.Company_Zip.match(/.{1,5}/g).join("-");
      }
      this.zipcodemask=null;
      this.zipcodemask=this.config.zipcode;      
      }
      if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length==5 && this.myform.value.zipcode.indexOf("-")!=5)
      {
        this.zipcodeisvalid=true;
      }
      else if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length>5 && this.myform.value.zipcode.length==10 && (this.myform.value.zipcode.substr(6,10).split('0').length-1!=4) && this.myform.value.zipcode.indexOf("-")!=-1 && (this.myform.value.zipcode.split('-').length-1)==1 && this.myform.value.zipcode.indexOf("-")==5)
      {
        this.zipcodeisvalid=true;
      }
      else 
      {
        this.zipcodeisvalid=false;
        this.myform.controls['zipcode'].setErrors({'incorrect': true});
      }
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
  footerfileUploadChange(event: any): void {
    this.footerimgurl = null;
    this.getfooterLogo = null;
    this.companyFooterLogo = event.target.files[0];
    if (event.target.files && event.target.files[0]) {
      if (event.target.files[0].size > '204800') {
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
        this.footerlogoErrorMessage = '';
        let myReader = new FileReader();
        myReader.readAsDataURL(this.companyFooterLogo);
        myReader.onload = () => {
          this.footerimgurl = myReader.result;          
        }
      }      
    }
  }
  addNewFacility(ID: number) {

  }
  // checkCompanyName(): any {
  //   let Company_Name = this.myform.value.companyName;
  //   let result = this.companyMaster.find(x => x.Company_Name === Company_Name);
  //   if (result) {
  //     this.alertService.error("company Name already exist");
  //     this.myform.patchValue({
  //       companyName: ''
  //     });
  //   }
  //   else {

  //   }
  // }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.CompanyMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getFingerDescDrop() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetFingersDescDrop)
      .subscribe(res => {
        this.fingerDescData = res;
      }, error => {
        this.alertService.error(error.message);
      });
  }
  resetNotMandatoryFields(value: any) {
    if (value == 'Email' && this.myform.value.email == '') {
      this.myform.controls['email'].reset();
    }
    if (value == 'ContactNum' && this.myform.value.contactNumber == '') {
      this.myform.controls['contactNumber'].reset();
    }
    if (value == 'Fax' && this.myform.value.fax == '') {
      this.myform.controls['fax'].reset();
    }
    if (value == 'ContactPerson' && this.myform.value.contactPerson == '') {
      this.myform.controls['contactPerson'].reset();
    }
    if (value == 'ContactPersonNum' && this.myform.value.contactPersonNo == '') {
      this.myform.controls['contactPersonNo'].reset();
    }
    if (value == 'CmpEIN' && this.myform.value.companyEIN == '') {
      this.myform.controls['companyEIN'].reset();
    }
  }
  fileChangeEvent(event: any): void {
    debugger
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
        if(this.myInputLogo!=undefined && this.myInputLogo !=null)
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
  // cropperReady() {
  //   console.log('Cropper ready')
  //   }
  // loadImageFailed () {
  //   console.log('Load failed');
  //   }
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
  zipcodeChange(value:any)
  {
   debugger
    this.zipcodemask=this.config.zipcode;
    if(value.length<=5){
    this.zipcodeValue =value.replace('-','');
    if(value.length<5)
    {
      this.zipcodeisvalid=false;
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
    if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length==5 && this.myform.value.zipcode.indexOf("-")==-1)
    {
      this.zipcodeisvalid=true;
    }
    else if(this.myform.value.zipcode!="" && this.myform.value.zipcode!=undefined && this.myform.value.zipcode!=null && this.myform.value.zipcode.length>5 && this.myform.value.zipcode.length==10 && (this.myform.value.zipcode.split('-').length-1)==1 && this.myform.value.zipcode.indexOf("-")!=-1 && this.myform.value.zipcode.indexOf("-")==5  && (this.myform.value.zipcode.substr(6,10).split('0').length-1!=4) )
    {
      this.zipcodeisvalid=true;
    }
    else 
    {
      this.zipcodeisvalid=false;
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
  }
}



