﻿namespace NotTwitter.DataAccess.Entities
{
    public class FriendRequests
	{
		//public int FriendRequestId { get; set; }

		public int SenderId { get; set; }

		public Users Sender { get; set; }

		public int ReceiverId { get; set; }

		public Users Receiver { get; set; }

		
		public int FriendRequestStatus { get; set; }
	}
}
