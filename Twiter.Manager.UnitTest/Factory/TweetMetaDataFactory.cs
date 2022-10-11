using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Data.Models;

namespace Twiter.Manager.UnitTest.Factory
{
    public class TweetMetaDataFactory
    {
        /// <summary>
        /// Generate an TweetMetaData object.
        /// </summary>
        /// <param name="overrideDTG">Provide this optional parameter if you'd like to specify the DTG of the actual tweet.</param>
        /// <returns>Returns a new TweetMetaData object with pre-populated attributes.</returns>
        public static TweetMetaData Generate(DateTime? overrideDTG = null)
        {
            return new Faker<TweetMetaData>()
                .RuleFor(tmd => tmd.ID, Guid.NewGuid().ToString())
                .RuleFor(tmd => tmd.Text, f => f.Rant.Review())
                .RuleFor(tmd => tmd.CreatedAt, overrideDTG.HasValue ? overrideDTG.Value : DateTime.UtcNow)
                .RuleFor(tmd => tmd.AuthorID, Guid.NewGuid().ToString());            
        }

        /// <summary>
        /// Generate an TweetMetaData object with provided text.
        /// </summary>
        /// <param name="text">Provide this parameter if you'd like to specify the text of the tweet.</param>
        /// <returns>Returns a new TweetMetaData object with pre-populated attributes.</returns>
        public static TweetMetaData Generate(string text)
        {
            return new Faker<TweetMetaData>()
                .RuleFor(tmd => tmd.ID, Guid.NewGuid().ToString())
                .RuleFor(tmd => tmd.Text, text)
                .RuleFor(tmd => tmd.CreatedAt, DateTime.UtcNow)
                .RuleFor(tmd => tmd.AuthorID, Guid.NewGuid().ToString());
        }
    }
}
