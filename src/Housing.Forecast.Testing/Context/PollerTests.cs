﻿using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;
using Xunit.Abstractions;

namespace Housing.Forecast.Testing.Context
{
    public class PollerTests
    {
        private ForecastContext _context;
        private static readonly DbContextOptions<ForecastContext> options = new DbContextOptionsBuilder<ForecastContext>()
            .UseInMemoryDatabase(databaseName: "InMemDb")
            .Options;
        private readonly ITestOutputHelper _output;

        public PollerTests(ITestOutputHelper output)
        {
            this._output = output;
        }
        
        public Name getNewName()
        {
            Name New = new Name();
            New.First = "testFirst";
            New.Last = "testLast";
            New.Middle = "testMiddle";
            New.NameId = Guid.NewGuid();

            return New;
        }

        public Address getNewAddress()
        {
            Address New = new Address();
            New.Address1 = "testAddress1";
            New.Address2 = "testAddress2";
            New.City = "testCity";
            New.Country = "testCountry";
            New.PostalCode = "testPostal";
            New.State = "TX";
            New.AddressId = Guid.NewGuid();

            return New;
        }
        public Room getNewRoom()
        {
            Room New = new Room();
            New.RoomId = Guid.NewGuid();
            New.Address = getNewAddress();
            New.Gender = "M";
            New.Location = "Tampa";
            New.Occupancy = 10;
            New.Vacancy = 10;

            return New;
        }

        [Fact]
        public void UpdateNewName()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Name insertTest = getNewName();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModFirstName()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Name insertTest = getNewName();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                insertTest.First = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModMiddleName()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Name insertTest = getNewName();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                insertTest.Middle = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModLastName()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Name insertTest = getNewName();
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                insertTest.Last = "testChanged";
                testPoller.UpdateName(insertTest);
                _context.SaveChanges();
                Name afterInsertTest = _context.Names.Where(p => p.NameId == insertTest.NameId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateNameNoChange()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Name insertTest = getNewName();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
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
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Address insertTest = getNewAddress();
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                insertTest.State = "changed";
                testPoller.UpdateAddress(insertTest);
                _context.SaveChanges();
                Address afterInsertTest = _context.Addresses.Where(p => p.AddressId == insertTest.AddressId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }

        public Batch getNewBatch()
        {
            Batch New = new Batch();
            New.BatchName = "testName";
            New.BatchOccupancy = 3;
            New.BatchSkill = "testSkill";
            New.State = "VA";
            New.BatchId = Guid.NewGuid();

            return New;
        }


        [Fact]
        public void UpdateNewBatch()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                ICollection<Batch> Batches = new List<Batch>();
                Batch insertTest = getNewBatch();
                Batches.Add(insertTest);
                testPoller.UpdateBatches(Batches);
                Batch afterInsertTest = _context.Batches.Where(p => p.BatchId == insertTest.BatchId).FirstOrDefault();
                Assert.Equal(insertTest, afterInsertTest);
            }
        }


        [Fact]
        public void UpdateNewRoom()
        {
            using(_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Room insertRoom = getNewRoom();
                _output.WriteLine(insertRoom.RoomId.ToString());
                ICollection<Room> rooms = new List<Room>();
                rooms.Add(insertRoom);
                testPoller.UpdateRooms(rooms);
                Room afterInsertTest = _context.Rooms.Where(p => p.RoomId == insertRoom.RoomId).FirstOrDefault();
                Assert.Equal(insertRoom, afterInsertTest);
            }
        }

        [Fact]
        public void UpdateModRoomVacancy()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Room insertRoom = getNewRoom();
                _output.WriteLine(insertRoom.RoomId.ToString());
                ICollection<Room> rooms = new List<Room>();
                rooms.Add(insertRoom);
                testPoller.UpdateRooms(rooms);
                insertRoom.Vacancy = 5;
                testPoller.UpdateRooms(rooms);
                Room afterInsertTest = _context.Rooms.Where(p => p.RoomId == insertRoom.RoomId).FirstOrDefault();
                Assert.Equal(insertRoom, afterInsertTest);
            }
        }

        [Fact]
        public void DeleteRoom()
        {
            using (_context = new ForecastContext(options))
            {
                Poller testPoller = new Poller(_context, TimeSpan.MinValue);
                Room insertRoom = getNewRoom();
                _output.WriteLine(insertRoom.RoomId.ToString());
                ICollection<Room> rooms = new List<Room>();
                rooms.Add(insertRoom);
                testPoller.UpdateRooms(rooms);
                rooms.Remove(insertRoom);
                testPoller.UpdateRooms(rooms);
                var actual = _context.Rooms.Where(p => p.RoomId == insertRoom.RoomId).FirstOrDefault().Deleted;
                var expected = DateTime.Today;
                Assert.Equal(expected, actual);
            }
        }
    }
}
