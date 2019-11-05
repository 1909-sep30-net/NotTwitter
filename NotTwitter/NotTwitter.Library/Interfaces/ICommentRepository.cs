using System;
using System.Collections.Generic;
using NotTwitter.Library.Models;

namespace NotTwitter.Library.Interfaces
{
	public interface ICommentRepository : IDisposable
	{
		public void CreateComment(Comment newComment);
        public IEnumerable<Comment> GetCommentsByPostId(int postId);
        public IEnumerable<Comment> GetCommentsByUserId(int userId);
        public void UpdateComment(Comment newComment);
        public void DeleteCommentsByPostId(int postId);
        /// <summary>
        /// Persist changes to database
        /// </summary>
        public void Save();

	}
}
