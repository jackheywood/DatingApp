import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs';

import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MemberService } from '../../_services/member.service';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  member: Member | undefined;
  user: User | null = null;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService,
    private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => this.user = user,
    });
  }

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  ngOnInit(): void {
    this.loadMember();
  }

  updateMember(): void {
    console.log(this.member);
    this.toastr.success('Profile updated successfully');
    this.editForm?.reset(this.member);
  }

  private loadMember(): void {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member,
    });
  }
}
