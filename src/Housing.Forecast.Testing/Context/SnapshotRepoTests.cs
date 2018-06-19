using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Get_ReturnCollection()
        {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();

                // Act
                snapshots = await _snapshotRepository.GetAsync();

                // Assert
                Assert.NotEmpty(snapshots);
            }
        }

        [Fact]
        public async Task GetByDate_WithValidDate_ReturnNonEmptyCollection()
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
                snapshots = await _snapshotRepository.GetByDateAsync(snapshot.Date);

                // Assert
                Assert.Equal(snapshots.FirstOrDefault().Id, snapshot.Id);
            }
        }

        [Fact]
        public async Task GetByDate_WithInvalidDate_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = await _snapshotRepository.GetByDateAsync(DateTime.MinValue);

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public async Task GetLocations_ReturnCollection() {
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
                locations = await _snapshotRepository.GetLocationsAsync();

                // Assert
                Assert.Equal(2, locations.Count());
            }
        }

        [Fact]
        public async Task GetByLocation_ValidDateValidLocation_ReturnNonEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = await _snapshotRepository.GetByLocationAsync(snapshot.Date, snapshot.Location);

                // Assert
                Assert.NotEmpty(snapshots);
            }
        }

        [Fact]
        public async Task GetByLocation_InvalidDateValidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = await _snapshotRepository.GetByLocationAsync(DateTime.MinValue, snapshot.Location);

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public async Task GetByLocation_ValidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = await _snapshotRepository.GetByLocationAsync(snapshot.Date, "Tampa");

                // Assert
                Assert.Empty(snapshots);
            }
        }

        [Fact]
        public async Task GetByLocation_InvalidDateInvalidLocation_ReturnEmptyCollection() {
            using(_context = new ForecastContext(options)) {
                // Arrange
                init(_context);
                IEnumerable<Snapshot> snapshots;
                _snapshotRepository = new SnapshotRepo(_context);
                _context.Snapshots.Add(getTestSnapshot());
                _context.SaveChanges();
                Snapshot snapshot = _context.Snapshots.FirstOrDefault();

                // Act
                snapshots = await _snapshotRepository.GetByLocationAsync(DateTime.MinValue, "Tampa");

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
