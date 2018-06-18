using Housing.Forecast.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Housing.Forecast.Testing.Context
{
    public class BatchTests
    {
        [Fact]
        public void DefaultBatchInvalidTest()
        {
            Batch test = new Batch();
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchValidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            Assert.True(test.Validate());
        }

        [Fact]
        public void BatchIdInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.Id = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchBatchIdInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.BatchId = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchNameInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.BatchName = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchOccupancyLessInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.BatchOccupancy = -1;
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchOccupancyOverInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.BatchOccupancy = 101;
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchSkillInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.BatchSkill = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void BatchStateInvalidTest()
        {
            Batch test = TestDataGenerator.getTestBatch();
            test.State = null;
            Assert.False(test.Validate());
        }
    }
}
