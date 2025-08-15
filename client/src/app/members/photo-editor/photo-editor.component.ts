import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment.development';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { Photo } from '../../_models/Photo';
import { CommonModule, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, NgStyle, NgClass, FileUploadModule],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
  member=input.required<Member>();
  // প্যারেন্ট কম্পোনেন্ট থেকে মেম্বার ডেটা গ্রহণ করার জন্য একটি ইনপুট সিগন্যাল।

  // প্যারেন্টকে মেম্বারের পরিবর্তনের বিষয়ে জানানোর জন্য একটি আউটপুট প্রপার্টি।
  memberChange = output<Member>();

  // JWT টোকেন পাওয়ার জন্য AccountService ইনজেক্ট করা হয়েছে।
  private accountService = inject(AccountService);
  // পরিবেশ ফাইল থেকে API-এর বেস URL নেওয়া হয়েছে।
  private baseUrl = environment.apiUrl;

  uploader?: FileUploader | undefined;
  hasBaseDropZoneOver = false;

  ngOnInit(): void {

    this.initializeUploader();
  }


  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }



  initializeUploader() {
    this.uploader = new FileUploader({
  
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      // রেসপন্স থেকে JSON ডেটা পার্স করে নতুন ফটো অবজেক্ট নেওয়া হয়েছে।
      const photo = JSON.parse(response) as Photo;

      // মেম্বার সিগন্যালের বর্তমান মান থেকে একটি নতুন অবজেক্ট তৈরি করা হয়েছে।
      const updatedMember = { ...this.member() };
      
      // নতুন ছবিটি আপডেট করা মেম্বারের ফটো অ্যারেতে যোগ করা হয়েছে।
      updatedMember.photos.push(photo);

      // মেম্বারের পরিবর্তনটি প্যারেন্ট কম্পোনেন্টকে জানানোর জন্য ইভেন্টটি নির্গত করা হয়েছে।
      this.memberChange.emit(updatedMember);
    };
  }
}
