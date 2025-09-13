import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from '../../members/member-card/member-card.component';
import { NgFor, NgIf } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/userParams';
import { AccountService } from '../../_services/account.service';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, NgFor, PaginationModule, NgIf, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private accountService = inject(AccountService);
  memberService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' }
  ];

  ngOnInit(): void {
    if (!this.memberService.paginatedResults()) {
      this.loadMembers();
    }
  }

  loadMembers() {
    console.log("Loading members with params:", this.memberService.userParams());
    this.memberService.getMembers();
  }
  filterParams = {
    gender: this.memberService.userParams().gender,
    minAge: this.memberService.userParams().minAge,
    maxAge: this.memberService.userParams().maxAge,
    pageNumber: this.memberService.userParams().pageNumber,
    pageSize: this.memberService.userParams().pageSize,
    orderBy: this.memberService.userParams().orderBy
  };
  applyFilters() {


    // Merge only filterable fields; required fields stay intact
    const currentParams = this.memberService.userParams(); // Get the current value of the signal
    this.memberService.userParams.set({ ...currentParams }); // Create a new object with current 
    console.log(currentParams);
    this.loadMembers();
  }

  resetFilters() {
    this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.memberService.userParams().pageNumber !== event.page) {
      this.memberService.userParams().pageNumber = event.page;
      this.loadMembers();
    }
  }

}
