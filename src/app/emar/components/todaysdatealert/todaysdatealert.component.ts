import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-todaysdatealert',
  templateUrl: './todaysdatealert.component.html',
  styleUrls: ['./todaysdatealert.component.css']
})
export class TodaysdatealertComponent implements OnInit {

  @Output() result: EventEmitter<any> = new EventEmitter();
  constructor(public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
   

  }
  closeModel() {
     
    this.result.emit(2);
  }
  saveChanges()
  {
     
    this.result.emit(1);
  }
}
