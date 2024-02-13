import { HttpInterceptorFn } from '@angular/common/http';
import { Inject, inject } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  accountService.user$.subscribe({next:user=>{
    if(user){
      req = req.clone({
        setHeaders:{
          Authorization:`Bearer ${user.JWT}`
        }
      })
    }
  }})
  return next(req);
};
