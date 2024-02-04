import { Component } from '@angular/core';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent {
  model: any = {};

  constructor(public accountService: AccountService) {}

  login(): void {
    this.accountService.login(this.model).subscribe({
      error: (error) => console.log(error),
    });
  }

  logout(): void {
    this.accountService.logout();
  }
}
