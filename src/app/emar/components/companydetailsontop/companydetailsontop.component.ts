import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SharedService } from '../../../services/shared/shared.service';
import { Company } from '../../../models/company.model';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';

@Component({
  selector: 'app-companydetailsontop',
  templateUrl: './companydetailsontop.component.html',
  styleUrls: ['./companydetailsontop.component.css']
})
export class CompanydetailsontopComponent implements OnInit {
  public companyselected: number;
  public companyObj: Company;
  private url: string;
  @Output() oncompanyselect = new EventEmitter<Company>();
  newcompanies: Company[];
  count: number;
  errorMessage: string;
  constructor(private dataservice: DataService, private config: APIConfiguration, public sharedService: SharedService) {
  }

  public sendvalue(): void {
    this.getCompanyDetailsByID(this.companyselected);
    
  }

  ngOnInit() {
    this.sharedService.currentCompany.subscribe(
      companies => {this.newcompanies = companies;
        this.getCompanyDetailsByID(this.companyselected);
      });
  }
  comapnySaved(e: string) {    
    console.log("saved");
  }
  getCompanyDetailsByID(ID: number) {
    this.url = this.config.Emar_CompanyMaster_GetCompanyDetailsByID + "/" + ID;
    this.dataservice.get<Company>(this.url)
      .subscribe(res => {this.fetchData(res);this.oncompanyselect.emit(this.companyObj);}, error => this.errorMessage = <any>error);
  }
  fetchData(res: Company) {
    
    this.companyObj = res;
  }
}
