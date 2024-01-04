
import { Component, ViewChild, ElementRef, OnInit, TemplateRef, Input, Output, EventEmitter } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';

import { DeleteService } from '../../shared/services/delete.service';
//import { RequestSerService } from 'src/app/shared/services/request-ser.service';

import { ActivatedRoute, Router } from '@angular/router';
//import { saveAs } from 'file-saver';
import { Title } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { HttpClient, HttpResponse, HttpRequest, HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { of } from 'rxjs';
import { MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';


import { LogDataService } from '../../shared/services/log-data.service';
import { LogData } from '../../Models/LogData';
import { NotificationService } from '../../shared/services/notification.service';
import { ConfigureService } from '../../shared/services/configure.service';
import { ToastrService } from 'ngx-toastr';
import { LoadingService } from '../../shared/services/loading.service';

//const swal = require('sweetalert2')

@Component({
  selector: 'app-logdata',
  templateUrl: './logdata.component.html',
  styleUrls: ['./logdata.component.css']
})
export class LogdataComponent implements OnInit {
  searchKey: string = '';
  public Requetss: any[] = [];
  public RequetFilter: any[] = [];
 
  @ViewChild(MatSort) sort?: MatSort;
  @ViewChild(MatPaginator) paginator?: MatPaginator;
 
  displayedColumns2: string[] = ['id' , 'rD_Id','desciption', 
'tableName', 'actionType', 'userName',
'creationDate'];
  columnsToDisplay: string[] = this.displayedColumns2.slice();
  public reqs: LogData[] = [];
  public delreq: LogData = new LogData();
  LogDataList?: LogData[] = [];
  LogDataListTab?: LogData[] = [];
  valdata = ""; valuid = 0;
  dataSource = new MatTableDataSource<any>();
  delpic: any;
  listName: string = '';
  selected: boolean = false;
  param1: any; settingtype = '';
  esptFlag:boolean=false;

  simflag=true;
  constructor(private dialog: MatDialog,
    private DeleteService: DeleteService,/*private reqser: RequestSerService ,*/
    private route: ActivatedRoute,
    private router: Router, private notser: ToastrService,
    private config: ConfigureService,
    private supportser: LogDataService , private titleService: Title, private _http: HttpClient,
    private _bottomSheet: MatBottomSheet,
    private loading:LoadingService
  ) {
    // var groupval= this.config.UserTeam();
    // if(groupval=="ESPT") this.esptFlag=true;

    
    // if(groupval?.toLocaleLowerCase() !="presalesfiber_presale")
    //  {
    //   this.notser.Warning("not permitted")
    //   this.router.navigate(['/'] );
    //  }
    // this.config.IsAuthentecated();
    this.titleService.setTitle('RD logdata Request');

  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort as MatSort; 
   this.dataSource.paginator = this.paginator as MatPaginator;
   
  }

  //http://172.29.29.8:2021/api/simdata/DownloadEmptyExcel
  //AddFromFile 



  sortColumnDef: string = "id";
  SortDirDef: string = 'DESC';
  pagesizedef: number = 25;
  pageIn = 0;
  previousSizedef = 25;
  requestid = 1;
  // searchKey!:string; DESC reqid

  
  ngOnInit() {

    // var team=  this.config.UserTeam();
    // if(team?.toLocaleLowerCase()!='esp')
    // {
    //   this.notser.Warninging("not permission");
    //    this.router.navigate(['/'] );    
    // }
    this.getRequestdata(50, 1, '', 'id', 'DESC', true);

  }
  getRequestdata(pageSize: number, pageNum: number, search: string, sortColumn: string, sortDir: string, initflag: boolean = false) {
    this.loading.busy();

    this.supportser.getLogOption(pageSize, pageNum, search, sortColumn, sortDir).subscribe(res => {
    

      if (res.status == true) {

        this.loading.idle();
        //   this.dataSource.paginator.length=10;
        this.Requetss = res.result.data;
        this.Requetss.length = res.result.totalrecords;
        if (initflag)
          this.RequetFilter = this.Requetss;


        this.dataSource = new MatTableDataSource<any>(this.Requetss);
        //this.dataSource._updateChangeSubscription();
        this.dataSource.paginator = this.paginator as MatPaginator;
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

    this.supportser.getLogOption(pageSize, pageNum, search, sortColumn, sortDir).subscribe(res => {
   

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
    }, err => {
      if (err.status == 401)
        this.router.navigate(['/login'], { relativeTo: this.route });
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
  lastcol: string = 'id';
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
  onCreate() {//this.requestid
    this.router.navigate(['/logdataRequestForm']);
  }

  onEdit(r: any) {

    this.router.navigate(['/logdataRequestForm'], { queryParams: { id: r.id } });

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
 

  ////////////////Export excel/////////////////////////////////////////
  // ExportTOViewData()
  // {
  //   // const ws: XLSX.WorkSheet=XLSX.utils.table_to_sheet((this.table as ElementRef).nativeElement);
  //   // const wb: XLSX.WorkBook = XLSX.utils.book_new();
  //   // XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  //   this.supportser.DownloadExporExcel().subscribe(res=>{

  //     const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
  //     const file = new File([blob],  'ViewSupportrequest' + '.xlsx', { type: 'application/vnd.ms.excel' });
  //     saveAs(file);

  //   },err=>{
  //     if(err.status==401)
  //     this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //     else 
  //     this.notser.Warninging("! Fail")

  //   });
  //   /* save to file */ 
  //   //XLSX.writeFile(wb, 'SheetJS.xlsx');

  // }
  @ViewChild('TABLE') table?: ElementRef;
  Ids: string[] = [];
  // ExportTOExcel()
  // {
  //   // const ws: XLSX.WorkSheet=XLSX.utils.table_to_sheet((this.table as ElementRef).nativeElement);
  //   // const wb: XLSX.WorkBook = XLSX.utils.book_new();
  //   // XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  //   this.supportser.DownloadEmptyExcel().subscribe(res=>{

  //     const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
  //     const file = new File([blob],  'Supportrequest' + '.xlsx', { type: 'application/vnd.ms.excel' });
  //     saveAs(file);

  //   },err=>{
  //     if(err.status==401)
  //     this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //     else 
  //     this.notser.Warninging("! Fail")

  //   });
  //   /* save to file */ 
  //   //XLSX.writeFile(wb, 'SheetJS.xlsx');

  // }
  // ExportTOExcelEdit()
  // {
  //   debugger;
  //   if(this.isall){
  //     this.supportser.DownloadExcelAllEdit().subscribe(res=>{

  //       const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
  //       const file = new File([blob],  'Supportrequestedit' + '.xlsx', { type: 'application/vnd.ms.excel' });
  //       saveAs(file);

  //     },err=>{
  //       if(err.status==401)
  //       this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //       else 
  //       this.notser.Warninging("! Fail")

  //     });
  //   }
  //   else{
  //   if(this.Ids.length==0){
  // this.notser.Warninging('select rows !')
  // return;
  //   }
  //   let idsmodel={ids:this.Ids}
  //   // const ws: XLSX.WorkSheet=XLSX.utils.table_to_sheet((this.table as ElementRef).nativeElement);
  //   // const wb: XLSX.WorkBook = XLSX.utils.book_new();
  //   // XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  //   this.supportser.DownloadEditExcel(idsmodel).subscribe(res=>{

  //     const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
  //     const file = new File([blob],  'Supportrequestedit' + '.xlsx', { type: 'application/vnd.ms.excel' });
  //     saveAs(file);

  //   },err=>{
  //     if(err.status==401)
  //     this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //     else 
  //     this.notser.Warninging("! Fail")

  //   });
  //   /* save to file */ 
  //   //XLSX.writeFile(wb, 'SheetJS.xlsx');
  // }
  // }
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

  //   upLoadF(){

  //     const fd = new FormData();
  //     debugger;

  //     fd.append(this.param, this.fileuploaded);
  //     this.fileAttr = 'Choose File';
  //     this.supportser.updateFromFile(fd).subscribe(x=>{

  //       if(x.status==true){

  //   //  this.notser.success('uploaded and detail is '+x.data);
  //     this.getRequestdata(25,1,'',this.service.colname,this.service.coldir);
  //     this.Ids=[];
  //     this._bottomSheet.dismiss();
  //     swal.fire(
  //       '!uploaded ',
  //       x.data,
  //       'success'
  //     )

  //   } else{
  //     swal.fire(
  //       '!not uploaded ',
  //       x.error,
  //       'error'
  //     )
  //    // this.notser.Warninging(x.error);
  //   }
  //   this.resetfile();

  //     },err=>{
  //       if(err.status==401)
  //       this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //       else 
  //       this.notser.Warninging("! Fail")

  //  this.resetfile();

  //     })
  //   }
  //      upLoad(){

  //     const fd = new FormData();
  //     debugger;

  //     fd.append(this.param, this.fileuploaded);
  //     this.fileAttr = 'Choose File';  
  //     this.supportser.addFromFile(fd).subscribe(x=>{

  //       if(x.status==true){
  //   //  this.notser.success('uploaded and detail is '+x.data);
  //     this.getRequestdata(25,1,'',this.service.colname,this.service.coldir);

  //     this._bottomSheet.dismiss();
  //     swal.fire(
  //       '!uploaded ',
  //       x.data,
  //       'success'
  //     )
  //   } else{
  //     this.notser.Warninging(x.error);
  //     swal.fire(
  //       '!not uploaded ',
  //       x.error,
  //       'error'
  //     )
  //   }
  //   this.resetfile();

  //     },err=>{
  //       if(err.status==401)
  //       this.router.navigate(['/loginuser'], { relativeTo: this.route });
  //       else 
  //       this.notser.Warninging("! Fail")

  //  this.resetfile();

  //     })
  //   }
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
    //(this.fileInput as ElementRef).nativeElement.value = "";


  }
  import() {

  }
  importEmpty() {

  }
  Upload() {

  }
}



