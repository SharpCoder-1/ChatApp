import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Register} from 'src/app/shared/models/account/register';
import { Login } from '../shared/models/account/login';
import { environment } from 'src/environments/environment.development';
import { User } from '../shared/models/account/user';
import {ReplaySubject,map,of} from 'rxjs'
import { Router } from '@angular/router';
import { ConfirmEmail } from 'src/app/shared/models/account/confirmEmail';
import { ResetPassword } from '../shared/models/account/resetPassword';
import { SharedService } from '../shared/shared.service';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private http: HttpClient, private router: Router,
              private sharedService:SharedService
  ) {

   }
  forgotUsernameOrPassword(email: string) {
    return this.http.post(`${environment.appUrl}/api/account/forgot-username-or-password/${email}`, {});
    
  }
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();

  register(model:Register){
    return this.http.post(`${environment.appUrl}/api/account/register`,model);
  }

  login(model:Login){
    return this.http.post<User>(`${environment.appUrl}/api/account/login`,model)
    .pipe(map((user:User)=>{
    
      if(user)
        this.setUser(user);
    }));
  }

  confirmEmail(model: ConfirmEmail) {
    return this.http.put(`${environment.appUrl}/api/account/confirm-email`, model);
  }


  refreshUser(){ 
    const jwt:string | null = this.sharedService.getJwt();
    console.log(`JWT ${jwt}`)
    if(!jwt){
      this.userSource.next(null);
      return of(undefined);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization','Bearer '+jwt)
    return this.http.get<User>(`${environment.appUrl}/api/account/refresh-user-token`,{headers})
    .pipe(map((user:User)=>{
      if(user)
        console.log(""+headers.getAll("Authorization"))
        this.setUser(user);
    }));
  }
  
  logout(){
    localStorage.removeItem(environment.key);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
  }

  resendEmailConfirmationLink(email: string) {
    console.log(email);
    
    return this.http.post(`${environment.appUrl}/api/account/resend-email-confirmation-link/${email}`, {})
  }

  resetPassword(model: ResetPassword) {
    return this.http.put(`${environment.appUrl}/api/account/reset-password`,model);
  }

  private setUser(user:User){
    localStorage.setItem(environment.key,JSON.stringify(user));
    this.userSource.next(user);
    // this.user$.subscribe({next:response=>{
    //   console.log(response);
    // }})
  }

}
