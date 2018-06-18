using System;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Testing.Context {
    public static class TestDataGenerator {
        public static Address getTestAddress()
        {
            Address address = new Address
            {
                Id = Guid.NewGuid(),
                AddressId = Guid.NewGuid(),
                Address1 = "123 test street",
                City = "Tampa",
                State = "Florida",
                PostalCode = "33617",
                Country = "US"
            };
            return address;
        }
        public static Batch getTestBatch()
        {
            Batch batch = new Batch
            {
                Id = Guid.NewGuid(),
                BatchId = Guid.NewGuid(),
                BatchName = "name",
                BatchOccupancy = 1,
                BatchSkill = ".Net",
                State = "VA"
            };
            return batch;
        }

        public static Name getTestName()
        {
            Name result = new Name
            {
                Id = Guid.NewGuid(),
                NameId = Guid.NewGuid(),
                First = "first",
                Middle = "middle",
                Last = "last",
                Created = DateTime.Now,
            };
            return result;
        }

        public static Room getTestRoom()
        {
            Room result = new Room
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Location = "Reston",
                Vacancy = 1,
                Occupancy = 1,
                Gender = "F",
                Address = getTestAddress(),
                Created = DateTime.Now
            };
            return result;
        }
        public static Snapshot getTestSnapshot()
        {
            Snapshot result = new Snapshot
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2018, 1, 1),
                Location = "Reston",
                RoomOccupancyCount = 1,
                UserCount = 1
            };
            return result;
        }
        public static User getTestUser()
        {
            User result = new User
            {
                Name = getTestName(),
                Batch = getTestBatch(),
                Address = getTestAddress(),
                Id = Guid.NewGuid(),
                Location = "Reston",
                Email = "test@test.com",
                Gender = "M",
                Type = "test",
                UserId = Guid.NewGuid(),
                Created = DateTime.Now
            };
            return result;
        }


    }
}