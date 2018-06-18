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
    public class SnapshotRepoTests
    {
        private ForecastContext _context;
        private ISnapshotRepo _snapshotRepository;
        private static readonly DbContextOptions<ForecastContext> options = new DbContextOptionsBuilder<ForecastContext>()
            .UseInMemoryDatabase(databaseName: "InMemDb")
            .Options;

        [Fact]
        public void Get_ReturnCollection()
        {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();

                // Act
                snapshots = _snapshotRepository.Get();

                // Assert
                Assert.NotEmpty(snapshots);
            }
        }

        [Fact]
        public void GetByDate_WithValidDate_ReturnNonEmptyCollection()
        {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByDate(snapshot.Date);

                // Assert
                Assert.Equal(snapshots.FirstOrDefault().Id, snapshot.Id);
            }
        }

        [Fact]
        public void GetByDate_WithInvalidDate_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByDate(DateTime.MinValue);

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public void GetLocations_ReturnCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<String> locations;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());

                Snapshot snapshot = getTestSnapshot();
                snapshot.Location = "Tampa";
                _context.Snapshots.Add(snapshot);

                _context.SaveChanges();

                // Act
                locations = _snapshotRepository.GetLocations();

                // Assert
                Assert.Equal(2, locations.Count());
            }
        }

        [Fact]
        public void GetByLocation_ValidDateValidLocation_ReturnNonEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByLocation(snapshot.Date, snapshot.Location);

                // Assert
                Assert.NotEmpty(snapshots);
            }
        }

        [Fact]
        public void GetByLocation_InvalidDateValidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByLocation(DateTime.MinValue, snapshot.Location);

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public void GetByLocation_ValidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByLocation(snapshot.Date, "Tampa");

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public void GetByLocation_InvalidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = _snapshotRepository.GetByLocation(DateTime.MinValue, "Tampa");

                // Assert
                Assert.Empty(snapshots);
            }
        }

        private void init(ForecastContext context) {
            if(context.Snapshots.Count() > 0)
                context.Snapshots.RemoveRange(context.Snapshots);
            context.SaveChanges();
        }

        private Snapshot getTestSnapshot() {
            Snapshot result = new Snapshot{
                Id = Guid.NewGuid(),
                Date = new DateTime(2018, 1, 1),
                Location = "Reston",
                RoomOccupancyCount = 1,
                UserCount = 1
            };
            return result;
        }
    }
}
