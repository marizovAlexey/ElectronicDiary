namespace SchoolServer.Services;

public interface IRoleCookieValidator
{
    Task<bool> CheckPermissions(HttpContext context);
    bool CheckAuthorization(HttpContext context);
}
