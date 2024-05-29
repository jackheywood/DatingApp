import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationParams } from './pagination-helper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getMessages(container: string, pageNumber: number, pageSize: number): Observable<PaginatedResult<Message[]>> {
    let params = getPaginationParams(pageNumber, pageSize);
    params = params.append('container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  sendMessage(username: string, content: string): Observable<Message> {
    return this.http.post<Message>(this.baseUrl + 'messages', { recipientUsername: username, content });
  }
}
