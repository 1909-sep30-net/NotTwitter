using Microsoft.EntityFrameworkCore;
using NotTwitter.DataAccess.Entities;
using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotTwitter.DataAccess.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly NotTwitterDbContext _context;
        public GenericRepository(NotTwitterDbContext db)
        {
            _context = db ?? throw new ArgumentNullException("Context cannot be null.",nameof(db));
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(Mapper.MapUsers);
        }

        /// <summary>
        /// Given an ID, returns matching user from DB
        /// </summary>
        /// <param name="id">User ID to be searched for</param>
        /// <returns>User matching the given ID</returns>
        public async Task<User> GetUserByID(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
            {
                return null;
            }
            else
            {
                return Mapper.MapUsers(user);
            }
        }

        /// <summary>
        /// Given an email, returns matching user from DB
        /// </summary>
        /// <param name="id">User email to be searched for</param>
        /// <returns>User matching the given email</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                return Mapper.MapUsers(user);
            }
        }

        /// <summary>
        /// Returns a user with a populated list of friends
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserWithFriends(int id)

        {
            var userFriends = await _context.Friendships.Where(fs => fs.User1ID == id).AsNoTracking().ToListAsync();
            var user = await GetUserByID(id);
            foreach (var fs in userFriends)
            {
                var frond = await GetUserByID(fs.User2ID);
                user.Friends.Add(frond);
            }
            return user;
        }

        /// <summary>
        /// Returns list of users with name matching given string
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>Checks combination of user's first and last name</remarks>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsersByName(string name)
        {
            var result = await _context.Users
                .Where(u => ((u.FirstName + u.LastName).ToUpper())
                .Contains(name.ToUpper()))
                .ToListAsync();

            return result.Select(Mapper.MapUsers);
        }

        /// <summary>
        /// Given a business model user, add user to database
        /// </summary>
        /// <param name="newUser">User to add to db</param>
        public void AddUser(User newUser)
        {
            var newEntity = Mapper.MapUsers(newUser);
            newEntity.UserID = 0;
            _context.Users.Add(newEntity);
        }

        /// <summary>
        /// Update the database with new user information
        /// </summary>
        /// <param name="user"></param>
        public async Task UpdateUser(User user)
        {
            var oldEntity = await _context.Users.FindAsync(user.UserID);
            var updatedEntity = Mapper.MapUsers(user);

            _context.Entry(oldEntity).CurrentValues.SetValues(updatedEntity);
        }

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id">ID of the user to be deleted</param>
        public async Task DeleteUserByID(int id)
        {
            var entityToBeRemoved = await _context.Users.FindAsync(id);
            _context.Remove(entityToBeRemoved);
        }

        public void AddFriendShip(Friendship newFriend)
        {
            var newEntity = Mapper.MapFriendships(newFriend);
            _context.Add(newEntity);
        }

        /// <summary>
        /// Stores new post in database, associated with a user
        /// </summary>
        /// <param name="post">Post to be stored</param>
        public async Task CreatePost(Post post, User author = null)
        {
            if (author != null)
            {
                var authorEntity = await _context.Users
                    .Include(u => u.Posts)
                    .FirstOrDefaultAsync(u => u.UserID == author.UserID);
                var postEntity = Mapper.MapPost(post);
                authorEntity.Posts.Add(postEntity);
            }
            else
            {
                var entity = Mapper.MapPost(post);
                entity.PostId = 0;
                _context.Posts.Add(entity);
            }
        }

        /// <summary>
        /// Deletes post and its comments from database
        /// </summary>
        /// <param name="postId">Id of the post to be removed</param>
        public async Task DeletePost(int postId)
        {
            // Find post and eager load its comments
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == postId);

            // Delete all comments from post
            foreach (var comment in post.Comments)
            {
                _context.Remove(comment);
            }

            // Then delete post
            _context.Remove(post);
        }

        /// <summary>
        /// Gets post by ID, including comments, including authors
        /// </summary>
        /// <param name="postId">Id of the specified post</param>
        /// <returns></returns>
        public async Task<Post> GetPostById(int postId)
        {
            // Immediately return null if post does not exist
            if (await _context.Posts.FindAsync(postId) == null)
            {
                return null;
            }

            // Then get the post with comments
            var postWithComments = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);

            return Mapper.MapPostsWithComments(postWithComments);
        }

        /// <summary>
        /// Gets posts from user, including comments
        /// </summary>
        /// <param name="userId">User id to get posts from</param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetPostsByUser(int userId)
        {
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .ToListAsync().ConfigureAwait(false);
			

			return posts.Select(Mapper.MapPostsWithComments);

        }

        /// <summary>
        /// Gets all posts in database
        /// </summary>
        /// <returns>All posts in data base</returns>
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var posts = await _context.Posts.Include(p=>p.Comments).ThenInclude(c=>c.User).ToListAsync();
            return posts.Select(Mapper.MapPostsWithComments);
        }

        /// <summary>
        /// Updates database with given post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        public async Task UpdatePost(Post post)
        {
            var newEntity = Mapper.MapPostWithUser(post);
            var oldEntity = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == post.PostID);

            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Add new comment to database
        /// </summary>
        /// <param name="newComment">Comment to be added</param>
        public async Task CreateComment(Comment newComment, User author = null, Post post = null)
        {
            Comments entityComment = Mapper.MapComments(newComment);

            if (post != null && author != null)
            {
                Posts entityPost = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == post.PostID);
                Users entityAuthor = await _context.Users.FindAsync(author.UserID);
                entityAuthor.Comments.Add(entityComment);
                entityPost.Comments.Add(entityComment);
            }
            else
            {
                _context.Add(entityComment);
                newComment.CommentId = entityComment.CommentId;
            }
        }

        /// <summary>
        /// Updates the content of the comment
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns></returns>
        public async Task UpdateComment(Comment newComment)
        {
            var oldEntity = await _context.Comments.FindAsync(newComment.CommentId);
            oldEntity.Content = newComment.Content;
            _context.Entry(oldEntity).CurrentValues.SetValues(oldEntity);
        }

        /// <summary>
        /// Deletes all comments from a post
        /// </summary>
        /// <remarks>Used in conjunction with post deletion</remarks>
        /// <param name="postId"></param>
		public async Task DeleteCommentsByPostId(int postId)
        {
            var comments = await _context.Comments.Where(p => p.PostId == postId).ToListAsync();
            foreach (var comment in comments)
            {
                _context.Remove(comment);
            }
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return null;
            }
            return Mapper.MapComments(comment);
        }

        /// <summary>
        /// Gets all comments from a post
        /// </summary>
        /// <param name="postId"></param>
        /// <remarks>Orders comments by time sent, descending</remarks>
        /// <returns></returns>
		public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            if (await _context.Posts.FindAsync(postId) == null)
            {
                return null;
            }
            var comments = await _context.Comments.Where(p => p.PostId == postId).OrderByDescending(d => d.TimeSent).ToListAsync();
            return comments.Select(Mapper.MapComments);
        }

        /// <summary>
        /// Gets all comments from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsByUserId(int userId)
        {
            // Return null if user doesnt exist, that way we can distinguish between invalid user or valid user but with no comments
            if (await _context.Users.FirstOrDefaultAsync(u=>u.UserID == userId) == null)
            {
                return null;
            }
            var comments = await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
            return comments.Select(Mapper.MapComments);
        }

        /// <summary>
        /// Get specific friend request
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        public async Task<FriendRequest> GetFriendRequest(int senderId, int receiverId)
        {
            var fr = await _context.FriendRequests
                    .FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
            if (fr == null)
            {
                return null;
            }
            return Mapper.MapFriendRequest(fr);
        }

        /// <summary>
        /// Returns a list of all pending requests for a user.
        /// </summary>
        /// <param name="userId">User with the pending requests</param>
        /// <returns>List of all pending friend requests</returns>
        public async Task<IEnumerable<FriendRequest>> GetAllFriendRequests(int userId)
        {
            var friendRequests = await _context.FriendRequests
                .Where(fr => fr.ReceiverId == userId)
                .ToListAsync();
            return friendRequests.Select(Mapper.MapFriendRequest);
        }

        /// <summary>
        /// Get's the status of a specific friend request
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
		public async Task<int> FriendRequestStatus(int senderId, int receiverId)
        {
            var fr = await _context.FriendRequests
                .Where(r => r.SenderId == senderId && r.ReceiverId == receiverId)
                .FirstOrDefaultAsync();
            return fr.FriendRequestStatus;
        }

        /// <summary>
        /// Creates friend request in database
        /// </summary>
        /// <param name="request">Request to be added</param>
        public void CreateFriendRequest(FriendRequest request)
        {
            _context.FriendRequests.Add(Mapper.MapFriendRequest(request));
        }


        /// <summary>
        /// Updates friend request in the database with new values
        /// </summary>
        /// <param name="request">Request to be updated</param>
        public async Task UpdateFriendRequest(FriendRequest request)
        {
            var newEntity = Mapper.MapFriendRequest(request);
            var oldEntity = await _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefaultAsync();
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes request from the database
        /// </summary>
        /// <param name="request">Request to be deleted</param>
		public async Task DeleteFriendRequest(FriendRequest request)
        {
            var requestEntity = await _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefaultAsync();

            _context.FriendRequests.Remove(requestEntity);
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        public async Task Save()
        {
            // TODO: Ideally put a log message here to notify when saving
            await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();

                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public async Task Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            await DisposeAsync(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
        }
        #endregion
    }
}
