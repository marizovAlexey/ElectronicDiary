using Microsoft.EntityFrameworkCore;
using School.Classes;
using System.Data;
using System.Security.Claims;

namespace SchoolServer.Services;

public class RoleCookieValidator: IRoleCookieValidator
{
    private readonly ApplicationContext _context;

    public RoleCookieValidator(ApplicationContext context)
    {
        _context = context;
    }

    public bool CheckAuthorization(HttpContext context)
    {
        var userRoleId = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
        if (string.IsNullOrEmpty(userRoleId))
        {
            return false;
        }
        return true;
    }

    public async Task<bool> CheckPermissions(HttpContext context)
    {
        var userRoleId = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
        if (string.IsNullOrEmpty(userRoleId))
        {
            return false;
        }

        var roles = await _context.Roles.Where(x => x.Name != "reader").ToListAsync();

        if (roles.Exists(x => x.Id == int.Parse(userRoleId!)))
        {
            return true;
        }
        return false;
    }
}
