using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;
using Xunit.Abstractions;
using System.Net.Http;
using Housing.Forecast.Context.ApiAccessors;

namespace Housing.Forecast.Testing.Context
{
    public class PollerTests
    {
        private ForecastContext _context;
        private static readonly DbContextOptions<ForecastContext> options = new DbContextOptionsBuilder<ForecastContext>()
            .UseInMemoryDatabase(databaseName: "InMemDb")
            .Options;
        private readonly ITestOutputHelper _output;
        private HttpClient testClient = new HttpClient();
        private ApiMethods testApi;

        public PollerTests(ITestOutputHelper output)
        {
            this._output = output;
            testApi = new ApiMethods(testClient);
        }

        [Fact]
        public void UpdateNewName()
        {
            using (_context = new ForecastContext(options))
            {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Name insertTest = TestDataGenerator.getTestName();

                // Act
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();

                // Assert
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModFirstName()
        {
            using (_context = new ForecastContext(options))
            {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Name insertTest = TestDataGenerator.getTestName();
                _context.Names.Add(insertTest);
                _context.SaveChanges();

                // Act
                insertTest.First = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();

                // Assert
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModMiddleName()
        {
            using (_context = new ForecastContext(options))
            {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Name insertTest = TestDataGenerator.getTestName();
                _context.Names.Add(insertTest);
                _context.SaveChanges();

                // Act
                insertTest.Middle = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();

                // Assert
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModLastName()
        {
            using (_context = new ForecastContext(options))
            {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Name insertTest = TestDataGenerator.getTestName();
                _context.Names.Add(insertTest);
                _context.SaveChanges();

                // Act
                insertTest.Last = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();

                // Assert
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateNewUsers() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                ICollection<User> list = new List<User>();

                // Act
                list.Add(TestDataGenerator.getTestUser());
                testPoller.UpdateUsers(list);

                // Assert
                list = _context.Users.ToList();
                Assert.NotEmpty(list);
            }
        }

        [Fact]
        public void UpdateModUsers() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                User user = TestDataGenerator.getTestUser();
                string oldLocation = user.Location;
                _context.Users.Add(user);
                _context.SaveChanges();

                // Act
                string newLocation = "Tampa";
                User newUser = new User {
                    Name = user.Name,
                    Batch = user.Batch,
                    Address = user.Address,
                    Id = user.Id,
                    Location = newLocation,
                    Email = user.Email,
                    Gender = user.Gender,
                    Type = user.Type,
                    UserId = user.UserId,
                    Created = user.Created,
                    Deleted = user.Deleted
                };
                ICollection<User> list = new List<User>
                {
                    newUser
                };
                testPoller.UpdateUsers(list);

                // Assert
                User updatedUser = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                Assert.True(!updatedUser.Location.Equals(oldLocation) && updatedUser.Location.Equals(newLocation));
            }
        }

        [Fact]
        public void UpdateDeleteUsers() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                User user = TestDataGenerator.getTestUser();
                _context.Users.Add(user);
                _context.SaveChanges();

                // Act
                testPoller.UpdateUsers(new List<User>());

                // Assert
                User updatedUser = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                Assert.NotNull(updatedUser.Deleted);
            }
        }


        [Fact]
        public void UpdateNewBatches() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                ICollection<Batch> list = new List<Batch>();

                // Act
                list.Add(TestDataGenerator.getTestBatch());
                testPoller.UpdateBatches(list);

                // Assert
                list = _context.Batches.ToList();
                Assert.NotEmpty(list);
            }
        }

        [Fact]
        public void UpdateModBatches() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Batch batch = TestDataGenerator.getTestBatch();
                string oldState = batch.State;
                _context.Batches.Add(batch);
                _context.SaveChanges();

                // Act
                string newState = "FL";
                Batch newBatch = new Batch {
                    Id = batch.Id,
                    BatchOccupancy = batch.BatchOccupancy,
                    BatchId = batch.BatchId,
                    BatchName = batch.BatchName,
                    BatchSkill = batch.BatchSkill,
                    State = newState,
                    StartDate = batch.StartDate,
                    EndDate = batch.EndDate,
                    Created = batch.Created,
                    Deleted = batch.Deleted
                };
                ICollection<Batch> list = new List<Batch>
                {
                    newBatch
                };
                testPoller.UpdateBatches(list);

                // Assert
                Batch updatedBatch = _context.Batches.Where(x => x.BatchId == batch.BatchId).FirstOrDefault();
                Assert.True(!updatedBatch.State.Equals(oldState) && updatedBatch.State.Equals(newState));
            }
        }

        [Fact]
        public void UpdateDeleteBatches() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Batch batch = TestDataGenerator.getTestBatch();
                _context.Batches.Add(batch);
                _context.SaveChanges();

                // Act
                testPoller.UpdateBatches(new List<Batch>());

                // Assert
                Batch updatedBatch = _context.Batches.Where(x => x.BatchId == batch.BatchId).FirstOrDefault();
                Assert.NotNull(updatedBatch.Deleted);
            }
        }

        [Fact]
        public void UpdateNewRooms() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                ICollection<Room> list = new List<Room>();

                // Act
                list.Add(TestDataGenerator.getTestRoom());
                testPoller.UpdateRooms(list);

                // Assert
                list = _context.Rooms.ToList();
                Assert.NotEmpty(list);
            }
        }

        [Fact]
        public void UpdateModRooms() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Room room = TestDataGenerator.getTestRoom();
                string oldLocation = room.Location;
                _context.Rooms.Add(room);
                _context.SaveChanges();

                // Act
                string newLocation = "Tampa";
                Room newRoom = new Room {
                    Address = room.Address,
                    Id = room.Id,
                    Location = newLocation,
                    Gender = room.Gender,
                    Occupancy = room.Occupancy,
                    RoomId = room.RoomId,
                    Vacancy = room.Vacancy,
                    Created = room.Created,
                    Deleted = room.Deleted
                };
                ICollection<Room> list = new List<Room>
                {
                    newRoom
                };
                testPoller.UpdateRooms(list);

                // Assert
                Room updatedRoom = _context.Rooms.Where(x => x.RoomId == room.RoomId).FirstOrDefault();
                Assert.True(!updatedRoom.Location.Equals(oldLocation) && updatedRoom.Location.Equals(newLocation));
            }
        }

        [Fact]
        public void UpdateDeleteRooms() {
            using (_context = new ForecastContext(options)) {
                // Arrange
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Room room = TestDataGenerator.getTestRoom();
                _context.Rooms.Add(room);
                _context.SaveChanges();

                // Act
                testPoller.UpdateRooms(new List<Room>());

                // Assert
                Room updatedRoom = _context.Rooms.Where(x => x.RoomId == room.RoomId).FirstOrDefault();
                Assert.NotNull(updatedRoom.Deleted);
            }
        }

        [Fact]
        public void UpdateNameNoChange()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Name insertTest = TestDataGenerator.getTestName();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateNewAddress()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddress1()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.Address1 = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddress2()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.Address2 = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddressCity()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.City = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddressCountry()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.Country = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddressPostalCode()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.PostalCode = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModAddressState()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                Address insertTest = TestDataGenerator.getTestAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.State = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateNewBatch()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue, testApi);
                ICollection<Batch> Batches = new List<Batch>();
                Batch insertTest = TestDataGenerator.getTestBatch();
                Batches.Add(insertTest);
                testPoller.UpdateBatches(Batches);
                Batch afterInsertTest = _context.Batches.Where(p => p.BatchId == insertTest.BatchId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }
    }
}
