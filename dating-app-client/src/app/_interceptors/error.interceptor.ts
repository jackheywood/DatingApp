import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                this.handle400ValidationError(error.error.errors);
              } else {
                this.handle400Error(error);
              }
              break;
            case 401:
              this.handle401Error(error);
              break;
            case 404:
              this.handle404Error();
              break;
            case 500:
              this.handle500Error(error);
              break;
            default:
              this.handleUnexpectedError(error);
              break;
          }
        }
        throw error;
      }),
    );
  }

  private handle400ValidationError(errors: Record<string, any>): void {
    const modelStateErrors = [];
    for (const key in errors) {
      if (errors[key]) {
        modelStateErrors.push(errors[key]);
      }
    }
    throw modelStateErrors.flat();
  }

  private handle400Error(error: HttpErrorResponse): void {
    this.toastr.error(error.error, error.status.toString());
  }

  private handle401Error(error: HttpErrorResponse): void {
    this.toastr.error('Unauthorised', error.status.toString());
  }

  private handle404Error(): void {
    this.router.navigateByUrl('/not-found');
  }

  private handle500Error(error: HttpErrorResponse): void {
    const navigationExtras: NavigationExtras = { state: { error: error.error } };
    this.router.navigateByUrl('/server-error', navigationExtras);
  }

  private handleUnexpectedError(error: HttpErrorResponse): void {
    this.toastr.error('Something unexpected went wrong');
    console.log(error);
  }
}
