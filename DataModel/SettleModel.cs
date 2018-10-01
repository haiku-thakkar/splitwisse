using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.DataModel
{
    public class SettleModel
    {
        
        public int PayerId { get; set; }
        public int SharedMemberId { get; set; }
        public int? GroupId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
