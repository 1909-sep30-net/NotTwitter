using NotTwitter.DataAccess.Entities.Enum;
using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace NotTwitter.DataAccess.Repositories
{
	public class FriendRequestRepository : IFriendRequestRepository
	{
		private readonly NotTwitterDbContext _context;

		public FriendRequestRepository(NotTwitterDbContext db)
		{
			_context = db;
		}

        public FriendRequest GetFriendRequest(int senderId, int receiverId)
        {
            return Mapper.MapFriendRequest(_context.FriendRequests
                    .FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId));
        }

        /// <summary>
        /// Returns a list of all pending requests for a user.
        /// </summary>
        /// <param name="userId">User with the pending requests</param>
        /// <returns>List of all pending friend requests</returns>
        public IEnumerable<FriendRequest> GetAllFriendRequests(int userId)
        {
			return _context.FriendRequests.Where(fr => fr.SenderId == userId)
                .Select(Mapper.MapFriendRequest).ToList();
        }

        /// <summary>
        /// Checks if a given friend request already exists
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
		public int FriendRequestStatus(int senderId, int receiverId)
		{
			return _context.FriendRequests.Where(r => r.SenderId == senderId && r.ReceiverId == receiverId).FirstOrDefault().FriendRequestStatus;
		}
	    
        /// <summary>
        /// Creates friend request in database
        /// </summary>
        /// <param name="request">Request to be added</param>
		public void CreateFriendRequest(FriendRequest request)
		{
			
			_context.FriendRequests.Add(Mapper.MapFriendRequest(request));
		}


        /// <summary>
        /// Updates friend request in the database with new values
        /// </summary>
        /// <param name="request">Request to be updated</param>
        public void UpdateFriendRequest(FriendRequest request)
        {
            var newEntity = Mapper.MapFriendRequest(request);
            var oldEntity = _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefault();
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes request from the database
        /// </summary>
        /// <param name="request">Request to be deleted</param>
		public void DeleteFriendRequest(FriendRequest request)
		{
            var requestEntity = _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefault();

            _context.FriendRequests.Remove(requestEntity);
		}

        /// <summary>
        /// Save changes to the context
        /// </summary>
        public void Save()
        {
            // TODO: log
            _context.SaveChanges();
        }
	}
}
