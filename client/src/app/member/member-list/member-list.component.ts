import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from '../../members/member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  members: Member[] = [];

  ngOnInit(): void {
    if (this.memberService.members().length === 0) {
      this.loadMembers();
    }
  }

  loadMembers() {
    this.memberService.getMembers().subscribe(
      {
        next: members => this.members = members

      }

    )
  }

}
