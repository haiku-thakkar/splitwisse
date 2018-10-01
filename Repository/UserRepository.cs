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
    public class UserRepository :IUserRepository
    {
        private readonly SecondSplitWiseContext _Context;
        private readonly ILogger _Logger;

        public UserRepository(SecondSplitWiseContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("UserRepository");
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _Context.User.ToListAsync();
            
        }

        public async Task<UserResponse> GetUserAsync(int id)
        {
           
            var userData = await _Context.User.SingleOrDefaultAsync(c => c.UserId == id);
            var user = new UserResponse();
            user.UserId = userData.UserId;
            user.UserName = userData.UserName;
            user.Email = userData.Email;
            user.Password = userData.Password;
            return user;
        }

        public async Task<User> InsertUserAsync(User user)
        {
            _Context.User.Add(user);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(InsertUserAsync)}: " + exp.Message);
            }
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            
            _Context.User.Attach(user);
            _Context.Entry(user).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateUserAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _Context.User
                .Include(c=>c.BillMembers)
                .Include(c=>c.Bills)
                .Include(c=>c.Friends)
                .Include(c=>c.groupMembers)
                .Include(c=>c.Groups)
                .Include(c => c.Payers)
                .Include(c => c.Payersdata)
                .Include(c => c.SharedMembers)
                .Include(c => c.TPayers)
                .Include(c => c.TReceivers)
                .Include(c => c.Users)
                .SingleOrDefaultAsync(c => c.UserId == id);
            _Context.Remove(user);

            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteUserAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            List<User> users = await _Context.User.ToListAsync();
            var user=users.SingleOrDefault(c => c.Email == email && c.Password == password);
            return user;           
        }
    }
}
