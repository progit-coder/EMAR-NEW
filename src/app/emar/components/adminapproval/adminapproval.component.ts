import { Component, OnInit, SimpleChanges, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { ExceldownloadService } from '../../../services/shared/exceldownload.service';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { SharedService } from '../../../services/shared/shared.service';
import { ApprovalEntity, ResidentDemographic } from '../../../models/residentdemographic.model';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Screens, Activity } from '../../../models/useractivity.model';

@Component({
  selector: 'app-adminapproval',
  templateUrl: './adminapproval.component.html',
  styleUrls: ['./adminapproval.component.css'],
  providers: [DataService, APIConfiguration, ExceldownloadService]

})
export class AdminapprovalComponent implements OnInit {
  public residentID: number;
  public template;
  public selectedResItem = [];
  dropdownSettings_Resident: any = {};
  ShowFilter = true;
  approvalData: ApprovalEntity[] = [];
  errorMessage: any;
  CheckAll: boolean = false;
  selectedOutboundList = [];
  outboundList: ApprovalEntity[];
  pageConfig = {};
  public residents: ResidentDemographic[];
  myform: FormGroup;
  gridPagination = this.config.gridPagination;
  p: number = 1;
  modalApprovalIsOpen = false;
  modalApprovalIsOpenAllergy = false;
  modalApprovalIsOpenDiagnosis = false;
  approvalChanges = [];
  searchText: string = "";
  public ApproveButton: boolean = true;
  public RejectButton: boolean = true;

  constructor(private dataservice: DataService, private config: APIConfiguration,
    private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService,
    private sharedService: SharedService, private persistanceService: PersistanceService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("AdminApproval");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.ApproveButton = true;
    this.RejectButton = true;
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getResidentDropData();
    this.GetAllApprovalPendingList();
    this.myform = new FormGroup({
      ddlresidents: new FormControl('0')
    });
    this.dropdownSettings_Resident = {
      singleSelection: true,
      idField: "Patient_Id",
      textField: "PatientName",
      itemsShowLimit: 1,
      closeDropDownOnSelection:true,
      allowSearchFilter: this.ShowFilter
    };
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.AdminApproval, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }

