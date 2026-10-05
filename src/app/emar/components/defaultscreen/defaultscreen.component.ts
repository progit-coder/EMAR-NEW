import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators, Validator } from '@angular/forms';
import { DataService } from '../../../services/shared/dataservice.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { APIConfiguration } from '../../../models/app.constants';
import {  DefaultScreen } from '../../../models/role.model';
import { AlertService } from '../../../_services/index';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-defaultscreen',
  templateUrl: './defaultscreen.component.html',
  styleUrls: ['./defaultscreen.component.css']
})
export class DefaultscreenComponent implements OnInit {

  @Output() allDefaultScreens: EventEmitter<any> = new EventEmitter();
  defaultScreenform: FormGroup;
  public defaulrScreensList:any;
  defaultScreenObj: DefaultScreen;
  public dropdownSettings_DefaultScreen: any = {};
  public selecteddItems: any[] = [];
  public screens:any[]=[];
  pageConfig: {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    private persistanceService: PersistanceService,public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DefaultScreen");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.getDefaultScreenDrop();
    this.defaultScreenform = new FormGroup({
      defaultScreen: new FormControl('', Validators.required),
    });
    this.dropdownSettings_DefaultScreen = {
      singleSelection: true,
      idField: "Screen_Id",
      textField: "Screen_Desc",
      text: "Select",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: true
    };
    this.getRoleScreensData();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  addDefaultScreen(obj: any) {
    if (obj == null) {
      this.defaultScreenObj = {
        DefaultScreen_Id: 0,
        Screen_Id: this.defaultScreenform.value.defaultScreen[0].Screen_Id,
        DefaultScreen_Status: 1,
        DefaultScreen_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        DefaultScreen_CreatedOn: new Date().toISOString(),
      };
    }
    else {
      this.defaultScreenObj = {
        DefaultScreen_Id: obj.DefaultScreen_Id,
        Screen_Id: obj.Screen_Id,
        DefaultScreen_Status: 2,
        DefaultScreen_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
        DefaultScreen_CreatedOn: new Date().toISOString(),
      };
    }
    this.ng4LoadingSpinnerService.show();
    this.dataservice.post(this.config.Emar_RoleConfigMaster_InsertDefaultScreen, this.defaultScreenObj)
      .subscribe(res => {
        if(res==2)
        {
          this.allDefaultScreens.emit(2);
          this.ng4LoadingSpinnerService.hide();
        }
        else if(res==1)
        {
        this.ng4LoadingSpinnerService.hide();
        this.getDefaultScreenDrop();
        this.getRoleScreensData();
        this.allDefaultScreens.emit( this.defaultScreenObj.DefaultScreen_Id==0?1:0);
        }
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.defaultScreenform.reset();
  }
  getDefaultScreenDrop() {
    this.dataservice.get<any[]>(this.config.Emar_RoleConfigMaster_GetDefaultScreenDropData)
      .subscribe(res => {
        this.defaulrScreensList = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message)
      });
  }
  getRoleScreensData() {
    this.dataservice.get<any[]>(this.config.Emar_RoleConfigMaster_GetScreens + 0)
      .subscribe(res => {
        this.screens = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message)
      });
  }
}
