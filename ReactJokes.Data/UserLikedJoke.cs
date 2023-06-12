using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReactJokes.Data
{
    public class UserLikedJoke
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public DateTime Date { get; set; }
        public bool Liked { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Joke Joke { get; set; }

    }
}
