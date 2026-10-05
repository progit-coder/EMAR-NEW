import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({
  name: 'customdate'
})
export class CustomdatePipe extends DatePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    return super.transform(value,"MM/dd/yyyy");
  }
  dateWithTime(value: any, args?: any): any {
    return super.transform(value,"MM/dd/yyyy HH:mm:ss.sss");
  }
  dateWithTimeFormat(value: any, args?: any): any {
    return super.transform(value,"MM/dd/yyyy hh:mm:ss a");
  }
  // dateForApiCall(value: any, args?: any): any {
  //   return super.transform(value,"MM-dd-yyyy");
  // }
  dateFormat(value: any, args?: any): any {
    return super.transform(value,"y-MM-dd");
  }
  transformISODate(value: any, args?: any): any {
    return super.transform(value,"yyyy-MM-dd");
  }
  get12HourTime(value: any, args?: any): any {
    return super.transform(value,"hh:mm a");
  }
  get24HourTime(value: any, args?: any): any {
    return super.transform(value,"HH:mm");
  }
  dateWithTimeFormatForApi(value: any, args?: any): any {
    return super.transform(value,"MM-dd-yyyy hh,mm,ss a");
  }
  dateWithOutTimeFormatForApi(value: any, args?: any): any {
    return super.transform(value,"MM-dd-yyyy");
  }
  get24HourDateTime(value: any, args?: any): any {
    return super.transform(value,"MM/dd/yyyy HH:mm");
  }
}