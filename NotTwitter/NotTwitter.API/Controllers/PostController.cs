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
        private readonly IUserRepository _urepo;

		public PostController(IPostRepository repo, IUserRepository urepo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _urepo = urepo ?? throw new ArgumentNullException(nameof(urepo));
        }

        /// <summary>
        /// Returns a post with comments
        /// </summary>
        /// <param name="postId">ID of specified post</param>
        /// <returns></returns>
        [HttpGet("{postId}", Name = "GetPostById")]
        public IActionResult GetPostById(int postId)
        {
            // Get post by Id; If post isn't found, return 404
            var post = _repo.GetPostById(postId);
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
        public List<PostModel> GetPostsByUser(int userId)
        {
            var posts = _repo.GetPostsByUser(userId);
            if (posts == null)
            {
                //return NotFound();
            }
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
            return ListPosts;
        }

        // POST: api/CreatePost
        [HttpPost]
        public ActionResult CreatePost(int authorId, string content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var postAuthor = _urepo.GetUserByID(authorId);
			var newPost = new Library.Models.Post
			{
				Content = content,
				TimeSent = DateTime.Now,
                User = postAuthor
			};
            _repo.CreatePost(newPost, postAuthor);
            _repo.Save();

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
		[HttpPut("{id}")]
        public IActionResult UpdatePost(int PostId, [FromBody] Models.PostModel postModel)
        {
            if (_repo.GetPostById(PostId) is null)
            {
                return NotFound();
            }

			var updatedPost = new Library.Models.Post
			{
				User = _urepo.GetUserByID(postModel.UserID),
				Content = postModel.Text,
				TimeSent = DateTime.Now,
			};
			_repo.UpdatePost(updatedPost);
            _repo.Save();

			return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{postId}")]
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
