using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace Housing.Forecast.Context.Repos
{
    public class UserRepo : IRepo<User>
    {
        private readonly IForecastContext _context;
        public UserRepo(IForecastContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Find all of the distinct locations for every snapshot.
        /// </summary>
        /// <returns>
        /// This method will return a list of all distinct locations for the snapshots within the database.
        /// </returns>
        public async Task<IList<string>> GetLocationsAsync()
        {
            return await _context.Users.Select(s => s.Location).Where(s => s != null).Distinct().ToListAsync();
        }

        public async Task<IList<User>> GetAsync()
        {
            return await _context.Users.Where(s => s != null).ToListAsync();
        }

        public async Task<IList<User>> GetByLocationAsync(DateTime datetime, string location)
        {
            return await _context.Users.Where(r =>
                r.Created.Date <= datetime.Date && (r.Deleted.Value == null || r.Deleted.Value.Date > datetime.Date) &&
                r.Location == location).ToListAsync();
        }

        public async Task<IList<User>> GetByDateAsync(DateTime datetime)
        {
            return await _context.Users.Where(u =>
                    u.Created.Date <= datetime.Date &&
                    (u.Deleted.Value == null || u.Deleted.Value.Date > datetime.Date))
                .ToListAsync();
        }
    }
}
