import { Routes } from '@angular/router';
import { TodoComponent } from './todo/todo.component';


export const routes: Routes = [
    { path: 'todo', component: TodoComponent },
    { path: '', redirectTo: '/home', pathMatch: 'full' }, // Optional: Redirect the root path
    // ... other routes for your application (e.g., home, register, etc.)
    // { path: '**', component: NotFoundComponent }        // Optional: Catch-all for invalid routes
];
