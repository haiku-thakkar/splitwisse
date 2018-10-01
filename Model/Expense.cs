using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class Expense
    {
        [Key]
        public int BillId { get; set; }
        public string BillName { get; set; }

        public int CreatorId { get; set; }
        public User User { get; set; }

        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public decimal Amount { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

        [ForeignKey("BillId")]
        public ICollection<Payer> Payers { get; set; }

        [ForeignKey("Billid")]
        public ICollection<ExpenseMember> BillMembers { get; set; }

    }
}
