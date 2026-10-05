import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
@Injectable()
export class ExceldownloadService {

  constructor() { }
//Download Excel Code
excelDownload(objArray,name)
{
  const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(objArray);
  const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
  const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    var a = document.createElement("a");
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    var blob = new Blob([excelBuffer], { type: 'EXCEL_TYPE' });
    var url= window.URL.createObjectURL(blob);
    a.href = url;
    var x:Date = new Date();
    var link:string =name + x.getMonth() +  "_" +  x.getDay() + '.csv';
    a.download = link.toLocaleLowerCase();
    a.click();
}
// convert Json to CSV data
ConvertToCSV(objArray) {
    
  var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
  var str = '';
  var row = "";

  for (var index in objArray[0]) {
      //Now convert each value to string and comma-separated
      row += index + ',';
  }
  row = row.slice(0, -1);
  //append Label row with line break
  str += row + '\r\n';

  for (var i = 0; i < array.length; i++) {
      var line = '';
      for (var index in array[i]) {
          if (line != '') line += ','

          line += array[i][index];
      }
      str += line + '\r\n';
  }
  return str;
}
}
