import { Component, OnInit } from '@angular/core';
import { AuthService } from './Services/Auth.service';
import { User } from './Interfaces/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService) { }
  ngOnInit(): void {
    var token = localStorage.getItem('token');
    var user:User = JSON.parse(localStorage.getItem('user'));
    if (token)
    this.authService.decodedToken = this.authService.jwtHelper.decodeToken(token);
    if(user){
      this.authService.currentUser=user;
     // this.authService.changeMemberPhoto(user.photoUrl);
      this.authService.photoUrl2=user.photoUrl;
    }
  }
}
