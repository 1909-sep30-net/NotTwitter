using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotTwitter.Library.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        /// <summary>
        /// Given an ID, returns matching user from DB
        /// </summary>
        /// <param name="id">User ID to be searched for</param>
        /// <returns>User matching the given ID</returns>
        public User GetUserByID(int id);

        /// <summary>
        /// Given an ID, returns matching user, including their list of friends from DB
        /// </summary>
        /// <param name="id">User ID to be searched for</param>
        /// <returns>User matching the given ID</returns>
        public User GetUserWithFriends(int id);

        /// <summary>
        /// Returns list of users with name matching given string
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>Checks combination of user's first and last name</remarks>
        /// <returns></returns>
        public IEnumerable<User> GetUsersByName(string name);

        /// <summary>
        /// Given a business model user, add user to database
        /// </summary>
        /// <param name="newUser">User to add to db</param>
        public void AddUser(User newUser);

        /// <summary>
        /// Update the database with new user information
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user);

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUserByID(int id);

	
		/// <summary>
		/// Add Friend
		/// </summary>
		/// <param name="senderId", name="receiverId"></param>
		public void AddFriendShip(Friendship newFriend);

		/// <summary>
		/// Saves changes to database
		/// </summary>
		public void Save();
    }
}
