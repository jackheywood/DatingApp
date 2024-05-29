import { Component, Input } from '@angular/core';
import { Message } from '../../_models/message';
import { CommonModule } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  imports: [CommonModule, TimeagoModule],
  styleUrls: ['./member-messages.component.scss']
})
export class MemberMessagesComponent {
  @Input() messages: Message[] = [];
  @Input() username?: string;
}
