import { Component, Input } from '@angular/core';
import { Member } from '../../_models/member';
import { MemberService } from '../../_services/member.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss'],
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(private memberService: MemberService, private toastr: ToastrService) {}

  addLike(member: Member): void {
    this.memberService.addLike(member.username).subscribe(() =>
      this.toastr.success(`You have liked ${member.knownAs}`));
  }
}
