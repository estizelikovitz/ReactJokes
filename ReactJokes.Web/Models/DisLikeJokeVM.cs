using ReactJokes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactJokes.Web.Models
{
    public class DisLikeJokeVM
    {
        public Joke joke { get; set; }
        public User user { get; set; }
    }
}
