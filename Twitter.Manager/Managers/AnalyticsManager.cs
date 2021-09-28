using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using Twitter.Data.Models;
using Newtonsoft.Json;

namespace Twitter.Manager.Managers
{
    public interface IAnalyticsManager
    {
        /// <summary>
        /// Get the total number of tweets currently consumed.
        /// </summary>
        /// <returns>Returns the total number of tweets currently consumed.</returns>
        public int GetTotalTweetCount();

        /// <summary>
        /// Get the average number of tweets consumed per minute.
        /// </summary>
        /// <returns>Returns the average number of tweets consumed per minute.</returns>
        public double GetAverageTweetsPerMinute();
    }

    public class AnalyticsManager : IAnalyticsManager
    {
        private IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsManager"/> class.
        /// </summary>
        /// <param name="cache">The MemoryCache.</param>
        public AnalyticsManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc/>
        public int GetTotalTweetCount()
        {
            return GetTweetsFromCache().Count;
        }

        /// <inheritdoc/>
        public double GetAverageTweetsPerMinute()
        {
            List<TweetMetaData> tweets = GetTweetsFromCache();
            double average = tweets.GroupBy(
                d => d.FormattedTime, 
                d => d,
                (timestamp, d) => new
                {
                    Key = timestamp,
                    Count = d.Count()
                }).Average(ad => ad.Count);

            return average;
        }

        /// <summary>
        /// Gets the tweets object out of the cache and returns a copy of it.
        /// </summary>
        /// <returns>Return a copy of the collection of tweets.</returns>
        private List<TweetMetaData> GetTweetsFromCache()
        {
            List<TweetMetaData> tweets;
            if (!_cache.TryGetValue("Tweets", out tweets))
            {
                //Log some exception because tweets object isn't instantiated
                throw new Exception("Tweets object isn't instantiated in memory yet.");
            }

            return tweets.ToList();
        }
    }
}
