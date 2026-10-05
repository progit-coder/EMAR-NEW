import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Room } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Screens, Activity } from '../../../models/useractivity.model';
import { SharedService } from '../../../services/shared/shared.service';
import { CustomdatePipe } from '../../../services/shared/customdate.pipe';
@Component({
  selector: 'app-roommaster',
  templateUrl: './roommaster.component.html',
  styleUrls: ['./roommaster.component.css'],
  providers: [DataService, APIConfiguration]
})
export class RoommasterComponent implements OnInit {
  myform: FormGroup;
  private room = new Room();
  public template;
  private roomId: number = 0;
  public rooms: Room[] = [];
  errorMessage: string;
  searchText: string = "";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  TableName = "Room";
  auditTable: any;
  public selectedRecords: any[] = [];
  public UpdateStatus: boolean = true;
  CheckAll: boolean = false;
  public roomList: any[] = [];
  public inactivecheckbox: boolean = false;
  public modalHistoryIsOpen: boolean = false;
  pageConfig: {};
  constructor(private dataservice: DataService,private dateFormatPipe: CustomdatePipe, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService, private persistanceService: PersistanceService, public sharedService: SharedService) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("RoomMaster");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.ng4LoadingSpinnerService.show();
    this.getRooms();
    this.myform = new FormGroup({
      roomname: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      roomcode: new FormControl('', [Validators.required, Validators.maxLength(20),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1')

    });
    this.userActivity();
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity() {
    this.sharedService.insertUserActivityDetails(Screens.RoomMaster, Activity.View, '')
      .subscribe(res => { }, error => {
        this.alertService.error(error.message);
      });
  }

  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }

  getHistoryById(roomId: number) {
    this.auditTable = {
      "tableName": "Room",
      "recordId": roomId
    }
    this.modalHistoryIsOpen = true;
  }
  closeModel() {
    this.modalHistoryIsOpen = false;
  }
  insertRoom() {
    this.ng4LoadingSpinnerService.show();
    this.room = {
      Room_Id: this.roomId,
      Room_Code: this.myform.value.roomcode,
      Room_Name: this.myform.value.roomname,
      Room_Status: (this.myform.value.status == true ? 1 : 0),
      Room_CreatedBy: this.persistanceService.get(this.config.loggedInUserKey),
      Room_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertRoom, this.room)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful"); this.getRooms();
        this.inactivecheckbox = false;
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });
    this.resetScreen();
  }
  resetScreen() {
    this.ng4LoadingSpinnerService.show();
    this.myform.reset();
    this.myform.patchValue({
      status: '1'
    });
    this.ng4LoadingSpinnerService.hide();
    // this.myform.patchValue({
    //   roomname: '',
    //   roomcode: '',
    //   status: '1'
    // });
    this.roomId = 0;
  }
  getRooms() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllRooms)
      .subscribe(res => {
        this.rooms = res;
        this.roomList = this.rooms.filter(r =>r.Room_Status == 1);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  updateRoomStatus() {
    this.ng4LoadingSpinnerService.show();
    if (this.selectedRecords.length == 0) {
      this.alertService.warn("Please Select Data To updateStatus.");
      this.UpdateStatus = true;
      this.ng4LoadingSpinnerService.hide();
    }
    else {
      this.dataservice.post(this.config.Emar_Facility_UpdateRoomStatus, this.selectedRecords)
        .subscribe(res => {
          if (res == 1) {
            this.alertService.success("Status Updated Successfully");
            this.UpdateStatus = true;
            this.inactivecheckbox = false;
            this.CheckAll = false;
            this.getRooms();
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
      this.roomList.forEach(element => {
        element.Room_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
        element.Room_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
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
      item.Room_CreatedBy = this.persistanceService.get(this.config.loggedInUserKey);
      item.Room_CreatedDate = this.dateFormatPipe.dateWithTime(new Date());
      this.selectedRecords.push(item);
    }
    else {
      const index = this.selectedRecords.findIndex(i => i.Room_Id == item.Room_Id);
      this.selectedRecords.splice(index, 1);
      if (this.selectedRecords.length == 0) {
        this.UpdateStatus = true;
      }
      else {
        this.UpdateStatus = false;
      }
    }
  }
  showInactiveRecords(value: any) {
    this.ng4LoadingSpinnerService.show();
    if (value == true) {
      this.roomList = this.rooms.filter(r =>r.Room_Status == 0);
      this.ng4LoadingSpinnerService.hide();
    }
    if (value == false) {
      this.getRooms();
    }
  }
  getRoomDetailsByID(roomId: number) {
    this.ng4LoadingSpinnerService.show();
    window.scroll(0, 0);
    // function high(edit);
    this.dataservice.get<Room>(this.config.Emar_Facility_GetRoomDetailsById + roomId)
      .subscribe(res => {
        this.fetchData(res);
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.error(error.message);
      });
  }
  fetchData(res: Room) {
    this.myform.patchValue({
      roomname: res.Room_Name,
      roomcode: res.Room_Code,
      status: res.Room_Status
    });
    this.roomId = res.Room_Id;
  }

  checkRoomMaster(): any {
    let Room_Name = this.myform.value.roomname;
    let result = this.rooms.find(x => x.Room_Name.replace(/\s/g, '').toLowerCase() === Room_Name.toLowerCase().replace(/\s/g, ''));
    if (result) {
      this.alertService.error("Room Name Already Exists");
      this.myform.patchValue({
        roomname: ''
      });
    }
    else { }
  }
}
