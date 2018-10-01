using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecondSplitWise.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    FriendListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    FriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.FriendListId);
                    table.ForeignKey(
                        name: "FK_Friend_User_FriendId",
                        column: x => x.FriendId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friend_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    BillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillName = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Expense_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    GroupMemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Group_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_Group_Id",
                        column: x => x.Group_Id,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settlement",
                columns: table => new
                {
                    SettlementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Payer_Person = table.Column<int>(nullable: false),
                    Requestor_Person = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement", x => x.SettlementId);
                    table.ForeignKey(
                        name: "FK_Settlement_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settlement_User_Payer_Person",
                        column: x => x.Payer_Person,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Settlement_User_Requestor_Person",
                        column: x => x.Requestor_Person,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransPayersId = table.Column<int>(nullable: false),
                    TransReceiversId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_User_TransPayersId",
                        column: x => x.TransPayersId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_User_TransReceiversId",
                        column: x => x.TransReceiversId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseMember",
                columns: table => new
                {
                    BillMemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Billid = table.Column<int>(nullable: false),
                    SharedMemberId = table.Column<int>(nullable: true),
                    AmountToPay = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseMember", x => x.BillMemberId);
                    table.ForeignKey(
                        name: "FK_ExpenseMember_Expense_Billid",
                        column: x => x.Billid,
                        principalTable: "Expense",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseMember_User_SharedMemberId",
                        column: x => x.SharedMemberId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(nullable: false),
                    PayerId = table.Column<int>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payer_Expense_BillId",
                        column: x => x.BillId,
                        principalTable: "Expense",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payer_User_PayerId",
                        column: x => x.PayerId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreatorId",
                table: "Expense",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_GroupId",
                table: "Expense",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseMember_Billid",
                table: "ExpenseMember",
                column: "Billid");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseMember_SharedMemberId",
                table: "ExpenseMember",
                column: "SharedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FriendId",
                table: "Friend",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_CreatorId",
                table: "Group",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_Group_Id",
                table: "GroupMember",
                column: "Group_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_User_Id",
                table: "GroupMember",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payer_BillId",
                table: "Payer",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Payer_PayerId",
                table: "Payer",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_GroupId",
                table: "Settlement",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_Payer_Person",
                table: "Settlement",
                column: "Payer_Person");

            migrationBuilder.CreateIndex(
                name: "IX_Settlement_Requestor_Person",
                table: "Settlement",
                column: "Requestor_Person");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GroupId",
                table: "Transactions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransPayersId",
                table: "Transactions",
                column: "TransPayersId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransReceiversId",
                table: "Transactions",
                column: "TransReceiversId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseMember");

            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "Payer");

            migrationBuilder.DropTable(
                name: "Settlement");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
