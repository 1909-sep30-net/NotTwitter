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
		public int FriendRequestId { get; set; }
		[Required]
		public int SenderId { get; set; }

		public User Sender { get; set; }
		[Required]
		public int ReceiverId { get; set; }

		public User Receiver { get; set; }

		public int FriendRequestStatus { get; set; }
	}
}
