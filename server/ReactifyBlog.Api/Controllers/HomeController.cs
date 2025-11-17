using Microsoft.AspNetCore.Mvc;

namespace ReactifyBlog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to ReactifyBlog API!");
        }
    }
}
