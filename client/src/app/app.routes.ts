import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberDetailsComponent } from './member/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

export const routes: Routes = [
    {path: '',component:HomeComponent},
    {path: 'members',component:MemberListComponent},
    {path: 'members/:id',component:MemberDetailsComponent},
    {path: 'lists',component:ListsComponent},
    {path: 'messages',component:MessagesComponent},
    {path: '**',component:HomeComponent,pathMatch: 'full'},
];
