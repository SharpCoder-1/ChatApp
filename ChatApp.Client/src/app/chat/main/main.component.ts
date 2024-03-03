import { Component, OnInit } from '@angular/core';
import { ChatService } from '../chat.service';
import { Message } from '../../shared/models/chat/message';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent implements OnInit {
  messages: Observable<Message[]> = new Observable<Message[]>();
  constructor(private chatService:ChatService) { }
  async ngOnInit(): Promise<any> {
    await this.chatService.startConnection()
    this.messages = this.chatService.getMessages();
    console.log(this.messages);
      
  }
  sendMessage() {
    this.messages = this.chatService.getMessages();

    this.chatService.sendMessage({ body: "message",senderId:"sad",receiverId:"sada"});
  }

}
