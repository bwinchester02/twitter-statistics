using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Twitter.Data.Models;
using Twitter.Data.Models.Config;
using Twitter.Data.Models.Enums;

namespace Twitter.Manager.Services
{
    public static class TwitterStreamService
    {
        public static async void ProcessTwitterStream(TwitterConfig twitterConfig, IMemoryCache cache)
        {
            List<TweetMetaData> tweets = new List<TweetMetaData>();
            string data;

            #region source sited: https://stackoverflow.com/questions/1081860/reading-data-from-an-open-http-stream
            WebRequest request = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=created_at&expansions=author_id&user.fields=created_at");
            request.Headers.Add("Authorization", $"Bearer {twitterConfig.BearerToken}");
            try
            {
                request.BeginGetResponse(ar =>
                {
                    WebRequest req = (WebRequest)ar.AsyncState;
                    using (WebResponse response = req.EndGetResponse(ar))
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        while (!reader.EndOfStream)
                        {
                            data = reader.ReadLine();
                            
                            if (!string.IsNullOrWhiteSpace(data))
                            {
                                TweetMetaData tweet = JsonConvert.DeserializeObject<StreamedTweet>(data).Data;
                                tweets = (List<TweetMetaData>)cache.Get(CacheMoneyKeys.Tweets);
                                tweets.Add(tweet);
                                cache.Set(CacheMoneyKeys.Tweets, tweets);
                            }
                        }
                    }
                }, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.UtcNow} :: An exception occured causing the stream to fail.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            #endregion
        }

        public static void InstantiateCache(IMemoryCache cache)
        {
            cache.Set(CacheMoneyKeys.Tweets, new List<TweetMetaData>());
        }
    }
}
