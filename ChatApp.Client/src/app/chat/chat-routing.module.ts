import { Router, RouterModule, Routes } from "@angular/router";
import { MainComponent } from "./main/main.component";
import { Injectable, NgModule } from "@angular/core";

const routes: Routes = [
  { path:'',component:MainComponent }
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
