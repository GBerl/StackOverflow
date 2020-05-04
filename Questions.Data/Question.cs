using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Questions.Data
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public List<QuestionTag> QuestionTags { get; set; }
        public List<Like> Likes { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
