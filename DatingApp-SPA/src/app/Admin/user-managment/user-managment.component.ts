import { Component, OnInit } from '@angular/core';
import { User } from '../../Interfaces/user';
import { AdminService } from '../../Services/admin.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { RoleModelComponent } from '../role-model/role-model.component';

@Component({
  selector: 'app-user-managment',
  templateUrl: './user-managment.component.html',
  styleUrls: ['./user-managment.component.css']
})
export class UserManagmentComponent implements OnInit {
  users: User[];
  bsModalRef: BsModalRef;

  constructor(private adminService:AdminService,
    private modalService: BsModalService) { }

  ngOnInit() {
  this.getUsersWithRoles();
  }
 getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      console.log(error);
    });
  }
  editRolesModal(user: User) {
    const initialState = {
      user,
      roles: this.getRolesArray(user)
    };
    this.bsModalRef = this.modalService.show(RoleModelComponent, {initialState});
    this.bsModalRef.content.updateSelectedRoles.subscribe((values) => {
      const rolesToUpdate = {
        rolesNames: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.rolesNames];
        }, error => {
          console.log(error);
        });
      }
    });
  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin',checked:false},
      {name: 'Moderator', value: 'Moderator',checked:false},
      {name: 'Member', value: 'Member',checked:false},
      {name: 'VIP', value: 'VIP',checked:false},
    ];

    for (let i = 0; i < availableRoles.length; i++) {
     if(userRoles.includes(availableRoles[i].name)){
        availableRoles[i].checked=true;
      }
    }
    return availableRoles;
  }
}
