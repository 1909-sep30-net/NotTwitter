using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
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

        // GET: api/Comment/user/4
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
        [HttpPost("Post")]
        public async Task<IActionResult> CreateComment([FromBody] CommentPostModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var post = await _repo.GetPostById(commentModel.PostId);
            var author = await _repo.GetUserByID(commentModel.AuthorId);

            if (post is null || author is null)
            {
				return NotFound();
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
        [HttpPut("{commentId}")]
        public async Task<IActionResult> Put(int commentId, string content)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
            var updateComment = await _repo.GetCommentById(commentId);
            updateComment.Content = content;
			await _repo.UpdateComment(updateComment);
            await _repo.Save();
			return NoContent();
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{postId}")]
        public ActionResult Delete(int postId)
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
