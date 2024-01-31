import { NgModule } from '@angular/core';
import { CommonModule  } from '@angular/common';
import { Route, RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
const routes:Routes =[
  {path: '' ,component:HomeComponent},
  {path: 'not-found' , component:NotFoundComponent},
  {path: 'account', loadChildren:()=>import('./account/account.module').then(module=>module.AccountModule)},
  {path: '**',component:NotFoundComponent,pathMatch: 'full'}
];


@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports:[RouterModule]
})
export class AppRoutingModule { }
