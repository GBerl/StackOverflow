using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Questions.Data;
using Questions.Web.Models;

namespace Questions.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connection;
        public HomeController(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("Con");
        }
        public IActionResult Index()
        {
            var repository = new QuestionRepository(_connection);
            return View(repository.ShowQuestions());
        }
        [Authorize]
        public IActionResult AskQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddQuestionTags(Question question, List<string> tags)
        {
            var repository = new QuestionRepository(_connection);
            repository.AddQuestion(question, tags);
            return Redirect("/");
        }
        public IActionResult Question(int id)
        {
            var repository = new QuestionRepository(_connection);
            if(repository.GetQuestionById(id)==null)
            {
                return Redirect("/");
            }
            var view = new QuestionViewModel
            {
                Question = repository.GetQuestionById(id),
                IsLoggedIn = User.Identity.IsAuthenticated,
                Liked=repository.UserLikedQuestion(id, User.Identity.Name)
            };
           
            return View(view);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Answer(Answer answer)
        {
            string email = User.Identity.Name;
            var repository = new QuestionRepository(_connection);
            var answered=repository.Answer(answer,email);
            return Json(answered.UserEmail);
        }
        [Authorize]
        [HttpPost]
        public IActionResult UpdateLikes(int id)
        {
            string email = User.Identity.Name;
            var repository = new QuestionRepository(_connection);
            repository.UpdateLikes(id,email);
            return Json(id);
        }
        [Authorize]
        public IActionResult GetLikesCount(int id)
        {
            var repository = new QuestionRepository(_connection);
            return Json(repository.GetLikesForQuestion(id));
        }
    }
}
public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        string value = session.GetString(key);

        return value == null ? default(T) :
            JsonConvert.DeserializeObject<T>(value);
    }
}
