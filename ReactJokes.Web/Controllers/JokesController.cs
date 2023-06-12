using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ReactJokes.Data;
using ReactJokes.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReactJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {

        private string _connectionString;
        private IConfiguration _configuration;

        public JokesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _configuration = configuration;
        }
        [HttpPost]
        [Route("signup")]
        public void Signup(User user)
        {
            var repo = new Repository(_connectionString);
            repo.AddUser(user);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginVM viewModel)
        {
            var repo = new Repository(_connectionString);
            var user = repo.Login(viewModel.Email, viewModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>()
            {
                new Claim("user", viewModel.Email)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecret")));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(signingCredentials: credentials,
                claims: claims);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { token = tokenString });
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            string email = User.FindFirst("user")?.Value; //get currently logged in users email - this is in place of User.Identity.Name
            if (String.IsNullOrEmpty(email))
            {
                return null;
            }

            var repo = new Repository(_connectionString);
            return repo.GetByEmail(email);
        }
        private class TempJoke
        {
            public int JokeId { get; set; }
            public string Setup { get; set; }
            public string Punchline { get; set; }
        }

        [HttpGet]
        [Route("getjoke")]
        public Joke GetJoke()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var tempjoke = JsonConvert.DeserializeObject<List<TempJoke>>(json).First();
            var joke = new Joke
            {
                OriginalId = tempjoke.JokeId,
                Setup = tempjoke.Setup,
                Punchline = tempjoke.Punchline
            };
            var repo = new Repository(_connectionString);
            if (!repo.HasJoke(joke.OriginalId))
            {
                repo.AddJoke(joke);
                return joke;
            }
            return repo.GetJokeByOriginalId(joke.OriginalId);
        }

        [HttpGet]
        [Route("getalljokes")]
        public List<Joke> GetAllJokes()
        {
            var repo = new Repository(_connectionString);
            return repo.GetAllJokes();
        }

        [HttpGet]
        [Route("getlikesforjoke")]
        public int GetLikesForJoke(int id)
        {
            var repo = new Repository(_connectionString);
            return repo.GetUserLikedJokes(id).Where(ulj => ulj.Liked == true).Count();
        }

        [HttpGet]
        [Route("getdislikesforjoke")]
        public int GetDislikesForJoke(int id)
        {
            var repo = new Repository(_connectionString);
            return repo.GetUserLikedJokes(id).Where(ulj => ulj.Liked == false).Count();
        }

        [HttpGet]
        [Route("wasliked")]
        public bool WasLiked(DisLikeJokeVM vm)
        {
            var repo = new Repository(_connectionString);
            return repo.IsLiked(vm.user.Id, vm.joke.Id);
           
        }

        [HttpPost]
        [Authorize]
        [Route("likejoke")]
        public void LikeJoke(DisLikeJokeVM vm)
        {
            var repo = new Repository(_connectionString);
            repo.LikeJoke(vm.joke, vm.user.Id);
        }

        [HttpPost]
        [Authorize]
        [Route("dislikejoke")]
        public void DislikeJoke(DisLikeJokeVM vm)
        {
            var repo = new Repository(_connectionString);
            repo.DislikeJoke(vm.joke, vm.user.Id);
        }

    }
}
