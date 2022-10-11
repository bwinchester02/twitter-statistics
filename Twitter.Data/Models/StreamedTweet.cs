using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Twitter.Data.Models
{
    /// <summary>
    /// StreamedTweet.cs
    /// The meta data of a tweet streamed from Twitter's API.
    /// https://api.twitter.com/2/tweets/sample/stream
    /// </summary>
    public class StreamedTweet
    {
        /// <summary>
        /// The MetaData of the streamed tweet.
        /// </summary>
        [JsonProperty("data")]
        public TweetMetaData Data { get; set; }
    }

    public partial class TweetMetaData
    {
        /// <summary>
        /// Unique identifier of the author.
        /// </summary>
        [JsonProperty("author_id")]
        public string AuthorID { get; set; }

        /// <summary>
        /// The timestamp of the tweets creation.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Unique identifer of the tweet.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// The "tweet".
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The FormattedTime for our GroupBy.
        /// </summary>
        public string FormattedTime
        {
            get
            {
                return CreatedAt.ToString("MMddyyyyHHmm");
            }
        }

        public List<string> HashTags
        {
            get
            {
                List<string> hashTags = new List<string>();
                MatchCollection tags = Regex.Matches(Text, @"#(\w+)");
                foreach (Match tag in tags)
                {
                    hashTags.Add(tag.ToString());
                }

                return hashTags;
            }
        }
    }
}
