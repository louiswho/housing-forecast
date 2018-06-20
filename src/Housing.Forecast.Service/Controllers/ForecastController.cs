using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Service.Controllers
{
    [Route("api/[controller]")]
    public class ForecastController : BaseController
    {
        private readonly ISnapshotRepo _snapshot;
        private readonly IRepo<Room> _room;
        private readonly IRepo<User> _user;
        public ForecastController(ILoggerFactory loggerFactory, 
            ISnapshotRepo snapshot, 
            IRepo<Room> rooms, 
            IRepo<User> users)
          : base(loggerFactory)
        {
            _snapshot = snapshot;
            _room = rooms;
            _user = users;
        }

        /// <summary>
        /// This endpoint will return all unique locations of snapshots
        /// </summary>
        /// <return>
        /// Return a list of all unique locations of snapshots.
        /// </return>
        // GET: api/forecast/Locations
        [HttpGet("Locations")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                //List<string> locations = _room.GetLocations().ToList();
                IList<string> locations = await _room.GetLocationsAsync();
                if (locations == null || locations.Count == 0)
                {
                    return NotFound("No locations found."); // No snapshots found in the DB.
                }
                return Ok(locations); // Return all of the distinct locations the snapshots are tied to.
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message); // Log the error
                return BadRequest("Something went wrong while processing the request.");
            }
        }

        /// <summary>
        /// This endpoint will return all of the snapshots to the caller.
        /// </summary>
        /// <return>
        /// Return all of the snapshots in the database.
        /// </return>
        // GET: api/forecast/Snapshots
        [Route("Snapshots")]
        [HttpGet]
        public async Task<IActionResult> Get() // .NET Core doesn't have IHttpActionResult instead we use IActionResult
        {
            try
            {
                IList<Snapshot> snapshots = await _snapshot.GetAsync();

                if (snapshots == null || snapshots.Count == 0)
                {
                    return NotFound("There are no snapshots in the database.");
                }

                return Ok(snapshots);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest("Error occurred while processing request.");
            }
        }

        /// <summary>
        /// Return the snapshot that was created on the provided date.
        /// </summary>
        /// <remarks>
        /// The format for date is yyyy-mm-dd
        /// </remarks>
        /// <param name="date">The date of the snapshot.</param>
        /// <return>
        /// Return the snapshot that was created on the provided date.
        /// </return>
        // GET: api/forecast/Snapshots/createdDate
        [HttpGet("Snapshots/{date:datetime}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            try
            {
                // check if the models are correct?
                if (!await Validate(date))
                {
                    return BadRequest("Not valid input");
                }

                IList<Snapshot> snapshots = await _snapshot.GetByDateAsync(date);
                if (snapshots == null || snapshots.Count == 0)
                {
                    // Let's create a new snapshot for the requested date
                    snapshots = await CreateSnapshots(date);
                    if (snapshots == null)
                    {
                        return NotFound("No snapshots found with the passed search criteria.");
                    }
                }

                return Ok(snapshots);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest("Something went wrong while processing the request.");
            }
        }

        /// <summary>
        /// This endpoint will find and return all snapshots were cover the provided date range.
        /// </summary>
        /// <format>The format for the dates is yyyy-mm-dd.</format>
        /// <param name="startDate">The starting point of the search.</param>
        /// <param name="endDate">The ending point of the saerch.</param>
        /// <returns>
        /// Return a list of all snapshots that created between the two provided dates.
        /// </returns>
        // GET: api/forecast/Snapshots/start/end
        [HttpGet("SnapshotsRange/{startDate:datetime}/{endDate:datetime}")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                // check if the models are correct?
                if (!(await Validate(startDate, endDate)))
                {
                    return BadRequest("Not valid input");
                }

                IList<Snapshot> snapshots = await _snapshot.GetBetweenDatesAtLocationAsync(startDate, endDate, "All");

                if (snapshots == null || snapshots.Count == 0)
                {
                    // Let's create a new snapshot for the requested date
                    var missing = new List<DateTime>();
                    for (var i = startDate; i <= endDate; i = i.AddDays(1))
                    {
                        missing.Add(i);
                    }
                    snapshots = await CreateSnapshots(null, null, missing);
                    if (snapshots == null)
                    {
                        return NotFound("No snapshots found with the passed search criteria.");
                    }
                }

                // Find which dates are missing a snapshot so we can make a new one for it
                IList<DateTime> missingDates = new List<DateTime>();
                for (var i = startDate; i <= endDate; i = i.AddDays(1))
                {
                    // Check if a snapshot was found for date i
                    if (!FoundSnapshot(i, snapshots))
                    {
                        missingDates.Add(i);
                    }
                }

                // Lets add the missing snapshots
                if (missingDates.Count > 0)
                {
                    var missingSnapshots = await CreateSnapshots(null, null, missingDates);

                    if (missingSnapshots == null)
                        return BadRequest("Something went wrong while creating new snapshots for the missing dates.");

                    foreach (var snap in missingSnapshots)
                    {
                        snapshots.Add(snap);
                    }
                    snapshots = snapshots.OrderBy(s => s.Date).ToList(); // Order the list by the date
                }

                return Ok(snapshots);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest("Something went wrong while processing the request.");
            }
        }

        /// <summary>
        /// This endpoint will find and return all snapshots for the provided location and cover the provided date range.
        /// </summary>
        /// <format>The format for the dates is yyyy-mm-dd and the format for location is a string of aplhabet characters.</format>
        /// <param name="startDate">The starting point of the search date range.</param>
        /// <param name="endDate">The ending point of the search date range.</param>
        /// <param name="location">The City name the snapshots should cover.</param>
        /// <returns>
        /// Returns a list of all snapshots for the specified city location that cover the provided search dates.
        /// </returns>
        // GET: api/forecast/Snapshots/start/end/location
        [HttpGet("SnapshotsByLocation/{startDate:datetime}/{endDate:datetime}/{location:alpha}")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate, string location)
        {
            try
            {
                if (!String.IsNullOrEmpty(location))
                {
                    location = location.ToLower(); // make location to be lowercase
                }

                if (location == "all")
                {
                    // Redirect the call to another endpoint
                    return await Get(startDate, endDate);
                }

                // check if the models are correct?
                if (!await Validate(startDate, endDate, location))
                {
                    return BadRequest("Not valid input");
                }

                TextInfo text = new CultureInfo("en-US", false).TextInfo;
                IList<Snapshot> snapshots = await _snapshot.GetBetweenDatesAtLocationAsync(startDate, endDate, text.ToTitleCase(location));
                if (snapshots == null || snapshots.Count == 0)
                {
                    // Let's create a new snapshot for the requested dates
                    var missing = new List<DateTime>();
                    for (var i = startDate; i <= endDate; i = i.AddDays(1))
                    {
                        missing.Add(i);
                    }
                    snapshots = await CreateSnapshots(null, location, missing);
                    if (snapshots == null)
                        return NotFound("No snapshots found with the passed search criteria.");
                }

                // Find which dates are missing a snapshot so we can make a new one for it
                List<DateTime> missingDates = new List<DateTime>();
                for (var i = startDate; i <= endDate; i = i.AddDays(1))
                {
                    // Check if a snapshot was found for date i
                    if (!FoundSnapshot(i, snapshots))
                    {
                        missingDates.Add(i);
                    }
                }

                // Let's add the missing snapshots
                if (missingDates.Count > 0)
                {
                    var missingSnapshots = await CreateSnapshots(null, location, missingDates);

                    if (missingSnapshots == null)
                        return BadRequest("Something went wrong while creating new snapshots for the missing dates.");

                    foreach (var snap in missingSnapshots)
                    {
                        snapshots.Add(snap);
                    }
                    snapshots = snapshots.OrderBy(s => s.Date).ToList(); // Order the list by the date
                }

                return Ok(snapshots);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest("Something went wrong while processing the request.");
            }
        }

        /// <summary>
        /// validate the user's input
        /// </summary>
        /// <param name="start">The earliest date the snapshot should have been created on.</param>
        /// <param name="end">The lastest date the snapshot shold have beed created on.</param>
        /// <param name="location">The city name the snapshot is tied to.</param>
        /// <returns>
        /// Returns true if all inputs are valid otherwise returns false if any input fails
        /// </returns>
        private async Task<bool> Validate(DateTime start, DateTime? end = null, string location = null)
        {
            try
            {
                // First let's find the earliest snapshot date
                var earliest = (await _snapshot.GetAsync()).Min(x => x.Date);

                // The City locations that are supported for the search
                var cities = await _room.GetLocationsAsync();

                // Remove 'All' from the cities list
                cities.Remove("All");

                for (int i = 0; i < cities.Count; i++)
                {
                    cities[i] = cities[i].ToLower();
                }

                if (end == null)
                {
                    // Only need to validate start to see that it's on/after the earliest snapshot date.
                    if (start < earliest)
                    {
                        return false; // Failed
                    }
                }
                else if (location == null)
                {
                    // Need to make sure that start is on/after the earlist snapshot date and that end is after start.
                    if (start < earliest || start > end)
                    {
                        return false; // failed
                    }
                }
                else
                {
                    // Validate all three inputs
                    if (start < earliest || start > end || cities.IndexOf(location) < 0)
                    {
                        return false; // Failed
                    }
                }
                return true; // Passed all validation checks
            }
            catch (Exception ex)
            {
                // There was an erro while trying to validate the user's input so log the error return false
                logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create new snapshots for dates that weren't found
        /// </summary>
        /// <param name="date">The date that needs a snapshot to be created</param>
        /// <param name="end">The last date the snapshots should be made for.</param>
        /// <param name="location">The location for which the snapshot is for.</param>
        /// <returns>
        /// Returns a list of the newly created snapshots.
        /// </returns>
        private async Task<List<Snapshot>> CreateSnapshots(DateTime? date, string location = null, IList<DateTime> missingDates = null)
        {
            try
            {
                IList<Room> rooms = new List<Room>();
                IList<User> users = new List<User>();
                TextInfo text = new CultureInfo("en-US", false).TextInfo;
                List<Snapshot> snapshots = new List<Snapshot>();

                if (date != null)
                {
                    // Create the only missing snapshot
                    rooms = await _room.GetByDateAsync(date.Value);
                    users = await _user.GetByDateAsync(date.Value);

                    var snapshot = CreateSnapshot(date.Value, rooms.Select(x => x.Occupancy).Sum(), users.Count, (String.IsNullOrEmpty(location)) ? "All" : text.ToTitleCase(location));

                    snapshots.Add(snapshot);
                }
                else
                {
                    foreach (var d in missingDates)
                    {
                        if (String.IsNullOrEmpty(location))
                        {
                            rooms = await _room.GetByDateAsync(d);
                            users = await _user.GetByDateAsync(d);
                        }
                        else
                        {
                            rooms = await _room.GetByLocationAsync(d, location);
                            users = await _user.GetByLocationAsync(d, location);
                        }

                        var snapshot = CreateSnapshot(d, rooms.Select(x => x.Occupancy).Sum(), users.Count, (String.IsNullOrEmpty(location)) ? "All" : text.ToTitleCase(location));

                        snapshots.Add(snapshot);
                    }
                }

                try
                {
                    await _snapshot.AddSnapshotsAsync(snapshots);
                }
                catch (Exception ex)
                {
                    // Failed to add the newly created snapshots to the database.
                    logger.LogError(ex.Message);
                    return null;
                }

                return snapshots;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null; // return null
            }
        }

        /// <summary>
        /// Create an instantance of a snapshot with the provided data.
        /// </summary>
        /// <param name="date">The date the snapshot is for.</param>
        /// <param name="rooms">The number of rooms that were found for the snapshot.</param>
        /// <param name="users">The number of users that were found for the snapshot.</param>
        /// <param name="location">The location the snapshot covers.</param>
        /// <returns>
        /// Returns a new snapshot instantance that was created with the provided data.
        /// </returns>
        private Snapshot CreateSnapshot(DateTime date, int rooms, int users, string location)
        {
            var snapshot = new Snapshot()
            {
                Date = date,
                RoomOccupancyCount = rooms,
                UserCount = users,
                Location = location,
                Created = DateTime.Now
            };

            return snapshot;
        }

        /// <summary>
        /// Checks to see if a snapshot was found the provided date
        /// </summary>
        /// <param name="date">Verify a snapshot was found for this date.</param>
        /// <param name="snapshots">List of Snapshots that were found.</param>
        /// <returns>
        /// Returns true if the snapshot for the provided date was found otherwise returns false.
        /// </returns>
        private bool FoundSnapshot(DateTime date, IList<Snapshot> snapshots)
        {
            try
            {
                foreach (var snap in snapshots)
                {
                    if (snap.Date == date)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
