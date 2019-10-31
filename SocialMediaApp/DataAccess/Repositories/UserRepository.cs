using Library.Interfaces;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly NotTwitterDbContext _context;

        /// <summary>
        /// Constructs repository with the dbdontext
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(NotTwitterDbContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Given an ID, returns matching user from DB
        /// </summary>
        /// <param name="id">User ID to be searched for</param>
        /// <returns>User matching the given ID</returns>
        public User GetUserByID(int id)
        {
            return Mapper.MapUsers( _context.Users.Find(id) );
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
                .Where(u => (u.FirstName + u.LastName)
                .Contains(name))
                .Select(Mapper.MapUsers);
        }

        /// <summary>
        /// Given a business model user, add user to database
        /// </summary>
        /// <param name="newUser">User to add to db</param>
        public void AddUser(User newUser)
        {
            var newEntity = Mapper.MapUsers(newUser);
            newUser.UserID = 0;
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
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

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
