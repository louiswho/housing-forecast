using Housing.Forecast.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Housing.Forecast.Testing.Context
{
    public class AddressTests
    {
        [Fact]
        public void DefaultAddressInvalidTest()
        {
            Address test = new Address();
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressValidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            Assert.True(test.Validate());
        }

        [Fact]
        public void addressIdInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.Id = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressAddressIdInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.AddressId = Guid.Empty;
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressAddress1InvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.Address1 = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressCityInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.City = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressStateInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.State = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressPostalCodeInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.PostalCode = "";
            Assert.False(test.Validate());
        }

        [Fact]
        public void addressCountryInvalidTest()
        {
            Address test = TestDataGenerator.getTestAddress();
            test.Country = "";
            Assert.False(test.Validate());
        }
    }
}
