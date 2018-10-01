using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.DataModel
{
    public class ExpenseModel
    {
        public string BillName { get; set; }

        public int CreatorId { get; set; }
       
        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public decimal Amount { get; set; }

        public int? GroupId { get; set; }
        public List<PayerModel> Payer { get; set; }
        public List<PayerModel> SharedMember { get; set; }

        public List<SettleModel> SettleModels { get; set; }
    }
}
