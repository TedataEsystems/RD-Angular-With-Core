import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../navigation/footer/footer.component';
import { NavlistComponent } from '../navigation/navlist/navlist.component';
import { HeaderComponent } from '../navigation/header/header.component';
import { LayoutComponent } from './layout.component';
import { MaterialModule } from '../../modules/material/material.module';
import { Title } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DeleteComponent } from '../msg/delete/delete.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { LoaderComponent } from '../loader/loader.component';
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { NgxMatMomentModule } from '@angular-material-components/moment-adapter';


import { HttpClientModule } from '@angular/common/http';
import { RangeFormComponent } from '../../../components/range-form/range-form.component';
import { CustomerRangeComponent } from '../../../components/customer-range/customer-range.component';
import { LogdataComponent } from '../../../components/logdata/logdata.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { RDComponent } from '../../../components/rd/rd.component';
import { RDFormComponent } from '../../../components/rd-form/rd-form.component';
import { UsersComponent } from '../../../components/users/users.component';


@NgModule({
  declarations: [
    LayoutComponent,
    HeaderComponent,
    NavlistComponent,
    FooterComponent,
    DeleteComponent,
    LoaderComponent,
    RDComponent,
    RDFormComponent,
    RangeFormComponent,
    LogdataComponent,
    CustomerRangeComponent,
    UsersComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    FlexLayoutModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgxMatMomentModule,
    HttpClientModule
  ],
  providers:[Title]
})
export class LayoutModule { }
