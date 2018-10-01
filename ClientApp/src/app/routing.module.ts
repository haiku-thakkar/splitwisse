import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FriendComponent } from './friend/friend.component';
import { GroupComponent } from './group/group.component';
import { AddGroupComponent } from './add-group/add-group.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { AllExpenseComponent } from './all-expense/all-expense.component';


const routes: Routes = [
  {    path: '',    component: LoginComponent,    children: [{ path: 'login', component: LoginComponent }]  },
  {    path: 'register',component: RegisterComponent  },
  {    path: ':id/editUser', component: EditUserComponent  },
  {    path: ':id',    component: HomeComponent,    children: [
      {  path: '',    component: DashboardComponent  },
      {  path: 'home',  component: DashboardComponent  },
      {  path: 'allExpense',  component: AllExpenseComponent },
      {  path: 'friend/:id',  component: FriendComponent },
      {  path: 'group/:id',   component: GroupComponent  },
      {  path: 'GroupAdd',    component: AddGroupComponent } ]
  }
];


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes, { useHash: true })
  ],
  exports: [
    RouterModule
  ],
  declarations: []
})
export class RoutingModule { }
