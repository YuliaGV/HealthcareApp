import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {

  username: string = '';

  constructor(private _router: Router, private _sharedService: SharedService){

  }

  ngOnInit(): void {
    const userToken = this._sharedService.getSession();

    if(userToken != null)
    {
      this.username = userToken.name+ ' '+userToken.lastName
    }
  }


  logOut(): void {
    this._sharedService.deleteSession();
    this._router.navigate(['login']);
  }

}