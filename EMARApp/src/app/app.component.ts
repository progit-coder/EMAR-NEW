import { Component, Input } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { HeaderComponent } from './components/header/header.component'
import { PersistanceService } from './services/shared/persistance.service';
import { AlertService } from './_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { Observable } from 'rxjs';
import * as $ from 'jquery';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],

})
export class AppComponent {
  // title = 'app';
  // isUserLoggedIn: any;
  // isUserToken = false;
  constructor(){//public alertService: AlertService, private persistanceService: PersistanceService, public ng4LoadingSpinnerService: Ng4LoadingSpinnerService) {
  }  
  ngOnInit() {

    // this.persistanceService.userLoggedIn.subscribe(res => this.isUserLoggedIn = res);

    // if (JSON.parse(localStorage.getItem("userToken")) != null) {
    //   this.isUserToken = true;
    // }
    // else
    // {
    //   this.isUserLoggedIn = false;
    //   this.isUserToken = false;
    // }


  }

  // startLoadingSpinner() {
  //   this.ng4LoadingSpinnerService.show();
  //   setTimeout(function () {
  //     this.ng4LoadingSpinnerService.hide();
  //   }.bind(this), 4000);
  // }
}
