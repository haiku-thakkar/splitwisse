using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SecondSplitWise.DBContext;
using SecondSplitWise.DataModel;
using SecondSplitWise.Model;
using Microsoft.Extensions.Logging;
using SecondSplitWise.Response;

namespace SecondSplitWise.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly SecondSplitWiseContext _Context;
        private readonly ILogger _Logger;

        public ExpenseRepository(SecondSplitWiseContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("BillRepository");
        }

       
        public async Task<ExpenseResponse> GetBillAsync(int id)
        {
            ExpenseResponse bill = new ExpenseResponse();
            var payers = new List<ExpenseMemberResponse>();
            var members = new List<ExpenseMemberResponse>();

            var billData = _Context.Expense.SingleOrDefault(c => c.BillId == id);
            bill.BillId = billData.BillId;
            bill.BillName = billData.BillName;
            bill.Amount = billData.Amount;
            bill.CreatedDate = billData.CreatedDate;
            bill.GroupId = billData.GroupId.GetValueOrDefault();

            if (bill.GroupId != 0)
            {
                var groupname = _Context.Group.SingleOrDefault(c => c.GroupId == billData.GroupId);
                bill.GroupName = groupname.GroupName;
            }
            else
            {
                bill.GroupName = "NonGroup Expense";
            }
            var name = _Context.User.SingleOrDefault(c => c.UserId == billData.CreatorId);
            bill.CreatorName = name.UserName;

            var data = _Context.ExpenseMember.Where(c => c.Billid == id).ToList();
            for (var i = 0; i < data.Count; i++)
            {
                var member = _Context.User.SingleOrDefault(c => c.UserId == data[i].SharedMemberId);
                members.Add(new ExpenseMemberResponse(member.UserId,member.UserName, data[i].AmountToPay));
            }

            var payer = _Context.Payer.Where(c => c.BillId == id).ToList();
            for (var i = 0; i < payer.Count; i++)
            {
                var p = _Context.User.SingleOrDefault(c => c.UserId == payer[i].PayerId);
                payers.Add(new ExpenseMemberResponse(p.UserId, p.UserName, payer[i].PaidAmount));
               
            }

            bill.Payers = payers;
            bill.BillMembers = members;
            return  bill;
        }

        public async Task<Expense> InsertBillAsync(ExpenseModel bill)
        {
            Expense newBill = new Expense();
            newBill.BillName = bill.BillName;
            newBill.CreatorId = bill.CreatorId;
            newBill.Amount = bill.Amount;
            newBill.CreatedDate = bill.CreatedDate;
            newBill.GroupId = bill.GroupId;
            _Context.Expense.Add(newBill);


            foreach (var person in bill.Payer)
            {
                Payer payer = new Payer();
                payer.BillId = newBill.BillId;
                payer.PayerId = person.Id;
                payer.PaidAmount = person.Amount;
                _Context.Payer.Add(payer);
            }


            foreach (var person in bill.SharedMember)
            {
                ExpenseMember member = new ExpenseMember();
                member.Billid = newBill.BillId;
                member.SharedMemberId = person.Id;
                member.AmountToPay = person.Amount;
                _Context.ExpenseMember.Add(member);
            }


            for (var i = 0; i < bill.SharedMember.Count - 1; i++)
            {
                for (var j = i + 1; j < bill.SharedMember.Count; j++)
                {
                    var fExist = _Context.Friend.SingleOrDefault(c => c.UserId == bill.SharedMember[i].Id && c.FriendId == bill.SharedMember[j].Id);
                    if (fExist == null)
                    {
                        var Exist = _Context.Friend.SingleOrDefault(c => c.UserId == bill.SharedMember[j].Id && c.FriendId == bill.SharedMember[i].Id);
                        if (Exist == null)
                        {
                            Friend newFriend = new Friend
                            {
                                UserId = bill.SharedMember[i].Id,
                                FriendId = bill.SharedMember[j].Id
                            };
                            _Context.Friend.Add(newFriend);
                            await _Context.SaveChangesAsync();
                        }
                    }

                }
            }


            foreach (var data in bill.SettleModels)
            {
                if (data.GroupId == null)
                {
                    var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.PayerId && c.SharedMemberId == data.SharedMemberId && c.GroupId==null);
                    if (settle != null)
                    {
                        settle.TotalAmount = settle.TotalAmount + data.TotalAmount;
                        _Context.Settlement.Attach(settle);
                    }
                    else
                    {
                        var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.SharedMemberId && c.SharedMemberId == data.PayerId && c.GroupId == null);
                        if (setle != null)
                        {
                            setle.TotalAmount = setle.TotalAmount - data.TotalAmount;
                            _Context.Settlement.Attach(setle);
                        }
                        else
                        {
                            var newSettle = new Settlement();
                            newSettle.PayerId = data.PayerId;
                            newSettle.SharedMemberId = data.SharedMemberId;
                            newSettle.TotalAmount = data.TotalAmount;
                            _Context.Settlement.Add(newSettle);
                        }
                    }
                }
                else
                {
                    var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.PayerId && c.SharedMemberId == data.SharedMemberId && c.GroupId == data.GroupId);
                    if (settle != null)
                    {
                        settle.TotalAmount = settle.TotalAmount + data.TotalAmount;
                        _Context.Settlement.Attach(settle);
                    }
                    else
                    {
                        var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.SharedMemberId && c.SharedMemberId == data.PayerId && c.GroupId == data.GroupId);
                        if (setle != null)
                        {
                            setle.TotalAmount = setle.TotalAmount - data.TotalAmount;
                            _Context.Settlement.Attach(setle);
                        }
                        else
                        {
                            var newSettle = new Settlement();
                            newSettle.PayerId = data.PayerId;
                            newSettle.SharedMemberId = data.SharedMemberId;
                            newSettle.TotalAmount = data.TotalAmount;
                            newSettle.GroupId = data.GroupId;
                            _Context.Settlement.Add(newSettle);
                        }
                    }
                }
            }
          
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(InsertBillAsync)}: " + exp.Message);
            }
            return newBill;
        }

        public async Task<bool> DeleteBillAsync(int id)
        {
            var bill = await _Context.Expense.SingleOrDefaultAsync(c => c.BillId == id);
            _Context.Remove(bill);
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteBillAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<List<ExpenseResponse>> GetAllExpenses(int id)
        {
            List<ExpenseResponse> bills = new List<ExpenseResponse>();
            var billData = _Context.Expense.Where(c => c.BillMembers.Any(aa => aa.SharedMemberId == id) || c.CreatorId == id || c.Payers.Any(aa => aa.PayerId == id)).OrderByDescending(c => c.CreatedDate).ToList();
            for (var i = 0; i < billData.Count; i++)
            {
                var bill = new ExpenseResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }            
            return bills;
        }

        public async Task<List<ExpenseResponse>> GetIndividualExpenses(int Userid, int Friendid)
        {
          
            List<ExpenseResponse> bills = new List<ExpenseResponse>();
            var billData = _Context.Expense.Where(c => c.BillMembers.Any(aa => aa.SharedMemberId == Userid) && c.BillMembers.Any(aa => aa.SharedMemberId == Friendid)).OrderByDescending(c => c.CreatedDate).ToList();
            for (var i = 0; i < billData.Count; i++)
            {
                var bill = new ExpenseResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }            
            return bills;
        }

        public async Task<List<ExpenseResponse>> GetGroupExpenses(int Groupid)
        {
           
            List<ExpenseResponse> bills = new List<ExpenseResponse>();
            var billData = _Context.Expense.Where(c => c.GroupId == Groupid).OrderByDescending(c => c.CreatedDate).ToList();

            for(var i = 0; i < billData.Count; i++)
            {
                var bill = new ExpenseResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }
            
            return bills;
        }

        public async Task<bool> UpdateBillAsync(Expense bill)
        {
            _Context.Expense.Attach(bill);
            _Context.Entry(bill).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateBillAsync)}: " + exp.Message);
            }
            return false;
        }
    }
}
