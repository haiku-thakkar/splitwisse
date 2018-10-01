using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class SinglePayer
    {
        [Key]
        public int IndividualPayerid { get; set; }

        public int BillId { get; set; }

        public int PayerId { get; set; }

        public decimal PaidAmount { get; set; }
    }
}
