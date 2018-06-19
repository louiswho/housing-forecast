using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Housing.Forecast.Context.ApiAccessors;
using Housing.Forecast.Context.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using AutoMapper;

namespace Housing.Forecast.Context
{
    /// <summary>
    /// This class is used to update the database using the service endpoints.
    /// </summary>
    /// <remarks>
    /// This class is used to update the database independently of the service
    /// allowing us to poll the service endpoints on a regular interval.
    /// </remarks>
    public class Poller : IPoller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ForecastContext _context;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Task mainTask = null;
        private readonly TimeSpan _interval;
        private ApiMethods api;

        /// <summary>
        /// Constructor for the Poller
        /// </summary>
        /// <remarks>
        /// Passes in the forecast context and the interval. 
        /// Not sure how to inject the context from this project, 
        /// since it's not running in the Service Project where
        /// we can register it with startup.
        /// </remarks>
        public Poller(ForecastContext context, TimeSpan interval, ApiMethods API)
        {
            _context = context;
            _interval = interval;
            api = API;
        }

        /// <summary>
        /// Starts the Poller object's task
        /// </summary>
        /// <remarks>
        /// Setting up a task to run asynchronously in the background. 
        /// </remarks>
        public void OnStart()
        {
            mainTask = new Task(Poll, cts.Token, TaskCreationOptions.LongRunning);
            mainTask.Start();
        }

        /// <summary>
        /// Stops the Poller object's task
        /// </summary>
        /// <remarks>
        /// Tell the cancellation token to cancel,
        /// then wait for the task to cancel.
        /// </remarks>
        public void OnStop()
        {
            cts.Cancel();
            mainTask.Wait();
        }

        /// <summary>
        /// Update individual addresses
        /// </summary>
        /// <remarks>
        /// Separated out the logic for this since it'd be
        /// duplicated in UpdateUsers and UpdateRooms. 
        /// Basically we check to see if the address is new. If so,
        /// we add it. Otherwise we check to see if data is modified
        /// and then update appropriately. Changes are saved after this function
        /// is called, so the changes are not saved directly in the method.
        /// </remarks>
        private void UpdateAddressWithoutSaving(Address check)
        {
            var mod = _context.Addresses.Where(p => p.AddressId == check.AddressId).FirstOrDefault();
            if (mod == null)
            {
                check.Id = Guid.NewGuid();
                check.Created = DateTime.Today;
                _context.Addresses.Add(check);
            }
            else if (mod.Address1 != check.Address1 ||
                    mod.Address2 != check.Address2 ||
                    mod.City != check.City ||
                    mod.Country != check.Country ||
                    mod.PostalCode != check.PostalCode ||
                    mod.State != check.State)
            {
                check.Id = mod.Id;
                _context.Entry(mod).CurrentValues.SetValues(check);
            }
        }

        /// <summary>
        /// Update individual names
        /// </summary>
        /// <remarks>
        /// Same as with Addresses in terms of logic. 
        /// Felt neater to separate this out into a separate method,
        /// even though it's only necessary in UpdateUsers.
        /// Changes are saved after this function is called, so the
        /// changes are not saved directly in the method.
        /// </remarks>
        private void UpdateNameWithoutSaving(Name check)
        {
            var mod = _context.Names.Where(p => p.NameId == check.NameId).FirstOrDefault();
            if (mod == null)
            {
                check.Id = Guid.NewGuid();
                check.Created = DateTime.Today;
                _context.Names.Add(check);
            }
            else if (mod.First != check.First ||
                    mod.Last != check.Last ||
                    mod.Middle != check.Middle)
            {
                check.Id = mod.Id;
                _context.Entry(mod).CurrentValues.SetValues(check);
            }
        }

