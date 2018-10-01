import { Component, OnInit, group } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { Bill } from '../Model/Bill';
import { forEach } from '@angular/router/src/utils/collection';
import { strictEqual } from 'assert';
import { Group, GroupResponse, Grpmember } from '../Model/Group';
import { AddBill, member } from '../Model/AddBill';
import { Settle, Balance } from '../Model/Settle';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {

  @ViewChild('closebill') closebill: ElementRef;

  UserId: number;
  groupId: number;
  group = new GroupResponse();

  AddBillModel = new AddBill();
  billPayer= new member();
  billPayers: member[] = [];
  totalAmount: number;
  memberCount: number;
  sharedAmount: number;
  sharedMembers: member[] = [];
  settlements: Settle[] = [];
  payer: member[] = [];
  payee: member[] = [];
  showMultiplePayer: boolean = false;
 
  groupBills: Bill;
  groupEditModel = new Group();
  billId: any[] = [];
  groupMember: any[] = [];
  AddGroupMembers: any[] = [];
  friends: any[] = [];
  addedMembers: number[] = [];
  removedMembers: number[] = [];
  showSettingData: boolean = false;

  grpBalanceShow: boolean = true;

  balance: Balance[] = [];
  TotalBalance: any[] = [];
  details:Grpmember[]=[];

  transactions: any[] = [];

  constructor(private _appService: AppService, private route: ActivatedRoute, private router: Router) {

  }

  ngOnInit() {
    
    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });
    console.log(this.UserId);

    this.groupId = this.route.snapshot.params['id'];
    this._appService.getGroupDetails(this.groupId).subscribe((data: any) => {
      this.group = data;
      this.groupMember = data.members;
      for (var i in this.groupMember) {
        this.groupMember[i] = { "id": this.groupMember[i].id, "name": this.groupMember[i].name, "removed": false };
      }
      for (var i in this.groupMember) {
        this.TotalBalance[i] = { "id": this.groupMember[i].id, "name": this.groupMember[i].name, "balance": Number(0) };
      }

      this._appService.getGroupBalance(this.groupId).subscribe((data: any) => {
        this.balance = data;
        console.log(this.balance);
        for (var i = 0; i < this.balance.length; i++) {
          if (this.balance[i].amount != 0) {
            var pId = this.TotalBalance.find(x => x.id === +this.balance[i].payer_id);
            var rId = this.TotalBalance.find(x => x.id === +this.balance[i].receiver_id);
            if (this.balance[i].amount > 0) {
              pId.balance = pId.balance + this.balance[i].amount;
              let x = new Grpmember();
              x.id = pId.id;
              x.name = this.balance[i].payerName + " owes ₹" + this.balance[i].amount + " to " + this.balance[i].receiverName;
              this.details.push(x);

              rId.balance = rId.balance - this.balance[i].amount;
              let y = new Grpmember();
              y.id = rId.id;
              y.name = this.balance[i].payerName + " owes " + this.balance[i].receiverName + " ₹" + this.balance[i].amount
              this.details.push(y);

            }
            else {
              rId.balance = rId.balance + Math.abs(this.balance[i].amount);
              let x = new Grpmember();
              x.id = rId.id;

              x.name = this.balance[i].receiverName + " owes " + this.balance[i].payerName + " ₹" + Math.abs(this.balance[i].amount);
              this.details.push(x);


              pId.balance = pId.balance - Math.abs(this.balance[i].amount);
              let y = new Grpmember();
              y.id = pId.id;
              y.name = this.balance[i].receiverName + " owes ₹" + Math.abs(this.balance[i].amount) + " to " + this.balance[i].payerName;
              this.details.push(y);
            }
          }

        }
      });

      console.log(this.TotalBalance);
      console.log(this.details);
      console.log(this.groupMember);
    });

    this._appService.getGroupBills(this.groupId).subscribe((data: any) => {
      this.groupBills = data;
      for (var i = 0; i < data.length; i++) {
        this.billId[i] = { "id": data[i].billId, "status": false };
      }

      console.log(this.billId);
    });

    this._appService.getAllFriends(this.UserId).subscribe((data: any) => {
      this.friends = data;
      for (var i = 0; i < data.length; i++) {
        this.AddGroupMembers[i] = { "id": data[i].userId, "name": this.friends[i].userName, "added": false };
      }
      this._appService.getGroupDetails(this.groupId).subscribe((data: any) => {
        var members = data.members;
        for (var i = 0; i < members.length; i++) {
          for (var j = 0; j < this.AddGroupMembers.length; j++) {

            if (members[i].id == this.AddGroupMembers[j].id) {
              this.AddGroupMembers.splice(j, 1);
            }
          }
        }
        console.log(this.AddGroupMembers);
      });
    });
    console.log(this.TotalBalance);

    this._appService.getGroupTransactions(this.groupId).subscribe((data: any) => {
      this.transactions = data;
    });
  }

  show(billId: number) {
    var bill = this.billId.find((item: any) => item.id === +billId);
    console.log(bill);
    if (bill.status == false) {
      bill.status = true;
    }
    else {
      bill.status = false;
    }
  }

  check(billId: number) {
    var bill = this.billId.find((item: any) => item.id === +billId);
    return bill.status;
  }

  showSetting() {
    if (this.grpBalanceShow == true) {
      this.grpBalanceShow = false;
    }

    if (this.showSettingData == false) {
      this.showSettingData = true;
    }
    else {
      this.showSettingData = false;
    }
  }

  removeMember(id: number) {
    var m = this.groupMember.find(item => item.id === +id);
    var Checkbalance = this.TotalBalance.find(x => x.id === +id);
    if (Checkbalance.balance == 0) {
      if (m.removed == false) {
        m.removed = true;
      }
      else {
        m.removed = false;
      }
    }
    else {
      m.removed = false;
      alert("please settle all balance then remove member...");
    }
  }

  checkMember(id: number) {
    var m = this.groupMember.find(item => item.id === +id);
    return m.removed;
  }

  AddMember(id: number) {
    var m = this.AddGroupMembers.find(item => item.id === +id);
    if (m.added == false) {
      m.added = true;
    }
    else {
      m.added = false;
    }
  }

  checkAddedMember(id: number) {
    var m = this.AddGroupMembers.find(item => item.id === +id);
    return m.added;
  }


  onSubmit() {
    for (var i = 0; i < this.AddGroupMembers.length; i++) {
      if (this.AddGroupMembers[i].added == true) {
        this.addedMembers.push(this.AddGroupMembers[i].id);
      }
    }

    for (var i = 0; i < this.groupMember.length; i++) {
      if (this.groupMember[i].removed == true) {
        this.removedMembers.push(this.groupMember[i].id);
      }
    }
    console.log(this.addedMembers);
    console.log(this.removedMembers);

    for (var i = 0; i < this.removedMembers.length; i++) {
      this._appService.removeGroupMember(this.groupId, this.removedMembers[i]).subscribe();
    }

    this.groupEditModel.groupId = this.group.groupId;
    this.groupEditModel.groupName = this.group.groupName;
    this.groupEditModel.createdDate = this.group.createdDate;
    this.groupEditModel.creatorId = this.group.creatorId;

    this._appService.editGroup(this.groupEditModel).subscribe();

    for (var i = 0; i < this.addedMembers.length; i++) {
      this._appService.addGroupMember(this.groupId, this.addedMembers[i]).subscribe();
    }
    alert("Group Updated successfully..");
    this.showSettingData = false;
    this.ngOnInit();

  }

  onCancel() {
    this.showSettingData = false;
    for (var i = 0; i < this.AddGroupMembers.length; i++) {
      if (this.AddGroupMembers[i].added == true) {
        this.AddGroupMembers[i].added = false;
      }
    }

    for (var i = 0; i < this.groupMember.length; i++) {
      if (this.groupMember[i].removed == true) {
        this.groupMember[i].removed = false;
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

    let x = this.groupMember.find(y => y.id === this.billPayer.id);
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
    this.AddBillModel.groupId = this.groupId;
    this.AddBillModel.createdDate = new Date().toLocaleString();
    this.AddBillModel.amount = this.totalAmount;
    this.sharedAmount = this.totalAmount / this.groupMember.length;

    for (var i = 0; i < this.groupMember.length; i++) {
      let x = new member();
      x.id = this.groupMember[i].id;
      x.amount = this.sharedAmount;
      this.sharedMembers.push(x);
    }
    this.AddBillModel.sharedMember = this.sharedMembers;

    if (this.billPayer != null) {
      this.AddBillModel.payer = [];
      this.AddBillModel.payer.push(this.billPayer);

      for (var i = 0; i < this.groupMember.length;i++) {
        if (this.groupMember[i].id != this.billPayer.id) {
          let settle = new Settle();
          settle.GroupId = this.groupId;
          settle.PayerId = this.groupMember[i].id;
          settle.SharedMemberId = this.billPayer.id;
          settle.TotalAmount = this.sharedAmount;
          this.settlements.push(settle);
        }
      }
      this.AddBillModel.SettleModels = this.settlements;
    }
    else
    {
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
            set.GroupId = this.groupId;
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
              set.GroupId = this.groupId;
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
              set.GroupId = this.groupId;

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

      console.log("settlements:");
      console.log(this.settlements);
      this.AddBillModel.SettleModels = this.settlements;
    }

    console.log(this.AddBillModel);
    this._appService.addBill(this.AddBillModel).subscribe((data: any) => {
      alert("bill added...");
      this.closebill.nativeElement.click();
    });

    this.settlements = [];
    this.billPayers = [];
    this.sharedMembers = [];
    this.payer = [];
    this.payee = [];
    this.AddBillModel.billName = '';
    this.totalAmount = 0;    
  }

  showBalanceTab() {
    if (this.showSettingData == true) {
      this.showSettingData = false;
    }
    if (this.grpBalanceShow == false) {
      this.grpBalanceShow = true;
    }
    else {
      this.grpBalanceShow = false;
    }
  }

  deleteGroup() {
    console.log(this.TotalBalance);
    if (this.TotalBalance.every(x => x.balance == 0)) {
      var a = confirm("Are you ABSOLUTELY sure you want to delete this group ?");
      if (a) {
        this._appService.removeGroup(this.groupId).subscribe((data: any) => {
          alert("Group Deleted");
        this.router.navigate(['/', this.UserId]);
        });
      }
    }
    else {
      alert("can't delete group first settle data with all members..");
    }
  }

  
}
