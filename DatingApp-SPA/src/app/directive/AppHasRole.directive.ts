import { Directive, Input, ViewContainerRef, TemplateRef, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';

@Directive({
  selector: '[AppHasRole]'
})
export class AppHasRoleDirective implements OnInit {
 
@Input() appHasRole:string[];
isVisiable = false;
  constructor(private viewContainterRef:ViewContainerRef,private templateRef:TemplateRef<any>,private authService:AuthService) { }
  ngOnInit(): void {
    debugger;
    const userRoles = this.authService.decodedToken.role as Array<string>;

    //if no roles clear viewcontainerref
    if(!userRoles)
    this.viewContainterRef.clear();
   // if the user has the role need then render the element
    if(this.authService.roleMatch(this.appHasRole)){
      if(!this.isVisiable){
        this.viewContainterRef.createEmbeddedView(this.templateRef);
        this.isVisiable=true;
      }else{
        this.isVisiable=false;
        this.viewContainterRef.clear();
      }
    }
  }
}
