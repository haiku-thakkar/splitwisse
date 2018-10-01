import { Component, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { UserEditModel } from '../Model/User';
import { GroupResponse, Grpmember } from '../Model/Group';
import { Balance, Settle } from '../Model/Settle';
import { Transaction } from '../Model/Transaction';
import { AddBill, member } from '../Model/AddBill';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  @ViewChild('close') close: ElementRef;
  
  @ViewChild('closebill') closebill: ElementRef;
  
  UserId: number;
  UserName: string;
  user=new UserEditModel();
  friends: UserEditModel[];
  groups: GroupResponse[] = [];
  friendId: number;
  groupId: number;
  amount: number;
  settleRecords: Balance[] = [];
  payment=new Transaction();

  oweData: number=0;
  owesData: number=0;
  total: number = 0;
  allSettleRecords: Balance[] = [];
  TotalBalance: any[] = [];
  details: Grpmember[] = [];

  AddBillModel = new AddBill();
  billPayer = new member();
  billPayers: member[] = [];
  sharedMembers: member[] = [];
  totalAmount: number;
  Members: any[] = [];
  SharedArray: any[] = [];
  PayersArray: any[] = [];
  memberCount: number;
  sharedAmount: number;
  settlements: Settle[] = [];
  payer: member[] = [];
  payee: member[] = [];
  showMultiplePayer: boolean = false;

  constructor(private _appService: AppService, private route: ActivatedRoute, private renderer: Renderer2) {
    this.UserId = this.route.snapshot.params['id'];

  }

  ngOnInit() {

    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });

    this._appService.getSingleFriendDetail(this.UserId).subscribe((data: any) => {
      this.user = data;
      this.UserName = data.userName;
      let x = { "id": data.userId, "name": data.userName };
      this.SharedArray.push(x);
      this.PayersArray.push(x);
      this.Members.push(x);
    });

    this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
      this.friends = data;
      for (var i = 0; i < this.friends.length; i++) {
        let x = { "id": this.friends[i].userId, "name": this.friends[i].userName };
        this.Members.push(x);
      }
    });
    
    this._appService.getAllSettldata(this.UserId).subscribe((data: any) => {
      this.allSettleRecords = data;

      this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
        this.friends = data;

        for (var i = 0; i < this.friends.length; i++) {
          this.TotalBalance[i] = { "id": this.friends[i].userId, "name": this.friends[i].userName, "balance": Number(0) };
        }

        for (var i = 0; i < this.allSettleRecords.length; i++) {
          if (this.allSettleRecords[i].amount != 0) {
            if (this.allSettleRecords[i].receiver_id == this.UserId) {
              var user = this.TotalBalance.find(x => x.id == this.allSettleRecords[i].payer_id);
              if (this.allSettleRecords[i].amount > 0) {
                user.balance = user.balance + this.allSettleRecords[i].amount;
                let x = new Grpmember();
                x.id = user.id;
                x.name = this.allSettleRecords[i].payerName + " owe " + this.allSettleRecords[i].amount + " to " + this.allSettleRecords[i].receiverName + " for " + this.allSettleRecords[i].groupName;
                this.details.push(x);
              }
              else {
                user.balance = user.balance - Math.abs(this.allSettleRecords[i].amount);
                let x = new Grpmember();
                x.id = user.id;
                x.name = this.allSettleRecords[i].receiverName + " owe " + Math.abs(this.allSettleRecords[i].amount) + " to " + this.allSettleRecords[i].payerName + " for " + this.allSettleRecords[i].groupName;
                this.details.push(x);
              }
            }
            else {
              var user = this.TotalBalance.find(x => x.id == this.allSettleRecords[i].receiver_id);
              if (this.allSettleRecords[i].amount > 0) {
                user.balance = user.balance - this.allSettleRecords[i].amount;
                let x = new Grpmember();
                x.id = user.id;
                x.name = this.allSettleRecords[i].payerName + " owe " + this.allSettleRecords[i].amount + " to " + this.allSettleRecords[i].receiverName + " for " + this.allSettleRecords[i].groupName;
                this.details.push(x);
              }
              else {
                user.balance = user.balance + Math.abs(this.allSettleRecords[i].amount);
                let x = new Grpmember();
                x.id = user.id;
                x.name = this.allSettleRecords[i].receiverName + " owe " + Math.abs(this.allSettleRecords[i].amount) + " to " + this.allSettleRecords[i].payerName + " for " + this.allSettleRecords[i].groupName;
                this.details.push(x);
              }
            }
          }
        }

        for (var i = 0; i < this.TotalBalance.length; i++) {
          if (this.TotalBalance[i].balance < 0) {
            this.oweData = this.oweData + Math.abs(this.TotalBalance[i].balance);
          }
          else {
            this.owesData = this.owesData + this.TotalBalance[i].balance;
          }
        }
        this.total = this.owesData - this.oweData;
        
      });
      console.log(this.allSettleRecords);
      console.log(this.TotalBalance);
      console.log(this.details);
    });
  }

  selectFriend(id: number) {
    var x = this.friends.find(item => item.userId === +id);
    this.friendId = x.userId;

    document.getElementById("receiver").innerText = x.userName;

    this._appService.getCommenGroups(this.UserId, x.userId).subscribe((data: any) => {
      this.groups = data;
      let x = new GroupResponse();
      x.groupId = 0;
      x.groupName = "Non-Group";
      this.groups.push(x);
      
      var gdata = this.groups.find(item => item.groupId === 0);
      document.getElementById("groups").innerText = gdata.groupName;
      this.groupId = gdata.groupId;
      this.getBalance();
    });    
  }

  selectGroup(id: number) {
    var gdata = this.groups.find(item => item.groupId === +id);
    document.getElementById("groups").innerText = gdata.groupName;
    this.groupId = gdata.groupId;
   this.getBalance();
  }

  getBalance() {
    this._appService.getIndividualbalance(this.UserId, this.friendId).subscribe((data: any) => {
      this.settleRecords = data;
      console.log(this.settleRecords);
      
      var exist = this.settleRecords.find(item => item.group_id === +this.groupId);
      if (exist == null) {
        alert("You do not owe anything...");
        this.amount = 0;
      }
      else {
        for (var i = 0; i < this.settleRecords.length; i++) {
          if (this.groupId == this.settleRecords[i].group_id) {
            if (this.UserId == this.settleRecords[i].payer_id && this.settleRecords[i].amount > 0) {
              this.amount = this.settleRecords[i].amount;
            }
            else if (this.UserId == this.settleRecords[i].receiver_id && this.settleRecords[i].amount < 0) {
              this.amount = Math.abs(this.settleRecords[i].amount);
           }
            else {
              alert("You do not owe anything...");
             this.amount = 0;
            }
          }
        }
      }
    });
  }

  savePayment() {
    if (this.amount == 0) {
      alert("can not record payment");
      this.close.nativeElement.click();
      this.payment = null;
    }
    else {
      this.payment = new Transaction();
      this.payment.transPayersId = this.UserId;
      this.payment.transReceiversId = this.friendId;
      this.payment.paidAmount = this.amount;
      if (this.groupId != 0) {
        this.payment.groupId = this.groupId;
      }
      this.payment.createdDate = new Date().toLocaleString();

      console.log(this.payment);
      if (this.payment.transPayersId != null && this.payment.transReceiversId != null && this.payment.paidAmount > 0) {
        this._appService.recordPayment(this.payment).subscribe((data: any) => {
          alert("Payment recorded..");
          this.payment = null;
          this.close.nativeElement.click();
        },
          err => { console.log(err); alert("Payment Doesn't recorded.."); }
        );
        this.amount = 0;
        document.getElementById("receiver").innerText = "receiver";
      }
      else {
        alert("can not record payment");
      }
    }
  }

  selectSharedMember(id: number, name: string) {

    var exist = this.SharedArray.find(item => item.id === +id);
    if (exist == null) {
      let member = { "id": id, "name": name };
      this.SharedArray.push(member);
    }
  }

  removeSharedMember(id: number) {
    if (id != this.UserId) {
      var exist = this.SharedArray.find(item => item.id === +id);
      if (exist != null) {
        let index = this.SharedArray.indexOf(exist);
        this.SharedArray.splice(index, 1);
      }
    }
  }

  showPayer() {
    document.getElementById("payers").innerText = "Multiple";
    if (this.showMultiplePayer == false) {
      this.showMultiplePayer = true;
    }
    else {
      this.showMultiplePayer = false;
    }
  }

  addSinglePayer(id: number) {

    this.billPayer = new member();
    this.billPayer.id = id;
    this.billPayer.amount = this.totalAmount;

    this.billPayers = [];

    let x = this.Members.find(y => y.id === this.billPayer.id);
    document.getElementById("payers").innerText = x.name;
  }

  addMultiplePayer(id: number, amount: number) {
    let x = new member();
    x.id = id;
    x.amount = amount;
    this.billPayers.push(x);
    this.billPayer = null;
  }

  saveBill() {
    this.AddBillModel.creatorId = this.UserId;
    this.AddBillModel.createdDate = new Date().toLocaleString();
    this.AddBillModel.amount = this.totalAmount;
    this.sharedAmount = this.totalAmount / this.SharedArray.length;

    for (var i = 0; i < this.SharedArray.length; i++) {
      let x = new member();
      x.id = this.SharedArray[i].id;
      x.amount = this.sharedAmount;
      this.sharedMembers.push(x);
    }

    this.AddBillModel.sharedMember = this.sharedMembers;

    if (this.billPayer != null) {
      this.AddBillModel.payer = [];
      this.AddBillModel.payer.push(this.billPayer);

      for (var i = 0; i < this.sharedMembers.length; i++) {
        if (this.sharedMembers[i].id != this.billPayer.id) {
          let settle = new Settle();
          settle.PayerId = this.sharedMembers[i].id;
          settle.SharedMemberId = this.billPayer.id;
          settle.TotalAmount = this.sharedAmount;
          this.settlements.push(settle);
        }
      }
      this.AddBillModel.SettleModels = this.settlements;
    }
    else {
      this.AddBillModel.payer = this.billPayers;

      for (var i = 0; i < this.sharedMembers.length; i++) {
        var payerExist = this.billPayers.find(item => item.id === this.sharedMembers[i].id);
        if (payerExist != null) {
          if (payerExist.amount > this.sharedAmount) {
            let x = new member();
            x.id = this.sharedMembers[i].id;
            x.amount = payerExist.amount - this.sharedAmount
            this.payee.push(x);
          }
          else if (payerExist.amount < this.sharedAmount) {
            let x = new member();
            x.id = this.sharedMembers[i].id;
            x.amount = this.sharedAmount - payerExist.amount;
            this.payer.push(x);
          }
        }
        else {
          let x = new member();
          x.id = this.sharedMembers[i].id;
          x.amount = this.sharedMembers[i].amount;
          this.payer.push(x);
        }
      }

      if (this.payee.length == 1) {
        for (var i = 0; i < this.payer.length; i++) {
          for (var j = 0; j < this.payee.length; j++) {
            var set = new Settle();
            set.PayerId = this.payer[i].id;
            set.SharedMemberId = this.payee[j].id;
            
            set.TotalAmount = this.payer[i].amount;
            this.settlements.push(set);
          }
        }
      }
      else {
        if (this.payer.length == 1) {
          for (var i = 0; i < this.payee.length; i++) {
            for (var j = 0; j < this.payer.length; j++) {
              var set = new Settle();
              set.PayerId = this.payer[j].id;
              set.SharedMemberId = this.payee[i].id;
              
              set.TotalAmount = this.payee[i].amount;
              this.settlements.push(set);
            }
          }
        }
        else {
          for (var i = 0; i < this.payer.length; i++) {
            for (var j = 0; j < this.payee.length; j++) {

              var set = new Settle();
              set.PayerId = this.payer[i].id;
              set.SharedMemberId = this.payee[j].id;
             

              if (this.payee[j].amount < this.payer[i].amount) {

                set.TotalAmount = this.payee[j].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payee[j].amount = this.payee[j].amount - set.TotalAmount;
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
              }
              else if (this.payee[j].amount > this.payer[i].amount) {
                set.TotalAmount = this.payer[i].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
                this.payee[j].amount = this.payee[j].amount - set.TotalAmount;
              }
              else {  
                set.TotalAmount = this.payer[i].amount;
                if (set.TotalAmount != 0) {
                  this.settlements.push(set);
                }
                this.payer[i].amount = this.payer[i].amount - set.TotalAmount;
                this.payee[j].amount = this.payee[i].amount - set.TotalAmount;
              }
            }
          }
        }
      }          
      this.AddBillModel.SettleModels = this.settlements;
    }
    console.log(this.AddBillModel);

    if (this.sharedMembers.length == 1) {
      alert("can not add bill please select more users");
    }
    else {
      this._appService.addBill(this.AddBillModel).subscribe((data: any) => {
        alert("bill addded..");
        this.closebill.nativeElement.click();
      });
    }
    this.PayersArray = [];
    this.SharedArray = [];
    let x = { "id": this.UserId, "name": this.UserName };
    this.SharedArray.push(x);
    this.settlements = [];
    this.billPayers = [];
    this.sharedMembers = [];
    this.payer = [];
    this.payee = [];
    this.AddBillModel.billName = '';
    this.totalAmount = null;

  }
}
