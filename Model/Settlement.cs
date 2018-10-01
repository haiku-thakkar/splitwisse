using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class Settlement
    {
        [Key]
        public int SettlementId { get; set; }

        [Column("Payer_Person")]
        public int PayerId { get; set; }
        public User Payer { get; set; }

        [Column("Requestor_Person")]
        public int? SharedMemberId { get; set; }
        public User SharedMember { get; set; }

        public int? GroupId { get; set; }
        public Group groupsId { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