  selectedRecords: ApprovalEntity[] = [];
  onselectRecord(event, item: ApprovalEntity) {
    if (event == true) {
      this.ApproveButton = false;
      this.RejectButton = false;
      item.ApprovalStatus = 1;
      item.ApprovedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.ApprovedDate = new Date().toISOString();
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Record_Id == item.Record_Id && i.ApprovalId == item.ApprovalId);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.ApproveButton = true;
        this.RejectButton = true;
      }
      else {
        this.ApproveButton = false;
        this.RejectButton = false;
      }
    }
  }
  viewChanges(item: ApprovalEntity) {
    this.dataservice.get<ApprovalEntity[]>(this.config.Emar_AdminApproval_ViewModifiedData + item.ApprovalId + "/" + item.Record_Id + "/" + item.Category)
      .subscribe(res => {
        this.approvalChanges = res;
        this.modalApprovalIsOpen = true;
        //   if(item.Category =='Allergy')
        //   {
        // this.modalApprovalIsOpenAllergy =true;
        //   }
        //   if(item.Category =='Diagnosis')
        //   {
        // this.modalApprovalIsOpenDiagnosis =true;
        //   }
      },
        error => {

        });
  }
  closeModel() {
    this.modalApprovalIsOpen = false;
    this.modalApprovalIsOpenAllergy = false;
    this.modalApprovalIsOpenDiagnosis = false;
  }
  onCheckAll(event) {
    if (event == true) {
      this.ApproveButton = false;
      this.RejectButton = false;
      this.CheckAll = true;
      this.approvalData.forEach(element => {
        element.ApprovalStatus = 1;
        element.ApprovedBy = 1;
        element.ApprovedDate = new Date().toISOString();
        this.selectedRecords.push(element);
      });
    }
    else {
      this.CheckAll = false;
      this.ApproveButton = true;
      this.RejectButton = true;
      this.selectedRecords = [];
    }
  }
  getApprovalPendingDataByPatientId(residentId: number) {
    this.dataservice.get<ApprovalEntity[]>(this.config.Emar_AdminApproval_GetApprovalPendingData + residentId)
      .subscribe(res => {
        this.approvalData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }

  GetAllApprovalPendingList() {

    this.dataservice.get<ApprovalEntity[]>(this.config.Emar_AdminApproval_GetAllApprovalPendingList)
      .subscribe(res => {
        this.approvalData = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  approve(item: ApprovalEntity) {
    this.ng4LoadingSpinnerService.show();
    item.ApprovalStatus = 1;
    item.ApprovedBy = this.persistanceService.get(this.config.loggedInUserKey);
    item.ApprovedDate = new Date().toISOString();

    this.dataservice.post(this.config.Emar_AdminApproval_ApprovePendingData, item)
      .subscribe(res => {
        if (res > 0) {
          if (this.myform.value.ddlresidents.length == 0) {
            if (this.approvalData.length == 0) {
              this.getResidentDropData();
            }
            this.GetAllApprovalPendingList();
          }
          else if (this.myform.value.ddlresidents.length > 0) {
            this.getApprovalPendingDataByPatientId(this.myform.value.ddlresidents[0].Patient_Id);
            if (this.approvalData.length == 0) {
              this.getResidentDropData();
              this.GetAllApprovalPendingList();
            }
          }
        }
        else {
          this.alertService.error('Unable to process request');
        }
      },
        error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
  }
  approvecheck() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select data to approve");
      this.ApproveButton = true;
      this.RejectButton = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_AdminApproval_ApprovePendingData, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Approval successful");
            this.ng4LoadingSpinnerService.hide();
            this.ApproveButton = true;
            this.RejectButton = true;
            this.getResidentDropData();
            this.selectedRecords = [];
            if (this.myform.value.ddlresidents==undefined || this.myform.value.ddlresidents==null || this.myform.value.ddlresidents.length == 0)
              this.GetAllApprovalPendingList();
            else if (this.myform.value.ddlresidents!=undefined && this.myform.value.ddlresidents!=null && this.myform.value.ddlresidents.length > 0)
              this.getApprovalPendingDataByPatientId(this.myform.value.ddlresidents[0].Patient_Id);
          }
          else if (res == 0) {
            this.alertService.error("Approval failed");
            this.ng4LoadingSpinnerService.hide();
          }
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  rejectcheck() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please select data to reject");
      this.RejectButton = true;
      this.ApproveButton = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.selectedRecords.forEach(element => {
        element.ApprovalStatus = 2;
        element.ApprovedBy = 1;
        element.ApprovedDate = new Date().toISOString();
      });
      this.dataservice.post(this.config.Emar_AdminApproval_RejectPendingData, this.selectedRecords)
        .subscribe(res => {
          this.alertService.error("Rejected");
          this.ApproveButton = true;
          this.RejectButton = true;
          this.selectedRecords = [];
          this.getResidentDropData();
          if (this.myform.value.ddlresidents == undefined || this.myform.value.ddlresidents == null || this.myform.value.ddlresidents.length == 0)
            this.GetAllApprovalPendingList();
          else if (this.myform.value.ddlresidents != undefined && this.myform.value.ddlresidents != null && this.myform.value.ddlresidents.length > 0)
            this.getApprovalPendingDataByPatientId(this.myform.value.ddlresidents[0].Patient_Id);
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
          this.alertService.error(error.message);
        });
      window.scroll(0, 0);
    }
  }
  getResidentDropData() {
    this.dataservice.get<ResidentDemographic[]>(this.config.Emar_AdminApproval_GetAdminApprovalResidentsData)
      .subscribe(res => this.residents = res, error => this.alertService.error(error.message)
      );
  }
  changeResident() {
    if (this.myform.value.ddlresidents.length == 0)
      this.GetAllApprovalPendingList();
    this.getApprovalPendingDataByPatientId(this.myform.value.ddlresidents[0].Patient_Id);
  }
  onResidentSelect(item: any) {
    this.changeResident();
  }
  onResidentDeSelect(item: any) {
    this.changeResident();
  }

}
