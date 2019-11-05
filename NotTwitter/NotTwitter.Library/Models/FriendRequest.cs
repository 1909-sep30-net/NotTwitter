namespace NotTwitter.Library.Models
{
	public class FriendRequest
	{
		//public int FriendRequestId { get; set; }

        /// <summary>
        /// Foreign key for sender
        /// </summary>
		public int SenderId { get; set; }

        /// <summary>
        /// Sender nav prop
        /// </summary>
		public User Sender { get; set; }

        /// <summary>
        /// Foreign key for receiver
        /// </summary>
		public int ReceiverId { get; set; }

        /// <summary>
        /// Receiver nav prop
        /// </summary>
		public User Receiver { get; set; }

        /// <summary>
        /// Status of the friendship
        /// </summary>
        /// <remarks>
        /// 0 - Pending
        /// 1 - Accepted
        /// 2 - Declined
        /// 3 - Blocked
        /// </remarks>
		public int FriendRequestStatus { get; set; }
	}
}
