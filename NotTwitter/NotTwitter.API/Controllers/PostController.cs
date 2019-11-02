using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        

        // GET: api/Post/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Post
        [HttpPost]
        public ActionResult Post([FromBody] Models.PostModel postModel)
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

		public IActionResult Like(int postId)
		{
			if (_repo.GetPosts(postId) is null)
			{
				return NotFound();
			}
			_repo.Likes(postId);

			return RedirectToAction(nameof(Get));
		}

		// PUT: api/Post/5
		[HttpPut("{id}")]
        public IActionResult Put(int PostId, [FromBody] Models.PostModel postModel)
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
