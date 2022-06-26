using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace URLS.MVC.Infrastructure.Extensions
{
    public static class HttpConvertorExtensions
    {
        public static HttpContent GetHttpContent(this object data)
        {
            var con = JsonSerializer.Serialize(data);
            var content = new StringContent(con, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
