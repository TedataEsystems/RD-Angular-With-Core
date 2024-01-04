
import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';

import { from } from 'rxjs';
import { ConfigureService } from './configure.service';
import { Login } from '../../Models/Logincls';
@Injectable({
  providedIn: 'root'
})
export class LoginserService {

  apiURL: string ="";
  constructor(private httpClient: HttpClient, private config: ConfigureService) {
    this.apiURL= this.config.ApiUrl() + "UserAccount";
   }
  public getLogin(model: Login)
  {
    return this.httpClient.post<any>(this.apiURL,model);
  }
}