using NotTwitter.Library.Models;
using System.ComponentModel.DataAnnotations;

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