using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questions.Data
{
    public class QuestionRepository
    {
        private string _connection;

        public QuestionRepository(string connection)
        {
            _connection = connection;
        }

        public void AddQuestion(Question question, List<string> Tags)
        {
            using (var context = new QuestionContext(_connection))
            {
                context.Questions.Add(question);
                foreach (string name in Tags)
                {
                    var tag = new Tag();
                    tag = context.Tags.FirstOrDefault(t => t.Name == name);
                    if (tag == null)
                    {
                        tag = new Tag { Name = name };
                        context.Tags.Add(tag);
                        context.SaveChanges();
                    }
                    var questionTag = new QuestionTag
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id
                    };

                    context.QuestionsTags.Add(questionTag);
                    context.SaveChanges();
                }
            }
        }
        public IEnumerable<Question> ShowQuestions()
        {
            using (var context = new QuestionContext(_connection))
            {
                return context.Questions.Include(q => q.QuestionTags)
                        .ThenInclude(qt => qt.Tag).Include(q => q.Likes).ToList().OrderByDescending(q => q.Id);
            }
        }
        public Question GetQuestionById(int id)
        {
            using (var context = new QuestionContext(_connection))
            {
                return context.Questions.Include(q => q.QuestionTags)
                        .ThenInclude(qt => qt.Tag).Include(q => q.Likes).Include(q => q.Answers).FirstOrDefault(q => q.Id == id);
            }
        }
        public Answer Answer(Answer answer, string email)
        {
            answer.UserEmail = email;
            using (var context = new QuestionContext(_connection))
            {
                context.Answers.Add(answer);
                context.SaveChanges();
                return answer;
            }
        }
        public void UpdateLikes(int id, string email)
        {
            using (var context = new QuestionContext(_connection))
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                var likes = new Like
                {
                    UserId = user.Id,
                    QuestionId = id
                };
                context.Likes.Add(likes);
                context.SaveChanges();
            }
        }
        public bool UserLikedQuestion(int id, string email)
        {
            using (var context = new QuestionContext(_connection))
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if(user==null)
                {
                    return false;
                }
                return context.Likes.Any(l => l.QuestionId == id && l.UserId == user.Id);
            }
        }
        public int GetLikesForQuestion(int id)
        {
            using (var context = new QuestionContext(_connection))
            {
                return context.Likes.Where(l => l.QuestionId == id).Count(); ;
            }
        }
    }
}
