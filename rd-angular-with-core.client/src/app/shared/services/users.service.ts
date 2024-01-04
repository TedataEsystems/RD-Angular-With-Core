import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { ConfigureService } from './configure.service';
import { users } from '../../Models/users';
@Injectable({
  providedIn: 'root'
})
export class usersService {
  private apiUrl: string;
  private headers = new HttpHeaders();

  constructor(private httpClient: HttpClient, private config: ConfigureService) {
    this.apiUrl = config.ApiUrl() + 'Users';
    this.headers = this.headers.set('Authorization',"Bearer "+ this.config.UserToken());
  }
  public getusers(){
    
    this.headers =this.headers.set('Authorization',"Bearer "+ this.config.UserToken()); 
    return this.httpClient.get<any>(`${this.apiUrl}`,{headers: this.headers});
  }
 
  public getuserOption(attribute:any,pageSize:number,pageNum:number ,search:string="",sortColumn:string="id",sortDir:string='ASC')
{
  this.headers =this.headers.set('Authorization',"Bearer "+ this.config.UserToken()); 

    var urlval=`${this.apiUrl}?pagesize=${pageSize}&pagenumber=${pageNum}&sortcolumn=${sortColumn}&sortcolumndir=${sortDir}&searchvalue=${search}&attributeName=${attribute}`;
    return this.httpClient.get<any>(urlval,{headers: this.headers});
}
userattr(attribute:any)
{   
  this.headers =this.headers.set('Authorization',"Bearer "+ this.config.UserToken()); 

    var urlval=`${this.apiUrl}?attributeName=${attribute}`;
    return this.httpClient.get<any>(urlval,{headers: this.headers});
}
  public getuserid(id:any){
    return this.httpClient.get<any>(`${this.apiUrl}/id`,{headers: this.headers});
  }
  public adduser(Val:users){
    return this.httpClient.post<any>(`${this.apiUrl}`,Val,{headers: this.headers});
  }
  public edituser(Val:users){
    
    return this.httpClient.post<any>(`${this.apiUrl}/Updateusers`,Val,{headers: this.headers});
  } 
  public deluser(Val:number)
  { 
   // return this.httpClient.delete<simResposeData>(this.apiUrl + "/" + Val,{headers: this.headers});
    return this.httpClient.get<any>(this.apiUrl + "/Removeusers/" + Val,{headers: this.headers});
    
  }
  
}
