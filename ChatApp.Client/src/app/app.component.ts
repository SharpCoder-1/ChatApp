import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private accountService:AccountService){}
  ngOnInit(): void {
    this.refreshUser();
  }
  private refreshUser() {
      this.accountService.refreshUser().subscribe({
        next: _=>{},
        error: _=>{
          this.accountService.logout()
        },

      })
    
    } 
  

  title = 'ChatApp.Client';
}
