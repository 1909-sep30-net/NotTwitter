using API.Controllers;
using DataAccess;
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
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetUserByIdShouldReturnResult")
                .Options;
            using var arrangeContext = new NotTwitterDbContext(options);
            var testId = 2;
            arrangeContext.Users.Add(new DataAccess.Entities.Users { });
            arrangeContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var repo = new UserRepository(actContext);

            var result = repo.GetUserByID(testId);

            Assert.NotNull(result);
        }
    }
}
