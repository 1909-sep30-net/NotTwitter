using System;
using System.Collections.Generic;
using System.Text;
using Library.Models;


namespace Library.Interfaces
{
	public interface ICommentRepository : IDisposable
	{
		public void CreateComment(Comment newComment);
        public IEnumerable<Models.Comment> GetCommentsByPostId(int postId);
        public void UpdateComment(Comment newComment);
        public void DeleteCommentsByPostId(int postId);

	}
}
