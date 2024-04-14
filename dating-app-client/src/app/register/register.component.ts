import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});

  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity(),
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { notMatching: true };
    };
  }

  register() {
    console.log(this.registerForm?.value);
    // this.accountService.register(this.model).subscribe({
    //   next: _ => this.cancel(),
    //   error: error => this.toastr.error(error.error),
    // });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
