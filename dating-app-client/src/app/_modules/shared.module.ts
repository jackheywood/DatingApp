import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ButtonsModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
    NgxSpinnerModule.forRoot({ type: 'ball-spin-clockwise-fade' }),
    TimeagoModule.forRoot(),
    FileUploadModule,
  ],
  exports: [
    BsDropdownModule,
    BsDatepickerModule,
    ButtonsModule,
    PaginationModule,
    TabsModule,
    ToastrModule,
    NgxSpinnerModule,
    TimeagoModule,
    FileUploadModule,
  ],
})
export class SharedModule {
}
