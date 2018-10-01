using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.DataModel
{
    public class GroupModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<int> Members { get; set; }

    }
}
