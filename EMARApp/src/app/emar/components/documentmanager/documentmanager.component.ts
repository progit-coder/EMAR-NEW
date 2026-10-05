import { Component, OnInit, ViewChild, Input, ElementRef, Output, EventEmitter } from '@angular/core';
import { UploadedDocuments, ResidentDemographic, FolderNames } from '../../../models/residentdemographic.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { SharedService } from '../../../services/shared/shared.service';
import {
  TreeviewItem, TreeviewConfig, TreeviewHelper, TreeviewComponent,
  TreeviewEventParser, OrderDownlineTreeviewEventParser, DownlineTreeviewItem, DropdownTreeviewComponent, TreeviewI18n
} from 'ngx-treeview';
import { AlertService } from '../../../_services/index';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Screens, Activity } from '../../../models/useractivity.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NurseStation } from '../../../models/facility.model';
@Component({
  selector: 'app-documentmanager',
  templateUrl: './documentmanager.component.html',
  styleUrls: ['./documentmanager.component.css'],
  providers: [{ provide: TreeviewEventParser, useClass: OrderDownlineTreeviewEventParser },]
})
export class DocumentmanagerComponent implements OnInit {
  checkedList: any;
  residentDocs: UploadedDocuments[] = [];
  uploaddocument: UploadedDocuments;
  loading = false;
  residentID: number = 0;
  docTypeName: string;
  errorMessage: any;
  uploadedFile: File;
  logoErrorMessage: string = '';
  selectedFile: any;
  myform: FormGroup;
  dropdownEnabled = true;
  items: TreeviewItem[] = [];
  values: number[];
  folderIds: string = '';
  residents: ResidentDemographic[];
  location: any;
  filenameextension: any;
  filename: any;
  filetype: any;
  public selectedfaItems = [];
  public nurseStations: NurseStation[];
  public facilities: any[];
  public selectednItems: any[];
  dropdownSettings_NurseStations: any = {};
  dropdownSettings_Facilities: any = {};
  userId: number;
  public loginUserReceFacility: any;
  public loginUserReceNurseStation: any;
  public template;
  public foldersList: FolderNames[];
  public showControls: boolean = false;
  public showTreeView: boolean = false;
  public selectstyle: number = 0;
  folderform: FormGroup;
  public selectedResItem = [];
  public residentId: number;
  searchText: string = "";
  p: number = 1;
  gridPagination = this.configs.gridPagination;
  @ViewChild(DropdownTreeviewComponent) dropdownTreeviewComponent: DropdownTreeviewComponent;
  @Input() value: any;
  ShowFilter = true;
  public selectedItems: number;
  dropdownSettings_Residents: any = {};
  public PatientId: any;
  pageConfig = {};

  config = TreeviewConfig.create({
    hasAllCheckBox: false,
    hasFilter: false,
    hasCollapseExpand: true,
    decoupleChildFromParent: false,
    maxHeight: 400
  });

