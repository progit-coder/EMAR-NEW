
import { tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { PersistanceService } from './persistance.service';
import { Router } from '@angular/router';

@Injectable()
export class InterceptorService implements HttpInterceptor {

  constructor(private persistanceService: PersistanceService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // console.log(JSON.stringify(req));
    if (req.url == 'http://api.ipify.org/?format=json')
      return next.handle(req.clone());
    if (req.headers.get('No-Auth') == "True")
      return next.handle(req.clone());
    const token: string = this.persistanceService.get("userToken");
    let changedRequest = req;
    // HttpHeader object immutable - copy values
    const headerSettings: { [name: string]: string | string[]; } = {};

    for (const key of req.headers.keys()) {
      headerSettings[key] = req.headers.getAll(key);
    }
    if (req.headers.get('No-Auth') == "True") {
      const newHeader = new HttpHeaders(headerSettings);
      req = req.clone({ headers: newHeader });

      return next.handle(req).pipe(tap(
        success => { },
        error => {
          if (error.status === 401)
            this.persistanceService.clear();
        }
      ));
    }
    else if (token) {
      headerSettings['Authorization'] = 'Bearer ' + token;
      // if (!req.headers.has('Content-Type')) {
      //   headerSettings['Content-Type'] = 'application/json;charset=UTF-8';//'application/json';
      // }
      const newHeader = new HttpHeaders(headerSettings);
      req = req.clone({ headers: newHeader });

      return next.handle(req).pipe(tap(
        success => { },
        // error => {
        //   if (error.status === 401)
        //     this.persistanceService.clear();
        //   else if (error.status === 0 && error.statusText === "Unknown Error")
        //     console.log('Something wrong with service');
        // }
        (error: HttpErrorResponse) => {
          let errorMessage = '';
          if (error.error instanceof ErrorEvent) {
            // client-side error
            errorMessage = `Error: ${error.error.message}`;
          } else {
            // server-side error
            if (error.status === 401)
              this.persistanceService.clear();
            else
              // if (error.url.startsWith('http://localhost:51810')) {

              // }
              // else {
                // if (error.status === 500)
                //   this.router.navigate(['/home/error500']);
                if (error.status === 404)
                  //errorMessage = `Error Code: ${error.status}\nMessage: ${error.error}`;
                  this.router.navigate(['/home/error404']);
                else
                {
                   
                  errorMessage = `Error Code: ${error.status}\nMessage: ${error.error}`;
                  console.log(errorMessage);
                  this.router.navigateByUrl('/login')
                }
                  //errorMessage = `Error Code: ${error.status}\nMessage: ${error.error}`;
                  //this.router.navigate(['/home/error500']);
              // }
          }
          //window.alert(errorMessage);
          //return throwError(errorMessage);
        })
      );
    }
    else {
      this.router.navigateByUrl('/login');
    }
  }
}

@Injectable()
export class NoCacheHeadersInterceptor implements HttpInterceptor {
intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authReq = req.clone({
      // Prevent caching in IE, in particular IE11.
      // See: https://support.microsoft.com/en-us/help/234067/how-to-prevent-caching-in-internet-explorer
      setHeaders: {
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache'
      }
    });
    return next.handle(authReq);
  }
}
