using System;
using System.Linq;

namespace NotTwitter.DataAccess
{
    public class Mapper
    {
        public static Library.Models.User MapUsers(Entities.Users users)
        {
            return new Library.Models.User
            {
                UserID = users.UserID,
                Username = users.Username,
                Password = users.Password,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Gender = users.Gender
            };
        }

        public static Entities.Users MapUsers(Library.Models.User users)
        {
            return new Entities.Users
            {
                UserID = users.UserID,
                Username = users.Username,
                Password = users.Password,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Gender = users.Gender
            };
        }

        public static Library.Models.User MapUsersWithPostsAndComments(Entities.Users users)
        {
            var mapuser = MapUsers(users);
            mapuser.Posts = users.Posts.Select(MapPostsWithComments).ToHashSet();
            return mapuser;
        }

        /// <summary>
        /// Map posts without comments
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static Library.Models.Post MapPosts(Entities.Posts posts)
        {
            return new Library.Models.Post
            {
                PostID = posts.PostId,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
                //User = MapUsers(posts.User)
            };
        }

        /// <summary>
        /// Map post without comments without user
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static Entities.Posts MapPost(Library.Models.Post posts)
        {
            return new Entities.Posts
            {
                PostId = posts.PostID,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
                UserId = posts.User.UserID,
                User = MapUsers(posts.User)
            };
        }

        /// <summary>
        /// Map post without comments with user
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static Entities.Posts MapPostWithUser(Library.Models.Post posts)
        {
            return new Entities.Posts
            {
                PostId = posts.PostID,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
                UserId = posts.User.UserID,
                User = MapUsers(posts.User)
            };
        }

        public static Library.Models.Post MapPostsWithComments(Entities.Posts posts)
        {
            return new Library.Models.Post
            {
               PostID = posts.PostId,
               Content = posts.Content,
               TimeSent = posts.TimeSent,
               Comments = posts.Comments.Select(MapComments).ToHashSet(),
               User = MapUsers(posts.User)
            };
        }

        public static Entities.Posts MapPostsWithComments(Library.Models.Post posts)
        {
            var postComments = posts.Comments.Select(MapComments).ToHashSet();
            return new Entities.Posts
            {
                PostId = posts.PostID,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
                Comments = postComments,
                User = MapUsers(posts.User)
            };
        }

        public static Library.Models.Comment MapComments(Entities.Comments comments)
        {
            return new Library.Models.Comment
            {
                CommentId = comments.CommentId,
                Content = comments.Content,
                TimeSent = comments.TimeSent
            };
        }

        public static Entities.Comments MapComments(Library.Models.Comment comments)
        {
            return new Entities.Comments
            {
                CommentId = comments.CommentId,
                Content = comments.Content,
                TimeSent = comments.TimeSent
            };
        }

        public static Library.Models.Comment MapCommentsWithUsers(Entities.Comments comments)
        {

            return new Library.Models.Comment
            {
                CommentId = comments.CommentId,
                Content = comments.Content,
                TimeSent = comments.TimeSent,
                Author = MapUsers(comments.User)
            };
        }

        public static Library.Models.Friendship MapFriendships(Entities.Friendships friendships)
        {
            return new Library.Models.Friendship
            {

				User1ID = friendships.User1ID,
				User2ID = friendships.User2ID,
                TimeRequestConfirmed = friendships.TimeRequestConfirmed,
            };
        }

        public static Entities.Friendships MapFriendships(Library.Models.Friendship friendships)
        {
            return new Entities.Friendships
            {

				User1ID = friendships.User1ID,
				User2ID = friendships.User2ID,
				TimeRequestConfirmed = friendships.TimeRequestConfirmed,
            };
        }

		public static Library.Models.FriendRequest MapFriendRequest(Entities.FriendRequests friendRequests)
		{
			return new Library.Models.FriendRequest
			{
				ReceiverId = friendRequests.ReceiverId,
				SenderId = friendRequests.SenderId,
				FriendRequestStatus = friendRequests.FriendRequestStatus
			};
		}

		public static Entities.FriendRequests MapFriendRequest(Library.Models.FriendRequest friendRequests)
		{
			return new Entities.FriendRequests
			{
				ReceiverId = friendRequests.ReceiverId,
				SenderId = friendRequests.SenderId,
				FriendRequestStatus = friendRequests.FriendRequestStatus
			};
		}
    }
}
