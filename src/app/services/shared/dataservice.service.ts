import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APIConfiguration } from '../../models/app.constants';
import 'rxjs/Rx';
@Injectable()
export class DataService {
  orderID: any;
  template = '<div class="loading" style="z-index: 1000000000"><img class="loading-image" width="20" height="auto" src="./assets/images/loading.gif"/></div>';
  constructor(private http: HttpClient, private config: APIConfiguration) {

  }
  search(url: string) {
    url = this.config.serverWithApiUrl + url;
    return this.http.get(url);//.map((res: Response) => res.json());
    //.map((r: Response) => { return (r.json().length != 0 ? r.json() : [{ "ClientId": 0, "ClientName": "No Record Found" }]) as any[] });
  }
  getIP<T>(url: string): Observable<T>{
    return this.http.get<T>(url);
  }
  getFile(url: string): Observable<any> {

    url = this.config.serverWithApiUrl + url;
    return this.http.get(url, { responseType: 'blob' as 'json' });
  }
  getFile1(url: string, body: any): Observable<any> {
    url = this.config.serverWithApiUrl + url;
   // let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json;charset=UTF-8' });
    return this.http.post(url, body , { responseType: 'blob' as 'json' });

  }
  get<T>(url: string): Observable<T> {
    url = this.config.serverWithApiUrl + url;
    return this.http.get<T>(url);
  }
  getNoAuth<T>(url: string): Observable<T> {
    url = this.config.serverWithApiUrl + url;
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get<T>(url, {headers: reqHeader});
  }
  post(url: string, body: any): Observable<any> {
    url = this.config.serverWithApiUrl + url;
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json;charset=UTF-8' });
    return this.http.post(url, body, { headers: httpHeaders });

  }
  postFormData(url: string, body: any, fd: FormData): Observable<any> {
    url = this.config.serverWithApiUrl + url;
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json;charset=UTF-8' });//,'Accept':'application/json'});
    return this.http.post(url, fd, { params: { 'body': JSON.stringify(body) } })//, { headers: httpHeaders });

  }
  // post<T>(url: string, body: any): Observable<any> {
  //   url = this.config.serverWithApiUrl + url;
  //   let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded;charset=UTF-8' });//,'Accept':'application/json'});
  //   //httpHeaders.set('Content-Type','application/json');
  //   //     let headers = new Headers();
  //   //     headers.set('Content-Type', 'application/json');
  //   // headers.set('Accept', 'application/json');
  //   // let options = new RequestOptions({ headers: headers })
  //   //let options=new RequestOptions({headers:httpHeaders});
  //   // httpHeaders.append('Content-Type','application/x-www-form-urlencoded;charset=UTF-8');
  //   // let options=new ();
  //   return this.http.post<T>(url, body, { headers: httpHeaders });
  // }

  put<T>(url: string, body: string): Observable<T> {
    url = this.config.serverWithApiUrl + url;
    return this.http.put<T>(url, body);
  }

  delete<T>(url: string): Observable<T> {
    url = this.config.serverWithApiUrl + url;
    return this.http.delete<T>(url);
  }
  postLogin(url: string, data: string) {
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded', 'No-Auth': 'True' });
    return this.http.post(url + '/token', data, { headers: reqHeader });
  }
  getIpAddress(url:string)
  {
    return this.http.get<string>(url);
  }
  getBiometric<T>(url: string): Observable<T> {
    url = this.config.biometricServerIp + url;
    return this.http.get<T>(url);
  }
  postBiometric(url: string, body: any): Observable<any> {
    url = this.config.biometricServerIp + url;
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json;charset=UTF-8' });
    return this.http.post(url, body, { headers: httpHeaders });
  }
  getReport(url: string, body: any): Observable<any> {

    url = this.config.serverWithApiUrl + url;
    return this.http.post(url, body, { responseType: 'blob' as 'json' });
  }
}
