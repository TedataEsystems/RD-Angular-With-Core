
import { Component, OnInit, Inject } from '@angular/core';
import { from } from 'rxjs';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { DOCUMENT } from '@angular/common';
import { LoginserService } from '../../services/loginser.service';
import { NotificationService } from '../../services/notification.service';
import { Login } from '../../../Models/Logincls';
import { ConfigureService } from '../../services/configure.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)

  });
  warning=false;
  currentLang: string;
  public loginInvalid: boolean = false;
  private formSubmitAttempt: boolean = false;
  private returnUrl: string = '';
  loginmodel: Login = {
    userName: "",
    password: ""
  }
  constructor(private serlogin: LoginserService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private notser: NotificationService,
    private titleService: Title,
    private config: ConfigureService,

    @Inject(DOCUMENT) private document: Document
  ) {
    this.config.Logout();
    this.titleService.setTitle('TEData | VLAN');
    this.currentLang = localStorage.getItem("currentLang") as string || 'en';
    //this.translate.use(this.currentLang);


  }

  ngOnInit(): void {


  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.loginmodel.userName = this.form.value.username.trim();
    this.loginmodel.password = this.form.value.password;
    this.serlogin.getLogin(this.loginmodel).subscribe(res => {

      if (res.status == true) {
        localStorage.setItem("tokNum", res.token);
        localStorage.setItem("usernam", res.userData.userName);
        localStorage.setItem("userGroup", res.userData.userGroup);

        window.location.href = "/home"


        //  this.router.navigate(['/'], { relativeTo: this.route });
      }
      else {
        //this.notser.Warning("Invalid username or password!");
        this.warning=true;

      }

      // Retrieve
    }, err => {
      // this.notser.Warning("Invalid username or password!");
      this.warning=true;

    });


  }












}