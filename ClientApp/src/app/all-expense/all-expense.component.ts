import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../service/app.service';
import { Bill } from '../Model/Bill';


@Component({
  selector: 'app-all-expense',
  templateUrl: './all-expense.component.html',
  styleUrls: ['./all-expense.component.css']
})
export class AllExpenseComponent implements OnInit {

  UserId: number;

  Bills: Bill;
  transactions: any[] = [];
  Total: number;

  constructor(private _appService: AppService, private route: ActivatedRoute) {
    this.route.parent.params.subscribe(params => {
      this.UserId = +params['id'];
    });
    console.log(this.UserId);
  }

  ngOnInit() {
     this._appService.getAllExpenses(this.UserId).subscribe((data: any) => {
      this.Bills = data;

    });

     this._appService.getAllTransactions(this.UserId).subscribe((data: any) => {
      this.transactions = data;
     });

    this._appService.getTotal(this.UserId).subscribe((data: any) => {
      this.Total = data;
    });
  }

}
