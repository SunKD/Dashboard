using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class User : BaseEntity
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserLevel { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        
        [InverseProperty("Writer")]
        public List<Message> Wrote { get; set; } = new List<Message>();
    
        [InverseProperty("Receiver")]
        public List<Message> Recieved { get; set; } = new List<Message>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}