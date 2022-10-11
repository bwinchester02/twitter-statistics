using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter.Data.Models.Enums
{
    public static class CacheMoneyKeys
    {
        /// <summary>
        /// They Key for our Tweets object in cache.
        /// </summary>
        public static string Tweets
        {
            get
            {
                return "Tweets";
            }
        }

        /// <summary>
        /// They Key for our Hashtag object in cache.
        /// </summary>
        public static string Hashtags
        {
            get
            {
                return "Hashtags";
            }
        }
    }
}
