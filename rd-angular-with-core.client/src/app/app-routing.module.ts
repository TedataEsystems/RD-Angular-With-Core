import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LogdataComponent } from './components/logdata/logdata.component';
import { RDFormComponent } from './components/rd-form/rd-form.component';
import { RDComponent } from './components/rd/rd.component';
import { UsersComponent } from './components/users/users.component';

import { LayoutComponent } from './shared/components/layout/layout.component';
import { LoginComponent } from './shared/components/login/login.component';

const routes: Routes = [
  {
    path:'login',
  component:LoginComponent,
 },
  {
    path:'',
    component: LoginComponent,
  },
  {
    path: '',
    component: LayoutComponent,


    children: [
   
    {
      path: 'home',
      component: RDComponent,

    },
    
    {
      path: 'logs',
      component: LogdataComponent,

    },
    {
      path: 'users',
      component: UsersComponent,

    }
  
    
    
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
