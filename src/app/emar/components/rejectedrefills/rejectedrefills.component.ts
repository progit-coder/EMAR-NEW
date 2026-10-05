import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-rejectedrefills',
  templateUrl: './rejectedrefills.component.html',
  styleUrls: ['./rejectedrefills.component.css']
})
export class RejectedrefillsComponent implements OnInit {
  @Output() result: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }
  cancelClick()
  {
    this.result.emit(0);
  }
}
