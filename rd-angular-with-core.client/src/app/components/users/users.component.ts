 
import {Component,Input,NgModule, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import { Title } from '@angular/platform-browser';
import {FormControl, Validators} from '@angular/forms';
import { DeleteService } from '../../shared/services/delete.service';
 
import {FormGroup} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigureService } from '../../shared/services/configure.service';
import { users } from '../../Models/users';
import { usersService } from '../../shared/services/users.service';
import { NotificationService } from '../../shared/services/notification.service';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';
 
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

displayedColumns: string[] = ['action','id', 'userName', 'roleID'];
columnsToDisplay: string[] = this.displayedColumns.slice();
usersList?:users[]=[];
usersListTab?:users[]=[];
valdata="";valuid=0;
dataSource = new MatTableDataSource<any>();
delpic:any;
  searchKey:string ='';
  listName:string ='';
 
  selected: boolean = false;
  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort?: MatSort;
  param1:any;settingtype=''
  constructor(private titleService:Title,private usersSer:usersService,private loading:LoadingService,
    private notser:ToastrService,private router: Router,private dialogService:DeleteService,
     private route: ActivatedRoute , private Config:ConfigureService
    ) {
       var teamval= localStorage.getItem("userGroup");
    
    if(teamval?.toLocaleLowerCase() != 'admin'){
      this.router.navigate(['/login'] );
    
 }
      this.Config.IsAuthentecated();
      this.titleService.setTitle('Service users');
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort as MatSort; 
   this.dataSource.paginator = this.paginator as MatPaginator;
   
  }
  simflag=true;
  ngOnInit() { 
    this.loading.busy();
  this.getRequestdata();
    
  }
  getRequestdata(){
    
    this.usersSer.getusers().subscribe(res=>{
      
      if(res.status==true){
        
        this.loading.idle();
     
     this.usersList = res.result?.data;
     this.apply(this.param1);
     
   
      }else{

        this.notser.warning(res.error);
        this.loading.idle();
      } 
    },err=>{
      
      if(err.users==401)
      this.router.navigate(['/loginuser'] );
      else 
      this.notser.warning("! Fail");
     
      this.loading.idle();
    })
   }
  onSearchClear(){
    this.searchKey ='';
    //this.applyFilter();
   
    if (this.dataSource.paginator) {
    this.dataSource.paginator.firstPage();
    }
  }
  // applyFilter(){ 
  //   this.dataSource.filter=this.searchKey.trim().toLowerCase();
    
  // }
  apply(filterValue:string) {
    
    this.selected=true;
    this.listName=filterValue;
    this.usersListTab=[];
    this.usersListTab=this.usersList ;
   
  this.setReactValue(Number(0),"",0);
  
  this.dataSource =new MatTableDataSource<any>(this.usersListTab);
      
    this.dataSource.paginator = this.paginator as MatPaginator;
    
   
  }
  applyFilter(filterValue: Event) { 
     
    this.dataSource.filter =(<HTMLInputElement>filterValue.target).value.trim().toLowerCase();
   
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  onEdit(r:any){
  
    
    this.valdata=r.value;
    this.valuid=r.id;
    if(r.roleID !=null)
    this.setReactValue(Number(r.id),r.userName,Number(r.roleID));
    else 
    this.setReactValue(Number(r.id),r.value,0);

//this.router.navigate(['/Simdetail'],{ queryParams: {id: r.id}});
 
  }
  form: FormGroup = new FormGroup({
    id: new FormControl(0),
    userName: new FormControl('',[Validators.required]),
    roleID: new FormControl(2, [Validators.required]),
        
  });//18
  isDisable=false;

  onSubmit() {
    // 
   this.isDisable=true;

      if (this.form.invalid||this.form.value.value==' ') {
        if (this.form.value.value==' ')
         this.setReactValue(Number(0),"",0);  
         this.isDisable=false;
          return;
      } 
      
      var listval:users=new users();
      listval.roleID= Number(this.form.value.roleID);
      listval.userName=this.form.value.userName;
       
      if(this.form.value.id==0||this.form.value.id==null||this.form.value.id==undefined){
        var HWData= this.usersListTab?.find(x=>x.userName==this.form.value.userName.trim());
        if(HWData)
        {
         this.isDisable=false;
         this.setReactValue(Number(0),"",0);  
          this.notser.warning("value already exist");
          return;
        }     
        
      this.usersSer.adduser(listval).subscribe((res)=>{
         this.isDisable=false;
         
      if(res.status==true)    {
        var SS:users=new users();
        
        SS.id=res.data?.id;
        SS.userName=res.data?.userName;
        SS.roleID=res.data?.roleID;

        this.usersListTab?.push(SS)
        
        this.dataSource =new MatTableDataSource<any>(this.usersListTab);
      
    this.dataSource.paginator = this.paginator as MatPaginator;
      this.notser.success("Added!") ;
      this.setReactValue(Number(0)," ",0);  
 
       
      }
      else{
      this.notser.warning("Not Added!") ;
  
      }
      },err=>{
        this.isDisable=false;

        if(err.users==401)
      this.router.navigate(['/loginuser'] );
      else 
      this.notser.warning("! Fail");
     
  
      }); 
    }
    else{
      var HWData= this.usersListTab?.find(x=>x.userName==this.form.value.userName);
      debugger;
      if(HWData &&HWData.id !=this.form.value.id)
      {
        this.isDisable=false;

       this.setReactValue(Number(0),"",0);  
        this.notser.warning("userName already exist");
        return;
      } 
      

     listval.id=Number(this.form.value.id);
  //   listval.serviceTypeID=this.param1;

      this.usersSer.edituser(listval).subscribe((res)=>{
         this.isDisable=false;
        
        if(res.status==true)    {
          // const index1 = this.dataSource.data.indexOf(this.l);
          // this.dataSource.data.splice(index1, 1);
          // this.dataSource._updateChangeSubscription()
       this.usersListTab?.forEach(x=>
        {
          if(x.id==res.data?.id){
          x.userName=res.data?.userName;
          x.roleID=res.data?.roleID;
          }
        });
        
        this.dataSource =new MatTableDataSource<any>(this.usersListTab);
      
    this.dataSource.paginator = this.paginator as MatPaginator;
   this.setReactValue(Number(0)," ",0);
 
    
          this.notser.success("saved!") ;
         
          }
          else{
          this.notser.warning("Not saved!") ;
      
          }
  
      },err=>{
        this.isDisable=false;
        if(err.users==401)
        this.router.navigate(['/loginuser']);
        else 
        this.notser.warning("! Fail");
       
  
  
      });
    }
  }
  setReactValue(id:number,val:any,num:any){
    this.form.patchValue({
      id: id,
      userName:val,
      roleID:num
    });
  
 }
 onDelete($key:any){
  this.dialogService.openConfirmDialog()
  .afterClosed().subscribe(res =>{
    if(res){
      this.usersSer.deluser($key).subscribe(
        res=>{        
      this.notser.success('Deleted successfully!');
      this.getRequestdata();
      });
      
    }
  });
}     
}


