using Microsoft.AspNetCore.Mvc;
using SampleAuthJWT.Filter;
using System.Web.Http.Filters;

namespace SampleAuthJWT.Model
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(CustomAuthorize))]
    public class HomeController : ControllerBase
    {       
        [HttpGet("getdata")]
        public IActionResult GetData()
        {
            return Ok("Success");
        }
    }
}