        /// <summary>
        /// Updates Batches using the batch servicehub endpoint 
        /// </summary>
        /// <remarks>
        /// An except for the deleted entries, and a couple joins 
        /// for the new and different entries. Deleted sets datetime
        /// equal to today if it's not already set. New adds the new entry completely
        /// and diff changes values if there are any modified values.
        /// </remarks>
        public void UpdateBatches(ICollection<Batch> Batch)
        {
            var dbBatches = _context.Batches;

            var deletedBatchIds = dbBatches.Select(p => p.BatchId).Except(Batch.Select(k => k.BatchId));
            var joinBatchNew = from New in Batch
                               join Old in dbBatches
                               on New.BatchId equals Old.BatchId into temp
                               from Old in temp.DefaultIfEmpty()
                               where Old == null
                               select New;
            var joinBatchDiff = from New in Batch
                                join Old in dbBatches
                                on New.BatchId equals Old.BatchId
                                where New.State != Old.State ||
                                New.BatchName != Old.BatchName ||
                                New.BatchOccupancy != Old.BatchOccupancy ||
                                New.BatchSkill != Old.BatchSkill ||
                                New.EndDate != Old.EndDate ||
                                New.StartDate != Old.StartDate
                                select New;

            foreach (var x in _context.Batches)
            {
                if (deletedBatchIds.Contains(x.BatchId) && x.Deleted == null)
                {
                    x.Deleted = DateTime.Today;
                }
            }
            foreach (var x in joinBatchDiff)
            {
                var modify = _context.Batches.Where(y => y.BatchId == x.BatchId).FirstOrDefault();
                x.Id = modify.Id;
                _context.Entry(modify).CurrentValues.SetValues(x);

            }
            foreach (var x in joinBatchNew)
            {
                x.Created = DateTime.Today;
                x.Id = Guid.NewGuid();
                _context.Batches.Add(x);
            }
            _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Users using the Users servicehub endpoint
        /// </summary>
        /// <remarks>
        /// Same logic as UpdateBatch, except we also need to update 
        /// address and name before adding a new user to insure
        /// that those are in the tables that the User will have
        /// a relation to.
        /// </remarks>
        public void UpdateUsers(ICollection<User> Users)
        {
            var dbUsers = _context.Users;

            var deletedUserIds = dbUsers.Select(p => p.UserId).Except(Users.Select(k => k.UserId));
            var joinUserNew = from New in Users
                              join Old in dbUsers
                              on New.UserId equals Old.UserId into temp
                              from Old in temp.DefaultIfEmpty()
                              where Old == null
                              select New;
            var joinUserDiff = from New in Users
                               join Old in dbUsers
                               on New.UserId equals Old.UserId
                               where New.Address.AddressId != Old.Address.AddressId ||
                               New.Batch.BatchId != Old.Batch.BatchId ||
                               New.Email != Old.Email ||
                               New.Gender != Old.Gender ||
                               New.Location != Old.Location ||
                               New.Name.NameId != Old.Name.NameId ||
                               New.Room.RoomId != Old.Room.RoomId ||
                               New.Type != Old.Type
                               select New;

            foreach (var x in _context.Users)
            {
                if (deletedUserIds.Contains(x.UserId) && x.Deleted == null)
                {
                    x.Deleted = DateTime.Today;
                }
            }
            foreach (var x in joinUserDiff)
            {
                var modify = _context.Users.Where(y => x.UserId == y.UserId).FirstOrDefault();
                x.Id = modify.Id;
                _context.Entry(modify).CurrentValues.SetValues(x);
            }
            foreach (var x in joinUserNew)
            {
                UpdateNameWithoutSaving(x.Name);
                UpdateAddressWithoutSaving(x.Address);
                x.Created = DateTime.Today;
                x.Id = Guid.NewGuid();
                _context.Users.Add(x);
            }
            _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Rooms using the Rooms servicehub endpoint
        /// </summary>
        /// <remarks>
        /// Same logic as UpdateUsers, minus Name
        /// </remarks>
        public void UpdateRooms(ICollection<Room> Rooms)
        {
            var dbRooms = _context.Rooms;

            var deletedRoomIds = dbRooms.Select(p => p.RoomId).Except(Rooms.Select(k => k.RoomId));
            var joinRoomNew = from New in Rooms
                              join Old in dbRooms
                              on New.RoomId equals Old.RoomId into temp
                              from Old in temp.DefaultIfEmpty()
                              where Old == null
                              select New;
            var joinRoomDiff = from New in Rooms
                               join Old in dbRooms
                               on New.RoomId equals Old.RoomId
                               where New.Address.AddressId != Old.Address.AddressId ||
                               New.Location != Old.Location ||
                               New.Occupancy != Old.Occupancy ||
                               New.Gender != Old.Gender ||
                               New.Vacancy != Old.Vacancy
                               select New;

            foreach (var x in _context.Rooms)
            {
                if (deletedRoomIds.Contains(x.RoomId) && x.Deleted == null)
                {
                    x.Deleted = DateTime.Today;
                }
            }
            foreach (var x in joinRoomDiff)
            {
                var modify = _context.Rooms.Where(y => x.RoomId == y.RoomId).FirstOrDefault();
                x.Id = modify.Id;
                _context.Entry(modify).CurrentValues.SetValues(x);
            }
            foreach (var x in joinRoomNew)
            {
                UpdateAddressWithoutSaving(x.Address);
                x.Created = DateTime.Today;
                x.Id = Guid.NewGuid();
                _context.Rooms.Add(x);
            }
            _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Function Poller object uses to update database
        /// </summary>
        /// <remarks>
        /// Calls the endpoints and then uses the collections,
        /// and then maps them onto our models to
        /// then update users, rooms, and batches
        /// and by extension names and addresses as well.
        /// Also adds Snapshots now.
        /// </remarks>
        public async Task Update()
        {
            var libBatch = await api.HttpGetFromApi<Library.Models.Batch>("9040", "Batches");
            var libUsers = await api.HttpGetFromApi<Library.Models.User>("9050", "Users");
            var libRooms = await api.HttpGetFromApi<Library.Models.Room>("9030", "Rooms");

            ICollection<Batch> Batch = new List<Batch>();
            foreach (var x in libBatch)
            {
                Batch.Add(Mapper.Map<Library.Models.Batch, Batch>(x));
            }
            ICollection<User> Users = new List<User>();
            foreach (var x in libUsers)
            {
                Users.Add(Mapper.Map<Library.Models.User, User>(x));
            }
            ICollection<Room> Rooms = new List<Room>();
            foreach (var x in libRooms)
            {
                Rooms.Add(Mapper.Map<Library.Models.Room, Room>(x));
            }

            UpdateUsers(Users);
            UpdateRooms(Rooms);
            UpdateBatches(Batch);

            AddSnapshots(Users, Rooms);
        }

        /// <summary>
        /// Function for the poller to add snapshots to the database
        /// </summary>
        /// <remarks>
        /// Takes in the collections of users and rooms and then
        /// creates a snapshot for the most recent data to
        /// put in the database for the current date.
        /// </remarks>
        public void AddSnapshots(ICollection<User> users, ICollection<Room> rooms)
        {
            IEnumerable<string> locations = rooms.Select(x => x.Location).Distinct();

            int totalOccupancy = 0;
            int totalUsers = 0;
            foreach (string location in locations)
            {
                Snapshot snap = new Snapshot
                {
                    Date = DateTime.Today,
                    RoomOccupancyCount = rooms.Where(x => x.Location.Equals(location)).Select(x => x.Occupancy).Sum(),
                    UserCount = users.Where(x => x.Location.Equals(location)).Count(),
                    Location = location,
                    Created = DateTime.Today
                };

                totalOccupancy += snap.RoomOccupancyCount;
                totalUsers += snap.UserCount;

                _context.Snapshots.Add(snap);
            }

            _context.Snapshots.Add(
                new Snapshot
                {
                    Date = DateTime.Today,
                    RoomOccupancyCount = totalOccupancy,
                    UserCount = totalUsers,
                    Location = "All",
                    Created = DateTime.Today
                }
            );
            _context.SaveChangesAsync();
        }

        /// <summary>
        /// Poll function set by Task
        /// </summary>
        /// <remarks>
        /// Waits for an interval and then attempts to update the
        /// database using Update(), and cancels if cancellation
        /// is requested. Exception for some logging.
        /// Potentially add retry pattern if db connection failed.
        /// </remarks>
        public void Poll()
        {
            CancellationToken cancellation = cts.Token;
            TimeSpan interval = _interval;
            while (!cancellation.WaitHandle.WaitOne(interval))
            {
                try
                {
                    Update();
                    if (cancellation.IsCancellationRequested)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

    }
}