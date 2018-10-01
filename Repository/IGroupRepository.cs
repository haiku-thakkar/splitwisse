using SecondSplitWise.DataModel;
using SecondSplitWise.Model;
using SecondSplitWise.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Repository
{
    public interface IGroupRepository
    {
        Task<Group> InsertGroupAsync(GroupModel group);
        
        Task<GroupResponse> GetGroupAsync(int id);
        Task<List<GroupResponse>> GetGroupsAsync(int id);
        Task<List<GroupResponse>> GetCommenGroupsAsync(int Userid, int Friendid);
        Task<bool> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int id);
        Task<GroupMember> InsertGroupMemberAsync(int Groupid, int Memberid);
        Task<bool> DeleteGroupMemberAsync(int Groupid, int Memberid);
    }
}
