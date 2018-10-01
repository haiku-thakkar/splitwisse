using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSplitWise.Response
{
    public class MemberResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MemberResponse()
        {
          
        }

        public MemberResponse(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
