import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from '../../members/member-card/member-card.component';
import { NgFor, NgIf } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/userParams';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, NgFor, PaginationModule, NgIf],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private accountService = inject(AccountService);
  memberService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());



  ngOnInit(): void {
    if (!this.memberService.paginatedResults()) {
      this.loadMembers();
    }
  }

  loadMembers() {
    this.memberService.getMembers(this.userParams);
  }
  pageChanged(event: any) {
    if (this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }

}
