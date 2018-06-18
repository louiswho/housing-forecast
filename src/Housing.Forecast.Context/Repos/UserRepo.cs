using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Context.Repos
{
    public class UserRepo : IRepo<User>
    {
        private readonly IForecastContext _context;
        public UserRepo(IForecastContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetLocations()
        {
            return _context.Users.Select(r => r.Location).Where(r => r != null).Distinct();
        }

        public IEnumerable<User> Get()
        {
            return _context.Users;
        }

        public IEnumerable<User> GetByLocation(DateTime datetime, string location)
        {
            return _context.Users.Where(r => r.Created.Date <= datetime.Date && (r.Deleted.Value == null || r.Deleted.Value.Date > datetime.Date) && r.Location == location);
        }

        public IEnumerable<User> GetByDate(DateTime datetime)
        {
            return _context.Users.Where(u => u.Created.Date <= datetime.Date && (u.Deleted.Value == null || u.Deleted.Value.Date > datetime.Date));
        }
    }
}
