using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Context.Repos
{
    public interface ISnapshotRepo: IRepo<Snapshot>
    {
        Task<IList<Snapshot>> GetBetweenDatesAsync(DateTime Start, DateTime End);

        Task<IList<Snapshot>> GetBetweenDatesAtLocationAsync(DateTime Start, DateTime End, string location);

        Task<bool> AddSnapshotsAsync(IEnumerable<Snapshot> snapshots);
    }
}
