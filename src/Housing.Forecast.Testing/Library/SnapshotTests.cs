using System;
using Xunit;
using Housing.Forecast.Library.Models;
using Housing.Forecast.Service;

namespace Housing.Forecast.Testing.Library
{
  public class SnapshotTests
  {
    [Fact]
    public void DefaultSnapshotInvalidTest() {
      // Arrange
      // Act
      Snapshot snap = new Snapshot();

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotValidTest() {
      // Arrange
      // Act
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Assert
      Assert.True(snap.Validate());
    }

    [Fact]
    public void SnapshotIdInvalidTest() {
      // Arrange
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Act
      snap.Id = Guid.Empty;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotDateInvalidTest() {
      // Arrange
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Act
      snap.Date = DateTime.MinValue;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotLocationInvalidTest() {
      // Arrange
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Act
      snap.Location = null;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotUserCountInvalidTest() {
      // Arrange
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Act
      snap.UserCount = -1;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotRoomCountInvalidTest() {
      // Arrange
      Snapshot snap = TestDataGenerator.getTestSnapshot();

      // Act
      snap.RoomOccupancyCount = -1;

      // Assert
      Assert.False(snap.Validate());
    }
  }
}
