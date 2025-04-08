import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  cancelRegister = output<boolean>();
  model: any = {}

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.model.picture = event.target.files[0]; // Store the selected file object
    }
  }

  register() {
    console.log(this.model);
    const formData = new FormData();
    formData.append('username', this.model.username);
    formData.append('password', this.model.password);
    if (this.model.picture) {
      formData.append('picture', this.model.picture, this.model.picture.name); // Append the file
    }


    this.accountService.register(formData).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error => console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}