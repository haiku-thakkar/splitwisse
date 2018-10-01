export class Settle {
  constructor() { }

  public PayerId: number;
  public SharedMemberId: number;
  public GroupId: number;
  public TotalAmount: number;
}

export class Balance {
  constructor() { }

  public id: number;
  public payer_id: number;
  public payerName: string;
  public receiver_id: number;
  public receiverName: string;
  public group_id: number;
  public groupName: string;
  public amount: number;
}
