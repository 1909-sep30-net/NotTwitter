using DataAccess.Entities;
using Library.Interfaces;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataAccess.Repositories
{
	public class CommentRepository : ICommentRepository, IDisposable
	{
		private readonly NotTwitterDbContext _context;

		public CommentRepository(NotTwitterDbContext db)
		{
            _context = db ?? throw new NullReferenceException();
		}

		public void CreateComment(Comment newComment)
		{

            var newEntity = Mapper.MapComments(newComment);
			_context.Add(newEntity);
			_context.SaveChanges();
		}

        public void UpdateComment(Comment newComment)
        {
            var newEntity = Mapper.MapComments(newComment);
            var oldEntity = _context.Comments.Find(newComment.CommentId);
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes all comments from a post
        /// </summary>
        /// <remarks>Used in conjunction with post deletion</remarks>
        /// <param name="postId"></param>
		public void DeleteCommentsByPostId(int postId)
		{
			var comments = _context.Comments.Where(p => p.PostId == postId);
			foreach (var comment in comments)
			{
				_context.Remove(comment);
			}

		}

        /// <summary>
        /// Gets all comments from a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
		public IEnumerable<Comment> GetCommentsByPostId(int postId)
		{
			return _context.Comments.Where(p => p.PostId == postId).OrderByDescending(d => d.TimeSent).Select(Mapper.MapComments);
		}

        public void Save()
        {
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
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CommentRepository()
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
