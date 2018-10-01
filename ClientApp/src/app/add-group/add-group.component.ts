import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { User, UserEditModel } from '../Model/User';
import { Group } from '../Model/Group';

@Component({
  selector: 'app-add-group',
  templateUrl: './add-group.component.html',
  styleUrls: ['./add-group.component.css']
})
export class AddGroupComponent implements OnInit {

  UserId: number;
  user: UserEditModel;
  friends: UserEditModel[];
  addFriend: any[] = [];
  groupName: string;
  current = 0;
  log = '';
  group = new Group();

  constructor(private _appService: AppService, private route: ActivatedRoute) { }

  ngOnInit() {

    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });
    console.log(this.UserId);

    this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
      this.friends = data, console.log(this.friends)
    });

    this._appService.getUserDetails(this.UserId).subscribe((data: any) => {
      this.user = data;
      this.log += ` ${data.userName}\n `;
      this.addFriend.push(data.userId);
    });
    
  }

  logDropdown(id: number): void {
    var user = this.friends.find((item: any) => item.userId === +id);

    var userExist = this.addFriend.find((item: any) => item === +user.userId);
  
    if (userExist == null) {
      this.log += `${user.userName}\n `;
      this.addFriend.push(user.userId);
    }

  }

  clear(): void {
    this.log = '';
    this.addFriend = [];
    this.log += ` ${this.user.userName}\n `;
    this.addFriend.push(this.user.userId);
   
  }

  onSubmit() {
   
    this.group.groupName = this.groupName;
    this.group.creatorId = this.user.userId;
    this.group.members = this.addFriend;
    this.group.createdDate = new Date().toLocaleString();
    console.log(this.group);

    this._appService.addGroup(this.group).subscribe((data: any) => {
      console.log(data);
    },
      err => { console.log(err) });

    this.groupName = "";
    this.clear();
    this.current = 0;
  }


}
