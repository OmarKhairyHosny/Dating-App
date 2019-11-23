import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/alertify.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) { }
  canActivate(next: ActivatedRouteSnapshot): boolean {
    if (this.authService.loggedIn()) {
      const roles = next.firstChild.data['roles'] as Array<string>;
      if (roles) {
        var result = this.authService.roleMatch(roles);
        if (result)
          return true;
        else {
          this.router.navigate(['/members']);
          this.alertify.error('you are not allow to access this area');
        }
      }
      return true;
    }

    this.alertify.error('You shall not pass!!');
    this.router.navigate(['/home']);
    return false;

  }

}
