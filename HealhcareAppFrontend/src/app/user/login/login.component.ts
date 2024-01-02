import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/shared/shared.service';
import { Login } from '../interfaces/login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  formLogin: FormGroup;
  hidePassword: boolean = true;
  showLoading: boolean = false;


  constructor(private formBuilder : FormBuilder, private router: Router, private authService: UserService, private sharedService: SharedService){
    this.formLogin = this.formBuilder.group({
        email: ['', Validators.required],
        password: ['', Validators.required]
    });
  }


  sigIn(){
    this.showLoading = true;
    const request: Login = {
      email: this.formLogin.value.email,
      password: this.formLogin.value.password
    }
   
    this.authService.signInService(request).subscribe({
      next: (response) => {
        this.sharedService.saveSession(response);
        this.router.navigate(['layout']);
      },
      complete: () => {
        this.showLoading = false;
      },
      error: (err) => {
        this.sharedService.showAlert(err.error, 'Error!');
        this.showLoading = false;
      }
    });
  }



}
