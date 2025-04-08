import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { HomeComponent } from "./home/home.component";
import { TodoComponent } from './todo/todo.component';
import { AccountService } from './_service/account.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavComponent, HomeComponent, TodoComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  http = inject(HttpClient)
  title = 'LinkUp';
  users: any;
  accountService = inject(AccountService);
  token: any;
  //constructor(private httpClient:HttpClient){}
  ngOnInit(): void {
    // this.http.get('http://localhost:5050/api/users').subscribe({
    //   next: response => this.users = response,
    //   error: error => console.log(error),
    //   complete: () => console.log('Request has Completed')
    // });

    this.token = this.accountService.setCurrentUser(); // Ensure user is loaded on app start


  }
}
