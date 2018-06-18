using Housing.Forecast.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Housing.Forecast.Testing.Context
{
    public class RoomTests
    {
        [Fact]
        public void DefaultRoomInvalidTest()
        {
            Room test = new Room();
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomValidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            Assert.True(test.Validate());
        }

        [Fact]
        public void RoomIdInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Id = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomRoomIdInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.RoomId = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomLocationNullInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Location = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomLocatioEmptyInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Location = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomGenderNullInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Gender = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomGenderEmptyInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Gender = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomOccupancyInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Occupancy = -1;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomVacancyInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Vacancy = -1;
            Assert.False(test.Validate());
        }

        [Fact]
        public void RoomAddressInvalidTest()
        {
            Room test = TestDataGenerator.getTestRoom();
            test.Address = null;
            Assert.False(test.Validate());
        }
    }
}
