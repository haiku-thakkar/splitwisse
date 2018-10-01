import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { User } from '../Model/User';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userId: number;
  userData: User;

  constructor(private _appService: AppService, private route: ActivatedRoute, private router: Router) {
    this.userId = this.route.snapshot.params['id'];
    this._appService.getUserDetails(this.userId).subscribe((data: any) => {
      this.userData = data 
    });
  }

  ngOnInit() {
  }

  logout() {
    var a = confirm("Do you want to Logout...???");
    if (a) {
      this.router.navigate(['/login']);
    }
  }
}
