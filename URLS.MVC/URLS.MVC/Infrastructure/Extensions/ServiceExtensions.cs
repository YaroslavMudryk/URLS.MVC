using Extensions.DeviceDetector;
using URLS.MVC.Infrastructure.Services.Implementations;
using URLS.MVC.Infrastructure.Services.Interfaces;

namespace URLS.MVC.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddURLSServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.Timeout = TimeSpan.FromMinutes(3);
            });
            services.AddDeviceDetector();
        }
    }
}
