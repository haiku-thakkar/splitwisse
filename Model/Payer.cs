using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class Payer
    {
        [Key]
        public int Id { get; set; }

        public int BillId { get; set; }
        public Expense Bill { get; set; }
        
        public int? PayerId { get; set; }
        public User User { get; set; }

        public decimal PaidAmount { get; set; }
    }
}
