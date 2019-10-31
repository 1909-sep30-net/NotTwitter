using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Interfaces
{
	public interface IFriendRequestRepository
	{
		public void Create(int senderId, int receiverId);
		public void Accept(int senderId, int receiverId);
		public void Decline(int senderId, int receiverId);
		public void Delete(int senderId, int receiverId);
	}
}
