using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using Housing.Forecast.Library.Models;
using Housing.Forecast.Context.Models;
using System.Threading.Tasks;

namespace Housing.Forecast.Context.ApiAccessors
{
    /// <summary>
    /// An ApiMethods instance is used to request resources from the service hub.
    /// </summary>
    public class ApiMethods : IApiMethods
    {
        private HttpClient client;
        public ApiMethods(HttpClient Client)
        {
            client = Client;
        }

        /// <summary>
        /// A ForecastContext instance represents a session with the database and is used to query and save instances of the entities.
        /// </summary>
        /// <param name="model">Specifies the model type expected to be returned.</param>
        /// <param name="portNumber">Specifies the port number that the requested Api call is found.</param>
        /// <returns>
        /// A ForecastContext instance contains DbSets of Users, Rooms, Batches, Addresses, and Names.
        /// </returns>
        public async Task<ICollection<T>> HttpGetFromApi<T>(string portNumber, string model)
        {
            ICollection<T> resultList = null;

            // TODO: get actual URI string
            client.BaseAddress = new Uri("http://ec2-13-57-218-138.us-west-1.compute.amazonaws.com:" + portNumber + $"/api/{model}");
            var responseTask = await client.GetAsync($"{model}");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsStringAsync();

                resultList = JsonConvert.DeserializeObject<ICollection<T>>(readTask);
            }
            else
            {
                resultList = (ICollection<T>)Enumerable.Empty<T>();
            }
            return resultList;

        }
    }
}