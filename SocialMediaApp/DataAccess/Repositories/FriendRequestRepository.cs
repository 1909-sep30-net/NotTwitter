using DataAccess.Entities;
using DataAccess.Entities.Enum;
using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
	public class FriendRequestRepository : IFriendRequestRepository
	{
		private readonly NotTwitterDbContext _context;
		private readonly IUserRepository _user;

		public FriendRequestRepository(NotTwitterDbContext db, IUserRepository user)
		{
			_context = db;
			_user = user;
		}

		public bool Exists(int senderId, int receiverId)
		{
			return _context.FriendRequests.Any(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
		}
			 
		public void Create(int senderId, int receiverId)
		{
			if (!this.Exists(senderId, receiverId))
			{
				var friendRequest = new FriendRequests
				{
					SenderId = senderId,
					ReceiverId = receiverId,
					FriendRequestStatus = FriendRequestStatus.Pending
				};

				_context.FriendRequests.Add(friendRequest);
				_context.SaveChanges();
			}
		}
		public void Accept(int senderId, int receiverId)
		{
			if (!this.Exists(senderId, receiverId))
			{
				var friendRequest = _context.FriendRequests.FirstOrDefault(fr => fr.ReceiverId == receiverId && fr.SenderId == senderId);
				friendRequest.FriendRequestStatus = FriendRequestStatus.Accepted;
				_user.MakeFriends(senderId, receiverId);
				_context.SaveChanges();
			}
		}
		public void Decline(int senderId, int receiverId)
		{
			if (!this.Exists(senderId, receiverId))
			{
				var friendRequest = _context.FriendRequests.FirstOrDefault(fr => fr.ReceiverId == receiverId && fr.SenderId == senderId);
				friendRequest.FriendRequestStatus = FriendRequestStatus.Declined;
				_context.FriendRequests.Remove(friendRequest);
				_context.SaveChanges();
			}
		}

		public void Delete(int senderId, int receiverId)
		{
			throw new NotImplementedException();
		}
	}
}
