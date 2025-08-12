import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);


  getMembers() {
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

    const member = this.members().find(x => x.userName === username);


    if (member) {

      return of(member);
    }


    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }
  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {

        this.members.update(members => members.map(m => m.userName === member.userName ? member : m));
      })
    );
  }

}
