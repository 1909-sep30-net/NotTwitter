using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
    public class PostsPostModel
    {
        [Required]
        [MaxLength(281)]
        public string Content { get; set; }
    }
}
