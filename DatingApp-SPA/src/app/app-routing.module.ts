import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './guards/auth.guard';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { MemberDetailResolver } from './resolvers/memberdetail.resolver';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { MemberEditResolver } from './resolvers/memberedit.resolver';
import { PrevetUnSaveChanges } from './guards/prevent-unsaveChanges.guard';
import { MessagesResolver } from './resolvers/messages.resolver';
import { AdminPanalComponent } from './Admin/admin-panal/admin-panal.component';


const routes: Routes = [
{path:'',component:HomeComponent},
{path:'',
runGuardsAndResolvers:'always',
canActivate:[AuthGuard],
children: [
  {path: 'members', component: MemberListComponent},
  {path: 'member/edit', component: MemberEditComponent,resolve:{user:MemberEditResolver},canDeactivate:[PrevetUnSaveChanges]},
  {path: 'members/:id', component: MemberDetailComponent,resolve:{user:MemberDetailResolver}},
  {path: 'messages', component: MessagesComponent,resolve: {messages: MessagesResolver}},
  {path: 'lists', component: ListsComponent},
  {path: 'admin', component: AdminPanalComponent,data:{roles:['Admin','Moderator']}},
]
},
 
{path:'**',redirectTo:'',pathMatch:'full'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
