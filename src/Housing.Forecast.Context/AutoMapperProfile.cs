using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Housing.Forecast.Context
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Library.Models.Address, Models.Address>();
            CreateMap<Library.Models.User, Models.User>();
            CreateMap<Library.Models.Room, Models.Room>();
            CreateMap<Library.Models.Name, Models.Name>();
            CreateMap<Library.Models.Batch, Models.Batch>();
            CreateMap<Library.Models.Snapshot, Models.Snapshot>();
        }
    }
}
