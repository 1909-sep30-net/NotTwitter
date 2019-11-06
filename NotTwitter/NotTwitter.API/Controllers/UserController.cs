using System;
using NotTwitter.API.Models;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NotTwitter.API.Controllers
{
    /*
     * Get FriendList
     */

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo; 
        private readonly IPostRepository _postRepo;

        public UserController(IUserRepository urepo, IPostRepository prepo)
        {
            _userRepo = urepo ?? throw new ArgumentNullException(nameof(urepo));
            _postRepo = prepo ?? throw new ArgumentNullException(nameof(prepo));
        }

        [HttpGet("post/{id}", Name = "GetFriendPosts")]
        public IActionResult GetFriendPost(int id)
        {
            //Check if user 1 is friend with user 2
            var x = _userRepo.GetUserWithFriends(id);

            var friendPostList = new List<PostModel>();

            //if(x == null)
            //{
            //    return NotFound();
            //}

            foreach(Library.Models.User friend in x.Friends)
            {
                var friendPost = _postRepo.GetPostsByUser(friend.UserID);

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

        //Get User by Name
        [HttpGet("name/{name}", Name = "GetUserByName")]
        public IActionResult GetName(string name)
        {
            var x = _userRepo.GetUsersByName(name);
            var userList = new List<UserViewModel>();
            if (x == null)
            {
                return NotFound();
            }
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
            return Ok(userList);
        }


    // Get User by Name
    // GET: api/User/5
    [HttpGet("{id}", Name = "GetUserByID")]
        public IActionResult Get(int id)
        {

            var x = _userRepo.GetUserWithFriends(id);
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

        // Post User Model
        // POST: api/User
        [HttpPost]
        public ActionResult Post([FromBody, Bind("FirstName, LastName, Username, Password, Email, Gender")] UserViewModel newUser)
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

            _userRepo.AddUser(mappedUser);
            _userRepo.Save();


            //Return a BadRequest message if User already exists
            //if (_userRepo.GetUserByID(mappedUser.UserID) == null)
            //{
            //    return BadRequest();
            //}
            return CreatedAtRoute("GetUserByID", new { id = mappedUser.UserID }, newUser);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserViewModel user)
        {
            var oldUser = _userRepo.GetUserByID(id);
            if (oldUser != null)
            {
                if(oldUser.UserID != user.Id || oldUser.Username != user.Username)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
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
                _userRepo.UpdateUser(updatedUser);
                _userRepo.Save();
                return NoContent();
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var oldUser = _userRepo.GetUserByID(id);
            if (oldUser != null)
            {
                _userRepo.DeleteUserByID(id);
                _userRepo.Save();
                return NoContent();
            }
            return NotFound();
        }
    }
}
