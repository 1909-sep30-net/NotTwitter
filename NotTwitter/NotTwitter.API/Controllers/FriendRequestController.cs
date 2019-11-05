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
		private readonly IFriendRequestRepository _frRepo;
		private readonly IUserRepository _userRepo;

		public FriendRequestController(IFriendRequestRepository repo, IUserRepository user)
		{
			_frRepo = repo;
			_userRepo = user;
		}
		[HttpPost]
		public ActionResult CreateRequest([FromBody] FriendRequestModel friendRequest)
		{
			if (_userRepo.GetUserByID(friendRequest.SenderId) is null || _userRepo.GetUserByID(friendRequest.ReceiverId) is null)
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
			_frRepo.CreateFriendRequest(newRequest);
            _frRepo.Save();
            
			return CreatedAtRoute("Get", new { Id = newRequest.ReceiverId }, friendRequest);
		}

		[HttpPost]
		public ActionResult AcceptRequest([FromBody] Library.Models.FriendRequest friendRequest)
		{
            // TODO: Enclose this in a try/catch block in case this fails

            try
            {
                friendRequest.FriendRequestStatus = (int)FriendRequestStatus.Accepted;
                _frRepo.UpdateFriendRequest(friendRequest);
                _frRepo.Save();

                //TODO: Update the friend list of the users involved
                //sender.Friends.Add(receiver)
                //receiver.Friends.Add(sender)
                //_userRepo.UpdateUser(sender)
                //_userRepo.UpdateUser(receiver)
                //_userRepo.Save()

                return RedirectToAction(nameof(MakeFriend));
            }
            catch
            {
                return NotFound();
            }

		}

		public ActionResult MakeFriend([FromBody] Models.FriendRequestModel friendRequest)
		{

			var newFriend = new Library.Models.Friendship
			{
				User1 = friendRequest.Sender,
				User2 = friendRequest.Receiver,
				TimeRequestConfirmed = DateTime.Now
			};
			_userRepo.MakeFriends(newFriend);
            _userRepo.Save();
			return CreatedAtRoute("Get", new { Id = friendRequest.ReceiverId }, friendRequest);
		}

        /// <summary>
        /// Declines the request and updates the database
        /// </summary>
        /// <param name="friendRequest"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult DeclineRequest([FromBody] Library.Models.FriendRequest friendRequest)
		{
            try
            {
                friendRequest.FriendRequestStatus = (int)FriendRequestStatus.Declined;
                _frRepo.UpdateFriendRequest(friendRequest);
                _frRepo.Save();
                return CreatedAtRoute("Get", new { Id = friendRequest.SenderId }, friendRequest);
            }
            catch
            {
                return NotFound();
            }
		}

	}
}
