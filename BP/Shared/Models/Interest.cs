using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Shared.Models
{
    public class Interest
    {
        public long Id { get; set; }
        public string InterestName { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public Interest()
        {
            Users = new List<ApplicationUser>();
        }
    }
}
