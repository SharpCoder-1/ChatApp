import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { NotificationComponent } from './components/modals/notification/notification.component';
import { environment } from '../../environments/environment.development';
import { User } from './models/account/user';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  bsModalRef?:BsModalRef;
  constructor(private modalService:BsModalService) { }
  showNotification(isSuccess:boolean,title:string,message:string){
      const initialState:ModalOptions={
        initialState:{
          isSuccess,title,message
        }
      }
      this.bsModalRef = this.modalService.show(NotificationComponent,initialState);
  }
  getJwt() {

    const key = localStorage.getItem(environment.key);
    if (key) {
      const user: User = JSON.parse(key) as User;
      return user.JWT;
    }
    return null;
  }
}
