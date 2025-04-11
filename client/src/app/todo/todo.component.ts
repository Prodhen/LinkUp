import { Component, OnInit, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AccountService } from '../_service/account.service';
import { environment } from '../../environment/environment';
import { ToastrService } from 'ngx-toastr';


interface TodoItem {
  id: number;
  title: string;
  description?: string | null;
  isCompleted?: boolean | null;
  dueDate?: string | null;
  completedDate?: Date | null;
  userId: number;
}

interface TodoItemAddDto {
  title: string;
  description?: string | null;
  dueDate?: Date | null;
}

interface TodoItemUpdateDto {
  id: number;
  title: string;
  description?: string | null;
  dueDate?: string | null;
  isCompleted?: boolean | null;
}

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css'],
})
export class TodoComponent implements OnInit {
  todos: TodoItem[] = [];
  newTodo: TodoItemAddDto = { title: '', description: null, dueDate: null };
  editingTodo: TodoItem | null = null;
  editedTodo: TodoItemUpdateDto = { id: 0, title: '', description: null, dueDate: null, isCompleted: false };
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);

  private apiUrl = environment.apiUrl + '/api/' + 'todos';
  private accountService = inject(AccountService);
  isAddTodoVisible: boolean = false;

  ngOnInit(): void {
    this.loadTodos();
  }
  toggleAddTodo() {
    this.isAddTodoVisible = !this.isAddTodoVisible;
  }

  loadTodos(): void {
    this.http.get<{ message: string | null; data: TodoItem[] }>(this.apiUrl, this.getAuthHeaders()).subscribe({
      next: (response) => {
        this.todos = response.data;
      },
      error: (error) => {
        console.error('Error loading todos:', error);
      },
    });
  }

  addTodo(): void {
    if (this.newTodo.title.trim()) {
      this.http.post<TodoItem>(`${this.apiUrl}/add`, this.newTodo, this.getAuthHeaders()).subscribe({
        next: (response) => {
          this.todos.unshift(response);
          this.newTodo = { title: '', description: null, dueDate: null };
          this.loadTodos();
          this.toastr.success('Todo added successfully!', 'Success');
          this.isAddTodoVisible = false;

        },
        error: (error) => {
          console.error('Error adding todo:', error);
        },
      });
    }
  }

  toggleComplete(todo: TodoItem): void {
    const updateDto: TodoItemUpdateDto = {
      id: todo.id,
      title: todo.title,
      description: todo.description,
      dueDate: todo.dueDate,
      isCompleted: !todo.isCompleted,
    };
    this.http.put(`${this.apiUrl}/Update`, updateDto, this.getAuthHeaders()).subscribe({
      next: () => {
        todo.isCompleted = !todo.isCompleted;
      },
      error: (error) => {
        console.error('Error updating todo:', error);
      },
    });
  }

  deleteTodo(todo: TodoItem): void {
    if (confirm(`Are you sure you want to delete "${todo.title}"?`)) {
      this.http.delete(`${this.apiUrl}/${todo.id}`, this.getAuthHeaders()).subscribe({
        next: () => {
          this.todos = this.todos.filter((t) => t.id !== todo.id);
        },
        error: (error) => {
          console.error('Error deleting todo:', error);
        },
      });
    }
  }

  editTodo(todo: TodoItem): void {
    this.editingTodo = { ...todo }; // Create a copy for editing
    this.editedTodo = {
      id: todo.id,
      title: todo.title,
      description: todo.description,
      dueDate: this.formatDateForInput(todo.dueDate),
      isCompleted: todo.isCompleted,
    };
  }


  private formatDateForInput(date: Date | string | null | undefined): string {
    if (!date) return '';


    if (typeof date === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(date)) {
      return date;
    }

    const jsDate = new Date(date);
    return jsDate.toISOString().split('T')[0];
  }



  saveEditedTodo(): void {
    if (this.editingTodo) {
      this.http.put(`${this.apiUrl}/Update`, this.editedTodo, this.getAuthHeaders()).subscribe({
        next: () => {
          const index = this.todos.findIndex((t) => t.id === this.editingTodo!.id);
          if (index !== -1) {
            this.todos[index] = { ...this.todos[index], ...this.editedTodo };
          }
          this.editingTodo = null;
          this.loadTodos();
        },
        error: (error) => {
          console.error('Error updating todo:', error);
        },
      });
    }
  }

  cancelEdit(): void {
    this.editingTodo = null;
  }

  private getAuthHeaders(): { headers: HttpHeaders } {
    let headers = new HttpHeaders();
    const token = this.accountService.authToken; // Get the token from the service
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    console.log("headers", headers);
    return { headers: headers };
  }


}
