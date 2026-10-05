import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-otherallergyalert',
  templateUrl: './otherallergyalert.component.html',
  styleUrls: ['./otherallergyalert.component.css']
})
export class OtherallergyalertComponent implements OnInit {

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
