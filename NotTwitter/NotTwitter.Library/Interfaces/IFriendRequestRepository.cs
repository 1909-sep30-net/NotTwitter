using NotTwitter.Library.Models;
using System.Collections.Generic;

namespace NotTwitter.Library.Interfaces
{
	public interface IFriendRequestRepository
	{
		public int FriendRequestStatus(int senderId, int receiverId);
		/// <summary>
		/// Get all pending friend requests pending for a user
		/// </summary>
		/// <param name="userId">User's id</param>
		/// <returns>List of friend requests for the given user</returns>
		public IEnumerable<FriendRequest> GetAllFriendRequests(int userId);

        /// <summary>
        /// Creates a new friend request inside database
        /// </summary>
        /// <param name="request"></param>
		public void CreateFriendRequest(FriendRequest request);

        /// <summary>
        /// Updates database with friend request with new status
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>Status may be changed to Accepted, Declined, etc</remarks>
        public void UpdateFriendRequest(FriendRequest request);
		public void DeleteFriendRequest(FriendRequest request);
        public void Save();
	}
}
