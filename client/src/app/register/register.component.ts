import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);
  cancelRegister = output<boolean>();
 
  model: any = { }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.model.picture = event.target.files[0]; // Store the selected file object
    }
  }

  register() {
    if (!this.model.username || !this.model.password) {
      this.toastr.error('Username and password are required', 'Validation Error');
      return;
    }

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
        this.toastr.success('Registration successful', 'Success');
        this.cancel();
      },
      error: (err) => {
        console.log(err);

        if (err.error?.data && Array.isArray(err.error.data)) {
          for (let msg of err.error.data) {
            this.toastr.error(msg, 'Validation Error');
          }
        } else if (err.error?.message) {
          this.toastr.error(err.error.message, 'Error');
        } else {
          this.toastr.error('Something went wrong', 'Error');
        }
      }
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}