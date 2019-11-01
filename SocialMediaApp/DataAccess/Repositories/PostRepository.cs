using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly NotTwitterDbContext _context;

        public PostRepository(NotTwitterDbContext db)
        {
            _context = db ?? throw new NullReferenceException();
        }
        public void CreatePost(Post post)
        {
            _context.Add(Mapper.MapPosts(post));
        }

        public void DeletePost(int postId)
        {
            // Throw exception if post was not found
            var post = _context.Posts.Find(postId) ?? throw new ArgumentException("Post does not exist.");

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
        public Post GetPost(int postId)
        {
            // First check if post exists
            var post = _context.Posts.Find(postId) ?? throw new ArgumentException("Post does not exist.");

            // Then get the post with comments
            var postWithComments = _context.Posts.Include(p => p.Comments).First(p => p.PostId == postId);

            return Mapper.MapPosts(postWithComments);
        }

        /// <summary>
        /// Gets posts from user, including comments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Post> GetPosts(int userId)
        {
            return _context.Posts
                .Include(p=>p.Comments)
                .Where(p => p.UserId == userId)
                .Select(Mapper.MapPosts);
        }


        public void UpdatePost(Post post)
        {
            var newEntity = Mapper.MapPosts(post);
            var oldEntity = _context.Posts.Find(post.PostID) ?? throw new ArgumentException("Post does not exist.");
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
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
