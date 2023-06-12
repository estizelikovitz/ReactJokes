using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactJokes.Data
{
    public class Repository
    {
        private string _connectionString;
        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user; //success!!
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using var context = new JokesDataContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public void AddUser(User user)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = hash;
            using var context = new JokesDataContext(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public bool HasJoke(int id)
        {
            using var context = new JokesDataContext(_connectionString);
            return context.Jokes.Any(j => j.OriginalId == id);
        }

        public bool IsLiked(int userId, int jokeId)
        {
            using var context = new JokesDataContext(_connectionString);
            return context.UserLikedJokes.Any(ulj => ulj.UserId == userId && ulj.JokeId == jokeId);

        }
        public void AddJoke(Joke joke)
        {
            using var context = new JokesDataContext(_connectionString);
            context.Jokes.Add(joke);
            context.SaveChanges();
        }
        public List<Joke> GetAllJokes()
        {
            using var context = new JokesDataContext(_connectionString);
            return context.Jokes.Include(j => j.UserLikedJokes).ThenInclude(ulj => ulj.Liked).ToList();
        }

        public Joke GetJokeByOriginalId(int id)
        {
            using var context = new JokesDataContext(_connectionString);
            return context.Jokes.FirstOrDefault(j=>j.OriginalId==id);
        }
        public List<UserLikedJoke> GetUserLikedJokes(int id)
        {
            using var context = new JokesDataContext(_connectionString);
            return context.UserLikedJokes.Where(ulj => ulj.JokeId == id).ToList();
        }
        public void LikeJoke(Joke joke, int userId)
        {
            using var context = new JokesDataContext(_connectionString);
            if(context.UserLikedJokes.Any(ulj=>ulj.Joke==joke && ulj.UserId==userId))
            {
                return;
            }
            else
            {
                UserLikedJoke ulj = new();
                ulj.JokeId = joke.Id;
                ulj.UserId = userId;
                ulj.Liked = true;
                ulj.Date = DateTime.Now.Date;
                context.UserLikedJokes.Add(ulj);
                context.SaveChanges();
            }

        }
        public void DislikeJoke(Joke joke, int userId)
        {
            using var context = new JokesDataContext(_connectionString);
            //var ulj=context.UserLikedJokes.FirstOrDefault(ulj => ulj.JokeId == joke.Id && ulj.UserId == userId);

            UserLikedJoke ulj2 = new();
            ulj2.JokeId = joke.Id;
            ulj2.UserId = userId;
            ulj2.Liked = false;
            ulj2.Date = DateTime.Now.Date;
            context.UserLikedJokes.Add(ulj2);
            context.SaveChanges();

        }

    }
}