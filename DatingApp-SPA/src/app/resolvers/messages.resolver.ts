import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../services/user.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Message } from '../Interfaces/Message';
import { User } from '../Interfaces/user';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';
    user:User ;

    constructor(private userService: UserService, private router: Router,
        private alertify: AlertifyService, private authService: AuthService) {
        }
        
        resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
            this.user = JSON.parse(localStorage.getItem('user'));
        console.log(this.user.id);
        return this.userService.getMessages(this.user.id,
              this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
