using SecondSplitWise.Model;
using SecondSplitWise.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<UserResponse> GetUserAsync(int id);
        Task<User> InsertUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);

        Task<User> LoginUserAsync(string email, string password);
    }
}
