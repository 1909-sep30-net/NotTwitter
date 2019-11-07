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
	public class PostController : ControllerBase
	{
        private readonly IGenericRepository _repo;

		public PostController(IGenericRepository repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Returns a post with comments
        /// </summary>
        /// <param name="postId">ID of specified post</param>
        /// <returns></returns>
        [HttpGet("{postId}", Name = "GetPostById")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            // Get post by Id; If post isn't found, return 404
            var post = await _repo.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }

            // Create a list of comment models from the queried Post, add to post model
            var postComments = new List<CommentModel>();
            foreach(var com in post.Comments)
            {
                postComments.Add(
                    new CommentModel
                    {
                        CommentId = com.CommentId,
                        Content = com.Content,
                        TimeSent = com.TimeSent,
                        UserId = com.Author.UserID
                    }
                );
            }

            // Return post model
            return Ok( new PostModel
            {
                PostID = post.PostID,
                UserID = post.User.UserID,
                TimeSent = post.TimeSent,
                Text = post.Content,
                Comments = postComments
            });
        }

        /// <summary>
        /// Returns a list of posts from the user, including comments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/Post/5
        [HttpGet("user/{userId}", Name = "GetPostsByUser")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            // If user doesnt exist, return 404
            if (_repo.GetUserByID(userId) == null)
            {
                return NotFound();
            }

            // Populate representation models for posts by user
            var posts = await _repo.GetPostsByUser(userId);
            List<PostModel> ListPosts = new List<PostModel>();
            foreach (var p in posts)
            {
                var post = new PostModel
                {
                    PostID = p.PostID,
                    UserID = userId,
                    Text = p.Content,
                    TimeSent = p.TimeSent
                };
                ListPosts.Add(post);
            }

            return Ok(ListPosts);
        }

        // POST: api/CreatePost
        [HttpPost]
        public async Task<IActionResult> CreatePost(int authorId, string content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var postAuthor = await _repo.GetUserByID(authorId);
			var newPost = new Library.Models.Post
			{
				Content = content,
				TimeSent = DateTime.Now,
				User = postAuthor
			};
            await _repo.CreatePost(newPost, postAuthor);

            await _repo.Save();


			return CreatedAtRoute("GetPostByID", new { postId = newPost.PostID }, newPost);
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
		[HttpPut("{PostId}")]
        public async Task<IActionResult> UpdatePost(int PostId, [FromBody] Models.PostModel postModel)
        {
            var currentPost = await _repo.GetPostById(PostId);
            if (currentPost is null)
            {
                return NotFound();
            }

            currentPost.Content = postModel.Text;

			//_repo.UpdatePost(currentPost);
            await _repo.Save();

			return NoContent();
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{postId}")]
		public async Task<IActionResult> Delete(int postId)
		{
			if (await _repo.GetPostById(postId) is null)
			{
				return NotFound();
			}

			await _repo.DeletePost(postId);
			await _repo.Save();

			return NoContent();
		}

	}
}