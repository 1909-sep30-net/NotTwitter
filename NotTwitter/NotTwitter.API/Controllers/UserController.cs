using System;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotTwitter.Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace NotTwitter.API.Controllers
{

    [Route("api/[controller]")]
	//[Authorize]
	[ApiController]

    public class UserController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public UserController(IGenericRepository urepo)
        {
            _repo = urepo ?? throw new ArgumentNullException("Cannot be null.", nameof(urepo));
        }

        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();
            var userList = new List<UserViewModel>();
            foreach (Library.Models.User user in users)
            {
                userList.Add(new UserViewModel()
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    Id = user.UserID
                });
            }
            return userList;
        }

        [HttpGet("name/{name}", Name = "GetUserByName")]
        public async Task<IEnumerable<UserViewModel>> GetName(string name)
        {
            var x = await _repo.GetUsersByName(name);
            var userList = new List<UserViewModel>();
            foreach (Library.Models.User user in x)
            {
                userList.Add(new UserViewModel()
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    Id = user.UserID
                });
            }
            return userList;
        }

		[HttpGet("email/{email}", Name ="GetUserByEmail")]
		public async Task<ActionResult> GetUserByEmail(string email)
		{
			var x = await _repo.GetUserByEmail(email);
			if (x == null)
			{
				return NotFound();
			}
			var modelFriends = new List<FriendViewModel>();

			// Populate friend view model using x's populated friend list
			// business model -> representational model
			foreach (var friend in x.Friends)
			{
				var f = new FriendViewModel
				{
					UserId = friend.UserID,
					FirstName = friend.FirstName,
					LastName = friend.LastName
				};
				modelFriends.Add(f);
			}

			// Create and return representational model of user
			return Ok(new UserViewModel()
			{
				Username = x.Username,
				FirstName = x.FirstName,
				LastName = x.LastName,
				Gender = x.Gender,
				Email = x.Email,
				Id = x.UserID,
				Friends = modelFriends
			});
		}

		// Get User by Name
		// GET: api/User/5
		[HttpGet("{id}", Name = "GetUserByID")]
        public async Task<IActionResult> Get(int id)
        {

            var x = await _repo.GetUserWithFriends(id);
            if (x == null)
            {
                return NotFound();
            }
            var modelFriends = new List<FriendViewModel>();

            // Populate friend view model using x's populated friend list
            // business model -> representational model
            foreach (var friend in x.Friends)
            {
                var f = new FriendViewModel
                {
                    UserId = friend.UserID,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName
                };
                modelFriends.Add(f);
            }

            // Create and return representational model of user
            return Ok(new UserViewModel()
            {
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Email = x.Email,
                Id = x.UserID,
                Friends = modelFriends
            });

        }

        [HttpGet("friendposts/{id}", Name = "GetFriendPosts")]
        public async Task<IActionResult> GetFriendPost(int id)
        {
            //Check if user 1 is friend with user 2
            var x = await _repo.GetUserWithFriends(id);

            var friendPostList = new List<PostModel>();

            // For each friend of the user
            foreach (Library.Models.User friend in x.Friends)
            {
                var friendPost = await _repo.GetPostsByUser(friend.UserID);

                // For each post of the friend
                foreach (Library.Models.Post fPost in friendPost)
                {
                    var commentList = new List<CommentModel>();

                    // For each comment of the post
                    foreach (var comment in fPost.Comments)
                    {
                        commentList.Add(new CommentModel
                        {
                            CommentId = comment.CommentId,
                            AuthorId = comment.Author.UserID,
                            Content = comment.Content,
                            TimeSent = comment.TimeSent
                        });
                    }

                    var postModel = new PostModel()
                    {
                        PostID = fPost.PostID,
                        Text = fPost.Content,
                        TimeSent = fPost.TimeSent,
                        UserID = fPost.User.UserID,
                        Comments = commentList
                    };

                    friendPostList.Add(postModel);
                }
            }
            return Ok(friendPostList);
        }

        // Post User Model
        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserPostModel newUser)
        {
            try
            {
                Library.Models.User mappedUser = new Library.Models.User()
                {
                    Username = newUser.Username,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Gender = newUser.Gender,
                    Email = newUser.Email,
                    DateCreated = DateTime.Now
                };

                _repo.AddUser(mappedUser);
                await _repo.Save();

                var newAddedUser = await _repo.GetUserByEmail(mappedUser.Email);

                return CreatedAtRoute("GetUserByID", new { id = newAddedUser.UserID }, newAddedUser);
            }
            catch
            {
                return BadRequest();
            }


        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserPostModel user)
        {
            var oldUser = await _repo.GetUserByID(id);
            if (oldUser != null)
            {
                oldUser.Username = user.Username;
                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                oldUser.Email = user.Email;
                oldUser.Gender = user.Gender;

                await _repo.UpdateUser(oldUser);
                await _repo.Save();
                return NoContent();
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oldUser = await _repo.GetUserByID(id);
            if (oldUser != null)
            {
                await _repo.DeleteUserByID(id);
                await _repo.Save();
                return NoContent();
            }
            return NotFound();
        }
    }
}
