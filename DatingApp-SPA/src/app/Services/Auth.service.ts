import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { logging } from 'protractor';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt'
import { environment } from '../../environments/environment';
import { User } from '../Interfaces/user';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl2: string;
  photoUrl = new BehaviorSubject("../../assets/user.png");
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }
  login(user: User) {
    return this.http.post(this.baseUrl + 'login', user).pipe(
      map(
        (response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUser = user.user;
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            //this.changeMemberPhoto(this.currentUser.photoUrl);
            if (this.currentUser.photoUrl == null)
            this.currentUser.photoUrl = "../../../assets/user.png";
            
            this.photoUrl2 = this.currentUser.photoUrl;
            localStorage.setItem('user', JSON.stringify(this.currentUser));
          }
        }
      )
    )
  }
  register(user: User) {
    return this.http.post(this.baseUrl + "register", user);
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  roleMatch(allowRoles):boolean{
    let isMatch = false;
    const userRoles = this.decodedToken.role as Array<string>;
    allowRoles.forEach(element => {
     if(userRoles.includes(element)){
       isMatch=true;
       return;
     }
    });
    return isMatch;
  }
}

