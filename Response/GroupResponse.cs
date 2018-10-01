using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Response
{
    public class GroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; } 
        public DateTime CreatedDate { get; set; }
        public List<MemberResponse> Members { get; set; }
    }
}
