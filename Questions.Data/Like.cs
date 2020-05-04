using System;
using System.Collections.Generic;
using System.Text;

namespace Questions.Data
{
    public class Like
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public Question Question { get; set; }
        public User User { get; set; }
    }
}
