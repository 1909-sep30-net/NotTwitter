using API.Controllers;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Testing.Repositories
{
    public class UserRepositoryTest
    {
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
                FirstName = "Jicky",
                LastName = "Johnson",
                Email = "abc.abc@abc.com",
                Username = "blah",
                Password = "blah",
                Gender = 1
            };
            arrangeContext.Users.Add(testUserEntity);
            arrangeContext.SaveChanges();
            using var actContext = new NotTwitterDbContext(options);
            var repo = new UserRepository(actContext);

            // Act
            var result = repo.GetUserByID(testId);
             
            // Assert
            Assert.NotNull(result);
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
                FirstName = "Tester",
                LastName = "Testy",
                Email = "abc.abc@abc.com",
                Username = "blah",
                Password = "blahblah",
                Gender = 1
            };
            var actRepo = new UserRepository(actContext);

            // Act
            actRepo.AddUser(newUser);
            actRepo.Save();

            // Assert
            using var assertContext = new NotTwitterDbContext(options);
            var assertUser = assertContext.Users.First(u => u.FirstName == newUser.FirstName);

        }
    }
}
