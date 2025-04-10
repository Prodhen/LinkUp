import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { AccountService } from '../_service/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../../environment/environment';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  private accountService = inject(AccountService);
  model: any = {};
  loggedIn = false;
  private apiUrl = environment.apiUrl;
  currentUser = this.accountService.currentUser.asReadonly();
  ngOnInit(): void {
    this.accountService.setCurrentUser();
  }
  login() {

    console.log(this.model);
    this.accountService.login(this.model).subscribe(
      {
        next: response => {
          console.log(response);
          this.loggedIn = true;
        },
        error: error => console.log(error)
      }
    )
  }

  setCurrentUser() {
    console.log("setCurrentUser");
    const userString = localStorage.getItem('user');
    if (userString) {
      try {
        const user: User = JSON.parse(userString);
        this.accountService.currentUser.set(user);
        console.log("afterSet", user);
      } catch (error) {
        console.error('Error parsing user from localStorage:', error);
        localStorage.removeItem('user');
        this.accountService.currentUser.set(null);
      }
    }
  }
  getUserImageUrl(): string {
    const user = this.currentUser();
    console.log(user);
    return user?.picture
      ? `${this.apiUrl}${user.picture}`//have to change
      : 'assets/default-profile.png';
  }
  logout() {
    this.accountService.logout();
  }

}
