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
    public class FriendRepository :IFriendRepository
    {
        private readonly SecondSplitWiseContext _Context;
        private readonly ILogger _Logger;

        public FriendRepository(SecondSplitWiseContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("FriendListRepository");
        }

        public async Task<FriendResponse> GetFriendAsync(int id)
        {
            var userData = await _Context.User.SingleOrDefaultAsync(c => c.UserId == id);
            var user = new FriendResponse();
            user.UserId = userData.UserId;
            user.UserName = userData.UserName;
            user.Email = userData.Email;
            return user;
        }

        public async Task<List<FriendResponse>> GetAllFriendsAsync(int id)
        {
          
            List<FriendResponse> friends = new List<FriendResponse>();

            var fData = await _Context.Friend.Where(c => c.UserId == id || c.FriendId == id).ToListAsync();
            for(var i = 0; i < fData.Count; i++)
            {
                if (fData[i].UserId == id)
                {
                    var x = new FriendResponse();
                    x.UserId = fData[i].FriendId;
                    var data = await _Context.User.SingleOrDefaultAsync(c => c.UserId == x.UserId);
                    x.UserName = data.UserName;
                    x.Email = data.UserName;
                    friends.Add(x);
                }
                else
                {
                    var x = new FriendResponse();
                    x.UserId = (int)fData[i].UserId;
                    var data = await _Context.User.SingleOrDefaultAsync(c => c.UserId == x.UserId);
                    x.UserName = data.UserName;
                    x.Email = data.UserName;
                    friends.Add(x);
                }
            }
            return friends;
        }

        public async Task<Friend> InsertFriendAsync(int id, string userName, string email)
        {
            var userExist = _Context.User.SingleOrDefault(c => c.UserName == userName && c.Email == email);
            if (userExist != null)
            {
                var member = _Context.Friend.SingleOrDefault(c => c.UserId == id && c.FriendId == userExist.UserId);
                if (member == null)
                {
                    var memExist= _Context.Friend.SingleOrDefault(c => c.UserId==userExist.UserId && c.FriendId==id);
                    if (memExist == null)
                    {
                        Friend newFriend = new Friend
                        {
                            UserId = id,
                            FriendId = userExist.UserId
                        };
                        _Context.Friend.Add(newFriend);
                        await _Context.SaveChangesAsync();
                        return newFriend;
                    }
                    else
                    {
                        _Context.Friend.Attach(memExist);
                        await _Context.SaveChangesAsync();
                        return memExist;
                    }
                }
                else
                {
                    _Context.Friend.Attach(member);
                    await _Context.SaveChangesAsync();
                    return member;
                }
            }
            else
            {
                Friend notExist = new Friend
                {
                    UserId = 0,
                    FriendId = 0
                };
                return notExist;
            }     
        }

        public async Task<bool> DeleteFriendAsync(int fid, int uid)
        {
            var data = _Context.Friend.SingleOrDefault(c => c.UserId == uid && c.FriendId == fid);
            if (data == null)
            {
                var fData = _Context.Friend.SingleOrDefault(c => c.UserId == fid && c.FriendId == uid);
                _Context.Remove(fData);
            }
            else
            {
                _Context.Remove(data);
            }
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteFriendAsync)}: " + exp.Message);
            }
            return false;
        }
    }
}
