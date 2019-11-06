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
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		[HttpPost]
		[Route("Create")]
		public ActionResult CreateRequest([FromBody, Bind("SenderId,ReceiverId,FriendRequestStatus")] FriendRequestModel friendRequest)
		{
			if (_userRepo.GetUserByID(friendRequest.SenderId) is null || _userRepo.GetUserByID(friendRequest.ReceiverId) is null)
			{
				return NotFound();
			}
			var sender = _userRepo.GetUserByID(friendRequest.SenderId);
			var receiver = _userRepo.GetUserByID(friendRequest.ReceiverId);
			var newRequest = new Library.Models.FriendRequest
			{
				SenderId = friendRequest.SenderId,
				ReceiverId = friendRequest.ReceiverId,
				FriendRequestStatus = (int)FriendRequestStatus.Pending
			};
			_frRepo.CreateFriendRequest(newRequest);
            _frRepo.Save();

			return StatusCode(200);
		}

		[HttpPatch]
		[Route("Accepted")]
		public ActionResult AcceptRequest([FromBody, Bind("SenderId, ReceiverId")] Library.Models.FriendRequest friendRequest)
		{
            // TODO: Enclose this in a try/catch block in case this fails

            try
            {
                friendRequest.FriendRequestStatus = (int)FriendRequestStatus.Accepted;
                _frRepo.UpdateFriendRequest(friendRequest);
                _frRepo.Save();

				var sender = _userRepo.GetUserByID(friendRequest.SenderId);
				var receiver = _userRepo.GetUserByID(friendRequest.ReceiverId);

				//TODO: Update the friend list of the users involved
				//sender.Friends.Add(receiver)
				//receiver.Friends.Add(sender)
				//_userRepo.UpdateUser(sender)
				//_userRepo.UpdateUser(receiver)
				//_userRepo.Save()
				
				var newFriend = new Library.Models.Friendship
				{
					User1ID = sender.UserID,
					User2ID = receiver.UserID,
					User1 = sender,
					User2 = receiver,
					TimeRequestConfirmed = DateTime.Now
				};

				var newFriendToReceiver = new Library.Models.Friendship
				{
					User1ID = receiver.UserID,
					User2ID = sender.UserID,
					User1 = receiver,
					User2 = sender,
					TimeRequestConfirmed = DateTime.Now
				};

				_userRepo.AddFriendShip(newFriend);
				_userRepo.AddFriendShip(newFriendToReceiver);
				_userRepo.Save();
				
				return StatusCode(200);
				//return RedirectToAction(nameof(MakeFriend));
			}
			catch
            {
                return NotFound();
            }

		}
		/*
		 [HttpPost]
		 [Route("AcceptedFriend")]
		public string MakeFriend(int id1, int id2)
		{

			var sender = _userRepo.GetUserByID(id1);
			var receiver = _userRepo.GetUserByID(id2);
			
			var newFriend = new Library.Models.Friendship
			{
				User1ID = sender.UserID,
				User2ID = receiver.UserID,
				User1 = sender,
				User2 = receiver,
				TimeRequestConfirmed = DateTime.Now
			};
			
			var newFriendToReceiver = new Library.Models.Friendship
			{
				User1ID = receiver.UserID,
				User2ID = sender.UserID,
				User1 = receiver,
				User2 = sender,
				TimeRequestConfirmed = DateTime.Now
			};
			
			_userRepo.AddFriendShip(newFriend);
			_userRepo.AddFriendShip(newFriendToReceiver);
			_userRepo.Save();
			
			return ("tested");
		}
		*/
	
		/// <summary>
		/// Declines the request and updates the database
		/// </summary>
		/// <param name="friendRequest"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Declined")]

		public ActionResult DeclineRequest([FromBody] Library.Models.FriendRequest friendRequest)
		{
            try
            {
                friendRequest.FriendRequestStatus = (int)FriendRequestStatus.Declined;
                _frRepo.UpdateFriendRequest(friendRequest);
                _frRepo.Save();
				return StatusCode(200);
            }
            catch
            {
                return NotFound();
            }
		}
	
	}
}
