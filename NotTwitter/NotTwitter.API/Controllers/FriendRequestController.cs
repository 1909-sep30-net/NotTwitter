using System;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Models.Enum;
using NotTwitter.API.Models;
using System.Collections.Generic;

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

		/// <summary>
		/// List All Requests
		/// </summary>
		/// <param name="userId"></param>
		[HttpGet("{userId}")]
		public List<FriendRequestModel> Get(int userId)
		{
			var requests = _frRepo.GetAllFriendRequests(userId);
			List<FriendRequestModel> requestList = new List<FriendRequestModel>();
			foreach (var req in requests)
			{
				var r = new FriendRequestModel
				{
					SenderId = req.SenderId,
					ReceiverId = req.ReceiverId,
				};
				requestList.Add(r);
				
			}
			return requestList;
		}
		/// <summary>
		/// Create Friend Request
		/// </summary>
		/// <param name="senderId", name="receiverId"></param>
        /// 
		[HttpPost]
		[Route("Create")]
		public ActionResult CreateRequest([FromBody] int senderId, int receiverId) 
		{
			if (_userRepo.GetUserByID(senderId) is null || _userRepo.GetUserByID(senderId) is null)
			{
				return NotFound();
			}
			var sender = _userRepo.GetUserByID(senderId);
			var receiver = _userRepo.GetUserByID(receiverId);
			var newRequest = new Library.Models.FriendRequest
			{
				SenderId = senderId,
				ReceiverId = receiverId,
				FriendRequestStatus = (int)FriendRequestStatus.Pending
			};
			_frRepo.CreateFriendRequest(newRequest);
            _frRepo.Save();

			return StatusCode(200);
		}
		/// <summary>
		/// Accepted Request
		/// </summary>
		/// <param name="friendRequest"></param>
		[HttpPatch]
		[Route("Accepted")]
		public ActionResult AcceptRequest([FromBody, Bind("SenderId, ReceiverId")] FriendRequestModel friendRequest)
		{
            // TODO: Enclose this in a try/catch block in case this fails

            try
            {
                var entityFriendRequest = _frRepo.GetFriendRequest(friendRequest.SenderId, friendRequest.ReceiverId);
                if ( entityFriendRequest == null)
                {
                    return NotFound();
                }

				var sender = _userRepo.GetUserByID(friendRequest.SenderId);
				var receiver = _userRepo.GetUserByID(friendRequest.ReceiverId);
				int status = _frRepo.FriendRequestStatus(friendRequest.SenderId,friendRequest.ReceiverId);

				if (status != 0)
                {
                    return StatusCode(400);
                }

                // Update the entity's FR status
                entityFriendRequest.FriendRequestStatus = (int)FriendRequestStatus.Accepted;

                // Persist in db
				_frRepo.UpdateFriendRequest(entityFriendRequest);
				_frRepo.Save();

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
			}
			catch
            {
                return NotFound();
            }

		}
		
	
		/// <summary>
		/// Declines the request and updates the database
		/// </summary>
		/// <param name="friendRequest"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("Declined")]

		public ActionResult DeclineRequest([FromBody] FriendRequestModel friendRequest)
		{
            try
            {
				var entityFriendRequest = _frRepo.GetFriendRequest(friendRequest.SenderId, friendRequest.ReceiverId);
				if (entityFriendRequest is null)
				{
					return NotFound();
				}
				int status = _frRepo.FriendRequestStatus(friendRequest.SenderId, friendRequest.ReceiverId);

				if (status != 0)
				{
					return StatusCode(400);
				}
				entityFriendRequest.FriendRequestStatus = (int)FriendRequestStatus.Declined;
                _frRepo.UpdateFriendRequest(entityFriendRequest);
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
