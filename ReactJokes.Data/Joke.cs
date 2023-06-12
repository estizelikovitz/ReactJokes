using System;
using System.Collections.Generic;

namespace ReactJokes.Data
{
    public class Joke
    {
        public int OriginalId { get; set; }
        public int Id { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }
        public List<UserLikedJoke> UserLikedJokes { get; set; }
    }
}
