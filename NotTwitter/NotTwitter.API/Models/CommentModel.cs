using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
	public class CommentModel
	{
		public int CommentId { get; set; }
		public int PostId { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
        [MinLength(1)]
		public string Content { get; set; }

		public DateTime TimeSent { get; set; }
	}
}
