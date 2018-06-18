using Housing.Forecast.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AutoMapper;

namespace Housing.Forecast.Testing.Context
{
    public class ModelMapperTests
    {
        [Fact]
        public void ModelMapperTest()
        {
            Mapper.Initialize(x => x.AddProfile(new AutoMapperProfile()));
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
