import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/Photo';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);
  paginatedResults = signal<PaginatedResult<Member[]> | null>(null);


  getMembers(pageNumber?: number, pageSize?: number) {

    let params = new HttpParams();
    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }
    return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).subscribe({
      next: response => {
        this.paginatedResults.set({
          items: response.body as Member[], // Type assertion
          pagination: JSON.parse(response.headers.get('Pagination')!)
        });
      }
    });


    if (this.members().length > 0) {

      return of(this.members());
    }
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      tap(members => {

        this.members.set(members);
      })
    );

  }

  getMember(username: string) {


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
