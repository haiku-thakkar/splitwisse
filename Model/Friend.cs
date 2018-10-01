using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Model
{
    public class Friend
    {
        [Key]
        public int FriendListId { get; set; }

        public int? UserId { get; set; }
        public User user { get; set; }

        public int FriendId { get; set; }
        public User friend { get; set; }

    }
}
