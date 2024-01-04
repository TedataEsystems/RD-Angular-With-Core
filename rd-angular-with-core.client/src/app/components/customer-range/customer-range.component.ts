import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { RDDataService } from '../../shared/services/RDData.service';
import { RangeData } from '../../Models/RangeData';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';


@Component({
  selector: 'app-customer-range',
  templateUrl: './customer-range.component.html',
  styleUrls: ['./customer-range.component.css']
})
export class CustomerRangeComponent implements OnInit {
   
     warn ="";
      constructor(private notificationService : ToastrService,private loading:LoadingService,public service:RDDataService, public dialogRef: MatDialogRef<CustomerRangeComponent>) { }
    
      ngOnInit(): void {
        this.service.getAll();
      }
      RangeData:RangeData = new RangeData();
     
      onSubmit() {
        this.loading.busy();
        // const p = {...this.RangeData, ...this.service.CustomerRangeForm.value}
        if (this.service.CustomerRangeForm.valid) {
         
     
            this.service.AddCustomerRange(this.service.CustomerRangeForm.value).subscribe(res =>{
             
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
    