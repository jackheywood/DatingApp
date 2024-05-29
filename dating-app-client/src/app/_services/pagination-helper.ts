import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

export function getPaginationParams(pageNumber: number, pageSize: number): HttpParams {
  let params = new HttpParams();

  params = params.append('pageNumber', pageNumber);
  params = params.append('pageSize', pageSize);

  return params;
}

export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient): Observable<PaginatedResult<T>> {
  const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
  return http.get<T>(url, { observe: 'response', params }).pipe(
    map(response => {
      if (response.body) {
        paginatedResult.result = response.body;
      }
      const pagination = response.headers.get('Pagination');
      if (pagination) {
        paginatedResult.pagination = JSON.parse(pagination);
      }
      return paginatedResult;
    }),
  );
}
