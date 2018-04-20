using System;
using System.Collections.Generic;

namespace Dashboard.Models
{
    public class Comment : BaseEntity
    {
        public int CommentID { get; set; }
        public string Cmt { get; set; }
        public int MessageID { get; set; }
        public Message Message { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}