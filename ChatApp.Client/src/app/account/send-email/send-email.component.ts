import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared/shared.service';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup ,FormBuilder,Validators,FormsModule} from '@angular/forms';
import { User } from '../../shared/models/account/user';
@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrl: './send-email.component.css',
})
export class SendEmailComponent implements OnInit {
  emailForm: FormGroup = new FormGroup({});
  submitted = false;
  mode: string | undefined;
  errorMessages:string[] = [];
  constructor(private sharedService: SharedService,
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder : FormBuilder
  ) {

  }
    ngOnInit(): void {
      this.accountService.user$.subscribe({
        next: (user: User | null) => {
          if (user)
            this.router.navigateByUrl('/');
          else {
            const mode = this.activatedRoute.snapshot.paramMap.get('mode');
            if (mode) {
              this.mode = mode;
              console.log(mode);
              this.initializeForm();
            }
          }
        }
      })
  }
  initializeForm() {
    this.emailForm = this.formBuilder.group({
      email: ['',[Validators.required,Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]]
    })
  }

  sendEmail() {
    this.submitted = true;
    this.errorMessages = [];
    if (this.emailForm.valid && this.mode) {
      if (this.mode.includes('resend-email-confirmation-link')) {
        this.accountService.resendEmailConfirmationLink(this.emailForm.get('email')?.value)
          .subscribe({
            next: (response: any) => {
              this.sharedService.showNotification(true, response.value.title, response.value.message);
              this.router.navigateByUrl('account/login');
            },
            error: error => {
              if (error.error.errors) {
                this.errorMessages = error.error.errors;
              }
              else {
                this.errorMessages.push(error.error);
              }
            }
          })
      }
      else if (this.mode.includes('forgot-username-or-password')) {
        this.accountService.forgotUsernameOrPassword(this.emailForm.get('email')?.value).subscribe({
          next: (response: any)=>{
            this.sharedService.showNotification(true, response.value.title, response.value.message);
            this.router.navigateByUrl('account/login');
          },
          error: error => {
            if (error.error.errors) {
              this.errorMessages = error.error.errors;
            }
            else {
              this.errorMessages.push(error);
            }
          }

        });
      }
     
    }
    
  }
  cancell() {
    this.router.navigateByUrl('account/login');
  }
}
