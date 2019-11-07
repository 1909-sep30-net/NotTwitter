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
    public class PostRepositoryTest
    {
        Users testEntityUser = new Users
        {
            UserID = 1,
            FirstName = "MyFirstName",
            LastName = "MyLastName",
            Email = "valid@email.com",
            Username = "myUsername",
            Password = "myPassword",
            Gender = 0,
            Posts = new List<Posts>(),
            Comments = new List<Comments>(),
            
        };

        Posts testEntityPost = new Posts
        {
            PostId = 1,
            Comments = new List<Comments>(),
            TimeSent = new DateTime(2001,6,6),
            Content = "post content",
            UserId = 1,
        };

        Comments testEntityComment = new Comments
        {
            CommentId = 1,
            Content = "comment content",
            TimeSent = new DateTime(2001,5,5),
            UserId = 1,
        };

        [Fact]
        public void GetPostShouldReturnResult()
        {
            // Assemble
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

            var actRepo = new GenericRepository(arrangeContext);

            // Act
            var assertPost = actRepo.GetPostById(postId);

            // Assert
            Assert.NotNull(arrangePost);
        }

        [Fact]
        public async Task CreatePostShouldStorePost()
        {
            // Assemble
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("CreatePostShouldStorePost")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            var repo = new GenericRepository(arrangeContext);

            var arrangePost = new Post
            {
                PostID = 1,
                Comments = new List<Comment>(),
                TimeSent = DateTime.Now,
                Content = "Mow"
            };

            // Act
            await repo.CreatePost(arrangePost);
            arrangeContext.SaveChanges();

            // Assert
            var assertPost = arrangeContext.Posts.Single();
            Assert.NotNull(arrangePost);
            Assert.Equal(1, arrangePost.PostID);

        }

        [Fact]
        public async Task GetPostsFromUserShouldReturnList()
        {
            // Assemble
            int userId = testEntityUser.UserID;

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetPostsFromUserShouldReturnList")
                .Options;
            using var assembleContext = new NotTwitterDbContext(options);

            // Posts to populate the context
            var assemblePosts = new List<Posts>
            {
                new Posts{PostId = 1, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId, User = testEntityUser},
                new Posts{PostId = 2, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId, User = testEntityUser},
                new Posts{PostId = 3, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId, User = testEntityUser}
            };

            // Add the posts to the context
            foreach(var post in assemblePosts)
            {
                assembleContext.Posts.Add(post);
            }
            await assembleContext.SaveChangesAsync();

            using var assertContext = new NotTwitterDbContext(options);
            var repo = new GenericRepository(assertContext);

            // Act
            var postsAssert = await repo.GetPostsByUser(userId);
            var posts = assertContext.Posts.Where(p => p.UserId == userId);
            // Assert
            Assert.NotNull(postsAssert);
            Assert.Equal(assemblePosts.Count(), postsAssert.Count());

        }

        [Fact]
        public async Task GetAllPostsShouldReturnList()
        {
            // Assemble

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetAllPostsShouldReturnList")
                .Options;
            using var assembleContext = new NotTwitterDbContext(options);

            // Posts to populate the context
            var assemblePosts = new List<Posts>
            {
                new Posts{PostId = 1, Comments = new List<Comments>{ testEntityComment }, TimeSent = DateTime.Now, Content = "post content", User = testEntityUser},
                new Posts{PostId = 2, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "post content", User = testEntityUser},
                new Posts{PostId = 3, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "post content", User = testEntityUser}
            };

            // Add the posts to the context
            foreach (var post in assemblePosts)
            {
                assembleContext.Posts.Add(post);
            }
            await assembleContext.SaveChangesAsync();

            using var actContext = new NotTwitterDbContext(options);
            var repo = new GenericRepository(actContext);

            // Act
            var postsAssert = await repo.GetAllPosts();

            // Assert
            Assert.NotNull(postsAssert);
            Assert.Equal(assemblePosts.Count(), postsAssert.Count());

        }

        [Fact]
        public async Task UpdatePostShouldUpdateEntity()
        {
            // Assemble
            var oldContent = "oldcontent..";
            var newContent = "newcontent!";

            // Assemble posts
            var postToUpdate = new Post
            {
                PostID = 1,
                Comments = new List<Comment>(),
                TimeSent = DateTime.Now,
                Content = newContent,

            };

            var postInDb = new Posts
            {
                PostId = 1,
                Comments = new List<Comments>(),
                TimeSent = DateTime.Now,
                Content = oldContent,
                User = testEntityUser
            };

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("UpdatePostShouldUpdateEntity")
                .Options;
            using var assembleContext = new NotTwitterDbContext(options);
            assembleContext.Posts.Add(postInDb);
            await assembleContext.SaveChangesAsync();

            using var actContext = new NotTwitterDbContext(options);
            var repo = new GenericRepository(actContext);

            // Act
            await repo.UpdatePost(postToUpdate);
            await actContext.SaveChangesAsync();

            // Assert
            var assertPost = actContext.Posts.First();
            Assert.Equal(newContent, assertPost.Content);

        }

        [Fact]
        public async Task DeletePostShouldRemovePostAndComments()
        {
            // Assemble
            var postId = 1;
            var commentId = 1;
            var commentInDb = new Comments
            {
                CommentId = commentId,
                Content = "delete me too",
                TimeSent = DateTime.Now,
            };

            // Assemble posts
            var postInDb = new Posts
            {
                PostId = postId,
                Comments = new List<Comments> { commentInDb },
                TimeSent = DateTime.Now,
                Content = "delete me!"
            };

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("DeletePostShouldRemovePost")
                .Options;
            using var assembleContext = new NotTwitterDbContext(options);
            assembleContext.Posts.Add(postInDb);
            assembleContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var repo = new GenericRepository(actContext);

            // Act
            await repo.DeletePost(postId);
            actContext.SaveChanges();

            // Assert
            var assertPost = actContext.Posts.Any();
            var assertComment = actContext.Comments.Any();
            Assert.False(assertPost);
            Assert.False(assertComment);
        }



    }
}
