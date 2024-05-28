import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MemberService } from '../_services/member.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
})
export class LikesComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private memberService: MemberService) {
  }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  pageChanged(event: any): void {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
}
