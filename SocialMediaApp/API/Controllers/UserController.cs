using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using DataAccess.Repositories;
using Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        // Get UserPosts
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Get User by Name
        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
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
        public ActionResult Post([FromBody, Bind("FirstName, LastName, Username, Email, Gender")] UserViewModel newUser)
        {
            Library.Models.User mappedUser = new Library.Models.User()
            {
                Username = newUser.Username,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Gender = newUser.Gender,
                Email = newUser.Email,
                UserID = newUser.Id

            };
            _userRepo.AddUser(mappedUser);
            return CreatedAtRoute("Get", new { Id = mappedUser.UserID}, newUser);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
