using System;
using NotTwitter.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Models.Enum;
using NotTwitter.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NotTwitter.API.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class FriendRequestController : ControllerBase
	{
		private readonly IGenericRepository _repo;

		public FriendRequestController(IGenericRepository repo)
		{
			_repo = repo ?? throw new ArgumentNullException("Cannot be null.", nameof(repo));
		}

		/// <summary>
		/// List All Requests
		/// </summary>
		/// <param name="userId"></param>
		[HttpGet("{userId}")]
		public async Task<IActionResult> Get(int userId) // Functionality should show all friend requests where userId == receiverId
		{
            if (await _repo.GetUserByID(userId) == null)
            {
                return NotFound();
            }

			var requests = await _repo.GetAllFriendRequests(userId);
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
			return Ok(requestList);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendRequest"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> CreateRequest([FromBody, Bind("SenderId, ReceiveId")] FriendRequestModel friendRequest)
        {
            // if the sender or receiver is not a valid user, return NotFound
			if (await _repo.GetUserByID(friendRequest.SenderId) is null || await _repo.GetUserByID(friendRequest.ReceiverId) is null)
			{
				return NotFound();
			}

			var newRequest = new Library.Models.FriendRequest
			{
				SenderId = friendRequest.SenderId,
				ReceiverId = friendRequest.ReceiverId,
				FriendRequestStatus = (int)FriendRequestStatus.Pending
			};
			_repo.CreateFriendRequest(newRequest);
            await _repo.Save();

			return StatusCode(200); // Change later to CreatedAtRoute([GetRequest])
		}
		/// <summary>
		/// Accepted Request
		/// </summary>
		/// <param name="friendRequest"></param>
		[HttpPatch]
		[Route("Accepted")]
		public async Task<IActionResult> AcceptRequest([FromBody, Bind("SenderId, ReceiverId")] FriendRequestModel friendRequest)
		{

            try
            {
                var entityFriendRequest = await _repo.GetFriendRequest(friendRequest.SenderId, friendRequest.ReceiverId);
                if ( entityFriendRequest == null)
                {
                    return NotFound();
                }

				var sender = await _repo.GetUserByID(friendRequest.SenderId);
				var receiver = await _repo.GetUserByID(friendRequest.ReceiverId);
				int status = await _repo.FriendRequestStatus(friendRequest.SenderId,friendRequest.ReceiverId);

				if (status != 0)
                {
                    // Method not allowed on this particular resource due to trying to accept request without Pending status
                    return StatusCode(405);
                }

                // Update the entity's FR status
                entityFriendRequest.FriendRequestStatus = (int)FriendRequestStatus.Accepted;

                // Persist in db
				await _repo.UpdateFriendRequest(entityFriendRequest);

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

				_repo.AddFriendShip(newFriend);
				_repo.AddFriendShip(newFriendToReceiver);
				await _repo.Save();
				
				return NoContent();
			}
			catch
            {
                return BadRequest();
            }

		}
		
	
		/// <summary>
		/// Declines the request and updates the database
		/// </summary>
		/// <param name="friendRequest"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("Declined")]
		public async Task<IActionResult> DeclineRequest([FromBody] FriendRequestModel friendRequest)
		{
            try
            {
				var entityFriendRequest = await _repo.GetFriendRequest(friendRequest.SenderId, friendRequest.ReceiverId);
				if (entityFriendRequest is null)
				{
					return NotFound();
				}
				int status = await _repo.FriendRequestStatus(friendRequest.SenderId, friendRequest.ReceiverId);

				if (status != 0)
				{
                    // Method not allowed on this particular resource due to trying to accept request without Pending status
                    return StatusCode(405);
				}
				entityFriendRequest.FriendRequestStatus = (int)FriendRequestStatus.Declined;
                await _repo.UpdateFriendRequest(entityFriendRequest);
                await _repo.Save();
				return StatusCode(200);
            }
            catch
            {
                return BadRequest();
            }
		}
	}
}
