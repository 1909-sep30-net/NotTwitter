using System;
using System.Collections.Generic;
using System.Text;

namespace NotTwitter.Library.Interfaces
{
	public interface IFriendRequestRepository
	{
		public void Create(Library.Models.FriendRequest request);
		public void Accept(Library.Models.FriendRequest request);
		public void Decline(Library.Models.FriendRequest request);
		public void Delete(int senderId, int receiverId);
	}
}
