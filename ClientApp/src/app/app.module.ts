import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RoutingModule } from './/routing.module';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AppService } from './service/app.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FriendComponent } from './friend/friend.component';
import { GroupComponent } from './group/group.component';
import { AddGroupComponent } from './add-group/add-group.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { AllExpenseComponent } from './all-expense/all-expense.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    NavMenuComponent,
    DashboardComponent,
    FriendComponent,
    GroupComponent,
    AddGroupComponent,
    EditUserComponent,
    AllExpenseComponent     
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    HttpModule,
    HttpClientModule,   
    RoutingModule   
  ],
  providers: [AppService],
  bootstrap: [AppComponent]
})
export class AppModule { }
