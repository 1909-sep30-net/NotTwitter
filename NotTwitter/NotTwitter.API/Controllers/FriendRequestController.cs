using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Models.Enum;
using NotTwitter.API.Models;

namespace NotTwitter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
		private readonly IFriendRequestRepository _repo;
		private readonly IUserRepository _user;

		public FriendRequestController(IFriendRequestRepository repo, IUserRepository user)
		{
			_repo = repo;
			_user = user;
		}
		[HttpPost]
		public ActionResult CreateRequest([FromBody] Models.FriendRequestModel friendRequest)
		{
			if (_user.GetUserByID(friendRequest.SenderId) is null || _user.GetUserByID(friendRequest.ReceiverId) is null)
			{
				return NotFound();
			}
			var newRequest = new Library.Models.FriendRequest
			{
				SenderId = friendRequest.SenderId,
				ReceiverId = friendRequest.ReceiverId,
				Sender = friendRequest.Sender,
				Receiver = friendRequest.Receiver,
				FriendRequestStatus = (int)FriendRequestStatus.Pending
			};
			_repo.Create(newRequest);
			return CreatedAtRoute("Get", new { Id = newRequest.ReceiverId }, friendRequest);
		}

		[HttpPost]
		public ActionResult AcceptRequest([FromBody] Library.Models.FriendRequest friendRequest)
		{
			_repo.Accept(friendRequest);
			return RedirectToAction(nameof(MakeFriend));
		}

		public ActionResult MakeFriend([FromBody] Models.FriendRequestModel friendRequest)
		{
			var newFriendModel = new FriendshipModel
			{
				User1 = friendRequest.Sender,
				User2 = friendRequest.Receiver,
				TimeRequestConfirmed = DateTime.Now
			};

			var newFriend = new Library.Models.Friendship
			{
				User1 = newFriendModel.User1,
				User2 = newFriendModel.User2,
				TimeRequestConfirmed = newFriendModel.TimeRequestConfirmed
			};
			_user.MakeFriends(newFriend);
			return CreatedAtRoute("Get", new { Id = friendRequest.SenderId }, friendRequest);
		}

		[HttpPost]
		public ActionResult DeclineRequest([FromBody] Library.Models.FriendRequest friendRequest)
		{
			_repo.Decline(friendRequest);
			return CreatedAtRoute("Get", new { Id = friendRequest.SenderId }, friendRequest);
		}
	}
}
