import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
import { SharedService } from '../shared.service';
import {map} from 'rxjs';
import { User } from '../models/account/user';
export const authorizationGuard: CanActivateFn = (route, state) => {
  const authService = inject(AccountService);
  const router = inject(Router);
  const sharedService = inject(SharedService);
  return authService.user$.pipe(map((user:User|null)=>{
    if(user)
      return true;
    sharedService.showNotification(false,"Restricted area","Leave immediately!");
    router.navigate(['account/login'],{queryParams:{returnUrl:state.url}});
    return false;
  }));
};
