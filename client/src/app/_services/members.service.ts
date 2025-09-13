import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable, inject, model, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/Photo';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  paginatedResults = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();


  getMembers(userParams: UserParams) {
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response) {
      this.paginatedResults.set(response);
      return of(response);
    }

    let params = this.setPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).subscribe({
      next: response => {
        this.setPaginatedResponse(response);
        this.memberCache.set(Object.values(userParams).join('-'), response);
      }
    });


  }
  private setPaginatedResponse(response: HttpResponse<Member[]>) {
    this.paginatedResults.set({
      items: response.body as Member[], // Type assertion
      pagination: JSON.parse(response.headers.get('Pagination')!)
    });
  }
  private setPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }

    return params;
  }

  getMember(username: string) {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [] as Member[])
      .find((m: Member) => m.userName === username);
    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }
  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {

        this.members.update(members => members.map(m => m.userName === member.userName ? member : m));
      })
    );
  }
  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
      tap(() => {
        this.members.update(members => members.map(m => {
          if (m.photos.includes(photo)) {
            m.photoUrl = photo.url
          }
          return m;
        }))
      })
    )
  }

  deletePhoto(photoId: number) {
    // return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
    //   tap(() => {
    //     this.members.update(members => members.map(m => {
    //       if (m.photos.includes(photo)) {
    //         m.photos = m.photos.filter(x => x.id !== photo.id)
    //       }
    //       return m
    //     }))
    //   })
    // )
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

}
