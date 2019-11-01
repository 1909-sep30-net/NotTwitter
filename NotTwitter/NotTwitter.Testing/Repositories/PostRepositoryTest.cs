using Microsoft.EntityFrameworkCore;
using NotTwitter.DataAccess;
using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotTwitter.Testing.Repositories
{
    public class PostRepositoryTest
    {
        [Fact]
        public void GetPostShouldReturnResult()
        {
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetPostShouldReturnResult")
                .Options;

            using var arrangeContext = new NotTwitterDbContext(options);
            var postId = 1;
            var arrangePost = new Posts
            {
                PostId = postId,
                UserId = 1,
                Content = "blah",
            };
            arrangeContext.Posts.Add(arrangePost);
            arrangeContext.SaveChanges();

            var actRepo = new PostRepository(arrangeContext);
            var assertPost = actRepo.GetPost(postId);
            Assert.NotNull(arrangePost);
        }
    }
}
