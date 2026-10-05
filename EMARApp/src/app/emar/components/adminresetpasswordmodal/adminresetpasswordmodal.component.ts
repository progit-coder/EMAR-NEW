import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { APIConfiguration } from '../../../models/app.constants';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';

@Component({
  selector: 'app-adminresetpasswordmodal',
  templateUrl: './adminresetpasswordmodal.component.html',
  styleUrls: ['./adminresetpasswordmodal.component.css']
})
export class AdminresetpasswordmodalComponent implements OnInit {

  @Output() changePasswordResult: EventEmitter<any> = new EventEmitter();
  changePasswordform: FormGroup;
  @Input() selectedUser: any;
  public changePwdUserId: number;
  pageConfig: {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    private persistanceService: PersistanceService, public activeDefaultModal: NgbActiveModal, private dateFormatPipe: CustomdatePipe) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("Users");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
        this.changePwdUserId = this.selectedUser;
        this.changePasswordform = new FormGroup({
          password: new FormControl('', [Validators.required, Validators.maxLength(20), Validators.pattern(this.config.password), Validators.minLength(4)]),
        });
      }
    }
    else
      this.persistanceService.redirectToHomePage();
  }
  changeUserPassword() {
    let userObj =
    {
      User_Id: this.changePwdUserId,
      Password: this.changePasswordform.value.password,
      NewUserFlag: 1,
      User_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      User_CreatedDate: this.dateFormatPipe.dateWithTime(new Date()),
    }
    this.dataservice.post(this.config.Emar_UserMaster_ResetUserPwdByAdmin, userObj)
      .subscribe(res => {
        this.changePasswordResult.emit(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.changePasswordform.reset();
  }
}
