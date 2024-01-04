import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { ConfigureService } from './configure.service';
import * as _ from 'lodash';
import { RDData } from '../../Models/RDData';
import { RangeData } from '../../Models/RangeData';
@Injectable({
  providedIn: 'root'
})
export class RDDataService {
  private apiUrl: string;
  private headers = new HttpHeaders();
  private subject = new Subject<any>();
  constructor(private config: ConfigureService, private http: HttpClient, private fb: FormBuilder) {
    this.apiUrl = config.ApiUrl() + 'RDData';
    this.headers = this.headers.set('Authorization',"Bearer "+ this.config.UserToken());
  }
  formData: RDData = new RDData();
  rangeData: RangeData = new RangeData();

  CustomerForm: FormGroup = this.fb.group({
    id: 0,
    vrfName: ['',  [Validators.required,this.WhitespacesInvalid]],
    customerName: ['', [Validators.required,this.WhitespacesInvalid]]
  });
  
 
  public WhitespacesInvalid(control: FormControl) {
    const isWhitespace = (control.value || '').trim().length === 0;
    const isValid = !isWhitespace;
    return isValid ? null : { 'whitespaceInvalid': true };
  }
  initializeFormGroup() {
    this.CustomerForm.setValue({
      id: 0,
      customerName: '',
      vrfName: ''
    });
  }
  populateForm(p: any) {
    this.CustomerForm.patchValue(_.omit(p));
  }
  resetForm(form: FormGroup) {
    form.reset();
    this.formData = new RDData();
  }
  RangeForm: FormGroup = this.fb.group({
    min:  [0, Validators.required],
    max:  [0, Validators.required]
   
  });
  initializeRangeFormGroup() {
    this.RangeForm.setValue({
      min:0,
      max:0
    });
  }
  CustomerRangeForm: FormGroup = this.fb.group({
    min:  [0, Validators.required],
    max:  [0, Validators.required],
    vrfName: ['',  [Validators.required,this.WhitespacesInvalid]],
    customerName: ['', [Validators.required,this.WhitespacesInvalid]]
   
  });
  initializeCustomerRangeFormGroup() {
    this.RangeForm.setValue({
      min:0,
      max:0,
      customerName: '',
      vrfName: ''
    });
  }
  populateRangeForm(p: any) {
    this.RangeForm.patchValue(_.omit(p));
  }
  resetRangeForm(form: FormGroup) {
    form.reset();
    this.rangeData = new RangeData();
  }

  public getAll(): Observable<any> {

    this.headers = this.headers.set('Authorization',"Bearer "+ this.config.UserToken());
    return this.http.get<any>(this.apiUrl , {headers:this.headers});
  }


  public getByOption(pageSize: number, pageNum: number, search: string = "", sortColumn: string = "id", sortDir: string = 'ASC') {
     this.headers =this.headers.set('Authorization',"Bearer "+ this.config.UserToken()); 
    var urlval = `${this.apiUrl}?=pagesize=${pageSize}&pagenumber=${pageNum}&sortcolumn=${sortColumn}&sortcolumndir=${sortDir}&searchvalue=${search}`;
     return this.http.get<any>(urlval,{headers: this.headers});
  }


  public getById(id: any) {
    return this.http.get<any>(this.apiUrl + '/' + id , {headers : this.headers})
  }


  public Add(model: any) {
    return this.http.post<any>(this.apiUrl , model , {headers : this.headers});
    // return this.http.post<any>(this.apiUrl, model);

  }
  public AddCustomerRange(model: any) {
    return this.http.post<any>(`${this.apiUrl}/CreateCustomerRange` , model , {headers : this.headers});
    // return this.http.post<any>(this.apiUrl, model);

  }
  public AddRange(model: RangeData) {
    return this.http.post<any>(`${this.apiUrl}/addrange` , model , {headers : this.headers});
    // return this.http.post<any>(`${this.apiUrl}/addrange` , model);

  }
  // public getAllComments(parm : any) : Observable<any>
  // {
  //   this.headers = this.headers.set('Authorization',"Bearer "+ this.config.UserToken());
  //   return this.http.get<any>(this.commentUrl +'/GetAllComments/' + parm , {headers:this.headers});
  // }
  // public AddComment(model : any)
  // {
  //   return this.http.post<any>(this.commentUrl , model , {headers : this.headers});
  // }

  public Update(model: any) {
    return this.http.post<any>(this.apiUrl + '/UpdateRDData', model, { headers: this.headers });
  }
 public DownloadExcel(){
  return this.http.get(this.apiUrl + '/export', {responseType: 'blob',headers: this.headers});
 }
 public DownloadFreeExcel(){
  return this.http.get(this.apiUrl + '/ExportFree', {responseType: 'blob',headers: this.headers});
 }



  public Remove(Val: number) {
    return this.http.get<any>(this.apiUrl + "/RemoveRDData/" + Val, { headers: this.headers });
    // return this.http.get<any>(this.apiUrl + "/RemoveRDData/" + Val);

  }

  sendMessage(message: any) {
    this.subject.next(message);
  }

  clearMessages(message: any) {
    this.subject.next(message);
  }

  onMessage(): Observable<any> {
    return this.subject.asObservable();
  }


}
