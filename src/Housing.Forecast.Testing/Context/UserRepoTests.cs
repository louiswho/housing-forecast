using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Housing.Forecast.Context;
using Housing.Forecast.Context.Repos;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Testing.Context
{
    public class UserRepoTests
    {
        private ForecastContext _context;
        private IRepo<User> _userRepository;
        private static readonly DbContextOptions<ForecastContext> options = new DbContextOptionsBuilder<ForecastContext>()
            .UseInMemoryDatabase(databaseName: "InMemDb")
            .Options;

        [Fact]
        public void Get_ReturnCollection()
        {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();

                // Act
                users = _userRepository.Get();

                // Assert
                Assert.NotEmpty(users);
            }
        }

        [Fact]
        public void GetByDate_WithValidDate_ReturnNonEmptyCollection()
        {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByDate(user.Created);

                // Assert
                Assert.Equal(users.FirstOrDefault().Id, user.Id);
            }
        }

        [Fact]
        public void GetByDate_WithInvalidDate_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByDate(DateTime.MinValue);

                // Assert
                Assert.Empty(users);
            }
        }

        [Fact]
        public void GetLocations_ReturnCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<String> locations;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());

                User user = getTestUser();
                user.Location = "Tampa";
                _context.Users.Add(user);

                _context.SaveChanges();

                // Act
                locations = _userRepository.GetLocations();

                // Assert
                Assert.Equal(2, locations.Count());
            }
        }

        [Fact]
        public void GetByLocations_ValidDateValidLocation_ReturnNonEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByLocation(user.Created, user.Location);

                // Assert
                Assert.NotEmpty(users);
            }
        }

        [Fact]
        public void GetByLocations_InvalidDateValidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByLocation(DateTime.MinValue, user.Location);

                // Assert
                Assert.Empty(users);
            }
        }

        [Fact]
        public void GetByLocations_ValidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByLocation(user.Created, "Tampa");

                // Assert
                Assert.Empty(users);
            }
        }

        [Fact]
        public void GetByLocations_InvalidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<User> users;
                _userRepository = new UserRepo(_context);
                _context.Users.Add(getTestUser());
                _context.SaveChanges();
                User user = _context.Users.FirstOrDefault();

                // Act
                users = _userRepository.GetByLocation(DateTime.MinValue, "Tampa");

                // Assert
                Assert.Empty(users);
            }
        }

        private void init(ForecastContext context) {
            if(context.Users.Count() > 0)
                context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }

        private User getTestUser() {
            User result = new User{
                Name = new Name {
                    NameId = Guid.NewGuid(),
                    First = "first",
                    Last = "last"
                },
                Id = Guid.NewGuid(),
                Location = "Reston",
                Email = "test@test.com",
                Gender = "M",
                Type = "test",
                UserId = Guid.NewGuid(),
                Batch = new Batch {
                    Id = Guid.NewGuid(),
                    BatchId = Guid.NewGuid(),
                    StartDate = new DateTime(2018, 1, 1),
                    EndDate = new DateTime(2018, 5, 1),
                    BatchName = "test batch",
                    BatchOccupancy = 1,
                    BatchSkill = "test skill",
                    State = "VA",
                    Deleted = DateTime.MaxValue
                },
                Deleted = DateTime.MaxValue
            };
            return result;
        }
    }
}
