using API.Controllers;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Testing.Repositories
{
    public class UserRepositoryTest
    {
        [Fact]
        public void GetUserByIdShouldReturnResult()
        {
            // Assemble
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
    }
}
