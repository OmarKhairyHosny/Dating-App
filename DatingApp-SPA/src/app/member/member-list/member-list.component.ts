import { Component, OnInit } from '@angular/core';
import { UserService } from '../../Services/user.service';
import { AlertifyService } from '../../Services/alertify.service';
import { User } from '../../Interfaces/user';
import { Pagination } from '../../Interfaces/Pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
users:User[];
user: User = JSON.parse(localStorage.getItem('user'));
genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
userParams: any = {};
pagination:Pagination;

  constructor(private userService:UserService,private alertify:AlertifyService) { }
  
  
  ngOnInit() { 
  
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
    this.loadUsers(1,5);
  }

   loadUsers(page:number, pageSize:number) {
     this.userService.getUsers(page,pageSize,this.userParams).subscribe(res => {
      this.pagination= res.pagination;
       this.users = res.result;
    }, error => {
       this.alertify.error(error);
     });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers(this.pagination.currentPage,this.pagination.itemsPerPage);
  }
  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.loadUsers(this.pagination.currentPage,this.pagination.itemsPerPage);
  }
}
