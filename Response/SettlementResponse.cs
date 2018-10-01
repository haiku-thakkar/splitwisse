using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Response
{
    public class SettlementResponse
    {
        public int Id { get; set; }
        public int Payer_id { get; set; }
        public string PayerName { get; set; }
        public int Receiver_id { get; set; }
        public string ReceiverName { get; set; }
        public int Group_id { get; set; }
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }
}
