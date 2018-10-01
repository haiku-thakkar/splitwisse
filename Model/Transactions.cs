using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }

        public int TransPayersId { get; set; }
        public User TransPayers { get; set; }

        public int? TransReceiversId { get; set; }
        public User TransReceivers { get; set; }

        public int? GroupId { get; set; }
        public Group groupsId { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
