using Microsoft.EntityFrameworkCore;
using NotTwitter.DataAccess;
using NotTwitter.DataAccess.Entities;
using NotTwitter.DataAccess.Entities.Enum;
using NotTwitter.DataAccess.Repositories;
using NotTwitter.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotTwitter.Testing.Repositories
{
    public class FriendRepositoryTest
    {
        [Fact]
        public async Task GetValidFriendRequestShouldReturnResult()
        {
            // Assemble

            int senderid = 1;
            int receiverid = 2;
            int status = (int)FriendRequestStatus.Pending;

            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetValidFriendRequestShouldReturnResult")
                .Options;

            using var arrangeContext = new NotTwitterDbContext(options);
            var assembleFR = new FriendRequests
            {
                SenderId = senderid,
                ReceiverId = receiverid,
                FriendRequestStatus = status
            };
            arrangeContext.FriendRequests.Add(assembleFR);
            arrangeContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);

            // Act
            var assertFR = await actRepo.GetFriendRequest(senderid,receiverid);

            // Assert
            Assert.NotNull(assertFR);
            Assert.Equal(status, assertFR.FriendRequestStatus);
        }

        [Fact]
        public async Task GetInvalidFriendRequestShouldReturnNull()
        {
            // Assemble

            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetInvalidFriendRequestShouldReturnResult")
                .Options;

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);

            // Act
            var assertFR = await actRepo.GetFriendRequest(1, 2);

            // Assert
            Assert.Null(assertFR);
        }

        [Fact]
        public async Task GetAllFriendRequestsShouldReturnList()
        {
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetAllFriendRequestsShouldReturnList")
                .Options;

            var assembleFR = new FriendRequests
            {
                SenderId = 1,
                ReceiverId = 2,
                FriendRequestStatus = 0
            };

            using var assembleContext = new NotTwitterDbContext(options);

            assembleContext.FriendRequests.Add(assembleFR);
            assembleContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);

            // Act
            var assertAllFR = await actRepo.GetAllFriendRequests(2);

            // Assert
            Assert.NotNull(assertAllFR);
            Assert.IsAssignableFrom<List<FriendRequest>>(assertAllFR.ToList());

        }

        [Fact]
        public async Task GetFriendRequestStatusShouldReturnInt()
        {
            // Arrange
            int arrangeStatus = 0;
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("GetFriendRequestStatusShouldReturnInt")
                .Options;

            var assembleFR = new FriendRequests
            {
                SenderId = 1,
                ReceiverId = 2,
                FriendRequestStatus = arrangeStatus
            };

            using var assembleContext = new NotTwitterDbContext(options);

            // Add friend request to in-memory database
            assembleContext.FriendRequests.Add(assembleFR);
            assembleContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);

            // Act
            var assertFRStatus = await actRepo.FriendRequestStatus(1,2);

            // Assert
            Assert.Equal(arrangeStatus, assertFRStatus);
        }

        [Fact]
        public void CreateFriendRequestShouldCreate()
        {
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("CreateFriendRequestShouldCreate")
                .Options;

            using var assembleContext = new NotTwitterDbContext(options);

            var assembleRepo = new GenericRepository(assembleContext);

            FriendRequest assembleFR = new FriendRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                FriendRequestStatus = 0
            };

            // Act
            assembleRepo.CreateFriendRequest(assembleFR);
            assembleContext.SaveChanges();

            // Assert
            Assert.NotNull(assembleContext.FriendRequests.FirstOrDefault(fr => fr.SenderId == 1 && fr.ReceiverId == 2));
        }

        [Fact]
        public async Task UpdateFriendRequestShouldUpdate()
        {
            var options = new DbContextOptionsBuilder<NotTwitterDbContext>()
                .UseInMemoryDatabase("UpdateFriendRequestShouldUpdate")
                .Options;

            using var arrangeContext = new NotTwitterDbContext(options);

            FriendRequests assembleFR = new FriendRequests
            {
                SenderId = 1,
                ReceiverId = 2,
                FriendRequestStatus = 0
            };

            FriendRequest actFR = new FriendRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                FriendRequestStatus = 2
            };

            arrangeContext.FriendRequests.Add(assembleFR);
            arrangeContext.SaveChanges();

            using var actContext = new NotTwitterDbContext(options);
            var actRepo = new GenericRepository(actContext);
            await actRepo.UpdateFriendRequest(actFR);
            await actContext.SaveChangesAsync();

            var result = await actContext.FriendRequests.FirstOrDefaultAsync();

            Assert.Equal(actFR.FriendRequestStatus, result.FriendRequestStatus);
        }
    }
}
