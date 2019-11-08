using System;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotTwitter.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public UserController(IGenericRepository urepo)
        {
            _repo = urepo ?? throw new ArgumentNullException(nameof(urepo));
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
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    Id = user.UserID
                });
            }
            return userList;
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

            foreach (Library.Models.User friend in x.Friends)
            {
                var friendPost = await _repo.GetPostsByUser(friend.UserID);

                foreach (Library.Models.Post fPost in friendPost)
                {
                    var commentList = new List<CommentModel>();

                    foreach (var comment in fPost.Comments)
                    {
                        commentList.Add(new CommentModel
                        {
                            CommentId = comment.CommentId,
                            UserId = comment.Author.UserID,
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
        public async Task<IActionResult> Post([FromBody, Bind("FirstName, LastName, Username, Password, Email, Gender")] UserViewModel newUser)
        {
            try
            {
                Library.Models.User mappedUser = new Library.Models.User()
                {
                    Username = newUser.Username,
                    Password = newUser.Password,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Gender = newUser.Gender,
                    Email = newUser.Email,
                };

                _repo.AddUser(mappedUser);
                await _repo.Save();

                return CreatedAtRoute("GetUserByID", new { id = mappedUser.UserID }, newUser);
            }
            catch
            {
                return BadRequest();
            }


        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserViewModel user)
        {
            var oldUser = await _repo.GetUserByID(id);
            if (oldUser != null)
            {
                if(oldUser.UserID != user.Id || oldUser.Username != user.Username)
                {
                    return Forbid();
                }
                Library.Models.User updatedUser = new Library.Models.User()
                {
                    UserID = user.Id, // this is redundant, should be removed
                    Username = user.Username, // technically also redundant since this should also not be changed according to the logic above
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender
                };
                await _repo.UpdateUser(updatedUser);
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
