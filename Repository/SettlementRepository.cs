using SecondSplitWise.DBContext;
using SecondSplitWise.Model;
using SecondSplitWise.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{

    public class SettlementRepository : ISettlementRepository
    {
        private readonly SecondSplitWiseContext _Context;
        private readonly ILogger _Logger;

        public SettlementRepository(SecondSplitWiseContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("SettlementRepository");
        }


        public async  Task<SettlementResponse> GetSettlementAsync(int id)
        {
            SettlementResponse settlement = new SettlementResponse();

            var sData = await _Context.Settlement.SingleOrDefaultAsync(c => c.SettlementId == id);
            settlement.Id = sData.SettlementId;

            var Pname = await _Context.User.SingleOrDefaultAsync(c => c.UserId == sData.PayerId);
            settlement.PayerName = Pname.UserName;
            settlement.Payer_id=sData.PayerId;

            var Rname = await _Context.User.SingleOrDefaultAsync(c => c.UserId == sData.SharedMemberId);
            settlement.Receiver_id =(int) sData.SharedMemberId;
            settlement.ReceiverName = Rname.UserName;

            if (sData.GroupId != null)
            {
                var Gname = await _Context.Group.SingleOrDefaultAsync(c => c.GroupId == sData.GroupId);
                settlement.Group_id = sData.GroupId.GetValueOrDefault();
                settlement.GroupName = Gname.GroupName;
            }
            else
            {
                settlement.Group_id = 0;
                settlement.GroupName = "Nongroup Expense";
            }          
            settlement.Amount = sData.TotalAmount;
            return settlement;
        }

        public async Task<List<SettlementResponse>> GetSettlementAsync(int Userid, int Friendid)
        {
            List<SettlementResponse> settlements = new List<SettlementResponse>();
            var sData= await _Context.Settlement.Where(c => (c.PayerId == Userid && c.SharedMemberId == Friendid) || (c.PayerId == Friendid && c.SharedMemberId == Userid)).ToListAsync();

            for(var i = 0; i < sData.Count; i++)
            {
                var settle = new SettlementResponse();
                settle = await GetSettlementAsync(sData[i].SettlementId);
                settlements.Add(settle);
            }
            return settlements;
        }


        public async Task<List<SettlementResponse>> GetGroupSettlementAsync(int Groupid)
        {
            List<SettlementResponse> settlements = new List<SettlementResponse>();
            var sData = await _Context.Settlement.Where(c => c.GroupId==Groupid).ToListAsync();
            for (var i = 0; i < sData.Count; i++)
            {
                var settle = new SettlementResponse();
                settle = await GetSettlementAsync(sData[i].SettlementId);
                settlements.Add(settle);
            }
            return settlements;
        }       

        public async Task<List<SettlementResponse>> GetAllSettlementAsync(int id)
        {
            List<SettlementResponse> settlements = new List<SettlementResponse>();
            var sData = await _Context.Settlement.Where(c => c.PayerId == id || c.SharedMemberId == id).ToListAsync();
            for (var i = 0; i < sData.Count; i++)
            {
                var settle = new SettlementResponse();
                settle = await GetSettlementAsync(sData[i].SettlementId);
                settlements.Add(settle);
            }
            return settlements;
        }

        public async Task<decimal> GetTotal(int Id)
        {
            decimal total = 0;
            var sData = await _Context.Settlement.Where(c => c.PayerId == Id || c.SharedMemberId == Id).ToListAsync();
            for(var i = 0; i < sData.Count; i++)
            {
                if (sData[i].PayerId == Id)
                {
                    if (sData[i].TotalAmount >= 0)
                    { total = total + sData[i].TotalAmount;}
                    else
                    { total = total - Math.Abs(sData[i].TotalAmount);}
                }
                else
                {
                    if (sData[i].TotalAmount >= 0)
                    { total = total - sData[i].TotalAmount; }
                    else
                    { total = total + Math.Abs(sData[i].TotalAmount); }
                }
            }
            return total;
        }
    }
}
