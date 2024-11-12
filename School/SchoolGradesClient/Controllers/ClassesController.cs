using Microsoft.AspNetCore.Mvc;

namespace SchoolGradesClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClassesController : Controller
    {
        public ClassesController()
        {

        }

        [HttpGet]
        public async Task GetClasses()
        {
            var response = HttpContext.Response;
            await response.SendFileAsync("wwwroot/classes.html");
        }

    }
}