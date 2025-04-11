import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { Response } from '../_models/response';
import { map } from 'rxjs';
import { environment } from '../../environment/environment';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);
  baseUrl = environment.apiUrl + '/api/';
  currentUser = signal<User | null>(null);
  get authToken(): string | null {
    return this.currentUser()?.token || null;
  }
  login(model: any) {
    return this.http.post<Response<User>>(this.baseUrl + 'account/login', model).pipe(
      map(response => {
        const user = response.data;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    )
  }

  register(formData: FormData) {
    return this.http.post<Response<User>>(this.baseUrl + 'account/register', formData).pipe(
      map(response => {
        const user = response.data;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
          console.log(user);
        }
        return user;
      })
    );
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (userString) {
      try {
        const user: User = JSON.parse(userString);
        this.currentUser.set(user);
      } catch (error) {
        console.error('Error parsing user from localStorage:', error);
        localStorage.removeItem('user'); // Clear invalid data
        this.currentUser.set(null);
      }
    }
  }



  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
