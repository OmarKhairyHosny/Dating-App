import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  model: any = {};
//photoUrl:string;

  constructor(private authService:AuthService,private alertifyService:AlertifyService,private router:Router) { }

  ngOnInit() {
  //  this.authService.currentPhotoUrl.subscribe(photoUrl=>this.photoUrl=photoUrl);
  }
Login(){
 this.authService.login(this.model).subscribe(next=>{
   this.alertifyService.success("Logged in successfully");

 },error=>{
   this.alertifyService.error(error);
 },()=>{
   this.router.navigate(['/members']);
 }); 
}
loggedIn(){
  return this.authService.loggedIn();
}
logout(){
  localStorage.removeItem('token');
  localStorage.removeItem('user');
  this.authService.currentUser=null;
  this.authService.decodedToken=null;
  this.alertifyService.message('logged out');
  this.router.navigate(['/home']);
}
}
