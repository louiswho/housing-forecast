using Housing.Forecast.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Housing.Forecast.Testing.Context
{
    public class NameTests
    {
        [Fact]
        public void DefaultNameInvalidTest()
        {
            Name test = new Name();
            Assert.False(test.Validate());
        }

        [Fact]
        public void NameValidTest()
        {
            Name test = TestDataGenerator.getTestName();
            Assert.True(test.Validate());
        }

        [Fact]
        public void NameIdInvalidTest()
        {
            Name test = new Name();
            test.Id = Guid.Empty;
            Assert.False(test.Validate());
        }
        [Fact]
        public void NameNameIdInvalidTest()
        {
            Name test = new Name();
            test.NameId = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void NameFirstNullInvalidTest()
        {
            Name test = new Name();
            test.First = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void NameFirstEmptyInvalidTest()
        {
            Name test = new Name();
            test.First = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void NameLastNullInvalidTest()
        {
            Name test = new Name();
            test.Last = null;
            Assert.False(test.Validate());
        }

        [Fact]
        public void NameLastEmptyInvalidTest()
        {
            Name test = new Name();
            test.Last = "";
            Assert.False(test.Validate());
        }
    }
}
