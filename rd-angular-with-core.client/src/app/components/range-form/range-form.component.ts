import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { RDDataService } from '../../shared/services/RDData.service';
import { RangeData } from '../../Models/RangeData';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';



@Component({
  selector: 'app-range-form',
  templateUrl: './range-form.component.html',
  styleUrls: ['./range-form.component.css']
})
export class RangeFormComponent implements OnInit {

 warn ="";
  constructor(private notificationService : ToastrService,private loading:LoadingService,public service:RDDataService, public dialogRef: MatDialogRef<RangeFormComponent>) { }

  ngOnInit(): void {
    this.service.getAll();
  }
  RangeData:RangeData = new RangeData();
 
  onSubmit() {
    this.loading.busy();
    const p = {...this.RangeData, ...this.service.RangeForm.value}
    if (this.service.RangeForm.valid) {
     
 
        this.service.AddRange(p).subscribe(res =>{
         
          if(res.status){
            setTimeout(()=> this.loading.idle(),2000)
            this.notificationService.success('Submitted successfully');
            this.onClose();
          }
          else{
            
            this.loading.idle()
          this.warn= ' * ' + res.error;
          
          }
         
        },
        err=> {
          this.loading.idle();
          this.warn= '* Error '
        }  
        
        );
    this.service.RangeForm.reset();
    this.service.initializeRangeFormGroup();
    //this.notificationService.success('Submitted successfully')
   
    }
  }
   onClear() {
    this.service.RangeForm.reset();
    this.service.initializeRangeFormGroup();
    this.notificationService.success('Submitted successfully')
  }
  onClose() {
    this.service.RangeForm.reset();
    this.service.initializeRangeFormGroup();
    this.dialogRef.close();
  }


 
 

}
