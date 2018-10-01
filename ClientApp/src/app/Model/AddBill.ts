import { Settle } from "./Settle";

export class AddBill {
  constructor(
  ) {

  }
  public billId: number;
  public billName: string;
  public creatorId: number;
  public groupId: number;
  public amount: number;
  public createdDate;
  public payer: member[];
  public sharedMember: member[];
  public SettleModels: Settle[];

}

export class member {
  id: number;
  amount: number;

  constructor() {  }
}



