import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-commonmodal',
  templateUrl: './commonmodal.component.html',
  styleUrls: ['./commonmodal.component.css']
})
export class CommonmodalComponent implements OnInit {

  @Input() title = `Information`;
  @Output() Result: EventEmitter<any> = new EventEmitter();
  constructor(
    public activeModal: NgbActiveModal, private ngbModal: NgbModal
  ) {}

  ngOnInit() {
  }
  cancelClick()
  {
    if(this.title=="Enter Reason for incorrect count")
    {
      this.Result.emit('');
    }
  else if (this.ngbModal.hasOpenModals() == true)
  this.ngbModal.dismissAll();
  }
}
