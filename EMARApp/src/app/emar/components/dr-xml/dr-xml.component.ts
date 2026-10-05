import { Component, OnInit } from '@angular/core';
import { APIConfiguration } from 'src/app/models/app.constants';
import { DataService } from 'src/app/services/shared/dataservice.service';

@Component({
  selector: 'app-dr-xml',
  templateUrl: './dr-xml.component.html',
  styleUrls: ['./dr-xml.component.css']
})
export class DrXmlComponent implements OnInit {
  data:any ;

  constructor(
    private dataservice:DataService ,  private config: APIConfiguration,
  ) { }

  ngOnInit() {
  }
  getData(){
    this.dataservice.get<any>(this.config.Emar_Rcopia_PostDrFirstTest).subscribe(res => {
      this.data=res
    })
    

  }

}
