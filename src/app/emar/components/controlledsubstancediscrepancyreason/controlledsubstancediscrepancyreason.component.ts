import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-controlledsubstancediscrepancyreason',
  templateUrl: './controlledsubstancediscrepancyreason.component.html',
  styleUrls: ['./controlledsubstancediscrepancyreason.component.css']
})
export class ControlledsubstancediscrepancyreasonComponent implements OnInit {
  @Input() orderDetails: any;
  @Output() reason: EventEmitter<any> = new EventEmitter();
  residentDrugData: any;
  myform: FormGroup;

  constructor() { }

  ngOnInit() {
    this.residentDrugData = this.orderDetails.ResidentName + '  ('+ this.orderDetails.DrugName+')';

    this.myform = new FormGroup({
      reason: new FormControl('')
    });
  }
  addReason()
  {
    this.reason.emit(this.myform.value.reason);
  }
  cancelClick() {
    this.residentDrugData = '';
    this.reason.emit('');
  }
}
