using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Models.Config;

namespace Twitter.Manager.Managers
{
    public interface IHealthManager
    {
        /// <summary>
        /// Can the API establish an authorized connection with Twitter.
        /// </summary>
        /// <returns>Returns true if the API can establish and authorized connection with Twitter.</returns>
        public Task<bool> InterfaceWithTwitter();
    }

    public class HealthManager : IHealthManager
    {
        private readonly TwitterConfig _twitterConfig;
        private static readonly HttpClient _client = new HttpClient() { Timeout = TimeSpan.FromMinutes(240) };

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthManager"/> class.
        /// </summary>
        /// <param name="twitterConfig">The TwitterConfig.</param>
        public HealthManager(IOptions<TwitterConfig> twitterConfig)
        {
            _twitterConfig = twitterConfig.Value;
        }

        /// <inheritdoc/>
        public async Task<bool> InterfaceWithTwitter()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _twitterConfig.BearerToken);
            HttpResponseMessage response = await _client.GetAsync($"https://api.twitter.com/2/tweets/search/stream/rules");

            return response.IsSuccessStatusCode;
        }
    }
}
