import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, of, take } from 'rxjs';

import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationParams } from './pagination-helper';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache: Map<string, PaginatedResult<Member[]>> = new Map();

  userParams: UserParams | undefined;
  user: User | undefined;

  constructor(private http: HttpClient, private accountService: AccountService) {

    this.accountService.currentUser$.subscribe({
      next: user => {
        if (user && user !== this.user) {
          this.userParams = new UserParams(user);
          this.memberCache = new Map();
          this.user = user;
        }
      },
    });
  }

  getUserParams(): UserParams | undefined {
    return this.userParams;
  }

  setUserParams(params: UserParams): void {
    this.userParams = params;
  }

  resetUserParams(): UserParams | undefined {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
  }

  getMembers(userParams: UserParams): Observable<PaginatedResult<Member[]>> {
    const cachedResult = this.memberCache.get(userParams.getCacheKey());

    if (cachedResult) return of(cachedResult);

    let params = getPaginationParams(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http).pipe(
      map(response => {
        this.memberCache.set(userParams.getCacheKey(), response);
        return response;
      }),
    );
  }

  getMember(username: string): Observable<Member> {
    const member = [...this.memberCache.values()]
      .reduce<Member[]>((arr, el) => arr.concat(el.result!), [])
      .find(m => m.username === username);

    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member): Observable<void> {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        if (index !== -1) {
          this.members[index] = { ...this.members[index], ...member };
        }
      }),
    );
  }

  setMainPhoto(photoId: number): Observable<void> {
    return this.http.put<void>(this.baseUrl + 'users/photos/set-main/' + photoId, {});
  }

  deletePhoto(photoId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'users/photos/' + photoId);
  }

  addLike(username: string): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number): Observable<PaginatedResult<Member[]>> {
    let params = getPaginationParams(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return getPaginatedResult<Member[]>(this.baseUrl + 'likes', params, this.http);
  }
}
