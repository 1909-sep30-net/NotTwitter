using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotTwitter.API.Models;
using NotTwitter.DataAccess.Repositories;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /*
       * Get FriendList
    */

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository urepo)
        {
            _userRepo = urepo ?? throw new ArgumentNullException(nameof(urepo));
        }

        // Get User by Name
        // GET: api/User/5
        [HttpGet("{id}")]
        public UserViewModel Get(int id)
        {

            var x = _userRepo.GetUserByID(id);

            return new UserViewModel()
            {
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Email = x.Email,
                Id = x.UserID
            };

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
                UserID = newUser.Id
            };

            _userRepo.AddUser(mappedUser);
            _userRepo.Save();

            //Return a BadRequest message if User already exists
            //if (_userRepo.GetUserByID(mappedUser.UserID) == null)
            //{
            //    return BadRequest();
            //}
            
            return CreatedAtRoute("Get", new { Id = mappedUser.UserID }, newUser);
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
                    UserID = user.Id,
                    Username = user.Username,
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
