using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Repositories;
using NotTwitter.Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using NotTwitter.DataAccess;

namespace NotTwitter.Testing.Repositories
{
    public class UserRepositoryTest
    {
        // Used to set up user with valid properties -- easily update if requirements change
        public string ValidName = "Jicky";
        public string ValidEmail = "abc.abc@abc.com";
        public string ValidUsername = "Jickytime";
        public string ValidPassword = "blahblah";
        public int ValidGender = 1;

        [Fact]
        public void GetUserByIdShouldReturnResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetUserByIdShouldReturnResult")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            var testId = 2;
            var testUserEntity = new Users
            {
                UserID = 2,
                FirstName = ValidName,
                LastName = ValidName,
                Email = ValidEmail,
                Username = ValidUsername,
                Password = ValidPassword,
                Gender = ValidGender
            };
            arrangeContext.Users.Add(testUserEntity);
            arrangeContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var repo = new UserRepository(actContext);

            // Act
            var result = repo.GetUserByID(testId);
             
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testId, result.UserID);
        }

        [Theory]
        [InlineData("Jicky","Jick")]

        public void GetUsersByNameShouldReturnList(string fullName, string partialName)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetUsersByNameShouldReturnList")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            for (int i=0;i<3;i++)
            {
                arrangeContext.Users.Add(
                    new Users
                    {
                        FirstName = fullName,
                        LastName = fullName,
                        Email = ValidEmail,
                        Username = ValidUsername,
                        Password = ValidPassword,
                        Gender = ValidGender
                    }
                );
            }
            arrangeContext.SaveChanges();

            var actContext = new NotTwitterDbContext(options);
            var repo = new UserRepository(actContext);

            // Act
            var result = repo.GetUsersByName(partialName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(actContext.Users.Count(), result.Count());
        }

        [Fact]
        public void AddUserShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("AddUserShouldAdd")
                .Options;
            using var actContext = new NotTwitterDbContext(options);
            var newUser = new User
            {
                FirstName = ValidName,
                LastName = ValidName,
                Email = ValidEmail,
                Username = ValidUsername,
                Password = ValidPassword,
                Gender = ValidGender
            };
            var actRepo = new UserRepository(actContext);

            // Act
            actRepo.AddUser(newUser);
            actContext.SaveChanges();

            // Assert
            using var assertContext = new NotTwitterDbContext(options);
            var assertUser = assertContext.Users.First(u => u.FirstName == newUser.FirstName);
            Assert.NotNull(assertUser);
        }

        [Fact]
        public void UpdateUserShouldUpdate()
        {
            // Arrange
            var updatedName = "Robby";
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("AddUserShouldUpdate")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            var arrangeUser = new Users
            {
                UserID = 1,
                FirstName = ValidName,
                LastName = ValidName,
                Email = ValidEmail,
                Username = ValidUsername,
                Password = ValidPassword,
                Gender = ValidGender
            };
            arrangeContext.Users.Add(arrangeUser);
            var updatedUser = new User 
            {
                UserID = 1,
                FirstName = updatedName,
                LastName = ValidName,
                Email = ValidEmail,
                Username = ValidUsername,
                Password = ValidPassword,
                Gender = ValidGender
            };

            // Act
            var repo = new UserRepository(arrangeContext);
            repo.UpdateUser(updatedUser);
            arrangeContext.SaveChanges();

            // Assert
            var assertContext = new NotTwitterDbContext(options);
            var assertUser = assertContext.Users.First(u => u.FirstName == updatedName);
            Assert.NotNull(assertUser);
            Assert.Equal(updatedName, assertUser.FirstName);
        }

        [Fact]
        public void DeleteUserShouldDelete()
        {
            //Assemble
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("DeleteUserShouldDelete")
                .Options;
            var assembleContext = new NotTwitterDbContext(options);
            var someUser = new Users
            {
                FirstName = ValidName,
                LastName = ValidName,
                Email = ValidEmail,
                Username = ValidUsername,
                Password = ValidPassword,
                Gender = ValidGender
            };
            assembleContext.Add(someUser);

            var repo = new UserRepository(assembleContext);

            // Act
            repo.DeleteUserByID(1);

            // Assert
            var users = assembleContext.Users.ToList();

            Assert.DoesNotContain(someUser,users);
        }
    }
}
