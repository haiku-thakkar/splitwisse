using SecondSplitWise.Model;
using SecondSplitWise.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{
    public interface ISettlementRepository
    {
        Task<SettlementResponse> GetSettlementAsync(int id);
        Task<List<SettlementResponse>> GetSettlementAsync(int Userid, int Friendid);
        Task<List<SettlementResponse>> GetGroupSettlementAsync(int Groupid);
        Task<List<SettlementResponse>> GetAllSettlementAsync(int id);
        Task<decimal> GetTotal(int Id);
    }
}
