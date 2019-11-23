import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { User } from '../../Interfaces/user';

@Component({
  selector: 'app-role-model',
  templateUrl: './role-model.component.html',
  styleUrls: ['./role-model.component.css']
})
export class RoleModelComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();

  user: User;
  roles: any[]; 
 
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit() {
    
  }
  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

}
