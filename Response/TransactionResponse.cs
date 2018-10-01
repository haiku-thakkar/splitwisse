using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Response
{
    public class TransactionResponse
    {
        
        public int Id { get; set; }

        public MemberResponse Payer { get; set; }
        public MemberResponse Receiver { get; set; }
        
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
