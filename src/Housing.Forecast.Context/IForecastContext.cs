using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Housing.Forecast.Context.Models;
using Housing.Forecast.Context;

namespace Housing.Forecast.Context
{
    public interface IForecastContext: IDisposable
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Batch> Batches { get; set; }
        DbSet<Name> Names{ get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Snapshot> Snapshots { get; set; }
        DbSet<User> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
