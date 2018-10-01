using SecondSplitWise.Model;
using SecondSplitWise.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{
    public interface IFriendRepository
    {
        Task<FriendResponse> GetFriendAsync(int id);
        Task<List<FriendResponse>> GetAllFriendsAsync(int id);
        Task<Friend> InsertFriendAsync(int id,string userName, string email);
        Task<bool> DeleteFriendAsync(int uid, int fid);
        
    }
}
