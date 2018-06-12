using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Housing.Forecast.Library.Models;
using Housing.Forecast.Service;

namespace Housing.Forecast.Testing.Library
{
  public class SnapshotTest
  {
    private Snapshot getTestSnapshot() {
      Snapshot result = new Snapshot{
          Id = Guid.NewGuid(),
          Date = new DateTime(2018, 1, 1),
          Location = "Reston",
          RoomCount = 1,
          UserCount = 1
      };
      return result;
    }

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
      Snapshot snap = getTestSnapshot();

      // Assert
      Assert.True(snap.Validate());
    }

    [Fact]
    public void SnapshotIdInvalidTest() {
      // Arrange
      Snapshot snap = getTestSnapshot();

      // Act
      snap.Id = Guid.Empty;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotDateInvalidTest() {
      // Arrange
      Snapshot snap = getTestSnapshot();

      // Act
      snap.Date = null;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotLocationInvalidTest() {
      // Arrange
      Snapshot snap = getTestSnapshot();

      // Act
      snap.Location = null;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotUserCountInvalidTest() {
      // Arrange
      Snapshot snap = getTestSnapshot();

      // Act
      snap.UserCount = null;

      // Assert
      Assert.False(snap.Validate());
    }

    [Fact]
    public void SnapshotRoomCountInvalidTest() {
      // Arrange
      Snapshot snap = getTestSnapshot();

      // Act
      snap.RoomCount = null;

      // Assert
      Assert.False(snap.Validate());
    }
  }
}
