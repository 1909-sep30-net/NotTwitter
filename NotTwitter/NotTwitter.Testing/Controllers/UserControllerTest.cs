using NotTwitter.API.Controllers;
using NotTwitter.API.Models;
using NotTwitter.Library.Models;
using Moq;
using NotTwitter.Library.Interfaces;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace NotTwitter.Testing.Controllers
{
    public class UserControllerTest
    {
        
        [Fact]
        public void GetUserByIdShouldReturnUser()
        {
            // Assemble 
            var userIdForTest = 2;
            var mockRepo = new Mock<IUserRepository>();
            var mockPostRepo = new Mock<IPostRepository>();
            mockRepo.Setup(x => x.GetUserByID(It.IsAny<int>()))
                .Returns(new User
                {
                    UserID = userIdForTest,
                    FirstName = "Moo",
                    LastName = "Lah",
                    Username = "Moolah",
                    Email = "Hippy@gmail.com",
                    Gender = 1
                });

            var controller = new UserController(mockRepo.Object, mockPostRepo.Object);

            // Act
            var result = controller.Get(userIdForTest);

            // Asserts
            var viewresult = Assert.IsAssignableFrom<UserViewModel>(result);
            Assert.Equal(userIdForTest, viewresult.Id);
        }

        [Fact]
        public void PostUserShouldPost()
        {
            UserViewModel newUser = new UserViewModel()
            {
                Username = "hithisistest",
                FirstName = "HiThis",
                LastName = "IsTest",
                Password = "password1",
                Gender = 1,
                Email = "hithisistest@test.com",
                Id = 3,
            };

            var userList = new List<User>
            {
                new User {UserID = 1, FirstName = "abc", LastName = "abc"},
                new User {UserID = 2, FirstName = "abc", LastName = "abc"},
                new User {UserID = 3, FirstName = "abc", LastName = "abc"},
            };


            var mockRepo = new Mock<IUserRepository>();
            var mockPostRepo = new Mock<IPostRepository>();

            mockRepo.Setup(x => x.AddUser(It.IsAny<User>()))
                .Callback(() => 
                {
                    userList.Add(new User { UserID = 4, FirstName = "abc", LastName = "abc" });
                });

            var controller = new UserController(mockRepo.Object, mockPostRepo.Object);

            // Act
            var response = controller.Post(newUser);
            var responseContent = response as CreatedAtRouteResult;

            // Assert
            mockRepo.Verify(x => x.AddUser(It.IsAny<User>()));
            Assert.Equal(4, userList.Count);
            Assert.Equal(4, userList.First(x=>x.UserID==4).UserID);

            Assert.NotNull(responseContent);

        }

        public void GetFriendPostShouldReturnPost()
        {

        }


    }
}
