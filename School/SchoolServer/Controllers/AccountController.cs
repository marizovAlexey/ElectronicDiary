using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using School.Classes;
using System.Security.Claims;

namespace SchoolServer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private ApplicationContext _context;
    private readonly ILogger<AccountController> _logger;
    public AccountController(ApplicationContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Login == model.Login);
        if (user == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NotFound();
        }
        if (model.Password != user.Password)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogWarning($"Пользователь {user.Login} ввел неверный пароль.");
            return Unauthorized();
        }

        await Authenticate(user);
        return Ok(new { message = "Authentication successful" });
    }
    private async Task Authenticate(User user)
    {
        // создаем один claim

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Login),
            new(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()!)
        };
        // создаем объект ClaimsIdentity
        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    [HttpGet("role")]
    public IActionResult GetUserRole()
    {
        var role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
        if (role != null)
        {
            return Ok(new { role });
        }

        return Unauthorized();
    }


}