  constructor(private dataservice: DataService, private APIconfig: APIConfiguration, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private persistanceService: PersistanceService, private configs: APIConfiguration, private sharedService: SharedService, private alertService: AlertService) {
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DocumentManager");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.userId = this.persistanceService.get(this.configs.loggedInUserKey);
        this.template = this.dataservice.template;
        this.ng4LoadingSpinnerService.show();
        this.getfoldernames();
        //this.alertService.warn("Please select resident.");
        this.myform = new FormGroup({
          ddlfacilities: new FormControl(''),
          ddlnursestations: new FormControl(''),
          ddlresidents: new FormControl('', Validators.required),
          date: new FormControl('', Validators.required),
          description: new FormControl('', Validators.required),
          filedata: new FormControl(),
          selectedFolder: new FormControl(),
        });
        this.folderform = new FormGroup({
          mainfolder: new FormControl('', Validators.required),
          ddlFolders: new FormControl('0', Validators.required),
          childfolder: new FormControl('', Validators.required)

        });
        this.dropdownSettings_Facilities = {
          singleSelection: true,
          idField: "Facility_Id",
          textField: "Facility_Name",
          text: "Facilities",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: this.ShowFilter
        };
        this.dropdownSettings_NurseStations = {
          singleSelection: true,
          idField: "NurseStation_Id",
          textField: "NurseStation_Name",
          text: "Nursing Stations",
          // selectAllText: "Select All",
          //  unSelectAllText: "UnSelect All",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          noDataAvailablePlaceholderText: 'Please Select Facility',
          allowSearchFilter: this.ShowFilter
        };
        this.dropdownSettings_Residents = {
          singleSelection: true,
          idField: "Patient_Id",
          textField: "PatientName",
          text: "Select",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
		      noDataAvailablePlaceholderText: 'Please Select Nursing Station',
          allowSearchFilter: this.ShowFilter
        }
        //   this.getResidentDropData();
        this.userActivity();
        this.getUserRecentFacilityNurseStations();
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.DocumentManager, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  onFacilitySelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.showTreeView = false;
    this.loadResidentDocuments(0, null);
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
    });
    this.getNurseStationByFacilityID(item.Facility_Id);
  }
  onFacilityDeSelect(item: any) {
    this.nurseStations = [];
    this.residents = [];
    this.myform.patchValue({
      ddlnursestations: '',
      ddlresidents: '',
    });
    this.showTreeView = false;
    this.loadResidentDocuments(0, null);
  }
  onNurseStationSelect(item: any) {
    this.getDemographicInfoByNurseStation(item.NurseStation_Id);
  }
  onNurseStationDeSelect(item: any) {
    this.residents = [];
    this.myform.patchValue({
      ddlresidents: '',
    })
    this.showTreeView = false;
    this.loadResidentDocuments(0, null);
  }
  getUserRecentFacilityNurseStations() {
    this.ng4LoadingSpinnerService.show();
    this.sharedService.getUserRecentFacNs(this.userId)
      .subscribe(res => {
        if (res != undefined) {
          this.ng4LoadingSpinnerService.hide();
          this.loginUserReceFacility = res.Facility_Id;
          this.loginUserReceNurseStation = res.NurseStation_Id;
        }
        this.getFiltersData(this.userId);
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getFiltersData(userId: number): any {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.APIconfig.Emar_Facility_GetUserAccessFacilityNurseStaions + userId)
      .subscribe((res: any) => {
        this.facilities = res.Facilities;
        this.ng4LoadingSpinnerService.hide();
        if (this.loginUserReceFacility != null) {
          if (this.facilities.length > 0) {
            let checkFacExist = this.facilities.find(r => r.Facility_Id == parseInt(this.loginUserReceFacility));
            this.selectedfaItems = [];
            if (checkFacExist != undefined) {
              this.selectedfaItems.push(checkFacExist);
              this.getNurseStationByFacilityID(this.loginUserReceFacility);
            }
            this.myform.patchValue({
              ddlfacility: this.selectedfaItems,
            });
          }
        } else if (res.Facilities.length == 1) {
          //  this.getCompanyToBedData(res.Facilities[0].Facility_Id);
          this.getNurseStationByFacilityID(res.Facilities[0].Facility_Id);
          this.myform.patchValue({
            ddlfacilities: this.facilities,
          });
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getNurseStationByFacilityID(facilityId: any) {
    this.ng4LoadingSpinnerService.show();
    ///let backClick = JSON.parse(localStorage.getItem("BackClick"));
    this.dataservice.get<any[]>(this.APIconfig.Emar_Facility_GetNurseStationsByFacilityId + this.userId + "/" + facilityId)
      .subscribe((res: any[]) => {
        this.nurseStations = res;
        if (this.loginUserReceNurseStation != undefined) {
          let userReceNSList = this.loginUserReceNurseStation.split(',');
          if (userReceNSList.length > 0) {
            this.selectednItems = [];
            for (let i = 0; i < userReceNSList.length; i++) {
              let checkNsExist = this.nurseStations.find(r => r.NurseStation_Id === parseInt(userReceNSList[i]));
              if (checkNsExist != undefined) {
                this.selectednItems.push(checkNsExist);
              }
            }
            if(this.selectednItems.length!=0)
            {
            this.myform.patchValue({
              ddlnursestations: this.selectednItems,
            });
            this.getDemographicInfoByNurseStation(this.myform.value.ddlnursestations[0].NurseStation_Id);
            //this.getResidentDropData();
            //this.getFiltersDataBySelection();
            //   this.getCompanyToBedByNurseStation();
            }
          }
        }
        else if (this.facilities.length == 1) {
          this.myform.patchValue({
            ddlnursestations: this.nurseStations,
          });
          this.getDemographicInfoByNurseStation(this.myform.value.ddlnursestations[0].NurseStation_Id);
        }
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getDemographicInfoByNurseStation(stationId: number) {
    this.dataservice.get<ResidentDemographic[]>(this.APIconfig.Emar_ResidentDemographic_GetResidentsListByNSId + stationId)
      .subscribe(res => {
        this.residents = res;
        if (res.length > 0) {
          this.residentId = this.residents[0].Patient_Id;
          this.selectedResItem = this.residents.filter(r => r.Patient_Id === this.residentId);
          this.onResidentSelect(this.selectedResItem[0]);
        }
        else {
          this.selectedResItem = [];
          this.residentId = 0;
        }
        this.myform.patchValue({
          ddlresidents: this.selectedResItem,
        })
      },
        error => {
          this.alertService.error('Error while loading Demographic Information')
          this.ng4LoadingSpinnerService.hide();
        });
  }
  select(item: TreeviewItem) {
    this.showControls = true;
    this.folderIds = '';
    if (item.children != undefined) {
      this.folderIds = item.value + ',';
      item.children.forEach(element => {
        this.folderIds += element.value + ',';
      });
      this.folderIds = this.folderIds.substring(0, this.folderIds.length - 1);
    }
    else {
      this.folderIds = item.value;
    }
    if (this.value !== item.value) {
      this.value = item.value;
      this.myform.patchValue(
        {
          selectedFolder: item.text,
        });
    }
    this.loadResidentDocuments(this.residentID, this.folderIds);
  }

  onFilterChange(value: string) {
  }

  fileUploadChange(event: any): void {
    this.uploadedFile = null;
    if (event.target.files && event.target.files[0]) {
      this.uploadedFile = event.target.files[0];
      this.filename = event.target.files[0].name;
      this.filetype = event.target.files[0].type;
      this.filenameextension = this.filename.substr((this.filename.lastIndexOf('.')));
      if (this.filenameextension.toLowerCase() == ".exe") {
        this.alertService.warn("Unable to upload .exe files");
        this.myform.patchValue({
          filedata: '',
        });
      }
    }
    console.log(this.filetype);
  }
  getTreeviewData() {
    this.items = [];
    this.dataservice.get<any[]>(this.configs.Emar_DocumentManager_GetDocFolders)
      .subscribe(res => {
        let list = res;
        list.forEach(item => {
          this.items.push(new TreeviewItem(item));
        });
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
    this.showTreeView = true;
  }
  onResidentDeSelect(residentID: any) {
    this.resetScreen();
    this.getTreeviewData();
    this.showControls = false;
    this.residentID = 0;
    // this.residentID =this.selectedItems;
    this.loadResidentDocuments(0, null);
    this.showTreeView = false;

  }
  onResidentSelect(residentID: any) {
    this.resetScreen();
    this.getTreeviewData();
    this.showControls = false;
    this.residentID = residentID.Patient_Id;
    // this.residentID =this.selectedItems;
    let selectedResident = this.residents.filter(r => r.Patient_Id == residentID.Patient_Id);
    this.myform.patchValue(
      {
        ddlresidents: selectedResident,
      });
    this.loadResidentDocuments(residentID.Patient_Id, null);
  }
  loadResidentDocuments(residentId: number, folderIds: string) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<UploadedDocuments[]>(this.configs.Emar_DocumentManager_GetUploadedDocuments + residentId + "/" + folderIds)
      .subscribe(res => {
        this.residentDocs = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }
  getuploaddocumentsbyfolderid() {

    // if (this.folderid == undefined) {
    //   this.alertService.warn("No data found");
    // }
    // else {
    //   this.dataservice.get<UploadedDocuments[]>(this.configs.Emar_Documentmanager_Uploadeddocumentsbyfolderid + this.folderid)
    //     .subscribe(res => {
    //       this.residentDocs = res;
    //       this.ng4LoadingSpinnerService.hide();
    //     }, error => {
    //       this.alertService.error(error.message);
    //       this.ng4LoadingSpinnerService.hide();

    //     });
    // }

  }

  getResidentDropData() {
    this.ng4LoadingSpinnerService.show();
    let userId = this.persistanceService.get(this.configs.loggedInUserKey);
    this.dataservice.get<any[]>(this.configs.Emar_ResidentDemographic_GetResidentDropData + userId)
      .subscribe(res => {
        this.residents = res;
        if (this.residentID == 0) {
          this.residentID = this.residents[0].Patient_Id;
          this.ng4LoadingSpinnerService.hide();
        }
        this.showControls = false;
      }, error => this.errorMessage = <any>error.message);
    this.ng4LoadingSpinnerService.hide();
  }

  downLoadFile(PatientDoc_Id, filename) {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.configs.Emar_DownloadUploadedDocuments + PatientDoc_Id)
      .subscribe((res: any) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: res.DocType });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = filename;
        a.download = link;//.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }

  DeleteDocument(PatientDoc_Id, patientId) {
    this.ng4LoadingSpinnerService.hide();
    this.dataservice.post(this.configs.Emar_Documentmanager_DeleteDocument + PatientDoc_Id, '')
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.value = this.value == undefined ? null : this.value;
        this.loadResidentDocuments(patientId, this.value);
        this.alertService.success("Deleted successfully");
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  uploadDocument() {

    let id = this.myform.value.ddlresidents[0].Patient_Id;
    if (this.value == undefined) {

      this.alertService.warn('Please choose only one folder');
    }
    else if (this.uploadedFile == null) {
      this.alertService.warn('Please upload file');
    }
    else if (this.filenameextension.toLowerCase() == ".exe") {
      this.alertService.warn("Unable to upload .exe files");
      this.myform.patchValue({
        filedata: '',
      });
    }
    else {

      this.ng4LoadingSpinnerService.show();
      var x: Date = new Date();
      var link: string = x.getMonth() + 1 + '' + x.getDate() + '' + x.getFullYear() + '' + x.getHours() + '' + x.getMinutes() + '' + x.getSeconds();
      this.uploaddocument = {
        PatientDoc_Id: 0,
        Patient_Id: id,
        DocName: this.myform.value.selectedFolder + link + "" + this.filenameextension,
        DocType: this.filetype,
        DocLocation: null,
        DocDescription: this.myform.value.description,
        FolderID: this.value,
        UDocuments_CreatedDate: new Date().toISOString(),
        UDocuments_CreatedBy: this.persistanceService.get(this.configs.loggedInUserKey),
      }
      let formData: FormData = new FormData();
      formData.append('excel', this.uploadedFile);

      this.dataservice.postFormData(this.configs.Emar_Resident_UploadResidentDocument, this.uploaddocument, formData)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
          //this.myform.reset();
          this.loadResidentDocuments(this.residentID, this.value);
          // this.myform.patchValue(
          //   {
          //     date:'',
          //     description:'',
          //     filedata:'',
          //   }
          // );
          this.alertService.success("Save successful");
          this.myform.controls["date"].reset();
          this.myform.controls["description"].reset();
          this.myform.controls["filedata"].reset();
        },
          error => {
            this.alertService.error(error.message);
            this.ng4LoadingSpinnerService.hide();
          });
    }
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.patchValue(
      {
        description: '',
        date: '',
        filedata: '',
      }
    );
    this.folderform.patchValue(
      {
        mainfolder: '',
        childfolder: '',
        ddlFolders: '0'
      }
    );
    this.ng4LoadingSpinnerService.hide();
    this.uploadedFile = null;
  }
  getfoldernames() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.configs.Emar_Documentmanager_GetFolderNames)
      .subscribe(res => {
        this.foldersList = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  mainfolder() {
    this.selectstyle = 1;
  }
  childfolder() {
    this.selectstyle = 2;
  }
  insertmainfolder() {
    let folderObj: FolderNames;
    folderObj = {
      FolderID: 0,
      FolderName: this.folderform.value.mainfolder,
      FolderParentID: 0,
      FolderLocation: null,
      Folder_Status: 1,
      Folder_CreatedBy: this.persistanceService.get(this.configs.loggedInUserKey),
      Folder_CreatedDate: this.dateFormatPipe.transform(new Date())
    }
    this.dataservice.post(this.configs.Emar_Documentmanager_InsertFolders, folderObj)
      .subscribe(res => {
        if (res > 0) {
          this.resetScreen();
          this.getTreeviewData();
          this.getfoldernames();
          this.myform.patchValue({
            selectedFolder: this.folderform.value.childfolder
          });
          this.value = res;
          this.selectstyle = 0;
          this.alertService.success("Created folder successfully");
          this.ng4LoadingSpinnerService.hide();
        }
        else
          this.alertService.error('Something went wrong.');
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  insertchildfolder() {
    let folderObj: FolderNames;
    folderObj = {
      FolderID: 0,
      FolderName: this.folderform.value.childfolder,
      FolderParentID: this.folderform.value.ddlFolders,
      FolderLocation: null,
      Folder_Status: 1,
      Folder_CreatedBy: this.persistanceService.get(this.configs.loggedInUserKey),
      Folder_CreatedDate: this.dateFormatPipe.transform(new Date())
    }
    this.dataservice.post(this.configs.Emar_Documentmanager_InsertFolders, folderObj)
      .subscribe(res => {
        if (res > 0) {
          this.resetScreen();
          this.getfoldernames();
          this.getTreeviewData();
          this.myform.patchValue({
            selectedFolder: this.folderform.value.childfolder
          });
          this.value = res;
          this.selectstyle = 0;
          this.alertService.success("Created Folder Successfully");
          this.ng4LoadingSpinnerService.hide();
        }
        else
          this.alertService.error('Something went wrong.');
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  closemodal() {
    this.selectstyle = 0;
    this.resetScreen();
  }
}
