 
import { Injectable } from '@angular/core';
import {Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ConfigureService {
//Apiurl:string = "https://localhost:7277/api/";
    Apiurl:string = "http://172.29.29.108:2025/api/";

public pIn:number=0;
  constructor(  private router: Router)
   {   }

   ApiUrl()
 
    {
       return this.Apiurl;
    }
     
   UserName()
   {
       return localStorage.getItem("usernam");
   }

   UserToken()
   {
       return localStorage.getItem("tokNum");
   }

   IsAuthentecated()
   {
     if(!this.UserToken() || !this.UserName()  )
     {
      this.router.navigateByUrl('/login');
     }
   }

   Logout()
   {
  
    localStorage.removeItem("teamName");
    localStorage.removeItem("tokNum");
    localStorage.removeItem("usernam");
   }


    
}
