using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class GroupMember
    {
        [Key]
        public int GroupMemberId { get; set; }
               
        public int Group_Id { get; set; }
        
        public Group Group { get; set; }
              
        public int? User_Id { get; set; }
        
        public User User { get; set; }
    }
}
