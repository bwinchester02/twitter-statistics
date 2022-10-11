using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Twitter.Manager.Managers;

namespace Twitter.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsManager _manager;

        public AnalyticsController(IAnalyticsManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Get the total number of tweets currently consumed.
        /// </summary>
        /// <returns>Returns the total number of tweets currently consumed.</returns>
        [HttpGet("tweet/total")]
        public IActionResult GetTotalTweetCount()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _manager.GetTotalTweetCount());
            }
            catch (Exception)
            {
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the average number of tweets consumed per minute.
        /// </summary>
        /// <returns>Returns the average number of tweets consumed per minute.</returns>
        [HttpGet("tweet/count")]
        public IActionResult GetAverageTweetCount()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _manager.GetAverageTweetsPerMinute());
            }
            catch (Exception)
            {
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the average number of tweets consumed per minute.
        /// </summary>
        /// <returns>Returns the average number of tweets consumed per minute.</returns>
        [HttpGet("tweet/length")]
        public IActionResult GetAverageTweetLength()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _manager.GetAverageTweetLength());
            }
            catch (Exception)
            {
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the top 10 hashtags used in tweets in descending order.
        /// </summary>
        /// <returns>Returns the top 10 hashtags used in tweets.</returns>
        [HttpGet("hashtag/top")]
        public IActionResult GetTopTenHashTags()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _manager.GetTopTenHashTags());
            }
            catch (Exception)
            {
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
