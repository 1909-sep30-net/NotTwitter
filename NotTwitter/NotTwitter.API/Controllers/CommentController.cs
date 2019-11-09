using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
	[Authorize]
	[ApiController]
    public class CommentController : ControllerBase
    {
		private readonly IGenericRepository _repo;

		public CommentController(IGenericRepository repo)
		{
			_repo = repo ?? throw new ArgumentNullException("Repository cannot be null", nameof(repo));
		}

        // GET: api/Comment/5
        [HttpGet("{commentId}", Name = "GetCommentByID")]
        public async Task<IActionResult> GetCommentByID(int commentId)
        {
            var comment = await _repo.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpGet("user/{userid}", Name = "GetCommentByUser")]
        public async Task<IActionResult> GetCommentByUser(int userid)
        {
            var comments = await _repo.GetCommentsByUserId(userid);
            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentModel commentModel)
        {
            var post = await _repo.GetPostById(commentModel.PostId);
            var author = await _repo.GetUserByID(commentModel.UserId);
            if (post is null)
            {
				return BadRequest();
            }
            if (author is null)
            {
                return BadRequest();
            }

			if (commentModel.Content is null || commentModel.Content == "") // Not sure if you need this if we have data annotations checking for the same thing
			{
				ModelState.AddModelError(string.Empty, "You cannot submit an empty comment!");
				return BadRequest(); // Is this the correct status code for incorrect input? I think it should just be 400 if it's Post
			}

			var comment = new Library.Models.Comment
			{
				Content = commentModel.Content,
				TimeSent = DateTime.Now
			};

			await _repo.CreateComment(comment, author, post);
            await _repo.Save();

			return CreatedAtRoute("GetCommentByID", new { commentId = comment.CommentId }, comment);
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
		[HttpDelete("{postId}")]
        public ActionResult Delete(int postId, PostModel postModel)
        {
			if (_repo.GetCommentsByPostId(postId) is null)
            {
				return NotFound();
            }

			_repo.DeleteCommentsByPostId(postId);
            _repo.Save();

			return NoContent();
		}
        
    }

}
