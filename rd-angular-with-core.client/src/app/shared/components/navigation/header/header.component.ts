import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/notification.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
   userName = localStorage.getItem("usernam");
  @Output() public sidenavToggle = new EventEmitter();
  constructor( private router :Router,private notificationService:NotificationService) { }
// constructor( private router :Router,private accountService : AccountService,private notificationService:NotificationMsgService) { }

ngOnInit(): void {
 
}

logOut(){
  // localStorage.clear();
  // this.accountService.logout().subscribe(res=>{
    this.router.navigateByUrl('/');
    
  // } 
  
  // ,error=>{this.notificationService.warn('occured an error ')}
  // );

}
public onToggleSidenav=()=> {
this.sidenavToggle.emit();
}
}
