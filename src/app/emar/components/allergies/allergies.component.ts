import { Component, OnInit, Input, SimpleChange } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validator, Validators, RequiredValidator } from '@angular/forms';
import { Allergies } from '../../../models/residentdemographic.model';
import { SharedService } from '../../../services/shared/shared.service';
import { AlertService } from '../../../_services';
import { Observable, Subject } from 'rxjs';
import { AllergyInfoMaster } from '../../../models/allergyandicd.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Hlsevenconfigs } from '../../../models/hlsevensegment.model';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PatientallergyhistorymodalComponent }from '../patientallergyhistorymodal/patientallergyhistorymodal.component';
import { PatientallergyeditmodalComponent }from '../patientallergyeditmodal/patientallergyeditmodal.component';

@Component({
  selector: 'app-allergies',
  templateUrl: './allergies.component.html',
  styleUrls: ['./allergies.component.css'],
  providers: [DataService, APIConfiguration]
})
export class AllergiesComponent implements OnInit {

  public allergies: Allergies[] = [];
  residentId: number;
  residentStatus: number;
  @Input() selectedResident: any;
  pageConfig = {};
  pAllergyId: number;
  

  public modalAllergyIsOpen: boolean = false;
  userId: number = 0;
  PAllergy_Id: number = 0;

  constructor(private dataservice: DataService, private config: APIConfiguration, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe,private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private modalService: NgbModal) {
  }
  ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
    if (changes.selectedResident && changes.selectedResident.currentValue != undefined) {
      this.residentId = this.selectedResident.residentId;
      this.residentStatus = this.selectedResident.residentStatus;
      this.getResidentAllergiesByPId();
    }
    else
      this.allergies = [];
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Allergies");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.sharedService.currentPatientId.subscribe(patientId => this.residentId = patientId);
    if (this.residentId != 0 && this.residentId != undefined) {
      this.getResidentAllergiesByPId();
    }
    else
      this.allergies = [];
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.Allergies, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }
  getHistoryById(allergyID: number) {
    const modalRef = this.modalService.open(PatientallergyhistorymodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.allergyID = allergyID;
  }
  getResidentAllergiesByPId() {
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
  getAllergiesbyAllergiesId(allergyId)
  {
    const modalRef = this.modalService.open(PatientallergyeditmodalComponent, { size: 'lg', windowClass: '' });
    modalRef.componentInstance.selectedResident=this.residentId;
    modalRef.componentInstance.AllergyId=allergyId;
    modalRef.componentInstance.Pstatus=this.residentStatus
    modalRef.componentInstance.allrgyInfoResult.subscribe((receivedResult) => {
          if (receivedResult == -1)
          {
           this.alertService.error("Allergy already recorded");
          }
          else if (receivedResult == 0)
          {
            this.alertService.success("Save successful, waiting for admin approval");
          }
          else if (receivedResult == 1) {
            this.alertService.success("Save successful");
          }
      this.getResidentAllergiesByPId();
      modalRef.close();
    });
  }

  closeAllergyModel() {
    this.modalAllergyIsOpen = false;
    this.PAllergy_Id = 0;
  }
  modalAllergiesopen(PAllergy_Id: any) {
    this.PAllergy_Id = PAllergy_Id;
    this.modalAllergyIsOpen = true;
  }

  RemoveAllergy() {
    //let userObj =
   // {
      //PAllergy_Id: this.PAllergy_Id,   
   // }
    this.dataservice.get<any>(this.config.Emar_ResidentDemographicAllergies_RemoveResAllergies +this.PAllergy_Id)
      .subscribe(res => {
        this.modalAllergyIsOpen = false;
        this.PAllergy_Id = 0;
        if (res == 1) {
          this.alertService.success("Allergy removed");
          this.getResidentAllergiesByPId();
        }
        else if (res == 0)
          this.alertService.error("Allergy removal failed");
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
  }

}
