using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Entities.Enum;
using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;
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
			_user = user;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<FriendRequest> GetAllPendingFriendRequests(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if a given friend request already exists
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
		public bool FriendRequestExists(FriendRequest request)
		{
			return _context.FriendRequests.Any(fr => fr.SenderId == request.SenderId && fr.ReceiverId == request.ReceiverId);
		}
			 
		public void CreateFriendRequest(FriendRequest request)
		{
			if (!this.FriendRequestExists(request))
			{
				var friendRequest = Mapper.MapFriendRequest(request);
				_context.FriendRequests.Add(friendRequest);
				_context.SaveChanges();
			}
		}


        /// <summary>
        /// Updates friend request in the database with new values
        /// </summary>
        /// <param name="request"></param>
        public void UpdateFriendRequest(FriendRequest request)
        {
            var newEntity = Mapper.MapFriendRequest(request);
            var oldEntity = _context.FriendRequests.Find(request.FriendRequestId) ?? throw new ArgumentNullException("Request does not exist.", nameof(request));
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes request from the database
        /// </summary>
        /// <param name="request"></param>
		public void DeleteFriendRequest(FriendRequest request)
		{
            // If the request doesnt exist, throw exception
            var requestEntity = _context.FriendRequests.Find(request.FriendRequestId) ?? throw new ArgumentNullException("Request does not exist.", nameof(request));

            // Otherwise, remove from database
            _context.FriendRequests.Remove(requestEntity);
		}

        public void Save()
        {
            // TODO: log
            _context.SaveChanges();
        }
	}
}
