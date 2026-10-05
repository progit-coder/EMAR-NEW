import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { State, Country, City } from '../../../models/common.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Facility } from '../../../models/facility.model';
import { Company } from '../../../models/company.model';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { DomSanitizer } from '@angular/platform-browser';
import { SharedService } from '../../../services/shared/shared.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { ImageCroppedEvent } from '../../interfaces/image-cropped-event.interface';
import { ImageCropperComponent } from '../../image-cropper/image-cropper/image-cropper.component';
@Component({
  selector: 'app-facilitymaster',
  templateUrl: './facilitymaster.component.html',
  styleUrls: ['./facilitymaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class FacilitymasterComponent implements OnInit {
  @ViewChild('cropHeaderlogofile') myFacilityHeaderLogo: ElementRef;
  @ViewChild(ImageCropperComponent) imageCropper: ImageCropperComponent;
  private facilityId: number = 0;
  public template;
  imgurl: any;
  getLogo = null;
  facilityLogo: File;
  myform: FormGroup;
  public countries: Country[];
  public states: State[];
  public cities: any[];
  public timeZones:any[];
  public inactivecheckbox: boolean = false;
  public facilityList: any[] = [];
  public companies: Company[];
  public facilities: any[] = [];
  public selectedCompanyItem = [];
  dropdownSettings_States: any = {};
  dropdownSettings_City: any = {};
  public selectedstateItems = [];
  public selectedcityItems = [];
  private facilityObj: Facility;
  ShowFilter = true;
  logoErrorMessage: string = '';
  dropdownSettings_Company: any = {};
  auditTable: any;
  public mobileNumberMask = this.config.mobileNumberMask;
  searchText: string = "";
  p: number = 1;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  CheckAll: boolean = false;
  gridPagination = this.config.gridPagination;
  public zipcode=this.config.zipcode; 
  public modalHistoryIsOpen: boolean = false;
  @ViewChild('inputLogo') myInputLogo: ElementRef;
  pageConfig: {};
  public companyHlFlag:number=0;
  public FacilityLogoModalOpen: boolean = false;
  imageChangedEvent: any = '';
  croppedImage: any = '';
  showCropper = false;
  public zipcodemask:any;  
  public zipcodeisvalid=false;
  public zipcodeValue:string="";

  constructor(private dataservice: DataService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef,
    public sharedService: SharedService, private alertService: AlertService, private persistanceService: PersistanceService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("FacilityMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.getCompanies();
        this.getAllCountries();
        this.getFacilityMaster();
        this.getTimeZones();
        this.myform = new FormGroup({
          company: new FormControl('', Validators.required),
          facilityName: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          address1: new FormControl('', [Validators.required, Validators.maxLength(100)]),
          address2: new FormControl('', Validators.maxLength(100)),
          city: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters5)]),
          state: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(2), Validators.pattern(this.config.alphabets)]),
          country: new FormControl('', Validators.required),
          zipcode: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(10),Validators.pattern(this.config.zipcodePattern)]),
          contactNumber: new FormControl('', [Validators.minLength(14)]),
          fax: new FormControl('', [Validators.minLength(14)]),
          shortname: new FormControl('', [Validators.maxLength(20),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          logo: new FormControl(),
          contactPerson: new FormControl('', [Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters3)]),
          contactPersonNo: new FormControl('', [Validators.minLength(14)]),
          status: new FormControl('1', Validators.required),
          timezone:new FormControl('', Validators.required)
        });
        this.userActivity();
        this.zipcodemask=this.config.zipcode;
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
    else
      this.persistanceService.redirectToHomePage();
  }
  UploadFacilityLogo() {
    this.facilityLogo=null;
    this.FacilityLogoModalOpen = true;
  }
  closeFacilityLogoModel(){
    if(this.myFacilityHeaderLogo!=undefined && this.myFacilityHeaderLogo!=null)
    {
    this.myFacilityHeaderLogo.nativeElement.value='';
    }
    this.showCropper=false;
    this.imgurl=null;
    this.facilityLogo=null;
    this.logoErrorMessage="";
    this.FacilityLogoModalOpen = false;
  } 
  onReset() {
    this.facilityLogo=null;
  }
  onSelect($event: any) {
    debugger    
    this.imgurl = null;
    this.facilityLogo=null;
    this.getLogo = null;
     let contentType = $event.split(';')[0];   
        var file=this.getBlob($event);
        var files = new File([file], 'test.png', {type: contentType, lastModified: Date.now()});   
        if (files.size>201452) {
          this.logoErrorMessage = 'File size must be less than 200KB';
          if(this.myInputLogo!=undefined && this.myInputLogo!=null)
          {
          this.myInputLogo.nativeElement.value = '';
          }
          this.myform.value.logo = '';
          this.imgurl = null;
          this.getLogo = null;
          this.facilityLogo = null;
        } 
        else
        {
          this.imgurl = [$event];
          this.facilityLogo=files;     
        }    
  }
  getBlob (b64Data) {
    debugger
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
  getHistoryById(facilityId: number) {
    this.auditTable = {
      "tableName": "Facility",
      "recordId": facilityId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.showCropper=false;
    if(this.myFacilityHeaderLogo!=undefined && this.myFacilityHeaderLogo!=null)
    {
    this.myFacilityHeaderLogo.nativeElement.value='';
    }
    this.logoErrorMessage="";
    this.FacilityLogoModalOpen = false;
    this.modalHistoryIsOpen = false;
  }
  insertFacility() {
    this.ng4LoadingSpinnerService.show();
    if(this.companyHlFlag==1 && (this.myform.value.shortname=='' ||this.myform.value.shortname==undefined ||this.myform.value.shortname==null))
    {
      this.alertService.warn("Facility External ID is required.")
      this.ng4LoadingSpinnerService.hide();
    }
    else{
    this.logoErrorMessage = '';
    this.facilityObj = {
      Facility_Id: this.facilityId,
      Company_Id: this.myform.value.company[0].Company_Id,
      Facility_Name: this.myform.value.facilityName,
      Facility_Addr1: this.myform.value.address1,
      Facility_Addr2: this.myform.value.address2,
      Facility_City: this.myform.value.city,
      Facility_Zip: this.myform.value.zipcode,
      Facility_State: this.myform.value.state,
      Facility_CountryId: this.myform.value.country,
      Facility_Phone: this.myform.value.contactNumber,
      Facility_Fax: this.myform.value.fax,
      Facility_ContactName: this.myform.value.contactPerson,
      Facility_ContactPhone: this.myform.value.contactPersonNo,
      Facility_ShortName: this.companyHlFlag==0 && (this.myform.value.shortname=="" ||this.myform.value.shortname==undefined)?null:this.myform.value.shortname,
      Facility_Logo: null,
      Facility_Status: (this.myform.value.status == true ? 1 : 0),
      Facility_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Facility_CreatedDate: new Date().toISOString(),
      TZ_Id : this.myform.value.timezone
    };
    let formData: FormData = new FormData();
    formData.append('Image', this.facilityLogo);

    this.dataservice.postFormData(this.config.Emar_FacilityMaster_InsertFacility, this.facilityObj, formData)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res == 1) {
          this.alertService.success("Save successful");
          this.getFacilityMaster();
          this.resetScreen();
          this.inactivecheckbox = false;
        }
        else if (res == 2)
          this.alertService.error("FacilityName already exists");
        else if (res == 3)
          this.alertService.error("Facility External ID already exists");
        else
          this.alertService.error("Something went wrong. Please try again");
      },
        error => {
          this.alertService.error(error.message)
          this.ng4LoadingSpinnerService.hide();
        });
      }
  }
  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }

  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      company: '',
      country: '1',
      state: '',
      city: '',
      status: '1',
      timezone: ''
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   company: '',
    //   facilityName: '',
    //   address1: '',
    //   address2: '',
    //   zipcode: '',
    //   state: '',
    //   city: '',
    //   country: '',
    //   contactNumber: '',
    //   shortname: '',
    //   fax: '',
    //   logo: '',
    //   contactPerson: '',
    //   contactPersonNo: '',
    //   status: '1'

    // });
    this.imgurl = null;
    this.facilityId = 0;
    this.facilityLogo = null;
    this.getLogo = null;
    this.companyHlFlag=0;
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode;
    this.zipcodeisvalid=false;
    this.logoErrorMessage="";
  }
  getCompanies() {
    this.dataservice.get<Company[]>(this.config.Emar_CompanyMaster_GetAllActiveCompanyDrop)
      .subscribe(res => {
        this.companies = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
      }
      );
  }
  getTimeZones() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetTimeZones)
      .subscribe(res => {
        this.timeZones = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
      }
      );
  }

  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.facilityList = this.facilities.filter(f => f.Facility_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getFacilityMaster();
    }
  }
  getFacilityMaster() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_FacilityMaster_GetAllFacilityMaster)
      .subscribe(res => {
        this.facilities = res;
        this.facilityList = this.facilities.filter(f => f.Facility_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });

  }
  updateFacilityStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateFacilityStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getFacilityMaster();
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
      this.facilityList.forEach(element => {
        element.Facility_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Facility_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
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
      item.Facility_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Facility_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Facility_Id == item.Facility_Id);
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
  getFacilityDetailsByID(ID: number, CompanyStatus: number) {
    this.ng4LoadingSpinnerService.show();
    this.selectedCompanyItem = [];
    if (CompanyStatus == 1) {
      this.dataservice.get<any>(this.config.Emar_FacilityMaster_GetFacilityDetailsByID + ID)
        .subscribe(res => {
          this.companyHlFlag=res.CompanyHlsevenFlag;
          this.zipcodemask=null;
          this.fetchData(res);
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
    }
    else {
      this.alertService.error("Selected Facility Company is InActive you can't update");
      this.ng4LoadingSpinnerService.hide();
    }
    window.scroll(0, 0);
  }

  fetchData(res:any ) {   
    debugger
    this.selectedCompanyItem = [];
    this.facilityLogo = null;
    if (res.Facility_Logo != "")
      this.getLogo = this.sanitize("data:image/jpeg;base64," + res.Facility_Logo);
    else
      this.getLogo = null;
    this.selectedCompanyItem.push(this.companies.filter(c => c.Company_Id == res.Company_Id)[0]);
    this.facilityId = res.Facility_Id;
    this.myform.patchValue({
      company: this.selectedCompanyItem,
      facilityName: res.Facility_Name,
      address1: res.Facility_Addr1,
      address2: res.Facility_Addr2,
      zipcode: res.Facility_Zip,
      country: res.Facility_CountryId,
      state:res.Facility_State,
      city:res.Facility_City,
      contactNumber: res.Facility_Phone,
      fax: res.Facility_Fax,
      shortname: res.Facility_ShortName,
      contactPerson: res.Facility_ContactName,
      contactPersonNo: res.Facility_ContactPhone,
      logo: '',
      status: res.Facility_Status,
      timezone: res.TZ_Id
    });
    this.myform.controls['zipcode'].markAsTouched();
    this.myform.controls['state'].markAsTouched();
    this.zipcodemask=this.config.zipcode;
    if(res.Facility_Zip.length<=5){
    this.zipcodeValue =res.Facility_Zip.replace('-','');
    if(res.Facility_Zip.length<5)
    {
      this.myform.controls['zipcode'].setErrors({'incorrect': true});
    }
    this.zipcodemask=null;
    this.zipcodemask=this.config.zipcode5digit;
    }
    else if(res.Facility_Zip.length==6){
    if(res.Facility_Zip.indexOf("-")==-1)
    {
      this.zipcodeValue =res.Facility_Zip.match(/.{1,5}/g).join("-");
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
  getcompanyselected(item: number) {
    alert('Selected vlalue : ' + item);
  }
  fileUploadChange(event: any): void {
    this.imgurl = null;
    this.getLogo = null;
    this.facilityLogo = event.target.files[0];
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
        this.facilityLogo = null;
      }
      else {
        this.logoErrorMessage = '';
        let myReader = new FileReader();
        myReader.readAsDataURL(this.facilityLogo);
        myReader.onload = () => {
          this.imgurl = myReader.result;
        }
      }
    }
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.FacilityMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  checkFacilityShotName(value: any) {
    if (value != '') {
      let facilityShortName = this.myform.value.shortname;
      let result = this.facilities.find(x => x.Facility_ShortName.replace(/\s/g, '').toLowerCase() === facilityShortName.toLowerCase().replace(/\s/g, ''));
      if (result) {
        this.alertService.warn("This shortName already exist for another Facility");
        this.myform.patchValue({
          shortname: ''
        });
      }
    }
  }
  onCompanySelect(item:any)
  {
    this.getCompanyHlSevenFlag(item.Company_Id);
  }
  onCompnayDeSelect(item:any)
  {
    this.companyHlFlag=0;
  }
  getCompanyHlSevenFlag(companyId:number)
  {
    this.dataservice.get<any>(this.config.Emar_Company_GetCompanyHlSevenFlag + 0 +"/"+ companyId)
        .subscribe(res => {
          this.companyHlFlag=res;
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    }
  imageCropped(event: ImageCroppedEvent) {
    debugger
      this.croppedImage = event.base64;
      this.imgurl = null;
    this.facilityLogo=null;
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
          this.facilityLogo = null;
        } 
        else
        {          
          this.imgurl = [event.base64];
          this.facilityLogo=files;     
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
}
