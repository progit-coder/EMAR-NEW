import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { PersistanceService } from 'src/app/services/shared/persistance.service';

@Component({
  selector: 'app-residentreactivate',
  templateUrl: './residentreactivate.component.html',
  styleUrls: ['./residentreactivate.component.css']
})
export class ResidentreactivateComponent implements OnInit {

  @Input() leaveChanges: any;
  @Output() result: EventEmitter<any> = new EventEmitter();
  visitId: any;
  isOnLeave: any;
  pageConfig = {};
  constructor(private dataservice: DataService,private persistanceService: PersistanceService, private config: APIConfiguration, ) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DemographicInformation");
    this.visitId = this.leaveChanges.visitId;
    this.isOnLeave = this.leaveChanges.isOnLeave;
  }

  reactivateResident() {
    let leaveObj = {
      IsOnLeave: this.isOnLeave,
      PVisit_Id: this.visitId
    };
    this.dataservice.post(this.config.Emar_ResidentDemographic_InsertUpdatePatientOnLeave, leaveObj)
      .subscribe(res => {
        this.result.emit(res);
      }, error => {
        this.result.emit(-1);
      });
  }
  closeModel() {
    this.result.emit(2);
  }
}
