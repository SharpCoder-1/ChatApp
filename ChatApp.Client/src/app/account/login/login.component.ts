import { Component, OnInit } from '@angular/core';
import {FormGroup,FormBuilder,Validators} from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import {take,map} from 'rxjs'
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
  
})
export class LoginComponent implements OnInit 
{

  loginForm:FormGroup = new FormGroup({});
  errorMessages:string[] = [];
  submitted = false;
  returnUrl:string| null = null;
  constructor(
    private formBuilder:FormBuilder,
    private accountService:AccountService,
    private router:Router,
    private activatedRoute:ActivatedRoute
    ){
      
      //this.accountService.refreshUser(this.accountService.getJwt());
     }
  ngOnInit(): void {
    this.accountService.user$.pipe(take(1)).subscribe({
        next:(user:User|null) =>{
          
          if(user)
            this.router.navigateByUrl('/');
          else{
          this.activatedRoute.queryParamMap.subscribe({
            next:(params:any)=>{
              if(params)
                this.returnUrl = params.get('returnUrl');
            }
          })}
        
      this.initializeForm();
    }})
  }

  initializeForm(){
    this.loginForm = this.formBuilder.group({
      userName: ['',[Validators.required]],
      password: ['',[Validators.required]],
    })

    
  }
  login(){
    this.submitted = false;
    this.errorMessages = [];

    if(this.loginForm.valid){
        this.accountService.login(this.loginForm.value).subscribe({
          next:(response)=>{
              if(this.returnUrl)
                this.router.navigateByUrl(this.returnUrl);
              else
              this.router.navigateByUrl('');        
          },
          error:error=>{
            if(error.error.errors){
              this.errorMessages = error.error.errors;
            }
            else{
              this.errorMessages.push(error.error);
            }
          }
        });

    } 

  }
}