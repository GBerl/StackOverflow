﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Questions.Data
{
    public class QuestionTag
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; }
        public Tag Tag { get; set; }
    }
}
