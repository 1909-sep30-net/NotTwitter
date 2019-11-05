using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotTwitter.DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly NotTwitterDbContext _context;

        /// <summary>
        /// Constructs repository with DbContext injected
        /// </summary>
        /// <param name="db">The DbContext</param>
        public PostRepository(NotTwitterDbContext db)
        {
            _context = db ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Stores new post in database
        /// </summary>
        /// <param name="post">Post to be stored</param>
        public void CreatePost(Post post)
        {
            _context.Add(Mapper.MapPosts(post));
        }

        /// <summary>
        /// Deletes post and its comments from database
        /// </summary>
        /// <param name="postId">Id of the post to be removed</param>
        public void DeletePost(int postId)
        {
            // Find post and eager load its comments
            var post = _context.Posts
                .Where(p => p.PostId == postId)
                .Include(p => p.Comments)
                .Single();

            // Delete all comments from post
            foreach (var comment in post.Comments)
            {
                _context.Remove(comment);
            }

            // Then delete post
            _context.Remove(post);
        }

        /// <summary>
        /// Gets post by ID, including comments
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public Post GetPostById(int postId)
        {
            // Then get the post with comments
            var postWithComments = _context.Posts.Include(p => p.Comments).First(p => p.PostId == postId);

            return Mapper.MapPosts(postWithComments);
        }

        /// <summary>
        /// Gets all posts in database
        /// </summary>
        /// <returns>All posts in data base</returns>
        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts.Select(Mapper.MapPosts);
        }

        /// <summary>
        /// Gets posts from user, including comments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Post> GetPostsByUser(int userId)
        {
            return _context.Posts
                .Include(p=>p.Comments)
                .Where(p => p.UserId == userId)
                .Select(Mapper.MapPosts);
        }

        /// <summary>
        /// Updates database with given post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        public void UpdatePost(Post post)
        {
            var newEntity = Mapper.MapPosts(post);
            var oldEntity = _context.Posts.Find(post.PostID);
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Returns a post including its likes, excluding its comments?
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
		public Post GetPostWithLikes(Post post)
		{
			var oldPost = _context.Posts.Find(post.PostID);
			var PostWithLikes = _context.Posts.Include(p =>p.Likes).First(p => p.PostId == post.PostID);
			return Mapper.MapPosts(PostWithLikes);
		}

        public void Save()
        {
            // todo: log
            _context.SaveChanges();
        }
		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PostRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

		


		#endregion
	}
}
