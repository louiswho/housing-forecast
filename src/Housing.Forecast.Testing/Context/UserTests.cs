using Housing.Forecast.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Housing.Forecast.Testing.Context
{
    public class UserTests
    {
        [Fact]
        public void DefaultUserInvalidTest()
        {
            User test = new User();
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserValidTest()
        {
            User test = TestDataGenerator.getTestUser();
            Assert.True(test.Validate());
        }

        [Fact]
        public void UserIdInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Id = Guid.Empty;
            Assert.False(test.Validate());
        }
        [Fact]
        public void UserUserIdInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.UserId = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserGenderNullInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Gender = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserGenderEmptyInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Gender = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserLocationNullInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Location = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserLocationEmptyInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Location = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserNameInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Name = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserBatchInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Batch = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserEmailNullInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Email = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserEmailEmptyInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Email = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserTypeNullInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Type = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void UserTypeEmptyInvalidTest()
        {
            User test = TestDataGenerator.getTestUser();
            test.Type = "";
            Assert.False(test.Validate());
        }
    }
}
