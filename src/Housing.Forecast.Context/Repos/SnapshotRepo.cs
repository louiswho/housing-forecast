using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;
using Microsoft.EntityFrameworkCore;
using l = Housing.Forecast.Library.Models;

namespace Housing.Forecast.Context.Repos
{
    public class SnapshotRepo : ISnapshotRepo
    {
        private readonly IForecastContext _context;
        public SnapshotRepo(IForecastContext context)
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
            return await _context.Snapshots.Select(s => s.Location).Where(s => s != null).Distinct().ToListAsync();
        }

        /// <summary>
        /// Find all of the snapshots within the database.
        /// </summary>
        /// <returns>
        /// This method will return a list of the all snapshots that are stored within the database.
        /// </returns>
        public async Task<IList<Snapshot>> GetAsync()
        {
            return await _context.Snapshots.Where(s => s != null).ToListAsync();
        }

        /// <summary>
        /// Find all snapshots that over the specified range of dates.
        /// </summary>
        /// <param name="Start">The starting point of the date range for the snapshots.</param>
        /// <param name="End">The ending pointing of the date range for the snapshots.</param>
        /// <returns>
        /// This method will return a list of snapshots that are within the specified range of dates.
        /// </returns>
        public async Task<IList<Snapshot>> GetBetweenDatesAsync(DateTime Start, DateTime End)
        {
            return await _context.Snapshots.Where(s => s.Date.Date >= Start.Date && s.Date.Date <= End.Date).ToListAsync();
        }

        /// <summary>
        /// Find all of the snapshots for the specified location that were created on the provided date.
        /// </summary>
        /// <param name="datetime">The date the snapshot should have been created on.</param>
        /// <param name="location">The location the snapshot should be for.</param>
        /// <returns>
        /// This method should a list of all snapshots for the specified location that were craeted on the provided date that are stored in the database.
        /// </returns>
        public async Task<IList<Snapshot>> GetByLocationAsync(DateTime datetime, string location)
        {
            return await _context.Snapshots.Where(s => s.Date.Date == datetime.Date && s.Location.Equals(location)).ToListAsync();
        }

        /// <summary>
        /// Find all of the snapshots for the specified location that fall within the provided date range.
        /// </summary>
        /// <param name="Start">The starting point of the date range for the snapshots.</param>
        /// <param name="End">The ending point of the date range for the snapshots.</param>
        /// <param name="location">The location the snapshots should be tied to.</param>
        /// <returns>
        /// This method should return a list of all snapshots that fall within the date range and tied to the location provided.
        /// </returns>
        public async Task<IList<Snapshot>> GetBetweenDatesAtLocationAsync(DateTime Start, DateTime End, string location)
        {
            return await _context.Snapshots.Where(s => s.Date.Date >= Start.Date && s.Date.Date <= End.Date && s.Location.Equals(location)).ToListAsync();
        }

        /// <summary>
        /// Find all snapshots that were created on the specified date.
        /// </summary>
        /// <param name="datetime">The date the snapshot should be created on.</param>
        /// <returns>
        /// This method should return a list of all snapshots within the datebase that were created on the specified date.
        /// </returns>
        public async Task<IList<Snapshot>> GetByDateAsync(DateTime datetime)
        {
            return await _context.Snapshots.Where(s => s.Date.Date == datetime.Date).ToListAsync();
        }

        /// <summary>
        /// Convert a list of Context's Snapshot objects to Library's Snapshot objects
        /// </summary>
        /// <param name="snapshots">List of Snapshots that need to be mapped.</param>
        /// <returns>
        /// Returns a new of the mapped Snapshots.
        /// </returns>
        private IEnumerable<l.Snapshot> Map(IEnumerable<Snapshot> snapshots)
        {
            var snaps = new List<l.Snapshot>();
            foreach (var snap in snapshots)
            {
                snaps.Add(Map(snap));
            }

            return snaps;
        }


        /// <summary>
        /// Add the newly created snapshots to the database
        /// </summary>
        /// <param name="snapshots">Snapshots that need to be added to the database.</param>
        /// <returns>
        /// Returns true if the snapshots are added successfully else return false.
        /// </returns>
        public async Task AddSnapshotsAsync(IEnumerable<Snapshot> snapshots)
        {
            foreach (var snap in snapshots)
            {
                _context.Snapshots.Add(snap);
            }
            await _context.SaveChangesAsync(default (CancellationToken));
        }

        /// <summary>
        /// Map the Context's Snapshot model to Library's Snapshot model
        /// </summary>
        /// <param name="snapshot">Snapshot that needs to be mapped.</param>
        /// <returns>
        /// Return the mapped Snapshot model.
        /// </returns>
        private l.Snapshot Map(Snapshot snapshot)
        {
            var snap = new l.Snapshot()
            {
                Id = Guid.Empty,
                Date = snapshot.Date,
                Created = snapshot.Created,
                Location = snapshot.Location,
                RoomOccupancyCount = snapshot.RoomOccupancyCount,
                UserCount = snapshot.UserCount
            };

            return snap;
        }
    }
}
