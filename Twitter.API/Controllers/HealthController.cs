using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Manager.Managers;

namespace Twitter.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthManager _manager;

        public HealthController(IHealthManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Determine whether the API can establish an authorized connection to twitter.
        /// </summary>
        /// <returns>Returns a success status code if succesful.</returns>
        [HttpGet("")]
        public IActionResult GetAPIHealth()
        {
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Determine whether the API can establish an authorized connection to twitter.
        /// </summary>
        /// <returns>Returns a success status code if succesful.</returns>
        [HttpGet("twitter")]
        public IActionResult GetTwitterHealth()
        {
            if (_manager.InterfaceWithTwitter().Result)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
