
import { Component, ViewChild, ElementRef, OnInit, TemplateRef, Input, Output, EventEmitter } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';

//import { RequestSerService } from '../../shared/services/request-ser.service';
import { RDData } from '../../Models/RDData';

import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../shared/services/notification.service';
import { DeleteService } from '../../shared/services/delete.service';
import { ConfigureService } from '../../shared/services/configure.service';
import { saveAs } from 'file-saver';
import { Title } from '@angular/platform-browser';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { HttpClient, HttpResponse, HttpRequest, HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { of } from 'rxjs';
import { MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';


import { RDFormComponent } from '../rd-form/rd-form.component';
import { RangeFormComponent } from '../range-form/range-form.component';
import { CustomerRangeComponent } from '../customer-range/customer-range.component';
import { RDDataService } from '../../shared/services/RDData.service';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';

//const swal = require('sweetalert2')

@Component({
  selector: 'app-rd',
  templateUrl: './rd.component.html',
  styleUrls: ['./rd.component.css']
})
export class RDComponent implements OnInit {
  searchKey: string = '';
  public Requetss: any[] = [];
  public RequetFilter: any[] = [];

  @ViewChild(MatSort) sort?: MatSort;
  @ViewChild(MatPaginator) paginator?: MatPaginator;
  displayedColumns2: string[] = [ 'action', 'rD_Id', 'customerName', 'vrfName', 'creationDate', 'modificationDate', 'createdBy', 'modifyiedBy'
  ];
  columnsToDisplay: string[] = this.displayedColumns2.slice();
  public reqs: RDData[] = [];
  public delreq: RDData = new RDData();
  RDDataList?: RDData[] = [];
  RDDataListTab?: RDData[] = [];
  valdata = ""; valuid = 0;
  dataSource = new MatTableDataSource<any>();
  delpic: any;
  listName: string = '';
  selected: boolean = false;
  param1: any; settingtype = '';
  esptFlag:boolean=false;
  isNotAdmin=false;

  simflag=true;
  constructor(private dialog: MatDialog,
    private DeleteService: DeleteService,/*private reqser: RequestSerService ,*/
    private route: ActivatedRoute,
    private router: Router, private notser: ToastrService,
    private config: ConfigureService,
    private RDDataser: RDDataService, private titleService: Title, private _http: HttpClient,
    private _bottomSheet: MatBottomSheet,
    private loading:LoadingService
  ) {
    var teamval= localStorage.getItem("userGroup");
    
    if(teamval?.toLocaleLowerCase() != 'admin'){
   this.isNotAdmin=true;  
    
 }
    this.config.IsAuthentecated();
    this.titleService.setTitle('TEData | VLAN');

  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort as MatSort; 
   this.dataSource.paginator = this.paginator as MatPaginator;
   
  }

  //http://172.29.29.8:2021/api/simdata/DownloadEmptyExcel
  //AddFromFile 



  sortColumnDef: string = "rD_Id"; //"id";
  SortDirDef: string = 'ASC';
  pagesizedef: number = 25;
  pageIn = 0;
  previousSizedef = 25;
 

  
  ngOnInit() {

    this.getRequestdata(30, 1, '', 'rD_Id', 'asc', true);

  }
  getRequestdata(pageSize: number, pageNum: number, search: string, sortColumn: string, sortDir: string, initflag: boolean = false) {
    this.loading.busy();

    this.RDDataser.getByOption(pageSize, pageNum, search, sortColumn, sortDir).subscribe(res => {
 

      if (res.status == true) {

     
        //   this.dataSource.paginator.length=10;
        this.Requetss = res.result.data;
        this.Requetss.length = res.result.totalrecords;
        if (initflag)
          this.RequetFilter = this.Requetss;

   

        this.dataSource = new MatTableDataSource<any>(this.Requetss);
        //this.dataSource._updateChangeSubscription();
        this.dataSource.paginator = this.paginator as MatPaginator;
        this.loading.idle();
      }
      else
        this.notser.warning(res.error)
        this.loading.idle();
    }, err => {

      if (err.status == 401)
        this.router.navigate(['/login'], { relativeTo: this.route });
      else
        this.notser.warning("! Fail")
        this.loading.idle();


    })
  }
  getRequestdataNext(cursize: number, pageSize: number, pageNum: number, search: string, sortColumn: string, sortDir: string) {
    this.loading.busy();

    this.RDDataser.getByOption(pageSize, pageNum, search, sortColumn, sortDir).subscribe(res => {
    

      if (res.status == true) {

      
        //   this.dataSource.paginator.length=10;
        this.Requetss.length = cursize;
        this.Requetss.push(...res.result.data);
        //this.Requetss = res.result.data;
        this.Requetss.length = res.result.totalrecords;
        this.dataSource = new MatTableDataSource<any>(this.Requetss);
        this.dataSource._updateChangeSubscription();
        this.dataSource.paginator = this.paginator as MatPaginator;
        this.loading.idle();
      }
      else
        this.notser.warning(res.error)

        this.loading.idle();
    }, err => {
      if (err.status == 401)
        this.router.navigate(['/loginuser'], { relativeTo: this.route });
      else
        this.notser.warning("! Fail");
        this.loading.idle();

    })
  }
 

  pageChanged(event: any) {
    this.loading.busy();
    this.config.pIn = event.pageIndex;
    this.pageIn = event.pageIndex;
    this.pagesizedef = event.pageSize;
    let pageIndex = event.pageIndex;
    let pageSize = event.pageSize;
    let previousSize = pageSize * pageIndex;
    this.previousSizedef = previousSize;
    this.getRequestdataNext(previousSize, pageSize, pageIndex + 1, '', this.sortColumnDef, this.SortDirDef)
    let previousIndex = event.previousPageIndex;
    //  let previousSize = pageSize * pageIndex; 
    //  this.getNextData(previousSize, (pageIndex).toString(), pageSize.toString());
  }
  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }
  applyFilter() {
    let searchData = this.searchKey.trim().toLowerCase();
    if (searchData != "")
      this.getRequestdata(25, 1, searchData, this.sortColumnDef, this.SortDirDef);
    else {
      this.Requetss = this.RequetFilter;
      this.dataSource.data = this.RequetFilter;
    }
  }
  lastcol: string = 'rD_Id'; //'id';
  lastdir: string = 'asc';
  sortData(sort: any) {
    if (this.config.pIn != 0)
      window.location.reload();
    if (this.lastcol == sort.active && this.lastdir == sort.direction) {
      if (this.lastdir == 'asc')
        sort.direction = 'desc';
      else
        sort.direction = 'asc';

    }

    this.lastcol = sort.active; this.lastdir = sort.direction;

    var c = this.pageIn;
    this.getRequestdata(25, 1, '', sort.active, this.lastdir);
  }
 
  isall: boolean = false;
  selectallviewflag = false;
  onselectcheckall(event: any) {
    if (event.checked) {
      this.isall = true; this.selectallviewflag = true;

    }
    else {
      this.isall = false; this.selectallviewflag = false;
    }

  }

  onselectcheck(event: any, r: any) {
    if (event.checked) {
      this.Ids.push(r.id.toString());
      // this.contentEditable = true;
    }
    else {
      const index: number = this.Ids.indexOf(r.id.toString());
      if (index !== -1) {
        this.Ids.splice(index, 1);
      }
    }


  }
  onDelete($key:any){
    this.DeleteService.openConfirmDialog()
    .afterClosed().subscribe(res =>{
      if(res){
        this.RDDataser.Remove($key).subscribe(
          res=>{        
        this.notser.success('Deleted successfully!');
        this.getRequestdata(25,1,'','rD_Id','asc',true);
        //this.getRequestdata(25, 1, '', 'id', 'asc', true);
        });
        
      }
    });
  }


  @ViewChild('TABLE') table?: ElementRef;
  Ids: string[] = [];
  ExportTOExcel(){
  
    this.RDDataser.DownloadExcel().subscribe(res=>{
      
      const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
      // const file = new File([blob],  'ReservedRD.xlsx', { type: 'application/vnd.ms.excel' });
      const file = 'ReservedRD.xlsx';
      saveAs(blob,file);
      
    },err=>{
      if(err.status==401)
      this.router.navigate(['/login'], { relativeTo: this.route });
     
    });
  }
  ExportFreeTOExcel(){
  
    this.RDDataser.DownloadFreeExcel().subscribe(res=>{
      
      const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
      // const file = new File([blob],  'FreeRD.xlsx', { type: 'application/vnd.ms.excel' });
      const file = 'FreeRD.xlsx';
      saveAs(blob,file);
      
    },err=>{
      if(err.status==401)
      this.router.navigate(['/login'], { relativeTo: this.route });
     
    });
  }
  @Input() param = 'file';


  @ViewChild('LIST') template!: TemplateRef<any>;
  @ViewChild('LISTF') templateF!: TemplateRef<any>;
  @ViewChild('fileInput') fileInput?: ElementRef;
  fileAttr = 'Choose File';
  fileAttrF = 'Choose File';

  fileuploaded: any;

  uploadFileEvt(imgFile: any) {
    this.fileuploaded = imgFile.target.files[0];

  

    if (imgFile.target.files && imgFile.target.files[0]) {
      this.fileAttr = '';
      Array.prototype.forEach.call(imgFile.target.files, (file) => {
        this.fileAttr += file.name + ' - ';
      });
      // Array.from(imgFile.target.files).forEach((file:File)=> {
      //   this.fileAttr += file.name + ' - ';

      // });



      // HTML25 FileReader API
      let reader = new FileReader();
      reader.onload = (e: any) => {
        let image = new Image();
        image.src = e.target.result;
        image.onload = rs => {
          let imgBase64Path = e.target.result;
        };
      };
      reader.readAsDataURL(imgFile.target.files[0]);

      // Reset if duplicate image uploaded again
      (this.fileInput as ElementRef).nativeElement.value = "";
    } else {
      this.fileAttr = 'Choose File';
    }
  }

  uploadFileEvtF(imgFile: any) {
    this.fileuploaded = imgFile.target.files[0];

    

    if (imgFile.target.files && imgFile.target.files[0]) {
      this.fileAttr = '';
      Array.prototype.forEach.call(imgFile.target.files, (file) => {
        this.fileAttr += file.name + ' - ';
      });
      // Array.from(imgFile.target.files).forEach((file:File)=> {
      //   this.fileAttr += file.name + ' - ';

      // });



      // HTML25 FileReader API
      let reader = new FileReader();
      reader.onload = (e: any) => {
        let image = new Image();
        image.src = e.target.result;
        image.onload = rs => {
          let imgBase64Path = e.target.result;
        };
      };
      reader.readAsDataURL(imgFile.target.files[0]);

      // Reset if duplicate image uploaded again
      (this.fileInput as ElementRef).nativeElement.value = "";
    } else {
      this.fileAttr = 'Choose File';
    }
  }

 
  openBottomSheet() {
    this._bottomSheet.open(this.template, {
      panelClass: 'botttom-dialog-container',
      disableClose: true


    });

  }

  openBottomSheetedit() {
    this._bottomSheet.open(this.templateF, {
      panelClass: 'botttom-dialog-container',
      disableClose: true


    });

  }
  close() {
    this.fileAttr = 'Choose File';
    this.resetfile();
    this._bottomSheet.dismiss();
    //  this.dialogRef.close();
  }


  resetfile() {
    this.fileAttr = 'Choose File';
  


  }
  import() {

  }
  importEmpty() {

  }
  Upload() {

  }
  onEdit(row:RDData){
    
    this.RDDataser.populateForm(row);
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = "50%";
    dialogConfig.height = '250px';
    dialogConfig.data= {dialogTitle: "Update"};
    dialogConfig.panelClass='modals-dialog';
    this.dialog.open(RDFormComponent,dialogConfig).afterClosed().subscribe(result => {
      this.getRequestdata(30,1,'','rD_Id','asc',true);
      //this.getRequestdata(30, 1, '', 'id', 'asc', true);
    });
  }
  onCreate() {
    this.RDDataser.initializeFormGroup();
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data= {dialogTitle: "Create"};
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '50%';
    dialogConfig.height = '397px';
    dialogConfig.panelClass = 'modals-dialog';
   
    this.dialog.open(RDFormComponent,dialogConfig).afterClosed().subscribe(result => {
      this.getRequestdata(30,1,'','rD_Id','asc',true);
      //this.getRequestdata(30, 1, '', 'id', 'asc', true);
    });
  }
  onCreateCustomerRange() {
    this.RDDataser.initializeFormGroup();
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data= {dialogTitle: "Create"};
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '50%';
    dialogConfig.height = '250px';
    dialogConfig.panelClass = 'modals-dialog';
    
    this.dialog.open(CustomerRangeComponent,dialogConfig).afterClosed().subscribe(result => {
      this.getRequestdata(30,1,'','rD_Id','asc',true);
      //this.getRequestdata(30, 1, '', 'id', 'asc', true);
    });
  }
  onRangeCreate() {
    this.RDDataser.initializeRangeFormGroup();
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '50%';
    dialogConfig.height = '250px';
    dialogConfig.panelClass = 'modals-dialog';
    this.dialog.open(RangeFormComponent,dialogConfig).afterClosed().subscribe(result => {
      this.getRequestdata(30,1,'','rD_Id','asc',true);
      //this.getRequestdata(30, 1, '', 'id', 'asc', true);
    });
  }
}



