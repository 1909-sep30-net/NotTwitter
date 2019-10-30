using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        //CRUD
        public void CreatePost(Post post);

        /// <summary>
        /// Gets all posts from a specific 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Post> GetPosts(int id);
        public void UpdatePost();
        public void DeletePost();
    }
}
