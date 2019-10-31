﻿using DataAccess.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Entities
{
	public class FriendRequests
	{
		public int FriendRequestId { get; set; }

		public int SenderId { get; set; }

		public Users Sender { get; set; }

		public int ReceiverId { get; set; }

		public Users Receiver { get; set; }

		
		public FriendRequestStatus FriendRequestStatus { get; set; }
	}
}
