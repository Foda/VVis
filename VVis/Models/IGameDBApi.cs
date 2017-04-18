using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Newtonsoft.Json;

namespace VVis.Models
{
    /// <summary>
    /// Giantbomb game api
    /// Key: 630ac64840685d60fb33dad5bb9400b93aa5c76c
    /// NOTE: You'll get a 403 if you don't send the user-agent
    /// </summary>
    [Headers("User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:45.0) Gecko/20100101 Firefox/45.0")]
    public interface IGameDBApi
    {
        [Get("/game/{id}/?api_key=630ac64840685d60fb33dad5bb9400b93aa5c76c&format=json")]
        Task<Result> GetGameInfo(string id);
    }

    public class Result
    {
        [JsonProperty("status_code")]
        int StatusCode { get; set; }

        [JsonProperty("results")]
        public GameInfo Info { get; set; }
    }
}
