import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { AccountService } from '../_service/account.service';
import { NgIf } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

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
  logout() {
    this.loggedIn = false;
  }

}
