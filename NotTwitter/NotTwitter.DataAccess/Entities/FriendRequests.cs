namespace NotTwitter.DataAccess.Entities
{
    public class FriendRequests
	{

		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
		public int FriendRequestStatus { get; set; }

		public Users Sender { get; set; }
		public Users Receiver { get; set; }

	}
}
