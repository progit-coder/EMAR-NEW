import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { State } from '../../../models/common.model';
import {FormGroup,FormControl,Validator, Validators} from '@angular/forms';

@Component({
  selector: 'app-devicemaster',
  templateUrl: './devicemaster.component.html',
  styleUrls: ['./devicemaster.component.css']
})
export class DevicemasterComponent implements OnInit {
  myform:FormGroup;
  constructor() { }

  ngOnInit() {
    this.myform=new FormGroup({
      companynameValidate:new FormControl('',Validators.required),
      devicenameValidate:new FormControl('',Validators.required),
      deviceipValidate:new FormControl('',Validators.required),
      communicatemodeValidate:new FormControl('',Validators.required),
      portValidate:new FormControl('',Validators.required),
      communicationpasswordValidate:new FormControl('',Validators.required),
      devicetypeValidate:new FormControl('',Validators.required),
      
    });
  }
  getcompanyselected(item:number)
  {
alert('Selected vlalue : '+item);
  }

}
