import { Component, OnInit,Input, Output, EventEmitter } from '@angular/core';

import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ResidentDemographic, ResidentsCount } from '../../../models/residentdemographic.model';
import { DemographicInfo } from '../../../models/residentdemographic.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from './../../../_services/index';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl,Validators } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { MedicationReason } from '../../../models/emar.model';

@Component({
  selector: 'app-removemarmodal',
  templateUrl: './removemarmodal.component.html',
  styleUrls: ['./removemarmodal.component.css']
})
export class RemovemarmodalComponent implements OnInit {

  @Input() selectedResident: any;
  @Output() RemovedResult = new EventEmitter<any>();
  @Input() selectedNsId:any;
  public residentId: number;
  public demographicInfoData = {} as DemographicInfo;
  myform: FormGroup;
  pageConfig = {};
  public userId: number;
  public DrugList: any[] = [];
  public drugRemoveObj : any;
  public UpdateStatus: boolean = true;
  public RemoveStatus: boolean = true;
  public modalUnlockIsOpen: boolean = false;
  selectedRecords: any[];
  @Input() selectedDrugs:any[];
  RecordsList:any[];
  public timeFormatId:number=0;
  public modalUndoIsOpen:boolean=false;
  public medicationReasonList: MedicationReason[] = [];
  public medicationForm:FormGroup;
  public drugAdministerId:any;
  dropdownSettings_MedicationReason = {};
  notestatus: number = 0;
  nursingStationZoneCurrentDate: any;
  constructor(private dataservice: DataService, private config: APIConfiguration ,private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,  private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe,public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.residentId = this.selectedResident;
    // this.RecordsList = this.selectedDrugs;
    this.getDemographicInfoData();
    this.getAllFlagsForCompanyByNSId(this.selectedNsId);

    this.pageConfig = this.persistanceService.getPermissionsByScreen("DocumentAdministeredOrders");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.userId = this.persistanceService.get(this.config.loggedInUserKey);
        this.myform = new FormGroup({
         
          startDate: new FormControl(new Date().toISOString().substring(0, 10)),
          // user: new FormControl(''),
          // nurseSheduleTime: new FormControl(''),
          endDate: new FormControl(new Date().toISOString().substring(0, 10))
        });
        this.medicationForm = new FormGroup({
          medicationReason: new FormControl('', Validators.required),
          note: new FormControl('', Validators.maxLength(50)),
          undoQuantity: new FormControl('0', Validators.required)
        });
        this.dropdownSettings_MedicationReason = {
          singleSelection: true,
          idField: "MedicationReason_ID",
          textField: "MedicationReason_Desc",
          text: "Medication Reason",
          itemsShowLimit: 1,
          closeDropDownOnSelection:true,
          allowSearchFilter: true
        };
  }
}
  }
  getDemographicInfoData() {
    this.dataservice.get<DemographicInfo>(this.config.Emar_ResidentDemographic_GetResidentInformation + this.residentId)
      .subscribe(res => {
        this.demographicInfoData = res;
        this.getDrugsToRemove();
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
      }

     
      getDrugsToRemove() {
        this.selectedRecords = [];
        this.DrugList=[];
        if(this.myform.value.startDate=="" || this.myform.value.endDate=="")
        {
          this.alertService.warn("Please select proper dates");
          this.ng4LoadingSpinnerService.hide();
        }
        else if (this.myform.value.startDate!="" && this.myform.value.endDate!="" && this.myform.value.startDate > this.myform.value.endDate) {
          this.alertService.warn("End date cannot be before start date");
          this.ng4LoadingSpinnerService.hide();
        }
        else if(this.myform.value.startDate!="" && this.myform.value.endDate!="" && this.myform.value.endDate < this.myform.value.startDate)
        {
          this.alertService.warn("Start date cannot be after end date");
          this.ng4LoadingSpinnerService.hide();
        }
        else
        {
        let st=this.myform.value.startDate;
        let startdate = this.myform.value.startDate.split('/').join('-');
        let enddate = this.myform.value.endDate.split('/').join('-');
        
        this.dataservice.get<any[]>(this.config.Emar_Common_GetDrugs + this.residentId + "/" + startdate + "/" + enddate)
          .subscribe(res => {
            this.DrugList = res;
            this.getMedicationReason();
         // this.RecordsList= res;
            //this.alertService.warn("No data available.");
            this.ng4LoadingSpinnerService.hide();
          },
            error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
          }
      }
      modalUnlockopen() {
        this.modalUnlockIsOpen = true;
      }
      closeUnlockModel() {
        this.modalUnlockIsOpen = false;
      }
      Remove()
      {
        debugger
        this.ng4LoadingSpinnerService.show();
        if (this.selectedRecords.length == 0) {
          this.alertService.warn("Please select data to remove");
         
          this.RemoveStatus==false;
          this.ng4LoadingSpinnerService.hide();
        }
        else {
          this.dataservice.post(this.config.Emar_Common_RemoveDAO, this.selectedRecords)
    
            .subscribe(res => {
              this.ng4LoadingSpinnerService.hide();
              if (res == 1) {
                this.RemovedResult.emit(1);
                // this.alertService.success("Removed Successfully");
                this.getDrugsToRemove();
                //this.resetScreen();
                // this.inactivecheckbox = false;
              }
              
    
            }, error => {
              this.alertService.error(error.message);
              this.ng4LoadingSpinnerService.hide();
            });
        }
        this.modalUnlockIsOpen = false;
      }
      resetScreen() {
        this.myform.reset();
      }
      onselectRecord(event, item: any) {
        debugger
        if (event == true) {
          this.UpdateStatus = false;
          this.drugAdministerId=item.DrugAdminister_Id;
          this.modalUndoIsOpen=true;
          // this.RemoveStatus = false;
          // this.drugRemoveObj = {
          //   DrugAdminister_Id: item.DrugAdminister_Id,
          //   POrder_Id: item.Porder_Id,
          //   pquantity_Id: item.PQuantity_Id,
          // };
          // this.selectedRecords.push(this.drugRemoveObj);
        }
        else {
          const index = this.selectedRecords.findIndex(i => i.DrugAdminister_Id == item.DrugAdminister_Id);
          this.selectedRecords.splice(index, 1);
          if (this.selectedRecords.length == 0) {
           
            this.RemoveStatus = true;
          }
          else {
           
            this.RemoveStatus = false;
          }
        }
      }
      getAllFlagsForCompanyByNSId(stationId: number) {
        this.dataservice.get<any>(this.config.Emar_Company_GetAllFlagsForCompanyByNSId + stationId)
          .subscribe(res => {
            this.timeFormatId = res.TimeFormat;;
            this.getNursingStationTimeZone(stationId);
          }, error => {
            this.alertService.error(error.message);
          });
      }
      getNursingStationTimeZone(stationId: number) {
        this.dataservice.get<any>(this.config.Emar_Common_GetTimeZoneDate + stationId)
          .subscribe(res => {
            debugger
            this.nursingStationZoneCurrentDate=res;
            // this.myform.patchValue({
            //   startDate:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate),
            //   endDate:this.dateFormatPipe.transformISODate(this.nursingStationZoneCurrentDate)
            // });
          }, error => {
            this.alertService.error(error.message);
          });
      }
  timeFormate(time: any): any {
    let hour = (time.split(':'))[0]
    let min = (time.split(':'))[1]
    let part = hour > 12 ? 'PM' : 'AM';
    min = (min + '').length == 1 ? `0${min}` : min;
    hour = hour > 12 ? hour - 12 : hour;
    hour = (hour + '').length == 1 ? `0${hour}` : hour;
    return `${hour}:${min} ${part}`
  }
  getMedicationReason() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<MedicationReason[]>(this.config.Emar_GetMedicationReason)
      .subscribe(res => {
        this.medicationReasonList = res;
        //this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  undoReasonSave()
  {
    var record=this.DrugList.find(dr=>dr.DrugAdminister_Id=this.drugAdministerId);
    this.RemoveStatus = false;
        this.drugRemoveObj = {
          DrugAdminister_Id: record.DrugAdminister_Id,
          POrder_Id: record.Porder_Id,
          pquantity_Id: record.PQuantity_Id,
          MedicationReason_ID:this.medicationForm.value.medicationReason[0].MedicationReason_ID,
          quantity:this.medicationForm.value.undoQuantity != '' ? this.medicationForm.value.undoQuantity : 0,
          AdministerComment:this.medicationForm.value.note,
          AdminsterBy:this.userId
        };
    this.selectedRecords.push(this.drugRemoveObj);
    this.modalUndoIsOpen=false;
    this.medicationForm.reset();
    const undoQuantityValidations = this.medicationForm.get('undoQuantity');
    undoQuantityValidations.setValidators([Validators.required]);;
    undoQuantityValidations.updateValueAndValidity();
    this.medicationForm.patchValue({
      undoQuantity: '0'
    });
  }
  closeUndoModel()
  {
    var record=this.DrugList.find(dr=>dr.DrugAdminister_Id=this.drugAdministerId);
    let CheckBoxId = "#" + this.drugAdministerId;
    $(CheckBoxId).prop("checked",false);
    this.onselectRecord(false,record);
    this.medicationForm.reset();
    const undoQuantityValidations = this.medicationForm.get('undoQuantity');
    undoQuantityValidations.setValidators([Validators.required]);;
    undoQuantityValidations.updateValueAndValidity();
    this.medicationForm.patchValue({
      undoQuantity: '0'
    });
    this.modalUndoIsOpen=false;
  }
  onMedicationReasonSelect(item: any) {
    if ((item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators([Validators.required, Validators.maxLength(50)]);
      notevalidation.updateValueAndValidity();
      this.notestatus = 1;
    }
    else {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
  }
  onMedicationReasonDeSelect(item: any) {
    if ((item != null && item != undefined) && item.MedicationReason_ID == 7) {
      const notevalidation = this.medicationForm.get('note');
      notevalidation.setValidators(null);
      notevalidation.clearValidators();
      notevalidation.updateValueAndValidity();
      this.notestatus = 0;
      this.medicationForm.patchValue({
        note: ''
      });
    }
    this.medicationForm.patchValue({
      note: ''
    });
  }
}






  