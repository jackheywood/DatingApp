import { Component, EventEmitter, Output } from '@angular/core';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private accountService: AccountService) { }

  register() {
    this.accountService.register(this.model).subscribe({
      next: _ => this.cancel(),
      error: error => console.log(error),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
