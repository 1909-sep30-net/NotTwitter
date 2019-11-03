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

            var actRepo = new PostRepository(arrangeContext);

            // Act
            var assertPost = actRepo.GetPost(postId);

            // Assert
            Assert.NotNull(arrangePost);
        }

        [Fact]
        public void CreatePostShouldStorePost()
        {
            // Assemble
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("CreatePostShouldStorePost")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            var repo = new PostRepository(arrangeContext);

            var arrangePost = new Post
            {
                PostID = 1,
                Comments = new List<Comment>(),
                TimeSent = DateTime.Now,
                Content = "Mow"
            };

            // Act
            repo.CreatePost(arrangePost);
            arrangeContext.SaveChanges();

            // Assert
            var assertPost = arrangeContext.Posts.Single();
            Assert.NotNull(arrangePost);
            Assert.Equal(1, arrangePost.PostID);

        }

        [Fact]
        public void GetPostsFromUserShouldReturnList()
        {
            // Assemble
            int userId = 1;

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetPostsFromUserShouldReturnList")
                .Options;
            var assembleContext = new NotTwitterDbContext(options);

            // Posts to populate the context
            var assemblePosts = new List<Posts>
            {
                new Posts{PostId = 1, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId},
                new Posts{PostId = 2, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId},
                new Posts{PostId = 3, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow", UserId = userId}
            };

            // Add the posts to the context
            foreach(var post in assemblePosts)
            {
                assembleContext.Posts.Add(post);
            }
            assembleContext.SaveChanges();

            var repo = new PostRepository(assembleContext);

            // Act
            var postsAssert = repo.GetPostsFromUser(userId);

            // Assert
            Assert.NotNull(postsAssert);
            Assert.Equal(assemblePosts.Count(), postsAssert.Count());

        }

        [Fact]
        public void GetAllPostsShouldReturnList()
        {
            // Assemble

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetAllPostsShouldReturnList")
                .Options;
            var assembleContext = new NotTwitterDbContext(options);

            // Posts to populate the context
            var assemblePosts = new List<Posts>
            {
                new Posts{PostId = 1, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow"},
                new Posts{PostId = 2, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow"},
                new Posts{PostId = 3, Comments = new List<Comments>(), TimeSent = DateTime.Now, Content = "Mow"}
            };

            // Add the posts to the context
            foreach (var post in assemblePosts)
            {
                assembleContext.Posts.Add(post);
            }
            assembleContext.SaveChanges();

            var repo = new PostRepository(assembleContext);

            // Act
            var postsAssert = repo.GetAllPosts();

            // Assert
            Assert.NotNull(postsAssert);
            Assert.Equal(assemblePosts.Count(), postsAssert.Count());

        }

        [Fact]
        public void UpdatePostShouldUpdate()
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
                Content = newContent
            };

            var postInDb = new Posts
            {
                PostId = 1,
                Comments = new List<Comments>(),
                TimeSent = DateTime.Now,
                Content = oldContent
            };

            // Assemble context
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("UpdatePostShouldUpdate")
                .Options;
            var assembleContext = new NotTwitterDbContext(options);
            assembleContext.Posts.Add(postInDb);

            var repo = new PostRepository(assembleContext);

            // Act
            repo.UpdatePost(postToUpdate);
            assembleContext.SaveChanges();

            // Assert
            var assertPost = assembleContext.Posts.First();
            Assert.Equal(newContent, assertPost.Content);

        }


    }
}
