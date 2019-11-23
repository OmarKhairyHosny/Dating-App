import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../Interfaces/user';
import { UserService } from '../Services/user.service';
import { AlertifyService } from '../Services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../Services/Auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User>{

    constructor(private userService: UserService,private authService:AuthService, private alertify: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(catchError(
            error => { this.alertify.error('can;t load user detail');
        this.router.navigate(['/members']);
        return of(null)
        }
        ));
    }
}