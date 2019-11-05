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
		[Display(Name = "What do you think?")]

		public string Text { get; set; }
        public Comment Comments { get; set; }

		/*[Display(Name = "Upload a photo")]
		public IFormFile Photo { get; set; }*/
	}
}
