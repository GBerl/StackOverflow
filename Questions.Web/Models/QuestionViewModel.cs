using Questions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questions.Web.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool Liked { get; set; }
    }
}
