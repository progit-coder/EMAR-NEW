import { Injectable, ErrorHandler } from '@angular/core';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { APIConfiguration } from 'src/app/models/app.constants';
import { DataService } from './dataservice.service';
import { HttpErrorResponse } from '@angular/common/http';
//import { AlertService } from 'src/app/_services/index';

@Injectable({
  providedIn: 'root'
})
export class AppGlobalErrorhandlerService implements ErrorHandler {

  constructor(private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private dataservice: DataService, private config: APIConfiguration)
  //, private alertService: AlertService)
  { }

  handleError(error: Error | HttpErrorResponse): void {
    debugger;
    if (error instanceof HttpErrorResponse) {
      // Server or connection error happened
      // if (!navigator.onLine) {
      //   // Handle offline error
      // } else {
      //   // Handle Http Error (error.status === 403, 404...)
      // }
      let msg="Error Url: "+error.url+" Error Message: "+error.message.substring(0, 100).replace(/[^\w\s]/gi, '');
      let url = this.config.Emar_AngularLog_LogError + msg;
      this.dataservice.get<any[]>(url)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
        });

    } else {
      // Handle Client Error (Angular Error, ReferenceError...)     
      //input.replace(/\W/g, '')
      let url = this.config.Emar_AngularLog_LogError + error.message.substring(0, 100).replace(/[^\w\s]/gi, '');
      this.dataservice.get<any[]>(url)
        .subscribe(res => {
          this.ng4LoadingSpinnerService.hide();
        }, error => {
          this.ng4LoadingSpinnerService.hide();
        });
      console.error(error.message);
    }

    //this.alertService.error(error.message);
    this.ng4LoadingSpinnerService.hide();
  }

}
