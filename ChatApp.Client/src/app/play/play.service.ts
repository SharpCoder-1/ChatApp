import { Injectable, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
@Injectable({
  providedIn: 'root'
})
export class PlayService implements OnInit {
  message:string | undefined;
  constructor(private http:HttpClient) { }
  ngOnInit(): void {
    
  }
  getPlayers(){
    return this.http.get(`${environment.appUrl}/api/play/get-players`)
    
    
  }

}
