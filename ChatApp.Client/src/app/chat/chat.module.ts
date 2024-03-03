import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from '../account/account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MainComponent } from './main/main.component';
import { ChatRoutingModule } from './chat-routing.module';



@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    ChatRoutingModule,
    SharedModule
  ]
})
export class ChatModule { }
