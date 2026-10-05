import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ekitmedsqtyupdatesdiscard',
  templateUrl: './ekitmedsqtyupdatesdiscard.component.html',
  styleUrls: ['./ekitmedsqtyupdatesdiscard.component.css']
})
export class EkitmedsqtyupdatesdiscardComponent implements OnInit {

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
