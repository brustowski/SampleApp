import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authenticationService';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

public UserName: string = "";

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.getUser().subscribe(user=>{
      this.UserName = user ? user.UserAccount : "";
    })
  }

}
