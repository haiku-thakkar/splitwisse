import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { map } from "rxjs/operators";
import { Observable } from 'rxjs/Rx';
import { User, UserEditModel } from '../Model/User';
import { retry } from 'rxjs/operator/retry';
import { Group } from '../Model/Group';
import { AddBill } from '../Model/AddBill';
import { Transaction } from '../Model/Transaction';

@Injectable()
export class AppService {

  constructor(private _httpService: Http) { }

  UserId: number;

  userLogin(email: string, password: string) {
    return this._httpService.get('/api/user/login/' + email + '/' + password)
      .pipe(map(res => res.json()))
      .catch((error: any) => {
        return Observable.throw(new Error(error.status));
      });
  }

  setUserId(id: number) {
    this.UserId = id;
  }

  getUserId() {
    return this.UserId;
  }

  registerUser(user: UserEditModel) {
    return this._httpService.post('api/user',user)
      .pipe(map(res => res.json()))
      .catch((error: any) => {
        return Observable.throw(new Error(error.status));
      });
  }

  getUserDetails(id: number) {
    return this._httpService.get('api/user/' + id)
      .pipe(map(res => res.json()));
  }

  editUserDetail(user: UserEditModel) {
    return this._httpService.put('api/user/' + user.userId, user);
  }

  addFriend(id: number, name: string, email: string) {
    return this._httpService.post('api/friends/' + id + '/' + name + '/' + email, id)
      .pipe(map(res => res.json()))
      .catch((error: any) => {
        return Observable.throw(new Error(error.status));
      });
  }

  getSingleFriendDetail(id: number) {
    return this._httpService.get('api/friends/' + id)
      .pipe(map(res => res.json()));
  }

  getAllFriends(id: number) {
    return this._httpService.get('api/friends/all/'+id)
      .pipe(map(res => res.json()));
  }

  removeFriend(userId: number, friendId: number) {
    return this._httpService.delete('api/friends/' + userId + "/" + friendId)
      .pipe(map(res => res.json()));
  }

  getAllGroups(id: number) {
    return this._httpService.get('api/group/all/' + id)
      .pipe(map(res => res.json()));
  }

  getGroupDetails(id: number) {
    return this._httpService.get('api/group/' + id)
      .map(res => res.json());
  }

  getCommenGroups(userId: number, friendId: number) {
    return this._httpService.get('api/group/all/' + userId + '/' + friendId)
      .map(res => res.json());
  }

  addGroup(group: Group) {
    return this._httpService.post('api/group/', group)
      .pipe(map(res => res.json()))
      .catch((error: any) => {
        return Observable.throw(new Error(error.status));
      });
  }

  editGroup(group: Group) {
    return this._httpService.put('api/group/' + group.groupId, group);
  }

  removeGroup(id: number) {
    return this._httpService.delete('api/group/' + id);
  }

  addGroupMember(groupId: number, MemberId: number) {
    return this._httpService.post('api/group/' + groupId + '/'+MemberId, groupId);
  }

  removeGroupMember(groupId: number, MemberId: number) {
    return this._httpService.delete('api/group/' + groupId + '/' + MemberId);
  }

  getGroupBills(groupId: number) {
    return this._httpService.get('api/bill/all/' + groupId)
      .map(res => res.json());
  }

  getIndividualBills(userId: number, friendId: number) {
    return this._httpService.get('api/bill/all/' + userId + '/' + friendId)
      .map(res => res.json());
  }

  addBill(bill: AddBill) {
    return this._httpService.post('api/bill/', bill);
  }

  getGroupBalance(groupId: number) {
    return this._httpService.get('api/settle/group/' + groupId)
      .map(res => res.json());
  }


  getIndividualbalance(userId: number, friendId: number) {
    return this._httpService.get('api/settle/' + userId + '/' + friendId)
      .map(res => res.json());
  }

  recordPayment(payment: Transaction) {
    return this._httpService.post('api/transaction', payment)
      .pipe(map(res => res.json()))
      .catch((error: any) => {
        return Observable.throw(new Error(error.status));
      });
  }

  getAllSettldata(id: number) {
    return this._httpService.get('api/settle/all/' + id)
      .map(res => res.json());
  }

  getGroupTransactions(id: number) {
    return this._httpService.get('api/transaction/all/' + id)
      .map(res => res.json());
  }

  getIndividualTransactions(userId: number, friendId: number) {
    return this._httpService.get('api/transaction/all/' + userId + '/' + friendId)
      .map(res => res.json());
  }

  getAllExpenses(id: number) {
    return this._httpService.get('api/bill/allbill/' + id)
      .map(res => res.json());
  }

  getAllTransactions(id: number) {
    return this._httpService.get('api/transaction/alltrans/' + id)
      .map(res => res.json());
  }

  getTotal(id: number) {
    return this._httpService.get('/api/settle/total/' + id)
      .map(res => res.json());
  }
}
