using URLS.MVC.Infrastructure.Settings;

namespace URLS.MVC.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetBearerToken(this HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return null;

            var claim = context.User.Claims.FirstOrDefault(s => s.Type == Constants.JwtCookieKey);
            return claim.Value;
        }
    }
}
