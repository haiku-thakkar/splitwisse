import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { User, UserEditModel } from '../Model/User';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {

  model = new UserEditModel();
  userId: number;

  constructor(private _appService: AppService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.userId = this.route.snapshot.params['id'];
    this._appService.getUserDetails(this.userId).subscribe((data: any) => {
      this.model = data
    });
  }

  onSubmit(form: any) {
    this._appService.editUserDetail(this.model).subscribe();
    this.router.navigate(['/', this.userId, 'home']);
  }

  btnCancel(event: Event) {
    event.preventDefault();
    this.router.navigate(['/', this.userId, 'home']);
  }
}
