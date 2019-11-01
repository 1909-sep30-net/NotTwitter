using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
		private readonly IFriendRequestRepository _repo;

		public FriendRequestController(IFriendRequestRepository repo)
		{
			_repo = repo;
		}
        /*
		[HttpPost]
		public ActionResult Create([FromBody] Library.Models.FriendRequest request)
		{
			
		}
        */
    }
}
