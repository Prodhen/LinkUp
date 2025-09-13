import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable, inject, model, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/Photo';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { PaginatedResult } from '../_models/pagination';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResults = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));


  resetUserParams() {

    const newParams = new UserParams(this.user);
    this.userParams.set(newParams);
    return newParams;

  }


  getMembers() {
    console.log(this.userParams(), 'userParams in getMembers');
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));
    console.log(response);
    if (response) {
      console.log(response, 'from cache');
      return setPaginatedResponse(response,this.paginatedResults);
    }
    const currentParams = this.userParams();

    let params = setPaginationHeaders(currentParams.pageNumber, currentParams.pageSize);

    params = params.append('minAge', currentParams.minAge);
    params = params.append('maxAge', currentParams.maxAge);
    params = params.append('gender', currentParams.gender);
    params = params.append('orderBy', currentParams.orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).subscribe({
      next: response => {
        console.log(response, 'from backend');
        setPaginatedResponse(response,this.paginatedResults);
        this.memberCache.set(Object.values(this.userParams()).join('-'), response);
      }
    });


  }
  // private setPaginatedResponse(response: HttpResponse<Member[]>) {
  //   this.paginatedResults.set({
  //     items: response.body as Member[], // Type assertion
  //     pagination: JSON.parse(response.headers.get('Pagination')!)
  //   });
  // }
  // private setPaginationHeaders(pageNumber: number, pageSize: number) {
  //   let params = new HttpParams();

  //   if (pageNumber && pageSize) {
  //     params = params.append('pageNumber', pageNumber);
  //     params = params.append('pageSize', pageSize);
  //   }

  //   return params;
  // }

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
        const current = this.paginatedResults();
        if (current?.items) {
          // Update the member in the current paginated items
          const updatedItems = current.items.map(m =>
            m.userName === member.userName ? { ...m, ...member } : m
          );
          this.paginatedResults.set({ ...current, items: updatedItems });
        }
      })
    );
  }
  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
      tap(() => {
        const current = this.paginatedResults();
        if (current?.items) {
          const updatedItems = current.items.map(m => {
            if (m.photos.some(p => p.id === photo.id)) {
              return {
                ...m,
                photoUrl: photo.url,
                photos: m.photos.map(p =>
                  p.id === photo.id ? { ...p, isMain: true } : { ...p, isMain: false }
                )
              };
            }
            return m;
          });
          this.paginatedResults.set({ ...current, items: updatedItems });
        }
      })
    );
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId).pipe(
      tap(() => {
        const current = this.paginatedResults();
        if (current?.items) {
          const updatedItems = current.items.map(m => ({
            ...m,
            photos: m.photos.filter(p => p.id !== photoId)
          }));
          this.paginatedResults.set({ ...current, items: updatedItems });
        }
      })
    );
  }

}
