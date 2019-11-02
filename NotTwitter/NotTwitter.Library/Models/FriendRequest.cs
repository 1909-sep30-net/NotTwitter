using Library.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotTwitter.Library.Models
{
	public class FriendRequest
	{
		public int FriendRequestId { get; set; }

		public int SenderId { get; set; }

		public User Sender { get; set; }

		public int ReceiverId { get; set; }

		public User Receiver { get; set; }


		public int FriendRequestStatus { get; set; }
	}
}
