using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Twitter.Data.Models;
using Twitter.Data.Models.Enums;

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

        /// <summary>
        /// Get the average length of a tweet.
        /// </summary>
        /// <returns>Returns the average length of a tweet.</returns>
        public double GetAverageTweetLength();

        /// <summary>
        /// Get the top 10 hashtags used in tweets.
        /// </summary>
        /// <returns>Returns the top 10 hashtags used in tweets.</returns>
        public List<string> GetTopTenHashTags();
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

        /// <inheritdoc/>
        public double GetAverageTweetLength()
        {
            List<TweetMetaData> tweets = GetTweetsFromCache();
            return tweets.Average(tmd => tmd.Text.Length);
        }

        /// <inheritdoc/>
        public List<string> GetTopTenHashTags()
        {
            Dictionary<string, int> hashtags = GetHashtagsFromCache();
            IOrderedEnumerable<KeyValuePair<string, int>> sortedHashtags = from entry in hashtags orderby entry.Value descending select entry;
            return sortedHashtags.Take(10).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
        }

        /// <summary>
        /// Gets the hashtags object out of the cache and returns a copy of it.
        /// </summary>
        /// <returns>Return a copy of the collection of Hashtags.</returns>
        private Dictionary<string, int> GetHashtagsFromCache()
        {
            Dictionary<string, int> hashtags;
            if (!_cache.TryGetValue(CacheMoneyKeys.Hashtags, out hashtags))
            {
                //Log some exception because tweets object isn't instantiated
                throw new Exception("Hashtag object isn't instantiated in memory yet.");
            }

            return hashtags;
        }

        /// <summary>
        /// Gets the tweets object out of the cache and returns a copy of it.
        /// </summary>
        /// <returns>Return a copy of the collection of tweets.</returns>
        private List<TweetMetaData> GetTweetsFromCache()
        {
            List<TweetMetaData> tweets;
            if (!_cache.TryGetValue(CacheMoneyKeys.Tweets, out tweets))
            {
                //Log some exception because tweets object isn't instantiated
                throw new Exception("Tweets object isn't instantiated in memory yet.");
            }

            return tweets.ToList();
        }
    }
}
