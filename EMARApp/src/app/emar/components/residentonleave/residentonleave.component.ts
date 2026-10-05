import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-residentonleave',
  templateUrl: './residentonleave.component.html',
  styleUrls: ['./residentonleave.component.css']
})
export class ResidentonleaveComponent implements OnInit {
  template: string;
  @Input() leaveChanges: any;
  @Output() leaveRequestResult: EventEmitter<any> = new EventEmitter();
  visitId: any;
  isOnLeave: any;
  errorMessage: string = '';
  residentleaveform: FormGroup;
  pageConfig = {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration,
    private formBuilder: FormBuilder, private persistanceService: PersistanceService, private dateFormatPipe: CustomdatePipe, public activeDefaultModal: NgbActiveModal) {
    this.residentleaveform = this.formBuilder.group({
      leaveStartDate: new FormControl('', Validators.required),
      reason: new FormControl('', Validators.required),
      leaveEndDate: new FormControl('')
    });
  }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DemographicInformation");
    this.visitId = this.leaveChanges.visitId;
    this.isOnLeave = this.leaveChanges.isOnLeave;
    this.template = this.dataservice.template;
  }
  saveClick() {
    this.errorMessage = '';
    if (this.residentleaveform.value.leaveEndDate != '') {
      if (this.residentleaveform.value.leaveEndDate < this.residentleaveform.value.leaveStartDate)
        this.errorMessage = "Please Select Valid End Date";
      else if (this.residentleaveform.value.leaveEndDate < this.dateFormatPipe.dateFormat(new Date()))
        this.errorMessage = "Please Select Valid End Date";
      else
        this.insertLeave();
    }
    else
        this.insertLeave();
  }
  insertLeave() {
    this.ng4LoadingSpinnerService.show();
    let leaveObj = {
      IsOnLeave: this.isOnLeave,
      PVisit_Id: this.visitId,
      LeaveFrom: this.residentleaveform.value.leaveStartDate,
      LeaveTo: this.residentleaveform.value.leaveEndDate,
      Reason: this.residentleaveform.value.reason,
      OnLeave_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey)
    };
    this.dataservice.post(this.config.Emar_ResidentDemographic_InsertUpdatePatientOnLeave, leaveObj)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        if (res > 0) {
          this.errorMessage = '';
          this.leaveRequestResult.emit(res);
        }
        else {
          this.errorMessage = 'Something went wrong.';
        }
      },
        error => {
          this.errorMessage = error.message;
          this.ng4LoadingSpinnerService.hide();
        });
  }
  cancelClick() {
    this.errorMessage = '';
    this.residentleaveform.patchValue({
      leaveStartDate: '',
      leaveEndDate: '',
      reason: ''
    });
  }
}