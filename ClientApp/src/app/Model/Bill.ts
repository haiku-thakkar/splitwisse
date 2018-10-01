export class Bill {
  constructor(
  ) {
   
  }
  public billId: number;
  public billName: string;
  public creatorName: string;
  public amount: number;
  public groupId: number;
  public groupName: string;
  public createdDate;
  public payers: member[];
  public billMembers: member[];
  
}

class member {
  constructor() { }

  id: number;
  name: string;
  amount: number;
}

