// import { HttpInterceptorFn } from '@angular/common/http';
// import { catchError, throwError } from 'rxjs';

// export const interceptorInterceptor: HttpInterceptorFn = (req, next) => {
//   let authReq = req;
//     const token = localStorage.getItem("tokNum");

    
//     if (token != null && token != undefined && token != '') {
//       authReq = req.clone({ headers: req.headers.set('Authorization', 'Bearer ' + token) });
//     }
//     return next.handle(authReq).pipe(catchError(err => {
//       if ((err && err.status === 401)||err.status===0) {
//         localStorage.clear();
//         err.error = { Message: "", status: 0 };
//         err.error.status = 401;
//         location.reload();
//       }
//       const error = err.error.message || err.statusText;
//       return throwError(err);
//     }));
//   // return next(req);
// };
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class interceptorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let authReq = req;
    const token = localStorage.getItem('tokNum');

    if (token !== null && token !== undefined && token !== '') {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
      });
    }

    return next.handle(authReq).pipe(
      catchError((err) => {
        if ((err && err.status === 401) || err.status === 0) {
          localStorage.clear();
          err.error = { Message: '', status: 0 };
          err.error.status = 401;
          location.reload();
        }
        const error = err.error.message || err.statusText;
        return throwError(err);
      })
    );
  }
}
