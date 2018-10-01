import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { User, UserEditModel } from '../Model/User';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private _appService: AppService,private route: ActivatedRoute, private router: Router) { }

  model = new UserEditModel;

  ngOnInit() {
  }

  onSubmit(form: any) {
    this.router.navigate(['/login']);
    this._appService.registerUser(this.model).subscribe((data: any) => {
      console.log(data);
    },
      err => { console.log(err)});
  }
}
