using SecondSplitWise.DBContext;
using SecondSplitWise.DataModel;
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
    public class GroupRepository :IGroupRepository
    {
        private readonly SecondSplitWiseContext _Context;
        private readonly ILogger _Logger;

        public GroupRepository(SecondSplitWiseContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("FriendListRepository");
        }

        public async Task<GroupResponse> GetGroupAsync(int id)
        {
            GroupResponse group = new GroupResponse();
            List<MemberResponse> members = new List<MemberResponse>();
            var groupData = _Context.Group.SingleOrDefault(c => c.GroupId == id);
            group.GroupId = groupData.GroupId;
            group.GroupName = groupData.GroupName;
            group.CreatedDate = groupData.CreatedDate;
            group.CreatorId = groupData.CreatorId;
            var name = _Context.User.SingleOrDefault(c => c.UserId == groupData.CreatorId);
            group.CreatorName = name.UserName;
            var memberData = _Context.GroupMember.Where(c => c.Group_Id == id).ToList();
            for (var i = 0; i < memberData.Count; i++)
            {
                var member = _Context.User.SingleOrDefault(c => c.UserId == memberData[i].User_Id);
                members.Add(new MemberResponse(member.UserId,member.UserName));               
            }
            group.Members = members;
            return group;
        }

        public async Task<List<GroupResponse>> GetGroupsAsync(int id)
        {
            List<GroupResponse> groups = new List<GroupResponse>();
            var groupData = _Context.Group.Where(c => c.groupMembers.Any(aa => aa.User_Id == id)).ToList();
            for(var i = 0; i < groupData.Count; i++)
            {
                var group = new GroupResponse();
                group = await GetGroupAsync(groupData[i].GroupId);
                groups.Add(group);
            }
            return groups;
        }

        public async Task<Group> InsertGroupAsync(GroupModel group)
        {
            Group grp = new Group();
            grp.GroupName = group.GroupName;
            grp.CreatedDate = group.CreatedDate;
            grp.CreatorId = group.CreatorId;
            _Context.Group.Add(grp);
            foreach(var member in group.Members)
            {
                GroupMember grpMember = new GroupMember();
                grpMember.Group_Id = grp.GroupId;
                grpMember.User_Id = member;
                _Context.GroupMember.Add(grpMember);
            }
            for(var i = 0; i < group.Members.Count-1; i++)
            {
                for(var j = i + 1; j < group.Members.Count; j++)
                {
                    var fExist = _Context.Friend.SingleOrDefault(c => c.UserId == group.Members[i] && c.FriendId == group.Members[j]);
                    if (fExist == null)
                    {
                        var Exist= _Context.Friend.SingleOrDefault(c => c.UserId == group.Members[j] && c.FriendId == group.Members[i]);
                        if (Exist == null)
                        {
                            Friend newFriend = new Friend
                            {
                                UserId = group.Members[i],
                                FriendId = group.Members[j]
                            };
                            _Context.Friend.Add(newFriend);
                            await _Context.SaveChangesAsync();
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
                _Logger.LogError($"Error in {nameof(InsertGroupAsync)}: " + exp.Message);
            }
            return grp;
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var sData = await _Context.Settlement.Where(c => c.GroupId == id).ToListAsync();
            if (sData.Count > 0)
            {
                for (var i = 0; i < sData.Count; i++)
                {
                    _Context.Remove(sData[i]);
                }
                await _Context.SaveChangesAsync();
            }

            var billData = await _Context.Expense.Where(c => c.GroupId == id).ToListAsync();
            if (billData.Count > 0)
            {
                for(var i = 0; i < billData.Count; i++)
                {
                    _Context.Remove(billData[i]);
                }
                await _Context.SaveChangesAsync();
            }

            var transData = await _Context.Transactions.Where(c => c.GroupId == id).ToListAsync();
            if (transData.Count > 0)
            {
                for(var i = 0; i < transData.Count; i++)
                {
                    _Context.Remove(transData[i]);
                }
                await _Context.SaveChangesAsync();
            }
            var group = await _Context.Group.SingleOrDefaultAsync(c => c.GroupId == id);
            _Context.Remove(group);       
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteGroupAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<GroupMember> InsertGroupMemberAsync(int Groupid, int Memberid)
        {
            var member = _Context.GroupMember.SingleOrDefault(c => c.Group_Id == Groupid && c.User_Id == Memberid);
            if (member == null)
            {
                GroupMember newMember = new GroupMember
                {
                    Group_Id = Groupid,
                    User_Id = Memberid
                };
                var memberData = _Context.GroupMember.Where(c => c.Group_Id == Groupid).ToList();
                for (var i = 0; i < memberData.Count; i++)
                {
                    var exist = _Context.Friend.SingleOrDefault(c => c.UserId == memberData[i].User_Id && c.FriendId == Memberid);
                    if (exist == null)
                    {
                        var rExist = _Context.Friend.SingleOrDefault(c => c.UserId == Memberid && c.FriendId == memberData[i].User_Id);
                        if (rExist == null)
                        {
                            Friend newFriend = new Friend
                            {
                                UserId = memberData[i].User_Id,
                                FriendId = Memberid
                            };
                            _Context.Friend.Add(newFriend);
                            await _Context.SaveChangesAsync();
                        }
                    }
                }
                _Context.GroupMember.Add(newMember);
                await _Context.SaveChangesAsync();
                return  newMember;
            }
            else
            {
                _Context.GroupMember.Attach(member);
                await _Context.SaveChangesAsync();
                return member;
            }
        }

        public async Task<bool> DeleteGroupMemberAsync(int Groupid, int Memberid)
        {
            var data = _Context.GroupMember.SingleOrDefault(c => c.Group_Id==Groupid && c.User_Id==Memberid);
            _Context.Remove(data);
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteGroupMemberAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> UpdateGroupAsync(Group group)
        {           
            _Context.Group.Attach(group);
            _Context.Entry(group).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateGroupAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<List<GroupResponse>> GetCommenGroupsAsync(int Userid, int Friendid)
        {
            List<GroupResponse> groups = new List<GroupResponse>();
            var groupData = _Context.Group.Where(c => c.groupMembers.Any(aa => aa.User_Id == Userid)).Include(c=>c.groupMembers).ToList();
            var gData = groupData.Where(c => c.groupMembers.Any(aa => aa.User_Id == Friendid)).ToList();
            for (var i = 0; i < gData.Count; i++)
            {
                var group = new GroupResponse();
                group = await GetGroupAsync(gData[i].GroupId);
                groups.Add(group);
            }
            return groups;
        }
    }
}
