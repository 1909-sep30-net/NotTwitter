using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.DataAccess.Entities
{
    public class Users
    {

        public int UserID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int Gender { get; set; }
		public DateTime DateCreated { get; set; }
		
		public ICollection <Posts> Posts { get; set; } = new HashSet<Posts>();
        public ICollection <Friendships> IncomingFriends { get; set; } = new HashSet<Friendships>();
        public ICollection<Friendships> OutgoingFriends { get; set; } = new HashSet<Friendships>();

        public ICollection <Comments> Comments { get; set; } = new HashSet<Comments>();

		public ICollection<FriendRequests> FriendRequestsSent { get; set; } = new List<FriendRequests>();

        public ICollection<FriendRequests> FriendRequestsReceived { get; set; } = new List<FriendRequests>();

	}
}
