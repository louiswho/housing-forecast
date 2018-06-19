using Housing.Forecast.Context.ApiAccessors;
using Housing.Forecast.Context.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Forecast.Context
{
    public interface IPoller
    {
        void OnStart();
        void OnStop();
        Task Update();
        void Poll();
        void UpdateBatches(ICollection<Batch> Batch);
        void UpdateRooms(ICollection<Room> Rooms);
        void UpdateUsers(ICollection<User> Users);
    }
}
