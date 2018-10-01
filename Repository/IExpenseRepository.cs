using SecondSplitWise.DataModel;
using SecondSplitWise.Model;
using SecondSplitWise.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{
    public interface IExpenseRepository
    {
        Task<ExpenseResponse> GetBillAsync(int id);

        Task<Expense> InsertBillAsync(ExpenseModel bill);

        Task<bool> UpdateBillAsync(Expense bill);

        Task<bool> DeleteBillAsync(int id);

        Task<List<ExpenseResponse>> GetAllExpenses(int id);

        Task<List<ExpenseResponse>> GetIndividualExpenses(int Userid, int Friendid);

        Task<List<ExpenseResponse>> GetGroupExpenses(int Groupid);
    }
}
