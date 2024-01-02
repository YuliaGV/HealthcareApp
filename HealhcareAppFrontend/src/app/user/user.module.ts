import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { UserService } from './services/user.service';
import { LoginComponent } from './login/login.component';
import { MaterialModule } from '../material/material.module';




@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule, SharedModule, MaterialModule
  ],
  exports: [
    LoginComponent
  ],
  providers: [
    UserService
  ]
})
export class UserModule { }
