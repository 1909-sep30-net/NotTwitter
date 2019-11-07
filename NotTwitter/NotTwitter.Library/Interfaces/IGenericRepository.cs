﻿using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotTwitter.Library.Interfaces
{
    public interface IGenericRepository: IDisposable
    {
        /* Comment Repository */
        public void CreateComment(Comment newComment);
        public IEnumerable<Comment> GetCommentsByPostId(int postId);
        public IEnumerable<Comment> GetCommentsByUserId(int userId);
        public void UpdateComment(Comment newComment);
        public void DeleteCommentsByPostId(int postId);

        /* End */

        /* FriendRequest Repository */

        public int FriendRequestStatus(int senderId, int receiverId);

        public FriendRequest GetFriendRequest(int senderId, int receiverId);

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
        /// <param name="request">Request model to be updated</param>
        /// <remarks>Status may be changed to Accepted, Declined, etc</remarks>
        public void UpdateFriendRequest(FriendRequest request);
        public void DeleteFriendRequest(FriendRequest request);
        /* End */

        /* Post Repository */
        /// <summary>
        /// Stores new post in database
        /// </summary>
        /// <param name="post">Post to be stored</param>
        public Task CreatePost(Post post, User author);

        /// <summary>
        /// Gets post by ID, including comments
        /// </summary>
        /// <param name="postId">ID of the post</param>
        /// <returns>A post including the comments</returns>
        public Task<Post> GetPostById(int postId);

        /// <summary>
        /// Gets all posts from a specific user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>All posts with comments from a user</returns>
        public Task<IEnumerable<Post>> GetPostsByUser(int userId);

        /// <summary>
        /// Gets all posts in database
        /// </summary>
        /// <returns>All posts in data base</returns>
        public Task<IEnumerable<Post>> GetAllPosts();

        /// <summary>
        /// Updates database with given post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        public Task UpdatePost(Post post);

        /// <summary>
        /// Deletes post from database
        /// </summary>
        /// <param name="postId">Id of the post to be removed</param>
        public Task DeletePost(int postId);

        /* End */

        /* User Repository */
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
        /// <param name="user">User to update</param>
        public void UpdateUser(User user);

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id">ID of User to delete</param>
        public void DeleteUserByID(int id);

        /// <summary>
        /// Creates new friendship
        /// </summary>
        /// <param name="newFriend">Represents a one way friend relationship</param>
        public void AddFriendShip(Friendship newFriend);
        /* End */

        public void Save();
    }
}
