using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;

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
        

        // GET: api/Post/5
        [HttpGet("{id}", Name = "GetPosts")]
        public List<PostModel> GetAllPosts(int postId)
        {
			var posts = _repo.GetPosts(postId);
			List<Models.PostModel> ListPosts = new List<PostModel>(); 
			foreach (var p in posts)
			{
				var post = new Models.PostModel
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
				Likes = 0,
				TimeSent = DateTime.Now,
				
			};
			_repo.CreatePost(newPost);
			return CreatedAtRoute("Get", postModel, new { Id = postModel.User.UserID});
        }

		public IActionResult Like(Post post)
		{
			if (_repo.GetPosts(post.PostID) is null)
			{
				return NotFound();
			}
			var liked = _repo.GetPost(post.PostID);
			liked.Likes++;
			_repo.Likes(liked);

			return RedirectToAction(nameof(GetAllPosts));
		}

		// PUT: api/Post/5
		[HttpPut("{id}")]
        public IActionResult UpdatePost(int PostId, [FromBody] Models.PostModel postModel)
        {
			if (_repo.GetPosts(PostId) is null)
				return NotFound();
			var updatedPost = new Library.Models.Post
			{
				User = postModel.User,
				Content = postModel.Text,
				TimeSent = DateTime.Now,
			};
			_repo.UpdatePost(updatedPost);
			return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int postId)
        {
			if (_repo.GetPosts(postId) is null)
				return NotFound();

			_repo.DeletePost(postId);
			return NoContent();
        }
    }
}
