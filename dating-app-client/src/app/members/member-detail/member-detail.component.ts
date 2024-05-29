import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

import { Member } from '../../_models/member';
import { MemberService } from '../../_services/member.service';
import { TimeagoModule } from 'ngx-timeago';
import { ToastrService } from 'ngx-toastr';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss'],
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, MemberMessagesComponent],
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;

  member: Member = {} as Member;
  images: GalleryItem[] = [];
  messages: Message[] = [];
  activeTab?: TabDirective;

  constructor(
    private memberService: MemberService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.route.data.subscribe(data => this.member = data['member']);

    this.route.queryParams.subscribe(params =>
      params['tab'] && this.selectTab(params['tab'])
    );

    this.getImages();
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const tab = this.memberTabs.tabs.find(t => t.heading === heading);
      if (tab) {
        tab.active = true;
      }
    }
  }

  onTabActivated(data: TabDirective): void {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages') {
      this.loadMessages();
    }
  }

  getImages() {
    if (!this.member) return;
    for (const photo of this.member.photos) {
      this.images.push(new ImageItem({ src: photo.url, thumb: photo.url }));
    }
  }

  addLike(member: Member): void {
    this.memberService.addLike(member.username).subscribe(() =>
      this.toastr.success(`You have liked ${ member.knownAs }`));
  }

  loadMessages(): void {
    if (this.member?.username) {
      this.messageService.getMessageThread(this.member.username).subscribe(messages =>
        this.messages = messages
      );
    }
  }
}
