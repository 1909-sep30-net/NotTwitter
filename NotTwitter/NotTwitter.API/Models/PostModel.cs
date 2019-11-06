using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
	public class PostModel
	{
		[Required]
		public User User { get; set; }

		[Required]
		public string Text { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; }

        [Required]
        public DateTime TimeSent { get; set; }

		/*[Display(Name = "Upload a photo")]
		public IFormFile Photo { get; set; }*/
	}
}
