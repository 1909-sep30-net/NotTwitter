using API.Controllers;
using API.Models;
using Library.Interfaces;
using Library.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Testing.Controllers
{
    public class UserControllerTest
    {
        /* Commenting out for now due to the actual controller method not being implemented yet
        [Fact]
        public void GetUserByIdShouldReturnUser()
        {
            // Assemble 
            var userIdForTest = 2;
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetUserByID(It.IsAny<int>()))
                .Returns(new Library.Models.User
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
        */
    }
}
