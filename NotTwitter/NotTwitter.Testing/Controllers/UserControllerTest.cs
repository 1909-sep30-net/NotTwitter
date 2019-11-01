using NotTwitter.API.Controllers;
using NotTwitter.API.Models;
using NotTwitter.Library.Models;
using Moq;
using NotTwitter.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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

            var controller = new UserController(mockRepo.Object);

            // Act
            var result = controller.Get(userIdForTest);

            // Asserts
            var viewresult = Assert.IsAssignableFrom<UserViewModel>(result);
            Assert.Equal(userIdForTest, viewresult.Id);
        }

        [Fact]
        public void PostUserShouldReturnPostUser()
        {
            //Assemble 
            UserViewModel newUser = new UserViewModel() {
                Username = "hithisistest",
                FirstName = "HiThis",
                LastName = "IsTest",
                Gender = 1,
                Email = "hithisistest@test.com",
                Id = 2,
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.AddUser(It.IsAny<User>()));

            var controller = new UserController(mockRepo.Object);

            //Act
            var result = controller.Post(newUser);
            //Assert
            var viewResult = controller.Get(2);
            Assert.Equal(viewResult.Username, newUser.Username);
        }

    }
}
