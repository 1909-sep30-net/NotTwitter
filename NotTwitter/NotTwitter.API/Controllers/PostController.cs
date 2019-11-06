using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
		private readonly IPostRepository _repo;

		public PostController(IPostRepository repo)
		{
			_repo =repo ?? throw new ArgumentNullException(nameof(repo));
		}
        

        /// <summary>
        /// Returns a list of posts from the user, including comments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/Post/5
        [HttpGet("{userId}", Name = "GetPostsByUser")]
        public List<PostModel> GetPostsByUser(int userId)
        {
            var posts = _repo.GetPostsByUser(userId);
            List<PostModel> ListPosts = new List<PostModel>();
            foreach (var p in posts)
            {
                var post = new PostModel
                {
                    User = p.User,
                    Text = p.Content,
                };
                ListPosts.Add(post);
            }
            return ListPosts;
        }

        // POST: api/CreatePost
        [HttpPost]
        public ActionResult CreatePost([FromBody] Models.PostModel postModel)
        {
			var newPost = new Library.Models.Post
			{
				User = postModel.User,
				Content = postModel.Text,
				//Likes = 0,
				TimeSent = DateTime.Now,
				
			};
			_repo.CreatePost(newPost);
			return CreatedAtRoute("Get", postModel, new { Id = postModel.User.UserID}); // TODO: Theres no method corresponding to this
        }

        // TODO: clarify; what is this method trying to do? Gets a post, increments the Likes property, gets a post from db with likes?
		//public IActionResult Like(Post post) //TODO what is this parameter post; does it need to be model binded?
		//{
        //  var liked = _repo.GetPostById(post.PostID);
		//	if (liked is null)
		//	{
		//		return NotFound();
		//	}
		//	liked.Likes++;
		//	_repo.GetPostWithLikes(liked);

		//	return RedirectToAction(nameof(GetAllPosts));
		//}


		// PUT: api/Post/5
		[HttpPut("{id}")]
        public IActionResult UpdatePost(int PostId, [FromBody] Models.PostModel postModel)
        {
            if (_repo.GetPostById(PostId) is null)
            {
                return NotFound();
            }

			var updatedPost = new Library.Models.Post
			{
				User = postModel.User,
				Content = postModel.Text,
				TimeSent = DateTime.Now,
			};
			_repo.UpdatePost(updatedPost);
            _repo.Save();

			return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int postId)
        {
			if (_repo.GetPostById(postId) is null)
            {
                return NotFound();
            }

            _repo.DeletePost(postId);
            _repo.Save();

			return NoContent();
        }
        
    }
}
