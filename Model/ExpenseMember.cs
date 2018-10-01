using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class ExpenseMember
    {
        [Key]
        public int BillMemberId { get; set; }

        public int Billid { get; set; }
        public Expense Bill { get; set; }

        public int? SharedMemberId { get; set; }
        public User User { get; set; }

        public decimal AmountToPay { get; set; }
    }
}
