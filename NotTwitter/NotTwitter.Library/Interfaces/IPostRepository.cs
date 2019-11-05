using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;

namespace NotTwitter.Library.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        /// <summary>
        /// Stores new post in database
        /// </summary>
        /// <param name="post">Post to be stored</param>
        public void CreatePost(Post post);

        /// <summary>
        /// Gets post by ID, including comments
        /// </summary>
        /// <param name="postId">ID of the post</param>
        /// <returns>A post including the comments</returns>
        public Post GetPostById(int postId);

        /// <summary>
        /// Gets all posts from a specific user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>All posts with comments from a user</returns>
        public IEnumerable<Post> GetPostsByUser(int userId);
        /// <summary>
        /// Gets all posts in database
        /// </summary>
        /// <returns>All posts in data base</returns>
        public IEnumerable<Post> GetAllPosts();
        /// <summary>
        /// Updates database with given post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        public void UpdatePost(Post post);
        /// <summary>
        /// Deletes post from database
        /// </summary>
        /// <param name="postId">Id of the post to be removed</param>
        public void DeletePost(int postId);
    }
}
