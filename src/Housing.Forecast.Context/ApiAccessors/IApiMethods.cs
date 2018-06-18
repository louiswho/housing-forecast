using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Housing.Forecast.Context.ApiAccessors
{
    public interface IApiMethods
    {
        Task<ICollection<T>> HttpGetFromApi<T>(string portNumber, string model);
    }
}