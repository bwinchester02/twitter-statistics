using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Twiter.Manager.UnitTest.Factory;
using Twitter.Data.Models;
using Twitter.Data.Models.Enums;
using Twitter.Manager.Managers;
using Xunit.Sdk;

namespace Twiter.Manager.UnitTest
{
    [TestClass]
    public class AnalyticsTest
    {
        /// <summary>
        /// Test our counts code. 
        /// If 3 results are in cache, the count should be 3.
        /// </summary>
        [TestMethod]
        public void TestCounts()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<TweetMetaData> tweets = new List<TweetMetaData>();

            tweets.Add(TweetMetaDataFactory.Generate());
            tweets.Add(TweetMetaDataFactory.Generate());
            tweets.Add(TweetMetaDataFactory.Generate());

            cache.Set(CacheMoneyKeys.Tweets, tweets);
            AnalyticsManager manager = new AnalyticsManager(cache);

            Assert.AreEqual(3, manager.GetTotalTweetCount());
        }

        /// <summary>
        /// Test our averages by minute code. 
        /// If 2 tweets happened 1 minute ago, and 4 tweets happen in the next minute, our average should be 3.
        /// </summary>
        [TestMethod]
        public void TestAverage()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<TweetMetaData> tweets = new List<TweetMetaData>();
            DateTime pastTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime futureTime = DateTime.UtcNow.AddMinutes(1);

            tweets.Add(TweetMetaDataFactory.Generate(pastTime));
            tweets.Add(TweetMetaDataFactory.Generate(pastTime));
            tweets.Add(TweetMetaDataFactory.Generate(futureTime));
            tweets.Add(TweetMetaDataFactory.Generate(futureTime));
            tweets.Add(TweetMetaDataFactory.Generate(futureTime));
            tweets.Add(TweetMetaDataFactory.Generate(futureTime));

            cache.Set(CacheMoneyKeys.Tweets, tweets);
            AnalyticsManager manager = new AnalyticsManager(cache);

            Assert.AreEqual(3, manager.GetAverageTweetsPerMinute());
        }

        /// <summary>
        /// Test our average length of tweet.
        /// </summary>
        [TestMethod]
        public void TestAverageLength()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<TweetMetaData> tweets = new List<TweetMetaData>();
            tweets.Add(TweetMetaDataFactory.Generate());
            tweets.Add(TweetMetaDataFactory.Generate());

            cache.Set(CacheMoneyKeys.Tweets, tweets);
            AnalyticsManager manager = new AnalyticsManager(cache);

            double average = (tweets.FirstOrDefault().Text.Length + tweets.LastOrDefault().Text.Length) / 2;
            Assert.AreEqual(average, manager.GetAverageTweetLength());
        }


        /// <summary>
        /// Test Hashtags returning Top 10.
        /// </summary>
        [TestMethod]
        public void TestHashTags()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            List<TweetMetaData> tweets = new List<TweetMetaData>();
            Dictionary<string, int> hashtags = new Dictionary<string, int>();
            List<string> expectedHashTagOrder = new List<string>() { "#jackhenry", "#applyToday", "#sustainability", "#teamwork" };
            /* Number of hastag occurences
             * 1. jackhenry - 3
             * 2. applyToday - 2
             * 3. sustainability - 1
             * 4. teamwork - 1
             */
            tweets.Add(TweetMetaDataFactory.Generate("at Jack Henry™, we believe in people. #jackhenry #applyToday"));
            tweets.Add(TweetMetaDataFactory.Generate("Jack Henry’s Sustainability Report includes highlights from each of Jack Henry's BIGs #jackhenry #sustainability"));
            tweets.Add(TweetMetaDataFactory.Generate("We Also Believe We Are Better Together #jackhenry #teamwork #applyToday"));

            foreach(TweetMetaData tweet in tweets)
            {
                if (tweet.HashTags.Any())
                {
                    foreach (string hashtag in tweet.HashTags)
                    {
                        if (hashtags.ContainsKey(hashtag))
                        {
                            hashtags[hashtag] = hashtags[hashtag] + 1;
                        }
                        else
                        {
                            hashtags.Add(hashtag, 1);
                        }
                    }

                    
                }
            }

            cache.Set(CacheMoneyKeys.Hashtags, hashtags);
            AnalyticsManager manager = new AnalyticsManager(cache);

            CollectionAssert.AreEqual(expectedHashTagOrder, manager.GetTopTenHashTags());
        }
    }
}
