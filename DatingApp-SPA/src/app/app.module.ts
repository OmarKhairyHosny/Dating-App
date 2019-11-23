import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import  {FormsModule, ReactiveFormsModule} from '@angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService } from './Services/Auth.service';
import { RegissterComponent } from './regisster/regisster.component';
import { HomeComponent } from './home/home.component';
import {  ErrorInterceptor } from './Services/error.interceptor';
import { AlertifyService } from './Services/alertify.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule,TabsModule, ModalModule ,BsDatepickerModule, PaginationModule,ButtonsModule } from 'ngx-bootstrap';
import { MemberListComponent } from './member/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './guards/auth.guard';
import { UserService } from './Services/user.service';
import { MemberCardComponent } from './member/member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { MemberDetailResolver } from './resolvers/memberdetail.resolver';
import { NgxGalleryModule } from 'ngx-gallery';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { MemberEditResolver } from './resolvers/memberedit.resolver';
import { PrevetUnSaveChanges } from './guards/prevent-unsaveChanges.guard';
import { PhotoEditorComponent } from './member/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import {TimeAgoPipe} from 'time-ago-pipe';
import { MessagesResolver } from './resolvers/messages.resolver';
import { MemberMessagesComponent } from './member/member-messages/member-messages.component';
import { AdminPanalComponent } from './Admin/admin-panal/admin-panal.component';
import { AppHasRoleDirective } from './directive/AppHasRole.directive';
import { UserManagmentComponent } from './Admin/user-managment/user-managment.component';
import { PhotoManagmentComponent } from './Admin/photo-managment/photo-managment.component';
import { AdminService } from './Services/admin.service';
import { RoleModelComponent } from './Admin/role-model/role-model.component';


export function tokenGetter(){
    return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      AppHasRoleDirective,
      NavBarComponent,
      RegissterComponent,
      HomeComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      MemberMessagesComponent,
      TimeAgoPipe,
      AdminPanalComponent,
     UserManagmentComponent,
     PhotoManagmentComponent,
     RoleModelComponent
   ],
   imports: [
      BrowserModule,
      FileUploadModule,
      AppRoutingModule,
      HttpClientModule,
      ReactiveFormsModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      BrowserAnimationsModule,
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      ModalModule.forRoot(),
      NgxGalleryModule,
      JwtModule.forRoot({
          config:{
              tokenGetter:tokenGetter,
              whitelistedDomains:['localhost:44330'],
              blacklistedRoutes:['localhost:44330/api/auth']
          }
      })
   ],
   providers: [
      AuthService,
      { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
      AlertifyService,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MemberEditResolver,
      PrevetUnSaveChanges,
      MessagesResolver,
      AdminService
   ],
   entryComponents: [
    RoleModelComponent
  ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
