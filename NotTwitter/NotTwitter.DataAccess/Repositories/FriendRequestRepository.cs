using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Entities.Enum;
using NotTwitter.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotTwitter.DataAccess.Repositories
{
	public class FriendRequestRepository : IFriendRequestRepository
	{
		private readonly NotTwitterDbContext _context;
		private readonly IUserRepository _user;

		public FriendRequestRepository(NotTwitterDbContext db, IUserRepository user)
		{
			_context = db;
			_user = user; // need to fix not call it as param
		}

		public bool Exists(int senderId, int receiverId)
		{
			return _context.FriendRequests.Any(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
		}
			 
		public void Create(Library.Models.FriendRequest request)
		{
			if (!this.Exists(request.Sender.UserID, request.Receiver.UserID))
			{
				var friendRequest = Mapper.MapFriendRequest(request);
				_context.FriendRequests.Add(friendRequest);
				_context.SaveChanges();
			}
		}
		public void Accept(Library.Models.FriendRequest request)
		{
			if (this.Exists(request.Sender.UserID, request.Receiver.UserID))
			{
				var friendRequest = Mapper.MapFriendRequest(request);
				_context.SaveChanges();
			}
		}
		public void Decline(Library.Models.FriendRequest request)
		{
			if (this.Exists(request.SenderId, request.ReceiverId))
			{
				var friendRequest = Mapper.MapFriendRequest(request);
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
