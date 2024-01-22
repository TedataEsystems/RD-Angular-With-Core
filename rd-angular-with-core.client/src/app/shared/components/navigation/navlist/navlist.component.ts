import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navlist',
  templateUrl: './navlist.component.html',
  styleUrls: ['./navlist.component.css']
})
export class NavlistComponent implements OnInit {
isNotAdmin=false
  constructor() { }

  ngOnInit(): void {
    var teamval= localStorage.getItem("userGroup");
    
    if(teamval?.toLocaleLowerCase() != 'admin'){
     
    this.isNotAdmin=true
 }
  }
   
 
  isExpanded = true;
  showSubmenu: boolean = false;
  isShowing = false;
  showSubSubMenu: boolean = false;

}
