using Microsoft.AspNetCore.Mvc;

namespace SchoolGradesClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public LoginController()
        {

        }

        [HttpGet]
        public async Task GetLogin()
        {
            var response = HttpContext.Response;
            await response.SendFileAsync("wwwroot/login.html");
        }

    }
}