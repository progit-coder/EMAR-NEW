import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Company } from '../../../models/company.model';

@Component({
  selector: 'app-companydrop',
  templateUrl: './companydrop.component.html',
  styleUrls: ['./companydrop.component.css']
})
export class CompanydropComponent implements OnInit {
  public companyMaster: any[];
  errorMessage: string;
  public companyselected: number;
  companydrop: number = 1;
  @Output()
  oncompanyselect = new EventEmitter<number>();
  constructor(private dataservice: DataService, private config: APIConfiguration) { }

  ngOnInit() {
    this.getCompanyMaster();
    //this.change()
    //this.loading = false;
  }
  public changeCompany() {
    this.oncompanyselect.emit(this.companyselected);
  }
  getCompanyMaster() {
    this.dataservice.get<any[]>(this.config.Emar_CompanyMaster_GetAllActiveCompanyDrop)
      .subscribe(res => {
        this.companyMaster = res;
      },
        error => {this.errorMessage = <any>error;
        });
  }
}
