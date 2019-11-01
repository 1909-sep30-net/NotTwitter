using Library.Models;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotTwitter.Library.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        //CRUD
        public void CreatePost(Post post);

        public Post GetPost(int postId);

        /// <summary>
        /// Gets all posts from a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Post> GetPosts(int userId);
        public void UpdatePost(Post post);
        public void DeletePost(int postId);
    }
}
