using Housing.Forecast.Context.ApiAccessors;
using Housing.Forecast.Context.Models;
using System.Collections.Generic;

namespace Housing.Forecast.Context
{
    public interface IPoller
    {
        void OnStart();
        void OnStop();
        void Update();
        void Poll();
        void UpdateAddress(Address check);
        void UpdateBatches(ICollection<Batch> Batch);
        void UpdateName(Name check);
        void UpdateRooms(ICollection<Room> Rooms);
        void UpdateUsers(ICollection<User> Users);
    }
}
