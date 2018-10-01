export class Group {
  constructor(
  ) { }
  public groupId: number;
  public groupName: string;
  public creatorId: number;
  public createdDate;
  public members: number[];
}

export class GroupResponse {
  constructor() { }

  public groupId: number;
  public groupName: string;
  public creatorId: number;
  public creatorName: string;
  public createdDate;
  public members: Grpmember[];
}

export class Grpmember {
  public id: number;
  public name: string;
}
