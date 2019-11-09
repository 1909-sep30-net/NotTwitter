using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
    public class CommentPostModel
    {

        [Required(ErrorMessage ="Author's user ID is required.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage ="Post ID is required.")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(1, ErrorMessage ="Content cannot be empty.")]
        [MaxLength(281, ErrorMessage ="Content cannot be more than 281 characters.")]
        public string Content { get; set; }
    }
}
