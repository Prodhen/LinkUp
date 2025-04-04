import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule],
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
        },
        error: error => console.log(error)
      }
    )
  }

}
