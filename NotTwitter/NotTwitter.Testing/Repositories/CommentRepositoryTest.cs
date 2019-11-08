using Microsoft.EntityFrameworkCore;
using NotTwitter.DataAccess;
using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Repositories;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotTwitter.Testing.Repositories
{
    public class CommentRepositoryTest
    {
        [Fact]
        public async Task CreateCommentShouldCreate()
        {
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("CreateCommentShouldCreate")
                .Options;
            using var assembleContext = new NotTwitterDbContext(options);

            Users author = new Users
            {
                UserID = 2,
                FirstName = "ValidName",
                LastName = "ValidName",
                Email = "ValidEmail@email.com",
                Username = "ValidUsername",
                Password = "ValidPassword",
                Gender = 0
            };

            Posts post = new Posts
            {
                PostId = 1,
                Comments = new List<Comments>(),
                TimeSent = new DateTime(2001, 6, 6),
                Content = "post content",
            };

            assembleContext.Add(author);
            assembleContext.Add(post);
            assembleContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);

            Comment assembleComment = new Comment
            {
                Content = "Comment content",
                TimeSent = DateTime.Now,
            };
            User assembleAuthor = new User
            {
                UserID = 2,
                FirstName = "ValidName",
                LastName = "ValidName",
                Email = "ValidEmail@email.com",
                Username = "ValidUsername",
                Password = "ValidPassword",
                Gender = 0
            };
            Post assemblePost = new Post
            {
                PostID = 1,
                Comments = new List<Comment>(),
                TimeSent = new DateTime(2001, 6, 6),
                Content = "post content",
            };

            await actRepo.CreateComment(assembleComment, assembleAuthor, assemblePost);
            actContext.SaveChanges();

            using var assertContext = new NotTwitterDbContext(options);
            var assertComment = assertContext.Comments.Include(c=>c.User).Include(c=>c.Post).FirstOrDefault();
            Assert.NotNull(assertComment);
            Assert.NotNull(assertComment.Post);
            Assert.NotNull(assertComment.User);
        }
    }
}
