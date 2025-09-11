import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from '../../members/member-card/member-card.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, NgFor],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  //members: Member[] = [];
  pageNumber = 1;
  pageSize = 5;
  ngOnInit(): void {
    if (!this.memberService.paginatedResults()) {
      this.loadMembers();
    }
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize);
  }

}
