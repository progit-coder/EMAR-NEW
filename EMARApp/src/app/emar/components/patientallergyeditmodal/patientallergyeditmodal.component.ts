
import { of as observableOf, Observable, Subject } from 'rxjs';

import { catchError, switchMap, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Allergies } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services';
import { AllergyInfoMaster } from '../../../models/allergyandicd.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { OtherallergyalertComponent } from '../otherallergyalert/otherallergyalert.component';

@Component({
  selector: 'app-patientallergyeditmodal',
  templateUrl: './patientallergyeditmodal.component.html',
  styleUrls: ['./patientallergyeditmodal.component.css']
 
})
export class PatientallergyeditmodalComponent implements OnInit {

  @Output() allrgyInfoResult = new EventEmitter<any>();
  @Input() selectedResident: any;
  @Input() AllergyId: any;
  @Input() Pstatus: any;
  public url: string;
  myform: FormGroup;
  residentId: number;
  residentStatus: number;
  allergyInfoObj: Allergies;
  public allergy;//: Observable<any[]>;
  private searchClassTerms = new Subject<string>();
  private searchDrugTerms = new Subject<string>();
  public flag: boolean = true;
  displayfield: any = {};
  searchByClass: any;
  allergyClassData: AllergyInfoMaster[];
  allergyDrugData: AllergyInfoMaster[];
  public Hlsevenconfig: Hlsevenconfigs[] = [];
  public dFlag: number = 1;
  public segmentDesc: string = "Allergy";
  AllergyName: any;
  selectedAllergy: string = '';
  selectedAllergyId: number = 0;
  pageConfig = {};
  public errormessage1: any;
  public errormessage2: any;
  searchText: string = "";
  pAllergyId: number;
  public approval: any;
  public allergies: Allergies[] = [];
  public errormessage3: any;
  public requiredErrorMessage: string = '';
  public isReadOnly: boolean = false;
  public allFields: number;
  public aFlag: boolean = true;
  AllergyTypeMasterId: any;
  public AllergyTypeId: number = 1;
  public allergyId: number = 0;
  maxStartDate: string = this.dateFormatPipe.dateFormat(new Date());
  reactiondropdownList = [];
  reaction_dropdownSettings = {};
  selectedItems = [];
  selectednItemsNew =[];
  public selectedReactionCodes: any;
  public reactionCode: string = "";
  public reactionOther: boolean = false;
  public reactionResponse:string;
  public OtherReactionResponse =[];
  public NameofCode:string="1";
    //other field
 public showOtherAllergyInput = false;
 public allergyNameChange: boolean = false;
 public otherAllergyitem:string=''
   modalOption: NgbModalOptions = {};
  constructor(private dataservice: DataService, private config: APIConfiguration, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, public activeDefaultModal: NgbActiveModal,  private modalService: NgbModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Allergies");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.myform = new FormGroup({
          ddlallergy: new FormControl('', Validators.required),// [Validators.maxLength(10), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          txtreaction: new FormControl('', [Validators.required, Validators.maxLength(15), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
          reactiondate: new FormControl(''),
          reactionothers: new FormControl('',Validators.maxLength(20)),
          otherAllergyName: new FormControl('')
        });
        this.reactiondropdownList = [
          { item_id: 'RASH', item_text: 'Skin rashes/hives' },
          { item_id: 'NAUSEA', item_text: 'Nausea/Vomiting' },
          { item_id: 'SHOCK', item_text: 'Shock/Unconsciousness' },
          { item_id: 'ANEMIA', item_text: 'Anemia/Blood' },
          { item_id: 'ASTHMA', item_text: 'Asthma' },
          { item_id: 'UNK', item_text: 'Additional/Other' }
        ];
        this.reaction_dropdownSettings = {
          singleSelection: false,
          idField: 'item_id',
          textField: 'item_text',
          selectAllText: 'Select All',
          unSelectAllText: 'UnSelect All',
          itemsShowLimit:1,
          allowSearchFilter: true
          
        };
        this.residentId = this.selectedResident;
        this.pAllergyId = this.AllergyId;
        this.residentStatus = this.Pstatus;
        if (this.residentId != 0 && this.residentId != undefined) {
          this.loadSearchData();
          this.checkAllergyStatus(this.pAllergyId);
          this.getResidentAllergies();
        }
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }

  checkAllergyStatus(allergyId: any) {
    if (allergyId == 0) {
      this.myform.reset();
      this.errormessage2 = '';
    }
    else {
      this.getAllergiesbyAllergiesId(allergyId);
    }
  }
  changeFlag(type: number) {
    this.dFlag = type;
  }
  // changeAllergyCode(type: number) {
  //   if (type != 1)
  //     this.aFlag = true;
  //   else
  //     this.aFlag = true;

  // }
  onSearchChange(searchValue: string) {

    console.log(searchValue);
  }
  saveAllergyInfo() {
    this.ng4LoadingSpinnerService.show();
    this.requiredErrorMessage = '';
    if (this.selectedAllergyId == 0 || (this.selectedAllergyId != 0 || this.selectedAllergy != '')) {
      if (this.selectedAllergyId == 0) {
        this.AllergyTypeId = 2;
        //this.selectedAllergy = this.myform.value.ddlallergy;
        this.selectedAllergy = this.myform.value.otherAllergyName;
        this.selectedAllergyId = null;
        if (this.myform.value.ddlallergy == "" || this.myform.value.ddlallergy == undefined || this.myform.value.ddlallergy == null) {
          this.requiredErrorMessage = "Allergy is required";
          this.ng4LoadingSpinnerService.hide();
        }
        else
        {
          this.insertUpdateAllergies();
        }
      }
      else if (this.selectedAllergyId != 0 || this.selectedAllergy != '') {
        let allergyDesc = this.myform.value.ddlallergy == undefined ? '' : this.myform.value.ddlallergy;
        let allergyId = this.AllergyTypeMasterId;
        let result = this.pAllergyId == 0 ? this.allergies.find(x => x.ClassDrugType == allergyId && x.ClassDrug_Name.replace(/\s/g, '').toLowerCase() == this.selectedAllergy.toLowerCase().replace(/\s/g, '')) : this.allergies.find(x => x.PAllergy_Id != this.pAllergyId && x.ClassDrugType == allergyId && x.ClassDrug_Name.replace(/\s/g, '').toLowerCase() == this.selectedAllergy.toLowerCase().replace(/\s/g, ''));
        if (result) {
          this.ng4LoadingSpinnerService.hide();
          this.allrgyInfoResult.emit(-1);
        }
        else if (this.myform.value.ddlallergy == "" || this.myform.value.ddlallergy == undefined || this.myform.value.ddlallergy == null) {
          this.requiredErrorMessage = "Allergy is required";
          this.ng4LoadingSpinnerService.hide();
        }
        else if ((this.myform.value.txtreaction == "" || this.myform.value.txtreaction == undefined || this.myform.value.txtreaction == null)) {
          this.requiredErrorMessage = "Reaction is required";
          this.ng4LoadingSpinnerService.hide();
        }
        // else if(this.AllergyTypeMasterId ==2 && (this.myform.value.txtreaction !=null || this.myform.value.txtreaction !=undefined || this.myform.value.txtreaction !="") )
        // {
        //    //   if(this.myform.value.txtreaction.length > 15 && (this.myform.value.txtreaction !=null || this.myform.value.txtreaction !=undefined || this.myform.value.txtreaction !=""))
        //     // {
        //     //  this.requiredErrorMessage = "Reaction must be 15 characters long.";
        //     //  this.ng4LoadingSpinnerService.hide();
        //     // }
        //      else
        //      {
        //        this.insertUpdateAllergies();
        //      }
        // }
        else {
          this.insertUpdateAllergies();
        }
      }
    }
  }
  insertUpdateAllergies() {
    if(this.aFlag == true){
    this.reactionCode = "UNK";
    if ((this.myform.value.txtreaction.filter(e => e.item_id === 'UNK').length >= 1) && (this.myform.value.reactionothers != null && this.myform.value.reactionothers != undefined && this.myform.value.reactionothers != "")) {
      this.selectedItems = [];
      let reactionIds =this.myform.value.txtreaction.filter(e=>e.item_id !='UNK');
      reactionIds.forEach(item => this.selectedItems.push(item.item_id));
      this.selectedReactionCodes = "";
      this.selectedReactionCodes = this.selectedItems.join(' ');
      if(this.selectedReactionCodes !="")
      {
        this.reactionCode = this.selectedReactionCodes + ' ' + this.myform.value.reactionothers;
      }
      else {
        this.reactionCode =this.myform.value.reactionothers;
      }

    }
    else if ((this.myform.value.txtreaction.filter(e => e.item_id === 'UNK').length >= 1) && (this.myform.value.reactionothers == null && this.myform.value.reactionothers == undefined && this.myform.value.reactionothers == "")) {
      this.selectedItems = [];
     this.myform.value.txtreaction.forEach(item => this.selectedItems.push(item.item_id));
      this.selectedReactionCodes = "";
      this.selectedReactionCodes = this.selectedItems.join(' ');
      this.reactionCode = this.selectedReactionCodes;
    }
    else if (this.aFlag == true) {
      this.selectedItems = [];
      this.myform.value.txtreaction.forEach(item => this.selectedItems.push(item.item_id));
      this.selectedReactionCodes = "";
      this.selectedReactionCodes = this.selectedItems.join(' ');
      this.reactionCode = this.selectedReactionCodes;
    }
  }
    this.allergyInfoObj = {
      Patient_Id: this.residentId,
      PAllergy_Id: this.pAllergyId,
      AllergyType_Id: this.AllergyTypeId,
      ClassDrug_Id: this.selectedAllergyId,
      ClassDrug_Name: this.selectedAllergy,
      ClassDrugType: this.AllergyTypeMasterId,
      NameOfCoding: this.NameofCode,
      AllergySeverityCode: "U",
      AllergyReactionCode: this.reactionCode,
      AllergyIdentificationDate: this.myform.value.reactiondate,
      PAllergy_Status: 1,
      PAllergy_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      PAllergy_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
      PAOutBoundFileStatus: 1,
      PAOutBoundApproval: null,
      PAOutBoundApprovalBy: null,
      PAOutBoundApprovalOn: null,
    };
    this.dataservice.post(this.config.Emar_AdminApproval_InsertUpdateAllergyInfo, this.allergyInfoObj)
      .subscribe(res => {
        this.getResidentAllergies();
        this.dataservice.get<number>(this.config.Emar_Company_GetApprovalFlag + this.residentId)
          .subscribe(res => {
            if (res == 1) {
              this.allrgyInfoResult.emit(0);
              this.ng4LoadingSpinnerService.hide();
            }
            else {
              this.allrgyInfoResult.emit(1);
              this.ng4LoadingSpinnerService.hide();
            }
            this.pAllergyId = 0;
            this.requiredErrorMessage = '';
          }, error => {
            this.ng4LoadingSpinnerService.hide();
            this.alertService.error(error.message);
          });
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.errormessage1 = '';
  }
  getResidentAllergies() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<Allergies[]>(this.config.Emar_ResidentDemographicAllergies_GetAllergies + this.residentId)
      .subscribe(res => {
        this.allergies = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  loadSearchData() {
    debugger;
    this.allergy = this.searchClassTerms.pipe(
      debounceTime(300),        // wait for 300ms pause in events
      distinctUntilChanged(),   // ignore if next search term is same as previous
      switchMap(term => term   // switch to new observable each time
        // return the http search observable
        ? this.dataservice.search(this.config.Emar_Allergy_GetActiveAllergiesByName + term)
        // or the observable of empty heroes if no search term
        : observableOf<any[]>([{ "AllergyId": 0, "AllergyName": "No Record Found" }])),
      catchError(error => {
        // TODO: real error handling
        this.alertService.error(error.message)
        return observableOf<any[]>([]);
      }));
  }
  // Push a search term into the observable stream.
  searchAllergy(term: string): void {
    debugger;
    //this.flag = true;
    this.allergyId = 0;
    if (this.selectedAllergy && this.selectedAllergy != this.myform.value.ddlallergy) {
      this.allergyNameChange = true;

    }
    if (term.length > 1) {
      this.flag = true;
      this.searchClassTerms.next(term.replace(/[&\/\\#,+()$~%'":.*?<>{}\s]/g, '"'));
    }
    if (term.length >= 1 && this.allergyId == 0) {
      this.myform.patchValue({
        txtreaction: ''
      });
    }
    else {
      this.flag = false;
      this.myform.patchValue({
        txtreaction: ''
      });
this.allergyNameChange = false
    }
  }
  searchAllergyDrug(term: string): void {
    //this.flag = true;
    if (term.length > 1) {
      this.flag = true;
      this.searchDrugTerms.next(term.replace(/[&\/\\#,+()$~%'":.*?<>{}\s]/g, '"'));
    }
    else {
      this.flag = false;
    }
  }
  onselectAllergy(item) {
    debugger;
    if (item.AllergyClassId != 0) {
  this.showOtherAllergyInput = false; 
      this.allergyNameChange = false
      this.requiredErrorMessage = '';

      this.allergyId = item.Allergy_Id;
      this.selectedAllergy = item.AllergyDesc;
      this.selectedAllergyId = item.Allergy_Id;
      this.AllergyName = item.AllergyDesc;
      this.AllergyTypeMasterId = item.AllergyTypeMaster_Id;
      if (this.AllergyTypeMasterId == 2) {
        this.NameofCode ="3";
      }
      else {
        this.AllergyTypeId = 2;
      }
      this.flag = false;
    }
    else {
      return false;
    }
  }
  getAllergiesbyAllergiesId(ID: number) {
    this.ng4LoadingSpinnerService.show();
    this.errormessage1 = '';
    this.requiredErrorMessage = '';
    this.dataservice.get<any>(this.config.Emar_Allgeries_GetAllgeriesById + ID)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.approval = res.PAOutBoundApproval;
        if (this.approval == 0 && res.PAllergy_Id != 0) {
          this.errormessage2 = "This resident record is pending for admin approval.";
        }
        this.fetchData(res);
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });

  }
  fetchData(res: any) {
    let reactionData = res.AllergyReactionCode.split(" ");
    if (reactionData.length > 0) {
      this.selectedItems = [];
      this.selectednItemsNew = [];
      for (let i = 0; i < reactionData.length; i++) {
        let checkRcExist = this.reactiondropdownList.find(r => r.item_id === reactionData[i]);
        if(checkRcExist ==undefined)
        {
          this.reactionOther =true;
          this.reactionResponse = reactionData[i];
          this.OtherReactionResponse.push(this.reactionResponse);
          this.selectednItemsNew.push(this.reactiondropdownList.find(r => r.item_id === 'UNK'));
        }
        if (checkRcExist != undefined) {
          if(checkRcExist.item_id =='UNK')
          {
            this.reactionOther =true;
          }
          this.selectednItemsNew.push(checkRcExist);
        }
      }
    }
    this.isReadOnly = true;
    this.NameofCode =res.NameOfCoding;
        if (res.ClassDrug_Id == null) {
      this.showOtherAllergyInput = true
      this.myform.patchValue({
        otherAllergyName: res.ClassDrug_Name,
        ddlallergy: 'OTHER ALLERGY'
      })
      this.myform.controls['otherAllergyName'].setValidators([Validators.required]);
      this.myform.controls['otherAllergyName'].updateValueAndValidity();

    }
    else {
            this.showOtherAllergyInput = false
      this.myform.patchValue({
        ddlallergy: res.ClassDrug_Name
      })
      this.myform.controls['otherAllergyName'].clearValidators();
      this.myform.controls['otherAllergyName'].updateValueAndValidity();
    }
    this.myform.patchValue({
      //ddlallergy: res.ClassDrug_Name,
      txtreaction: this.selectednItemsNew,
      reactionothers:this.OtherReactionResponse.join(" "),
      reactiondate: this.dateFormatPipe.dateFormat(res.AllergyIdentificationDate),
    });
    this.pAllergyId = res.PAllergy_Id;
    this.AllergyTypeMasterId = res.ClassDrugType;
    this.AllergyTypeId = res.AllergyType_Id;
    if (res.ClassDrugType == 1) {
      this.AllergyName = res.ClassDrug_Name;
      this.selectedAllergyId = res.ClassDrug_Id;
      this.selectedAllergy = res.ClassDrug_Name;
    }
    else if (res.ClassDrugType == 2) {
      this.AllergyName = res.ClassDrug_Name;
      this.selectedAllergyId = res.ClassDrug_Id;
      this.selectedAllergy = res.ClassDrug_Name;
    }
      else if(res.ClassDrugType == null){
      this.AllergyName = 'OTHER ALLERGY';
      this.selectedAllergyId = 0;
      this.selectedAllergy = '';
      this.otherAllergyitem=res.ClassDrug_Name
    }
    this.pAllergyId = res.PAllergy_Id;
  }
  onReactionSelect(item: any) {
    if (item.item_id == 'UNK') {
      this.reactionOther = true;
    }
  }
  onReactionDeSelect(item: any) {
    if (item.item_id == 'UNK') {
      this.reactionOther = false;
    }
  }
  onReactionSelectAll(item: any) {
    this.reactionOther = false;
    if (item[5].item_id == 'UNK') {
      this.reactionOther = true;
    }
  }
  onReactionDeSelectAll(item: any) {
    this.reactionOther = false;
  }
  onselectOtherAllergy() {

    this.AllergyName = 'OTHER';
    this.pAllergyId = 0;
    this.flag = false;
    if (this.showOtherAllergyInput) {
      this.myform.controls['otherAllergyName']
        .setValidators([Validators.required]);
    } else {
      this.myform.controls['otherAllergyName'].clearValidators();
    }
    this.myform.controls['otherAllergyName'].updateValueAndValidity();

  }
  onOtherAllergySelect() {
    this.flag = false;
    this.allergyNameChange = false
    this.allergyId = 0;

    this.openOtherAllergyModal()
  }



  openOtherAllergyModal() {
    debugger
    const modalRef = this.modalService.open(OtherallergyalertComponent, {
    //  size: 'md', windowClass: '', backdrop: 'static', // Prevents closing on outside click
    windowClass: '', backdrop: 'static', 
    keyboard: false
    });
    modalRef.componentInstance.result.subscribe((receivedResult) => {
      if (receivedResult == 2) {
        this.showOtherAllergyInput = false;
        this.myform.patchValue({
          ddlallergy: ''
        });
        this.allergyNameChange = false
        this.flag = false;
        modalRef.close();
      }
      else if (receivedResult == 1) {
        this.allergyNameChange = false
        this.showOtherAllergyInput = true;
        this.myform.patchValue({
          ddlallergy: 'OTHER ALLERGY'
        });
        this.flag = false;
        if (this.myform.value.ddlallergy == 'OTHER ALLERGY') {
          this.myform.controls['otherAllergyName']
            .setValidators([Validators.required]);
        } else {
          this.myform.controls['otherAllergyName'].clearValidators();
        }
        this.myform.controls['otherAllergyName'].updateValueAndValidity();

      }
      modalRef.close();
    });
  }
}
