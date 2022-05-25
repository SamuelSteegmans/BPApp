using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Shared.Models
{
    public class ApplicationUser: IdentityUser
    {
        //Chat
        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }

        //Connections
        public virtual ICollection<Like> Likes { get; set; }

        //Profile
        public string? FirstName { get; set; }
        public int? Age { get; set; }
        public string? Area { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Interests { get; set; }
        public string? Description { get; set; }
        public string? ProfilePicturePath { get; set; }

        //Constructor
        public ApplicationUser()
        {
            ChatMessagesFromUsers = new HashSet<ChatMessage>();
            ChatMessagesToUsers = new HashSet<ChatMessage>();
            Likes = new HashSet<Like>();
        }
    }
}
