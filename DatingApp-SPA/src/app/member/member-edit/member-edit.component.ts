import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../../Interfaces/user';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../Services/Auth.service';
import { AlertifyService } from '../../Services/alertify.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
@ViewChild('editForm',null) editForm:NgForm;

@HostListener('window:beforeunload', ['$event'])
unloadNotification($event: any) {
  if (this.editForm.dirty) {
    $event.returnValue = true;
  }
}

user:User;
  constructor(private userService:UserService,private authService:AuthService,private route:ActivatedRoute,
  private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data=>this.user=data['user']);
   // this.authService.currentPhotoUrl.subscribe(photoUrl=>this.user.photoUrl=photoUrl);
  }
  updateUser(){
    this.userService.updateUser(this.authService.decodedToken.nameid,this.user).subscribe(next=>{
      this.alertify.success('Updated Successfully');
      this.editForm.reset(this.user)
    },error=>this.alertify.message(error))
  }
  changeMainPhoto(photoUrl){
    this.user.photoUrl=photoUrl;
  }
}
