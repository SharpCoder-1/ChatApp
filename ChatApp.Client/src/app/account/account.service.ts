import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Register} from 'src/app/shared/models/register';
import { Login } from '../shared/models/login';
import { environment } from 'src/environments/environment.development';
import { User } from '../shared/models/user';
import {ReplaySubject,map,of} from 'rxjs'
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();
  constructor(private http:HttpClient,private router:Router) {

   }
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


  refreshUser(){ 
    const jwt:string | null = this.getJwt();
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
  getJwt(){
    
    const key = localStorage.getItem(environment.key);
    if(key){
      const user:User = JSON.parse(key) as User;
      return user.JWT;
    }
    return null;
  }
  logout(){
    localStorage.removeItem(environment.key);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
  }

  private setUser(user:User){
    localStorage.setItem(environment.key,JSON.stringify(user));
    this.userSource.next(user);
    // this.user$.subscribe({next:response=>{
    //   console.log(response);
    // }})
  }

}
