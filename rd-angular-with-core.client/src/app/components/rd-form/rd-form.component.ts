import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef,MAT_DIALOG_DATA  } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../shared/services/notification.service';
import { RDDataService } from '../../shared/services/RDData.service';
import { RDData } from '../../Models/RDData';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';



@Component({
  selector: 'app-rd-form',
  templateUrl: './rd-form.component.html',
  styleUrls: ['./rd-form.component.css']
})
export class RDFormComponent implements OnInit {
  dialogTitle: string= "";
  show=false;
 
 warn ="";
  constructor(private notificationService : ToastrService,private loading:LoadingService,public service:RDDataService, public dialogRef: MatDialogRef<RDFormComponent>, private router: Router, private route: ActivatedRoute,@Inject(MAT_DIALOG_DATA) public data: any ) { }

  ngOnInit(): void {
    this.service.getAll();
   this.dialogTitle = this.data.dialogTitle;
   if(this.dialogTitle == 'Update' ){
     this.show=true;

   }
  

  }
  RDData:RDData = new RDData();
 

  onSubmit() { 
    this.loading.busy();
    const p = {...this.RDData, ...this.service.CustomerForm.value}

  

    if (this.service.CustomerForm) {
      if (p.id === 0) {
        this.service.Add(p).subscribe(res => {
       
          
          if (res.status == true) {

           
            this.notificationService.success('Saved Successfully With Port = ' + res.data.rD_Id);
            this.service.CustomerForm.reset();
            this.service.initializeFormGroup();
            this.onClose()
            setTimeout(()=> this.loading.idle(),5000) 
          }
          else {
           // this.notificationService.Warning(res.error);
           this.warn= ' * ' + res.error;
           this.loading.idle()
          }
        },err=>{
         
          if(err.status==401){ 
            this.onClose()
            this.router.navigate(['/login'], { relativeTo: this.route });

           this.loading.idle()
          }
       
          else 
          this.notificationService.warning("! Fail");
         
         
        });
      }
      else {
        this.service.Update(p).subscribe(res => {
   
          
          if (res.status == true) {
            this.notificationService.success('Saved Successfully With Port = ' + res.rD_Id);
            this.service.CustomerForm.reset();
            this.service.initializeFormGroup();
            this.onClose()
            setTimeout(()=> this.loading.idle(),5000)  
          }
          else {
            this.warn= ' * ' + res.error;

            this.loading.idle()
            //this.notificationService.Warning(res.error);
          }
        },err=>{
         
          if(err.status==401)
         { 
          this.onClose()
          this.router.navigate(['/login'], { relativeTo: this.route });
          }
          else 
          {
           this.notificationService.warning("! Fail");
          }
        
          this.loading.idle()
         
        });

      }

    }

  
    this.loading.idle()

  }
  onSubmit1() {
    this.loading.busy()
    // const p = {...this.RangeData, ...this.service.CustomerRangeForm.value}
    if (this.service.CustomerRangeForm.valid) {
     
 
        this.service.AddCustomerRange(this.service.CustomerRangeForm.value).subscribe(res =>{
         
          if(res.status){
            setTimeout(()=> this.loading.idle(),2000)
            this.notificationService.success('Submitted successfully');
            this.onClose();

          }
          else{
            
            setTimeout(()=> this.loading.idle(),2000)
          this.warn= ' * ' + res.error;
          
          }
         
        },
        err=> {
          setTimeout(()=> this.loading.idle(),2000)
          this.warn= '* Error '
        }  
        
        );
    this.service.CustomerRangeForm.reset();
 this.service.initializeCustomerRangeFormGroup();;
    //this.notificationService.success('Submitted successfully')
   
    }
  }
   onClear() {
    this.service.CustomerForm.reset();
    this.service.initializeFormGroup();
    this.notificationService.success('Submitted successfully')
  }
  onClose() {
   
    this.dialogRef.close();
    this.service.CustomerForm.reset();
    this.service.CustomerRangeForm.reset();
    this.service.initializeFormGroup();
    this.service.initializeCustomerRangeFormGroup();
    
  }
  onClear1() {
    this.service.CustomerRangeForm.reset();
    this.service.initializeCustomerRangeFormGroup();
    this.notificationService.success('Submitted successfully')
  }
  onClose1() {
    this.service.RangeForm.reset();
    this.service.initializeRangeFormGroup();
    this.dialogRef.close();
  }

 
 

}
