using System;
using System.Collections.Generic;
using System.Text;
using Housing.Forecast.Context.Models;

namespace Housing.Forecast.Context.Repos
{
    public interface ISnapshotRepo: IRepo<Snapshot>
    {
         IEnumerable<Snapshot> GetBetweenDates(DateTime Start, DateTime End);

         IEnumerable<Snapshot> GetBetweenDatesAtLocation(DateTime Start, DateTime End, string location);
    }
}
