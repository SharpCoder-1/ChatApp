import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment.development';
import { Message } from '../shared/models/chat/message';
import { User } from '../shared/models/account/user';
import { SharedService } from '../shared/shared.service';
import { Subject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private sharedService:SharedService) { }
  private hubConnection: HubConnection | undefined;
  messages: Subject<Message[]> = new Subject<Message[]>();
  public async startConnection(): Promise<unknown> {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder().withUrl(`${environment.appUrl}/hubs/chat`, {
        accessTokenFactory: () => {
          var token = this.sharedService.getJwt()
          if (token !== null) {
            return token;
          }
          return "";
           
        },
       
      })
        .build();

      this.hubConnection.start().then(()=> {
        console.log("Connection is established");
        this.receiveMessages();
        return resolve(true);
      }
      ).catch((err: any) => {
        console.log(err);
        reject(err)
      })
    })
  }
  receiveMessages() {
    this.hubConnection?.on("ReceiveMessages", (messages: any) =>
    {
        this.messages.next(messages);
      console.log("Message" + messages.map());
      

    })
  }
  getMessages() {
    return this.messages.asObservable();
  }
  public sendMessage(model: Message)
  {
    this.hubConnection?.invoke('ReceiveMessage',model);
  }
  public receiveMessage() {
    this.hubConnection?.on('ReceiveMessage', (model: Message) => {
      console.log(model);
    })

  }
}
