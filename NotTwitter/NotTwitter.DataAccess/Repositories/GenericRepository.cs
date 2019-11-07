using Microsoft.EntityFrameworkCore;
using NotTwitter.Library.Interfaces;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotTwitter.DataAccess.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly NotTwitterDbContext _context;
        public GenericRepository(NotTwitterDbContext db)
        {
            _context = db ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Given an ID, returns matching user from DB
        /// </summary>
        /// <param name="id">User ID to be searched for</param>
        /// <returns>User matching the given ID</returns>
        public User GetUserByID(int id)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserID == id);
            //var user = _context.Users.Find(id);
            if (user == null)
            {
                return null;
            }
            else
            {
                return Mapper.MapUsers(user);
            }
        }

        public User GetUserWithFriends(int id)
        {
            var userFriends = _context.Friendships.Where(fs => fs.User1ID == id).AsNoTracking().ToList();
            var user = GetUserByID(id);
            //var user = _context.Users.Find(id);
            foreach (var fs in userFriends)
            {
                var frond = GetUserByID(fs.User2ID);
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
        public IEnumerable<User> GetUsersByName(string name)
        {
            return _context.Users
                .Where(u => ((u.FirstName + u.LastName).ToUpper())
                .Contains(name.ToUpper()))
                .Select(Mapper.MapUsers);
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
        public void UpdateUser(User user)
        {
            var oldEntity = _context.Users.Find(user.UserID);
            var updatedEntity = Mapper.MapUsers(user);

            _context.Entry(oldEntity).CurrentValues.SetValues(updatedEntity);
        }

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id">ID of the user to be deleted</param>
        public void DeleteUserByID(int id)
        {
            var entityToBeRemoved = _context.Users.Find(id);
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
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return posts.Select(Mapper.MapPostsWithComments);

        }

        /// <summary>
        /// Gets all posts in database
        /// </summary>
        /// <returns>All posts in data base</returns>
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts.Select(Mapper.MapPostsWithComments);
        }

        /// <summary>
        /// Updates database with given post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        public async Task UpdatePost(Post post)
        {
            var newEntity = Mapper.MapPostsWithComments(post);
            var oldEntity = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.PostId == post.PostID);

            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        public void CreateComment(Comment newComment)
        {

            var newEntity = Mapper.MapComments(newComment);
            _context.Add(newEntity);
            _context.SaveChanges();
        }

        public void UpdateComment(Comment newComment)
        {
            var newEntity = Mapper.MapComments(newComment);
            var oldEntity = _context.Comments.Find(newComment.CommentId);
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes all comments from a post
        /// </summary>
        /// <remarks>Used in conjunction with post deletion</remarks>
        /// <param name="postId"></param>
		public void DeleteCommentsByPostId(int postId)
        {
            var comments = _context.Comments.Where(p => p.PostId == postId);
            foreach (var comment in comments)
            {
                _context.Remove(comment);
            }

        }

        /// <summary>
        /// Gets all comments from a post
        /// </summary>
        /// <param name="postId"></param>
        /// <remarks>Orders comments by time sent, descending</remarks>
        /// <returns></returns>
		public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return _context.Comments.Where(p => p.PostId == postId).OrderByDescending(d => d.TimeSent).Select(Mapper.MapComments);
        }

        public IEnumerable<Comment> GetCommentsByUserId(int userId)
        {
            return _context.Comments.Where(c => c.UserId == userId).Select(Mapper.MapComments);
        }

        public FriendRequest GetFriendRequest(int senderId, int receiverId)
        {
            return Mapper.MapFriendRequest(_context.FriendRequests
                    .FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId));
        }

        /// <summary>
        /// Returns a list of all pending requests for a user.
        /// </summary>
        /// <param name="userId">User with the pending requests</param>
        /// <returns>List of all pending friend requests</returns>
        public IEnumerable<FriendRequest> GetAllFriendRequests(int userId)
        {
            return _context.FriendRequests.Where(fr => fr.SenderId == userId)
                .Select(Mapper.MapFriendRequest).ToList();
        }

        /// <summary>
        /// Checks if a given friend request already exists
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
		public int FriendRequestStatus(int senderId, int receiverId)
        {
            return _context.FriendRequests.Where(r => r.SenderId == senderId && r.ReceiverId == receiverId).FirstOrDefault().FriendRequestStatus;
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
        public void UpdateFriendRequest(FriendRequest request)
        {
            var newEntity = Mapper.MapFriendRequest(request);
            var oldEntity = _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefault();
            _context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Deletes request from the database
        /// </summary>
        /// <param name="request">Request to be deleted</param>
		public void DeleteFriendRequest(FriendRequest request)
        {
            var requestEntity = _context.FriendRequests
                .Where(r => r.SenderId == request.SenderId && r.ReceiverId == request.ReceiverId)
                .FirstOrDefault();

            _context.FriendRequests.Remove(requestEntity);
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        public void Save()
        {
            // TODO: Ideally put a log message here to notify when saving
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();

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
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
