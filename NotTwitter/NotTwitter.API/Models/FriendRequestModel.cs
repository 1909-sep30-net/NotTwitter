using Library.Models;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.API.Models
{
	public class FriendRequestModel
	{
		[Required]
		public int SenderId { get; set; }

		[Required]
		public int ReceiverId { get; set; }

        public int Status { get; set; }

	}
}
