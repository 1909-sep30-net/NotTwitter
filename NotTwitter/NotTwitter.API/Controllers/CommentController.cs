using System;
using Microsoft.AspNetCore.Mvc;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
		private readonly ICommentRepository _repo;
		private readonly IPostRepository _post;

		public CommentController(ICommentRepository repo, IPostRepository post)
		{
			_repo = repo;
			_post = post;
		}
        

        // GET: api/Comment
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Comment/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        

        // POST: api/Comment
        [HttpPost]
        public ActionResult CreateComment([FromBody] CommentModel commentModel)
        {
			if (_post.GetPostById(commentModel.PostId) is null)
            {
				return NotFound();
            }
			if (commentModel.Content is null || commentModel.Content == "") // Not sure if you need this if we have data annotations checking for the same thing
			{
				ModelState.AddModelError(string.Empty, "You cannot submit an empty comment!");
				return NotFound(); // Is this the correct status code for incorrect input? I think it should just be 400 if it's Post
			}
			var comment = new Library.Models.Comment
			{
				Content = commentModel.Content,
				TimeSent = DateTime.Now
			};
			_repo.CreateComment(comment);
            _repo.Save();

			return CreatedAtRoute("Get", commentModel, new { Id =  commentModel.PostId});
		}

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CommentModel commentModel)
        {
			if (commentModel.Content is null || commentModel.Content == "")
			{
				ModelState.AddModelError(string.Empty, "You cannot submit an empty comment!");
				return NotFound();
			}
			var updateComment = new Library.Models.Comment
			{
				Content = commentModel.Content,
				TimeSent = DateTime.Now,
			};
			_repo.UpdateComment(updateComment);
            _repo.Save();
			return NoContent();
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
        public ActionResult Delete(int postId, PostModel postModel)
        {
			if (_repo.GetCommentsByPostId(postId) is null)
            {
				return NotFound();
            }
			_repo.DeleteCommentsByPostId(postId);
            _repo.Save();
			return CreatedAtRoute("Get", postModel, new { Id = postId });
		}
        
    }

}
