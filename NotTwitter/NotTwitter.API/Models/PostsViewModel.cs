using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
    public class PostsViewModel
    {
   
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeSent { get; set; }
        public string Content { get; set; }

        public int Likes { get; set; }

    }
}
