using System;
using System.Collections.Generic;

namespace Dashboard.Models
{
    public class Message : BaseEntity
    {
        public int MessageID { get; set; }
        public string Msg { get; set; }
        public int WriterID { get; set; }
        public User Writer { get; set; }
        public int ReceiverID { get; set; }
        public User Receiver { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}