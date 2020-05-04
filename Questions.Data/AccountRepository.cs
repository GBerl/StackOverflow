using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questions.Data
{
    public class AccountRepository
    {
        private string _connection;

        public AccountRepository(string connection)
        {
            _connection = connection;
        }

        public void AddUser(string name, string email, string password)
        {
            string hashedpassword = BCrypt.Net.BCrypt.HashPassword(password);
            using (var context = new QuestionContext(_connection))
            {
                var user = new User
                {
                    Name = name,
                    Email = email,
                    HashedPassword = hashedpassword
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public User Login(string password, string email)
        {
            User user = new User();

            using (var context = new QuestionContext(_connection))
            {
                user = context.Users.FirstOrDefault(u => u.Email == email);
                if (BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
                {
                    return user;
                }
                return null;
            }
        }
    }
}
