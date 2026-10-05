import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../services/shared/shared.service';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Screens, Activity } from '../../../models/useractivity.model';
import { Router } from '@angular/router';
import { AlertService } from '../../../_services/index';
import { PersistanceService } from '../../../services/shared/persistance.service';
import { Observable, Subject } from 'rxjs';
@Component({
  selector: 'app-drfirstfiles',
  templateUrl: './drfirstfiles.component.html',
  styleUrls: ['./drfirstfiles.component.css']
})
export class DrfirstfilesComponent implements OnInit {
  p: number = 1;
  searchText:string="";
  gridPagination = this.config.gridPagination;
  public template;
  public userId:number;
  public drFirstFilesGrid:any[]=[];
  pageConfig = {};
  constructor(private dataservice: DataService, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService, private alertService: AlertService, private config: APIConfiguration,
    public sharedService: SharedService,private persistanceService: PersistanceService, private router: Router) { }

  ngOnInit() {
    this.pageConfig = this.persistanceService.getPermissionsByScreen("DrFirstFiles");
    if (this.pageConfig != undefined) {
      if (this.pageConfig["AccessRead"] == 0) {
        this.persistanceService.redirectToHomePage();
      }
      else {
    this.template = this.dataservice.template;
    this.getDrFirstFiles();
    this.userId = this.persistanceService.get(this.config.loggedInUserKey);
    
  }
}
else
this.persistanceService.redirectToHomePage();
  }
  userActivity()
  {    
    this.sharedService.insertUserActivityDetails(Screens.DrFirstFiles,Activity.View,'')
    .subscribe(res=>{},error=>{
      this.alertService.error(error.message);
    });
  }
  getDrFirstFiles() {
    this.ng4LoadingSpinnerService.show();
    this.dataservice.get<any[]>(this.config.Emar_Orders_GetDrFirstFilesData)
      .subscribe(res => {
        this.drFirstFilesGrid = res;
        this.ng4LoadingSpinnerService.hide();
      },
        error => {
          this.alertService.error(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  downLoadFile(fileId) {

    this.ng4LoadingSpinnerService.show();
    this.dataservice.getFile(this.config.Emar_DrFirstIntegration_DownloadXML + fileId)
      .subscribe((res: any) => {
        var a = document.createElement("a");
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        var file = new Blob([res], { type: 'application/xml' });
        var url = window.URL.createObjectURL(file);
        a.href = url;
        var x: Date = new Date();
        var link: string = "DrFirstPrescription";
        a.download = link;//.toLocaleLowerCase();
        a.click();
        this.ng4LoadingSpinnerService.hide();
      }, error => {
        this.alertService.error(error.message);
        this.ng4LoadingSpinnerService.hide();
      });

  }
}
