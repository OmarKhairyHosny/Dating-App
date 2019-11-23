import { Component, OnInit } from '@angular/core';
import { User } from '../Interfaces/user';
import { Pagination } from '../Interfaces/Pagination';
import { AlertifyService } from '../Services/alertify.service';
import { UserService } from '../Services/user.service';
import { AuthService } from '../Services/Auth.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;
  constructor(private authService: AuthService, private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    console.log(this.authService.decodedToken);
    this.likesParam = 'Likers';
 this.loadUsers(1,5);
  }
  loadUsers(page:number, pageSize:number) {
    this.userService.getUsers(page,pageSize,null,this.likesParam).subscribe(res => {
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
}
