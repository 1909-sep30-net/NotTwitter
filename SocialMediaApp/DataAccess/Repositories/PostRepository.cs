using Library.Interfaces;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly NotTwitterDbContext _context;

        public PostRepository(NotTwitterDbContext db)
        {
            _context = db ?? throw new NullReferenceException();
        }
        public void CreatePost(Post post)
        {
            _context.Add(Mapper.MapPosts(post));
        }

        public void DeletePost()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPosts(int id)
        {
            throw new NotImplementedException();
        }


        public void UpdatePost()
        {
            throw new NotImplementedException();
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PostRepository()
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
