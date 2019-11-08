using System.Linq;

namespace NotTwitter.DataAccess
{
    public static class Mapper
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
        /// Map posts without comments, without Users
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static Library.Models.Post MapPost(Entities.Posts posts)
        {
            return new Library.Models.Post
            {
                PostID = posts.PostId,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
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
            };
        }

        /// <summary>
        /// Map posts without comments, without Users
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static Library.Models.Post MapPostWithUser(Entities.Posts posts)
        {
            return new Library.Models.Post
            {
                PostID = posts.PostId,
                Content = posts.Content,
                TimeSent = posts.TimeSent,
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

        /// <summary>
        /// Maps posts with comments and users
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static Library.Models.Post MapPostsWithComments(Entities.Posts post)
        {
            return new Library.Models.Post
            {
               PostID = post.PostId,
               Content = post.Content,
               TimeSent = post.TimeSent,
               Comments = post.Comments.Select(MapCommentsWithUsers).ToHashSet(),
               User = MapUsers(post.User)
            };
        }

        /// <summary>
        /// Maps posts with comments and users
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static Entities.Posts MapPostsWithComments(Library.Models.Post post)
        {
            return new Entities.Posts
            {
                PostId = post.PostID,
                Content = post.Content,
                TimeSent = post.TimeSent,
                Comments = post.Comments.Select(MapComments).ToHashSet(),
                //User = MapUsers(post.User)
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

        public static Entities.Comments MapCommentsWithUsers(Library.Models.Comment comments)
        {

            return new Entities.Comments
            {
                CommentId = comments.CommentId,
                Content = comments.Content,
                TimeSent = comments.TimeSent,
                User = MapUsers(comments.Author)
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
