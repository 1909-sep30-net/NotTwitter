using NotTwitter.API.Controllers;
using NotTwitter.API.Models;
using NotTwitter.Library.Models;
using Moq;
using NotTwitter.Library.Interfaces;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NotTwitter.Testing.Controllers
{
    public class UserControllerTest
    {
        
        [Fact]
        public void GetValidIdShouldReturnOk()
        {
            // Assemble 
            var userIdForTest = 2;
            var mockRepo = new Mock<IGenericRepository>();
            mockRepo.Setup(x => x.GetUserWithFriends(It.IsAny<int>()))
                .ReturnsAsync(new User
                {
                    UserID = userIdForTest,
                    FirstName = "Moo",
                    LastName = "Lah",
                    Username = "Moolah",
                    Email = "Hippy@gmail.com",
                    Gender = 1
                });

            var controller = new UserController(mockRepo.Object);

            // Act
            var result = controller.Get(userIdForTest);
            var asyncResult = result.Result;
            // Asserts
            var viewresult = Assert.IsAssignableFrom<OkObjectResult>(asyncResult);
            Assert.NotNull(viewresult);
        }

        [Fact]
        public void GetInvalidIdShouldReturnNotFound()
        {
            // Assemble 
            var userIdForTest = 2;
            var mockRepo = new Mock<IGenericRepository>();
            mockRepo.Setup(x => x.GetUserWithFriends(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var controller = new UserController(mockRepo.Object);

            // Act
            var result = controller.Get(userIdForTest);
            var asyncResult = result.Result;
            // Asserts
            var viewresult = Assert.IsAssignableFrom<NotFoundResult>(asyncResult);
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


            var mockRepo = new Mock<IGenericRepository>();
            mockRepo.Setup(x => x.AddUser(It.IsAny<User>()))
                .Callback(() => 
                {
                    userList.Add(new User { UserID = 4, FirstName = "abc", LastName = "abc" });
                });

            var controller = new UserController(mockRepo.Object);

            // Act
            var response = controller.Post(newUser);
            var responseContent = response as Task<IActionResult>;

            // Assert
            mockRepo.Verify(x => x.AddUser(It.IsAny<User>()));
            Assert.Equal(4, userList.Count);
            Assert.Equal(4, userList.First(x=>x.UserID==4).UserID);

            Assert.NotNull(responseContent);

        }
        
    }
}
