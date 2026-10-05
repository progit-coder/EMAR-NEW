import { Component, OnInit, ChangeDetectorRef, Output, EventEmitter,ViewChild,ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validator, Validators } from '@angular/forms';
import { Room } from '../../../models/facility.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import {AlertService} from '../../../_services/index'; 
import { PersistanceService } from '../../../services/shared/persistance.service';

@Component({
  selector: 'app-bedconfigroom',
  templateUrl: './bedconfigroom.component.html',
  styleUrls: ['./bedconfigroom.component.css'],
  providers: [DataService, APIConfiguration]
})
export class BedconfigroomComponent implements OnInit {
  @ViewChild("closeAddRoomModal") closeRoomModal:ElementRef;
  myform: FormGroup;
  private room = new Room();
  public template;
  private roomId: number = 0;
  public rooms: Room[]=[];
  errorMessage: string;
  searchText:string="";
  p: number = 1;
  gridPagination = this.config.gridPagination;
  TableName="Room";
  auditTable:any;
  public modalHistoryIsOpen : boolean = false;
  @Output()
  NewRoom=new EventEmitter();
  pageConfig: {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private config: APIConfiguration, private chRef: ChangeDetectorRef, private alertService: AlertService,private persistanceService: PersistanceService) { }

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
      roomname: new FormControl('', [Validators.required,Validators.maxLength(50), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      roomcode: new FormControl('', [Validators.required, Validators.maxLength(20),Validators.minLength(4), Validators.pattern(this.config.alphaNumericFewSpecialCharacters1)]),
      status: new FormControl('1')

    });
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  // getcompanyselected(item: number) {
  //   alert('Selected vlalue : ' + item);
  // }

  getHistoryById(roomId: number)
  {
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
      Room_CreatedBy:this.persistanceService.get(this.config.loggedInUserKey),
      Room_CreatedDate: new Date().toISOString()
    };
    this.dataservice.post(this.config.Emar_Facility_InsertRoom, this.room)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.alertService.success("Save successful");
        this.NewRoom.emit();
        this.closeRoomModal.nativeElement.click();
        this.getRooms();
      }, error => {this.errorMessage = <any>error.message;
        this.alertService.error(this.errorMessage);
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
     this.NewRoom.emit();
  }
  getRooms() {
    this.dataservice.get<any[]>(this.config.Emar_Facility_GetAllRooms)
      .subscribe(res => {
      this.rooms = res;
        this.ng4LoadingSpinnerService.hide();
      }, error => {
      this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)
        this.ng4LoadingSpinnerService.hide();
      });

  }
  getRoomDetailsByID(roomId: number) {
    window.scroll(0,0);
    // function high(edit);
    this.dataservice.get<Room>(this.config.Emar_Facility_GetRoomDetailsById + roomId)
      .subscribe(res => this.fetchData(res), error =>{ this.errorMessage = <any>error.message;
      this.alertService.error(this.errorMessage)});
  }
  fetchData(res: Room) {
    this.myform.patchValue({
      roomname: res.Room_Name,
      roomcode: res.Room_Code,
      status: res.Room_Status
    });
    this.roomId = res.Room_Id;
  }

  checkRoomMaster():any{
    let Room_Name=this.myform.value.roomname;
    let result=this.rooms.find(x => x.Room_Name.replace(/\s/g, '').toLowerCase() === Room_Name.toLowerCase().replace(/\s/g, ''));
    if(result)
    {
      this.alertService.warn("Room Name Already Exists");
      this.myform.patchValue({
        roomname:''
      });
    }
    else{}
  }

}
