import { Component, OnInit, Input, Output, EventEmitter,ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-pharmacydiscard',
  templateUrl: './pharmacydiscard.component.html',
  styleUrls: ['./pharmacydiscard.component.css']
})
export class PharmacydiscardComponent implements OnInit {
  @Output() results: EventEmitter<any> = new EventEmitter();

  constructor(public activeDefaultModal: NgbActiveModal) { }

  ngOnInit() {
  }
  closeModel() {

    this.results.emit(2);
  }
  saveChanges()
  {

    this.results.emit(1);
  }

}
