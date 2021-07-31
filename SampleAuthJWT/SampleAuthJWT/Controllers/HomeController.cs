using Microsoft.AspNetCore.Mvc;
using SampleAuthJWT.Filter;
using SampleAuthJWT.Middleware;
using System.Web.Http.Filters;

namespace SampleAuthJWT.Controllers
{
    //This Controller defines the API authentication at the MVC level.    
    [ApiController]   
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Below Action is Shield at Filter
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [ServiceFilter(typeof(CustomAuthorize))]
        public IActionResult GetDataAuthFilter()
        {
            return Ok("Success");
        }

        /// <summary>
        /// Below Action is Shield at Middleware
        /// </summary>
        /// <returns></returns> 
        [Route("[action]")]
        [HttpGet]
        public IActionResult GetDataAuthMiddleware()
        {
            return Ok("Success");
        }
    }
}