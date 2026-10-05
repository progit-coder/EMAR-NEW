import { Component, OnInit, SimpleChanges, Input, SimpleChange, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {

  records: Observable<any[]>;
  headers =[];
  @Input() AuditTable: any;
  @Input() DashboardData: any;
  p: number = 1;
  tableType = 1;
  gridColumns: any[];
  staticColumns: any[];
  dynamicColumns: any[];
  firstCreated: any;
  @Output()
  SelectedRow = new EventEmitter<string>();
  public indexValue: number;
  constructor(private dataservice: DataService, private config: APIConfiguration, private ng4LoadingSpinnerService: Ng4LoadingSpinnerService) {
  }
  ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
    if (changes.AuditTable && changes.AuditTable.currentValue != undefined) {
      this.getAuditData(this.AuditTable.tableName, this.AuditTable.recordId);
    }
    else if (changes.DashboardData && changes.DashboardData.currentValue != undefined) {
      this.getDashboardData(this.DashboardData.columns, this.DashboardData.rows, this.DashboardData.type);
    }
  }

  ngOnInit() {
  }
  getAuditData(tableName: string, recordId: number) {
    this.ng4LoadingSpinnerService.show();
    this.tableType = 1;
    this.dataservice.get<any>(this.config.Emar_AuditTables_GetAdmitDateByID + tableName + "/" + recordId)
      .subscribe(res => {
        this.ng4LoadingSpinnerService.hide();
        this.indexValue = res.Records.findIndex(r => r.ColumnName == null && r.OldValue == null && r.NewValue == null);
        this.firstCreated = res.Records[this.indexValue];
        this.headers = [
          { item_id: 'ColumnName', item_text: 'Column Name' },
          { item_id: 'OldValue', item_text: 'Old Value' },
          { item_id: 'NewValue', item_text: 'New Value' },
          { item_id: 'UpdatedBy', item_text: 'Updated By ' },
          { item_id: 'UpdatedOn', item_text: 'Updated On' },
       
        ];
        //this.headers = ["ColumnName", "OldValue", "NewValue", "UpdatedBy", "UpdatedOn"];
        this.records = res.Records.filter(r => r.ColumnName != null && r.OldValue != null && r.NewValue != null);
      },
        error => {
          console.log(error.message);
          this.ng4LoadingSpinnerService.hide();
        });
  }
  getDashboardData(columns: any, rows: any, type: number) {
    this.tableType = type;
    this.gridColumns = columns;
    this.records = rows;
    this.staticColumns = this.gridColumns.filter(e => e.columnType === 'static');
    this.dynamicColumns = this.gridColumns.filter(e => e.columnType === 'dynamic');
  }
  onRowSelect(PatientName: any) {
    this.SelectedRow.emit(PatientName);
  }
}
