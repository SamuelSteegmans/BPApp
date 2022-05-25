using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Shared.Models
{
    public class Like
    {
        public int ID { get; set; }
        public string originUserId { get; set; }
        public virtual ApplicationUser? originUser { get; set; }
        public string targetUserId { get; set; }
        public virtual ApplicationUser? targetUser { get; set; }
    }
}
